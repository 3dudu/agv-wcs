namespace AGVDispatchServer
{
    partial class FrmSetCurrSite
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSetCurrSite));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtAGVID = new DevExpress.XtraEditors.TextEdit();
            this.txtCurrSite = new DevExpress.XtraEditors.TextEdit();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtAGVID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrSite.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(24, 17);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(40, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "车辆ID:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(0, 43);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(64, 14);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "当前站点号:";
            // 
            // txtAGVID
            // 
            this.txtAGVID.Location = new System.Drawing.Point(70, 14);
            this.txtAGVID.Name = "txtAGVID";
            this.txtAGVID.Size = new System.Drawing.Size(188, 20);
            this.txtAGVID.TabIndex = 2;
            // 
            // txtCurrSite
            // 
            this.txtCurrSite.Location = new System.Drawing.Point(70, 42);
            this.txtCurrSite.Name = "txtCurrSite";
            this.txtCurrSite.Size = new System.Drawing.Size(188, 20);
            this.txtCurrSite.TabIndex = 3;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(39, 69);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(183, 69);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FrmSetCarSite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(269, 96);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtCurrSite);
            this.Controls.Add(this.txtAGVID);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Name = "FrmSetCarSite";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设置车辆当前站点";
            ((System.ComponentModel.ISupportInitialize)(this.txtAGVID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCurrSite.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtAGVID;
        private DevExpress.XtraEditors.TextEdit txtCurrSite;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
    }
}