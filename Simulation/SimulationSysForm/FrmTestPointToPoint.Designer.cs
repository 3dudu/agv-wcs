namespace Simulation.SimulationSysForm
{
    partial class FrmTestPointToPoint
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.txtBeginLandCode = new DevExpress.XtraEditors.TextEdit();
            this.txtEndLandCode = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBeginLandCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndLandCode.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(64, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "开始地标号:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(226, 12);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(64, 14);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "结束地标号:";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(27, 36);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(126, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(292, 36);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(126, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtBeginLandCode
            // 
            this.txtBeginLandCode.Location = new System.Drawing.Point(81, 10);
            this.txtBeginLandCode.Name = "txtBeginLandCode";
            this.txtBeginLandCode.Size = new System.Drawing.Size(123, 20);
            this.txtBeginLandCode.TabIndex = 4;
            // 
            // txtEndLandCode
            // 
            this.txtEndLandCode.Location = new System.Drawing.Point(295, 10);
            this.txtEndLandCode.Name = "txtEndLandCode";
            this.txtEndLandCode.Size = new System.Drawing.Size(123, 20);
            this.txtEndLandCode.TabIndex = 5;
            // 
            // FrmTestPointToPoint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 65);
            this.Controls.Add(this.txtEndLandCode);
            this.Controls.Add(this.txtBeginLandCode);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.MaximizeBox = false;
            this.Name = "FrmTestPointToPoint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "测试两点生成路线";
            ((System.ComponentModel.ISupportInitialize)(this.txtBeginLandCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndLandCode.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.TextEdit txtBeginLandCode;
        private DevExpress.XtraEditors.TextEdit txtEndLandCode;
    }
}