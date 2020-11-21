using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas.CanvasInterfaces
{
    /// <summary>
    /// 绘画图形元素时鼠标落下的操作类型
    /// </summary>
    public enum eDrawObjectMouseDownEnum
    {
        /// <summary>
        /// 当前元素绘画结束
        /// </summary>
        Done,
        /// <summary>
        /// 重新绘画当前元素
        /// </summary>
        DoneRepeat,
        /// <summary>
        /// 继续绘画当前元素
        /// </summary>
        Continue,
    }
}
