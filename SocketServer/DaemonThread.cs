using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketServer
{
    public class DaemonThread
    {
        #region 属性
        private Thread m_thread;
        private TCPServer tcpserver;
        private TCPSimulatorAGV Simulatortcpserver;
        #endregion


        #region 函数方法
        public DaemonThread(TCPServer _tcpserver)
        {
            tcpserver = _tcpserver;
            m_thread = new Thread(DaemonThreadStart);
            m_thread.IsBackground = true;
            m_thread.Start();
        }


     	public DaemonThread(TCPSimulatorAGV _tcpserver)
        {
            Simulatortcpserver = _tcpserver;
            m_thread = new Thread(DaemonThreadSimulatorStart);
            m_thread.IsBackground = true;
            m_thread.Start();
        }

        /// <summary>
        /// 心跳处理线程
        /// </summary>
        public void DaemonThreadSimulatorStart()
        {
            try
            {
                //如果线程还活着就30秒清理一下下线的线程
                while (m_thread.IsAlive)
                {
                    Thread.Sleep(1000 * 30);//30秒清理一次下线的客户端
                    Dictionary<string, AppSession> clientsessionarry = Simulatortcpserver._sessionTable;
                    if (clientsessionarry != null && clientsessionarry.Count > 0)
                    {
                        try
                        {
                            foreach (AppSession ClientSessionItem in clientsessionarry.Values)
                            {
                                if (!m_thread.IsAlive) { break; }
                                TimeSpan span = DateTime.Now - ClientSessionItem.LastActiveDateTime;
                                //客户端每10秒发送一次心跳，那么判断一个最后一个到当前时间超过11秒则
                                //认为是该客户端下线，做清理动作；
                                if (span.TotalSeconds > 11)
                                { Simulatortcpserver.CloseClientSocket(ClientSessionItem.ID); }
                            }
                        }
                        catch (Exception ex)
                        { }
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 心跳处理线程
        /// </summary>
        public void DaemonThreadStart()
        {
            try
            {
                //如果线程还活着就30秒清理一下下线的线程
                while (m_thread.IsAlive)
                {
                    Thread.Sleep(1000 * 30);//30秒清理一次下线的客户端
                    Dictionary<string, AppSession> clientsessionarry = tcpserver._sessionTable;
                    if (clientsessionarry != null && clientsessionarry.Count > 0)
                    {
                        try
                        {
                            foreach (AppSession ClientSessionItem in clientsessionarry.Values)
                            {
                                if (!m_thread.IsAlive) { break; }
                                TimeSpan span = DateTime.Now - ClientSessionItem.LastActiveDateTime;
                                //客户端每10秒发送一次心跳，那么判断一个最后一个到当前时间超过11秒则
                                //认为是该客户端下线，做清理动作；
                                if (span.TotalSeconds > 11)
                                { tcpserver.CloseClientSocket(ClientSessionItem.ID); }
                            }
                        }
                        catch (Exception ex)
                        { }
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 停用线程
        /// </summary>
        public void Close()
        {
            try
            {
                m_thread.Abort();
            }
            catch (Exception ex)
            { }
        }
        #endregion
    }
}
