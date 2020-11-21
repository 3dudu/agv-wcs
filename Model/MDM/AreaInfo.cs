using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    [Serializable]
    public class AreaInfo
    {
        public AreaInfo()
        {
            AreaName = "";
        }
        /// <summary>
        /// 区域编码
        /// </summary>
        public int OwnArea { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 最大同时执行任务数 0 表示不限制
        /// </summary>
        public int MaxTaskCount { get; set; } = 0;
    }
}
