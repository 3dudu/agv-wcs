namespace AGVDispatchServer
{
    partial class FormStorageReset
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtFrom = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtEnd = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.cbxsotragestate = new DevExpress.XtraEditors.ComboBoxEdit();
            this.cbxLockState = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxsotragestate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxLockState.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(125, 69);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确认";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(229, 69);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(86, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(46, 25);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(40, 14);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "储位ID:";
            // 
            // txtFrom
            // 
            this.txtFrom.Location = new System.Drawing.Point(92, 23);
            this.txtFrom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(75, 20);
            this.txtFrom.TabIndex = 3;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(22, 68);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(12, 14);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "到";
            this.labelControl2.Visible = false;
            // 
            // txtEnd
            // 
            this.txtEnd.Location = new System.Drawing.Point(40, 66);
            this.txtEnd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtEnd.Name = "txtEnd";
            this.txtEnd.Size = new System.Drawing.Size(81, 20);
            this.txtEnd.TabIndex = 5;
            this.txtEnd.Visible = false;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(186, 25);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(24, 14);
            this.labelControl3.TabIndex = 6;
            this.labelControl3.Text = "改为";
            // 
            // cbxsotragestate
            // 
            this.cbxsotragestate.Location = new System.Drawing.Point(227, 23);
            this.cbxsotragestate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbxsotragestate.Name = "cbxsotragestate";
            this.cbxsotragestate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbxsotragestate.Properties.Items.AddRange(new object[] {
            "无料",
            "空料车",
            "有料"});
            this.cbxsotragestate.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbxsotragestate.Size = new System.Drawing.Size(75, 20);
            this.cbxsotragestate.TabIndex = 7;
            // 
            // cbxLockState
            // 
            this.cbxLockState.Location = new System.Drawing.Point(307, 23);
            this.cbxLockState.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbxLockState.Name = "cbxLockState";
            this.cbxLockState.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbxLockState.Properties.Items.AddRange(new object[] {
            "无锁",
            "有锁"});
            this.cbxLockState.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbxLockState.Size = new System.Drawing.Size(75, 20);
            this.cbxLockState.TabIndex = 8;
            // 
            // FormStorageReset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 103);
            this.Controls.Add(this.cbxLockState);
            this.Controls.Add(this.cbxsotragestate);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.txtEnd);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txtFrom);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormStorageReset";
            this.Text = "储位状态修改";
            this.Shown += new System.EventHandler(this.FormStorageReset_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.txtFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxsotragestate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbxLockState.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtFrom;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtEnd;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.ComboBoxEdit cbxsotragestate;
        private DevExpress.XtraEditors.ComboBoxEdit cbxLockState;
    }
}