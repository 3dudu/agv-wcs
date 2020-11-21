using AGVCore;
using Model.CarInfoExtend;
using Model.Comoon;
using Model.MDM;
using Model.MSM;
using SQLServerOperator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Tools;

namespace AGVDAccess
{
    public class AGVServerDAccess
    {
        /// <summary>
        /// 加载agv的当前路线
        /// </summary>
        public static List<LandmarkInfo> LoadCarRoute(CarInfo Car)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                List<LandmarkInfo> route = new List<LandmarkInfo>();
                Hashtable dic = new Hashtable();
                dic["AGVID"] = Car.AgvID;
                DataTable dt = dbOperator.LoadDatas("QueryAGVRoute", dic);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string RouteLandCodes = dt.Rows[0]["RouteLandCodes"].ToString();
                    if (string.IsNullOrEmpty(RouteLandCodes))
                    { return route; }
                    else
                    {
                        string[] LandCodes = RouteLandCodes.Split(',');
                        foreach (string item in LandCodes)
                        {
                            LandmarkInfo Land = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == item);
                            if (Land == null) { continue; }
                            else
                            { route.Add(ConnectConfigTool.CreateDeepCopy<LandmarkInfo>(Land)); }
                        }
                    }
                    return route;
                }
                else
                { return route; }
            }
            catch (Exception ex)
            { throw ex; }
        }


        /// <summary>
        /// 保存当前车辆路线
        /// </summary>
        /// <param name="car"></param>
        public static void SaveCarCurrentRoute(CarInfo car)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable dic = new Hashtable();
                    dic["AGVID"] = car.AgvID;
                    if (car.Route.Count > 0)
                    { dic["RouteLandCodes"] = string.Join(",", car.Route.Select(p => p.LandmarkCode)); }
                    else
                    { dic["RouteLandCodes"] = ""; }
                    dbOperator.SetDatas("SaveAGVRoute", dic);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 更新锁定储位状态
        /// </summary>
        public static void UnLockStorage(CarInfo car)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable dic = new Hashtable();
                    StorageInfo ToStorage = null;
                    dic["lockcar"] = car.AgvID;
                    dic["landmarkcode"] = car.ArmLand.ToString();
                    if (!string.IsNullOrEmpty(car.ArmLand) && car.CurrSite.ToString() == car.ArmLand)
                    { ToStorage = CoreData.StorageList.Where(p => p.LankMarkCode == car.CurrSite.ToString()).FirstOrDefault(); }
                    if (ToStorage != null)
                    {
                        if (car.OperType == 0)
                        {
                            ToStorage.StorageState = 0;
                            ToStorage.LockCar = 0;
                            ToStorage.LockState = 0;
                        }
                        else
                        {

                            ToStorage.StorageState = 2;
                            ToStorage.LockCar = 0;
                            ToStorage.LockState = 0;
                            
                        }
                        dic["StorageState"] = ToStorage.StorageState;
                        dic["AGVArriveTime"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                        dbOperator.SetDatas("UnLockStock", dic);
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 到达新步点后更新任务明细为完成
        /// </summary>
        public static void HandTaskDetail(int AGVID,string TaskNo, int TaskDetailID, string CurrLandCode)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable hs = new Hashtable();
                    hs["LandCode"] = CurrLandCode;
                    hs["dispatchNo"] = TaskNo;
                    hs["TaskDetailID"] = TaskDetailID;
                    hs["AGVID"] = AGVID;
                    hs["FinishTime"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                    dbOperator.SetDatas("HandTaskDetail", hs);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 界面手动更新任务明细
        /// </summary>
        public static void TaskHandle(string dispatchNo, int TaskDetailID, int TaskState)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable dic = new Hashtable();
                    dic["State"] = TaskState;
                    dic["dispatchNo"] = dispatchNo;
                    dic["TaskDetailID"] = TaskDetailID;
                    if (TaskState == 0)
                    { dic["FinishTime"] = ""; }
                    else
                    { dic["FinishTime"] = DateTime.Now.ToString("yyyyMMddHHmmss"); }
                    dbOperator.SetDatas("UpdateTaskState", dic);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 任务明细被领用后更新任务信息
        /// </summary>
        public static void TaskHandle(string dispatchNo, int AGVID, int TaskState, string LandCode, int TaskDetialID)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable dic = new Hashtable();
                    dic["dispatchNo"] = dispatchNo;
                    dic["ExeAgvID"] = AGVID;
                    dic["TaskState"] = TaskState;
                    dic["LandCode"] = LandCode;
                    dic["TaskDetialID"] = TaskDetialID;
                    if (TaskState == 2)
                    {
                        dic["State"] = TaskState;
                        dic["FinishTime"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                        dbOperator.SetDatas("UpdateTaskForFinish", dic);
                    }
                    else if (TaskState == 1)
                    {
                        dic["ExeTime"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                        dbOperator.SetDatas("UpdateTaskForExcute", dic);
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public static void addTask()
        {



        }

        /// <summary>
        /// 根据条件加载任务信息
        /// </summary>
        public static IList<DispatchTaskInfo> LoadDispatchTask(string Condition)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable dic = new Hashtable();
                dic["TaskState"] = Condition;
                dic["CurrDate"] = DateTime.Now.ToString("yyyyMMdd");
                dic["PreDate"] = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
                DataTable dtTaskInfo = dbOperator.LoadDatas("LoadTaskInfo", dic);
                IList<DispatchTaskInfo> Tasks = ConnectConfigTool.TableToEntity<DispatchTaskInfo>(dtTaskInfo);
                foreach (DispatchTaskInfo item in Tasks)
                {
                    dic["dispatchNo"] = item.dispatchNo;
                    DataTable dtTaskDetail = dbOperator.LoadDatas("LoadTaskDetail", dic);
                    IList<DispatchTaskDetail> TaskDetail = ConnectConfigTool.TableToEntity<DispatchTaskDetail>(dtTaskDetail);
                    item.TaskDetail = TaskDetail;
                }
                return Tasks;
            }
            catch (Exception ex)
            { throw ex; }
        }
        
        /// <summary>
        /// 根据条件加载任务明细
        /// </summary>
        /// <returns></returns>
        public static DispatchTaskDetail LoadTaskDetailByCondition(string disPatchNo, int DetailID)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable dic = new Hashtable();
                dic["disPatchNo"] = disPatchNo;
                dic["DetailID"] = DetailID;
                DataTable dtTaskDetailInfo = dbOperator.LoadDatas("LoadTaskDetailByConditon", dic);
                IList<DispatchTaskDetail> TaskDetail = ConnectConfigTool.TableToEntity<DispatchTaskDetail>(dtTaskDetailInfo);
                if (TaskDetail != null && TaskDetail.Count > 0)
                { return TaskDetail[0]; }
                else
                { return null; }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 更新储位信息
        /// </summary>
        public static void UpdateStorageState(int StoreState, int lockstate, int SotreID)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable dic = new Hashtable();
                    dic["ID"] = SotreID;
                    StorageInfo ToStorage = CoreData.StorageList.Where(p => p.ID == SotreID).FirstOrDefault();
                    if (ToStorage != null)
                    {
                        if (StoreState == -1)
                        { dic["storeState"] = ToStorage.StorageState; }
                        else
                        {
                            dic["storeState"] = StoreState;
                            ToStorage.StorageState = StoreState;
                        }
                        if (lockstate == -1)
                        { dic["LockState"] = ToStorage.LockState; }
                        else
                        {
                            ToStorage.LockState = lockstate;
                            dic["LockState"] = lockstate;
                        }
                        dbOperator.SetDatas("UpdateStorageState", dic);
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public static DispatchTaskDetail GetTaskDetail(string No, int id)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable dic = new Hashtable();
                dic["dispatchNo"] = No;
                dic["TaskDetailID"] = id;
                DataTable dtDetail = dbOperator.LoadDatas("GetTaskDetail", dic);
                DispatchTaskDetail detail = null;
                if (dtDetail != null && dtDetail.Rows.Count > 0)
                {
                    detail = ConnectConfigTool.TableToEntity<DispatchTaskDetail>(dtDetail)[0];
                    return detail;
                }
                else
                { return null; }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 更新储位备注
        /// </summary>
        /// <param name="SotreID"></param>
        /// <param name="remark"></param>
        public static void UpdteStorageRemark(int SotreID, string remark)
        {
            IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
            Hashtable dic = new Hashtable();
            dic["ID"] = SotreID;
            dic["remark"] = remark;
            dbOperator.SetDatas("UpdteStorageRemark", dic);
        }

        /// <summary>
        /// 验证用于信息
        /// </summary>
        /// <returns></returns>
        public static UserInfo LoadUserInfo(string UserID, string PassWord)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable dic = new Hashtable();
                dic["UserID"] = UserID;
                dic["PassWord"] = PassWord;
                DataTable dtUser = dbOperator.LoadDatas("LoadUserInfo", dic);
                UserInfo user = null;
                if (dtUser != null && dtUser.Rows.Count > 0)
                {
                    user = ConnectConfigTool.TableToEntity<UserInfo>(dtUser)[0];
                    return user;
                }
                else
                { return null; }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 加载用户可操作按钮
        /// </summary>
        public static IList<SysOprButtonToCategory> LoadUserOprBtn(string UserID)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["UserID"] = UserID;
                DataTable dt = dbOperator.LoadDatas("QueryUserOperButttons", hs);
                return ConnectConfigTool.TableToEntity<SysOprButtonToCategory>(dt);
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 根据呼叫器ID加载呼叫器信息
        /// </summary>
        /// <param name="BoxID"></param>
        /// <returns></returns>
        public static CallBoxInfo LoadAllCallBoxByID(int BoxID)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable dic = new Hashtable();
                dic["CallBoxID"] = BoxID;
                DataTable dt = dbOperator.LoadDatas("LoadCallBoxByID", dic);
                IList<CallBoxInfo> CallBoxInfos = ConnectConfigTool.TableToEntity<CallBoxInfo>(dt);
                if (CallBoxInfos == null || (CallBoxInfos != null && CallBoxInfos.Count <= 0))
                { return null; }
                else
                { return CallBoxInfos.FirstOrDefault(); }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 根据呼叫器ID加载呼叫器配置明细
        /// </summary>
        /// <param name="CallBoxID"></param>
        /// <returns></returns>
        public static IList<CallBoxDetail> LoadCallBoxDetails(int CallBoxID)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable dic = new Hashtable();
                dic["CallBoxID"] = CallBoxID;
                DataTable dt = dbOperator.LoadDatas("LoadCallBoxDetails", dic);
                return ConnectConfigTool.TableToEntity<CallBoxDetail>(dt);
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 根据任务配置明细加载其必经地标档案
        /// </summary>
        /// <returns></returns>
        public static IList<TaskConfigMustPass> LoadTaskMustPass(string TaskConditonCode, int TaskConfigDetailID)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable dic = new Hashtable();
                dic["TaskConditonCode"] = TaskConditonCode;
                dic["TaskConfigDetailID"] = TaskConfigDetailID;
                DataTable dt = dbOperator.LoadDatas("LoadTaskMustPass", dic);
                return ConnectConfigTool.TableToEntity<TaskConfigMustPass>(dt);
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 根据IO动作ID加载对应的IO动作信息
        /// </summary>
        public static IOActionInfo LoadAllIOAction(int IOActionID)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["IOActionID"] = IOActionID;
                DataTable dt = dbOperator.LoadDatas("LoadIOActionByID");
                IList<IOActionInfo> IOActiones = ConnectConfigTool.TableToEntity<IOActionInfo>(dt);
                if (IOActiones != null && IOActiones.Count > 0)
                { return IOActiones.FirstOrDefault(); }
                else
                { return null; }
            }
            catch (Exception e)
            { throw e; }
        }

        /// <summary>
        /// 创建任务
        /// </summary>
        public static void CreatTaskInfo(DispatchTaskInfo TaskInfo, bool IsUserStoreState)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable hs = new Hashtable();
                    hs["dispatchNo"] = TaskInfo.dispatchNo;
                    hs["stationNo"] = TaskInfo.stationNo;
                    hs["BuildTime"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                    hs["TaskState"] = TaskInfo.TaskState;
                    hs["CallLand"] = TaskInfo.CallLand;
                    hs["CallID"] = TaskInfo.CallID;
                    hs["taskType"] = TaskInfo.taskType;
                    hs["GoodsInfo"] = TaskInfo.GoodsInfo;
                    hs["OwerArea"] = TaskInfo.OwerArea;
                    //保存任务主表
                    dbOperator.SetDatas("InsertTaskInfo", hs);
                    //保存任务明细信息
                    foreach (DispatchTaskDetail detail in TaskInfo.TaskDetail)
                    {
                        hs["DetailID"] = detail.DetailID;
                        hs["LandCode"] = detail.LandCode;
                        hs["OperType"] = detail.OperType;
                        hs["IsAllowExcute"] = detail.IsAllowExcute;
                        hs["PassType"] = detail.PassType;
                        hs["State"] = detail.State;
                        hs["PutType"] = detail.PutType;
                        hs["IsSensorStop"] = detail.IsSensorStop;
                        hs["PassTye"] = detail.PassType;
                        hs["IsNeedCallBack"] = detail.IsNeedCallBack;
                        hs["IsCallGoods"] = detail.IsCallGoods;
                        hs["StorageID"] = detail.StorageID;
                        dbOperator.SetDatas("InsertTaskDetail", hs);
                        //处理储位占用
                        if (IsUserStoreState)
                        {
                            StorageInfo CurrentTaskUseStore = CoreData.StorageList.FirstOrDefault(p => p.ID  == detail.StorageID);
                            if (CurrentTaskUseStore != null)
                            {
                                hs["storeState"] = CurrentTaskUseStore.StorageState;
                                hs["LockState"] = 1;
                                hs["ID"] = CurrentTaskUseStore.ID;
                                dbOperator.SetDatas("UpdateStorageState", hs);
                                CurrentTaskUseStore.LockState = 1;
                            }
                        }
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 接受按钮盒放行命令后处理任务明细
        /// </summary>
        public static void ReleaseCarByCallBox(string TaskNo, string LandCode)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["TaskNo"] = TaskNo;
                hs["LandCode"] = LandCode;
                dbOperator.SetDatas("ReleaseCar", hs);
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 更新任务状态
        /// </summary>
        /// <param name="dispathNo"></param>
        /// <param name="UpdateState"></param>
        public static void UpdateTaskState(string dispathNo, int UpdateState)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["dispathNo"] = dispathNo;
                hs["UpdateState"] = UpdateState;
                if (UpdateState == 2)
                { hs["FinishTime"] = DateTime.Now.ToString("yyyyMMddHHmmss"); }
                else
                { hs["FinishTime"] = ""; }
                using (TransactionScope scope = new TransactionScope())
                {
                    dbOperator.SetDatas("ModifyTaskStateByNo", hs);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 匹配线段配置
        /// </summary>
        /// <returns></returns>
        public static IList<RouteFragmentConfigInfo> Load_RouteFragment()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                DataTable dtResult = dbOperator.LoadDatas("Load_RouteFragment");
                IList<RouteFragmentConfigInfo> RouteFragmens = ConnectConfigTool.TableToEntity<RouteFragmentConfigInfo>(dtResult);
                return RouteFragmens;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 加载所有指令档案
        /// </summary>
        /// <returns></returns>
        public static IList<CmdInfo> Load_Cmd()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dtResult = dbOperator.LoadDatas("Load_Cmd");
                IList<CmdInfo> CmdInfos = ConnectConfigTool.TableToEntity<CmdInfo>(dtResult);
                return CmdInfos;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 加载调度程序里配置信息
        /// </summary>
        /// <returns></returns>
        public static IList<DispatchAssembly> Load_DisptchAssembly()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dtResult = dbOperator.LoadDatas("Load_DisptchAssembly");
                IList<DispatchAssembly> DispatchAssems = ConnectConfigTool.TableToEntity<DispatchAssembly>(dtResult);
                return DispatchAssems;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 根据任务明细查询相应必经地标的动作档案信息
        /// </summary>
        /// <returns></returns>
        public static IList<TaskConfigMustPass> LoadIOActiones(string TaskConditonCode, int TaskConfigDetailID)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["TaskConditonCode"] = TaskConditonCode;
                hs["TaskConfigDetailID"] = TaskConfigDetailID;
                DataTable dtResult = dbOperator.LoadDatas("LoadMustPassbyConditonCode");
                IList<TaskConfigMustPass> TaskConfigMustPasses = ConnectConfigTool.TableToEntity<TaskConfigMustPass>(dtResult);
                return TaskConfigMustPasses;
            }
            catch (Exception ex)
            { throw ex; }
        }

        //根据储位状态修改任务状态
        public static void UpdateTask()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                using (TransactionScope scope = new TransactionScope())
                {
                    dbOperator.SetDatas("UpdateTask", hs);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public static void DelUnUseTask()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                using (TransactionScope scope = new TransactionScope())
                {
                    hs["date"] = DateTime.Now.AddDays(-3).ToString("yyyyMMdd");
                    dbOperator.SetDatas("DelUnUseTask", hs);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 根据条件加载任务明细
        /// </summary>
        /// <returns></returns>
        public static DispatchTaskDetail LoadTaskDetailByNo(string disPatchNo, int DetailID)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable dic = new Hashtable();
                dic["dispatchNo"] = disPatchNo;
                dic["DetailID"] = DetailID;
                DataTable dtTaskDetailInfo = dbOperator.LoadDatas("LoadTaskDetailByNo", dic);
                IList<DispatchTaskDetail> TaskDetail = ConnectConfigTool.TableToEntity<DispatchTaskDetail>(dtTaskDetailInfo);
                if (TaskDetail != null && TaskDetail.Count > 0)
                { return TaskDetail[0]; }
                else
                { return null; }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 更新PLC回写状态
        /// </summary>
        /// <param name="No"></param>
        /// <param name="DetailID"></param>
        public static void UpdateIsNeedCallBack(int AGVID)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable dic = new Hashtable();
                    dic["AGVID"] = AGVID;
                    dbOperator.SetDatas("UpdateIsNeedCallBack", dic);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 一键清除所有任务并将车辆复位
        /// </summary>
        /// <param name="AGVID"></param>
        public static void ClearTaskAndCar()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    dbOperator.SetDatas("ClearTaskAndCar");
                    scope.Complete();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 根据IO动作ID加载对应的IO动作信息
        /// </summary>
        public static DispatchTaskInfo LoadTaskByNo(string No)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["dispatchNo"] = No;
                DataTable dt = dbOperator.LoadDatas("LoadTaskByNo", hs);
                IList<DispatchTaskInfo> Tasks = ConnectConfigTool.TableToEntity<DispatchTaskInfo>(dt);
                if (Tasks != null && Tasks.Count > 0)
                { return Tasks.FirstOrDefault(); }
                else
                { return null; }
            }
            catch (Exception e)
            { throw e; }
        }

        public static IList<TraJunctionSegInfo> QueryJunctionSeg(int TraJunctionID)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["TraJunctionID"] = TraJunctionID;
                DataTable dt = dbOperator.LoadDatas("QueryJunctionSeg", hs);
                IList<TraJunctionSegInfo> segs = ConnectConfigTool.TableToEntity<TraJunctionSegInfo>(dt);
                return segs;
            }
            catch (Exception e)
            { throw e; }
        }

        public static void DeleteCarRemeberRoute(int AGVID)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable hs = new Hashtable();
                    hs["AGVID"] = AGVID;
                    dbOperator.SetDatas("DeleteCarRemeberRoute", hs);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 移动任务历史表
        /// </summary>
        public static void MoveTaskHistory()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    dbOperator.SetDatas("MoveTaskHistory");
                    scope.Complete();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

    }
}
