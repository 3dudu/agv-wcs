using DipatchModel;
using Model.CarInfoExtend;
using Model.MDM;
using Model.MSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVCore
{
    public interface IDispacther
    {
        /// <summary>
        /// 初始化调度服务
        /// </summary>
        bool DispatchInit();

        /// <summary>
        /// 启动调度服务
        /// </summary>
        /// <returns></returns>
        bool DispatchStart();

        /// <summary>
        /// 调度终止
        /// </summary>
        /// <returns></returns>
        bool DispatchStop();

        /// <summary>
        /// 车辆信息反馈回调
        /// </summary>
        void HandleCarFeedBack(CarBaseStateInfo agvcar);

        /// <summary>
        /// 接受处理IO设备回馈信息
        /// </summary>
        /// <param name="ReceveIO"></param>
        void HandleIOFeedBack(IODeviceInfo ReceveIO);

        /// <summary>
        /// 接受处理充电桩反馈信息
        /// </summary>
        /// <param name="ChargeInfo"></param>
        void HandleChargeFeedBack(ChargeStationInfo ChargeInfo);

        /// <summary>
        /// 添加AGV指令
        /// </summary>
        /// <param name="agvid"></param>
        /// <param name="cmd"></param>
        /// <param name="para"></param>
        void AGV_AddCommand(int agv_id, CommandToValue ctov);

        /// <summary>
        /// 同步数据库档案
        /// </summary>
        void ReadBaseInfo();

        /// <summary>
        /// 界面调用发送车辆执行路线
        /// </summary>
        void SendCarRoute(CarInfo car);

        /// <summary>
        /// 向充电桩通讯层添加指令
        /// </summary>
        /// <param name="cmd"></param>
        void Add_EleCommand(int chargeid, CommandToValue ctov);

        /// <summary>
        /// 添加IO设备指令
        /// </summary>
        void AddIOControl(int ioid, CommandToValue ctov);

        /// <summary>
        /// 调用WebAPI
        /// </summary>
        /// <param name="Parm"></param>
        /// <returns></returns>
        string InvokWebAPI(CarInfo car);

        void ClearCarLockSource(CarInfo car);

        string getTrafficInfo();
    }
}
