using Model.MSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    /// <summary>
    /// 任务条件配置必经地标
    /// </summary>
    [Serializable]
    public class TaskConfigMustPass
    {
        public TaskConfigMustPass()
        {
            TaskConditonCode = "";
            MustPassLandCode = "";
            Remark = "";
            MustPassIOAction = new List<IOActionInfo>();
        }
        /// <summary>
        /// 任务条件配置编号
        /// </summary>
        public string TaskConditonCode { get; set; }
        /// <summary>
        /// 任务条件配置明细ID
        /// </summary>
        public int TaskConfigDetailID { get; set; }

        /// <summary>
        /// 明细ID
        /// </summary>
        public int DetailID { get; set; }

        /// <summary>
        /// 必经地标号
        /// </summary>
        public string MustPassLandCode { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 动作列表
        /// </summary>
        public IList<IOActionInfo> MustPassIOAction = new List<IOActionInfo>();
    }
}
