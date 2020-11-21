using Canvas.CanvasInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas.DrawTools
{
    /// <summary>
    /// 象限吸附点
    /// </summary>
    public class QuadrantSnapPoint: SnapPointBase
    {
        public QuadrantSnapPoint(ICanvas canvas, IDrawObject owner, UnitPoint snappoint)
           : base(canvas, owner, snappoint)
        { }

        public override void Draw(ICanvas canvas)
        {
            DrawPoint(canvas, System.Drawing.Pens.White, System.Drawing.Brushes.YellowGreen);
        }
    }
}
