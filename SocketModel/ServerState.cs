using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketModel
{
    public enum ServerStateEnum:int
    {
        /// <summary>
        /// 未初始化
        /// </summary>
        NotInitialized = ServerStateConst.NotInitialized,

        /// <summary>
        /// 正在初始化
        /// </summary>
        Initializing = ServerStateConst.Initializing,

        /// <summary>
        /// 已经初始化，但未启动
        /// </summary>
        NotStarted = ServerStateConst.NotStarted,

        /// <summary>
        /// 启动服务中
        /// </summary>
        Starting = ServerStateConst.Starting,

        /// <summary>
        /// 正在运行
        /// </summary>
        Running = ServerStateConst.Running,

        /// <summary>
        /// 停止中
        /// </summary>
        Stopping = ServerStateConst.Stopping,
    }

    internal class ServerStateConst
    {
        public const int NotInitialized = 0;

        public const int Initializing = 1;

        public const int NotStarted = 2;

        public const int Starting = 3;

        public const int Running = 4;

        public const int Stopping = 5;
    }
}
