using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AGVDispatchServer
{
    public partial class FormPara : BaseForm
    {
        /// <summary>
        /// 车号
        /// </summary>
        public string AGVID { get { return txtAGVID.Text; } }

        /// <summary>
        /// 参数
        /// </summary>
        public string Para { get { return txtPara.Text; } }

        public FormPara()
        {
            InitializeComponent();
        }

        private void FormPara_Shown(object sender, EventArgs e)
        {
            this.CenterToParent();

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
