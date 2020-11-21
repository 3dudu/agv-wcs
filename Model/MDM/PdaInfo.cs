using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    /// <summary>
    /// 平板档案
    /// </summary>
    public class PdaInfo : IEquatable<PdaInfo>
    {
        public PdaInfo()
        {
            PadID = 0;
            Area = 0;
            Discripetion = "";
            IsWarm = false;
            PdaOperSetList = new List<PdaOperSetInfo>();
        }

        public bool Equals(PdaInfo other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(this, other)) return true;
            return PadID.Equals(other.PadID);
        }


        /// <summary>
        /// 平板ID
        /// </summary>
        public int PadID { get; set; }


        ///// <summary>
        ///// 所属区域 0 固晶 3 清洗 4 焊线 5 点胶 6 空料盒回收区 7 待固红光 8 烤房 9 待焊线区 12 空料盒区
        ///// </summary>
        public int Area { get; set; }

        /// <summary>
        /// 平板类型[0 呼叫 1 监控]
        /// </summary>
        public int PadType { get; set; }

        /// <summary>
        ///是否显示区域[0 否 1 是]
        /// </summary>
        public int IsViewArea { get; set; }


        /// <summary>
        /// 描述
        /// </summary>
        public string Discripetion { get; set; }

        /// <summary>
        /// 按钮集合
        /// </summary>
        public IList<PdaOperSetInfo> PdaOperSetList { get; set; }

        /// <summary>
        /// 所属区域
        /// </summary>
        public string AreaStr { get; set; }
        //public string AreaStr
        //{
        //    get
        //    {
        //        switch (Area)
        //        {
        //            case 0:
        //                return "固晶区";
        //            case 3:
        //                return "清洗";
        //            case 4:
        //                return "焊线";
        //            case 5:
        //                return "点胶";
        //            case 6:
        //                return "空料盒回收区";
        //            case 7:
        //                return "待固红光";
        //            case 8:
        //                return "烤房";
        //            case 9:
        //                return "待焊线区";
        //            case 12:
        //                return "空料盒区";
        //            default:
        //                return "";
        //        }
        //    }
        //    set
        //    {
        //        switch (value)
        //        {
        //            case "固晶区":
        //                Area = 0;
        //                break;
        //            case "清洗":
        //                Area = 3;
        //                break;
        //            case "焊线":
        //                Area = 4;
        //                break;
        //            case "点胶":
        //                Area = 5;
        //                break;
        //            case "空料盒回收区":
        //                Area = 6;
        //                break;
        //            case "待固红光":
        //                Area = 7;
        //                break;
        //            case "烤房":
        //                Area = 8;
        //                break;
        //            case "待焊线区":
        //                Area = 9;
        //                break;
        //            case "空料盒区":
        //                Area = 12;
        //                break;
        //            default:
        //                Area = 13;
        //                break;
        //        }
        //    }
        //}

        public string PadTypeStr
        {
            get
            {
                switch (PadType)
                {
                    case 0:
                        return "呼叫";
                    case 1:
                        return "监控";
                    default:
                        return "";
                }
            }
            set
            {
                switch (value)
                {
                    case "呼叫":
                        PadType = 0;
                        break;
                    case "监控":
                        PadType = 1;
                        break;
                    default:
                        PadType = 0;
                        break;
                }
            }
        }

        /// <summary>
        /// 是否显示区域
        /// </summary>
        public string IsViewAreaStr
        {
            get
            {
                switch (IsViewArea)
                {
                    case 0:
                        return "否";
                    case 1:
                        return "是";
                    default:
                        return "";
                }
            }
            set
            {
                switch (value)
                {
                    case "否":
                        IsViewArea = 0;
                        break;
                    case "是":
                        IsViewArea = 1;
                        break;
                    default:
                        IsViewArea = 0;
                        break;
                }
            }
        }

        /// <summary>
        /// 是否新建
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// 选择
        /// </summary>
        public bool IsSelect { get; set; }

        /// <summary>
        /// 是否超时报警
        /// </summary>
        public bool IsWarm { get; set; }
    }
}
