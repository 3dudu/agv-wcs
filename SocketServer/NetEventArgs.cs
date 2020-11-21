using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer
{
    public class NetEventArgs : EventArgs
    {
        private AppSession _session;
        private List<byte> Mes;
        public NetEventArgs(AppSession session, List<byte> mes)
        {
            if (null == session)
            {
                throw (new ArgumentNullException());
            }

            _session = session;
            Mes = mes;
        }


        /// <summary> 
        /// 获得激发该事件的会话对象 
        /// </summary> 
        public AppSession Session
        {
            get
            {
                return _session;
            }
        }

        /// <summary>
        /// 报文
        /// </summary>
        public List<byte> MesContent
        {
            get
            {
                return Mes;
            }
        }
    }
}
