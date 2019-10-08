﻿#pragma warning disable IDE0017 // Simplify object initialization
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Reflection;

using Newtonsoft.Json;

using ParsingModels;

namespace EF6Parser
{
   public class Parser
   {
      private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

      private readonly Assembly assembly;
      private readonly DbContext dbContext;
      private readonly MetadataWorkspace metadata;

      public Parser(Assembly assembly, string dbContextTypeName = null)
      {
         this.assembly = assembly;
         Type contextType;

         if (dbContextTypeName != null)
         {
            log.Info($"dbContextTypeName parameter is {dbContextTypeName}");
            contextType = assembly.GetExportedTypes().FirstOrDefault(t => t.FullName == dbContextTypeName);
            log.Info($"Using contextType = {contextType.FullName}");
         }
         else
         {
            log.Info("dbContextTypeName parameter is null");
            List<Type> types = assembly.GetExportedTypes().Where(t => typeof(DbContext).IsAssignableFrom(t)).ToList();

            // ReSharper disable once UnthrowableException
            if (types.Count != 1)
            {
               log.Error($"Found more than one class derived from DbContext: {string.Join(", ", types.Select(t => t.FullName))}");
               throw new AmbiguousMatchException("Found more than one class derived from DbContext");
            }

            contextType = types[0];
            log.Info($"Using contextType = {contextType.FullName}");
         }

         ConstructorInfo constructor = contextType.GetConstructor(new[] {typeof(string)});

         // ReSharper disable once UnthrowableException
         if (constructor == null)
         {
            log.Error("Can't find constructor with one string parameter (connection string or connection name)");
            throw new MissingMethodException("Can't find constructor with one string parameter (connection string or connection name)");
         }

         dbContext = assembly.CreateInstance(contextType.FullName, false, BindingFlags.Default, null, new object[] {"App=EntityFramework"}, null, null) as DbContext;
         metadata = ((IObjectContextAdapter)dbContext).ObjectContext.MetadataWorkspace;
      }

      private static Multiplicity ConvertMultiplicity(RelationshipMultiplicity relationshipMultiplicity)
      {
         Multiplicity multiplicity = Multiplicity.ZeroOne;

         switch (relationshipMultiplicity)
         {
            case RelationshipMultiplicity.ZeroOrOne:
               multiplicity = Multiplicity.ZeroOne;

               break;

            case RelationshipMultiplicity.One:
               multiplicity = Multiplicity.One;

               break;

            case RelationshipMultiplicity.Many:
               multiplicity = Multiplicity.ZeroMany;

               break;
         }

         return multiplicity;
      }

      private NavigationProperty Inverse(NavigationProperty navProperty)
      {
         // ReSharper disable once UseNullPropagation
         if (navProperty == null)
            return null;

         EntityType toEntity = navProperty.ToEndMember.GetEntityType();
         return toEntity.NavigationProperties
                        .SingleOrDefault(n => ReferenceEquals(n.RelationshipType, navProperty.RelationshipType) && !ReferenceEquals(n, navProperty));
      }

      private List<ModelUnidirectionalAssociation> GetUnidirectionalAssociations(EntityType entityType)
      {
         List<ModelUnidirectionalAssociation> result = new List<ModelUnidirectionalAssociation>();

         foreach (NavigationProperty navigationProperty in entityType.DeclaredNavigationProperties.Where(np => Inverse(np) == null))
         {
            // ReSharper disable UseObjectOrCollectionInitializer
            ModelUnidirectionalAssociation association = new ModelUnidirectionalAssociation();

            association.SourceClassName = navigationProperty.DeclaringType.Name;
            association.SourceClassNamespace = navigationProperty.DeclaringType.NamespaceName;
            association.TargetClassName = navigationProperty.ToEndMember.GetEntityType().Name;
            association.TargetClassNamespace = navigationProperty.ToEndMember.GetEntityType().NamespaceName;

            // the property in the source class (referencing the target class)
            association.TargetPropertyTypeName = navigationProperty.ToEndMember.GetEntityType().Name;
            association.TargetPropertyName = navigationProperty.Name;
            association.TargetMultiplicity = ConvertMultiplicity(navigationProperty.ToEndMember.RelationshipMultiplicity);
            association.TargetSummary = navigationProperty.ToEndMember.Documentation?.Summary;
            association.TargetDescription = navigationProperty.ToEndMember.Documentation?.LongDescription;

            // the property in the target class (referencing the source class)
            association.SourceMultiplicity = ConvertMultiplicity(navigationProperty.FromEndMember.RelationshipMultiplicity);
            // ReSharper restore UseObjectOrCollectionInitializer

            log.Info($"Found unidirectional association {association.SourceClassName}.{association.TargetPropertyName} -> {association.TargetClassName}");
            log.Info("\n   " + JsonConvert.SerializeObject(association));

            result.Add(association);
         }

         return result;
      }

