namespace Sawczyn.EFDesigner.EFModel.Interfaces
{
   public interface IDisplaysWarning
   {
      bool GetHasWarningValue();

      void ResetWarning();

      void RedrawItem();

   }
}
