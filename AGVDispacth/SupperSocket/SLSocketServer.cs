using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVDispacth.SupperSocket
{
    public class SLSocketServer : AppServer<SLSocketSession>
    {
        public SLSocketServer()
            : base(new TerminatorReceiveFilterFactory("ENDFLAG",Encoding.UTF8, new SLRequestInfoParser("MFLAG", "PFLAG")))
        {

        }
    }
}
