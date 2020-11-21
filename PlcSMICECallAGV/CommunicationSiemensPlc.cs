using AGVCore;
using AGVDAccess;
using HslCommunication;
using HslCommunication.Profinet.Siemens;
using Model.MDM;
using Model.MSM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tools;

namespace PlcSMICECallAGV
{
    public class CommunicationSiemensPlc
    {

        #region 属性
        private static object lockstorage = new object();

        /// <summary>
        /// 通信参数
        /// </summary>
        public SiemensConnectInfo ConnParam { get; set; }
        /// <summary>
        /// PLC编号
        /// </summary>
        public int PLCCode { get; set; }
        /// <summary>
        /// S7系列通信组件
        /// </summary>
        SiemensS7Net melsecMc = null;

        private Thread processor;
        #endregion

        #region 自定义函数

        public CommunicationSiemensPlc(int pclCode, SiemensConnectInfo Param)
        {
            this.PLCCode = pclCode;
            this.ConnParam = Param;
        }




        public bool InitSiemens()
        {
            try
            {
                melsecMc = new SiemensS7Net(SiemensPLCS.S1200) { IpAddress = this.ConnParam.ServerIP, Port = this.ConnParam.Port };
               
                return true;
            }
            catch (Exception ex)
            {
                DelegateState.InvokeDispatchStateEvent(ex.Message);
                return false;
            }
        }

        public void Start()
        {
            Task.Run(() =>
            {
                try
                {
                    //while (true)
                    //{
                    //    OperateResult opr = melsecMc.ConnectServer();
                    //    if (!opr.IsSuccess)
                    //    {
                    //        DelegateState.InvokeDispatchStateEvent("呼叫器:" + this.PLCCode.ToString() + "IP:" + melsecMc.IpAddress + "连接失败! 将重新尝试连接...");
                    //        Thread.Sleep(60 * 1000);
                    //        //return false;
                    //    }
                    //    else
                    //    {
                    //        break;
                    //    }
                    //}
                    melsecMc.SetPersistentConnection();
                    DelegateState.InvokeDispatchStateEvent("呼叫器:" + this.PLCCode.ToString() + "IP:" + melsecMc.IpAddress + "连接成功！");
                    processor = new Thread(Polling);
                    processor.IsBackground = true;
                    processor.Start();
                }
                catch (Exception ex)
                {
                    DelegateState.InvokeDispatchStateEvent(ex.Message);
                }
            });
        }

