using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.MDM
{
    /// <summary>
    /// 小车基本状态,涉及小车类公用的属性在此增加,特殊小车属性在子类里添加
    /// </summary>
    [Serializable]
    public class CarBaseStateInfo
    {
        public CarBaseStateInfo()
        {
            bIsCommBreak = false;
            IsUse = 1;
            CarName = "";
            CarIP = "";
            CarPort = "";
            X = 0;
            Y = 0;
            Route = new List<LandmarkInfo>();
            RealyRoute= new List<LandmarkInfo>();
            RouteLands = new List<string>();
            TurnLands = new List<string>();
            StandbyLandMark = "";
            ExcuteTaksNo = "";
            TaskDetailID = -1;
            CarState = 0;
            Angel = -1;
            ErrorMessage = "";
            ArmLand = "";
            CurrentLandMark = new LandmarkInfo();
            OldHistoryUpLandCode = HistoryUpLandCode = "";
            AllRouteLandCode = RealyAllRouteLandCode = "";
        }

        /// <summary>
        /// 是否启用 0否1是
        /// </summary>
        public int IsUse { get; set; }

        public string IsUseStr
        {
            get { return IsUse == 0 ? "停用" : "启用"; }
            set { IsUse = value == "停用" ? 0 : 1; }
        }

        /// <summary>
        /// 选择
        /// </summary>
        public bool IsSelect { get; set; }

        /// <summary>
        /// 车号
        /// </summary>
        public int AgvID { get; set; }

        /// <summary>
        /// AGV名称
        /// </summary>
        public string CarName { get; set; }

        /// <summary>
        /// 小车IP
        /// </summary>
        public string CarIP { get; set; }

        /// <summary>
        /// 小车端口号
        /// </summary>
        public string CarPort { get; set; }


        /// <summary>
        /// 所属区域
        /// </summary>
        public int OwerArea { get; set; }

        /// <summary>
        /// X坐标（单位 m）
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Y坐标 （单位m）
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// 电池电压
        /// </summary>
        public double fVolt { get; set; }


        /// <summary>
        /// 充电地标
        /// </summary>
        public string ChargeCode { get; set; } = "";

        /// <summary>
        /// 速度
        /// </summary>
        public int speed { get; set; }

        /// <summary>
        /// 行走方向[0 前进模式 1 后退模式]
        /// </summary>
        public int Move_Derict { get; set; }

        /// <summary>
        /// 转弯方向[ 0 右转模式 1 左转模式]
        /// </summary>
        public int Turn_Derict { get; set; }

        /// <summary>
        /// 通信中断
        /// </summary>
        public bool bIsCommBreak { get; set; }

        /// <summary>
        /// 设置PBS
        /// </summary>
        public int PBSValue { get; set; }

        /// <summary>
        /// 当前站点
        /// </summary>
        public int CurrSite { get; set; }

        /// <summary>
        /// 车体角度 0度，90度，180度，270度
        /// </summary>
        public float Angel { get; set; }

        /// <summary>
        /// 待命点
        /// </summary>
        public string StandbyLandMark { get; set; }


        /// <summary>
        /// 运行状态  0 待命  1 执行中  2 完成  3 急停 4 脱线 5 满线
        /// OMARK  0 待命  1 执行中  2 完成  3 任务暂停 4 请求重发路线 5 遥控中
        /// </summary>
        public int CarState { get; set; }

        /// <summary>
        /// 距离上一个地表的距离
        /// </summary>
        public double Rundistance { get; set; }

        /// <summary>
        /// 逻辑站点
        /// </summary>
        public int LogicSite { get; set; }

        /// <summary>
        /// 中泰兴运行状态Str
        /// </summary>
        public string CarStateStr
        {
            get
            {
                if (CarState == 0)
                {
                    return "待命";
                }
                if (CarState == 1)
                {
                    return "任务执行中";
                }
                if (CarState == 2)
                {
                    return "任务完成";
                }
                if (CarState == 3)
                {
                    return "任务暂停";
                }
                if (CarState == 4)
                {
                    return "请求重发路线";
                }
                if (CarState == 5)
                {
                    return "遥控操作中";
                }
                if (CarState == 6)
                {
                    return "机械防撞触发";
                }
                if (CarState == 7)
                {
                    return "光电传感器触发";
                }
                if (CarState == 8)
                {
                    return "电量不足";
                }
                if (CarState == 9)
                {
                    return "内部错误";
                }
                if (CarState == 10)
                {
                    return "调度指令超时或中断";
                }
                if (CarState == 11)
                {
                    return "执行超时";
                }
                if (CarState == 12)
                {
                    return "限位触发";
                }
                if (CarState == 255)
                {
                    return "未准备";
                }
                return "";
            }
        }

        /// <summary>
        /// OMARK运行状态Str
        /// </summary>
        public string OmarkCarStateStr
        {
            get
            {
                if (CarState == 0)
                {
                    return "待命";
                }
                else if (CarState == 1)
                {
                    return "任务执行中";
                }
                else if (CarState == 2)
                {
                    return "任务完成";
                }
                else if (CarState == 3)
                {
                    return "任务暂停";
                }
                else if (CarState == 4)
                {
                    return "请求重发路线";
                }
                else if (CarState == 5)
                {
                    return "遥控操作中";
                }
                else
                { return ""; }
            }
        }

        /// <summary>
        /// 低电量
        /// </summary>
        public bool LowPower { get; set; }

        /// <summary>
        /// 路线地标集
        /// </summary>
        public List<LandmarkInfo> Route { get; set; }

        /// <summary>
        /// 实际运行线路
        /// </summary>
        public List<LandmarkInfo> RealyRoute { get; set; }

        /// <summary>
        /// 线路所有地表code字符串 如,12,23,45,77,
        /// </summary>
        public string AllRouteLandCode { get; set; }


        /// <summary>
        /// 实际运行线路所有地表code字符串 如,12,23,45,77,
        /// </summary>
        public string RealyAllRouteLandCode { get; set; }

        /// <summary>
        /// 路线资源集和旋转集合
        /// </summary>
        public List<string> RouteLands { get; set; }

        /// <summary>
        /// 旋转资源集合
        /// </summary>
        public List<string> TurnLands { get; set; }

        /// <summary>
        /// 当前执行任务号
        /// </summary>
        public string ExcuteTaksNo { get; set; }

        /// <summary>
        /// 当前执行任务明细ID
        /// </summary>
        public int TaskDetailID { get; set; }

        /// <summary>
        /// 重载ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.AgvID.ToString() + "号AGV,站点:" + CurrSite.ToString() + "任务号:[" + ExcuteTaksNo + "]，明细:[" + TaskDetailID.ToString() + "]";
        }

        /// <summary>
        /// 是否正在回待命点
        /// </summary>
        public bool IsBack { get; set; }

        /// <summary>
        /// 当前任务目标站点
        /// </summary>
        public string ArmLand { get; set; }

        /// <summary>
        /// 车的当前充电站点号
        /// </summary>
        public string NowChargeLandCode { get; set; }

        /// <summary>
        /// 到达目的点取放类型（0 放  1 取）
        /// </summary>
        public int OperType { get; set; }

        /// <summary>
        /// 到达目的点后所放载具的状态(0 空料车  1 满料车)
        /// </summary>
        public int PutType { get; set; }

        /// <summary>
        /// 升降台/钩状态 0 下 1 上 2 未到位
        /// </summary>
        public int BangState { get; set; }
        public string BangStateStr
        {
            get
            {
                if (BangState == 0)
                { return "低位"; }
                else if (BangState == 1)
                { return "高位"; }
                else
                { return "未到位"; }
            }
        }

        /// <summary>
        /// OMARK夹持状态 0 张开  1 夹持位置1  2 夹持位置2 3 夹持位置3 4 未到位
        /// </summary>
        public int JCState { get; set; }

        public string JCStateStr
        {
            get
            {
                switch (JCState)
                {
                    case 0:
                        return "张开";
                    case 1:
                        return "夹持位置1";
                    case 2:
                        return "夹持位置2";
                    case 3:
                        return "夹持位置3";
                    case 4:
                        return "未到位";
                    default:
                        return "张开";
                }
            }
        }

        /// <summary>
        /// 是否在码上/卡上/坐标点上 (二维码or磁导航AGV告诉调度系统)
        /// </summary>
        public int IsUpLand { get; set; }
        public string IsUpLandStr
        {
            get
            {
                return IsUpLand == 1 ? "在" : "不在";
            }
        }

        /// <summary>
        /// 是否在定位码上 (是激光心跳发送给AGV)
        /// </summary>
        public int IsUpQCode { get; set; }

        public string IsUpQCodeStr
        {
            get
            {
                return IsUpQCode == 1 ? "在" : "不在";
            }
        }

        /// <summary>
        /// 报警信息  如果异常信息对应不同字节，可以拼接一起显示
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 当前地标
        /// </summary>
        public LandmarkInfo CurrentLandMark { get; set; }

        /// <summary>
        /// 之前运行的上一个地标,用于判断下次启动方向
        /// </summary>
        public string HistoryUpLandCode { get; set; }
        /// <summary>
        ///下发路线时的之前运行的上一个地标
        /// </summary>
        public string OldHistoryUpLandCode { get; set; }

        /// <summary>
        /// 是否请求任务完成
        /// </summary>
        public int IsNeedFinshTask { get; set; }

        /// <summary>
        /// 是否请求任务重做
        /// </summary>
        public int IsNeedRedoTask { get; set; }
    }//end
}
