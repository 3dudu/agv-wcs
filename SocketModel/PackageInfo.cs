using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketModel
{
    public class PackageInfo
    {
        public PackageInfo()
        {
            PackHead = "";
            PackType = PackTypeEnum.StringMessage;
            PackTime = "";
            PackContent = "";
            Command = "";
            PackContentLengh = 0;
        }


        /// <summary>
        /// 包头
        /// </summary>
        public string PackHead { get; set; }

        /// <summary>
        /// 包体类型
        /// </summary>
        public PackTypeEnum PackType { get; set; }

        /// <summary>
        /// 包体时间
        /// </summary>
        public string PackTime { get; set; }

        /// <summary>
        /// 包内容
        /// </summary>
        public string PackContent { get; set; }

        /// <summary>
        /// 命令
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// 包体长度
        /// </summary>
        public int PackContentLengh { get; set; }
    }//end
}
