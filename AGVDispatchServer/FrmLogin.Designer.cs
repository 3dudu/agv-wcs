namespace AGVDispatchServer
{
    partial class FrmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
            this.pictureEdit2 = new DevExpress.XtraEditors.PictureEdit();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtPass = new DevExpress.XtraEditors.TextEdit();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.txtUserID = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPass.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureEdit2
            // 
            this.pictureEdit2.EditValue = global::AGVDispatchServer.Properties.Resources.pass_word;
            this.pictureEdit2.Location = new System.Drawing.Point(13, 116);
            this.pictureEdit2.Name = "pictureEdit2";
            this.pictureEdit2.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit2.Size = new System.Drawing.Size(35, 35);
            this.pictureEdit2.TabIndex = 1;
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.EditValue = global::AGVDispatchServer.Properties.Resources.user_name;
            this.pictureEdit1.Location = new System.Drawing.Point(13, 65);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pictureEdit1.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit1.Size = new System.Drawing.Size(35, 35);
            this.pictureEdit1.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("OCR A Std", 23F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.labelControl1.Location = new System.Drawing.Point(28, 14);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(267, 33);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "AGV调度控制系统";
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(54, 121);
            this.txtPass.Name = "txtPass";
            this.txtPass.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.txtPass.Properties.Appearance.Options.UseFont = true;
            this.txtPass.Properties.PasswordChar = '*';
            this.txtPass.Size = new System.Drawing.Size(251, 30);
            this.txtPass.TabIndex = 4;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(45, 165);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(94, 32);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(184, 165);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(94, 32);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point(54, 69);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.txtUserID.Properties.Appearance.Options.UseFont = true;
            this.txtUserID.Size = new System.Drawing.Size(251, 30);
            this.txtUserID.TabIndex = 7;
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 202);
            this.Controls.Add(this.txtUserID);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtPass);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.pictureEdit2);
            this.Controls.Add(this.pictureEdit1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FrmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登陆";
            this.Shown += new System.EventHandler(this.FrmLogin_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmLogin_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPass.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserID.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.PictureEdit pictureEdit2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtPass;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.TextEdit txtUserID;
    }
}