      private List<ModelBidirectionalAssociation> GetBidirectionalAssociations(EntityType entityType)
      {
         List<ModelBidirectionalAssociation> result = new List<ModelBidirectionalAssociation>();

         foreach (NavigationProperty navigationProperty in entityType.DeclaredNavigationProperties.Where(np => Inverse(np) != null))
         {
            // ReSharper disable UseObjectOrCollectionInitializer
            ModelBidirectionalAssociation association = new ModelBidirectionalAssociation();

            association.SourceClassName = navigationProperty.DeclaringType.Name;
            association.SourceClassNamespace = navigationProperty.DeclaringType.NamespaceName;
            association.TargetClassName = navigationProperty.ToEndMember.GetEntityType().Name;
            association.TargetClassNamespace = navigationProperty.ToEndMember.GetEntityType().NamespaceName;

            NavigationProperty inverse = Inverse(navigationProperty);

            // the property in the source class (referencing the target class)
            association.TargetPropertyTypeName = navigationProperty.ToEndMember.GetEntityType().Name;
            association.TargetPropertyName = navigationProperty.Name;
            association.TargetMultiplicity = ConvertMultiplicity(navigationProperty.ToEndMember.RelationshipMultiplicity);
            association.TargetSummary = navigationProperty.ToEndMember.Documentation?.Summary;
            association.TargetDescription = navigationProperty.ToEndMember.Documentation?.LongDescription;

            // the property in the target class (referencing the source class)
            association.SourcePropertyTypeName = navigationProperty.FromEndMember.GetEntityType().Name;
            association.SourcePropertyName = inverse?.Name;
            association.SourceMultiplicity = ConvertMultiplicity(navigationProperty.FromEndMember.RelationshipMultiplicity);
            association.SourceSummary = navigationProperty.FromEndMember.Documentation?.Summary;
            association.SourceDescription = navigationProperty.FromEndMember.Documentation?.LongDescription;
            // ReSharper restore UseObjectOrCollectionInitializer

            log.Info($"Found bidirectional association {association.SourceClassName}.{association.TargetPropertyName} <-> {association.TargetClassName}.{association.SourcePropertyTypeName}");
            log.Info("\n   " + JsonConvert.SerializeObject(association));

            result.Add(association);
         }

         return result;

      }

      private static string GetCustomAttributes(Type type)
      {
         return type == null
                   ? string.Empty
                   : GetCustomAttributes(type.CustomAttributes);
      }

      private static string GetCustomAttributes(IEnumerable<CustomAttributeData> customAttributeData)
      {
         List<string> customAttributes = customAttributeData.Select(a => a.ToString()).ToList();
         customAttributes.Remove("[System.SerializableAttribute()]");
         customAttributes.Remove("[System.Runtime.InteropServices.ComVisibleAttribute((Boolean)True)]");
         customAttributes.Remove("[__DynamicallyInvokableAttribute()]");
         customAttributes.Remove("[System.Reflection.DefaultMemberAttribute(\"Chars\")]");
         customAttributes.Remove("[System.Runtime.Versioning.NonVersionableAttribute()]");
         customAttributes.Remove("[System.FlagsAttribute()]");
         return string.Join("", customAttributes);
      }

      private static string GetTableName(Type type, DbContext context)
      {
         EntitySet table = GetTable(type, context);

         if (table == null) 
            return null;

         // Return the table name from the storage entity set
         return (string)table.MetadataProperties["Table"].Value ?? table.Name;
      }

      private static EntitySet GetTable(Type type, DbContext context)
      {
         EntitySet table = null;
         MetadataWorkspace metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

         // Get the part of the model that contains info about the actual CLR types
         ObjectItemCollection objectItemCollection = (ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace);

         // Get the entity type from the model that maps to the CLR type
         EntityType entityType = metadata.GetItems<EntityType>(DataSpace.OSpace)
                                         .SingleOrDefault(e => objectItemCollection.GetClrType(e) == type);

         if (entityType != null)
         {
            // Get the entity set that uses this entity type
            EntitySet entitySet = metadata.GetItems<EntityContainer>(DataSpace.CSpace)
                                          .SingleOrDefault()
                                         ?.EntitySets
                                         ?.SingleOrDefault(s => s.ElementType.Name == entityType.Name);

            if (entitySet == null)
            {

               // Find the mapping between conceptual and storage model for this entity set
               EntitySetMapping mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                                                  .SingleOrDefault()
                                                 ?.EntitySetMappings
                                                 ?.SingleOrDefault(s => s.EntitySet == entitySet);

               // Find the storage entity set (table) that the entity is mapped
               table = mapping?.EntityTypeMappings.SingleOrDefault()?.Fragments?.SingleOrDefault()?.StoreEntitySet;
            }
         }

         return table;
      }

