using Model.CarInfoExtend;
using Model.MDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVCore
{
    /// <summary>
    /// 小车事件参数
    /// </summary>
    public class CarEventArgs
    {
        /// <summary>
        /// 小车信息
        /// </summary>
        public CarInfo Car { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Msg { get; set; }

        public CarEventArgs(string msg, CarInfo car)
        {
            Msg = msg;
            Car = car;
        }
    }
}
