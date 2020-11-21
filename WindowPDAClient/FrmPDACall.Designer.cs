namespace WindowPDAClient
{
    partial class FrmPDACall
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
            this.picClose = new System.Windows.Forms.PictureBox();
            this.pnlBtn = new System.Windows.Forms.Panel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTitle
            // 
            this.pnlTitle.Controls.Add(this.lblTitle);
            this.pnlTitle.Controls.Add(this.picClose);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(793, 30);
            this.pnlTitle.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Gray;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(763, 30);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "出入库物料选择";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picClose
            // 
            this.picClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.picClose.Image = global::WindowPDAClient.Properties.Resources.Home_Exit;
            this.picClose.Location = new System.Drawing.Point(763, 0);
            this.picClose.Name = "picClose";
            this.picClose.Size = new System.Drawing.Size(30, 30);
            this.picClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picClose.TabIndex = 0;
            this.picClose.TabStop = false;
            this.picClose.Click += new System.EventHandler(this.picClose_Click);
            // 
            // pnlBtn
            // 
            this.pnlBtn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBtn.Location = new System.Drawing.Point(0, 370);
            this.pnlBtn.Name = "pnlBtn";
            this.pnlBtn.Size = new System.Drawing.Size(793, 165);
            this.pnlBtn.TabIndex = 1;
            // 
            // pnlMain
            // 
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 30);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(793, 340);
            this.pnlMain.TabIndex = 2;
            // 
            // FrmPDACall
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 535);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlBtn);
            this.Controls.Add(this.pnlTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmPDACall";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmPDACall";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.FrmPDACall_Shown);
            this.pnlTitle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.Panel pnlBtn;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox picClose;
    }
}