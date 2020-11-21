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
    /// 垂足吸附点
    /// </summary>
    public class VertextSnapPoint: SnapPointBase
    {
        public VertextSnapPoint(ICanvas canvas, IDrawObject owner, UnitPoint snappoint)
           : base(canvas, owner, snappoint)
        { }

        public override void Draw(ICanvas canvas)
        {
            DrawPoint(canvas, Pens.Blue, Brushes.YellowGreen);
        }
    }//end
}
