using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas.CanvasInterfaces
{
    public interface ICanvasOwner
    {
        /// <summary>
        /// 重置元素位置
        /// </summary>
        void SetPositionInfo(UnitPoint unitpos);
        /// <summary>
        /// 设置吸附点
        /// </summary>
        void SetSnapInfo(ISnapPoint snap);
    }
}
