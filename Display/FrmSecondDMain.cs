using AGVDAccess;
using Canvas;
using Canvas.CanvasCtrl;
using Canvas.CanvasInterfaces;
using Canvas.DrawTools;
using Canvas.Layers;
using DevExpress.LookAndFeel;
using DevExpress.Utils;
using Model.CarInfoExtend;
using Model.MDM;
using SocketClient;
using SocketModel;
using SQLServerOperator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools;

namespace Display
{
    public partial class FrmSecondDMain : BaseForm, ICanvasOwner
    {
        #region 全局变量
        CanvasCtrller m_canvas;
        DataModel m_data;
        string m_filename = string.Empty;
        #endregion

        #region 构造函数
        public FrmSecondDMain()
        {
            InitializeComponent();
            this.Text = "AGV调度系统实时显示";
            bool isSucc = true;
            try
            {
                ConnectConfigTool.setDBase();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message + "请检查...");
                isSucc = false;
            }
            if (ConnectConfigTool.DBase == null || !isSucc)
            {
                //弹出维护数据库维护界面
                using (FrmSysConnSet frm = new FrmSysConnSet())
                {
                    if (frm.ShowDialog() != DialogResult.OK)
                    {
                        Application.ExitThread();
                        Application.Exit();
                        return;
                    }
                }
            }
            using (WaitDialogForm dialog = new WaitDialogForm("正在启动,请稍后...", "提示"))
            {
                InitCanvas("", false);
            }
        }
        #endregion

