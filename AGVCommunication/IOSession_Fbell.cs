using AGVCore;
using DipatchModel;
using Model.MSM;
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
    public class IOSession_Fbell : AGVSessionBase
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
        /// 写list
        /// </summary>
        List<byte> writebytelist = new List<byte>();
        #endregion

        #region 方法
        public IOSession_Fbell(int IOID, AGVComPara ComPara) : base()
        {
            try
            {
                this.DeviceID = IOID;
                this.DeviceType = 2;
                this.DeviceName = IOID.ToString() + "号IO设备";
                this.ComPara = ComPara;
                communicationobserve_timer.Elapsed += new System.Timers.ElapsedEventHandler(CommunicationObser);
                connect = ConnectSocket;
                //SocketSend = SendData;
                ////初始化读
                //readbytelist.AddRange(BitConverter.GetBytes(NextDataIndex()));
                //readbytelist.AddRange(new byte[] { 0x00, 0x00 });
                //readbytelist.AddRange(new byte[] { 0x00, 0x06 });
                //readbytelist.Add((byte)IOID);
                //readbytelist.Add(0x03);
                //readbytelist.AddRange(new byte[] { 0x01, 0x2c, 0x00, 0x02 });
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }


        /// <summary>
        /// 初始化通信
        /// </summary>
        /// <returns></returns>
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
                IODeviceInfo IOInfo = new IODeviceInfo();
                IOInfo.ID = this.DeviceID;
                IOInfo.bIsCommBreak = true;
                DelegateState.InvokeIOFeedBackEvent(IOInfo);
                return false;
            }
        }

        /// <summary>
        /// 建立和主机的连接
        /// </summary>
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
                    if (QueueCommand.Count > 0&& QueueCommand.TryPeek(out ctov))//有动作命令
                    {
                        while ((!ExeCommand(ctov)))
                        {
                            Thread.Sleep(90);
                        }
                        QueueCommand.TryDequeue(out ctov);
                    }
                    //else
                    //{
                    //    //发送查询命令
                    //    Tcpsocket.Send(readbytelist.ToArray());
                    //    Thread.Sleep(300);
                    //}
                }
            }
            catch (Exception ex)
            {
                Clear();
            }
        }

        /// <summary>
        /// 接受IO设备反馈命令
        /// </summary>
        public void ReceverMes()
        {
            try
            {
                while (true)
                {
                    if (/*QueueCommand.Count <= 0 && */Tcpsocket != null && Tcpsocket.Available > 0)
                    {
                        GetCallBack();
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
                if (LastConnectLong > 3)
                {
                    LogHelper.WriteLog("重连" + DeviceID.ToString() + "号IO设备");
                    ReConnect();
                }
                if (LastRecLong > 2)
                {
                    IODeviceInfo IOInfo = new IODeviceInfo();
                    IOInfo.ID = this.DeviceID;
                    DelegateState.InvokeDispatchStateEvent(this.DeviceID.ToString() + "号IO设备掉线,重新尝试连接...");
                    //通知调度程序  小车已经掉线
                    IOInfo.bIsCommBreak = true;
                    DelegateState.InvokeIOFeedBackEvent(IOInfo);
                }
            }
            catch (Exception ex)
            {
                DelegateState.InvokeDispatchStateEvent(this.DeviceID.ToString() + "号IO设备，观察线程异常");
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
                LastConnectTime = DateTime.Now;
                if (Init())
                {
                    IODeviceInfo IOInfo = new IODeviceInfo();
                    IOInfo.ID = this.DeviceID;
                    if (ReStart())
                    {
                        LastConnectTime = DateTime.Now;
                    }
                    else
                    {
                        DelegateState.InvokeDispatchStateEvent("尝试连接" + this.DeviceID.ToString() + "号IO设备...");
                        IOInfo.bIsCommBreak = true;
                        DelegateState.InvokeIOFeedBackEvent(IOInfo);
                    }
                }
                else
                {
                    DelegateState.InvokeDispatchStateEvent("尝试连接" + this.DeviceID.ToString() + "号IO设备...");
                    IODeviceInfo IOInfo = new IODeviceInfo();
                    IOInfo.ID = DeviceID;
                    IOInfo.bIsCommBreak = true;
                    DelegateState.InvokeIOFeedBackEvent(IOInfo);
                }
            }
            catch (Exception ex)
            {
                DelegateState.InvokeDispatchStateEvent("尝试连接" + this.DeviceID.ToString() + "号IO设备...");
                IODeviceInfo IOInfo = new IODeviceInfo();
                IOInfo.ID = DeviceID;
                IOInfo.bIsCommBreak = true;
                DelegateState.InvokeIOFeedBackEvent(IOInfo);
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

                //Tcpsocket.Connect(new IPEndPoint(ip, ComPara.Port));
                ConnectSocketDelegate connect = ConnectSocket;
                IAsyncResult asyncResult = connect.BeginInvoke(ipep, Tcpsocket, null, null);

                bool connectSuccess = asyncResult.AsyncWaitHandle.WaitOne(3 * 1000, false);
                if (!connectSuccess)
                {
                    //MessageBox.Show(string.Format("失败！错误信息：{0}", "连接超时"));
                    return false;
                }

                bool exmessage = connect.EndInvoke(asyncResult);
                if (exmessage == false)
                { return false; }
                //if (!string.IsNullOrEmpty(exmessage))
                //{
                //    //MessageBox.Show(string.Format("失败！错误信息：{0}", exmessage));
                //    return false;
                //}
                LastRecTime = DateTime.Now;

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
                IODeviceInfo IOInfo = new IODeviceInfo();
                IOInfo.ID = DeviceID;
                IOInfo.bIsCommBreak = true;
                DelegateState.InvokeIOFeedBackEvent(IOInfo);
                return false;
            }
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        public bool ExeCommand(CommandToValue ctov)
        {
            try
            {
                writebytelist.Clear();
                writebytelist.AddRange(BitConverter.GetBytes(NextDataIndex()));
                writebytelist.AddRange(new byte[] { 0x00, 0x00 });
                writebytelist.AddRange(new byte[] { 0x00, 0x0B });
                writebytelist.Add((byte)this.DeviceID);
                writebytelist.Add(0x10);
                writebytelist.AddRange(new byte[] { 0x01, 0x2e, 0x00, 0x02, 0x04 });
                int PostID = Convert.ToInt16(ctov.CommandValue);
                //通过IO端子口号计算其对应的字节信息
                IList<byte> cmdbytelist = GetByteListByPortState(PostID);
                writebytelist.AddRange(cmdbytelist);
                string cmd = "";
                foreach (byte b in writebytelist)
                {
                    cmd += Convert.ToString(b, 16).PadLeft(2, '0') + " ";
                }
                LogHelper.WriteLog("向" + this.DeviceID.ToString() + "号IO设备发送指令：" + cmd);
                //Tcpsocket.Send(writebytelist.ToArray());//发送字节
                //{ Thread.Sleep(60); }
                //var Result = GetCallBack();
                //return Result;
                return SendData(writebytelist, FunctionCode.Write);

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("发送AGV命令异常:" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 处理命令反馈
        /// </summary>
        public bool GetCallBack()
        {
            try
            {
                int offlinecount = 0;
                int allheadleftlengh = 6;
                int receivedlengh = 0;
                byte[] bufferhead = new byte[6];
                while (allheadleftlengh - receivedlengh > 0)
                {
                    byte[] buffertemp = new byte[allheadleftlengh - receivedlengh];
                    int lengh = Tcpsocket.Receive(buffertemp);
                    if (lengh <= 0)
                    {
                        if (offlinecount == 3)
                        {
                            throw new Exception("Socket  错误！");
                        }
                        offlinecount += 1;
                        Thread.Sleep(1000 * 2);
                    }
                    Buffer.BlockCopy(buffertemp, 0, bufferhead, receivedlengh, lengh);
                    receivedlengh += lengh;
                }
                offlinecount = 0;
                receivedlengh = 0;
                int allcontentleftlengh = int.Parse(Convert.ToString(bufferhead[5], 10));
                byte[] buffercontent = new byte[allcontentleftlengh];
                while (allcontentleftlengh - receivedlengh > 0)
                {
                    byte[] buffertemp = new byte[allcontentleftlengh - receivedlengh];
                    int lengh = Tcpsocket.Receive(buffertemp);
                    if (lengh <= 0)
                    {
                        if (offlinecount == 3)
                        {
                            throw new Exception("Socket  错误！");
                        }
                        offlinecount += 1;
                        Thread.Sleep(1000 * 2);
                    }
                    Buffer.BlockCopy(buffertemp, 0, buffercontent, receivedlengh, lengh);
                    receivedlengh += lengh;
                }

                //读返回
                if (buffercontent[1] == 0x03)
                {
                    IODeviceInfo IOInfo = new IODeviceInfo();
                    IOInfo.ID = this.DeviceID;
                    //解析 1-8口
                    char[] bytestr = Convert.ToString(buffercontent[4], 2).PadLeft(8, '0').ToArray();
                    for (int i = 0; i < 8; i++)
                    {
                        IOPortInfo ioport = new IOPortInfo();
                        ioport.PortNo = 8 - i;
                        ioport.PortState = bytestr[i] == '1' ? 1 : 0;
                        IOInfo.DIPortList.Add(ioport);
                    }
                    //9-16口
                    bytestr = Convert.ToString(buffercontent[3], 2).PadLeft(8, '0').ToArray();
                    for (int i = 0; i < 8; i++)
                    {
                        IOPortInfo ioport = new IOPortInfo();
                        ioport.PortNo = 16 - i;
                        ioport.PortState = bytestr[i] == '1' ? 1 : 0;
                        IOInfo.DIPortList.Add(ioport);
                    }
                    //17-24口
                    bytestr = Convert.ToString(buffercontent[6], 2).PadLeft(8, '0').ToArray();
                    for (int i = 0; i < 8; i++)
                    {
                        IOPortInfo ioport = new IOPortInfo();
                        ioport.PortNo = 24 - i;
                        ioport.PortState = bytestr[i] == '1' ? 1 : 0;
                        IOInfo.DIPortList.Add(ioport);
                    }
                    //25-32口
                    bytestr = Convert.ToString(buffercontent[5], 2).PadLeft(8, '0').ToArray();
                    for (int i = 0; i < 8; i++)
                    {
                        IOPortInfo ioport = new IOPortInfo();
                        ioport.PortNo = 32 - i;
                        ioport.PortState = bytestr[i] == '1' ? 1 : 0;
                        IOInfo.DIPortList.Add(ioport);
                    }
                    DelegateState.InvokeIOFeedBackEvent(IOInfo);
                    LastRecTime = DateTime.Now;
                    return true;
                }
                //写返回
                if (buffercontent[1] == 0x10)
                {
                    LastRecTime = DateTime.Now;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            { LogHelper.WriteLog("AGV解析编解码错误!" + ex.Message); }
            return false;
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
        public bool SendData(List<byte> bytelist, FunctionCode fc)
        {
            try
            {
                Tcpsocket.Send(bytelist.ToArray());//发送字节
                if (fc == FunctionCode.Write)
                {
                    Thread.Sleep(60);
                }

                int offlinecount = 0;
                int allheadleftlengh = 6;
                int receivedlengh = 0;
                byte[] bufferhead = new byte[6];
                while (allheadleftlengh - receivedlengh > 0)
                {
                    byte[] buffertemp = new byte[allheadleftlengh - receivedlengh];
                    int lengh = Tcpsocket.Receive(buffertemp);
                    if (lengh <= 0)
                    {
                        if (offlinecount == 3)
                        {
                            throw new Exception("Socket  错误！");
                        }
                        offlinecount += 1;
                        Thread.Sleep(1000 * 2);
                    }
                    Buffer.BlockCopy(buffertemp, 0, bufferhead, receivedlengh, lengh);
                    receivedlengh += lengh;
                }
                offlinecount = 0;
                receivedlengh = 0;
                int allcontentleftlengh = int.Parse(Convert.ToString(bufferhead[5], 10));
                byte[] buffercontent = new byte[allcontentleftlengh];
                while (allcontentleftlengh - receivedlengh > 0)
                {
                    byte[] buffertemp = new byte[allcontentleftlengh - receivedlengh];
                    int lengh = Tcpsocket.Receive(buffertemp);
                    if (lengh <= 0)
                    {
                        if (offlinecount == 3)
                        {
                            throw new Exception("Socket  错误！");
                        }
                        offlinecount += 1;
                        Thread.Sleep(1000 * 2);
                    }
                    Buffer.BlockCopy(buffertemp, 0, buffercontent, receivedlengh, lengh);
                    receivedlengh += lengh;
                }
                if (fc == FunctionCode.Read && buffercontent[1] == 0x0f)
                {
                    IODeviceInfo io = new IODeviceInfo();
                    io.ID = this.DeviceID;
                    //解析
                    //1-8口
                    char[] bytestr = Convert.ToString(buffercontent[4], 2).PadLeft(8, '0').ToArray();
                    for (int i = 0; i < 8; i++)
                    {
                        IOPortInfo ioport = new IOPortInfo();
                        ioport.PortNo = 8 - i;
                        ioport.PortState = bytestr[i] == '1' ? 1 : 0;
                        io.DIPortList.Add(ioport);
                    }
                    //9-16口
                    bytestr = Convert.ToString(buffercontent[3], 2).PadLeft(8, '0').ToArray();
                    for (int i = 0; i < 8; i++)
                    {
                        IOPortInfo ioport = new IOPortInfo();
                        ioport.PortNo = 16 - i;
                        ioport.PortState = bytestr[i] == '1' ? 1 : 0;
                        io.DIPortList.Add(ioport);
                    }
                    //17-24口
                    bytestr = Convert.ToString(buffercontent[6], 2).PadLeft(8, '0').ToArray();
                    for (int i = 0; i < 8; i++)
                    {
                        IOPortInfo ioport = new IOPortInfo();
                        ioport.PortNo = 24 - i;
                        ioport.PortState = bytestr[i] == '1' ? 1 : 0;
                        io.DIPortList.Add(ioport);
                    }
                    //25-32口
                    bytestr = Convert.ToString(buffercontent[5], 2).PadLeft(8, '0').ToArray();
                    for (int i = 0; i < 8; i++)
                    {
                        IOPortInfo ioport = new IOPortInfo();
                        ioport.PortNo = 32 - i;
                        ioport.PortState = bytestr[i] == '1' ? 1 : 0;
                        io.DIPortList.Add(ioport);
                    }

                    DelegateState.InvokeIOFeedBackEvent(io);
                    LastRecTime = DateTime.Now;
                    return true;
                }
                if (fc == FunctionCode.Write && buffercontent[1] == 0x0f)
                {
                    LastRecTime = DateTime.Now;
                    return true;
                }
                return false;
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

                //Tcpsocket.Connect(new IPEndPoint(ip, ComPara.Port));
                ConnectSocketDelegate connect = ConnectSocket;
                IAsyncResult asyncResult = connect.BeginInvoke(ipep, Tcpsocket, null, null);

                bool connectSuccess = asyncResult.AsyncWaitHandle.WaitOne(3 * 1000, false);
                if (!connectSuccess)
                {
                    //MessageBox.Show(string.Format("失败！错误信息：{0}", "连接超时"));
                    return false;
                }

                bool exmessage = connect.EndInvoke(asyncResult);
                if (exmessage == false)
                { return false; }
                //if (!string.IsNullOrEmpty(exmessage))
                //{
                //    //MessageBox.Show(string.Format("失败！错误信息：{0}", exmessage));
                //    return false;
                //}
                LastRecTime = DateTime.Now;

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
                IODeviceInfo IOInfo = new IODeviceInfo();
                IOInfo.ID = this.DeviceID;
                IOInfo.bIsCommBreak = true;
                DelegateState.InvokeIOFeedBackEvent(IOInfo);
                return false;
            }
            finally
            {
                this.communicationobserve_timer.Enabled = true;
            }
        }

        #endregion


        #region 自定义方法
        /// <summary>
        /// 计算当前通讯中唯一标识
        /// </summary>
        protected short NextDataIndex()
        {
            if ((int)dataIndex > 65535)
            {
                dataIndex = 0;
            }
            return ++this.dataIndex;
        }

        /// <summary>
        /// 按一共32个端口计算,根据端口号计对应的发送字节
        /// </summary>
        /// <param name="portnum"></param>
        /// <returns></returns>
        IList<byte> GetByteListByPortState(int portnum)
        {
            try
            {
                List<byte> result = new List<byte>() { 0x00, 0x00, 0x00, 0x00 };
                if (portnum >= 1 && portnum <= 32)
                {
                    int bytenum = portnum / 8;
                    if (bytenum == 0)
                    {
                        result[1] = BuildByteByPortNum(portnum);
                    }
                    if (bytenum == 1)
                    {
                        result[0] = BuildByteByPortNum(portnum);
                    }
                    if (bytenum == 2)
                    {
                        result[3] = BuildByteByPortNum(portnum);
                    }
                    if (bytenum == 3)
                    {
                        result[2] = BuildByteByPortNum(portnum);
                    }
                }
                return result;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 根据端口号创建对应的字节内容
        /// </summary>
        private byte BuildByteByPortNum(int portnum)
        {
            try
            {
                int portbyteindex = portnum % 8;
                List<string> bitList = new List<string>() { "0", "0", "0", "0", "0", "0", "0", "0" };
                bitList[8 - portbyteindex] = "1";
                return (byte)Convert.ToInt32(string.Join("", bitList.ToArray()), 2);
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion


    }
}
