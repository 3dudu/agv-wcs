using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MDM
{
    /// <summary>
    /// 地标类
    /// </summary>
    [Serializable]
    public class LandmarkInfo
    {
        public LandmarkInfo()
        {
            LandmarkCode = "";
            Acts = new List<CmdInfo>();
            LandMarkName = "";
            IsRotateLand = false;
            sway = SwayEnum.None;
            movedirect = MoveDirectEnum.None;
            ExcuteSpeed =-1;
            ExcuteAvoidance = -1;
        }

        /// <summary>
        /// 地标编码
        /// </summary>
        public string LandmarkCode { get; set; }

        /// <summary>
        /// 地标名称
        /// </summary>
        //public string LandmarkName { get; set; }

        /// <summary>
        /// 地标X坐标
        /// </summary>
        public double LandX { get; set; }

        /// <summary>
        /// 地标Y坐标
        /// </summary>
        public double LandY { get; set; }


        public double LandMidX { get; set; }

        /// <summary>
        /// 地标Y坐标
        /// </summary>
        public double LandMidY { get; set; }

        /// <summary>
        /// 是否等待地标
        /// </summary>
        public int IsWaitmark { get; set; }

        /// <summary>
        ///所属区域类型 0 无  1 发货取 2 卸货区
        /// </summary>
        public int AreaType { get; set; }

        /// <summary>
        /// 二维数组X索引
        /// </summary>
        public int IndexX { get; set; }

        /// <summary>
        /// 二维数组Y索引
        /// </summary>
        public int IndexY { get; set; }

        /// <summary>
        /// 是否下料点
        /// </summary>
        public int IsDelivery { get; set; }

        /// <summary>
        /// 下料点编号
        /// </summary>
        public int DeliverCode { get; set; }

        /// <summary>
        /// 是否上料点
        /// </summary>
        public int IsSorting { get; set; }

        /// <summary>
        /// 上料点编号
        /// </summary>
        public int SortingCode { get; set; }

        /// <summary>
        /// 是否拐点
        /// </summary>
        public int IsInflectionPoint { get; set; }

        /// <summary>
        /// 是否避让点
        /// </summary>
        public int IsWaitPoint { get; set; }

        /// <summary>
        /// 路线中地标对应的动作集
        /// </summary>
        public IList<CmdInfo> Acts { get; set; }

        /// <summary>
        /// 地标名称
        /// </summary>
        public string LandMarkName { get; set; }

        /// <summary>
        ///地标转向
        /// </summary>
        public SwayEnum sway { get; set; }

        public string swayStr
        {
            get
            {
                switch (sway)
                {
                    case SwayEnum.Left:
                        return "左转";
                    case SwayEnum.Right:
                        return "右转";
                    default:
                        return "无";
                }
            }
        }

        /// <summary>
        /// 前进还是后退
        /// </summary>
        public MoveDirectEnum movedirect { get; set; }

        public string movedirectStr
        {
            get
            {
                switch (movedirect)
                {
                    case MoveDirectEnum.Backoff:
                        return "后退";
                    case MoveDirectEnum.Forward:
                        return "前进";
                    case MoveDirectEnum.Reverse:
                        return "反向启动";
                    default:
                        return "无";
                }
            }
        }

        /// <summary>
        /// 根据路线计算的车头角度
        /// </summary>
        public int Angle { get; set; }

        /// <summary>
        /// 是否线路中旋转地标
        /// </summary>
        public bool IsRotateLand { get; set; }

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
