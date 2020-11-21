using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas.CanvasCtrl
{
    /// <summary>
    /// 工具命令枚举
    /// </summary>
    public enum eCommandType
    {
        /// <summary>
        /// 选择
        /// </summary>
        select,
        /// <summary>
        /// 抓手
        /// </summary>
        pan,
        /// <summary>
        /// 移动
        /// </summary>
        move,
        /// <summary>
        /// 绘制
        /// </summary>
        draw,
        /// <summary>
        /// 编辑
        /// </summary>
        edit,
        /// <summary>
        /// 编辑点
        /// </summary>
        editNode,
    }
}