        #region 画布方法
        private void InitCanvas(string filename, bool IsNew)
        {
            if (!IsNew)
            {
                try
                {
                    AGVDAccess.AGVClientDAccess.GetPlanSet();
                    string tempFile = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\temSet.agv";
                    if (File.Exists(tempFile))
                    { filename = tempFile; }
                }
                catch (Exception ex)
                { MsgBox.ShowError(ex.Message); }
            }
            try
            {
                m_data = new DataModel();
                if (filename.Length > 0 && File.Exists(filename) && m_data.Load(filename, null))
                { m_filename = filename; }
                m_canvas = new CanvasCtrller(this, m_data);
                m_canvas.Dock = DockStyle.Fill;
                pclMain.Controls.Clear();
                pclMain.Controls.Add(m_canvas);
                m_canvas.SetCenter(new UnitPoint(0, 0));
                m_canvas.IsChooseSpecial = false;


                string storageColorPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\StorageColor.txt";
                //加载记忆画布背景颜色
                if (File.Exists(storageColorPath))
                {
                    Hashtable hs = XMLClass.GetXMLByParentNode(storageColorPath, "StorageColor");
                    if (hs["BackGroundColor"] != null && !string.IsNullOrEmpty(hs["BackGroundColor"].ToString()))
                    {
                        string[] bgColor = hs["BackGroundColor"].ToString().Split(',');
                        Color BackGroundColor = Color.FromArgb(Convert.ToInt16(bgColor[0]), Convert.ToInt16(bgColor[1]), Convert.ToInt16(bgColor[2]));
                        if (m_canvas != null)
                        {
                            BackgroundLayer layer = m_canvas.Model.BackgroundLayer as BackgroundLayer;
                            layer.Color = BackGroundColor;
                        }
                    }
                }

            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        //显示选择元素的信息
        public void SetPositionInfo(UnitPoint unitpos)
        { }
        //显示选择的元素的聚焦点信息
        public void SetSnapInfo(ISnapPoint snap)
        { }

        #endregion

        #region 窗体事件
        private void FrmSecondDMain_Shown(object sender, EventArgs e)
        {
            try
            {
                Connect();
                UpdateStock();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// 连接参数设置
        /// </summary>
        private void btnConnetSet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //弹出维护数据库维护界面
                using (FrmSysConnSet frm = new FrmSysConnSet())
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        if (MsgBox.ShowQuestion("需要重启生效,是否重新启动?") != DialogResult.Yes)
                        {
                            Application.Restart();
                        }
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void FrmSecondDMain_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Shift && e.KeyCode == Keys.S)
                {
                    this.bar1.Visible = !this.bar1.Visible;
                }
                else if (e.Control && e.KeyCode == Keys.F)
                {
                    using (FrmFindLand frm = new FrmFindLand())
                    {
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            using (WaitDialogForm dialog = new WaitDialogForm("正在查找,请稍后...", "提示"))
                            {

                                IDrawObject land = (from a in m_data.ActiveLayer.Objects
                                                    where a.Id == "LandMark" && (a as LandMarkTool).LandCode == frm.LandCode
                                                    select a).FirstOrDefault();
                                if (land != null)
                                {
                                    (land as LandMarkTool).Selected = true;
                                }
                                else
                                {
                                    MsgBox.ShowWarn("未找到地标号" + frm.LandCode + "!");
                                    return;
                                }
                                m_data.AddSelectedObject(land);
                                m_canvas.DoInvalidate(true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void FrmSecondDMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (MsgBox.ShowQuestion("确定退出当前系统?") != DialogResult.Yes)
                { e.Cancel = true; return; }
                string RemeberFilePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\SkinFile.txt";
                if (File.Exists(RemeberFilePath))
                { File.Delete(RemeberFilePath); }
                File.AppendAllText(RemeberFilePath, UserLookAndFeel.Default.ActiveSkinName);

                string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\StorageColor.txt";
                BackgroundLayer layer = m_canvas.Model.BackgroundLayer as BackgroundLayer;
                Color BackGroundColor = layer.Color;
                Hashtable hs = new Hashtable();
                hs["BackGroundColor"] = BackGroundColor.R + "," + BackGroundColor.G + "," + BackGroundColor.B;
                XMLClass.AppendXML(path, "StorageColor", hs);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Close();
                return;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        /// <summary>
        /// 显示信息位置发生变化时
        /// </summary>
        private void dockPanel1_DockChanged(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraBars.Docking.DockPanel dockpnel = sender as DevExpress.XtraBars.Docking.DockPanel;
                if (dockpnel != null && (dockpnel.Dock == DevExpress.XtraBars.Docking.DockingStyle.Left || dockpnel.Dock == DevExpress.XtraBars.Docking.DockingStyle.Right))
                { spilWarm.Horizontal = false; }
                else
                { spilWarm.Horizontal = true; }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }
        #endregion

        #region 启动实时显示
        private IList<CarInfo> agvs = new List<CarInfo>();//所有的车辆信息集合
        private IList<LandmarkInfo> AllLands = new List<LandmarkInfo>();//所有地标档案信息
        System.Windows.Forms.Timer FreshDate_Timer = new System.Windows.Forms.Timer();
        private TcpClientSever clientserver = new TcpClientSever();//客户端通信对象
        DateTime UpRecTime;//上次接受到服务端心跳的时间
        private Thread CONTEST_Thread;//心跳线程


        //连接服务端函数
        private void Connect()
        {
            try
            {
                FreshDate_Timer.Enabled = true;
                FreshDate_Timer.Interval = 5000;
                FreshDate_Timer.Tick += FreshDate_Timer_Tick;
                //连接通信
                bool IsConnSuccess = false;
                using (WaitDialogForm dialog = new WaitDialogForm("正在通信,可能需要数秒,请稍后...", "提示"))
                {
                    IsConnSuccess = clientserver.Setup(ConnectConfigTool.serverconfig);
                    if (IsConnSuccess)
                    { IsConnSuccess = clientserver.Start(); }
                }
                if (!IsConnSuccess)
                {
                    //this.Text = "AGV调度系统实时显示---连接服务器失败!";
                    return;
                }
                clientserver.RecvSuccess += this.RecvSuccess;
                //向服务端请求所有车辆当前实时信息
                SuperSocketMsg Message = new SuperSocketMsg("", SocketCommand.AllCarsInfo);
                clientserver.SendMessage(Message);
                //向服务端发送心跳
                CONTEST_Thread = new Thread(SendCONTEST);
                CONTEST_Thread.IsBackground = true;
                CONTEST_Thread.Start();
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }


        //定时发送心跳包
        private void SendCONTEST()
        {
            try
            {
                while (true)
                {
                    SuperSocketMsg Message = new SuperSocketMsg("", SocketCommand.CONTEST);
                    clientserver.SendMessage(Message);
                    Thread.Sleep(1000);
                }
            }
            catch
            { }
        }

        /// <summary>
        /// 定时刷新界面储位信息
        /// </summary>
        private void FreshDate_Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                new Thread(new ThreadStart(UpdateStock)) { IsBackground = true }.Start();
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 处理接受到服务端的信息
        /// </summary>
        /// <param name="sendr"></param>
        /// <param name="package"></param>
        private void RecvSuccess(object sendr, PackageInfo package)
        {
            try
            {
                Task.Run(() =>
                {
                    switch (package.Command)
                    {
                        case SocketCommand.AllCarsInfo://初始化接收所有小车信息
                            IList<CarInfo> Cars = JosnTool.ParseFormJson<IList<CarInfo>>(package.PackContent);
                            if (Cars == null) { return; }
                            CarInit(Cars);
                            //new Thread(new ParameterizedThreadStart(cc)) { IsBackground = true }.Start(Cars);
                            UpRecTime = DateTime.Now;
                            break;
                        case SocketCommand.OneCarInfo://获取到某辆小车的当前状态信息
                            CarInfo AGV = JosnTool.ParseFormJson<CarInfo>(package.PackContent);
                            if (AGV == null) { return; }
                            //实时显示车辆当前位置
                            stepCar(AGV);
                            //new Thread(new ParameterizedThreadStart(stepCar)) { IsBackground = true }.Start(AGV);
                            //刷新异常提示信息
                            FreshWarnMessage(AGV);
                            //new Thread(new ParameterizedThreadStart(FreshWarnMessage)) { IsBackground = true }.Start(AGV);
                            UpRecTime = DateTime.Now;
                            break;
                        case SocketCommand.CONTEST:
                            UpRecTime = DateTime.Now;
                            break;
                        default:
                            break;
                    }
                });
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 重新连接调度服务端
        /// </summary>
        private void ReConnect()
        {
            try
            {
                DateTime now = DateTime.Now;
                TimeSpan timeSpan = now - UpRecTime;
                if (timeSpan.TotalSeconds > 3)
                {
                    if (!this.IsHandleCreated) { return; }
                    //this.Invoke((EventHandler)(delegate
                    //{
                    //    this.Text = "AGV调度系统实时显示---正在重连服务器...";
                    //}));
                    if (clientserver != null)
                    {
                        clientserver.Exit();
                        clientserver = null;
                    }
                    clientserver = new TcpClientSever();//客户端通信对象
                    clientserver.RecvSuccess += this.RecvSuccess;
                    clientserver.Setup(ConnectConfigTool.serverconfig);
                    clientserver.Start();
                    SuperSocketMsg Message = new SuperSocketMsg("", SocketCommand.AllCarsInfo);
                    clientserver.SendMessage(Message);
                    //向服务端发送心跳
                    if (this.CONTEST_Thread != null)
                    { CONTEST_Thread.Abort(); }
                    CONTEST_Thread = new Thread(SendCONTEST);
                    CONTEST_Thread.IsBackground = true;
                    CONTEST_Thread.Start();
                }
                else
                {
                    if (!this.IsHandleCreated) { return; }
                    //this.Invoke((EventHandler)(delegate
                    //{
                    //    this.Text = "AGV调度系统实时显示";
                    //}));
                }
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void CarInit(object obj)
        {
            try
            {
                if (obj == null) { return; }
                if (!this.m_canvas.IsHandleCreated) { return; }
                this.m_canvas.Invoke((EventHandler)(delegate
                {
                    IList<CarInfo> Cars = obj as IList<CarInfo>;
                    agvs.Clear();
                    AllLands.Clear();
                    AllLands = AGVDAccess.AGVClientDAccess.LoadLandByCondition("1=1");
                    List<IDrawObject> agvobjs = m_data.ActiveLayer.Objects.Where(p => p.Id == "AGVTool").ToList();
                    m_data.DeleteObjects(agvobjs);
                    foreach (CarInfo carinfo in Cars)
                    {
                        AGVTool agv = new AGVTool();
                        agv.HandState = carinfo.JCState;
                        agv.Agv_id = carinfo.AgvID.ToString();
                        //LandmarkInfo CurLand = AllLands.Where(p => p.LandmarkCode == carinfo.CurrSite.ToString()).FirstOrDefault();
                        //if (CurLand != null)
                        //{ agv.Position = new UnitPoint(CurLand.LandX, CurLand.LandY); }
                        //else
                        //{ agv.Position = new UnitPoint(carinfo.X, carinfo.Y); }
                        agv.Position = new UnitPoint(carinfo.X, carinfo.Y);
                        if (carinfo.bIsCommBreak)
                        { agv.IsViewable = false; }
                        else
                        { agv.IsViewable = true; }
                        m_data.AddObject(m_data.ActiveLayer, agv);
                        m_canvas.DoInvalidate(true);
                        agvs.Add(carinfo);
                    }
                    bsCar.DataSource = agvs;
                    bsCar.ResetBindings(false);
                }));
            }
            catch (Exception ex)
            { }
        }


        private void stepCar(object obj)
        {
            try
            {
                if (obj == null) { return; }
                if (!this.m_canvas.IsHandleCreated) { return; }
                this.m_canvas.Invoke((EventHandler)(delegate
                {
                    CarInfo car = obj as CarInfo;
                    if (agvs.Count <= 0) { return; }
                    IEnumerable<IDrawObject> objects = from a in m_data.ActiveLayer.Objects
                                                       where a.Id == "AGVTool" && (a as AGVTool).Agv_id == car.AgvID.ToString()
                                                       select a;
                    if (objects != null && objects.Count() > 0)
                    {
                        AGVTool agv = objects.FirstOrDefault() as AGVTool;
                        if (agv != null)
                        {
                            if (car.bIsCommBreak)
                            { agv.IsViewable = false; }
                            else
                            {
                                agv.IsViewable = true;
                                agv.Position = new UnitPoint(car.X, car.Y);
                                agv.HandState = car.JCState;
                                agv.Agv_id = car.AgvID.ToString();
                                //agv.Move(new UnitPoint(agv.Position.X - car.X, agv.Position.Y - car.Y));
                                //LandmarkInfo CurLand = AllLands.Where(p => p.LandmarkCode == car.CurrSite.ToString()).FirstOrDefault();
                                //if (CurLand == null)
                                //{
                                //    agv.Position = new UnitPoint(car.X, car.Y);
                                //    return;
                                //}
                                //else
                                //{
                                //    agv.Position = new UnitPoint(CurLand.LandX, CurLand.LandY);
                                //}
                            }
                            m_canvas.DoInvalidate(true);
                        }
                    }
                    else if (car != null)
                    {
                        //LandmarkInfo CurLand = AllLands.Where(p => p.LandmarkCode == car.CurrSite.ToString()).FirstOrDefault();
                        //if (CurLand == null) { return; }
                        AGVTool agv = new AGVTool();
                        agv.HandState = car.JCState;
                        agv.Agv_id = car.AgvID.ToString();
            
                        agv.Position = new UnitPoint(car.X, car.Y);
                        if (car.bIsCommBreak)
                        { agv.IsViewable = false; }
                        else
                        { agv.IsViewable = true; }
                        m_data.AddObject(m_data.ActiveLayer, agv);
                        m_canvas.DoInvalidate(true);
                    }
                    //更新一下AGV集合中的对应车辆信息
                    CarInfo CurrentCar = agvs.FirstOrDefault(p => p.AgvID == car.AgvID);
                    if (CurrentCar != null)
                    {
                        CurrentCar.bIsCommBreak = car.bIsCommBreak;
                        CurrentCar.CarState = car.CarState;
                        CurrentCar.ExcuteTaksNo = car.ExcuteTaksNo;
                        CurrentCar.CurrSite = car.CurrSite;
                        CurrentCar.fVolt = car.fVolt;
                        CurrentCar.LowPower = car.LowPower;
                        CurrentCar.TaskDetailID = car.TaskDetailID;
                        bsCar.DataSource = agvs;
                        bsCar.ResetBindings(false);
                    }
                }));
            }
            catch (Exception ex)
            { }
        }

        private void FreshWarnMessage(object obj)
        {
            try
            {
                if (obj == null) { return; }
                if (!this.txtWarnMess.IsHandleCreated) { return; }
                this.txtWarnMess.Invoke((EventHandler)(delegate
                {
                    CarInfo car = obj as CarInfo;
                    CarInfo agv = agvs.Where(P => P.AgvID == car.AgvID).FirstOrDefault();
                    agv.bIsCommBreak = car.bIsCommBreak;
                    agv.ErrorMessage = car.ErrorMessage;
                    agv.CarState = car.CarState;
                    agv.ExcuteTaksNo = car.ExcuteTaksNo;
                    txtWarnMess.Text = "";
                    foreach (CarInfo itemagv in agvs)
                    {
                        if (!string.IsNullOrEmpty(itemagv.ErrorMessage))
                        {
                            string Mess_Text = itemagv.AgvID + "号车:" + itemagv.ErrorMessage;
                            txtWarnMess.Text += Mess_Text;
                        }
                    }
                }));
            }
            catch
            { }
        }

        //更新储位信息
        private void UpdateStock()
        {
            try
            {
                this.Invoke((EventHandler)(delegate
                {
                    IList<StorageInfo> all_stocks = AGVClientDAccess.LoadStorages();
                    if (all_stocks != null)
                    {
                        foreach (StorageInfo item in all_stocks)
                        {
                            StorageTool storage = m_data.ActiveLayer.Objects.Where(p => p.Id == "StorageTool" && (p as StorageTool).StcokID == item.ID).FirstOrDefault() as StorageTool;
                            if (storage != null)
                            {
                                storage.OwnArea = item.OwnArea;
                                storage.SubOwnArea = item.SubOwnArea;
                                storage.matterType = item.matterType;
                                storage.StorageState = item.StorageState;
                                storage.LockState = item.LockState;
                                storage.MaterielType = item.MaterielType;
                            }
                        }
                    }
                    m_canvas.DoInvalidate(true);
                }));
                ClearMemory();
                ReConnect();
            }
            catch (Exception ex)
            { }
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

        private void btnOption_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                BackgroundLayer layer = m_canvas.Model.BackgroundLayer as BackgroundLayer;
                GridLayer Grid = m_canvas.Model.GridLayer as GridLayer;
                DrawingLayer Drw = m_canvas.Model.ActiveLayer as DrawingLayer;
                Color nullStorageColor = m_canvas.Model.NullStorageColor;
                Color emptyShelfStorageColor = m_canvas.Model.EmptyShelfStorageColor;
                Color fullShelfStorageColor = m_canvas.Model.FillShelfStorageColor;
                if (layer != null && Grid != null && Drw != null)
                {
                    using (FrmOption frm = new FrmOption(Grid.Enabled, Grid.GridStyle, Grid.Color, layer.Color, Drw.Color, nullStorageColor, emptyShelfStorageColor, fullShelfStorageColor, Drw.Width))
                    {
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            Grid.Enabled = frm.GridEnable;
                            Grid.GridStyle = frm.GridStyle;
                            Grid.Color = frm.GridColor;
                            layer.Color = frm.BackGroudColor;
                            Drw.Color = frm.PenColor;
                            Drw.Width = frm.PenWidth;
                            m_canvas.Model.NullStorageColor = frm.NullStorageColor;
                            m_canvas.Model.EmptyShelfStorageColor = frm.EmptyShelfStorageColor;
                            m_canvas.Model.FillShelfStorageColor = frm.FullShelfStorageColor;
                            m_canvas.DoInvalidate(true);
                        }
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }
    }
}
