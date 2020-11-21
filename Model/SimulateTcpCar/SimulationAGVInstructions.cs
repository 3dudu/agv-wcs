using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;

namespace Model.SimulateTcpCar
{
    public class SimulationAGVInstructions
    {

        public SimulationAGVInstructions()
        {
            SiteCommandParameter = new SimulateCommand();
        }

        public string LnitiatingCharacterStr
        {
            get
            {
                return "55";
            }
            set
            {
                if (value == "")
                { LnitiatingCharacter = 55; }
            }
        }
        /// <summary>
        /// 起始符
        /// </summary>
        public int LnitiatingCharacter { get; set; }
        /// <summary>
        /// AGVID
        /// </summary>
        public int AGVId { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        public int SerialNumber { get; set; }

        /// <summary>
        /// 响应码
        /// </summary>
        public int ResponseCode { get; set; }

        public string RunningStateStr
        {
            get
            {
                if (RunningState == 0)
                { return "待机停车"; }
                else if (RunningState == 1)
                { return "行驶状态"; }
                else if (RunningState == 2)
                { return "急停触发"; }
                else if (RunningState == 3)
                { return "驱动器故障"; }
                else if (RunningState == 4)
                { return "脱线"; }
                else if (RunningState == 5)
                { return "满线"; }
                else if (RunningState == 6)
                { return "机械防撞触发"; }
                else if (RunningState == 7)
                { return "光电避障传感器触发"; }
                else if (RunningState == 8)
                { return "电量不足"; }
                else if (RunningState == 9)
                { return "内部错误"; }
                else if (RunningState == 10)
                { return "调度指令超时或中断"; }
                else
                { return "未准备"; }
            }
            set
            {
                if (value == "待机停车")
                { RunningState = 0; }
                else if (value == "行驶状态")
                { RunningState = 1; }
                else if (value == "急停触发")
                { RunningState = 2; }
                else if (value == "驱动器故障")
                { RunningState = 3; }
                else if (value == "脱线")
                { RunningState = 4; }
                else if (value == "满线")
                { RunningState = 5; }
                else if (value == "机械防撞触发")
                { RunningState = 6; }
                else if (value == "光电避障传感器触发")
                { RunningState = 7; }
                else if (value == "电量不足")
                { RunningState = 8; }
                else if (value == "内部错误")
                { RunningState = 9; }
                else if (value == "调度指令超时或中断")
                { RunningState = 10; }
                else
                { RunningState = 255; }
            }
        }
        /// <summary>
        /// 运行状态
        /// </summary>
        public int RunningState { get; set; }

        /// <summary>
        /// 物理站点
        /// </summary>
        public int PhysicalSite { get; set; }

        /// <summary>
        /// 里程计数
        /// </summary>
        public int CountMileage { get; set; }

        /// <summary>
        /// 逻辑站点
        /// </summary>
        public int LogicalSite { get; set; }

        public string SensingStateStr
        {
            get
            {
                if (SensingState == 7)
                { return "停车状态下脱线"; }
                if (SensingState == 6)
                { return "电池电压较低"; }
                if (SensingState == 5)
                { return "接近障碍物"; }
                if (SensingState == 4)
                { return "发现障碍物"; }
                else
                { return "光电避障故障 "; }
            }
            set
            {
                if (value == "停车状态下脱线")
                { SensingState = 7; }
                else if (value == "电池电压较低")
                { SensingState = 6; }
                else if (value == "接近障碍物")
                { SensingState = 5; }
                else if (value == "发现障碍物")
                { SensingState = 4; }
                else
                { SensingState = 3; }
            }
        }
        /// <summary>
        /// 传感状态
        /// </summary>
        public int SensingState { get; set; }

        /// <summary>
        /// 当前速度
        /// </summary>
        public int CurrentSpeed { get; set; }

        public string EnentRetreatStr
        {
            get
            {
                if (EnentRetreat == 0)
                {
                    return "前进";
                }
                else
                {
                    return "后退";
                }
            }
            set
            {
                if (value == "前进")
                {
                    EnentRetreat = 0;
                }
                else
                {
                    EnentRetreat = 1;
                }
            }
        }
        /// <summary>
        /// 方向进退
        /// </summary>
        public int EnentRetreat { get; set; }

        public string LeftRightStr
        {
            get
            {
                if (LeftRight == 0)
                { return "左转"; }
                else
                { return "右转"; }
            }
            set
            {
                if (value == "左转")
                { LeftRight = 0; }
                else
                { LeftRight = 1; }
            }
        }
        /// <summary>
        /// 方向左右
        /// </summary>
        public int LeftRight { get; set; }

        public string HookStatusStr
        {
            get
            {
                if (HookStatus == 0)
                {
                    return "未挂接";
                }
                else
                {
                    return "已挂接";
                }
            }
            set
            {
                if (value == "未挂接")
                {
                    HookStatus = 0;
                }
                else
                {
                    HookStatus = 1;
                }
            }
        }
        /// <summary>
        /// 挂钩状态
        /// </summary>
        public int HookStatus { get; set; }

        /// <summary>
        /// 扩展输入
        /// </summary>
        public int ExtendedInput { get; set; }

        /// <summary>
        /// 扩展输出
        /// </summary>
        public int ExtendedOutput { get; set; }

        /// <summary>
        /// 电池电压
        /// </summary>
        public double BatteryVoltage { get; set; }

        /// <summary>
        /// 填充扩展RFID
        /// </summary>
        public int FillExtendedRFID { get; set; }

        /// <summary>
        /// 显示或隐藏查询
        /// </summary>
        public bool DisplayHide { get; set; }

        public string TerminatorStr { get { return "AA"; } set { if (value == "") Terminator = "AA"; } }
        /// <summary>
        /// 结束符
        /// </summary>
        public string Terminator { get; set; }

        public SimulateCommand SiteCommandParameter { get; set; }

        Parameter par = null;

        public delegate void SAGVCar(SimulationAGVInstructions SAGV);
        public event SAGVCar DelegateSAGVCar;

        /// <summary>
        /// 小车移动方法
        /// </summary>
        public void Move()
        {
            try
            {
                #region
                if (SiteCommandParameter != null)
                {
                    foreach (var item in SiteCommandParameter.ParameterList)
                    {
                        //物理站点
                        PhysicalSite = item.Site;

                        //逻辑站点
                        LogicalSite = item.Site;

                        #region 指令码为0(返回0)
                        if (item.Instructions == 0)
                        {
                            continue;
                        }
                        #endregion

                        #region 运行状态(启动8)
                        if (item.Instructions == 8)
                        {
                            RunningState = item.ParameterS;
                        }
                        #endregion
                        
                        #region 方向左右(左右转向9)
                        if (item.Instructions == 9)
                        {
                            if (item.ParameterS == 2)
                            {
                                if (LeftRight == 1)
                                {
                                    LeftRight = 0;
                                }
                                else
                                {
                                    LeftRight = 1;
                                }
                            }
                            else if (item.ParameterS == 1)
                            {
                                LeftRight = 1;
                            }
                            else if (item.ParameterS == 0)
                            {
                                LeftRight = 0;
                            }
                        }
                        #endregion
                        
                        #region 运行状态(停车6)
                        if (item.Instructions == 6)
                        {
                            RunningState = item.ParameterS;
                        }
                        #endregion
                        
                        #region 挂钩动作(0X0A/10)
                        if (item.Instructions == 10)
                        {
                            if (item.ParameterS == 2)
                            {
                                if (HookStatus == 1)
                                {
                                    HookStatus = 0;
                                }
                                else
                                {
                                    HookStatus = 1;
                                }
                            }
                            else if (item.ParameterS == 0)
                            {
                                HookStatus = 0;
                            }
                            else if (item.ParameterS == 1)
                            {
                                HookStatus = 1;
                            }
                        }
                        #endregion
                        
                        #region 速度设定(0x01,速度百分比,1-100)
                        if (item.Instructions == 1)
                        {
                            CurrentSpeed = item.ParameterS;
                        }
                        #endregion
                        delegateState();
                        Thread.Sleep(400);
                    }

                }
                #endregion
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }

        /// <summary>
        /// 执行线程
        /// </summary>
        public void Run(List<byte> msg)
        {
            try
            {
                int CarSite = Convert.ToInt32(Convert.ToString(msg[6], 2).PadLeft(8, '0') + Convert.ToString(msg[5], 2).PadLeft(8, '0'), 2);
                if (msg[4] == 9)
                {
                    foreach (var item in SiteCommandParameter.ParameterList)
                    {
                        if (CarSite == item.Site)
                        {
                            if (item.Instructions == 8)
                            {
                                LogicalSite = CarSite;
                                Thread threads = new Thread(Move);
                                threads.IsBackground = true;
                                threads.Start();
                            }
                        }
                    }
                }

                if (msg[4] == 5)
                {
                    Thread thread = new Thread(Move);
                    thread.IsBackground = true;
                    thread.Start();
                    //LogicalSite = CarSite;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 委托传出当前小车状态
        /// </summary>
        public void delegateState()
        {
            DelegateSAGVCar(this);
        }

        /// <summary>
        /// 获取请求指令
        /// </summary>
        public void GetRequestCommand(List<byte> msg)
        {
            try
            {
                //SimulateCommand SCom = new SimulateCommand();
                if (msg[5] == 0)
                {
                    AGVId = msg[1];
                    int CurrSite = Convert.ToInt32(Convert.ToString(msg[7], 2).PadLeft(8, '0') + Convert.ToString(msg[6], 2).PadLeft(8, '0'), 2);
                    //SiteCommandParameter.Site = CurrSite;
                    for (int i = 8; i < msg.Count; i++)
                    {
                        par = new Parameter();
                        par.Site = CurrSite;
                        par.Instructions = msg[i];
                        i++;
                        par.ParameterS = msg[i];

                        SiteCommandParameter.ParameterList.Add(par);
                        //SCom.ParameterList.Add(par);
                    }
                    return;
                }
                else
                {
                    AGVId = msg[1];
                    PhysicalSite = 1;
                    SiteCommandParameter.ParameterList.Clear();
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
