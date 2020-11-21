namespace WindowPDAClient
{
    partial class FrmConfig
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
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.picExit = new System.Windows.Forms.PictureBox();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.btnWarter = new System.Windows.Forms.Button();
            this.btnCharge = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.timNightEndTime = new DevExpress.XtraEditors.TimeEdit();
            this.timNightBeginTime = new DevExpress.XtraEditors.TimeEdit();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.timDayBeginTime = new DevExpress.XtraEditors.TimeEdit();
            this.timDayEndTime = new DevExpress.XtraEditors.TimeEdit();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picExit)).BeginInit();
            this.pnlBtn.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timNightEndTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timNightBeginTime.Properties)).BeginInit();
            this.GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timDayBeginTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timDayEndTime.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTitle
            // 
            this.pnlTitle.BackColor = System.Drawing.Color.Gray;
            this.pnlTitle.Controls.Add(this.lblTitle);
            this.pnlTitle.Controls.Add(this.picExit);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(992, 30);
            this.pnlTitle.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(962, 30);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "喷淋周期设置";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picExit
            // 
            this.picExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.picExit.Image = global::WindowPDAClient.Properties.Resources.Home_Exit;
            this.picExit.Location = new System.Drawing.Point(962, 0);
            this.picExit.Name = "picExit";
            this.picExit.Size = new System.Drawing.Size(30, 30);
            this.picExit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picExit.TabIndex = 0;
            this.picExit.TabStop = false;
            this.picExit.Click += new System.EventHandler(this.picExit_Click);
            // 
            // pnlBtn
            // 
            this.pnlBtn.Controls.Add(this.btnWarter);
            this.pnlBtn.Controls.Add(this.btnCharge);
            this.pnlBtn.Controls.Add(this.btnStop);
            this.pnlBtn.Controls.Add(this.btnStart);
            this.pnlBtn.Controls.Add(this.btnSave);
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 529);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(992, 63);
            this.pnlBtn.TabIndex = 1;
            // 
            // btnWarter
            // 
            this.btnWarter.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnWarter.BackColor = System.Drawing.Color.Orange;
            this.btnWarter.FlatAppearance.BorderSize = 0;
            this.btnWarter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWarter.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWarter.Location = new System.Drawing.Point(710, 10);
            this.btnWarter.Name = "btnWarter";
            this.btnWarter.Size = new System.Drawing.Size(117, 43);
            this.btnWarter.TabIndex = 4;
            this.btnWarter.Text = "加水";
            this.btnWarter.UseVisualStyleBackColor = false;
            // 
            // btnCharge
            // 
            this.btnCharge.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCharge.BackColor = System.Drawing.Color.RosyBrown;
            this.btnCharge.FlatAppearance.BorderSize = 0;
            this.btnCharge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCharge.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCharge.Location = new System.Drawing.Point(574, 10);
            this.btnCharge.Name = "btnCharge";
            this.btnCharge.Size = new System.Drawing.Size(117, 43);
            this.btnCharge.TabIndex = 3;
            this.btnCharge.Text = "充电";
            this.btnCharge.UseVisualStyleBackColor = false;
            // 
            // btnStop
            // 
            this.btnStop.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnStop.BackColor = System.Drawing.Color.IndianRed;
            this.btnStop.FlatAppearance.BorderSize = 0;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStop.Location = new System.Drawing.Point(438, 10);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(117, 43);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = false;
            // 
            // btnStart
            // 
            this.btnStart.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnStart.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStart.Location = new System.Drawing.Point(302, 10);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(117, 43);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "启动";
            this.btnStart.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.Location = new System.Drawing.Point(166, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(117, 43);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.groupBox2);
            this.pnlMain.Controls.Add(this.GroupBox1);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 30);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(992, 499);
            this.pnlMain.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.timNightEndTime);
            this.groupBox2.Controls.Add(this.timNightBeginTime);
            this.groupBox2.Controls.Add(this.comboBox2);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.groupBox2.Location = new System.Drawing.Point(0, 328);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(992, 171);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "夜间喷淋周期设置";
            // 
            // timNightEndTime
            // 
            this.timNightEndTime.EditValue = new System.DateTime(2019, 3, 1, 0, 0, 0, 0);
            this.timNightEndTime.Location = new System.Drawing.Point(425, 49);
            this.timNightEndTime.Name = "timNightEndTime";
            this.timNightEndTime.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.timNightEndTime.Properties.Appearance.Options.UseFont = true;
            this.timNightEndTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.timNightEndTime.Size = new System.Drawing.Size(199, 30);
            this.timNightEndTime.TabIndex = 7;
            // 
            // timNightBeginTime
            // 
            this.timNightBeginTime.EditValue = new System.DateTime(2019, 3, 1, 0, 0, 0, 0);
            this.timNightBeginTime.Location = new System.Drawing.Point(189, 49);
            this.timNightBeginTime.Name = "timNightBeginTime";
            this.timNightBeginTime.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.timNightBeginTime.Properties.Appearance.Options.UseFont = true;
            this.timNightBeginTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.timNightBeginTime.Size = new System.Drawing.Size(199, 30);
            this.timNightBeginTime.TabIndex = 6;
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "1小时一次",
            "2小时一次",
            "3小时一次",
            "4小时一次",
            "5小时一次"});
            this.comboBox2.Location = new System.Drawing.Point(191, 118);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(245, 28);
            this.comboBox2.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(395, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 20);
            this.label6.TabIndex = 4;
            this.label6.Text = "至";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(135, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 20);
            this.label4.TabIndex = 1;
            this.label4.Text = "频率:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(115, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "时间段:";
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.timDayBeginTime);
            this.GroupBox1.Controls.Add(this.timDayEndTime);
            this.GroupBox1.Controls.Add(this.comboBox1);
            this.GroupBox1.Controls.Add(this.label5);
            this.GroupBox1.Controls.Add(this.label2);
            this.GroupBox1.Controls.Add(this.label1);
            this.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.GroupBox1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.GroupBox1.ForeColor = System.Drawing.Color.Red;
            this.GroupBox1.Location = new System.Drawing.Point(0, 0);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(992, 328);
            this.GroupBox1.TabIndex = 0;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "白天喷淋周期设置";
            // 
            // timDayBeginTime
            // 
            this.timDayBeginTime.EditValue = new System.DateTime(2019, 3, 1, 0, 0, 0, 0);
            this.timDayBeginTime.Location = new System.Drawing.Point(189, 95);
            this.timDayBeginTime.Name = "timDayBeginTime";
            this.timDayBeginTime.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.timDayBeginTime.Properties.Appearance.Options.UseFont = true;
            this.timDayBeginTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.timDayBeginTime.Size = new System.Drawing.Size(199, 30);
            this.timDayBeginTime.TabIndex = 8;
            // 
            // timDayEndTime
            // 
            this.timDayEndTime.EditValue = new System.DateTime(2019, 3, 1, 0, 0, 0, 0);
            this.timDayEndTime.Location = new System.Drawing.Point(425, 93);
            this.timDayEndTime.Name = "timDayEndTime";
            this.timDayEndTime.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.timDayEndTime.Properties.Appearance.Options.UseFont = true;
            this.timDayEndTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.timDayEndTime.Size = new System.Drawing.Size(199, 30);
            this.timDayEndTime.TabIndex = 7;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "1小时一次",
            "2小时一次",
            "3小时一次",
            "4小时一次",
            "5小时一次"});
            this.comboBox1.Location = new System.Drawing.Point(190, 169);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(235, 28);
            this.comboBox1.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(394, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 20);
            this.label5.TabIndex = 3;
            this.label5.Text = "至";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(135, 172);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "频率:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(115, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "时间段:";
            // 
            // FrmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 592);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlBtn);
            this.Controls.Add(this.pnlTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmConfig";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picExit)).EndInit();
            this.pnlBtn.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timNightEndTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timNightBeginTime.Properties)).EndInit();
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timDayBeginTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timDayEndTime.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.PictureBox picExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlBtn;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox GroupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnWarter;
        private System.Windows.Forms.Button btnCharge;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnSave;
        private DevExpress.XtraEditors.TimeEdit timNightEndTime;
        private DevExpress.XtraEditors.TimeEdit timNightBeginTime;
        private DevExpress.XtraEditors.TimeEdit timDayBeginTime;
        private DevExpress.XtraEditors.TimeEdit timDayEndTime;
    }
}