using AGVCore;
using SocketModel;
using System;
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
    public class TCPServer
    {
        #region 属性
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
                if (_config == null || State == ServerStateEnum.NotInitialized)
                { State = ServerStateEnum.NotInitialized; }
                _sessionTable = new Dictionary<string, AppSession>();
                _threadlist = new List<Thread>();
                processor = new Thread(new ThreadStart(OnStart));//新建一个处理线程
                processor.IsBackground = true;
                processor.Start();//线程开始
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
                    Thread newThreadSend = new Thread(new ParameterizedThreadStart(ServiceClientForHeart));
                    newThreadSend.Name=newthead.Name = aSessionID;
                    newthead.IsBackground = true;
                    newThreadSend.IsBackground = true;
                    newthead.Start(aSessionID);
                    newThreadSend.Start(aSessionID);
                    _threadlist.Add(newthead);
                    _threadlist.Add(newThreadSend);
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

        public void ServiceClientForHeart(object obj)
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
                while (keepalive)
                {
                    List<byte> BackMsg = new List<byte>();
                    BackMsg.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01 });
                    clientsession.ClientSocket.Send(BackMsg.ToArray());
                    Thread.Sleep(1000*5);
                }
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }


        private static object lockRiceve = new object();
        /// <summary>
        /// 给一个客户提供服务，接受公司an【】
        /// </summary>
        /// <param name="obj"></param>
        public void ServiceClient(object obj)
        {
            try
            {
                //lock (lockRiceve)
                //{
                    bool keepalive = true;
                    AppSession clientsession = FindSession(obj.ToString());
                    if (clientsession == null)
                    {
                        keepalive = false;
                        return;
                    }
                    
                    DelegateState.InvokeDispatchStateEvent(clientsession._clientsocket.RemoteEndPoint.ToString() + "已连接");
                    try
                    {
                        while (keepalive)
                        {
                            //处理接受包
                            int offlinecount = 0;
                            int allheadleftlengh = 6;
                            int receivedlengh = 0;
                            byte[] bufferhead = new byte[6];
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
                                    Thread.Sleep(1000 * 2);
                                }
                                Buffer.BlockCopy(buffertemp, 0, bufferhead, receivedlengh, lengh);
                                receivedlengh += lengh;
                                clientsession._lastactivedatetime = DateTime.Now;
                            }
                            offlinecount = 0;
                            receivedlengh = 0;
                            int allcontentleftlengh = int.Parse(Convert.ToString(bufferhead[5], 10)); //Convert.ToInt32(Convert.ToString(bufferhead[4], 2).PadLeft(8, '0') + Convert.ToString(bufferhead[5], 2).PadLeft(8, '0'), 2);
                            byte[] buffercontent = new byte[allcontentleftlengh];
                            while (allcontentleftlengh - receivedlengh > 0)
                            {
                                byte[] buffertemp = new byte[allcontentleftlengh - receivedlengh];
                                int lengh = clientsession.ClientSocket.Receive(buffertemp);
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
                                clientsession._lastactivedatetime = DateTime.Now;
                            }
                            //List<byte> msg = buffercontent.ToList();
                            //记录接受日志
                            List<byte> msg = new List<byte>();
                            msg.AddRange(bufferhead);
                            msg.AddRange(buffercontent);
                            string log_text = "";
                            foreach (byte item in msg)
                            { log_text += ((int)item).ToString("X") + " "; }
                            string IP = ((IPEndPoint)clientsession.ClientSocket.RemoteEndPoint).Address.ToString();
                            LogHelper.WriteCallBoxLog("接受到IP地址为【" + IP + "】指令:" + log_text);
                            //接受完外部指令后处理回掉处理逻辑
                            List<byte> CallBoxConten = new List<byte>();
                            CallBoxConten.AddRange(buffercontent);
                            if (ReceiveMes != null)
                            { ReceiveMes(new NetEventArgs(clientsession, CallBoxConten)); }
                        }
                    }
                    catch (Exception ex)
                    { LogHelper.WriteErrorLog(ex); }
                //}
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }

        public void Stop()
        {
            if (State != ServerStateEnum.Running) { return; }
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
                            List<Thread> threads = _threadlist.Where(p => p.Name == sessionid).ToList();
                            if (threads != null)
                            {
                                foreach (Thread thrd in threads)
                                { thrd.Abort(); }
                            }
                        }
                        client.Close();
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion
    }//endClass
}
