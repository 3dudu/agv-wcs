using AGVCore;
using DipatchModel;
using Model.MDM;
using Model.MSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tools;

namespace AGVCommunication
{
    public class ChargeSession : AGVSessionBase
    {
        #region 属性
        ///
        /// 客户端socket对象
        /// </summary>
        public Socket Tcpsocket;

        /// <summary>
        /// 服务线程(负责socket接收)
        /// </summary>
        private Thread processor;

        /// <summary>
        /// 查List
        /// </summary>
        List<byte> readbytelist = new List<byte>();

        /// <summary>
        /// 写list
        /// </summary>
        List<byte> writebytelist = new List<byte>();

        /// <summary>
        /// 通讯观察线程,每秒观察一次
        /// </summary>
        System.Timers.Timer communicationobserve_timer = new System.Timers.Timer(1000 * 1);
        #endregion

        #region 方法
        public ChargeSession()
        {
            communicationobserve_timer.AutoReset = true;
            communicationobserve_timer.Enabled = true;
            communicationobserve_timer.Elapsed += new System.Timers.ElapsedEventHandler(CommunicationObser);
        }


        /// <summary>
        /// 初始化
        /// </summary>
        public bool Init()
        {
            try
            {
                Clear();
                Tcpsocket = new Socket(AddressFamily.InterNetwork,
                 SocketType.Stream, ProtocolType.Tcp);
                Tcpsocket.ReceiveTimeout = 200;
                return true;
            }
            catch (Exception ex)
            {
                ChargeStationInfo io = new ChargeStationInfo();
                io.ID = this.DeviceID;
                io.IsCommBreak = true;
                DelegateState.InvokeChargeChangeEvent(io);
                return false;
            }
        }

        /// <summary>
        /// 启动
        /// </summary>
        public bool Start()
        {
            try
            {
                IPAddress ip = IPAddress.Parse(ComPara.ServerIP);
                IPEndPoint ipep = new IPEndPoint(ip, ComPara.Port);//IP和端口

                Tcpsocket.Connect(new IPEndPoint(ip, ComPara.Port));

                processor = new Thread(Communication);
                processor.IsBackground = true;
                processor.Start();



                //IPAddress ip = IPAddress.Parse(ComPara.ServerIP);
                //IPEndPoint ipep = new IPEndPoint(ip, ComPara.Port);//IP和端口

                ////Tcpsocket.Connect(new IPEndPoint(ip, ComPara.Port));
                //ConnectSocketDelegate connect = ConnectSocket;
                //IAsyncResult asyncResult = connect.BeginInvoke(ipep, Tcpsocket, null, null);

                //bool connectSuccess = asyncResult.AsyncWaitHandle.WaitOne(3 * 1000, false);
                //if (!connectSuccess)
                //{
                //    //MessageBox.Show(string.Format("失败！错误信息：{0}", "连接超时"));
                //    return false;
                //}

                //bool result = connect.EndInvoke(asyncResult);
                //if (!result)
                //{ return false; }
                //LastRecTime = DateTime.Now;

                //processor = new Thread(Communication);
                //processor.IsBackground = true;
                //processor.Start();
                return true;
            }
            catch (Exception ex)
            {
                ChargeStationInfo io = new ChargeStationInfo();
                io.ID = this.DeviceID;
                io.IsCommBreak = true;
                DelegateState.InvokeChargeChangeEvent(io);
                return false;
            }
            finally
            {
                communicationobserve_timer.Enabled = true;
            }
        }

