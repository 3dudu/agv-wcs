using Model.MDM;
using Model.MSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace AGVCore
{
    //小车改变时回调
    public delegate void CarChangeCallBack(CarEventArgs e);

    /// <summary>
    ///小车反馈消息时回调
    /// </summary>
    /// <param name="Car"></param>
    public delegate void CarFeedbackCallBack(CarBaseStateInfo e);


    /// <summary>
    /// 调度状态回调
    /// </summary>
    /// <param name="msg"></param>
    public delegate void DispatchStateCallBack(string msg);


    /// <summary>
    /// 充电桩反馈消息时回调
    /// </summary>
    /// <param name="charge"></param>
    public delegate void ChargeFeedBack(ChargeStationInfo charge);

    
    /// <summary>
    /// IO设备反馈消息回调
    /// </summary>
    /// <param name="charge"></param>
    public delegate void IOFeedBack(IODeviceInfo IODevice);


    /// <summary>
    /// 委托发布类
    /// </summary>
    public class DelegateState
    {
        /// <summary>
        /// //小车改变时触发事件
        /// </summary>
        public static event CarChangeCallBack CarChangeEvent;
        /// <summary>
        /// 触发小车改变时事件
        /// </summary>
        /// <param name="e"></param>
        public async static void InvokeCarChangeEvent(CarEventArgs e)
        {
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    CarChangeEvent?.Invoke(e);
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
            }
        }



        /// <summary>
        /// 小车反馈消息时事件
        /// </summary>
        public static event CarFeedbackCallBack CarFeedbackEvent;

        /// <summary>
        /// 触发小车反馈消息时事件
        /// </summary>
        /// <param name="e"></param>
        public static async void InvokeCarFeedbackEvent(CarBaseStateInfo e)
        {
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    CarFeedbackEvent?.Invoke(e);
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
            }

        }



        /// <summary>
        /// 调度状态显示事件
        /// </summary>
        public static event DispatchStateCallBack DispatchStateEvent;

        /// <summary>
        /// 触发状态显示事件
        /// </summary>
        /// <param name="msg"></param>
        public static async void InvokeDispatchStateEvent(string msg)
        {
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    DispatchStateEvent?.Invoke(msg);
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
            }
        }


        /// <summary>
        /// 充电桩改变时触发事件
        /// </summary>
        public static event ChargeFeedBack ChargeEvent;
        /// <summary>
        /// 触发充电桩改变时事件
        /// </summary>
        /// <param name="e"></param>
        public async static void InvokeChargeChangeEvent(ChargeStationInfo e)
        {
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    ChargeEvent?.Invoke(e);
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
            }
        }

        public static event IOFeedBack IOFeedEvent;

        /// <summary>
        /// 触发IO设备信息反馈
        /// </summary>
        /// <param name="e"></param>
        public async static void InvokeIOFeedBackEvent(IODeviceInfo e)
        {
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    IOFeedEvent?.Invoke(e);
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
            }
        }



    }
}
