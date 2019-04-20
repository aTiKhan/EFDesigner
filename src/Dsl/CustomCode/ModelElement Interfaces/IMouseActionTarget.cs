using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel.Interfaces
{
   public interface IMouseActionTarget
   {
      void DoMouseUp(ModelElement dragFrom, DiagramMouseEventArgs e);
   }
}