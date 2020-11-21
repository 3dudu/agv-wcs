using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    [Serializable]
    public class TaskConfigDetail
    {
        public TaskConfigDetail()
        {
            TaskConditonCode = "";
            TaskConfigMustPass = new List<TaskConfigMustPass>();
            ArmOwnArea = -1;
        }
        /// <summary>
        /// 任务条件配置编号
        /// </summary>
        public string TaskConditonCode { get; set; }
        /// <summary>
        /// 明细ID
        /// </summary>
        public int DetailID { get; set; }
        /// <summary>
        /// 目的区域编码
        /// </summary>
        public int ArmOwnArea { get; set; }
        /// <summary>
        /// 目的储位状态
        /// </summary>
        public int StorageState { get; set; }

        public string StorageStateStr
        {
            get
            {
                switch (StorageState)
                {
                    case 0:
                        return "空储位";
                    case 1:
                        return "空料架";
                    default:
                        return "满料架";
                }
            }
            set
            {
                switch (value)
                {
                    case "空储位":
                        StorageState = 0;
                        break;
                    case "空料架":
                        StorageState = 1;
                        break;
                    default:
                        StorageState = 2;
                        break;
                }
            }
        }

        /// <summary>
        /// 目的储位物料类型
        /// </summary>
        public int MaterialType { get; set; }
        /// <summary>
        /// 目的点动作[0 取料 1 放料]
        /// </summary>
        public int Action { get; set; }
        /// <summary>
        /// 是否等待放行[0 否 1 是]
        /// </summary>
        public int IsWaitPass { get; set; }
        public string ActionUI
        {
            get
            {
                switch (Action)
                {
                    case 0:
                        return "取料";
                    case 1:
                        return "放料";
                    default:
                        return "";
                }
            }
            set
            {
                switch (value)
                {
                    case "取料":
                        Action = 0;
                        break;
                    case "放料":
                        Action = 1;
                        break;
                    default:
                        Action = -1;
                        break;
                }
            }
        }
        public string IsWaitPassUI
        {
            get
            {
                switch (IsWaitPass)
                {
                    case 1:
                        return "否";
                    case 0:
                        return "是";
                    default:
                        return "";
                }
            }
            set
            {
                switch (value)
                {
                    case "否":
                        IsWaitPass = 1;
                        break;
                    case "是":
                        IsWaitPass = 0;
                        break;
                    default:
                        IsWaitPass = -1;
                        break;
                }
            }
        }

        /// <summary>
        /// 放行类型
        /// </summary>
        public int PassType { get; set; }

        public string PassTypeUI
        {
            get
            {
                switch (PassType)
                {
                    case 0:
                        return "通信设备放行";
                    case 1:
                        return "AGV启动放行";
                    default:
                        return "";
                }
            }
            set
            {
                switch (value)
                {
                    case "通信设备放行":
                        PassType = 0;
                        break;
                    case "AGV启动放行":
                        PassType = 1;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 是否传感器停车[对接机台]
        /// </summary>
        public int IsSensorStop { get; set; }

        public string IsSensorStopUI
        {
            get
            {
                return IsSensorStop == 0 ? "否" : "是";
            }
            set
            {
                IsSensorStop = value == "否" ? 0 : 1;
            }
        }


        public IList<TaskConfigMustPass> TaskConfigMustPass { get; set; }

        /// <summary>
        /// 任务延时开始执行时间
        /// </summary>
        public double TaskDelayed { get; set; }


        /// <summary>
        /// 是否需要回传
        /// </summary>
        public int IsNeedCallBack { get; set; }

        public string IsNeedCallBackStr
        {
            get
            {
                return IsNeedCallBack == 0 ? "否" : "是";
            }
            set
            {
                IsNeedCallBack = value == "否" ? 0 : 1;
            }
        }

        /// <summary>
        /// 是否回调Goods信息
        /// </summary>
        public int IsCallGoods { get; set; } = 0;

        public string IsCallGoodsStr
        {
            get
            {
                return IsCallGoods == 0 ? "否" : "是";
            }
            set
            {
                IsCallGoods = value == "否" ? 0 : 1;
            }
        }
    }
}
