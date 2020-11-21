using DevExpress.XtraEditors;
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
    public partial class FormStorageReset : BaseForm
    {
        public FormStorageReset()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 起始
        /// </summary>
        public int StartID { get; set; }

        /// <summary>
        /// 结束
        /// </summary>
        public int EndID { get; set; }

        /// <summary>
        /// 存储状态
        /// </summary>
        public int StorageState { get; set; }

        /// <summary>
        /// 锁状态
        /// </summary>
        public int LockState { get; set; }
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.StartID = int.Parse(this.txtFrom.Text.Trim());
                //this.EndID = int.Parse(this.txtEnd.Text.Trim());
                //if (StartID > EndID)
                //{
                //    XtraMessageBox.Show("开始序号应小于结束序号");
                //    return;
                //}
                this.StorageState = this.cbxsotragestate.SelectedIndex;
                this.LockState = this.cbxLockState.SelectedIndex;
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("请输入正确的数字");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void FormStorageReset_Shown(object sender, EventArgs e)
        {
            this.CenterToParent();
        }
    }
}
