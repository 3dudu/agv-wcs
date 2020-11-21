using AGVDAccess;
using Canvas;
using Canvas.CanvasCtrl;
using Canvas.CanvasInterfaces;
using Canvas.DrawTools;
using Canvas.EditTools;
using Canvas.Layers;
using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraNavBar;
using Model.Comoon;
using Model.MDM;
using Model.MSM;
using Simulation.SimulationCommon;
using Simulation.SimulationExternalForm;
using SimulationModel;
using SQLServerOperator;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools;
using static Canvas.DrawTools.Arc3Point;
using DrawTools = Canvas.DrawTools;

namespace Simulation.SimulationSysForm
{
    public partial class FrmMain : BaseForm, ICanvasOwner, IEditToolOwner
    {
        #region 全局变量
        CanvasCtrller m_canvas;
        DataModel m_data;
        string m_filename = string.Empty;
        bool IsSaveStore = false;
        static object LockObj = new object();


        #endregion
        public FrmMain()
        {
            InitializeComponent();
            pnlChoose.Height = 0;
            pnlChoose.Visible = false;
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
            try
            {
                ConnectConfigTool.setDBase();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message + "请检查...");
                return;
            }
            using (WaitDialogForm dialog = new WaitDialogForm("正在启动,请稍后...", "提示"))
            {
                InitCanvas("", false);
            }
        }

        #region 画布方法

        private void InitCanvas(string filename, bool IsNew)
        {
            bool IsConnectDB = false;
            try
            {
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                IsConnectDB = dbOperator.ServerIsThrough();
            }
            catch (Exception ex)
            { /*MsgBox.ShowError(ex.Message);*/ }
            if (!IsNew && IsConnectDB)
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
                if (filename.Length > 0 && File.Exists(filename) && m_data.Load(filename,null))
                { m_filename = filename; }
                m_canvas = new CanvasCtrller(this, m_data);
                m_canvas.Dock = DockStyle.Fill;
                pnlMain.Controls.Clear();
                pnlMain.Controls.Add(m_canvas);
                m_canvas.SetCenter(new UnitPoint(0, 0));
                m_canvas.RunningSnaps = new Type[]
                    {
                typeof(VertextSnapPoint),
                typeof(MidpointSnapPoint),
                typeof(IntersectSnapPoint),
                typeof(QuadrantSnapPoint),
                typeof(CenterSnapPoint),
                typeof(DivisionSnapPoint),
                    };
                m_canvas.KeyDown += new KeyEventHandler(OnCanvasKeyDown);
                m_canvas.MouseUp += new MouseEventHandler(OnCanvasMouseUp);
                m_canvas.MouseDoubleClick += new MouseEventHandler(OnCanvasMouseDoubleClick);
                SetupDrawTools();
                SetupEditTools();
                UpdateLayerUI();
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }


        //工具选择事件
        private void OnToolSelect(object sender, NavBarLinkEventArgs e)
        {
            string toolid = string.Empty;
            bool fromKeyboard = false;
            if (sender is NavBarItem)
            {
                toolid = ((NavBarItem)sender).Tag.ToString();
                fromKeyboard = true;
            }
            if (toolid == "select")
            {
                m_canvas.CommandEscape();
                return;
            }
            if (toolid == "pan")
            {
                m_canvas.CommandPan();
                return;
            }
            if (toolid == "move")
            {
                m_canvas.CommandMove(fromKeyboard);
                return;
            }
            m_canvas.CommandSelectDrawTool(toolid);
        }

        /// <summary>
        /// 编辑动作事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEditToolSelect(object sender, NavBarLinkEventArgs e)
        {
            string toolid = string.Empty;
            if (sender is NavBarItem)
            {
                toolid = ((NavBarItem)sender).Tag.ToString();
            }
            m_canvas.CommandEdit(toolid);
        }

        //画布键盘事件
        private void OnCanvasKeyDown(object sender, KeyEventArgs e)
        {

        }


        private void SetupDrawTools()
        {
            btnLine.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnLine.Tag = "Line";
            m_data.AddDrawTool(btnLine.Tag.ToString(), new LineTool(LineType.Line));
            btnPointLine.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnPointLine.Tag = "PointLine";
            m_data.AddDrawTool(btnPointLine.Tag.ToString(), new LineTool(LineType.PointLine));
            btnImg.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnImg.Tag = "imgTool";
            m_data.AddDrawTool(btnImg.Tag.ToString(), new ImgeTool());
            btntxt.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btntxt.Tag = "txtTool";
            m_data.AddDrawTool(btntxt.Tag.ToString(), new TextTool());
            btnlandmark.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnlandmark.Tag = "LandMark";
            m_data.AddDrawTool(btnlandmark.Tag.ToString(), new LandMarkTool());
            btnBesize.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnBesize.Tag = "BezierTool";
            m_data.AddDrawTool(btnBesize.Tag.ToString(), new BezierTool(BezierType.Bezier));
            btnPointBesize.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnPointBesize.Tag = "PointBezier";
            m_data.AddDrawTool(btnPointBesize.Tag.ToString(), new BezierTool(BezierType.PointBezier));
            btnStock.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnStock.Tag = "StorageTool";
            m_data.AddDrawTool(btnStock.Tag.ToString(), new StorageTool());
            btnCallBox.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnCallBox.Tag = "ButtonTool";
            m_data.AddDrawTool(btnCallBox.Tag.ToString(), new ButtonTool());
            btnselect.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);

