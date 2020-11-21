using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketModel
{
    public class PackParser
    {
        public static PackageInfo ParserHead(string packhead)
        {
            string _packhead = packhead;
            PackTypeEnum _packtype = packhead.Substring(8, 1) == "0" ? PackTypeEnum.StringMessage : PackTypeEnum.StreamMessage;
            string _packtime = packhead.Substring(9, 14);
            string _command = packhead.Substring(23, 4);
            int _contentlengh = int.Parse(packhead.Substring(0, 8));
            return new PackageInfo() { Command = _command, PackType = _packtype, PackHead = _packhead, PackTime = _packtime, PackContentLengh = _contentlengh };
        }
    }
}
