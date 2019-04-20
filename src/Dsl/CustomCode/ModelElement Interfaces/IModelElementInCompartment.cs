namespace Sawczyn.EFDesigner.EFModel.Interfaces
{
   /// <summary>
   /// Tag interface indicating diagram items for this element are compartments in a parent element
   /// </summary>
   public interface IModelElementInCompartment
   {
      IModelElementWithCompartments ParentModelElement { get; }
      string CompartmentName { get; }
   }
}
