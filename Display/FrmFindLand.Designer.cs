﻿namespace Display
{
    partial class FrmFindLand
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtLandCode = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "地标号:";
            // 
            // txtLandCode
            // 
            this.txtLandCode.Location = new System.Drawing.Point(55, 23);
            this.txtLandCode.Name = "txtLandCode";
            this.txtLandCode.Size = new System.Drawing.Size(201, 21);
            this.txtLandCode.TabIndex = 1;
            // 
            // FrmFindLand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 66);
            this.Controls.Add(this.txtLandCode);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.Name = "FrmFindLand";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "查找地标";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmFindLand_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLandCode;
    }
}