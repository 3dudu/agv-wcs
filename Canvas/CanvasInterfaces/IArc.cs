using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas.CanvasInterfaces
{
    public interface IArc
    {
        /// <summary>
        /// 圆弧中心点
        /// </summary>
        UnitPoint Center { get; }
        /// <summary>
        /// 圆弧半径
        /// </summary>
        float Radius { get; }
        /// <summary>
        /// 开始角度
        /// </summary>
        float StartAngle { get; }
        /// <summary>
        /// 结束角度
        /// </summary>
        float EndAngle { get; }
    }
}
