using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    [Serializable]
    public class CallBoxDetail
    {
        public CallBoxDetail()
        {
            TaskConditonCode = "";
            DBAddress = "";
        }
        /// <summary>
        /// 呼叫器ID
        /// </summary>
        public int CallBoxID { get; set; }

        /// <summary>
        /// 读取PLC地址
        /// </summary>
        public string DBAddress { get; set; }

        /// <summary>
        /// 按钮号
        /// </summary>
        public int ButtonID { get; set; }
        /// <summary>
        /// 任务条件配置编号
        /// </summary>
        public string TaskConditonCode { get; set; }
        /// <summary>
        /// 操作类型[0 呼叫 1监控  2 放行]
        /// </summary>
        public int OperaType { get; set; }
        /// <summary>
        /// 监控储位ID
        /// </summary>
        public int LocationID { get; set; }
        /// <summary>
        /// 监控修改储位状态[0 空位置 1 空料架 2 满料架]
        /// </summary>
        public int LocationState { get; set; }
        public string OperTypeStr
        {
            get
            {
                switch (OperaType)
                {
                    case 0:
                        return "呼叫";
                    case 1:
                        return "呼叫盒监控";
                    case 2:
                        return "放行";
                    case 3:
                        return "传感器监控";
                    default:
                        return "";
                }
            }
            set
            {
                switch (value)
                {
                    case "呼叫":
                        OperaType = 0;
                        break;
                    case "呼叫盒监控":
                        OperaType = 1;
                        break;
                    case "放行":
                        OperaType = 2;
                        break;
                    case "传感器监控":
                        OperaType = 3;
                        break;
                    default:
                        OperaType = -1;
                        break;
                }
            }
        }
        public string LocationStateStr
        {
            get
            {
                switch (LocationState)
                {
                    case 0:
                        return "空位置";
                    case 1:
                        return "空料架";
                    case 2:
                        return "满料架";
                    default:
                        return "";
                }
            }
            set
            {
                switch (value)
                {
                    case "空位置":
                        LocationState = 0;
                        break;
                    case "空料架":
                        LocationState = 1;
                        break;
                    case "满料架":
                        LocationState = 2;
                        break;
                    default:
                        LocationState = -1;
                        break;
                }
            }
        }
    }
}
