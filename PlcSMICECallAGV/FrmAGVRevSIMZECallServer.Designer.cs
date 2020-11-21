namespace PlcSMICECallAGV
{
    partial class FrmAGVRevSIMZECallServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAGVRevSIMZECallServer));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barManager2 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnToBot = new DevExpress.XtraBars.BarButtonItem();
            this.btnStart = new DevExpress.XtraBars.BarButtonItem();
            this.btnClose = new DevExpress.XtraBars.BarButtonItem();
            this.btnExit = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControl1 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl2 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl3 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl4 = new DevExpress.XtraBars.BarDockControl();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon();
            this.listboxtask = new DevExpress.XtraEditors.ListBoxControl();
            this.tmClear = new System.Windows.Forms.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listboxtask)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItem1});
            this.barManager1.MaxItemId = 1;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 46);
            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.barDockControlTop.Size = new System.Drawing.Size(753, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 426);
            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.barDockControlBottom.Size = new System.Drawing.Size(753, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 46);
            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 380);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(753, 46);
            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 380);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "barButtonItem1";
            this.barButtonItem1.Id = 0;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barManager2
            // 
            this.barManager2.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager2.DockControls.Add(this.barDockControl1);
            this.barManager2.DockControls.Add(this.barDockControl2);
            this.barManager2.DockControls.Add(this.barDockControl3);
            this.barManager2.DockControls.Add(this.barDockControl4);
            this.barManager2.Form = this;
            this.barManager2.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnToBot,
            this.btnStart,
            this.btnClose,
            this.btnExit});
            this.barManager2.MaxItemId = 4;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnToBot, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnStart, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnClose, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnExit, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            this.bar1.Visible = false;
            // 
            // btnToBot
            // 
            this.btnToBot.Caption = "转到后台进程";
            this.btnToBot.Glyph = ((System.Drawing.Image)(resources.GetObject("btnToBot.Glyph")));
            this.btnToBot.Id = 0;
            this.btnToBot.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnToBot.LargeGlyph")));
            this.btnToBot.Name = "btnToBot";
            this.btnToBot.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnToBot_ItemClick);
            // 
            // btnStart
            // 
            this.btnStart.Caption = "启动服务";
            this.btnStart.Glyph = ((System.Drawing.Image)(resources.GetObject("btnStart.Glyph")));
            this.btnStart.Id = 1;
            this.btnStart.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnStart.LargeGlyph")));
            this.btnStart.Name = "btnStart";
            this.btnStart.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnStart.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnStart_ItemClick);
            // 
            // btnClose
            // 
            this.btnClose.Caption = "关闭服务";
            this.btnClose.Glyph = ((System.Drawing.Image)(resources.GetObject("btnClose.Glyph")));
            this.btnClose.Id = 2;
            this.btnClose.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnClose.LargeGlyph")));
            this.btnClose.Name = "btnClose";
            this.btnClose.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnClose_ItemClick);
            // 
            // btnExit
            // 
            this.btnExit.Caption = "退出";
            this.btnExit.Glyph = ((System.Drawing.Image)(resources.GetObject("btnExit.Glyph")));
            this.btnExit.Id = 3;
            this.btnExit.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnExit.LargeGlyph")));
            this.btnExit.Name = "btnExit";
            this.btnExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExit_ItemClick);
            // 
            // barDockControl1
            // 
            this.barDockControl1.CausesValidation = false;
            this.barDockControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControl1.Location = new System.Drawing.Point(0, 0);
            this.barDockControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.barDockControl1.Size = new System.Drawing.Size(753, 46);
            // 
            // barDockControl2
            // 
            this.barDockControl2.CausesValidation = false;
            this.barDockControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControl2.Location = new System.Drawing.Point(0, 426);
            this.barDockControl2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.barDockControl2.Size = new System.Drawing.Size(753, 0);
            // 
            // barDockControl3
            // 
            this.barDockControl3.CausesValidation = false;
            this.barDockControl3.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControl3.Location = new System.Drawing.Point(0, 46);
            this.barDockControl3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.barDockControl3.Size = new System.Drawing.Size(0, 380);
            // 
            // barDockControl4
            // 
            this.barDockControl4.CausesValidation = false;
            this.barDockControl4.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControl4.Location = new System.Drawing.Point(753, 46);
            this.barDockControl4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.barDockControl4.Size = new System.Drawing.Size(0, 380);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.Click += new System.EventHandler(this.notifyIcon1_Click);
            // 
            // listboxtask
            // 
            this.listboxtask.Appearance.BackColor = System.Drawing.Color.Black;
            this.listboxtask.Appearance.ForeColor = System.Drawing.Color.White;
            this.listboxtask.Appearance.Options.UseBackColor = true;
            this.listboxtask.Appearance.Options.UseForeColor = true;
            this.listboxtask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listboxtask.Location = new System.Drawing.Point(0, 46);
            this.listboxtask.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listboxtask.Name = "listboxtask";
            this.listboxtask.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.listboxtask.ShowFocusRect = false;
            this.listboxtask.Size = new System.Drawing.Size(753, 380);
            this.listboxtask.TabIndex = 13;
            // 
            // tmClear
            // 
            this.tmClear.Tick += new System.EventHandler(this.tmClear_Tick);
            // 
            // FrmAGVRevSIMZECallServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 426);
            this.Controls.Add(this.listboxtask);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Controls.Add(this.barDockControl3);
            this.Controls.Add(this.barDockControl4);
            this.Controls.Add(this.barDockControl2);
            this.Controls.Add(this.barDockControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "FrmAGVRevSIMZECallServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AGV-西门子PLC呼叫服务";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmAGVRevSIMZECallServer_FormClosing);
            this.Load += new System.EventHandler(this.FrmAGVRevSIMZECallServer_Load);
            this.Shown += new System.EventHandler(this.FrmAGVRevSIMZECallServer_Shown);
            this.SizeChanged += new System.EventHandler(this.FrmAGVRevSIMZECallServer_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmAGVRevSIMZECallServer_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listboxtask)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarDockControl barDockControl3;
        private DevExpress.XtraBars.BarDockControl barDockControl4;
        private DevExpress.XtraBars.BarDockControl barDockControl2;
        private DevExpress.XtraBars.BarDockControl barDockControl1;
        private DevExpress.XtraBars.BarManager barManager2;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem btnToBot;
        private DevExpress.XtraBars.BarButtonItem btnStart;
        private DevExpress.XtraBars.BarButtonItem btnClose;
        private DevExpress.XtraBars.BarButtonItem btnExit;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private DevExpress.XtraEditors.ListBoxControl listboxtask;
        private System.Windows.Forms.Timer tmClear;
    }
}