            btnselect.Tag = "select";
            btnHand.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnHand.Tag = "pan";
            btnMove.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnMove.Tag = "move";
        }

        private void OnCanvasMouseUp(object sender, MouseEventArgs e)
        {
            luArea.EditValueChanged -= luArea_EditValueChanged;
            luMaterialInfo.EditValueChanged -= luMaterialInfo_EditValueChanged;
            try
            {
                this.Property.SelectedObject = null;
                this.Property.UpdateRows();
                if (this.m_data.SelectedCount > 0)
                {
                    this.Property.SelectedObject = m_data.SelectedObjects.FirstOrDefault();
                    StorageTool curr = m_data.SelectedObjects.FirstOrDefault() as StorageTool;
                    if (curr != null)
                    {
                        this.Property.SelectedObject = curr;
                        luArea.EditValue = curr.OwnArea;
                        luMaterialInfo.EditValue = curr.MaterielType;
                        this.pnlChoose.Visible = true;
                        this.pnlChoose.Height = 98;
                    }
                    else
                    {
                        this.pnlChoose.Visible = false;
                        this.pnlChoose.Height = 0;
                    }
                }
                else
                {
                    this.pnlChoose.Visible = false;
                    this.pnlChoose.Height = 0;
                }
                UpdateLayerUI();
                List<IDrawObject> LandTools = m_data.ActiveLayer.Objects.Where(p => p.Id == "LandMark" || p.Id == "AGVTool").ToList();
                m_data.DeleteObjects(LandTools);
                List<IDrawObject> Lands = LandTools.Where(p => p.Id == "LandMark").ToList();
                foreach (IDrawObject item in Lands)
                { m_data.AddObject(m_data.ActiveLayer, item); }
                List<IDrawObject> agvs = LandTools.Where(p => p.Id == "AGVTool").ToList();
                foreach (IDrawObject item in agvs)
                { m_data.AddObject(m_data.ActiveLayer, item); }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
            finally
            {
                luArea.EditValueChanged += luArea_EditValueChanged;
                luMaterialInfo.EditValueChanged += luMaterialInfo_EditValueChanged;
            }
        }

        private void OnCanvasMouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (m_data.SelectedObjects.Count() == 1)
                {
                    IDrawObject obj = m_data.SelectedObjects.FirstOrDefault();
                    if (obj != null && obj.Id == "ButtonTool" && IsRunninSimula)
                    {
                        using (FrmSimulationCallBox frm = new FrmSimulationCallBox((obj as ButtonTool).CallBoxID, simula))
                        {
                            frm.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void Property_CellValueChanging(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
        {
            if (e.Row.Properties.Caption == "MarkValue")
            {
                int count = m_data.ActiveLayer.Objects.Where(p => p.Id == "LandMark" && (p as LandMarkTool).LandCode == e.Value.ToString()).Count();
                if (count > 0)
                {
                    this.lblMaxLand.Caption = "地标号:" + e.Value.ToString() + "已存在!";
                }
                else
                { this.lblMaxLand.Caption = ""; }
            }
        }

        private void Property_CellValueChanged(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
        {
            try
            {
                m_canvas.Focus();
                UpdateLayerUI();
                //m_canvas.DoInvalidate(true);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void UpdateLayerUI()
        {
            string MaxValue = "1";
            if (m_data.ActiveLayer.Objects.Count() > 0)
            {
                if (m_data.ActiveLayer.Objects.Where(P => P.Id == "LandMark").Count() > 0)
                {
                    MaxValue = m_data.ActiveLayer.Objects.Where(p => p.Id == "LandMark").Max(p => Convert.ToDecimal((p as DrawTools.LandMarkTool).LandCode)).ToString();
                }
            }
            this.lblCoorateInfo.Caption = "当前最大地标号:" + MaxValue;
            m_canvas.DoInvalidate(true);
        }

        //撤销
        void OnUndo(object sender, System.EventArgs e)
        {
            if (m_data.DoUndo())
                m_canvas.DoInvalidate(true);
        }

        //恢复
        void OnRedo(object sender, System.EventArgs e)
        {
            if (m_data.DoRedo())
                m_canvas.DoInvalidate(true);
        }


        public void SetHint(string text)
        {
            this.lblCoorateInfo.Caption = text;
        }

        void UpdateData()
        {
            m_data.CenterPoint = m_canvas.GetCenter();
        }

        void SetupEditTools()
        {
            m_data.AddEditTool("meet2lines", new LinesMeetEditTool(this));
            m_data.AddEditTool("shrinkextend", new LineShrinkExtendEditTool(this));
        }

        public void SetPositionInfo(UnitPoint unitpos)
        {

        }
        public void SetSnapInfo(ISnapPoint snap)
        {

        }
        #endregion

        #region 窗体事件
        private void FrmMain_Shown(object sender, EventArgs e)
        {
            try
            {
                bool IsConnectDB = false;
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                IsConnectDB = dbOperator.ServerIsThrough();
                if (IsConnectDB)
                {
                    IList<AreaInfo> AllAreas = AGVClientDAccess.LoadAllArea();
                    this.bsAreaInfo.DataSource = AllAreas;
                    this.bsAreaInfo.ResetBindings(false);

                    IList<MaterialInfo> AllMaterial = AGVClientDAccess.LoadAllMaterial();
                    this.bsMaterialInfo.DataSource = AllMaterial;
                    this.bsMaterialInfo.ResetBindings(false);
                    //更新界面储位
                    new Thread(new ThreadStart(UpdateStock)) { IsBackground = true }.Start();
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnSysConnSet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FrmSysConnSet frm = new FrmSysConnSet())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        InitCanvas("", false);
                        FrmMain_Shown(null, null);
                    }
                    catch (Exception ex)
                    { MsgBox.ShowError(ex.Message); }
                }
            }
        }

        private void btnSysSet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FrmSystemConfig frm = new FrmSystemConfig())
            { frm.ShowDialog(); }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (MsgBox.ShowQuestion("确认保存当前地图?") == DialogResult.Yes)
                {
                    IList<IDrawObject> StocksDraws = m_data.ActiveLayer.Objects.Where(P => P.Id == "StorageTool").ToList<IDrawObject>();
                    if (StocksDraws.Count > 0 && MsgBox.ShowQuestion("是否保存更新储位信息?") == DialogResult.Yes)
                    { IsSaveStore = true; }
                    else
                    { IsSaveStore = false; }
                    Save();
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnOpenFile_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "Cad XML files (*.agv)|*.agv";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    InitCanvas(dlg.FileName, true);
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                InitCanvas("", true);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnSaveAs_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                SaveAs();
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnExit_itemclick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (MsgBox.ShowQuestion("确定退出当前系统?") == DialogResult.Yes)
                {
                    Application.ExitThread();
                    Application.Exit();
                    return;
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnReExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                btnExit_itemclick(null, null);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnOption_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                BackgroundLayer layer = m_canvas.Model.BackgroundLayer as BackgroundLayer;
                GridLayer Grid = m_canvas.Model.GridLayer as GridLayer;
                DrawingLayer Drw = m_canvas.Model.ActiveLayer as DrawingLayer;
                if (layer != null && Grid != null && Drw != null)
                {
                    using (FrmOption frm = new FrmOption(Grid.Enabled, Grid.GridStyle, Grid.Color, layer.Color, Drw.Color, Drw.Width))
                    {
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            Grid.Enabled = frm.GridEnable;
                            Grid.GridStyle = frm.GridStyle;
                            Grid.Color = frm.GridColor;
                            layer.Color = frm.BackGroudColor;
                            Drw.Color = frm.PenColor;
                            Drw.Width = frm.PenWidth;
                            m_canvas.DoInvalidate(true);
                        }
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnCoorNateSet_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (FrmDirectionSet frm = new FrmDirectionSet())
                { frm.ShowDialog(); }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnAreaset_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (FrmAreaSet frm = new FrmAreaSet())
                { frm.ShowDialog(); }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnGoodsSet_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (FrmMaterialSet frm = new FrmMaterialSet())
                { frm.ShowDialog(); }
                bool IsConnectDB = false;
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                IsConnectDB = dbOperator.ServerIsThrough();
                if (IsConnectDB)
                {
                    IList<MaterialInfo> AllMaterial = AGVClientDAccess.LoadAllMaterial();
                    this.bsMaterialInfo.DataSource = AllMaterial;
                    this.bsMaterialInfo.ResetBindings(false);
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnActionsSet_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnCallInfoSet_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (FrmCallBoxSet frm = new FrmCallBoxSet())
                { frm.ShowDialog(); }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnConfigInfoSet_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (FrmTaskConfigInfo frm = new FrmTaskConfigInfo())
                { frm.ShowDialog(); }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnStart_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (WaitDialogForm dialog = new WaitDialogForm("正在启动,请稍后...", "提示"))
                {
                    m_canvas.IsChooseSpecial = true;
                    this.dockPanel1.Enabled = false;
                    this.dockPanel2.Enabled = false;
                    btnFile.Enabled = false;
                    btnSysConfig.Enabled = false;
                    btnExternel.Enabled = false;
                    btnExternelInfo.Enabled = false;
                    IniMoni();
                    IsRunninSimula = true;
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnStop_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (simula == null)
                { return; }
                using (WaitDialogForm dialog = new WaitDialogForm("正在停止,请稍后...", "提示"))
                {

                    FreshDate_Timer.Enabled = false;
                    FreshDate_Timer.Enabled = false;
                    m_canvas.IsChooseSpecial = false;
                    this.dockPanel1.Enabled = true;
                    this.dockPanel2.Enabled = true;
                    btnFile.Enabled = true;
                    btnSysConfig.Enabled = true;
                    btnExternel.Enabled = true;
                    btnExternelInfo.Enabled = true;
                    simula.StopSimula();
                    IsRunninSimula = false;
                    IEnumerable<IDrawObject> objects = from a in m_data.ActiveLayer.Objects
                                                       where a.Id == "AGVTool"
                                                       select a;
                    int AgvToolCount = objects.Count();
                    while (AgvToolCount > 0)
                    {
                        m_data.DeleteObjects(objects);
                        m_canvas.DoInvalidate(true);
                        objects = from a in m_data.ActiveLayer.Objects
                                  where a.Id == "AGVTool"
                                  select a;
                        AgvToolCount = objects.Count();
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnPointToPoint_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!IsRunninSimula)
                {
                    MsgBox.ShowWarn("请先启动!");
                    return;
                }
                if (simula != null)
                {
                    using (FrmTestPointToPoint frm = new FrmTestPointToPoint())
                    {
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            string Result = simula.CreatPointToPointTask(frm.BeginLandCode, frm.EndLandCode);
                            MsgBox.ShowWarn(Result);
                        }
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void luArea_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_data.SelectedObjects.Count() > 0)
                {
                    StorageTool curr = m_data.SelectedObjects.FirstOrDefault() as StorageTool;
                    if (curr != null)
                    {
                        DevExpress.XtraEditors.LookUpEdit lku = sender as DevExpress.XtraEditors.LookUpEdit;
                        if (lku != null)
                        {
                            int Area = int.Parse(lku.EditValue.ToString());
                            curr.OwnArea = Area;
                        }
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        private void btnCarInfo_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (FrmAGVAchive frm = new FrmAGVAchive())
            { frm.ShowDialog(); }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (MsgBox.ShowQuestion("确定退出当前系统?") != DialogResult.Yes)
                { e.Cancel = true; return; }
                string RemeberFilePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\SkinFile.txt";
                if (File.Exists(RemeberFilePath))
                { File.Delete(RemeberFilePath); }
                File.AppendAllText(RemeberFilePath, UserLookAndFeel.Default.ActiveSkinName);
                //Application.Exit();
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void navBarControl1_MouseEnter(object sender, EventArgs e)
        {
            navBarControl1.Focus();
        }

        private void navBarControl1_MouseLeave(object sender, EventArgs e)
        {
            m_canvas.Focus();
        }
        #endregion

        #region 自定义函数
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
                    IList<AllSegment> AllSegs = AGVClientDAccess.LoadAllSegment();
                    if (AllSegs != null)
                    {
                        foreach (AllSegment item in AllSegs)
                        {
                            IDrawObject LandDraw1 = m_data.ActiveLayer.Objects.Where(p => p.Id == "LandMark" && (p as LandMarkTool).LandCode == item.BeginLandMakCode).FirstOrDefault();
                            IDrawObject LandDraw2 = m_data.ActiveLayer.Objects.Where(p => p.Id == "LandMark" && (p as LandMarkTool).LandCode == item.EndLandMarkCode).FirstOrDefault();
                            if (LandDraw1 == null || LandDraw2 == null)
                            { continue; }
                            else
                            {
                                IList<IDrawObject> Draws = m_data.ActiveLayer.Objects.Where(p => p.Id == "LineTool" && (p as LineTool).Type == LineType.PointLine).ToList();
                                IDrawObject CurrDraw = Draws.Where(q => Math.Abs(Math.Round((q as LineTool).P1.X, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw1 as LandMarkTool).MidPoint.X, 2, MidpointRounding.AwayFromZero)) <= 0.02
                                 && Math.Abs(Math.Round((q as LineTool).P1.Y, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw1 as LandMarkTool).MidPoint.Y, 2, MidpointRounding.AwayFromZero)) <= 0.02 &&
                                 Math.Abs(Math.Round((q as LineTool).P2.X, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw2 as LandMarkTool).MidPoint.X, 2, MidpointRounding.AwayFromZero)) <= 0.02 &&
                                 Math.Abs(Math.Round((q as LineTool).P2.Y, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw2 as LandMarkTool).MidPoint.Y, 2, MidpointRounding.AwayFromZero)) <= 0.02).FirstOrDefault();
                                if (CurrDraw != null)
                                { (CurrDraw as LineTool).Lenth = item.Length; }


                                Draws = m_data.ActiveLayer.Objects.Where(p => p.Id == "BezierTool" && (p as BezierTool).Type == BezierType.PointBezier).ToList();
                                CurrDraw = Draws.Where(q => Math.Abs(Math.Round((q as BezierTool).P1.X, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw1 as LandMarkTool).MidPoint.X, 2, MidpointRounding.AwayFromZero)) <= 0.02
                                 && Math.Abs(Math.Round((q as BezierTool).P1.Y, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw1 as LandMarkTool).MidPoint.Y, 2, MidpointRounding.AwayFromZero)) <= 0.02 &&
                                 Math.Abs(Math.Round((q as BezierTool).P2.X, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw2 as LandMarkTool).MidPoint.X, 2, MidpointRounding.AwayFromZero)) <= 0.02 &&
                                 Math.Abs(Math.Round((q as BezierTool).P2.Y, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw2 as LandMarkTool).MidPoint.Y, 2, MidpointRounding.AwayFromZero)) <= 0.02).FirstOrDefault();
                                if (CurrDraw != null)
                                { (CurrDraw as BezierTool).Lenth = item.Length; }
                            }
                        }
                    }
                    m_canvas.DoInvalidate(true);
                }));
            }
            catch (Exception ex)
            { }
        }

        //保存函数
        private void Save()
        {
            try
            {
                OperateReturnInfo opr;
                UpdateData();
                if (m_filename.Length == 0)
                { SaveAs(); }
                else
                {
                    m_filename = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\temSet.agv";
                    m_data.Save(m_filename);
                }
                if (string.IsNullOrEmpty(m_filename)) { return; }
                using (DevExpress.Utils.WaitDialogForm dialog = new DevExpress.Utils.WaitDialogForm("正在保存,请稍后...", "提示"))
                {
                    //将xml文件以二进制保存到数据库中
                    string fileName = "";
                    if (m_filename.Contains("\\"))
                        fileName = m_filename.Substring(m_filename.LastIndexOf('\\')).Replace("\\", "");
                    else
                        fileName = m_filename;
                    IList<IDrawObject> lands = m_data.ActiveLayer.Objects.Where(P => P.Id == "LandMark").ToList<IDrawObject>();
                    IList<LandmarkInfo> Lands = new List<LandmarkInfo>();
                    foreach (IDrawObject item in lands)
                    {
                        LandMarkTool Mark = item as LandMarkTool;
                        if (Mark == null) { continue; }
                        LandmarkInfo land = new LandmarkInfo();
                        land.LandX =Mark.Location.X;
                        land.LandY = Mark.Location.Y;
                        land.LandMidX = Mark.MidPoint.X;
                        land.LandMidY =Mark.MidPoint.Y;
                        land.LandmarkCode = Mark.LandCode;
                        land.LandMarkName = Mark.LandName;
                        Lands.Add(land);
                    }
                    //判断地标重复
                    var q = Lands.GroupBy(x => x.LandmarkCode).Where(x => x.Count() > 1).ToList();
                    if (q.Count > 0)
                    {
                        string landCodeStr = "";
                        foreach (var item in q)
                        {
                            landCodeStr += item.Key + ",";
                            if (landCodeStr.Length > 20)
                            { landCodeStr += "\r\n"; }
                        }
                        if (!string.IsNullOrEmpty(landCodeStr))
                        {
                            MsgBox.ShowWarn("有重复地标:" + landCodeStr);
                            return;
                        }
                    }


                    IList<StorageInfo> Stocks = new List<StorageInfo>();
                    if (IsSaveStore)
                    {
                        IList<IDrawObject> StocksDraws = m_data.ActiveLayer.Objects.Where(P => P.Id == "StorageTool").ToList<IDrawObject>();
                        foreach (IDrawObject item in StocksDraws)
                        {
                            StorageTool Stock = item as StorageTool;
                            if (Stock == null) { continue; }
                            StorageInfo stockInfo = new StorageInfo();
                            stockInfo.ID = Stock.StcokID;
                            stockInfo.StorageName = Stock.StorageName;
                            stockInfo.OwnArea = Stock.OwnArea;
                            stockInfo.SubOwnArea = Stock.SubOwnArea;
                            stockInfo.matterType = Stock.matterType;
                            stockInfo.MaterielType = Stock.MaterielType;
                            stockInfo.LankMarkCode = Stock.LankMarkCode;
                            stockInfo.StorageState = Stock.StorageState;
                            Stocks.Add(stockInfo);
                        }
                    }

                    //保存全局线段
                    IList<AllSegment> allsegment = new List<AllSegment>();
                    if (Lands.Count > 0)
                    {
                        IList<IDrawObject> Draws = m_data.ActiveLayer.Objects.Where(P => P.Id == "LineTool" || P.Id == "BezierTool").ToList<IDrawObject>();
                        foreach (IDrawObject Draw in Draws)
                        {
                            if (Draw is LineTool)
                            {
                                LineTool line = Draw as LineTool;
                                if (line == null) { continue; }
                                if (line.Type == LineType.Line) { continue; }
                                LandmarkInfo BegLand = Lands.Where(p => Math.Round(p.LandMidX, 1, MidpointRounding.AwayFromZero) == Math.Round(line.P1.X, 1, MidpointRounding.AwayFromZero) && Math.Round(p.LandMidY, 1, MidpointRounding.AwayFromZero) == Math.Round(line.P1.Y, 1, MidpointRounding.AwayFromZero)).FirstOrDefault();
                                LandmarkInfo EndLand = Lands.Where(p => Math.Round(p.LandMidX, 1, MidpointRounding.AwayFromZero) == Math.Round(line.P2.X, 1, MidpointRounding.AwayFromZero) && Math.Round(p.LandMidY, 1, MidpointRounding.AwayFromZero) == Math.Round(line.P2.Y, 1, MidpointRounding.AwayFromZero)).FirstOrDefault();
                                if (BegLand != null && EndLand != null)
                                {
                                    if (allsegment.Where(p => p.BeginLandMakCode == BegLand.LandmarkCode && p.EndLandMarkCode == EndLand.LandmarkCode).Count() <= 0)
                                    {
                                        AllSegment newitem = new AllSegment();
                                        newitem.BeginLandMakCode = BegLand.LandmarkCode;
                                        newitem.EndLandMarkCode = EndLand.LandmarkCode;
                                        newitem.Length = line.Lenth;
                                        newitem.ExcuteAngle = line.ExcuteAngle;
                                        newitem.ExcuteMoveDirect = line.ExcuteMoveDirect;
                                        newitem.ExcuteTurnDirect = line.ExcuteTurnDirect;
                                        newitem.SegmentType = 0;
                                        allsegment.Add(newitem);
                                    }
                                }
                            }
                            else if (Draw is BezierTool)
                            {
                                BezierTool Bezier = Draw as BezierTool;
                                if (Bezier == null) { continue; }
                                if (Bezier.Type == BezierType.Bezier) { continue; }
                                LandmarkInfo BegLand = Lands.Where(p => Math.Round(p.LandMidX, 1, MidpointRounding.AwayFromZero) == Math.Round(Bezier.P1.X, 1, MidpointRounding.AwayFromZero) && Math.Round(p.LandMidY, 1, MidpointRounding.AwayFromZero) == Math.Round(Bezier.P1.Y, 1, MidpointRounding.AwayFromZero)).FirstOrDefault();
                                LandmarkInfo EndLand = Lands.Where(p => Math.Round(p.LandMidX, 1, MidpointRounding.AwayFromZero) == Math.Round(Bezier.P2.X, 1, MidpointRounding.AwayFromZero) && Math.Round(p.LandMidY, 1, MidpointRounding.AwayFromZero) == Math.Round(Bezier.P2.Y, 1, MidpointRounding.AwayFromZero)).FirstOrDefault();
                                if (BegLand != null && EndLand != null)
                                {
                                    if (allsegment.Where(p => p.BeginLandMakCode == BegLand.LandmarkCode && p.EndLandMarkCode == EndLand.LandmarkCode).Count() <= 0)
                                    {
                                        AllSegment newitem = new AllSegment();
                                        newitem.BeginLandMakCode = BegLand.LandmarkCode;
                                        newitem.EndLandMarkCode = EndLand.LandmarkCode;
                                        newitem.Length = Bezier.Lenth;
                                        newitem.ExcuteMoveDirect = Bezier.ExcuteMoveDirect;
                                        newitem.ExcuteTurnDirect = Bezier.ExcuteTurnDirect;
                                        newitem.SegmentType = 1;
                                        newitem.Point3X = Bezier.P3.X;
                                        newitem.Point3Y = Bezier.P3.Y;
                                        newitem.Point4X = Bezier.P4.X;
                                        newitem.Point4Y = Bezier.P4.Y;
                                        allsegment.Add(newitem);
                                    }
                                }
                            }

                        }
                        SysParameter sys = AGVClientDAccess.GetParameterByCode("IsImportLenth");
                        if (sys != null && sys.ParameterValue == "是" && allsegment.Count > 0)
                        {
                            int NoLenCount = allsegment.Where(a => a.Length == 0).Count();
                            if (NoLenCount > 0)
                            {
                                MsgBox.ShowWarn("存在路径长度没维护!");
                                return;
                            }
                        }
                    }
                    opr = AGVClientDAccess.SaveMap(m_filename, fileName, m_data.Zoom, Lands, Stocks, allsegment);
                }
                MsgBox.Show(opr);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        /// <summary>
        /// 地图另存为文件
        /// </summary>
        private void SaveAs()
        {
            UpdateData();
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Cad XML files (*.agv)|*.agv";
            dlg.OverwritePrompt = true;
            if (m_filename.Length > 0)
                dlg.FileName = m_filename;
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                m_filename = dlg.FileName;
                m_data.Save(m_filename);
            }
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

        #region 模拟运行
        System.Windows.Forms.Timer FreshDate_Timer = new System.Windows.Forms.Timer();
        Simulator simula = null;
        bool IsRunninSimula = false;
        IDictionary<string, UnitPoint[]> BezierPoints = new Dictionary<string, UnitPoint[]>(); //存储贝塞尔曲线上的点集
        private void IniMoni()
        {
            try
            {
                BezierPoints.Clear();
                simula = new Simulator();
                FreshDate_Timer.Enabled = true;
                FreshDate_Timer.Interval = 2000;
                FreshDate_Timer.Tick += FreshDate_Timer_Tick;
                simula.Car_Move += Simula_Car_Move;
                simula.Car_Ini += Simula_Car_Ini;
                simula.Inital();
                m_canvas.DoInvalidate(true);
                lblInflection.Caption = "";
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        double ScalingRate = 0;
        private void Simula_Car_Ini(object obj)
        {
            try
            {
                if (!this.IsHandleCreated) { return; }
                this.Invoke((EventHandler)(delegate
                {

                    SysParameter sys = AGVClientDAccess.GetParameterByCode("ScalingRate");
                    if (sys != null)
                    {
                        try
                        {
                            ScalingRate = Convert.ToDouble(sys.ParameterValue);
                        }
                        catch
                        {
                            return;
                        }
                    }

                    if (ScalingRate <= 0)
                    {
                        return;
                    }
                    IEnumerable<IDrawObject> objects = from a in m_data.ActiveLayer.Objects
                                                       where a.Id == "AGVTool"
                                                       select a;
                    m_data.DeleteObjects(objects);

                    List<CarMonitor> allCars = obj as List<CarMonitor>;
                    if (allCars != null)
                    {
                        foreach (CarMonitor car in allCars)
                        {
                            AGVTool agv = new AGVTool();
                            agv.Agv_id = car.AgvID.ToString();
                            agv.Position = new UnitPoint(car.X / ScalingRate, car.Y / ScalingRate);
                            m_data.AddObject(m_data.ActiveLayer, agv);
                            m_canvas.DoInvalidate(true);
                        }
                    }
                }));
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void Simula_Car_Move(object obj)
        {
            try
            {
                if (obj == null) { return; }
                if (!this.m_canvas.IsHandleCreated) { return; }
                this.m_canvas.Invoke((EventHandler)(delegate
                {
                    if (ScalingRate <= 0)
                    {
                        return;
                    }

                    CarMonitor car = obj as CarMonitor;
                    IEnumerable<IDrawObject> objects = from a in m_data.ActiveLayer.Objects
                                                       where a.Id == "AGVTool" && (a as AGVTool).Agv_id == car.AgvID.ToString()
                                                       select a;
                    if (objects.Count() <= 0)
                    {
                        AGVTool agv = new AGVTool();
                        agv.Agv_id = car.AgvID.ToString();
                        agv.Position = new UnitPoint(car.X / ScalingRate, car.Y / ScalingRate);
                        m_data.AddObject(m_data.ActiveLayer, agv);
                        FreshCanvas();
                    }
                    else
                    {
                        AGVTool agv = objects.FirstOrDefault() as AGVTool;
                        if (agv != null)
                        {
                            if (car.Rundistance != 0)
                            {
                                AllSegment segment = SimulatorVar.AllSegs.FirstOrDefault(p => p.BeginLandMakCode == car.CurrLand.LandmarkCode && p.EndLandMarkCode == car.NextLand.LandmarkCode);
                                if (segment != null && segment.SegmentType == 0)//直线
                                {
                                    double runrate = car.Rundistance / segment.Length;//当前线段行走百分比
                                    car.X = (float)(car.CurrLand.LandX + (car.NextLand.LandX - car.CurrLand.LandX) * runrate);
                                    car.Y = (float)(car.CurrLand.LandY + (car.NextLand.LandY - car.CurrLand.LandY) * runrate);
                                    agv.Position = new UnitPoint(car.X / ScalingRate, car.Y / ScalingRate);
                                }
                                else if (segment != null && segment.SegmentType == 1)
                                {
                                    double runrate = car.Rundistance / segment.Length;//当前线段行走百分比
                                    if (!BezierPoints.Keys.Contains(car.CurrLand + "|" + car.CurrLand))
                                    {
                                        IDrawObject LandDraw1 = m_data.ActiveLayer.Objects.Where(p => p.Id == "LandMark" && (p as LandMarkTool).LandCode == car.CurrLand.LandmarkCode).FirstOrDefault();
                                        IDrawObject LandDraw2 = m_data.ActiveLayer.Objects.Where(p => p.Id == "LandMark" && (p as LandMarkTool).LandCode == car.NextLand.LandmarkCode).FirstOrDefault();
                                        IList<IDrawObject> Draws = m_data.ActiveLayer.Objects.Where(p => p.Id == "BezierTool" && (p as BezierTool).Type == BezierType.PointBezier).ToList();
                                        IDrawObject CurrDraw = Draws.Where(q => Math.Abs(Math.Round((q as BezierTool).P1.X, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw1 as LandMarkTool).MidPoint.X, 2, MidpointRounding.AwayFromZero)) <= 0.02
                                         && Math.Abs(Math.Round((q as BezierTool).P1.Y, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw1 as LandMarkTool).MidPoint.Y, 2, MidpointRounding.AwayFromZero)) <= 0.02 &&
                                         Math.Abs(Math.Round((q as BezierTool).P2.X, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw2 as LandMarkTool).MidPoint.X, 2, MidpointRounding.AwayFromZero)) <= 0.02 &&
                                         Math.Abs(Math.Round((q as BezierTool).P2.Y, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw2 as LandMarkTool).MidPoint.Y, 2, MidpointRounding.AwayFromZero)) <= 0.02).FirstOrDefault();
                                        BezierTool nextSeg = CurrDraw as BezierTool;
                                        if (nextSeg == null)
                                        { return; }
                                        UnitPoint[] pointList = new UnitPoint[] { nextSeg.P1, nextSeg.P4, nextSeg.P3, nextSeg.P2 };
                                        UnitPoint[] unitlist = draw_bezier_curves(pointList, pointList.Length, 0.001F); // 在起点和终点之间
                                        BezierPoints[car.CurrLand.LandmarkCode + "|" + car.NextLand.LandmarkCode] = unitlist;
                                    }
                                    UnitPoint[] aa = BezierPoints[car.CurrLand.LandmarkCode + "|" + car.NextLand.LandmarkCode];
                                    int currindex = (int)Math.Floor(aa.Length * runrate);
                                    if (currindex >= aa.Length - 2)
                                    {
                                        currindex = aa.Length - 2;
                                    }
                                    car.X = (float)(aa[currindex].X) - 0.1f;
                                    car.Y = (float)(aa[currindex].Y) + 0.1f;
                                    agv.Position = new UnitPoint(car.X / ScalingRate, car.Y / ScalingRate);
                                }
                            }

                            lblInflection.Caption = "";
                            if (car.CurrLand.sway == SwayEnum.Left)
                            { lblInflection.Caption += "左转"; }
                            if (car.CurrLand.sway == SwayEnum.Right)
                            { lblInflection.Caption += "右转"; }
                            if (car.CurrLand.sway == SwayEnum.ExcuteLand)
                            { lblInflection.Caption += "强制地标"; }
                            if (car.CurrLand.movedirect == MoveDirectEnum.Forward)
                            { lblInflection.Caption += "前进"; }
                            if (car.CurrLand.movedirect == MoveDirectEnum.Backoff)
                            { lblInflection.Caption += "后退"; }
                            if (car.CurrLand.movedirect == MoveDirectEnum.Reverse)
                            { lblInflection.Caption += "反向"; }
                            if (car.CurrLand.sway != SwayEnum.None)
                            { lblInflection.Caption += car.CurrLand.Angle.ToString(); }
                            FreshCanvas();
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private async void FreshCanvas()
        {
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    //Thread.Sleep(1);
                    m_canvas.DoInvalidate(true);
                });
            }
            catch
            { }
        }

        private void FreshDate_Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                new Thread(new ThreadStart(UpdateStock)) { IsBackground = true }.Start();
                ClearMemory();
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region 贝塞尔曲线
        /// <summary>  
        /// 绘制n阶贝塞尔曲线路径  
        /// </summary>  
        /// <param name="points">输入点</param>  
        /// <param name="count">点数(n+1)</param>  
        /// <param name="step">步长,步长越小，轨迹点越密集</param>  
        /// <returns></returns>  
        public static UnitPoint[] draw_bezier_curves(UnitPoint[] points, int count, float step)
        {
            List<UnitPoint> bezier_curves_points = new List<UnitPoint>();
            float t = 0F;
            do
            {
                UnitPoint temp_point = bezier_interpolation_func(t, points, count);    // 计算插值点  
                t += step;
                bezier_curves_points.Add(temp_point);
            }
            while (t <= 1 && count > 1);    // 一个点的情况直接跳出.  
            return bezier_curves_points.ToArray();  // 曲线轨迹上的所有坐标点  
        }


        /// <summary>  
        /// n阶贝塞尔曲线插值计算函数  
        /// 根据起点，n个控制点，终点 计算贝塞尔曲线插值  
        /// </summary>  
        /// <param name="t">当前插值位置0~1 ，0为起点，1为终点</param>  
        /// <param name="points">起点，n-1个控制点，终点</param>  
        /// <param name="count">n+1个点</param>  
        /// <returns></returns>  
        private static UnitPoint bezier_interpolation_func(float t, UnitPoint[] points, int count)
        {
            UnitPoint PointF = new UnitPoint();
            float[] part = new float[count];
            float sum_x = 0, sum_y = 0;
            for (int i = 0; i < count; i++)
            {
                ulong tmp;
                int n_order = count - 1;    // 阶数  
                tmp = calc_combination_number(n_order, i);
                sum_x += (float)(tmp * points[i].X * Math.Pow((1 - t), n_order - i) * Math.Pow(t, i));
                sum_y += (float)(tmp * points[i].Y * Math.Pow((1 - t), n_order - i) * Math.Pow(t, i));
            }
            PointF.X = sum_x;
            PointF.Y = sum_y;
            return PointF;
        }

        /// <summary>  
        /// 计算组合数公式  
        /// </summary>  
        /// <param name="n"></param>  
        /// <param name="k"></param>  
        /// <returns></returns>  
        private static ulong calc_combination_number(int n, int k)
        {
            ulong[] result = new ulong[n + 1];
            for (int i = 1; i <= n; i++)
            {
                result[i] = 1;
                for (int j = i - 1; j >= 1; j--)
                    result[j] += result[j - 1];
                result[0] = 1;
            }
            return result[k];
        }
        #endregion

        private void luMaterialInfo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_data.SelectedObjects.Count() > 0)
                {
                    StorageTool curr = m_data.SelectedObjects.FirstOrDefault() as StorageTool;
                    if (curr != null)
                    {
                        DevExpress.XtraEditors.LookUpEdit lku = sender as DevExpress.XtraEditors.LookUpEdit;
                        if (lku != null)
                        {
                            int Material = int.Parse(lku.EditValue.ToString());
                            curr.MaterielType = Material;
                        }
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
