using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    public class ScheduingInfo
    {
        public ScheduingInfo()
        {
            ProduceBillID = "";
            isNew = false;
        }

        /// <summary>
        /// 生成订单
        /// </summary>
        public string ProduceBillID { get; set; }

        public int ProduceID { get; set; }

        public string ProduceName { get; set; }

        /// <summary>
        /// 0 红光 1 绿光 2 蓝光
        /// </summary>
        public int RGBType { get; set; }

        public bool isNew { get; set; }

        public string RGBTypeName
        {
            get
            {
                switch (RGBType)
                {
                    case 0:
                        return "红光";
                    case 1:
                        return "绿光";
                    case 2:
                        return "蓝光";
                }
                return "";
            }
            set
            {
                switch (value)
                {
                    case "红光":
                        RGBType = 0;
                        break;
                    case "绿光":
                        RGBType = 1;
                        break;
                    case "蓝光":
                        RGBType = 2;
                        break;
                }
            }
        }
    }
}
