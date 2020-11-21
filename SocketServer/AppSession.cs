using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer
{
    public class AppSession
    {
        #region 属性
        internal Socket _clientsocket { get; set; }
        internal DateTime _lastactivedatetime { get; set; }
        /// <summary>
        /// 客户端
        /// </summary>
        public Socket ClientSocket { get { return _clientsocket; } }
        /// <summary>
        /// 最后一次被激活时间
        /// </summary>
        public DateTime LastActiveDateTime { get { return _lastactivedatetime; } }
        /// <summary>
        /// 会话ID
        /// </summary>
        public string ID { get; set; }
        #endregion

        #region 函数方法
        /// <summary>
        /// 重载构造函数
        /// </summary>
        public AppSession(Socket client)
        {
            _clientsocket = client;
        }

        /// <summary>
        /// 关闭会话
        /// </summary>
        public void Close()
        { //关闭数据的接受和发送 
            try
            {
                if ((_clientsocket != null))
                {
                    _clientsocket.Shutdown(SocketShutdown.Both);
                    //清理资源
                    _clientsocket.Close();
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion
    }
}
