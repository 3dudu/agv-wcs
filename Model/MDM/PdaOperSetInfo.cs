using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    /// <summary>
    /// 平板按钮操作
    /// </summary>
    public class PdaOperSetInfo
    {
        public PdaOperSetInfo()
        {
            PdaID = 0;
            BtnID = 0;
            BtnDescript = "";
            BtnSendValue = "";
            Area = 0;
            BtnState = -1;
        }


        /// <summary>
        /// PADID
        /// </summary>
        public int PdaID { get; set; }

        /// <summary>
        /// 平板类型[0 呼叫 1 监控]
        /// </summary>
        public int PadType { get; set; }

        /// <summary>
        ///是否显示区域[0 否 1 是]
        /// </summary>
        public int IsViewArea { get; set; }

        /// <summary>
        ///是否需要选择订单[0 否 1 是]
        /// </summary>
        public int IsChooseBill { get; set; }

        /// <summary>
        /// 按钮ID
        /// </summary>
        public int BtnID { get; set; }


        /// <summary>
        /// 按钮描述
        /// </summary>
        public string BtnDescript { get; set; }

        /// <summary>
        /// 发送值
        /// </summary>
        public string BtnSendValue { get; set; }

        /// <summary>
        /// 所属区域【烤房特殊使用】[0 待烘烤区 1 待固绿光 2 待固蓝光 3 待机检区]
        /// </summary>
        public int Area { get; set; }

        /// <summary>
        /// 所属区域【烤房特殊使用】[0 待烘烤区 1 待固绿光 2 待固蓝光 3 待机检区]
        /// </summary>
        public string AreaStr
        {
            get
            {
                switch (Area)
                {
                    case 0:
                        return "待烘烤区";
                    case 1:
                        return "待固绿光";
                    case 2:
                        return "待固蓝光";
                    case 3:
                        return "待机检区";
                    default:
                        return "";
                }
            }
            set
            {
                switch (value)
                {
                    case "待烘烤区":
                        Area = 0;
                        break;
                    case "待固绿光":
                        Area = 1;
                        break;
                    case "待固蓝光":
                        Area = 2;
                        break;
                    case "待机检区":
                        Area = 3;
                        break;
                    default:
                        Area = 4;
                        break;
                }
            }
        }

        public string IsChooseBillStr
        {
            get
            { return IsChooseBill == 0 ? "否" : "是"; }
            set
            { IsChooseBill = value.Equals("否") ? 0 : 1; }
        }

        /// <summary>
        /// 操作类型[0 上料 1 下料]
        /// </summary>
        public int OperType { get; set; }


        public string OperTypeStr
        {
            get
            { return OperType == 0 ? "上料" : "下料"; }
            set
            { OperType = value.Equals("上料") ? 0 : 1; }
        }

        /// <summary>
        /// 平板所属区域
        /// </summary>
        public int PadArea { get; set; }

        public string PadAreaStr
        {
            get
            {
                if (PadArea == 8)
                {
                    switch (Area)
                    {
                        case 0:
                            return "待烘烤区";
                        case 1:
                            return "待固绿光";
                        case 2:
                            return "待固蓝光";
                        case 3:
                            return "待机检区";
                        default:
                            return "";
                    }
                }
                else
                {
                    switch (Area)
                    {
                        case 0:
                            return "红光";
                        case 1:
                            return "绿光";
                        case 2:
                            return "蓝光";
                        case 3:
                            return "清洗";
                        case 4:
                            return "焊线";
                        case 5:
                            return "点胶";
                        case 6:
                            return "空料盒回收区";
                        case 7:
                            return "待固红光";
                        case 8:
                            return "烤房";
                        case 9:
                            return "待焊线区";
                        case 12:
                            return "空料盒区";
                        default:
                            return "";
                    }
                }
            }
        }

        /// <summary>
        /// 按钮状态【显示当前位置的颜色0空(白色) 1空料架(紫色) 2满料架(蓝色)】
        /// </summary>
        public int BtnState { get; set; }

    }//end Class
}
