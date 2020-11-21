using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer
{
    public class ServerConfig : IServerConfig
    {
        /// <summary>
        /// 默认IP
        /// </summary>
        public IPAddress DefaultIP = IPAddress.Any;

        /// <summary>
        /// 默认服务端口
        /// </summary>
        public const int DefaultPort = 6000;

        /// <summary>
        /// 默认最大客户端连接数
        /// </summary>
        public const int DefaultMaxClient = 50;

        /// <summary>
        /// 默认发送超时
        /// </summary>
        public const int DefaultSendOutTime = 60;

        /// <summary>
        /// 默认接收超时
        /// </summary>
        public const int DefaultRecOutTime = 60;

        /// <summary>
        /// 默认接收缓冲去大小
        /// </summary>
        public const int DefaultReceiveBufferSize = 4096;

        /// <summary>
        /// 默认监听数
        /// </summary>
        public const int DefaultListenBacklog = 100;

        /// <summary>
        /// 默认发送缓冲区
        /// </summary>
        public const int DefaultSendBufferSize = 2048;

        /// <summary>
        /// 默认 接收心跳消息
        /// </summary>
        public const bool DefaultIsRecHeartbeat = true;

        public ServerConfig()
        {
            this.IP = DefaultIP;
            this.Port = DefaultPort;
            this.MaxClientCount = DefaultMaxClient;
            this.RecOutTime = DefaultRecOutTime;
            this.SendOutTime = DefaultSendOutTime;
            this.ReceiveBufferSize = DefaultReceiveBufferSize;
            this.ListenBacklog = DefaultListenBacklog;
            this.SendBufferSize = DefaultSendBufferSize;
            this.TextEncoding = Encoding.GetEncoding("utf-8");
            this.IsRecHeartbeat = DefaultIsRecHeartbeat;
        }

        public ServerConfig(int port) : this()
        {
            this.Port = port;
        }
        /// <summary>
        /// IP配置
        /// </summary>
        public IPAddress IP { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 客户端最大连接数
        /// </summary>
        public int MaxClientCount { get; set; }

        /// <summary>
        /// 发送超时(秒)
        /// </summary>
        public int SendOutTime { get; set; }

        /// <summary>
        /// 接收超时(秒)
        /// </summary>
        public int RecOutTime { get; set; }

        /// <summary>
        /// 默认接收缓冲去大小
        /// </summary>
        public int ReceiveBufferSize { get; set; }

        /// <summary>
        /// 默认监听数
        /// </summary>
        public int ListenBacklog { get; set; }

        /// <summary>
        /// 默认发送缓冲区
        /// </summary>
        public int SendBufferSize { get; set; }

        /// <summary>
        /// 消息编码
        /// </summary>
        public Encoding TextEncoding { get; set; }

        /// <summary>
        /// 是否接收心跳消息
        /// </summary>
        public bool IsRecHeartbeat { get; set; }
    }
}
