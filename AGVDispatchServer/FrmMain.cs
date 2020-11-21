using AGVComponents;
using AGVCore;
using AGVDAccess;
using AGVDispacth;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DipatchModel;
using Model.CarInfoExtend;
using Model.Comoon;
using Model.MDM;
using SQLServerOperator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools;

namespace AGVDispatchServer
{
    public partial class FrmMain : BaseForm
    {
        public FrmMain()
        {
            InitializeComponent();
            btnTestStart.Enabled = false;
            btnTestStop.Enabled = false;
            btnCancelTask.Enabled = false;
            btnStorageReset.Enabled = false;
            btnSame.Enabled = false;
            btnTestTraffic.Enabled = false;
            btnBack.Enabled = false;
            btnStartCharge.Enabled = false;
            btnStopCharge.Enabled = false;
            btnReDo.Enabled = false;
            btnFinish.Enabled = false;
            btnClearSysCarInfo.Enabled = false;
            btnClearAllTask.Enabled = false;
            btnSetCarSite.Enabled = false;
            btnBroadCast.Enabled = false;
            btnTestchazi.Enabled = false;
        }

        #region 委托事件
        /// <summary>
        /// 显示日志信息
        /// </summary>
        private DispatchStateCallBack delshowMessage;

        private delegate void DelBind();
        #endregion

        #region 属性

        /// <summary>
        /// 锁对象
        /// </summary>
        private static object lockobj = new object();

        /// <summary>
        /// 调度者
        /// </summary>
        IDispacther Dispacther { get; set; }

        ///// <summary>
        ///// 外围服务逻辑处理
        ///// </summary>
        //ExternelHandler ExternelHandel { get; set; }

        /// <summary>
        /// 服务线程 
        /// </summary>
        private Thread thServer;
        #endregion

        #region 事件
        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_Shown(object sender, EventArgs e)
        {
            try
            {
                Process[] processByName = Process.GetProcessesByName("AGVDispatchServer");
                if (processByName.Length > 1)
                {
                    this.FormClosing -= this.FrmMain_FormClosing;
                    MessageBox.Show("已有调度服务进程运行");
                    Application.Exit();
                    return;
                }
                //绑定日志显示
                delshowMessage = AddMessage;
                //创建调度者
                Dispacther = new Dispacther();
                //ExternelHandel = new ExternelHandler();
                //绑定事件
                DelegateState.DispatchStateEvent += this.AddMessage;
                DelegateState.CarFeedbackEvent += Dispacther.HandleCarFeedBack;
                DelegateState.IOFeedEvent += Dispacther.HandleIOFeedBack;
                DelegateState.ChargeEvent += Dispacther.HandleChargeFeedBack;

                this.timerClearMemory.Interval = 1000 * 60 * 1;//一分钟回收一次内存
                this.timerClearMemory.Enabled = true;
                ConnectConfigTool.setDBase();
                //启动服务
                thServer = new Thread(Start);
                thServer.IsBackground = true;
                thServer.Start();

                timerFormRefsh.Enabled = true;
                timerFormRefsh.Interval = 300;

            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
                XtraMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                thServer = new Thread(Start);
                thServer.IsBackground = true;
                thServer.Start();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //Dispacther.DispatchStop();
                //ExternelHandel.StopExternelServer();
                //if (this.thServer != null)
                //{
                //    thServer.Abort();
                //}
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 窗口关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                MinForm();
                e.Cancel = true;
            }
            catch (Exception ex)
            {
                DelegateState.InvokeDispatchStateEvent("最小化窗口失败");
            }
        }

