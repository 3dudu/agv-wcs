using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    /// <summary>
    /// 地标对应动作
    /// </summary>
    public class LandmarkToActionsInfo
    {

        public LandmarkToActionsInfo()
        {
            LandCode = "";
            ActionList = new List<RouteFragmentConfigInfo>();
        }
        /// <summary>
        /// 地标编码
        /// </summary>
        public string LandCode { get; set; }


        /// <summary>
        /// 动作集合
        /// </summary>
        public IList<RouteFragmentConfigInfo> ActionList { get; set; }
    }
}
