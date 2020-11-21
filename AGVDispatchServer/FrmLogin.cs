using AGVDAccess;
using Model.Comoon;
using SQLServerOperator;
using System;
using System.Collections;
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
    public partial class FrmLogin : DevExpress.XtraEditors.XtraForm
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void FrmLogin_Shown(object sender, EventArgs e)
        {
            txtUserID.Focus();
        }

        private void FrmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnOK_Click(null, null);
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                string UserID = this.txtUserID.Text.Trim();
                string Pass = txtPass.Text.Trim();
                if (!string.IsNullOrEmpty(UserID) && !string.IsNullOrEmpty(Pass))
                {
                    UserInfo user = AGVServerDAccess.LoadUserInfo(UserID, Pass);
                    if (user == null)
                    {
                        MessageBox.Show("用户名或密码不正确!");
                        return;
                    }
                    else
                    {
                        IList<SysOprButtonToCategory> LoadUserOprBtns = AGVServerDAccess.LoadUserOprBtn(UserID);
                        CurrentLogin.CurrentUser = user;
                        CurrentLogin.SysOprButtons = LoadUserOprBtns;
                        this.DialogResult = DialogResult.OK;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(UserID))
                    { txtUserID.Focus(); }
                    if (string.IsNullOrEmpty(Pass))
                    { txtPass.Focus(); }
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
                return;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
    }
}
