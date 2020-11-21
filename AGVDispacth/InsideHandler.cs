using AGVCore;
using AGVDispacth.SupperSocket;
using DipatchModel;
using SocketModel;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Tools;
using Model.MDM;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Globalization;
using Model.Comoon;
using System.Xml;
using SQLServerOperator;
using System.IO;

namespace AGVDispacth
{
    /// <summary>
    /// 调度系统内部通信处理类
    /// </summary>
    public class InsideHandler
    {
        #region 属性
        //调度实例
        Dispacther Dispacther { get; set; }
        //supersocket服务端
        public SLSocketServer appserver = new SLSocketServer();
        #endregion

        #region 变量
        public static object lockobj = new object();
        #endregion

        #region 方法
        public InsideHandler(Dispacther disPatch)
        {
            Dispacther = disPatch;
            appserver.NewRequestReceived += new RequestHandler<SLSocketSession, StringRequestInfo>(appServer_NewRequestReceived);
            DelegateState.CarChangeEvent += CarChangeEvent;
        }

        public bool Init()
        {
            try
            {
                #region 调度系统客户端通讯
                var serverConfig = new ServerConfig
                {
                    Port = 6000,
                    TextEncoding = "UTF-8",
                    MaxRequestLength = 409600 * 1000
                };
                if (appserver.State == ServerState.NotInitialized)
                {
                    if (!appserver.Setup(serverConfig))
                    {
                        DelegateState.InvokeDispatchStateEvent("初始化操作台通讯失败.....");
                        return false;
                    }
                    else
                    {
                        DelegateState.InvokeDispatchStateEvent("初始化操作台通讯成功.....");
                    }
                }

                if (!appserver.Start())
                {
                    DelegateState.InvokeDispatchStateEvent("启动操作台服务失败.....");
                    return false;
                }
                else
                {
                    DelegateState.InvokeDispatchStateEvent("启动操作台服务成功.....");
                }
                return true;
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
                return false;
            }
        }

