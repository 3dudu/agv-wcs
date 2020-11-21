using AGVDAccess;
using Model.Comoon;
using RCSEditTool;
using RCSEditTool.FrmCommon;
using RCSEditTool.FrmSystem;
using ShilinetSoftVerify;
using SQLServerOperator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DXAGVClient
{
    public partial class FrmLogin : BaseForm
    {
        public FrmLogin()
        {
            InitializeComponent();
            //判断数据库链接
            bool isSucc = true;
            try
            {
                ConnectConfigTool.setDBase();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message + "请检查...");
                isSucc = false;
            }
            if (ConnectConfigTool.DBase == null || !isSucc)
            {
                //弹出维护数据库维护界面
                using (FrmSysConnSet frm = new FrmSysConnSet())
                {
                    if (frm.ShowDialog() != DialogResult.OK)
                    {
                        Application.ExitThread();
                        Application.Exit();
                        return;
                    }
                    else
                    {
                        ConnectConfigTool.setDBase();
                    }
                }
            }
        }

        private void FrmLogin_Shown(object sender, EventArgs e)
        {
            txtUserID.Focus();

            if (!CheckSoft())
            {
                Application.ExitThread();
                Application.Exit();
                return;
            }

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
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                string UserID = this.txtUserID.Text.Trim();
                string Pass = txtPass.Text.Trim();
                if (!string.IsNullOrEmpty(UserID)&&!string.IsNullOrEmpty(Pass))
                {
                    UserInfo user = AGVDAccess.AGVClientDAccess.GetUserInfo(UserID, Pass);
                    if (user == null)
                    {
                        MsgBox.ShowWarn("用户名或密码不正确!");
                        return;
                    }
                    else
                    {
                        CurrentLogin.CurrentUser = user;
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
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Application.ExitThread();
                Application.Exit();
                return;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private bool CheckSoft()
        {
            try
            {
                //ReturnInfo op = Verify.VerifyCarAmount(4);
                //if (!op.Result)
                //{
                //    if (!string.IsNullOrEmpty(op.Msg))
                //    { MsgBox.ShowError(op.Msg); }
                //    else
                //    { MsgBox.ShowError("序列号过期!"); }
                //    return false;
                //}
                return true;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); return false; }
            //return true;
        }

    }
}
