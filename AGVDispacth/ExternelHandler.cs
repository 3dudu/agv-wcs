using AGVCore;
using AGVDAccess;
using Model.CarInfoExtend;
using Model.Comoon;
using Model.MDM;
using Model.MSM;
using SocketServer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools;

namespace AGVDispacth
{
    public class ExternelHandler
    {
        #region 属性
        private static object HandCallBoxlockObj = new object();
        private TCPServer _server = null;
        //上次接受监控传感器的时间
        Hashtable hs_PreReciveSensorTime = new Hashtable();
        #endregion

        #region 函数方法
        public bool ExternelServerStart()
        {
            try
            {
                DelegateState.InvokeDispatchStateEvent("正在初始化外围服务...");
                string FILEPATH = Application.StartupPath + @"/sysconfig.xml";
                Hashtable htServer = XMLClass.GetXMLByParentNode(FILEPATH, XMLConstClass.NODE_SERVER);
                string port = "2018";
                string maxclient = "200";
                string rcvtimeout = "100";
                string sendtimeout = "100";
                string buffersize = "128";
                if (htServer.Contains(XMLConstClass.PORT))
                {
                    port = htServer[XMLConstClass.PORT].ToString();
                }
                if (htServer.Contains(XMLConstClass.MAXCONNECTCOUNT))
                {
                    maxclient = htServer[XMLConstClass.MAXCONNECTCOUNT].ToString();
                }
                if (htServer.Contains(XMLConstClass.RECTIMEOUT))
                {
                    rcvtimeout = htServer[XMLConstClass.RECTIMEOUT].ToString();
                }
                if (htServer.Contains(XMLConstClass.SENDTIMEOUT))
                {
                    sendtimeout = htServer[XMLConstClass.SENDTIMEOUT].ToString();
                }
                if (htServer.Contains(XMLConstClass.BUFFERSIZE))
                {
                    buffersize = htServer[XMLConstClass.BUFFERSIZE].ToString();
                }
                IServerConfig nannyclient_serverconfig = new SocketServer.ServerConfig() { Port = Convert.ToInt16(port), MaxClientCount = Convert.ToInt16(maxclient), RecOutTime = Convert.ToInt16(rcvtimeout), ReceiveBufferSize = Convert.ToInt16(buffersize), SendBufferSize = Convert.ToInt16(buffersize) };
                if (_server == null)
                {
                    _server = new TCPServer();
                    _server.ReceiveMes += _server_ReceiveMes;
                }
                if (!_server.Setup(nannyclient_serverconfig))
                { return false; }
                else
                {
                    if (!_server.Start())
                    { return false; }
                }
                return true;
            }
            catch (Exception ex)
            {
                DelegateState.InvokeDispatchStateEvent(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 服务端接收消息回调
        /// </summary>
        private void _server_ReceiveMes(object e)
        {
            try
            {
                lock (HandCallBoxlockObj)
                {
                    NetEventArgs einfo = e as NetEventArgs;
                    if (einfo == null) { return; }
                    List<byte> Message = einfo.MesContent;
                    AppSession ClientSession = einfo.Session;
                    if (Message == null || ClientSession == null) { return; }
                    //读开关输入状态
                    LogHelper.WriteCallBoxLog("已接受到按钮呼叫!");
                    string IP = ((IPEndPoint)ClientSession.ClientSocket.RemoteEndPoint).Address.ToString();
                    LogHelper.WriteCallBoxLog("接受到按钮盒" + IP + "呼叫!");
                    CallBoxInfo CallBox = CoreData.AllCallBoxes.FirstOrDefault(p => p.CallBoxIP == IP);
                    if (CallBox != null && Message[0] == 0x01 && Message[1] == 0x02)//按钮盒呼叫
                    {
                        LogHelper.WriteCallBoxLog("开始处理按钮盒子呼叫!");
                        HandleCallBoxRequest(e, CallBox);
                    }
                }
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }
        #endregion

        #region 接受信息后处理具体业务
        private void HandleCallBoxRequest(object obj, CallBoxInfo CallBox)
        {
            try
            {
                //lock (HandCallBoxlockObj)
                //{
                LogHelper.WriteCallBoxLog("接受到:" + CallBox.CallBoxID.ToString() + "号按钮盒信息");
                NetEventArgs einfo = obj as NetEventArgs;
                if (einfo == null) { return; }
                LogHelper.WriteCallBoxLog("开始解析按钮值");
                List<byte> Message = einfo.MesContent;
                AppSession ClientSession = einfo.Session;
                int CallBoxID = CallBox.CallBoxID;
                //本次被开的按钮
                List<int> CallBoxBtnList = new List<int>();
                //本次被关闭的按钮
                List<int> CallBoxBtnCloseList = new List<int>();
                //int ButID = (int)Message[3];
                //if (ButID == 0) { return; }
                //CallBoxBtnList.Add(ButID);
                ////最多支持9个按键
                //string BtnPortNextStr = Convert.ToString(Message[4], 2).PadLeft(8, '0');
                //LogHelper.WriteCallBoxLog(BtnPortNextStr);
                //if (BtnPortNextStr.Substring(7, 1) == "1")
                //{ CallBoxBtnList.Add(9); }
                string BtnPortStr = Convert.ToString(Message[3], 2).PadLeft(8, '0');
                if (BtnPortStr.Substring(0, 1) == "1")
                { CallBoxBtnList.Add(8); }
                else
                { CallBoxBtnCloseList.Add(8); }
                if (BtnPortStr.Substring(1, 1) == "1")
                { CallBoxBtnList.Add(7); }
                else
                { CallBoxBtnCloseList.Add(7); }
                if (BtnPortStr.Substring(2, 1) == "1")
                { CallBoxBtnList.Add(6); }
                else
                { CallBoxBtnCloseList.Add(6); }
                if (BtnPortStr.Substring(3, 1) == "1")
                { CallBoxBtnList.Add(5); }
                else
                { CallBoxBtnCloseList.Add(5); }
                if (BtnPortStr.Substring(4, 1) == "1")
                { CallBoxBtnList.Add(4); }
                else
                { CallBoxBtnCloseList.Add(4); }
                if (BtnPortStr.Substring(5, 1) == "1")
                { CallBoxBtnList.Add(3); }
                else
                { CallBoxBtnCloseList.Add(3); }
                if (BtnPortStr.Substring(6, 1) == "1")
                { CallBoxBtnList.Add(2); }
                else
                { CallBoxBtnCloseList.Add(2); }
                if (BtnPortStr.Substring(7, 1) == "1")
                { CallBoxBtnList.Add(1); }
                else
                { CallBoxBtnCloseList.Add(1); }


                //记忆上次被开的按钮
                string OldCallBoxBtnList = CallBox.CallBoxBtnList;
                CallBox.CallBoxBtnList = string.Join(",", CallBoxBtnList.Select(p => p));

                CallBoxInfo BoxInfo = AGVServerDAccess.LoadAllCallBoxByID(CallBoxID);
                IList<CallBoxDetail> BoxDetails = AGVServerDAccess.LoadCallBoxDetails(CallBoxID);
                //根据按钮盒信息处理业务逻辑
                foreach (int BtnID in CallBoxBtnList)
                {
                    CallBoxDetail CurrBoxDetail = BoxDetails.FirstOrDefault(p => p.CallBoxID == CallBoxID && p.ButtonID == BtnID);
                    if (CurrBoxDetail.OperaType != 3)
                    {
                        LightContrl(ClientSession, BtnID, 1);
                    }
                  
                    //抽取当前按钮盒当前按钮对应的操作类型
                    LogHelper.WriteCallBoxLog("接受到" + CallBox.CallBoxID + "号按钮盒按键" + BtnID);

                    //处理按钮盒的过程为：
                    //1.接受到按钮盒的请求后,产生任务后将对应的按钮灯点亮
                    //2.当有车领取了该任务后将按钮等熄灭
                    #region 根据配置生成任务
                    if (BoxInfo == null)
                    {
                        LogHelper.WriteCallBoxLog("未配置按钮盒" + CallBox.CallBoxID.ToString() + "档案信息");
                        Thread.Sleep(1000 * 3);
                        LightContrl(ClientSession, BtnID, 0);
                        continue;
                    }

                    if (BoxDetails == null || (BoxDetails != null && BoxDetails.Count <= 0))
                    {
                        LogHelper.WriteCallBoxLog("未配置按钮盒" + CallBox.CallBoxID.ToString() + "明细档案信息1");
                        Thread.Sleep(1000 * 3);
                        LightContrl(ClientSession, BtnID, 0);
                        continue;
                    }

                    
                    if (CurrBoxDetail == null)
                    {
                        LogHelper.WriteCallBoxLog("未配置按钮盒" + CallBox.CallBoxID.ToString() + "明细档案信息2");
                        Thread.Sleep(1000 * 3);
                        LightContrl(ClientSession, BtnID, 0);
                        continue;
                    }

                    if (CurrBoxDetail.OperaType == 0)//呼叫
                    {
                        bool HandleCallResult = HandleCallBoxCall(CallBox, CurrBoxDetail, BtnID);
                        if (!HandleCallResult)
                        {
                            Thread.Sleep(1000 * 3);
                            LightContrl(ClientSession, BtnID, 0);
                            continue;
                        }
                        else
                        {
                            Thread.Sleep(3000);
                            LightContrl(ClientSession, BtnID, 0);
                        }
                    }
                    else if (CurrBoxDetail.OperaType == 1)//监控
                    {
                        bool HandleCallBoxMonitorResult = HandleCallBoxMonitor(CallBox, CurrBoxDetail, BtnID);
                        if (!HandleCallBoxMonitorResult)
                        {
                            Thread.Sleep(1000 * 3);
                            LightContrl(ClientSession, BtnID, 0);
                            continue;
                        }
                        else
                        {
                            Thread.Sleep(3000);
                            LightContrl(ClientSession, BtnID, 0);
                        }
                    }
                    else if (CurrBoxDetail.OperaType == 3)//传感器监控
                    {
                        if (string.IsNullOrEmpty(OldCallBoxBtnList) || (!string.IsNullOrEmpty(OldCallBoxBtnList) && !OldCallBoxBtnList.Contains(BtnID.ToString())))
                        {
                            hs_PreReciveSensorTime[CurrBoxDetail.CallBoxID.ToString() + "," + CurrBoxDetail.ButtonID.ToString()] = DateTime.Now;
                        }
                        else
                        {
                            double FilterTime = 0;
                            //传感器监控需要增加滤波功能
                            string FilterTimeStr = CoreData.SysParameter["FilterTime"].ToString();
                            if (!string.IsNullOrEmpty(FilterTimeStr))
                            {
                                try
                                {
                                    FilterTime = Convert.ToDouble(FilterTimeStr);
                                }
                                catch
                                { }
                            }
                            ArrayList keyList = new ArrayList(hs_PreReciveSensorTime.Keys);
                            if (!keyList.Contains(CurrBoxDetail.CallBoxID.ToString() + "," + CurrBoxDetail.ButtonID.ToString()))
                            {
                                hs_PreReciveSensorTime[CurrBoxDetail.CallBoxID.ToString() + "," + CurrBoxDetail.ButtonID.ToString()] = DateTime.Now;
                            }
                            else
                            {
                                DateTime PreReciveSensorTime = Convert.ToDateTime(hs_PreReciveSensorTime[CurrBoxDetail.CallBoxID.ToString() + "," + CurrBoxDetail.ButtonID.ToString()]);
                                double SpanTime = (DateTime.Now - PreReciveSensorTime).TotalSeconds;
                                if (FilterTime <= SpanTime)
                                {
                                    HandleCallBoxMonitor(CallBox, CurrBoxDetail, BtnID);
                                    hs_PreReciveSensorTime[CurrBoxDetail.CallBoxID.ToString() + "," + CurrBoxDetail.ButtonID.ToString()] = DateTime.Now;
                                }
                                else
                                { continue; }
                            }
                        }
                    }
                    else//确认放行
                    {
                        bool HandleCallBoxReleaseResult = HandleCallBoxRelease(CallBox, CurrBoxDetail, BtnID);
                        if (!HandleCallBoxReleaseResult)
                        {
                            Thread.Sleep(1000 * 3);
                            LightContrl(ClientSession, BtnID, 0);
                            continue;
                        }
                        else
                        {
                            Thread.Sleep(3000);
                            LightContrl(ClientSession, BtnID, 0);
                        }
                    }
                    #endregion
                    //Thread.Sleep(3000);
                    //LightContrl(ClientSession, BtnID, 0);
                }
                
                ///将传感器没有物品时需要将对应储位状态清空
                foreach (int BtnID in CallBoxBtnCloseList)
                {
                    if (BoxDetails == null || (BoxDetails != null && BoxDetails.Count <= 0))
                    {
                        LogHelper.WriteCallBoxLog("未配置按钮盒" + CallBox.CallBoxID.ToString() + "明细档案信息4");
                        continue;
                    }
                    CallBoxDetail CurrBoxDetail = BoxDetails.FirstOrDefault(p => p.CallBoxID == CallBoxID && p.ButtonID == BtnID);
                    if (CurrBoxDetail == null)
                    {
                        //LogHelper.WriteCallBoxLog("未配置按钮盒" + CallBox.CallBoxID.ToString() + "明细档案信息");
                        continue;
                    }
                    if (CurrBoxDetail.OperaType == 3)//传感器监控
                    {
                        ArrayList keyList = new ArrayList(hs_PreReciveSensorTime.Keys);
                        if (!keyList.Contains(CurrBoxDetail.CallBoxID.ToString() + "," + CurrBoxDetail.ButtonID.ToString()))
                        {
                            if (string.IsNullOrEmpty(OldCallBoxBtnList) || (!string.IsNullOrEmpty(OldCallBoxBtnList) && OldCallBoxBtnList.Contains(BtnID.ToString())))
                            {
                                hs_PreReciveSensorTime[CurrBoxDetail.CallBoxID.ToString() + "," + CurrBoxDetail.ButtonID.ToString()] = DateTime.Now;
                            }
                        }
                        else
                        {
                            double FilterTime = 0;
                            //传感器监控需要增加滤波功能
                            string FilterTimeStr = CoreData.SysParameter["FilterTime"].ToString();
                            if (!string.IsNullOrEmpty(FilterTimeStr))
                            {
                                try
                                {
                                    FilterTime = Convert.ToDouble(FilterTimeStr);
                                }
                                catch
                                { }
                            }
                            keyList = new ArrayList(hs_PreReciveSensorTime.Keys);
                            if (!keyList.Contains(CurrBoxDetail.CallBoxID.ToString() + "," + CurrBoxDetail.ButtonID.ToString()))
                            {
                                hs_PreReciveSensorTime[CurrBoxDetail.CallBoxID.ToString() + "," + CurrBoxDetail.ButtonID.ToString()] = DateTime.Now;
                            }
                            else
                            {
                                DateTime PreReciveSensorTime = Convert.ToDateTime(hs_PreReciveSensorTime[CurrBoxDetail.CallBoxID.ToString() + "," + CurrBoxDetail.ButtonID.ToString()]);
                                double SpanTime = (DateTime.Now - PreReciveSensorTime).TotalSeconds;
                                if (FilterTime <= SpanTime)
                                {
                                    CurrBoxDetail.LocationState = 0;
                                    HandleCallBoxMonitor(CallBox, CurrBoxDetail, BtnID);
                                    hs_PreReciveSensorTime[CurrBoxDetail.CallBoxID.ToString() + "," + CurrBoxDetail.ButtonID.ToString()] = DateTime.Now;
                                }
                                else
                                { continue; }
                            }
                        }
                    }
                }
                //}
            }
            catch (Exception ex)
            { LogHelper.WriteCallBoxLog("呼叫器处理异常:" + ex.Message); }
        }

        /// <summary>
        /// 处理呼叫盒呼叫
        /// </summary>
        /// <param name="CallBoxID">呼叫盒ID</param>
        /// <param name="BtnID">按钮ID</param>
        /// <returns></returns>
        private bool HandleCallBoxCall(CallBoxInfo CallBox, CallBoxDetail CurrBoxDetail, int BtnID)
        {
            try
            {
                #region 处理呼叫逻辑
                //加载是否启用储位状态
                string IsUserStoreState = CoreData.SysParameter["IsUserStoreState"].ToString();
                if (string.IsNullOrEmpty(IsUserStoreState)) { IsUserStoreState = "否"; }
                int CallBoxID = CallBox.CallBoxID;
                IList<TaskConfigDetail> TaskConfigDetails = AGVClientDAccess.load_TaskDetail(CurrBoxDetail.TaskConditonCode);
                if (TaskConfigDetails == null && (TaskConfigDetails != null && TaskConfigDetails.Count <= 0))
                {
                    LogHelper.WriteCallBoxLog("未配置按钮盒" + CallBox.CallBoxID.ToString() + "任务信息");
                    return false;
                }

                //开始根据任务配置信息创建任务
                StorageInfo CallStore = CoreData.StorageList.FirstOrDefault(q => q.ID == CurrBoxDetail.LocationID);
                if (CallStore == null)
                {
                    LogHelper.WriteCallBoxLog("未配置按钮盒" + CallBox.CallBoxID.ToString() + "对应的监控储位");
                    return false;
                }
                //验证当前按钮盒下是否存在未完成的任务
                if (AGVSimulationDAccess.ChekAllowCreatTask(CallBoxID, CallStore.LankMarkCode, BtnID) > 0)
                {
                    LogHelper.WriteCallBoxLog("按钮盒" + CallBox.CallBoxID.ToString() + "对应呼叫存在未完成任务,请稍后再试!");
                    return false;
                }

                //组装任务信息
                string dispatchNo = Guid.NewGuid().ToString();
                DispatchTaskInfo TaskInfo = new DispatchTaskInfo();
                TaskInfo.dispatchNo = dispatchNo;
                TaskInfo.TaskState = 0;
                TaskInfo.CallLand = CallStore.LankMarkCode;
                TaskInfo.stationNo = CallBoxID;
                TaskInfo.CallID = BtnID;
                TaskInfo.taskType = 0;

                //创建任务明细
                int DetailID = 1;
                int PrePutType = -1;
                foreach (TaskConfigDetail item in TaskConfigDetails)
                {
                    //通过任务任务配置明细寻找目标地标
                    StorageInfo ArmStore = null;
                    LogHelper.WriteCallBoxLog("呼叫储位为：" + item.ArmOwnArea.ToString());
                    if (item.ArmOwnArea == -1)
                    {
                        ArmStore = CallStore;
                        LogHelper.WriteCallBoxLog("呼叫储位地标为：" + CallStore.LankMarkCode);
                    }
                    else
                    {
                        if (IsUserStoreState == "否")
                        { ArmStore = CoreData.StorageList.FirstOrDefault(p => p.OwnArea == item.ArmOwnArea && p.StorageState == item.StorageState && p.MaterielType == item.MaterialType); }
                        else
                        { ArmStore = CoreData.StorageList.FirstOrDefault(p => p.OwnArea == item.ArmOwnArea && p.StorageState == item.StorageState && p.MaterielType == item.MaterialType && p.LockState == 0); }
                    }
                    if (ArmStore == null)
                    {
                        LogHelper.WriteCallBoxLog("按钮盒" + CallBox.CallBoxID.ToString() + "对应呼叫,目的储位不存在[原因:不存在或被占用]");
                        return false;
                    }

                    //再通过配置任务号来处理必经地标
                    IList<TaskConfigMustPass> ConfigMustPasses = AGVServerDAccess.LoadTaskMustPass(item.TaskConditonCode, item.DetailID);
                    if (ConfigMustPasses != null && ConfigMustPasses.Count > 0)
                    {
                        ////如果配置的必经地标则需要产生任务经过必经地标
                        foreach (TaskConfigMustPass MustPassItem in ConfigMustPasses)
                        {
                            foreach (IOActionInfo Action in MustPassItem.MustPassIOAction)
                            {
                                IOActionInfo CurrIOActionInfo = AGVServerDAccess.LoadAllIOAction(Action.ActionID);
                                if (CurrIOActionInfo == null)
                                {
                                    LogHelper.WriteCallBoxLog("按钮盒" + CallBox.CallBoxID.ToString() + "对应呼叫,必经地标中的IO动作" + Action.ActionID.ToString() + "档案信息不存在");
                                    continue;
                                }
                                DispatchTaskDetail newTaskDetail = new DispatchTaskDetail();
                                newTaskDetail.dispatchNo = dispatchNo;
                                newTaskDetail.DetailID = DetailID;
                                newTaskDetail.LandCode = MustPassItem.MustPassLandCode;
                                newTaskDetail.IsAllowExcute = Action.IsPass;
                                newTaskDetail.IsSensorStop = 0;
                                newTaskDetail.OperType = -1;
                                newTaskDetail.IsWait = Action.IsWait;
                                newTaskDetail.WaitTime = Action.WaitTime;
                                newTaskDetail.TaskConditonCode = MustPassItem.TaskConditonCode;
                                newTaskDetail.TaskConfigDetailID = item.DetailID;
                                TaskInfo.TaskDetail.Add(newTaskDetail);
                            }
                        }





                        //foreach (TaskConfigMustPass MustPassItem in ConfigMustPasses)
                        //{
                        //    //根据配置的IO动作ID加载对应的IO动作明细信息
                        //    IOActionInfo CurrIOActionInfo = AGVServerDAccess.LoadAllIOAction(MustPassItem.Action);
                        //    if (CurrIOActionInfo == null)
                        //    {
                        //        LogHelper.WriteCallBoxLog("按钮盒" + CallBox.CallBoxID.ToString() + "对应呼叫,必经地标中的IO动作" + MustPassItem.Action.ToString() + "档案信息不存在");
                        //        continue;
                        //    }
                        //    //找到必经地标中的IO动作后组装任务体
                        //    DispatchTaskDetail dispathDetail_MustPas = new DispatchTaskDetail();
                        //    dispathDetail_MustPas.dispatchNo = dispatchNo;
                        //    dispathDetail_MustPas.DetailID = DetailID;
                        //    dispathDetail_MustPas.LandCode = MustPassItem.MustPassLandCode;
                        //    dispathDetail_MustPas.OperType = -1;
                        //    dispathDetail_MustPas.IsAllowExcute = CurrIOActionInfo.IsWait;
                        //    dispathDetail_MustPas.PutType = -1;
                        //    dispathDetail_MustPas.IOActionID = CurrIOActionInfo.ActionID;
                        //    TaskInfo.TaskDetail.Add(dispathDetail_MustPas);
                        //    DetailID += 1;
                        //}//结束循环处理必经地标配置
                    }

                    //如果没有必经地标或者添加完必经地标后需要继续添加后续的任务配置明细
                    DispatchTaskDetail dispathDetail_TaskConfig = new DispatchTaskDetail();
                    dispathDetail_TaskConfig.dispatchNo = dispatchNo;
                    dispathDetail_TaskConfig.DetailID = DetailID;
                    dispathDetail_TaskConfig.LandCode = ArmStore.LankMarkCode;
                    dispathDetail_TaskConfig.OperType = item.Action;
                    dispathDetail_TaskConfig.IsAllowExcute = item.IsWaitPass;
                    dispathDetail_TaskConfig.PassType = item.PassType;
                    if (PrePutType == -1)
                    { dispathDetail_TaskConfig.PutType = ArmStore.StorageState; }
                    else
                    { dispathDetail_TaskConfig.PutType = PrePutType; }
                    dispathDetail_TaskConfig.IsSensorStop = item.IsSensorStop;
                    TaskInfo.TaskDetail.Add(dispathDetail_TaskConfig);
                    PrePutType = ArmStore.StorageState;
                    DetailID += 1;
                }//结束循环处理任务配置

                //循环组装完任务信息后保存到数据库
                if (TaskInfo != null && TaskInfo.TaskDetail.Count > 0)
                {
                    AGVServerDAccess.CreatTaskInfo(TaskInfo, IsUserStoreState == "是" ? true : false);
                }
                return true;
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.WriteCallBoxLog("处理呼叫器呼叫异常:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 处理呼叫盒呼叫
        /// </summary>
        /// <param name="CallBoxID">呼叫盒ID</param>
        /// <param name="BtnID">按钮ID</param>
        /// <returns></returns>
        private bool HandleCallBoxMonitor(CallBoxInfo CallBox, CallBoxDetail CurrBoxDetail, int BtnID)
        {
            try
            {
                #region 处理监控功能
                //处理监控功能
                //监控功能则为更新储位的状态
                StorageInfo CallStore = CoreData.StorageList.FirstOrDefault(q => q.ID == CurrBoxDetail.LocationID);
                if (CallStore == null)
                {
                    LogHelper.WriteCallBoxLog("未配置按钮盒" + CallBox.CallBoxID.ToString() + "对应的监控储位");
                    return false;
                }
                int StoreState = CurrBoxDetail.LocationState;
                if (CallStore.StorageState != StoreState)
                {
                    //更新数据库中的对应监控的储位状态信息
                    AGVServerDAccess.UpdateStorageState(StoreState, -1, CallStore.ID);
                    CallStore.StorageState = StoreState;
                }
                return true;
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.WriteCallBoxLog("处理呼叫器监控异常:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 处理呼叫盒放行逻辑
        /// </summary>
        /// <returns></returns>
        private bool HandleCallBoxRelease(CallBoxInfo CallBox, CallBoxDetail CurrBoxDetail, int BtnID)
        {
            try
            {
                #region 处理呼叫器放行逻辑
                //接收到按钮盒放行命令
                //取出按钮对应监控的储位
                StorageInfo CallStore = CoreData.StorageList.FirstOrDefault(q => q.ID == CurrBoxDetail.LocationID);
                if (CallStore == null)
                {
                    LogHelper.WriteCallBoxLog("未配置按钮盒" + CallBox.CallBoxID.ToString() + "对应的监控储位");
                    return false;
                }
                //然后将停靠监控储位地标上的AGV允许放行
                string WaintLandCode = CallStore.LankMarkCode;
                if (string.IsNullOrEmpty(WaintLandCode))
                {
                    LogHelper.WriteCallBoxLog("未配置按钮盒" + CallBox.CallBoxID.ToString() + "对应的监控储位地标");
                    return false;
                }
                CarInfo WaitCar = CoreData.CarList.FirstOrDefault(p => p.CurrSite == Convert.ToInt32(WaintLandCode));
                if (WaitCar == null)
                {
                    LogHelper.WriteCallBoxLog("按钮盒" + CallBox.CallBoxID.ToString() + "对应的监控储位地标上没有小车");
                    return false;
                }
                if (string.IsNullOrEmpty(WaitCar.ExcuteTaksNo))
                {
                    LogHelper.WriteCreatTaskLog("放行地标上的小车没有可执行任务");
                    return false;
                }
                AGVServerDAccess.ReleaseCarByCallBox(WaitCar.ExcuteTaksNo, WaitCar.CurrSite.ToString());
                return true;
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.WriteCallBoxLog("处理呼叫器放行异常:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 按钮灯控制
        /// </summary>
        /// <param name="session"></param>
        /// <param name="btnID"></param>
        /// <param name="onoroff"></param>
        public void LightContrl(AppSession session, int btnID, int onoroff)
        {
            try
            {
                LogHelper.WriteCallBoxLog("开关按钮灯:" + btnID.ToString() + "," + onoroff.ToString());
                IList<byte> lightbytelist = new List<byte>() { 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x01, 0x0f, 0x00, 0x00, 0x00, 0x01, 0x01, 0x00 };
                lightbytelist[9] = (byte)(btnID - 1);
                lightbytelist[13] = (byte)(onoroff);
                session.ClientSocket.Send(lightbytelist.ToArray());
            }
            catch (Exception)
            {
                LogHelper.WriteLog("OffLight 关闭灯异常");
            }

        }
        #endregion
    }
}
