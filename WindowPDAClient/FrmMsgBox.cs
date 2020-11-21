using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowPDAClient
{
    public partial class FrmMsgBox : Form
    {
        public FrmMsgBox()
        {
            InitializeComponent();
        }

        public FrmMsgBox(string MsgTitle, string MsgContent, MsgTypeEnum MsgType, MsgBtnTypeEnum MsgBtn) : this()
        {
            this.lblTitle.Text = MsgTitle;
            this.txtContent.Text = MsgContent;
            if (MsgType == MsgTypeEnum.Question)
            {
                this.picMessage.Image = global::WindowPDAClient.Properties.Resources.Ask;
            }
            else if (MsgType == MsgTypeEnum.Success)
            {
                this.picMessage.Image = global::WindowPDAClient.Properties.Resources.OK;
            }

            if (MsgBtn == MsgBtnTypeEnum.YesCancel)
            {
                this.btnNo.Visible = false;
            }
            else if (MsgBtn == MsgBtnTypeEnum.YesNo)
            {
                this.btnCancel.Visible = false;
                this.btnNo.Location = btnCancel.Location;
            }
            else if (MsgBtn == MsgBtnTypeEnum.YesNoCancel)
            { }
            else
            {
                this.btnNo.Visible = false;
                this.btnCancel.Visible = false;
                this.btnYes.Location = btnNo.Location;
            }
        }

        public FrmMsgBox(string MsgTitle, string MsgContent) : this()
        {
            this.lblTitle.Text = MsgTitle;
            this.txtContent.Text = MsgContent;
            this.btnNo.Visible = false;
            this.btnCancel.Visible = false;
            this.btnYes.Location = btnNo.Location;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
