namespace Display
{
    partial class FrmSysConnSet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSysConnSet));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtDBName = new System.Windows.Forms.TextBox();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtServePort = new System.Windows.Forms.TextBox();
            this.txtServeIP = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtPass);
            this.groupBox2.Controls.Add(this.txtUser);
            this.groupBox2.Controls.Add(this.txtDBName);
            this.groupBox2.Controls.Add(this.txtIP);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.ForeColor = System.Drawing.Color.Black;
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(298, 152);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "数据库设置";
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(102, 106);
            this.txtPass.Name = "txtPass";
            this.txtPass.PasswordChar = '*';
            this.txtPass.Size = new System.Drawing.Size(188, 26);
            this.txtPass.TabIndex = 7;
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(102, 78);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(188, 26);
            this.txtUser.TabIndex = 6;
            // 
            // txtDBName
            // 
            this.txtDBName.Location = new System.Drawing.Point(102, 50);
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.Size = new System.Drawing.Size(188, 26);
            this.txtDBName.TabIndex = 5;
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(102, 22);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(188, 26);
            this.txtIP.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(56, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "密码:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "用户名:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "数据库名称:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据库地址:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtServePort);
            this.groupBox1.Controls.Add(this.txtServeIP);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(315, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(290, 151);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "上位机通信设置";
            // 
            // txtServePort
            // 
            this.txtServePort.Location = new System.Drawing.Point(98, 50);
            this.txtServePort.Name = "txtServePort";
            this.txtServePort.Size = new System.Drawing.Size(188, 26);
            this.txtServePort.TabIndex = 3;
            // 
            // txtServeIP
            // 
            this.txtServeIP.Location = new System.Drawing.Point(98, 22);
            this.txtServeIP.Name = "txtServeIP";
            this.txtServeIP.Size = new System.Drawing.Size(188, 26);
            this.txtServeIP.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 16);
            this.label6.TabIndex = 1;
            this.label6.Text = "上位机端口:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 16);
            this.label5.TabIndex = 0;
            this.label5.Text = "上位机地址:";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Controls.Add(this.btnTest);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 169);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(610, 41);
            this.panel2.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(361, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(105, 33);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOK.BackColor = System.Drawing.Color.White;
            this.btnOK.ForeColor = System.Drawing.Color.Black;
            this.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(253, 4);
            this.btnOK.Margin = new System.Windows.Forms.Padding(0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(105, 33);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnTest
            // 
            this.btnTest.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnTest.BackColor = System.Drawing.Color.White;
            this.btnTest.ForeColor = System.Drawing.Color.Black;
            this.btnTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTest.Location = new System.Drawing.Point(145, 4);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(105, 33);
            this.btnTest.TabIndex = 1;
            this.btnTest.Text = "测试";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // FrmSysConnSet
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 210);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.KeyPreview = true;
            this.Name = "FrmSysConnSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "调度通信设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSysConnSet_FormClosing);
            this.Shown += new System.EventHandler(this.FrmSysConnSet_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSysConnSet_KeyDown);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.TextBox txtDBName;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtServePort;
        private System.Windows.Forms.TextBox txtServeIP;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnTest;
    }
}