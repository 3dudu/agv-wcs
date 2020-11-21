namespace Simulation.SimulationCommon
{
    partial class FrmDirectionSet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDirectionSet));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnClear = new DevExpress.XtraBars.BarButtonItem();
            this.btnSave = new DevExpress.XtraBars.BarButtonItem();
            this.btnExit = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.cbxXi = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbxNan = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbxDong = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbxBei = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection24)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbxXi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxNan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxDong.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxBei.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // imageCollection24
            // 
            this.imageCollection24.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection24.ImageStream")));
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl1.Location = new System.Drawing.Point(7, 216);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(136, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "注:对应地图上的东西南北";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Location = new System.Drawing.Point(200, 104);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(21, 24);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "东";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.labelControl3.Location = new System.Drawing.Point(161, 130);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(21, 24);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "南";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.labelControl4.Location = new System.Drawing.Point(119, 104);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(21, 24);
            this.labelControl4.TabIndex = 3;
            this.labelControl4.Text = "西";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.labelControl5.Location = new System.Drawing.Point(161, 70);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(21, 24);
            this.labelControl5.TabIndex = 4;
            this.labelControl5.Text = "北";
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
            this.btnClear,
            this.btnSave,
            this.btnExit});
            this.barManager1.MaxItemId = 3;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnClear, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnSave, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnExit, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            // 
            // btnClear
            // 
            this.btnClear.Caption = "清空";
            this.btnClear.Id = 0;
            this.btnClear.ImageIndex = 26;
            this.btnClear.Name = "btnClear";
            this.btnClear.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.btnClear.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnClear_ItemClick);
            // 
            // btnSave
            // 
            this.btnSave.Caption = "保存";
            this.btnSave.Id = 1;
            this.btnSave.ImageIndex = 63;
            this.btnSave.Name = "btnSave";
            this.btnSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSave_ItemClick);
            // 
            // btnExit
            // 
            this.btnExit.Caption = "退出";
            this.btnExit.Id = 2;
            this.btnExit.ImageIndex = 25;
            this.btnExit.Name = "btnExit";
            this.btnExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnExit_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(372, 31);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 274);
            this.barDockControlBottom.Size = new System.Drawing.Size(372, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 31);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 243);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(372, 31);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 243);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.cbxXi);
            this.panelControl1.Controls.Add(this.cbxNan);
            this.panelControl1.Controls.Add(this.cbxDong);
            this.panelControl1.Controls.Add(this.cbxBei);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.labelControl5);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl4);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 31);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(372, 243);
            this.panelControl1.TabIndex = 9;
            // 
            // cbxXi
            // 
            this.cbxXi.EditValue = "180";
            this.cbxXi.Location = new System.Drawing.Point(46, 107);
            this.cbxXi.MenuManager = this.barManager1;
            this.cbxXi.Name = "cbxXi";
            this.cbxXi.Properties.Appearance.Options.UseTextOptions = true;
            this.cbxXi.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.cbxXi.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.cbxXi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbxXi.Properties.Items.AddRange(new object[] {
            "0",
            "90",
            "180",
            "270"});
            this.cbxXi.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbxXi.Size = new System.Drawing.Size(67, 20);
            this.cbxXi.TabIndex = 8;
            // 
            // cbxNan
            // 
            this.cbxNan.EditValue = "270";
            this.cbxNan.Location = new System.Drawing.Point(138, 160);
            this.cbxNan.MenuManager = this.barManager1;
            this.cbxNan.Name = "cbxNan";
            this.cbxNan.Properties.Appearance.Options.UseTextOptions = true;
            this.cbxNan.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.cbxNan.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbxNan.Properties.Items.AddRange(new object[] {
            "0",
            "90",
            "180",
            "270"});
            this.cbxNan.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbxNan.Size = new System.Drawing.Size(67, 20);
            this.cbxNan.TabIndex = 7;
            // 
            // cbxDong
            // 
            this.cbxDong.EditValue = "0";
            this.cbxDong.Location = new System.Drawing.Point(227, 107);
            this.cbxDong.MenuManager = this.barManager1;
            this.cbxDong.Name = "cbxDong";
            this.cbxDong.Properties.Appearance.Options.UseTextOptions = true;
            this.cbxDong.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.cbxDong.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbxDong.Properties.Items.AddRange(new object[] {
            "0",
            "90",
            "180",
            "270"});
            this.cbxDong.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbxDong.Size = new System.Drawing.Size(67, 20);
            this.cbxDong.TabIndex = 6;
            // 
            // cbxBei
            // 
            this.cbxBei.EditValue = "90";
            this.cbxBei.Location = new System.Drawing.Point(138, 44);
            this.cbxBei.MenuManager = this.barManager1;
            this.cbxBei.Name = "cbxBei";
            this.cbxBei.Properties.Appearance.Options.UseTextOptions = true;
            this.cbxBei.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.cbxBei.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbxBei.Properties.Items.AddRange(new object[] {
            "0",
            "90",
            "180",
            "270"});
            this.cbxBei.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbxBei.Size = new System.Drawing.Size(67, 20);
            this.cbxBei.TabIndex = 5;
            // 
            // FrmDirectionSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 274);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.MaximizeBox = false;
            this.Name = "FrmDirectionSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AGV坐标体系设置";
            this.Shown += new System.EventHandler(this.FrmDirectionSet_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection24)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbxXi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxNan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxDong.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxBei.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem btnClear;
        private DevExpress.XtraBars.BarButtonItem btnSave;
        private DevExpress.XtraBars.BarButtonItem btnExit;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cbxXi;
        private DevExpress.XtraEditors.ComboBoxEdit cbxNan;
        private DevExpress.XtraEditors.ComboBoxEdit cbxDong;
        private DevExpress.XtraEditors.ComboBoxEdit cbxBei;
    }
}