        /// <summary>
        /// 重启
        /// </summary>
        /// <returns></returns>
        public bool ReStart()
        {
            try
            {
                IPAddress ip = IPAddress.Parse(ComPara.ServerIP);
                IPEndPoint ipep = new IPEndPoint(ip, ComPara.Port);
                //IP和端口
                ////Tcpsocket.Connect(new IPEndPoint(ip, ComPara.Port));
                //ConnectSocketDelegate connect = ConnectSocket;
                //IAsyncResult asyncResult = connect.BeginInvoke(ipep, Tcpsocket, null, null);

                //bool connectSuccess = asyncResult.AsyncWaitHandle.WaitOne(3 * 1000, false);
                //if (!connectSuccess)
                //{
                //    //MessageBox.Show(string.Format("失败！错误信息：{0}", "连接超时"));
                //    return false;
                //}
                //else
                //{
                //    LogHelper.WriteLog("连接超时");
                //}

                //bool result = connect.EndInvoke(asyncResult);
                //if (!result)
                //{ return false; }
                //LastRecTime = DateTime.Now;
                Tcpsocket.Connect(new IPEndPoint(ip, ComPara.Port));
                processor = new Thread(Communication);
                processor.IsBackground = true;
                processor.Start();
                return true;
            }
            catch (Exception ex)
            {
                ChargeStationInfo io = new ChargeStationInfo();
                io.ID = this.DeviceID;
                io.IsCommBreak = true;
                DelegateState.InvokeChargeChangeEvent(io);
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
                this.communicationobserve_timer.Enabled = false;
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

        public void Communication()
        {
            try
            {
                while (true)
                {
                    CommandToValue ctov = null;
                    if (QueueCommand.Count > 0 && Tcpsocket.Available > 0)
                    {
                        ///如果缓存区有数据应该将数据消耗完
                        /// 否则再发送执行指令返回的指令则不知道是执行指令
                        /// 返回的还是发送读取指令返回的
                        GetCallBack();
                    }
                    else if (QueueCommand.Count > 0&& QueueCommand.TryPeek(out ctov))//有动作命令
                    {
                       
                        while ((!ExeCommand(ctov)))
                        {
                            Thread.Sleep(500);
                        }
                        QueueCommand.TryDequeue(out ctov);
                    }
                    else
                    {
                        /*
                         * 发送查询命令
                         */
                        SetReadByteList();
                        Tcpsocket.Send(readbytelist.ToArray());//发送字节
                        Thread.Sleep(50);
                        GetCallBack();
                        Thread.Sleep(500);
                    }
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteErrorLog(ex);
                //Clear();
                //ReConnect();
            }
        }

        /// <summary>
        /// 设置读指令
        /// </summary>
        public void SetReadByteList()
        {
            readbytelist.Clear();
            //readbytelist.AddRange(BitConverter.GetBytes(NextDataIndex()));
            readbytelist.AddRange(new byte[] { 0x01, 0x01 });
            readbytelist.AddRange(new byte[] { 0x00, 0x40 });
            readbytelist.AddRange(new byte[] { 0x00, 0x18 });
            readbytelist.Add(0x3D);
            readbytelist.Add(0xD4);

        }

        public bool ExeCommand(CommandToValue ctov)
        {
            try
            {
                if (ctov.Command == AGVCommandEnum.StartCharge)//开始充电
                {
                    writebytelist.Clear();
                    writebytelist.AddRange(new byte[] { 0x01, 0x05 });
                    writebytelist.AddRange(new byte[] { 0x00, 0x07 });
                    writebytelist.AddRange(new byte[] { 0xFF, 0x00 });
                    writebytelist.AddRange(new byte[] { 0x3D, 0xFB });
                    string cmd = "";
                    foreach (byte b in writebytelist)
                    {
                        cmd += Convert.ToString(b, 16).PadLeft(2, '0') + " ";
                    }
                    LogHelper.WriteSendChargeMessLog(this.DeviceID.ToString() + "号充电桩 发送指令：" + cmd);
                    return SendData(writebytelist);
                }
                if (ctov.Command == AGVCommandEnum.StopCharge)//停止充电
                {
                    writebytelist.Clear();
                    writebytelist.AddRange(new byte[] { 0x01, 0x05 });
                    writebytelist.AddRange(new byte[] { 0x00, 0x07 });
                    writebytelist.AddRange(new byte[] { 0x00, 0x00 });
                    writebytelist.Add(0x7C);
                    writebytelist.Add(0x0B);
                    string cmd = "";
                    foreach (byte b in writebytelist)
                    {
                        cmd += Convert.ToString(b, 16).PadLeft(2, '0') + " ";
                    }
                    LogHelper.WriteSendChargeMessLog(this.DeviceID.ToString() + "号充电桩 发送停止充电指令：" + cmd);
                    return SendData(writebytelist);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
                //DelegateState.InvokeDispatchStateEvent("充电桩指令发送失败,将重新发送");
                return false;
            }
        }


        private bool SendData(List<byte> bytelist)
        {
            try
            {
                Tcpsocket.Send(bytelist.ToArray());
                Thread.Sleep(50);
                return GetCallBack();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("组装AGV执行命令异常:" + ex.Message);
                return false;
            }
        }



        /// <summary>
        /// 通讯观察线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CommunicationObser(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                communicationobserve_timer.Enabled = false;
                if (LastRecLong > 2)
                {
                    //如果接受消息时间已经大于2秒,则认为车子掉线了。
                    //DelegateState.InvokeDispatchStateEvent(this.DeviceID.ToString() + "充电桩，已经掉线，将重新尝试连接...");
                    //通知调度程序  充电桩已经掉线
                    ChargeStationInfo ChargeInfo = new ChargeStationInfo();
                    ChargeInfo.ID = this.DeviceID;
                    ChargeInfo.IsCommBreak = true;
                    DelegateState.InvokeChargeChangeEvent(ChargeInfo);
                    if (LastConnectLong > 3)
                    {
                        //如果车子掉线且连接时间超过3秒则需要重连
                        LogHelper.WriteLog("重连充电桩" + DeviceID.ToString());
                        ReConnect();
                    }
                }
                //if (LastConnectLong > 4 && LastRecLong > 3)
                //{
                //    ReConnect();//重新尝试连接
                //}
                //if (LastRecLong > 3)
                //{
                //    //通知调度程序  充电桩已经掉线
                //    ChargeStationInfo ChargeInfo = new ChargeStationInfo();
                //    ChargeInfo.ID = this.DeviceID;
                //    ChargeInfo.IsCommBreak = true;
                //    DelegateState.InvokeChargeChangeEvent(ChargeInfo);
                //}
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
                //DelegateState.InvokeDispatchStateEvent(this.DeviceID.ToString() + "车，观察线程异常");
            }
            finally
            {
                communicationobserve_timer.Enabled = true;
            }
        }

        /// <summary>
        /// 重新尝试连接小车
        /// </summary>
        public void ReConnect()
        {
            try
            {
                if (Init())
                {
                    if (ReStart())
                    {
                        LastConnectTime = DateTime.Now;
                    }
                    else
                    {
                        ChargeStationInfo io = new ChargeStationInfo();
                        io.ID = this.DeviceID;
                        io.IsCommBreak = true;
                        DelegateState.InvokeChargeChangeEvent(io);
                    }
                }
                else
                {
                    ChargeStationInfo io = new ChargeStationInfo();
                    io.ID = this.DeviceID;
                    io.IsCommBreak = true;
                    DelegateState.InvokeChargeChangeEvent(io);
                }
            }
            catch (Exception ex)
            {
                ChargeStationInfo io = new ChargeStationInfo();
                io.ID = this.DeviceID;
                io.IsCommBreak = true;
                DelegateState.InvokeChargeChangeEvent(io);
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
                        GetCallBack();
                    }
                    else
                    { Thread.Sleep(10); }
                }
            }
            catch (Exception ex)
            {
                //Clear();
            }
        }

