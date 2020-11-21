using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    /// <summary>
    /// 任务类
    /// </summary>
    [Serializable]
    public class DispatchTaskInfo
    {
        public DispatchTaskInfo()
        {

            dispatchNo = "";
            stationNo = 0;
            matterType = 0;
            ExeAgvID = 0;
            BuildTime = "";
            TaskState = 0;
            FinishTime = "";
            CallLand = "";
            StorageName = "";
            TaskDetail = new List<DispatchTaskDetail>();
        }

        /// <summary>
        /// 系统站点
        /// </summary>
        public int Site { get; set; }

        /// <summary>
        /// 调度任务号
        /// </summary>
        public string dispatchNo { get; set; }


        /// <summary>
        /// 站点号
        /// </summary>
        public int stationNo { get; set; }

        /// <summary>
        /// 呼叫ID(按钮ID、接口ID)
        /// </summary>
        public int CallID { get; set; }


        /// <summary>
        /// 任务类型 0|要料   1|下料
        /// </summary>
        public int taskType { get; set; }


        /// <summary>
        /// 档次
        /// </summary>
        public int matterType { get; set; }


        /// <summary>
        /// 执行小车
        /// </summary>
        public int ExeAgvID { get; set; }


        /// <summary>
        ///创建时间
        /// </summary>
        public string BuildTime { get; set; }


        /// <summary>
        /// 任务状态[0 未执行 1 执行中 2 已完成 3 已超时]
        /// </summary>
        public int TaskState { get; set; }


        public string TaskStateStr
        {
            get
            {
                switch (TaskState)
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
        /// 完成时间
        /// </summary>
        public string FinishTime { get; set; }

        /// <summary>
        /// 物料信息
        /// </summary>
        public string GoodsInfo { get; set; } = "";

        /// <summary>
        /// 所属区域
        /// </summary>
        public int OwerArea { get; set; } = 0;

        #region 额外属性
        /// <summary>
        /// r任务类型
        /// </summary>
        public string taskTypeStr
        {
            get
            {
                if (taskType == 0)
                {
                    return "入库";
                }
                else
                {
                    return "出库";
                }
            }


        }

        /// <summary>
        /// 执行车辆
        /// </summary>
        public string ExeAgv
        {
            get
            {
                if (ExeAgvID == 0)
                {
                    return "";
                }
                else
                {
                    return ExeAgvID.ToString() + "号AGV";
                }
            }
        }

        /// <summary>
        /// 呼叫地标
        /// </summary>
        public string CallLand { get; set; }

        /// <summary>
        /// 选择
        /// </summary>
        public bool IsSelect { get; set; }

        /// <summary>
        ///站点对应的储位名称
        /// </summary>
        public string StorageName { get; set; }

        /// <summary>
        /// 任务明细
        /// </summary>
        public IList<DispatchTaskDetail> TaskDetail { get; set; }
        #endregion
    }
}
