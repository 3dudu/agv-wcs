using Canvas;
using Canvas.DrawTools;
using DevExpress.XtraNavBar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DrawTools = Canvas.DrawTools;
using AGVDAccess;
using System.IO;
using DevExpress.Utils;
using DevExpress.XtraBars;
using System.Runtime.InteropServices;
using RCSEditTool.FrmSystem;
using Canvas.CanvasInterfaces;
using Canvas.CanvasCtrl;
using RCSEditTool.FrmCommon;
using Canvas.EditTools;
using Tools;
using Model.MSM;
using Model.MDM;
using Canvas.Layers;
using DXAGVClient;
using DXAGVClient.FormSystem;
using RCSEditTool.FrmExternal;
using DXAGVClient.FrmExternal;
using System.Threading;
using Model.Comoon;
using SQLServerOperator;
using System.Drawing;
using System.Collections;

namespace RCSEditTool
{
    public partial class FrmMainView : BaseForm, ICanvasOwner, IEditToolOwner
    {
        #region 全局变量
        CanvasCtrller m_canvas;
        DataModel m_data;
        string m_filename = string.Empty;
        bool IsSaveStore = false;


        #endregion

        public FrmMainView()
        {
            InitializeComponent();
            //dpInfo.Enabled = false;
            pnlChoose.Height = 0;
            pnlChoose.Visible = false;
            bool isSucc = true;
            //try
            //{
            //    ConnectConfigTool.setDBase();
            //}
            //catch (Exception ex)
            //{
            //    MsgBox.ShowError(ex.Message + "请检查...");
            //    isSucc = false;
            //}
            //if (ConnectConfigTool.DBase == null || !isSucc)
            //{
            //    //弹出维护数据库维护界面
            //    using (FrmSysConnSet frm = new FrmSysConnSet())
            //    {
            //        if (frm.ShowDialog() != DialogResult.OK)
            //        {
            //            Application.ExitThread();
            //            Application.Exit();
            //            return;
            //        }
            //    }
            //}
            using (WaitDialogForm dialog = new WaitDialogForm("正在启动,请稍后...", "提示"))
            {
                try
                {
                    // InitCanvas("", false);
                    InitCanvas(AppDomain.CurrentDomain.BaseDirectory+"1.agv", true);
                }
                catch (Exception ex)
                { MsgBox.ShowError(ex.Message); }
            }
        }

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
                string storageColorPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\StorageColor.txt";
                if (File.Exists(storageColorPath))
                {
                    Hashtable hs = XMLClass.GetXMLByParentNode(storageColorPath, "StorageColor");
                    if (hs["NullStorageColor"] != null && !string.IsNullOrEmpty(hs["NullStorageColor"].ToString()))
                    {
                        string[] nullStorageColorRGB = hs["NullStorageColor"].ToString().Split(',');
                        Color nullStorageColor = Color.FromArgb(Convert.ToInt16(nullStorageColorRGB[0]), Convert.ToInt16(nullStorageColorRGB[1]), Convert.ToInt16(nullStorageColorRGB[2]));
                        m_data.NullStorageColor = nullStorageColor;
                    }
                    if (hs["EmptyShelfStorageColor"] != null && !string.IsNullOrEmpty(hs["EmptyShelfStorageColor"].ToString()))
                    {
                        string[] emptyShelfStorageRGB = hs["EmptyShelfStorageColor"].ToString().Split(',');
                        Color emptyShelfStorageColor = Color.FromArgb(Convert.ToInt16(emptyShelfStorageRGB[0]), Convert.ToInt16(emptyShelfStorageRGB[1]), Convert.ToInt16(emptyShelfStorageRGB[2]));
                        m_data.EmptyShelfStorageColor = emptyShelfStorageColor;
                    }
                    if (hs["FillShelfStorageColor"] != null && !string.IsNullOrEmpty(hs["FillShelfStorageColor"].ToString()))
                    {
                        string[] fillShelfStorageRGB = hs["FillShelfStorageColor"].ToString().Split(',');
                        Color fillShelfStorageColor = Color.FromArgb(Convert.ToInt16(fillShelfStorageRGB[0]), Convert.ToInt16(fillShelfStorageRGB[1]), Convert.ToInt16(fillShelfStorageRGB[2]));
                        m_data.FillShelfStorageColor = fillShelfStorageColor;
                    }

                }

