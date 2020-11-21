using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    /// <summary>
    /// 自动充电桩实体类
    /// </summary>
    [Serializable]
    public class ChargeStationInfo
    {
        public ChargeStationInfo()
        {
            IP = "";
            Port = "";
            ChargeLandCode = "";
            //ChargeState = -1;
            ChargeState = -1;
        }

        /// <summary>
        /// 充电桩编码
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 状态 【0待机 1故障 2进行 3完成】
        /// </summary>
        public int ChargeState { get; set; }

        public int State { get; set; }

        public string StateStr
        {
            get { return State == 0 ? "停用" : "启用" ; }
            set { State = value == "停用" ? 0 : 1; }
        }

        /// <summary>
        /// 充电桩当前状态 【0待机 1故障 2进行 3完成】
        /// </summary>
        public string ChargeStateStr
        {
            get
            {
                switch (ChargeState)
                {
                    case 0:
                        return "待机";
                    case 1:
                        return "故障";
                    case 2:
                        return "进行";
                    case 3:
                        return "完成";
                    default:
                        return "其他状态";
                }
            }
        }

        /// <summary>
        /// 是否被锁定0 否1 是
        /// </summary>
        public int ChargeLock { get; set; }
        /// <summary>
        /// 充电桩IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 充电桩端口号
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// 充电桩对照地标号
        /// </summary>
        public string ChargeLandCode { get; set; }

        /// <summary>
        /// 是否通信中断
        /// </summary>
        public bool IsCommBreak { get; set; }


        public void GetValue(ChargeStationInfo charge)
        {
            ChargeState = charge.ChargeState;
            IsCommBreak = charge.IsCommBreak;
        }
    }
}
