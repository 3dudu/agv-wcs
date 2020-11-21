using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketModel
{
    public static class SocketCommand
    {
        /// <summary>
        /// 心跳测试
        /// </summary>
        public const string CONTEST = "0001";

        /// <summary>
        /// 获得所有小车当前状态
        /// </summary>
        public const string AllCarsInfo = "0002";

        /// <summary>
        /// 某辆小车的当前信息
        /// </summary>
        public const string OneCarInfo = "0003";

        /// <summary>
        /// 切换小车路线
        /// </summary>
        public const string ChageCarRoute = "0004";

        /// <summary>
        /// 启动小车
        /// </summary>
        public const string SetCarRun = "0005";

        /// <summary>
        /// 清理小车
        /// </summary>
        public const string ClearCar = "0006";



        /// <summary>
        /// 同步平板按钮设置
        /// </summary>
        public const string SynBeeperBtnLayout = "0007";

        /// <summary>
        /// 同步呼叫器工单
        /// </summary>
        public const string SynBeeperBill = "0008";

        /// <summary>
        /// 呼叫器呼叫
        /// </summary>
        public const string BeeperCall = "0009";

        /// <summary>
        /// 发送平板报警
        /// </summary>
        public const string PadWarm = "0010";

        /// <summary>
        /// 客户端触发同步订单
        /// </summary>
        public const string ClientSendSynBill = "0011";

        /// <summary>
        /// 平板登陆权限验证
        /// </summary>
        public const string AppLoginCheck = "0012";

        /// <summary>
        /// 同步平板上的储位状态
        /// </summary>
        public const string SynAppStoreState = "0013";
    }
}
