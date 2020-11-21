using AGVDAccess;
using Model.Comoon;
using Model.MDM;
using Model.MSM;
using SQLServerOperator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tools;

namespace SimulationModel
{
    public class Simulator
    {
        #region 属性
        IList<CarBaseStateInfo> AllCar = new List<CarBaseStateInfo>();//存放所有的小车
        public static IList<AllSegment> AllSegs = new List<AllSegment>();//所有线段
        //IList<JunctionInfo> Juncts = new List<JunctionInfo>();
        static List<CarMonitor> MoniCars = new List<CarMonitor>();
        static IList<LandmarkInfo> AllLands = new List<LandmarkInfo>();
        public RoutePlanData CountRoute { get; set; }
        public TrafficController Traffic { get; set; }
        public IDictionary<string, string> System = new Dictionary<string, string>();
        public delegate void CarMove(object sender);
        public delegate void CarIni(object sender);
        //public delegate void RouteIni(object sender);
        //public event RouteIni Route_Ini;
        public event CarIni Car_Ini;
        public event CarMove Car_Move;
        private System.Timers.Timer timerStarBeStopedCar = new System.Timers.Timer(1000 * 1);
        public static readonly object lockStopObj = new object();
        public static readonly object lockStartObj = new object();
        public static object HandleTaskobj = new object();
        private List<LockResource> lockResource = new List<LockResource>();
        static IList<StorageInfo> Stores = new List<StorageInfo>();
        private System.Timers.Timer timerFreshTask = new System.Timers.Timer(1000 * 2);
        #endregion

        #region 模拟器方法
        public bool Inital()
        {
            try
            {
                MoniCars.Clear();
                AllCar = AGVClientDAccess.LoadAGVAchive();
                AllLands = AGVClientDAccess.LoadLandByCondition("1=1");
                AllSegs = AGVClientDAccess.LoadAllSegment();
                SimulatorVar.AllSegs = AllSegs;
                Stores = AGVClientDAccess.LoadStorages();
                System = AGVSimulationDAccess.LoadSystem();
                foreach (CarBaseStateInfo item in AllCar)
                {
                    CarMonitor moniCar = new CarMonitor();
                    moniCar.AgvID = item.AgvID;
                    moniCar.CurrSite = Convert.ToInt32(item.StandbyLandMark);
                    moniCar.StandbyLandMark = item.StandbyLandMark;
                    double ScalingRate = 0;
                    string ScalingRateStr = System["ScalingRate"].ToString();
                    try
                    {
                        ScalingRate = Convert.ToDouble(ScalingRateStr);
                    }
                    catch
                    { }
                    if (ScalingRate > 0)
                    {
                        LandmarkInfo CurrLand = AllLands.FirstOrDefault(p => p.LandmarkCode == item.StandbyLandMark);
                        if (CurrLand != null)
                        {
                            moniCar.X = (float)(CurrLand.LandX * ScalingRate);
                            moniCar.Y = (float)(CurrLand.LandY * ScalingRate);
                        }
                    }
                    moniCar.ScalingRate = ScalingRate;
                    MoniCars.Add(moniCar);
                }
                if (Car_Ini != null)
                { Car_Ini(MoniCars); }
                //Juncts.Clear();
                //IList<TrafficController> Traffics = AGVClientDAccess.GetTraffics();
                //foreach (TrafficController item in Traffics)
                //{
                //    JunctionInfo junct = new JunctionInfo();
                //    foreach (string s in item.EnterLandCode.Split(','))
                //    { junct.JunctionLandMarkCodes.Add(s); }
                //    junct.JunctionID = item.JunctionID;
                //    foreach (string s in item.JunctionLandMarkCodes.Split(','))
                //    { junct.JunctionLandMarkCodes.Add(s); }
                //    junct.RealseLandMarkCode = item.RealseLandMarkCode;
                //    Juncts.Add(junct);
                //}

                CountRoute = new RoutePlanData(AllSegs);
                Traffic = new TrafficController(MoniCars, AllSegs, System, AllLands);
                timerStarBeStopedCar.Enabled = true;
                timerStarBeStopedCar.AutoReset = true;
                timerStarBeStopedCar.Elapsed += TimerStarBeStopedCar_Elapsed;
                timerFreshTask.Enabled = true;
                timerFreshTask.AutoReset = true;
                timerFreshTask.Elapsed += TimerFreshTask_Elapsed;
                return true;
            }
            catch (Exception ex)
            { return false; throw ex; }
        }

