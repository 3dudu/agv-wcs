using Canvas.CanvasInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas.DrawTools
{
    /// <summary>
    /// 交点吸附点
    /// </summary>
    public class IntersectSnapPoint: SnapPointBase
    {
        public IntersectSnapPoint(ICanvas canvas, IDrawObject owner, UnitPoint snappoint)
          : base(canvas, owner, snappoint)
        { }

        public override void Draw(ICanvas canvas)
        {
            DrawPoint(canvas, System.Drawing.Pens.White, System.Drawing.Brushes.YellowGreen);
        }
    }//end
}
