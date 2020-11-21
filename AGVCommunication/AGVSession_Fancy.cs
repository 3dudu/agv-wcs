using AGVCore;
using DipatchModel;
using Model.CarInfoExtend;
using Model.MDM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tools;

namespace AGVCommunication
{
    public class AGVSession_Fancy : AGVSessionBase
    {
        #region 属性
        /// <summary>
        /// 锁变量
        /// </summary>
        public object lockobj = new object();

        /// <summary>
        /// 是否是停止服务
        /// </summary>
        private bool IsStop = false;

        ///
        /// 客户端socket对象
        /// </summary>
        public Socket Tcpsocket;

        /// <summary>
        /// 服务状态
        /// </summary>
        public bool KeepServer { get; set; }

        /// <summary>
        /// 服务线程(负责socket接收)
        /// </summary>
        private Thread processor;

        /// <summary>
        /// 接受包体
        /// </summary>
        private Thread RecevProvessor;

        /// <summary>
        /// 通讯观察线程,每秒观察一次
        /// </summary>
        System.Timers.Timer communicationobserve_timer = new System.Timers.Timer(1000 * 1);

        /// <summary>
        /// 用于TCP连接之前测试小车ping通 
        /// </summary>
        Ping ping = new Ping();

        /// <summary>
        /// 连接初委托
        /// </summary>
        ConnectSocketDelegate connect = null;

        /// <summary>
        ///发送数据委托 
        /// </summary>
        SocketSendDataDelegate SocketSend = null;

        /// <summary>
        /// 查List
        /// </summary>
        List<byte> readbytelist = new List<byte>();
        /// <summary>
        /// 查寄存器通道List
        /// </summary>
        List<byte> readrebytelist = new List<byte>();

        /// <summary>
        /// 写list
        /// </summary>
        List<byte> writebytelist = new List<byte>();
        #endregion

        #region 方法
         public AGVSession_Fancy()
         {
            communicationobserve_timer.AutoReset = true;
            communicationobserve_timer.Enabled = true;
            communicationobserve_timer.Elapsed += new System.Timers.ElapsedEventHandler(CommunicationObser);
            connect = ConnectSocket;
            SocketSend = SendData;
          }


        //public AGVSession_Fancy(int agvid, AGVComPara ComPara) : base()
        //{
        //    try
        //    {
        //        this.DeviceID = agvid;
        //        this.DeviceType = 0;
        //        this.DeviceName = agvid.ToString() + "号AGV小车";
        //        this.ComPara = ComPara;
        //        communicationobserve_timer.Elapsed += new System.Timers.ElapsedEventHandler(CommunicationObser);
        //        connect = ConnectSocket;
        //        SocketSend = SendData;
        //        //初始化读
        //        readbytelist.Clear();
        //        readbytelist.Add(0x55);
        //        readbytelist.Add((byte)agvid);
        //        readbytelist.AddRange(BitConverter.GetBytes(NextDataIndex()));
        //        readbytelist.Add(0x02);
        //        readbytelist.AddRange(GetSpaceBytes(19));
        //        readbytelist.Add(GetCheckByte(readbytelist));
        //        readbytelist.Add(0xaa);
        //        //读寄存器通道1
        //        readrebytelist.Clear();
        //        readrebytelist.Add(0x55);
        //        readrebytelist.Add((byte)this.DeviceID);
        //        readrebytelist.AddRange(BitConverter.GetBytes(NextDataIndex()));
        //        readrebytelist.Add(0x14);
        //        readrebytelist.Add(0x00);//起始位
        //        readrebytelist.Add(0x01);//数量
        //        readrebytelist.AddRange(GetSpaceBytes(17));
        //        readrebytelist.Add(GetCheckByte(readrebytelist));
        //        readrebytelist.Add(0xaa);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteErrorLog(ex);
        //    }
        //}

        public bool Init()
        {
            try
            {
                ////connect = ConnectSocket;
                ////SocketSend = SendData;
                ////初始化读
                //readbytelist.Clear();
                //readbytelist.Add(0x55);
                //readbytelist.Add((byte)this.DeviceID);
                //readbytelist.AddRange(BitConverter.GetBytes(NextDataIndex()));
                //readbytelist.Add(0x02);
                //readbytelist.AddRange(GetSpaceBytes(19));
                //readbytelist.Add(GetCheckByte(readbytelist));
                //readbytelist.Add(0xaa);
                ////读寄存器通道1
                //readrebytelist.Clear();
                //readrebytelist.Add(0x55);
                //readrebytelist.Add((byte)this.DeviceID);
                //readrebytelist.AddRange(BitConverter.GetBytes(NextDataIndex()));
                //readrebytelist.Add(0x14);
                //readrebytelist.Add(0x00);//起始位
                //readrebytelist.Add(0x01);//数量
                //readrebytelist.AddRange(GetSpaceBytes(17));
                //readrebytelist.Add(GetCheckByte(readrebytelist));
                //readrebytelist.Add(0xaa);

                Clear();
                Tcpsocket = new Socket(AddressFamily.InterNetwork,
                 SocketType.Stream, ProtocolType.Tcp);
                IsStop = false;
                return true;
            }
            catch (Exception ex)
            {

                CarBaseStateInfo car = new CarBaseStateInfo();
                car.AgvID = this.DeviceID;
                car.bIsCommBreak = true;
                DelegateState.InvokeCarFeedbackEvent(car);
                LogHelper.WriteErrorLog(ex);
                return false;
            }
        }

