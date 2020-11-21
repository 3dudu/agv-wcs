using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{

    //全局线段类
    public class AllSegment
    {
        public AllSegment()
        {
            BeginLandMakCode = "";
            EndLandMarkCode = "";
            Length = 0;
            ExcuteAngle = -1;
            ExcuteMoveDirect = -1;
            ExcuteTurnDirect = -1;
            ExcuteAvoidance = -1;
            ExcuteSpeed = -1;
        }
        /// <summary>
        /// 线段类型
        /// </summary>
        public int SegmentType { get; set; }
        /// <summary>
        /// 线段长度
        /// </summary>
        public double Length { get; set; }
        /// <summary>
        /// 线段的开始地标
        /// </summary>
        public string BeginLandMakCode { get; set; }
        /// <summary>
        /// 线段的结束地标
        /// </summary>
        public string EndLandMarkCode { get; set; }

        /// <summary>
        /// 贝塞尔曲线Point1 X坐标
        /// </summary>
        public double Point1X { get; set; }

        /// <summary>
        /// 贝塞尔曲线Point1 Y坐标
        /// </summary>
        public double Point1Y { get; set; }

        /// <summary>
        /// 贝塞尔曲线Point2 X坐标
        /// </summary>
        public double Point2X { get; set; }

        /// <summary>
        /// 贝塞尔曲线Point2 Y坐标
        /// </summary>
        public double Point2Y { get; set; }

        /// <summary>
        /// 贝塞尔曲线Point3 X坐标
        /// </summary>
        public double Point3X { get; set; }

        /// <summary>
        /// 贝塞尔曲线Point3 Y坐标
        /// </summary>
        public double Point3Y { get; set; }

        /// <summary>
        /// 贝塞尔曲线Point4 X坐标
        /// </summary>
        public double Point4X { get; set; }

        /// <summary>
        /// 贝塞尔曲线Point4 Y坐标
        /// </summary>
        public double Point4Y { get; set; }

        /// <summary>
        /// 线段的强制车头角度
        /// </summary>
        public int ExcuteAngle { get; set; }
        /// <summary>
        /// 强制行走方向 0前进1后退
        /// </summary>
        public int ExcuteMoveDirect { get; set; }
        /// <summary>
        /// 强制拐弯方向,0右拐1左拐
        /// </summary>
        public int ExcuteTurnDirect { get; set; }
        /// <summary>
        /// 线路规划级别 默认0；1>2>3>4。。。
        /// </summary>
        public int PlanRouteLevel { get; set; }

        /// <summary>
        /// 强制避障值
        /// </summary>
        public int ExcuteAvoidance { get; set; }

        /// <summary>
        /// 强制速度
        /// </summary>
        public int ExcuteSpeed { get; set; }
    }
}
