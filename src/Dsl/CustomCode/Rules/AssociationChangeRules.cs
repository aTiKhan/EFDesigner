﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(Association), FireTime = TimeToFire.TopLevelCommit)]
   public class AssociationChangeRules : ChangeRule
   {
      // matches [Display(Name="*")] and [System.ComponentModel.DataAnnotations.Display(Name="*")]
      private static readonly Regex DisplayAttributeRegex = new Regex("^(.*)\\[(System\\.ComponentModel\\.DataAnnotations\\.)?Display\\(Name=\"([^\"]+)\"\\)\\](.*)$", RegexOptions.Compiled);

      private static void CheckSourceForDisplayText(BidirectionalAssociation bidirectionalAssociation)
      {
         Match match = DisplayAttributeRegex.Match(bidirectionalAssociation.SourceCustomAttributes);

         // is there a custom attribute for [Display]?
         if (match != Match.Empty)
         {
            // if SourceDisplayText is empty, move the Name down to SourceDisplayText
            if (string.IsNullOrWhiteSpace(bidirectionalAssociation.SourceDisplayText))
               bidirectionalAssociation.SourceDisplayText = match.Groups[3].Value;

            // if custom attribute's Name matches SourceDisplayText, remove that attribute, leaving other custom attributes if present
            if (match.Groups[3].Value == bidirectionalAssociation.SourceDisplayText)
               bidirectionalAssociation.SourceCustomAttributes = match.Groups[1].Value + match.Groups[4].Value;
         }
      }

      private static void CheckTargetForDisplayText(Association association)
      {
         Match match = DisplayAttributeRegex.Match(association.TargetCustomAttributes);

         // is there a custom attribute for [Display]?
         if (match != Match.Empty)
         {
            // if TargetDisplayText is empty, move the Name down to TargetDisplayText
            if (string.IsNullOrWhiteSpace(association.TargetDisplayText))
               association.TargetDisplayText = match.Groups[3].Value;

            // if custom attribute's Name matches TargetDisplayText, remove that attribute, leaving other custom attributes if present
            if (match.Groups[3].Value == association.TargetDisplayText)
               association.TargetCustomAttributes = match.Groups[1].Value + match.Groups[4].Value;
         }
      }

      public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
      {
         base.ElementPropertyChanged(e);

         Association element = (Association)e.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing)
            return;

         if (Equals(e.NewValue, e.OldValue))
            return;

         List<string> errorMessages = EFCoreValidator.GetErrors(element).ToList();
         BidirectionalAssociation bidirectionalAssociation = element as BidirectionalAssociation;
         
         switch (e.DomainProperty.Name)
         {
            case "Persistent":
               UpdateDisplayForPersistence(element);

               break;

            case "SourceCustomAttributes":

               if (bidirectionalAssociation != null && !string.IsNullOrWhiteSpace(bidirectionalAssociation.SourceCustomAttributes))
               {
                  bidirectionalAssociation.SourceCustomAttributes = $"[{bidirectionalAssociation.SourceCustomAttributes.Trim('[', ']')}]";
                  CheckSourceForDisplayText(bidirectionalAssociation);
               }

               break;

            case "SourceDeleteAction":
               DeleteAction sourceDeleteAction = (DeleteAction)e.NewValue;
               UpdateDisplayForCascadeDelete(element, sourceDeleteAction);

               break;

            case "SourceDisplayText":

               if (bidirectionalAssociation != null)
                  CheckSourceForDisplayText(bidirectionalAssociation);

               break;

            case "SourceForeignKeyPropertyName":

               if (bidirectionalAssociation != null)
               {
                  if (element.ExposeForeignKeyProperties)
                  {
                     string newSourceFKProperty = (string)e.NewValue;

                     if (string.IsNullOrEmpty(newSourceFKProperty))
                        errorMessages.Add($"Foreign key property for {bidirectionalAssociation.SourcePropertyName}: Property name must be a valid .NET identifier");
                     else
                     {
                        ParseResult fragment;

                        try
                        {
                           fragment = ModelAttribute.Parse(element.Target.ModelRoot, newSourceFKProperty);

                           if (fragment == null)
                              errorMessages.Add($"Foreign key property for {bidirectionalAssociation.SourcePropertyName}: Could not parse entry '{newSourceFKProperty}'");
                           else
                           {
                              if (string.IsNullOrEmpty(fragment.Name) || !CodeGenerator.IsValidLanguageIndependentIdentifier(fragment.Name))
                                 errorMessages.Add($"Foreign key property for {bidirectionalAssociation.SourcePropertyName}: Property name '{fragment.Name}' isn't a valid .NET identifier");
                              else if (element.Source.AllAttributes.Any(x => x.Name == fragment.Name))
                                 errorMessages.Add($"Foreign key property for {bidirectionalAssociation.SourcePropertyName}: Property name '{fragment.Name}' already in use");
                              else if (element.Source.AllNavigationProperties().Any(p => p.PropertyName == fragment.Name || p.AssociationObject.TargetForeignKeyPropertyName == newSourceFKProperty))
                                 errorMessages.Add($"Foreign key property for {bidirectionalAssociation.SourcePropertyName}: Property name '{fragment.Name}' already in use");
                              else
                                 bidirectionalAssociation.SourcePropertyName = fragment.Name;
                           }
                        }
                        catch (Exception exception)
                        {
                           errorMessages.Add($"Foreign key property for {bidirectionalAssociation.SourcePropertyName}: Could not parse entry '{newSourceFKProperty}': {exception.Message}");
                        }
                     }
                  }
                  else
                  {
                     bidirectionalAssociation.SourceForeignKeyPropertyName = null;
                  }
               }

               break;

            case "SourceMultiplicity":
               Multiplicity sourceMultiplicity = (Multiplicity)e.NewValue;

               // change unidirectional source cardinality
               // if target is dependent
               //    source cardinality is 0..1 or 1
               if (element.Target.IsDependentType && sourceMultiplicity == Multiplicity.ZeroMany)
               {
                  errorMessages.Add($"Can't have a 0..* association from {element.Target.Name} to dependent type {element.Source.Name}");

                  break;
               }

               if ((sourceMultiplicity == Multiplicity.One && element.TargetMultiplicity == Multiplicity.One) ||
                   (sourceMultiplicity == Multiplicity.ZeroOne && element.TargetMultiplicity == Multiplicity.ZeroOne))
               {
                  if (element.SourceRole != EndpointRole.NotSet)
                     element.SourceRole = EndpointRole.NotSet;

                  if (element.TargetRole != EndpointRole.NotSet)
                     element.TargetRole = EndpointRole.NotSet;
               }
               else
                  SetEndpointRoles(element);

               UpdateDisplayForCascadeDelete(element, null, null, sourceMultiplicity);

               break;

            case "SourcePropertyName":
               string sourcePropertyNameErrorMessage = ValidateAssociationIdentifier(element, element.Target, (string)e.NewValue);

               if (EFModelDiagram.IsDropping && sourcePropertyNameErrorMessage != null)
                  element.Delete();
               else
                  errorMessages.Add(sourcePropertyNameErrorMessage);

               break;

            case "SourceRole":

               if (element.Source.IsDependentType)
               {
                  element.SourceRole = EndpointRole.Dependent;
                  element.TargetRole = EndpointRole.Principal;
               }
               else
               {
                  EndpointRole sourceRole = (EndpointRole)e.NewValue;

                  if (sourceRole == EndpointRole.Dependent && element.TargetRole != EndpointRole.Principal)
                     element.TargetRole = EndpointRole.Principal;
                  else if (sourceRole == EndpointRole.Principal && element.TargetRole != EndpointRole.Dependent)
                     element.TargetRole = EndpointRole.Dependent;

                  SetEndpointRoles(element);
               }

               break;

            case "TargetCustomAttributes":

               if (!string.IsNullOrWhiteSpace(element.TargetCustomAttributes))
               {
                  element.TargetCustomAttributes = $"[{element.TargetCustomAttributes.Trim('[', ']')}]";
                  CheckTargetForDisplayText(element);
               }

               break;

            case "TargetDisplayText":

               CheckTargetForDisplayText(element);

               break;

            case "TargetDeleteAction":
               DeleteAction targetDeleteAction = (DeleteAction)e.NewValue;
               UpdateDisplayForCascadeDelete(element, null, targetDeleteAction);

               break;

            case "TargetForeignKeyPropertyName":

               if (element.ExposeForeignKeyProperties)
               {
                  string newTargetFKProperty = (string)e.NewValue;

                  if (string.IsNullOrEmpty(newTargetFKProperty))
                     errorMessages.Add($"Foreign key property for {element.TargetPropertyName}: Property name must be a valid .NET identifier");
                  else
                  {
                     ParseResult fragment;

                     try
                     {
                        fragment = ModelAttribute.Parse(element.Source.ModelRoot, newTargetFKProperty);

                        if (fragment == null)
                           errorMessages.Add($"Foreign key property for {element.TargetPropertyName}: Could not parse entry '{newTargetFKProperty}'");
                        else
                        {
                           if (string.IsNullOrEmpty(fragment.Name) || !CodeGenerator.IsValidLanguageIndependentIdentifier(fragment.Name))
                              errorMessages.Add($"Foreign key property for {element.TargetPropertyName}: Property name '{fragment.Name}' isn't a valid .NET identifier");
                           else if (element.Source.AllAttributes.Any(x => x.Name == fragment.Name))
                              errorMessages.Add($"Foreign key property for {element.TargetPropertyName}: Property name '{fragment.Name}' already in use");
                           else if (element.Source.AllNavigationProperties().Any(p => p.PropertyName == fragment.Name || p.AssociationObject.TargetForeignKeyPropertyName == newTargetFKProperty))
                              errorMessages.Add($"Foreign key property for {element.TargetPropertyName}: Property name '{fragment.Name}' already in use");
                           else
                              element.TargetPropertyName = fragment.Name;
                        }
                     }
                     catch (Exception exception)
                     {
                        errorMessages.Add($"Foreign key property for {element.TargetPropertyName}: Could not parse entry '{newTargetFKProperty}': {exception.Message}");
                     }
                  }
               }
               else
               {
                  element.TargetForeignKeyPropertyName = null;
               }

               break;

            case "TargetMultiplicity":
               Multiplicity newTargetMultiplicity = (Multiplicity)e.NewValue;

               // change unidirectional target cardinality
               // if target is dependent
               //    target cardinality must be 0..1 or 1
               if (element.Target.IsDependentType && newTargetMultiplicity == Multiplicity.ZeroMany)
               {
                  errorMessages.Add($"Can't have a 0..* association from {element.Source.Name} to dependent type {element.Target.Name}");

                  break;
               }

               if ((element.SourceMultiplicity == Multiplicity.One && newTargetMultiplicity == Multiplicity.One) ||
                   (element.SourceMultiplicity == Multiplicity.ZeroOne && newTargetMultiplicity == Multiplicity.ZeroOne))
               {
                  if (element.SourceRole != EndpointRole.NotSet)
                     element.SourceRole = EndpointRole.NotSet;

                  if (element.TargetRole != EndpointRole.NotSet)
                     element.TargetRole = EndpointRole.NotSet;
               }
               else
                  SetEndpointRoles(element);

               UpdateDisplayForCascadeDelete(element, null, null, null, newTargetMultiplicity);

               break;

            case "TargetPropertyName":

               // if we're creating an association via drag/drop, it's possible the existing property name
               // is the same as the default property name. The default doesn't get created until the transaction is 
               // committed, so the drop's action will cause a name clash. Remove the clashing property, but
               // only if drag/drop.

               string targetPropertyNameErrorMessage = ValidateAssociationIdentifier(element, element.Source, (string)e.NewValue);

               if (EFModelDiagram.IsDropping && targetPropertyNameErrorMessage != null)
                  element.Delete();
               else
                  errorMessages.Add(targetPropertyNameErrorMessage);

               break;

            case "TargetRole":

               if (element.Target.IsDependentType)
               {
                  element.SourceRole = EndpointRole.Principal;
                  element.TargetRole = EndpointRole.Dependent;
               }
               else
               {
                  EndpointRole targetRole = (EndpointRole)e.NewValue;

                  if (targetRole == EndpointRole.Dependent && element.SourceRole != EndpointRole.Principal)
                     element.SourceRole = EndpointRole.Principal;
                  else if (targetRole == EndpointRole.Principal && element.SourceRole != EndpointRole.Dependent)
                     element.SourceRole = EndpointRole.Dependent;

                  SetEndpointRoles(element);
               }

               break;
         }

         errorMessages = errorMessages.Where(m => m != null).ToList();

         if (errorMessages.Any())
         {
            current.Rollback();
            ErrorDisplay.Show(string.Join("\n", errorMessages));
         }
      }

      internal static void SetEndpointRoles(Association element)
      {
         switch (element.TargetMultiplicity)
         {
            case Multiplicity.ZeroMany:

               switch (element.SourceMultiplicity)
               {
                  case Multiplicity.ZeroMany:
                     element.SourceRole = EndpointRole.NotApplicable;
                     element.TargetRole = EndpointRole.NotApplicable;

                     break;
                  case Multiplicity.One:
                     element.SourceRole = EndpointRole.Principal;
                     element.TargetRole = EndpointRole.Dependent;

                     break;
                  case Multiplicity.ZeroOne:
                     element.SourceRole = EndpointRole.Principal;
                     element.TargetRole = EndpointRole.Dependent;

                     break;
               }

               break;
            case Multiplicity.One:

               switch (element.SourceMultiplicity)
               {
                  case Multiplicity.ZeroMany:
                     element.SourceRole = EndpointRole.Dependent;
                     element.TargetRole = EndpointRole.Principal;

                     break;
                  case Multiplicity.One:

                     break;
                  case Multiplicity.ZeroOne:
                     element.SourceRole = EndpointRole.Dependent;
                     element.TargetRole = EndpointRole.Principal;

                     break;
               }

               break;
            case Multiplicity.ZeroOne:

               switch (element.SourceMultiplicity)
               {
                  case Multiplicity.ZeroMany:
                     element.SourceRole = EndpointRole.Dependent;
                     element.TargetRole = EndpointRole.Principal;

                     break;
                  case Multiplicity.One:
                     element.SourceRole = EndpointRole.Principal;
                     element.TargetRole = EndpointRole.Dependent;

                     break;
                  case Multiplicity.ZeroOne:

                     break;
               }

               break;
         }
      }

      internal static void UpdateDisplayForCascadeDelete(Association element,
                                                         DeleteAction? sourceDeleteAction = null,
                                                         DeleteAction? targetDeleteAction = null,
                                                         Multiplicity? sourceMultiplicity = null,
                                                         Multiplicity? targetMultiplicity = null)
      {
         ModelRoot modelRoot = element.Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();

         sourceDeleteAction = sourceDeleteAction ?? element.SourceDeleteAction;
         targetDeleteAction = targetDeleteAction ?? element.TargetDeleteAction;
         sourceMultiplicity = sourceMultiplicity ?? element.SourceMultiplicity;
         targetMultiplicity = targetMultiplicity ?? element.TargetMultiplicity;

         bool cascade = modelRoot.ShowCascadeDeletes &&
                        element.Persistent &&
                        (sourceMultiplicity == Multiplicity.One ||
                         targetMultiplicity == Multiplicity.One ||
                         targetDeleteAction == DeleteAction.Cascade ||
                         sourceDeleteAction == DeleteAction.Cascade);

         List<AssociationConnector> changeColor =
            PresentationViewsSubject.GetPresentation(element)
                                    .OfType<AssociationConnector>()
                                    .Where(connector => (cascade && connector.Color != Color.Red) ||
                                                        (!cascade && connector.Color != Color.Black))
                                    .ToList();

         List<AssociationConnector> changeStyle =
            PresentationViewsSubject.GetPresentation(element)
                                    .OfType<AssociationConnector>()
                                    .Where(connector => (cascade && connector.DashStyle != DashStyle.Dash) ||
                                                        (!cascade && connector.DashStyle != DashStyle.Solid))
                                    .ToList();

         foreach (AssociationConnector connector in changeColor)
            connector.Color = cascade
                                 ? Color.Red
                                 : Color.Black;

         foreach (AssociationConnector connector in changeStyle)
            connector.DashStyle = cascade
                                     ? DashStyle.Dash
                                     : DashStyle.Solid;
      }

      internal static void UpdateDisplayForPersistence(Association element)
      {
         // don't change unless necessary so as to not set the model's dirty flag without need
         bool persistent = element.Persistent;

         List<AssociationConnector> changeColors =
            PresentationViewsSubject.GetPresentation(element)
                                    .OfType<AssociationConnector>()
                                    .Where(connector => (persistent && connector.Color != Color.Black) ||
                                                        (!persistent && connector.Color != Color.DarkGray))
                                    .ToList();

         List<AssociationConnector> changeStyle =
            PresentationViewsSubject.GetPresentation(element)
                                    .OfType<AssociationConnector>()
                                    .Where(connector => (persistent && connector.DashStyle != DashStyle.Solid) ||
                                                        (!persistent && connector.DashStyle != DashStyle.Dash))
                                    .ToList();

         foreach (AssociationConnector connector in changeColors)
            connector.Color = persistent
                                 ? Color.Black
                                 : Color.DarkGray;

         foreach (AssociationConnector connector in changeStyle)
            connector.DashStyle = persistent
                                     ? DashStyle.Solid
                                     : DashStyle.Dash;
      }

      private static string ValidateAssociationIdentifier(Association association, ModelClass targetedClass, string identifier)
      {
         if (string.IsNullOrWhiteSpace(identifier) || !CodeGenerator.IsValidLanguageIndependentIdentifier(identifier))
            return $"{identifier} isn't a valid .NET identifier";

         ModelClass offendingModelClass = targetedClass.AllAttributes.FirstOrDefault(x => x.Name == identifier)?.ModelClass ?? targetedClass.AllNavigationProperties(association).FirstOrDefault(x => x.PropertyName == identifier)?.ClassType;

         return offendingModelClass != null
                   ? $"Duplicate symbol {identifier} in {offendingModelClass.Name}"
                   : null;
      }
   }
}
