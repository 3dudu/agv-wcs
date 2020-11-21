using AGVCore;
using DipatchModel;
using Model.CarInfoExtend;
using Model.MDM;
using System;
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
    public class AGVSession_OMARK : AGVSessionBase
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
        /// 写list
        /// </summary>
        List<byte> writebytelist = new List<byte>();
        #endregion

        #region 方法

        public AGVSession_OMARK()
        {
            communicationobserve_timer.Elapsed += new System.Timers.ElapsedEventHandler(CommunicationObser);
            connect = ConnectSocket;
            SocketSend = SendData;
        }

        public AGVSession_OMARK(int agvid, AGVComPara ComPara) : base()
        {
            try
            {
                this.DeviceID = agvid;
                this.DeviceType = 0;
                this.DeviceName = "AGV小车";
                this.ComPara = ComPara;
                communicationobserve_timer.Elapsed += new System.Timers.ElapsedEventHandler(CommunicationObser);
                connect = ConnectSocket;
                SocketSend = SendData;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
            }
        }

        public bool Init()
        {
            try
            {

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
                    LogHelper.WriteSendAGVMessLog($"{this.DeviceID}号AGV 当前线程{Thread.CurrentThread.ManagedThreadId}");
                    ///因为考虑到发送指令判断返回状态,需要在发送指令前将读取指令
                    /// 返回的状态指令消耗调，所以应该判断一下缓存中是否有指令
                    if (QueueCommand.Count > 0 && Tcpsocket.Available > 0)
                    {
                        ///如果缓存区有数据应该将数据消耗完
                        /// 否则再发送执行指令返回的指令则不知道是执行指令
                        /// 返回的还是发送读取指令返回的
                        GetCallBack();
                    }
                    else if (QueueCommand.Count > 0 && QueueCommand.TryPeek(out ctov))//有动作命令
                    {
                        while ((!ExeCommand(ctov)))
                        {
                            Thread.Sleep(200);
                        }
                        QueueCommand.TryDequeue(out ctov);
                    }
                    else
                    {
                        //查询心跳指令告知在地标上的信息
                        SetBitComand();
                        //发送查询命令
                        string SenDLog = "";
                        foreach (byte item in readbytelist)
                        { SenDLog += ((int)item).ToString("X") + " "; }
                        byte[] arr = new byte[] { readbytelist[4], readbytelist[3] };
                        Int16 SendPackIndex = BitConverter.ToInt16(arr, 0);
                        LogHelper.WriteSendAGVMessLog("报文序号:" + SendPackIndex.ToString() + "发送--心跳--AGV" + this.DeviceID.ToString() + "命令" + ":" + SenDLog);
                        while (!SendExeCommand(readbytelist))
                        {
                            LogHelper.WriteSendAGVMessLog("报文序号:" + SendPackIndex.ToString() + "发送--心跳--AGV" + this.DeviceID.ToString()+"未成功，等待200ms再发送");
                            Thread.Sleep(200);
                        }
                        Thread.Sleep(100);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteSendAGVMessLog($"{this.DeviceID}号AGV  Communication 异常:" +ex.Message);
                //Clear();
                //ReConnect();
            }
        }

        /// <summary>
        //设置心跳命令
        /// </summary>
        private void SetBitComand()
        {
            try
            {
                readbytelist.Clear();
                readbytelist.AddRange(new byte[] { 0xEB, 0x90, 0XDD });
                readbytelist.AddRange(BitConverter.GetBytes(NextDataIndex()));
                readbytelist.Add(0x00);
                readbytelist.Add(0x01);
                CarInfo car = CoreData.CarList.FirstOrDefault(p => p.AgvID == this.DeviceID);
                if (car != null && car.CarState == 5 && car.IsUpQCode == 1)
                { readbytelist.Add(0x01); }
                else
                { readbytelist.Add(0x00); }
                byte crc8 = CRC.CRC8(readbytelist.ToArray());
                readbytelist.Add(crc8);
            }
            catch (Exception ex)
            { throw ex; }
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
                    //解决线程过多情况,因为线程执行Abort，状态是AbortRequested,还是会存在继续执行
                    while (processor.ThreadState != ThreadState.Aborted)
                    { Thread.Sleep(100); }
                }
                return true;
            }
            catch (Exception ex)
            {
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
        /// 通讯状态观察
        /// </summary>
        private void CommunicationObser(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                communicationobserve_timer.Enabled = false;

                //先判断小车是否已经掉线
                if (LastRecLong > 1)
                {
                    //如果接受消息时间已经大于1秒,则认为车子掉线了。
                    //DelegateState.InvokeDispatchStateEvent(this.DeviceID.ToString() + "车，已经掉线，将在1秒后重新尝试连接...");
                    //通知调度程序  小车已经掉线
                    CarBaseStateInfo car = new CarBaseStateInfo();
                    car.bIsCommBreak = true;
                    car.AgvID = this.DeviceID;
                    DelegateState.InvokeCarFeedbackEvent(car);
                    if (LastConnectLong > 3)
                    {
                        //如果车子掉线且连接时间超过3秒则需要重连
                        LogHelper.WriteLog("重连小车" + DeviceID.ToString());
                        ReConnect();
                    }
                }
            }
            catch (Exception ex)
            {
                //DelegateState.InvokeDispatchStateEvent(this.DeviceID.ToString() + "车，观察线程异常");
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
                //LastConnectTime = DateTime.Now;
                if (Init())
                {
                    CarBaseStateInfo car = new CarBaseStateInfo();
                    car.AgvID = this.DeviceID;
                    if (ReStart())
                    {
                        LastConnectTime = DateTime.Now;
                    }
                    else
                    {
                        //DelegateState.InvokeDispatchStateEvent("尝试连接" + this.DeviceID.ToString() + "车失败，将在1秒后重新尝试连接...");
                        //car.bIsCommBreak = true;
                        //DelegateState.InvokeCarFeedbackEvent(car);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
                //DelegateState.InvokeDispatchStateEvent("尝试连接" + this.DeviceID.ToString() + "车异常，将在3秒后重新尝试连接...");
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
                return true;
            }
            catch (Exception ex)
            {
                KeepServer = false;
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
                    writebytelist.AddRange(new byte[] { 0xEB, 0x90, 0xDF });
                    writebytelist.AddRange(BitConverter.GetBytes(NextDataIndex()));
                    writebytelist.AddRange(new byte[] { 0x00, 0x02, 0x04, 0x00 });
                    byte crc8 = CRC.CRC8(writebytelist.ToArray());
                    writebytelist.Add(crc8);
                    #endregion

                    foreach (byte item in writebytelist)
                    { SenDLog += ((int)item).ToString("X") + " "; }
                    byte[] arr = new byte[] { writebytelist[4], writebytelist[3] };
                    Int16 SendPackIndex = BitConverter.ToInt16(arr, 0);
                    LogHelper.WriteSendAGVMessLog("报文序号:" + SendPackIndex.ToString() + "发送启动AGV" + this.DeviceID.ToString() + "命令" + ":" + SenDLog);
                    //while (!SendExeCommand(writebytelist))
                    //{
                    //    Thread.Sleep(50);
                    //}
                    if (!SendExeCommand(writebytelist))
                    { return false; }
                    LogHelper.WriteSendAGVMessLog("报文序号:" + SendPackIndex.ToString() + "发送启动AGV" + this.DeviceID.ToString() + "命令" + ":" + SenDLog + "成功!");
                    return true;
                }

                else if (ctov.Command == AGVCommandEnum.Stop)//停止,暂时停车，不清除路线
                {
                    writebytelist.Clear();
                    #region RTU
                    writebytelist.AddRange(new byte[] { 0xEB, 0x90, 0xDF });
                    writebytelist.AddRange(BitConverter.GetBytes(NextDataIndex()));
                    writebytelist.AddRange(new byte[] { 0x00, 0x02, 0x03, 0x00 });
                    byte crc8 = CRC.CRC8(writebytelist.ToArray());
                    writebytelist.Add(crc8);
                    #endregion

                    foreach (byte item in writebytelist)
                    { SenDLog += ((int)item).ToString("X") + " "; }
                    byte[] arr = new byte[] { writebytelist[4], writebytelist[3] };
                    Int16 SendPackIndex = BitConverter.ToInt16(arr, 0);
                    LogHelper.WriteSendAGVMessLog("报文序号:" + SendPackIndex.ToString() + "发送停止AGV" + this.DeviceID.ToString() + "命令" + ":" + SenDLog);
                    //while (!SendExeCommand(writebytelist))
                    //{
                    //    Thread.Sleep(50);
                    //}
                    if (!SendExeCommand(writebytelist))
                    { return false; }
                    LogHelper.WriteSendAGVMessLog("报文序号:" + SendPackIndex.ToString() + "发送停止AGV" + this.DeviceID.ToString() + "命令" + ":" + SenDLog + "成功!");
                    return true;
                }
                else if (ctov.Command == AGVCommandEnum.CancelTast)
                {
                    writebytelist.Clear();
                    #region RTU
                    writebytelist.AddRange(new byte[] { 0xEB, 0x90, 0xDF });
                    writebytelist.AddRange(BitConverter.GetBytes(NextDataIndex()));
                    writebytelist.AddRange(new byte[] { 0x00, 0x02, 0x05, 0x00 });
                    byte crc8 = CRC.CRC8(writebytelist.ToArray());
                    writebytelist.Add(crc8);
                    #endregion

                    foreach (byte item in writebytelist)
                    { SenDLog += ((int)item).ToString("X") + " "; }
                    byte[] arr = new byte[] { writebytelist[4], writebytelist[3] };
                    Int16 SendPackIndex = BitConverter.ToInt16(arr, 0);
                    LogHelper.WriteSendAGVMessLog("报文序号:" + SendPackIndex.ToString() + "发送复位AGV" + this.DeviceID.ToString() + "命令" + ":" + SenDLog);
                    //while (!SendExeCommand(writebytelist,true))
                    //{
                    //    Thread.Sleep(50);
                    //}
                    if (!SendExeCommand(writebytelist, true))
                    { return false; }
                    LogHelper.WriteSendAGVMessLog("报文序号:" + SendPackIndex.ToString() + "发送复位AGV" + this.DeviceID.ToString() + "命令" + ":" + SenDLog + "成功!");
                    return true;
                }
                else if (ctov.Command == AGVCommandEnum.ChangeRoute)//下发路径
                {
                    writebytelist.Clear();
                    #region RTU

                    List<CommandLandMark> CommandLandmarkList = ctov.CommandValue as List<CommandLandMark>;
                    int totalbytecount = CommandLandmarkList.Count * 15;
                    writebytelist.AddRange(new byte[] { 0xEB, 0x90, 0xDE });
                    writebytelist.AddRange(BitConverter.GetBytes(NextDataIndex()));
                    writebytelist.Add(BitConverter.GetBytes(totalbytecount)[1]);
                    writebytelist.Add(BitConverter.GetBytes(totalbytecount)[0]);

                    string LandAcotcinLog = "";
                    //循环添加功能地标信息
                    foreach (CommandLandMark cmdlm in CommandLandmarkList)
                    {
                        //组装导航路径中的节点坐标和角度
                        writebytelist.Add((byte)cmdlm.Avoidance);
                        byte[] Xbytes = CRC.ToByte(cmdlm.X);
                        writebytelist.AddRange(Xbytes.Reverse().ToArray());
                        byte[] Ybytes = CRC.ToByte(cmdlm.Y);
                        writebytelist.AddRange(Ybytes.Reverse().ToArray());
                        byte[] Wbytes = CRC.ToByte(cmdlm.Angle);
                        writebytelist.AddRange(Wbytes.Reverse().ToArray());
                        //组装导航信息中的最高移速
                        LandAcotcinLog += "地标:[" + cmdlm.LandmarkCode + "] X:[" + cmdlm.X.ToString() + "] Y:[" + cmdlm.Y.ToString() + "] 角度:[" + cmdlm.Angle.ToString() + "] ";
                        writebytelist.Add((byte)cmdlm.Move_Speed);
                        LandAcotcinLog += "速度:[" + cmdlm.Move_Speed.ToString() + "] 导航方式:[" + cmdlm.Avoidance.ToString() + "] ";
                        //组装节点中的动作
                        writebytelist.Add((byte)cmdlm.ActionType);
                        LandAcotcinLog += "动作:[" + cmdlm.ActionType.ToString() + "] ";
                    }
                    byte crc8 = CRC.CRC8(writebytelist.ToArray());
                    writebytelist.Add(crc8);

                    #endregion

                    foreach (byte item in writebytelist)
                    { SenDLog += ((int)item).ToString("X").PadLeft(2, '0') + " "; }
                    byte[] arr = new byte[] { writebytelist[4], writebytelist[3] };
                    Int16 SendPackIndex = BitConverter.ToInt16(arr, 0);
                    LogHelper.WriteSendAGVMessLog("发送线路及动作说明:" + LandAcotcinLog);
                    LogHelper.WriteSendAGVMessLog("报文序号:" + SendPackIndex.ToString() + "发送导航路径AGV" + this.DeviceID.ToString() + "命令" + ":" + SenDLog);
                    //while (!SendExeCommand(writebytelist))
                    //{
                    //    Thread.Sleep(50);
                    //}
                    if (!SendExeCommand(writebytelist))
                    { return false; }
                    LogHelper.WriteSendAGVMessLog("报文序号:" + SendPackIndex.ToString() + "发送导航路径AGV" + this.DeviceID.ToString() + "命令" + ":" + SenDLog + "成功!");
                    return true;
                }
                else if (ctov.Command == AGVCommandEnum.SpeedSet)//速度设置
                {
                    return true;
                }
                else if (ctov.Command == AGVCommandEnum.SetPBS)//设置避障
                {
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

        private bool SendExeCommand(List<byte> bytelist, bool IsCheckCarState = false)
        {
            try
            {
                Tcpsocket.Send(bytelist.ToArray());
                byte[] arr = new byte[] { bytelist[4], bytelist[3] };
                Int16 SendPackIndex = BitConverter.ToInt16(arr, 0);
                Thread.Sleep(200);
                return GetCallBack(SendPackIndex, IsCheckCarState);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(this.DeviceID.ToString() + "车 组装AGV执行命令异常:" + ex.Message);
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
                byte[] arr = new byte[] { byelist[4], byelist[3] };
                Int16 SendPackIndex = BitConverter.ToInt16(arr, 0);
                Thread.Sleep(50);
                var r = GetCallBack(SendPackIndex);
                LogHelper.WriteLog(this.DeviceID.ToString() + "车 ，   " + (DateTime.Now - sendtime).TotalMilliseconds.ToString() + "    结果：" + r.ToString());
                return r;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// SendPackIndex 为发送包序号
        /// 命令反馈
        /// </summary>
        public bool GetCallBack(Int16 SendPackIndex = 0, bool IsCheckCarState = false)
        {
            try
            {
                int AGVID = this.DeviceID;
                int offlinecount = 0;
                int allheadleftlengh = 7;
                int receivedlengh = 0;
                byte[] bufferhead = new byte[7];//定义6位长度为接收包头缓存长度
                while (allheadleftlengh - receivedlengh > 0)
                {
                    byte[] buffertemp = new byte[allheadleftlengh - receivedlengh];
                    if (Tcpsocket.Available <= 0)
                    { continue; }
                    int lengh = Tcpsocket.Receive(buffertemp);
                    if (lengh <= 0)
                    {
                        if (offlinecount == 3)
                        {
                            LogHelper.WriteReciveAGVMessLog("接受的小车" + AGVID.ToString() + "反馈命令超时");
                            return false;
                        }
                        offlinecount += 1;
                        Thread.Sleep(50);
                    }
                    Buffer.BlockCopy(buffertemp, 0, bufferhead, receivedlengh, lengh);
                    receivedlengh += lengh;
                }
                //解析AGV反馈信息
                if (bufferhead[0] == 0xEB && bufferhead[1] == 0x90 && bufferhead[2] == 0XD1)
                {
                    //判断一下报文序号
                    byte[] arr = new byte[] { bufferhead[4], bufferhead[3] };
                    Int16 RecvPackIndex = BitConverter.ToInt16(arr, 0);
                    if (SendPackIndex == 0 || (SendPackIndex != 0 && RecvPackIndex == SendPackIndex))//如果发送和接受报文序号一致,说明成功
                    {
                        offlinecount = 0;


                        byte[] bytess = new byte[2] { bufferhead[6], bufferhead[5] };
                        //int allcontentleftlengh = bufferhead[6] << 8 + bufferhead[5];
                        int allcontentleftlengh = BitConverter.ToInt16(bytess, 0) + 1;


                        receivedlengh = 0;
                        byte[] buffercontent = new byte[allcontentleftlengh];
                        while (allcontentleftlengh - receivedlengh > 0)
                        {
                            byte[] buffertemp = new byte[allcontentleftlengh - receivedlengh];
                            if (Tcpsocket.Available <= 0)
                            { continue; }
                            int lengh = Tcpsocket.Receive(buffertemp);
                            if (lengh <= 0)
                            {
                                if (offlinecount == 3)
                                {
                                    LogHelper.WriteReciveAGVMessLog("接受的小车" + AGVID.ToString() + "反馈命令超时");
                                    return false;
                                }
                                offlinecount += 1;
                                Thread.Sleep(50);
                            }
                            Buffer.BlockCopy(buffertemp, 0, buffercontent, receivedlengh, lengh);
                            receivedlengh += lengh;
                        }

                        //接受完内容后校验
                        List<byte> msg = new List<byte>();
                        msg.AddRange(bufferhead);
                        msg.AddRange(buffercontent);
                        string SenDLog = "";
                        foreach (byte item in msg)
                        { SenDLog += ((int)item).ToString("X") + " "; }
                        LogHelper.WriteReciveAGVMessLog("接受的小车" + AGVID.ToString() + "反馈命令:" + SenDLog);

                        byte CRC8 = CRC.CRC8(msg.Take(allcontentleftlengh + 6).ToArray());
                        if (CRC8 != msg[msg.Count - 1])
                        {
                            LogHelper.WriteReciveAGVMessLog("接受的小车" + AGVID.ToString() + "反馈命令校验位不正确");
                            return false;
                        }

                        //解析车辆信息
                        CarInfo car = new CarInfo();
                        car.AgvID = this.DeviceID;
                        car.CarState = Convert.ToInt32(msg[7]);
                        car.IsNeedFinshTask = Convert.ToInt32(msg[8]);
                        car.IsNeedRedoTask = Convert.ToInt32(msg[9]);
                        byte[] bytes = new byte[4] { msg[13], msg[12], msg[11], msg[10] };
                        car.X = BitConverter.ToInt32(bytes, 0) / 1000.00F;
                        bytes = new byte[4] { msg[17], msg[16], msg[15], msg[14] };
                        car.Y = BitConverter.ToInt32(bytes, 0) / 1000.00F;
                        bytes = new byte[4] { msg[21], msg[20], msg[19], msg[18] };
                        car.Angel = BitConverter.ToInt32(bytes, 0) / 1000.00F;
                        //是否在码上
                        car.IsUpLand = Convert.ToInt32(msg[22].ToString());
                        //升降平台的状态
                        car.BangState = Convert.ToInt32(msg[23].ToString());
                        //夹持机构状态
                        car.JCState = Convert.ToInt32(msg[24].ToString());
                        //剩余电池容量 百分比
                        car.fVolt = Convert.ToInt32(msg[25].ToString());
                        //速度
                        car.speed = Convert.ToInt32(msg[26].ToString());
                        //报警长度
                        int WarnLen = Convert.ToInt32(msg[27].ToString());
                        car.ErrorMessage = "";
                        if (WarnLen > 0)//有报警
                        {
                            int StartIndex = 27;
                            for (int i = 1; i <= WarnLen; i++)
                            {
                                int Cat = StartIndex + 1;
                                int Code = StartIndex + 2;
                                car.WarnType = Convert.ToInt32(msg[Cat].ToString());
                                car.WarnBinaryCode = Convert.ToString(msg[Code], 2).PadLeft(8, '0');
                                switch (car.WarnType)
                                {
                                    case 0:
                                        if (car.WarnBinaryCode.Substring(7, 1) == "1")
                                        { car.ErrorMessage += "24C02错误-写入错误!\r\n"; }
                                        if (car.WarnBinaryCode.Substring(6, 1) == "1")
                                        { car.ErrorMessage += "24C02错误-读取错误\r\n"; }
                                        break;
                                    case 1:
                                        if (car.WarnBinaryCode.Substring(7, 1) == "1")
                                        { car.ErrorMessage += "电量检测错误-通信错误\r\n"; }
                                        if (car.WarnBinaryCode.Substring(6, 1) == "1")
                                        { car.ErrorMessage += "电量检测错误-电量低\r\n"; }
                                        break;
                                    case 2:
                                        if (car.WarnBinaryCode.Substring(7, 1) == "1")
                                        { car.ErrorMessage += "WiFi通信错误-校验错误\r\n"; }
                                        if (car.WarnBinaryCode.Substring(6, 1) == "1")
                                        { car.ErrorMessage += "WiFi通信错误-心跳超时\r\n"; }
                                        break;
                                    case 3:
                                        if (car.WarnBinaryCode.Substring(7, 1) == "1")
                                        { car.ErrorMessage += "433M通信错误\r\n"; }
                                        if (car.WarnBinaryCode.Substring(6, 1) == "1")
                                        { car.ErrorMessage += "校验错误\r\n"; }
                                        break;
                                    case 4:
                                        if (car.WarnBinaryCode.Substring(7, 1) == "1")
                                        { car.ErrorMessage += "PGV100错误-校验错误1\r\n"; }
                                        if (car.WarnBinaryCode.Substring(6, 1) == "1")
                                        { car.ErrorMessage += "PGV100错误-通信超时1\r\n"; }
                                        if (car.WarnBinaryCode.Substring(5, 1) == "1")
                                        { car.ErrorMessage += "PGV100错误-硬件故障1\r\n"; }
                                        if (car.WarnBinaryCode.Substring(4, 1) == "1")
                                        { car.ErrorMessage += "PGV100错误-校验错误2\r\n"; }
                                        if (car.WarnBinaryCode.Substring(3, 1) == "1")
                                        { car.ErrorMessage += "PGV100错误-通信超时2\r\n"; }
                                        if (car.WarnBinaryCode.Substring(2, 1) == "1")
                                        { car.ErrorMessage += "PGV100错误-硬件故障2\r\n"; }
                                        break;
                                    case 5:
                                        if (car.WarnBinaryCode.Substring(7, 1) == "1")
                                        { car.ErrorMessage += "驱动器-1号通信异常\r\n"; }
                                        if (car.WarnBinaryCode.Substring(6, 1) == "1")
                                        { car.ErrorMessage += "驱动器-2号通信异常\r\n"; }
                                        if (car.WarnBinaryCode.Substring(5, 1) == "1")
                                        { car.ErrorMessage += "驱动器-3号通信异常\r\n"; }
                                        if (car.WarnBinaryCode.Substring(4, 1) == "1")
                                        { car.ErrorMessage += "驱动器-4号通信异常\r\n"; }
                                        if (car.WarnBinaryCode.Substring(3, 1) == "1")
                                        { car.ErrorMessage += "驱动器-1号硬件故障\r\n"; }
                                        if (car.WarnBinaryCode.Substring(2, 1) == "1")
                                        { car.ErrorMessage += "驱动器-2号硬件故障\r\n"; }
                                        if (car.WarnBinaryCode.Substring(1, 1) == "1")
                                        { car.ErrorMessage += "驱动器-3号硬件故障\r\n"; }
                                        if (car.WarnBinaryCode.Substring(0, 1) == "1")
                                        { car.ErrorMessage += "驱动器-3号硬件故障\r\n"; }
                                        break;
                                    case 6:
                                        if (car.WarnBinaryCode.Substring(7, 1) == "1")
                                        { car.ErrorMessage += "安全传感器-触边\r\n"; }
                                        if (car.WarnBinaryCode.Substring(6, 1) == "1")
                                        { car.ErrorMessage += "安全传感器-激光减速\r\n"; }
                                        if (car.WarnBinaryCode.Substring(5, 1) == "1")
                                        { car.ErrorMessage += "安全传感器-激光停车\r\n"; }
                                        if (car.WarnBinaryCode.Substring(4, 1) == "1")
                                        { car.ErrorMessage += "安全传感器-急停按钮\r\n"; }
                                        break;
                                    case 7:
                                        if (car.WarnBinaryCode.Substring(7, 1) == "1")
                                        { car.ErrorMessage += "SIM2000-校验错误\r\n"; }
                                        if (car.WarnBinaryCode.Substring(6, 1) == "1")
                                        { car.ErrorMessage += "SIM2000-通信超时\r\n"; }
                                        if (car.WarnBinaryCode.Substring(5, 1) == "1")
                                        { car.ErrorMessage += "SIM2000-硬件故障\r\n"; }
                                        break;
                                    case 8:
                                        if (car.WarnBinaryCode.Substring(7, 1) == "1")
                                        { car.ErrorMessage += "升降平台-心跳超时\r\n"; }
                                        if (car.WarnBinaryCode.Substring(6, 1) == "1")
                                        { car.ErrorMessage += "升降平台-动作超时\r\n"; }
                                        if (car.WarnBinaryCode.Substring(5, 1) == "1")
                                        { car.ErrorMessage += "升降平台-检测异常\r\n"; }
                                        if (car.WarnBinaryCode.Substring(4, 1) == "1")
                                        { car.ErrorMessage += "升降平台-驱动器报警\r\n"; }
                                        break;
                                    case 9:
                                        if (car.WarnBinaryCode.Substring(7, 1) == "1")
                                        { car.ErrorMessage += "夹持机构-心跳超时\r\n"; }
                                        if (car.WarnBinaryCode.Substring(6, 1) == "1")
                                        { car.ErrorMessage += "夹持机构-动作超时\r\n"; }
                                        if (car.WarnBinaryCode.Substring(5, 1) == "1")
                                        { car.ErrorMessage += "升降平台-检测异常\r\n"; }
                                        if (car.WarnBinaryCode.Substring(4, 1) == "1")
                                        { car.ErrorMessage += "升降平台-驱动器报警\r\n"; }
                                        break;
                                    case 10:
                                        if (car.WarnBinaryCode.Substring(7, 1) == "1")
                                        { car.ErrorMessage += "导航-目的坐标异常\r\n"; }
                                        if (car.WarnBinaryCode.Substring(6, 1) == "1")
                                        { car.ErrorMessage += "导航-地图匹配异常\r\n"; }
                                        if (car.WarnBinaryCode.Substring(5, 1) == "1")
                                        { car.ErrorMessage += "导航-定位标记匹配异常\r\n"; }
                                        if (car.WarnBinaryCode.Substring(4, 1) == "1")
                                        { car.ErrorMessage += "导航-磁条导航异常\r\n"; }
                                        break;
                                    case 11:
                                        if (car.WarnBinaryCode.Substring(7, 1) == "1")
                                        { car.ErrorMessage += "物料检测-物料检测异常\r\n"; }
                                        break;
                                    case 12:
                                        if (car.WarnBinaryCode.Substring(7, 1) == "1")
                                        { car.ErrorMessage += "消息超时A-BMS消息超时\r\n"; }
                                        if (car.WarnBinaryCode.Substring(6, 1) == "1")
                                        { car.ErrorMessage += "消息超时A-驱动器消息超时\r\n"; }
                                        if (car.WarnBinaryCode.Substring(5, 1) == "1")
                                        { car.ErrorMessage += "消息超时A-遥控器消息超时\r\n"; }
                                        if (car.WarnBinaryCode.Substring(4, 1) == "1")
                                        { car.ErrorMessage += "消息超时A-安全避障消息超时\r\n"; }
                                        if (car.WarnBinaryCode.Substring(3, 1) == "1")
                                        { car.ErrorMessage += "消息超时A-开关消息超时\r\n"; }
                                        if (car.WarnBinaryCode.Substring(2, 1) == "1")
                                        { car.ErrorMessage += "消息超时A-举升消息超时\r\n"; }
                                        if (car.WarnBinaryCode.Substring(1, 1) == "1")
                                        { car.ErrorMessage += "消息超时A-导航消息超时\r\n"; }
                                        if (car.WarnBinaryCode.Substring(0, 1) == "1")
                                        { car.ErrorMessage += "消息超时A-控速消息超时\r\n"; }
                                        break;
                                    case 13:
                                        if (car.WarnBinaryCode.Substring(7, 1) == "1")
                                        { car.ErrorMessage += "消息超时B-循迹消息超时\r\n"; }
                                        if (car.WarnBinaryCode.Substring(6, 1) == "1")
                                        { car.ErrorMessage += "消息超时B-磁导航消息超时\r\n"; }
                                        break;
                                    default:
                                        break;
                                }
                                StartIndex += 2;
                            }
                        }
                        if (!string.IsNullOrEmpty(car.ErrorMessage))
                        { LogHelper.WriteAGVWarnMessLog(car.AgvID.ToString() + "号AGV报警异常信息:" + car.ErrorMessage); }
                        LandmarkInfo CurrLand = CoreData.AllLands.FirstOrDefault(p => distance((float)p.LandX, (float)p.LandY, car.X, car.Y) <= 0.2);
                        if (CurrLand != null)
                        {
                            car.CurrSite = Convert.ToInt16(CurrLand.LandmarkCode);
                            LogHelper.WriteReciveAGVMessLog("小车" + AGVID.ToString() + "坐标得到的地标为:" + CurrLand.LandmarkCode + ",电量为:" + car.fVolt.ToString());
                            car.IsUpQCode = 1;
                        }
                        else
                        { car.IsUpQCode = 0; }
                        DelegateState.InvokeCarFeedbackEvent(car);
                        LastRecTime = DateTime.Now;
                        //发送复位需要判断车子状态是否为待命状态
                        if (IsCheckCarState)
                        {
                            LogHelper.WriteSendAGVMessLog("判断发送复位指令是否真正完成!");
                            if (car.CarState != 0)
                            {
                                LogHelper.WriteSendAGVMessLog("发送复位指令车子:" + car.AgvID.ToString() + "状态为:" + car.CarState.ToString());
                                return false;
                            }
                        }
                        return true;
                    }
                    else
                    { return false; }
                }
            }
            catch (Exception ex)
            { LogHelper.WriteLog(this.DeviceID.ToString() + "车 AGV解析编解码错误!" + ex.Message); }
            return false;
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
                    //解决线程过多情况,因为线程执行Abort，状态是AbortRequested,还是会存在继续执行
                    while (processor.ThreadState != ThreadState.Aborted)
                    { Thread.Sleep(100); }
                }
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
                return true;
            }
            catch (Exception ex)
            {
                KeepServer = false;
                return false;
            }
            finally
            {
                this.communicationobserve_timer.Enabled = true;
            }
        }
        #endregion

    }
}