      public string Process()
      {
         if (dbContext == null)
         {
            log.Error("Process: dbContext is null");
            throw new ArgumentNullException("dbContext");
         }

         ReadOnlyCollection<GlobalItem> oSpace = metadata.GetItems(DataSpace.OSpace);
         log.Info($"Found {oSpace.Count} OSpace items");

         ReadOnlyCollection<GlobalItem> sSpace = metadata.GetItems(DataSpace.SSpace);
         log.Info($"Found {sSpace.Count} SSpace items");

         // Context
         ///////////////////////////////////////////
         ModelRoot modelRoot = ProcessRoot();

         // Entities
         ///////////////////////////////////////////
         List<ModelClass> modelClasses = oSpace.OfType<EntityType>()
                                               .Select(e => ProcessEntity(e.FullName, oSpace.OfType<EntityType>().SingleOrDefault(o => o.FullName == e.FullName), sSpace.OfType<EntityType>().SingleOrDefault(s => s.FullName == "CodeFirstDatabaseSchema." + e.FullName.Split('.').Last())))
                                               .Where(x => x != null)
                                               .ToList();
         log.Info($"Adding {modelClasses.Count} classes");
         modelRoot.Classes.AddRange(modelClasses);

         // Complex types
         ///////////////////////////////////////////
         modelClasses = oSpace.OfType<ComplexType>()
                              .Select(e => ProcessComplexType(e.FullName, oSpace.OfType<EntityType>().SingleOrDefault(s => s.FullName == e.FullName), 
                                                              sSpace.OfType<EntityType>().SingleOrDefault(s => s.FullName == "CodeFirstDatabaseSchema." + e.FullName.Split('.').Last())))
                              .Where(x => x != null)
                              .ToList();
         log.Info($"Adding {modelClasses.Count} complex types");
         modelRoot.Classes.AddRange(modelClasses);

         // Enums
         ///////////////////////////////////////////
         List<ModelEnum> modelEnums = oSpace.OfType<EnumType>().Select(ProcessEnum).Where(x => x != null).ToList();
         log.Info($"Adding {modelEnums.Count} enumerations");
         modelRoot.Enumerations.AddRange(modelEnums);

         // Put it all together
         ///////////////////////////////////////////
         log.Info("Serializing to JSON string");
         return JsonConvert.SerializeObject(modelRoot);
      }

      private ModelClass ProcessEntity(string entityFullName, EntityType oSpaceType, EntityType sSpaceType)
      {
         Type type = assembly.GetType(entityFullName);
       
         if (type == null)
         {
            log.Warn($"Could not find type for entity {entityFullName}");
            return null;
         }
         
         log.Info($"Found entity {entityFullName}");
         string customAttributes = GetCustomAttributes(type);

         ModelClass result = new ModelClass
                             {
                                Name = oSpaceType.Name
                              , Namespace = oSpaceType.NamespaceName
                              , IsAbstract = oSpaceType.Abstract
                              , BaseClass = oSpaceType.BaseType?.Name
                              , CustomInterfaces = type.GetInterfaces().Any() 
                                                      ? string.Join(",", type.GetInterfaces().Select(t => t.FullName))
                                                      : null
                              , IsDependentType = false
                              , CustomAttributes = customAttributes.Length > 2
                                                      ? customAttributes
                                                      : null
                              , Properties = oSpaceType.DeclaredProperties
                                                       .Select(x => x.Name)
                                                       .Select(propertyName => ProcessProperty(oSpaceType, 
                                                                                               oSpaceType.DeclaredProperties.FirstOrDefault(q => q.Name == propertyName), 
                                                                                               sSpaceType.DeclaredProperties.FirstOrDefault(q => q.Name == propertyName)))
                                                       .Where(x => x != null)
                                                       .ToList()
                              , UnidirectionalAssociations = GetUnidirectionalAssociations(oSpaceType)
                              , BidirectionalAssociations = GetBidirectionalAssociations(oSpaceType)
                              , TableName = GetTableName(type, dbContext)
                             };

         log.Info("\n   " + JsonConvert.SerializeObject(result));
         return result;
      }

