using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketModel
{
    public enum PackTypeEnum
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        StringMessage = 0,

        /// <summary>
        /// 流消息
        /// </summary>
        StreamMessage,
    }
}
