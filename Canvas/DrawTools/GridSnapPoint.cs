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
    /// 栅格的吸附点
    /// </summary>
    public class GridSnapPoint: SnapPointBase
    {
        public GridSnapPoint(ICanvas canvas, UnitPoint snappoint)
           : base(canvas, null, snappoint)
        { }

        #region 实现 ISnapPoint 接口
        public override void Draw(ICanvas canvas)
        {
            DrawPoint(canvas, Pens.Gray, null);
        }
        #endregion
    }//end Class
}
