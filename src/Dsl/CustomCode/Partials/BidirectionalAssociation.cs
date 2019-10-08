﻿using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;

namespace Sawczyn.EFDesigner.EFModel
{
   [ValidationState(ValidationState.Enabled)]
   public partial class BidirectionalAssociation
   {
      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void SummaryDescriptionIsEmpty(ValidationContext context)
      {
         ModelRoot modelRoot = Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();
         if (modelRoot.WarnOnMissingDocumentation && string.IsNullOrWhiteSpace(SourceSummary))
         {
            context.LogWarning($"{Target.Name}.{SourcePropertyName}: Association end should be documented", "AWMissingSummary", this);
            hasWarning = true;
            RedrawItem();
            Source.RedrawItem();
            Target.RedrawItem();
         }
      }

      #region SourceImplementNotify tracking property

      /// <summary>Storage for the SourceImplementNotify property.</summary>  
      private bool sourceImplementNotifyStorage;

      /// <summary>Gets the storage for the SourceImplementNotify property.</summary>
      /// <returns>The SourceImplementNotify value.</returns>
      public bool GetSourceImplementNotifyValue()
      {
         bool loading = Store.TransactionManager.InTransaction && Store.TransactionManager.CurrentTransaction.IsSerializing;

         return !loading && IsSourceImplementNotifyTracking ? Target.ImplementNotify : sourceImplementNotifyStorage;
      }

      /// <summary>Sets the storage for the SourceImplementNotify property.</summary>
      /// <param name="value">The SourceImplementNotify value.</param>
      public void SetSourceImplementNotifyValue(bool value)
      {
         sourceImplementNotifyStorage = value;
         bool loading = Store.TransactionManager.InTransaction && Store.TransactionManager.CurrentTransaction.IsSerializing;

         if (!Store.InUndoRedoOrRollback && !loading)
            // ReSharper disable once ArrangeRedundantParentheses
            IsSourceImplementNotifyTracking = (sourceImplementNotifyStorage == Target.ImplementNotify);
      }

      internal sealed partial class IsSourceImplementNotifyTrackingPropertyHandler
      {
         /// <summary>
         ///    Called after the IsSourceImplementNotifyTracking property changes.
         /// </summary>
         /// <param name="element">The model element that has the property that changed. </param>
         /// <param name="oldValue">The previous value of the property. </param>
         /// <param name="newValue">The new value of the property. </param>
         protected override void OnValueChanged(BidirectionalAssociation element, bool oldValue, bool newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);
            if (!element.Store.InUndoRedoOrRollback && newValue)
            {
               DomainPropertyInfo propInfo = element.Store.DomainDataDirectory.GetDomainProperty(SourceImplementNotifyDomainPropertyId);
               propInfo.NotifyValueChange(element);
            }
         }

         /// <summary>Performs the reset operation for the IsSourceImplementNotifyTracking property for a model element.</summary>
         /// <param name="element">The model element that has the property to reset.</param>
         internal void ResetValue(BidirectionalAssociation element)
         {
            element.isSourceImplementNotifyTrackingPropertyStorage = (element.SourceImplementNotify == element.Target.ImplementNotify);
         }

         /// <summary>
         ///    Method to set IsSourceImplementNotifyTracking to false so that this instance of this tracking property is not
         ///    storage-based.
         /// </summary>
         /// <param name="element">
         ///    The element on which to reset the property value.
         /// </param>
         internal void PreResetValue(BidirectionalAssociation element)
         {
            // Force the IsSourceImplementNotifyTracking property to false so that the value  
            // of the SourceImplementNotify property is retrieved from storage.  
            element.isSourceImplementNotifyTrackingPropertyStorage = false;
         }
      }

      #endregion

      /// <summary>
      ///    Calls the pre-reset method on the associated property value handler for each
      ///    tracking property of this model element.
      /// </summary>
      // ReSharper disable once UnusedMember.Global
      internal override void PreResetIsTrackingProperties()
      {
         base.PreResetIsTrackingProperties();
         IsSourceImplementNotifyTrackingPropertyHandler.Instance.PreResetValue(this);
         // same with other tracking properties as they get added
      }

      /// <summary>
      ///    Calls the reset method on the associated property value handler for each
      ///    tracking property of this model element.
      /// </summary>
      // ReSharper disable once UnusedMember.Global
      internal override void ResetIsTrackingProperties()
      {
         base.ResetIsTrackingProperties();
         IsSourceImplementNotifyTrackingPropertyHandler.Instance.ResetValue(this);
         // same with other tracking properties as they get added
      }
   }
}
