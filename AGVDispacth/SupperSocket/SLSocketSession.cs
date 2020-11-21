using DipatchModel;
using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace AGVDispacth.SupperSocket
{
    public class SLSocketSession : AppSession<SLSocketSession>
    {
        /// <summary>
        /// 客户端类型
        /// </summary>
        public TCPClienTypeEnum ClienType { get; set; }

        /// <summary>
        /// 所属分组
        /// </summary>
        public string OwnGroup { get; set; }

        /// <summary>
        /// 线路
        /// </summary>
        public int Route { get; set; }
        
        /// <summary>
        /// PadID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 区域类型
        /// </summary>
        public int AreaType { get; set; }

        /// <summary>
        /// 发送信息
        /// </summary>
        public override bool TrySend(byte[] data, int offset, int length)
        {
            try
            {
                return base.TrySend(data, offset, length);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
                return false;
            }
        }
    }
}