                if (filename.Length > 0 && File.Exists(filename) && m_data.Load(filename, null))
                { m_filename = filename; }
                m_canvas = new CanvasCtrller(this, m_data);
                m_canvas.Dock = DockStyle.Fill;
                plcenter.Controls.Clear();
                plcenter.Controls.Add(m_canvas);
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
            btnDashLine.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnDashLine.Tag = "DashLine";
            m_data.AddDrawTool(btnDashLine.Tag.ToString(), new LineTool(LineType.Dote));
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
            btnArc.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnArc.Tag = "arc3p";
            m_data.AddDrawTool(btnArc.Tag.ToString(), new Arc3Point());
            btnBesize.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnBesize.Tag = "BezierTool";
            m_data.AddDrawTool(btnBesize.Tag.ToString(), new BezierTool(BezierType.Bezier));
            btnPointBesize.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnPointBesize.Tag = "PointBezier";
            m_data.AddDrawTool(btnPointBesize.Tag.ToString(), new BezierTool(BezierType.PointBezier));
            btnStock.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnStock.Tag = "StorageTool";
            m_data.AddDrawTool(btnStock.Tag.ToString(), new StorageTool());
            //操作工具
            btnUndo.LinkClicked += new NavBarLinkEventHandler(OnUndo);
            btnRedo.LinkClicked += new NavBarLinkEventHandler(OnRedo);
            btnselect.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnselect.Tag = "select";
            btnHand.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnHand.Tag = "pan";
            btnMove.LinkClicked += new NavBarLinkEventHandler(OnToolSelect);
            btnMove.Tag = "move";
        }

