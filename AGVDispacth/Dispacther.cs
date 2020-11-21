using AGVCommunication;
using AGVComponents;
using AGVCore;
using AGVDAccess;
using DipatchModel;
using MesModel;
using Model.CarInfoExtend;
using Model.Comoon;
using Model.MDM;
using Model.MSM;
using ShilinetSoftVerify;
using SQLServerOperator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools;

namespace AGVDispacth
{
    public class Dispacther : IDispacther
    {
        #region 属性
        //车辆信息反馈锁
        static object ReciveAGVMsg_lockobj = new object();
        //IO设备信息反馈锁
        static object ReciveIOMsg_lockobj = new object();
        //充电桩信息反馈锁
        static object ReciveChargeMsg_lockobj = new object();
        // 定时处理任务锁
        static object HandleTaskobj = new object();
        // 通讯接口类
        CommunicationBase Commer { get; set; }
        // 交通管制类
        AGVComponents.TrafficController Traffic { get; set; }
        //计算路线类
        public RoutePlan CountRoute { get; set; }

        //调度系统内部tcp通信类
        InsideHandler InsideHandler { get; set; }

        /// <summary>
        /// 非标通信或逻辑处理类
        /// </summary>
        //NonstandardDispatch NonStandarDispatch { get; set; }
        #endregion

        #region 定时器
        //定时判断启动被交通管制停止的车辆
        private System.Timers.Timer timerStarBeStopedCar = new System.Timers.Timer(1000 * 1);
        //定时拉取未执行任务
        private System.Timers.Timer timerFreshTask = new System.Timers.Timer(1000 * 3);
        /// <summary>
        /// 定时触发检查需要报警的平板
        /// </summary>
        private System.Timers.Timer timerCheckWarmPad = new System.Timers.Timer(1000 * 2);

        /// <summary>
        /// 定时删任务
        /// </summary>
        private System.Timers.Timer timerdeltask = new System.Timers.Timer(1000 * 60 * 60 * 24);
        #endregion

