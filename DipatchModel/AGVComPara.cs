using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DipatchModel
{
    /// <summary>
    /// AGV通信指令参数
    /// </summary>
    public class AGVComPara
    {
        #region Socket 小车作为服务端
        /// <summary>
        /// 服务端IP
        /// </summary>
        public string ServerIP { get; set; }

        /// <summary>
        /// 服务端口
        /// </summary>
        public int Port { get; set; }
        #endregion
    }
}
