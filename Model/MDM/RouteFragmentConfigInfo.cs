using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    [Serializable]
    public class RouteFragmentConfigInfo
    {
        public RouteFragmentConfigInfo()
        {
            CmdName ="";
            Fragment = "";
            ActionLandMark = "";
            CmdCode = 0;
            CmdIndex = 0;
            CmdPara = 0;
            
        }
        /// <summary>
        /// 片段((地标集合 格式如：,0001,0002,))
        /// </summary>
        public string Fragment { get; set; }
        /// <summary>
        /// 动作地标
        /// </summary>
        public string ActionLandMark { get; set; }
        /// <summary>
        /// 命令编码
        /// </summary>
        public int CmdCode { get; set; }
        /// <summary>
        /// 命令参数
        /// </summary>
        public int CmdPara { get; set; }
        /// <summary>
        /// 命令顺序
        /// </summary>
        public int CmdIndex { get; set; }
        /// <summary>
        /// 命令名称
        /// </summary>
        public string CmdName { get; set; } 
    }
}
