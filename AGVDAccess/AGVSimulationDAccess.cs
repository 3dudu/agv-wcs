using Canvas3D.Canvass3DDrawObj;
using Model.CarInfoExtend;
using Model.Comoon;
using Model.MDM;
using Model.MSM;
using SQLServerOperator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Tools;

namespace AGVDAccess
{
    public class AGVSimulationDAccess
    {
        /// <summary>
        /// 加载所有的地标
        /// </summary>
        /// <returns></returns>
        public static IList<LandMarkShap> LoadLandShaps()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                DataTable dt = dbOperator.LoadDatas("getLandMarks", hs);
                IList<LandMarkShap> lands = ConnectConfigTool.TableToEntity<LandMarkShap>(dt);
                return lands;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 加载所有的车辆信息
        /// </summary>
        /// <returns></returns>
        public static IList<CarInfo> LoadCarShap()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                DataTable dt = dbOperator.LoadDatas("LoadAGVAchive", hs);
                IList<CarInfo> Cars = ConnectConfigTool.TableToEntity<CarInfo>(dt);
                return Cars;
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
                IList<CallBoxInfo> CallBoxInfos = DataToObject.TableToEntity<CallBoxInfo>(dt);
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
                return DataToObject.TableToEntity<CallBoxDetail>(dt);
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 判断当前呼叫点是否可以再次呼叫
        /// </summary>
        /// <returns></returns>
        public static int ChekAllowCreatTask(int CallBoxID, string CallLand,int callid)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable dic = new Hashtable();
                dic["CallBoxID"] = CallBoxID;
                dic["CallLand"] = CallLand;
                dic["CallID"] = callid;
                DataTable dt = dbOperator.LoadDatas("ChekAllowCreatTask", dic);
                if (dt != null && dt.Rows.Count > 0)
                { return Convert.ToInt16(dt.Rows[0][0]); }
                else
                { return 0; }
            }
            catch (Exception ex)
            { return 0; }
        }

        /// <summary>
        /// 判断当前呼叫点是否可以再次呼叫
        /// </summary>
        /// <returns></returns>
        public static int ChekAllowCreatTask2(int CallBoxID, string CallLand, int callid)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable dic = new Hashtable();
                dic["CallBoxID"] = CallBoxID;
                dic["CallLand"] = CallLand;
                dic["CallID"] = callid;
                DataTable dt = dbOperator.LoadDatas("ChekAllowCreatTask2", dic);
                if (dt != null && dt.Rows.Count > 0)
                { return Convert.ToInt16(dt.Rows[0][0]); }
                else
                { return 0; }
            }
            catch (Exception ex)
            { return 0; }
        }

        public static int ChekAllowCreatTask3(int callarea)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable dic = new Hashtable();
                dic["callarea"] = callarea; 
                DataTable dt = dbOperator.LoadDatas("ChekAllowCreatTask3", dic);
                if (dt != null && dt.Rows.Count > 0)
                { return Convert.ToInt16(dt.Rows[0][0]); }
                else
                { return 0; }
            }
            catch (Exception ex)
            { return 0; }
        }

        /// <summary>
        /// 加载系统参数
        /// </summary>
        /// <returns></returns>
        public static IDictionary<string, string> LoadSystem()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dtSysparameter = dbOperator.LoadDatas("LoadSystem");
                IDictionary<string, string> Result = new Dictionary<string, string>();
                foreach (DataRow row in dtSysparameter.Rows)
                {
                    Result[row["ParameterCode"].ToString()] = row["ParameterValue"].ToString();
                }
                return Result;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 保存呼叫任务信息
        /// </summary>
        /// <param name="taskInfo"></param>
        /// <returns></returns>
        public static OperateReturnInfo SaveTask(DispatchTaskInfo taskInfo)
        {
            try
            {
                string TaskSQL = "";
                TaskSQL = "insert into tbDispatchTaskInfo( \r\n" +
                                                   "dispatchNo,stationNo,taskType,BuildTime,CallLand) values ( \r\n" +
                                                   "'" + taskInfo.dispatchNo + "'," + taskInfo.stationNo.ToString() + ",0,'" + DateTime.Now.ToString("yyyyMMddHHmmss") + "','" + taskInfo.CallLand + "') \r\n";
                foreach (DispatchTaskDetail item in taskInfo.TaskDetail)
                {
                    TaskSQL += "insert into tbDispatchTaskDetail ( \r\n" +
                                                       "dispatchNo,DetailID,LandCode,OperType,PutType,IsAllowExcute) values ( \r\n" +
                                                       "'" + taskInfo.dispatchNo + "'," + item.DetailID.ToString() + ",'" + item.LandCode + "'," + item.OperType + "," + item.PutType + "," +
                                                       item.IsAllowExcute.ToString()/*(TempLand == ArmLand4 ? "0" : "1")*/ + ") \r\n";
                }
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable hs = new Hashtable();
                    hs["sql"] = TaskSQL;
                    dbOperator.SetDatas("SaveTask", hs);
                    scope.Complete();
                }
                return new OperateReturnInfo(OperateCodeEnum.Success);
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }


        /// <summary>
        /// 更新任务明细被领用后信息
        /// </summary>
        public static void TaskHandle(string dispatchNo, int AGVID, int TaskState, string LandCode, int TaskDetialID)
        {
            try
            {
                //更新任务状态
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
                        dbOperator.SetDatas("UpdateTaskInfo", dic);
                    }
                    else if (TaskState == 1)
                    {
                        dic["ExeTime"] = DateTime.Now.ToString("yyyyMMddHHmmss");
                        dbOperator.SetDatas("UpdateTaskDetail", dic);
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
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
                IList<DispatchTaskInfo> Tasks = DataToObject.TableToEntity<DispatchTaskInfo>(dtTaskInfo);
                foreach (DispatchTaskInfo item in Tasks)
                {
                    dic["dispatchNo"] = item.dispatchNo;
                    DataTable dtTaskDetail = dbOperator.LoadDatas("LoadTaskDetail", dic);
                    IList<DispatchTaskDetail> TaskDetail = DataToObject.TableToEntity<DispatchTaskDetail>(dtTaskDetail);
                    item.TaskDetail = TaskDetail;
                }
                return Tasks;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 放行逻辑
        /// </summary>
        public static OperateReturnInfo UpdateStore(int StoreID, int StorageState)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable hs = new Hashtable();
                    hs["ID"] = StoreID;
                    hs["StorageState"] = StorageState;
                    dbOperator.SetDatas("UpdateStore", hs);
                    scope.Complete();
                }
                return new OperateReturnInfo(OperateCodeEnum.Success);
            }
            catch (Exception ex)
            {
                return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message);
            }
        }

        /// <summary>
        /// 放行逻辑
        /// </summary>
        public static OperateReturnInfo ReleaseCar(string ExcuteTaksNo, string LandCode)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable hs = new Hashtable();
                    hs["TaskNo"] = ExcuteTaksNo;
                    hs["LandCode"] = LandCode;
                    dbOperator.SetDatas("ReleaseCar", hs);
                    scope.Complete();
                }
                return new OperateReturnInfo(OperateCodeEnum.Success);
            }
            catch (Exception ex)
            {
                return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message);
            }
        }

        public static void HandTaskDetail(int ExeAgvID, string LandCode, string DispatchNo, int TaskDetailID)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable hs = new Hashtable();
                    hs["ExeAgvID"] = ExeAgvID;
                    hs["LandCode"] = LandCode;
                    hs["dispatchNo"] = DispatchNo;
                    hs["TaskDetailID"] = TaskDetailID;
                    dbOperator.SetDatas("UpdateTaskDetailForFinish", hs);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }
        public static void HandTaskzb(string DispatchNo)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable hs = new Hashtable();
                    hs["dispatchNo"] = DispatchNo;
                    dbOperator.SetDatas("UpdateTaskForFinish", hs);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); }
        }

        public static DispatchTaskInfo CheckTaskIsFinish(string dispathNo)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["dispatchNo"] = dispathNo;
                DataTable dt = dbOperator.LoadDatas("CheckTaskIsFinish", hs);
                IList<DispatchTaskInfo> DispatchTaskInfos = DataToObject.TableToEntity<DispatchTaskInfo>(dt);
                if (DispatchTaskInfos == null || (DispatchTaskInfos != null && DispatchTaskInfos.Count <= 0))
                { return null; }
                else
                { return DispatchTaskInfos.FirstOrDefault(); }
            }
            catch (Exception ex)
            { LogHelper.WriteErrorLog(ex); return null; }
        }
    }
}
