using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Comoon
{
    /// <summary>
    /// 明细查询字段
    /// </summary>
    [Serializable]
    public class DetailQueryFiled
    {
        public DetailQueryFiled()
        {
            FiledCode = "";
            FiledName = "";
        }

        /// <summary>
        /// 字段编码
        /// </summary>
        public string FiledCode { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string FiledName { get; set; }

        /// <summary>
        /// 汇总方式
        /// </summary>
        public int SummaryType { get; set; }

        public string SummaryTypeUI
        {
            get
            {
                switch (SummaryType)
                {
                    case 0:
                        return "无";
                    case 1:
                        return "计数";
                    case 2:
                        return "求和";
                    case 3:
                        return "平均值";
                    default:
                        return "无";
                }
            }
            set
            {
                switch (value)
                {
                    case "无":
                        SummaryType = 0;
                        break;
                    case "计数":
                        SummaryType = 1;
                        break;
                    case "求和":
                        SummaryType = 2;
                        break;
                    case "平均值":
                        SummaryType = 3;
                        break;
                    default:
                        SummaryType = 0;
                        break;
                }
            }
        }


    }
}