        public bool Start()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            try
            {
                appserver.Stop();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        void appServer_NewRequestReceived(SLSocketSession session, StringRequestInfo requestInfo)
        {
            try
            {
                switch (requestInfo.Key)
                {
                    //心跳 最基本的
                    case SocketCommand.CONTEST:
                        {
                            MessagePackage pack = new MessagePackage("", SocketCommand.CONTEST);
                            byte[] data = pack.ToBuffer();
                            session.TrySend(data, 0, data.Length);
                        }
                        break;
                    case SocketCommand.AllCarsInfo:
                        {
                            session.ClienType = TCPClienTypeEnum.Client;
                            MessagePackage pack = new MessagePackage(JosnTool.GetJson(CoreData.CarList), SocketCommand.AllCarsInfo);
                            byte[] data = pack.ToBuffer();
                            session.TrySend(data, 0, data.Length);
                        }
                        break;

                    /////////////////////以下是接受处理平板//////////////////////////
                    case SocketCommand.SynBeeperBtnLayout://平板开启启动程序请求按钮设置信息【赞处理监控类型平板】
                        {
                            Thread T = new Thread(delegate ()
                            {
                                SynAloneBeeperLayout(ref session, requestInfo.Body);
                            });
                            T.IsBackground = true;
                            T.Start();
                        }
                        break;

                    case SocketCommand.SynBeeperBill://平板重连后单独发送其对应的所有的生产订单【平板只要重连后就发送该请求】
                        {
                            Thread T = new Thread(delegate ()
                            {
                                SynAloneBeeperBill(ref session, requestInfo.Body);
                            });
                            T.IsBackground = true;
                            T.Start();
                        }
                        break;
                    case SocketCommand.BeeperCall://接受并处理触摸屏的呼叫/监控请求
                        {
                            Thread T = new Thread(delegate ()
                            {
                                HandleBeeperRequest(ref session, requestInfo.Body);
                            });
                            T.IsBackground = true;
                            T.Start();
                        }
                        break;
                    case SocketCommand.AppLoginCheck:
                        {
                            Thread T = new Thread(delegate ()
                            {
                                CheckAppLog(ref session, requestInfo.Body);
                            });
                            T.IsBackground = true;
                            T.Start();
                        }
                        break;
                    case SocketCommand.ClientSendSynBill:
                        {
                            MessagePackage pack = null;
                            byte[] data = null;
                            try
                            {
                                SynAllBeeperBill();
                                pack = new MessagePackage("下发成功!", SocketCommand.ClientSendSynBill);
                            }
                            catch
                            {
                                pack = new MessagePackage("下发失败!", SocketCommand.ClientSendSynBill);
                            }
                            data = pack.ToBuffer();
                            session.TrySend(data, 0, data.Length);
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 向操作台推送小车位置变化信息
        /// </summary>
        public async void CarChangeEvent(CarEventArgs e)
        {
            try
            {
                //向所有的操作太客户端推送小车的步点变化信息
                await Task.Factory.StartNew(() =>
                {
                    var sessions = appserver.GetAllSessions().Where(p => p.ClienType == TCPClienTypeEnum.Client);
                    if (sessions != null)
                    {
                        MessagePackage pack = new MessagePackage(JosnTool.GetJson(e.Car), SocketCommand.OneCarInfo);
                        byte[] data = pack.ToBuffer();
                        foreach (var session in sessions)
                        { session.TrySend(data, 0, data.Length); }
                    }
                });
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }
        #endregion

        #region 自定义函数
        static object lockResponseCallObj = new object();
        /// <summary>
        /// 处理呼叫器的请求逻辑
        /// BeeperID 呼叫器ID
        /// BeeperSendMes 发送的信息 这里是储位ID
        /// CallType 呼叫类型 用于区别是上料还是下料
        /// BillNO 
        /// </summary>

        private void HandleBeeperRequest(ref SLSocketSession session, string Mes)
        {
            try
            {
                lock (lockResponseCallObj)
                {
                    MessagePackage pack = null;
                    byte[] data = null;
                    LogHelper.WriteCallBoxLog("接受到的平板呼叫请求信息:" + Mes);
                    if (string.IsNullOrEmpty(Mes))
                    {
                        pack = new MessagePackage("0|请求消息格式错误|1", SocketCommand.BeeperCall);
                        data = pack.ToBuffer();
                        session.TrySend(data, 0, data.Length);
                        return;
                    }
                    string[] meses = Mes.Split(',');
                    if (meses == null || (meses != null && meses.Length < 4))
                    {

                        pack = new MessagePackage("0|请求消息格式错误|1", SocketCommand.BeeperCall);
                        data = pack.ToBuffer();
                        session.TrySend(data, 0, data.Length);
                        return;
                    }
                    if (string.IsNullOrEmpty(meses[0].ToString().Trim()) ||
                        string.IsNullOrEmpty(meses[1].ToString().Trim()) ||
                        string.IsNullOrEmpty(meses[2].ToString().Trim()) ||
                        string.IsNullOrEmpty(meses[3].ToString().Trim()))
                    {
                        pack = new MessagePackage("0|请求消息格式错误|1", SocketCommand.BeeperCall);
                        data = pack.ToBuffer();
                        session.TrySend(data, 0, data.Length);
                        return;
                    }
                    int BeeperID = Convert.ToInt16(meses[0]);
                    int BtnID = Convert.ToInt32(meses[1]);
                    string BeeperSendMes = meses[2].ToString();
                    int CallType = Convert.ToInt16(meses[3]);
                    PdaInfo pad = CoreData.AllPads.FirstOrDefault(p => p.PadID == BeeperID);
                    if (pad == null)
                    {
                        LogHelper.WriteCallBoxLog("接受到的平板ID:" + BeeperID.ToString() + "档案不存在!");
                        pack = new MessagePackage("0|平板ID:" + BeeperID.ToString() + "不存在!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                        data = pack.ToBuffer();
                        session.TrySend(data, 0, data.Length);
                        return;
                    }


                    string BillNO = "";
                    int RGBType = -1;
                    int MachineType = -1;
                    if (pad.PadType == 0)
                    {
                        if (meses == null || (meses != null && meses.Length != 6))
                        {
                            pack = new MessagePackage("0|请求消息格式错误|" + BtnID.ToString(), SocketCommand.BeeperCall);
                            data = pack.ToBuffer();
                            session.TrySend(data, 0, data.Length);
                            return;
                        }
                        if (/*string.IsNullOrEmpty(meses[4].ToString().Trim()) ||*/ string.IsNullOrEmpty(meses[5].ToString().Trim()))
                        {
                            pack = new MessagePackage("0|请求消息格式错误|" + BtnID.ToString(), SocketCommand.BeeperCall);
                            data = pack.ToBuffer();
                            session.TrySend(data, 0, data.Length);
                            return;
                        }
                        BillNO = meses[4].ToString();
                        RGBType = Convert.ToInt16(meses[5]);
                    }
                    else
                    {
                        if (meses == null || (meses != null && meses.Length != 5))
                        {
                            pack = new MessagePackage("0|请求消息格式错误|" + BtnID.ToString(), SocketCommand.BeeperCall);
                            data = pack.ToBuffer();
                            session.TrySend(data, 0, data.Length);
                            return;
                        }
                        if (string.IsNullOrEmpty(meses[4].ToString().Trim()))
                        {
                            pack = new MessagePackage("0|请求消息格式错误|" + BtnID.ToString(), SocketCommand.BeeperCall);
                            data = pack.ToBuffer();
                            session.TrySend(data, 0, data.Length);
                            return;
                        }
                        MachineType = Convert.ToInt16(meses[4]);
                    }

                    //根据呼叫类型(上料/下料)按钮来确定呼叫储位
                    StorageInfo CallStore = CoreData.StorageList.FirstOrDefault(q => q.ID.ToString() == BeeperSendMes);
                    if (CallStore == null)
                    {
                        LogHelper.WriteCallBoxLog("接受到的平板ID:" + BeeperID.ToString() + "发送的储位ID:" + BeeperSendMes + "档案不存在!");
                        pack = new MessagePackage("0|储位ID:" + BeeperSendMes + "不存在!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                        data = pack.ToBuffer();
                        session.TrySend(data, 0, data.Length);
                        return;
                    }
                    //先判断是否重复任务产生
                    if (pad.PadType == 0 && ChekAllowCreatTask(pad.PadID, CallStore.LankMarkCode) > 0)
                    {
                        pack = new MessagePackage("0|存在未完成任务|" + BtnID.ToString(), SocketCommand.BeeperCall);
                        data = pack.ToBuffer();
                        session.TrySend(data, 0, data.Length);
                        return;
                    }
                    //再根据接受到的订单号来取对应的物料信息
                    ProduceBillInfo ProduceBill = null;

                    if (string.IsNullOrEmpty(CallStore.LankMarkCode))
                    {
                        LogHelper.WriteCallBoxLog("接受到的平板ID:" + BeeperID.ToString() + "呼叫位置" + CallStore.ID.ToString() + "没对照地标");
                        pack = new MessagePackage("0|当前位置没有对照地标|" + BtnID.ToString(), SocketCommand.BeeperCall);
                        data = pack.ToBuffer();
                        session.TrySend(data, 0, data.Length);
                        return;
                    }

                    if (pad.PadType == 1)//平板为监控类型的
                    {
                        if (CallStore.StorageState == 0)
                        {
                            pack = new MessagePackage("0|当前位置状态不满足!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                            data = pack.ToBuffer();
                            session.TrySend(data, 0, data.Length);
                            return;
                        }
                        //再根据平板所属的区域来判断监控位置的储位状态，有满料，空料车，空位置
                        switch (pad.Area)
                        {
                            case 7://待固红光
                                #region
                                //ProduceBill = QueryProduceBillByBillID(BillNO, RGBType);
                                //if (ProduceBill == null)
                                //{
                                //    LogHelper.WriteCallBoxLog("接受到的平板ID:" + BeeperID.ToString() + "发送的订单号:" + BillNO + "不存在!");
                                //    pack = new MessagePackage("0|订单号:" + BillNO + "不存在!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                //    data = pack.ToBuffer();
                                //    session.TrySend(data, 0, data.Length);
                                //    return;
                                //}
                                UpdateStore(2, MachineType, CallStore.LankMarkCode);
                                CallStore.StorageState = 2;
                                CallStore.HandTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                                CallStore.MaterielType = MachineType;
                                pack = new MessagePackage("1|操作成功!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                data = pack.ToBuffer();
                                session.TrySend(data, 0, data.Length);
                                break;
                            #endregion
                            case 8://烤房
                                #region
                                //烤房包含四个区域[待烤区 待固绿光 待固蓝光 待机检区]
                                //但是要求对应一个触摸屏上，那么只能通过按钮来区别区域了
                                //1，通过平板ID来取其按钮设置信息
                                PdaOperSetInfo padSets = CoreData.PadOperSets.FirstOrDefault(p => p.PdaID == pad.PadID && p.BtnSendValue == BeeperSendMes);
                                if (padSets == null)
                                {
                                    LogHelper.WriteCallBoxLog("接受到的平板ID:" + BeeperID.ToString() + "发送的储位ID:" + BeeperSendMes + "没维护按钮设置信息");
                                    pack = new MessagePackage("0|未设置按钮信息|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                    data = pack.ToBuffer();
                                    session.TrySend(data, 0, data.Length);
                                    return;
                                }
                                //此时通过按钮设置信息来判断具体属于烤房中的哪个区域
                                if (padSets.Area == 0)//待烘烤区，触发后状态更改为1,即空料架
                                {
                                    UpdateStore(1, -1, CallStore.LankMarkCode);
                                    CallStore.StorageState = 1;
                                    CallStore.HandTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                                    CallStore.MaterielType = -1;
                                    pack = new MessagePackage("1|操作成功!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                    data = pack.ToBuffer();
                                    session.TrySend(data, 0, data.Length);
                                }
                                else//烤房内的其他区域,触发后更改状态为2,即满料架
                                {
                                    //ProduceBill = QueryProduceBillByBillID(BillNO, RGBType);
                                    //if (ProduceBill == null)
                                    //{
                                    //    LogHelper.WriteCallBoxLog("接受到的平板ID:" + BeeperID.ToString() + "发送的订单号:" + BillNO + "不存在!");
                                    //    pack = new MessagePackage("0|订单号:" + BillNO + "不存在!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                    //    data = pack.ToBuffer();
                                    //    session.TrySend(data, 0, data.Length);
                                    //    return;
                                    //}
                                    UpdateStore(2, MachineType, CallStore.LankMarkCode);
                                    CallStore.StorageState = 2;
                                    CallStore.HandTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                                    CallStore.MaterielType = MachineType;
                                    pack = new MessagePackage("1|操作成功!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                    data = pack.ToBuffer();
                                    session.TrySend(data, 0, data.Length);
                                }
                                break;
                            #endregion
                            case 9://待焊线区
                                #region
                                //ProduceBill = QueryProduceBillByBillID(BillNO, RGBType);
                                //if (ProduceBill == null)
                                //{
                                //    LogHelper.WriteCallBoxLog("接受到的平板ID:" + BeeperID.ToString() + "发送的订单号:" + BillNO + "不存在!");
                                //    pack = new MessagePackage("0|订单号:" + BillNO + "不存在!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                //    data = pack.ToBuffer();
                                //    session.TrySend(data, 0, data.Length);
                                //    return;
                                //}
                                UpdateStore(2, MachineType, CallStore.LankMarkCode);
                                CallStore.StorageState = 2;
                                CallStore.HandTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                                CallStore.MaterielType = MachineType;
                                pack = new MessagePackage("1|操作成功!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                data = pack.ToBuffer();
                                session.TrySend(data, 0, data.Length);
                                #endregion
                                break;
                            case 5://点胶
                                #region
                                UpdateStore(1, -1, CallStore.LankMarkCode);
                                CallStore.StorageState = 1;
                                CallStore.HandTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                                CallStore.MaterielType = -1;
                                pack = new MessagePackage("1|操作成功!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                data = pack.ToBuffer();
                                session.TrySend(data, 0, data.Length);
                                break;
                            #endregion
                            case 12://空料盒区
                                #region
                                UpdateStore(2, -1, CallStore.LankMarkCode);
                                CallStore.StorageState = 2;
                                CallStore.HandTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                                CallStore.MaterielType = -1;
                                pack = new MessagePackage("1|操作成功!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                data = pack.ToBuffer();
                                session.TrySend(data, 0, data.Length);
                                break;
                            #endregion
                            default:
                                #region
                                LogHelper.WriteCallBoxLog("接受到的平板ID:" + BeeperID.ToString() + "不属于监控区域");
                                pack = new MessagePackage("0|当前平板不属于监控区域|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                data = pack.ToBuffer();
                                session.TrySend(data, 0, data.Length);
                                break;
                                #endregion
                        }//处理监控逻辑完成
                    }//end监控处理


                    else if (pad.PadType == 0)//平板是呼叫类型的
                    {
                        //存放一个任务的分解目标地标加到达购销动作,0放，1取
                        //item为字符串格式，目的地标,动作类型[0放1取],所放物料类型[0空料架1满料架]
                        List<string> ArmLands = new List<string>();
                        string TaskSQL = "";
                        string dispatchNo = Guid.NewGuid().ToString();
                        switch (pad.Area)
                        {
                            //如果平板属于固晶区,当前呼叫需要根据排产订单来决定是否红光还是绿光还是蓝光
                            #region 固晶区
                            case 0:
                                if (CallType == 0)//呼叫
                                {
                                    ScheduingInfo ScheduInfo = QuerySchudingInfoByBillNo(BillNO, BeeperID, RGBType);
                                    if (ScheduInfo == null)
                                    {
                                        LogHelper.WriteCallBoxLog("接受到的平板ID:" + BeeperID.ToString() + "发送的订单号:" + BillNO + "无排产信息!");
                                        pack = new MessagePackage("0|订单号:" + BillNO + "无排产信息!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                        return;
                                    }
                                    int Area = ScheduInfo.RGBType;
                                    if (Area == 0)//红光固晶
                                    {
                                        #region 红光上料
                                        ProduceBill = QueryProduceBillByBillID(BillNO, RGBType);
                                        if (ProduceBill == null)
                                        {
                                            LogHelper.WriteCallBoxLog("接受到的平板ID:" + BeeperID.ToString() + "发送的订单号:" + BillNO + "不存在!");
                                            pack = new MessagePackage("0|订单号:" + BillNO + "不存在!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                            data = pack.ToBuffer();
                                            session.TrySend(data, 0, data.Length);
                                            return;
                                        }
                                        //先去呼叫地标拿空料车
                                        ArmLands.Add(CallStore.LankMarkCode + ",1,0");
                                        //查找可以放空料架的地标
                                        // 1 通过预热区域找对照的平板
                                        PdaInfo Warm_UpPad = CoreData.AllPads.FirstOrDefault(p => p.Area == 7);
                                        if (Warm_UpPad == null)
                                        {
                                            LogHelper.WriteCreatTaskLog("接受到的平板ID:" + BeeperID.ToString() + "呼叫固红上料【没找到待固红平板】");
                                            pack = new MessagePackage("0|未找到待固红光区域|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                            data = pack.ToBuffer();
                                            session.TrySend(data, 0, data.Length);
                                            return;
                                        }
                                        //根据预热区的呼叫盒找对照地标明细
                                        List<PdaOperSetInfo> Warm_UpPad_Toland = CoreData.PadOperSets.Where(p => p.PdaID == Warm_UpPad.PadID).ToList();
                                        if (Warm_UpPad_Toland.Count <= 0)
                                        {
                                            LogHelper.WriteCreatTaskLog("待固红没有设置按钮信息");
                                            pack = new MessagePackage("0|待固红光平板未设置按钮信息|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                            data = pack.ToBuffer();
                                            session.TrySend(data, 0, data.Length);
                                            return;
                                        }
                                        //再找可以放空料架的位置地标
                                        StorageInfo NoneStorge = (from a in CoreData.StorageList
                                                                  where a.LockState == 0 && a.StorageState == 0 &&
                                                                  Warm_UpPad_Toland.Where(p => p.BtnSendValue == a.ID.ToString()).Count() > 0
                                                                  select a).FirstOrDefault();
                                        if (NoneStorge == null || (NoneStorge != null && string.IsNullOrEmpty(NoneStorge.LankMarkCode)))
                                        {
                                            LogHelper.WriteCreatTaskLog("待固红没有空位置");
                                            pack = new MessagePackage("0|待固红光没有空位置|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                            data = pack.ToBuffer();
                                            session.TrySend(data, 0, data.Length);
                                            return;
                                        }
                                        ArmLands.Add(NoneStorge.LankMarkCode + ",0,0");
                                        //再找满料位置地标
                                        StorageInfo FillStorge = (from a in CoreData.StorageList
                                                                  where a.LockState == 0 && a.StorageState == 2 &&
                                                                  a.MaterielType == ProduceBill.MachineType && Warm_UpPad_Toland.Where(p => p.BtnSendValue == a.ID.ToString()).Count() > 0
                                                                  select a).FirstOrDefault();
                                        if (FillStorge == null || (FillStorge != null && string.IsNullOrEmpty(FillStorge.LankMarkCode)))
                                        {
                                            LogHelper.WriteCreatTaskLog("待固红没有满料位置");
                                            pack = new MessagePackage("0|待固红光没有满料位置|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                            data = pack.ToBuffer();
                                            session.TrySend(data, 0, data.Length);
                                            return;
                                        }
                                        //找到满料架的位置地标
                                        ArmLands.Add(FillStorge.LankMarkCode + ",1,0");
                                        //再到呼叫点去放满料
                                        ArmLands.Add(CallStore.LankMarkCode + ",0,1");
                                        if (ChekAllowCreatTask(pad.PadID, CallStore.LankMarkCode) <= 0)
                                        {
                                            //持久化任务
                                            TaskSQL = "if not exists(select 1 from tbDispatchTaskInfo where stationNo=" + pad.PadID.ToString() + " and CallLand='" + CallStore.LankMarkCode + "' and TaskState in(0,1)) \r\n" +
                                                            " begin \r\n" +
                                                            "insert into tbDispatchTaskInfo( \r\n" +
                                                           "dispatchNo,stationNo,taskType,BuildTime,CallLand) values ( \r\n" +
                                                           "'" + dispatchNo + "'," + pad.PadID.ToString() + ",0,'" + DateTime.Now.ToString("yyyy/MM/dd/HH/mm/ss") + "','" + CallStore.LankMarkCode + "') \r\n";

                                            int TaskDetailID = 1;
                                            foreach (string item in ArmLands)
                                            {
                                                string TempLand = item.Split(',')[0];
                                                string OperType = item.Split(',')[1];
                                                string PutType = item.Split(',')[2];
                                                TaskSQL += "insert into tbDispatchTaskDetail ( \r\n" +
                                               "dispatchNo,DetailID,LandCode,OperType,PutType,IsAllowExcute) values ( \r\n" +
                                               "'" + dispatchNo + "'," + TaskDetailID.ToString() + ",'" + TempLand + "'," + OperType + "," + PutType + "," +
                                               "1" + ") \r\n";
                                                TaskSQL += " UPDATE  tbLocaton \r\n" +
                                                "SET LockState =2 \r\n" +
                                                "where LankMarkCode = '" + TempLand + "'\r\n";
                                                TaskDetailID++;
                                            }
                                            TaskSQL = TaskSQL + " end \r\n";
                                            Hashtable hs = new Hashtable();
                                            hs["sql"] = TaskSQL;
                                            CoreData.DbHelper.ExecuteSql("0029", hs);
                                            foreach (string item in ArmLands)
                                            {
                                                string TempLand = item.Split(',')[0];
                                                StorageInfo store = CoreData.StorageList.FirstOrDefault(p => p.LankMarkCode == TempLand);
                                                if (store != null) { store.LockState = 2; }
                                            }
                                            pack = new MessagePackage("1|操作成功!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                            data = pack.ToBuffer();
                                            session.TrySend(data, 0, data.Length);
                                        }
                                        else
                                        {
                                            LogHelper.WriteCreatTaskLog("存在未完成任务");
                                            pack = new MessagePackage("0|存在未完成任务|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                            data = pack.ToBuffer();
                                            session.TrySend(data, 0, data.Length);
                                        }
                                        #endregion
                                    }
                                    else if (Area == 1)
                                    {
                                        #region 绿光上料
                                        ProduceBill = QueryProduceBillByBillID(BillNO, RGBType);
                                        if (ProduceBill == null)
                                        {
                                            LogHelper.WriteCallBoxLog("接受到的平板ID:" + BeeperID.ToString() + "发送的订单号:" + BillNO + "不存在!");
                                            pack = new MessagePackage("0|订单号:" + BillNO + "不存在!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                            data = pack.ToBuffer();
                                            session.TrySend(data, 0, data.Length);
                                            return;
                                        }
                                        //先去呼叫地标拿空料车
                                        ArmLands.Add(CallStore.LankMarkCode + ",1,0");
                                        //查找可以放空料架的地标
                                        // 1 通过烤房待固绿光区域找对照的平板
                                        PdaInfo HandBakeRedPad = CoreData.AllPads.FirstOrDefault(p => p.Area == 8);
                                        if (HandBakeRedPad == null)
                                        {
                                            LogHelper.WriteCreatTaskLog("接受到的平板ID:" + BeeperID.ToString() + "呼叫固绿上料【没找到待固绿光平板】");
                                            pack = new MessagePackage("0|未找到待固绿光区域|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                            data = pack.ToBuffer();
                                            session.TrySend(data, 0, data.Length);
                                            return;
                                        }
                                        //根据烤房待固绿光的平板找对照的位置地标
                                        List<PdaOperSetInfo> HandBakeRedPad_Toland = CoreData.PadOperSets.Where(p => p.PdaID == HandBakeRedPad.PadID && p.Area == 1).ToList();
                                        if (HandBakeRedPad_Toland.Count <= 0)
                                        {
                                            LogHelper.WriteCreatTaskLog("待固绿没有设置按钮信息");
                                            pack = new MessagePackage("0|待固绿光平板未设置按钮信息|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                            data = pack.ToBuffer();
                                            session.TrySend(data, 0, data.Length);
                                            return;
                                        }
                                        //再找可以放空料架的位置地标
                                        StorageInfo NoneStorge = (from a in CoreData.StorageList
                                                                  where a.LockState == 0 && a.StorageState == 0 &&
                                                                  HandBakeRedPad_Toland.Where(p => p.BtnSendValue == a.ID.ToString()).Count() > 0
                                                                  select a).FirstOrDefault();
                                        if (NoneStorge == null || (NoneStorge != null && string.IsNullOrEmpty(NoneStorge.LankMarkCode)))
                                        {
                                            LogHelper.WriteCreatTaskLog("待固绿没有空位置");
                                            pack = new MessagePackage("0|待固绿光没有空位置|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                            data = pack.ToBuffer();
                                            session.TrySend(data, 0, data.Length);
                                            return;
                                        }
                                        ArmLands.Add(NoneStorge.LankMarkCode + ",0,0");
                                        //再找满料位置地标
                                        StorageInfo FillStorge = (from a in CoreData.StorageList
                                                                  where a.LockState == 0 && a.StorageState == 2 &&
                                                                  a.MaterielType == ProduceBill.MachineType && HandBakeRedPad_Toland.Where(p => p.BtnSendValue == a.ID.ToString()).Count() > 0
                                                                  select a).FirstOrDefault();
                                        if (FillStorge == null || (FillStorge != null && string.IsNullOrEmpty(FillStorge.LankMarkCode)))
                                        {
                                            LogHelper.WriteCreatTaskLog("待固绿没有满料位置");
                                            pack = new MessagePackage("0|待固绿光没有满料位置|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                            data = pack.ToBuffer();
                                            session.TrySend(data, 0, data.Length);
                                            return;
                                        }
                                        ArmLands.Add(FillStorge.LankMarkCode + ",1,0");
                                        //再到呼叫点去放满料
                                        ArmLands.Add(CallStore.LankMarkCode + ",0,1");
                                        if (ChekAllowCreatTask(pad.PadID, CallStore.LankMarkCode) <= 0)
                                        {
                                            TaskSQL = "if not exists(select 1 from tbDispatchTaskInfo where stationNo=" + pad.PadID.ToString() + " and CallLand='" + CallStore.LankMarkCode + "' and TaskState in(0,1)) \r\n" +
                                                       " begin \r\n" +
                                                       " insert into tbDispatchTaskInfo( \r\n" +
                                                      "dispatchNo,stationNo,taskType,BuildTime,CallLand) values ( \r\n" +
                                                      "'" + dispatchNo + "'," + pad.PadID.ToString() + ",0,'" + DateTime.Now.ToString("yyyy/MM/dd/HH/mm/ss") + "','" + CallStore.LankMarkCode + "') \r\n";

                                            int TaskDetailID = 1;
                                            foreach (string item in ArmLands)
                                            {
                                                string TempLand = item.Split(',')[0];
                                                string OperType = item.Split(',')[1];
                                                string PutType = item.Split(',')[2];
                                                TaskSQL += "insert into tbDispatchTaskDetail ( \r\n" +
                                               "dispatchNo,DetailID,LandCode,OperType,PutType,IsAllowExcute) values ( \r\n" +
                                               "'" + dispatchNo + "'," + TaskDetailID.ToString() + ",'" + TempLand + "'," + OperType + "," + PutType + "," +
                                               "1"/*(TempLand == ArmLand4 ? "0" : "1")*/ + ") \r\n";
                                                TaskSQL += " UPDATE  tbLocaton \r\n" +
                                              "SET LockState =2 \r\n" +
                                              "where LankMarkCode = '" + TempLand + "'\r\n";
                                                TaskDetailID++;
                                            }

                                            TaskSQL = TaskSQL + " end \r\n";
                                            Hashtable hs = new Hashtable();
                                            hs["sql"] = TaskSQL;
                                            CoreData.DbHelper.ExecuteSql("0029", hs);
                                            foreach (string item in ArmLands)
                                            {
                                                string TempLand = item.Split(',')[0];
                                                StorageInfo store = CoreData.StorageList.FirstOrDefault(p => p.LankMarkCode == TempLand);
                                                if (store != null) { store.LockState = 2; }
                                            }
                                            pack = new MessagePackage("1|操作成功!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                            data = pack.ToBuffer();
                                            session.TrySend(data, 0, data.Length);
                                        }
                                        else
                                        {
                                            LogHelper.WriteCreatTaskLog("存在未完成任务");
                                            pack = new MessagePackage("0|存在未完成任务|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                            data = pack.ToBuffer();
                                            session.TrySend(data, 0, data.Length);
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region 蓝光上料
                                        ProduceBill = QueryProduceBillByBillID(BillNO, RGBType);
                                        if (ProduceBill == null)
                                        {
                                            LogHelper.WriteCallBoxLog("接受到的平板ID:" + BeeperID.ToString() + "发送的订单号:" + BillNO + "不存在!");
                                            pack = new MessagePackage("0|订单号:" + BillNO + "不存在!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                            data = pack.ToBuffer();
                                            session.TrySend(data, 0, data.Length);
                                            return;
                                        }
                                        //先去呼叫地标拿空料车
                                        ArmLands.Add(CallStore.LankMarkCode + ",1,0");
                                        //查找可以放空料架的地标
                                        // 1 通过绿光已烘烤区找对照的平板
                                        PdaInfo BakeGreenBox = CoreData.AllPads.FirstOrDefault(p => p.Area == 8);
                                        if (BakeGreenBox == null)
                                        {
                                            LogHelper.WriteCreatTaskLog("接受到的平板ID:" + BeeperID.ToString() + "呼叫固蓝上料【没找到待固蓝平板】");
                                            pack = new MessagePackage("0|未找到待固蓝光区域|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                            data = pack.ToBuffer();
                                            session.TrySend(data, 0, data.Length);
                                            return;
                                        }
                                        //根据绿光已烘烤区的呼叫盒找对照地标明细
                                        List<PdaOperSetInfo> BakeGreenBox_Toland = CoreData.PadOperSets.Where(p => p.PdaID == BakeGreenBox.PadID && p.Area == 2).ToList();
                                        if (BakeGreenBox_Toland.Count <= 0)
                                        {
                                            LogHelper.WriteCreatTaskLog("待固蓝平板没有设置按钮信息");
                                            pack = new MessagePackage("0|待固蓝光平板未设置按钮信息|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                            data = pack.ToBuffer();
                                            session.TrySend(data, 0, data.Length);
                                            return;
                                        }
                                        //再找可以放空料架的位置地标
                                        StorageInfo NoneStorge = (from a in CoreData.StorageList
                                                                  where a.LockState == 0 && a.StorageState == 0 &&
                                                                  BakeGreenBox_Toland.Where(p => p.BtnSendValue == a.ID.ToString()).Count() > 0
                                                                  select a).FirstOrDefault();

                                        if (NoneStorge == null || (NoneStorge != null && string.IsNullOrEmpty(NoneStorge.LankMarkCode)))
                                        {
                                            LogHelper.WriteCreatTaskLog("待固蓝区没有空位置");
                                            pack = new MessagePackage("0|待固蓝光区无空位置|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                            data = pack.ToBuffer();
                                            session.TrySend(data, 0, data.Length);
                                            return;
                                        }
                                        //找到可以放空料架的位置地标
                                        ArmLands.Add(NoneStorge.LankMarkCode + ",0,0");
                                        //再找满料位置地标
                                        StorageInfo FillStorge = (from a in CoreData.StorageList
                                                                  where a.LockState == 0 && a.StorageState == 2 &&
                                                                  a.MaterielType == ProduceBill.MachineType && BakeGreenBox_Toland.Where(p => p.BtnSendValue == a.ID.ToString()).Count() > 0
                                                                  select a).FirstOrDefault();

                                        if (FillStorge == null || (FillStorge != null && string.IsNullOrEmpty(FillStorge.LankMarkCode)))
                                        {
                                            LogHelper.WriteCreatTaskLog("待固蓝区没有满料架");
                                            pack = new MessagePackage("0|待固蓝光区无满料架|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                            data = pack.ToBuffer();
                                            session.TrySend(data, 0, data.Length);
                                            return;
                                        }
                                        //找到满料架的位置地标
                                        ArmLands.Add(FillStorge.LankMarkCode + ",1,0");
                                        //再到呼叫点去放满料架
                                        ArmLands.Add(CallStore.LankMarkCode + ",0,1");
                                        //持久化任务
                                        if (ChekAllowCreatTask(pad.PadID, CallStore.LankMarkCode) <= 0)
                                        {
                                            TaskSQL = "if not exists(select 1 from tbDispatchTaskInfo where stationNo=" + pad.PadID.ToString() + " and CallLand='" + CallStore.LankMarkCode + "' and TaskState in(0,1)) \r\n" +
                                                         " begin \r\n" +
                                                         " insert into tbDispatchTaskInfo( \r\n" +
                                                        "dispatchNo,stationNo,taskType,BuildTime,CallLand) values ( \r\n" +
                                                        "'" + dispatchNo + "'," + pad.PadID.ToString() + ",0,'" + DateTime.Now.ToString("yyyy/MM/dd/HH/mm/ss") + "','" + CallStore.LankMarkCode + "') \r\n";

                                            int TaskDetailID = 1;
                                            foreach (string item in ArmLands)
                                            {
                                                string TempLand = item.Split(',')[0];
                                                string OperType = item.Split(',')[1];
                                                string PutType = item.Split(',')[2];
                                                TaskSQL += "insert into tbDispatchTaskDetail ( \r\n" +
                                               "dispatchNo,DetailID,LandCode,OperType,PutType,IsAllowExcute) values ( \r\n" +
                                               "'" + dispatchNo + "'," + TaskDetailID.ToString() + ",'" + TempLand + "'," + OperType + "," + PutType + "," +
                                               "1"/*(TempLand == ArmLand4 ? "0" : "1")*/ + ") \r\n";
                                                TaskSQL += " UPDATE  tbLocaton \r\n" +
                                                "SET LockState =2 \r\n" +
                                                "where LankMarkCode = '" + TempLand + "'\r\n";
                                                TaskDetailID++;
                                            }
                                            TaskSQL = TaskSQL + " end \r\n";
                                            Hashtable hs = new Hashtable();
                                            hs["sql"] = TaskSQL;
                                            CoreData.DbHelper.ExecuteSql("0029", hs);
                                            foreach (string item in ArmLands)
                                            {
                                                string TempLand = item.Split(',')[0];
                                                StorageInfo store = CoreData.StorageList.FirstOrDefault(p => p.LankMarkCode == TempLand);
                                                if (store != null) { store.LockState = 2; }
                                            }
                                            pack = new MessagePackage("1|操作成功!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                            data = pack.ToBuffer();
                                            session.TrySend(data, 0, data.Length);
                                        }
                                        else
                                        {
                                            LogHelper.WriteCreatTaskLog("存在未完成任务");
                                            pack = new MessagePackage("0|存在未完成任务|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                            data = pack.ToBuffer();
                                            session.TrySend(data, 0, data.Length);
                                        }
                                        #endregion
                                    }
                                }
                                else//下料
                                {
                                    #region 固晶下料
                                    //先添加目的地标,去拉满料
                                    ArmLands.Add(CallStore.LankMarkCode + ",1,1");
                                    //找满料放置的位置地标
                                    //1 找待烤区域的平板
                                    PdaInfo WaitBakeBox = CoreData.AllPads.FirstOrDefault(p => p.Area == 8);
                                    if (WaitBakeBox == null)
                                    {
                                        LogHelper.WriteCreatTaskLog("接受到的平板ID:" + BeeperID.ToString() + "呼叫固红下料【没找到待烤平板】");
                                        pack = new MessagePackage("0|未找到待烤区域|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                        return;
                                    }
                                    //2 再根据待烤平板来找对照的位置地标
                                    List<PdaOperSetInfo> WaitBakeBoxToland = CoreData.PadOperSets.Where(p => p.PdaID == WaitBakeBox.PadID && p.Area == 0).ToList();
                                    if (WaitBakeBoxToland.Count <= 0)
                                    {
                                        LogHelper.WriteCreatTaskLog("待烤区平板没有设置按钮信息");
                                        pack = new MessagePackage("0|待烤平板未设置按钮信息|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                        return;
                                    }
                                    //3 在其中再找空位置地标,送烤房不需要考虑物料类型
                                    StorageInfo NoneStorge = (from a in CoreData.StorageList
                                                              where a.LockState == 0 && a.StorageState == 0 &&
                                                              WaitBakeBoxToland.Where(p => p.BtnSendValue == a.ID.ToString()).Count() > 0
                                                              select a).FirstOrDefault();
                                    if (NoneStorge == null || (NoneStorge != null && string.IsNullOrEmpty(NoneStorge.LankMarkCode)))
                                    {
                                        LogHelper.WriteCreatTaskLog("待烤区没有空位置");
                                        pack = new MessagePackage("0|待烤区没有空位置|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                        return;
                                    }
                                    //4 找到可以放待烤物料的空位置地标
                                    ArmLands.Add(NoneStorge.LankMarkCode + ",0,1");
                                    //5 再找可带回的空料架
                                    StorageInfo FillStorge = (from a in CoreData.StorageList
                                                              where a.LockState == 0 && a.StorageState == 1 &&
                                                              WaitBakeBoxToland.Where(p => p.BtnSendValue == a.ID.ToString()).Count() > 0
                                                              select a).FirstOrDefault();
                                    if (FillStorge == null || (FillStorge != null && string.IsNullOrEmpty(FillStorge.LankMarkCode)))
                                    {
                                        LogHelper.WriteCreatTaskLog("待烤区没有空料架");
                                        pack = new MessagePackage("0|待烤区没有空料架|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                        return;
                                    }
                                    ArmLands.Add(FillStorge.LankMarkCode + ",1,0");
                                    //6 最后到叫料位置地标放空料车
                                    ArmLands.Add(CallStore.LankMarkCode + ",0,0");
                                    //持久化任务
                                    if (ChekAllowCreatTask(pad.PadID, CallStore.LankMarkCode) <= 0)
                                    {
                                        TaskSQL = "if not exists(select 1 from tbDispatchTaskInfo where stationNo=" + pad.PadID.ToString() + " and CallLand='" + CallStore.LankMarkCode + "' and TaskState in(0,1)) \r\n" +
                                                     " begin \r\n" +
                                                     " insert into tbDispatchTaskInfo( \r\n" +
                                                    "dispatchNo,stationNo,taskType,BuildTime,CallLand) values ( \r\n" +
                                                    "'" + dispatchNo + "'," + pad.PadID.ToString() + ",1,'" + DateTime.Now.ToString("yyyy/MM/dd/HH/mm/ss") + "','" + CallStore.LankMarkCode + "') \r\n";

                                        int TaskDetailID = 1;
                                        foreach (string item in ArmLands)
                                        {
                                            string TempLand = item.Split(',')[0];
                                            string OperType = item.Split(',')[1];
                                            string PutType = item.Split(',')[2];
                                            TaskSQL += "insert into tbDispatchTaskDetail ( \r\n" +
                                           "dispatchNo,DetailID,LandCode,OperType,PutType,IsAllowExcute) values ( \r\n" +
                                           "'" + dispatchNo + "'," + TaskDetailID.ToString() + ",'" + TempLand + "'," + OperType + "," + PutType + "," +
                                           "1" + ") \r\n";
                                            TaskSQL += " UPDATE  tbLocaton \r\n" +
                                           "SET LockState =2 \r\n" +
                                            "where LankMarkCode = '" + TempLand + "'\r\n";
                                            TaskDetailID++;
                                        }
                                        TaskSQL = TaskSQL + " end \r\n";
                                        Hashtable hs = new Hashtable();
                                        hs["sql"] = TaskSQL;
                                        CoreData.DbHelper.ExecuteSql("0029", hs);
                                        foreach (string item in ArmLands)
                                        {
                                            string TempLand = item.Split(',')[0];
                                            StorageInfo store = CoreData.StorageList.FirstOrDefault(p => p.LankMarkCode == TempLand);
                                            if (store != null) { store.LockState = 2; }
                                        }
                                        pack = new MessagePackage("1|操作成功!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                    }
                                    else
                                    {
                                        LogHelper.WriteCreatTaskLog("存在未完成任务");
                                        pack = new MessagePackage("0|存在未完成任务|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                    }
                                    #endregion
                                }


                                #region
                                //if (Area == 0)//红光固晶
                                //{
                                //    if (CallType == 0)
                                //    {

                                //    }
                                //    else
                                //    {

                                //    }
                                //}
                                //else if (Area == 1)//绿光固晶
                                //{
                                //    if (CallType == 0)
                                //    {

                                //    }
                                //    else
                                //    {
                                //        #region 绿光下料
                                //        //先添加目的地标,去拉满料
                                //        ArmLands.Add(CallStore.LankMarkCode + ",1,0");
                                //        //找满料放置的位置地标
                                //        //1 找待烤区域的按钮盒           
                                //        PdaInfo WaitBakeBox = CoreData.AllPads.FirstOrDefault(p => p.Area == 8);
                                //        if (WaitBakeBox == null)
                                //        {
                                //            LogHelper.WriteCreatTaskLog("接受到的平板ID:" + BeeperID.ToString() + "呼叫固绿下料【没找到待烤平板】");
                                //            pack = new MessagePackage("0|未找到待烤区域|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                //            data = pack.ToBuffer();
                                //            session.TrySend(data, 0, data.Length);
                                //            return;
                                //        }
                                //        //2 再根据待烤平板来找对照的位置地标
                                //        List<PdaOperSetInfo> WaitBakeBoxToland = CoreData.PadOperSets.Where(p => p.PdaID == WaitBakeBox.PadID && p.Area == 0).ToList();
                                //        if (WaitBakeBoxToland.Count <= 0)
                                //        {
                                //            LogHelper.WriteCreatTaskLog("待烤区平板没有设置按钮信息");
                                //            pack = new MessagePackage("0|待烤平板未设置按钮信息|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                //            data = pack.ToBuffer();
                                //            session.TrySend(data, 0, data.Length);
                                //            return;
                                //        }
                                //        //3 在其中再找空位置地标,送烤房不需要考虑物料类型
                                //        StorageInfo NoneStorge = (from a in CoreData.StorageList
                                //                                  where a.LockState == 0 && a.StorageState == 0 &&
                                //                                  WaitBakeBoxToland.Where(p => p.BtnSendValue == a.ID.ToString()).Count() > 0
                                //                                  select a).FirstOrDefault();

                                //        if (NoneStorge == null || (NoneStorge != null && string.IsNullOrEmpty(NoneStorge.LankMarkCode)))
                                //        {
                                //            LogHelper.WriteCreatTaskLog("待烤区没有空位置");
                                //            pack = new MessagePackage("0|待烤区没有空位置|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                //            data = pack.ToBuffer();
                                //            session.TrySend(data, 0, data.Length);
                                //            return;
                                //        }
                                //        //4 找到可以放待烤物料的空位置地标
                                //        ArmLands.Add(NoneStorge.LankMarkCode + ",0,1");
                                //        //5 再找可带回的空料架
                                //        StorageInfo FillStorge = (from a in CoreData.StorageList
                                //                                  where a.LockState == 0 && a.StorageState == 1 &&
                                //                                  WaitBakeBoxToland.Where(p => p.BtnSendValue == a.ID.ToString()).Count() > 0
                                //                                  select a).FirstOrDefault();
                                //        if (FillStorge == null || (FillStorge != null && string.IsNullOrEmpty(FillStorge.LankMarkCode)))
                                //        {
                                //            LogHelper.WriteCreatTaskLog("待烤区没有空料架");
                                //            pack = new MessagePackage("0|待烤区没有空料架|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                //            data = pack.ToBuffer();
                                //            session.TrySend(data, 0, data.Length);
                                //            return;
                                //        }
                                //        //6 找到可以带回的空料架位置地标
                                //        ArmLands.Add(FillStorge.LankMarkCode + ",1,0");
                                //        //7 最后到叫料位置地标放空料架
                                //        ArmLands.Add(CallStore.LankMarkCode + ",0,0");
                                //        //持久化任务
                                //        if (ChekAllowCreatTask(pad.PadID, CallStore.LankMarkCode) <= 0)
                                //        {
                                //            TaskSQL = "if not exists(select 1 from tbDispatchTaskInfo where stationNo=" + pad.PadID.ToString() + " and CallLand='" + CallStore.LankMarkCode + "' and TaskState in(0,1)) \r\n" +
                                //                       " begin \r\n" +
                                //                       " insert into tbDispatchTaskInfo( \r\n" +
                                //                      "dispatchNo,stationNo,taskType,BuildTime,CallLand) values ( \r\n" +
                                //                      "'" + dispatchNo + "'," + pad.PadID.ToString() + ",1,'" + DateTime.Now.ToString("yyyy/MM/dd/HH/mm/ss") + "','" + CallStore.LankMarkCode + "') \r\n";

                                //            int TaskDetailID = 1;
                                //            foreach (string item in ArmLands)
                                //            {
                                //                string TempLand = item.Split(',')[0];
                                //                string OperType = item.Split(',')[1];
                                //                string PutType = item.Split(',')[2];
                                //                TaskSQL += "insert into tbDispatchTaskDetail ( \r\n" +
                                //               "dispatchNo,DetailID,LandCode,OperType,PutType,IsAllowExcute) values ( \r\n" +
                                //               "'" + dispatchNo + "'," + TaskDetailID.ToString() + ",'" + TempLand + "'," + OperType + "," + PutType + "," +
                                //              "1" + ") \r\n";
                                //                TaskSQL += " UPDATE  tbLocaton \r\n" +
                                //               "SET LockState =2 \r\n" +
                                //               "where LankMarkCode = '" + TempLand + "'\r\n";
                                //                TaskDetailID++;
                                //            }

                                //            TaskSQL = TaskSQL + " end \r\n";
                                //            Hashtable hs = new Hashtable();
                                //            hs["sql"] = TaskSQL;
                                //            CoreData.DbHelper.ExecuteSql("0029", hs);
                                //            foreach (string item in ArmLands)
                                //            {
                                //                string TempLand = item.Split(',')[0];
                                //                StorageInfo store = CoreData.StorageList.FirstOrDefault(p => p.LankMarkCode == TempLand);
                                //                if (store != null) { store.LockState = 2; }
                                //            }
                                //            pack = new MessagePackage("1|操作成功!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                //            data = pack.ToBuffer();
                                //            session.TrySend(data, 0, data.Length);
                                //        }
                                //        else
                                //        {
                                //            LogHelper.WriteCreatTaskLog("存在未完成任务");
                                //            pack = new MessagePackage("0|存在未完成任务|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                //            data = pack.ToBuffer();
                                //            session.TrySend(data, 0, data.Length);
                                //        }

                                //        #endregion
                                //    }
                                //}
                                //else//蓝光固晶
                                //{
                                //    if (CallType == 0)
                                //    {

                                //    }
                                //    else
                                //    {
                                //        #region 蓝光下料
                                //        //先添加目的地标,去拉满料
                                //        ArmLands.Add(CallStore.LankMarkCode + ",1,0");
                                //        //找满料放置的位置地标
                                //        //1 找待烤区域的按钮盒
                                //        PdaInfo WaitBakeBox = CoreData.AllPads.FirstOrDefault(p => p.Area == 8);
                                //        if (WaitBakeBox == null)
                                //        {
                                //            LogHelper.WriteCreatTaskLog("接受到的平板ID:" + BeeperID.ToString() + "呼叫固蓝下料【没找到待烤平板】");
                                //            pack = new MessagePackage("0|未找到待烤区域|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                //            data = pack.ToBuffer();
                                //            session.TrySend(data, 0, data.Length);
                                //            return;
                                //        }
                                //        //2 再根据待烤平板来找对照的位置地标
                                //        List<PdaOperSetInfo> WaitBakeBoxToland = CoreData.PadOperSets.Where(p => p.PdaID == WaitBakeBox.PadID && p.Area == 0).ToList();
                                //        if (WaitBakeBoxToland.Count <= 0)
                                //        {
                                //            LogHelper.WriteCreatTaskLog("待烤区平板没有设置按钮信息");
                                //            pack = new MessagePackage("0|待烤平板未设置按钮信息|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                //            data = pack.ToBuffer();
                                //            session.TrySend(data, 0, data.Length);
                                //            return;
                                //        }
                                //        //3 在其中再找空位置地标,送烤房不需要考虑物料类型
                                //        StorageInfo NoneStorge = (from a in CoreData.StorageList
                                //                                  where a.LockState == 0 && a.StorageState == 0 &&
                                //                                  WaitBakeBoxToland.Where(p => p.BtnSendValue == a.ID.ToString()).Count() > 0
                                //                                  select a).FirstOrDefault();

                                //        if (NoneStorge == null || (NoneStorge != null && string.IsNullOrEmpty(NoneStorge.LankMarkCode)))
                                //        {
                                //            LogHelper.WriteCreatTaskLog("待烤区没有空位置");
                                //            pack = new MessagePackage("0|待烤区没有空位置|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                //            data = pack.ToBuffer();
                                //            session.TrySend(data, 0, data.Length);
                                //            return;
                                //        }
                                //        ArmLands.Add(NoneStorge.LankMarkCode + ",0,1");
                                //        //5 再找可带回的空料架
                                //        StorageInfo FillStorge = (from a in CoreData.StorageList
                                //                                  where a.LockState == 0 && a.StorageState == 1 &&
                                //                                  WaitBakeBoxToland.Where(p => p.BtnSendValue == a.ID.ToString()).Count() > 0
                                //                                  select a).FirstOrDefault();
                                //        if (FillStorge == null || (FillStorge != null && string.IsNullOrEmpty(FillStorge.LankMarkCode)))
                                //        {
                                //            LogHelper.WriteCreatTaskLog("待烤区没有空料架");
                                //            pack = new MessagePackage("0|待烤区没有空料架|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                //            data = pack.ToBuffer();
                                //            session.TrySend(data, 0, data.Length);
                                //            return;
                                //        }
                                //        //6 找到可以带回的空料架位置地标
                                //        ArmLands.Add(FillStorge.LankMarkCode + ",1,0");
                                //        //7 最后到叫料位置地标放空料车
                                //        ArmLands.Add(CallStore.LankMarkCode + ",0,0");
                                //        //持久化任务
                                //        if (ChekAllowCreatTask(pad.PadID, CallStore.LankMarkCode) <= 0)
                                //        {
                                //            TaskSQL = "if not exists(select 1 from tbDispatchTaskInfo where stationNo=" + pad.PadID.ToString() + " and CallLand='" + CallStore.LankMarkCode + "' and TaskState in(0,1)) \r\n" +
                                //                          " begin \r\n" +
                                //                          " insert into tbDispatchTaskInfo( \r\n" +
                                //                         "dispatchNo,stationNo,taskType,BuildTime,CallLand) values ( \r\n" +
                                //                         "'" + dispatchNo + "'," + pad.PadID.ToString() + ",1,'" + DateTime.Now.ToString("yyyy/MM/dd/HH/mm/ss") + "','" + CallStore.LankMarkCode + "') \r\n";

                                //            int TaskDetailID = 1;
                                //            foreach (string item in ArmLands)
                                //            {
                                //                string TempLand = item.Split(',')[0];
                                //                string OperType = item.Split(',')[1];
                                //                string PutType = item.Split(',')[2];
                                //                TaskSQL += " insert into tbDispatchTaskDetail ( \r\n" +
                                //               "dispatchNo,DetailID,LandCode,OperType,PutType,IsAllowExcute) values ( \r\n" +
                                //               "'" + dispatchNo + "'," + TaskDetailID.ToString() + ",'" + TempLand + "'," + OperType + "," + PutType + "," +
                                //               "1" + ") \r\n";
                                //                TaskSQL += " UPDATE  tbLocaton \r\n" +
                                //               "SET LockState =2 \r\n" +
                                //               "where LankMarkCode = '" + TempLand + "'\r\n";
                                //                TaskDetailID++;
                                //            }

                                //            TaskSQL = TaskSQL + " end \r\n";
                                //            Hashtable hs = new Hashtable();
                                //            hs["sql"] = TaskSQL;
                                //            CoreData.DbHelper.ExecuteSql("0029", hs);
                                //            foreach (string item in ArmLands)
                                //            {
                                //                string TempLand = item.Split(',')[0];
                                //                StorageInfo store = CoreData.StorageList.FirstOrDefault(p => p.LankMarkCode == TempLand);
                                //                if (store != null) { store.LockState = 2; }
                                //            }
                                //            pack = new MessagePackage("1|操作成功!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                //            data = pack.ToBuffer();
                                //            session.TrySend(data, 0, data.Length);
                                //        }
                                //        else
                                //        {
                                //            LogHelper.WriteCreatTaskLog("存在未完成任务");
                                //            pack = new MessagePackage("0|存在未完成任务|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                //            data = pack.ToBuffer();
                                //            session.TrySend(data, 0, data.Length);
                                //        }
                                //        #endregion
                                //    }
                                //}
                                #endregion
                                break;
                            #endregion

                            #region 清洗呼叫[只有上料没有下料]
                            case 3:
                                if (CallType == 0)
                                {
                                    #region 清洗上料
                                    //ProduceBill = QueryProduceBillByBillID(BillNO, RGBType);
                                    //if (ProduceBill == null)
                                    //{
                                    //    LogHelper.WriteCallBoxLog("接受到的平板ID:" + BeeperID.ToString() + "发送的订单号:" + BillNO + "不存在!");
                                    //    pack = new MessagePackage("0|订单号:" + BillNO + "不存在!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                    //    data = pack.ToBuffer();
                                    //    session.TrySend(data, 0, data.Length);
                                    //    return;
                                    //}
                                    //先去呼叫地标拿空料车
                                    ArmLands.Add(CallStore.LankMarkCode + ",1,0");
                                    //查找可以放空料架的地标
                                    // 1 通过蓝光已烤区域找对照的平板
                                    PdaInfo Bake_BulePad = CoreData.AllPads.FirstOrDefault(p => p.Area == 8);
                                    if (Bake_BulePad == null)
                                    {
                                        LogHelper.WriteCreatTaskLog("接受到的平板ID:" + BeeperID.ToString() + "呼叫清洗上料【没找到待清洗平板】");
                                        pack = new MessagePackage("0|未找到待清洗区域|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                        return;
                                    }
                                    //根据蓝光已烤区的呼叫盒找对照地标明细
                                    List<PdaOperSetInfo> Bake_BulePad_Toland = CoreData.PadOperSets.Where(p => p.PdaID == Bake_BulePad.PadID && p.Area == 3).ToList();
                                    if (Bake_BulePad_Toland.Count <= 0)
                                    {
                                        LogHelper.WriteCreatTaskLog("待机检没有设置按钮信息");
                                        pack = new MessagePackage("0|待机检平板未设置按钮信息|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                        return;
                                    }
                                    //再找可以放空料架的位置地标
                                    StorageInfo NoneStorge = (from a in CoreData.StorageList
                                                              where a.LockState == 0 && a.StorageState == 0 &&
                                                              Bake_BulePad_Toland.Where(p => p.BtnSendValue == a.ID.ToString()).Count() > 0
                                                              select a).FirstOrDefault();
                                    if (NoneStorge == null || (NoneStorge != null && string.IsNullOrEmpty(NoneStorge.LankMarkCode)))
                                    {
                                        LogHelper.WriteCreatTaskLog("待机检没有空位置");
                                        pack = new MessagePackage("0|待机检区没有空位置|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                        return;
                                    }
                                    ArmLands.Add(NoneStorge.LankMarkCode + ",0,0");
                                    //再找满料位置地标
                                    StorageInfo FillStorge = (from a in CoreData.StorageList
                                                              where a.LockState == 0 && a.StorageState == 2 &&
                                                              /*a.MaterielType == ProduceBill.MachineType && */Bake_BulePad_Toland.Where(p => p.BtnSendValue == a.ID.ToString()).Count() > 0
                                                              select a).FirstOrDefault();

                                    if (FillStorge == null || (FillStorge != null && string.IsNullOrEmpty(FillStorge.LankMarkCode)))
                                    {
                                        LogHelper.WriteCreatTaskLog("待机检没有满料位置");
                                        pack = new MessagePackage("0|待机检区没有满料|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                        return;
                                    }
                                    ArmLands.Add(FillStorge.LankMarkCode + ",1,0");
                                    //再到呼叫点去放满料
                                    ArmLands.Add(CallStore.LankMarkCode + ",0,1");
                                    //持久化任务
                                    if (ChekAllowCreatTask(pad.PadID, CallStore.LankMarkCode) <= 0)
                                    {
                                        TaskSQL = "if not exists(select 1 from tbDispatchTaskInfo where stationNo=" + pad.PadID.ToString() + " and CallLand='" + CallStore.LankMarkCode + "' and TaskState in(0,1)) \r\n" +
                                                 " begin \r\n" +
                                                 " insert into tbDispatchTaskInfo( \r\n" +
                                                "dispatchNo,stationNo,taskType,BuildTime,CallLand) values ( \r\n" +
                                                "'" + dispatchNo + "'," + pad.PadID.ToString() + ",0,'" + DateTime.Now.ToString("yyyy/MM/dd/HH/mm/ss") + "','" + CallStore.LankMarkCode + "') \r\n";

                                        int TaskDetailID = 1;
                                        foreach (string item in ArmLands)
                                        {
                                            string TempLand = item.Split(',')[0];
                                            string OperType = item.Split(',')[1];
                                            string PutType = item.Split(',')[2];
                                            TaskSQL += "insert into tbDispatchTaskDetail ( \r\n" +
                                           "dispatchNo,DetailID,LandCode,OperType,PutType,IsAllowExcute) values ( \r\n" +
                                           "'" + dispatchNo + "'," + TaskDetailID.ToString() + ",'" + TempLand + "'," + OperType + "," + PutType + "," +
                                           "1"/*(TempLand == ArmLand4 ? "0" : "1")*/ + ") \r\n";
                                            TaskSQL += " UPDATE  tbLocaton \r\n" +
                                            "SET LockState =2 \r\n" +
                                            "where LankMarkCode = '" + TempLand + "'\r\n";
                                            TaskDetailID++;
                                        }
                                        TaskSQL = TaskSQL + " end \r\n";
                                        Hashtable hs = new Hashtable();
                                        hs["sql"] = TaskSQL;
                                        CoreData.DbHelper.ExecuteSql("0029", hs);
                                        foreach (string item in ArmLands)
                                        {
                                            string TempLand = item.Split(',')[0];
                                            StorageInfo store = CoreData.StorageList.FirstOrDefault(p => p.LankMarkCode == TempLand);
                                            if (store != null) { store.LockState = 2; }
                                        }
                                        pack = new MessagePackage("1|操作成功!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                        return;
                                    }
                                    else
                                    {
                                        LogHelper.WriteCreatTaskLog("存在未完成任务");
                                        pack = new MessagePackage("0|存在未完成任务|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                    }
                                    #endregion
                                }
                                break;
                            #endregion

                            #region 焊线呼叫
                            case 4:
                                if (CallType == 0)
                                {
                                    #region 焊线上料
                                    ProduceBill = QueryProduceBillByBillID(BillNO, RGBType);
                                    if (ProduceBill == null)
                                    {
                                        LogHelper.WriteCallBoxLog("接受到的平板ID:" + BeeperID.ToString() + "发送的订单号:" + BillNO + "不存在!");
                                        pack = new MessagePackage("0|订单号:" + BillNO + "不存在!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                        return;
                                    }
                                    //先去呼叫地标拿空料车
                                    ArmLands.Add(CallStore.LankMarkCode + ",1,0");
                                    //根据物料类型来找清洗区下料点的所有平板
                                    PdaInfo WashDown_Pad = CoreData.AllPads.FirstOrDefault(p => p.Area == 9);
                                    if (WashDown_Pad == null)
                                    {
                                        LogHelper.WriteCreatTaskLog("接受到的平板ID:" + BeeperID.ToString() + "呼叫焊线上料【没找到待焊线平板】");
                                        pack = new MessagePackage("0|未找到待焊线区域|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                        return;
                                    }
                                    //根据清洗区下料点的平板找对照地标明细
                                    List<PdaOperSetInfo> WashDown_Pad_Toland = CoreData.PadOperSets.Where(p => p.PdaID == WashDown_Pad.PadID).ToList();
                                    if (WashDown_Pad_Toland.Count <= 0)
                                    {
                                        LogHelper.WriteCreatTaskLog("待焊线区没有设置按钮信息");
                                        pack = new MessagePackage("0|待焊线区平板未设置按钮信息|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                        return;
                                    }

                                    //再找可以放空料架的位置地标
                                    StorageInfo NoneStorge = (from a in CoreData.StorageList
                                                              where a.LockState == 0 && a.StorageState == 0 &&
                                                              WashDown_Pad_Toland.Where(p => p.BtnSendValue == a.ID.ToString()).Count() > 0
                                                              select a).FirstOrDefault();

                                    if (NoneStorge == null || (NoneStorge != null && string.IsNullOrEmpty(NoneStorge.LankMarkCode)))
                                    {
                                        LogHelper.WriteCreatTaskLog("待焊线区没有空位置");
                                        pack = new MessagePackage("0|待焊线区无空位置|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                        return;
                                    }
                                    ArmLands.Add(NoneStorge.LankMarkCode + ",0,0");
                                    //再找满料位置地标
                                    StorageInfo FillStorge = (from a in CoreData.StorageList
                                                              where a.LockState == 0 && a.StorageState == 2 &&
                                                              a.MaterielType == ProduceBill.MachineType && WashDown_Pad_Toland.Where(p => p.BtnSendValue == a.ID.ToString()).Count() > 0
                                                              select a).FirstOrDefault();
                                    if (FillStorge == null || (FillStorge != null && string.IsNullOrEmpty(FillStorge.LankMarkCode)))
                                    {
                                        LogHelper.WriteCreatTaskLog("待焊线区没有满料位置");
                                        pack = new MessagePackage("0|待焊线区无满料位置|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                        return;
                                    }
                                    ArmLands.Add(FillStorge.LankMarkCode + ",1,0");
                                    //再到呼叫点去放满料架
                                    ArmLands.Add(CallStore.LankMarkCode + ",0,1");
                                    //持久化任务
                                    if (ChekAllowCreatTask(pad.PadID, CallStore.LankMarkCode) <= 0)
                                    {
                                        TaskSQL = "if not exists(select 1 from tbDispatchTaskInfo where stationNo=" + pad.PadID.ToString() + " and CallLand='" + CallStore.LankMarkCode + "' and TaskState in(0,1)) \r\n" +
                                                          " begin \r\n" +
                                                          " insert into tbDispatchTaskInfo(\r\n" +
                                                     "dispatchNo,stationNo,taskType,BuildTime,CallLand) values (\r\n" +
                                                     "'" + dispatchNo + "'," + pad.PadID.ToString() + ",0,'" + DateTime.Now.ToString("yyyy/MM/dd/HH/mm/ss") + "','" + CallStore.LankMarkCode + "')\r\n";

                                        int TaskDetailID = 1;
                                        foreach (string item in ArmLands)
                                        {
                                            string TempLand = item.Split(',')[0];
                                            string OperType = item.Split(',')[1];
                                            string PutType = item.Split(',')[2];
                                            TaskSQL += "insert into tbDispatchTaskDetail ( \r\n" +
                                           "dispatchNo,DetailID,LandCode,OperType,PutType,IsAllowExcute) values ( \r\n" +
                                           "'" + dispatchNo + "'," + TaskDetailID.ToString() + ",'" + TempLand + "'," + OperType + "," + PutType + "," +
                                          "1" /*(TempLand == ArmLand3 ? "0" : "1")*/ + ") \r\n";
                                            TaskSQL += " UPDATE  tbLocaton \r\n" +
                                            "SET LockState =2 \r\n" +
                                            "where LankMarkCode = '" + TempLand + "'\r\n";
                                            TaskDetailID++;
                                        }

                                        TaskSQL = TaskSQL + " end \r\n";
                                        Hashtable hs = new Hashtable();
                                        hs["sql"] = TaskSQL;
                                        CoreData.DbHelper.ExecuteSql("0029", hs);
                                        foreach (string item in ArmLands)
                                        {
                                            string TempLand = item.Split(',')[0];
                                            StorageInfo store = CoreData.StorageList.FirstOrDefault(p => p.LankMarkCode == TempLand);
                                            if (store != null) { store.LockState = 2; }
                                        }
                                        pack = new MessagePackage("1|操作成功!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                    }
                                    else
                                    {
                                        LogHelper.WriteCreatTaskLog("存在未完成任务");
                                        pack = new MessagePackage("0|存在未完成任务|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 焊线下料
                                    //先添加目的地标,去拉满料
                                    ArmLands.Add(CallStore.LankMarkCode + ",1,0");
                                    //1 根据物料类型找点胶区域的按钮盒
                                    PdaInfo WaitDJBox = CoreData.AllPads.FirstOrDefault(p => p.Area == 5);
                                    if (WaitDJBox == null)
                                    {
                                        LogHelper.WriteCreatTaskLog("接受到的平板ID:" + BeeperID.ToString() + "呼叫焊线下料【没找到点胶区平板】");
                                        pack = new MessagePackage("0|未找到点胶区平板|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                        return;
                                    }
                                    //2 再根据点胶按钮盒来找对照的位置地标
                                    List<PdaOperSetInfo> WaitDJBoxToland = CoreData.PadOperSets.Where(p => p.PdaID == WaitDJBox.PadID).ToList();
                                    if (WaitDJBoxToland.Count <= 0)
                                    {
                                        LogHelper.WriteCreatTaskLog("点胶区没有设置按钮信息");
                                        pack = new MessagePackage("0|点胶区平板未设置按钮信息|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                        return;
                                    }
                                    //3 在其中再找空位置地标，不考虑物料类型
                                    StorageInfo NoneStorge = (from a in CoreData.StorageList
                                                              where a.LockState == 0 && a.StorageState == 0 &&
                                                              WaitDJBoxToland.Where(p => p.BtnSendValue == a.ID.ToString()).Count() > 0
                                                              select a).FirstOrDefault();

                                    if (NoneStorge == null || (NoneStorge != null && string.IsNullOrEmpty(NoneStorge.LankMarkCode)))
                                    {
                                        LogHelper.WriteCreatTaskLog("点胶区没有空位置");
                                        pack = new MessagePackage("0|点胶区无空位置|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                        return;
                                    }
                                    //4 找到可以放焊线成品的空位置地标放满料
                                    ArmLands.Add(NoneStorge.LankMarkCode + ",0,1");
                                    //5 找一个空料车拉回
                                    StorageInfo FillStorge = (from a in CoreData.StorageList
                                                              where a.LockState == 0 && a.StorageState == 1 &&
                                                              WaitDJBoxToland.Where(p => p.BtnSendValue == a.ID.ToString()).Count() > 0
                                                              select a).FirstOrDefault();


                                    if (FillStorge == null || (FillStorge != null && string.IsNullOrEmpty(FillStorge.LankMarkCode)))
                                    {
                                        LogHelper.WriteCreatTaskLog("点胶区没有空料架位置");
                                        pack = new MessagePackage("0|点胶区无空料架|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                        return;
                                    }
                                    ArmLands.Add(FillStorge.LankMarkCode + ",1,0");
                                    //6 最后到叫料位置地标放空车
                                    ArmLands.Add(CallStore.LankMarkCode + ",0,0");
                                    //持久化任务
                                    if (ChekAllowCreatTask(pad.PadID, CallStore.LankMarkCode) <= 0)
                                    {
                                        TaskSQL = "if not exists(select 1 from tbDispatchTaskInfo where stationNo=" + pad.PadID.ToString() + " and CallLand='" + CallStore.LankMarkCode + "' and TaskState in(0,1)) \r\n" +
                                                      " begin \r\n" +
                                                      " insert into tbDispatchTaskInfo( \r\n" +
                                                     "dispatchNo,stationNo,taskType,BuildTime,CallLand) values ( \r\n" +
                                                     "'" + dispatchNo + "'," + pad.PadID.ToString() + ",1,'" + DateTime.Now.ToString("yyyy/MM/dd/HH/mm/ss") + "','" + CallStore.LankMarkCode + "') \r\n";

                                        int TaskDetailID = 1;

                                        foreach (string item in ArmLands)
                                        {
                                            string TempLand = item.Split(',')[0];
                                            string OperType = item.Split(',')[1];
                                            string PutType = item.Split(',')[2];
                                            TaskSQL += "insert into tbDispatchTaskDetail ( \r\n" +
                                           "dispatchNo,DetailID,LandCode,OperType,PutType,IsAllowExcute) values ( \r\n" +
                                           "'" + dispatchNo + "'," + TaskDetailID.ToString() + ",'" + TempLand + "'," + OperType + "," + PutType + "," +
                                           "1" + ") \r\n";
                                            TaskSQL += " UPDATE  tbLocaton \r\n" +
                                           "SET LockState =2 \r\n" +
                                           "where LankMarkCode = '" + TempLand + "'\r\n";
                                            TaskDetailID++;
                                        }
                                        TaskSQL = TaskSQL + " end \r\n";
                                        Hashtable hs = new Hashtable();
                                        hs["sql"] = TaskSQL;
                                        CoreData.DbHelper.ExecuteSql("0029", hs);
                                        foreach (string item in ArmLands)
                                        {
                                            string TempLand = item.Split(',')[0];
                                            StorageInfo store = CoreData.StorageList.FirstOrDefault(p => p.LankMarkCode == TempLand);
                                            if (store != null) { store.LockState = 2; }
                                        }
                                        pack = new MessagePackage("1|操作成功!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                    }
                                    else
                                    {
                                        LogHelper.WriteCreatTaskLog("存在未完成任务");
                                        pack = new MessagePackage("0|存在未完成任务|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                    }
                                    #endregion
                                }
                                break;
                            #endregion

                            #region 空料盒回收区
                            case 6:
                                if (CallType == 0)
                                {
                                    #region 呼叫回收空料盒到当前呼叫位置
                                    //先去呼叫地标拿空料车
                                    ArmLands.Add(CallStore.LankMarkCode + ",1,0");
                                    //查找有空料盒的位置
                                    //1,找空料盒区域的空位置
                                    PdaInfo EmptyPad = CoreData.AllPads.FirstOrDefault(p => p.Area == 12);
                                    if (EmptyPad == null)
                                    {
                                        LogHelper.WriteCreatTaskLog("接受到的平板ID:" + BeeperID.ToString() + "呼叫回收空料盒【没找到空料盒平板】");
                                        pack = new MessagePackage("0|未找到空料盒区域|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                        return;
                                    }
                                    //2,根据空料盒区的呼叫盒找对照地标明细
                                    List<PdaOperSetInfo> EmptyBox_Toland = CoreData.PadOperSets.Where(p => p.PdaID == EmptyPad.PadID).ToList();
                                    if (EmptyBox_Toland.Count <= 0)
                                    {
                                        LogHelper.WriteCreatTaskLog("空料盒区没有设置按钮信息");
                                        pack = new MessagePackage("0|空料盒区未设置按钮信息|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                        return;
                                    }
                                    //3,再找空的位置地标
                                    StorageInfo NoneStorge = (from a in CoreData.StorageList
                                                              where a.LockState == 0 && a.StorageState == 0 &&
                                                              EmptyBox_Toland.Where(p => p.BtnSendValue == a.ID.ToString()).Count() > 0
                                                              select a).FirstOrDefault();

                                    if (NoneStorge == null || (NoneStorge != null && string.IsNullOrEmpty(NoneStorge.LankMarkCode)))
                                    {
                                        LogHelper.WriteCreatTaskLog("空料盒区没有空位置");
                                        pack = new MessagePackage("0|空料盒区无空位置|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                        return;
                                    }
                                    //4,找到有空位置
                                    ArmLands.Add(NoneStorge.LankMarkCode + ",0,0");
                                    //5,找有空料盒的位置
                                    StorageInfo FillStorge = (from a in CoreData.StorageList
                                                              where a.LockState == 0 && a.StorageState == 2 &&
                                                              EmptyBox_Toland.Where(p => p.BtnSendValue == a.ID.ToString()).Count() > 0
                                                              select a).FirstOrDefault();
                                    if (FillStorge == null || (FillStorge != null && string.IsNullOrEmpty(FillStorge.LankMarkCode)))
                                    {
                                        LogHelper.WriteCreatTaskLog("空料盒区没有满料位置");
                                        pack = new MessagePackage("0|空料盒区无满料|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                        return;
                                    }
                                    //6,找到有空料盒的位置
                                    ArmLands.Add(FillStorge.LankMarkCode + ",1,0");
                                    //7,最后把空料盒送到呼叫的位置
                                    ArmLands.Add(CallStore.LankMarkCode + ",0,1");
                                    //持久化任务
                                    if (ChekAllowCreatTask(pad.PadID, CallStore.LankMarkCode) <= 0)
                                    {
                                        TaskSQL = "if not exists(select 1 from tbDispatchTaskInfo where stationNo=" + pad.PadID.ToString() + " and CallLand='" + CallStore.LankMarkCode + "' and TaskState in(0,1)) \r\n" +
                                                     " begin \r\n" +
                                                     " insert into tbDispatchTaskInfo( \r\n" +
                                                    "dispatchNo,stationNo,taskType,BuildTime,CallLand) values ( \r\n" +
                                                    "'" + dispatchNo + "'," + pad.PadID.ToString() + ",0,'" + DateTime.Now.ToString("yyyy/MM/dd/HH/mm/ss") + "','" + CallStore.LankMarkCode + "') \r\n";

                                        int TaskDetailID = 1;
                                        foreach (string item in ArmLands)
                                        {
                                            string TempLand = item.Split(',')[0];
                                            string OperType = item.Split(',')[1];
                                            string PutType = item.Split(',')[2];
                                            TaskSQL += "insert into tbDispatchTaskDetail ( \r\n" +
                                           "dispatchNo,DetailID,LandCode,OperType,PutType,IsAllowExcute) values ( \r\n" +
                                           "'" + dispatchNo + "'," + TaskDetailID.ToString() + ",'" + TempLand + "'," + OperType + "," + PutType + "," +
                                           "1"/*(TempLand == ArmLand4 ? "0" : "1")*/ + ") \r\n";
                                            TaskSQL += " UPDATE  tbLocaton \r\n" +
                                            "SET LockState =2 \r\n" +
                                            "where LankMarkCode = '" + TempLand + "'\r\n";
                                            TaskDetailID++;
                                        }

                                        TaskSQL = TaskSQL + " end \r\n";
                                        Hashtable hs = new Hashtable();
                                        hs["sql"] = TaskSQL;
                                        CoreData.DbHelper.ExecuteSql("0029", hs);
                                        foreach (string item in ArmLands)
                                        {
                                            string TempLand = item.Split(',')[0];
                                            StorageInfo store = CoreData.StorageList.FirstOrDefault(p => p.LankMarkCode == TempLand);
                                            if (store != null) { store.LockState = 2; }
                                        }
                                        pack = new MessagePackage("1|操作成功!|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                    }
                                    else
                                    {
                                        LogHelper.WriteCreatTaskLog("存在未完成任务");
                                        pack = new MessagePackage("0|存在未完成任务|" + BtnID.ToString(), SocketCommand.BeeperCall);
                                        data = pack.ToBuffer();
                                        session.TrySend(data, 0, data.Length);
                                    }
                                    #endregion
                                }
                                break;
                                #endregion
                        }
                    }//end呼叫处理
                }
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }

        static object lockResponseLayoutObj = new object();
        /// <summary>
        /// 单独响应平板按钮布局
        /// </summary>
        private void SynAloneBeeperLayout(ref SLSocketSession session, string Mes)
        {
            try
            {
                lock (lockResponseLayoutObj)
                {
                    session.ClienType = TCPClienTypeEnum.App;
                    int PDAID = Convert.ToInt16(Mes);
                    session.ID = PDAID;
                    IList<PdaOperSetInfo> PdaOperSets = LoadPadOperSetInfo(PDAID);
                    if (PdaOperSets != null && PdaOperSets.Count > 0)
                    {
                        MessagePackage pack = new MessagePackage("1|" + JosnTool.GetJson(PdaOperSets), SocketCommand.SynBeeperBtnLayout);
                        byte[] data = pack.ToBuffer();
                        session.TrySend(data, 0, data.Length);
                    }
                    else
                    {
                        MessagePackage pack = new MessagePackage("0|无按钮操作档案", SocketCommand.SynBeeperBtnLayout);
                        byte[] data = pack.ToBuffer();
                        session.TrySend(data, 0, data.Length);
                    }

                }
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }

        static object lockResponseBillObj = new object();
        /// <summary>
        /// 单独返回呼叫器的相应订单逻辑
        /// </summary>
        private void SynAloneBeeperBill(ref SLSocketSession session, string Mes)
        {
            try
            {
                lock (lockResponseBillObj)
                {
                    session.ClienType = TCPClienTypeEnum.App;
                    int PDAID = Convert.ToInt16(Mes);
                    session.ID = PDAID;
                    PdaInfo pad = CoreData.AllPads.FirstOrDefault(p => p.PadID == PDAID);
                    IList<ProduceBillInfo> ProduceBills = null;
                    MessagePackage pack = null;
                    byte[] data = null;
                    if (pad != null)
                    {
                        if (pad.PadType == 1)
                        { ProduceBills = LoadAllBill(); }
                        else
                        { ProduceBills = LoadPDABill(PDAID); }
                        if (ProduceBills != null && ProduceBills.Count > 0)
                        {
                            pack = new MessagePackage("1|" + JosnTool.GetJson(ProduceBills), SocketCommand.SynBeeperBill);
                            data = pack.ToBuffer();
                            session.TrySend(data, 0, data.Length);
                            return;
                        }
                    }
                    ProduceBills = new List<ProduceBillInfo>();
                    pack = new MessagePackage("1|" + JosnTool.GetJson(ProduceBills), SocketCommand.SynBeeperBill);
                    data = pack.ToBuffer();
                    session.TrySend(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }

        /// <summary>
        /// 向所有的触摸屏同步对应的生产订单(指在人员在排产完成后点击“下发”按钮主动向所有的平板推送其对用的订单)
        /// </summary>
        public async void SynAllBeeperBill()
        {
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    var sessions = appserver.GetAllSessions().Where(p => p.ClienType == TCPClienTypeEnum.App);
                    if (sessions != null)
                    {
                        foreach (var session in sessions)
                        {
                            int PDAID = session.ID;
                            PdaInfo pad = CoreData.AllPads.FirstOrDefault(p => p.PadID == PDAID);
                            if (pad == null) { return; }
                            IList<ProduceBillInfo> ProduceBills = new List<ProduceBillInfo>();
                            if (pad.PadType == 1)
                            { ProduceBills = LoadAllBill(); }
                            else
                            { ProduceBills = LoadPDABill(PDAID); }
                            if (ProduceBills != null && ProduceBills.Count > 0)
                            {
                                MessagePackage pack = new MessagePackage("1|" + JosnTool.GetJson(ProduceBills), SocketCommand.SynBeeperBill);
                                byte[] data = pack.ToBuffer();
                                session.TrySend(data, 0, data.Length);
                            }
                            else
                            {
                                ProduceBills = new List<ProduceBillInfo>();
                                MessagePackage pack = new MessagePackage("1|" + JosnTool.GetJson(ProduceBills), SocketCommand.SynBeeperBill);
                                byte[] data = pack.ToBuffer();
                                session.TrySend(data, 0, data.Length);
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }

        /// <summary>
        /// 发送平板报警
        /// </summary>
        public async void SendPadWarm()
        {
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    double lookingOutTime = 0;
                    try
                    {
                        string lookingOutTimeStr = CoreData.SysParameter["lookingOutTime"].ToString();
                        lookingOutTime = Convert.ToDouble(lookingOutTimeStr);
                    }
                    catch
                    { LogHelper.WriteLog("系统参数监控状态超时时间(分)值设置错误!"); return; }
                    if (lookingOutTime == 0) { return; }
                    //1，先找到已经报警的平板
                    List<PdaInfo> HasWarmPads = CoreData.AllPads.Where(p => p.PadType == 1 && p.IsWarm == true).ToList();
                    List<PdaInfo> MonitorPads = CoreData.AllPads.Where(p => p.PadType == 1).ToList();
                    List<PdaOperSetInfo> PadSets = CoreData.PadOperSets.Where(P => MonitorPads.Count(q => q.PadID == P.PdaID) > 0).ToList();
                    if (PadSets != null && PadSets.Count > 0)
                    {
                        List<StorageInfo> MonitorStores = CoreData.StorageList.Where(q => PadSets.Where(p => p.BtnSendValue == q.ID.ToString()).Count() > 0).ToList();
                        if (MonitorStores != null && MonitorStores.Count > 0)
                        {
                            //找到了所有的监控区域的储位
                            //找到本次需要报警的储位
                            List<StorageInfo> NeedWarmStore = (from a in MonitorStores
                                                               where CheckIsPadWarm(a.AGVArriveTime, a.HandTime, lookingOutTime)
                                                               select a).ToList();
                            if (NeedWarmStore != null && NeedWarmStore.Count > 0)
                            {
                                //再通过需要报警的储位找到其所属的平板
                                List<PdaOperSetInfo> WarmPdaSets = (from a in CoreData.PadOperSets
                                                                    where NeedWarmStore.Count(p => p.ID.ToString() == a.BtnSendValue) > 0
                                                                    select a).ToList();
                                List<PdaInfo> WarmPads = (from a in CoreData.AllPads
                                                          where WarmPdaSets.Count(q => q.PdaID == a.PadID) > 0
                                                          select a).Distinct().ToList();
                                if (WarmPads != null)
                                {
                                    foreach (PdaInfo item in WarmPads)
                                    {
                                        var session = appserver.GetAllSessions().FirstOrDefault(p => p.ClienType == TCPClienTypeEnum.App && p.ID == item.PadID);
                                        if (session != null)
                                        {
                                            MessagePackage pack = new MessagePackage("1", SocketCommand.PadWarm);
                                            byte[] data = pack.ToBuffer();
                                            session.TrySend(data, 0, data.Length);
                                            item.IsWarm = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //查找现在报警的平板
                    List<PdaInfo> NowWarmPads = CoreData.AllPads.Where(p => p.PadType == 1 && p.IsWarm == true).ToList();
                    //排除不需要报警的
                    List<PdaInfo> NotWarmPads = NowWarmPads.Where(p => HasWarmPads.Count(q => q.PadID == p.PadID) <= 0).ToList();
                    foreach (PdaInfo item in NotWarmPads)
                    {
                        var session = appserver.GetAllSessions().FirstOrDefault(p => p.ClienType == TCPClienTypeEnum.App && p.ID == item.PadID);
                        if (session != null)
                        {
                            MessagePackage pack = new MessagePackage("0", SocketCommand.PadWarm);
                            byte[] data = pack.ToBuffer();
                            session.TrySend(data, 0, data.Length);
                            item.IsWarm = false;
                        }
                    }
                });
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }

        /// <summary>
        /// 判断具体平板是否报警
        /// </summary>
        private bool CheckIsPadWarm(string AGVArriveTime, string HandTime, double lookingOutTime)
        {
            try
            {
                if (!string.IsNullOrEmpty(AGVArriveTime) && string.IsNullOrEmpty(HandTime))
                {
                    DateTime ArriveTime = DateTime.ParseExact(AGVArriveTime, "yyyyMMddHHmmss", CultureInfo.CurrentCulture);
                    DateTime dtNow = DateTime.Now;
                    if ((dtNow - ArriveTime).TotalMinutes >= lookingOutTime)
                    { return true; }
                }
                if (!string.IsNullOrEmpty(AGVArriveTime) && !string.IsNullOrEmpty(HandTime))
                {
                    DateTime ArriveTime = DateTime.ParseExact(AGVArriveTime, "yyyyMMddHHmmss", CultureInfo.CurrentCulture);
                    DateTime HandleTime = DateTime.ParseExact(HandTime, "yyyyMMddHHmmss", CultureInfo.CurrentCulture);
                    DateTime dtNow = DateTime.Now;
                    if (ArriveTime > HandleTime && (dtNow - ArriveTime).TotalMinutes >= lookingOutTime)
                    { return true; }
                }
                return false;
            }
            catch (Exception ex)
            { throw ex; }
        }

        static object lockResponseAppLog = new object();
        /// <summary>
        /// 验证平板登陆
        /// </summary>
        private void CheckAppLog(ref SLSocketSession session, string Mes)
        {
            try
            {
                lock (lockResponseAppLog)
                {
                    MessagePackage pack = null;
                    byte[] data = null;
                    LogHelper.WriteCallBoxLog("接受到的平板登陆信息:" + Mes);
                    if (string.IsNullOrEmpty(Mes))
                    {
                        pack = new MessagePackage("0", SocketCommand.AppLoginCheck);
                        data = pack.ToBuffer();
                        session.TrySend(data, 0, data.Length);
                        return;
                    }
                    string[] meses = Mes.Split('|');
                    if (meses == null || (meses != null && meses.Length < 2))
                    {
                        pack = new MessagePackage("0", SocketCommand.AppLoginCheck);
                        data = pack.ToBuffer();
                        session.TrySend(data, 0, data.Length);
                        return;
                    }
                    string UserAccount = meses[0].ToString();
                    string PassWord = meses[1].ToString();
                    if (CheckUser(UserAccount, PassWord))
                    { pack = new MessagePackage("1", SocketCommand.AppLoginCheck); }
                    else
                    { pack = new MessagePackage("0", SocketCommand.AppLoginCheck); }
                    data = pack.ToBuffer();
                    session.TrySend(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }

        static object lockSynAppStoreStateObj = new object();
        /// <summary>
        /// 同步平板端储位状态
        /// </summary>
        public void SynAppStoreState()
        {
            try
            {
                lock (lockSynAppStoreStateObj)
                {
                    var sessions = appserver.GetAllSessions().Where(p => p.ClienType == TCPClienTypeEnum.App);
                    if (sessions != null)
                    {
                        foreach (var session in sessions)
                        {
                            int PDAID = session.ID;
                            PdaInfo pad = CoreData.AllPads.FirstOrDefault(p => p.PadID == PDAID);
                            if (pad != null && pad.PadType == 1)
                            {
                                IList<PdaOperSetInfo> PdaOperSets = LoadPadOperSetInfo(PDAID);
                                if (PdaOperSets != null && PdaOperSets.Count > 0)
                                {
                                    MessagePackage pack = new MessagePackage("1|" + JosnTool.GetJson(PdaOperSets), SocketCommand.SynAppStoreState);
                                    byte[] data = pack.ToBuffer();
                                    session.TrySend(data, 0, data.Length);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }
        #endregion

        #region 操作数据库
        /// <summary>
        /// 加载平板对应的生产订单
        /// </summary>
        private IList<ProduceBillInfo> LoadPDABill(int PadID)
        {
            try
            {
                Hashtable dic = new Hashtable();
                dic["PDAID"] = PadID;
                DataTable dtPadBillInfo = CoreData.DbHelper.Query("LoadPDABill", dic);
                return DataToObject.TableToEntity<ProduceBillInfo>(dtPadBillInfo);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 监控平板加载所有订单
        /// </summary>
        private IList<ProduceBillInfo> LoadAllBill()
        {
            try
            {
                Hashtable dic = new Hashtable();
                DataTable dtPadBillInfo = CoreData.DbHelper.Query("LoadAllBill", dic);
                return DataToObject.TableToEntity<ProduceBillInfo>(dtPadBillInfo);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 根据平板ID来判断是否为监控类型的平板，如果是则返回其对应的按钮操作设置
        /// </summary>
        private IList<PdaOperSetInfo> LoadPadOperSetInfo(int PadID)
        {
            try
            {
                //先判断平板是否为监控类型的
                PdaInfo pad = CoreData.AllPads.FirstOrDefault(p => p.PadID == PadID);
                if (pad != null)//如果平板是监控类型的
                {
                    Hashtable hs = new Hashtable();
                    hs["PadID"] = PadID;
                    DataTable dtPadSetInfos = CoreData.DbHelper.Query("QueryPadOperSetByPadID", hs);
                    return DataToObject.TableToEntity<PdaOperSetInfo>(dtPadSetInfos);
                }
                return null;
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); return null; }
        }

        /// <summary>
        /// 根据生产订单号取生产订单信息
        /// </summary>
        private ProduceBillInfo QueryProduceBillByBillID(string BillID, int RGBType)
        {
            try
            {
                Hashtable dic = new Hashtable();
                dic["ProduceBillID"] = BillID;
                dic["RGBType"] = RGBType;
                DataTable dtProduces = CoreData.DbHelper.Query("QueryProduceBillByBillID", dic);
                IList<ProduceBillInfo> Produces = DataToObject.TableToEntity<ProduceBillInfo>(dtProduces);
                if (Produces != null && Produces.Count > 0)
                { return Produces[0]; }
                else
                { return null; }
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); return null; }
        }

        /// <summary>
        /// 加载当前平板呼叫的排产信息
        /// </summary>
        private ScheduingInfo QuerySchudingInfoByBillNo(string BillNo, int PadID, int RGBType)
        {
            try
            {
                Hashtable dic = new Hashtable();
                dic["BillNo"] = BillNo;
                dic["PadID"] = PadID;
                dic["RGBType"] = RGBType;
                DataTable dtSchedu = CoreData.DbHelper.Query("QuerySchudingInfo", dic);
                IList<ScheduingInfo> Schedus = DataToObject.TableToEntity<ScheduingInfo>(dtSchedu);
                if (Schedus != null && Schedus.Count > 0)
                { return Schedus[0]; }
                else
                { return null; }
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); return null; }
        }

        /// <summary>
        /// 根据地标编码更新储位状态
        /// </summary>
        private async void UpdateStore(int storeState, int MaterielType, string LandCode)
        {
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    Hashtable dic = new Hashtable();
                    dic["storeState"] = storeState;
                    dic["LandCode"] = LandCode;
                    dic["MaterielType"] = MaterielType;
                    dic["HandTime"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                    CoreData.DbHelper.ExecuteSql("00333", dic);
                });
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 判断当前呼叫点是否可以再次呼叫
        /// </summary>
        /// <returns></returns>
        public int ChekAllowCreatTask(int PadID, string CallLand)
        {
            try
            {
                Hashtable dic = new Hashtable();
                dic["CallBoxID"] = PadID;
                dic["CallLand"] = CallLand;
                DataTable dt = CoreData.DbHelper.Query("0041", dic);
                if (dt != null && dt.Rows.Count > 0)
                { return Convert.ToInt16(dt.Rows[0][0]); }
                else
                { return 0; }
            }
            catch (Exception ex)
            { return 0; }
        }

        /// <summary>
        /// 验证用户
        /// </summary>
        private bool CheckUser(string UserID, string PassWord)
        {
            try
            {
                Hashtable dic = new Hashtable();
                dic["UserID"] = UserID;
                dic["PassWord"] = PassWord;
                DataTable dtUser = CoreData.DbHelper.Query("0047", dic);
                UserInfo user = null;
                if (dtUser != null && dtUser.Rows.Count > 0)
                { user = DataToObject.TableToEntity<UserInfo>(dtUser)[0]; }
                else
                { return false; }
                if (user == null)
                { return false; }
                DataTable dt = CoreData.DbHelper.Query("0048", dic);
                IList<SysOprButtonToCategory> OperBtns = DataToObject.TableToEntity<SysOprButtonToCategory>(dt);
                if (OperBtns == null || (OperBtns != null && OperBtns.Count <= 0))
                { return false; }
                else
                {
                    if (OperBtns.Count(p => p.ButtonName == "allowAppSet" && p.ButtonType == 2) <= 0)
                    { return false; }
                    else
                    { return true; }
                }
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); return false; }
        }
        #endregion
    }
}