        public bool Stop()
        {
            try
            {
                melsecMc.ConnectClose();
                melsecMc.Dispose();
                if (processor != null)
                {
                    processor.Abort();
                    //解决线程过多情况,因为线程执行Abort，状态是AbortRequested,还是会存在继续执行
                    while (processor.ThreadState != ThreadState.Aborted)
                    { Thread.Sleep(100); }
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
        /// 轮询
        /// </summary>
        private void Polling()
        {

            //定时轮询PLC
            while (true)
            {

                try
                {

                    //根据PLCCode加载呼叫盒子信息
                    CallBoxInfo CallInfo = CoreData.AllCallBoxes.FirstOrDefault(P => P.CallBoxID == PLCCode);
                    if (CallInfo == null)
                    {
                        LogHelper.WriteCallBoxLog("未匹配到呼叫器:" + PLCCode + "档案信息!");
                        return;
                    }

                    int readLen = CallInfo.ReadLenth;
                    if (readLen == 0)
                    {
                        LogHelper.WriteCallBoxLog("未配置呼叫器:" + PLCCode + "读取长度!");
                        return;
                    }
                    if (CallInfo.CallType == 1)//呼叫类型的
                    {
                        var t = CoreData.AllCallBoxDetail.Where(z => z.CallBoxID == PLCCode).Select(k => k.DBAddress).Distinct();
                        foreach (var address in t)
                        {
                            OperateResult<short[]> read = melsecMc.ReadInt16(address, (ushort)readLen);
                            if (!read.IsSuccess)
                            {
                                LogHelper.WriteCallBoxLog("读取呼叫器失败!  " + string.Format("[{0},{2},{1}]", PLCCode, address, ConnParam.ServerIP));
                                OperateResult opr = melsecMc.ConnectServer();
                                melsecMc.ConnectClose();
                                melsecMc.Dispose();
                                Thread.Sleep(5000);
                                melsecMc = new SiemensS7Net(SiemensPLCS.S1200) { IpAddress = this.ConnParam.ServerIP, Port = this.ConnParam.Port };
                                melsecMc.SetPersistentConnection();

                                LogHelper.WriteCallBoxLog("重连呼叫器!  " + string.Format("[{0},{2},{1}] ", PLCCode, address, ConnParam.ServerIP));
                                continue;
                            }
                            string SenDLog = "";
                            foreach (short item in read.Content)
                            { SenDLog += ((int)item).ToString("X") + " "; }
                            LogHelper.WriteCallBoxLog("读取呼叫器 " + string.Format("[{0},{2},{1}]", PLCCode, address, ConnParam.ServerIP) + "内容为:" + SenDLog);
                            int site = Convert.ToInt32(read.Content[0]);
                            int PlcRequestState = Convert.ToInt32(read.Content[2]);
                            if (PlcRequestState == 1)
                            {
                                int RequestContent = Convert.ToInt32(read.Content[1]);
                                var cdetail = CoreData.AllCallBoxDetail.FirstOrDefault(p => p.CallBoxID == PLCCode && p.DBAddress == address && p.ButtonID == RequestContent);
                                bool HandleCallResult = HandleCallBoxCall(CallInfo, cdetail, cdetail.ButtonID);
                                if (!HandleCallResult)
                                {
                                    LogHelper.WriteCallBoxLog("呼叫器:" + PLCCode + "地址 ：" + address + " 呼叫失败!");
                                }
                                #region 处理需要回写的子任务
                                IList<DispatchTaskDetail> detaillist = AGVClientDAccess.LoadTaskDetailByPLCCode(PLCCode, cdetail.LocationID);
                                foreach (var detail in detaillist)
                                {
                                    if (detail != null && (!string.IsNullOrEmpty(detail.dispatchNo)))
                                    {
                                        //写确认信息
                                        List<byte> write = new List<byte>();
                                        //{ BitConverter.GetBytes(read.Content[0]), BitConverter.GetBytes(read.Content[1]) , BitConverter.GetBytes(read.Content[2])}
                                        write.Add(BitConverter.GetBytes(read.Content[0])[1]);
                                        write.Add(BitConverter.GetBytes(read.Content[0])[0]);
                                        write.Add(BitConverter.GetBytes(read.Content[1])[1]);
                                        write.Add(BitConverter.GetBytes(read.Content[1])[0]);
                                        write.Add(BitConverter.GetBytes(read.Content[2])[1]);
                                        write.Add(BitConverter.GetBytes(read.Content[2])[0]);
                                        OperateResult opr = melsecMc.Write(address.Replace(".0", ".6"), write.ToArray());
                                        int writcount = 1;
                                        while (!opr.IsSuccess)
                                        {
                                            if (writcount > 3)
                                            {
                                                opr = melsecMc.Write(address, write.ToArray());
                                                LogHelper.WriteCallBoxLog("回写呼叫器:" + PLCCode + "地址 ：" + address + "失败!");
                                                break;
                                            }
                                            writcount++;
                                            Thread.Sleep(1000);
                                        }
                                        if (opr.IsSuccess)
                                        {
                                            AGVClientDAccess.UpdatePlcCallBackState(detail.dispatchNo, detail.DetailID);
                                            LogHelper.WriteCallBoxLog("回写呼叫器:" + PLCCode + "地址 ：" + address + "成功!");
                                        }
                                    }
                                }
                                #endregion

                                #region 处理需要回写物料信息的子任务
                                IList<DispatchTaskDetail> goodsdetaillist = AGVClientDAccess.LoadGoodsInfoTaskDetailByPLCCode(PLCCode, cdetail.LocationID);
                                foreach (var detail in goodsdetaillist)
                                {
                                    if (detail != null && (!string.IsNullOrEmpty(detail.dispatchNo)) && (!string.IsNullOrEmpty(detail.GoodsInfo)))
                                    {
                                        //写确认信息
                                        List<byte> write = new List<byte>();
                                        string[] arr = detail.GoodsInfo.Split(',');
                                        foreach (var s in arr)
                                        {
                                            if (!string.IsNullOrEmpty(s))
                                            {
                                                short ts = short.Parse(s);
                                                write.Add(BitConverter.GetBytes(ts)[1]);
                                                write.Add(BitConverter.GetBytes(ts)[0]);
                                            }
                                        }
                                        OperateResult opr = melsecMc.Write(address.Replace(".0", ".12"), write.ToArray());
                                        int writcount = 1;
                                        while (!opr.IsSuccess)
                                        {
                                            if (writcount > 3)
                                            {
                                                opr = melsecMc.Write(address.Replace(".0", ".12"), write.ToArray());
                                                LogHelper.WriteCallBoxLog("回写呼叫器 物料编码:" + PLCCode + "地址 ：" + address + "失败!");
                                                break;
                                            }
                                            writcount++;
                                            Thread.Sleep(1000);
                                        }
                                        if (opr.IsSuccess)
                                        {
                                            AGVClientDAccess.UpdateGoodsCallBackState(detail.dispatchNo, detail.DetailID);
                                            LogHelper.WriteCallBoxLog("回写呼叫器 物料编码:" + PLCCode + "地址 ：" + address + "成功!");
                                        }
                                    }
                                }
                                #endregion
                            }
                        }

                        #region OlD
                        ////根据当前呼叫器ID加载呼叫器明细
                        //OperateResult<short[]> read = melsecMc.ReadInt16(ReadAddr, (ushort)readLen);
                        //if (!read.IsSuccess)
                        //{
                        //    LogHelper.WriteCallBoxLog("读取呼叫器:" + PLCCode + "失败!");
                        //    return;
                        //}

                        //string SenDLog = "";
                        //foreach (byte item in read.Content)
                        //{ SenDLog += ((int)item).ToString("X") + " "; }
                        //LogHelper.WriteCallBoxLog("读取呼叫器:" + PLCCode + "内容为:" + SenDLog);

                        //int site = Convert.ToInt32(read.Content[0]);
                        //int PlcRequestState = Convert.ToInt32(read.Content[1]);
                        //string[] codes = GetCode(read.Content);
                        //if (codes.Length != 2)
                        //{
                        //    LogHelper.WriteCallBoxLog("读取呼叫器:" + PLCCode + " 物料编码获取失败");
                        //}
                        //if (PlcRequestState == 1)
                        //{
                        //    int RequestContent = Convert.ToInt32(read.Content[2]);
                        //    CallBoxDetail CallDetail = CoreData.AllCallBoxDetail.FirstOrDefault(p => p.CallBoxID == PLCCode && p.ButtonID == RequestContent);
                        //    if (CallDetail == null)
                        //    {
                        //        LogHelper.WriteCallBoxLog("未配置呼叫器:" + PLCCode + "按钮号:" + RequestContent.ToString() + "明细!");
                        //        return;
                        //    }
                        //    bool HandleCallResult = HandleCallBoxCall(CallInfo, CallDetail, RequestContent, codes);

                        //    if (HandleCallResult)
                        //    {
                        //        ////写确认信息
                        //        //byte[] write = new byte[] { (byte)site, 0x00, (byte)RequestContent, (byte)site, 0x01, (byte)RequestContent };
                        //        //OperateResult opr = melsecMc.Write(ReadAddr, write);
                        //        //int writcount = 1;
                        //        //while (!opr.IsSuccess)
                        //        //{
                        //        //    if (writcount > 3)
                        //        //    {
                        //        //        opr = melsecMc.Write(ReadAddr, write);
                        //        //        LogHelper.WriteCallBoxLog("回写呼叫器:" + PLCCode + "失败!");
                        //        //        break;
                        //        //    }
                        //        //    writcount++;
                        //        //    Thread.Sleep(1000);
                        //        //}
                        //        //if (opr.IsSuccess)
                        //        //{
                        //        //    LogHelper.WriteCallBoxLog("回写呼叫器:" + PLCCode + "成功!");
                        //        //}

                        //        //LogHelper.WriteCallBoxLog("呼叫器:" + PLCCode + "呼叫成功!");
                        //    }
                        //    else
                        //    {
                        //        LogHelper.WriteCallBoxLog("呼叫器:" + PLCCode + "呼叫失败!");
                        //    }


                        //    #region 处理需要回写的子任务
                        //    IList<DispatchTaskDetail> detaillist = AGVClientDAccess.LoadTaskDetailByPLCCode(PLCCode, CallDetail.LocationID);
                        //    foreach (var detail in detaillist)
                        //    {
                        //        if (detail != null && (!string.IsNullOrEmpty(detail.dispatchNo)))
                        //        {
                        //            //写确认信息
                        //            List<byte> write = new List<byte>();
                        //            //{ BitConverter.GetBytes(read.Content[0]), BitConverter.GetBytes(read.Content[1]) , BitConverter.GetBytes(read.Content[2])}
                        //            write.Add(BitConverter.GetBytes(read.Content[0])[1]);
                        //            write.Add(BitConverter.GetBytes(read.Content[0])[0]);
                        //            write.Add(BitConverter.GetBytes(read.Content[1])[1]);
                        //            write.Add(BitConverter.GetBytes(read.Content[1])[0]);
                        //            write.Add(BitConverter.GetBytes(read.Content[2])[1]);
                        //            write.Add(BitConverter.GetBytes(read.Content[2])[0]);


                        //            OperateResult opr = melsecMc.Write(ReadAddr.Replace(".0", ".6"), write.ToArray());
                        //            int writcount = 1;
                        //            while (!opr.IsSuccess)
                        //            {
                        //                if (writcount > 3)
                        //                {
                        //                    opr = melsecMc.Write(ReadAddr, write.ToArray());
                        //                    LogHelper.WriteCallBoxLog("回写呼叫器:" + PLCCode + "失败!");
                        //                    break;
                        //                }
                        //                writcount++;
                        //                Thread.Sleep(1000);
                        //            }
                        //            if (opr.IsSuccess)
                        //            {
                        //                AGVClientDAccess.UpdatePlcCallBackState(detail.dispatchNo, detail.DetailID);
                        //                LogHelper.WriteCallBoxLog("回写呼叫器:" + PLCCode + "成功!");
                        //            }
                        //            LogHelper.WriteCallBoxLog("呼叫器:" + PLCCode + "呼叫成功!");
                        //        }
                        //    }
                        //    #endregion

                        //}
                        #endregion

                    }
                    else if (CallInfo.CallType == 2)//监控
                    {

                        var t = CoreData.AllCallBoxDetail.Where(p => p.CallBoxID == CallInfo.CallBoxID);
                        foreach (var d in t)
                        {
                            //if (d.DBAddress != "DB1000.0")
                            //{
                            //    continue;
                            //}
                            if (d.LocationState == 0)
                            {
                                //根据当前呼叫器ID加载呼叫器明细
                                OperateResult<short[]> read = melsecMc.ReadInt16(d.DBAddress, 1);
                                if (!read.IsSuccess)
                                {
                                    LogHelper.WriteCallBoxLog("读取呼叫器失败!  " + string.Format("[{0},{2},{1}]", PLCCode, d.DBAddress, ConnParam.ServerIP));


                                    melsecMc.ConnectClose();
                                    melsecMc.Dispose();
                                    Thread.Sleep(5000);
                                    melsecMc = new SiemensS7Net(SiemensPLCS.S1200) { IpAddress = this.ConnParam.ServerIP, Port = this.ConnParam.Port };
                                    melsecMc.SetPersistentConnection();
                                    
                                    LogHelper.WriteCallBoxLog("重连呼叫器!  " + string.Format("[{0},{2},{1}] ", PLCCode, d.DBAddress, ConnParam.ServerIP));
                                    continue;
                                }
                                string SenDLog = "";
                                foreach (short item in read.Content)
                                { SenDLog += ((int)item).ToString("X") + " "; }
                                LogHelper.WriteCallBoxLog("读取呼叫器 " + string.Format("[{0},{2},{1}]", PLCCode, d.DBAddress, ConnParam.ServerIP) + "内容为:" + SenDLog);
                                if (Convert.ToInt32(read.Content[0]) == 1)
                                {
                                    HandleCallBoxMonitor(CallInfo, d, 0);
                                }
                                else
                                {
                                    HandleCallBoxMonitor(CallInfo, d, 1);
                                }
                            }
                            else if (d.LocationState == 2)
                            {
                                //根据当前呼叫器ID加载呼叫器明细
                                OperateResult<short[]> read = melsecMc.ReadInt16(d.DBAddress, 15);
                                if (!read.IsSuccess)
                                {
                                    LogHelper.WriteCallBoxLog("读取呼叫器 " + string.Format("[{0},{2},{1}]", PLCCode, d.DBAddress, ConnParam.ServerIP) + "失败！");
                                    continue;
                                }
                                string SenDLog = "";
                                foreach (short item in read.Content)
                                { SenDLog += ((int)item).ToString("X") + " "; }
                                LogHelper.WriteCallBoxLog("读取呼叫器 " + string.Format("[{0},{2},{1}]", PLCCode, d.DBAddress, ConnParam.ServerIP) + "内容为:" + SenDLog);
                                string codes = "";
                                foreach (short s in read.Content.Skip(3))
                                {
                                    codes += s.ToString() + ",";
                                }
                                if (Convert.ToInt32(read.Content[0]) == 1)
                                {
                                    HandleCallBoxMonitor(CallInfo, d, 2, codes);
                                }
                                else
                                {
                                    HandleCallBoxMonitor(CallInfo, d, 0, codes);
                                }
                            }
                        }

                    }
                    else//无，呼叫器类型按照按钮来区分，秦川不支持
                    {
                    }
                }
                catch (Exception ex)
                { LogHelper.WriteCallBoxLog("处理呼叫器:" + PLCCode + "任务失败->" + ex.Message); }

                Thread.Sleep(1000 * 2);
            }


        }

        /// <summary>
        /// 处理呼叫器呼叫任务
        /// </summary>
        /// <param name="CallBox"></param>
        /// <param name="CurrBoxDetail"></param>
        /// <param name="BtnID"></param>
        /// <returns></returns>
        private bool HandleCallBoxCall(CallBoxInfo CallBox, CallBoxDetail CurrBoxDetail, int BtnID)
        {
            try
            {
                lock (lockstorage)
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
                    DataTable dtstorage = CoreData.dbOperator.LoadDatas("QueryAllStore");
                    CoreData.StorageList = DataToObject.TableToEntity<StorageInfo>(dtstorage);
                    //开始根据任务配置信息创建任务
                    StorageInfo CallStore = CoreData.StorageList.FirstOrDefault(q => q.ID == CurrBoxDetail.LocationID);
                    if (CallStore == null)
                    {
                        LogHelper.WriteCallBoxLog("未配置按钮盒" + CallBox.CallBoxID.ToString() + "对应的监控储位");
                        return false;
                    }
                    //验证当前按钮盒下是否存在未完成的任务
                    if (AGVSimulationDAccess.ChekAllowCreatTask(CallBoxID, CallStore.LankMarkCode, CurrBoxDetail.ButtonID) > 0)
                    {
                        LogHelper.WriteCallBoxLog("按钮盒" + CallBox.CallBoxID.ToString() + "对应呼叫存在未完成任务,请稍后再试!");
                        return false;
                    }
                    //验证当前按钮盒下是否存在未完成的任务
                    if (AGVSimulationDAccess.ChekAllowCreatTask2(CallBoxID, CallStore.LankMarkCode, CurrBoxDetail.ButtonID) > 0)
                    {
                        LogHelper.WriteCallBoxLog("按钮盒" + CallBox.CallBoxID.ToString() + "对应呼叫存在未完成任务 未回写的任务,请稍后再试!");
                        return false;
                    }
                    AreaInfo callarea = CoreData.AllAreaList.FirstOrDefault(p=>p.OwnArea==CallStore.OwnArea);
                    if (callarea != null && callarea.MaxTaskCount > 0)
                    {
                        if (AGVSimulationDAccess.ChekAllowCreatTask3(callarea.OwnArea) >= callarea.MaxTaskCount)
                        {
                            LogHelper.WriteCallBoxLog("按钮盒" + CallBox.CallBoxIP.ToString() + " 地址" + CurrBoxDetail.DBAddress + " 对应的区域任务数量达到最大，不产生任务");
                            return false;
                        }
                    }
                    if (!string.IsNullOrEmpty(CallStore.StorageName))
                    {
                        LogHelper.WriteCallBoxLog("按钮盒" + CallBox.CallBoxIP.ToString() + " 地址" + CurrBoxDetail.DBAddress + "对应储位未启用");
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
                    TaskInfo.OwerArea = CallStore.OwnArea;
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
                            {
                                ArmStore = CoreData.StorageList.FirstOrDefault(p => p.OwnArea == item.ArmOwnArea && p.StorageState == item.StorageState && p.MaterielType == item.MaterialType && p.LockState == 0);
                            }
                        }
                        if (ArmStore == null)
                        {
                            LogHelper.WriteCallBoxLog(string.Format("按钮盒{0}   地址：{1}   BtnID{2}   对应呼叫,目的储位不存在[原因:不存在或被占用]", CallBox.CallBoxID, CurrBoxDetail.DBAddress, CurrBoxDetail.ButtonID));
                            return false;
                        }

                        #region 不用
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
                                        LogHelper.WriteCallBoxLog("按钮器" + CallBox.CallBoxID.ToString() + "对应呼叫,必经地标中的IO动作" + Action.ActionID.ToString() + "档案信息不存在");
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
                        }
                        #endregion


                        //如果没有必经地标或者添加完必经地标后需要继续添加后续的任务配置明细
                        DispatchTaskDetail dispathDetail_TaskConfig = new DispatchTaskDetail();
                        dispathDetail_TaskConfig.dispatchNo = dispatchNo;
                        dispathDetail_TaskConfig.DetailID = DetailID;
                        dispathDetail_TaskConfig.LandCode = ArmStore.LankMarkCode;
                        dispathDetail_TaskConfig.StorageID = ArmStore.ID;
                        if (!string.IsNullOrEmpty(ArmStore.Remark))
                        {
                            TaskInfo.GoodsInfo = ArmStore.Remark;
                        }
                        dispathDetail_TaskConfig.OperType = item.Action;
                        dispathDetail_TaskConfig.IsAllowExcute = item.IsWaitPass;
                        dispathDetail_TaskConfig.PassType = item.PassType;
                        dispathDetail_TaskConfig.IsNeedCallBack = item.IsNeedCallBack;
                        dispathDetail_TaskConfig.IsCallGoods = item.IsCallGoods;

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
            }
            catch (Exception ex)
            {
                DelegateState.InvokeDispatchStateEvent(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 获得物料编码和批次编码
        /// </summary>
        /// <returns></returns>
        private string[] GetCode(short[] content)
        {
            //物料编码
            StringBuilder sb1 = new StringBuilder();
            sb1.Append(Chr(BitConverter.GetBytes(content[6])[1]));
            sb1.Append(Chr(BitConverter.GetBytes(content[6])[0]));
            sb1.Append(Chr(BitConverter.GetBytes(content[7])[1]));
            sb1.Append(Chr(BitConverter.GetBytes(content[7])[0]));
            sb1.Append(Chr(BitConverter.GetBytes(content[8])[1]));
            sb1.Append(Chr(BitConverter.GetBytes(content[8])[0]));
            sb1.Append(Chr(BitConverter.GetBytes(content[9])[1]));
            sb1.Append(Chr(BitConverter.GetBytes(content[9])[0]));
            sb1.Append(Chr(BitConverter.GetBytes(content[10])[1]));
            sb1.Append(Chr(BitConverter.GetBytes(content[10])[0]));
            sb1.Append(Chr(BitConverter.GetBytes(content[11])[1]));
            sb1.Append(Chr(BitConverter.GetBytes(content[11])[0]));
            sb1.Append(Chr(BitConverter.GetBytes(content[12])[1]));

            StringBuilder sb2 = new StringBuilder();
            sb2.Append(Chr(BitConverter.GetBytes(content[13])[1]));
            sb2.Append(Chr(BitConverter.GetBytes(content[13])[0]));
            sb2.Append(Chr(BitConverter.GetBytes(content[14])[1]));
            sb2.Append(Chr(BitConverter.GetBytes(content[14])[0]));
            sb2.Append(Chr(BitConverter.GetBytes(content[15])[1]));
            sb2.Append(Chr(BitConverter.GetBytes(content[15])[0]));
            sb2.Append(Chr(BitConverter.GetBytes(content[16])[1]));
            sb2.Append(Chr(BitConverter.GetBytes(content[16])[0]));
            sb2.Append(Chr(BitConverter.GetBytes(content[17])[1]));
            sb2.Append(Chr(BitConverter.GetBytes(content[17])[0]));

            return new string[] { sb1.ToString().Trim(), sb2.ToString().Trim() };
        }

        public string Chr(int asciiCode)
        {
            if (asciiCode >= 0 && asciiCode <= 255)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)asciiCode };
                string strCharacter = asciiEncoding.GetString(byteArray);
                return (strCharacter);
            }
            else
            {
                throw new Exception("无效的ASCII码");
            }
        }
        private bool HandleCallBoxMonitor(CallBoxInfo CallBox, CallBoxDetail CurrBoxDetail, int staragestate, string codes = "")
        {
            try
            {
                lock (lockstorage)
                {
                    #region 处理监控功能
                    //处理监控功能
                    //监控功能则为更新储位的状态
                    DataTable dtstorage = CoreData.dbOperator.LoadDatas("QueryAllStore");
                    CoreData.StorageList = DataToObject.TableToEntity<StorageInfo>(dtstorage);
                    StorageInfo CallStore = CoreData.StorageList.FirstOrDefault(q => q.ID == CurrBoxDetail.LocationID);
                    if (CallStore == null)
                    {
                        LogHelper.WriteCallBoxLog("未配置按钮盒" + CallBox.CallBoxID.ToString() + "对应的监控储位");
                        return false;
                    }
                    int StoreState = staragestate;
                    if (CallStore.StorageState != StoreState)
                    {
                        //更新数据库中的对应监控的储位状态信息
                        AGVServerDAccess.UpdateStorageState(StoreState, -1, CallStore.ID);
                        if (!string.IsNullOrEmpty(codes))
                        {
                            //更新备注
                            AGVServerDAccess.UpdteStorageRemark(CallStore.ID, codes);
                        }
                        //更新储位的名称 逗号分隔
                        CallStore.StorageState = StoreState;
                        CallStore.Remark = codes;
                    }
                    return true;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                DelegateState.InvokeDispatchStateEvent(ex.Message);
                return false;
            }
        }
        #endregion

    }
}
