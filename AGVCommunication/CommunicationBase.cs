using AGVCore;
using DipatchModel;
using Model.CarInfoExtend;
using Model.Comoon;
using Model.MDM;
using Model.MSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tools;

namespace AGVCommunication
{
    public class CommunicationBase
    {
        #region 属性
        /// <summary>
        /// 小车通讯集合
        /// </summary>
        IList<object> AGVSessions = new List<object>();

        /// <summary>
        /// 充电桩通讯集合
        /// </summary>
        IList<object> ChargeStationSessions = new List<object>();
        /// <summary>
        /// IO设备通讯session
        /// </summary>
        IList<IOSession_Fbell> IOSessions = new List<IOSession_Fbell>();
        #endregion

        #region 方法
        /// <summary>
        /// 添加AGV发送指令
        /// </summary>
        public void AGV_AddControl(int AGVID, CommandToValue ctov)
        {
            try
            {
                AGVSessionBase session = AGVSessions.FirstOrDefault(p => (p as AGVSessionBase).DeviceID == AGVID) as AGVSessionBase;
                if (session != null)
                {
                    //LogHelper.WriteCreatTaskLog("添加AGV:"+ AGVID.ToString()+"指令-->"+ ctov.Command.ToString());
                    session.QueueCommand.Enqueue(ctov);
                }
                else
                {
                    DelegateState.InvokeDispatchStateEvent("添加指令失败,未能找到" + AGVID.ToString() + "号AGV通信管道!");
                    LogHelper.WriteLog("添加指令失败,未能找到" + AGVID.ToString() + "号AGV通信管道!");
                }
            }
            catch (Exception ex)
            {
                DelegateState.InvokeDispatchStateEvent("添加指令异常");
                LogHelper.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 通讯初始化
        /// </summary>
        public bool CommInit()
        {
            try
            {
                #region 小车
                DispatchAssembly AgvCommunitClass = CoreData.DispathAssemblies.FirstOrDefault(p => p.AssemblyType == 0);
                if (AgvCommunitClass == null)
                {
                    DelegateState.InvokeDispatchStateEvent("未配置AGV通信处理程序!");
                    LogHelper.WriteLog("未配置AGV通信处理程序!");
                    return false;
                }

                DispatchAssembly ChargeCommunitClass = CoreData.DispathAssemblies.FirstOrDefault(q => q.AssemblyType == 1);
                if (ChargeCommunitClass == null)
                {
                    DelegateState.InvokeDispatchStateEvent("未配置充电桩通信处理程序!");
                    LogHelper.WriteLog("未配置充电桩通信处理程序!");
                    return false;
                }

                //先停止清除所有的通信管道
                foreach (var item in AGVSessions)
                {
                    ExcuteRflectMethod(item, "Stop", null);
                }
                AGVSessions.Clear();
                //初始化所有AGV小车的通信
                foreach (CarInfo car in CoreData.CarList)
                {
                    AGVComPara para = new AGVComPara();
                    para.ServerIP = car.CarIP;
                    para.Port = Convert.ToInt32(car.CarPort);
                    int agvid = Convert.ToInt32(car.AgvID);
                    Type objType = Type.GetType(AgvCommunitClass.ClassName, true);
                    object obj = Activator.CreateInstance(objType);
                    SetModelValue("DeviceID", agvid, obj);
                    SetModelValue("ComPara", para, obj);
                    //obj.DeviceID = agvid;
                    //obj.ComPara = para;
                    AGVSessions.Add(obj);
                }
                foreach (var item in AGVSessions)
                {
                    bool InitResult = (bool)ExcuteRflectMethod(item, "Init", null);
                    if (InitResult)
                    {
                        DelegateState.InvokeDispatchStateEvent(GetModelValue("DeviceID", item).ToString() + "号AGV通讯初始化成功");
                        LogHelper.WriteLog(GetModelValue("DeviceID", item).ToString() + "号AGV通讯初始化成功");
                    }
                    else
                    {
                        DelegateState.InvokeDispatchStateEvent(GetModelValue("DeviceID", item).ToString() + "号AGV通讯初始化失败");
                        LogHelper.WriteLog(GetModelValue("DeviceID", item).ToString() + "号AGV通讯初始化失败");
                    }
                }
                #endregion

                #region 充电桩
                foreach (var item in ChargeStationSessions)
                {
                    ExcuteRflectMethod(item, "Stop", null);
                }
                ChargeStationSessions.Clear();


                foreach (ChargeStationInfo Charge in CoreData.ChargeList)
                {
                    AGVComPara para = new AGVComPara();
                    para.ServerIP = Charge.IP;
                    para.Port = Convert.ToInt32(Charge.Port);
                    int ChargeID = Convert.ToInt32(Charge.ID);
                    Type objType = Type.GetType(ChargeCommunitClass.ClassName, true);
                    object obj = Activator.CreateInstance(objType);
                    SetModelValue("DeviceID", ChargeID, obj);
                    SetModelValue("ComPara", para, obj);
                    ChargeStationSessions.Add(obj);
                }

                foreach (var item in ChargeStationSessions)
                {
                    bool InitResult = (bool)ExcuteRflectMethod(item, "Init", null);
                    if (InitResult)
                    {
                        DelegateState.InvokeDispatchStateEvent(GetModelValue("DeviceID", item).ToString() + "号充电桩通讯初始化成功");
                    }
                    else
                    {
                        DelegateState.InvokeDispatchStateEvent(GetModelValue("DeviceID", item).ToString() + "号充电桩通讯初始化失败");
                    }
                }
                #endregion
                #region io设备
                foreach (var item in IOSessions)
                {
                    item.Stop();
                }
                IOSessions.Clear();


                foreach (IODeviceInfo node in CoreData.IOList)
                {
                    AGVComPara para = new AGVComPara();
                    para.ServerIP = node.IP;
                    para.Port = Convert.ToInt32(node.Port);
                    int ioid = Convert.ToInt32(node.ID);
                    IOSessions.Add(new IOSession_Fbell(ioid, para));
                }

                foreach (var item in IOSessions)
                {
                    if (item.Init())
                    {
                        DelegateState.InvokeDispatchStateEvent(item.DeviceID.ToString() + "号IO设备通讯初始化成功");
                    }
                    else
                    {
                        DelegateState.InvokeDispatchStateEvent(item.DeviceID.ToString() + "号IO设备通讯初始化失败");
                    }
                }
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                DelegateState.InvokeDispatchStateEvent("初始化客户端通讯异常");
                LogHelper.WriteErrorLog(ex);
                return false;
            }
        }


        /// <summary>
        /// 启动通讯
        /// </summary>
        public bool Start()
        {
            try
            {
                foreach (var item in AGVSessions)
                {
                    new Thread(delegate ()
                    {
                        bool StartResult = (bool)ExcuteRflectMethod(item, "Start", null);
                        if (StartResult)
                        {
                            DelegateState.InvokeDispatchStateEvent(GetModelValue("DeviceID", item).ToString() + "号AGV启动通讯成功");
                            LogHelper.WriteLog(GetModelValue("DeviceID", item).ToString() + "号AGV启动通讯成功");
                        }
                        else
                        {
                            try
                            {
                                CarInfo Car = CoreData.CarList.FirstOrDefault(p => p.AgvID == Convert.ToInt16(GetModelValue("DeviceID", item)));
                                if (Car != null)
                                { Car.bIsCommBreak = true; }
                            }
                            catch
                            { }
                            DelegateState.InvokeDispatchStateEvent(GetModelValue("DeviceID", item).ToString() + "号AGV启动通讯失败");
                            LogHelper.WriteLog(GetModelValue("DeviceID", item).ToString() + "号AGV启动通讯失败");
                        }
                    })
                    { IsBackground = true }.Start();
                }
                foreach (var item in ChargeStationSessions)
                {
                    new Thread(delegate ()
                    {
                        bool StartResult = (bool)ExcuteRflectMethod(item, "Start", null);
                        if (StartResult)
                        {
                            DelegateState.InvokeDispatchStateEvent(GetModelValue("DeviceID", item).ToString() + "号充电桩通讯启动成功...");
                        }
                        else
                        {
                            DelegateState.InvokeDispatchStateEvent(GetModelValue("DeviceID", item).ToString() + "号充电桩通讯启动失败...");
                        }
                    })
                    { IsBackground = true }.Start();
                }
                foreach (var item in IOSessions)
                {
                    new Thread(delegate ()
                    {
                        if (item.Start())
                        {
                            DelegateState.InvokeDispatchStateEvent(item.DeviceID.ToString() + "号IO设备通讯启动成功...");
                        }
                        else
                        {
                            IODeviceInfo IOInfo = CoreData.IOList.FirstOrDefault(p => p.ID == item.DeviceID);
                            if (IOInfo != null)
                            { IOInfo.bIsCommBreak = true; }
                            DelegateState.InvokeDispatchStateEvent(item.DeviceID.ToString() + "号IO设备通讯启动失败...");
                        }
                    })
                    { IsBackground = true }.Start();
                }
                return true;
            }
            catch (Exception ex)
            {
                DelegateState.InvokeDispatchStateEvent("设备通讯启动异常");
                LogHelper.WriteErrorLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 添加充电桩指令
        /// </summary>
        /// <param name="chargeid"></param>
        /// <param name="ctov"></param>
        public void Charge_AddControl(int chargeid, CommandToValue ctov)
        {
            try
            {
                AGVSessionBase session = ChargeStationSessions.FirstOrDefault(p => (p as AGVSessionBase).DeviceID == chargeid) as AGVSessionBase;
                if (session != null)
                {
                    session.QueueCommand.Enqueue(ctov);
                }
                else
                {
                    DelegateState.InvokeDispatchStateEvent("添加指令失败,未能找到" + chargeid.ToString() + "号充电桩 ");
                }
            }
            catch (Exception ex)
            {
                DelegateState.InvokeDispatchStateEvent("添加充电桩指令异常");
            }
        }
        /// <summary>
        /// 添加IO设备指令
        /// </summary>
        public void IO_AddControl(int ioid, CommandToValue ctov)
        {
            try
            {
                IOSession_Fbell session = IOSessions.FirstOrDefault(p => p.DeviceID == ioid);
                if (session != null)
                {
                    session.QueueCommand.Enqueue(ctov);
                }
                else
                {
                    DelegateState.InvokeDispatchStateEvent("添加指令失败,未能找到" + ioid.ToString() + "号IO设备 ");
                }
            }
            catch (Exception ex)
            {
                DelegateState.InvokeDispatchStateEvent("添加IO设备指令异常");
            }
        }
        #endregion


        #region 自定义函数
        /// <summary>
        /// 反射执行方法
        /// </summary>
        private object ExcuteRflectMethod(object SeesionObj, string MethodName, object[] parameters)
        {
            try
            {
                //Type objType = Type.GetType(AssemClass.ClassName, true);
                //object obj = Activator.CreateInstance(objType);
                Type objType = SeesionObj.GetType();
                MethodInfo method = objType.GetMethod(MethodName, new Type[] { });
                return method.Invoke(SeesionObj, parameters);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
                return null;
            }
        }

        /// <summary>
        /// 反射获取属性值
        /// </summary>
        public object GetModelValue(string FieldName, object obj)
        {
            try
            {
                Type Ts = obj.GetType();
                object Value = Ts.GetProperty(FieldName).GetValue(obj, null);
                //string Value = Convert.ToString(o);
                //if (string.IsNullOrEmpty(Value)) return null;
                return Value;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 反射设置属性值
        /// </summary>
        public bool SetModelValue(string FieldName, object Value, object obj)
        {
            try
            {
                Type Ts = obj.GetType();
                object v = Convert.ChangeType(Value, Ts.GetProperty(FieldName).PropertyType);
                Ts.GetProperty(FieldName).SetValue(obj, v, null);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

    }
}
