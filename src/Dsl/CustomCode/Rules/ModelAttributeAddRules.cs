﻿using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel.Rules
{
   [RuleOn(typeof(ModelAttribute), FireTime = TimeToFire.TopLevelCommit)]
   internal class ModelAttributeAddRules : AddRule
   {
      public override void ElementAdded(ElementAddedEventArgs e)
      {
         base.ElementAdded(e);

         ModelAttribute element = (ModelAttribute)e.ModelElement;
         ModelClass modelClass = element.ModelClass;

         // set a new default value if we want to implement notify, to reduce the chance of forgetting to change it
         if (modelClass?.ImplementNotify == true)
            element.AutoProperty = false;
      }
   }
}