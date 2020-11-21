namespace WindowPDAClient
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.bsCar = new System.Windows.Forms.BindingSource();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlAGVInfo = new System.Windows.Forms.Panel();
            this.gvAgvInfo = new System.Windows.Forms.DataGridView();
            this.pnlBtns = new System.Windows.Forms.Panel();
            this.pnlTime = new System.Windows.Forms.Panel();
            this.lblTime = new System.Windows.Forms.Label();
            this.pnlDate = new System.Windows.Forms.Panel();
            this.lblDate = new System.Windows.Forms.Label();
            this.btnSpray = new System.Windows.Forms.Button();
            this.btnMoveStore = new System.Windows.Forms.Button();
            this.btnOutStore = new System.Windows.Forms.Button();
            this.btnInStore = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.picClose = new System.Windows.Forms.PictureBox();
            this.lblTile = new System.Windows.Forms.Label();
            this.agvIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.carNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bIsCommBreakDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.carIPDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.carPortDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fVoltDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WaterAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lowPowerDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.speedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currSiteDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.carStateStrDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bangStateStrDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.errorMessageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.bsCar)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.pnlAGVInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvAgvInfo)).BeginInit();
            this.pnlBtns.SuspendLayout();
            this.pnlTime.SuspendLayout();
            this.pnlDate.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).BeginInit();
            this.SuspendLayout();
            // 
            // bsCar
            // 
            this.bsCar.DataSource = typeof(Model.CarInfoExtend.CarInfo);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(957, 521);
            this.panel2.TabIndex = 5;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.pnlMain);
            this.panel4.Controls.Add(this.pnlAGVInfo);
            this.panel4.Controls.Add(this.pnlBtns);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 30);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(957, 491);
            this.panel4.TabIndex = 1;
            // 
            // pnlMain
            // 
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(757, 303);
            this.pnlMain.TabIndex = 4;
            // 
            // pnlAGVInfo
            // 
            this.pnlAGVInfo.Controls.Add(this.gvAgvInfo);
            this.pnlAGVInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAGVInfo.Location = new System.Drawing.Point(0, 303);
            this.pnlAGVInfo.Name = "pnlAGVInfo";
            this.pnlAGVInfo.Size = new System.Drawing.Size(757, 188);
            this.pnlAGVInfo.TabIndex = 3;
            // 
            // gvAgvInfo
            // 
            this.gvAgvInfo.AllowUserToAddRows = false;
            this.gvAgvInfo.AllowUserToDeleteRows = false;
            this.gvAgvInfo.AllowUserToResizeColumns = false;
            this.gvAgvInfo.AllowUserToResizeRows = false;
            this.gvAgvInfo.AutoGenerateColumns = false;
            this.gvAgvInfo.BackgroundColor = System.Drawing.Color.Gray;
            this.gvAgvInfo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.gvAgvInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvAgvInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gvAgvInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvAgvInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.agvIDDataGridViewTextBoxColumn,
            this.carNameDataGridViewTextBoxColumn,
            this.bIsCommBreakDataGridViewCheckBoxColumn,
            this.carIPDataGridViewTextBoxColumn,
            this.carPortDataGridViewTextBoxColumn,
            this.fVoltDataGridViewTextBoxColumn,
            this.WaterAmount,
            this.lowPowerDataGridViewCheckBoxColumn,
            this.speedDataGridViewTextBoxColumn,
            this.currSiteDataGridViewTextBoxColumn,
            this.carStateStrDataGridViewTextBoxColumn,
            this.bangStateStrDataGridViewTextBoxColumn,
            this.errorMessageDataGridViewTextBoxColumn});
            this.gvAgvInfo.DataSource = this.bsCar;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gvAgvInfo.DefaultCellStyle = dataGridViewCellStyle2;
            this.gvAgvInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvAgvInfo.GridColor = System.Drawing.Color.Gray;
            this.gvAgvInfo.Location = new System.Drawing.Point(0, 0);
            this.gvAgvInfo.MultiSelect = false;
            this.gvAgvInfo.Name = "gvAgvInfo";
            this.gvAgvInfo.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gvAgvInfo.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gvAgvInfo.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            this.gvAgvInfo.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.gvAgvInfo.RowTemplate.Height = 23;
            this.gvAgvInfo.Size = new System.Drawing.Size(757, 188);
            this.gvAgvInfo.TabIndex = 4;
            // 
            // pnlBtns
            // 
            this.pnlBtns.Controls.Add(this.pnlTime);
            this.pnlBtns.Controls.Add(this.pnlDate);
            this.pnlBtns.Controls.Add(this.btnSpray);
            this.pnlBtns.Controls.Add(this.btnMoveStore);
            this.pnlBtns.Controls.Add(this.btnOutStore);
            this.pnlBtns.Controls.Add(this.btnInStore);
            this.pnlBtns.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlBtns.Location = new System.Drawing.Point(757, 0);
            this.pnlBtns.Name = "pnlBtns";
            this.pnlBtns.Size = new System.Drawing.Size(200, 491);
            this.pnlBtns.TabIndex = 2;
            // 
            // pnlTime
            // 
            this.pnlTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pnlTime.Controls.Add(this.lblTime);
            this.pnlTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTime.Location = new System.Drawing.Point(0, 41);
            this.pnlTime.Name = "pnlTime";
            this.pnlTime.Size = new System.Drawing.Size(200, 34);
            this.pnlTime.TabIndex = 5;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTime.ForeColor = System.Drawing.Color.White;
            this.lblTime.Location = new System.Drawing.Point(75, 10);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(48, 16);
            this.lblTime.TabIndex = 0;
            this.lblTime.Text = "14:49";
            // 
            // pnlDate
            // 
            this.pnlDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.pnlDate.Controls.Add(this.lblDate);
            this.pnlDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDate.Location = new System.Drawing.Point(0, 0);
            this.pnlDate.Name = "pnlDate";
            this.pnlDate.Size = new System.Drawing.Size(200, 41);
            this.pnlDate.TabIndex = 4;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDate.Location = new System.Drawing.Point(25, 7);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(152, 27);
            this.lblDate.TabIndex = 0;
            this.lblDate.Text = "2019-02-27";
            // 
            // btnSpray
            // 
            this.btnSpray.BackColor = System.Drawing.Color.Fuchsia;
            this.btnSpray.FlatAppearance.BorderSize = 0;
            this.btnSpray.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSpray.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSpray.Location = new System.Drawing.Point(26, 380);
            this.btnSpray.Name = "btnSpray";
            this.btnSpray.Size = new System.Drawing.Size(150, 64);
            this.btnSpray.TabIndex = 3;
            this.btnSpray.Text = "喷淋";
            this.btnSpray.UseVisualStyleBackColor = false;
            this.btnSpray.Click += new System.EventHandler(this.btnSpray_Click);
            // 
            // btnMoveStore
            // 
            this.btnMoveStore.BackColor = System.Drawing.Color.Teal;
            this.btnMoveStore.FlatAppearance.BorderSize = 0;
            this.btnMoveStore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoveStore.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMoveStore.Location = new System.Drawing.Point(26, 290);
            this.btnMoveStore.Name = "btnMoveStore";
            this.btnMoveStore.Size = new System.Drawing.Size(150, 64);
            this.btnMoveStore.TabIndex = 2;
            this.btnMoveStore.Text = "移库";
            this.btnMoveStore.UseVisualStyleBackColor = false;
            this.btnMoveStore.Click += new System.EventHandler(this.btnMoveStore_Click);
            // 
            // btnOutStore
            // 
            this.btnOutStore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnOutStore.FlatAppearance.BorderSize = 0;
            this.btnOutStore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOutStore.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOutStore.Location = new System.Drawing.Point(26, 196);
            this.btnOutStore.Name = "btnOutStore";
            this.btnOutStore.Size = new System.Drawing.Size(150, 64);
            this.btnOutStore.TabIndex = 1;
            this.btnOutStore.Text = "出库";
            this.btnOutStore.UseVisualStyleBackColor = false;
            this.btnOutStore.Click += new System.EventHandler(this.btnOutStore_Click);
            // 
            // btnInStore
            // 
            this.btnInStore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnInStore.FlatAppearance.BorderSize = 0;
            this.btnInStore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInStore.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnInStore.ForeColor = System.Drawing.Color.Black;
            this.btnInStore.Location = new System.Drawing.Point(26, 99);
            this.btnInStore.Name = "btnInStore";
            this.btnInStore.Size = new System.Drawing.Size(150, 64);
            this.btnInStore.TabIndex = 0;
            this.btnInStore.Text = "入库";
            this.btnInStore.UseVisualStyleBackColor = false;
            this.btnInStore.Click += new System.EventHandler(this.btnInStore_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.picClose);
            this.panel3.Controls.Add(this.lblTile);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(957, 30);
            this.panel3.TabIndex = 0;
            // 
            // picClose
            // 
            this.picClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.picClose.Image = global::WindowPDAClient.Properties.Resources.Home_Exit;
            this.picClose.Location = new System.Drawing.Point(927, 0);
            this.picClose.Name = "picClose";
            this.picClose.Size = new System.Drawing.Size(30, 30);
            this.picClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picClose.TabIndex = 1;
            this.picClose.TabStop = false;
            this.picClose.Click += new System.EventHandler(this.picClose_Click);
            // 
            // lblTile
            // 
            this.lblTile.BackColor = System.Drawing.Color.Gray;
            this.lblTile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTile.Font = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTile.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblTile.Location = new System.Drawing.Point(0, 0);
            this.lblTile.Name = "lblTile";
            this.lblTile.Size = new System.Drawing.Size(957, 30);
            this.lblTile.TabIndex = 0;
            this.lblTile.Text = "中铁AGV操作终端";
            this.lblTile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // agvIDDataGridViewTextBoxColumn
            // 
            this.agvIDDataGridViewTextBoxColumn.DataPropertyName = "AgvID";
            this.agvIDDataGridViewTextBoxColumn.HeaderText = "AGV编号";
            this.agvIDDataGridViewTextBoxColumn.Name = "agvIDDataGridViewTextBoxColumn";
            this.agvIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // carNameDataGridViewTextBoxColumn
            // 
            this.carNameDataGridViewTextBoxColumn.DataPropertyName = "CarName";
            this.carNameDataGridViewTextBoxColumn.HeaderText = "小车名称";
            this.carNameDataGridViewTextBoxColumn.Name = "carNameDataGridViewTextBoxColumn";
            this.carNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // bIsCommBreakDataGridViewCheckBoxColumn
            // 
            this.bIsCommBreakDataGridViewCheckBoxColumn.DataPropertyName = "bIsCommBreak";
            this.bIsCommBreakDataGridViewCheckBoxColumn.HeaderText = "是否掉线";
            this.bIsCommBreakDataGridViewCheckBoxColumn.Name = "bIsCommBreakDataGridViewCheckBoxColumn";
            this.bIsCommBreakDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // carIPDataGridViewTextBoxColumn
            // 
            this.carIPDataGridViewTextBoxColumn.DataPropertyName = "CarIP";
            this.carIPDataGridViewTextBoxColumn.HeaderText = "小车IP";
            this.carIPDataGridViewTextBoxColumn.Name = "carIPDataGridViewTextBoxColumn";
            this.carIPDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // carPortDataGridViewTextBoxColumn
            // 
            this.carPortDataGridViewTextBoxColumn.DataPropertyName = "CarPort";
            this.carPortDataGridViewTextBoxColumn.HeaderText = "小车端口号";
            this.carPortDataGridViewTextBoxColumn.Name = "carPortDataGridViewTextBoxColumn";
            this.carPortDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // fVoltDataGridViewTextBoxColumn
            // 
            this.fVoltDataGridViewTextBoxColumn.DataPropertyName = "fVolt";
            this.fVoltDataGridViewTextBoxColumn.HeaderText = "当前电量";
            this.fVoltDataGridViewTextBoxColumn.Name = "fVoltDataGridViewTextBoxColumn";
            this.fVoltDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // WaterAmount
            // 
            this.WaterAmount.DataPropertyName = "WaterAmount";
            this.WaterAmount.HeaderText = "剩余水量";
            this.WaterAmount.Name = "WaterAmount";
            this.WaterAmount.ReadOnly = true;
            // 
            // lowPowerDataGridViewCheckBoxColumn
            // 
            this.lowPowerDataGridViewCheckBoxColumn.DataPropertyName = "LowPower";
            this.lowPowerDataGridViewCheckBoxColumn.HeaderText = "是否低电量";
            this.lowPowerDataGridViewCheckBoxColumn.Name = "lowPowerDataGridViewCheckBoxColumn";
            this.lowPowerDataGridViewCheckBoxColumn.ReadOnly = true;
            // 
            // speedDataGridViewTextBoxColumn
            // 
            this.speedDataGridViewTextBoxColumn.DataPropertyName = "speed";
            this.speedDataGridViewTextBoxColumn.HeaderText = "当前速度";
            this.speedDataGridViewTextBoxColumn.Name = "speedDataGridViewTextBoxColumn";
            this.speedDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // currSiteDataGridViewTextBoxColumn
            // 
            this.currSiteDataGridViewTextBoxColumn.DataPropertyName = "CurrSite";
            this.currSiteDataGridViewTextBoxColumn.HeaderText = "当前站点";
            this.currSiteDataGridViewTextBoxColumn.Name = "currSiteDataGridViewTextBoxColumn";
            this.currSiteDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // carStateStrDataGridViewTextBoxColumn
            // 
            this.carStateStrDataGridViewTextBoxColumn.DataPropertyName = "CarStateStr";
            this.carStateStrDataGridViewTextBoxColumn.HeaderText = "车辆状态";
            this.carStateStrDataGridViewTextBoxColumn.Name = "carStateStrDataGridViewTextBoxColumn";
            this.carStateStrDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // bangStateStrDataGridViewTextBoxColumn
            // 
            this.bangStateStrDataGridViewTextBoxColumn.DataPropertyName = "BangStateStr";
            this.bangStateStrDataGridViewTextBoxColumn.HeaderText = "平台状态";
            this.bangStateStrDataGridViewTextBoxColumn.Name = "bangStateStrDataGridViewTextBoxColumn";
            this.bangStateStrDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // errorMessageDataGridViewTextBoxColumn
            // 
            this.errorMessageDataGridViewTextBoxColumn.DataPropertyName = "ErrorMessage";
            this.errorMessageDataGridViewTextBoxColumn.HeaderText = "异常报警信息";
            this.errorMessageDataGridViewTextBoxColumn.Name = "errorMessageDataGridViewTextBoxColumn";
            this.errorMessageDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 521);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmMain";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.FrmMain_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.bsCar)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.pnlAGVInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvAgvInfo)).EndInit();
            this.pnlBtns.ResumeLayout(false);
            this.pnlTime.ResumeLayout(false);
            this.pnlTime.PerformLayout();
            this.pnlDate.ResumeLayout(false);
            this.pnlDate.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource bsCar;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel pnlBtns;
        private System.Windows.Forms.Panel pnlTime;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Panel pnlDate;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Button btnSpray;
        private System.Windows.Forms.Button btnMoveStore;
        private System.Windows.Forms.Button btnOutStore;
        private System.Windows.Forms.Button btnInStore;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblTile;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlAGVInfo;
        private System.Windows.Forms.DataGridView gvAgvInfo;
        private System.Windows.Forms.PictureBox picClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn agvIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn carNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn bIsCommBreakDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn carIPDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn carPortDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fVoltDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn WaterAmount;
        private System.Windows.Forms.DataGridViewCheckBoxColumn lowPowerDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn speedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn currSiteDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn carStateStrDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bangStateStrDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn errorMessageDataGridViewTextBoxColumn;
    }
}