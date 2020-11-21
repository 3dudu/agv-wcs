using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MDM
{
    /// <summary>
    /// 线段资源类
    /// </summary>
    [Serializable]
    public class SegmentResInfo
    {

        public SegmentResInfo()
        {
            StartLandMark = new LandmarkInfo();
            EndLandMark = new LandmarkInfo();
            LinkLandMarks = new List<LandmarkInfo>();
        }

        /// <summary>
        /// 线路ID
        /// </summary>
        public int RouteID { get; set; }
        /// <summary>
        /// 线段资源ID
        /// </summary>
        public int SegmentResID { get; set; }

        /// <summary>
        /// 开始地标ID
        /// </summary>
        public int StartLandMarkID { get; set; }
        /// <summary>
        /// 结束地标ID
        /// </summary>
        public int EndLandMarkID { get; set; }
        /// <summary>
        /// 起始点
        /// </summary>
        public LandmarkInfo StartLandMark { get; set; }

        /// <summary>
        /// 结束点
        /// </summary>
        public LandmarkInfo EndLandMark { get; set; }

        /// <summary>
        /// 关联地标集合
        /// </summary>
        public List<LandmarkInfo> LinkLandMarks { get; set; }

        /// <summary>
        /// 线段中所有地标集合（关联地标加上 起始点和结束点）
        /// </summary>
        public List<LandmarkInfo> AllLandmarks
        {
            get
            {
                return LinkLandMarks.Union(new List<LandmarkInfo>() { StartLandMark, EndLandMark }).ToList();
            }
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        public bool IsNullOrEmpty
        {
            get
            {
                if (StartLandMark == null)
                {
                    return true;
                }
                else
                {
                    return string.IsNullOrEmpty(StartLandMark.LandmarkCode);
                }
            }
        }

        /// <summary>
        ///排序
        /// </summary>
        public int ORD
        {
            get;
            set;
        }
    }
}
