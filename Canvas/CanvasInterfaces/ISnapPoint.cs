using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas.CanvasInterfaces
{
    /// <summary>
    /// 吸附点接口类
    /// </summary>
    public interface ISnapPoint
    {
        IDrawObject Owner { get; }
        UnitPoint SnapPoint { get; }
        RectangleF BoundingRect { get; }
        void Draw(ICanvas canvas);
    }
}
