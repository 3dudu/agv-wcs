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
using System.Transactions;

namespace AGVDAccess
{
    public class AGVClientDAccess
    {
        #region 系统参数处理
        /// <summary>
        /// 根据系统参数编码取系统参数
        /// </summary>
        public static SysParameter GetParameterByCode(string ParameterCode)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["ParameterCode"] = ParameterCode;
                DataTable dt = dbOperator.LoadDatas("GetParameterByCode", hs);
                if (dt == null) { return null; }
                if (dt.Rows.Count <= 0) { return null; }
                return ConnectConfigTool.TableToEntity<SysParameter>(dt)[0];
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 保存系统参数
        /// </summary>
        /// <param name="systems"></param>
        /// <returns></returns>
        public static OperateReturnInfo SaveParameter(IList<SysParameter> systems)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    dbOperator.SetDatas("Dele_SysParamater");
                    Hashtable hs = new Hashtable();
                    foreach (SysParameter item in systems)
                    {
                        hs["ParameterCode"] = item.ParameterCode;
                        hs["ParameterValue"] = item.ParameterValue;
                        dbOperator.SetDatas("Save_SysParamater", hs);
                    }
                    scope.Complete();
                }
                return new OperateReturnInfo(OperateCodeEnum.Success);
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }

        /// <summary>
        /// 加载所有的系统参数模板
        /// </summary>
        /// <returns></returns>
        public static IList<ParameterMode> LoadAllParameterMode()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("getSystemModes");
                return ConnectConfigTool.TableToEntity<ParameterMode>(dt);
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion

        #region 程序集配置
        /// <summary>
        /// 保存AGV基础档案
        /// </summary>
        /// <returns></returns>
        public static OperateReturnInfo SaveAssemblySet(IList<DispatchAssembly> assemblys)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable dic = new Hashtable();
                    dbOperator.SetDatas("DeleAssemblys");
                    foreach (DispatchAssembly assembly in assemblys)
                    {
                        dic["AssemblyType"] = assembly.AssemblyType;
                        dic["ClassName"] = assembly.ClassName;
                        dic["Discript"] = assembly.Discript;
                        dbOperator.SetDatas("SaveAssemblys", dic);
                    }
                    scope.Complete();
                    return new OperateReturnInfo(OperateCodeEnum.Success);
                }
            }
            catch (Exception x)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, x.Message); }
        }

        /// <summary>
        /// 加载所有程序集配置
        /// </summary>
        /// <returns></returns>
        public static IList<DispatchAssembly> LoadAssemblys()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("LoadAssemblys");
                return ConnectConfigTool.TableToEntity<DispatchAssembly>(dt);
            }
            catch (Exception e)
            { throw e; }
        }
        #endregion

        #region 处理AGV档案信息
        /// <summary>
        /// 加载所有AGV档案
        /// </summary>
        /// <returns></returns>
        public static IList<CarBaseStateInfo> LoadAGVAchive()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("LoadAGVAchive");
                return ConnectConfigTool.TableToEntity<CarBaseStateInfo>(dt);
            }
            catch (Exception e)
            { throw e; }
        }

        /// <summary>
        /// 加载CarInfo
        /// </summary>
        /// <returns></returns>
        public static IList<CarInfo> LoadCarInfoAchive()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("LoadAGVAchive");
                return ConnectConfigTool.TableToEntity<CarInfo>(dt);
            }
            catch (Exception e)
            { throw e; }
        }



        /// <summary>
        /// 保存AGV基础档案
        /// </summary>
        /// <returns></returns>
        public static OperateReturnInfo SaveAGVAchive(IList<CarBaseStateInfo> cars)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable dic = new Hashtable();
                    dbOperator.SetDatas("DeleAGVs");
                    foreach (CarBaseStateInfo car in cars)
                    {
                        dic["CarCode"] = car.AgvID;
                        dic["CarName"] = car.CarName;
                        dic["CarIP"] = car.CarIP;
                        dic["CarPort"] = car.CarPort;
                        dic["CarState"] = car.IsUse;
                        dic["StandbyLandMark"] = car.StandbyLandMark;
                        dic["OwerArea"] = car.OwerArea;
                        dic["ChargeCode"] = car.ChargeCode;
                        dbOperator.SetDatas("SaveAGVs", dic);
                    }
                    scope.Complete();
                    return new OperateReturnInfo(OperateCodeEnum.Success);
                }
            }
            catch (Exception x)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, x.Message); }
        }
        #endregion

        #region 处理AGV特殊指令
        /// <summary>
        /// 保存动作信息
        /// </summary>
        /// <returns></returns>
        public static OperateReturnInfo Save_agv_Cmd(IList<CmdInfo> acts)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable hs = new Hashtable();
                    dbOperator.SetDatas("dele_Action", hs);
                    foreach (CmdInfo ActionInfo in acts)
                    {
                        hs["ActionCode"] = ActionInfo.CmdCode;
                        hs["ActionName"] = ActionInfo.CmdName;
                        hs["ActionOrder"] = ActionInfo.CmdOrder;
                        dbOperator.SetDatas("Save_Action", hs);
                    }
                    scope.Complete();
                    return new OperateReturnInfo(OperateCodeEnum.Success);
                }
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }

        /// <summary>
        /// 加载所有动作
        /// </summary>
        /// <returns></returns>
        public static IList<CmdInfo> LoadAction()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("load_action");
                IList<CmdInfo> Actions = ConnectConfigTool.TableToEntity<CmdInfo>(dt);
                return Actions;
            }
            catch (Exception ex)
            { throw ex; }
        }


        /// <summary>
        /// 初始化系统命令
        /// </summary>
        /// <returns></returns>
        public static OperateReturnInfo InitSysCmd()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("load_action_System");
                if (dt != null && dt.Rows.Count > 0 && Convert.ToInt16(dt.Rows[0][0]) < 8)
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        dbOperator.SetDatas("InitSysCmd");
                        scope.Complete();
                    }
                }
                return new OperateReturnInfo(OperateCodeEnum.Success);
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, "初始化系统命令失败!\r\n" + ex.Message); }
        }
        #endregion

        #region 处理地图信息
        /// <summary>
        /// 保存地图
        /// </summary>
        public static OperateReturnInfo SaveMap(string FilePath, string FileName, float Zoom, IList<LandmarkInfo> Lands, IList<StorageInfo> stocks, IList<AllSegment> allsegment)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    //保存布局XML文件到数据库
                    Hashtable dic = new Hashtable();
                    dic["fileName"] = FileName;
                    dbOperator.SetDatas("Dele_Planset", dic);
                    int New_ID = 0;
                    DataTable dt = dbOperator.LoadDatas("getNewMapID");//删除布局设置表
                    if (dt != null && dt.Rows.Count > 0 && !string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                    {
                        New_ID = Convert.ToInt16(dt.Rows[0][0]);
                    }
                    dbOperator.SaveFile(New_ID, FilePath, FileName, Zoom);//保存布局设置表
                    dbOperator.SetDatas("Dele_LandMarks");//删除地标档案数据
                    int LandCodeID = 0;//地标号
                    string Value_Sql = "";
                    int Count = 0;
                    foreach (LandmarkInfo item in Lands)
                    {
                        Value_Sql += @"(" + "'" + item.LandmarkCode + "','" + item.LandMarkName + "'," + item.LandY + ","
                            + item.LandX + "),\r\n";
                        LandCodeID++;
                        Count++;
                        if (Count > 0 && Count % 500 == 0 && !string.IsNullOrEmpty(Value_Sql))
                        {
                            Value_Sql = Value_Sql.Remove(Value_Sql.LastIndexOf(','));
                            dic["Value_Sql"] = Value_Sql;
                            dbOperator.SetDatas("SaveLandMarks", dic);//保存地标档案数据
                            Value_Sql = "";
                        }
                    }
                    if (Count > 0 && !string.IsNullOrEmpty(Value_Sql))
                    {
                        Value_Sql = Value_Sql.Remove(Value_Sql.LastIndexOf(','));
                        dic["Value_Sql"] = Value_Sql;
                        dbOperator.SetDatas("SaveLandMarks", dic);//保存地标档案数据
                        Value_Sql = "";
                    }

                    //保存储位档案
                    if (stocks != null && stocks.Count > 0)
                    {
                        Hashtable hs = new Hashtable();
                        string DelStockConditon = string.Join(",", stocks.Select(p => p.ID));
                        hs["DelStockConditon"] = DelStockConditon;
                        dbOperator.SetDatas("Dele_StockRages", hs);//删除储位档案数据
                        string Value_Sql_Stock = "";
                        Count = 0;
                        foreach (StorageInfo item in stocks)
                        {
                            Value_Sql_Stock += @"if exists(select * from tbLocaton where ID=" + item.ID.ToString() + ") \r\n" +
                                                "begin \r\n" +
                                                "update tbLocaton \r\n" +
                                                "set StorageName='" + item.StorageName + "', \r\n" +
                                                "OwnArea=" + item.OwnArea + ", \r\n" +
                                                //"SubOwnArea=" + item.SubOwnArea + ", \r\n" +
                                                //"matterType=" + item.matterType + ", \r\n" +
                                                "MaterielType=" + item.MaterielType + ", \r\n" +
                                                "LankMarkCode='" + item.LankMarkCode + "', \r\n" +
                                                "StorageState='" + item.StorageState + "' \r\n" +
                                                "where ID=" + item.ID.ToString() + "\r\n" +
                                                "end \r\n" +
                                                "else \r\n" +
                                                "begin \r\n" +
                                                "insert into tbLocaton( \r\n" +
                                                "ID, \r\n" +
                                                "StorageName, \r\n" +
                                                "OwnArea, \r\n" +
                                                //"SubOwnArea, \r\n" +
                                                //"matterType, \r\n" +
                                                "MaterielType, \r\n" +
                                                "LankMarkCode,StorageState) values \r\n" +
                                                "(" + item.ID + ",'" + item.StorageName + "'," + item.OwnArea + "," + item.MaterielType + ",'" + item.LankMarkCode + "'," + item.StorageState.ToString() + ") \r\n end \r\n";
                            //                "(" + item.ID + ",'" + item.StorageName + "'," + item.OwnArea + "," + item.SubOwnArea + ","
                            //+ item.matterType + "," + item.MaterielType + ",'" + item.LankMarkCode + "'," + item.StorageState.ToString() + ") \r\n end \r\n";
                            Count++;
                            if (Count > 0 && Count % 500 == 0 && !string.IsNullOrEmpty(Value_Sql_Stock))
                            {
                                //Value_Sql_Stock = Value_Sql_Stock.Remove(Value_Sql_Stock.LastIndexOf(','));
                                hs["Value_Sql"] = Value_Sql_Stock;
                                dbOperator.SetDatas("SaveStorages", hs);
                                Value_Sql_Stock = "";
                            }
                        }
                        if (Count > 0 && !string.IsNullOrEmpty(Value_Sql_Stock))
                        {
                            //Value_Sql_Stock = Value_Sql_Stock.Remove(Value_Sql_Stock.LastIndexOf(','));
                            hs["Value_Sql"] = Value_Sql_Stock;
                            dbOperator.SetDatas("SaveStorages", hs);
                            Value_Sql_Stock = "";
                        }
                    }

                    // 保存全局线段
                    string Value_Sql_Seg = "";
                    Count = 0;
                    //allsegment = allsegment.Distinct();
                    if (allsegment != null && allsegment.Count > 0)
                    {
                        dbOperator.SetDatas("DeleAllSegment");
                        Hashtable hss = new Hashtable();
                        foreach (AllSegment item in allsegment)
                        {
                            Value_Sql_Seg += @"('" + item.BeginLandMakCode + "','" + item.EndLandMarkCode + "'," +
                                item.Length + "," + item.ExcuteAngle + "," + item.ExcuteMoveDirect + "," +
                                item.ExcuteTurnDirect + "," + item.SegmentType + "," + item.Point1X + "," + item.Point1Y + "," + item.Point2X + "," + item.Point2Y + "," + item.Point3X + "," + item.Point3Y + "," +
                                item.Point4X + "," + item.Point4Y + ',' + item.PlanRouteLevel +
                                "," + item.ExcuteAvoidance + "," + item.ExcuteSpeed + "),\r\n";
                            LandCodeID++;
                            Count++;
                            if (Count > 0 && Count % 500 == 0 && !string.IsNullOrEmpty(Value_Sql_Seg))
                            {
                                Value_Sql_Seg = Value_Sql_Seg.Remove(Value_Sql_Seg.LastIndexOf(','));
                                hss["Value_Sql"] = Value_Sql_Seg;
                                dbOperator.SetDatas("SaveAllSegment", hss);
                                Value_Sql_Seg = "";
                            }
                        }
                        if (Count > 0 && !string.IsNullOrEmpty(Value_Sql_Seg))
                        {
                            Value_Sql_Seg = Value_Sql_Seg.Remove(Value_Sql_Seg.LastIndexOf(','));
                            hss["Value_Sql"] = Value_Sql_Seg;
                            dbOperator.SetDatas("SaveAllSegment", hss);
                            Value_Sql_Seg = "";
                        }
                    }



                    scope.Complete();
                    return new OperateReturnInfo(OperateCodeEnum.Success);
                }
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }

        /// <summary>
        /// 加载所有储位
        /// </summary>
        /// <returns></returns>
        public static IList<StorageInfo> LoadStorages()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("LoadStorages");
                return ConnectConfigTool.TableToEntity<StorageInfo>(dt);
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 加载所有线段
        /// </summary>
        public static IList<AllSegment> LoadAllSegment()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("GetAllSegment");
                return ConnectConfigTool.TableToEntity<AllSegment>(dt);
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 根据条件查询地标
        /// </summary>
        /// <param name="conditon"></param>
        /// <returns></returns>
        public static IList<LandmarkInfo> LoadLandByCondition(string conditon)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["Condition"] = conditon;
                DataTable dt = dbOperator.LoadDatas("getLandMarkByCondition", hs);
                IList<LandmarkInfo> lands = ConnectConfigTool.TableToEntity<LandmarkInfo>(dt);
                return lands;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 根据地标号查询地标档案
        /// </summary>
        /// <param name="LandCode"></param>
        /// <returns></returns>
        public static LandmarkInfo LoadLandByCode(string LandCode)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["Condition"] = "LandCode='" + LandCode + "'";
                DataTable dt = dbOperator.LoadDatas("getLandMarkByCondition", hs);
                IList<LandmarkInfo> lands = ConnectConfigTool.TableToEntity<LandmarkInfo>(dt);
                if (lands != null && lands.Count > 0)
                { return lands.FirstOrDefault(); }
                return null;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 获取当前保存的布局地图
        /// </summary>
        /// <returns></returns>
        public static Hashtable GetPlanSet()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                return dbOperator.GetPlanSetXML();
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion

        #region 处理特殊交通管制
        public static IList<TrafficController> GetTraffics()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("LoadTraffic");
                IList<TrafficController> Traffics = ConnectConfigTool.TableToEntity<TrafficController>(dt);
                return Traffics;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public static IList<TraJunction> GetTraJunction()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("LoadTraJunction");
                IList<TraJunction> Traffics = ConnectConfigTool.TableToEntity<TraJunction>(dt);
                return Traffics;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public static OperateReturnInfo SaveTraJunction(IList<TraJunction> traffics)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (TraJunction item in traffics)
                    {
                        Hashtable hs = new Hashtable();
                        hs["TraJunctionID"] = item.TraJunctionID;
                        hs["Carnumber"] = item.Carnumber;
                        hs["JunctionLandMarkCodes"] = item.JunctionLandMarkCodes;
                        dbOperator.SetDatas("SaveTraJunction", hs);
                    }
                    scope.Complete();
                }
                return new OperateReturnInfo(OperateCodeEnum.Success);
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }

        public static OperateReturnInfo SaveTraJunctionByOne(TraJunction traffic)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable hs = new Hashtable();
                    hs["TraJunctionID"] = traffic.TraJunctionID;
                    hs["Carnumber"] = traffic.Carnumber;
                    hs["JunctionLandMarkCodes"] = traffic.JunctionLandMarkCodes;
                    dbOperator.SetDatas("SaveTraJunctionByOne", hs);
                    dbOperator.SetDatas("DeletTraSeg", hs);
                    foreach (TraJunctionSegInfo seg in traffic.Segments)
                    {
                        hs["SegmentID"] = seg.SegmentID;
                        hs["LandCodes"] = seg.LandCodes;
                        dbOperator.SetDatas("SaveTraSegByOne", hs);
                    }
                    scope.Complete();
                }
                return new OperateReturnInfo(OperateCodeEnum.Success);
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }
        #endregion

        #region 处理报表信息
        /// <summary>
        /// 根据明细查询编码查询对应报表
        /// </summary>
        /// <param name="QueryCode"></param>
        /// <returns></returns>
        public static DetailQueryInfo get_QueryByCode(string QueryCode)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["QueryCode"] = QueryCode;
                DataTable dt = dbOperator.LoadDatas("LoadDetailQuery", hs);
                IList<DetailQueryInfo> Details = ConnectConfigTool.TableToEntity<DetailQueryInfo>(dt);
                if (Details == null) { return null; }
                if (Details.Count <= 0) { return null; }
                DetailQueryInfo QueryInfo = Details[0];
                DataTable dtCondition = dbOperator.LoadDatas("LoadConditon", hs);
                IList<DetailCondition> Conditions = ConnectConfigTool.TableToEntity<DetailCondition>(dtCondition);
                QueryInfo.Condition = Conditions;
                DataTable dtFiled = dbOperator.LoadDatas("LoadQueryFiled", hs);
                IList<DetailQueryFiled> Fileds = ConnectConfigTool.TableToEntity<DetailQueryFiled>(dtFiled);
                QueryInfo.Fileds = Fileds;
                return QueryInfo;
            }
            catch (Exception ex)
            { throw ex; }
        }


        /// <summary>
        /// 执行报表
        /// </summary>
        /// <returns></returns>
        public static DataTable ExcuteBI(DetailQueryInfo Query)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                string Excute_SQL = Query.sqlStr;
                while (Excute_SQL.Contains("Empty_Null"))
                {
                    string sql1 = Excute_SQL.Substring(0, Excute_SQL.IndexOf("Empty_Null"));
                    string sql2 = Excute_SQL.Substring(Excute_SQL.IndexOf("}") + 1);
                    string Empty_NullStr = Excute_SQL.Substring(Excute_SQL.IndexOf("Empty_Null"), Excute_SQL.IndexOf("}") - Excute_SQL.IndexOf("Empty_Null") + 1);
                    string Context = Empty_NullStr.Substring(Empty_NullStr.IndexOf("{") + 1, Empty_NullStr.IndexOf("}") - 1 - Empty_NullStr.IndexOf("{"));
                    string[] condition = Context.Split('|');
                    if (condition.Count() < 2 || condition.Count() > 2) { throw new Exception("报表:" + Query.QueryCode + "配置有误!"); }
                    string QueryCode = condition[1].Replace("'", "").Replace("[", "").Replace("]", "");
                    string Value = Query.Condition.Where(P => P.ConditionCode.Equals(QueryCode)).FirstOrDefault().RealyValue;
                    if (!string.IsNullOrEmpty(Value))
                    { Excute_SQL = sql1 + " " + condition[0] + "=" + condition[1] + " " + sql2; }
                    else
                    { Excute_SQL = sql1 + " 1=1 " + sql2; }
                }
                foreach (DetailCondition con in Query.Condition)
                { Excute_SQL = Excute_SQL.Replace("[" + con.ConditionCode + "]", con.RealyValue); }
                Excute_SQL = Excute_SQL.Replace("''", "'");
                Hashtable hs = new Hashtable();
                hs["sql"] = Excute_SQL;
                DataTable Res = dbOperator.LoadDatas("ExcuteDetailQuerySQL", hs);
                return Res;
            }
            catch (Exception ex)
            { throw new Exception("SQL有误,请检查!", ex); }
        }

        /// <summary>
        /// 批量保存报表
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static OperateReturnInfo SaveDetailQuery(IList<DetailQueryInfo> Querys)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable hs = new Hashtable();
                    OperateReturnInfo opr;
                    foreach (DetailQueryInfo item in Querys)
                    {
                        opr = SaveDetailQuery(item);
                        if (opr.ReturnCode == OperateCodeEnum.Failed)
                        { return opr; }
                    }
                    scope.Complete();
                    return new OperateReturnInfo(OperateCodeEnum.Success);
                }

            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }

        //单个保存报表
        public static OperateReturnInfo SaveDetailQuery(DetailQueryInfo item)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable hs = new Hashtable();
                    hs["QueryCode"] = item.QueryCode;
                    hs["QueryName"] = item.QueryName;
                    hs["sqlStr"] = item.sqlStr;
                    dbOperator.SetDatas("DeleBI", hs);
                    dbOperator.SetDatas("SaveDetailQuery", hs);
                    foreach (DetailCondition conditon in item.Condition)
                    {
                        if (conditon.IsSystem) { continue; }
                        hs["ConditionCode"] = conditon.ConditionCode;
                        hs["ConditionName"] = conditon.ConditionName;
                        hs["ConditionValue"] = conditon.ConditionValue;
                        hs["control_type"] = conditon.control_type;
                        hs["X"] = conditon.X;
                        hs["Y"] = conditon.Y;
                        dbOperator.SetDatas("SaveDetailQueryConditon", hs);
                    }
                    foreach (DetailQueryFiled filed in item.Fileds)
                    {
                        hs["FiledCode"] = filed.FiledCode;
                        hs["FiledName"] = filed.FiledName;
                        hs["SummaryType"] = filed.SummaryType;
                        dbOperator.SetDatas("SaveQueryFiled", hs);
                    }
                    scope.Complete();
                    return new OperateReturnInfo(OperateCodeEnum.Success);
                }
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }

        /// <summary>
        /// 加载所有自定义报表
        /// </summary>
        /// <returns></returns>
        public static IList<DetailQueryInfo> get_Querys()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("LoadtbDetailQuery");
                IList<DetailQueryInfo> Details = ConnectConfigTool.TableToEntity<DetailQueryInfo>(dt);
                Hashtable hs = new Hashtable();
                foreach (DetailQueryInfo item in Details)
                {
                    hs["QueryCode"] = item.QueryCode;
                    DataTable dtCondition = dbOperator.LoadDatas("LoadConditon", hs);
                    IList<DetailCondition> Conditions = ConnectConfigTool.TableToEntity<DetailCondition>(dtCondition);
                    item.Condition = Conditions;
                    item.sqlStr = item.sqlStr.Replace("'", "''");
                    DataTable dtFiled = dbOperator.LoadDatas("LoadField", hs);
                    IList<DetailQueryFiled> Fileds = ConnectConfigTool.TableToEntity<DetailQueryFiled>(dtFiled);
                    item.Fileds = Fileds;
                }
                return Details;
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion

        #region 处理坐标体系
        /// <summary>
        /// 保存AGV的坐标系
        /// </summary>
        public static OperateReturnInfo SaveAGVCoordinate(List<string> Parameter)
        {
            try
            {
                Hashtable hs = new Hashtable();
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    dbOperator.SetDatas("DeleteAGVCoordinate");
                    foreach (string item in Parameter)
                    {
                        hs["Direction"] = item.Split(',')[0];
                        hs["Angle"] = item.Split(',')[1];
                        dbOperator.SetDatas("SaveAGVCoordinate", hs);
                    }
                    scope.Complete();
                    return new OperateReturnInfo(OperateCodeEnum.Success);
                }
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }

        /// <summary>
        /// 加载AGV坐标体系设置
        /// </summary>
        public static DataTable LoadAGVCoordinate()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                return dbOperator.LoadDatas("LoadAGVCoordinate");
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion

        #region 处理用户信息
        /// <summary>
        /// 加载所有用户分类
        /// </summary>
        /// <returns></returns>
        public static IList<UserCategory> get_Category()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("Load_Category");
                IList<UserCategory> Categorys = ConnectConfigTool.TableToEntity<UserCategory>(dt);
                if (Categorys.Count == 0)
                {
                    return new List<UserCategory>();
                }
                return Categorys;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// 加载所有操作按钮
        /// </summary>
        /// <returns></returns>
        public static IList<SysOperButton> load_Button()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("Load_Button");
                IList<SysOperButton> button = ConnectConfigTool.TableToEntity<SysOperButton>(dt);
                return button;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IList<SysOprButtonToCategory> load_UserCategoryToButton()
        {
            IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
            DataTable dt = dbOperator.LoadDatas("Load_UserCategoryToButton");
            IList<SysOprButtonToCategory> Sotc = ConnectConfigTool.TableToEntity<SysOprButtonToCategory>(dt);
            return Sotc;
        }

        /// <summary>
        /// 根据用户编码查询用户
        /// </summary>
        /// <param name="CategoryCode"></param>
        /// <returns></returns>
        public static IList<UserInfo> LoadUserByCategoryCode(string CategoryCode)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["CategoryCode"] = CategoryCode;
                DataTable dt = dbOperator.LoadDatas("LoadUserByCategoryCode", hs);
                return ConnectConfigTool.TableToEntity<UserInfo>(dt);
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 根据用户编码查询系统功能按钮
        /// </summary>
        /// <param name="CategoryCode"></param>
        /// <returns></returns>
        public static IList<SysOprButtonToCategory> loadButtonByCategoryCode(string CategoryCode)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["CategoryCode"] = CategoryCode;
                DataTable dt = dbOperator.LoadDatas("Load_ButtonFromCategory", hs);
                return ConnectConfigTool.TableToEntity<SysOprButtonToCategory>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存用户分类信息
        /// </summary>
        /// <param name="CategoryCode"></param>
        /// <returns></returns>
        public static OperateReturnInfo saveCategory(IList<UserCategory> Catrgory)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (UserCategory item in Catrgory)
                    {

                        hs["CategoryCode"] = item.CategoryCode;
                        hs["CategoryName"] = item.CategoryName;
                        dbOperator.SetDatas("Del_Category", hs);
                        dbOperator.SetDatas("Save_Category", hs);
                    }
                    scope.Complete();
                    return new OperateReturnInfo(OperateCodeEnum.Success);
                }

            }
            catch (Exception ex)
            {
                return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message);
            }
        }
        /// <summary>
        /// 保存系统功能按钮设置
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public static OperateReturnInfo saveButton(UserCategory CategoryCode, IList<SysOperButton> button)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (SysOperButton item in button)
                    {
                        hs["ButtonName"] = item.ButtonName;
                        dbOperator.SetDatas("del_ButtonToCategory", hs);
                        hs["ButtonName"] = item.ButtonName;
                        hs["ButtonType"] = item.ButtonType;
                        hs["CategoryCode"] = CategoryCode.CategoryCode;
                        dbOperator.SetDatas("save_Button", hs);

                    }
                    scope.Complete();
                    return new OperateReturnInfo(OperateCodeEnum.Success);
                }
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }
        public static OperateReturnInfo saveButton(IList<SysOprButtonToCategory> button)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                using (TransactionScope scope = new TransactionScope())
                {
                    if (button.Count <= 0)
                    {
                        dbOperator.SetDatas("del_ButtonToCategory");
                    }
                    else
                    {
                        hs["CategoryCode"] = button[0].CategoryCode;
                        dbOperator.SetDatas("del_ButtonToCategory", hs);
                    }
                    foreach (SysOprButtonToCategory item in button)
                    {
                        hs["ButtonName"] = item.ButtonName;
                        hs["ButtonType"] = item.ButtonType;
                        hs["CategoryCode"] = item.CategoryCode;

                        dbOperator.SetDatas("save_Button", hs);

                    }
                    scope.Complete();
                    return new OperateReturnInfo(OperateCodeEnum.Success);
                }
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="Catrgory"></param>
        /// <returns></returns>
        public static OperateReturnInfo saveUSer(UserInfo user, String CategoryCode)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);

                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable hs = new Hashtable();
                    hs["UserID"] = user.UserID;
                    hs["CategoryCode"] = CategoryCode;
                    dbOperator.SetDatas("del_ByUserToCategory", hs);
                    dbOperator.SetDatas("save_UserToCategory", hs);
                    hs["PassWord"] = user.PassWord;
                    hs["UserName"] = user.UserName;
                    dbOperator.SetDatas("Del_User", hs);
                    dbOperator.SetDatas("Save_User", hs);

                    scope.Complete();
                    return new OperateReturnInfo(OperateCodeEnum.Success);
                }
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }

        /// <summary>
        /// 查询最后一条用户分类编码
        /// </summary>
        /// <returns></returns>
        public static IList<UserCategory> select_TheLastFromCategory()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("select_TheLastFromCategory");
                IList<UserCategory> Categorys = ConnectConfigTool.TableToEntity<UserCategory>(dt);
                return Categorys;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询最后一条用户ID
        /// </summary>
        /// <returns></returns>
        public static IList<UserInfo> select_TheLastUserID()
        {
            try
            {
                IDbOperator dbOperatpr = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperatpr.LoadDatas("select_TheLastFromUser");
                IList<UserInfo> users = ConnectConfigTool.TableToEntity<UserInfo>(dt);
                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据用户账号获取用户信息
        /// </summary>
        public static UserInfo GetUserInfo(string UserID, string PassWord)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["UserID"] = UserID;
                hs["PassWord"] = PassWord;
                DataTable dt = dbOperator.LoadDatas("GetUserInfo", hs);
                if (dt == null) { return null; }
                if (dt.Rows.Count <= 0) { return null; }
                return ConnectConfigTool.TableToEntity<UserInfo>(dt)[0];
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 删除用户分类
        /// </summary>
        /// <param name="cate"></param>
        /// <returns></returns>
        public static OperateReturnInfo DelCategory(UserCategory cate)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable hs = new Hashtable();
                    hs["CategoryCode"] = cate.CategoryCode;
                    dbOperator.SetDatas("Del_Category", hs);
                    //dbOperator.SetDatas("Del_UserByCategory", hs);
                    //dbOperator.SetDatas("del_UserToCategory", hs);
                    //dbOperator.SetDatas("del_ButtonOfCategory", hs);

                    scope.Complete();
                    return new OperateReturnInfo(OperateCodeEnum.Success);
                }
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static OperateReturnInfo DelUser(UserInfo user)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable hs = new Hashtable();
                    hs["UserID"] = user.UserID;
                    dbOperator.SetDatas("Del_User", hs);
                    dbOperator.SetDatas("Del_userTocate", hs);
                    scope.Complete();
                    return new OperateReturnInfo(OperateCodeEnum.Success);
                }
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }

        #endregion

        #region 处理区域、物料等简易档案
        /// <summary>
        /// 区域档案
        /// </summary>
        /// <returns></returns>
        public static IList<AreaInfo> LoadAllArea()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("LoadAllArea");
                return ConnectConfigTool.TableToEntity<AreaInfo>(dt);
            }
            catch (Exception e)
            { throw e; }
        }

        /// <summary>
        /// 保存区域档案信息
        /// </summary>
        public static OperateReturnInfo SaveArea(IList<AreaInfo> area)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    dbOperator.SetDatas("deleArea");
                    foreach (AreaInfo item in area)
                    {
                        Hashtable hs = new Hashtable();
                        hs["OwnArea"] = item.OwnArea;
                        hs["AreaName"] = item.AreaName;
                        hs["MaxTaskCount"] = item.MaxTaskCount;
                        dbOperator.SetDatas("SaveArea", hs);
                    }
                    scope.Complete();
                }
                return new OperateReturnInfo(OperateCodeEnum.Success);
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }

        /// <summary>
        /// 加载所有的物料档案
        /// </summary>
        /// <returns></returns>
        public static IList<MaterialInfo> LoadAllMaterial()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("LoadAllMaterial");
                return ConnectConfigTool.TableToEntity<MaterialInfo>(dt);
            }
            catch (Exception e)
            { throw e; }
        }

        /// <summary>
        /// 保存物料档案
        /// </summary>
        public static OperateReturnInfo SaveMaterial(IList<MaterialInfo> area)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    dbOperator.SetDatas("deleMaterial");
                    foreach (MaterialInfo item in area)
                    {
                        Hashtable hs = new Hashtable();
                        hs["MaterialType"] = item.MaterialType;
                        hs["MaterialName"] = item.MaterialName;
                        hs["PickSize"] = item.PickSize;
                        dbOperator.SetDatas("SaveMaterial", hs);
                    }
                    scope.Complete();
                }
                return new OperateReturnInfo(OperateCodeEnum.Success);
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }

        #endregion

        #region 呼叫器档案处理
        /// <summary>
        /// 加载所有的呼叫器档案
        /// </summary>
        public static IList<CallBoxInfo> LoadAllCallBoxs()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("LoadCallBox");
                return ConnectConfigTool.TableToEntity<CallBoxInfo>(dt);
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 加载所有的呼叫器明细
        /// </summary>
        public static IList<CallBoxDetail> LoadCallBoxDetails(CallBoxInfo CallBox)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable dic = new Hashtable();
                dic["CallBoxID"] = CallBox.CallBoxID;
                DataTable dt = dbOperator.LoadDatas("LoadCallBoxDetails", dic);
                return ConnectConfigTool.TableToEntity<CallBoxDetail>(dt);
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 删除一条呼叫器信息
        /// </summary>
        public static OperateReturnInfo DeleCallBox(CallBoxInfo CallBox)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable hs = new Hashtable();
                    hs["CallBoxID"] = CallBox.CallBoxID;
                    dbOperator.SetDatas("DeleCallInfo", hs);
                    scope.Complete();
                }
                return new OperateReturnInfo(OperateCodeEnum.Success);
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }

        /// <summary>
        /// 保存一条呼叫器信息
        /// </summary>
        /// <param name="CallBox"></param>
        /// <returns></returns>
        public static OperateReturnInfo SaveCallBox(CallBoxInfo CallBox)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable hs = new Hashtable();
                    hs["CallBoxID"] = CallBox.CallBoxID;
                    hs["CallBoxIP"] = CallBox.CallBoxIP;
                    hs["CallBoxPort"] = CallBox.CallBoxPort;
                    hs["CallBoxReadAddr"] = CallBox.CallBoxReadAddr;
                    hs["ReadLenth"] = CallBox.ReadLenth;
                    hs["CallType"] = CallBox.CallType;
                    hs["CallBoxName"] = CallBox.CallBoxName;
                    dbOperator.SetDatas("SaveCallInfo", hs);
                    dbOperator.SetDatas("DeleCalldetail", hs);
                    foreach (CallBoxDetail item in CallBox.CallBoxDetails)
                    {
                        hs["CallBoxID"] = CallBox.CallBoxID;
                        hs["ButtonID"] = item.ButtonID;
                        hs["DBAddress"] = item.DBAddress;
                        hs["TaskConditonCode"] = item.TaskConditonCode;
                        hs["OperaType"] = item.OperaType;
                        hs["LocationID"] = item.LocationID;
                        hs["LocationState"] = item.LocationState;
                        dbOperator.SetDatas("SaveCalldetail", hs);
                    }
                    scope.Complete();
                }
                return new OperateReturnInfo(OperateCodeEnum.Success);
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }
        #endregion

        #region 任务生成配置处理
        /// <summary>
        /// 加载所有的任务配置信息
        /// </summary>
        public static IList<TaskConfigInfo> LoadAllTaskConfig()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("LoadTaskConfigInfo");
                return ConnectConfigTool.TableToEntity<TaskConfigInfo>(dt);
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 根据任务配置档案加载任务配置明细
        /// </summary>
        public static IList<TaskConfigDetail> LoadTaskConfigDetails(TaskConfigInfo TaskConfig)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable dic = new Hashtable();
                dic["TaskConditonCode"] = TaskConfig.TaskConditonCode;
                DataTable dt = dbOperator.LoadDatas("LoadTaskConfigDetails", dic);
                IList<TaskConfigDetail> TaskConfigDetails = ConnectConfigTool.TableToEntity<TaskConfigDetail>(dt);
                foreach (TaskConfigDetail detail in TaskConfigDetails)
                {
                    dic["TaskConfigDetailID"] = detail.DetailID;
                    dt = dbOperator.LoadDatas("LoadTaskMustPassLands", dic);
                    IList<TaskConfigMustPass> MustPassLands = ConnectConfigTool.TableToEntity<TaskConfigMustPass>(dt);
                    detail.TaskConfigMustPass = MustPassLands;
                    foreach (TaskConfigMustPass PassLand in MustPassLands)
                    {
                        dic["MustPassLandCode"] = PassLand.MustPassLandCode;
                        dt = dbOperator.LoadDatas("LoadTaskMustPassLandsAction", dic);
                        IList<IOActionInfo> MustPassActions = ConnectConfigTool.TableToEntity<IOActionInfo>(dt);
                        PassLand.MustPassIOAction = MustPassActions;
                    }
                }
                return TaskConfigDetails;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 删除任务配置信息
        /// </summary>
        public static OperateReturnInfo DeleTaskConfig(TaskConfigInfo TaskConfig)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable hs = new Hashtable();
                    hs["TaskConditonCode"] = TaskConfig.TaskConditonCode;
                    dbOperator.SetDatas("DeleTaskConfig", hs);
                    scope.Complete();
                }
                return new OperateReturnInfo(OperateCodeEnum.Success);
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }

        /// <summary>
        /// 根据任务配置ID加载配置任务信息
        /// </summary>
        /// <param name="TaskConditonCode"></param>
        /// <returns></returns>
        public static IList<TaskConfigDetail> load_TaskDetail(string TaskConditonCode)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["TaskConditonCode"] = TaskConditonCode;
                DataTable dt = dbOperator.LoadDatas("load_TaskDetail", hs);
                IList<TaskConfigDetail> TaskDetail = ConnectConfigTool.TableToEntity<TaskConfigDetail>(dt);
                if (TaskDetail.Count == 0)
                {
                    return new List<TaskConfigDetail>();
                }
                return TaskDetail;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据任务配置编码加载必经地标信息
        /// </summary>
        public static IList<TaskConfigMustPass> LoadMustPassByConfigDetail(string TaskConditonCode, int DetailID)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["TaskConditonCode"] = TaskConditonCode;
                hs["DetailID"] = DetailID;
                DataTable dt = dbOperator.LoadDatas("LoadMustPassByConfigDetail", hs);
                return ConnectConfigTool.TableToEntity<TaskConfigMustPass>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存任务配置信息
        /// </summary>
        public static OperateReturnInfo SaveTaskConfig(TaskConfigInfo TaskConfig)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable hs = new Hashtable();
                    hs["TaskConditonCode"] = TaskConfig.TaskConditonCode;
                    hs["TaskConditonName"] = TaskConfig.TaskConditonName;
                    dbOperator.SetDatas("SaveTaskConfig", hs);
                    dbOperator.SetDatas("DeleTaskConfigdetail", hs);
                    foreach (TaskConfigDetail item in TaskConfig.TaskConfigDetail)
                    {
                        hs["TaskConditonCode"] = TaskConfig.TaskConditonCode;
                        hs["DetailID"] = item.DetailID;
                        hs["ArmOwnArea"] = item.ArmOwnArea;
                        hs["StorageState"] = item.StorageState;
                        hs["MaterialType"] = item.MaterialType;
                        hs["Action"] = item.Action;
                        hs["IsSensorStop"] = item.IsSensorStop;
                        hs["IsWaitPass"] = item.IsWaitPass;
                        hs["PassType"] = item.PassType;
                        hs["TaskDelayed"] = item.TaskDelayed;
                        hs["IsNeedCallBack"] = item.IsNeedCallBack;
                        hs["IsCallGoods"] = item.IsCallGoods;
                        dbOperator.SetDatas("SaveTaskConfigdetail", hs);
                        dbOperator.SetDatas("DeleteTaskMustPassLand", hs);
                        dbOperator.SetDatas("DeleteMustPassLandIOAction", hs);
                        int PassLandDetail = 0;
                        foreach (TaskConfigMustPass passLand in item.TaskConfigMustPass)
                        {
                            hs["MustPassLandCode"] = passLand.MustPassLandCode;
                            hs["PassLandDetail"] = PassLandDetail;
                            hs["Remark"] = passLand.Remark;
                            dbOperator.SetDatas("SaveTaskMustPassLand", hs);
                            int ActionDetailID = 0;
                            foreach (IOActionInfo IOAction in passLand.MustPassIOAction)
                            {
                                hs["ActionDetailID"] = ActionDetailID;
                                hs["ActionID"] = IOAction.ActionID;
                                dbOperator.SetDatas("SaveMustPassLandIOAction", hs);
                                ActionDetailID += 1;
                            }
                            PassLandDetail += 1;
                        }
                    }
                    scope.Complete();
                }
                return new OperateReturnInfo(OperateCodeEnum.Success);
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }


        /// <summary>
        /// 更新子任务PLC回写状态
        /// </summary>
        /// <param name="taskno"></param>
        /// <param name="detailid"></param>
        /// <returns></returns>
        public static OperateReturnInfo UpdatePlcCallBackState(string taskno, int detailid)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["taskno"] = taskno;
                hs["detailid"] = detailid;
                dbOperator.SetDatas("UpdateTaskDetailCallBack", hs);
                return new OperateReturnInfo(OperateCodeEnum.Success);
            }
            catch (Exception ex)
            {
                return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message);
            }
        }

        /// <summary>
        /// 更新子任务物料回写状态
        /// </summary>
        /// <param name="taskno"></param>
        /// <param name="detailid"></param>
        /// <returns></returns>
        public static OperateReturnInfo UpdateGoodsCallBackState(string taskno, int detailid)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["taskno"] = taskno;
                hs["detailid"] = detailid;
                dbOperator.SetDatas("UpdateTaskDetailGoodsCallBack", hs);
                return new OperateReturnInfo(OperateCodeEnum.Success);
            }
            catch (Exception ex)
            {
                return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message);
            }
        }

        /// <summary>
        /// 根据plccode获得需要回写的子任务
        /// </summary>
        /// <param name="plccode"></param>
        /// <returns></returns>
        public static IList<DispatchTaskDetail> LoadTaskDetailByPLCCode(int plccode, int locationid)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["plccdoe"] = plccode;
                hs["locationid"] = locationid;
                DataTable dt = dbOperator.LoadDatas("GetCallBackTaskDetail", hs);
                return ConnectConfigTool.TableToEntity<DispatchTaskDetail>(dt);
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 根据plccode获得需要回写的子任务
        /// </summary>
        /// <param name="plccode"></param>
        /// <returns></returns>
        public static IList<DispatchTaskDetail> LoadGoodsInfoTaskDetailByPLCCode(int plccode, int locationid)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["plccdoe"] = plccode;
                hs["locationid"] = locationid;
                DataTable dt = dbOperator.LoadDatas("LoadGoodsInfoTaskDetailByPLCCode", hs);
                return ConnectConfigTool.TableToEntity<DispatchTaskDetail>(dt);
            }
            catch (Exception ex)
            { throw ex; }
        }



        #endregion

        #region 特殊线路线段配置
        /// <summary>
        /// 根据线路线段分组查询线路线段设置
        /// </summary>
        /// <returns></returns>
        public static IList<RouteFragmentConfigInfo> get_RouteFragmentByFragment()
        {
            try
            {

                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("Load_RouteFragmentByFragment");
                IList<RouteFragmentConfigInfo> frag = ConnectConfigTool.TableToEntity<RouteFragmentConfigInfo>(dt);
                return frag;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 查询线路线段设置
        /// </summary>
        /// <returns></returns>
        public static IList<RouteFragmentConfigInfo> get_RouteFragment()
        {
            try
            {

                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("Load_RouteFragment");
                IList<RouteFragmentConfigInfo> frag = ConnectConfigTool.TableToEntity<RouteFragmentConfigInfo>(dt);
                return frag;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 查询指令档案
        /// </summary>
        /// <returns></returns>
        public static IList<CmdInfo> get_Cmd()
        {
            try
            {

                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("Load_Cmd");
                IList<CmdInfo> frag = ConnectConfigTool.TableToEntity<CmdInfo>(dt);
                return frag;
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 删除线路片段
        /// </summary>
        /// <param name="frag"></param>
        /// <returns></returns>
        public static OperateReturnInfo del_RouteFragment(RouteFragmentConfigInfo frag)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable hs = new Hashtable();
                    hs["Fragment"] = frag.Fragment;
                    dbOperator.SetDatas("del_RouteFrament", hs);
                    scope.Complete();
                    return new OperateReturnInfo(OperateCodeEnum.Success);
                }
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }
        /// <summary>
        /// 根据线路片段查询
        /// </summary>
        /// <param name="frag"></param>
        /// <returns></returns>
        public static IList<RouteFragmentConfigInfo> load_OtherByFragment(string frag)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["Fragment"] = frag;
                DataTable dt = dbOperator.LoadDatas("load_OtherByFragment", hs);
                return ConnectConfigTool.TableToEntity<RouteFragmentConfigInfo>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IList<RouteFragmentConfigInfo> loadOtherByAction(string action)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["ActionLandMark"] = action;
                DataTable dt = dbOperator.LoadDatas("load_ByAction", hs);
                return ConnectConfigTool.TableToEntity<RouteFragmentConfigInfo>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 保存线路线段设置
        /// </summary>
        /// <param name="frag"></param>
        /// <returns></returns>
        public static OperateReturnInfo save_tbRouteFragmentConfig(IList<RouteFragmentConfigInfo> frag)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                using (TransactionScope scope = new TransactionScope())
                {
                    string Fragment = frag[0].Fragment;
                    hs["Fragment"] = Fragment;
                    dbOperator.SetDatas("del_ByFragment", hs);
                    foreach (var item in frag)
                    {
                        hs["Fragment"] = item.Fragment;
                        hs["ActionLandMark"] = item.ActionLandMark;
                        hs["CmdCode"] = item.CmdCode;
                        hs["CmdPara"] = item.CmdPara;
                        hs["CmdIndex"] = item.CmdIndex;
                        dbOperator.SetDatas("save_RouteFragment", hs);
                    }
                    scope.Complete();
                }
                return new OperateReturnInfo(OperateCodeEnum.Success);

            }
            catch (Exception ex)
            {
                return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message);
            }
        }

        public static IList<SpeRunDirConfigInfo> get_RunDir()
        {
            try
            {

                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("Load_SpeRunDir");
                IList<SpeRunDirConfigInfo> RunDir = ConnectConfigTool.TableToEntity<SpeRunDirConfigInfo>(dt);
                return RunDir;
            }
            catch (Exception ex) { throw ex; }

        }
        #endregion

        #region IO模块功能配置
        /// <summary>
        /// 加载电梯IO配置
        /// </summary>
        /// <returns></returns>
        public static IList<IODeviceInfo> LoadIODeviceInfo()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                DataTable dt = dbOperator.LoadDatas("LoadIODeviceInfo", hs);
                return ConnectConfigTool.TableToEntity<IODeviceInfo>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除一条IO设备信息
        /// </summary>
        public static OperateReturnInfo Delete_IODeviceInfo(IODeviceInfo DeviceInfo)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                using (TransactionScope scope = new TransactionScope())
                {
                    hs["ID"] = DeviceInfo.ID;
                    dbOperator.SetDatas("Delete_IODeviceInfo", hs);
                    scope.Complete();
                    return new OperateReturnInfo(OperateCodeEnum.Success);
                }
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }

        /// <summary>
        /// 保存IO配置
        /// </summary>
        /// <param name="LiftIOConfig"></param>
        /// <returns></returns>
        public static OperateReturnInfo SaveIO(IList<IODeviceInfo> IO_Infoes)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (IODeviceInfo ioinfo in IO_Infoes)
                    {
                        hs["ID"] = ioinfo.ID;
                        hs["DeviceName"] = ioinfo.DeviceName;
                        hs["IP"] = ioinfo.IP;
                        hs["Port"] = ioinfo.Port;
                        dbOperator.SetDatas("SaveIOInfo", hs);
                    }
                    scope.Complete();
                    return new OperateReturnInfo(OperateCodeEnum.Success);
                }
            }
            catch (Exception ex)
            {
                return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message);
            }
        }

        /// <summary>
        /// 删除IO动作配置
        /// </summary>
        public static OperateReturnInfo DeleIOAction(IOActionInfo action)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable hs = new Hashtable();
                    hs["ActionID"] = action.ActionID;
                    dbOperator.SetDatas("deleAction", hs);
                    scope.Complete();
                }
                return new OperateReturnInfo(OperateCodeEnum.Success);
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }

        /// <summary>
        /// 动作档案
        /// </summary>
        /// <returns></returns>
        public static IList<IOActionInfo> LoadAllIOAction()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("LoadAllAction");
                return ConnectConfigTool.TableToEntity<IOActionInfo>(dt);
            }
            catch (Exception e)
            { throw e; }
        }

        /// <summary>
        /// 保存IO
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static OperateReturnInfo SaveIOAction(IOActionInfo item)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable hs = new Hashtable();
                    hs["ActionID"] = item.ActionID;
                    hs["ActionName"] = item.ActionName;
                    hs["IsPass"] = item.IsPass;
                    hs["IsWait"] = item.IsWait;
                    hs["DeviceID"] = item.DeviceID;
                    hs["IOUseState"] = item.IOUseState;
                    hs["TerminalID"] = item.TerminalID;
                    hs["TerminalType"] = item.TerminalType;
                    hs["WaitTime"] = item.WaitTime;
                    hs["TerminalData"] = item.TerminalData;
                    dbOperator.SetDatas("SaveAction", hs);
                    scope.Complete();
                }
                return new OperateReturnInfo(OperateCodeEnum.Success);
            }
            catch (Exception ex)
            { return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message); }
        }

        /// <summary>
        /// 加载充电桩档案
        /// </summary>
        /// <returns></returns>
        public static IList<ChargeStationInfo> LoadChargeInfo()
        {
            IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
            DataTable dt = dbOperator.LoadDatas("LoadCharge");
            return ConnectConfigTool.TableToEntity<ChargeStationInfo>(dt);
        }

        public static OperateReturnInfo SaveChargeInfo(IList<ChargeStationInfo> ChargeInfo)
        {
            IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
            Hashtable hs = new Hashtable();
            using (TransactionScope scope = new TransactionScope())
            {
                dbOperator.SetDatas("DeleteChargeInfo");
                foreach (ChargeStationInfo info in ChargeInfo)
                {
                    hs["ID"] = info.ID;
                    hs["State"] = info.State;
                    hs["IP"] = info.IP;
                    hs["Port"] = info.Port;
                    hs["ChargeLandCode"] = info.ChargeLandCode;
                    dbOperator.SetDatas("SaveChargeInfo", hs);
                }
                scope.Complete();
            }
            return new OperateReturnInfo(OperateCodeEnum.Success);

        }

        #endregion

        #region 平板档案处理

        /// <summary>
        /// 查询所有平板
        /// </summary>
        /// <returns></returns>
        public static IList<PdaInfo> LoadAllPad()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("LoadAllPad");
                return ConnectConfigTool.TableToEntity<PdaInfo>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得所有平板设置
        /// </summary>
        /// <returns></returns>
        public static IList<PdaOperSetInfo> LoadAllPdaOperSet()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("LoadAllPdaOperSet");
                return ConnectConfigTool.TableToEntity<PdaOperSetInfo>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存平板
        /// </summary>
        /// <param name="pda"></param>
        /// <returns></returns>
        public static OperateReturnInfo SavePad(PdaInfo pda)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                if (pda.IsNew)
                {
                    Hashtable hs = new Hashtable();
                    hs["PadID"] = pda.PadID;
                    DataTable dt = dbOperator.LoadDatas("LoadAllPad");
                    IList<PdaInfo> pdalist = ConnectConfigTool.TableToEntity<PdaInfo>(dt);
                    if (pdalist.Count(p => p.PadID == pda.PadID) > 0)
                    {
                        return new OperateReturnInfo(OperateCodeEnum.Failed, "平板ID已存在");
                    }
                }
                using (TransactionScope scope = new TransactionScope())
                {
                    //根据ID删除平板

                    Hashtable hs = new Hashtable();
                    hs["PadID"] = pda.PadID;
                    hs["Area"] = pda.Area;
                    hs["PadType"] = pda.PadType;
                    hs["IsViewArea"] = pda.IsViewArea;
                    hs["Discripetion"] = pda.Discripetion;
                    dbOperator.SetDatas("DeletePadByID", hs);
                    dbOperator.SetDatas("InsertPdaInfo", hs);
                    foreach (PdaOperSetInfo item in pda.PdaOperSetList)
                    {
                        hs["PdaID"] = pda.PadID;
                        hs["BtnID"] = item.BtnID;
                        hs["BtnDescript"] = item.BtnDescript;
                        hs["BtnSendValue"] = item.BtnSendValue;
                        hs["Area"] = item.Area;
                        hs["IsChooseBill"] = item.IsChooseBill;
                        hs["OperType"] = item.OperType;
                        dbOperator.SetDatas("InsertPdaOperSet", hs);
                    }
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
        /// 删除平板
        /// </summary>
        /// <param name="PdaID"></param>
        /// <returns></returns>
        public static OperateReturnInfo DeletePad(int PdaID)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                using (TransactionScope scope = new TransactionScope())
                {
                    Hashtable hs = new Hashtable();
                    hs["PadID"] = PdaID;
                    dbOperator.SetDatas("DeletePadByID", hs);
                    scope.Complete();
                }
                return new OperateReturnInfo(OperateCodeEnum.Success);
            }
            catch (Exception ex)
            {
                return new OperateReturnInfo(OperateCodeEnum.Failed, ex.Message);
            }
        }

        public static IList<PdaOperSetInfo> LoadallPdaOperSet(int PdaID)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable hs = new Hashtable();
                hs["PdaID"] = PdaID;
                DataTable dt = dbOperator.LoadDatas("LoadAllPdaOperSet", hs);
                return ConnectConfigTool.TableToEntity<PdaOperSetInfo>(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询区域
        /// </summary>
        /// <returns></returns>
        public static DataTable LoadAread()
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                DataTable dt = dbOperator.LoadDatas("LoadAread");
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据资源集查询线段配置
        /// </summary>
        /// <param name="TraID"></param>
        /// <returns></returns>
        public static IList<TraJunctionSegInfo> LoadTraSegByTraID(int TraID)
        {
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                Hashtable dic = new Hashtable();
                dic["TraJunctionID"] = TraID;
                DataTable dt = dbOperator.LoadDatas("LoadTraSegByTraID", dic);
                return ConnectConfigTool.TableToEntity<TraJunctionSegInfo>(dt);
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion
    }
}

