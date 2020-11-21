using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    [Serializable]
    public  class CmdInfo
    {
        public CmdInfo()
        {
            CmdName = "";
            CmdPara = -1;
        }
        /// <summary>
        /// 动作编码
        /// </summary>
        public int CmdCode { get; set; }
        /// <summary>
        /// 动作名称
        /// </summary>
        public string CmdName { get; set; }
        /// <summary>
        /// 动作命令
        /// </summary>
        public string CmdOrder { get; set; }

        /// <summary>
        /// 动作参数
        /// </summary>
        public int CmdPara { get; set; }

        /// <summary>
        /// 是否系统指令
        /// </summary>
        public bool IsSysCmd { get; set; }
    }
}