        private void OnCanvasMouseUp(object sender, MouseEventArgs e)
        {
            cbxeOwnArea.SelectedIndexChanged -= cbxeOwnArea_SelectedIndexChanged;
            luArea.EditValueChanged -= luArea_EditValueChanged;
            cbxematterType.SelectedIndexChanged -= cbxematterType_SelectedIndexChanged;
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
                        this.cbxeOwnArea.SelectedIndex = curr.OwnArea;
                        luArea.EditValue = curr.OwnArea;
                        this.cbxematterType.SelectedIndex = curr.matterType;
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
                //更改属性表格行高
                this.Property.UpdateRows();
                foreach (DevExpress.XtraVerticalGrid.Rows.CategoryRow BaseRow in Property.Rows)
                {
                    foreach (DevExpress.XtraVerticalGrid.Rows.BaseRow child in BaseRow.ChildRows)
                    { child.Height = 80; }
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
                cbxeOwnArea.SelectedIndexChanged += cbxeOwnArea_SelectedIndexChanged;
                luArea.EditValueChanged += luArea_EditValueChanged;
                cbxematterType.SelectedIndexChanged += cbxematterType_SelectedIndexChanged;
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
                    if (obj != null && obj.Id == "LandMark")
                    {
                        LandMarkTool curr = m_data.SelectedObjects.FirstOrDefault() as LandMarkTool;
                        if (curr == null) { return; }
                        using (FrmLandCooraSet frm = new FrmLandCooraSet(""))
                        {
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                double ScalingRate = 0;
                                SysParameter sys = AGVClientDAccess.GetParameterByCode("ScalingRate");
                                if (sys != null)
                                {
                                    try
                                    {
                                        ScalingRate = Convert.ToDouble(sys.ParameterValue);
                                    }
                                    catch
                                    {
                                        MsgBox.ShowWarn("系统参数[缩放比率]错误!");
                                        return;
                                    }
                                }

                                if (ScalingRate <= 0)
                                {
                                    MsgBox.ShowWarn("系统参数[缩放比率]错误!");
                                    return;
                                }
                                curr.Location = new UnitPoint(frm.LandX / ScalingRate, frm.LandY / ScalingRate);



                                //IDrawObject ReferLandObj = m_data.ActiveLayer.Objects.FirstOrDefault(p => p.Id == "LandMark" && (p as LandMarkTool).MarkValue == frm.ReferLand);
                                //if (ReferLandObj == null)
                                //{
                                //    MsgBox.ShowWarn("参照地标不存在!");
                                //    return;
                                //}
                                //LandMarkTool ReferLand = ReferLandObj as LandMarkTool;
                                //double ScalingRate = 0;
                                //SysParameter sys = AGVClientDAccess.GetParameterByCode("ScalingRate");
                                //if (sys != null)
                                //{
                                //    try
                                //    {
                                //        ScalingRate = Convert.ToDouble(sys.ParameterValue);
                                //    }
                                //    catch
                                //    {
                                //        MsgBox.ShowWarn("系统参数[缩放比率]错误!");
                                //        return;
                                //    }
                                //}

                                //if (ScalingRate <= 0)
                                //{
                                //    MsgBox.ShowWarn("系统参数[缩放比率]错误!");
                                //    return;
                                //}
                                //double MapLenth = frm.Lenth / ScalingRate;
                                //switch (frm.RelativePos)
                                //{
                                //    case 0://上

                                //        curr.Location = new UnitPoint(curr.Location.X, ReferLand.Location.Y + MapLenth);
                                //        break;
                                //    case 1://下
                                //        curr.Location = new UnitPoint(curr.Location.X, ReferLand.Location.Y - MapLenth);
                                //        break;
                                //    case 2://左
                                //        curr.Location = new UnitPoint(ReferLand.Location.X - MapLenth, curr.Location.Y);
                                //        break;
                                //    case 3://右
                                //        curr.Location = new UnitPoint(ReferLand.Location.X + MapLenth, curr.Location.Y);
                                //        break;
                                //    default:
                                //        break;
                                //}
                                ////记忆
                                //PreLandLocationInfo = curr.MarkValue + "," + frm.RelativePos.ToString();
                            }
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
                string Id = e.Row.Properties.FieldName;
                IList<IDrawObject> LandTools = m_data.SelectedObjects.ToList();
                IList<IDrawObject> Lines = LandTools.Where(p => p.Id == "LineTool").ToList();
                foreach (LineTool item in Lines)
                {
                    if (Id == "ExcuteAngle")
                    { item.ExcuteAngle = int.Parse(e.Value.ToString()); }
                    if (Id == "Color")
                    { item.Color = (System.Drawing.Color)e.Value; }
                    if (Id == "ExcuteMoveDirect")
                    { item.ExcuteMoveDirect = int.Parse(e.Value.ToString()); }
                    if (Id == "ExcuteTurnDirect")
                    { item.ExcuteTurnDirect = int.Parse(e.Value.ToString()); }
                    if (Id == "Lenth")
                    { item.Lenth = int.Parse(e.Value.ToString()); }
                    if (Id == "PlanRouteLevel")
                    { item.PlanRouteLevel = int.Parse(e.Value.ToString()); }
                    if (Id == "ExcuteAvoidance")
                    { item.ExcuteAvoidance = int.Parse(e.Value.ToString()); }
                    if (Id == "ExcuteSpeed")
                    { item.ExcuteSpeed = int.Parse(e.Value.ToString()); }
                }
                IList<IDrawObject> Beziers = LandTools.Where(p => p.Id == "BezierTool").ToList();
                foreach (BezierTool item in Beziers)
                {
                    if (Id == "ExcuteAngle")
                    { item.ExcuteAngle = int.Parse(e.Value.ToString()); }
                    if (Id == "Color")
                    { item.Color = (System.Drawing.Color)e.Value; }
                    if (Id == "ExcuteMoveDirect")
                    { item.ExcuteMoveDirect = int.Parse(e.Value.ToString()); }
                    if (Id == "ExcuteTurnDirect")
                    { item.ExcuteTurnDirect = int.Parse(e.Value.ToString()); }
                    if (Id == "Lenth")
                    { item.Lenth = int.Parse(e.Value.ToString()); }
                    if (Id == "PlanRouteLevel")
                    { item.PlanRouteLevel = int.Parse(e.Value.ToString()); }
                    if (Id == "ExcuteAvoidance")
                    { item.ExcuteAvoidance = int.Parse(e.Value.ToString()); }
                    if (Id == "ExcuteSpeed")
                    { item.ExcuteSpeed = int.Parse(e.Value.ToString()); }
                }
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
                ////bool IsConnectDB = false;
                ////IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                ////IsConnectDB = dbOperator.ServerIsThrough();
                ////if (IsConnectDB)
                ////{
                ////    IList<AreaInfo> AllAreas = AGVClientDAccess.LoadAllArea();
                ////    this.bsAreaInfo.DataSource = AllAreas;
                ////    this.bsAreaInfo.ResetBindings(false);

                ////    IList<MaterialInfo> AllMaterial = AGVClientDAccess.LoadAllMaterial();
                ////    this.bsMaterialInfo.DataSource = AllMaterial;
                ////    this.bsMaterialInfo.ResetBindings(false);
                ////}
                ////更新界面储位
                //new Thread(new ThreadStart(UpdateStock)) { IsBackground = true }.Start();
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

        private void btnAGVInfo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FrmAGVAchive frm = new FrmAGVAchive())
            { frm.ShowDialog(); }
        }

        private void btnOrderSet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FrmCmdToAgv frm = new FrmCmdToAgv())
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

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

