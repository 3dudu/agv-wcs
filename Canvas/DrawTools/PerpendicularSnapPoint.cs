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
    /// 垂直吸附点
    /// </summary>
    public class PerpendicularSnapPoint: SnapPointBase
    {
        public PerpendicularSnapPoint(ICanvas canvas, IDrawObject owner, UnitPoint snappoint)
           : base(canvas, owner, snappoint)
        { }
        public override void Draw(ICanvas canvas)
        {
            DrawPoint(canvas, Pens.White, Brushes.YellowGreen);
        }
    }
}
