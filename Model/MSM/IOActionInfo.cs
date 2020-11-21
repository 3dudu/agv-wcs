using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MSM
{
    [Serializable]
    public class IOActionInfo
    {
        public IOActionInfo()
        {
            ActionName = "";
            WaitTime = 0;
            DeviceID = -1;
            TerminalID = -1;
            TerminalData = "";
            IsNew = false;
            IsSelect = false;
            IOUseState = 0;
        }

        /// <summary>
        /// 明细ID
        /// </summary>
        public int DetailID { get; set; }

        /// <summary>
        /// 动作ID
        /// </summary>
        public int ActionID { get; set; }
        /// <summary>
        /// 动作名称
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// 等待时间(分钟)
        /// </summary>
        public double WaitTime { get; set; }
        /// <summary>
        /// IsWait[0 否1 是]
        /// </summary>
        public int IsWait { get; set; }
        public string IsWaitStr
        {
            get
            {
                switch (IsWait)
                {
                    case 0:
                        return "否";
                    default:
                        return "是";
                }
            }
            set
            {
                switch (value)
                {
                    case "否":
                        IsWait = 0;
                        break;
                    default:
                        IsWait = 1;
                        break;
                }
            }
        }

        /// <summary>
        /// 是否新建
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// 是否选择
        /// </summary>
        public bool IsSelect { get; set; }


        /// <summary>
        /// IO设备ID
        /// </summary>
        public int DeviceID { get; set; }

        /// <summary>
        /// IO设备名称
        /// </summary>
        public string DeviceName { get; set; }


        /// <summary>
        /// 控制字节索引
        /// </summary>
        public int TerminalID { get; set; }

        /// <summary>
        /// 是否放行
        /// </summary>
        public int IsPass { get; set; }

        public string IsPassStr
        {
            get
            {
                return IsPass == 0 ? "否" : "是";
            }
            set
            {
                IsPass = value == "否" ? 0 : 1;
            }
        }

        /// <summary>
        /// 控制类型 【0无 1 读 2 写】
        /// </summary>
        public int TerminalType { get; set; }

        public string TerminalTypeStr
        {
            get
            {
                switch (TerminalType)
                {
                    case 0:
                        return "无";
                    case 1:
                        return "读";
                    case 2:
                        return "写";
                    default:
                        return "无";
                }
            }
            set
            {
                switch (value)
                {
                    case "无":
                        TerminalType = 0;
                        break;
                    case "读":
                        TerminalType = 1;
                        break;
                    case "写":
                        TerminalType = 2;
                        break;
                    default:
                        TerminalType = 0;
                        break;
                }
            }
        }

        /// <summary>
        /// 发送或读取控制端子的数据 [十进制数据] 例：0x01 0x00 0x10 即为1,0,16
        /// </summary>
        public string TerminalData { get; set; }

        /// <summary>
        /// IO设备使用状态[0 空闲 1 占用]
        /// </summary>
        public int IOUseState { get; set; }

        public string IOUseStateUI
        {
            get
            {
                switch (IOUseState)
                {
                    case 0:
                        return "空闲";
                    case 1:
                        return "占用";
                    default:
                        return "";
                }
            }
            set
            {
                switch (value)
                {
                    case "空闲":
                        IOUseState = 0;
                        break;
                    case "占用":
                        IOUseState = 1;
                        break;
                    default:
                        IOUseState = 0;
                        break;
                }
            }
        }
    }
}
