using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas.CanvasInterfaces
{
    /// <summary>
    /// 图层接口类
    /// </summary>
    public interface ICanvasLayer
    {
        string Id { get; }
        void Draw(ICanvas canvas, RectangleF unitrect);
        ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, List<IDrawObject> otherobj);
        IEnumerable<IDrawObject> Objects { get; }
        bool Enabled { get; set; }
        bool Visible { get; }
    }
}