        /// <summary>
        /// 命令反馈
        /// </summary>
        public bool GetCallBack()
        {
            try
            {
                int offlinecount = 0;
                int allheadleftlengh = 3;
                int receivedlengh = 0;
                byte[] bufferhead = new byte[3];
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
                            LogHelper.WriteReciveChargeMessLog("接受的充电桩" + DeviceID.ToString() + "反馈命令超时");
                            return false;
                        }
                        offlinecount += 1;
                        Thread.Sleep(50);
                    }
                    Buffer.BlockCopy(buffertemp, 0, bufferhead, receivedlengh, lengh);
                    receivedlengh += lengh;
                }
                if (bufferhead[0] == 0x01 && bufferhead[1] == 0x01)
                {
                    offlinecount = 0;
                    receivedlengh = 0;
                    int allcontentleftlengh = Convert.ToInt32(bufferhead[2]) + 2;
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
                                LogHelper.WriteReciveChargeMessLog("接受的充电桩" + DeviceID.ToString() + "反馈命令超时");
                                return false;
                            }
                            offlinecount += 1;
                            Thread.Sleep(50);
                        }
                        Buffer.BlockCopy(buffertemp, 0, buffercontent, receivedlengh, lengh);
                        receivedlengh += lengh;
                    }
                    List<byte> msg = new List<byte>();
                    string SenDLog = "";
                    msg.AddRange(bufferhead);
                    msg.AddRange(buffercontent);
                    foreach (byte item in msg)
                    { SenDLog += ((int)item).ToString("X") + " "; }
                    LogHelper.WriteReciveChargeMessLog("接受的充电桩" + this.DeviceID.ToString() + "反馈命令:" + SenDLog);


                    //List<byte> dd = new List<byte>();
                    //dd.AddRange(new byte[] { 0x01, 0x01, 0x03, 0x06, 0x01, 0x01, 0x1C, 0x1F });
                    //var rr = BitConverter.GetBytes(CRC.CRC16(dd.Take(dd.Count - 2).ToArray(), 0, dd.Count - 3));


                    var r = BitConverter.GetBytes(CRC.CRC16(msg.Take(msg.Count - 2).ToArray(), 0, msg.Count - 3));
                    if (msg[msg.Count - 2] != r[1] || msg[msg.Count - 1] != r[0])
                    {
                        LogHelper.WriteReciveChargeMessLog("接受的充电桩" + this.DeviceID.ToString() + "校验位错误!");
                        return false;
                    }


                    //分析充电桩状态 0待机 1故障 2进行 3完成
                    ChargeStationInfo chargestation = new ChargeStationInfo();
                    chargestation.ID = this.DeviceID;
                    string StateStr = Convert.ToString(msg[3], 2).PadLeft(8, '0');
                    if (StateStr.Substring(7, 1) == "1")
                    { chargestation.ChargeState = 0; }
                    else if (StateStr.Substring(6, 1) == "1")
                    { chargestation.ChargeState = 1; }
                    else if (StateStr.Substring(5, 1) == "1")
                    { chargestation.ChargeState = 2; }
                    else if (StateStr.Substring(4, 1) == "1")
                    { chargestation.ChargeState = 3; }
                    else
                    { chargestation.ChargeState = -1; }
                    chargestation.IsCommBreak = false;
                    DelegateState.InvokeChargeChangeEvent(chargestation);
                    LastRecTime = DateTime.Now;
                    return true;
                }
                else if (bufferhead[0] == 0x01 && bufferhead[1] == 0x05)
                { return true; }
                return false;
            }
            catch (Exception ex)
            { LogHelper.WriteLog("充电桩解析编解码错误!" + ex.Message); }
            return false;
        }
        #endregion
    }
}