        public bool StopSimula()
        {
            try
            {
                timerFreshTask.Enabled = false;
                timerStarBeStopedCar.Enabled = false;
                foreach (CarMonitor car in MoniCars)
                { car.Dispose(); }
                return true;
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion

        /// <summary>
        /// 创建任务
        /// </summary>
        public string CreatTask(int CallBoxID, int BtnID)
        {
            try
            {
                CallBoxInfo BoxInfo = AGVSimulationDAccess.LoadAllCallBoxByID(CallBoxID);
                if (BoxInfo == null)
                {
                    return "未配置按钮盒档案信息";
                }
                IList<CallBoxDetail> BoxDetails = AGVSimulationDAccess.LoadCallBoxDetails(CallBoxID);
                if (BoxDetails == null || (BoxDetails != null && BoxDetails.Count <= 0))
                {
                    return "未配置按钮盒明细档案信息";
                }


                CallBoxDetail CurrBoxDetail = BoxDetails.FirstOrDefault(p => p.CallBoxID == CallBoxID && p.ButtonID == BtnID);
                if (CurrBoxDetail == null)
                {
                    return "当前按钮没有配置信息";
                }
                if (CurrBoxDetail.OperaType == 0)
                {
                    IList<TaskConfigDetail> TaskConfigDetails = AGVClientDAccess.load_TaskDetail(CurrBoxDetail.TaskConditonCode);
                    if (TaskConfigDetails == null && (TaskConfigDetails != null && TaskConfigDetails.Count <= 0))
                    { return "当前按钮未配置任务信息"; }
                    //开始创建任务
                    StorageInfo CallStore = Stores.FirstOrDefault(q => q.ID == CurrBoxDetail.LocationID);
                    if (CallStore == null)
                    { return "未设置当前按钮的监控储位!"; }
                    //CallStore.StorageState = CurrBoxDetail.LocationState;

                    ///后面再处理
                    if (AGVSimulationDAccess.ChekAllowCreatTask(CallBoxID, CallStore.LankMarkCode, 0) > 0)
                    {
                        return "存在未完成任务,请稍后再试!";
                    }
                    string dispatchNo = Guid.NewGuid().ToString();
                    DispatchTaskInfo TaskInfo = new DispatchTaskInfo();
                    TaskInfo.dispatchNo = dispatchNo;
                    TaskInfo.taskType = 0;
                    TaskInfo.TaskState = 0;
                    TaskInfo.CallLand = CallStore.LankMarkCode;
                    TaskInfo.stationNo = CallBoxID;
                    //创建任务明细
                    int DetailID = 1;
                    int PreStoreState = -1;
                    foreach (TaskConfigDetail item in TaskConfigDetails)
                    {
                        DispatchTaskDetail taskDetail = new DispatchTaskDetail();
                        taskDetail.dispatchNo = dispatchNo;
                        taskDetail.DetailID = DetailID;
                        //寻找目标地表
                        StorageInfo ArmStore = null;
                        if (item.ArmOwnArea == -1)
                        { ArmStore = CallStore; }
                        else
                        { ArmStore = Stores.FirstOrDefault(p => p.OwnArea == item.ArmOwnArea && p.StorageState == item.StorageState && p.MaterielType == item.MaterialType); }
                        if (ArmStore == null)
                        { return "任务条件不满足!"; }
                        taskDetail.LandCode = ArmStore.LankMarkCode;
                        taskDetail.OperType = item.Action;
                        taskDetail.PutType = PreStoreState == -1 ? 0 : (PreStoreState == 1 ? 0 : 1);
                        taskDetail.IsAllowExcute = item.IsWaitPass;
                        taskDetail.State = 0;
                        TaskInfo.TaskDetail.Add(taskDetail);
                        DetailID += 1;
                        PreStoreState = ArmStore.StorageState;
                    }
                    OperateReturnInfo opr = AGVSimulationDAccess.SaveTask(TaskInfo);
                    if (opr.ReturnCode == OperateCodeEnum.Success)
                    {
                        foreach (TaskConfigDetail item in TaskConfigDetails)
                        {
                            StorageInfo ArmStore = Stores.FirstOrDefault(p => p.OwnArea == item.ArmOwnArea && p.StorageState == item.StorageState && p.MaterielType == item.MaterialType);
                            if (ArmStore != null)
                            { ArmStore.LockState = 2; }
                        }
                    }
                    else
                    { return opr.ReturnInfo.ToString(); }
                }
                else if (CurrBoxDetail.OperaType == 1)
                {
                    CurrBoxDetail = BoxDetails.FirstOrDefault(p => p.CallBoxID == CallBoxID && p.ButtonID == BtnID);
                    if (CurrBoxDetail == null)
                    {
                        return "当前按钮没有配置信息";
                    }
                    IList<TaskConfigDetail> TaskConfigDetails = AGVClientDAccess.load_TaskDetail(CurrBoxDetail.TaskConditonCode);
                    if (TaskConfigDetails == null && (TaskConfigDetails != null && TaskConfigDetails.Count <= 0))
                    { return "当前按钮未配置任务信息"; }
                    //开始创建任务
                    StorageInfo CallStore = Stores.FirstOrDefault(q => q.ID == CurrBoxDetail.LocationID);
                    if (CallStore == null)
                    { return "未设置当前按钮的监控储位!"; }
                    CallStore.StorageState = CurrBoxDetail.LocationState;
                    AGVSimulationDAccess.UpdateStore(CallStore.ID, CallStore.StorageState);
                }
                else if (CurrBoxDetail.OperaType == 2)//放行
                {
                    StorageInfo CheckStore = Stores.FirstOrDefault(p => p.ID == CurrBoxDetail.LocationID);
                    if (CheckStore == null)
                    { return "监控放行储位不存在!"; }
                    CarMonitor ReleaseCar = MoniCars.FirstOrDefault(p => p.CurrSite.ToString() == CheckStore.LankMarkCode);
                    if (ReleaseCar == null)
                    { return "当前没有放行车辆!"; }
                    OperateReturnInfo opr = AGVSimulationDAccess.ReleaseCar(ReleaseCar.ExcuteTaksNo, ReleaseCar.ArmLand);
                    if (opr.ReturnCode != OperateCodeEnum.Success)
                    { return opr.ReturnInfo.ToString(); }
                }
                else//取消任务
                {
                }
                return "操作成功!";
            }
            catch (Exception ex)
            {
                return "发送异常!" + ex.Message;
            }
        }

        /// <summary>
        /// 创建点到点任务
        /// </summary>
        public string CreatPointToPointTask(string BeginLandCode, string EndLandCode)
        {
            try
            {
                string dispatchNo = Guid.NewGuid().ToString();
                DispatchTaskInfo TaskInfo = new DispatchTaskInfo();
                TaskInfo.dispatchNo = dispatchNo;
                TaskInfo.taskType = 0;
                TaskInfo.TaskState = 0;
                TaskInfo.CallLand = "Test";
                TaskInfo.stationNo = 0;
                DispatchTaskDetail taskFirstDetail = new DispatchTaskDetail();
                taskFirstDetail.dispatchNo = dispatchNo;
                taskFirstDetail.DetailID = 1;
                taskFirstDetail.LandCode = BeginLandCode;
                taskFirstDetail.State = 0;
                taskFirstDetail.IsAllowExcute = 1;
                TaskInfo.TaskDetail.Add(taskFirstDetail);
                DispatchTaskDetail taskSecondDetail = new DispatchTaskDetail();
                taskSecondDetail.dispatchNo = dispatchNo;
                taskSecondDetail.DetailID = 2;
                taskSecondDetail.LandCode = EndLandCode;
                taskSecondDetail.State = 0;
                taskSecondDetail.IsAllowExcute = 1;
                TaskInfo.TaskDetail.Add(taskSecondDetail);
                OperateReturnInfo opr = AGVSimulationDAccess.SaveTask(TaskInfo);
                if (opr.ReturnCode == OperateCodeEnum.Success)
                { return "操作成功"; }
                else
                { return "操作失败:" + opr.ReturnInfo.ToString(); }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 分派任务
        /// </summary>
        private void TimerFreshTask_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                timerFreshTask.Enabled = false;
                lock (HandleTaskobj)
                { assignTask(); }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { timerFreshTask.Enabled = true; }
        }


        // 定时处理任务锁
        static object HandleTaskobje = new object();
        /// <summary>
        /// 分派任务
        /// </summary>
        private async void assignTask()
        {
            try
            {
                lock (HandleTaskobje)
                {
                    timerFreshTask.Enabled = false;
                    foreach (CarMonitor car in MoniCars.Where(p => (!string.IsNullOrEmpty(p.ExcuteTaksNo)) && p.TaskDetailID >= 0))
                    {

                        //将当前的任务明细更新完成
                        if (!string.IsNullOrEmpty(car.ExcuteTaksNo) && car.TaskDetailID != -1)
                        {
                            //HandTaskDetail(car.CurrSite.ToString(), car.ExcuteTaksNo, car.TaskDetailID);
                            if (car.Sate == 0 && car.CurrSite.ToString() == car.ArmLand)
                            {
                                #region 更新储位状态
                                UnLockStorage(car, Stores);
                                #endregion
                                AGVSimulationDAccess.HandTaskDetail(car.AgvID, car.CurrSite.ToString(), car.ExcuteTaksNo, car.TaskDetailID);
                                AGVSimulationDAccess.HandTaskzb(car.ExcuteTaksNo);
                                DispatchTaskInfo CarTaskInfo = AGVSimulationDAccess.CheckTaskIsFinish(car.ExcuteTaksNo);
                                if (CarTaskInfo == null)
                                { car.ExcuteTaksNo = ""; }
                            }
                        }
                    }
                       

                    Hashtable hs = new Hashtable();
                    //查找当前有效任务
                    IList<DispatchTaskInfo> tasks = AGVSimulationDAccess.LoadDispatchTask("0,1");
                    if (tasks != null && tasks.Count > 0)
                    {
                        foreach (DispatchTaskInfo task in tasks)
                        {
                            if (task.TaskDetail.Count <= 0) { continue; }
                            //查找合适的AGV来领取任务
                            CarMonitor NoTaskCar = null;
                            //如果任务为待处理的,就找空闲的车来执行
                            //如果任务是正在执行中的，则找对应的空闲agv来继续执行
                            //查找任务到具体任务明细
                            DispatchTaskDetail TaskDetail = (from a in task.TaskDetail
                                                             where (a.State == 0 || a.State == 1)
                                                             orderby a.DetailID ascending
                                                             select a).FirstOrDefault();
                            if (task.TaskState == 0)
                            {
                                if (TaskDetail != null)
                                {
                                    if (task.ExeAgvID != 0)
                                    {
                                        NoTaskCar = (from a in MoniCars
                                                     where task.ExeAgvID == a.AgvID && !string.IsNullOrEmpty(a.CurrSite.ToString()) && (a.Sate == 0 || a.IsBack) && a.ExcuteTaksNo == "" /*a.TaskDetailID == -1*/
                                                     && getDistant(a.CurrSite.ToString(), TaskDetail.LandCode) >= 0
                                                     orderby getDistant(a.CurrSite.ToString(), TaskDetail.LandCode) ascending
                                                     select a).FirstOrDefault();
                                    }
                                    else
                                    {
                                        NoTaskCar = (from a in MoniCars
                                                     where !string.IsNullOrEmpty(a.CurrSite.ToString()) && (a.Sate == 0 || a.IsBack) && a.ExcuteTaksNo == "" /*a.TaskDetailID == -1*/
                                                     && getDistant(a.CurrSite.ToString(), TaskDetail.LandCode) >= 0
                                                     orderby getDistant(a.CurrSite.ToString(), TaskDetail.LandCode) ascending
                                                     select a).FirstOrDefault();
                                    }
                                }
                            }
                            else if (task.TaskState == 1)
                            { NoTaskCar = MoniCars.FirstOrDefault(p => p.AgvID == task.ExeAgvID && ((p.Sate == 0 || p.Sate == 2 || p.IsBack))); }
                            //找到对应的agv
                            if (NoTaskCar != null && NoTaskCar.CurrSite > 0)
                            {
                                if (NoTaskCar.IsBack)
                                { NoTaskCar.IsBack = !NoTaskCar.IsBack; }
                            }
                            else
                            { continue; }
                            //如果任务明细全部完成，那么需要将主任务状态更新并且让对应的agv回待命点
                            LandmarkInfo BeginLand, EndLand = null;
                            if (TaskDetail == null)
                            {
                               
                            }
                            else
                            {
                                //判断一下当前小时是否就在任务地标上
                                if (NoTaskCar.CurrSite.ToString() == TaskDetail.LandCode && (TaskDetail.State == 0 || TaskDetail.State == 1))
                                {
                                    NoTaskCar.OperType = TaskDetail.OperType;
                                    NoTaskCar.PutType = TaskDetail.PutType;
                                    NoTaskCar.ArmLand = TaskDetail.LandCode;
                                    NoTaskCar.ExcuteTaksNo = TaskDetail.dispatchNo;
                                    NoTaskCar.TaskDetailID = TaskDetail.DetailID;
                                    AGVSimulationDAccess.HandTaskDetail(NoTaskCar.AgvID, TaskDetail.LandCode, TaskDetail.dispatchNo, TaskDetail.DetailID);
                                    UnLockStorage(NoTaskCar, Stores);
                                    //NoTaskCar.TaskDetailID = -1;
                                    continue;
                                }

                                //判断小车当前的任务明细是否允许需要等待放行命令
                                DispatchTaskDetail PreTaskDetail = (from a in task.TaskDetail
                                                                    where a.LandCode == NoTaskCar.CurrSite.ToString()
                                                                    orderby a.DetailID ascending
                                                                    select a).FirstOrDefault();
                                //如果不允许执行则跳过
                                if (PreTaskDetail != null && PreTaskDetail.IsAllowExcute == 0)
                                { continue; }
                                //记录当前小车的任务信息
                                //更新任务为执行状态
                                AGVSimulationDAccess.TaskHandle(task.dispatchNo, NoTaskCar.AgvID, 1, TaskDetail.LandCode, TaskDetail.DetailID);
                                NoTaskCar.ExcuteTaksNo = task.dispatchNo;
                                NoTaskCar.TaskDetailID = TaskDetail.DetailID;
                                NoTaskCar.ArmLand = TaskDetail.LandCode;
                                NoTaskCar.PutType = TaskDetail.PutType;
                                NoTaskCar.OperType = TaskDetail.OperType;
                                BeginLand = AllLands.FirstOrDefault(p => p.LandmarkCode == NoTaskCar.CurrSite.ToString());
                                EndLand = AllLands.FirstOrDefault(p => p.LandmarkCode == TaskDetail.LandCode);
                                if (BeginLand != null && EndLand != null)
                                {
                                    NoTaskCar.CurrRoute = DataToObject.CreateDeepCopy<List<LandmarkInfo>>(CountRoute.GetRoute(BeginLand, EndLand));
                                    string CountRouteResult = string.Join(",", NoTaskCar.CurrRoute.Select(p => p.LandmarkCode));
                                    LogHelper.WriteLog(CountRouteResult);
                                    if (NoTaskCar.CurrRoute.Count <= 0) { continue; }
                                    if (!Traffic.BeforStartTrafficForStop(NoTaskCar))
                                    {
                                        Traffic.GetTrafficResour(NoTaskCar);
                                        //if (Route_Ini != null)
                                        //{ Route_Ini(NoTaskCar.CurrRoute); }
                                        NoTaskCar.StepChange += DoCar_StepChange;
                                        NoTaskCar.Start();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        List<CarMonitor> NoReleaseCars = MoniCars.Where(p => p.Sate != 1 && p.Sate != 2 && !string.IsNullOrEmpty(p.ExcuteTaksNo)).ToList();
                        foreach (CarMonitor item in NoReleaseCars)
                        { item.ExcuteTaksNo = ""; }
                    }
                    List<CarMonitor> NoBackCars = MoniCars.Where(p => p.Sate != 1 && p.Sate != 2 && string.IsNullOrEmpty(p.ExcuteTaksNo) && p.CurrSite.ToString() != p.StandbyLandMark).ToList();
                    foreach (CarMonitor car in NoBackCars)
                    {
                        if (string.IsNullOrEmpty(car.StandbyLandMark)) { continue; }
                        LandmarkInfo BeginLand = AllLands.FirstOrDefault(p => p.LandmarkCode == car.CurrSite.ToString());
                        LandmarkInfo EndLand = AllLands.FirstOrDefault(p => p.LandmarkCode == car.StandbyLandMark);
                        //以车的当前地标为开始地标,待命点为结束地标规划回待命点路线
                        if (BeginLand == null || EndLand == null)
                        { continue; }
                        else
                        {
                            //规划路线
                            car.CurrRoute = DataToObject.CreateDeepCopy<List<LandmarkInfo>>(CountRoute.GetRoute(BeginLand, EndLand));
                            if (car.CurrRoute.Count <= 0) { continue; }
                            if (!Traffic.BeforStartTrafficForStop(car))
                            {
                                Traffic.GetTrafficResour(car);
                                //if (Route_Ini != null)
                                //{ Route_Ini(car.CurrRoute); }
                                car.StepChange += DoCar_StepChange;
                                car.Start();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { timerFreshTask.Enabled = true; }
        }

        /// <summary>
        /// 定时启动被交通管制的车
        /// </summary>
        private void TimerStarBeStopedCar_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                timerStarBeStopedCar.Enabled = false;
                new Thread(new ThreadStart(Traffic.HandleTrafficForStart)) { IsBackground = true }.Start();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
            }
            finally
            { timerStarBeStopedCar.Enabled = true; }
        }

        private void DoCar_StepChange(object sender)
        {
            try
            {
                CarMonitor car = sender as CarMonitor;
                if (car != null)
                {
                    HandleDate(car);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        private async void HandleDate(CarMonitor car)
        {
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    Car_Move(car);
                    if (car.OldSite != car.CurrSite)
                    {
                        Traffic.HandleTrafficForStop(car);
                        //new Thread(HandleTrafficForStop) { IsBackground = true }.Start(sender);
                    }
                });
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 计算两点之间距离
        /// </summary>
        /// <returns></returns>
        private double getDistant(string CurrLandCode, string TaskDetailLandCode)
        {
            try
            {
                LandmarkInfo land1 = AllLands.FirstOrDefault(p => p.LandmarkCode == CurrLandCode);
                LandmarkInfo land2 = AllLands.FirstOrDefault(p => p.LandmarkCode == TaskDetailLandCode);
                if (land1 != null && land2 != null)
                {
                    double distant = Math.Sqrt(Math.Pow(Math.Abs(land1.LandX - land2.LandX), 2) + Math.Pow(Math.Abs(land1.LandY - land2.LandY), 2));
                    return distant;
                }
                else
                {
                    LogHelper.WriteLog("领取任务时查找直线最近车辆时找不到车当前地标或任务明细的目的地标");
                }
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
            return -1;
        }

        /// <summary>
        /// 更新储位状态为空
        /// </summary>
        public void UnLockStorage(CarMonitor car, IList<StorageInfo> StorageList)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable dic = new Hashtable();
                dic["lockcar"] = car.AgvID;
                dic["landmarkcode"] = car.CurrSite.ToString();
                StorageInfo ToStorage = null;
                if (!string.IsNullOrEmpty(car.ArmLand) && car.CurrSite.ToString() == car.ArmLand)
                { ToStorage = StorageList.Where(p => p.LankMarkCode == car.CurrSite.ToString()).FirstOrDefault(); }
                if (ToStorage != null)
                {
                    if (car.OperType == 1)
                    {
                        if (car.PutType == 0)
                        {
                            ToStorage.StorageState = 1;
                            ToStorage.LockCar = 0;
                            ToStorage.LockState = 0;
                        }
                        else
                        {
                            ToStorage.StorageState = 2;
                            ToStorage.LockCar = 0;
                            ToStorage.LockState = 0;
                        }
                    }
                    else
                    {
                        ToStorage.StorageState = 0;
                        ToStorage.LockCar = 0;
                        ToStorage.LockState = 0;
                    }
                    dic["StorageState"] = ToStorage.StorageState;
                    dbOperator.SetDatas("ReleaseStore", dic);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 到达新步点后更新任务明细为完成
        /// </summary>
        //public void HandTaskDetail(string LandCode, string DispatchNo, int TaskDetailID)
        //{
        //    try
        //    {
        //        IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
        //        Hashtable hs = new Hashtable();
        //        hs["LandCode"] = LandCode;
        //        hs["dispatchNo"] = DispatchNo;
        //        hs["TaskDetailID"] = TaskDetailID;
        //        dbOperator.SetDatas("UpdateTaskDetailForFinish", hs);
        //    }
        //    catch (Exception ex)
        //    { LogHelper.WriteErrorLog(ex); }
        //}
        //public void HandTaskzb(string DispatchNo)
        //{
        //    try
        //    {
        //        IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
        //        Hashtable hs = new Hashtable();
        //        hs["dispatchNo"] = DispatchNo;
        //        dbOperator.SetDatas("UpdateTaskForFinish", hs);
        //    }
        //    catch (Exception ex)
        //    { LogHelper.WriteErrorLog(ex); }
        //}
    }
}