            try
            {
                string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\StorageColor.txt";

                Color nullStorageColor = m_canvas.Model.NullStorageColor;
                Color emptyShelfStorageColor = m_canvas.Model.EmptyShelfStorageColor;
                Color fillShelfStorageColor = m_canvas.Model.FillShelfStorageColor;
                BackgroundLayer layer = m_canvas.Model.BackgroundLayer as BackgroundLayer;
                Color BackGroundColor = layer.Color;
                Hashtable hs = new Hashtable();
                hs["NullStorageColor"] = nullStorageColor.R + "," + nullStorageColor.G + "," + nullStorageColor.B;
                hs["EmptyShelfStorageColor"] = emptyShelfStorageColor.R + "," + emptyShelfStorageColor.G + "," + emptyShelfStorageColor.B;
                hs["FillShelfStorageColor"] = fillShelfStorageColor.R + "," + fillShelfStorageColor.G + "," + fillShelfStorageColor.B;
                hs["BackGroundColor"] = BackGroundColor.R + "," + BackGroundColor.G + "," + BackGroundColor.B;
                XMLClass.AppendXML(path, "StorageColor", hs);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnLogin_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            { }
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

        private void btnBISet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                using (FromDetailQuerySet frm = new FromDetailQuerySet())
                { frm.ShowDialog(); }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void BtnPad_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (FrmPad frm = new FrmPad())
                { frm.ShowDialog(); }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void btnAssemblySet_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (FrmAssemblyConfig frm = new FrmAssemblyConfig())
            { frm.ShowDialog(); }
        }

