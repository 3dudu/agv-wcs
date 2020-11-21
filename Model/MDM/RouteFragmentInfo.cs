using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    public class RouteFragmentInfo
    {
        public RouteFragmentInfo()
        {
            Lands = "";
            ActionLand = "";
            Cmd = "";
        }

        public int FagmentID { get; set; }

        /// <summary>
        /// 片段((地标集合 格式如：,0001,0002,))
        /// </summary>
        public string Lands { get; set; }

        /// <summary>
        /// 动作地标
        /// </summary>
        public string ActionLand { get; set; }

        /// <summary>
        /// 命令
        /// </summary>
        public string Cmd { get; set; }
    }
}
