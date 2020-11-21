namespace PlcSMICECallAGV
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtIP = new DevExpress.XtraEditors.TextEdit();
            this.txtPort = new DevExpress.XtraEditors.TextEdit();
            this.txtReadAddr = new DevExpress.XtraEditors.TextEdit();
            this.txtRecv = new System.Windows.Forms.TextBox();
            this.btnConnect = new DevExpress.XtraEditors.SimpleButton();
            this.btnRead = new DevExpress.XtraEditors.SimpleButton();
            this.btnOff = new DevExpress.XtraEditors.SimpleButton();
            this.btnClear = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtIP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReadAddr.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(24, 26);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(49, 18);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "IP地址:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(24, 55);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(50, 18);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "端口号:";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(8, 84);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(65, 18);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "读取地址:";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(5, 121);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(65, 18);
            this.labelControl4.TabIndex = 3;
            this.labelControl4.Text = "读取内容:";
            // 
            // txtIP
            // 
            this.txtIP.EditValue = "192.168.40.30";
            this.txtIP.Location = new System.Drawing.Point(79, 22);
            this.txtIP.Margin = new System.Windows.Forms.Padding(4);
            this.txtIP.Name = "txtIP";
            this.txtIP.Properties.Mask.EditMask = "((25[0-5]|2[0-4]\\d|1\\d{2}|[1-9]?\\d)\\.){3}(25[0-5]|2[0-4]\\d|1\\d{2}|[1-9]?\\d)";
            this.txtIP.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtIP.Size = new System.Drawing.Size(324, 24);
            this.txtIP.TabIndex = 4;
            // 
            // txtPort
            // 
            this.txtPort.EditValue = "102";
            this.txtPort.Location = new System.Drawing.Point(79, 52);
            this.txtPort.Margin = new System.Windows.Forms.Padding(4);
            this.txtPort.Name = "txtPort";
            this.txtPort.Properties.Mask.EditMask = "([1-9][0-9]*)";
            this.txtPort.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtPort.Size = new System.Drawing.Size(324, 24);
            this.txtPort.TabIndex = 5;
            // 
            // txtReadAddr
            // 
            this.txtReadAddr.EditValue = "DB1000.0";
            this.txtReadAddr.Location = new System.Drawing.Point(79, 82);
            this.txtReadAddr.Margin = new System.Windows.Forms.Padding(4);
            this.txtReadAddr.Name = "txtReadAddr";
            this.txtReadAddr.Size = new System.Drawing.Size(324, 24);
            this.txtReadAddr.TabIndex = 6;
            // 
            // txtRecv
            // 
            this.txtRecv.Location = new System.Drawing.Point(76, 126);
            this.txtRecv.Margin = new System.Windows.Forms.Padding(4);
            this.txtRecv.Multiline = true;
            this.txtRecv.Name = "txtRecv";
            this.txtRecv.Size = new System.Drawing.Size(449, 67);
            this.txtRecv.TabIndex = 7;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(423, 20);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(100, 29);
            this.btnConnect.TabIndex = 8;
            this.btnConnect.Text = "连接";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(423, 81);
            this.btnRead.Margin = new System.Windows.Forms.Padding(4);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(100, 29);
            this.btnRead.TabIndex = 9;
            this.btnRead.Text = "读取";
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnOff
            // 
            this.btnOff.Location = new System.Drawing.Point(423, 284);
            this.btnOff.Margin = new System.Windows.Forms.Padding(4);
            this.btnOff.Name = "btnOff";
            this.btnOff.Size = new System.Drawing.Size(100, 32);
            this.btnOff.TabIndex = 10;
            this.btnOff.Text = "断开";
            this.btnOff.Click += new System.EventHandler(this.btnOff_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(315, 285);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 29);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "清除";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(5, 211);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(65, 18);
            this.labelControl5.TabIndex = 12;
            this.labelControl5.Text = "写入内容:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(76, 201);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(339, 67);
            this.textBox1.TabIndex = 13;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(423, 211);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(4);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(100, 29);
            this.simpleButton1.TabIndex = 14;
            this.simpleButton1.Text = "写入";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 322);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnOff);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtRecv);
            this.Controls.Add(this.txtReadAddr);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "西门子PLC呼叫AGV服务系统";
            this.Shown += new System.EventHandler(this.FrmMain_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.txtIP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReadAddr.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtIP;
        private DevExpress.XtraEditors.TextEdit txtPort;
        private DevExpress.XtraEditors.TextEdit txtReadAddr;
        private System.Windows.Forms.TextBox txtRecv;
        private DevExpress.XtraEditors.SimpleButton btnConnect;
        private DevExpress.XtraEditors.SimpleButton btnRead;
        private DevExpress.XtraEditors.SimpleButton btnOff;
        private DevExpress.XtraEditors.SimpleButton btnClear;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private System.Windows.Forms.TextBox textBox1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}