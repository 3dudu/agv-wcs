using AGVCore;
using Model.SimulateTcpCar;
using SocketModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tools;

namespace SocketServer
{
    public class TCPSimulatorAGV
    {
        #region 属性
        SimulationAGVInstructions AGVState = null;
        List<byte> d = new List<byte>();
        private Object LockObj = new Object();
        private IServerConfig _config;
        public IServerConfig Config
        {
            get { return _config; }
        }

        /// <summary>
        /// 服务运行状态
        /// </summary>
        public ServerStateEnum State { get; set; }

        /// <summary>
        /// 服务端socket对象
        /// </summary>
        Socket _sersocket = null;

        /// <summary>
        /// 客户端会话连接session
        /// </summary>
        public Dictionary<string, AppSession> _sessionTable;

        /// <summary>
        /// 线程池
        /// </summary>
        private List<Thread> _threadlist;

        /// <summary>
        /// 启动服务线程
        /// </summary>
        private Thread processor;//处理线程

        /// <summary>
        /// 心跳处理类
        /// </summary>
        private DaemonThread daemonthread;

        /// <summary>
        /// 当前连接数
        /// </summary>
        private int _concount;
        #endregion

        #region 事件委托
        /// <summary>
        /// 外围设备数据处理
        /// </summary>
        public delegate void ReceiveClientMes(object sender);
        public event ReceiveClientMes ReceiveMes;

        public delegate void ServerCarState(AppSession Session, SimulationAGVInstructions sender, List<byte> msg);
        public event ServerCarState ServerCarStates;
        #endregion

        #region 函数方法
        /// <summary>
        /// 设置tcp服务端通信
        /// </summary>
        public bool Setup(IServerConfig config)
        {

            try
            {
                if (_sersocket != null)
                { _sersocket.Close(); }
                _config = config;
                //初始化socket 
                _sersocket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);
                //绑定端口 
                IPEndPoint iep = new IPEndPoint(_config.IP, _config.Port);
                try
                {
                    _sersocket.Bind(iep);
                }
                catch (Exception ex)
                {
                    State = ServerStateEnum.NotInitialized;
                    return false;
                }
                State = ServerStateEnum.NotStarted;
                AGVState = new SimulationAGVInstructions();
                AGVState.DelegateSAGVCar += ServiceClient;
                return true;
            }
            catch (Exception ex)
            {
                State = ServerStateEnum.NotInitialized;
                return false;
            }
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public bool Start()
        {
            try
            {
                //服务器配置和服务器状态判断
                if (_config == null || State == ServerStateEnum.NotInitialized)
                { State = ServerStateEnum.NotInitialized; }
                _sessionTable = new Dictionary<string, AppSession>();//客户端会话连接Session
                _threadlist = new List<Thread>();//线程池
                processor = new Thread(new ThreadStart(OnStart));//新建一个处理线程
                processor.IsBackground = true;//线程启动
                processor.Start();//线程开始准备就绪
                //客户端心跳处理判断
                daemonthread = new DaemonThread(this);
                State = ServerStateEnum.Running;
                return true;
            }
            catch (Exception ex)
            {
                State = ServerStateEnum.NotStarted;
                return false;
            }
        }

        private void OnStart()
        {
            try
            {
                _sersocket.Listen(_config.ListenBacklog);
                State = ServerStateEnum.Running;
                while (true)
                {
                    Socket client = _sersocket.Accept();
                    AppSession newSession = new AppSession(client);
                    newSession._lastactivedatetime = DateTime.Now;
                    string aSessionID = Guid.NewGuid().ToString();
                    newSession.ID = aSessionID;
                    lock (LockObj)
                    { _sessionTable.Add(aSessionID, newSession); }
                    Thread newthead = new Thread(new ParameterizedThreadStart(ServiceClient));
                    newthead.Name = aSessionID;
                    newthead.IsBackground = true;
                    newthead.Start(aSessionID);
                    _threadlist.Add(newthead);
                }
            }
            catch (Exception ex)
            { State = ServerStateEnum.NotStarted; }
        }

