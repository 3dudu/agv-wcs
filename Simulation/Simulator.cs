using AGVDAccess;
using AGVModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Simulation
{
    public class Simulator
    {
        IList<CarInfo> AllCar = new List<CarInfo>();//存放所有的小车
        static List<CarMonitor> MoniCars = new List<CarMonitor>();
        static IList<SegmentResInfo> Segments = new List<SegmentResInfo>();//存放所有线段
        static IList<AllSegment> AllSegs = new List<AllSegment>();//所有线段
        static IList<LandmarkInfo> AllLands = new List<LandmarkInfo>();
        public Queue TaskQueue = new Queue();//任务队列
        public delegate void CarMove(object sender);
        public delegate void RouteIni(object sender);
        public event RouteIni Route_Ini;
        public event CarMove Car_Move;
        private System.Timers.Timer timerStarBeStopedCar = new System.Timers.Timer(1000 * 1);
        public static readonly object lockStopObj = new object();
        public static readonly object lockStartObj = new object();
        private List<LockResource> lockResource = new List<LockResource>();


        public bool Inital()
        {
            try
            {
                MoniCars.Clear();
                AllCar = AGVClientDAccess.LoadAGVAchive();
                foreach (CarInfo item in AllCar)
                {
                    CarMonitor moniCar = new CarMonitor();
                    moniCar.CarCode = item.AgvID;
                    MoniCars.Add(moniCar);
                }
                Segments = AGVClientDAccess.LoadAllSegments();
                AllSegs = AGVClientDAccess.LoadAllSegment();
                AllLands = AGVClientDAccess.LoadLandByCondition("1=1");
                timerStarBeStopedCar.Enabled = true;
                timerStarBeStopedCar.AutoReset = true;
                timerStarBeStopedCar.Elapsed += TimerStarBeStopedCar_Elapsed;
                return true;
            }
            catch (Exception ex)
            { return false; throw ex; }
        }

        public static void IniLogicalDate()
        {
            try
            {
                Segments = AGVClientDAccess.LoadAllSegments();
            }
            catch (Exception ex)
            { throw ex; }
        }


        /// <summary>
        /// 创建任务
        /// </summary>
        public void CreatTask(LandmarkInfo StartLand, LandmarkInfo endLand)
        {
            try
            {
                List<LandmarkInfo> Task1 = new List<LandmarkInfo>();
                //LandmarkInfo StartLandd = AllLands.FirstOrDefault(p=>p.LandmarkCode=="102");
                //LandmarkInfo endLandd = AllLands.FirstOrDefault(p => p.LandmarkCode == "12");
                Task1.Add(StartLand);
                Task1.Add(endLand);
                TaskQueue.Enqueue(Task1);
                //Thread.Sleep(200);
                //List<LandmarkInfo> Task2 = new List<LandmarkInfo>();
                //LandmarkInfo StartLandd2 = AllLands.FirstOrDefault(p => p.LandmarkCode == "104");
                //LandmarkInfo endLandd2 = AllLands.FirstOrDefault(p => p.LandmarkCode == "3");
                //Task2.Add(StartLandd2);
                //Task2.Add(endLandd2);
                //TaskQueue.Enqueue(Task2);
                AssignmentTask();
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 分派任务
        /// </summary>
        public void AssignmentTask()
        {
            try
            {
                if (TaskQueue.Count <= 0) { return; }
                CarMonitor DoCar = MoniCars.Where(p => p.Sate == 0).FirstOrDefault();
                if (DoCar != null)
                {
                    List<LandmarkInfo> Task = TaskQueue.Dequeue() as List<LandmarkInfo>;
                    RoutePlanData PlanRoute = new RoutePlanData(Segments);
                    DoCar.CurrRoute = PlanRoute.CreateDeepCopy<List<LandmarkInfo>>(AcountRoute(Task[0], Task[1]));
                    if (DoCar.CurrRoute.Count <= 0) return;
                    if (Route_Ini != null)
                    { Route_Ini(DoCar.CurrRoute); }
                    DoCar.StepChange += DoCar_StepChange;
                    DoCar.Start();
                    if (!SameHandleTrafficForStop(DoCar))
                    {
                        //启动的时候需要把当前车的锁定资源集恢复正常
                        GetTrafficResour(DoCar);
                    }
                }

            }
            catch (Exception ex)
            { throw ex; }
        }

        private void DoCar_StepChange(object sender)
        {
            try
            {
                if (Car_Move != null)
                { Car_Move(sender); }
                CarMonitor car = sender as CarMonitor;
                if (car != null)
                {
                    HandleTrafficForStop(sender as CarMonitor);
                    //new Thread(HandleTrafficForStop) { IsBackground = true }.Start(sender);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 动态计算路线
        /// </summary>
        public List<LandmarkInfo> AcountRoute(LandmarkInfo StarLand, LandmarkInfo EndLand)
        {
            try
            {
                //List<List<LandmarkInfo>> hs_route = new List<List<LandmarkInfo>>();
                RoutePlanData PlanRoute = new RoutePlanData(Segments);
                List<LandmarkInfo> route = PlanRoute.GetRoute(StarLand, EndLand);
                if (route.Count <= 0) { return route; }
                List<LandmarkInfo> TurnLand = route.Where(p => p.sway != SwayEnum.None).ToList();
                int PreAngle = route[0].Angle;
                foreach (LandmarkInfo item in TurnLand)
                {
                    if (item.Angle != PreAngle)
                    { item.IsRotateLand = true; }
                    PreAngle = item.Angle;
                }
                //PlanRoute.GetRouteStr(route);
                return route;
                //List<LandmarkInfo> routeCopy = PlanRoute.CreateDeepCopy<List<LandmarkInfo>>(route);
                //List<LandmarkInfo> HasTow = routeCopy.Where(p => p.IsHasTwoChildLand).ToList();
                //foreach (LandmarkInfo item in HasTow)
                //{
                //    if (item.IsHasTwoChildLand)
                //    {
                //        int index = routeCopy.FindIndex(p => p.LandmarkCode == item.LandmarkCode);
                //        if (index != 0)
                //        {
                //            PlanRoute.RouteList.Clear();
                //            PlanRoute.CloseList.Clear();
                //            List<LandmarkInfo> TemRouteHead = routeCopy.GetRange(0, index);
                //            List<LandmarkInfo> TempRoute = PlanRoute.GetRoute(item, EndLand);
                //            List<LandmarkInfo> itemRoute = TemRouteHead.Concat(TempRoute).ToList();
                //            hs_route.Add(itemRoute);
                //        }
                //    }
                //}
                //List<LandmarkInfo> Routes = (from a in hs_route
                //                             where a.Where(p => p.LandmarkCode == EndLand.LandmarkCode).Count() > 0
                //                             orderby a.Count ascending
                //                             select a).FirstOrDefault();
                //return Routes;
            }
            catch (Exception ex)
            { throw ex; }
        }

        #region 模拟车辆内部类
        /// <summary>
        /// 模拟车辆类
        /// </summary>
        public class CarMonitor
        {
            public CarMonitor()
            {
                IsLock = false;
                Sate = 0;
                CurrLand = new LandmarkInfo();
                NextLand = new LandmarkInfo();
                CurrRoute = new List<LandmarkInfo>();
                RouteLands = new List<string>();
                TurnLands = new List<string>();
                LockLand = new List<LandmarkInfo>();
                CurrSite = -1;
                CarActive_Timer = new System.Timers.Timer() { Enabled = false };
                CarActive_Timer.Elapsed += new System.Timers.ElapsedEventHandler(CarActive);
                CarActive_Timer.Interval = 2000;
            }

            public bool IsLock { get; set; }

            public int CarCode { get; set; }

            public LandmarkInfo CurrLand { get; set; }
            public int CurrSite { get; set; }

            public LandmarkInfo NextLand { get; set; }

            public List<LandmarkInfo> LockLand { get; set; }

            public CarMonitor LockCar { get; set; }

            /// <summary>
            /// 0,待分配 1，运行中 2，停止
            /// </summary>
            public int Sate { get; set; }

            public List<LandmarkInfo> CurrRoute { get; set; }

            public List<string> RouteLands { get; set; }

            public List<string> TurnLands { get; set; }


            public delegate void CarStepChange(object sender);
            public event CarStepChange StepChange;
            public System.Timers.Timer CarActive_Timer;
            public static object locker = new object();
            public void Move()
            {
                try
                {
                    lock (locker)
                    {
                        if (CurrRoute.Count() > 0)
                        {
                            if (!CarActive_Timer.Enabled) { return; }
                            CurrRoute = CurrRoute.Distinct().ToList<LandmarkInfo>();
                            LockLand.Clear();
                            if (CurrSite.ToString() == CurrRoute.Last().LandmarkCode)
                            {
                                CurrRoute.Clear();
                                Sate = 0;
                                return;
                            }
                            if (CurrSite == -1)
                            {
                                CurrLand = CurrRoute[0];
                                CurrSite = Convert.ToInt32(CurrLand.LandmarkCode);
                                if (CurrRoute.Count > 1)
                                {
                                    NextLand = CurrRoute[1];
                                }
                                else
                                { NextLand = null; }
                            }
                            else
                            {
                                int index = CurrRoute.FindIndex(p => p.LandmarkCode == CurrSite.ToString());
                                if (index + 1 >= 0 && index + 1 < CurrRoute.Count)
                                {
                                    CurrLand = CurrRoute[index + 1];
                                    CurrSite = Convert.ToInt32(CurrLand.LandmarkCode);
                                }
                                if (index + 2 >= 0 && index + 2 < CurrRoute.Count)
                                {
                                    NextLand = CurrRoute[index + 2];
                                }
                                else
                                { NextLand = null; }
                            }
                            //CurrLand = CurrRoute[i];
                            //CurrSite = Convert.ToInt32(CurrLand.LandmarkCode);


                            //LockLand.Add(CurrLand);

                            //List<SegmentResInfo> associateSegs = (from a in Segments
                            //                                      where a.StartLandMark.LandmarkCode == CurrLand.LandmarkCode || a.EndLandMark.LandmarkCode == CurrLand.LandmarkCode
                            //                                      select a).ToList<SegmentResInfo>();

                            //foreach (SegmentResInfo item in associateSegs)
                            //{
                            //    if (LockLand.Where(p => p.LandmarkCode == item.StartLandMark.LandmarkCode).Count() <= 0)
                            //    { LockLand.Add(item.StartLandMark); }
                            //    if (LockLand.Where(p => p.LandmarkCode == item.EndLandMark.LandmarkCode).Count() <= 0)
                            //    { LockLand.Add(item.EndLandMark); }
                            //}
                            //if (CurrRoute.Count > 1)
                            //{
                            //    NextLand = CurrRoute[1];
                            //LockLand.Add(NextLand);
                            //List<SegmentResInfo> associateSegs = (from a in Segments
                            //                                      where a.StartLandMark.LandmarkCode == NextLand.LandmarkCode || a.EndLandMark.LandmarkCode == NextLand.LandmarkCode
                            //                                      select a).ToList<SegmentResInfo>();

                            //foreach (SegmentResInfo item in associateSegs)
                            //{
                            //    if (LockLand.Where(p => p.LandmarkCode == item.StartLandMark.LandmarkCode).Count() <= 0)
                            //    { LockLand.Add(item.StartLandMark); }
                            //    if (LockLand.Where(p => p.LandmarkCode == item.EndLandMark.LandmarkCode).Count() <= 0)
                            //    { LockLand.Add(item.EndLandMark); }
                            //}
                            //}
                            //else
                            //{ NextLand = null; }

                            //CarMonitor lockcars = MoniCars.Where(p => p.CarCode != CarCode
                            //&& p.LockLand.Intersect(LockLand).ToList().Count() > 0).FirstOrDefault();

                            //if (lockcars != null)
                            //{
                            //    LockCar = lockcars;
                            //    lockcars.Stop();
                            //}
                            //else
                            //{
                            //    if (LockCar != null)
                            //    {
                            //        LockCar.Start();
                            //        LockCar = null;
                            //    }
                            //}

                            Sate = 1;
                            if (StepChange != null)
                            { StepChange(this); }
                            //CurrRoute.RemoveAt(0);
                        }
                    }
                }
                catch (Exception ex)
                { throw ex; }
            }


            public void Run()
            {
                try
                {
                    Thread thread = new Thread(Move);
                    thread.IsBackground = true;
                    thread.Start();
                }
                catch (Exception ex)
                { throw ex; }
            }


            /// <summary>
            /// 停车
            /// </summary>
            public void Stop()
            {
                CarActive_Timer.Enabled = false;
                Sate = 2;
            }

            /// <summary>
            /// 启动
            /// </summary>
            public void Start()
            {
                try
                {
                    CarActive_Timer.Enabled = true;
                }
                catch (Exception ex)
                { throw ex; }
            }


            public void CarActive(object sender, System.Timers.ElapsedEventArgs e)
            {
                try
                {
                    this.CarActive_Timer.Enabled = false;
                    Run();
                }
                catch (Exception ex)
                { }
                finally
                {
                    this.CarActive_Timer.Enabled = true;
                }
            }

        }
        #endregion



        private void TimerStarBeStopedCar_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                timerStarBeStopedCar.Enabled = false;
                new Thread(new ThreadStart(HandleTrafficForStart)) { IsBackground = true }.Start();
            }
            catch (Exception ex)
            {
                //DelegateState.InvokeDispatchStateEvent("定时启动被交通车辆异常:" + ex.Message);
            }
            finally
            { timerStarBeStopedCar.Enabled = true; }
        }


        #region 交通管制
        /// <summary>
        /// 管制后只发启动
        /// </summary>
        /// <param name="CurrCar"></param>
        public void HandleTrafficForStart()
        {
            try
            {
                lock (lockStartObj)
                {
                    RoutePlanData PlanRoute = new RoutePlanData(Segments);
                    List<LockResource> CurrCarBlock = PlanRoute.CreateDeepCopy<List<LockResource>>(lockResource);
                    foreach (LockResource Block in CurrCarBlock)
                    {
                        CarMonitor CurrCar = MoniCars.FirstOrDefault(p => p.CarCode == Block.BeLockCarID);
                        if (CurrCar == null) { continue; }
                        #region//将停止的车的资源集处理
                        //1 前进资源只添加当前位置
                        //2 如果当前停止的点是自己的旋转点那么添加旋转集
                        //List<string> newRouteLand = PlanRoute.CreateDeepCopy<List<string>>(CurrCar.RouteLands);
                        //if (CurrCar.RouteLands.Count > 2)
                        //{
                        //    CurrCar.RouteLands.Clear();
                        //    CurrCar.RouteLands.Add(newRouteLand[0]);
                        //    CurrCar.RouteLands.Add(newRouteLand[1]);
                        //}

                        //CurrCar.RouteLands.Clear();
                        //CurrCar.RouteLands.Add(CurrCar.CurrSite.ToString());
                        CurrCar.TurnLands.Clear();
                        LandmarkInfo Land = null;
                        if (CurrCar.CurrRoute.Count > 0)
                        { Land = AllLands.FirstOrDefault(q => q.LandmarkCode == CurrCar.CurrSite.ToString()); }
                        if (Land != null && Land.IsRotateLand)
                        {
                            double TurnLen = 130;
                            double ScalingRateLen = 90;
                            List<LandmarkInfo> CloseLands = (from a in AllLands
                                                             where Math.Round(getDistant(a.LandX, a.LandY, Land.LandX, Land.LandY) * ScalingRateLen, 3) <= Math.Round(TurnLen, 3)
                                                             select a).ToList();
                            foreach (LandmarkInfo Copy in CloseLands)
                            {
                                if (CurrCar.TurnLands.Where(p => p == Copy.LandmarkCode).Count() <= 0)
                                {
                                    CurrCar.TurnLands.Add(Copy.LandmarkCode);
                                }
                            }
                        }
                        #endregion
                        WriteLog("启动管制逻辑车" + CurrCar.CarCode.ToString() + "行走资源集合:" + string.Join(",", CurrCar.RouteLands.Select(p => p)));
                        WriteLog("启动管制逻辑车" + CurrCar.CarCode.ToString() + "旋转资源集合:" + string.Join(",", CurrCar.TurnLands.Select(p => p)));
                        #region New 
                        if (!SameHandleTrafficForStop(CurrCar))
                        {
                            //把启动后的车辆在锁资源中清除
                            List<LockResource> CurrCarLocks = lockResource.Where(p => p.BeLockCarID == CurrCar.CarCode).ToList();
                            foreach (LockResource item in CurrCarLocks)
                            {
                                int index = lockResource.FindIndex(p => p.BeLockCarID == item.BeLockCarID);
                                lockResource.RemoveAt(index);
                            }
                            GetTrafficResour(CurrCar);
                            CurrCar.Start();
                            //Commer.AddControl(CurrCar.AgvID, new CommandToValue(AGVCommandEnum.Start));
                            CurrCar.IsLock = false;
                            //LogHelper.WriteLog("车:" + CurrCar.AgvID.ToString() + "在站点" + CurrCar.CurrSite.ToString() + "启动");
                        }
                        else
                        {
                            //如果当前车停在旋转点上，那么试图让其走一个，然后有交通管制让其再停下来
                            int CurrSiteIndex = CurrCar.CurrRoute.FindIndex(p => p.LandmarkCode == CurrCar.CurrSite.ToString());
                            if (CurrSiteIndex + 1 < CurrCar.CurrRoute.Count)
                            {
                                LandmarkInfo CurrLand = CurrCar.CurrRoute.FirstOrDefault(p => p.LandmarkCode == CurrCar.CurrSite.ToString());
                                LandmarkInfo NextLand = CurrCar.CurrRoute[CurrSiteIndex + 1];
                                if (CurrLand != null && NextLand != null && CurrLand.IsRotateLand && MoniCars.FirstOrDefault(p => p.CarCode != CurrCar.CarCode && p.CurrSite.ToString() == NextLand.LandmarkCode) == null)
                                {
                                    //把启动后的车辆在锁资源中清除
                                    List<LockResource> CurrCarLocks = lockResource.Where(p => p.BeLockCarID == CurrCar.CarCode).ToList();
                                    foreach (LockResource item in CurrCarLocks)
                                    {
                                        int index = lockResource.FindIndex(p => p.BeLockCarID == item.BeLockCarID);
                                        lockResource.RemoveAt(index);
                                    }
                                    WriteLog("停在了自己的旋转点上:" + CurrCar.CurrSite.ToString() + ",往前挪一格");
                                    CurrCar.Start();
                                    CurrCar.IsLock = false;
                                }
                                else if (CurrLand != null && NextLand != null && MoniCars.FirstOrDefault(q => q.CarCode != CurrCar.CarCode&&q.TurnLands.Contains(CurrCar.CurrSite.ToString()) && !q.TurnLands.Contains(q.CurrSite.ToString())) !=null && MoniCars.FirstOrDefault(p => p.CarCode!= CurrCar.CarCode&& p.CurrSite.ToString() == NextLand.LandmarkCode) == null)
                                {
                                    //把启动后的车辆在锁资源中清除
                                    List<LockResource> CurrCarLocks = lockResource.Where(p => p.BeLockCarID == CurrCar.CarCode).ToList();
                                    foreach (LockResource item in CurrCarLocks)
                                    {
                                        int index = lockResource.FindIndex(p => p.BeLockCarID == item.BeLockCarID);
                                        lockResource.RemoveAt(index);
                                    }
                                    WriteLog("停在了别人的旋转集合中:" + CurrCar.CurrSite.ToString() + ",往前挪一格");
                                    CurrCar.Start();
                                    CurrCar.IsLock = false;
                                }
                            }
                        }
                        #endregion
                    }//endFor
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog("交通管制启动异常:" + ex.Message);
            }
        }

        /// <summary>
        /// 管制后只发停止
        /// </summary>
        /// <param name="CurrCar"></param>
        public void HandleTrafficForStop(object obj)
        {
            try
            {
                lock (lockStopObj)
                {
                    if (obj == null) { return; }
                    CarMonitor CurrCar = obj as CarMonitor;
                    if (CurrCar == null) { return; }
                    WriteLog("停止管制逻辑车" + CurrCar.CarCode.ToString() + "行走资源集合:" + string.Join(",", CurrCar.RouteLands.Select(p => p)));
                    WriteLog("停止管制逻辑车" + CurrCar.CarCode.ToString() + "旋转资源集合:" + string.Join(",", CurrCar.TurnLands.Select(p => p)));
                    #region New
                    GetTrafficResour(CurrCar);
                    List<string> lands = CurrCar.RouteLands;
                    if (CheckIsStop(CurrCar))
                    {
                        //Commer.AddControl(CurrCar.AgvID, new CommandToValue(AGVCommandEnum.Stop));
                        CurrCar.Stop();
                        CurrCar.IsLock = true;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {

            }
        }


        private void WriteLog(string Mes)
        {
            try
            {
                File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + @"\CoreLog\" + "Log" + DateTime.Now.ToString("yyyyMMddHH") + ".txt", DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss") + " " + Mes + Environment.NewLine);
            }
            catch
            { }
        }

        private bool CheckIsStop(CarMonitor CurrCar)
        {
            try
            {
                //1 先判断同向，即我当前行走路线资源集中是否有车辆的当前位置
                //2 判断是否路线交叉，即我当前的行走路线资源集有交集的
                //3 判断是否有旋转，即我的行走资源中是否有其他车的旋转资源
                //4 判断管制区域内是否有车

                //判断同向跟车
                CarMonitor HasSamDeriAnotherCar = (from a in MoniCars
                                                   where a.CarCode != CurrCar.CarCode && CurrCar.RouteLands.Where(q => q == a.CurrSite.ToString()).Count() > 0
                                                   select a).FirstOrDefault();

                //判断是否路线交叉
                CarMonitor HasIntersectAnotherCar = (from a in MoniCars
                                                     where a.CarCode != CurrCar.CarCode && CurrCar.RouteLands.Intersect(a.RouteLands).Count() > 0
                                                     //&& lockResource.Count(p => p.BeLockCarID == a.CarCode && p.LockCarID == CurrCar.CarCode) <= 0
                                                     orderby CurrCar.RouteLands.Intersect(a.RouteLands).Count() descending
                                                     select a).FirstOrDefault();

                //判断是否有旋转交集  排除了被我锁住的
                CarMonitor HasTurnAnotherCar = (from a in MoniCars
                                                where a.CarCode != CurrCar.CarCode && CurrCar.RouteLands.Intersect(a.TurnLands).Count() > 0
                                                //&& lockResource.Count(p => p.BeLockCarID == a.CarCode && p.LockCarID == CurrCar.CarCode) <= 0
                                                select a).FirstOrDefault();

                //判断我的旋转资源中是否有其他车  排除了被我锁住的
                CarMonitor MyTurnHasAnotherCar = (from a in MoniCars
                                                  where a.CarCode != CurrCar.CarCode && CurrCar.TurnLands.Contains(a.CurrSite.ToString())
                                                  //&& lockResource.Count(p => p.BeLockCarID == a.CarCode && p.LockCarID == CurrCar.CarCode) <= 0
                                                  select a).FirstOrDefault();


                if (HasSamDeriAnotherCar != null)//同向必须停
                {
                    if (lockResource.Where(p => p.LockCarID == HasSamDeriAnotherCar.CarCode && p.BeLockCarID == CurrCar.CarCode).Count() <= 0)
                    {
                        LockResource LockBlock = new LockResource();
                        LockBlock.BeLockCarID = CurrCar.CarCode;
                        LockBlock.LockCarID = HasSamDeriAnotherCar.CarCode;
                        lockResource.Add(LockBlock);
                    }
                    WriteLog("跟车:" + CurrCar.CarCode.ToString() + "在站点" + CurrCar.CurrSite.ToString() + "停,车" + HasSamDeriAnotherCar.CarCode.ToString() + "在站点" + HasSamDeriAnotherCar.CurrSite.ToString() + "走");
                    return true;
                }

                if (MyTurnHasAnotherCar != null)
                {
                    if (lockResource.Where(p => p.LockCarID == MyTurnHasAnotherCar.CarCode && p.BeLockCarID == CurrCar.CarCode).Count() <= 0)
                    {
                        LockResource LockBlock = new LockResource();
                        LockBlock.BeLockCarID = CurrCar.CarCode;
                        LockBlock.LockCarID = MyTurnHasAnotherCar.CarCode;
                        lockResource.Add(LockBlock);
                    }
                    WriteLog("我的旋转资源中有车:" + CurrCar.CarCode.ToString() + "在站点" + CurrCar.CurrSite.ToString() + "停,车" + MyTurnHasAnotherCar.CarCode.ToString() + "在站点" + MyTurnHasAnotherCar.CurrSite.ToString() + "走");
                    return true;
                }


                if (HasTurnAnotherCar != null)
                {
                    ////如果发现我的行走路线中有需要旋转的车辆,并且旋转的车已经在旋转集合中那么不需要判断对方是否暂停,必须自己停
                    if (HasTurnAnotherCar.TurnLands.Contains(HasTurnAnotherCar.CurrSite.ToString()))
                    {
                        if (lockResource.Where(p => p.LockCarID == HasTurnAnotherCar.CarCode && p.BeLockCarID == CurrCar.CarCode).Count() <= 0)
                        {
                            LockResource LockBlock = new LockResource();
                            LockBlock.BeLockCarID = CurrCar.CarCode;
                            LockBlock.LockCarID = HasTurnAnotherCar.CarCode;
                            lockResource.Add(LockBlock);
                        }
                        WriteLog("有旋转且在旋转半径内车:" + CurrCar.CarCode.ToString() + "在站点" + CurrCar.CurrSite.ToString() + "停,车" + HasTurnAnotherCar.CarCode.ToString() + "在站点" + HasTurnAnotherCar.CurrSite.ToString() + "走");
                        return true;
                    }
                    //判断对方是否被我锁住的
                    //if (lockResource.Count(p => p.BeLockCarID == HasTurnAnotherCar.CarCode && p.LockCarID == CurrCar.CarCode) <= 0)
                    //{
                    if (lockResource.Where(p => p.LockCarID == HasTurnAnotherCar.CarCode && p.BeLockCarID == CurrCar.CarCode).Count() <= 0)
                    {
                        LockResource LockBlock = new LockResource();
                        LockBlock.BeLockCarID = CurrCar.CarCode;
                        LockBlock.LockCarID = HasTurnAnotherCar.CarCode;
                        lockResource.Add(LockBlock);
                    }
                    return true;
                    //}
                    //else
                    //{ WriteLog("车:" + HasTurnAnotherCar.CarCode.ToString() + "已被我:" + CurrCar.CarCode.ToString() + "锁了"); }
                }

                if (HasIntersectAnotherCar != null)
                {
                    List<string> CurrInsect = CurrCar.RouteLands.Intersect(HasIntersectAnotherCar.RouteLands).ToList();
                    List<string> AnotherInsect= HasIntersectAnotherCar.RouteLands.Intersect(CurrCar.RouteLands).ToList();
                    if (CurrInsect.Count > 0 && AnotherInsect.Count > 0)
                    {
                        int CurrInsectIndex = CurrCar.RouteLands.FindIndex(q => q == CurrInsect[0]);
                        int AnotherInsectIndex= HasIntersectAnotherCar.RouteLands.FindIndex(q => q == AnotherInsect[0]);
                        if (AnotherInsectIndex >= CurrInsectIndex)//他车停
                        {
                            if (lockResource.Where(p => p.LockCarID == CurrCar.CarCode && p.BeLockCarID == HasIntersectAnotherCar.CarCode).Count() <= 0)
                            {
                                LockResource LockBlock = new LockResource();
                                LockBlock.BeLockCarID = HasIntersectAnotherCar.CarCode;
                                LockBlock.LockCarID = CurrCar.CarCode;
                                lockResource.Add(LockBlock);
                                HasIntersectAnotherCar.Stop();
                                HasIntersectAnotherCar.IsLock = true;
                            }
                        }
                        else
                        {
                            if (lockResource.Where(p => p.LockCarID == HasIntersectAnotherCar.CarCode && p.BeLockCarID == CurrCar.CarCode).Count() <= 0)
                            {
                                LockResource LockBlock = new LockResource();
                                LockBlock.BeLockCarID = CurrCar.CarCode;
                                LockBlock.LockCarID = HasIntersectAnotherCar.CarCode;
                                lockResource.Add(LockBlock);
                            }

                            WriteLog("交叉车:" + CurrCar.CarCode.ToString() + "在站点" + CurrCar.CurrSite.ToString() + "停,车" + HasIntersectAnotherCar.CarCode.ToString() + "在站点" + HasIntersectAnotherCar.CurrSite.ToString() + "走");
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog("交通管制异常:" + ex.Message);
                return false;
            }
        }


        public void GetTrafficResour(CarMonitor CurrCar)
        {
            try
            {
                if (CurrCar == null) { return; }
                CurrCar.RouteLands.Clear();
                CurrCar.TurnLands.Clear();
                RoutePlanData PlanRoute = new RoutePlanData(Segments);
                List<string> TempRouteLands = new List<string>();
                List<string> TempTurnLands = new List<string>();
                double StopLen = 90;
                double TrafficLen = 270;
                double TotalStopLenth = StopLen + TrafficLen;//需要发送停车距离
                double TurnLen = 130;
                double ScalingRateLen = 90;

                //如果当前agv没有路线的话就把当前的站点放入到自己的行走资源集合中
                if (CurrCar.CurrRoute.Count <= 0)
                { return; }
                int CurrSiteIndex = CurrCar.CurrRoute.FindIndex(p => p.LandmarkCode == CurrCar.CurrSite.ToString());
                if (CurrSiteIndex < 0) { return; }

                List<LandmarkInfo> TrafficLand = new List<LandmarkInfo>();
                //通过管制距离的系统参数来计算需要锁定的地标集合
                double ResourLenth = 0;
                double UpResourLenth = 0;
                for (int i = CurrSiteIndex + 1; i < CurrCar.CurrRoute.Count; i++)
                {
                    AllSegment segment = AllSegs.Where(p => p.BeginLandMakCode == (CurrCar.CurrRoute[i - 1]).LandmarkCode && p.EndLandMarkCode == (CurrCar.CurrRoute[i]).LandmarkCode).FirstOrDefault();
                    if (segment == null) { continue; }
                    ResourLenth += segment.Length;
                    if (UpResourLenth <= TotalStopLenth && ResourLenth >= TotalStopLenth)
                    {
                        if (TempRouteLands.Where(p => p == (CurrCar.CurrRoute[i]).LandmarkCode).Count() <= 0 && CurrCar.CurrRoute[i].LandmarkCode != CurrCar.CurrSite.ToString())
                        {
                            TempRouteLands.Add(CurrCar.CurrRoute[i].LandmarkCode);
                            TrafficLand.Add(PlanRoute.CreateDeepCopy<LandmarkInfo>(CurrCar.CurrRoute[i]));
                        }
                        break;
                    }
                    else
                    {
                        if (TempRouteLands.Where(p => p == (CurrCar.CurrRoute[i - 1]).LandmarkCode).Count() <= 0 && CurrCar.CurrRoute[i - 1].LandmarkCode != CurrCar.CurrSite.ToString())
                        {
                            TempRouteLands.Add(CurrCar.CurrRoute[i - 1].LandmarkCode);
                            TrafficLand.Add(PlanRoute.CreateDeepCopy<LandmarkInfo>(CurrCar.CurrRoute[i - 1]));
                        }
                        if (TempRouteLands.Where(p => p == (CurrCar.CurrRoute[i]).LandmarkCode).Count() <= 0 && CurrCar.CurrRoute[i].LandmarkCode != CurrCar.CurrSite.ToString())
                        {
                            TempRouteLands.Add(CurrCar.CurrRoute[i].LandmarkCode);
                            TrafficLand.Add(PlanRoute.CreateDeepCopy<LandmarkInfo>(CurrCar.CurrRoute[i]));
                        }
                    }
                    UpResourLenth += segment.Length;
                }

                LandmarkInfo ld = CurrCar.CurrRoute.FirstOrDefault(q => q.LandmarkCode == CurrCar.CurrSite.ToString());
                if (ld != null && TrafficLand.Where(p => p.LandmarkCode == ld.LandmarkCode).Count() <= 0)
                { TrafficLand.Add(PlanRoute.CreateDeepCopy<LandmarkInfo>(ld)); }

                //计算路线资源集中的旋转资源集
                LandmarkInfo Land = (from a in TrafficLand
                                     where a.IsRotateLand
                                     orderby getDistant(a.LandX, a.LandY, ld.LandX, ld.LandY) ascending
                                     select a).FirstOrDefault();
                if (Land != null && Land.IsRotateLand)
                {
                    //根据旋转半径找到范围内的地标集
                    List<LandmarkInfo> CloseLands = (from a in AllLands
                                                     where Math.Round(getDistant(a.LandX, a.LandY, Land.LandX, Land.LandY) * ScalingRateLen, 3) <= Math.Round(TurnLen, 3)
                                                     select a).ToList();
                    foreach (LandmarkInfo Copy in CloseLands)
                    {
                        if (TempTurnLands.Where(p => p == Copy.LandmarkCode).Count() <= 0)
                        {
                            TempTurnLands.Add(Copy.LandmarkCode);
                        }
                    }
                }
                CurrCar.RouteLands = TempRouteLands;
                CurrCar.TurnLands = TempTurnLands;
            }
            catch (Exception ex)
            { }
        }



        private double getDistant(double X1, double Y1, double X2, double Y2)
        {
            try
            {
                double distant = Math.Sqrt(Math.Pow(Math.Abs(X1 - X2), 2) + Math.Pow(Math.Abs(Y1 - Y2), 2));
                return distant;
            }
            catch (Exception ex)
            { throw ex; }
        }


        //单独计算当前车的行走资源集合
        public Hashtable SameGetRouteLandResource(CarMonitor CurrCar)
        {
            try
            {
                if (CurrCar == null)
                { return null; }
                else
                {
                    List<string> TempRouteLands = new List<string>();
                    List<string> TempTurnLands = new List<string>();
                    RoutePlanData PlanRoute = new RoutePlanData(Segments);
                    double StopLen = 90;
                    double TrafficLen = 270;
                    double TotalStopLenth = StopLen + TrafficLen;//需要发送停车距离
                    double TurnLen = 130;
                    double ScalingRateLen = 90;

                    //如果当前agv没有路线的话就把当前的站点放入到自己的行走资源集合中
                    if (CurrCar.CurrRoute.Count <= 0)
                    { return null; }
                    int CurrSiteIndex = CurrCar.CurrRoute.FindIndex(p => p.LandmarkCode == CurrCar.CurrSite.ToString());
                    if (CurrSiteIndex < 0) { return null; }

                    List<LandmarkInfo> TrafficLand = new List<LandmarkInfo>();
                    //通过管制距离的系统参数来计算需要锁定的地标集合
                    double ResourLenth = 0;
                    double UpResourLenth = 0;
                    for (int i = CurrSiteIndex + 1; i < CurrCar.CurrRoute.Count; i++)
                    {
                        AllSegment segment = AllSegs.Where(p => p.BeginLandMakCode == (CurrCar.CurrRoute[i - 1]).LandmarkCode && p.EndLandMarkCode == (CurrCar.CurrRoute[i]).LandmarkCode).FirstOrDefault();
                        if (segment == null) { continue; }
                        ResourLenth += segment.Length;
                        if (UpResourLenth <= TotalStopLenth && ResourLenth >= TotalStopLenth)
                        {
                            if (TempRouteLands.Where(p => p == (CurrCar.CurrRoute[i]).LandmarkCode).Count() <= 0 && CurrCar.CurrRoute[i].LandmarkCode != CurrCar.CurrSite.ToString())
                            {
                                TempRouteLands.Add(CurrCar.CurrRoute[i].LandmarkCode);
                                TrafficLand.Add(PlanRoute.CreateDeepCopy<LandmarkInfo>(CurrCar.CurrRoute[i]));
                            }
                            break;
                        }
                        else
                        {
                            if (TempRouteLands.Where(p => p == (CurrCar.CurrRoute[i - 1]).LandmarkCode).Count() <= 0 && CurrCar.CurrRoute[i - 1].LandmarkCode != CurrCar.CurrSite.ToString())
                            {
                                TempRouteLands.Add(CurrCar.CurrRoute[i - 1].LandmarkCode);
                                TrafficLand.Add(PlanRoute.CreateDeepCopy<LandmarkInfo>(CurrCar.CurrRoute[i - 1]));
                            }
                            if (TempRouteLands.Where(p => p == (CurrCar.CurrRoute[i]).LandmarkCode).Count() <= 0 && CurrCar.CurrRoute[i].LandmarkCode != CurrCar.CurrSite.ToString())
                            {
                                TempRouteLands.Add(CurrCar.CurrRoute[i].LandmarkCode);
                                TrafficLand.Add(PlanRoute.CreateDeepCopy<LandmarkInfo>(CurrCar.CurrRoute[i]));
                            }
                        }
                        UpResourLenth += segment.Length;
                    }

                    LandmarkInfo ld = CurrCar.CurrRoute.FirstOrDefault(q => q.LandmarkCode == CurrCar.CurrSite.ToString());
                    if (ld != null && TrafficLand.Where(p => p.LandmarkCode == ld.LandmarkCode).Count() <= 0)
                    { TrafficLand.Add(PlanRoute.CreateDeepCopy<LandmarkInfo>(ld)); }


                    LandmarkInfo Land = (from a in TrafficLand
                                         where a.IsRotateLand
                                         orderby getDistant(a.LandX, a.LandY, ld.LandX, ld.LandY) ascending
                                         select a).FirstOrDefault();

                    if (Land != null && Land.IsRotateLand)
                    {
                        //根据旋转半径找到范围内的地标集
                        List<LandmarkInfo> CloseLands = (from a in AllLands
                                                         where Math.Round(getDistant(a.LandX, a.LandY, Land.LandX, Land.LandY) * ScalingRateLen, 3) <= Math.Round(TurnLen, 3)
                                                         select a).ToList();
                        foreach (LandmarkInfo Copy in CloseLands)
                        {
                            if (TempTurnLands.Where(p => p == Copy.LandmarkCode).Count() <= 0)
                            {
                                TempTurnLands.Add(Copy.LandmarkCode);
                            }
                        }
                    }
                    Hashtable hs = new Hashtable();
                    hs["RouteLands"] = TempRouteLands;
                    hs["TurnLands"] = TempTurnLands;
                    return hs;
                }
            }
            catch (Exception ex)
            {
                WriteLog("单独计算启动资源集异常:" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 单独计算当前车是否行走
        /// </summary>
        private bool SameCheckIsStop(int CurrCarID, int CurrSite, Hashtable hs)
        {
            try
            {
                //1 先判断同向，即我当前行走路线资源集中是否有车辆的当前位置
                //2 判断是否路线交叉，即我当前的行走路线资源集有交集的
                //3 判断是否有旋转，即我的行走资源中是否有其他车的旋转资源
                //4 判断管制区域内是否有车
                if (hs == null) { return true; }
                List<string> RouteLand = hs["RouteLands"] as List<string>;
                List<string> TurnLands = hs["TurnLands"] as List<string>;
                WriteLog("实算是否可以走" + CurrCarID.ToString() + "行走资源集合:" + string.Join(",", RouteLand.Select(p => p)));
                WriteLog("实算是否可以走" + CurrCarID.ToString() + "旋转资源集合:" + string.Join(",", TurnLands.Select(p => p)));
                //判断同向跟车
                CarMonitor HasSamDeriAnotherCar = (from a in MoniCars
                                                   where a.CarCode != CurrCarID && RouteLand.Where(q => q == a.CurrSite.ToString()).Count() > 0
                                                   select a).FirstOrDefault();

                //判断是否路线交叉 排除了被我锁住的
                CarMonitor HasIntersectAnotherCar = (from a in MoniCars
                                                     where a.CarCode != CurrCarID && RouteLand.Intersect(a.RouteLands).Count() > 0
                                                     orderby RouteLand.Intersect(a.RouteLands).Count() descending
                                                     //&& lockResource.Count(p => p.BeLockCarID == a.CarCode && p.LockCarID == CurrCarID) <= 0
                                                     select a).FirstOrDefault();

                //判断是否有旋转交集  排除了被我锁住的
                CarMonitor HasTurnAnotherCar = (from a in MoniCars
                                                where a.CarCode != CurrCarID && RouteLand.Intersect(a.TurnLands).Count() > 0
                                                //&& lockResource.Count(p => p.BeLockCarID == a.CarCode && p.LockCarID == CurrCarID) <= 0
                                                select a).FirstOrDefault();

                //判断我的旋转资源中是否有其他车  排除了被我锁住的
                CarMonitor MyTurnHasAnotherCar = (from a in MoniCars
                                                  where a.CarCode != CurrCarID && TurnLands.Contains(a.CurrSite.ToString())
                                                  //&& lockResource.Count(p => p.BeLockCarID == a.CarCode && p.LockCarID == CurrCarID) <= 0
                                                  select a).FirstOrDefault();


                if (HasSamDeriAnotherCar != null)//同向必须停
                {
                    //if (lockResource.Where(p => p.LockCarID == HasSamDeriAnotherCar.CarCode && p.BeLockCarID == CurrCarID).Count() <= 0)
                    //{
                    //    LockResource LockBlock = new LockResource();
                    //    LockBlock.BeLockCarID = CurrCarID;
                    //    LockBlock.LockCarID = HasSamDeriAnotherCar.CarCode;
                    //    lockResource.Add(LockBlock);
                    //}
                    WriteLog("单独跟车:" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "停,车" + HasSamDeriAnotherCar.CarCode.ToString() + "在站点" + HasSamDeriAnotherCar.CurrSite.ToString() + "走");
                    return true;
                }


                //JunctionInfo Junc = (from a in CoreData.JunctionList
                //                     where RouteLand.Where(p => p == a.EnterLandCode).Count() > 0
                //                     && a.OwerAGVID != -1
                //                     select a).FirstOrDefault();

                //if (Junc != null)
                //{
                //    if (lockResource.Where(p => p.LockCarID == Junc.OwerAGVID && p.BeLockCarID == CurrCarID).Count() <= 0)
                //    {
                //        LockResource LockBlock = new LockResource();
                //        LockBlock.BeLockCarID = CurrCarID;
                //        LockBlock.LockCarID = Junc.OwerAGVID;
                //        lockResource.Add(LockBlock);
                //    }
                //    CarInfo SourceCar = CoreData.CarList.FirstOrDefault(p => p.AgvID == Junc.OwerAGVID);
                //    if (SourceCar != null)
                //    {
                //        LogHelper.WriteLog("单独管制区域有车:" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "停" + SourceCar.AgvID.ToString() + "在站点" + SourceCar.CurrSite.ToString() + "继续走");
                //    }
                //    return true;
                //}
                //else
                //{ Junc.OwerAGVID = CurrCarID; }

                //我的旋转区域内有车，我必须停,不管对方是否不是被我锁住的，也要被对方锁住
                if (MyTurnHasAnotherCar != null)
                {
                    //if (lockResource.Where(p => p.LockCarID == MyTurnHasAnotherCar.AgvID && p.BeLockCarID == CurrCar.AgvID).Count() <= 0)
                    //{
                    //LockResource LockBlock = new LockResource();
                    //LockBlock.BeLockCarID = CurrCarID;
                    //LockBlock.LockCarID = MyTurnHasAnotherCar.CarCode;
                    //lockResource.Add(LockBlock);
                    //}
                    WriteLog("单独我的旋转资源中有车:" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "停,车" + MyTurnHasAnotherCar.CarCode.ToString() + "在站点" + MyTurnHasAnotherCar.CurrSite.ToString() + "走");
                    return true;
                }


                if (HasTurnAnotherCar != null)
                {
                    ////如果发现我的行走路线中有需要旋转的车辆,并且旋转的车已经在旋转集合中那么不需要判断对方是否暂停,必须自己停
                    if (HasTurnAnotherCar.TurnLands.Contains(HasTurnAnotherCar.CurrSite.ToString()))
                    {
                        //if (lockResource.Where(p => p.LockCarID == HasTurnAnotherCar.CarCode && p.BeLockCarID == CurrCarID).Count() <= 0)
                        //{
                        //    LockResource LockBlock = new LockResource();
                        //    LockBlock.BeLockCarID = CurrCarID;
                        //    LockBlock.LockCarID = HasTurnAnotherCar.CarCode;
                        //    lockResource.Add(LockBlock);
                        //}
                        WriteLog("单独有旋转且在旋转半径内车:" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "停,车" + HasTurnAnotherCar.CarCode.ToString() + "在站点" + HasTurnAnotherCar.CurrSite.ToString() + "走");
                        return true;
                    }
                    //判断对方是否被我锁住的
                    //if (lockResource.Count(p => p.BeLockCarID == HasTurnAnotherCar.CarCode && p.LockCarID == CurrCarID) <= 0)
                    //{
                    //if (lockResource.Where(p => p.LockCarID == HasTurnAnotherCar.CarCode && p.BeLockCarID == CurrCarID).Count() <= 0)
                    //{
                    //    LockResource LockBlock = new LockResource();
                    //    LockBlock.BeLockCarID = CurrCarID;
                    //    LockBlock.LockCarID = HasTurnAnotherCar.CarCode;
                    //    lockResource.Add(LockBlock);
                    //}
                    WriteLog("单独有旋转车:" + HasTurnAnotherCar.CarCode.ToString() + "已被我:" + CurrCarID.ToString() + "锁了");
                    return true;
                    //}
                    //else
                    //{ WriteLog("单独车:" + HasTurnAnotherCar.CarCode.ToString() + "已被我:" + CurrCarID.ToString() + "锁了"); }
                }

                if (HasIntersectAnotherCar != null)
                {
                    List<string> CurrInsect = RouteLand.Intersect(HasIntersectAnotherCar.RouteLands).ToList();
                    List<string> AnotherInsect = HasIntersectAnotherCar.RouteLands.Intersect(RouteLand).ToList();
                    if (CurrInsect.Count > 0 && AnotherInsect.Count > 0)
                    {
                        int CurrInsectIndex = RouteLand.FindIndex(q => q == CurrInsect[0]);
                        int AnotherInsectIndex = HasIntersectAnotherCar.RouteLands.FindIndex(q => q == AnotherInsect[0]);
                        if (AnotherInsectIndex < CurrInsectIndex)
                        {
                            WriteLog("单独交叉车:" + CurrCarID.ToString() + "在站点" + CurrSite.ToString() + "停,车" + HasIntersectAnotherCar.CarCode.ToString() + "在站点" + HasIntersectAnotherCar.CurrSite.ToString() + "走");
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                WriteLog("单独交通管制异常:" + ex.Message);
                return true;
            }
        }

        /// <summary>
        /// 单独判断交通管制
        /// </summary>
        public bool SameHandleTrafficForStop(object obj)
        {
            try
            {
                if (obj == null) { return true; }
                CarMonitor CurrCar = obj as CarMonitor;
                if (CurrCar == null) { return true; }
                WriteLog("单独停止管制逻辑车" + CurrCar.CarCode.ToString() + "行走资源集合:" + string.Join(",", CurrCar.RouteLands.Select(p => p)));
                WriteLog("单独停止管制逻辑车" + CurrCar.CarCode.ToString() + "旋转资源集合:" + string.Join(",", CurrCar.TurnLands.Select(p => p)));
                #region New
                Hashtable hs = SameGetRouteLandResource(CurrCar);
                return SameCheckIsStop(CurrCar.CarCode, CurrCar.CurrSite, hs);
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog("单独交通管制异常:" + ex.Message);
                return true;
            }
        }

        #endregion
    }
}