        private bool ConnectSocket(IPEndPoint ipep, Socket sock)
        {
            try
            {
                sock.Connect(ipep);
                return true;
            }
            catch (System.Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 通讯方法
        /// </summary>
        public void Communication()
        {
            try
            {
                while (true)
                {
                    CommandToValue ctov = null;
                    if (QueueCommand.Count > 0 && QueueCommand.TryPeek(out ctov))//有动作命令
                    {
                        int SendCount = 0;
                        //CommandToValue ctov = QueueCommand.Dequeue();
                       
                        //CarInfo CurrCar = CoreData.CarList.FirstOrDefault(p => p.AgvID == this.DeviceID);
                        //if (CurrCar != null && !string.IsNullOrEmpty(CurrCar.HistoryUpLandCode)
                        //    && !string.IsNullOrEmpty(CurrCar.OldHistoryUpLandCode)
                        //    && CurrCar.OldHistoryUpLandCode != "0" && CurrCar.HistoryUpLandCode != "0"
                        //    && CurrCar.OldHistoryUpLandCode == CurrCar.HistoryUpLandCode
                        //    && (ctov.Command == AGVCommandEnum.CancelTast || ctov.Command == AGVCommandEnum.ChangeRoute))
                        //{
                        //    //发送查询命令
                        //    SetReadByteList();
                        //    Tcpsocket.Send(readbytelist.ToArray());
                        //    //发送查寄存器通道命令
                        //    //Thread.Sleep(80);
                        //    //Tcpsocket.Send(readrebytelist.ToArray());
                        //    Thread.Sleep(400);
                        //    GetCallBack();
                        //    LogHelper.WriteLog("判断上次经过的地标");
                        //}
                        //else
                        //{

                        while ((!ExeCommand(ctov)) && SendCount < 20)
                        {
                            SendCount++;
                            Thread.Sleep(150);
                        }
                        QueueCommand.TryDequeue(out ctov);
                        //}
                    }
                    else
                    {
                        //发送查询命令
                        SetReadByteList();
                        Tcpsocket.Send(readbytelist.ToArray());
                        string SenDLog = "";
                        foreach (byte item in readbytelist)
                        {
                            SenDLog += ((int)item).ToString("X") + " ";
                        }
                        LogHelper.WriteSendAGVMessLog("发送" + this.DeviceID.ToString() + "查询命令:" + SenDLog);
                        //发送查寄存器通道命令
                        //Thread.Sleep(100);
                        //GetCallBack();
                        Thread.Sleep(300);

                        //Tcpsocket.Send(readrebytelist.ToArray());
                        //Thread.Sleep(400);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
                Clear();
            }
        }

        /// <summary>
        /// 设置读list
        /// </summary>
        public void SetReadByteList()
        {
            try
            {
                readbytelist.Clear();
                readbytelist.Add(0x55);
                readbytelist.Add((byte)this.DeviceID);
                readbytelist.AddRange(BitConverter.GetBytes(NextDataIndex()));
                readbytelist.Add(0x02);
                readbytelist.AddRange(GetSpaceBytes(19));
                readbytelist.Add(GetCheckByte(readbytelist));
                readbytelist.Add(0xaa);
            }
            catch (Exception ex)
            { throw ex; }
        }

        int count = 0;
        /// <summary>
        /// 接受小车反馈命令
        /// </summary>
        public void ReceverMes()
        {
            try
            {
                while (true)
                {
                    if (QueueCommand.Count <= 0 && Tcpsocket != null && Tcpsocket.Available > 0)
                    {
                        DateTime dtbegin = DateTime.Now;
                        GetCallBack();
                        DateTime dtend = DateTime.Now;
                        LogHelper.WriteLog(count.ToString() + "次" + this.DeviceID.ToString() + "号AGV解码耗时:" + (dtend - dtbegin).TotalSeconds.ToString() + ":" + (dtend - dtbegin).TotalMilliseconds.ToString());

                    }
                    else
                    { Thread.Sleep(10); }
                }
            }
            catch (Exception ex)
            {
                Clear();
            }
        }

        /// <summary>
        /// 清理通讯
        /// </summary>
        private bool Clear()
        {
            try
            {
                if (Tcpsocket != null)
                {
                    Tcpsocket.Shutdown(SocketShutdown.Both);
                    Tcpsocket.Close();
                    Tcpsocket.Dispose();
                }
                if (processor != null)
                {
                    processor.Abort();
                }
                if (RecevProvessor != null)
                { RecevProvessor.Abort(); }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 通讯状态观察
        /// </summary>
        private void CommunicationObser(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                communicationobserve_timer.Enabled = false;
                //接收超时,进行重新连接
                if (LastRecLong>2&& LastConnectLong >4)
                {
                    LogHelper.WriteLog("重连小车" + DeviceID.ToString());
                    ReConnect();
                }

                if (LastRecLong > 2)
                {
                    //通知调度程序  小车已经掉线
                    CarBaseStateInfo car = new CarBaseStateInfo();
                    car.AgvID = this.DeviceID;
                    DelegateState.InvokeDispatchStateEvent(this.DeviceID.ToString() + "车，已经掉线，将在1秒后重新尝试连接...");
                    //通知调度程序  小车已经掉线
                    car.bIsCommBreak = true;
                    DelegateState.InvokeCarFeedbackEvent(car);
                }
            }
            catch (Exception ex)
            {
                DelegateState.InvokeDispatchStateEvent(this.DeviceID.ToString() + "车，观察线程异常");
                LogHelper.WriteErrorLog(ex);
            }
            finally
            {
                if (!IsStop)
                { communicationobserve_timer.Enabled = true; }
            }
        }

        /// <summary>
        /// 重新创建连接
        /// </summary>
        public void ReConnect()
        {
            try
            {
                if (Init())
                {
                    CarBaseStateInfo car = new CarBaseStateInfo();
                    car.AgvID = this.DeviceID;
                    LastConnectTime = DateTime.Now;
                    if (ReStart())
                    {
                        LastConnectTime = DateTime.Now;
                        //car.bIsCommBreak = false;
                    }
                    else
                    {
                        DelegateState.InvokeDispatchStateEvent("尝试连接" + this.DeviceID.ToString() + "车失败，将在1秒后重新尝试连接...");
                        car.bIsCommBreak = true;
                        DelegateState.InvokeCarFeedbackEvent(car);
                    }

                }
                else
                {
                    DelegateState.InvokeDispatchStateEvent("尝试连接" + this.DeviceID.ToString() + "车失败，将在1秒后重新尝试连接...");
                    CarBaseStateInfo car = new CarBaseStateInfo();
                    car.AgvID = DeviceID;
                    car.bIsCommBreak = true;
                    DelegateState.InvokeCarFeedbackEvent(car);
                }
            }
            catch (Exception ex)
            {
                DelegateState.InvokeDispatchStateEvent("尝试连接" + this.DeviceID.ToString() + "车异常，将在3秒后重新尝试连接...");
                CarBaseStateInfo car = new CarBaseStateInfo();
                car.AgvID = DeviceID;
                car.bIsCommBreak = true;
                DelegateState.InvokeCarFeedbackEvent(car);
                LogHelper.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 重新启动
        /// </summary>
        public bool ReStart()
        {
            try
            {
                KeepServer = true;

                IPAddress ip = IPAddress.Parse(ComPara.ServerIP);
                IPEndPoint ipep = new IPEndPoint(ip, ComPara.Port);//IP和端口

                Tcpsocket.Connect(new IPEndPoint(ip, ComPara.Port));

                processor = new Thread(Communication);
                processor.IsBackground = true;
                processor.Start();

                RecevProvessor = new Thread(ReceverMes);
                RecevProvessor.IsBackground = true;
                RecevProvessor.Start();
                return true;
            }
            catch (Exception ex)
            {
                KeepServer = false;
                CarBaseStateInfo car = new CarBaseStateInfo();
                car.AgvID = DeviceID;
                car.bIsCommBreak = true;
                DelegateState.InvokeCarFeedbackEvent(car);
                LogHelper.WriteErrorLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 解析执行AGV指令
        /// </summary>
        public bool ExeCommand(CommandToValue ctov)
        {
            try
            {
                string SenDLog = "";
                //查询
                if (ctov.Command == AGVCommandEnum.Start)//启动
                {
                    writebytelist.Clear();
                    #region RTU
                    writebytelist.Add(0x55);
                    writebytelist.Add((byte)this.DeviceID);
                    writebytelist.AddRange(BitConverter.GetBytes(NextDataIndex()));
                    writebytelist.Add(0x05);
                    writebytelist.Add((byte)0);
                    //writebytelist.Add((byte)(int.Parse(ctov.CommandValue.ToString())));
                    writebytelist.AddRange(GetSpaceBytes(18));
                    writebytelist.Add(GetCheckByte(writebytelist));
                    writebytelist.Add(0xaa);
                    #endregion
                    //Thread.Sleep(100);
                    Tcpsocket.Send(writebytelist.ToArray());
                    foreach (byte item in writebytelist)
                    {
                        SenDLog += ((int)item).ToString("X") + " ";
                    }
                    LogHelper.WriteSendAGVMessLog("发送启动AGV" + this.DeviceID.ToString() + "命令" + ":" + SenDLog);
                    var t = GetCallBack();
                    return t;
                }

                if (ctov.Command == AGVCommandEnum.Stop)//停止,暂时停车，不清除路线
                {
                    writebytelist.Clear();
                    #region RTU
                    writebytelist.Add(0x55);
                    writebytelist.Add((byte)this.DeviceID);
                    writebytelist.AddRange(BitConverter.GetBytes(NextDataIndex()));
                    writebytelist.Add(0x06);
                    writebytelist.Add((byte)0);
                    writebytelist.AddRange(GetSpaceBytes(18));
                    writebytelist.Add(GetCheckByte(writebytelist));
                    writebytelist.Add(0xaa);
                    #endregion
                    //Thread.Sleep(100);
                    Tcpsocket.Send(writebytelist.ToArray());
                    foreach (byte item in writebytelist)
                    {
                        SenDLog += ((int)item).ToString("X") + " ";
                    }
                    LogHelper.WriteSendAGVMessLog("发送停止AGV" + this.DeviceID.ToString() + "命令" + ":" + SenDLog);
                    var t = GetCallBack();
                    return t;

                }
                if (ctov.Command == AGVCommandEnum.ChangeRoute)//站点缓存
                {
                    #region RTU
                    LogHelper.WriteLog("开始发线路并启动");
                    List<CommandLandMark> CommandLandmarkList = ctov.CommandValue as List<CommandLandMark>;
                    for (int i = 0; i < CommandLandmarkList.Count; i++)
                    {
                        CommandLandMark cmdlm = CommandLandmarkList[i];
                        SenDLog = "";
                        writebytelist.Clear();
                        writebytelist.Add(0x55);
                        writebytelist.Add((byte)this.DeviceID);
                        writebytelist.AddRange(BitConverter.GetBytes(NextDataIndex()));
                        writebytelist.Add(0x10);
                        writebytelist.Add(0x00);
                        byte[] barr = BitConverter.GetBytes(short.Parse(cmdlm.LandmarkCode));
                        writebytelist.Add(barr[0]);
                        writebytelist.Add(barr[1]);
                        //判断启动方向
                        if (i == 0)
                        {
                            //先判断启动方向
                            if (cmdlm.Move_Direact == 0)
                            {
                                writebytelist.Add(0x08);
                                writebytelist.Add(0x00);
                            }
                            else if (cmdlm.Move_Direact == 1)
                            {
                                writebytelist.Add(0x08);
                                writebytelist.Add(0x01);
                            }
                            else if (cmdlm.Move_Direact == 2)
                            {
                                writebytelist.Add(0x08);
                                writebytelist.Add(0x03);
                            }
                            else
                            {
                                writebytelist.Add(0x08);
                                writebytelist.Add(0x02);
                            }

                            //一开始执行一下动作
                            if (cmdlm.ActionType == 1)
                            {
                                writebytelist.Add(0x0A);
                                writebytelist.Add(0x01);
                            }
                            else if (cmdlm.ActionType == 2)
                            {
                                writebytelist.Add(0x0A);
                                writebytelist.Add(0x00);
                            }
                            writebytelist.Add(0x0C);
                            writebytelist.Add(0x00);
                        }
                        //添加路径中间动作地标
                        if (i != 0 && 1 != CommandLandmarkList.Count - 1)
                        {
                            if (cmdlm.Move_Direact == 1)
                            {
                                writebytelist.Add(0x06);
                                writebytelist.Add(0x00);
                                writebytelist.Add(0xFF);
                                writebytelist.Add(0x02);
                                writebytelist.Add(0x08);
                                writebytelist.Add(0x01);
                            }
                            else if (cmdlm.Move_Direact == 0)
                            {
                                writebytelist.Add(0x06);
                                writebytelist.Add(0x00);
                                writebytelist.Add(0xFF);
                                writebytelist.Add(0x02);
                                writebytelist.Add(0x08);
                                writebytelist.Add(0x00);
                            }
                            else if (cmdlm.Move_Direact == 2)
                            {
                                writebytelist.Add(0x06);
                                writebytelist.Add(0x00);
                                writebytelist.Add(0xFF);
                                writebytelist.Add(0x02);
                                writebytelist.Add(0x08);
                                writebytelist.Add(0x03);
                            }
                            //else
                            //{
                            //    writebytelist.Add(0x06);
                            //    writebytelist.Add(0x00);
                            //    writebytelist.Add(0xFF);
                            //    writebytelist.Add(0x02);
                            //    writebytelist.Add(0x08);
                            //    writebytelist.Add(0x02);
                            //}
                        }

                        //计算地标拐弯方向
                        if (cmdlm.Turn_Direact == 0)//右转
                        {
                            writebytelist.Add(0x09);
                            writebytelist.Add(0x01);
                        }
                        else if (cmdlm.Turn_Direact == 1)//左转
                        {
                            writebytelist.Add(0x09);
                            writebytelist.Add(0x00);
                        }
                        if (cmdlm.Move_Speed != -1)
                        {
                            writebytelist.Add(0x01);
                            writebytelist.Add((byte)cmdlm.Move_Speed);
                        }
                        if (cmdlm.Avoidance != -1)
                        {
                            writebytelist.Add(0x0B);
                            writebytelist.Add((byte)cmdlm.Avoidance);
                        }

                        //处理最后一个地标动作
                        if (i == CommandLandmarkList.Count - 1)
                        {
                            if (cmdlm.IsSensorStop == 1)//需要传感器停车
                            {
                                writebytelist.Add(0x04);
                                writebytelist.Add((byte)Convert.ToInt32("10111111", 2));//一号IO未打开则切换到强制地标255虚拟地标上
                                writebytelist.Add(0x12);
                                writebytelist.Add((byte)255);
                                writebytelist.Add(0x04);
                                writebytelist.Add((byte)Convert.ToInt32("01111111", 2));
                                writebytelist.Add(0x05);
                                writebytelist.Add(0x00);
                                if (cmdlm.ActionType == 1)//取料
                                {
                                }
                                else if (cmdlm.ActionType == 2)//放料
                                {
                                }
                                else if (cmdlm.ActionType == 3)//自动充电
                                {
                                }
                                else
                                {
                                }
                            }
                            else//不需要传感器停车,即正常停车
                            {
                                writebytelist.Add(0x06);
                                writebytelist.Add(0x00);
                                writebytelist.Add(0xFF);
                                writebytelist.Add(0x02);
                                if (cmdlm.ActionType == 1)
                                {
                                    writebytelist.Add(0x0A);
                                    writebytelist.Add(0x01);
                                }
                                else if (cmdlm.ActionType == 2)
                                {
                                    writebytelist.Add(0x0A);
                                    writebytelist.Add(0x00);
                                }
                                else if (cmdlm.ActionType == 3)
                                {
                                    writebytelist.Add(0x0C);
                                    writebytelist.Add(0x01);
                                }
                            }
                        }

                        int SpaceCount = 24 - writebytelist.Count;
                        if (SpaceCount > 0)
                        { writebytelist.AddRange(GetSpaceBytes(SpaceCount)); }
                        writebytelist.Add(GetCheckByte(writebytelist));
                        writebytelist.Add(0xaa);
                        //Tcpsocket.Send(writebytelist.ToArray());
                        foreach (byte item in writebytelist)
                        {
                            SenDLog += ((int)item).ToString("X") + " ";
                        }
                        string routeLog = "无";
                        CarInfo carInfo = CoreData.CarList.FirstOrDefault(p => p.AgvID == this.DeviceID);
                        if (carInfo != null)
                        { routeLog = string.Join(",", carInfo.Route.Select(p => p.LandmarkCode + ":" + p.movedirectStr + "-" + p.swayStr+"-"+p.ExcuteSpeed.ToString())); }
                        LogHelper.WriteSendAGVMessLog("发送AGV" + this.DeviceID.ToString() + "路径地表信息" + ":" + routeLog);
                        LogHelper.WriteSendAGVMessLog("发送AGV" + this.DeviceID.ToString() + "路径导航命令" + ":" + SenDLog);
                        //var res = GetCallBack();
                        while (!SendSite(writebytelist))
                        {
                            Thread.Sleep(80);
                        }
                    }
                    LogHelper.WriteLog("开始发线路强制站点");
                    //最后执行一下强制站点
                    writebytelist.Clear();
                    writebytelist.Add(0x55);
                    writebytelist.Add((byte)this.DeviceID);
                    writebytelist.AddRange(BitConverter.GetBytes(NextDataIndex()));
                    writebytelist.Add(0x09);
                    byte[] barrr = BitConverter.GetBytes(short.Parse(CommandLandmarkList.First().LandmarkCode));
                    writebytelist.Add(barrr[0]);
                    writebytelist.Add(barrr[1]);
                    //writebytelist.AddRange(BitConverter.GetBytes(short.Parse(CommandLandmarkList.First().LandmarkCode)));
                    writebytelist.AddRange(GetSpaceBytes(17));
                    writebytelist.Add(GetCheckByte(writebytelist));
                    writebytelist.Add(0xaa);
                    //Tcpsocket.Send(writebytelist.ToArray());
                    SenDLog = "";
                    foreach (byte item in writebytelist)
                    {
                        SenDLog += ((int)item).ToString("X") + " ";
                    }
                    LogHelper.WriteSendAGVMessLog("发送强制站点任务AGV" + this.DeviceID.ToString() + "命令" + ":" + SenDLog);
                    #endregion
                    //确定握手

                    while (!SendSite(writebytelist))
                    {
                        Thread.Sleep(80);
                    }
                    CarInfo car = CoreData.CarList.FirstOrDefault(p => p.AgvID == this.DeviceID);
                    if (car != null)
                    { car.OldHistoryUpLandCode = car.HistoryUpLandCode; }
                    return true;
                    //var t = GetCallBack();
                    //if (t)
                    //{
                    //    CarInfo car = CoreData.CarList.FirstOrDefault(p => p.AgvID == this.DeviceID);
                    //    if (car != null)
                    //    { car.OldHistoryUpLandCode = car.HistoryUpLandCode; }
                    //}
                    //return t;
                }
                if (ctov.Command == AGVCommandEnum.RouteSiteSet)
                {
                    #region RTU
                    List<CommandLandMark> CommandLandmarkList = ctov.CommandValue as List<CommandLandMark>;
                    for (int i = 0; i < CommandLandmarkList.Count; i++)
                    {
                        CommandLandMark cmdlm = CommandLandmarkList[i];
                        SenDLog = "";
                        writebytelist.Clear();
                        writebytelist.Add(0x55);
                        writebytelist.Add((byte)this.DeviceID);
                        writebytelist.AddRange(BitConverter.GetBytes(NextDataIndex()));
                        writebytelist.Add(0x10);
                        writebytelist.Add(0x00);
                        byte[] barr = BitConverter.GetBytes(short.Parse(cmdlm.LandmarkCode));
                        writebytelist.Add(barr[0]);
                        writebytelist.Add(barr[1]);
                        //判断启动方向
                        if (i == 0)
                        {
                            ////先判断启动方向
                            //if (cmdlm.Move_Direact == 0)
                            //{
                            //    writebytelist.Add(0x08);
                            //    writebytelist.Add(0x00);
                            //}
                            //else if (cmdlm.Move_Direact == 1)
                            //{
                            //    writebytelist.Add(0x08);
                            //    writebytelist.Add(0x01);
                            //}
                            //else if (cmdlm.Move_Direact == 2)
                            //{
                            //    writebytelist.Add(0x08);
                            //    writebytelist.Add(0x03);
                            //}
                            //else
                            //{
                            //    writebytelist.Add(0x08);
                            //    writebytelist.Add(0x02);
                            //}

                            //一开始执行一下动作
                            if (cmdlm.ActionType == 1)
                            {
                                writebytelist.Add(0x0A);
                                writebytelist.Add(0x01);
                            }
                            else if (cmdlm.ActionType == 2)
                            {
                                writebytelist.Add(0x0A);
                                writebytelist.Add(0x00);
                            }
                            writebytelist.Add(0x0C);
                            writebytelist.Add(0x00);
                        }
                        //添加路径中间动作地标
                        if (i != 0 && 1 != CommandLandmarkList.Count - 1)
                        {
                            if (cmdlm.Move_Direact == 1)
                            {
                                writebytelist.Add(0x06);
                                writebytelist.Add(0x00);
                                writebytelist.Add(0xFF);
                                writebytelist.Add(0x02);
                                writebytelist.Add(0x08);
                                writebytelist.Add(0x01);
                            }
                            else if (cmdlm.Move_Direact == 0)
                            {
                                writebytelist.Add(0x06);
                                writebytelist.Add(0x00);
                                writebytelist.Add(0xFF);
                                writebytelist.Add(0x02);
                                writebytelist.Add(0x08);
                                writebytelist.Add(0x00);
                            }
                            else if (cmdlm.Move_Direact == 2)
                            {
                                writebytelist.Add(0x06);
                                writebytelist.Add(0x00);
                                writebytelist.Add(0xFF);
                                writebytelist.Add(0x02);
                                writebytelist.Add(0x08);
                                writebytelist.Add(0x03);
                            }
                            //else
                            //{
                            //    writebytelist.Add(0x06);
                            //    writebytelist.Add(0x00);
                            //    writebytelist.Add(0xFF);
                            //    writebytelist.Add(0x02);
                            //    writebytelist.Add(0x08);
                            //    writebytelist.Add(0x02);
                            //}
                        }

                        //计算地标拐弯方向
                        if (cmdlm.Turn_Direact == 0)//右转
                        {
                            writebytelist.Add(0x09);
                            writebytelist.Add(0x01);
                        }
                        else if (cmdlm.Turn_Direact == 1)//左转
                        {
                            writebytelist.Add(0x09);
                            writebytelist.Add(0x00);
                        }
                        if (cmdlm.Move_Speed != -1)
                        {
                            writebytelist.Add(0x01);
                            writebytelist.Add((byte)cmdlm.Move_Speed);
                        }
                        if (cmdlm.Avoidance != -1)
                        {
                            writebytelist.Add(0x0B);
                            writebytelist.Add((byte)cmdlm.Avoidance);
                        }

                        //处理最后一个地标动作
                        if (i == CommandLandmarkList.Count - 1)
                        {
                            if (cmdlm.IsSensorStop == 1)//需要传感器停车
                            {
                                writebytelist.Add(0x04);
                                writebytelist.Add((byte)Convert.ToInt32("10111111", 2));//一号IO未打开则切换到强制地标255虚拟地标上
                                writebytelist.Add(0x12);
                                writebytelist.Add((byte)255);
                                writebytelist.Add(0x04);
                                writebytelist.Add((byte)Convert.ToInt32("01111111", 2));
                                writebytelist.Add(0x05);
                                writebytelist.Add(0x00);
                                if (cmdlm.ActionType == 1)//取料
                                {
                                }
                                else if (cmdlm.ActionType == 2)//放料
                                {
                                }
                                else if (cmdlm.ActionType == 3)//自动充电
                                {
                                }
                                else
                                {
                                }
                            }
                            else//不需要传感器停车,即正常停车
                            {
                                writebytelist.Add(0x06);
                                writebytelist.Add(0x00);
                                writebytelist.Add(0xFF);
                                writebytelist.Add(0x02);
                                if (cmdlm.ActionType == 1)
                                {
                                    writebytelist.Add(0x0A);
                                    writebytelist.Add(0x01);
                                }
                                else if (cmdlm.ActionType == 2)
                                {
                                    writebytelist.Add(0x0A);
                                    writebytelist.Add(0x00);
                                }
                                else if (cmdlm.ActionType == 3)
                                {
                                    writebytelist.Add(0x0C);
                                    writebytelist.Add(0x01);
                                }
                            }
                        }

                        int SpaceCount = 24 - writebytelist.Count;
                        if (SpaceCount > 0)
                        { writebytelist.AddRange(GetSpaceBytes(SpaceCount)); }
                        writebytelist.Add(GetCheckByte(writebytelist));
                        writebytelist.Add(0xaa);
                        //Tcpsocket.Send(writebytelist.ToArray());
                        foreach (byte item in writebytelist)
                        {
                            SenDLog += ((int)item).ToString("X") + " ";
                        }
                        string routeLog = "无";
                        CarInfo car = CoreData.CarList.FirstOrDefault(p => p.AgvID == this.DeviceID);
                        if (car != null)
                        { routeLog = string.Join(",", car.Route.Select(p => p.LandmarkCode + ":" + p.movedirectStr + "-" + p.swayStr)); }
                        LogHelper.WriteSendAGVMessLog("发送AGV" + this.DeviceID.ToString() + "路径地表信息" + ":" + routeLog);
                        LogHelper.WriteSendAGVMessLog("发送AGV" + this.DeviceID.ToString() + "路径导航命令" + ":" + SenDLog);
                        //var res = GetCallBack();
                        while (!SendSite(writebytelist))
                        {
                            Thread.Sleep(80);
                        }
                    }
                    return true;
                    #endregion
                }
                if (ctov.Command == AGVCommandEnum.CancelTast)//取消当前任务，停车
                {
                    writebytelist.Clear();
                    #region RTU
                    writebytelist.Add(0x55);
                    writebytelist.Add((byte)this.DeviceID);
                    writebytelist.AddRange(BitConverter.GetBytes(NextDataIndex()));
                    writebytelist.Add(0x10);
                    writebytelist.Add(0x02);
                    writebytelist.AddRange(GetSpaceBytes(18));
                    writebytelist.Add(GetCheckByte(writebytelist));
                    writebytelist.Add(0xaa);
                    //Tcpsocket.Send(writebytelist.ToArray());
                    foreach (byte item in writebytelist)
                    {
                        SenDLog += ((int)item).ToString("X") + " ";
                    }
                    LogHelper.WriteSendAGVMessLog("发送取消任务AGV" + this.DeviceID.ToString() + "命令" + ":" + SenDLog);
                    #endregion
                    //确定握手
                    while (!SendSite(writebytelist))
                    {
                        Thread.Sleep(80);
                    }
                    return true;
                }
                if (ctov.Command == AGVCommandEnum.LogicSiteSet)//强制站点
                {
                    writebytelist.Clear();
                    writebytelist.Add(0x55);
                    writebytelist.Add((byte)this.DeviceID);
                    writebytelist.AddRange(BitConverter.GetBytes(NextDataIndex()));
                    writebytelist.Add(0x09);
                    writebytelist.AddRange(BitConverter.GetBytes(short.Parse(ctov.CommandValue.ToString())));
                    writebytelist.AddRange(GetSpaceBytes(17));
                    writebytelist.Add(GetCheckByte(writebytelist));
                    writebytelist.Add(0xaa);
                    //Tcpsocket.Send(writebytelist.ToArray());
                    foreach (byte item in writebytelist)
                    {
                        SenDLog += ((int)item).ToString("X") + " ";
                    }
                    LogHelper.WriteSendAGVMessLog("发送强制站点任务AGV" + this.DeviceID.ToString() + "命令" + ":" + SenDLog);

                    //确定握手
                    while (!SendSite(writebytelist))
                    {
                        Thread.Sleep(80);
                    }
                    return true;
                }
                if (ctov.Command == AGVCommandEnum.SpeedSet)//速度设置
                {
                    return true;
                }
                if (ctov.Command == AGVCommandEnum.SetPBS)//设置避障
                {
                    return true;
                }
                if (ctov.Command == AGVCommandEnum.WriteRegister1)//写寄存器通道   1表示下降2表示上升
                {
                    List<CommandForkliftEnum> CommandForkliftList = ctov.CommandValue as List<CommandForkliftEnum>;  //叉举动作触发 1降 2升 ;叉举高度;
                    writebytelist.Clear();
                    writebytelist.Add(0x55);
                    writebytelist.Add((byte)this.DeviceID);
                    writebytelist.AddRange(BitConverter.GetBytes(NextDataIndex()));
                    writebytelist.Add(0x13);
                    writebytelist.Add(0x00);//起始位
                    writebytelist.Add(0x02);//数量
                    foreach (CommandForkliftEnum cmdcc in CommandForkliftList)
                    {
                        byte[] barr = BitConverter.GetBytes(short.Parse(cmdcc.ForkAction.ToString()));
                        writebytelist.Add(barr[0]);
                        writebytelist.Add(barr[1]);
                        byte[] barheight = BitConverter.GetBytes(cmdcc.height);
                        writebytelist.Add(barheight[0]);
                        writebytelist.Add(barheight[1]);
                    }
                    writebytelist.AddRange(GetSpaceBytes(13));
                    writebytelist.Add(GetCheckByte(writebytelist));
                    writebytelist.Add(0xaa);
                    //Tcpsocket.Send(writebytelist.ToArray());
                    foreach (byte item in writebytelist)
                    {
                        SenDLog += ((int)item).ToString("X") + " ";
                    }
                    LogHelper.WriteSendAGVMessLog("发送写寄存器通道1" + this.DeviceID.ToString() + "命令" + ":" + SenDLog);
                    while (!SendSite(writebytelist))
                    {
                        Thread.Sleep(80);
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("发送AGV命令异常:" + ex.Message);
                return false;
            }
        }

        public bool SendSite(List<byte> commd)
        {
            try
            {
                LogHelper.WriteLog("发送站点缓存指令!");
                Tcpsocket.Send(commd.ToArray());
                Thread.Sleep(60);
                int alllengh = 26;
                int offlinecount = 0;
                int receivedlengh = 0;
                byte[] buffermsg = new byte[26];
                while (alllengh - receivedlengh > 0)
                {
                    byte[] buffertemp = new byte[alllengh - receivedlengh];
                    int lengh = 0;
                    if (Tcpsocket.Available > 0)
                    { lengh = Tcpsocket.Receive(buffertemp); }
                    if (lengh <= 0)
                    {
                        if (offlinecount == 3)
                        {
                            throw new Exception("Socket  错误！");
                        }
                        offlinecount += 1;
                        Thread.Sleep(30);
                    }
                    Buffer.BlockCopy(buffertemp, 0, buffermsg, receivedlengh, lengh);
                    receivedlengh += lengh;
                }
                LastRecTime = DateTime.Now;
                LogHelper.WriteLog("接受完发送站点缓存指令!");
                //校验
                var r = GetCheckByte(buffermsg.Take(24).ToList());
                LogHelper.WriteLog("接受完发送站点缓存指令并验证!");
                LogHelper.WriteLog(r.ToString() + "|" + buffermsg[24].ToString());
                if (buffermsg[24] != r)
                {
                    LogHelper.WriteLog("发送指令验证失败!");
                    return false;
                }
                LogHelper.WriteLog("发送指令验证成功!");
                string SenDLog = "";
                foreach (byte item in buffermsg)
                { SenDLog += ((int)item).ToString("X") + " "; }
                LogHelper.WriteReciveAGVMessLog("接受的小车" + this.DeviceID.ToString() + "反馈命令:" + SenDLog);
                if (int.Parse(Convert.ToString(buffermsg[6], 10)) != 0)//指令执行失败
                {
                    LogHelper.WriteLog("发送指令功能码:" + Convert.ToInt16(buffermsg[6]).ToString() + "不正确失败!");
                    return false;
                }
                LastRecTime = DateTime.Now;
                return true;
            }
            catch (Exception ex)
            {
                Clear();
                LogHelper.WriteErrorLog(ex);
                return false;
            }
        }

        private static object LockReceive =new object();
        /// <summary>
        /// 命令反馈
        /// </summary>
        public bool GetCallBack()
        {

            try
            {
                //lock(LockReceive)
                //{
                    int alllengh = 26;
                    int offlinecount = 0;
                    int receivedlengh = 0;
                    byte[] buffermsg = new byte[26];
                    while (alllengh - receivedlengh > 0)
                    {
                        byte[] buffertemp = new byte[alllengh - receivedlengh];
                        int lengh = 0;
                        if (Tcpsocket.Available > 0)
                        { lengh = Tcpsocket.Receive(buffertemp); }
                        if (lengh <= 0)
                        {
                            if (offlinecount == 3)
                            {
                                throw new Exception("Socket  错误！");
                            }
                            offlinecount += 1;
                            Thread.Sleep(30);
                        }
                        Buffer.BlockCopy(buffertemp, 0, buffermsg, receivedlengh, lengh);
                        receivedlengh += lengh;
                    }
                    //LastRecTime = DateTime.Now;
                    //校验
                    var r = GetCheckByte(buffermsg.Take(24).ToList());
                    if (buffermsg[24] != r)
                    {
                        return false;
                    }
                    string SenDLog = "";
                    foreach (byte item in buffermsg)
                    { SenDLog += ((int)item).ToString("X") + " "; }
                    LogHelper.WriteReciveAGVMessLog("接受的小车" + this.DeviceID.ToString() + "反馈命令:" + SenDLog);
                    //LastRecTime = DateTime.Now;
                    CarInfo car = new CarInfo();
                    if (buffermsg[4] == 0xFF)
                    {
                        CarInfo ReceiveCar = CoreData.CarList.FirstOrDefault(p => p.AgvID == this.DeviceID);
                        if (ReceiveCar != null)
                        {
                            //car = DataToObject.CreateDeepCopy<CarInfo>(ReceiveCar);
                            ReceiveCar.bIsCommBreak = false;
                            //DelegateState.InvokeCarFeedbackEvent(car);
                        }
                        if (writebytelist.Count >= 4)
                        {
                            if (buffermsg[2] == writebytelist[2] && buffermsg[3] == writebytelist[3])
                            {
                                LastRecTime = DateTime.Now;
                                return true;
                            }
                        }

                    }
                    else if (buffermsg[4] == 0x82)
                    {
                        //转换成小车实体信息

                        car.AgvID = this.DeviceID;
                        car.CurrSite = Convert.ToInt32(Convert.ToString(buffermsg[7], 2).PadLeft(8, '0') + Convert.ToString(buffermsg[6], 2).PadLeft(8, '0'), 2);
                        car.CarState = int.Parse(Convert.ToString(buffermsg[5], 10));
                        car.Rundistance = Convert.ToDouble(Convert.ToString(buffermsg[8], 2).PadLeft(8, '0') + Convert.ToString(buffermsg[9], 2).PadLeft(8, '0') + Convert.ToString(buffermsg[10], 2).PadLeft(8, '0') + Convert.ToString(buffermsg[11], 2).PadLeft(8, '0'));
                        car.LogicSite = Convert.ToInt32(Convert.ToString(buffermsg[13], 2).PadLeft(8, '0') + Convert.ToString(buffermsg[12], 2).PadLeft(8, '0'), 2);
                        car.speed = int.Parse(Convert.ToString(buffermsg[15], 10));
                        car.bIsCommBreak = false;
                        //解析行走方向
                        string s = Convert.ToString(buffermsg[16], 2).PadLeft(8, '0');
                        if (s.Substring(0, 1) == "1")
                        { car.Move_Derict = 1; }
                        else
                        { car.Move_Derict = 0; }
                        if (s.Substring(1, 1) == "1")
                        { car.Turn_Derict = 0; }
                        else
                        { car.Turn_Derict = 1; }
                        //car.BangState = int.Parse(Convert.ToString(buffermsg[17], 10));
                        car.fVolt = double.Parse(Convert.ToInt32(Convert.ToString(buffermsg[20], 2), 2).ToString() + "." + Convert.ToInt32(Convert.ToString(buffermsg[21], 2), 2).ToString());

                        //解析异常报警
                        s = Convert.ToString(buffermsg[14], 2).PadLeft(8, '0');
                        car.ErrorMessage = "";
                        if (s.Substring(0, 1) == "1")
                        { car.ErrorMessage += "停车状态下脱线\r\n"; }
                        if (s.Substring(1, 1) == "1")
                        { car.ErrorMessage += "低电量\r\n"; }
                        if (s.Substring(2, 1) == "1")
                        { car.ErrorMessage += "接近障碍物\r\n"; }
                        if (s.Substring(3, 1) == "1")
                        { car.ErrorMessage += "发现障碍物\r\n"; }
                        if (s.Substring(4, 1) == "1")
                        { car.ErrorMessage += "光电传感器故障\r\n"; }
                        if (!string.IsNullOrEmpty(car.ErrorMessage))
                        {
                            LogHelper.WriteAGVWarnMessLog(car.AgvID.ToString() + "号AGV报警异常信息:" + car.ErrorMessage);
                        }
                        DelegateState.InvokeCarFeedbackEvent(car);
                        LastRecTime = DateTime.Now;
                        return true;
                    }//end Query
                    else if (buffermsg[4] == 0x94)
                    {
                        car.BangState = int.Parse(Convert.ToString(buffermsg[7], 10));
                        car.bIsCommBreak = false;
                        DelegateState.InvokeCarFeedbackEvent(car);
                        LastRecTime = DateTime.Now;
                        return true;
                    }
                    else
                    { return false; }
                //}
            }
            catch (Exception ex)
            { LogHelper.WriteLog("AGV解析编解码错误!" + ex.Message); }
            return false;
        }

        #endregion

        #region 自定义方法
        /// <summary>
        /// 获得校验位
        /// </summary>
        /// <param name="byelist"></param>
        /// <returns></returns>
        public byte GetCheckByte(List<byte> byelist)
        {
            int r = 0;
            for (int i = 1; i < 24; i++)
            {
                r += int.Parse(Convert.ToString(byelist[i], 10));
            }

            r = 256 - (r % 256);
            var b = (byte)r;
            return b;
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="byelist"></param>
        /// <returns></returns>
        public bool SendData(List<byte> bytelist)
        {
            try
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="byelist"></param>
        /// <returns></returns>
        public bool SendData(Socket socket, List<byte> byelist)
        {
            try
            {
                DateTime sendtime = DateTime.Now;
                socket.Send(byelist.ToArray());
                var r = GetCallBack();
                LogHelper.WriteLog(this.DeviceID.ToString() + "车 ，   " + (DateTime.Now - sendtime).TotalMilliseconds.ToString() + "    结果：" + r.ToString());
                return r;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {
            try
            {
                IsStop = true;
                this.communicationobserve_timer.Enabled = false;
                KeepServer = false;
                Tcpsocket.Close();
                Tcpsocket.Dispose();
                if (processor != null)
                {
                    processor.Abort();
                }
                //if (RecevProvessor != null)
                //{ RecevProvessor.Abort(); }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            try
            {
                //先执行ping 命令

                KeepServer = true;
                IPAddress ip = IPAddress.Parse(ComPara.ServerIP);
                IPEndPoint ipep = new IPEndPoint(ip, ComPara.Port);//IP和端口

                Tcpsocket.Connect(new IPEndPoint(ip, ComPara.Port));

                processor = new Thread(Communication);
                processor.IsBackground = true;
                processor.Start();

                RecevProvessor = new Thread(ReceverMes);
                RecevProvessor.IsBackground = true;
                RecevProvessor.Start();
                return true;
            }
            catch (Exception ex)
            {
                KeepServer = false;
                CarBaseStateInfo car = new CarBaseStateInfo();
                car.AgvID = this.DeviceID;
                car.bIsCommBreak = true;
                DelegateState.InvokeCarFeedbackEvent(car);
                LogHelper.WriteErrorLog(ex);
                return false;
            }
            finally
            {
                this.communicationobserve_timer.Enabled = true;
            }
        }

        /// <summary>
        /// 获得空填充的字节
        /// </summary>
        /// <param name="bytecount"></param>
        /// <returns></returns>
        public byte[] GetSpaceBytes(int bytecount)
        {
            byte[] spacebytes = new byte[bytecount];
            for (int i = 0; i < bytecount; i++)
            {
                spacebytes[i] = 0x00;
            }
            return spacebytes;
        }
        #endregion
    }
}
