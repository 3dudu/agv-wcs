using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Comoon
{
    /// <summary>
    /// 明细查询条件
    /// </summary>
    [Serializable]
    public class DetailCondition
    {
        public DetailCondition()
        {
            ConditionCode = "";
            ConditionName = "";
            ConditionValue = "";
            Location = Point.Empty;
            control_type_ui = "";
            RealyValue = "";
            IsSystem = false;
        }

        /// <summary>
        /// 条件编码
        /// </summary>
        public string ConditionCode { get; set; }

        /// <summary>
        /// 是否系统函数
        /// </summary>
        public bool IsSystem { get; set; }

        /// <summary>
        /// 条件值
        /// </summary>
        public string ConditionValue { get; set; }

        /// <summary>
        /// 实际条件值
        /// </summary>
        public string RealyValue { get; set; }

        /// <summary>
        ///条件名称
        /// </summary>
        public string ConditionName { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        /// <summary>
        /// Location
        /// </summary>
        public Point Location
        {
            get
            { return new Point(X,Y); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public int control_type { get; set; }
        public string control_type_ui
        {
            get
            {
                switch (control_type)
                {
                    case 0:
                        return "输入框";
                    case 1:
                        return "日期";
                    case 2:
                        return "选择框";
                    default:return "";
                }
            }
            set
            {
                switch (value)
                {
                    case "输入框":
                        control_type = 0;
                        break;
                    case "日期":
                        control_type = 1;
                        break;
                    case "选择框":
                        control_type = 2;
                        break;
                    default:break;
                }
            }
        }
    }
}
