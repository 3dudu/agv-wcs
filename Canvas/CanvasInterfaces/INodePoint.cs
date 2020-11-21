using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas.CanvasInterfaces
{
    /// <summary>
    /// 绘图节点接口
    /// </summary>
    public interface INodePoint
    {
        IDrawObject GetClone();
        IDrawObject GetOriginal();
        void Cancel();
        void Finish();
        void SetPosition(UnitPoint pos);
        void Undo();
        void Redo();
        void OnKeyDown(ICanvas canvas, System.Windows.Forms.KeyEventArgs e);
    }
}