      private ModelClass ProcessComplexType(string entityFullName, EntityType oSpaceType, EntityType sSpaceType)
      {
         Type type = assembly.GetType(entityFullName);

         if (type == null)
         {
            log.Warn($"Could not find type for complex type {entityFullName}");
            return null;
         }

         log.Info($"Found complex type {entityFullName}");
         string customAttributes = GetCustomAttributes(type);

         ModelClass result = new ModelClass
                             {
                                Name = oSpaceType.Name
                              , Namespace = oSpaceType.NamespaceName
                              , IsAbstract = oSpaceType.Abstract
                              , BaseClass = oSpaceType.BaseType?.Name
                              , IsDependentType = true
                              , CustomAttributes = customAttributes.Length > 2
                                                      ? customAttributes
                                                      : null
                              , CustomInterfaces = type.GetInterfaces().Any() 
                                                      ? string.Join(",", type.GetInterfaces().Select(t => t.FullName))
                                                      : null
                              , Properties = oSpaceType.DeclaredProperties
                                                       .Select(x => x.Name)
                                                       .Select(propertyName => ProcessProperty(oSpaceType, 
                                                                                               oSpaceType.DeclaredProperties.FirstOrDefault(q => q.Name == propertyName), 
                                                                                               sSpaceType.DeclaredProperties.FirstOrDefault(q => q.Name == propertyName),
                                                                                               true))
                                                       .Where(x => x != null)
                                                       .ToList()
                              , TableName = null
                             };

         log.Info("\n   " + JsonConvert.SerializeObject(result));
         return result;
      }

      private ModelEnum ProcessEnum(EnumType enumType)
      {
         Type type = assembly.GetType(enumType.FullName);

         if (type == null)
         {
            log.Warn($"Could not find type for complex type {enumType.FullName}");
            return null;
         }

         log.Info($"Found enum {enumType.FullName}");
         string customAttributes = GetCustomAttributes(type);

         ModelEnum result = new ModelEnum
                            {
                               Name = enumType.Name
                             , Namespace = enumType.NamespaceName
                             , IsFlags = enumType.IsFlags
                             , ValueType = enumType.UnderlyingType.ClrEquivalentType.Name
                             , CustomAttributes = customAttributes.Length > 2
                                                     ? customAttributes
                                                     : null
                             , Values = enumType.Members
                                                .Select(enumMember => new ModelEnumValue
                                                                      {
                                                                         Name = enumMember.Name
                                                                       , Value = enumMember.Value?.ToString()
                                                                      })
                                                .ToList()
                            };



         log.Info("\n   " + JsonConvert.SerializeObject(result));
         return result;
      }

      private ModelProperty ProcessProperty(EntityType parent, 
                                            EdmProperty oSpaceProperty, 
                                            EdmProperty sSpaceProperty,
                                            bool isComplexType = false)
      {
         if (oSpaceProperty == null || sSpaceProperty == null)
            return null;

         log.Info($"Found property {parent.Name}.{oSpaceProperty.Name}");
         try
         {
            ModelProperty result = new ModelProperty
                                   {
                                      TypeName = oSpaceProperty.TypeUsage.EdmType.Name
                                    , Name = oSpaceProperty.Name
                                    , IsIdentity = !isComplexType && parent.KeyProperties.Any(p => p.Name == oSpaceProperty.Name)
                                    , IsIdentityGenerated = sSpaceProperty.IsStoreGeneratedIdentity
                                    , Required = !(bool)sSpaceProperty.TypeUsage.Facets.First(facet => facet.Name == "Nullable").Value
                                    , Indexed = bool.TryParse(oSpaceProperty.TypeUsage.Facets.FirstOrDefault(facet => facet.Name == "Indexed")?.Value?.ToString(), out bool indexed) && indexed
                                    , MaxStringLength = int.TryParse(sSpaceProperty.TypeUsage.Facets.FirstOrDefault(facet => facet.Name == "MaxLength")?.Value?.ToString(), out int maxLength) && maxLength < int.MaxValue/2 ? maxLength : 0
                                    , MinStringLength = int.TryParse(sSpaceProperty.TypeUsage.Facets.FirstOrDefault(facet => facet.Name == "MinLength")?.Value?.ToString(), out int minLength) ? minLength : 0
                                   };

            log.Info("\n   " + JsonConvert.SerializeObject(result));
            return result;
         }
         catch (InvalidOperationException)
         {
         }

         return null;
      }

      private ModelRoot ProcessRoot()
      {
         ModelRoot result = new ModelRoot();
         Type contextType = dbContext.GetType();
         if (contextType == null)
            throw new InvalidDataException();

         log.Info($"Found DbContext {contextType.Name}");

         result.EntityContainerName = contextType.Name;
         result.Namespace = contextType.Namespace;

         log.Info("\n   " + JsonConvert.SerializeObject(result));

         return result;
      }
   }
}