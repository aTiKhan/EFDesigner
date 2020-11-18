﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Sawczyn.EFDesigner.EFModel.EditingOnly
{
   public partial class GeneratedTextTransformation
   {
      #region Template
      // EFDesigner v3.0.0.1
      // Copyright (c) 2017-2020 Michael Sawczyn
      // https://github.com/msawczyn/EFDesigner

      public class EFCore5ModelGenerator : EFCoreModelGenerator
      {
         public EFCore5ModelGenerator(GeneratedTextTransformation host) : base(host) { }

         //protected override void ConfigureModelClasses(List<string> segments, ModelClass[] classesWithTables, List<string> foreignKeyColumns, List<Association> visited)
         //{
         //   foreach (ModelClass modelClass in modelRoot.Classes.OrderBy(x => x.Name))
         //   {
         //      segments.Clear();
         //      foreignKeyColumns.Clear();
         //      NL();

         //      ConfigureStandardClass(segments, classesWithTables, modelClass, visited, foreignKeyColumns);
         //   }
         //}

         //protected override void ConfigureModelAttributes(List<string> segments, ModelClass modelClass)
         //{
         //   string declaration = $"modelBuilder.Entity<{modelClass.FullName}>()";

         //   foreach (ModelAttribute modelAttribute in modelClass.Attributes.Where(x => x.Persistent && !SpatialTypes.Contains(x.Type)))
         //   {
         //      string attributeDeclaration = $"Property(t => t.{modelAttribute.Name})";
         //      segments.Clear();

         //      if ((modelAttribute.MaxLength ?? 0) > 0)
         //         // ReSharper disable once PossibleInvalidOperationException
         //         segments.Add($"HasMaxLength({modelAttribute.MaxLength.Value})");

         //      if (modelAttribute.Required)
         //         segments.Add("IsRequired()");

         //      if (modelAttribute.ColumnName != modelAttribute.Name && !string.IsNullOrEmpty(modelAttribute.ColumnName))
         //         segments.Add($"HasColumnName(\"{modelAttribute.ColumnName}\")");

         //      if (!modelAttribute.AutoProperty)
         //      {
         //         segments.Add($"HasField(\"{modelAttribute.BackingFieldName}\")");
         //         segments.Add($"UsePropertyAccessMode(PropertyAccessMode.{modelAttribute.PropertyAccessMode})");
         //      }

         //      if (!string.IsNullOrEmpty(modelAttribute.ColumnType) && modelAttribute.ColumnType.ToLowerInvariant() != "default")
         //      {
         //         if (modelAttribute.ColumnType.ToLowerInvariant() == "varchar"
         //          || modelAttribute.ColumnType.ToLowerInvariant() == "nvarchar"
         //          || modelAttribute.ColumnType.ToLowerInvariant() == "char")
         //            segments.Add($"HasColumnType(\"{modelAttribute.ColumnType}({(modelAttribute.MaxLength > 0 ? modelAttribute.MaxLength.ToString() : "max")})\")");
         //         else
         //            segments.Add($"HasColumnType(\"{modelAttribute.ColumnType}\")");
         //      }

         //      if (!string.IsNullOrEmpty(modelAttribute.InitialValue))
         //      {
         //         string initialValue = modelAttribute.InitialValue;

         //         // using switch statements since more exceptions will undoubtedly be created in the future
         //         switch (modelAttribute.Type)
         //         {
         //            case "DateTime":
         //               switch (modelAttribute.InitialValue)
         //               {
         //                  case "DateTime.Now":
         //                     segments.Add("HasDefaultValue(DateTime.Now)");

         //                     //segments.Add("HasDefaultValueSql(\"getdate()\")");
         //                     break;

         //                  case "DateTime.UtcNow":
         //                     segments.Add("HasDefaultValue(DateTime.UtcNow)");

         //                     //segments.Add("HasDefaultValueSql(\"getutcdate()\")");
         //                     break;

         //                  default:
         //                     if (!initialValue.StartsWith("\""))
         //                        initialValue = "\"" + initialValue;

         //                     if (!initialValue.EndsWith("\""))
         //                        initialValue = initialValue + "\"";

         //                     segments.Add($"HasDefaultValue(DateTime.Parse({initialValue}))");

         //                     break;
         //               }

         //               break;

         //            case "String":
         //               if (!initialValue.StartsWith("\""))
         //                  initialValue = "\"" + initialValue;

         //               if (!initialValue.EndsWith("\""))
         //                  initialValue = initialValue + "\"";

         //               segments.Add($"HasDefaultValue({initialValue})");

         //               break;

         //            default:
         //               segments.Add($"HasDefaultValue({modelAttribute.InitialValue})");

         //               break;
         //         }
         //      }

         //      if (modelAttribute.IsConcurrencyToken)
         //         segments.Add("IsRowVersion()");

         //      if (modelAttribute.IsIdentity)
         //      {
         //         segments.Add(modelAttribute.IdentityType == IdentityType.AutoGenerated
         //                         ? "ValueGeneratedOnAdd()"
         //                         : "ValueGeneratedNever()");
         //      }

         //      if (segments.Any())
         //      {
         //         segments.Insert(0, declaration);
         //         segments.Insert(1, attributeDeclaration);

         //         Output(segments);
         //      }

         //      if (modelAttribute.Indexed && !modelAttribute.IsIdentity)
         //      {
         //         segments.Clear();

         //         segments.Add($"{declaration}.HasIndex(t => t.{modelAttribute.Name})");

         //         if (modelAttribute.IndexedUnique)
         //            segments.Add("IsUnique()");

         //         Output(segments);
         //      }
         //   }
         //}

      }

      #endregion Template
   }
}