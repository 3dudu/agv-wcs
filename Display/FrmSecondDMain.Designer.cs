namespace Display
{
    partial class FrmSecondDMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSecondDMain));
            this.spilWarm = new DevExpress.XtraEditors.SplitContainerControl();
            this.gcCar = new DevExpress.XtraGrid.GridControl();
            this.bsCar = new System.Windows.Forms.BindingSource();
            this.gvCar = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colAgvID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCurrSite = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colExcuteTaksNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTaskDetailID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCarState = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLowPower = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colfVolt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcWarnMess = new DevExpress.XtraEditors.GroupControl();
            this.txtWarnMess = new DevExpress.XtraEditors.MemoEdit();
            this.barManager1 = new DevExpress.XtraBars.BarManager();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnConnetSet = new DevExpress.XtraBars.BarButtonItem();
            this.btnOption = new DevExpress.XtraBars.BarButtonItem();
            this.skinBarSubItem2 = new DevExpress.XtraBars.SkinBarSubItem();
            this.btnExit = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager();
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.pclMain = new DevExpress.XtraEditors.PanelControl();
            this.colbIsCommBreak = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spilWarm)).BeginInit();
            this.spilWarm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcCar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcWarnMess)).BeginInit();
            this.gcWarnMess.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtWarnMess.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanel1.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pclMain)).BeginInit();
            this.SuspendLayout();
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "-.png");
            this.imageCollection1.Images.SetKeyName(1, "+.png");
            this.imageCollection1.Images.SetKeyName(2, "add-favorite.png");
            this.imageCollection1.Images.SetKeyName(3, "add-printer.png");
            this.imageCollection1.Images.SetKeyName(4, "alarm.png");
            this.imageCollection1.Images.SetKeyName(5, "alert.png");
            this.imageCollection1.Images.SetKeyName(6, "announce.png");
            this.imageCollection1.Images.SetKeyName(7, "back.png");
            this.imageCollection1.Images.SetKeyName(8, "back2.png");
            this.imageCollection1.Images.SetKeyName(9, "backward.png");
            this.imageCollection1.Images.SetKeyName(10, "basket.png");
            this.imageCollection1.Images.SetKeyName(11, "binoculars.png");
            this.imageCollection1.Images.SetKeyName(12, "burn.png");
            this.imageCollection1.Images.SetKeyName(13, "cal.png");
            this.imageCollection1.Images.SetKeyName(14, "calculator.png");
            this.imageCollection1.Images.SetKeyName(15, "cash-register.png");
            this.imageCollection1.Images.SetKeyName(16, "cc.png");
            this.imageCollection1.Images.SetKeyName(17, "cd.png");
            this.imageCollection1.Images.SetKeyName(18, "config.png");
            this.imageCollection1.Images.SetKeyName(19, "connect.png");
            this.imageCollection1.Images.SetKeyName(20, "construction.png");
            this.imageCollection1.Images.SetKeyName(21, "contacts.png");
            this.imageCollection1.Images.SetKeyName(22, "copy.png");
            this.imageCollection1.Images.SetKeyName(23, "cut.png");
            this.imageCollection1.Images.SetKeyName(24, "database.png");
            this.imageCollection1.Images.SetKeyName(25, "delete.png");
            this.imageCollection1.Images.SetKeyName(26, "delete-folder.png");
            this.imageCollection1.Images.SetKeyName(27, "down3.png");
            this.imageCollection1.Images.SetKeyName(28, "download.png");
            this.imageCollection1.Images.SetKeyName(29, "edit.png");
            this.imageCollection1.Images.SetKeyName(30, "email.png");
            this.imageCollection1.Images.SetKeyName(31, "email2.png");
            this.imageCollection1.Images.SetKeyName(32, "export.png");
            this.imageCollection1.Images.SetKeyName(33, "export1.png");
            this.imageCollection1.Images.SetKeyName(34, "faq.png");
            this.imageCollection1.Images.SetKeyName(35, "favorite.png");
            this.imageCollection1.Images.SetKeyName(36, "file.png");
            this.imageCollection1.Images.SetKeyName(37, "folder.png");
            this.imageCollection1.Images.SetKeyName(38, "forward.png");
            this.imageCollection1.Images.SetKeyName(39, "front.png");
            this.imageCollection1.Images.SetKeyName(40, "front1.png");
            this.imageCollection1.Images.SetKeyName(41, "fulltrash.png");
            this.imageCollection1.Images.SetKeyName(42, "hd.png");
            this.imageCollection1.Images.SetKeyName(43, "hd1.png");
            this.imageCollection1.Images.SetKeyName(44, "help.png");
            this.imageCollection1.Images.SetKeyName(45, "home.png");
            this.imageCollection1.Images.SetKeyName(46, "image.png");
            this.imageCollection1.Images.SetKeyName(47, "import.png");
            this.imageCollection1.Images.SetKeyName(48, "import2.png");
            this.imageCollection1.Images.SetKeyName(49, "info.png");
            this.imageCollection1.Images.SetKeyName(50, "install.png");
            this.imageCollection1.Images.SetKeyName(51, "locked.png");
            this.imageCollection1.Images.SetKeyName(52, "music.png");
            this.imageCollection1.Images.SetKeyName(53, "network.png");
            this.imageCollection1.Images.SetKeyName(54, "new-folder.png");
            this.imageCollection1.Images.SetKeyName(55, "ok.png");
            this.imageCollection1.Images.SetKeyName(56, "opened.png");
            this.imageCollection1.Images.SetKeyName(57, "open-folder.png");
            this.imageCollection1.Images.SetKeyName(58, "paste.png");
            this.imageCollection1.Images.SetKeyName(59, "photo.png");
            this.imageCollection1.Images.SetKeyName(60, "police.png");
            this.imageCollection1.Images.SetKeyName(61, "printer.png");
            this.imageCollection1.Images.SetKeyName(62, "public.png");
            this.imageCollection1.Images.SetKeyName(63, "radar.png");
            this.imageCollection1.Images.SetKeyName(64, "save.png");
            this.imageCollection1.Images.SetKeyName(65, "save-as.png");
            this.imageCollection1.Images.SetKeyName(66, "screen-capture.png");
            this.imageCollection1.Images.SetKeyName(67, "search.png");
            this.imageCollection1.Images.SetKeyName(68, "send.png");
            this.imageCollection1.Images.SetKeyName(69, "software 2.png");
            this.imageCollection1.Images.SetKeyName(70, "software update.png");
            this.imageCollection1.Images.SetKeyName(71, "software.png");
            this.imageCollection1.Images.SetKeyName(72, "sound.png");
            this.imageCollection1.Images.SetKeyName(73, "statics.png");
            this.imageCollection1.Images.SetKeyName(74, "statics-1.png");
            this.imageCollection1.Images.SetKeyName(75, "statics-2.png");
            this.imageCollection1.Images.SetKeyName(76, "stop.png");
            this.imageCollection1.Images.SetKeyName(77, "stop-alt.png");
            this.imageCollection1.Images.SetKeyName(78, "support.png");
            this.imageCollection1.Images.SetKeyName(79, "switcher.png");
            this.imageCollection1.Images.SetKeyName(80, "trash.png");
            this.imageCollection1.Images.SetKeyName(81, "truck.png");
            this.imageCollection1.Images.SetKeyName(82, "up4.png");
            this.imageCollection1.Images.SetKeyName(83, "update.png");
            this.imageCollection1.Images.SetKeyName(84, "upload.png");
            this.imageCollection1.Images.SetKeyName(85, "user.png");
            this.imageCollection1.Images.SetKeyName(86, "uses.png");
            this.imageCollection1.Images.SetKeyName(87, "viewi-pr.png");
            this.imageCollection1.Images.SetKeyName(88, "web.png");
            this.imageCollection1.Images.SetKeyName(89, "window.png");
            this.imageCollection1.Images.SetKeyName(90, "window-2.png");
            this.imageCollection1.Images.SetKeyName(91, "zoom-.png");
            this.imageCollection1.Images.SetKeyName(92, "zoom+.png");
            // 
            // spilWarm
            // 
            this.spilWarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spilWarm.Location = new System.Drawing.Point(0, 0);
            this.spilWarm.Name = "spilWarm";
            this.spilWarm.Panel1.Controls.Add(this.gcCar);
            this.spilWarm.Panel1.Text = "Panel1";
            this.spilWarm.Panel2.Controls.Add(this.gcWarnMess);
            this.spilWarm.Panel2.Text = "Panel2";
            this.spilWarm.Size = new System.Drawing.Size(1123, 173);
            this.spilWarm.SplitterPosition = 839;
            this.spilWarm.TabIndex = 0;
            this.spilWarm.Text = "splitContainerControl1";
            // 
            // gcCar
            // 
            this.gcCar.DataSource = this.bsCar;
            this.gcCar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcCar.Location = new System.Drawing.Point(0, 0);
            this.gcCar.MainView = this.gvCar;
            this.gcCar.Name = "gcCar";
            this.gcCar.Size = new System.Drawing.Size(839, 173);
            this.gcCar.TabIndex = 0;
            this.gcCar.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvCar});
            // 
            // bsCar
            // 
            this.bsCar.DataSource = typeof(Model.CarInfoExtend.CarInfo);
            // 
            // gvCar
            // 
            this.gvCar.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.gvCar.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvCar.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvCar.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvCar.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gvCar.Appearance.Row.Options.UseFont = true;
            this.gvCar.Appearance.Row.Options.UseTextOptions = true;
            this.gvCar.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvCar.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.gvCar.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colbIsCommBreak,
            this.colAgvID,
            this.colCurrSite,
            this.colExcuteTaksNo,
            this.colTaskDetailID,
            this.colCarState,
            this.colLowPower,
            this.colfVolt});
            this.gvCar.GridControl = this.gcCar;
            this.gvCar.Name = "gvCar";
            this.gvCar.OptionsBehavior.Editable = false;
            this.gvCar.OptionsBehavior.ReadOnly = true;
            this.gvCar.OptionsView.ShowDetailButtons = false;
            this.gvCar.OptionsView.ShowGroupPanel = false;
            this.gvCar.OptionsView.ShowIndicator = false;
            // 
            // colAgvID
            // 
            this.colAgvID.Caption = "车号";
            this.colAgvID.FieldName = "AgvID";
            this.colAgvID.Name = "colAgvID";
            this.colAgvID.Visible = true;
            this.colAgvID.VisibleIndex = 1;
            // 
            // colCurrSite
            // 
            this.colCurrSite.Caption = "当前站点";
            this.colCurrSite.FieldName = "CurrSite";
            this.colCurrSite.Name = "colCurrSite";
            this.colCurrSite.Visible = true;
            this.colCurrSite.VisibleIndex = 2;
            this.colCurrSite.Width = 104;
            // 
            // colExcuteTaksNo
            // 
            this.colExcuteTaksNo.Caption = "当前任务号";
            this.colExcuteTaksNo.FieldName = "ExcuteTaksNo";
            this.colExcuteTaksNo.Name = "colExcuteTaksNo";
            this.colExcuteTaksNo.Visible = true;
            this.colExcuteTaksNo.VisibleIndex = 3;
            this.colExcuteTaksNo.Width = 125;
            // 
            // colTaskDetailID
            // 
            this.colTaskDetailID.Caption = "当前任务明细ID";
            this.colTaskDetailID.FieldName = "TaskDetailID";
            this.colTaskDetailID.Name = "colTaskDetailID";
            this.colTaskDetailID.Visible = true;
            this.colTaskDetailID.VisibleIndex = 4;
            this.colTaskDetailID.Width = 171;
            // 
            // colCarState
            // 
            this.colCarState.Caption = "状态";
            this.colCarState.FieldName = "CarStateStr";
            this.colCarState.Name = "colCarState";
            this.colCarState.Visible = true;
            this.colCarState.VisibleIndex = 5;
            // 
            // colLowPower
            // 
            this.colLowPower.Caption = "是否低电量";
            this.colLowPower.FieldName = "LowPower";
            this.colLowPower.Name = "colLowPower";
            this.colLowPower.Visible = true;
            this.colLowPower.VisibleIndex = 6;
            this.colLowPower.Width = 125;
            // 
            // colfVolt
            // 
            this.colfVolt.Caption = "电量";
            this.colfVolt.FieldName = "fVolt";
            this.colfVolt.Name = "colfVolt";
            this.colfVolt.Visible = true;
            this.colfVolt.VisibleIndex = 7;
            // 
            // gcWarnMess
            // 
            this.gcWarnMess.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.gcWarnMess.AppearanceCaption.Options.UseFont = true;
            this.gcWarnMess.AppearanceCaption.Options.UseTextOptions = true;
            this.gcWarnMess.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcWarnMess.Controls.Add(this.txtWarnMess);
            this.gcWarnMess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcWarnMess.Location = new System.Drawing.Point(0, 0);
            this.gcWarnMess.Name = "gcWarnMess";
            this.gcWarnMess.Size = new System.Drawing.Size(279, 173);
            this.gcWarnMess.TabIndex = 0;
            this.gcWarnMess.Text = "报警异常信息";
            // 
            // txtWarnMess
            // 
            this.txtWarnMess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtWarnMess.Location = new System.Drawing.Point(2, 31);
            this.txtWarnMess.MenuManager = this.barManager1;
            this.txtWarnMess.Name = "txtWarnMess";
            this.txtWarnMess.Size = new System.Drawing.Size(275, 140);
            this.txtWarnMess.TabIndex = 0;
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockManager = this.dockManager1;
            this.barManager1.Form = this;
            this.barManager1.Images = this.imageCollection1;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.skinBarSubItem2,
            this.btnExit,
            this.btnConnetSet,
            this.btnOption});
            this.barManager1.MaxItemId = 6;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnConnetSet, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnOption, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.skinBarSubItem2, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnExit, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            this.bar1.Visible = false;
            // 
            // btnConnetSet
            // 
            this.btnConnetSet.Caption = "连接设置";
            this.btnConnetSet.Id = 4;
            this.btnConnetSet.ImageIndex = 19;
            this.btnConnetSet.Name = "btnConnetSet";
            this.btnConnetSet.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnConnetSet_ItemClick);
            // 
            // btnOption
            // 
            this.btnOption.Caption = "选项";
            this.btnOption.Id = 5;
            this.btnOption.ImageIndex = 35;
            this.btnOption.Name = "btnOption";
            this.btnOption.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnOption_ItemClick);
            // 
            // skinBarSubItem2
            // 
            this.skinBarSubItem2.Caption = "皮肤设置";
            this.skinBarSubItem2.Id = 2;
            this.skinBarSubItem2.ImageIndex = 2;
            this.skinBarSubItem2.Name = "skinBarSubItem2";
            // 
            // btnExit
            // 
            this.btnExit.Caption = "退出";
            this.btnExit.Id = 3;
            this.btnExit.ImageIndex = 25;
            this.btnExit.Name = "btnExit";
            this.btnExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExit_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1131, 31);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 511);
            this.barDockControlBottom.Size = new System.Drawing.Size(1131, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 31);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 480);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1131, 31);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 480);
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.MenuManager = this.barManager1;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanel1});
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane"});
            // 
            // dockPanel1
            // 
            this.dockPanel1.Controls.Add(this.dockPanel1_Container);
            this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.dockPanel1.ID = new System.Guid("0dab2e6d-feed-4900-8739-fe201ae23511");
            this.dockPanel1.Location = new System.Drawing.Point(0, 311);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.OriginalSize = new System.Drawing.Size(200, 200);
            this.dockPanel1.Size = new System.Drawing.Size(1131, 200);
            this.dockPanel1.Text = "信息显示";
            this.dockPanel1.DockChanged += new System.EventHandler(this.dockPanel1_DockChanged);
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.spilWarm);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(1123, 173);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // pclMain
            // 
            this.pclMain.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pclMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pclMain.Location = new System.Drawing.Point(0, 31);
            this.pclMain.Name = "pclMain";
            this.pclMain.Size = new System.Drawing.Size(1131, 280);
            this.pclMain.TabIndex = 2;
            // 
            // colbIsCommBreak
            // 
            this.colbIsCommBreak.Caption = "是否已掉线";
            this.colbIsCommBreak.FieldName = "bIsCommBreak";
            this.colbIsCommBreak.Name = "colbIsCommBreak";
            this.colbIsCommBreak.Visible = true;
            this.colbIsCommBreak.VisibleIndex = 0;
            this.colbIsCommBreak.Width = 125;
            // 
            // FrmSecondDMain
            // 
            this.Appearance.ForeColor = System.Drawing.Color.Black;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1131, 511);
            this.Controls.Add(this.pclMain);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "FrmSecondDMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AGV调度系统实时显示";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSecondDMain_FormClosing);
            this.Shown += new System.EventHandler(this.FrmSecondDMain_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSecondDMain_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spilWarm)).EndInit();
            this.spilWarm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcCar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcWarnMess)).EndInit();
            this.gcWarnMess.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtWarnMess.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanel1.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pclMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl pclMain;
        private DevExpress.XtraEditors.SplitContainerControl spilWarm;
        private System.Windows.Forms.BindingSource bsCar;
        private DevExpress.XtraGrid.GridControl gcCar;
        private DevExpress.XtraGrid.Views.Grid.GridView gvCar;
        private DevExpress.XtraGrid.Columns.GridColumn colAgvID;
        private DevExpress.XtraGrid.Columns.GridColumn colCarState;
        private DevExpress.XtraEditors.GroupControl gcWarnMess;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.SkinBarSubItem skinBarSubItem2;
        private DevExpress.XtraBars.BarButtonItem btnExit;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraBars.BarButtonItem btnConnetSet;
        private DevExpress.XtraEditors.MemoEdit txtWarnMess;
        private DevExpress.XtraBars.BarButtonItem btnOption;
        private DevExpress.XtraGrid.Columns.GridColumn colCurrSite;
        private DevExpress.XtraGrid.Columns.GridColumn colExcuteTaksNo;
        private DevExpress.XtraGrid.Columns.GridColumn colTaskDetailID;
        private DevExpress.XtraGrid.Columns.GridColumn colLowPower;
        private DevExpress.XtraGrid.Columns.GridColumn colfVolt;
        private DevExpress.XtraGrid.Columns.GridColumn colbIsCommBreak;
    }
}