        private void cbxeOwnArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_data.SelectedObjects.Count() > 0)
                {
                    StorageTool curr = m_data.SelectedObjects.FirstOrDefault() as StorageTool;
                    if (curr != null)
                    {
                        curr.OwnArea = cbxeOwnArea.SelectedIndex;
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void cbxematterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_data.SelectedObjects.Count() > 0)
                {
                    StorageTool curr = m_data.SelectedObjects.FirstOrDefault() as StorageTool;
                    if (curr != null)
                    {
                        curr.matterType = cbxematterType.SelectedIndex;
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }
        #endregion

        #region 窗体自定义函数
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
                { m_data.Save(m_filename); }
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
                        land.LandX = Mark.Location.X;
                        land.LandY = Mark.Location.Y;
                        land.LandMidX = Mark.MidPoint.X;
                        land.LandMidY = Mark.MidPoint.Y;
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
                            stockInfo.ActionLandMarkCode = Stock.AdditionalLandCode;
                            stockInfo.StorageState = Stock.StorageState;
                            Stocks.Add(stockInfo);
                        }
                    }

                    //保存全局线段
                    IList<AllSegment> allsegment = new List<AllSegment>();
                    if (Lands.Count > 0)
                    {
                        IList<IDrawObject> Draws = m_data.ActiveLayer.Objects.Where(P => P.Id == "LineTool" || P.Id == "BezierTool" || P.Id == "arc3p").ToList<IDrawObject>();
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
                                        newitem.SegmentType = 0;
                                        newitem.BeginLandMakCode = BegLand.LandmarkCode;
                                        newitem.EndLandMarkCode = EndLand.LandmarkCode;
                                        newitem.ExcuteAngle = line.ExcuteAngle;
                                        newitem.ExcuteMoveDirect = line.ExcuteMoveDirect;
                                        newitem.ExcuteTurnDirect = line.ExcuteTurnDirect;
                                        newitem.PlanRouteLevel = line.PlanRouteLevel;
                                        newitem.ExcuteAvoidance = line.ExcuteAvoidance;
                                        newitem.ExcuteSpeed = line.ExcuteSpeed;


                                        SysParameter IsAccountLenth = AGVClientDAccess.GetParameterByCode("IsAcountSegLenth");
                                        if (IsAccountLenth != null && IsAccountLenth.ParameterValue == "是")
                                        {
                                            double len = distance(BegLand.LandMidX, BegLand.LandMidY, EndLand.LandMidX, EndLand.LandMidY);
                                            newitem.Length = Math.Round(len,2);
                                        }
                                        else
                                        { newitem.Length = Math.Round(line.Lenth,2); }
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
                                        newitem.SegmentType = 1;
                                        newitem.BeginLandMakCode = BegLand.LandmarkCode;
                                        newitem.EndLandMarkCode = EndLand.LandmarkCode;
                                        newitem.Length = Bezier.Lenth;
                                        newitem.ExcuteAngle = Bezier.ExcuteAngle;
                                        newitem.ExcuteMoveDirect = Bezier.ExcuteMoveDirect;
                                        newitem.ExcuteTurnDirect = Bezier.ExcuteTurnDirect;
                                        newitem.PlanRouteLevel = Bezier.PlanRouteLevel;
                                        newitem.ExcuteAvoidance = Bezier.ExcuteAvoidance;
                                        newitem.ExcuteSpeed = Bezier.ExcuteSpeed;
                                        newitem.Point3X = Bezier.P3.X;
                                        newitem.Point3Y = Bezier.P3.Y;
                                        newitem.Point4X = Bezier.P4.X;
                                        newitem.Point4Y = Bezier.P4.Y;
                                        allsegment.Add(newitem);
                                    }
                                }
                            }
                            else if (Draw is Arc3Point)
                            {
                                Arc3Point arc3Point = Draw as Arc3Point;
                                if (arc3Point == null) { continue; }
                                LandmarkInfo BegLand = Lands.Where(p => Math.Round(p.LandMidX, 1, MidpointRounding.AwayFromZero) == Math.Round(arc3Point.P1.X, 1, MidpointRounding.AwayFromZero) && Math.Round(p.LandMidY, 1, MidpointRounding.AwayFromZero) == Math.Round(arc3Point.P1.Y, 1, MidpointRounding.AwayFromZero)).FirstOrDefault();
                                LandmarkInfo EndLand = Lands.Where(p => Math.Round(p.LandMidX, 1, MidpointRounding.AwayFromZero) == Math.Round(arc3Point.P3.X, 1, MidpointRounding.AwayFromZero) && Math.Round(p.LandMidY, 1, MidpointRounding.AwayFromZero) == Math.Round(arc3Point.P3.Y, 1, MidpointRounding.AwayFromZero)).FirstOrDefault();
                                if (BegLand != null && EndLand != null)
                                {
                                    if (allsegment.Where(p => p.BeginLandMakCode == BegLand.LandmarkCode && p.EndLandMarkCode == EndLand.LandmarkCode).Count() <= 0)
                                    {
                                        AllSegment newitem = new AllSegment();
                                        newitem.BeginLandMakCode = BegLand.LandmarkCode;
                                        newitem.EndLandMarkCode = EndLand.LandmarkCode;
                                        newitem.SegmentType = 2;
                                        newitem.Point1X = arc3Point.P1.X;
                                        newitem.Point1Y = arc3Point.P1.Y;
                                        newitem.Point2X = arc3Point.P2.X;
                                        newitem.Point2Y = arc3Point.P2.Y;
                                        newitem.Point3X = arc3Point.P3.X;
                                        newitem.Point3Y = arc3Point.P3.Y;
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
                                storage.AdditionalLandCode = item.ActionLandMarkCode;
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
                                {
                                    (CurrDraw as LineTool).Lenth = item.Length;
                                    (CurrDraw as LineTool).ExcuteAngle = item.ExcuteAngle;
                                    (CurrDraw as LineTool).ExcuteMoveDirect = item.ExcuteMoveDirect;
                                    (CurrDraw as LineTool).ExcuteTurnDirect = item.ExcuteTurnDirect;
                                    (CurrDraw as LineTool).PlanRouteLevel = item.PlanRouteLevel;
                                    (CurrDraw as LineTool).ExcuteSpeed = item.ExcuteSpeed;
                                    (CurrDraw as LineTool).ExcuteAvoidance = item.ExcuteAvoidance;
                                }


                                Draws = m_data.ActiveLayer.Objects.Where(p => p.Id == "BezierTool" && (p as BezierTool).Type == BezierType.PointBezier).ToList();
                                CurrDraw = Draws.Where(q => Math.Abs(Math.Round((q as BezierTool).P1.X, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw1 as LandMarkTool).MidPoint.X, 2, MidpointRounding.AwayFromZero)) <= 0.02
                                 && Math.Abs(Math.Round((q as BezierTool).P1.Y, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw1 as LandMarkTool).MidPoint.Y, 2, MidpointRounding.AwayFromZero)) <= 0.02 &&
                                 Math.Abs(Math.Round((q as BezierTool).P2.X, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw2 as LandMarkTool).MidPoint.X, 2, MidpointRounding.AwayFromZero)) <= 0.02 &&
                                 Math.Abs(Math.Round((q as BezierTool).P2.Y, 2, MidpointRounding.AwayFromZero) - Math.Round((LandDraw2 as LandMarkTool).MidPoint.Y, 2, MidpointRounding.AwayFromZero)) <= 0.02).FirstOrDefault();
                                if (CurrDraw != null)
                                {
                                    (CurrDraw as BezierTool).Lenth = item.Length;
                                    (CurrDraw as BezierTool).ExcuteAngle = item.ExcuteAngle;
                                    (CurrDraw as BezierTool).ExcuteMoveDirect = item.ExcuteMoveDirect;
                                    (CurrDraw as BezierTool).ExcuteTurnDirect = item.ExcuteTurnDirect;
                                    (CurrDraw as BezierTool).PlanRouteLevel = item.PlanRouteLevel;
                                    (CurrDraw as BezierTool).ExcuteAvoidance = item.ExcuteAvoidance;
                                    (CurrDraw as BezierTool).ExcuteSpeed = item.ExcuteSpeed;
                                }
                            }
                        }
                    }
                    m_canvas.DoInvalidate(true);
                }));
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

        #region 权限控制
        private void btnAuthority_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (FrmSysAut frm = new FrmSysAut())
                {
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }
        #endregion

        #region 档案配置
        /// <summary>
        /// 线路线段设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRouteLineSet_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (FrmRouteLineSet frm = new FrmRouteLineSet())
            { frm.ShowDialog(); }
        }

        private void btnOption_ItemClick(object sender, ItemClickEventArgs e)
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

        private void btnTraJunction_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (FrmTraJunction frm = new FrmTraJunction())
                { frm.ShowDialog(); }
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

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnAreaset_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (FrmAreaSet frm = new FrmAreaSet())
                { frm.ShowDialog(); }
                bool IsConnectDB = false;
                IDbOperator dbOperator = CreateDbOperator.DbOperatorInstance(ConnectConfigTool.DBase);
                IsConnectDB = dbOperator.ServerIsThrough();
                if (IsConnectDB)
                {
                    IList<AreaInfo> AllAreas = AGVClientDAccess.LoadAllArea();
                    this.bsAreaInfo.DataSource = AllAreas;
                    this.bsAreaInfo.ResetBindings(false);
                }
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
                using (FrmIOActionInfo frm = new FrmIOActionInfo())
                { frm.ShowDialog(); }
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

        private void btnIOSet_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (FrmIODeviceInfo frm = new FrmIODeviceInfo())
                {
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }
        private void btnChargeSet_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                using (FrmChargeInfo frm = new FrmChargeInfo())
                {
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
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
         
        private void navBarControl1_MouseLeave(object sender, EventArgs e)
        {
            m_canvas.Focus();
        }
        #endregion


        public double distance(double x1, double y1, double x2, double y2)
        {
            try
            {
                double result = 0;
                result = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
                return result;
            }
            catch (Exception ex)
            { return 0; }
        }
    }
}
