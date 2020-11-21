using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DipatchModel
{
    /// <summary>
    /// 小车动作功能枚举
    /// </summary>
    public enum AGVCommandEnum
    {
        /// <summary>
        /// 启动
        /// </summary>
        Start = 0,

        /// <summary>
        /// 停止
        /// </summary>
        Stop = 1,

        /// <summary>
        /// 切换路线
        /// </summary>
        ChangeRoute = 2,

        /// <summary>
        /// 速度设置
        /// </summary>
        SpeedSet = 3,

        /// <summary>
        /// 设置PBS
        /// </summary>
        SetPBS = 4,

        /// <summary>
        /// 升起勾销
        /// </summary>
        HookOn = 5,

        /// <summary>
        /// 降下勾销
        /// </summary>
        HookOff = 6,

        /// <summary>
        /// 取消当前任务
        /// </summary>
        CancelTast = 7,
        /// <summary>
        /// 读寄存器通道1
        /// </summary>
        ReadRegister1 = 8,
        /// <summary>
        /// 写寄存器通道1
        /// </summary>
        WriteRegister1 = 9,
        /// <summary>
        /// 读寄存器通道2
        /// </summary>
        ReadRegister2 = 10,
        /// <summary>
        /// 写寄存器通道2
        /// </summary>
        WriteRegister2 = 11,
        /// <summary>
        /// 转向
        /// </summary>
        Turn,
        /// <summary>
        /// 强制IO
        /// </summary>
        IOSet,
        /// <summary>
        /// 强制IO(充电用)
        /// </summary>
        SetIO1,
        /// <summary>
        /// 强制站点
        /// </summary>
        LogicSiteSet,
        /// <summary>
        /// 避障设置
        /// </summary>
        AvoidanceSet,
        /// <summary>
        /// 模拟按钮动作
        /// </summary>
        BtnSet,
        /// <summary>
        /// 读取站点指令
        /// </summary>
        ReadSiteCmd,
        /// <summary>
        /// 线路站点设置
        /// </summary>
        RouteSiteSet,

        #region 充电桩
        /// <summary>
        /// 开始充电
        /// </summary>
        StartCharge,

        /// <summary>
        /// 结束充电
        /// </summary>
        StopCharge,

        /// <summary>
        /// 心跳查询
        /// </summary>
        HartBet,
        #endregion
    }
}
