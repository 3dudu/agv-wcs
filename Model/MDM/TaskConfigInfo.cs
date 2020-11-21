using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    [Serializable]
    public class TaskConfigInfo
    {
        public TaskConfigInfo()
        {
            TaskConditonCode = "";
            TaskConditonName = "";
            IsNew = false;
            TaskConfigDetail = new List<TaskConfigDetail>();
        }
        /// <summary>
        /// 任务条件配置编号
        /// </summary>
        public string TaskConditonCode { get; set; }
        /// <summary>
        /// 任务条件名称
        /// </summary>
        public string TaskConditonName { get; set; }
        /// <summary>
        /// 是否新增
        /// </summary>
        public bool IsNew { get; set; }
        public IList<TaskConfigDetail> TaskConfigDetail { get; set; }
    }
}
