using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlcSMICECallAGV
{
    /// <summary>
    /// 西门子PLC连接
    /// </summary>
    public class SiemensConnectInfo
    {

        public SiemensConnectInfo()
        {
            ServerIP = "";
        }
        /// <summary>
        /// 服务端IP
        /// </summary>
        public string ServerIP { get; set; }

        /// <summary>
        /// 服务端口
        /// </summary>
        public int Port { get; set; }
    }
}
