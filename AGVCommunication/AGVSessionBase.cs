using DipatchModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AGVCommunication
{
    public class AGVSessionBase
    {
        #region 委托
        /// <summary>
        /// 通信连接委托
        /// </summary>
        public delegate bool ConnectSocketDelegate(IPEndPoint ipep, Socket sock);
        /// <summary>
        /// 发送数据包委托
        /// </summary>
        public delegate bool SocketSendDataDelegate(Socket sock, List<byte> bytedata);
        #endregion

        #region 属性
        /// <summary>
        /// 数据序号标识
        /// </summary>
        public short dataIndex = 0;

        /// <summary>
        /// 设备ID
        /// </summary>
        public int DeviceID { get; set; }

        /// <summary>
        /// 设备类型(0|AGV 1|充电桩 2|其他远程ID)
        /// </summary>
        public int DeviceType { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// 通讯参数
        /// </summary>
        public AGVComPara ComPara { get; set; }

        /// <summary>
        /// 最后一次收到命令反馈的时间
        /// </summary>
        public DateTime LastRecTime { get; set; }

        /// <summary>
        /// 距最后收到消息的时长（秒）
        /// </summary>
        public double LastRecLong
        {
            get
            {
                return (DateTime.Now - LastRecTime).TotalSeconds;
            }
        }

        /// <summary>
        /// 最后一次重新连接的时间
        /// </summary>
        public DateTime LastConnectTime { get; set; }

        /// <summary>
        /// 距最后一次重连接的时长（秒）
        /// </summary>
        public double LastConnectLong
        {
            get
            {
                return (DateTime.Now - LastConnectTime).TotalSeconds;
            }
        }

        /// <summary>
        /// 命令队列
        /// </summary>
        //public  Queue<CommandToValue> QueueCommand = new Queue<CommandToValue>();
        public ConcurrentQueue<CommandToValue> QueueCommand = new ConcurrentQueue<CommandToValue>();
        private object queuelock = new object();

        //public void Enqueue(CommandToValue ctov)
        //{
        //    lock (queuelock)
        //    {
        //        QueueCommand.Enqueue(ctov);
        //    }
        //}

        //public CommandToValue Peek()
        //{
        //    lock (queuelock)
        //    {
        //      return  QueueCommand.Peek();
        //    }
        //}

        //public void Dequeue()
        //{
        //    lock (queuelock)
        //    {
        //          QueueCommand.Dequeue();
        //    }
        //}

        #endregion

        #region 方法
        public AGVSessionBase()
        {
            LastRecTime = DateTime.Now;
            LastConnectTime = DateTime.Now;
        }

        #endregion

        #region 自定义方法
        private static object objg = new object();
        public short NextDataIndex()
        {
            lock(objg)
            {
                if ((int)dataIndex > 65535)
                {
                    dataIndex = 0;
                }
                dataIndex += 1;
                return dataIndex;
            }
        }


        public double distance(float x1, float y1, float x2, float y2)
        {
            try
            {
                double result = 0;
                result = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
                return result;
            }
            catch (Exception ex)
            { return 0; }
        }
        #endregion

        #region 内部类
        public enum FunctionCode
        {
            Read,
            Write,
        }
        #endregion
    }
}
