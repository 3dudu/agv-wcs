namespace AGVDispatchServer
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnStart = new DevExpress.XtraBars.BarButtonItem();
            this.btnSkinSet = new DevExpress.XtraBars.SkinBarSubItem();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.btnLog = new DevExpress.XtraBars.BarButtonItem();
            this.btnExitLog = new DevExpress.XtraBars.BarButtonItem();
            this.btnExit = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.btnStop = new DevExpress.XtraBars.BarButtonItem();
            this.btnDBSet = new DevExpress.XtraBars.BarButtonItem();
            this.tpInfo = new DevExpress.XtraBars.Navigation.TabPane();
            this.tabNavigationPage1 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.gcCars = new DevExpress.XtraGrid.GridControl();
            this.bsCars = new System.Windows.Forms.BindingSource(this.components);
            this.gvcars = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colAgvID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colX = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcurrsite = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAngle = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colfVolt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsCommBreak = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCarState = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLowPower = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colspeed = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCurrLogicStr = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTaskDetailID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsBack = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colErrorMes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsUpLand = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBangState = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colJCState = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txtTrafficInfo = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tabNavigationPage3 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.listboxtask = new DevExpress.XtraEditors.ListBoxControl();
            this.tabNavigationPage4 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.btnClearAllTask = new DevExpress.XtraEditors.SimpleButton();
            this.txtMesAgvSite = new DevExpress.XtraEditors.TextEdit();
            this.txtMesAgvid = new DevExpress.XtraEditors.TextEdit();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.btnTestMes = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.btnTestchazi = new DevExpress.XtraEditors.SimpleButton();
            this.txtchazi = new DevExpress.XtraEditors.TextEdit();
            this.btnSetCarSite = new DevExpress.XtraEditors.SimpleButton();
            this.btnClearSysCarInfo = new DevExpress.XtraEditors.SimpleButton();
            this.btnStopCharge = new DevExpress.XtraEditors.SimpleButton();
            this.btnStartCharge = new DevExpress.XtraEditors.SimpleButton();
            this.txtChargeID = new DevExpress.XtraEditors.TextEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.btnBack = new System.Windows.Forms.Button();
            this.txtBackAGV = new DevExpress.XtraEditors.TextEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txtAGVID = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtArmLand = new DevExpress.XtraEditors.TextEdit();
            this.btnTestTraffic = new System.Windows.Forms.Button();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnCancelTask = new DevExpress.XtraEditors.SimpleButton();
            this.btnStorageReset = new DevExpress.XtraEditors.SimpleButton();
            this.btnSame = new DevExpress.XtraEditors.SimpleButton();
            this.btnBroadCast = new DevExpress.XtraEditors.SimpleButton();
            this.btnTestStop = new DevExpress.XtraEditors.SimpleButton();
            this.btnTestStart = new DevExpress.XtraEditors.SimpleButton();
            this.tabNavigationPage2 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.gcTask = new DevExpress.XtraGrid.GridControl();
            this.bsTask = new System.Windows.Forms.BindingSource(this.components);
            this.gvTask = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colselect = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colsite = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coltasktype = new DevExpress.XtraGrid.Columns.GridColumn();
            this.coltaskstate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colexeagv = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEndLand = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStorageName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.gcDetail = new DevExpress.XtraGrid.GridControl();
            this.bsTaskDetail = new System.Windows.Forms.BindingSource(this.components);
            this.gvDetail = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.clSelect = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetailID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLandCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.golStorageName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOperType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDetailState = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsAllowExcute = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnFinish = new DevExpress.XtraEditors.SimpleButton();
            this.btnReDo = new DevExpress.XtraEditors.SimpleButton();
            this.btnRefsh = new DevExpress.XtraEditors.SimpleButton();
            this.tabNavigationPage5 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.panelControl5 = new DevExpress.XtraEditors.PanelControl();
            this.gcChargeInfo = new DevExpress.XtraGrid.GridControl();
            this.bsChargeInfo = new System.Windows.Forms.BindingSource(this.components);
            this.gvChargeInfo = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIsBreak = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStateStr = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIP = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPort = new DevExpress.XtraGrid.Columns.GridColumn();
            this.clLandCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.npIOInfo = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.gcIOInfo = new DevExpress.XtraGrid.GridControl();
            this.bsIOInfoes = new System.Windows.Forms.BindingSource(this.components);
            this.gvIOInfo = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colIOID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDeviceName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colbIsCommBreak = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIOIP = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIOPort = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tabNavigationPage6 = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.outputedLabel = new DevExpress.XtraEditors.LabelControl();
            this.labelControl26 = new DevExpress.XtraEditors.LabelControl();
            this.inputedLabel = new DevExpress.XtraEditors.LabelControl();
            this.labelControl24 = new DevExpress.XtraEditors.LabelControl();
            this.outputTotalLabel = new DevExpress.XtraEditors.LabelControl();
            this.labelControl22 = new DevExpress.XtraEditors.LabelControl();
            this.inputTotalLabel = new DevExpress.XtraEditors.LabelControl();
            this.labelControl20 = new DevExpress.XtraEditors.LabelControl();
            this.totalStorageLabel = new DevExpress.XtraEditors.LabelControl();
            this.totalStorageUsedLabel = new DevExpress.XtraEditors.LabelControl();
            this.labelControl19 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl18 = new DevExpress.XtraEditors.LabelControl();
            this.startTest = new DevExpress.XtraEditors.SimpleButton();
            this.outputWaitLabel = new DevExpress.XtraEditors.LabelControl();
            this.inputWaitLabel = new DevExpress.XtraEditors.LabelControl();
            this.totalStorageLeafLabel = new DevExpress.XtraEditors.LabelControl();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.labelControl17 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl16 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
            this.xxxxxxx = new DevExpress.XtraEditors.LabelControl();
            this.addOutputTask = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            this.addInputTask = new DevExpress.XtraEditors.SimpleButton();
            this.LF = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.timerClearMemory = new System.Windows.Forms.Timer(this.components);
            this.notify = new System.Windows.Forms.NotifyIcon(this.components);
            this.timerFormRefsh = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection24)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.tpInfo.SuspendLayout();
            this.tabNavigationPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcCars)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCars)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvcars)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabNavigationPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listboxtask)).BeginInit();
            this.tabNavigationPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMesAgvSite.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMesAgvid.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtchazi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChargeID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBackAGV.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAGVID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArmLand.Properties)).BeginInit();
            this.tabNavigationPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcTask)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTask)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTask)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTaskDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.tabNavigationPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).BeginInit();
            this.panelControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcChargeInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsChargeInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvChargeInfo)).BeginInit();
            this.npIOInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcIOInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsIOInfoes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvIOInfo)).BeginInit();
            this.tabNavigationPage6.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageCollection24
            // 
            this.imageCollection24.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection24.ImageStream")));
            this.imageCollection24.Images.SetKeyName(0, "-.png");
            this.imageCollection24.Images.SetKeyName(1, "+.png");
            this.imageCollection24.Images.SetKeyName(2, "add-favorite.png");
            this.imageCollection24.Images.SetKeyName(3, "add-printer.png");
            this.imageCollection24.Images.SetKeyName(4, "alarm.png");
            this.imageCollection24.Images.SetKeyName(5, "alert.png");
            this.imageCollection24.Images.SetKeyName(6, "announce.png");
            this.imageCollection24.Images.SetKeyName(7, "back.png");
            this.imageCollection24.Images.SetKeyName(8, "back2.png");
            this.imageCollection24.Images.SetKeyName(9, "backward.png");
            this.imageCollection24.Images.SetKeyName(10, "basket.png");
            this.imageCollection24.Images.SetKeyName(11, "binoculars.png");
            this.imageCollection24.Images.SetKeyName(12, "burn.png");
            this.imageCollection24.Images.SetKeyName(13, "cal.png");
            this.imageCollection24.Images.SetKeyName(14, "calculator.png");
            this.imageCollection24.Images.SetKeyName(15, "cash-register.png");
            this.imageCollection24.Images.SetKeyName(16, "cc.png");
            this.imageCollection24.Images.SetKeyName(17, "cd.png");
            this.imageCollection24.Images.SetKeyName(18, "config.png");
            this.imageCollection24.Images.SetKeyName(19, "connect.png");
            this.imageCollection24.Images.SetKeyName(20, "construction.png");
            this.imageCollection24.Images.SetKeyName(21, "contacts.png");
            this.imageCollection24.Images.SetKeyName(22, "copy.png");
            this.imageCollection24.Images.SetKeyName(23, "cut.png");
            this.imageCollection24.Images.SetKeyName(24, "database.png");
            this.imageCollection24.Images.SetKeyName(25, "delete.png");
            this.imageCollection24.Images.SetKeyName(26, "delete-folder.png");
            this.imageCollection24.Images.SetKeyName(27, "down3.png");
            this.imageCollection24.Images.SetKeyName(28, "download.png");
            this.imageCollection24.Images.SetKeyName(29, "edit.png");
            this.imageCollection24.Images.SetKeyName(30, "email.png");
            this.imageCollection24.Images.SetKeyName(31, "email2.png");
            this.imageCollection24.Images.SetKeyName(32, "export.png");
            this.imageCollection24.Images.SetKeyName(33, "export1.png");
            this.imageCollection24.Images.SetKeyName(34, "faq.png");
            this.imageCollection24.Images.SetKeyName(35, "favorite.png");
            this.imageCollection24.Images.SetKeyName(36, "file.png");
            this.imageCollection24.Images.SetKeyName(37, "folder.png");
            this.imageCollection24.Images.SetKeyName(38, "forward.png");
            this.imageCollection24.Images.SetKeyName(39, "front.png");
            this.imageCollection24.Images.SetKeyName(40, "front1.png");
            this.imageCollection24.Images.SetKeyName(41, "fulltrash.png");
            this.imageCollection24.Images.SetKeyName(42, "hd.png");
            this.imageCollection24.Images.SetKeyName(43, "hd1.png");
            this.imageCollection24.Images.SetKeyName(44, "help.png");
            this.imageCollection24.Images.SetKeyName(45, "home.png");
            this.imageCollection24.Images.SetKeyName(46, "image.png");
            this.imageCollection24.Images.SetKeyName(47, "import.png");
            this.imageCollection24.Images.SetKeyName(48, "import2.png");
            this.imageCollection24.Images.SetKeyName(49, "info.png");
            this.imageCollection24.Images.SetKeyName(50, "install.png");
            this.imageCollection24.Images.SetKeyName(51, "locked.png");
            this.imageCollection24.Images.SetKeyName(52, "music.png");
            this.imageCollection24.Images.SetKeyName(53, "network.png");
            this.imageCollection24.Images.SetKeyName(54, "new-folder.png");
            this.imageCollection24.Images.SetKeyName(55, "ok.png");
            this.imageCollection24.Images.SetKeyName(56, "opened.png");
            this.imageCollection24.Images.SetKeyName(57, "open-folder.png");
            this.imageCollection24.Images.SetKeyName(58, "paste.png");
            this.imageCollection24.Images.SetKeyName(59, "photo.png");
            this.imageCollection24.Images.SetKeyName(60, "police.png");
            this.imageCollection24.Images.SetKeyName(61, "printer.png");
            this.imageCollection24.Images.SetKeyName(62, "public.png");
            this.imageCollection24.Images.SetKeyName(63, "radar.png");
            this.imageCollection24.Images.SetKeyName(64, "save.png");
            this.imageCollection24.Images.SetKeyName(65, "save-as.png");
            this.imageCollection24.Images.SetKeyName(66, "screen-capture.png");
            this.imageCollection24.Images.SetKeyName(67, "search.png");
            this.imageCollection24.Images.SetKeyName(68, "send.png");
            this.imageCollection24.Images.SetKeyName(69, "software 2.png");
            this.imageCollection24.Images.SetKeyName(70, "software update.png");
            this.imageCollection24.Images.SetKeyName(71, "software.png");
            this.imageCollection24.Images.SetKeyName(72, "sound.png");
            this.imageCollection24.Images.SetKeyName(73, "statics.png");
            this.imageCollection24.Images.SetKeyName(74, "statics-1.png");
            this.imageCollection24.Images.SetKeyName(75, "statics-2.png");
            this.imageCollection24.Images.SetKeyName(76, "stop.png");
            this.imageCollection24.Images.SetKeyName(77, "stop-alt.png");
            this.imageCollection24.Images.SetKeyName(78, "support.png");
            this.imageCollection24.Images.SetKeyName(79, "switcher.png");
            this.imageCollection24.Images.SetKeyName(80, "trash.png");
            this.imageCollection24.Images.SetKeyName(81, "truck.png");
            this.imageCollection24.Images.SetKeyName(82, "up4.png");
            this.imageCollection24.Images.SetKeyName(83, "update.png");
            this.imageCollection24.Images.SetKeyName(84, "upload.png");
            this.imageCollection24.Images.SetKeyName(85, "user.png");
            this.imageCollection24.Images.SetKeyName(86, "uses.png");
            this.imageCollection24.Images.SetKeyName(87, "viewi-pr.png");
            this.imageCollection24.Images.SetKeyName(88, "web.png");
            this.imageCollection24.Images.SetKeyName(89, "window.png");
            this.imageCollection24.Images.SetKeyName(90, "window-2.png");
            this.imageCollection24.Images.SetKeyName(91, "zoom-.png");
            this.imageCollection24.Images.SetKeyName(92, "zoom+.png");
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Images = this.imageCollection24;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnStart,
            this.btnStop,
            this.btnDBSet,
            this.btnSkinSet,
            this.btnExit,
            this.barSubItem1,
            this.btnLog,
            this.btnExitLog});
            this.barManager1.MaxItemId = 12;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnStart, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnSkinSet, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barSubItem1, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnExit, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            // 
            // btnStart
            // 
            this.btnStart.Caption = "启动服务";
            this.btnStart.Id = 1;
            this.btnStart.ImageIndex = 38;
            this.btnStart.Name = "btnStart";
            this.btnStart.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnStart.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnStart_ItemClick);
            // 
            // btnSkinSet
            // 
            this.btnSkinSet.Caption = "皮肤设置";
            this.btnSkinSet.Id = 5;
            this.btnSkinSet.ImageIndex = 35;
            this.btnSkinSet.Name = "btnSkinSet";
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "登陆";
            this.barSubItem1.Id = 9;
            this.barSubItem1.ImageIndex = 84;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnLog),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnExitLog)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // btnLog
            // 
            this.btnLog.Caption = "登陆";
            this.btnLog.Id = 10;
            this.btnLog.ImageIndex = 83;
            this.btnLog.Name = "btnLog";
            this.btnLog.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnLog_ItemClick);
            // 
            // btnExitLog
            // 
            this.btnExitLog.Caption = "退出登陆";
            this.btnExitLog.Id = 11;
            this.btnExitLog.ImageIndex = 61;
            this.btnExitLog.Name = "btnExitLog";
            this.btnExitLog.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExitLog_ItemClick);
            // 
            // btnExit
            // 
            this.btnExit.Caption = "退出系统";
            this.btnExit.Id = 6;
            this.btnExit.ImageIndex = 4;
            this.btnExit.Name = "btnExit";
            this.btnExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExit_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1139, 31);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 552);
            this.barDockControlBottom.Size = new System.Drawing.Size(1139, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 31);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 521);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1139, 31);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 521);
            // 
            // btnStop
            // 
            this.btnStop.Caption = "停止服务";
            this.btnStop.Id = 2;
            this.btnStop.ImageIndex = 25;
            this.btnStop.Name = "btnStop";
            this.btnStop.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnStop.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnStop_ItemClick);
            // 
            // btnDBSet
            // 
            this.btnDBSet.Id = 7;
            this.btnDBSet.Name = "btnDBSet";
            // 
            // tpInfo
            // 
            this.tpInfo.Controls.Add(this.tabNavigationPage1);
            this.tpInfo.Controls.Add(this.tabNavigationPage3);
            this.tpInfo.Controls.Add(this.tabNavigationPage4);
            this.tpInfo.Controls.Add(this.tabNavigationPage2);
            this.tpInfo.Controls.Add(this.tabNavigationPage5);
            this.tpInfo.Controls.Add(this.npIOInfo);
            this.tpInfo.Controls.Add(this.tabNavigationPage6);
            this.tpInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpInfo.Location = new System.Drawing.Point(0, 31);
            this.tpInfo.Name = "tpInfo";
            this.tpInfo.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            this.tabNavigationPage1,
            this.tabNavigationPage3,
            this.tabNavigationPage4,
            this.tabNavigationPage2,
            this.tabNavigationPage5,
            this.npIOInfo,
            this.tabNavigationPage6});
            this.tpInfo.RegularSize = new System.Drawing.Size(1139, 521);
            this.tpInfo.SelectedPage = this.tabNavigationPage2;
            this.tpInfo.SelectedPageIndex = 6;
            this.tpInfo.Size = new System.Drawing.Size(1139, 521);
            this.tpInfo.TabIndex = 4;
            this.tpInfo.Text = "状态监控";
            // 
            // tabNavigationPage1
            // 
            this.tabNavigationPage1.Caption = "车辆信息";
            this.tabNavigationPage1.Controls.Add(this.splitContainerControl1);
            this.tabNavigationPage1.Image = global::AGVDispatchServer.Properties.Resources.Car;
            this.tabNavigationPage1.ItemShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.tabNavigationPage1.Name = "tabNavigationPage1";
            this.tabNavigationPage1.Properties.ShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.tabNavigationPage1.Size = new System.Drawing.Size(1121, 457);
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.gcCars);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.txtTrafficInfo);
            this.splitContainerControl1.Panel2.Controls.Add(this.panel1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1121, 457);
            this.splitContainerControl1.SplitterPosition = 315;
            this.splitContainerControl1.TabIndex = 1;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // gcCars
            // 
            this.gcCars.DataSource = this.bsCars;
            this.gcCars.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcCars.Location = new System.Drawing.Point(0, 0);
            this.gcCars.MainView = this.gvcars;
            this.gcCars.MenuManager = this.barManager1;
            this.gcCars.Name = "gcCars";
            this.gcCars.Size = new System.Drawing.Size(1121, 315);
            this.gcCars.TabIndex = 0;
            this.gcCars.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvcars});
            this.gcCars.Click += new System.EventHandler(this.gcCars_Click);
            // 
            // bsCars
            // 
            this.bsCars.DataSource = typeof(Model.CarInfoExtend.CarInfo);
            // 
            // gvcars
            // 
            this.gvcars.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colAgvID,
            this.colX,
            this.colY,
            this.colcurrsite,
            this.colAngle,
            this.colfVolt,
            this.colIsCommBreak,
            this.colCarState,
            this.colLowPower,
            this.colspeed,
            this.colCurrLogicStr,
            this.colTaskDetailID,
            this.colIsBack,
            this.colErrorMes,
            this.colIsUpLand,
            this.colBangState,
            this.colJCState});
            this.gvcars.GridControl = this.gcCars;
            this.gvcars.Name = "gvcars";
            this.gvcars.OptionsBehavior.Editable = false;
            this.gvcars.OptionsSelection.MultiSelect = true;
            this.gvcars.OptionsView.ShowDetailButtons = false;
            this.gvcars.OptionsView.ShowFooter = true;
            this.gvcars.OptionsView.ShowGroupPanel = false;
            this.gvcars.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.True;
            this.gvcars.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.True;
            // 
            // colAgvID
            // 
            this.colAgvID.Caption = "小车ID号";
            this.colAgvID.FieldName = "AgvID";
            this.colAgvID.Name = "colAgvID";
            this.colAgvID.OptionsColumn.AllowEdit = false;
            this.colAgvID.Visible = true;
            this.colAgvID.VisibleIndex = 0;
            // 
            // colX
            // 
            this.colX.Caption = "X坐标";
            this.colX.FieldName = "X";
            this.colX.Name = "colX";
            this.colX.Visible = true;
            this.colX.VisibleIndex = 1;
            // 
            // colY
            // 
            this.colY.Caption = "Y坐标";
            this.colY.FieldName = "Y";
            this.colY.Name = "colY";
            this.colY.Visible = true;
            this.colY.VisibleIndex = 2;
            // 
            // colcurrsite
            // 
            this.colcurrsite.Caption = "当前站点";
            this.colcurrsite.FieldName = "CurrSite";
            this.colcurrsite.Name = "colcurrsite";
            this.colcurrsite.Visible = true;
            this.colcurrsite.VisibleIndex = 3;
            // 
            // colAngle
            // 
            this.colAngle.Caption = "车头方向";
            this.colAngle.FieldName = "Angel";
            this.colAngle.Name = "colAngle";
            this.colAngle.Visible = true;
            this.colAngle.VisibleIndex = 4;
            // 
            // colfVolt
            // 
            this.colfVolt.Caption = "电压";
            this.colfVolt.FieldName = "fVolt";
            this.colfVolt.Name = "colfVolt";
            this.colfVolt.Visible = true;
            this.colfVolt.VisibleIndex = 8;
            // 
            // colIsCommBreak
            // 
            this.colIsCommBreak.Caption = "通讯中断";
            this.colIsCommBreak.FieldName = "bIsCommBreak";
            this.colIsCommBreak.Name = "colIsCommBreak";
            this.colIsCommBreak.Visible = true;
            this.colIsCommBreak.VisibleIndex = 5;
            // 
            // colCarState
            // 
            this.colCarState.Caption = "当前状态";
            this.colCarState.FieldName = "CarStateStr";
            this.colCarState.Name = "colCarState";
            this.colCarState.Visible = true;
            this.colCarState.VisibleIndex = 6;
            // 
            // colLowPower
            // 
            this.colLowPower.Caption = "低电量";
            this.colLowPower.FieldName = "LowPower";
            this.colLowPower.Name = "colLowPower";
            this.colLowPower.Visible = true;
            this.colLowPower.VisibleIndex = 7;
            // 
            // colspeed
            // 
            this.colspeed.Caption = "速度";
            this.colspeed.FieldName = "speed";
            this.colspeed.Name = "colspeed";
            this.colspeed.Visible = true;
            this.colspeed.VisibleIndex = 9;
            // 
            // colCurrLogicStr
            // 
            this.colCurrLogicStr.Caption = "当前任务号";
            this.colCurrLogicStr.FieldName = "ExcuteTaksNo";
            this.colCurrLogicStr.Name = "colCurrLogicStr";
            this.colCurrLogicStr.Visible = true;
            this.colCurrLogicStr.VisibleIndex = 10;
            this.colCurrLogicStr.Width = 80;
            // 
            // colTaskDetailID
            // 
            this.colTaskDetailID.Caption = "任务明细ID";
            this.colTaskDetailID.FieldName = "TaskDetailID";
            this.colTaskDetailID.Name = "colTaskDetailID";
            this.colTaskDetailID.Visible = true;
            this.colTaskDetailID.VisibleIndex = 11;
            this.colTaskDetailID.Width = 80;
            // 
            // colIsBack
            // 
            this.colIsBack.Caption = "是否正在回待命点";
            this.colIsBack.FieldName = "IsBack";
            this.colIsBack.Name = "colIsBack";
            this.colIsBack.Visible = true;
            this.colIsBack.VisibleIndex = 12;
            this.colIsBack.Width = 116;
            // 
            // colErrorMes
            // 
            this.colErrorMes.Caption = "异常信息";
            this.colErrorMes.FieldName = "ErrorMessage";
            this.colErrorMes.MaxWidth = 200;
            this.colErrorMes.Name = "colErrorMes";
            this.colErrorMes.Visible = true;
            this.colErrorMes.VisibleIndex = 13;
            // 
            // colIsUpLand
            // 
            this.colIsUpLand.Caption = "是否在码上";
            this.colIsUpLand.FieldName = "IsUpQCodeStr";
            this.colIsUpLand.Name = "colIsUpLand";
            this.colIsUpLand.Visible = true;
            this.colIsUpLand.VisibleIndex = 14;
            this.colIsUpLand.Width = 80;
            // 
            // colBangState
            // 
            this.colBangState.Caption = "升举泵状态";
            this.colBangState.FieldName = "BangStateStr";
            this.colBangState.Name = "colBangState";
            this.colBangState.OptionsColumn.ReadOnly = true;
            this.colBangState.Visible = true;
            this.colBangState.VisibleIndex = 15;
            this.colBangState.Width = 80;
            // 
            // colJCState
            // 
            this.colJCState.Caption = "夹持状态";
            this.colJCState.FieldName = "JCStateStr";
            this.colJCState.Name = "colJCState";
            this.colJCState.Visible = true;
            this.colJCState.VisibleIndex = 16;
            // 
            // txtTrafficInfo
            // 
            this.txtTrafficInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTrafficInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTrafficInfo.Location = new System.Drawing.Point(0, 20);
            this.txtTrafficInfo.Multiline = true;
            this.txtTrafficInfo.Name = "txtTrafficInfo";
            this.txtTrafficInfo.Size = new System.Drawing.Size(1121, 117);
            this.txtTrafficInfo.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1121, 20);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(4, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "实时交管信息:";
            // 
            // tabNavigationPage3
            // 
            this.tabNavigationPage3.Caption = "运行日志";
            this.tabNavigationPage3.Controls.Add(this.listboxtask);
            this.tabNavigationPage3.Image = global::AGVDispatchServer.Properties.Resources.paste_32x32;
            this.tabNavigationPage3.ItemShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.tabNavigationPage3.Name = "tabNavigationPage3";
            this.tabNavigationPage3.Properties.ShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.tabNavigationPage3.Size = new System.Drawing.Size(1121, 457);
            // 
            // listboxtask
            // 
            this.listboxtask.Appearance.BackColor = System.Drawing.Color.White;
            this.listboxtask.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.listboxtask.Appearance.ForeColor = System.Drawing.Color.Black;
            this.listboxtask.Appearance.Options.UseBackColor = true;
            this.listboxtask.Appearance.Options.UseFont = true;
            this.listboxtask.Appearance.Options.UseForeColor = true;
            this.listboxtask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listboxtask.HorizontalScrollbar = true;
            this.listboxtask.Location = new System.Drawing.Point(0, 0);
            this.listboxtask.Name = "listboxtask";
            this.listboxtask.Size = new System.Drawing.Size(1121, 457);
            this.listboxtask.TabIndex = 1;
            // 
            // tabNavigationPage4
            // 
            this.tabNavigationPage4.Caption = "附加功能";
            this.tabNavigationPage4.Controls.Add(this.btnClearAllTask);
            this.tabNavigationPage4.Controls.Add(this.txtMesAgvSite);
            this.tabNavigationPage4.Controls.Add(this.txtMesAgvid);
            this.tabNavigationPage4.Controls.Add(this.labelControl12);
            this.tabNavigationPage4.Controls.Add(this.labelControl11);
            this.tabNavigationPage4.Controls.Add(this.btnTestMes);
            this.tabNavigationPage4.Controls.Add(this.labelControl10);
            this.tabNavigationPage4.Controls.Add(this.btnTestchazi);
            this.tabNavigationPage4.Controls.Add(this.txtchazi);
            this.tabNavigationPage4.Controls.Add(this.btnSetCarSite);
            this.tabNavigationPage4.Controls.Add(this.btnClearSysCarInfo);
            this.tabNavigationPage4.Controls.Add(this.btnStopCharge);
            this.tabNavigationPage4.Controls.Add(this.btnStartCharge);
            this.tabNavigationPage4.Controls.Add(this.txtChargeID);
            this.tabNavigationPage4.Controls.Add(this.labelControl9);
            this.tabNavigationPage4.Controls.Add(this.labelControl8);
            this.tabNavigationPage4.Controls.Add(this.btnBack);
            this.tabNavigationPage4.Controls.Add(this.txtBackAGV);
            this.tabNavigationPage4.Controls.Add(this.labelControl7);
            this.tabNavigationPage4.Controls.Add(this.labelControl6);
            this.tabNavigationPage4.Controls.Add(this.labelControl5);
            this.tabNavigationPage4.Controls.Add(this.txtAGVID);
            this.tabNavigationPage4.Controls.Add(this.labelControl4);
            this.tabNavigationPage4.Controls.Add(this.labelControl3);
            this.tabNavigationPage4.Controls.Add(this.txtArmLand);
            this.tabNavigationPage4.Controls.Add(this.btnTestTraffic);
            this.tabNavigationPage4.Controls.Add(this.labelControl2);
            this.tabNavigationPage4.Controls.Add(this.labelControl1);
            this.tabNavigationPage4.Controls.Add(this.btnCancelTask);
            this.tabNavigationPage4.Controls.Add(this.btnStorageReset);
            this.tabNavigationPage4.Controls.Add(this.btnSame);
            this.tabNavigationPage4.Controls.Add(this.btnBroadCast);
            this.tabNavigationPage4.Controls.Add(this.btnTestStop);
            this.tabNavigationPage4.Controls.Add(this.btnTestStart);
            this.tabNavigationPage4.Image = global::AGVDispatchServer.Properties.Resources.showall_32x32;
            this.tabNavigationPage4.ItemShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.tabNavigationPage4.Name = "tabNavigationPage4";
            this.tabNavigationPage4.Properties.ShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.tabNavigationPage4.Size = new System.Drawing.Size(1121, 457);
            // 
            // btnClearAllTask
            // 
            this.btnClearAllTask.Enabled = false;
            this.btnClearAllTask.Location = new System.Drawing.Point(659, 92);
            this.btnClearAllTask.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClearAllTask.Name = "btnClearAllTask";
            this.btnClearAllTask.Size = new System.Drawing.Size(86, 24);
            this.btnClearAllTask.TabIndex = 34;
            this.btnClearAllTask.Text = "清除所有任务";
            this.btnClearAllTask.Click += new System.EventHandler(this.btnClearAllTask_Click);
            // 
            // txtMesAgvSite
            // 
            this.txtMesAgvSite.Location = new System.Drawing.Point(295, 355);
            this.txtMesAgvSite.MenuManager = this.barManager1;
            this.txtMesAgvSite.Name = "txtMesAgvSite";
            this.txtMesAgvSite.Size = new System.Drawing.Size(100, 20);
            this.txtMesAgvSite.TabIndex = 33;
            // 
            // txtMesAgvid
            // 
            this.txtMesAgvid.Location = new System.Drawing.Point(108, 358);
            this.txtMesAgvid.MenuManager = this.barManager1;
            this.txtMesAgvid.Name = "txtMesAgvid";
            this.txtMesAgvid.Size = new System.Drawing.Size(100, 20);
            this.txtMesAgvid.TabIndex = 32;
            // 
            // labelControl12
            // 
            this.labelControl12.Location = new System.Drawing.Point(236, 361);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(28, 14);
            this.labelControl12.TabIndex = 31;
            this.labelControl12.Text = "站点:";
            // 
            // labelControl11
            // 
            this.labelControl11.Location = new System.Drawing.Point(52, 361);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(28, 14);
            this.labelControl11.TabIndex = 30;
            this.labelControl11.Text = "车号:";
            // 
            // btnTestMes
            // 
            this.btnTestMes.Location = new System.Drawing.Point(426, 357);
            this.btnTestMes.Name = "btnTestMes";
            this.btnTestMes.Size = new System.Drawing.Size(75, 23);
            this.btnTestMes.TabIndex = 29;
            this.btnTestMes.Text = "测试调用";
            this.btnTestMes.Click += new System.EventHandler(this.btnTestMes_Click);
            // 
            // labelControl10
            // 
            this.labelControl10.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl10.Location = new System.Drawing.Point(30, 331);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(69, 14);
            this.labelControl10.TabIndex = 28;
            this.labelControl10.Text = "测试Mes接口";
            // 
            // btnTestchazi
            // 
            this.btnTestchazi.Location = new System.Drawing.Point(550, 30);
            this.btnTestchazi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTestchazi.Name = "btnTestchazi";
            this.btnTestchazi.Size = new System.Drawing.Size(86, 24);
            this.btnTestchazi.TabIndex = 27;
            this.btnTestchazi.Text = "叉子升降";
            this.btnTestchazi.Click += new System.EventHandler(this.btnTestchazi_Click);
            // 
            // txtchazi
            // 
            this.txtchazi.Location = new System.Drawing.Point(445, 33);
            this.txtchazi.MenuManager = this.barManager1;
            this.txtchazi.Name = "txtchazi";
            this.txtchazi.Size = new System.Drawing.Size(100, 20);
            this.txtchazi.TabIndex = 26;
            // 
            // btnSetCarSite
            // 
            this.btnSetCarSite.Location = new System.Drawing.Point(346, 92);
            this.btnSetCarSite.Name = "btnSetCarSite";
            this.btnSetCarSite.Size = new System.Drawing.Size(107, 23);
            this.btnSetCarSite.TabIndex = 25;
            this.btnSetCarSite.Text = "设置车辆当前站点";
            this.btnSetCarSite.Click += new System.EventHandler(this.btnSetCarSite_Click);
            // 
            // btnClearSysCarInfo
            // 
            this.btnClearSysCarInfo.Location = new System.Drawing.Point(236, 92);
            this.btnClearSysCarInfo.Name = "btnClearSysCarInfo";
            this.btnClearSysCarInfo.Size = new System.Drawing.Size(104, 23);
            this.btnClearSysCarInfo.TabIndex = 24;
            this.btnClearSysCarInfo.Text = "清除车辆系统缓存";
            this.btnClearSysCarInfo.Click += new System.EventHandler(this.btnClearSysCarInfo_Click);
            // 
            // btnStopCharge
            // 
            this.btnStopCharge.Location = new System.Drawing.Point(336, 294);
            this.btnStopCharge.Name = "btnStopCharge";
            this.btnStopCharge.Size = new System.Drawing.Size(75, 23);
            this.btnStopCharge.TabIndex = 23;
            this.btnStopCharge.Text = "停止充电";
            this.btnStopCharge.Click += new System.EventHandler(this.btnStopCharge_Click);
            // 
            // btnStartCharge
            // 
            this.btnStartCharge.Location = new System.Drawing.Point(236, 294);
            this.btnStartCharge.Name = "btnStartCharge";
            this.btnStartCharge.Size = new System.Drawing.Size(75, 23);
            this.btnStartCharge.TabIndex = 22;
            this.btnStartCharge.Text = "开始充电";
            this.btnStartCharge.Click += new System.EventHandler(this.btnStartCharge_Click);
            // 
            // txtChargeID
            // 
            this.txtChargeID.Location = new System.Drawing.Point(108, 295);
            this.txtChargeID.MenuManager = this.barManager1;
            this.txtChargeID.Name = "txtChargeID";
            this.txtChargeID.Size = new System.Drawing.Size(100, 20);
            this.txtChargeID.TabIndex = 21;
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(50, 298);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(52, 14);
            this.labelControl9.TabIndex = 20;
            this.labelControl9.Text = "充电桩号:";
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl8.Location = new System.Drawing.Point(30, 268);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(84, 14);
            this.labelControl8.TabIndex = 19;
            this.labelControl8.Text = "自动充电桩测试";
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(214, 218);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(86, 23);
            this.btnBack.TabIndex = 18;
            this.btnBack.Text = "启动";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // txtBackAGV
            // 
            this.txtBackAGV.Location = new System.Drawing.Point(86, 219);
            this.txtBackAGV.MenuManager = this.barManager1;
            this.txtBackAGV.Name = "txtBackAGV";
            this.txtBackAGV.Size = new System.Drawing.Size(100, 20);
            this.txtBackAGV.TabIndex = 17;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(52, 222);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(28, 14);
            this.labelControl7.TabIndex = 16;
            this.labelControl7.Text = "车号:";
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl6.Location = new System.Drawing.Point(28, 192);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(48, 14);
            this.labelControl6.TabIndex = 15;
            this.labelControl6.Text = "回待命点";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl5.Location = new System.Drawing.Point(30, 132);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(72, 14);
            this.labelControl5.TabIndex = 14;
            this.labelControl5.Text = "启动指定AGV";
            // 
            // txtAGVID
            // 
            this.txtAGVID.Location = new System.Drawing.Point(248, 154);
            this.txtAGVID.MenuManager = this.barManager1;
            this.txtAGVID.Name = "txtAGVID";
            this.txtAGVID.Size = new System.Drawing.Size(100, 20);
            this.txtAGVID.TabIndex = 13;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(214, 156);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(28, 14);
            this.labelControl4.TabIndex = 12;
            this.labelControl4.Text = "车号:";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(52, 157);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(40, 14);
            this.labelControl3.TabIndex = 11;
            this.labelControl3.Text = "目的点:";
            // 
            // txtArmLand
            // 
            this.txtArmLand.Location = new System.Drawing.Point(98, 154);
            this.txtArmLand.MenuManager = this.barManager1;
            this.txtArmLand.Name = "txtArmLand";
            this.txtArmLand.Size = new System.Drawing.Size(106, 20);
            this.txtArmLand.TabIndex = 10;
            // 
            // btnTestTraffic
            // 
            this.btnTestTraffic.Location = new System.Drawing.Point(389, 153);
            this.btnTestTraffic.Name = "btnTestTraffic";
            this.btnTestTraffic.Size = new System.Drawing.Size(86, 23);
            this.btnTestTraffic.TabIndex = 8;
            this.btnTestTraffic.Text = "启动";
            this.btnTestTraffic.UseVisualStyleBackColor = true;
            this.btnTestTraffic.Click += new System.EventHandler(this.btnTestTraffic_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl2.Location = new System.Drawing.Point(29, 72);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 7;
            this.labelControl2.Text = "信息更新";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl1.Location = new System.Drawing.Point(28, 9);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(72, 14);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "车辆功能测试";
            // 
            // btnCancelTask
            // 
            this.btnCancelTask.Location = new System.Drawing.Point(236, 30);
            this.btnCancelTask.Name = "btnCancelTask";
            this.btnCancelTask.Size = new System.Drawing.Size(75, 23);
            this.btnCancelTask.TabIndex = 5;
            this.btnCancelTask.Text = "取消任务";
            this.btnCancelTask.Click += new System.EventHandler(this.btnCancelTask_Click);
            // 
            // btnStorageReset
            // 
            this.btnStorageReset.Location = new System.Drawing.Point(52, 91);
            this.btnStorageReset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStorageReset.Name = "btnStorageReset";
            this.btnStorageReset.Size = new System.Drawing.Size(86, 24);
            this.btnStorageReset.TabIndex = 4;
            this.btnStorageReset.Text = "储位状态调整";
            this.btnStorageReset.Click += new System.EventHandler(this.btnStorageReset_Click);
            // 
            // btnSame
            // 
            this.btnSame.Location = new System.Drawing.Point(144, 91);
            this.btnSame.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSame.Name = "btnSame";
            this.btnSame.Size = new System.Drawing.Size(86, 24);
            this.btnSame.TabIndex = 3;
            this.btnSame.Text = "同步基础档案";
            this.btnSame.Click += new System.EventHandler(this.btnSame_Click);
            // 
            // btnBroadCast
            // 
            this.btnBroadCast.Enabled = false;
            this.btnBroadCast.Location = new System.Drawing.Point(459, 91);
            this.btnBroadCast.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnBroadCast.Name = "btnBroadCast";
            this.btnBroadCast.Size = new System.Drawing.Size(86, 24);
            this.btnBroadCast.TabIndex = 2;
            this.btnBroadCast.Text = "广播平板订单";
            this.btnBroadCast.Click += new System.EventHandler(this.btnBroadCast_Click);
            // 
            // btnTestStop
            // 
            this.btnTestStop.Location = new System.Drawing.Point(144, 29);
            this.btnTestStop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTestStop.Name = "btnTestStop";
            this.btnTestStop.Size = new System.Drawing.Size(86, 24);
            this.btnTestStop.TabIndex = 1;
            this.btnTestStop.Text = "停止";
            this.btnTestStop.Click += new System.EventHandler(this.btnTestStop_Click);
            // 
            // btnTestStart
            // 
            this.btnTestStart.Location = new System.Drawing.Point(52, 29);
            this.btnTestStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTestStart.Name = "btnTestStart";
            this.btnTestStart.Size = new System.Drawing.Size(86, 24);
            this.btnTestStart.TabIndex = 0;
            this.btnTestStart.Text = "启动";
            this.btnTestStart.Click += new System.EventHandler(this.btnTestStart_Click);
            // 
            // tabNavigationPage2
            // 
            this.tabNavigationPage2.Caption = "任务管理";
            this.tabNavigationPage2.Controls.Add(this.panelControl2);
            this.tabNavigationPage2.Controls.Add(this.panelControl1);
            this.tabNavigationPage2.Image = global::AGVDispatchServer.Properties.Resources.namemanager_32x32;
            this.tabNavigationPage2.ItemShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.tabNavigationPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabNavigationPage2.Name = "tabNavigationPage2";
            this.tabNavigationPage2.Properties.ShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.tabNavigationPage2.Size = new System.Drawing.Size(1121, 457);
            // 
            // panelControl2
            // 
            this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl2.Controls.Add(this.panelControl3);
            this.panelControl2.Controls.Add(this.panelControl4);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 32);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1121, 425);
            this.panelControl2.TabIndex = 2;
            // 
            // panelControl3
            // 
            this.panelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl3.Controls.Add(this.gcTask);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(1121, 300);
            this.panelControl3.TabIndex = 4;
            // 
            // gcTask
            // 
            this.gcTask.DataSource = this.bsTask;
            this.gcTask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcTask.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcTask.Location = new System.Drawing.Point(0, 0);
            this.gcTask.MainView = this.gvTask;
            this.gcTask.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gcTask.MenuManager = this.barManager1;
            this.gcTask.Name = "gcTask";
            this.gcTask.Size = new System.Drawing.Size(1121, 300);
            this.gcTask.TabIndex = 1;
            this.gcTask.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvTask});
            // 
            // bsTask
            // 
            this.bsTask.DataSource = typeof(Model.MDM.DispatchTaskInfo);
            // 
            // gvTask
            // 
            this.gvTask.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colselect,
            this.colsite,
            this.coltasktype,
            this.coltaskstate,
            this.colexeagv,
            this.colEndLand,
            this.colStorageName});
            this.gvTask.GridControl = this.gcTask;
            this.gvTask.Name = "gvTask";
            this.gvTask.OptionsView.ShowFooter = true;
            this.gvTask.OptionsView.ShowGroupPanel = false;
            this.gvTask.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvTask_FocusedRowChanged);
            // 
            // colselect
            // 
            this.colselect.Caption = "选择";
            this.colselect.FieldName = "IsSelect";
            this.colselect.Name = "colselect";
            // 
            // colsite
            // 
            this.colsite.Caption = "呼叫盒ID";
            this.colsite.FieldName = "stationNo";
            this.colsite.Name = "colsite";
            this.colsite.OptionsColumn.AllowEdit = false;
            this.colsite.Visible = true;
            this.colsite.VisibleIndex = 0;
            // 
            // coltasktype
            // 
            this.coltasktype.Caption = "任务类型";
            this.coltasktype.FieldName = "taskTypeStr";
            this.coltasktype.Name = "coltasktype";
            this.coltasktype.OptionsColumn.AllowEdit = false;
            this.coltasktype.Visible = true;
            this.coltasktype.VisibleIndex = 1;
            // 
            // coltaskstate
            // 
            this.coltaskstate.Caption = "任务状态";
            this.coltaskstate.FieldName = "TaskStateStr";
            this.coltaskstate.Name = "coltaskstate";
            this.coltaskstate.OptionsColumn.AllowEdit = false;
            this.coltaskstate.Visible = true;
            this.coltaskstate.VisibleIndex = 2;
            // 
            // colexeagv
            // 
            this.colexeagv.Caption = "执行小车";
            this.colexeagv.FieldName = "ExeAgv";
            this.colexeagv.Name = "colexeagv";
            this.colexeagv.OptionsColumn.AllowEdit = false;
            this.colexeagv.Visible = true;
            this.colexeagv.VisibleIndex = 3;
            // 
            // colEndLand
            // 
            this.colEndLand.Caption = "呼叫目的地标";
            this.colEndLand.FieldName = "CallLand";
            this.colEndLand.Name = "colEndLand";
            this.colEndLand.OptionsColumn.AllowEdit = false;
            this.colEndLand.Visible = true;
            this.colEndLand.VisibleIndex = 4;
            // 
            // colStorageName
            // 
            this.colStorageName.Caption = "呼叫储位名称";
            this.colStorageName.FieldName = "StorageName";
            this.colStorageName.Name = "colStorageName";
            this.colStorageName.OptionsColumn.AllowEdit = false;
            this.colStorageName.Visible = true;
            this.colStorageName.VisibleIndex = 5;
            // 
            // panelControl4
            // 
            this.panelControl4.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl4.Controls.Add(this.gcDetail);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl4.Location = new System.Drawing.Point(0, 300);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(1121, 125);
            this.panelControl4.TabIndex = 3;
            // 
            // gcDetail
            // 
            this.gcDetail.DataSource = this.bsTaskDetail;
            this.gcDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDetail.Location = new System.Drawing.Point(0, 0);
            this.gcDetail.MainView = this.gvDetail;
            this.gcDetail.MenuManager = this.barManager1;
            this.gcDetail.Name = "gcDetail";
            this.gcDetail.Size = new System.Drawing.Size(1121, 125);
            this.gcDetail.TabIndex = 0;
            this.gcDetail.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDetail});
            // 
            // bsTaskDetail
            // 
            this.bsTaskDetail.DataSource = typeof(Model.MDM.DispatchTaskDetail);
            // 
            // gvDetail
            // 
            this.gvDetail.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gvDetail.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.clSelect,
            this.colDetailID,
            this.colLandCode,
            this.golStorageName,
            this.colOperType,
            this.colDetailState,
            this.colIsAllowExcute});
            this.gvDetail.GridControl = this.gcDetail;
            this.gvDetail.Name = "gvDetail";
            this.gvDetail.OptionsView.ShowDetailButtons = false;
            this.gvDetail.OptionsView.ShowGroupPanel = false;
            this.gvDetail.OptionsView.ShowIndicator = false;
            // 
            // clSelect
            // 
            this.clSelect.Caption = "选择";
            this.clSelect.FieldName = "IsSelect";
            this.clSelect.Name = "clSelect";
            this.clSelect.Visible = true;
            this.clSelect.VisibleIndex = 0;
            // 
            // colDetailID
            // 
            this.colDetailID.Caption = "序号";
            this.colDetailID.FieldName = "DetailID";
            this.colDetailID.Name = "colDetailID";
            this.colDetailID.OptionsColumn.AllowEdit = false;
            this.colDetailID.Visible = true;
            this.colDetailID.VisibleIndex = 1;
            // 
            // colLandCode
            // 
            this.colLandCode.Caption = "目的地标";
            this.colLandCode.FieldName = "LandCode";
            this.colLandCode.Name = "colLandCode";
            this.colLandCode.OptionsColumn.AllowEdit = false;
            this.colLandCode.Visible = true;
            this.colLandCode.VisibleIndex = 2;
            // 
            // golStorageName
            // 
            this.golStorageName.Caption = "呼叫储位名称";
            this.golStorageName.FieldName = "StorageName";
            this.golStorageName.Name = "golStorageName";
            this.golStorageName.OptionsColumn.AllowEdit = false;
            this.golStorageName.Visible = true;
            this.golStorageName.VisibleIndex = 3;
            // 
            // colOperType
            // 
            this.colOperType.Caption = "操作类型";
            this.colOperType.FieldName = "OperTypeStr";
            this.colOperType.Name = "colOperType";
            this.colOperType.OptionsColumn.AllowEdit = false;
            this.colOperType.Visible = true;
            this.colOperType.VisibleIndex = 4;
            // 
            // colDetailState
            // 
            this.colDetailState.Caption = "任务状态";
            this.colDetailState.FieldName = "TaskStateStr";
            this.colDetailState.Name = "colDetailState";
            this.colDetailState.OptionsColumn.AllowEdit = false;
            this.colDetailState.Visible = true;
            this.colDetailState.VisibleIndex = 5;
            // 
            // colIsAllowExcute
            // 
            this.colIsAllowExcute.Caption = "是否允许执行";
            this.colIsAllowExcute.FieldName = "IsAllowExcute";
            this.colIsAllowExcute.Name = "colIsAllowExcute";
            this.colIsAllowExcute.OptionsColumn.AllowEdit = false;
            this.colIsAllowExcute.Visible = true;
            this.colIsAllowExcute.VisibleIndex = 6;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnFinish);
            this.panelControl1.Controls.Add(this.btnReDo);
            this.panelControl1.Controls.Add(this.btnRefsh);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1121, 32);
            this.panelControl1.TabIndex = 0;
            // 
            // btnFinish
            // 
            this.btnFinish.Location = new System.Drawing.Point(75, 4);
            this.btnFinish.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(67, 23);
            this.btnFinish.TabIndex = 2;
            this.btnFinish.Text = "完成";
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // btnReDo
            // 
            this.btnReDo.Location = new System.Drawing.Point(4, 4);
            this.btnReDo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnReDo.Name = "btnReDo";
            this.btnReDo.Size = new System.Drawing.Size(66, 23);
            this.btnReDo.TabIndex = 1;
            this.btnReDo.Text = "重做";
            this.btnReDo.Click += new System.EventHandler(this.btnReDo_Click);
            // 
            // btnRefsh
            // 
            this.btnRefsh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefsh.Location = new System.Drawing.Point(1031, 4);
            this.btnRefsh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRefsh.Name = "btnRefsh";
            this.btnRefsh.Size = new System.Drawing.Size(85, 24);
            this.btnRefsh.TabIndex = 0;
            this.btnRefsh.Text = "刷新";
            this.btnRefsh.Click += new System.EventHandler(this.btnRefsh_Click);
            // 
            // tabNavigationPage5
            // 
            this.tabNavigationPage5.Caption = "充电桩信息";
            this.tabNavigationPage5.Controls.Add(this.panelControl5);
            this.tabNavigationPage5.Image = global::AGVDispatchServer.Properties.Resources.Charge;
            this.tabNavigationPage5.ItemShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.tabNavigationPage5.Name = "tabNavigationPage5";
            this.tabNavigationPage5.Properties.ShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.tabNavigationPage5.Size = new System.Drawing.Size(1121, 457);
            // 
            // panelControl5
            // 
            this.panelControl5.Controls.Add(this.gcChargeInfo);
            this.panelControl5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl5.Location = new System.Drawing.Point(0, 0);
            this.panelControl5.Name = "panelControl5";
            this.panelControl5.Size = new System.Drawing.Size(1121, 457);
            this.panelControl5.TabIndex = 0;
            // 
            // gcChargeInfo
            // 
            this.gcChargeInfo.DataSource = this.bsChargeInfo;
            this.gcChargeInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcChargeInfo.Location = new System.Drawing.Point(2, 2);
            this.gcChargeInfo.MainView = this.gvChargeInfo;
            this.gcChargeInfo.MenuManager = this.barManager1;
            this.gcChargeInfo.Name = "gcChargeInfo";
            this.gcChargeInfo.Size = new System.Drawing.Size(1117, 453);
            this.gcChargeInfo.TabIndex = 0;
            this.gcChargeInfo.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvChargeInfo});
            // 
            // bsChargeInfo
            // 
            this.bsChargeInfo.DataSource = typeof(Model.MDM.ChargeStationInfo);
            // 
            // gvChargeInfo
            // 
            this.gvChargeInfo.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gvChargeInfo.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colIsBreak,
            this.colStateStr,
            this.colIP,
            this.colPort,
            this.clLandCode});
            this.gvChargeInfo.GridControl = this.gcChargeInfo;
            this.gvChargeInfo.Name = "gvChargeInfo";
            this.gvChargeInfo.OptionsBehavior.Editable = false;
            this.gvChargeInfo.OptionsBehavior.ReadOnly = true;
            this.gvChargeInfo.OptionsCustomization.AllowFilter = false;
            this.gvChargeInfo.OptionsCustomization.AllowSort = false;
            this.gvChargeInfo.OptionsView.ShowDetailButtons = false;
            this.gvChargeInfo.OptionsView.ShowGroupPanel = false;
            this.gvChargeInfo.OptionsView.ShowIndicator = false;
            // 
            // colID
            // 
            this.colID.Caption = "ID";
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            this.colID.Visible = true;
            this.colID.VisibleIndex = 0;
            // 
            // colIsBreak
            // 
            this.colIsBreak.Caption = "是否中断";
            this.colIsBreak.FieldName = "IsCommBreak";
            this.colIsBreak.Name = "colIsBreak";
            this.colIsBreak.Visible = true;
            this.colIsBreak.VisibleIndex = 2;
            // 
            // colStateStr
            // 
            this.colStateStr.Caption = "状态";
            this.colStateStr.FieldName = "ChargeStateStr";
            this.colStateStr.Name = "colStateStr";
            this.colStateStr.Visible = true;
            this.colStateStr.VisibleIndex = 1;
            // 
            // colIP
            // 
            this.colIP.Caption = "IP地址";
            this.colIP.FieldName = "IP";
            this.colIP.Name = "colIP";
            this.colIP.Visible = true;
            this.colIP.VisibleIndex = 3;
            // 
            // colPort
            // 
            this.colPort.Caption = "端口号";
            this.colPort.FieldName = "Port";
            this.colPort.Name = "colPort";
            this.colPort.Visible = true;
            this.colPort.VisibleIndex = 4;
            // 
            // clLandCode
            // 
            this.clLandCode.Caption = "停止地标";
            this.clLandCode.FieldName = "ChargeLandCode";
            this.clLandCode.Name = "clLandCode";
            this.clLandCode.Visible = true;
            this.clLandCode.VisibleIndex = 5;
            // 
            // npIOInfo
            // 
            this.npIOInfo.Caption = "远程IO信息";
            this.npIOInfo.Controls.Add(this.gcIOInfo);
            this.npIOInfo.Image = ((System.Drawing.Image)(resources.GetObject("npIOInfo.Image")));
            this.npIOInfo.ItemShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.npIOInfo.Name = "npIOInfo";
            this.npIOInfo.Properties.ShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.npIOInfo.Size = new System.Drawing.Size(1121, 457);
            // 
            // gcIOInfo
            // 
            this.gcIOInfo.DataSource = this.bsIOInfoes;
            this.gcIOInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcIOInfo.Location = new System.Drawing.Point(0, 0);
            this.gcIOInfo.MainView = this.gvIOInfo;
            this.gcIOInfo.MenuManager = this.barManager1;
            this.gcIOInfo.Name = "gcIOInfo";
            this.gcIOInfo.Size = new System.Drawing.Size(1121, 457);
            this.gcIOInfo.TabIndex = 0;
            this.gcIOInfo.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvIOInfo});
            // 
            // bsIOInfoes
            // 
            this.bsIOInfoes.DataSource = typeof(Model.MSM.IODeviceInfo);
            // 
            // gvIOInfo
            // 
            this.gvIOInfo.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gvIOInfo.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colIOID,
            this.colDeviceName,
            this.colbIsCommBreak,
            this.colIOIP,
            this.colIOPort});
            this.gvIOInfo.GridControl = this.gcIOInfo;
            this.gvIOInfo.Name = "gvIOInfo";
            this.gvIOInfo.OptionsBehavior.Editable = false;
            this.gvIOInfo.OptionsBehavior.ReadOnly = true;
            this.gvIOInfo.OptionsCustomization.AllowColumnMoving = false;
            this.gvIOInfo.OptionsCustomization.AllowFilter = false;
            this.gvIOInfo.OptionsCustomization.AllowSort = false;
            this.gvIOInfo.OptionsView.ShowDetailButtons = false;
            this.gvIOInfo.OptionsView.ShowGroupPanel = false;
            this.gvIOInfo.OptionsView.ShowIndicator = false;
            // 
            // colIOID
            // 
            this.colIOID.Caption = "IO设备IO";
            this.colIOID.FieldName = "ID";
            this.colIOID.Name = "colIOID";
            this.colIOID.Visible = true;
            this.colIOID.VisibleIndex = 1;
            // 
            // colDeviceName
            // 
            this.colDeviceName.Caption = "IO设备名称";
            this.colDeviceName.FieldName = "DeviceName";
            this.colDeviceName.Name = "colDeviceName";
            this.colDeviceName.Visible = true;
            this.colDeviceName.VisibleIndex = 2;
            // 
            // colbIsCommBreak
            // 
            this.colbIsCommBreak.Caption = "是否通信中断";
            this.colbIsCommBreak.FieldName = "bIsCommBreak";
            this.colbIsCommBreak.Name = "colbIsCommBreak";
            this.colbIsCommBreak.Visible = true;
            this.colbIsCommBreak.VisibleIndex = 0;
            // 
            // colIOIP
            // 
            this.colIOIP.Caption = "IO设备地址";
            this.colIOIP.FieldName = "IP";
            this.colIOIP.Name = "colIOIP";
            this.colIOIP.Visible = true;
            this.colIOIP.VisibleIndex = 3;
            // 
            // colIOPort
            // 
            this.colIOPort.Caption = "IO设备端口号";
            this.colIOPort.FieldName = "Port";
            this.colIOPort.Name = "colIOPort";
            this.colIOPort.Visible = true;
            this.colIOPort.VisibleIndex = 4;
            // 
            // tabNavigationPage6
            // 
            this.tabNavigationPage6.Caption = "模拟仿真";
            this.tabNavigationPage6.Controls.Add(this.outputedLabel);
            this.tabNavigationPage6.Controls.Add(this.labelControl26);
            this.tabNavigationPage6.Controls.Add(this.inputedLabel);
            this.tabNavigationPage6.Controls.Add(this.labelControl24);
            this.tabNavigationPage6.Controls.Add(this.outputTotalLabel);
            this.tabNavigationPage6.Controls.Add(this.labelControl22);
            this.tabNavigationPage6.Controls.Add(this.inputTotalLabel);
            this.tabNavigationPage6.Controls.Add(this.labelControl20);
            this.tabNavigationPage6.Controls.Add(this.totalStorageLabel);
            this.tabNavigationPage6.Controls.Add(this.totalStorageUsedLabel);
            this.tabNavigationPage6.Controls.Add(this.labelControl19);
            this.tabNavigationPage6.Controls.Add(this.labelControl18);
            this.tabNavigationPage6.Controls.Add(this.startTest);
            this.tabNavigationPage6.Controls.Add(this.outputWaitLabel);
            this.tabNavigationPage6.Controls.Add(this.inputWaitLabel);
            this.tabNavigationPage6.Controls.Add(this.totalStorageLeafLabel);
            this.tabNavigationPage6.Controls.Add(this.checkBox5);
            this.tabNavigationPage6.Controls.Add(this.checkBox4);
            this.tabNavigationPage6.Controls.Add(this.checkBox3);
            this.tabNavigationPage6.Controls.Add(this.checkBox2);
            this.tabNavigationPage6.Controls.Add(this.checkBox1);
            this.tabNavigationPage6.Controls.Add(this.labelControl17);
            this.tabNavigationPage6.Controls.Add(this.labelControl16);
            this.tabNavigationPage6.Controls.Add(this.labelControl15);
            this.tabNavigationPage6.Controls.Add(this.xxxxxxx);
            this.tabNavigationPage6.Controls.Add(this.addOutputTask);
            this.tabNavigationPage6.Controls.Add(this.labelControl13);
            this.tabNavigationPage6.Controls.Add(this.addInputTask);
            this.tabNavigationPage6.Image = ((System.Drawing.Image)(resources.GetObject("tabNavigationPage6.Image")));
            this.tabNavigationPage6.ItemShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.tabNavigationPage6.Name = "tabNavigationPage6";
            this.tabNavigationPage6.Properties.ShowMode = DevExpress.XtraBars.Navigation.ItemShowMode.ImageAndText;
            this.tabNavigationPage6.Size = new System.Drawing.Size(1121, 457);
            this.tabNavigationPage6.Paint += new System.Windows.Forms.PaintEventHandler(this.tabNavigationPage6_Paint);
            // 
            // outputedLabel
            // 
            this.outputedLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputedLabel.Appearance.ForeColor = System.Drawing.Color.Green;
            this.outputedLabel.Location = new System.Drawing.Point(377, 320);
            this.outputedLabel.Name = "outputedLabel";
            this.outputedLabel.Size = new System.Drawing.Size(24, 19);
            this.outputedLabel.TabIndex = 34;
            this.outputedLabel.Text = "----";
            // 
            // labelControl26
            // 
            this.labelControl26.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl26.Appearance.ForeColor = System.Drawing.Color.Black;
            this.labelControl26.Location = new System.Drawing.Point(307, 320);
            this.labelControl26.Name = "labelControl26";
            this.labelControl26.Size = new System.Drawing.Size(64, 19);
            this.labelControl26.TabIndex = 33;
            this.labelControl26.Text = "已出库：";
            // 
            // inputedLabel
            // 
            this.inputedLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputedLabel.Appearance.ForeColor = System.Drawing.Color.Green;
            this.inputedLabel.Location = new System.Drawing.Point(377, 251);
            this.inputedLabel.Name = "inputedLabel";
            this.inputedLabel.Size = new System.Drawing.Size(24, 19);
            this.inputedLabel.TabIndex = 32;
            this.inputedLabel.Text = "----";
            // 
            // labelControl24
            // 
            this.labelControl24.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl24.Appearance.ForeColor = System.Drawing.Color.Black;
            this.labelControl24.Location = new System.Drawing.Point(307, 251);
            this.labelControl24.Name = "labelControl24";
            this.labelControl24.Size = new System.Drawing.Size(64, 19);
            this.labelControl24.TabIndex = 31;
            this.labelControl24.Text = "已入库：";
            // 
            // outputTotalLabel
            // 
            this.outputTotalLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputTotalLabel.Appearance.ForeColor = System.Drawing.Color.Green;
            this.outputTotalLabel.Location = new System.Drawing.Point(179, 320);
            this.outputTotalLabel.Name = "outputTotalLabel";
            this.outputTotalLabel.Size = new System.Drawing.Size(24, 19);
            this.outputTotalLabel.TabIndex = 30;
            this.outputTotalLabel.Text = "----";
            // 
            // labelControl22
            // 
            this.labelControl22.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl22.Appearance.ForeColor = System.Drawing.Color.Black;
            this.labelControl22.Location = new System.Drawing.Point(109, 320);
            this.labelControl22.Name = "labelControl22";
            this.labelControl22.Size = new System.Drawing.Size(64, 19);
            this.labelControl22.TabIndex = 29;
            this.labelControl22.Text = "总出库：";
            // 
            // inputTotalLabel
            // 
            this.inputTotalLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputTotalLabel.Appearance.ForeColor = System.Drawing.Color.Green;
            this.inputTotalLabel.Location = new System.Drawing.Point(179, 251);
            this.inputTotalLabel.Name = "inputTotalLabel";
            this.inputTotalLabel.Size = new System.Drawing.Size(24, 19);
            this.inputTotalLabel.TabIndex = 28;
            this.inputTotalLabel.Text = "----";
            // 
            // labelControl20
            // 
            this.labelControl20.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl20.Appearance.ForeColor = System.Drawing.Color.Black;
            this.labelControl20.Location = new System.Drawing.Point(109, 251);
            this.labelControl20.Name = "labelControl20";
            this.labelControl20.Size = new System.Drawing.Size(64, 19);
            this.labelControl20.TabIndex = 27;
            this.labelControl20.Text = "总入库：";
            // 
            // totalStorageLabel
            // 
            this.totalStorageLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalStorageLabel.Appearance.ForeColor = System.Drawing.Color.Green;
            this.totalStorageLabel.Location = new System.Drawing.Point(179, 184);
            this.totalStorageLabel.Name = "totalStorageLabel";
            this.totalStorageLabel.Size = new System.Drawing.Size(24, 19);
            this.totalStorageLabel.TabIndex = 26;
            this.totalStorageLabel.Text = "----";
            // 
            // totalStorageUsedLabel
            // 
            this.totalStorageUsedLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalStorageUsedLabel.Appearance.ForeColor = System.Drawing.Color.Green;
            this.totalStorageUsedLabel.Location = new System.Drawing.Point(377, 184);
            this.totalStorageUsedLabel.Name = "totalStorageUsedLabel";
            this.totalStorageUsedLabel.Size = new System.Drawing.Size(24, 19);
            this.totalStorageUsedLabel.TabIndex = 25;
            this.totalStorageUsedLabel.Text = "----";
            // 
            // labelControl19
            // 
            this.labelControl19.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl19.Appearance.ForeColor = System.Drawing.Color.Black;
            this.labelControl19.Location = new System.Drawing.Point(514, 184);
            this.labelControl19.Name = "labelControl19";
            this.labelControl19.Size = new System.Drawing.Size(48, 19);
            this.labelControl19.TabIndex = 24;
            this.labelControl19.Text = "剩余：";
            // 
            // labelControl18
            // 
            this.labelControl18.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl18.Appearance.ForeColor = System.Drawing.Color.Black;
            this.labelControl18.Location = new System.Drawing.Point(307, 184);
            this.labelControl18.Name = "labelControl18";
            this.labelControl18.Size = new System.Drawing.Size(64, 19);
            this.labelControl18.TabIndex = 23;
            this.labelControl18.Text = "已使用：";
            // 
            // startTest
            // 
            this.startTest.Location = new System.Drawing.Point(530, 408);
            this.startTest.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.startTest.Name = "startTest";
            this.startTest.Size = new System.Drawing.Size(94, 24);
            this.startTest.TabIndex = 22;
            this.startTest.Text = "开始模拟";
            this.startTest.Click += new System.EventHandler(this.startTest_Click);
            // 
            // outputWaitLabel
            // 
            this.outputWaitLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputWaitLabel.Appearance.ForeColor = System.Drawing.Color.Green;
            this.outputWaitLabel.Location = new System.Drawing.Point(584, 319);
            this.outputWaitLabel.Name = "outputWaitLabel";
            this.outputWaitLabel.Size = new System.Drawing.Size(24, 19);
            this.outputWaitLabel.TabIndex = 21;
            this.outputWaitLabel.Text = "----";
            // 
            // inputWaitLabel
            // 
            this.inputWaitLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputWaitLabel.Appearance.ForeColor = System.Drawing.Color.Green;
            this.inputWaitLabel.Location = new System.Drawing.Point(584, 251);
            this.inputWaitLabel.Name = "inputWaitLabel";
            this.inputWaitLabel.Size = new System.Drawing.Size(24, 19);
            this.inputWaitLabel.TabIndex = 20;
            this.inputWaitLabel.Text = "----";
            // 
            // totalStorageLeafLabel
            // 
            this.totalStorageLeafLabel.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalStorageLeafLabel.Appearance.ForeColor = System.Drawing.Color.Green;
            this.totalStorageLeafLabel.Location = new System.Drawing.Point(584, 184);
            this.totalStorageLeafLabel.Name = "totalStorageLeafLabel";
            this.totalStorageLeafLabel.Size = new System.Drawing.Size(24, 19);
            this.totalStorageLeafLabel.TabIndex = 19;
            this.totalStorageLeafLabel.Text = "----";
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Checked = true;
            this.checkBox5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox5.Enabled = false;
            this.checkBox5.Location = new System.Drawing.Point(533, 71);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(57, 18);
            this.checkBox5.TabIndex = 17;
            this.checkBox5.Text = "5号车";
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.Enabled = false;
            this.checkBox4.Location = new System.Drawing.Point(415, 71);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(57, 18);
            this.checkBox4.TabIndex = 16;
            this.checkBox4.Text = "4号车";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Enabled = false;
            this.checkBox3.Location = new System.Drawing.Point(307, 71);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(57, 18);
            this.checkBox3.TabIndex = 15;
            this.checkBox3.Text = "3号车";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Enabled = false;
            this.checkBox2.Location = new System.Drawing.Point(203, 71);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(57, 18);
            this.checkBox2.TabIndex = 14;
            this.checkBox2.Text = "2号车";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(94, 71);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(57, 18);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "1号车";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // labelControl17
            // 
            this.labelControl17.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl17.Appearance.ForeColor = System.Drawing.Color.Black;
            this.labelControl17.Location = new System.Drawing.Point(46, 29);
            this.labelControl17.Name = "labelControl17";
            this.labelControl17.Size = new System.Drawing.Size(63, 24);
            this.labelControl17.TabIndex = 12;
            this.labelControl17.Text = "车辆：";
            // 
            // labelControl16
            // 
            this.labelControl16.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl16.Appearance.ForeColor = System.Drawing.Color.Black;
            this.labelControl16.Location = new System.Drawing.Point(514, 319);
            this.labelControl16.Name = "labelControl16";
            this.labelControl16.Size = new System.Drawing.Size(64, 19);
            this.labelControl16.TabIndex = 11;
            this.labelControl16.Text = "待出库：";
            // 
            // labelControl15
            // 
            this.labelControl15.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl15.Appearance.ForeColor = System.Drawing.Color.Black;
            this.labelControl15.Location = new System.Drawing.Point(514, 251);
            this.labelControl15.Name = "labelControl15";
            this.labelControl15.Size = new System.Drawing.Size(64, 19);
            this.labelControl15.TabIndex = 10;
            this.labelControl15.Text = "待入库：";
            // 
            // xxxxxxx
            // 
            this.xxxxxxx.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xxxxxxx.Appearance.ForeColor = System.Drawing.Color.Black;
            this.xxxxxxx.Location = new System.Drawing.Point(109, 184);
            this.xxxxxxx.Name = "xxxxxxx";
            this.xxxxxxx.Size = new System.Drawing.Size(64, 19);
            this.xxxxxxx.TabIndex = 9;
            this.xxxxxxx.Text = "总容量：";
            // 
            // addOutputTask
            // 
            this.addOutputTask.Enabled = false;
            this.addOutputTask.Location = new System.Drawing.Point(729, 319);
            this.addOutputTask.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.addOutputTask.Name = "addOutputTask";
            this.addOutputTask.Size = new System.Drawing.Size(94, 24);
            this.addOutputTask.TabIndex = 8;
            this.addOutputTask.Text = "添加出库任务";
            this.addOutputTask.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // labelControl13
            // 
            this.labelControl13.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl13.Appearance.ForeColor = System.Drawing.Color.Black;
            this.labelControl13.Location = new System.Drawing.Point(46, 122);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(105, 24);
            this.labelControl13.TabIndex = 7;
            this.labelControl13.Text = "库存管理：";
            // 
            // addInputTask
            // 
            this.addInputTask.Enabled = false;
            this.addInputTask.Location = new System.Drawing.Point(729, 246);
            this.addInputTask.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.addInputTask.Name = "addInputTask";
            this.addInputTask.Size = new System.Drawing.Size(94, 24);
            this.addInputTask.TabIndex = 5;
            this.addInputTask.Text = "添加入库任务";
            this.addInputTask.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // LF
            // 
            this.LF.EnableBonusSkins = true;
            // 
            // timerClearMemory
            // 
            this.timerClearMemory.Tick += new System.EventHandler(this.timerClearMemory_Tick);
            // 
            // notify
            // 
            this.notify.Icon = ((System.Drawing.Icon)(resources.GetObject("notify.Icon")));
            this.notify.Tag = "AGV调度服务";
            this.notify.Text = "AGV调度服务";
            this.notify.Visible = true;
            this.notify.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // timerFormRefsh
            // 
            this.timerFormRefsh.Interval = 1000;
            this.timerFormRefsh.Tick += new System.EventHandler(this.timerFormRefsh_Tick);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1139, 552);
            this.Controls.Add(this.tpInfo);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmMain";
            this.Text = "AGV调度服务";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Shown += new System.EventHandler(this.FrmMain_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection24)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.tpInfo.ResumeLayout(false);
            this.tabNavigationPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcCars)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCars)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvcars)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabNavigationPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listboxtask)).EndInit();
            this.tabNavigationPage4.ResumeLayout(false);
            this.tabNavigationPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMesAgvSite.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMesAgvid.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtchazi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChargeID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBackAGV.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAGVID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtArmLand.Properties)).EndInit();
            this.tabNavigationPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcTask)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTask)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTask)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTaskDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.tabNavigationPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).EndInit();
            this.panelControl5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcChargeInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsChargeInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvChargeInfo)).EndInit();
            this.npIOInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcIOInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsIOInfoes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvIOInfo)).EndInit();
            this.tabNavigationPage6.ResumeLayout(false);
            this.tabNavigationPage6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.Navigation.TabPane tpInfo;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage1;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage3;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage4;
        private DevExpress.LookAndFeel.DefaultLookAndFeel LF;
        private DevExpress.XtraBars.BarButtonItem btnStart;
        private DevExpress.XtraBars.BarButtonItem btnStop;
        private DevExpress.XtraBars.BarButtonItem btnDBSet;
        private DevExpress.XtraBars.SkinBarSubItem btnSkinSet;
        private DevExpress.XtraGrid.GridControl gcCars;
        private DevExpress.XtraGrid.Views.Grid.GridView gvcars;
        private DevExpress.XtraEditors.ListBoxControl listboxtask;
        private System.Windows.Forms.BindingSource bsCars;
        private DevExpress.XtraGrid.Columns.GridColumn colAgvID;
        private System.Windows.Forms.Timer timerClearMemory;
        private System.Windows.Forms.NotifyIcon notify;
        private DevExpress.XtraBars.BarButtonItem btnExit;
        private System.Windows.Forms.Timer timerFormRefsh;
        private DevExpress.XtraGrid.Columns.GridColumn colcurrsite;
        private DevExpress.XtraGrid.Columns.GridColumn colfVolt;
        private DevExpress.XtraGrid.Columns.GridColumn colIsCommBreak;
        private DevExpress.XtraGrid.Columns.GridColumn colCarState;
        private DevExpress.XtraGrid.Columns.GridColumn colLowPower;
        private DevExpress.XtraGrid.Columns.GridColumn colspeed;
        private DevExpress.XtraEditors.SimpleButton btnTestStop;
        private DevExpress.XtraEditors.SimpleButton btnTestStart;
        private DevExpress.XtraEditors.SimpleButton btnBroadCast;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage2;
        private DevExpress.XtraGrid.GridControl gcTask;
        private DevExpress.XtraGrid.Views.Grid.GridView gvTask;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnRefsh;
        private System.Windows.Forms.BindingSource bsTask;
        private DevExpress.XtraGrid.Columns.GridColumn colselect;
        private DevExpress.XtraGrid.Columns.GridColumn colsite;
        private DevExpress.XtraGrid.Columns.GridColumn coltasktype;
        private DevExpress.XtraGrid.Columns.GridColumn coltaskstate;
        private DevExpress.XtraGrid.Columns.GridColumn colexeagv;
        private DevExpress.XtraEditors.SimpleButton btnFinish;
        private DevExpress.XtraEditors.SimpleButton btnReDo;
        private DevExpress.XtraGrid.Columns.GridColumn colCurrLogicStr;
        private DevExpress.XtraEditors.SimpleButton btnSame;
        private DevExpress.XtraEditors.SimpleButton btnStorageReset;
        private DevExpress.XtraGrid.Columns.GridColumn colAngle;
        private DevExpress.XtraGrid.Columns.GridColumn colEndLand;
        private DevExpress.XtraEditors.SimpleButton btnCancelTask;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraGrid.GridControl gcDetail;
        private System.Windows.Forms.BindingSource bsTaskDetail;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetail;
        private DevExpress.XtraGrid.Columns.GridColumn colDetailID;
        private DevExpress.XtraGrid.Columns.GridColumn colLandCode;
        private DevExpress.XtraGrid.Columns.GridColumn colOperType;
        private DevExpress.XtraGrid.Columns.GridColumn colIsAllowExcute;
        private DevExpress.XtraGrid.Columns.GridColumn colTaskDetailID;
        private DevExpress.XtraGrid.Columns.GridColumn colErrorMes;
        private DevExpress.XtraGrid.Columns.GridColumn clSelect;
        private DevExpress.XtraGrid.Columns.GridColumn colDetailState;
        private DevExpress.XtraGrid.Columns.GridColumn colIsUpLand;
        private System.Windows.Forms.Button btnTestTraffic;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtArmLand;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit txtAGVID;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtBackAGV;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private System.Windows.Forms.Button btnBack;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage5;
        private DevExpress.XtraEditors.PanelControl panelControl5;
        private DevExpress.XtraGrid.GridControl gcChargeInfo;
        private DevExpress.XtraGrid.Views.Grid.GridView gvChargeInfo;
        private System.Windows.Forms.BindingSource bsChargeInfo;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colIsBreak;
        private DevExpress.XtraGrid.Columns.GridColumn colStateStr;
        private DevExpress.XtraGrid.Columns.GridColumn colIP;
        private DevExpress.XtraGrid.Columns.GridColumn colPort;
        private DevExpress.XtraGrid.Columns.GridColumn clLandCode;
        private DevExpress.XtraEditors.TextEdit txtChargeID;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.SimpleButton btnStopCharge;
        private DevExpress.XtraEditors.SimpleButton btnStartCharge;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem btnLog;
        private DevExpress.XtraBars.BarButtonItem btnExitLog;
        private DevExpress.XtraGrid.Columns.GridColumn colBangState;
        private DevExpress.XtraEditors.SimpleButton btnClearSysCarInfo;
        private DevExpress.XtraEditors.SimpleButton btnSetCarSite;
        private DevExpress.XtraEditors.SimpleButton btnTestchazi;
        private DevExpress.XtraEditors.TextEdit txtchazi;
        private DevExpress.XtraGrid.Columns.GridColumn colStorageName;
        private DevExpress.XtraGrid.Columns.GridColumn golStorageName;
        private DevExpress.XtraBars.Navigation.TabNavigationPage npIOInfo;
        private DevExpress.XtraGrid.GridControl gcIOInfo;
        private DevExpress.XtraGrid.Views.Grid.GridView gvIOInfo;
        private System.Windows.Forms.BindingSource bsIOInfoes;
        private DevExpress.XtraGrid.Columns.GridColumn colIOID;
        private DevExpress.XtraGrid.Columns.GridColumn colDeviceName;
        private DevExpress.XtraGrid.Columns.GridColumn colbIsCommBreak;
        private DevExpress.XtraGrid.Columns.GridColumn colIOIP;
        private DevExpress.XtraGrid.Columns.GridColumn colIOPort;
        private DevExpress.XtraGrid.Columns.GridColumn colX;
        private DevExpress.XtraGrid.Columns.GridColumn colY;
        private DevExpress.XtraGrid.Columns.GridColumn colJCState;
        private DevExpress.XtraEditors.SimpleButton btnTestMes;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.TextEdit txtMesAgvSite;
        private DevExpress.XtraEditors.TextEdit txtMesAgvid;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private DevExpress.XtraGrid.Columns.GridColumn colIsBack;
        private DevExpress.XtraEditors.SimpleButton btnClearAllTask;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private System.Windows.Forms.TextBox txtTrafficInfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraBars.Navigation.TabNavigationPage tabNavigationPage6;
        private DevExpress.XtraEditors.LabelControl labelControl16;
        private DevExpress.XtraEditors.LabelControl labelControl15;
        private DevExpress.XtraEditors.LabelControl xxxxxxx;
        private DevExpress.XtraEditors.SimpleButton addOutputTask;
        private DevExpress.XtraEditors.LabelControl labelControl13;
        private DevExpress.XtraEditors.SimpleButton addInputTask;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private DevExpress.XtraEditors.LabelControl labelControl17;
        private DevExpress.XtraEditors.LabelControl outputWaitLabel;
        private DevExpress.XtraEditors.LabelControl inputWaitLabel;
        private DevExpress.XtraEditors.LabelControl totalStorageLeafLabel;
        private DevExpress.XtraEditors.SimpleButton startTest;
        private DevExpress.XtraEditors.LabelControl totalStorageUsedLabel;
        private DevExpress.XtraEditors.LabelControl labelControl19;
        private DevExpress.XtraEditors.LabelControl labelControl18;
        private DevExpress.XtraEditors.LabelControl outputedLabel;
        private DevExpress.XtraEditors.LabelControl labelControl26;
        private DevExpress.XtraEditors.LabelControl inputedLabel;
        private DevExpress.XtraEditors.LabelControl labelControl24;
        private DevExpress.XtraEditors.LabelControl outputTotalLabel;
        private DevExpress.XtraEditors.LabelControl labelControl22;
        private DevExpress.XtraEditors.LabelControl inputTotalLabel;
        private DevExpress.XtraEditors.LabelControl labelControl20;
        private DevExpress.XtraEditors.LabelControl totalStorageLabel;
    }
}