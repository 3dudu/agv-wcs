using Model.CarInfoExtend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MDM
{
    /// <summary>
    /// 线路实体类
    /// </summary>
    [Serializable]
    public  class RouteInfo
    {

        public RouteInfo()
        {
            RouteName = "";
            LandCodeStr = "";
            FileName = "";
            RouteID = 1;
            LandMarkList = new List<LandmarkInfo>();
            LandmarkListStr = new List<string>();
            ToCars = new List<CarBaseStateInfo>();
            IsNew = true;
        }

        public bool IsNew { get; set; }

        /// <summary>
        /// 线路ID
        /// </summary>
        public int RouteID { get; set; }

        /// <summary>
        /// 线路名称
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// 线路类型
        /// </summary>
        public RouteTypeEnum RouteType { get; set; }


        public string RouteTypeStr
        {
            get
            { return RouteTypeInt ==0 ? "工作路线" : "充电路线"; }
            set
            {
                RouteTypeInt = value.Equals("工作路线") ? 0 : 1;
            }
        }

        public int RouteTypeInt { get; set; }

        /// <summary>
        /// 步点集合
        /// </summary>
        public List<SegmentResInfo> SegmentResList { get; set; }

        /// <summary>
        /// 当前线路中最后一个地标
        /// </summary>
        public LandmarkInfo LastLandmark
        {
            get
            {
                if (LandMarkList != null && LandMarkList.Count > 0)
                {
                    return LandMarkList[LandMarkList.Count];
                }
                else
                {
                    return new LandmarkInfo();
                }
            }
        }

        /// <summary>
        /// 当前线路中第一个
        /// </summary>
        public LandmarkInfo FistLandmark
        {
            get
            {
                if (LandMarkList != null && LandMarkList.Count > 0)
                {
                    return LandMarkList[0];
                }
                else
                {
                    return new LandmarkInfo();
                }
            }
        }

        /// <summary>
        /// 最后一个线段
        /// </summary>
        public SegmentResInfo LastSegmentRes
        {
            get
            {
                return SegmentResList.OrderByDescending(k => k.ORD).FirstOrDefault();
            }
        }

        /// <summary>
        /// 线路中第一段
        /// </summary>
        public SegmentResInfo FirstSegmentRes
        {
            get
            {
                return SegmentResList.OrderBy(k => k.ORD).FirstOrDefault();
            }
        }

        /// <summary>
        /// 组成路线的地标编码
        /// </summary>
        public string LandCodeStr { get; set; }

        /// <summary>
        /// 路线文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///地标集合
        /// </summary>
        public IList<LandmarkInfo> LandMarkList { get; set; }

        /// <summary>
        /// 地标集合（只有地标号）
        /// </summary>
        public IList<string> LandmarkListStr { get; set; }

        /// <summary>
        /// 站点
        /// </summary>
        public int StationNo { get; set; }

        /// <summary>
        /// 站点方向[0 往站点去  1从站点出]
        /// </summary>
        public int DirectionForStation { get; set; }

        public string DirectionUI
        {
            get
            {
                return DirectionForStation == 0 ? "往站点去" : "从站点出";
            }
            set
            {
                DirectionForStation = value == "往站点去" ? 0 : 1;
            }
        }


        //public string StarLandCode
        //{
        //    get
        //    {
        //        if (!string.IsNullOrEmpty(LandCodeStr))
        //        {return LandCodeStr.Split(',').Where(p => !string.IsNullOrEmpty(p.Trim())).FirstOrDefault();}
        //        else
        //        { return ""; }
        //    }
        //}

        //public string EndLandCode
        //{
        //    get
        //    {
        //        if (!string.IsNullOrEmpty(LandCodeStr))
        //        { return LandCodeStr.Split(',').Where(p => !string.IsNullOrEmpty(p.Trim())).LastOrDefault(); }
        //        else
        //        { return ""; }
        //    }
        //}

        /// <summary>
        /// 对照的小车
        /// </summary>
        public IList<CarBaseStateInfo> ToCars { get; set; }
    }
}
