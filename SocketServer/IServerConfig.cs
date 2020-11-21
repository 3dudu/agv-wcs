using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer
{
    public interface IServerConfig
    {
        /// <summary>
        /// IP配置
        /// </summary>
        IPAddress IP { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        int Port { get; set; }

        /// <summary>
        /// 客户端最大连接数
        /// </summary>
        int MaxClientCount { get; set; }

        /// <summary>
        /// 发送超时(秒)
        /// </summary>
        int SendOutTime { get; set; }

        /// <summary>
        /// 接收超时(秒)
        /// </summary>
        int RecOutTime { get; set; }

        /// <summary>
        /// 默认接收缓冲去大小
        /// </summary>
        int ReceiveBufferSize { get; set; }

        /// <summary>
        /// 默认监听数
        /// </summary>
        int ListenBacklog { get; set; }

        /// <summary>
        /// 默认发送缓冲区
        /// </summary>
        int SendBufferSize { get; set; }

        /// <summary>
        /// 文本编码
        /// </summary>
        Encoding TextEncoding { get; set; }
        /// <summary>
        /// 是否接收心跳消息
        /// </summary>
        bool IsRecHeartbeat { get; set; }
    }
}