        private AppSession FindSession(string sid)
        {

            AppSession clientsession;
            try
            {
                clientsession = null;
                lock (LockObj)
                {
                    clientsession = _sessionTable[sid];
                }
                return clientsession;
            }
            catch
            {
                return null;
            }
        }

        private AppSession FindSessionByIP(string IP)
        {
            AppSession clientsession;
            try
            {
                clientsession = null;
                lock (LockObj)
                {
                    foreach (AppSession ClientSessionItem in _sessionTable.Values)
                    {
                        if (ClientSessionItem.ClientSocket.RemoteEndPoint.ToString().Split(':')[0] == IP)
                        { clientsession = ClientSessionItem; break; }
                    }
                }
                return clientsession;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 给一个客户提供服务，接受公司an【】
        /// </summary>
        /// <param name="obj"></param>
        public void ServiceClient(object obj)
        {
            try
            {
                bool keepalive = true;
                AppSession clientsession = FindSession(obj.ToString());
                if (clientsession == null)
                {
                    keepalive = false;
                    return;
                }
                try
                {
                    while (keepalive)
                    {
                        //处理接受包
                        int offlinecount = 0;         //离线计数
                        int allheadleftlengh = 26;     //全部长度
                        int receivedlengh = 0;
                        byte[] bufferhead = new byte[26];    //
                        while (allheadleftlengh - receivedlengh > 0)
                        {
                            byte[] buffertemp = new byte[allheadleftlengh - receivedlengh];
                            int lengh = clientsession.ClientSocket.Receive(buffertemp);
                            if (lengh <= 0)
                            {
                                if (offlinecount == 3)
                                {
                                    throw new Exception("Socket  错误！");
                                }
                                offlinecount += 1;
                                Thread.Sleep(1000 * 2);//再次循环休眠2000毫秒
                            }
                            Buffer.BlockCopy(buffertemp, 0, bufferhead, receivedlengh, lengh);
                            receivedlengh += lengh;
                            clientsession._lastactivedatetime = DateTime.Now;
                        }

                        if (Convert.ToString(bufferhead[0], 16) != "55" && Convert.ToString(bufferhead[25], 16) != "aa")
                        {
                            throw new Exception("Socket 错误！");
                        }
                        List<byte> msg = bufferhead.ToList();

                        #region 手动
                        if (ReceiveMes != null)
                        {
                            if (msg[4] == 2 || msg[4] == 0 || msg[4] == 1 || msg[4] == 3 || msg[4] == 4 || msg[4] == 6 || msg[4] == 7 || msg[4] == 8 || msg[4] == 9 || msg[4] == 10 || msg[4] == 11 || msg[4] == 12 || msg[4] == 13 || msg[4] == 14 || msg[4] == 17 || msg[4] == 18 || msg[4] == 5 || msg[4] == 16)
                            {
                                GetInstructions(msg[4]);
                                ResponseCode(AGVState.ResponseCode);
                                ReceiveMes(new NetEventArgs(clientsession, msg));
                            }
                        }
                        #endregion

                        #region 自动
                        if (ServerCarStates != null)
                        {
                            if (msg[4] == 16)
                            {
                                AGVState.GetRequestCommand(msg);
                                //AGVState.delegateState();
                                //ServerCarStates(clientsession, AGVState, msg);
                            }

                            if (msg[4] == 5)
                            {
                                AGVState.Run(msg);
                                //GetInstructions(msg[4]);
                                //ServerCarStates(clientsession, AGVState, msg);
                            }
                            if (msg[4] == 9)
                            {
                                AGVState.Run(msg);
                                //GetInstructions(msg[4]);
                                //ServerCarStates(clientsession, AGVState, msg);
                            }

                            GetInstructions(msg[4]);
                            ResponseCode(AGVState.ResponseCode);
                            ServerCarStates(clientsession, AGVState, msg);
                        }
                        #endregion


                    }
                }
                catch (Exception ex)
                { }
            }
            catch (Exception ex)
            { }
        }

        public void Stop()
        {
            //if (State != ServerStateEnum.Running) { return; }
            try
            {
                //先断开服务端本身的socket通信
                //否则客户端会提示，服务器强制断开远程连接
                if (_sersocket != null)
                {
                    if (_sersocket.Connected)
                    {
                        _sersocket.Shutdown(SocketShutdown.Both);
                    }
                    _sersocket.Close();
                }
                CloseAllClient();
                _sessionTable = null;
                _threadlist = null;
                if (Config.IsRecHeartbeat)
                { this.daemonthread.Close(); }
                State = ServerStateEnum.NotStarted;
            }
            catch (Exception ex)
            { State = ServerStateEnum.NotStarted; }
        }

        /// <summary>
        /// 关闭所有的客户端会话
        /// </summary>
        public virtual void CloseAllClient()
        {
            try
            {
                lock (LockObj)
                {
                    foreach (AppSession client in _sessionTable.Values)
                    { client.Close(); }
                    foreach (Thread thrad in _threadlist)
                    { thrad.Abort(); }
                    _concount = 0;
                    _sessionTable.Clear();
                    _threadlist.Clear();
                }
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 根据客户端的会话ID来结束一个客户端会话
        /// </summary>
        /// <param name="sessionid"></param>
        public virtual void CloseClientSocket(string sessionid)
        {
            try
            {
                AppSession client = FindSession(sessionid);
                if (client != null)
                {
                    if (_sessionTable.ContainsKey(sessionid))
                    {
                        lock (LockObj)
                        {
                            _sessionTable.Remove(client.ID);
                            _concount = _concount - 1;
                            Thread thread = _threadlist.FirstOrDefault(p => p.Name == sessionid);
                            if (thread != null)
                            { thread.Abort(); }
                        }
                        client.Close();
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 解包服务端
        /// </summary>
        /// <param name="AGV">对象</param>
        public List<byte> Unpack(AppSession Session, SimulationAGVInstructions SendMes)
        {
            try
            {

                d.Clear();

                d.Add(Convert.ToByte(85));

                //AGVID
                d.Add((byte)(SendMes.AGVId));

                //序列号
                byte[] SerialNumber = BitConverter.GetBytes(short.Parse(SendMes.SerialNumber.ToString()));
                //physicaSite(SerialNumber);
                d.AddRange(SerialNumber);

                //响应码
                d.Add(Convert.ToByte(ResponseCode(SendMes.ResponseCode)));

                //运行状态
                d.Add((byte)(SendMes.RunningState));

                //物理站点2
                byte[] sks = BitConverter.GetBytes(short.Parse(SendMes.PhysicalSite.ToString()));
                d.AddRange(sks);
                //physicaSite(sks);

                //里程计数4 
                byte[] skt = BitConverter.GetBytes(SendMes.CountMileage);
                //physicaSite(skt);
                d.AddRange(skt);

                //逻辑站点2
                byte[] skst = BitConverter.GetBytes(short.Parse(SendMes.LogicalSite.ToString()));
                //physicaSite(skst);
                d.AddRange(skst);

                //传感状态
                d.Add((byte)SendMes.SensingState);

                //当前速度
                d.Add((byte)(SendMes.CurrentSpeed));

                //方向代码
                byte EnterRetreat = (byte)(SendMes.EnentRetreat);//进退
                byte About = (byte)(SendMes.LeftRight);//左右
                int[] LengthByte = { 0, 0, 0, 0, 0, 0, 0, 0 };

                LengthByte[0] = EnterRetreat;
                LengthByte[1] = About;

                string LengthByteStr = ""; ;
                for (int i = 0; i < LengthByte.Length; i++)
                {
                    LengthByteStr += LengthByte[i];
                }
                var DirectionCode = Convert.ToByte(LengthByteStr, 2);
                d.Add(DirectionCode);

                //挂钩状态
                d.Add((byte)(SendMes.HookStatus));

                //扩展输入
                d.Add((byte)(SendMes.ExtendedInput));

                //扩展输出
                d.Add((byte)(SendMes.ExtendedOutput));

                string fq = SendMes.BatteryVoltage.ToString();

                //电池电压2
                BatteryVoltage(SendMes.BatteryVoltage);

                //填 充 扩 展 RFID 2
                byte[] kg = BitConverter.GetBytes(short.Parse(SendMes.FillExtendedRFID.ToString()));
                //physicaSite(kg);
                d.AddRange(kg);

                byte bs = GetCheckByte(d);

                d.Add(bs);
                d.Add(0xaa);
                return d;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void BatteryVoltage(double BatteryVoltage)
        {
            string BVoltage = BatteryVoltage.ToString();
            int Battery = 0;
            int Voltage = 0;
            if (BVoltage.Length == 1)
            {
                BVoltage += ".0";
                Battery = Convert.ToInt32(BVoltage.Substring(0, 1));
                Voltage = Convert.ToInt32(BVoltage.Substring(2, 1));
                d.Add((byte)Battery);
                d.Add((byte)Voltage);
            }
            else if (BVoltage.Length == 2)
            {
                BVoltage += ".0";
                Battery = Convert.ToInt32(BVoltage.Substring(0, 2));
                Voltage = Convert.ToInt32(BVoltage.Substring(3, 1));
                d.Add((byte)Battery);
                d.Add((byte)Voltage);
            }
            else if (BVoltage.Length == 3)
            {

                Battery = Convert.ToInt32(BVoltage.Substring(0, 1));
                Voltage = Convert.ToInt32(BVoltage.Substring(2, 1));
                d.Add((byte)Battery);
                d.Add((byte)Voltage);
            }
            else if (BVoltage.Length != 2 || BVoltage.Length != 1)
            {
                Battery = Convert.ToInt32(BVoltage.Substring(0, 2));
                Voltage = Convert.ToInt32(BVoltage.Substring(3, 1));
                d.Add((byte)Battery);
                d.Add((byte)Voltage);
            }
            else
            {
                d.Add((byte)Battery);
                d.Add((byte)Voltage);
            }

        }

        /// <summary>
        /// 计算出校验码
        /// </summary>
        /// <param name="byelist"></param>
        /// <returns></returns>
        public byte GetCheckByte(List<byte> byelist)
        {
            try
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
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 获取响应码
        /// </summary>
        public int ResponseCode(int Response_code)
        {
            try
            {
                if (Response_code == 82)
                {
                    return Response_code = 130;
                }
                else if (Response_code == 85)
                {
                    return Response_code = 133;
                }
                else if (Response_code == 83)
                {
                    return Response_code = 131;
                }
                else if (Response_code == 84)
                {
                    return Response_code = 132;
                }
                else if (Response_code == 85)
                {
                    return Response_code = 133;
                }
                else if (Response_code == 86)
                {
                    return Response_code = 134;
                }
                else if (Response_code == 87)
                {
                    return Response_code = 135;
                }
                else if (Response_code == 88)
                {
                    return Response_code = 136;
                }
                else if (Response_code == 89)
                {
                    return Response_code = 137;
                }
                else
                {
                    return Response_code = 138;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 获取指令码转成响应码
        /// </summary>
        public int GetInstructions(int Instructions)
        {
            if (Instructions == 0)
            {
                return AGVState.ResponseCode = 80;
            }
            else if (Instructions == 1)
            {
                return AGVState.ResponseCode = 81;
            }
            else if (Instructions == 2)
            {
                return AGVState.ResponseCode = 82;
            }
            else if (Instructions == 3)
            {
                return AGVState.ResponseCode = 83;
            }
            else if (Instructions == 4)
            {
                return AGVState.ResponseCode = 84;
            }
            else if (Instructions == 5)
            {
                return AGVState.ResponseCode = 85;
            }
            else if (Instructions == 6)
            {
                return AGVState.ResponseCode = 86;
            }
            else if (Instructions == 7)
            {
                return AGVState.ResponseCode = 87;
            }
            else if (Instructions == 8)
            {
                return AGVState.ResponseCode = 88;
            }
            else if (Instructions == 9)
            {
                return AGVState.ResponseCode = 89;
            }
            else if (Instructions == 16)
            {
                return AGVState.ResponseCode = 0X8A;
            }
            else
            {
                return AGVState.ResponseCode = Convert.ToInt16("8A");
            }
        }
        #endregion
    }
}
