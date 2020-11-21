using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVDispacth.SupperSocket
{
    public class SLAppSocketServer : AppServer<SLSocketSession>
    {
        public SLAppSocketServer()
            : base(new TerminatorReceiveFilterFactory("ENDFLAG\r\n", Encoding.UTF8, new SLRequestInfoParser("MFLAG", "PFLAG")))
        {

        }
    }
}
