using System.Drawing;
using System.Drawing.Drawing2D;

namespace Sawczyn.EFDesigner.EFModel.Interfaces
{
   public interface IHighlightFromModelExplorer
   {
      Color OutlineColor { get; set; }
      DashStyle OutlineDashStyle { get; set; }
      float OutlineThickness { get; set; }
      bool Visible { get; set; }
   }
}