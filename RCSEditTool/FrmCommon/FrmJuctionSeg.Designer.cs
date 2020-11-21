namespace RCSEditTool.FrmCommon
{
    partial class FrmJuctionSeg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmJuctionSeg));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnAdd = new DevExpress.XtraBars.BarButtonItem();
            this.btnDel = new DevExpress.XtraBars.BarButtonItem();
            this.btnSave = new DevExpress.XtraBars.BarButtonItem();
            this.btnExit = new DevExpress.XtraBars.BarButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.gcDetail = new DevExpress.XtraGrid.GridControl();
            this.bsDetail = new System.Windows.Forms.BindingSource(this.components);
            this.gvDetail = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colSegmentID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLandCodes = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).BeginInit();
            this.SuspendLayout();
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
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnAdd,
            this.btnDel,
            this.btnSave,
            this.btnExit});
            this.barManager1.MaxItemId = 4;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnAdd, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnDel, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnSave, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnExit, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            // 
            // btnAdd
            // 
            this.btnAdd.Caption = "增行";
            this.btnAdd.Glyph = ((System.Drawing.Image)(resources.GetObject("btnAdd.Glyph")));
            this.btnAdd.Id = 0;
            this.btnAdd.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnAdd.LargeGlyph")));
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAdd_ItemClick);
            // 
            // btnDel
            // 
            this.btnDel.Caption = "删行";
            this.btnDel.Glyph = ((System.Drawing.Image)(resources.GetObject("btnDel.Glyph")));
            this.btnDel.Id = 1;
            this.btnDel.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnDel.LargeGlyph")));
            this.btnDel.Name = "btnDel";
            this.btnDel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDel_ItemClick);
            // 
            // btnSave
            // 
            this.btnSave.Caption = "保存";
            this.btnSave.Glyph = ((System.Drawing.Image)(resources.GetObject("btnSave.Glyph")));
            this.btnSave.Id = 2;
            this.btnSave.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnSave.LargeGlyph")));
            this.btnSave.Name = "btnSave";
            this.btnSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSave_ItemClick);
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
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(660, 31);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 376);
            this.barDockControlBottom.Size = new System.Drawing.Size(660, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 31);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 345);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(660, 31);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 345);
            // 
            // gcDetail
            // 
            this.gcDetail.DataSource = this.bsDetail;
            this.gcDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcDetail.Location = new System.Drawing.Point(0, 31);
            this.gcDetail.MainView = this.gvDetail;
            this.gcDetail.MenuManager = this.barManager1;
            this.gcDetail.Name = "gcDetail";
            this.gcDetail.Size = new System.Drawing.Size(660, 345);
            this.gcDetail.TabIndex = 4;
            this.gcDetail.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDetail});
            // 
            // bsDetail
            // 
            this.bsDetail.DataSource = typeof(Model.Comoon.TraJunctionSegInfo);
            // 
            // gvDetail
            // 
            this.gvDetail.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gvDetail.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colSegmentID,
            this.colLandCodes});
            this.gvDetail.GridControl = this.gcDetail;
            this.gvDetail.Name = "gvDetail";
            this.gvDetail.OptionsView.ShowDetailButtons = false;
            this.gvDetail.OptionsView.ShowGroupPanel = false;
            this.gvDetail.OptionsView.ShowIndicator = false;
            this.gvDetail.BeforeLeaveRow += new DevExpress.XtraGrid.Views.Base.RowAllowEventHandler(this.gvDetail_BeforeLeaveRow);
            // 
            // colSegmentID
            // 
            this.colSegmentID.Caption = "序号";
            this.colSegmentID.FieldName = "SegmentID";
            this.colSegmentID.Name = "colSegmentID";
            this.colSegmentID.OptionsColumn.AllowEdit = false;
            this.colSegmentID.OptionsColumn.ReadOnly = true;
            this.colSegmentID.Visible = true;
            this.colSegmentID.VisibleIndex = 0;
            // 
            // colLandCodes
            // 
            this.colLandCodes.Caption = "片段地标集";
            this.colLandCodes.FieldName = "LandCodes";
            this.colLandCodes.Name = "colLandCodes";
            this.colLandCodes.Visible = true;
            this.colLandCodes.VisibleIndex = 1;
            // 
            // FrmJuctionSeg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 376);
            this.Controls.Add(this.gcDetail);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmJuctionSeg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "资源集片段维护";
            this.Shown += new System.EventHandler(this.FrmJuctionSeg_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetail)).EndInit();
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
        private DevExpress.XtraBars.BarButtonItem btnAdd;
        private DevExpress.XtraBars.BarButtonItem btnDel;
        private DevExpress.XtraBars.BarButtonItem btnSave;
        private DevExpress.XtraBars.BarButtonItem btnExit;
        private DevExpress.XtraGrid.GridControl gcDetail;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetail;
        private System.Windows.Forms.BindingSource bsDetail;
        private DevExpress.XtraGrid.Columns.GridColumn colSegmentID;
        private DevExpress.XtraGrid.Columns.GridColumn colLandCodes;
    }
}