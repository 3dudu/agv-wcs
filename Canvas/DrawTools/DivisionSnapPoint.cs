using Canvas.CanvasInterfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas.DrawTools
{
    /// <summary>
    /// 分界吸附点
    /// </summary>
    public class DivisionSnapPoint: SnapPointBase
    {
        public DivisionSnapPoint(ICanvas canvas, IDrawObject owner, UnitPoint snappoint)
            : base(canvas, owner, snappoint)
        { }

        public override void Draw(ICanvas canvas)
        {
            DrawPoint(canvas, System.Drawing.Pens.White, Brushes.YellowGreen);
        }
    }
}
