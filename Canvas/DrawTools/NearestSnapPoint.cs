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
    /// 附近吸附点
    /// </summary>
    public class NearestSnapPoint: SnapPointBase
    {
        public NearestSnapPoint(ICanvas canvas, IDrawObject owner, UnitPoint snappoint)
           : base(canvas, owner, snappoint)
        { }

        #region 实现 ISnapPoint 接口
        public override void Draw(ICanvas canvas)
        {
            DrawPoint(canvas, System.Drawing.Pens.White, Brushes.YellowGreen);
        }
        #endregion
    }
}
