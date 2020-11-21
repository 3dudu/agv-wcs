using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    /// <summary>
    /// 特殊动作设置类
    /// </summary>
    public class SpeRunDirConfigInfo
    {
        public SpeRunDirConfigInfo()
        {
            Fragment = "";
            Dir = 0;
        }
        /// <summary>
        /// 片段
        /// </summary>
        public string Fragment { get; set; }
        /// <summary>
        /// 方向
        /// </summary>
        public int Dir { get; set; }
    }
}
