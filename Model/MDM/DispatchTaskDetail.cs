using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    [Serializable]
    public class DispatchTaskDetail
    {
        public DispatchTaskDetail()
        {
            dispatchNo = "";
            LandCode = "";
            StorageName = "";
            BeginTime = "";
            FinishTime = "";
            TaskConditonCode = "";
        }

        public bool IsSelect { get; set; }

        /// <summary>
        /// 任务号
        /// </summary>
        public string dispatchNo { get; set; }

        /// <summary>
        /// 明细ID
        /// </summary>
        public int DetailID { get; set; }


        /// <summary>
        /// 目标地标号
        /// </summary>
        public string LandCode { get; set; }

        /// <summary>
        /// 操作类型[0 取 1 放 2 自动充电]
        /// </summary>
        public int OperType { get; set; }


        public string OperTypeStr
        {
            get
            {
                switch (OperType)
                {
                    case 0:
                        return "降平台";
                    case 1:
                        return "升平台";
                    case 2:
                        return "自动充电";
                    default:
                        return "";
                }
            }
        }
        /// <summary>
        /// 是否允许执行[0 否   1 是]
        /// </summary>
        public int IsAllowExcute { get; set; }

        /// <summary>
        /// 放行类型[0 呼叫盒放行 1 小车启动按钮放行]
        /// </summary>
        public int PassType { get; set; }

        /// <summary>
        /// 0 未执行  1 正在执行  2 已完成
        /// </summary>
        public int State { get; set; }

        public string TaskStateStr
        {
            get
            {
                switch (State)
                {
                    case 0:
                        return "未执行";
                    case 1:
                        return "执行中";
                    case 2:
                        return "已完成";
                    default:
                        return "已超时";
                }
            }
        }

        /// <summary>
        /// 所放物料信息  0 空料车  1 满料车
        /// </summary>
        public int PutType { get; set; }

        /// <summary>
        /// 小车动作参数
        /// </summary>
        public int AGVActionParameter { get; set; }

        /// <summary>
        /// 是否等待[0 否1 是]
        /// </summary>
        public int IsWait { get; set; }

        /// <summary>
        /// 等待时间(分钟)
        /// </summary>
        public double WaitTime { get; set; }

        /// <summary>
        /// 站点对应储位ID
        /// </summary>
        public int StorageID { get; set; } = 0;
        /// <summary>
        ///站点对应的储位名称
        /// </summary>
        public string StorageName { get; set; }

        /// <summary>
        /// 是否传感器停车[对接机台]
        /// </summary>
        public int IsSensorStop { get; set; }

        /// <summary>
        /// 开始执行时间
        /// </summary>
        public string BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string FinishTime { get; set; }

        /// <summary>
        /// 任务条件配置编号
        /// </summary>
        public string TaskConditonCode { get; set; }

        /// <summary>
        /// 任务条件配置明细ID
        /// </summary>
        public int TaskConfigDetailID { get; set; }

        /// <summary>
        /// 是否需要回调PLC/MES...
        /// </summary>
        public int IsNeedCallBack { get; set; }

        /// <summary>
        /// 是否回调Goods信息
        /// </summary>
        public int IsCallGoods { get; set; } = 0;

        /// <summary>
        /// 物料编码和批次信息
        /// </summary>
        public string  GoodsInfo { get; set; } = "";
    }
}
