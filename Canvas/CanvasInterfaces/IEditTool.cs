using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Canvas.CanvasInterfaces
{
    /// <summary>
    /// 图形编辑接口
    /// </summary>
    public interface IEditTool
    {
        IEditTool Clone();
        bool SupportSelection { get; }
        void SetHitObjects(UnitPoint mousepoint, List<IDrawObject> list);
        void OnMouseMove(ICanvas canvas, UnitPoint point);
        eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint);
        void OnMouseUp(ICanvas canvas, UnitPoint point, ISnapPoint snappoint);
        void OnKeyDown(ICanvas canvas, KeyEventArgs e);
        void Finished();
        void Undo();
        void Redo();
    }
}
