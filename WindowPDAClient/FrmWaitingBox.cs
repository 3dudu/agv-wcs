using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowPDAClient
{
    public partial class FrmWaitingBox : Form
    {
        #region 窗体属性
        public string Message { get;set; }
        #endregion


        public FrmWaitingBox()
        {
            InitializeComponent();
        }

        public FrmWaitingBox(string WaitingText) : this()
        {
            this.lblMessage.Text = WaitingText;
        }
    }
}