        #region 实现接口方法
        public void AGV_AddCommand(int agvid, CommandToValue ctov)
        {
            try
            {
                Commer.AGV_AddControl(agvid, ctov);
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }
        /// <summary>
        /// 充电桩操作
        /// </summary>
        /// <param name="eleid"></param>
        /// <param name="cmd"></param>
        public void Add_EleCommand(int eleid, CommandToValue ctov)
        {
            try
            {
                //查询对应的IO设备
                ChargeStationInfo ele = CoreData.ChargeList.FirstOrDefault(p => p.ID == eleid);
                if (ele != null)
                {
                    int eid = ele.ID;
                    Commer.Charge_AddControl(eid, ctov);
                }
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }

        public void AddIOControl(int ioid, CommandToValue ctov)
        {
            try
            {
                IODeviceInfo iODevice = CoreData.IOList.FirstOrDefault(p => p.ID == ioid);
                if (iODevice != null)
                {
                    Commer.IO_AddControl(iODevice.ID, new CommandToValue(AGVCommandEnum.Turn, ctov.CommandValue));
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 调度初始化
        /// </summary>
        public bool DispatchInit()
        {
            try
            {
//                bool res = CheckSoft();
//                if (!res) { return false; }
                #region 初始化数据库连接
                DelegateState.InvokeDispatchStateEvent("正在读取数据库连接...");
                ConnectConfigTool.setDBase();
                if (ConnectConfigTool.DBase == null)
                {
                    DelegateState.InvokeDispatchStateEvent("数据库连接文件不存在!");
                    return false;
                }
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                if (dbOperator == null)
                {
                    DelegateState.InvokeDispatchStateEvent("数据库连接文件不存在!");
                    return false;
                }
                bool IsConnectDB = false;
                try
                {
                    IsConnectDB = dbOperator.ServerIsThrough();
                }
                catch (Exception ex)
                {
                    DelegateState.InvokeDispatchStateEvent("数据库连接失败【" + ex.Message + "】");
                }
                if (!IsConnectDB)
                { return false; }
                CoreData.dbOperator = dbOperator;
                #endregion
                #region 读取系统参数
                DelegateState.InvokeDispatchStateEvent("正在读取系统参数...");
                DataTable dtSysparameter = CoreData.dbOperator.LoadDatas("QuerySyspara");
                foreach (DataRow row in dtSysparameter.Rows)
                {
                    CoreData.SysParameter[row["ParameterCode"].ToString()] = row["ParameterValue"].ToString();
                }
                #endregion
                #region 读取基础档案
                DelegateState.InvokeDispatchStateEvent("正在初始化系统档案");
                //读取车辆档案
                DataTable dtcar = CoreData.dbOperator.LoadDatas("QueryAllCars");
                CoreData.CarList = DataToObject.TableToEntity<CarInfo>(dtcar);
                //读取储位
                DataTable dtstorage = CoreData.dbOperator.LoadDatas("QueryAllStore");
                CoreData.StorageList = DataToObject.TableToEntity<StorageInfo>(dtstorage);
                //读取所有地标
                DataTable dtlandmark = CoreData.dbOperator.LoadDatas("QueryAllLand");
                CoreData.AllLands = DataToObject.TableToEntity<LandmarkInfo>(dtlandmark);
                //读取所有的线段
                DataTable dtseg = CoreData.dbOperator.LoadDatas("QueryAllSegment");
                CoreData.AllSeg = DataToObject.TableToEntity<AllSegment>(dtseg);
                //读取所有物料档案
                DataTable dtMat = CoreData.dbOperator.LoadDatas("QueryAllMaterial");
                CoreData.AllMaterials = DataToObject.TableToEntity<MaterialInfo>(dtMat);
                //读取AGV地图坐标对照
                DataTable AGVCoordinate = CoreData.dbOperator.LoadDatas("QueryCoordinate");
                CoreData.AGVCoordinate = AGVCoordinate;
                //读取充电桩档案信息
                DataTable dtChargeInfo = CoreData.dbOperator.LoadDatas("QueryAllCharge");
                CoreData.ChargeList = DataToObject.TableToEntity<ChargeStationInfo>(dtChargeInfo);
                //读取IO设备档案信息
                DataTable dtIoInfo = CoreData.dbOperator.LoadDatas("QueryAllIODevice");
                CoreData.IOList = DataToObject.TableToEntity<IODeviceInfo>(dtIoInfo);


                //读取所有平板档案
                DataTable dtPadInfos = CoreData.dbOperator.LoadDatas("QueryPadInfos");
                CoreData.AllPads = DataToObject.TableToEntity<PdaInfo>(dtPadInfos);
                DataTable dtPadOperSets = CoreData.dbOperator.LoadDatas("QueryPadOperSet");
                CoreData.PadOperSets = DataToObject.TableToEntity<PdaOperSetInfo>(dtPadOperSets);


                DataTable dtAllCallBox = CoreData.dbOperator.LoadDatas("QueryAllCallBox");
                CoreData.AllCallBoxes = DataToObject.TableToEntity<CallBoxInfo>(dtAllCallBox);
                DataTable dtAllCallBoxDetail = CoreData.dbOperator.LoadDatas("QueryAllCallBoxDetails");
                CoreData.AllCallBoxDetail = DataToObject.TableToEntity<CallBoxDetail>(dtAllCallBoxDetail);


                //读取地图缩放比例
                if (CoreData.SysParameter.Keys.Contains("ScalingRate"))
                {
                    string ScalingRateStr = CoreData.SysParameter["ScalingRate"].ToString();
                    try
                    {
                        CoreData.ScalingRate = Convert.ToInt16(ScalingRateStr);
                    }
                    catch
                    {
                        DelegateState.InvokeDispatchStateEvent("系统参数缩放比例设置错误!");
                        return false;
                    }
                }
                else
                {
                    DelegateState.InvokeDispatchStateEvent("系统参数缩放比例设置错误!");
                    return false;
                }

                //读取交通管制档案
                CoreData.JunctionList.Clear();
                DataTable dtJunction = CoreData.dbOperator.LoadDatas("QueryJunction", new Hashtable());
                foreach (DataRow row in dtJunction.Rows)
                {
                    TraJunction item = new TraJunction();
                    item.TraJunctionID = Convert.ToInt32(row["TraJunctionID"]);

                    if (!string.IsNullOrEmpty(row["JunctionLandMarkCodes"].ToString()) && row["JunctionLandMarkCodes"].ToString().Split(',').Length > 0)
                    {
                        item.JunctionLandMarkCodes = row["JunctionLandMarkCodes"].ToString();
                        foreach (string s in row["JunctionLandMarkCodes"].ToString().Split(','))
                        {
                            if (!string.IsNullOrEmpty(s.Trim()))
                            { item.JuncLandCodes.Add(s); }
                        }
                    }
                    try
                    {
                        item.Carnumber = Convert.ToInt32(row["Carnumber"]);
                    }
                    catch
                    { }
                    //item.RealseLandMarkCode = row["RealseLandMarkCode"].ToString();
                    IList<TraJunctionSegInfo> segs = AGVServerDAccess.QueryJunctionSeg(item.TraJunctionID);
                    if (segs != null)
                    { item.Segments = segs; }
                    CoreData.JunctionList.Add(item);
                }

                //读取线路片段配置
                CoreData.RouteFrages = AGVServerDAccess.Load_RouteFragment();

                //加载所有的指令档案
                CoreData.Cmdes = AGVServerDAccess.Load_Cmd();

                //加载所有的调度程序类配置信息
                CoreData.DispathAssemblies = AGVServerDAccess.Load_DisptchAssembly();
                #endregion
                #region 加载记忆文件
                //加载AGV记忆信息【线路,执行的任务】
                foreach (CarInfo car in CoreData.CarList)
                {
                    //加载记忆路线
                    List<LandmarkInfo> route = AGVServerDAccess.LoadCarRoute(car);
                    if (route != null)
                    {
                        car.Route = DataToObject.CreateDeepCopy<List<LandmarkInfo>>(route);
                        car.AllRouteLandCode = string.Join(",", car.Route.Select(p => p.LandmarkCode));
                        try
                        {
                            AGVServerDAccess.SaveCarCurrentRoute(car);
                        }
                        catch (Exception ex)
                        { LogHelper.WriteCreatTaskLog("保存AGV当前线路异常:" + ex.Message); }
                    }
                    //加载记忆任务号
                    Hashtable hs = XMLClass.GetXMLByParentNode(CoreData.RemerberFilePath, "N" + car.AgvID.ToString());
                    if (hs != null && hs.Count > 0)
                    {
                        if (hs["TaskNo"] != null)
                        { car.ExcuteTaksNo = hs["TaskNo"].ToString(); }
                        if (hs["TaskDetailID"] != null)
                        {
                            try
                            {
                                car.TaskDetailID = Convert.ToInt16(hs["TaskDetailID"]);
                            }
                            catch
                            { }
                        }
                        if (hs["PreChargeTime"] != null)
                        {
                            DateTime PreChargeTime = DateTime.ParseExact(hs["PreChargeTime"].ToString(), "yyyyMMddHHmmss", CultureInfo.CurrentCulture);
                            car.PreChargeTime = PreChargeTime;
                        }
                        if (hs["ArmLand"] != null)
                        {
                            car.ArmLand = hs["ArmLand"].ToString();
                        }
                    }
                }
                #endregion
                #region 初始化实例
                Commer = new CommunicationBase();
                Traffic = new AGVComponents.TrafficController();
                Traffic.Commer = Commer;
                CountRoute = new RoutePlan();
                #endregion
                #region 初始化定时器
                //定时启动被交通管制停止的车
                timerStarBeStopedCar.Enabled = true;
                timerStarBeStopedCar.AutoReset = true;
                timerStarBeStopedCar.Elapsed += TimerStarBeStopedCar_Elapsed;
                //定时拉去调度任务
                timerFreshTask.Enabled = true;
                timerFreshTask.AutoReset = true;
                timerFreshTask.Elapsed += TimerFreshTask_Elapsed;


                timerCheckWarmPad.Enabled = false;
                timerCheckWarmPad.Elapsed += TimerCheckWarmPad_Elapsed;

                //定时删除过期任务
                timerdeltask.AutoReset = true;
                timerdeltask.Enabled = true;
                timerdeltask.Elapsed += TimerDelTask_Elapsed;
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                DelegateState.InvokeDispatchStateEvent(ex.Message);
                LogHelper.WriteErrorLog(ex);
                return false;
            }
        }

        private void TimerCheckWarmPad_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                timerCheckWarmPad.Enabled = false;
                InsideHandler.SendPadWarm();
                InsideHandler.SynAppStoreState();
            }
            catch (Exception ex)
            { DelegateState.InvokeDispatchStateEvent("定时检查平板是否报警异常:" + ex.Message); }
            finally
            { timerCheckWarmPad.Enabled = true; }
        }

        private void TimerDelTask_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                timerdeltask.Enabled = false;
                bool res = CheckSoft();
                if (!res)
                {
                    timerFreshTask.Enabled = false;
                    Application.ExitThread();
                    Application.Exit();
                }
                AGVServerDAccess.DelUnUseTask();
            }
            catch (Exception ex)
            { DelegateState.InvokeDispatchStateEvent("定时删除过期任务异常:" + ex.Message); }
            finally
            { timerdeltask.Enabled = true; }
        }


        /// <summary>
        /// 启动调度服务
        /// </summary>
        public bool DispatchStart()
        {
            try
            {
                InsideHandler = new InsideHandler(this);
                //NonStandarDispatch = new NonstandardDispatch(this);
                if (!InsideHandler.Init())
                {
                    DelegateState.InvokeDispatchStateEvent("初始化操作台服务失败...");
                    return false;
                }
                if (!InsideHandler.Start())
                {
                    DelegateState.InvokeDispatchStateEvent("启动操作台服务失败...");
                    return false;
                }
                if (!Commer.CommInit())
                {
                    DelegateState.InvokeDispatchStateEvent("初始化AGV通信失败...");
                    return false;
                }
                if (!Commer.Start())
                {
                    DelegateState.InvokeDispatchStateEvent("启动AGV通信失败...");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 停止调度服务
        /// </summary>
        public bool DispatchStop()
        {
            try
            {
                //timerStarBeStopedCar.Enabled = false;
                //timerFreshTask.Enabled = false;
                //Commer.Stop();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 同步基础信息数据
        /// </summary>
        public void ReadBaseInfo()
        {
            try
            {
                #region 读取系统参数
                DelegateState.InvokeDispatchStateEvent("正在读取系统参数...");
                DataTable dtSysparameter = CoreData.dbOperator.LoadDatas("QuerySyspara");
                foreach (DataRow row in dtSysparameter.Rows)
                {
                    CoreData.SysParameter[row["ParameterCode"].ToString()] = row["ParameterValue"].ToString();
                }
                #endregion

                #region 读取基础档案
                DelegateState.InvokeDispatchStateEvent("正在初始化系统档案");
                //读取储位
                DataTable dtstorage = CoreData.dbOperator.LoadDatas("QueryAllStore");
                CoreData.StorageList = DataToObject.TableToEntity<StorageInfo>(dtstorage);
                //读取所有地标
                DataTable dtlandmark = CoreData.dbOperator.LoadDatas("QueryAllLand");
                CoreData.AllLands = DataToObject.TableToEntity<LandmarkInfo>(dtlandmark);
                //读取所有的线段
                DataTable dtseg = CoreData.dbOperator.LoadDatas("QueryAllSegment");
                CoreData.AllSeg = DataToObject.TableToEntity<AllSegment>(dtseg);
                //读取所有物料档案
                DataTable dtMat = CoreData.dbOperator.LoadDatas("QueryAllMaterial");
                CoreData.AllMaterials = DataToObject.TableToEntity<MaterialInfo>(dtMat);

                //读取AGV地图坐标对照
                DataTable AGVCoordinate = CoreData.dbOperator.LoadDatas("QueryCoordinate");
                CoreData.AGVCoordinate = AGVCoordinate;
                //读取充电桩档案信息
                DataTable dtChargeInfo = CoreData.dbOperator.LoadDatas("QueryAllCharge");
                CoreData.ChargeList = DataToObject.TableToEntity<ChargeStationInfo>(dtChargeInfo);
                //读取IO设备档案信息
                DataTable dtIoInfo = CoreData.dbOperator.LoadDatas("QueryAllIODevice");
                CoreData.IOList = DataToObject.TableToEntity<IODeviceInfo>(dtIoInfo);


                //读取所有平板档案
                DataTable dtPadInfos = CoreData.dbOperator.LoadDatas("QueryPadInfos");
                CoreData.AllPads = DataToObject.TableToEntity<PdaInfo>(dtPadInfos);
                DataTable dtPadOperSets = CoreData.dbOperator.LoadDatas("QueryPadOperSet");
                CoreData.PadOperSets = DataToObject.TableToEntity<PdaOperSetInfo>(dtPadOperSets);

                DataTable dtAllCallBox = CoreData.dbOperator.LoadDatas("QueryAllCallBox");
                CoreData.AllCallBoxes = DataToObject.TableToEntity<CallBoxInfo>(dtAllCallBox);
                DataTable dtAllCallBoxDetail = CoreData.dbOperator.LoadDatas("QueryAllCallBoxDetails");
                CoreData.AllCallBoxDetail = DataToObject.TableToEntity<CallBoxDetail>(dtAllCallBoxDetail);
                //读取地图缩放比例
                if (CoreData.SysParameter.Keys.Contains("ScalingRate"))
                {
                    string ScalingRateStr = CoreData.SysParameter["ScalingRate"].ToString();
                    try
                    {
                        CoreData.ScalingRate = Convert.ToInt16(ScalingRateStr);
                    }
                    catch
                    {
                        DelegateState.InvokeDispatchStateEvent("系统参数缩放比例设置错误!");
                    }
                }
                else
                {
                    DelegateState.InvokeDispatchStateEvent("系统参数缩放比例设置错误!");
                }

                //读取交通管制档案
                //CoreData.JunctionList.Clear();
                //DataTable dtJunction = CoreData.dbOperator.LoadDatas("QueryJunction", new Hashtable());
                //foreach (DataRow row in dtJunction.Rows)
                //{
                //    TraJunction item = new TraJunction();
                //    item.TraJunctionID = Convert.ToInt32(row["TraJunctionID"]);

                //    if (!string.IsNullOrEmpty(row["JunctionLandMarkCodes"].ToString()) && row["JunctionLandMarkCodes"].ToString().Split(',').Length > 0)
                //    {
                //        item.JunctionLandMarkCodes = row["JunctionLandMarkCodes"].ToString();
                //        foreach (string s in row["JunctionLandMarkCodes"].ToString().Split(','))
                //        {
                //            if (!string.IsNullOrEmpty(s.Trim()))
                //            { item.JuncLandCodes.Add(s); }
                //        }
                //    }
                //    try
                //    {
                //        item.Carnumber = Convert.ToInt32(row["Carnumber"]);
                //    }
                //    catch
                //    { }
                //    //item.RealseLandMarkCode = row["RealseLandMarkCode"].ToString();
                //    IList<TraJunctionSegInfo> segs = AGVServerDAccess.QueryJunctionSeg(item.TraJunctionID);
                //    if (segs != null)
                //    { item.Segments = segs; }
                //    CoreData.JunctionList.Add(item);
                //}

                //读取线路片段配置
                IList<RouteFragmentConfigInfo> RouteFrages = AGVServerDAccess.Load_RouteFragment();
                CoreData.RouteFrages = RouteFrages;
                #endregion
            }
            catch (Exception ex)
            {
                DelegateState.InvokeDispatchStateEvent("读取系统基础档案异常");
                LogHelper.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 异步调用交管
        /// </summary>
        /// <param name="car"></param>
        public void TrafficForStop(CarInfo car)
        {
            try
            {
                Traffic.HandleTrafficForStop(car);
                Traffic.CheckCarIsLockJuck(car);
                //await Task.Factory.StartNew(() =>
                //{
                //    Traffic.HandleTrafficForStop(car);
                //    Traffic.CheckCarIsLockJuck(car);
                //});
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }
        #endregion

        #region 回调
        /// <summary>
        /// 接受处理AGV回馈信息--以及重连上后指向启动
        /// </summary>
        public void HandleCarFeedBack(CarBaseStateInfo agvcar)
        {
            try
            {
                DateTime begintime = DateTime.Now;
                //bool res = CheckSoft();
                //if (!res) { return; }
                CarInfo car = CoreData.CarList.FirstOrDefault(p => p.AgvID == agvcar.AgvID);
                if (car == null)
                {
                    DelegateState.InvokeDispatchStateEvent("未能找到ID为" + agvcar.AgvID.ToString() + "的AGV信息");
                    return;
                }
                if (agvcar.CurrSite == 0)
                {
                    agvcar.CurrSite = car.CurrSite;
                }
                bool IsChange = car.IsChange(agvcar);

                lock (ReciveAGVMsg_lockobj)
                {
                    int oldsite = car.CurrSite;
                    car.GetValue(agvcar);
                    if (car.CarState == 5)
                    {
                        car.LowPower = false;
                        car.NowChargeLandCode = "";
                    }

                    #region 掉线处理
                    if (car.bIsCommBreak && !agvcar.bIsCommBreak)
                    {
                        LogHelper.WriteOffLineLog(car.AgvID.ToString() + "号小车在地标" + car.CurrSite.ToString() + "掉线后重连上");
                        //if (car.CarState == 1 && agvcar.CarState == 10)
                        //{
                        //    AGV_AddCommand(agvcar.AgvID, new CommandToValue(AGVCommandEnum.BtnSet));
                        //    AGV_AddCommand(agvcar.AgvID, new CommandToValue(AGVCommandEnum.Start));
                        //}
                    }
                    if (!car.bIsCommBreak && agvcar.bIsCommBreak)
                    {
                        car.bIsCommBreak = true;
                        DelegateState.InvokeCarChangeEvent(new CarEventArgs("", car));
                        LogHelper.WriteOffLineLog(car.AgvID.ToString() + "号小车在地标" + car.CurrSite.ToString() + "掉线");
                        return;
                    }
                    #endregion

                    #region 处理自动充电
                    DateTime cdbegintime = DateTime.Now;
                    CheckIsStartCharge(car);
                    DateTime cdendtime = DateTime.Now;
                    LogHelper.WriteLog(car.AgvID.ToString() + "号AGV  充电判断耗时:" + (cdendtime - cdbegintime).TotalMilliseconds.ToString());
                    #endregion

                    #region 处理任务是否重做或完成
                    // FinishTask(car);
                    #endregion

                    #region 步点变化

                    if (oldsite != car.CurrSite)//到达新步点
                    {
                        LogHelper.WriteLog(string.Format("{0}号AGV  到达新步点{1}", car.AgvID, car.CurrSite));
                        //car.CurrentLandMark = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == car.CurrSite.ToString());
                        //if (car.CurrentLandMark != null)
                        //{
                        //    car.X = car.CurrentLandMark.LandX;
                        //    car.Y = car.CurrentLandMark.LandY;
                        //}
                        #region 步点变化时进行交通管制
                        DateTime jgbegintime = DateTime.Now;
                        //new Thread(new ParameterizedThreadStart(Traffic.HandleTrafficForStop)) { IsBackground = true }.Start(car);
                        if (oldsite != 0)
                        { TrafficForStop(car); }
                        DateTime jgendtime = DateTime.Now;
                        LogHelper.WriteLog(car.AgvID.ToString() + "号AGV  交管耗时:" + (jgendtime - jgbegintime).TotalMilliseconds.ToString());
                        #endregion

                        //将前一个地标赋值给小车
                        if (oldsite != 0)
                        { car.HistoryUpLandCode = oldsite.ToString(); }

                        //将车子当前步点反馈给MES
                        CallWebAPI(car);

                        #region 判断回写PLC
                        ChekIsCallBack(car);
                        #endregion
                    }
                    #endregion

                    #region 处理一下回待命点的车
                    if ((car.CarState == 0 || car.CarState == 2 || car.CarState == 4) && car.CurrSite.ToString() == car.StandbyLandMark)
                    { car.IsBack = false; }
                    #endregion

                    //#region 处理任务状态
                    //if (!string.IsNullOrEmpty(car.ExcuteTaksNo) && car.TaskDetailID != -1)
                    //{
                    //    if ((car.CarState == 2 || car.CarState == 0 || car.CarState == 4) && car.CurrSite.ToString() == car.ArmLand)
                    //    {
                    //        #region 判断一下是否为需要等待一段时间再执行下条任务明细的
                    //        if (car.IsWait)
                    //        { car.BeginWaitTime = DateTime.Now; }
                    //        #endregion

                    //        #region 更新储位信息
                    //        UnLockStorage(car);
                    //        #endregion

                    //        #region 更新任务信息
                    //        AGVServerDAccess.HandTaskDetail(car.ExcuteTaksNo, car.TaskDetailID, car.CurrSite.ToString());
                    //        car.TaskDetailID = -1;
                    //        #endregion
                    //        //#region 特殊逻辑(叉车叉举走寄存器管道)
                    //        //NonStandarDispatch.WriteRegister(car.AgvID, car.OperType);
                    //        //#endregion
                    //    }
                    //}
                    //#endregion
                }
                #region 车辆信息变化时需要推送客户端
                if (IsChange)
                {
                    DelegateState.InvokeCarChangeEvent(new CarEventArgs("", car));
                    car.PreCoordinateTime = DateTime.Now;
                }
                #endregion
                DateTime endtime = DateTime.Now;
                LogHelper.WriteLog(car.AgvID.ToString() + "号AGV状态回报耗时:" + (endtime - begintime).TotalMilliseconds.ToString());
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
            }
        }


        /// <summary>
        /// 接受处理IO设备回馈信息
        /// </summary>
        public void HandleIOFeedBack(IODeviceInfo ReceveIO)
        {
            try
            {
                IODeviceInfo IOInfo = CoreData.IOList.FirstOrDefault(p => p.ID == ReceveIO.ID);
                if (IOInfo == null)
                {
                    DelegateState.InvokeDispatchStateEvent("未能找到ID为" + IOInfo.ID.ToString() + "的IO设备信息");
                    return;
                }
                lock (ReciveIOMsg_lockobj)
                {
                    if (IOInfo.bIsCommBreak && !ReceveIO.bIsCommBreak)
                    { LogHelper.WriteOffLineLog(IOInfo.ID.ToString() + "号IO设备掉线后重连上"); }
                    if (!IOInfo.bIsCommBreak && ReceveIO.bIsCommBreak)
                    { LogHelper.WriteOffLineLog(IOInfo.ID.ToString() + "号IO设备掉线"); }
                    IOInfo.GetValue(ReceveIO);
                }
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }

        /// <summary>
        /// 接受处理充电桩设备回馈信息
        /// </summary>
        /// <param name="ChargeInfo"></param>
        public void HandleChargeFeedBack(ChargeStationInfo RecChargeInfo)
        {
            try
            {
                ChargeStationInfo ChargeInfo = CoreData.ChargeList.FirstOrDefault(p => p.ID == RecChargeInfo.ID);
                if (ChargeInfo == null)
                {
                    DelegateState.InvokeDispatchStateEvent("未能找到ID为" + ChargeInfo.ID.ToString() + "的充电桩信息");
                    return;
                }
                lock (ReciveChargeMsg_lockobj)
                {
                    if (ChargeInfo.IsCommBreak && !RecChargeInfo.IsCommBreak)
                    { LogHelper.WriteOffLineLog(ChargeInfo.ID.ToString() + "号充电桩掉线后重连上"); }
                    if (!ChargeInfo.IsCommBreak && RecChargeInfo.IsCommBreak)
                    { LogHelper.WriteOffLineLog(ChargeInfo.ID.ToString() + "号充电桩掉线"); }
                    ChargeInfo.GetValue(RecChargeInfo);
                    //处理充电完成
                    CarInfo CurrChargeCar = CoreData.CarList.FirstOrDefault(p => p.CurrSite.ToString() == ChargeInfo.ChargeLandCode);
                    //充电电量阈值
                    string ChargeVoltageStr = CoreData.SysParameter["ChargeVoltage"].ToString();
                    //结束充电阈值
                    string EndChargeVoltageStr = CoreData.SysParameter["EndChargeVoltage"].ToString();
                    //开始充电时间
                    string BeginChargeTime = CoreData.SysParameter["BeginChargeTime"].ToString();
                    //结束充电时间
                    string EndChargeTime = CoreData.SysParameter["EndChargeTime"].ToString();
                    TimeSpan workStartDT = DateTime.Parse(BeginChargeTime).TimeOfDay;
                    TimeSpan workEndDT = DateTime.Parse(EndChargeTime).TimeOfDay;
                    TimeSpan nowDt = DateTime.Now.TimeOfDay;
                    double Voltage = 0;
                    try
                    {
                        //Voltage = Convert.ToDouble(ChargeVoltageStr);
                        Voltage = Convert.ToDouble(EndChargeVoltageStr);
                    }
                    catch
                    { }
                    if (ChargeInfo.ChargeState == 3 ||
                        ((!(nowDt > workStartDT && nowDt < workEndDT)) && (CurrChargeCar != null && Voltage > 0 && CurrChargeCar.fVolt >= Voltage))
                        )
                    {
                        Add_EleCommand(ChargeInfo.ID, new CommandToValue(AGVCommandEnum.StopCharge));
                        ChargeInfo.ChargeLock = 0;

                        if (CurrChargeCar != null)
                        {
                            CurrChargeCar.LowPower = false;
                            CurrChargeCar.PreChargeTime = DateTime.Now;
                            CurrChargeCar.NowChargeLandCode = "";
                            //记忆一下当前车本次充电时间
                            //try
                            //{
                            //    string filePath = System.AppDomain.CurrentDomain.BaseDirectory + @"\MemoryFile\CarTaskInfo.xml";
                            //    Hashtable ht = new Hashtable();
                            //    ht["PreChargeTime"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                            //    XMLClass.AppendXML(filePath, "N" + CurrChargeCar.AgvID.ToString(), ht);
                            //}
                            //catch (Exception ex)
                            //{ }
                        }
                    }
                }
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }

        /// <summary>
        /// 定时启动被交管停止的agv
        /// </summary>
        private void TimerStarBeStopedCar_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                timerStarBeStopedCar.Enabled = false;
                Traffic.HandleTrafficForStart();
                //new Thread(new ThreadStart(Traffic.HandleTrafficForStart)) { IsBackground = true }.Start();
            }
            catch (Exception ex)
            {
                DelegateState.InvokeDispatchStateEvent("定时启动被交通车辆异常:" + ex.Message);
                LogHelper.WriteErrorLog(ex);
            }
            finally
            { timerStarBeStopedCar.Enabled = true; }
        }

        /// <summary>
        /// 定时拉取执行任务并规划相应路线
        /// </summary>
        private void TimerFreshTask_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                lock (HandleTaskobj)
                {
                    timerFreshTask.Enabled = false;
                    #region 处理完成任务的车子
                    foreach (CarInfo car in CoreData.CarList.Where(p => (!string.IsNullOrEmpty(p.ExcuteTaksNo)) && p.TaskDetailID >= 0))
                    {
                        FinishTask(car);

                        #region 处理任务状态
                        if (!string.IsNullOrEmpty(car.ExcuteTaksNo) && car.TaskDetailID != -1)
                        {
                            if ((car.CarState == 2 || car.CarState == 0 || car.CarState == 4) && car.CurrSite.ToString() == car.ArmLand)
                            {
                                #region 判断一下是否为需要等待一段时间再执行下条任务明细的
                                if (car.IsWait)
                                { car.BeginWaitTime = DateTime.Now; }
                                #endregion

                                #region 更新储位信息
                                UnLockStorage(car);
                                #endregion

                                #region 更新任务信息
                                AGVServerDAccess.HandTaskDetail(car.AgvID, car.ExcuteTaksNo, car.TaskDetailID, car.CurrSite.ToString());
                                car.TaskDetailID = -1;

                                DispatchTaskInfo CurrTaskInfo = AGVServerDAccess.LoadTaskByNo(car.ExcuteTaksNo);
                                if (CurrTaskInfo == null || (CurrTaskInfo != null && (CurrTaskInfo.TaskState != 1 && CurrTaskInfo.TaskState != 0)))
                                {
                                    car.ExcuteTaksNo = "";
                                    LogHelper.WriteCreatTaskLog("更新车:" + car.AgvID.ToString() + "所执行的任务" + car.ExcuteTaksNo + "完成!");
                                    try
                                    {
                                        //将AGV车辆任务信息记录到记忆文件当中
                                        string filePath = System.AppDomain.CurrentDomain.BaseDirectory + @"\MemoryFile\CarTaskInfo.xml";
                                        Hashtable ht = new Hashtable();
                                        ht["TaskNo"] = "";
                                        ht["TaskDetailID"] = "-1";
                                        XMLClass.AppendXML(filePath, "N" + car.AgvID.ToString(), ht);
                                    }
                                    catch (Exception ex)
                                    { LogHelper.WriteErrorLog(ex); }
                                }
                                #endregion
                                //#region 特殊逻辑(叉车叉举走寄存器管道)
                                //NonStandarDispatch.WriteRegister(car.AgvID, car.OperType);
                                //#endregion
                            }
                        }
                        #endregion
                    }
                    #endregion

                    #region 判断是否处理自动充电
                    CheckSetToChargeRoute();
                    #endregion

                    #region 处理分解具体任务并派发

                    #region 搞不清楚要这个干嘛
                    //修改任务状态
                    //string IsKeepCall = CoreData.SysParameter["IsKeepCall"].ToString();
                    //if (IsKeepCall == "是")
                    //{
                    //    AGVServerDAccess.UpdateTask();
                    //}
                    #endregion

                    //查找当前有效任务
                    IList<DispatchTaskInfo> tasks = AGVServerDAccess.LoadDispatchTask("0,1");
                    if (tasks != null)
                    {
                        foreach (DispatchTaskInfo task in tasks)
                        {
                            //判断一下任务超时
                            bool IsOutTime = ChekTaskOverTime(task);
                            if (IsOutTime)
                            { continue; }

                            //没有超时的任务则将分派具体任务信息
                            if (task.TaskDetail.Count <= 0) { continue; }
                            //开始查找合适的AGV来领取任务
                            //如果任务状态为待处理的,就找空闲的车来执行
                            //如果任务是正在执行中的，则找对应执行小车且状态变成可接受任务的agv来继续执行
                            CarInfo NoTaskCar = null;
                            //查找任务到具体任务明细
                            DispatchTaskDetail TaskDetail = null;
                            if (task.TaskState == 0)
                            {
                                TaskDetail = (from a in task.TaskDetail
                                              where (a.State == 0/* || a.State == 1*/)
                                              orderby a.DetailID ascending
                                              select a).FirstOrDefault();
                                if (TaskDetail != null)
                                {
                                    NoTaskCar = (from a in CoreData.CarList
                                                 where (!string.IsNullOrEmpty(a.CurrSite.ToString())) && a.CurrSite > 0
                                                 && (a.CarState == 0 || a.CarState == 2 || a.CarState == 4 || a.IsBack/* || a.CarState == 5*/) && !a.LowPower
                                                 && !a.bIsCommBreak && a.ExcuteTaksNo == ""
                                                 //根据任务区域查找车辆
                                                 && (a.OwerArea == task.OwerArea || a.OwerArea == 0)
                                                 //并且没有执行其他任务的车
                                                 && tasks.Count(p => p.ExeAgvID == a.AgvID && p.TaskState == 1) <= 0
                                                 orderby getDistant(a.CurrSite.ToString(), TaskDetail.LandCode) ascending
                                                 select a).FirstOrDefault();
                                }
                            }
                            else if (task.TaskState == 1)
                            {
                                TaskDetail = (from a in task.TaskDetail
                                              where (a.State == 0 || a.State == 1)
                                              orderby a.DetailID ascending
                                              select a).FirstOrDefault();
                                NoTaskCar = CoreData.CarList.FirstOrDefault(p => p.AgvID == task.ExeAgvID && ((p.CarState == 0 || p.CarState == 2 || p.CarState == 4 || p.IsBack /*|| p.CarState == 5*/) && !p.LowPower && !p.bIsCommBreak /*&& (p.CarState == 4 || p.TaskDetailID == -1)*/));
                            }
                            //到此步已找到相应执行任务的agv
                            if (NoTaskCar != null && NoTaskCar.CurrSite > 0)
                            {
                                if (NoTaskCar.CarState != 4 && Traffic.CheckCarIsInLockSource(NoTaskCar))
                                {
                                    LogHelper.WriteCreatTaskLog("车号:" + NoTaskCar.AgvID.ToString() + "还被交管中或交管了别车,所以先不派发任务");
                                    continue;
                                }


                                LandmarkInfo BeginLand, EndLand = null;
                                //如果当前任务明细已经没有了则让领取当前的车回其待命点
                                if (TaskDetail == null)
                                {
                                    #region 任务明细没有
                                    LogHelper.WriteCreatTaskLog("定时拉取任务,没有任务明细!");

                                    //AGVServerDAccess.UpdateTaskState(task.dispatchNo, 2);
                                    //NoTaskCar.ExcuteTaksNo = "";
                                    //NoTaskCar.TaskDetailID = -1;
                                    //LogHelper.WriteCreatTaskLog("更新车:" + NoTaskCar.AgvID.ToString() + "所执行的任务" + task.dispatchNo + "完成!");
                                    //try
                                    //{
                                    //    //将AGV车辆任务信息记录到记忆文件当中
                                    //    string filePath = System.AppDomain.CurrentDomain.BaseDirectory + @"\MemoryFile\CarTaskInfo.xml";
                                    //    Hashtable ht = new Hashtable();
                                    //    ht["TaskNo"] = "";
                                    //    ht["TaskDetailID"] = "-1";
                                    //    XMLClass.AppendXML(filePath, "N" + NoTaskCar.AgvID.ToString(), ht);
                                    //}
                                    //catch (Exception ex)
                                    //{ LogHelper.WriteErrorLog(ex); }


                                    #region 回待命点下面统一处理
                                    ////先判断当前车是否维护了待命点
                                    //if (string.IsNullOrEmpty(NoTaskCar.StandbyLandMark)) { continue; }
                                    //BeginLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == NoTaskCar.CurrSite.ToString());
                                    //EndLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == NoTaskCar.StandbyLandMark);
                                    ////以车的当前地标为开始地标,待命点为结束地标规划回待命点路线
                                    //if (BeginLand == null || EndLand == null)
                                    //{
                                    //    LogHelper.WriteCreatTaskLog("车:" + NoTaskCar.AgvID.ToString() + "任务完成后回待命点,但是待命点地标不存在!");
                                    //    continue;
                                    //}
                                    //else
                                    //{
                                    //    if (NoTaskCar.CarState!=4&&Traffic.CheckCarIsInLockSource(NoTaskCar))
                                    //    {
                                    //        LogHelper.WriteCreatTaskLog("车号:" + NoTaskCar.AgvID.ToString() + "还被交管中或交管了别车,所以先不回待命点");
                                    //        continue;
                                    //    }
                                    //    //规划路线
                                    //    NoTaskCar.Route = DataToObject.CreateDeepCopy<List<LandmarkInfo>>(CountRoute.GetRoute(BeginLand, EndLand));
                                    //    NoTaskCar.AllRouteLandCode = "," + string.Join(",", NoTaskCar.Route.Select(p => p.LandmarkCode)) + ",";
                                    //    try
                                    //    {
                                    //        AGVServerDAccess.SaveCarCurrentRoute(NoTaskCar);
                                    //    }
                                    //    catch (Exception ex)
                                    //    { LogHelper.WriteCreatTaskLog("保存AGV当前线路异常:" + ex.Message); }
                                    //    if (NoTaskCar.Route.Count <= 0)
                                    //    {
                                    //        NoTaskCar.IsBack = false;
                                    //        LogHelper.WriteCreatTaskLog("车:" + NoTaskCar.AgvID.ToString() + "任务完成后回待命点,回待命点路线长度为0,表示车子就在待命点了!");
                                    //        continue;
                                    //    }
                                    //    //如果车辆状态不为初始化状态时需要发送一下初始化命令or取消任务命令
                                    //    //if (NoTaskCar.CarState != 0)
                                    //    //{ AGV_AddCommand(NoTaskCar.AgvID, new CommandToValue(AGVCommandEnum.CancelTast)); }
                                    //    if (SendCarRoute(NoTaskCar, SendRouteType.BackStandLand, null))
                                    //    {
                                    //        LogHelper.WriteCreatTaskLog(NoTaskCar.AgvID.ToString() + "已发送回待命点!");
                                    //        NoTaskCar.IsBack = true;
                                    //        NoTaskCar.ExcuteTaksNo = "";
                                    //        NoTaskCar.TaskDetailID = -1;
                                    //        try
                                    //        {
                                    //            BackTaskInfo BackTask = new BackTaskInfo();
                                    //            BackTask.TaskNo = task.dispatchNo;
                                    //            BackTask.TaskState = "2";
                                    //            string Params = JosnTool.GetJson(BackTask);
                                    //            //CallWebService("BackTaskInfo", Params);
                                    //        }
                                    //        catch
                                    //        {
                                    //        }
                                    //       c("更新车:" + NoTaskCar.AgvID.ToString() + "所执行的任务完成!");
                                    //        try
                                    //        {
                                    //            //将AGV车辆任务信息记录到记忆文件当中
                                    //            string filePath = System.AppDomain.CurrentDomain.BaseDirectory + @"\MemoryFile\CarTaskInfo.xml";
                                    //            Hashtable ht = new Hashtable();
                                    //            ht["TaskNo"] = "";
                                    //            ht["TaskDetailID"] = "-1";
                                    //            XMLClass.AppendXML(filePath, "N" + NoTaskCar.AgvID.ToString(), ht);
                                    //        }
                                    //        catch (Exception ex)
                                    //        { LogHelper.WriteErrorLog(ex); }
                                    //    }
                                    //}
                                    #endregion

                                    #endregion

                                }//end TaskDetail==null
                                else
                                {
                                    #region
                                    //DispatchTaskDetail PreTaskDetail = (from a in task.TaskDetail
                                    //                                    where a.LandCode == NoTaskCar.CurrSite.ToString() && (NoTaskCar.CarState == 0 || NoTaskCar.CarState == 2)
                                    //                                    orderby a.DetailID ascending
                                    //                                    select a).FirstOrDefault();
                                    //判断是否放行
                                    //DispatchTaskDetail PreTaskDetail = AGVServerDAccess.LoadTaskDetailByCondition(NoTaskCar.ExcuteTaksNo, NoTaskCar.TaskDetailID - 1);

                                    ////如果不允许执行则跳过s
                                    //if (PreTaskDetail != null && PreTaskDetail.IsAllowExcute == 0 && PreTaskDetail.PassType == 0)
                                    //{
                                    //    //判断是否需要远程IO通信
                                    //    if (!string.IsNullOrEmpty(TaskDetail.TaskConditonCode) && TaskDetail.TaskConfigDetailID != -1)
                                    //    {
                                    //        //根据配置的IO通信信号进行发送相应指令
                                    //        //IList <TaskConfigMustPass>
                                    //    }
                                    //    continue;
                                    //}
                                    ////如果为延时自动启动
                                    //else if (PreTaskDetail != null && PreTaskDetail.IsAllowExcute == 1 && PreTaskDetail.PassType == 1)
                                    //{
                                    //    //判断是否到达延时时间
                                    //    double WaitTime = Convert.ToDouble(CoreData.SysParameter["WaitTime"]);
                                    //    DateTime FinishTime = DateTime.ParseExact(PreTaskDetail.FinishTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                                    //    if ((DateTime.Now - FinishTime).TotalSeconds < WaitTime)
                                    //    {
                                    //        continue;
                                    //    }
                                    //}
                                    #endregion
                                    //判断是否达到等待时间
                                    if (TaskDetail.WaitTime > 0)
                                    {
                                        if (TaskDetail.DetailID == 1)//第一条
                                        {
                                            DateTime FinishTime = DateTime.ParseExact(task.BuildTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                                            if ((DateTime.Now - FinishTime).TotalSeconds < TaskDetail.WaitTime)
                                            {
                                                LogHelper.WriteCreatTaskLog(NoTaskCar.AgvID.ToString() + "在等待:" + NoTaskCar.WaitTime.ToString() + "秒后启动!");
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            DispatchTaskDetail PreTaskDetail = AGVServerDAccess.LoadTaskDetailByCondition(NoTaskCar.ExcuteTaksNo, TaskDetail.DetailID - 1);
                                            if (PreTaskDetail != null && !string.IsNullOrEmpty(PreTaskDetail.FinishTime))
                                            {
                                                DateTime FinishTime = DateTime.ParseExact(PreTaskDetail.FinishTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                                                if ((DateTime.Now - FinishTime).TotalSeconds < TaskDetail.WaitTime)
                                                {
                                                    LogHelper.WriteCreatTaskLog(NoTaskCar.AgvID.ToString() + "在等待:" + NoTaskCar.WaitTime.ToString() + "秒后启动!");
                                                    continue;
                                                }
                                            }

                                        }
                                    }

                                    //if (NoTaskCar.IsWait && NoTaskCar.BeginWaitTime.ToString("yyyy") != "0001" && (DateTime.Now - NoTaskCar.BeginWaitTime).TotalMinutes < NoTaskCar.WaitTime)
                                    //{
                                    //    LogHelper.WriteCreatTaskLog(NoTaskCar.AgvID.ToString() + "在等待:" + NoTaskCar.WaitTime.ToString() + "分钟后启动!");
                                    //    continue;
                                    //}

                                    LogHelper.WriteCreatTaskLog("分派任务");
                                    //判断小车当前的任务明细是否允许需要等待放行命令
                                    //if (TaskDetail.IsAllowExcute == 0 && TaskDetail.PassType == 0)
                                    //{ continue; }


                                    //以小车当前站点为路线的开始点
                                    //以任务明细的站点为路线的结束点
                                    BeginLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == NoTaskCar.CurrSite.ToString());
                                    EndLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == TaskDetail.LandCode);
                                    if (EndLand == null)
                                    { DelegateState.InvokeDispatchStateEvent("执行任务小车ID:" + NoTaskCar.AgvID.ToString() + "目的站点:" + TaskDetail.LandCode + "不存在!"); }
                                    //DelegateState.InvokeDispatchStateEvent("执行小车ID:" + NoTaskCar.AgvID.ToString() + "当前站点:" + NoTaskCar.CurrSite.ToString() + "执行任务明细:" + TaskDetail.DetailID);
                                    //DelegateState.InvokeDispatchStateEvent("实际执行小车ID:" + NoTaskCar.AgvID.ToString() + "开始地标:" + BeginLand.LandmarkCode + "结束地标:" + EndLand.LandmarkCode);
                                    if (BeginLand != null && EndLand != null)
                                    {
                                        NoTaskCar.Route = DataToObject.CreateDeepCopy<List<LandmarkInfo>>(CountRoute.GetRoute(BeginLand, EndLand));
                                        if (NoTaskCar.Route.Count <= 0)
                                        {
                                            DelegateState.InvokeDispatchStateEvent("实际执行小车ID:" + NoTaskCar.AgvID.ToString() + "开始地标:" + BeginLand.LandmarkCode + "结束地标:" + EndLand.LandmarkCode + "线路不通!");
                                            continue;
                                        }
                                        NoTaskCar.AllRouteLandCode = "," + string.Join(",", NoTaskCar.Route.Select(p => p.LandmarkCode)) + ",";
                                        try
                                        {
                                            AGVServerDAccess.SaveCarCurrentRoute(NoTaskCar);
                                        }
                                        catch (Exception ex)
                                        { LogHelper.WriteCreatTaskLog("保存AGV当前线路异常:" + ex.Message); }

                                        if (SendCarRoute(NoTaskCar, SendRouteType.ExcuteTask, TaskDetail))
                                        {
                                            //调用接口返回任务状态
                                            try
                                            {
                                                BackTaskInfo BackTask = new BackTaskInfo();
                                                BackTask.TaskNo = TaskDetail.dispatchNo;
                                                BackTask.TaskState = "1";
                                                string Params = JosnTool.GetJson(BackTask);
                                                //CallWebService("BackTaskInfo", Params);
                                            }
                                            catch
                                            {
                                            }

                                            //领取并开始执行任务
                                            //记录当前小车的任务信息
                                            //更新任务为执行状态
                                            AGVServerDAccess.TaskHandle(task.dispatchNo, NoTaskCar.AgvID, 1, TaskDetail.LandCode, TaskDetail.DetailID);
                                            NoTaskCar.ExcuteTaksNo = task.dispatchNo;
                                            NoTaskCar.TaskDetailID = TaskDetail.DetailID;
                                            NoTaskCar.IsBack = false;
                                            NoTaskCar.ArmLand = TaskDetail.LandCode;
                                            NoTaskCar.PutType = TaskDetail.PutType;
                                            NoTaskCar.OperType = TaskDetail.OperType;
                                            NoTaskCar.IsWait = TaskDetail.IsWait == 0 ? false : true;
                                            NoTaskCar.WaitTime = TaskDetail.WaitTime;
                                            //需要文件记录一下小车的当前任务号，因为如果程序重启，内存中的任务号会清空，导致可能出现低电压的小车带着货架去充电
                                            //因为判断小车是否去充电是要判断当前小车是否有任务号的，没有任务时小车才能去充电的
                                            try
                                            {
                                                string filePath = System.AppDomain.CurrentDomain.BaseDirectory + @"\MemoryFile\CarTaskInfo.xml";
                                                Hashtable ht = new Hashtable();
                                                ht["TaskNo"] = task.dispatchNo;
                                                ht["TaskDetailID"] = TaskDetail.DetailID.ToString();
                                                ht["ArmLand"] = NoTaskCar.ArmLand;
                                                XMLClass.AppendXML(filePath, "N" + NoTaskCar.AgvID.ToString(), ht);
                                            }
                                            catch (Exception ex)
                                            { LogHelper.WriteErrorLog(ex); }
                                            //DelegateState.InvokeDispatchStateEvent("启动小车:" + NoTaskCar.AgvID.ToString() + "执行任务号:" + NoTaskCar.ExcuteTaksNo + "任务明细:" + NoTaskCar.TaskDetailID.ToString() + "成功!");
                                        }
                                    }
                                }//end TaskDetail!=null

                            }//end NoTaskCar!=null
                        }//end foreach Tasks
                    }

                    #endregion

                    #region 处理未回待命点的车辆
                    LogHelper.WriteCreatTaskLog("查找未回待命点的车!");
                    List<CarInfo> NoBackCars = CoreData.CarList.Where(p =>/* !p.IsBack &&*/ !p.LowPower && !p.bIsCommBreak && (p.CarState == 0 || p.CarState == 2 || p.CarState == 4) && string.IsNullOrEmpty(p.ExcuteTaksNo) && p.CurrSite.ToString() != p.StandbyLandMark).ToList();
                    foreach (CarInfo car in NoBackCars)
                    {
                        if (car.CarState != 4 && Traffic.CheckCarIsInLockSource(car))
                        {
                            LogHelper.WriteCreatTaskLog("车号:" + car.AgvID.ToString() + "还被交管中或交管了别车,所以先不回待命点");
                            continue;
                        }


                        LogHelper.WriteCreatTaskLog(car.AgvID.ToString() + "开始回待命点!");
                        //判断当前agv是否维护待命点
                        if (string.IsNullOrEmpty(car.StandbyLandMark)) { continue; }
                        LandmarkInfo BeginLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == car.CurrSite.ToString());
                        LandmarkInfo EndLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == car.StandbyLandMark);
                        //以车的当前地标为开始地标,待命点为结束地标规划回待命点路线
                        if (BeginLand == null || EndLand == null)
                        { continue; }
                        else
                        {
                            //规划路线
                            car.Route = DataToObject.CreateDeepCopy<List<LandmarkInfo>>(CountRoute.GetRoute(BeginLand, EndLand));
                            if (car.Route.Count <= 0) { continue; }
                            car.AllRouteLandCode = "," + string.Join(",", car.Route.Select(p => p.LandmarkCode)) + ",";
                            try
                            {
                                AGVServerDAccess.SaveCarCurrentRoute(car);
                            }
                            catch (Exception ex)
                            { LogHelper.WriteCreatTaskLog("保存AGV当前线路异常:" + ex.Message); }

                            //if (car.CarState != 0)
                            //{
                            //    //先复位
                            //    AGV_AddCommand(car.AgvID, new CommandToValue(AGVCommandEnum.CancelTast));
                            //}
                            if (SendCarRoute(car, SendRouteType.BackStandLand, null))
                            {
                                LogHelper.WriteCreatTaskLog(car.AgvID.ToString() + "已发送回待命点!");
                                car.IsBack = true;
                            }
                        }
                    }
                    #endregion

                    #region 处理一下任务完成的车子的记忆路线
                    List<CarInfo> FinishCars = CoreData.CarList.Where(P => P.Route.Count > 0 && (P.Route.FindLastIndex(q => q.LandmarkCode == P.CurrSite.ToString()) == P.Route.Count - 1)).ToList();
                    foreach (CarInfo car in FinishCars)
                    {
                        AGVServerDAccess.DeleteCarRemeberRoute(car.AgvID);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                //DelegateState.InvokeDispatchStateEvent("定时拉取未执行任务异常:" + ex.Message);
                LogHelper.WriteErrorLog(ex);
            }
            finally
            { timerFreshTask.Enabled = true; }
        }
        #endregion

        #region 自定义函数

        private CommandToValue getRouteCommd(CarInfo car, SendRouteType RouteType, DispatchTaskDetail TaskDetail)
        {
            try
            {
                int PreLandAngel = 0;
                string RoutLog = "";
                //中间所有有动作的地标
                List<LandmarkInfo> MidLands = car.Route.Where(p => p.LandmarkCode != car.Route[0].LandmarkCode && p.LandmarkCode != car.Route.Last().LandmarkCode).ToList();
                List<LandmarkInfo> ActionLand = MidLands.Where(p => p.sway != SwayEnum.None || p.movedirect != MoveDirectEnum.None || p.ExcuteSpeed != -1 || p.ExcuteAvoidance != -1).ToList();
                //动作命令集合
                List<CommandLandMark> ComodLandList = new List<CommandLandMark>();
                //动作命令实体
                CommandLandMark ComodLand = null;
                //起始地标
                //开始位
                ComodLand = new CommandLandMark();
                ComodLand.LandmarkCode = car.Route[0].LandmarkCode;
                ComodLand.X = car.Route[0].LandX * CoreData.ScalingRate;//car.X;
                ComodLand.Y = car.Route[0].LandY * CoreData.ScalingRate;//car.Y;
                //方向
                ComodLand.Turn_Direact = (int)car.Route[0].sway;
                if (car.Route[0].movedirect != MoveDirectEnum.None)
                {
                    ComodLand.Move_Direact = (int)car.Route[0].movedirect;
                }
                else if (car.Route[1].LandmarkCode == car.HistoryUpLandCode)
                { ComodLand.Move_Direact = (int)MoveDirectEnum.Reverse; }
                else
                { ComodLand.Move_Direact = (int)MoveDirectEnum.None; }

                //速度
                ComodLand.Move_Speed = car.Route[0].ExcuteSpeed == -1 ? car.speed : car.Route[0].ExcuteSpeed;
                ComodLand.Avoidance = car.Route[0].ExcuteAvoidance == -1 ? 1 : car.Route[0].ExcuteAvoidance;

                //角度,如果是二维码则赋值角度字段
                ComodLand.Angle = car.Route[0].Angle;
                //如果车辆支持车头方向的则根据车头方向判断是否车辆拐弯旋转
                if (car.Angel >= 0 && car.Route[0] != null && Math.Abs(car.Angel - car.Route[0].Angle) > 10)
                { car.Route[0].IsRotateLand = true; }
                PreLandAngel = car.Route[0].Angle;
                //动作
                if (RouteType == SendRouteType.BackStandLand || RouteType == SendRouteType.GoToCharge || RouteType == SendRouteType.HandTask)
                {
                    //如果是回待命点则先将机械动作归位,如:降下勾销、降下托盘、降下叉臂等
                    if (car.BangState != 0 || car.JCState != 0)
                    { ComodLand.ActionType = 8; }
                    else
                    { ComodLand.ActionType = 0; }
                }
                else
                {
                    //根据任务明细判断车到达起点处执行什么样的动作
                    //如果车就在任务的目的地标上,那么就需要执行相应的目的动作
                    if (TaskDetail != null && car.CurrSite.ToString() == TaskDetail.LandCode)
                    {
                        if (TaskDetail.OperType == 0)//取 就是升
                        {
                            //根据物料档案中的夹抱尺寸
                            StorageInfo store = CoreData.StorageList.FirstOrDefault(p => p.LankMarkCode == TaskDetail.LandCode);
                            if (store != null)
                            {
                                MaterialInfo mat = CoreData.AllMaterials.FirstOrDefault(p => p.MaterialType == store.MaterielType);
                                if (mat != null)
                                {
                                    ComodLand.ActionType = 6 - mat.PickSize;
                                }
                                else
                                { ComodLand.ActionType = 0; }
                            }
                            else
                            { ComodLand.ActionType = 0; }
                        }
                        else if (TaskDetail.OperType == 1)//放 将
                        {
                            //放都是先张大后下降
                            ComodLand.ActionType = 8;
                        }
                        else if (TaskDetail.OperType == 2)//自动充电
                        {
                            ComodLand.ActionType = 1;
                        }
                        else//无动作
                        {
                            ComodLand.ActionType = 0;
                        }
                    }
                    else
                    { ComodLand.ActionType = 0; }
                }
                //赋值一下小车动作的参数
                ComodLand.ActionParameter = TaskDetail != null ? TaskDetail.AGVActionParameter : 0;
                ComodLandList.Add(ComodLand);
                RoutLog += ComodLand.LandmarkCode + ";";
                //添加中间地标
                for (int i = 0; i < ActionLand.Count; i++)
                {
                    LandmarkInfo land = ActionLand[i];
                    if (ComodLandList.Count(p => p.LandmarkCode == land.LandmarkCode) <= 0)
                    {
                        ComodLand = new CommandLandMark();
                        //地标
                        ComodLand.LandmarkCode = land.LandmarkCode;
                        ComodLand.X = land.LandX * CoreData.ScalingRate;
                        ComodLand.Y = land.LandY * CoreData.ScalingRate;
                        ComodLand.Angle = land.Angle;
                        ComodLand.Turn_Direact = (int)land.sway;
                        ComodLand.Move_Direact = (int)land.movedirect;
                        ComodLand.Move_Speed = land.ExcuteSpeed == -1 ? car.speed : car.Route[0].ExcuteSpeed;
                        ComodLand.Avoidance = land.ExcuteAvoidance == -1 ? 1 : land.ExcuteAvoidance;
                        ComodLand.ActionType = 0;
                        #region 暂时不考虑,因为在开始领任务是就将平台夹抱到最大
                        ////在路线中的倒数第二个地表需要做预备取放料动作
                        //if (i == ActionLand.Count - 1)
                        //{
                        //    if (TaskDetail.OperType == 0)//取 就是升
                        //    {
                        //        //根据物料档案中的夹抱尺寸
                        //        StorageInfo store = CoreData.StorageList.FirstOrDefault(p => p.LankMarkCode == TaskDetail.LandCode);
                        //        if (store != null)
                        //        {
                        //            MaterialInfo mat = CoreData.AllMaterials.FirstOrDefault(p => p.MaterialType == store.MaterielType);
                        //            if (mat != null)
                        //            {
                        //                ComodLand.ActionType = 6 - mat.PickSize;
                        //            }
                        //            else
                        //            { ComodLand.ActionType = 7; }
                        //        }
                        //        else
                        //        { ComodLand.ActionType = 7; }
                        //    }
                        //}
                        #endregion

                        if (land != null && Math.Abs(PreLandAngel - land.Angle) > 10)
                        { land.IsRotateLand = true; }
                        PreLandAngel = land.Angle;
                        //方向
                        ComodLandList.Add(ComodLand);
                        RoutLog += ComodLand.LandmarkCode + ";";
                    }
                }
                //添加结束地标
                LandmarkInfo lastLand = car.Route.Last();
                if (lastLand != null)
                {
                    if (ComodLandList.Count(p => p.LandmarkCode == lastLand.LandmarkCode) <= 0)
                    {
                        ComodLand = new CommandLandMark();
                        //地标
                        ComodLand.LandmarkCode = lastLand.LandmarkCode;
                        ComodLand.X = lastLand.LandX * CoreData.ScalingRate;
                        ComodLand.Y = lastLand.LandY * CoreData.ScalingRate;
                        ComodLand.Turn_Direact = (int)lastLand.sway;
                        ComodLand.Move_Direact = (int)lastLand.movedirect;
                        ComodLand.Move_Speed = lastLand.ExcuteSpeed == -1 ? car.speed : car.Route[0].ExcuteSpeed;
                        ComodLand.Avoidance = lastLand.ExcuteAvoidance == -1 ? 1 : lastLand.ExcuteAvoidance;
                        //最后一个地标的角度按照前一个动作地标的角度,即,需要在在最后一个地标的角度需要提前去调整好
                        ComodLand.Angle = PreLandAngel;
                        //动作
                        if (RouteType == SendRouteType.BackStandLand)
                        {
                            //如果是回待命点则先将机械动作归位,如:降下勾销、降下托盘、降下叉臂等
                            ComodLand.ActionType = 8;
                        }
                        else if (RouteType == SendRouteType.GoToCharge)
                        {
                            //如果是去充电则切换到自动充电动作
                            ComodLand.ActionType = 1;
                        }
                        else
                        {
                            //根据任务明细判断车到达起点处执行什么样的动作
                            //如果车就在任务的目的地标上,那么就需要执行相应的目的动作
                            if (TaskDetail != null/* && car.CurrSite.ToString() == TaskDetail.LandCode*/)
                            {
                                if (TaskDetail.OperType == 0)//取 就是升
                                {
                                    //根据物料档案中的夹抱尺寸
                                    StorageInfo store = CoreData.StorageList.FirstOrDefault(p => p.LankMarkCode == TaskDetail.LandCode);
                                    if (store != null)
                                    {
                                        MaterialInfo mat = CoreData.AllMaterials.FirstOrDefault(p => p.MaterialType == store.MaterielType);
                                        if (mat != null)
                                        {
                                            ComodLand.ActionType = 6 - mat.PickSize;
                                        }
                                        else
                                        { ComodLand.ActionType = 0; }
                                    }
                                    else
                                    { ComodLand.ActionType = 0; }
                                }
                                else if (TaskDetail.OperType == 1)//放 将
                                {
                                    ComodLand.ActionType = 8;
                                }
                                else if (TaskDetail.OperType == 2)//自动充电
                                { ComodLand.ActionType = 1; }
                                else//无动作
                                { ComodLand.ActionType = 0; }
                            }
                            else
                            { ComodLand.ActionType = 0; }
                            if (TaskDetail != null)
                            { ComodLand.IsSensorStop = TaskDetail.IsSensorStop; }
                        }
                        ComodLandList.Add(ComodLand);
                        RoutLog += ComodLand.LandmarkCode + ";";
                    }
                }
                CommandToValue CommandTo = null;
                if (RouteType == SendRouteType.BackStandLand)
                {
                    //环红
                    //CommandTo = new CommandToValue(AGVCommandEnum.RouteSiteSet);
                    //名硕
                    CommandTo = new CommandToValue(AGVCommandEnum.ChangeRoute);
                    LogHelper.WriteLog("发送回待命点手动启动");
                }
                else
                {
                    if (TaskDetail != null)
                    {
                        //DispatchTaskDetail PreDetail = AGVServerDAccess.LoadTaskDetailByCondition(TaskDetail.dispatchNo, TaskDetail.DetailID - 1);
                        LogHelper.WriteLog("发送当前任务号:" + TaskDetail.dispatchNo + ";任务明细:" + TaskDetail.DetailID);
                        //if (PreDetail != null && PreDetail.DetailID != TaskDetail.DetailID && (car.CarState == 0 || car.CarState == 2) && PreDetail.PassType == 1/*&& TaskDetail.LandCode== car.CurrSite.ToString()*/)
                        //{
                        //    CommandTo = new CommandToValue(AGVCommandEnum.RouteSiteSet);
                        //    LogHelper.WriteLog("发送手动启动");
                        //}
                        //else
                        //{
                        CommandTo = new CommandToValue(AGVCommandEnum.ChangeRoute);
                        LogHelper.WriteLog("发送自动启动");
                        //}
                    }
                    else
                    {
                        CommandTo = new CommandToValue(AGVCommandEnum.ChangeRoute);
                        LogHelper.WriteLog("发送自动启动");
                    }
                }
                CommandTo.CommandValue = ComodLandList;
                LogHelper.WriteCreatTaskLog("组装车辆: " + car.AgvID.ToString() + "-- > 路线为:" + RoutLog);
                //DelegateState.InvokeDispatchStateEvent("组装车辆:" + car.AgvID.ToString() + "-->路线指令为:" + RoutLog);
                return CommandTo;
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); return null; }
        }

        /// <summary>
        /// 发送AGV路线指令
        /// </summary>
        public bool SendCarRoute(CarInfo car, SendRouteType RouteType, DispatchTaskDetail TaskDetail)
        {
            try
            {
                if (car.Route.Count <= 1) { return false; }
                string Route = string.Join(",", car.Route.Select(p => p.LandmarkCode));
                LogHelper.WriteCreatTaskLog("车号:" + car.AgvID.ToString() + "下发路线为:" + Route);


                CommandToValue CommandTo = getRouteCommd(car, RouteType, TaskDetail);
                if (CommandTo == null)
                {
                    LogHelper.WriteCreatTaskLog("车号:" + car.AgvID.ToString() + "转换路线指令失败!");
                    return false;
                }
                //下发路线前判断一下交通管制
                if (!Traffic.BeforStartTrafficForStop(car))
                {
                    car.RealyRoute = DataToObject.CreateDeepCopy<List<LandmarkInfo>>(car.Route);
                    car.RealyAllRouteLandCode = "," + string.Join(",", car.RealyRoute.Select(p => p.LandmarkCode)) + ",";
                    //启动的时候需要把当前车的锁定资源集恢复正常，并且删除当前车的锁资源集
                    Traffic.GetTrafficResour(car);
                    ChargeStationInfo FreeChargeStation = CoreData.ChargeList.FirstOrDefault(p => p.ChargeLandCode == car.CurrSite.ToString());
                    if (FreeChargeStation != null && FreeChargeStation.ChargeState == 0)
                    {
                        FreeChargeStation.ChargeLock = 0;
                        Add_EleCommand(FreeChargeStation.ID, new CommandToValue(AGVCommandEnum.StopCharge));
                        car.NowChargeLandCode = "";
                        car.LowPower = false;
                    }
                    if (car.CarState == 1 || car.CarState == 3)
                    {
                        AGV_AddCommand(car.AgvID, new CommandToValue(AGVCommandEnum.CancelTast));
                        //DelegateState.InvokeDispatchStateEvent("下发车辆:" + car.AgvID.ToString() + "清除任务,因为状态为正在执行中.");
                        SendRouteCommd(car, RouteType, TaskDetail);
                    }
                    else
                    {
                        car.RealyRoute = DataToObject.CreateDeepCopy<List<LandmarkInfo>>(car.Route);
                        car.RealyAllRouteLandCode = "," + string.Join(",", car.RealyRoute.Select(p => p.LandmarkCode)) + ",";
                        AGV_AddCommand(car.AgvID, CommandTo);
                    }



                    //if (RouteType == SendRouteType.HandTask)
                    //{ AGV_AddCommand(car.AgvID, CommandTo); }
                    //else
                    //{
                    //    if (car.CarState == 0 || car.CarState == 2 || car.CarState == 4)
                    //    { AGV_AddCommand(car.AgvID, CommandTo); }
                    //    else
                    //    { return false; }
                    //}

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); return false; }
        }

        /// <summary>
        /// 判断是否重新计算路线
        /// </summary>
        private void SendRouteCommd(CarInfo car, SendRouteType RouteType, DispatchTaskDetail TaskDetail)
        {
            try
            {
                Task.Run(() =>
              {
                  DateTime beginTime = DateTime.Now;


                  //while (SendCar!=null&&SendCar.CarState == 0)
                  while (true)
                  {
                      CarInfo SendCar = CoreData.CarList.FirstOrDefault(p => p.AgvID == car.AgvID);
                      if (SendCar == null) { return; }
                      if (SendCar.CarState == 0)
                      {
                          LandmarkInfo BeginLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == SendCar.CurrSite.ToString());
                          LandmarkInfo EndLand = car.Route.Last();
                          if (BeginLand == null || EndLand == null)
                          { return; }
                          SendCar.Route = DataToObject.CreateDeepCopy<List<LandmarkInfo>>(CountRoute.GetRoute(BeginLand, EndLand));
                          if (SendCar.Route.Count <= 1) { return; }
                          SendCar.AllRouteLandCode = "," + string.Join(",", SendCar.Route.Select(p => p.LandmarkCode)) + ",";
                          try
                          {
                              AGVServerDAccess.SaveCarCurrentRoute(SendCar);
                          }
                          catch (Exception ex)
                          { LogHelper.WriteErrorLog(ex); }
                          string Route = string.Join(",", SendCar.Route.Select(p => p.LandmarkCode));
                          LogHelper.WriteCreatTaskLog("车号:" + SendCar.AgvID.ToString() + "下发路线为:" + Route);


                          CommandToValue CommandTo = getRouteCommd(SendCar, RouteType, TaskDetail);
                          if (CommandTo == null)
                          {
                              LogHelper.WriteCreatTaskLog("车号:" + SendCar.AgvID.ToString() + "转换路线指令失败!");
                              return;
                          }
                          car.RealyRoute = DataToObject.CreateDeepCopy<List<LandmarkInfo>>(car.Route);
                          car.RealyAllRouteLandCode = "," + string.Join(",", car.RealyRoute.Select(p => p.LandmarkCode)) + ",";
                          AGV_AddCommand(SendCar.AgvID, CommandTo);
                          List<LockResource> CurrCarLockstemp = Traffic.lockResource.Where(p => p.BeLockCarID == car.AgvID).ToList();
                          if (CurrCarLockstemp.Count() > 0)
                          {
                              AGV_AddCommand(car.AgvID, new CommandToValue(AGVCommandEnum.Stop));
                          }
                          return;
                      }
                      else
                      {
                          DateTime endTime = DateTime.Now;
                          if ((endTime - beginTime).TotalSeconds > 10)
                          {
                              LogHelper.WriteCreatTaskLog("判断车号:" + SendCar.AgvID.ToString() + "检查是否切换待命超时!");
                              return;
                          }
                          LogHelper.WriteCreatTaskLog("判断车号:" + SendCar.AgvID.ToString() + "检查是否切换待命!");
                          Thread.Sleep(1000);
                      }
                  }
              });
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }



        /// <summary>
        /// 界面调用发送车辆执行路线
        /// </summary>
        public void SendCarRoute(CarInfo car)
        {
            try
            {
                SendCarRoute(car, SendRouteType.HandTask, null);
                car.ExcuteTaksNo = "手工任务";
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        ///  判断是否需要切换自动充电路线
        /// </summary>
        public void CheckSetToChargeRoute()
        {
            try
            {
                if (CoreData.SysParameter.Keys.Contains("ChargeVoltage"))
                {
                    //充电电量阈值
                    string ChargeVoltageStr = CoreData.SysParameter["ChargeVoltage"].ToString();
                    //充电桩安装位置[在待命点上|不在待命点上]
                    string ChargeInstallTypeStr = CoreData.SysParameter["ChargeInstallType"].ToString();


                    #region 判断时间段内是否充电
                    //开始充电时间
                    string BeginChargeTime = CoreData.SysParameter["BeginChargeTime"].ToString();
                    //结束充电时间
                    string EndChargeTime = CoreData.SysParameter["EndChargeTime"].ToString();
                    if (!string.IsNullOrEmpty(BeginChargeTime) && !string.IsNullOrEmpty(EndChargeTime))
                    {
                        try
                        {
                            TimeSpan workStartDT = DateTime.Parse(BeginChargeTime).TimeOfDay;
                            TimeSpan workEndDT = DateTime.Parse(EndChargeTime).TimeOfDay;
                            TimeSpan nowDt = DateTime.Now.TimeOfDay;
                            if (nowDt > workStartDT && nowDt < workEndDT)
                            {

                                //在轮班期间强制去充电
                                IList<CarInfo> NoLowCar = CoreData.CarList.Where(p => !p.LowPower).ToList();
                                foreach (CarInfo car in NoLowCar)
                                {
                                    car.LowPower = true;
                                    //if (car.CarState == 4 && !string.IsNullOrEmpty(car.NowChargeLandCode))
                                    //{ car.NowChargeLandCode = ""; }
                                }
                                List<CarInfo> LowPowerCars = CoreData.CarList.Where(p => p.LowPower == true && string.IsNullOrEmpty(p.NowChargeLandCode)).ToList();
                                foreach (CarInfo car in LowPowerCars)
                                {
                                    if (!string.IsNullOrEmpty(ChargeInstallTypeStr) && ChargeInstallTypeStr == "在待命点")
                                    {
                                        #region 充电桩在待命点上
                                        if (string.IsNullOrEmpty(car.ChargeCode))
                                        {
                                            LogHelper.WriteAGVChargeLog("时间段内车号为:" + car.AgvID.ToString() + "低电量，计算回充电点充电,但是没对照充电点");
                                            continue;
                                        }
                                        ChargeStationInfo ChargeStation = CoreData.ChargeList.FirstOrDefault(p => !string.IsNullOrEmpty(p.ChargeLandCode) && p.ChargeLandCode == car.ChargeCode);
                                        if (ChargeStation == null)
                                        {
                                            LogHelper.WriteAGVChargeLog("时间段内需要为车号:" + car.AgvID.ToString() + "充电,但是充电点地标没有对照充电桩");
                                            continue;
                                        }
                                        //如果车就在待命点的充电桩位置那么就直接充电
                                        if (ChargeStation.ChargeLandCode == car.CurrSite.ToString())
                                        {
                                            car.NowChargeLandCode = ChargeStation.ChargeLandCode;
                                        }
                                        else
                                        {
                                            if (car.CarState != 4 && Traffic.CheckCarIsInLockSource(car))
                                            {
                                                LogHelper.WriteCreatTaskLog("车号:" + car.AgvID.ToString() + "还被交管中或交管了别车,所以先不发送充电路线");
                                                continue;
                                            }
                                            LandmarkInfo StandLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == ChargeStation.ChargeLandCode);
                                            LandmarkInfo CurrLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == car.CurrSite.ToString());
                                            if (StandLand != null && CurrLand != null)
                                            {
                                                car.Route = DataToObject.CreateDeepCopy<List<LandmarkInfo>>(CountRoute.GetRoute(CurrLand, StandLand));
                                                car.AllRouteLandCode = "," + string.Join(",", car.Route.Select(p => p.LandmarkCode)) + ",";
                                                try
                                                {
                                                    AGVServerDAccess.SaveCarCurrentRoute(car);
                                                }
                                                catch (Exception ex)
                                                { LogHelper.WriteCreatTaskLog("保存AGV当前线路异常:" + ex.Message); }
                                                if (car.CurrSite.ToString() != ChargeStation.ChargeLandCode && car.Route.Count <= 0)
                                                {
                                                    LogHelper.WriteAGVChargeLog("时间段内需要为车号:" + car.AgvID.ToString() + "充电,但是从车当前位置" + car.CurrSite.ToString() + "到充电桩位置" + ChargeStation.ChargeLandCode + "计算路线不通");
                                                    continue;
                                                }
                                                if (SendCarRoute(car, SendRouteType.GoToCharge, null))
                                                {
                                                    //ChargeStation.ChargeLock = 1;
                                                    car.NowChargeLandCode = ChargeStation.ChargeLandCode;
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                }
                                return;
                            }
                            //else
                            //{
                            //    List<CarInfo> LowVoltCars = CoreData.CarList.Where(p => p.LowPower).ToList();
                            //    foreach (CarInfo car in LowVoltCars)
                            //    {
                            //        car.LowPower = false;
                            //        car.NowChargeLandCode = "";
                            //    }
                            //}
                        }
                        catch (Exception ex)
                        { LogHelper.WriteAGVChargeLog("解决时间段内充电逻辑异常:" + ex.Message + ex.StackTrace); }
                    }
                    #endregion

                    double Voltage = 0;
                    try
                    {
                        Voltage = Convert.ToDouble(ChargeVoltageStr);
                    }
                    catch
                    { }

                    if (Voltage > 0)
                    {
                        ////检查充电周期
                        //double VoltageTime = 0;
                        //if (CoreData.SysParameter.Keys.Contains("CheckVoltageTime"))
                        //{
                        //    string CheckVoltageTimeStr = CoreData.SysParameter["CheckVoltageTime"].ToString();
                        //    try
                        //    {
                        //        VoltageTime = Convert.ToInt16(CheckVoltageTimeStr);
                        //    }
                        //    catch
                        //    { }
                        //}

                        //欧米麦克老吕说只能让一个低电量的车去充电
                        if (CoreData.CarList.Count(p => p.LowPower) >= 1)
                        {
                            CarInfo LowCar = CoreData.CarList.FirstOrDefault(p => p.LowPower);
                            LogHelper.WriteAGVChargeLog("老吕说只能让一个低电量的车去充电!当前有"+ LowCar.AgvID.ToString()+"号车低电量。");
                            return;
                        }

                        List<CarInfo> LowVoltCars = CoreData.CarList.Where(p => !p.bIsCommBreak && p.fVolt > 0 && !p.LowPower && p.fVolt <= Voltage && (p.CarState != 5 || (p.CarState == 4 && !string.IsNullOrEmpty(p.NowChargeLandCode))) && p.ExcuteTaksNo == "").ToList();


                        foreach (CarInfo car in LowVoltCars)
                        {
                            //if (car.PreChargeTime.ToString("yyyy") != "0001")
                            //{
                            //    TimeSpan span = DateTime.Now - car.PreChargeTime;
                            //    if (span.TotalMinutes < VoltageTime)
                            //    {
                            //        LogHelper.WriteAGVChargeLog("车号为:" + car.AgvID.ToString() + "还没到达充电周期");
                            //        continue;
                            //    }
                            //}
                            car.LowPower = true;
                            //欧米麦克老吕说只能让一个低电量的车去充电
                            break;
                            //if (!string.IsNullOrEmpty(car.NowChargeLandCode))
                            //{ car.NowChargeLandCode = ""; }
                        }
                        List<CarInfo> LowPowerCars = CoreData.CarList.Where(p => p.LowPower == true && string.IsNullOrEmpty(p.NowChargeLandCode)).ToList();
                        foreach (CarInfo car in LowPowerCars)
                        {
                            if (!string.IsNullOrEmpty(ChargeInstallTypeStr) && ChargeInstallTypeStr == "在待命点")
                            {
                                #region 充电桩在待命点上
                                if (string.IsNullOrEmpty(car.ChargeCode))
                                {
                                    LogHelper.WriteAGVChargeLog("车号为:" + car.AgvID.ToString() + "低电量，计算回充电点充电,但是没对照充电点");
                                    continue;
                                }
                                ChargeStationInfo ChargeStation = CoreData.ChargeList.FirstOrDefault(p => !string.IsNullOrEmpty(p.ChargeLandCode) && p.ChargeLandCode == car.ChargeCode);
                                if (ChargeStation == null)
                                {
                                    LogHelper.WriteAGVChargeLog("需要为车号:" + car.AgvID.ToString() + "充电,但是充电点地标没有对照充电桩");
                                    continue;
                                }
                                //如果车就在待命点的充电桩位置那么就直接充电
                                if (ChargeStation.ChargeLandCode == car.CurrSite.ToString())
                                {
                                    LandmarkInfo CurrLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == car.CurrSite.ToString());
                                    if (CurrLand == null)
                                    {
                                        LogHelper.WriteAGVChargeLog("需要为车号:" + car.AgvID.ToString() + "充电,但是车当前站点没有地标档案");
                                        continue;
                                    }
                                    //发送当前站点的一个地标的路径，主要用于打开AGV充电口
                                    List<CommandLandMark> ComodLandList = new List<CommandLandMark>();
                                    CommandLandMark ComodLand = new CommandLandMark();
                                    ComodLand.LandmarkCode = car.CurrSite.ToString();
                                    ComodLand.X = CurrLand.LandX * CoreData.ScalingRate; //car.X;
                                    ComodLand.Y = CurrLand.LandY * CoreData.ScalingRate; //car.Y;
                                    ComodLand.Angle = (int)car.Angel;
                                    ComodLand.Avoidance = 1;
                                    ComodLand.ActionType = 1;
                                    ComodLand.Move_Direact = (int)MoveDirectEnum.None;
                                    ComodLandList.Add(ComodLand);
                                    CommandToValue CommandTo = new CommandToValue(AGVCommandEnum.ChangeRoute);
                                    CommandTo.CommandValue = ComodLandList;
                                    AGV_AddCommand(car.AgvID, CommandTo);
                                    LogHelper.WriteAGVChargeLog("需要为车号:" + car.AgvID.ToString() + "充电,车就在充电桩位置,并发送了一个站点来打开充电回路");
                                    car.NowChargeLandCode = ChargeStation.ChargeLandCode;
                                }
                                else
                                {
                                    if (car.CarState != 4 && Traffic.CheckCarIsInLockSource(car))
                                    {
                                        LogHelper.WriteCreatTaskLog("车号:" + car.AgvID.ToString() + "还被交管中或交管了别车,所以先不发送充电路线");
                                        continue;
                                    }
                                    LandmarkInfo StandLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == ChargeStation.ChargeLandCode);
                                    LandmarkInfo CurrLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == car.CurrSite.ToString());
                                    if (StandLand != null && CurrLand != null)
                                    {
                                        car.Route = DataToObject.CreateDeepCopy<List<LandmarkInfo>>(CountRoute.GetRoute(CurrLand, StandLand));
                                        car.AllRouteLandCode = "," + string.Join(",", car.Route.Select(p => p.LandmarkCode)) + ",";
                                        try
                                        {
                                            AGVServerDAccess.SaveCarCurrentRoute(car);
                                        }
                                        catch (Exception ex)
                                        { LogHelper.WriteCreatTaskLog("保存AGV当前线路异常:" + ex.Message); }
                                        if (car.CurrSite.ToString() != ChargeStation.ChargeLandCode && car.Route.Count <= 0)
                                        {
                                            LogHelper.WriteAGVChargeLog("需要为车号:" + car.AgvID.ToString() + "充电,但是从车当前位置" + car.CurrSite.ToString() + "到充电桩位置" + ChargeStation.ChargeLandCode + "计算路线不通");
                                            continue;
                                        }
                                        if (SendCarRoute(car, SendRouteType.GoToCharge, null))
                                        {
                                            //ChargeStation.ChargeLock = 1;
                                            car.NowChargeLandCode = ChargeStation.ChargeLandCode;
                                        }
                                        else
                                        {
                                            car.LowPower = false;
                                            car.NowChargeLandCode = "";
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region 充电桩和待命点分开的充电逻辑
                                //先判断当前车是否就在充电桩
                                ChargeStationInfo FreeChargeStation = null;
                                FreeChargeStation = CoreData.ChargeList.FirstOrDefault(p => p.ChargeLandCode == car.CurrSite.ToString() && p.ChargeLock == 0);
                                if (FreeChargeStation == null)
                                {
                                    FreeChargeStation = (from a in CoreData.ChargeList
                                                         where a.ChargeLock == 0 || a.ChargeLandCode == car.NowChargeLandCode/* && a.ChargeState != 0 && a.ChargeState != 1 && CoreData.CarList.Where(p => p.CurrSite.ToString() == a.ChargeLandCode && p.AgvID != car.AgvID && p.CarState != 1).Count() <= 0*/
                                                         select a).FirstOrDefault();
                                    LandmarkInfo StandLand = null;
                                    LandmarkInfo CurrLand = null;
                                    int ChargeLock = 0;
                                    if (FreeChargeStation != null)
                                    {
                                        StandLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == FreeChargeStation.ChargeLandCode);
                                        CurrLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == car.CurrSite.ToString());
                                        ChargeLock = 1;
                                        LogHelper.WriteAGVChargeLog("找到充电桩:" + FreeChargeStation.ID.ToString());
                                    }
                                    else
                                    {
                                        StandLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == car.StandbyLandMark);
                                        CurrLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == car.CurrSite.ToString());
                                        LogHelper.WriteAGVChargeLog(car.AgvID.ToString() + "车没找到充电桩,回待命点");
                                    }
                                    if (StandLand != null && CurrLand != null)
                                    {
                                        if (car.CarState != 4 && Traffic.CheckCarIsInLockSource(car))
                                        {
                                            LogHelper.WriteCreatTaskLog("车号:" + car.AgvID.ToString() + "还被交管中或交管了别车,所以先不发送充电路线");
                                            continue;
                                        }
                                        car.Route = DataToObject.CreateDeepCopy<List<LandmarkInfo>>(CountRoute.GetRoute(CurrLand, StandLand));
                                        car.AllRouteLandCode = "," + string.Join(",", car.Route.Select(p => p.LandmarkCode)) + ",";
                                        try
                                        {
                                            AGVServerDAccess.SaveCarCurrentRoute(car);
                                        }
                                        catch (Exception ex)
                                        { LogHelper.WriteCreatTaskLog("保存AGV当前线路异常:" + ex.Message); }
                                        if (/*car.CurrSite.ToString() != FreeChargeStation.ChargeLandCode && */car.Route.Count <= 0) { continue; }
                                        bool SendResult = SendCarRoute(car, SendRouteType.GoToCharge, null);
                                        LogHelper.WriteAGVChargeLog(car.AgvID.ToString() + "车发送完充电或回待命点路径");
                                        if (FreeChargeStation != null && SendResult)
                                        {
                                            FreeChargeStation.ChargeLock = ChargeLock;
                                            car.NowChargeLandCode = FreeChargeStation.ChargeLandCode;
                                        }
                                    }
                                }
                                else
                                {
                                    if (FreeChargeStation != null)
                                    {
                                        FreeChargeStation.ChargeLock = 1;
                                        car.NowChargeLandCode = FreeChargeStation.ChargeLandCode;
                                        LogHelper.WriteAGVChargeLog("找到充电桩:" + FreeChargeStation.ID.ToString() + "就在待命点");
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }

        /// <summary>
        /// 检查任务是否超时
        /// </summary>
        public bool ChekTaskOverTime(DispatchTaskInfo TaskInfo)
        {
            try
            {
                string TaskOutTimeStr = CoreData.SysParameter["TaskOutTime"].ToString();
                if (string.IsNullOrEmpty(TaskOutTimeStr)) { return false; }
                double TaskOutTime = 0;
                try
                {
                    TaskOutTime = Convert.ToDouble(TaskOutTimeStr);
                }
                catch
                { }
                if (TaskOutTime == 0) { return false; }
                DateTime TaskBuildTime;
                try
                {
                    TaskBuildTime = DateTime.ParseExact(TaskInfo.BuildTime, "yyyyMMddHHmmss", CultureInfo.CurrentCulture);
                }
                catch
                { return false; }
                //如果当前任务已经创建时间到当前时间间隔已经超过系统参数任务超时时间
                //则将对应任务状态修改为任务超时状态
                if ((DateTime.Now - TaskBuildTime).TotalMinutes >= TaskOutTime)
                {
                    AGVServerDAccess.UpdateTaskState(TaskInfo.dispatchNo, 5);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); return false; }
        }

        /// <summary>
        /// 计算小车当前站点到达任务目的站点的路径长度
        /// </summary>
        /// <returns></returns>
        private double getDistant(string CurrLandCode, string TaskDetailLandCode)
        {
            try
            {
                LandmarkInfo LandCarCurrentLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == CurrLandCode);
                LandmarkInfo TaskArmLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == TaskDetailLandCode);
                if (LandCarCurrentLand == null || TaskArmLand == null)
                {
                    LogHelper.WriteLog("分派任务时查找最近车辆时找不到车当前地标或任务的目的地标");
                    return 0;
                }
                string IsImportLenthStr = CoreData.SysParameter["IsImportLenth"].ToString();
                //如果系统参数是否导入路径长度为是时,则需要计算实际的到达线路长度
                List<LandmarkInfo> ToRoute = CountRoute.GetRoute(LandCarCurrentLand, TaskArmLand);
                if (!string.IsNullOrEmpty(IsImportLenthStr) && IsImportLenthStr.Equals("是"))
                {
                    double RouteLenth = 0;
                    for (int i = 1; i < ToRoute.Count; i++)
                    {
                        string LandCodeBegin = ToRoute[i - 1].LandmarkCode;
                        string LandCodeEnd = ToRoute[i].LandmarkCode;
                        AllSegment Seg = CoreData.AllSeg.FirstOrDefault(p => p.BeginLandMakCode == LandCodeBegin && p.EndLandMarkCode == LandCodeEnd);
                        if (Seg != null)
                        { RouteLenth += Seg.Length; }
                    }
                    return RouteLenth;
                }
                else
                { return ToRoute.Count; }
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
            return -1;
        }

        /// <summary>
        /// 判断是否需要启动充电桩
        /// </summary>
        private void CheckIsStartCharge(CarInfo Car)
        {
            try
            {
                if (Car.LowPower && Car.CurrSite.ToString() == Car.NowChargeLandCode && (Car.CarState == 2 || Car.CarState == 0 || Car.CarState == 4))
                {
                    ChargeStationInfo charge = CoreData.ChargeList.FirstOrDefault(p => p.ChargeLandCode == Car.NowChargeLandCode);
                    if (charge != null && charge.ChargeState == 0)
                    {
                        Add_EleCommand(charge.ID, new CommandToValue(AGVCommandEnum.StartCharge));
                        LogHelper.WriteAGVChargeLog("本次车:" + Car.AgvID.ToString() + "低电量自动充电时间为:" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm"));
                    }
                    else if (charge == null)
                    {
                        LogHelper.WriteAGVChargeLog(Car.AgvID.ToString() + "车低电量需要自动充电,但是充电桩没对照车的充电点!");
                    }
                }
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }


        /// <summary>
        /// 广播平板订单信息
        /// </summary>
        public OperateReturnInfo BroadPadBills()
        {
            try
            {
                if (InsideHandler != null)
                {
                    InsideHandler.SynAllBeeperBill();
                }
                return new OperateReturnInfo(OperateCodeEnum.Success);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
                return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message);
            }
        }

        /// <summary>
        /// 更新储位状态和lock状态
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        private async void UnLockStorage(CarInfo car)
        {
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    string IsUserStoreState = CoreData.SysParameter["IsUserStoreState"].ToString();
                    if (string.IsNullOrEmpty(IsUserStoreState)) { IsUserStoreState = "否"; }
                    if (IsUserStoreState == "是")
                    { AGVServerDAccess.UnLockStorage(car); }
                });
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }
        #endregion

        #region 自定义内部类
        public enum SendRouteType : int
        {
            /// <summary>
            /// 执行任务
            /// </summary>
            ExcuteTask = 0,
            /// <summary>
            /// 返回待命点
            /// </summary>
            BackStandLand = 1,
            /// <summary>
            /// 去充电桩充电
            /// </summary>
            GoToCharge = 2,
            /// <summary>
            /// 手工任务
            /// </summary>
            HandTask = 3,
        }

        #endregion

        #region 验证序列号
        private bool CheckSoft()
        {
            try
            {
                //ReturnInfo op = Verify.VerifyCarAmount(CoreData.CarList.Count);
                //if (!op.Result)
                //{
                //    if (!string.IsNullOrEmpty(op.Msg))
                //    { DelegateState.InvokeDispatchStateEvent(op.Msg); }
                //    else
                //    { DelegateState.InvokeDispatchStateEvent("序列号过期!"); }
                //    return false;
                //}
                return true;
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); return false; }
            //return true;
        }
        #endregion

        #region 调用webService
        /// <summary>
        /// 调用webservice接口
        /// </summary>
        /// <param name="URL">地址</param>
        /// <param name="Method">调用方法</param>
        /// <param name="para">方法参数</param>
        /// <returns></returns>
        public async Task<string> CallWebService(string Method, string Params)
        {
            try
            {
                await Task.Run(() =>
                {
                    //string url = "http://192.168.4.99:7000/HandleDispath_MingShuo.asmx";
                    ////string Method = "Task_CallBack";
                    //WebServiceAgent ServiceAgent = new WebServiceAgent(url);
                    //object[] para = new object[1];
                    //para[0] = Params;
                    //object result = ServiceAgent.Invoke(Method, para);
                    //return result.ToString();
                    return "";
                });
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
                return ex.Message;
            }
        }

        /// <summary>
        /// 调用WebApi接口
        /// </summary>
        /// <param name="Method"></param>
        /// <param name="Params"></param>
        /// <returns></returns>
        public async void CallWebAPI(CarInfo car)
        {
            try
            {
                await Task.Run(() =>
                {
                    if (CoreData.SysParameter.Keys.Contains("MesUrl"))
                    {
                        string url = CoreData.SysParameter["MesUrl"].ToString();
                        if (!string.IsNullOrEmpty(url.Trim()))
                        {
                            CarMessage carmes = new CarMessage();
                            carmes.setValue(car);
                            List<CarMessage> listMes = new List<CarMessage>();
                            listMes.Add(carmes);
                            string Parm = JosnTool.GetJson<List<CarMessage>>(listMes);
                            string res = WebServiceAgent.HttpGet(url + "?json=" + Parm);
                            LogHelper.WriteLog("调用MES反馈结果为:" + res);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
            }
        }


        public void FinishTask(CarInfo car)
        {
            try
            {
                //  Task.Run(() =>
                //{
                //判断车子任务请求状态
                //任务完成

                DispatchTaskDetail taskDetail = AGVServerDAccess.GetTaskDetail(car.ExcuteTaksNo, car.TaskDetailID);
                if (car != null && taskDetail != null && car.IsNeedFinshTask == 1 && !string.IsNullOrEmpty(car.ExcuteTaksNo) && car.TaskDetailID != -1)
                {
                    AGVServerDAccess.TaskHandle(car.ExcuteTaksNo, car.TaskDetailID, 2);
                    StorageInfo store = CoreData.StorageList.FirstOrDefault(p => p.LankMarkCode == taskDetail.LandCode);
                    if (store != null)
                    {
                        store.LockState = 0;
                        AGVServerDAccess.UpdateStorageState(-1, 0, store.ID);
                        LogHelper.WriteCreatTaskLog(car.AgvID.ToString() + "号AGV车上手工完成了的任务号为:" + car.ExcuteTaksNo + "对应的明细为:" + car.TaskDetailID.ToString());
                        string filePath = System.AppDomain.CurrentDomain.BaseDirectory + @"\MemoryFile\CarTaskInfo.xml";
                        Hashtable ht = new Hashtable();
                        ht["TaskNo"] = "";
                        XMLClass.AppendXML(filePath, "N" + car.AgvID.ToString(), ht);
                        car.ExcuteTaksNo = "";
                        car.TaskDetailID = -1;
                        if (car.Route != null)
                        {
                            car.Route.Clear();
                            car.RouteLands.Clear();
                            car.TurnLands.Clear();
                        }
                    }
                    //处理一下回写PLC
                    //if (taskDetail.IsNeedCallBack == 1)
                    //{
                    //    AGVServerDAccess.UpdateIsNeedCallBack(car.ExcuteTaksNo, taskDetail.DetailID);
                    //    LogHelper.WriteCreatTaskLog(car.AgvID.ToString() + "号AGV车上手工完成了的任务号为:" + car.ExcuteTaksNo + "对应的明细为:" + car.TaskDetailID.ToString() + "并回写了PLC信号");
                    //}
                }
                //任务重做
                else if (car != null && taskDetail != null && car.IsNeedRedoTask == 1 && !string.IsNullOrEmpty(car.ExcuteTaksNo) && car.TaskDetailID != -1)
                {
                    AGVServerDAccess.TaskHandle(car.ExcuteTaksNo, car.TaskDetailID, 0);
                    StorageInfo store = CoreData.StorageList.FirstOrDefault(p => p.LankMarkCode == taskDetail.LandCode);
                    if (store != null)
                    {
                        //store.LockState = 0;
                        //AGVServerDAccess.UpdateStorageState(-1, 0, store.ID);
                        LogHelper.WriteCreatTaskLog(car.AgvID.ToString() + "号AGV车上手工重做的任务号为:" + car.ExcuteTaksNo + "对应的明细为:" + car.TaskDetailID.ToString());
                        string filePath = System.AppDomain.CurrentDomain.BaseDirectory + @"\MemoryFile\CarTaskInfo.xml";
                        Hashtable ht = new Hashtable();
                        ht["TaskNo"] = "";
                        XMLClass.AppendXML(filePath, "N" + car.AgvID.ToString(), ht);
                        car.ExcuteTaksNo = "";
                        car.TaskDetailID = -1;
                        if (car.Route != null)
                        {
                            car.Route.Clear();
                            car.RouteLands.Clear();
                            car.TurnLands.Clear();
                        }
                    }
                }
                //});
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
            }
        }

        public string InvokWebAPI(CarInfo car)
        {
            try
            {
                CarMessage carmes = new CarMessage();
                carmes.setValue(car);
                List<CarMessage> listMes = new List<CarMessage>();
                listMes.Add(carmes);
                string Parm = JosnTool.GetJson<List<CarMessage>>(listMes);
                return WebServiceAgent.HttpGet("http://172.16.21.100:13000/API/Coordinate/SetAGV" + "?json=" + Parm);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
                return ex.Message;
            }
        }
        #endregion


        #region 出工位判断是否给PLC信号
        public void ChekIsCallBack(CarInfo car)
        {
            try
            {
                Task.Run(() =>
                {
                    if (car != null && car.Route.Count > 0)
                    {
                        //在判断当前车子站点是否在车路线索引位置
                        int CurrSiteIndex = car.Route.FindIndex(q => q.LandmarkCode == car.CurrSite.ToString());
                        LogHelper.WriteCreatTaskLog("判断回写PLC,index=" + CurrSiteIndex.ToString());
                        if (CurrSiteIndex >= 2)
                        {
                            LogHelper.WriteCreatTaskLog("判断回写PLC");
                            AGVServerDAccess.UpdateIsNeedCallBack(car.AgvID);
                        }
                    }
                });

            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
            }
        }

        public void ClearCarLockSource(CarInfo car)
        {
            try
            {
                List<LockResource> CurrCarLockstemp = DataToObject.CreateDeepCopy<List<LockResource>>(Traffic.lockResource.Where(p => p.BeLockCarID == car.AgvID || p.LockCarID == car.AgvID).ToList());
                foreach (LockResource item in CurrCarLockstemp)
                {
                    int index = Traffic.lockResource.FindIndex(p => p.LockCarID == item.LockCarID && p.BeLockCarID == item.BeLockCarID);
                    Traffic.lockResource.RemoveAt(index);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 获取交管信息
        /// </summary>
        /// <returns></returns>
        public string getTrafficInfo()
        {
            try
            {
                if (Traffic == null) { return ""; }
                string TrafficInfo = "";
                if (Traffic.lockResource == null)
                { return TrafficInfo; }
                foreach (LockResource item in Traffic.lockResource)
                {
                    TrafficInfo += "车:" + item.LockCarID + "阻挡住车:" + item.BeLockCarID + "\r\n";
                }
                return TrafficInfo;
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion

    }

}
