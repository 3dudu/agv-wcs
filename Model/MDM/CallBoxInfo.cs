using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    /// <summary>
    /// 按钮盒实体
    /// </summary>
    [Serializable]
    public class CallBoxInfo
    {
        public CallBoxInfo()
        {
            CallBoxDetails = new List<CallBoxDetail>();
            CallBoxBtnList = "";
            CallBoxName = "";
            IsNew = false;
            CallBoxIP = "";
            CallBoxReadAddr = "";
        }
        /// <summary>
        /// 是否新建
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// 呼叫器ID
        /// </summary>
        public int CallBoxID { get; set; }

        /// <summary>
        /// 呼叫盒IP地址
        /// </summary>
        public string CallBoxIP { get; set; }

        /// <summary>
        /// 呼叫盒端口号
        /// </summary>
        public int CallBoxPort { get; set; }

        /// <summary>
        /// PLC类型呼叫器读取起始地址
        /// </summary>
        public string CallBoxReadAddr { get; set; }

        /// <summary>
        /// 读取长度
        /// </summary>
        public int ReadLenth { get; set; }

        /// <summary>
        /// 呼叫器名称
        /// </summary>
        public string CallBoxName { get; set; }

        /// <summary>
        /// 呼叫器类型[0 无 1 呼叫  2监控]
        /// </summary>
        public int CallType { get; set; }

        public string CallTypeStr
        {
            get
            {
                switch (CallType)
                {
                    case 0:
                        return "无";
                    case 1:
                        return "呼叫";
                    case 2:
                        return "监控";
                    default:
                        return "无";
                }
            }
            set
            {
                switch (value)
                {
                    case "无":
                        CallType = 0;
                        break;
                    case "呼叫":
                        CallType = 1;
                        break;
                    case "监控":
                        CallType = 2;
                        break;
                    default:
                        CallType = 0;
                        break;
                }
            }
        }

        /// <summary>
        /// 呼叫器按钮明细
        /// </summary>
        public IList<CallBoxDetail> CallBoxDetails { get; set; }
        /// <summary>
        /// 按钮盒按钮状态信息
        /// </summary>
        public string CallBoxBtnList { get; set; }
    }

}
