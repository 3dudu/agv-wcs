using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DipatchModel
{
    public class CommandLandMark
    {
        public CommandLandMark()
        {
            ActionType = 0;
            NaviType = 1;
            TaskConditonCode = "";
        }

        /// <summary>
        /// 地标
        /// </summary>
        public string LandmarkCode { get; set; }

        /// <summary>
        /// 导航方式 for 混合导航使用 [1 激光导航 2 激光导航+二维码终点定位 3 二维码导航]
        /// </summary>
        public int NaviType { get; set; }

        /// <summary>
        /// 坐标X(用于激光导航用)
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// 坐标Y(用于激光导航用)
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// 角度
        /// </summary>
        public int Angle { get; set; }

        /// <summary>
        /// 前进方向 0 前进 1 后退
        /// </summary>
        public int Move_Direact { get; set; }

        /// <summary>
        /// 转向 0 右转 1 左转
        /// </summary>
        public int Turn_Direact { get; set; }

        /// <summary>
        /// 速度
        /// </summary>
        public int Move_Speed { get; set; }

        /// <summary>
        /// 避障
        /// </summary>
        public int Avoidance { get; set; }

        /// <summary>
        /// 动作 
        /// 通用 1 升，2 降，3 自动充电
        /// OMARK 0 无动作 1 充电 2 升平台+夹持无动作 3 升平台+夹持机构张开
        /// 4 升平台+夹持机构夹紧到位置1（对应最大的托盘）
        /// 5 升平台+夹持机构夹紧到位置2（对应中等的托盘）
        /// 6 升平台+夹持机构夹紧到位置3（对应最小的托盘）
        /// 7 降平台+夹持无动作
        /// 8 降平台+夹持机构张开
        /// 9 降平台+夹持机构夹紧到位置1（对应最大的托盘）
        /// 10 降平台+夹持机构夹紧到位置2（对应中等的托盘）
        /// 11 降平台+夹持机构夹紧到位置3（对应最小的托盘）
        /// </summary>
        public int ActionType { get; set; }

        /// <summary>
        /// 是否传感器停车[0 否 1 是]
        /// </summary>
        public int IsSensorStop { get; set; }

        /// <summary>
        /// 动作参数[如叉车,抬升高度等]
        /// </summary>
        public int ActionParameter { get; set; }

        //-----必经过程地标中的参数属性-----//
        /// <summary>
        /// 任务条件配置编号
        /// </summary>
        public string TaskConditonCode { get; set; }

        /// <summary>
        /// 任务条件配置明细ID
        /// </summary>
        public int TaskConfigDetailID { get; set; }

        /// <summary>
        /// 等待时间(分钟)
        /// </summary>
        public double WaitTime { get; set; }
    }
}
