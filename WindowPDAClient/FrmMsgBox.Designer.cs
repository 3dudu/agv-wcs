namespace WindowPDAClient
{
    partial class FrmMsgBox
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.picMessage = new System.Windows.Forms.PictureBox();
            this.txtContent = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picMessage)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.Location = new System.Drawing.Point(98, 69);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(93, 20);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "操作提示";
            // 
            // btnYes
            // 
            this.btnYes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnYes.FlatAppearance.BorderSize = 0;
            this.btnYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYes.ForeColor = System.Drawing.Color.White;
            this.btnYes.Location = new System.Drawing.Point(27, 156);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(75, 34);
            this.btnYes.TabIndex = 3;
            this.btnYes.Text = "是";
            this.btnYes.UseVisualStyleBackColor = false;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // btnNo
            // 
            this.btnNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnNo.FlatAppearance.BorderSize = 0;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNo.ForeColor = System.Drawing.Color.White;
            this.btnNo.Location = new System.Drawing.Point(110, 156);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(75, 34);
            this.btnNo.TabIndex = 4;
            this.btnNo.Text = "否";
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(193, 156);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 34);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // picMessage
            // 
            this.picMessage.Image = global::WindowPDAClient.Properties.Resources.warm;
            this.picMessage.Location = new System.Drawing.Point(119, 13);
            this.picMessage.Name = "picMessage";
            this.picMessage.Size = new System.Drawing.Size(50, 50);
            this.picMessage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMessage.TabIndex = 0;
            this.picMessage.TabStop = false;
            // 
            // txtContent
            // 
            this.txtContent.BackColor = System.Drawing.Color.White;
            this.txtContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtContent.Font = new System.Drawing.Font("宋体", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtContent.ForeColor = System.Drawing.Color.Gray;
            this.txtContent.Location = new System.Drawing.Point(27, 98);
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(241, 52);
            this.txtContent.TabIndex = 6;
            this.txtContent.Text = "保存成功!";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.picMessage);
            this.panel1.Controls.Add(this.txtContent);
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnYes);
            this.panel1.Controls.Add(this.btnNo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(299, 206);
            this.panel1.TabIndex = 7;
            // 
            // FrmMsgBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(299, 206);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmMsgBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "提示信息";
            ((System.ComponentModel.ISupportInitialize)(this.picMessage)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picMessage;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RichTextBox txtContent;
        private System.Windows.Forms.Panel panel1;
    }
}