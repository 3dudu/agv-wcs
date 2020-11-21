using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketClient
{
    public class ClientConfig
    {
        private const string DefauleServerIP = "192.168.1.230";

        private const int DefaultPort = 6001;

        private const int DefaultTimeOut = 60;

        private const int DefaultBufferSize = 1024;


        public ClientConfig()
        {
            ServerIP = DefauleServerIP;
            Port = DefaultPort;
            TimeOut = DefaultTimeOut;
            ReceiveBufferSize = DefaultBufferSize;
        }

        public ClientConfig(int port)
            : this()
        {
            Port = port;
        }

        /// <summary>
        /// 服务器ip
        /// </summary>
        public string ServerIP { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public int TimeOut { get; set; }

        /// <summary>
        /// 接收缓冲区大小
        /// </summary>
        public int ReceiveBufferSize { get; set; }
    }
}
