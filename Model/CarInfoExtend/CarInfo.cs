using Model.MDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.CarInfoExtend
{
    public class CarInfo : CarBaseStateInfo
    {
        public CarInfo() : base()
        {
            WarnBinaryCode = "";
            BeginWaitTime = default(DateTime);
            PreChargeTime = default(DateTime);
            PreCoordinateTime= default(DateTime);
        }

        #region 正常必要属性


        /// <summary>
        /// 判断是否改变
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public bool IsChange(CarBaseStateInfo car)
        {
            if (/*CurrSite != car.CurrSite ||*/ ErrorMessage != car.ErrorMessage || bIsCommBreak != car.bIsCommBreak
                || ((DateTime.Now - PreCoordinateTime).TotalMilliseconds >500) || CarState != car.CarState)
            {
                return true;
            }
            else
            { return false; }
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public void GetValue(CarBaseStateInfo car)
        {
            X = car.X;
            Y = car.Y;
            JCState = car.JCState;
            speed = car.speed;
            Angel = car.Angel;
            fVolt = car.fVolt;
            PBSValue = car.PBSValue;
            CurrSite = car.CurrSite;
            CarState = car.CarState;
            bIsCommBreak = car.bIsCommBreak;
            IsUpLand = car.IsUpLand;
            IsUpQCode = car.IsUpQCode;
            ErrorMessage = car.ErrorMessage;
            BangState = car.BangState;
            IsNeedFinshTask = car.IsNeedFinshTask;
            IsNeedRedoTask = car.IsNeedRedoTask;
        }

        /// <summary>
        /// 异常类别
        /// </summary>
        public int WarnType { get; set; }

        public string WarnTypeStr
        {
            get
            {
                switch (WarnType)
                {
                    case 0:
                        return "24C02错误";
                    case 1:
                        return "电量检测异常";
                    case 2:
                        return "WiFi通信异常";
                    case 3:
                        return "433M通信异常";
                    case 5:
                        return "PGV100异常";
                    case 6:
                        return "二维码导航异常";
                    case 7:
                        return "里程计异常";
                    case 8:
                        return "电机异常";
                    case 10:
                        return "安全传感器异常";
                    default:
                        return "";
                }
            }
        }

        /// <summary>
        /// 异常二进制编码
        /// </summary>
        public string WarnBinaryCode { get; set; }
        #endregion

        #region 通用逻辑控制属性
        /// <summary>
        /// 是否在等待
        /// </summary>
        public bool IsWait { get; set; }

        /// <summary>
        /// 需要等待的时间
        /// </summary>
        public double WaitTime { get; set; }

        /// <summary>
        /// 开始等待的时间
        /// </summary>
        public DateTime BeginWaitTime { get; set; }

        /// <summary>
        /// 是否被交管
        /// </summary>
        public bool IsLock { get; set; }

        /// <summary>
        /// 上次充电时间
        /// </summary>
        public DateTime PreChargeTime { get; set; }

        /// <summary>
        /// 上次推送坐标时间
        /// </summary>
        public DateTime PreCoordinateTime { get; set; }
        #endregion

        #region 额外特殊属性
        /// <summary>
        /// 加水车水量
        /// </summary>
        public double WaterAmount { get; set; }
        #endregion

    }
}
