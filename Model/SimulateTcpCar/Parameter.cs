using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SimulateTcpCar
{
    public class Parameter
    {
        /// <summary>
        /// 站点
        /// </summary>
        public int Site { get; set; }

        /// <summary>
        /// 指令
        /// </summary>
        public byte Instructions { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        public byte ParameterS { get; set; }
    }
}