        /// <summary>
        /// 托盘图标双击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                MaxForm();
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 退出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (XtraMessageBox.Show("该操作将会停止监控小车作业信息，是否继续？", "警告", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    this.FormClosing -= this.FrmMain_FormClosing;
                    Application.Exit();
                    return;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 刷新任务
        /// </summary>
        /// <param name="sender"></param>
        private void btnRefsh_Click(object sender, EventArgs e)
        {
            try
            {
                IList<DispatchTaskInfo> tasklist = AGVServerDAccess.LoadDispatchTask("0,1");
                this.bsTask.DataSource = tasklist;
                this.bsTask.ResetBindings(false);
                this.bsTaskDetail.ResetBindings(false);
                gvTask_FocusedRowChanged(null, null);
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 重做任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReDo_Click(object sender, EventArgs e)
        {
            try
            {
                if (XtraMessageBox.Show("确认重做?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                { return; }
                this.btnFinish.Focus();
                DispatchTaskInfo task = bsTask.Current as DispatchTaskInfo;
                if (task == null) { return; }
                foreach (DispatchTaskDetail taskDetail in task.TaskDetail)
                {
                    if (taskDetail.IsSelect)
                    {
                        AGVServerDAccess.TaskHandle(task.dispatchNo, taskDetail.DetailID, 0);
                    }
                }
                this.bsTask.DataSource = AGVServerDAccess.LoadDispatchTask("0,1,5");
                this.bsTask.ResetBindings(false);
                gvTask_FocusedRowChanged(null, null);
                LogHelper.WriteCallBoxLog("操作员:" + (CurrentLogin.CurrentUser != null ? CurrentLogin.CurrentUser.UserName : "") + "执行了重做按钮操作");
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
            }
        }
        /// <summary>
        /// 完成任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFinish_Click(object sender, EventArgs e)
        {
            try
            {
                if (XtraMessageBox.Show("确认完成?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                { return; }
                this.btnFinish.Focus();
                DispatchTaskInfo task = bsTask.Current as DispatchTaskInfo;
                if (task == null) { return; }
                foreach (DispatchTaskDetail taskDetail in task.TaskDetail)
                {
                    if (taskDetail.IsSelect)
                    {
                        AGVServerDAccess.TaskHandle(task.dispatchNo, taskDetail.DetailID, 2);
                        StorageInfo store = CoreData.StorageList.FirstOrDefault(p => p.LankMarkCode == taskDetail.LandCode);
                        if (store != null)
                        {
                            store.LockState = 0;
                            AGVServerDAccess.UpdateStorageState(-1, 0, store.ID);
                        }
                    }
                }
                DispatchTaskInfo NowTask = AGVServerDAccess.LoadTaskByNo(task.dispatchNo);
                if (NowTask != null && NowTask.TaskState == 2)
                {
                    CarInfo TaskCar = CoreData.CarList.FirstOrDefault(p => p.AgvID == NowTask.ExeAgvID);
                    if (TaskCar != null)
                    {
                        string filePath = System.AppDomain.CurrentDomain.BaseDirectory + @"\MemoryFile\CarTaskInfo.xml";
                        Hashtable ht = new Hashtable();
                        ht["TaskNo"] = "";
                        XMLClass.AppendXML(filePath, "N" + TaskCar.AgvID.ToString(), ht);
                        TaskCar.ExcuteTaksNo = "";
                        TaskCar.TaskDetailID = -1;
                        if (TaskCar.Route != null)
                        {
                            TaskCar.Route.Clear();
                            TaskCar.AllRouteLandCode = "";
                            TaskCar.RealyRoute.Clear();
                            TaskCar.RouteLands.Clear();
                            TaskCar.TurnLands.Clear();
                        }
                    }
                }
                this.bsTask.DataSource = AGVServerDAccess.LoadDispatchTask("0,1,5");
                this.bsTask.ResetBindings(false);
                gvTask_FocusedRowChanged(null, null);
                LogHelper.WriteCreatTaskLog("操作员:" + (CurrentLogin.CurrentUser != null ? CurrentLogin.CurrentUser.UserName : "") + "执行了完成按钮操作");
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 定时垃圾回收
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerClearMemory_Tick(object sender, EventArgs e)
        {
            try
            {
                timerClearMemory.Enabled = false;
                ClearMemory();
                //删除过时日志
                LogHelper.DeleteLogFile();
                //处理任务,记录历史任务表

            }
            catch (Exception ex)
            {

            }
            finally
            {
                timerClearMemory.Enabled = true;
            }
        }

        /// <summary>
        /// 平板界面刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerFormRefsh_Tick(object sender, EventArgs e)
        {
            try
            {
                timerFormRefsh.Enabled = false;
                if (CoreData.CarList.Count > 0)
                {
                    //绑定AGV小车
                    this.bsCars.DataSource = CoreData.CarList;
                    this.gvcars.RefreshData();
                    //this.gvcars.BestFitColumns();
                }

                if (CoreData.ChargeList.Count > 0)
                {
                    this.bsChargeInfo.DataSource = CoreData.ChargeList;
                    this.gvChargeInfo.RefreshData();
                    //this.gvChargeInfo.BestFitColumns();
                }

                if (CoreData.IOList.Count > 0)
                {
                    this.bsIOInfoes.DataSource = CoreData.IOList;
                    gvIOInfo.RefreshData();
                    //gvIOInfo.BestFitColumns();
                }
                string TrafficInfo = Dispacther.getTrafficInfo();
                this.txtTrafficInfo.Text = TrafficInfo;
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
            }
            finally
            {
                timerFormRefsh.Enabled = true;
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 启动服务
        /// </summary>
        private void Start()
        {
            try
            {
                DelegateState.InvokeDispatchStateEvent("正在初始化调度组件...");
                //调度初始化
                if (Dispacther.DispatchInit())
                {
                    DelegateState.InvokeDispatchStateEvent("初始化调度组件成功...");
                }
                else
                {
                    DelegateState.InvokeDispatchStateEvent("初始化调度组件失败...");
                    return;
                }
                if (Dispacther.DispatchStart())
                {
                    DelegateState.InvokeDispatchStateEvent("启动调度服务成功...");
                }
                else
                {
                    DelegateState.InvokeDispatchStateEvent("启动调度服务失败...");
                }
                //启动外围设备服务
                #region 启动外围服务
                //if (ExternelHandel.ExternelServerStart())
                //{ DelegateState.InvokeDispatchStateEvent("启动外围服务成功..."); }
                //else
                //{ DelegateState.InvokeDispatchStateEvent("启动外围服务失败..."); }
                #endregion
            }
            catch (Exception ex)
            {
                DelegateState.InvokeDispatchStateEvent("启动服务失败...");
                LogHelper.WriteErrorLog(ex);
            }
        }


        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="msg"></param>
        private void AddMessage(string msg)
        {
            try
            {
                if (listboxtask.InvokeRequired)
                {
                    this.listboxtask.Invoke(delshowMessage, new object[] { msg });
                }
                else
                {
                    lock (lockobj)
                    {
                        try
                        {
                            if (this.listboxtask.Items.Count > 50)
                            {
                                this.listboxtask.Items.Remove(this.listboxtask.Items[0]);
                            }
                            this.listboxtask.Items.Add(msg);
                            this.listboxtask.SelectedIndex = this.listboxtask.Items.Count - 1;//定位显示最后一条信息 
                        }
                        catch
                        {
                            LogHelper.WriteLog("捕捉到异常");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(ex);
            }
        }



        /// <summary>
        /// 最小化窗口
        /// </summary>
        private void MinForm()
        {
            this.Hide();
            this.WindowState = FormWindowState.Minimized;
            notify.Visible = true;
        }

        /// <summary>
        /// 最大化窗口
        /// </summary>
        private void MaxForm()
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Maximized;  //还原窗体
            notify.Visible = false;  //托盘图标隐藏
        }
        #endregion

        #region 内存回收
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        /// <summary>
        /// 释放内存
        /// </summary>
        public static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }
        #endregion

        #region 功能测试
        private void btnSame_Click(object sender, EventArgs e)
        {
            try
            {
                if (XtraMessageBox.Show("确认同步所有数据信息?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Dispacther.ReadBaseInfo();
                    XtraMessageBox.Show("同步基础档案数据成功");
                    LogHelper.WriteCallBoxLog("操作员:" + (CurrentLogin.CurrentUser != null ? CurrentLogin.CurrentUser.UserName : "") + "执行了同步数据按钮操作");
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("同步基础档案数据异常");
            }
        }

        private void btnTestStart_Click(object sender, EventArgs e)
        {
            FormPara frm = new FormPara();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                int agvid = Convert.ToInt32(frm.AGVID);
                string para = frm.Para;
                if (XtraMessageBox.Show("确认启动当前AGV?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Dispacther.AGV_AddCommand(agvid, new CommandToValue(AGVCommandEnum.Start));
                    LogHelper.WriteCallBoxLog("操作员:" + (CurrentLogin.CurrentUser != null ? CurrentLogin.CurrentUser.UserName : "") + "执行了启动AGV按钮操作");
                }
            }
        }

        private void btnTestStop_Click(object sender, EventArgs e)
        {
            FormPara frm = new FormPara();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                int agvid = -1;
                try
                {
                    agvid = Convert.ToInt32(frm.AGVID);
                }
                catch
                { XtraMessageBox.Show("请输入有效车号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                string para = frm.Para;
                if (XtraMessageBox.Show("确认停止当前AGV?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Dispacther.AGV_AddCommand(agvid, new CommandToValue(AGVCommandEnum.Stop));
                    LogHelper.WriteCallBoxLog("操作员:" + (CurrentLogin.CurrentUser != null ? CurrentLogin.CurrentUser.UserName : "") + "执行了停止AGV按钮操作");
                }
            }
        }

        private void btnBroadCast_Click(object sender, EventArgs e)
        {
            try
            {
                OperateReturnInfo opr = null;
                if (XtraMessageBox.Show("确认广播订单信息?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    using (WaitDialogForm dialog = new WaitDialogForm("正在推送,请稍后...", "提示"))
                    {
                        //opr = Dispacther.BroadPadBills();
                    }
                }
                if (opr != null && opr.ReturnCode == OperateCodeEnum.Success)
                { XtraMessageBox.Show("操作成功!"); }
                else
                { XtraMessageBox.Show("操作失败!"); }
            }
            catch (Exception ex)
            { XtraMessageBox.Show(ex.Message); }
        }
        private void btnStorageReset_Click(object sender, EventArgs e)
        {
            try
            {
                FormStorageReset frm = new FormStorageReset();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    int startid = frm.StartID;
                    int endid = frm.EndID;
                    int storagestate = frm.StorageState;
                    int lockstate = frm.LockState;
                    if (XtraMessageBox.Show("确认更新储位状态?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        AGVServerDAccess.UpdateStorageState(storagestate, lockstate, startid);
                        XtraMessageBox.Show("更新成功！");
                        LogHelper.WriteCallBoxLog("操作员:" + (CurrentLogin.CurrentUser != null ? CurrentLogin.CurrentUser.UserName : "") + "执行了更新储位按钮操作,更新储位号:" + startid.ToString() + "修改状态为:" + storagestate.ToString() + "锁状态为:" + lockstate.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btnCancelTask_Click(object sender, EventArgs e)
        {
            int agvid = -1;
            FormPara frm = new FormPara();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrEmpty(frm.AGVID)) { return; }
                try
                {
                    agvid = Convert.ToInt32(frm.AGVID);
                }
                catch
                { XtraMessageBox.Show("请输入有效车号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                string para = frm.Para;
                if (XtraMessageBox.Show("确认复位当前AGV?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Dispacther.AGV_AddCommand(agvid, new CommandToValue(AGVCommandEnum.CancelTast));
                    LogHelper.WriteCallBoxLog("操作员:" + (CurrentLogin.CurrentUser != null ? CurrentLogin.CurrentUser.UserName : "") + "执行了复位" + agvid.ToString() + "AGV按钮操作");
                }
            }
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
        /// <summary>
        /// 获得对应的字节
        /// </summary>
        /// <param name="portnum"></param>
        /// <returns></returns>
        IList<byte> GetByteListByPortState(int portnum)
        {
            List<byte> result = new List<byte>() { 0x00, 0x00, 0x00, 0x00 };
            if (portnum >= 1 && portnum <= 32)
            {
                int bytenum = portnum / 8;
                if (bytenum == 0)
                {
                    result[1] = BuildByteByPortNum(portnum);
                }
                if (bytenum == 1)
                {
                    result[0] = BuildByteByPortNum(portnum);
                }
                if (bytenum == 2)
                {
                    result[3] = BuildByteByPortNum(portnum);
                }
                if (bytenum == 3)
                {
                    result[2] = BuildByteByPortNum(portnum);
                }
            }
            return result;
        }
        /// <summary>
        /// 根据设备口号生成对应的byte
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        byte BuildByteByPortNum(int portnum)
        {
            int portbyteindex = portnum % 8;
            List<string> bitList = new List<string>() { "0", "0", "0", "0", "0", "0", "0", "0" };
            bitList[8 - portbyteindex] = "1";
            return (byte)Convert.ToInt32(string.Join("", bitList.ToArray()), 2);
        }
        private void btnTestTraffic_Click(object sender, EventArgs e)
        {
            try
            {
                RoutePlan CountRoute = new RoutePlan();
                if (string.IsNullOrEmpty(txtAGVID.Text.Trim())) { return; }
                string ArmLand = txtArmLand.Text.Trim();
                if (string.IsNullOrEmpty(ArmLand)) { return; }
                //LandmarkInfo BeginLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == "14" /*car.CurrSite.ToString()*/);
                //LandmarkInfo EndLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == ArmLand);
                //List<LandmarkInfo> route = CountRoute.GetRoute(BeginLand, EndLand);
                //return;
                try
                {
                    Convert.ToInt32(txtAGVID.Text);
                }
                catch
                { XtraMessageBox.Show("请输入有效车号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                CarInfo car = CoreData.CarList.FirstOrDefault(p => p.AgvID == Convert.ToInt32(txtAGVID.Text)/* && !p.bIsCommBreak*/);
                if (car == null) { return; }
                if (car.LowPower)
                {
                    XtraMessageBox.Show(car.AgvID.ToString() + "号车低电量!");
                    return;
                }
                LandmarkInfo BeginLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == car.CurrSite.ToString());
                LandmarkInfo EndLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == ArmLand);
                if (BeginLand == null)
                {
                    XtraMessageBox.Show("输入的车辆所在地标不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (EndLand == null)
                {
                    XtraMessageBox.Show("输入的目的地标不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                car.Route = CountRoute.GetRoute(BeginLand, EndLand);
                car.AllRouteLandCode = "," + string.Join(",", car.Route.Select(p => p.LandmarkCode)) + ",";
                if (car.Route.Count <= 0) { return; }
                if (XtraMessageBox.Show("确认启动当前AGV?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (car.bIsCommBreak)
                    {
                        XtraMessageBox.Show(car.AgvID.ToString() + "号车已掉线!");
                        return;
                    }
                    //在发送AGV路线命令之前需要清除一下车辆上路线缓存信息
                    ChargeStationInfo FreeChargeStation = CoreData.ChargeList.FirstOrDefault(p => p.ChargeLandCode == car.CurrSite.ToString());
                    if (FreeChargeStation != null)
                    {

                        Dispacther.Add_EleCommand(FreeChargeStation.ID, new CommandToValue(AGVCommandEnum.StopCharge));
                        car.NowChargeLandCode = "";
                    }
                    //Dispacther.AGV_AddCommand(car.AgvID, new CommandToValue(AGVCommandEnum.CancelTast));
                    //清除路线缓存信息后发送路线指令
                    Dispacther.SendCarRoute(car);
                    LogHelper.WriteCallBoxLog("操作员:" + (CurrentLogin.CurrentUser != null ? CurrentLogin.CurrentUser.UserName : "") + "执行了指定AGV" + car.AgvID.ToString() + "到目的地标" + EndLand.LandmarkCode + "去");
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void gvTask_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                DispatchTaskInfo CurrTask = bsTask.Current as DispatchTaskInfo;
                if (CurrTask != null)
                {
                    bsTaskDetail.DataSource = CurrTask.TaskDetail;
                    bsTaskDetail.ResetBindings(false);
                }
                else
                {
                    bsTaskDetail.Clear();
                    bsTaskDetail.ResetBindings(false);
                }
            }
            catch (Exception ex)
            { }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtBackAGV.Text.Trim())) { return; }
                try
                {
                    Convert.ToInt32(txtBackAGV.Text);
                }
                catch
                { XtraMessageBox.Show("请输入有效车号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                CarInfo NoTaskCar = CoreData.CarList.FirstOrDefault(p => p.AgvID == Convert.ToInt32(txtBackAGV.Text));
                LandmarkInfo BeginLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == NoTaskCar.CurrSite.ToString());
                LandmarkInfo EndLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == NoTaskCar.StandbyLandMark);
                if (BeginLand != null && EndLand != null)
                {
                    RoutePlan CountRoute = new RoutePlan();
                    NoTaskCar.Route = CountRoute.GetRoute(BeginLand, EndLand);
                    NoTaskCar.AllRouteLandCode = "," + string.Join(",", NoTaskCar.Route.Select(p => p.LandmarkCode)) + ",";
                    if (NoTaskCar.Route.Count <= 0) { return; }
                    if (XtraMessageBox.Show("确认启动当前AGV?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (NoTaskCar.bIsCommBreak)
                        {
                            XtraMessageBox.Show(NoTaskCar.AgvID.ToString() + "号车已掉线!");
                            return;
                        }
                        ChargeStationInfo FreeChargeStation = CoreData.ChargeList.FirstOrDefault(p => p.ChargeLandCode == NoTaskCar.CurrSite.ToString());
                        if (FreeChargeStation != null)
                        {
                            Dispacther.Add_EleCommand(FreeChargeStation.ID, new CommandToValue(AGVCommandEnum.StopCharge));
                            NoTaskCar.NowChargeLandCode = "";
                        }
                        //Dispacther.AGV_AddCommand(NoTaskCar.AgvID, new CommandToValue(AGVCommandEnum.CancelTast));
                        //Thread.Sleep(20);
                        //清除路线缓存信息后发送路线指令
                        Dispacther.SendCarRoute(NoTaskCar);
                        LogHelper.WriteCallBoxLog("操作员:" + (CurrentLogin.CurrentUser != null ? CurrentLogin.CurrentUser.UserName : "") + "执行了让AGV" + NoTaskCar.AgvID.ToString() + "回待命点");
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        private void btnStartCharge_Click(object sender, EventArgs e)
        {
            try
            {
                int ChargeID = 0;
                if (string.IsNullOrEmpty(txtChargeID.Text.Trim())) { return; }
                try
                {
                    ChargeID = Convert.ToInt32(txtChargeID.Text);
                }
                catch
                { XtraMessageBox.Show("请输入有效充电桩号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (XtraMessageBox.Show("确认启动充电?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Dispacther.AddChargeCommand(ChargeID, new CommandToValue(AGVCommandEnum.Start));

                    Dispacther.Add_EleCommand(ChargeID, new CommandToValue(AGVCommandEnum.StartCharge));
                }
            }
            catch (Exception ex)
            { }
        }

        private void btnStopCharge_Click(object sender, EventArgs e)
        {
            try
            {
                int ChargeID = 0;
                if (string.IsNullOrEmpty(txtChargeID.Text.Trim())) { return; }
                try
                {
                    ChargeID = Convert.ToInt32(txtChargeID.Text);
                }
                catch
                { XtraMessageBox.Show("请输入有效充电桩号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (XtraMessageBox.Show("确认停止充电?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Dispacther.Add_EleCommand(ChargeID, new CommandToValue(AGVCommandEnum.StopCharge));
                }
            }
            catch (Exception ex)
            { }
        }


        //设置权限
        private void SetAuthority()
        {
            try
            {
                if (CurrentLogin.CurrentUser != null && CurrentLogin.SysOprButtons != null && CurrentLogin.SysOprButtons.Count > 0)
                {
                    btnTestStart.Enabled = CurrentLogin.SysOprButtons.Where(p => p.ButtonName == "btnTestStart").Count() > 0 ? true : false;
                    btnTestStop.Enabled = CurrentLogin.SysOprButtons.Where(p => p.ButtonName == "btnTestStop").Count() > 0 ? true : false;
                    btnCancelTask.Enabled = CurrentLogin.SysOprButtons.Where(p => p.ButtonName == "btnCancelTask").Count() > 0 ? true : false;
                    btnStorageReset.Enabled = CurrentLogin.SysOprButtons.Where(p => p.ButtonName == "btnStorageReset").Count() > 0 ? true : false;
                    btnSame.Enabled = CurrentLogin.SysOprButtons.Where(p => p.ButtonName == "btnSame").Count() > 0 ? true : false;
                    btnTestTraffic.Enabled = CurrentLogin.SysOprButtons.Where(p => p.ButtonName == "btnTestTraffic").Count() > 0 ? true : false;
                    btnBack.Enabled = CurrentLogin.SysOprButtons.Where(p => p.ButtonName == "btnBack").Count() > 0 ? true : false;
                    btnStartCharge.Enabled = CurrentLogin.SysOprButtons.Where(p => p.ButtonName == "btnStartCharge").Count() > 0 ? true : false;
                    btnStopCharge.Enabled = CurrentLogin.SysOprButtons.Where(p => p.ButtonName == "btnStopCharge").Count() > 0 ? true : false;
                    //btnReDo.Enabled = CurrentLogin.SysOprButtons.Where(p => p.ButtonName == "btnReDo").Count() > 0 ? true : false;
                    btnFinish.Enabled = CurrentLogin.SysOprButtons.Where(p => p.ButtonName == "btnFinish").Count() > 0 ? true : false;
                    btnClearSysCarInfo.Enabled = CurrentLogin.SysOprButtons.Where(p => p.ButtonName == "btnClearSysCarInfo").Count() > 0 ? true : false;
                    btnSetCarSite.Enabled = CurrentLogin.SysOprButtons.Where(p => p.ButtonName == "btnSetCarSite").Count() > 0 ? true : false;
                    btnBroadCast.Enabled = CurrentLogin.SysOprButtons.Where(p => p.ButtonName == "btnBroadCast").Count() > 0 ? true : false;
                    btnTestchazi.Enabled = CurrentLogin.SysOprButtons.Where(p => p.ButtonName == "btnTestchazi").Count() > 0 ? true : false;
                    btnClearAllTask.Enabled = CurrentLogin.SysOprButtons.Where(p => p.ButtonName == "btnClearAllTask").Count() > 0 ? true : false;
                }
                else if (CurrentLogin.SysOprButtons == null || CurrentLogin.SysOprButtons.Count <= 0)
                {
                    btnTestStart.Enabled = true;
                    btnTestStop.Enabled = true;
                    btnCancelTask.Enabled = true;
                    btnStorageReset.Enabled = true;
                    btnSame.Enabled = true;
                    btnTestTraffic.Enabled = true;
                    btnBack.Enabled = true;
                    btnStartCharge.Enabled = true;
                    btnStopCharge.Enabled = true;
                    btnReDo.Enabled = false;
                    btnFinish.Enabled = true;
                    btnClearSysCarInfo.Enabled = true;
                    btnClearAllTask.Enabled = true;
                    btnSetCarSite.Enabled = true;
                    btnBroadCast.Enabled = true;
                    btnTestchazi.Enabled = true;
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void btnExitLog_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (XtraMessageBox.Show("确认退出登陆?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                btnTestStart.Enabled = false;
                btnTestStop.Enabled = false;
                btnCancelTask.Enabled = false;
                btnStorageReset.Enabled = false;
                btnSame.Enabled = false;
                btnTestTraffic.Enabled = false;
                btnBack.Enabled = false;
                btnStartCharge.Enabled = false;
                btnStopCharge.Enabled = false;
                btnReDo.Enabled = false;
                btnFinish.Enabled = false;
                btnClearSysCarInfo.Enabled = false;
                btnClearAllTask.Enabled = false;
                btnSetCarSite.Enabled = false;
                btnBroadCast.Enabled = false;
                btnTestchazi.Enabled = false;
            }
        }

        private void btnLog_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FrmLogin frm = new FrmLogin())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                { SetAuthority(); }
            }
        }

        private void btnClearSysCarInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (XtraMessageBox.Show("确认清除车辆缓存信息?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        int agvid = -1;
                        FormPara frm = new FormPara();
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            if (string.IsNullOrEmpty(frm.AGVID)) { return; }
                            try
                            {
                                agvid = Convert.ToInt32(frm.AGVID);
                            }
                            catch
                            {
                                XtraMessageBox.Show("请输入有效车号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
                            }
                            CarInfo car = CoreData.CarList.FirstOrDefault(p => p.AgvID == agvid);
                            if (car != null)
                            {
                                //清空锁定资源集
                                List<TraJunction> CurrCarLockJuncs = CoreData.JunctionList.Where(p => p.LockCars.Count(q => q.AgvID == car.AgvID) > 0).ToList();
                                foreach (TraJunction Junc in CurrCarLockJuncs)
                                {
                                    LogHelper.WriteTrafficLog("手工清除了车" + car.AgvID.ToString() + "锁定的管制区域ID是:" + Junc.TraJunctionID.ToString() + ",车行走资源集是:" + string.Join(",", car.RouteLands.Select(p => p)) + "移除前锁定车集合数量为:" + Junc.LockCars.Count.ToString());

                                    CarInfo removeCar = Junc.LockCars.FirstOrDefault(p => p.AgvID == car.AgvID);
                                    if (removeCar != null)
                                    {
                                        Junc.LockCars.Remove(removeCar);
                                        LogHelper.WriteTrafficLog("移除后锁定车集合数量为:" + Junc.LockCars.Count.ToString());
                                    }
                                }

                                //删除车的交管所资源
                                if (XtraMessageBox.Show("是否清除该车锁定的AGV?\r\n说明:如清除掉,则被其交管的车不会自动启动,需手工启动;\r\n如不清除则该车需要重新请求线路方发送线路给当前车,根据实际情况选择。", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                { Dispacther.ClearCarLockSource(car); }


                                string filePath = System.AppDomain.CurrentDomain.BaseDirectory + @"\MemoryFile\CarTaskInfo.xml";
                                Hashtable ht = new Hashtable();
                                ht["TaskNo"] = "";
                                XMLClass.AppendXML(filePath, "N" + car.AgvID.ToString(), ht);
                                car.ExcuteTaksNo = "";
                                car.TaskDetailID = -1;
                                car.CurrSite = 0;
                                if (car.Route != null)
                                {
                                    //car.Route.Clear();
                                    //car.RealyRoute.Clear();
                                    car.RouteLands.Clear();
                                    car.TurnLands.Clear();
                                }
                                //Dispacther.DeleMemoryRoute(car.AgvID);
                                XtraMessageBox.Show("操作成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            { XtraMessageBox.Show("不存在该车辆!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                        }
                    }
                    catch (Exception ex)
                    { XtraMessageBox.Show(ex.Message); }
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }


        private void btnSetCarSite_Click(object sender, EventArgs e)
        {
            try
            {
                //int para = int.Parse(txtchazi.Text);
                //int port = int.Parse(txtChargeID.Text);
                //Dispacther.AddIOControl(para, new CommandToValue(AGVCommandEnum.Turn, port));
                using (FrmSetCurrSite frm = new FrmSetCurrSite())
                { frm.ShowDialog(); }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void btnTestchazi_Click(object sender, EventArgs e)
        {
            try
            {
                int para = int.Parse(txtchazi.Text);
                FormPara frm = new FormPara();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    int agvid = Convert.ToInt32(frm.AGVID);
                    if (XtraMessageBox.Show("确认启动当前AGV?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Dispacther.AGV_AddCommand(agvid, new CommandToValue(AGVCommandEnum.WriteRegister1, para));//1升0降
                    }
                }
            }
            catch (Exception ex)
            {
                { MessageBox.Show(ex.Message); }
            }
        }

        private void btnTestMes_Click(object sender, EventArgs e)
        {
            try
            {
                string Agvid = txtMesAgvid.Text.Trim();
                string Site = txtMesAgvSite.Text.Trim();
                if (string.IsNullOrEmpty(Agvid) || string.IsNullOrEmpty(Site))
                {
                    MessageBox.Show("请维护参数!");
                    return;
                }
                CarInfo car = new CarInfo();
                car.AgvID = Convert.ToInt16(Agvid);
                car.CurrSite = Convert.ToInt16(Site);

                string res = Dispacther.InvokWebAPI(car);
                MessageBox.Show(res);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void btnClearAllTask_Click(object sender, EventArgs e)
        {
            try
            {
                if (XtraMessageBox.Show("确认清除所有任务并复位所有小车内存信息?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                { return; }
                AGVServerDAccess.ClearTaskAndCar();
                foreach (CarInfo car in CoreData.CarList)
                {
                    string filePath = System.AppDomain.CurrentDomain.BaseDirectory + @"\MemoryFile\CarTaskInfo.xml";
                    Hashtable ht = new Hashtable();
                    ht["TaskNo"] = "";
                    XMLClass.AppendXML(filePath, "N" + car.AgvID.ToString(), ht);
                    car.ExcuteTaksNo = "";
                    car.TaskDetailID = -1;
                    if (car.Route != null)
                    {
                        car.Route.Clear();
                        car.AllRouteLandCode = "";
                        car.RealyRoute.Clear();
                        car.RouteLands.Clear();
                        car.TurnLands.Clear();
                    }
                }
                XtraMessageBox.Show("操作成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LogHelper.WriteCallBoxLog("操作员:" + (CurrentLogin.CurrentUser != null ? CurrentLogin.CurrentUser.UserName : "") + "确认清除所有任务并复位所有小车内存信息操作");
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }


        private void randomMoveAllCar()
        {

            Thread.Sleep(1000);
            try
            {
                List<LandmarkInfo> list = new List<LandmarkInfo>();
                foreach (CarInfo car in CoreData.CarList)
                {

                    RoutePlan CountRoute = new RoutePlan();


                    if (car.LowPower)
                    {
                        AddMessage(car.AgvID.ToString() + "号车低电量!");
                        continue;
                    }
                    if (car.bIsCommBreak)
                    {
                        AddMessage(car.AgvID.ToString() + "号车已掉线!");
                        continue;
                    }
                    LandmarkInfo BeginLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == car.CurrSite.ToString());
                    if (BeginLand == null)
                    {
                        AddMessage(car.AgvID.ToString() + "号车辆所在地标不存在");
                        continue;
                    }
                    Random rd = new Random((int)(DateTime.Now.Ticks / 1000));
                    LandmarkInfo EndLand = null;
                    while (true)
                    {
                        EndLand = CoreData.AllLands[rd.Next(CoreData.AllLands.Count)];
                        if (!EndLand.LandMarkName.Equals("storage"))
                        {
                            continue;
                        }
                        if (list.Contains(EndLand))
                        {
                            continue;
                        }
                        if (EndLand != BeginLand)
                        {
                            list.Add(EndLand);

                            break;
                        }
                    }


                    car.Route = CountRoute.GetRoute(BeginLand, EndLand);
                    car.AllRouteLandCode = "," + string.Join(",", car.Route.Select(p => p.LandmarkCode)) + ",";
                    if (car.Route.Count <= 0)
                    {
                        AddMessage(car.AgvID.ToString() + "号车从‘" + BeginLand.LandmarkCode + "’到‘" + EndLand.LandmarkCode + "’没有线路可走");
                        continue;
                    }

                    {

                        //在发送AGV路线命令之前需要清除一下车辆上路线缓存信息
                        ////ChargeStationInfo FreeChargeStation = CoreData.ChargeList.FirstOrDefault(p => p.ChargeLandCode == car.CurrSite.ToString());
                        ////if (FreeChargeStation != null)
                        ////{

                        ////    Dispacther.Add_EleCommand(FreeChargeStation.ID, new CommandToValue(AGVCommandEnum.StopCharge));
                        ////    car.NowChargeLandCode = "";
                        ////}
                        //Dispacther.AGV_AddCommand(car.AgvID, new CommandToValue(AGVCommandEnum.CancelTast));
                        //清除路线缓存信息后发送路线指令
                        Dispacther.SendCarRoute(car);
                    }
                    AddMessage("指定AGV" + car.AgvID.ToString() + "到目的地标" + EndLand.LandmarkCode + "去");

                }
            }
            catch (Exception ex)
            { LogHelper.WriteTrafficLog(ex.Message); }

        }

        private void randomMove_Click(object sender, EventArgs e)
        {

            randomMoveAllCar();


        }
        private void allCarComback()
        {
            Thread.Sleep(1000);

            foreach (CarInfo NoTaskCar in CoreData.CarList)
            {
                try
                {
                    LandmarkInfo BeginLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == NoTaskCar.CurrSite.ToString());
                    LandmarkInfo EndLand = CoreData.AllLands.FirstOrDefault(p => p.LandmarkCode == NoTaskCar.StandbyLandMark);
                    if (BeginLand != null && EndLand != null)
                    {
                        RoutePlan CountRoute = new RoutePlan();
                        NoTaskCar.Route = CountRoute.GetRoute(BeginLand, EndLand);
                        NoTaskCar.AllRouteLandCode = "," + string.Join(",", NoTaskCar.Route.Select(p => p.LandmarkCode)) + ",";
                        //if (NoTaskCar.Route.Count <= 0) {
                        //    AddMessage(NoTaskCar.AgvID.ToString() + "号车正在执行任务!");
                        //    continue;
                        //}
                        //if (XtraMessageBox.Show("确认启动当前AGV?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            if (NoTaskCar.bIsCommBreak)
                            {
                                AddMessage(NoTaskCar.AgvID.ToString() + "号车已掉线!");
                                continue;
                            }
                            //ChargeStationInfo FreeChargeStation = CoreData.ChargeList.FirstOrDefault(p => p.ChargeLandCode == NoTaskCar.CurrSite.ToString());
                            //if (FreeChargeStation != null)
                            //{
                            //    Dispacther.Add_EleCommand(FreeChargeStation.ID, new CommandToValue(AGVCommandEnum.StopCharge));
                            //    NoTaskCar.NowChargeLandCode = "";
                            //}

                            //清除路线缓存信息后发送路线指令
                            Dispacther.SendCarRoute(NoTaskCar);
                            AddMessage("操作员:" + (CurrentLogin.CurrentUser != null ? CurrentLogin.CurrentUser.UserName : "") + "执行了让AGV" + NoTaskCar.AgvID.ToString() + "回待命点");
                        }
                    }
                }
                catch (Exception ex)
                { }
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
            allCarComback();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (CarInfo NoTaskCar in CoreData.CarList)
            {
                Dispacther.AGV_AddCommand(NoTaskCar.AgvID, new CommandToValue(AGVCommandEnum.CancelTast));
            }

        }

        private const int storageArea = 1;
        private const int inputArea = 2;
        private const int outputArea = 3;
        private const int waitArea = 4;
        private void testMainLoop(object source , System.Timers.ElapsedEventArgs args)
        {

            
            this.Invoke(new Action(this.refreshUI));
        }

        private void refreshUI()
        {

            //模拟添加任务，装货卸货流程

            if (inputCount > 0 && this.addInputTaskImpl())
            {
                inputCount--;
            }

            if (outputCount > 0 && this.addOutputTaskImpl())
            {
                outputCount--;
            }


            StorageInfo ArmStore = CoreData.StorageList.FirstOrDefault(p => p.OwnArea == outputArea && p.LockState == 0 && p.StorageState == 2);
            if (ArmStore != null)
            {
                ArmStore.StorageState = 0;
                AGVServerDAccess.UpdateStorageState(0, 0, ArmStore.ID);
            }

            if (inputCount > 0)
            {
                ArmStore = CoreData.StorageList.FirstOrDefault(p => p.OwnArea == inputArea && p.LockState == 0 && p.StorageState == 0);
                if (ArmStore != null)
                {
                    ArmStore.StorageState = 2;
                    AGVServerDAccess.UpdateStorageState(2, 0, ArmStore.ID);
                }
            }
            

            //刷新UI

            long count = CoreData.StorageList.LongCount(p => p.OwnArea == storageArea && (p.LockState == 1 && p.StorageState == 0 || p.LockState == 0 && p.StorageState == 2));
            this.totalStorageLabel.Text = "" + CoreData.StorageList.Count;
            this.totalStorageUsedLabel.Text = "" + count;
            this.totalStorageLeafLabel.Text = "" + (CoreData.StorageList.Count - count);

            this.inputTotalLabel.Text = "" + inputTotal;
            this.inputedLabel.Text = "" + (inputTotal - inputCount);
            this.inputWaitLabel.Text = "" + inputCount;

            this.outputTotalLabel.Text = "" + outputTotal;
            this.outputedLabel.Text = "" + (outputTotal - outputCount);
            this.outputWaitLabel.Text = "" + outputCount;

        }

        //入库任务
        private bool addInputTaskImpl()
        {
            
            
            //组装任务信息
            string dispatchNo = Guid.NewGuid().ToString();
            DispatchTaskInfo TaskInfo = new DispatchTaskInfo();
            TaskInfo.dispatchNo = dispatchNo;
            TaskInfo.TaskState = 0;
            //TaskInfo.CallLand =  "";//CallStore.LankMarkCode;
            //TaskInfo.stationNo = CallBoxID;
            //TaskInfo.CallID = BtnID;
            TaskInfo.taskType = 0;

            //创建任务明细
            int DetailID = 1;
            int PrePutType = -1;
            //foreach (TaskConfigDetail item in TaskConfigDetails)
            {
                //通过任务任务配置明细寻找目标地标
                StorageInfo ArmStore = null;
                ArmStore = CoreData.StorageList.FirstOrDefault(p => p.OwnArea == inputArea && p.LockState == 0 && p.StorageState == 2);
                if (ArmStore == null)
                {
                    return false;
                }
                DispatchTaskDetail dispathDetail_TaskConfig = new DispatchTaskDetail();
                dispathDetail_TaskConfig.dispatchNo = dispatchNo;
                dispathDetail_TaskConfig.DetailID = DetailID++;
                dispathDetail_TaskConfig.LandCode = ArmStore.LankMarkCode;
                dispathDetail_TaskConfig.OperType = 0;
                dispathDetail_TaskConfig.IsAllowExcute = 1;
                dispathDetail_TaskConfig.PassType = 0;
                dispathDetail_TaskConfig.PutType = -1;
             //   dispathDetail_TaskConfig.StorageID = ArmStore.ID;
                dispathDetail_TaskConfig.IsSensorStop = 0;
                TaskInfo.TaskDetail.Add(dispathDetail_TaskConfig);
                PrePutType = ArmStore.StorageState;
            }

            {
                //通过任务任务配置明细寻找目标地标
                StorageInfo ArmStore = null;
                ArmStore = CoreData.StorageList.FirstOrDefault(p => p.OwnArea == storageArea && p.StorageState == 0 && p.LockState == 0);
                if (ArmStore == null)
                {
                    return false;
                }
                DispatchTaskDetail dispathDetail_TaskConfig = new DispatchTaskDetail();
                dispathDetail_TaskConfig.dispatchNo = dispatchNo;
                dispathDetail_TaskConfig.DetailID = DetailID++;
                dispathDetail_TaskConfig.LandCode = ArmStore.LankMarkCode;
                dispathDetail_TaskConfig.OperType = 1;
                dispathDetail_TaskConfig.IsAllowExcute = 1;
                dispathDetail_TaskConfig.PassType = 0;
                dispathDetail_TaskConfig.StorageID = ArmStore.ID;
                dispathDetail_TaskConfig.PutType = 1;
                dispathDetail_TaskConfig.IsSensorStop = 0;
                TaskInfo.TaskDetail.Add(dispathDetail_TaskConfig);
                PrePutType = ArmStore.StorageState;
            }

            //循环组装完任务信息后保存到数据库
            if (TaskInfo != null && TaskInfo.TaskDetail.Count > 0)
            {
                AGVServerDAccess.CreatTaskInfo(TaskInfo, true);
                return true;
            }
            return false;
        }


        //出库任务
        private bool addOutputTaskImpl()
        {

            //组装任务信息
            string dispatchNo = Guid.NewGuid().ToString();
            DispatchTaskInfo TaskInfo = new DispatchTaskInfo();
            TaskInfo.dispatchNo = dispatchNo;
            TaskInfo.TaskState = 0;
            //TaskInfo.CallLand =  "";//CallStore.LankMarkCode;
            //TaskInfo.stationNo = CallBoxID;
            //TaskInfo.CallID = BtnID;
            TaskInfo.taskType = 1;

            //创建任务明细
            int DetailID = 1;
            int PrePutType = -1;


            {
                //通过任务任务配置明细寻找目标地标
                StorageInfo ArmStore = null;
                ArmStore = CoreData.StorageList.FirstOrDefault(p => p.OwnArea == storageArea && p.StorageState == 2 && p.LockState == 0);
                if (ArmStore == null)
                {
                    return false;
                }
                DispatchTaskDetail dispathDetail_TaskConfig = new DispatchTaskDetail();
                dispathDetail_TaskConfig.dispatchNo = dispatchNo;
                dispathDetail_TaskConfig.DetailID = DetailID++;
                dispathDetail_TaskConfig.LandCode = ArmStore.LankMarkCode;
                dispathDetail_TaskConfig.OperType = 0;
                dispathDetail_TaskConfig.IsAllowExcute = 1;
                dispathDetail_TaskConfig.PassType = 0;
                dispathDetail_TaskConfig.PutType = 0;
                dispathDetail_TaskConfig.StorageID = ArmStore.ID;

                dispathDetail_TaskConfig.IsSensorStop = 0;
                TaskInfo.TaskDetail.Add(dispathDetail_TaskConfig);
                PrePutType = ArmStore.StorageState;
            }

            //foreach (TaskConfigDetail item in TaskConfigDetails)
            {
                StorageInfo ArmStore = null;
                ArmStore = CoreData.StorageList.FirstOrDefault(p => p.OwnArea == outputArea && p.LockState == 0 && p.StorageState == 0);
                if (ArmStore == null)
                {
                    return false;
                }
                DispatchTaskDetail dispathDetail_TaskConfig = new DispatchTaskDetail();
                dispathDetail_TaskConfig.dispatchNo = dispatchNo;
                dispathDetail_TaskConfig.DetailID = DetailID++;
                dispathDetail_TaskConfig.LandCode = ArmStore.LankMarkCode;
                dispathDetail_TaskConfig.OperType = 1;
                dispathDetail_TaskConfig.IsAllowExcute = 1;
                dispathDetail_TaskConfig.PutType = -1;
                dispathDetail_TaskConfig.PassType = 0;
                //dispathDetail_TaskConfig.StorageID = ArmStore.ID;
                dispathDetail_TaskConfig.IsSensorStop = 0;
                TaskInfo.TaskDetail.Add(dispathDetail_TaskConfig);
                PrePutType = ArmStore.StorageState;
            }

            //循环组装完任务信息后保存到数据库
            if (TaskInfo != null && TaskInfo.TaskDetail.Count > 0)
            {
                AGVServerDAccess.CreatTaskInfo(TaskInfo, true);
                return true;
            }

            return false;
        }

        private void tabNavigationPage6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gcCars_Click(object sender, EventArgs e)
        {

        }


        private int inputCount = 0;
        private int outputCount = 0;
        private int inputTotal = 0;
        private int outputTotal = 0;

        //添加入库任务
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            FormPara frm = new FormPara();
            frm.labelControl2.Text = "入库量:";
            if (frm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    int count = Convert.ToInt32(frm.AGVID);
                    if(count > 0 && count <50)
                    {
                        inputCount += count;
                        inputTotal += count;
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        //添加出库任务
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            FormPara frm = new FormPara();
            frm.labelControl2.Text = "出库量:";
            if (frm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    int count = Convert.ToInt32(frm.AGVID);
                    if (count > 0 && count < 50)
                    {
                        outputCount += count;
                        outputTotal += count;
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }
        }

        //开始模拟任务
        private void startTest_Click(object sender, EventArgs e)
        {
            
            if (CurrentLogin.CurrentUser == null)
            {
                this.btnLog_ItemClick(null,null);
                return;
            }

            this.startTest.Enabled = false;
            this.checkBox1.Enabled = true;
            this.checkBox2.Enabled = true;
            this.checkBox3.Enabled = true;
            this.checkBox4.Enabled = true;
            this.checkBox5.Enabled = true;
            this.addInputTask.Enabled = true;
            this.addOutputTask.Enabled = true;

            System.Timers.Timer t = new System.Timers.Timer(2000);

            t.Elapsed += new System.Timers.ElapsedEventHandler(testMainLoop);

            t.AutoReset = true;

            t.Enabled = true;

        }


        private int enabledCar(CheckBox check, int index)
        {
            if(index > CoreData.CarList.Count())
            {
                return 0;
            }

            CarInfo car = CoreData.CarList[index];
            if (check.Checked)
            {
                car.OwerArea = 0;
                return 1;
            }
            else
            {
                car.OwerArea = waitArea;
                return 0;
            }
        }
        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            enabledCar(box, Convert.ToInt32(box.Name.Replace("checkBox",""))-1);
        }
    }
}
