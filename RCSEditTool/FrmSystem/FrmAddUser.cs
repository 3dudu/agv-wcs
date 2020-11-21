using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using RCSEditTool;
using Model.Comoon;
using Model.MSM;
using RCSEditTool.FrmCommon;

namespace DXAGVClient.FormSystem
{
    public partial class FrmAddUser : BaseForm
    {
        string UserID = "";
        int Set = 0;
        int Index = 0;
        public FrmAddUser(string userid, int set,int index)
        {
            InitializeComponent();
            UserID = userid;
            Set = set;
            Index = index;
        }
        public UserInfo txtUser = new UserInfo();
        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (Valid())
                {
                    this.labelControl2.Focus();
                    OperateReturnInfo opr = null;
                    UserInfo Curr = bsUser.Current as UserInfo;
                    UserCategory cate = lookUpEdit1.EditValue as UserCategory;
                    String categoryCode = lookUpEdit1.EditValue.ToString();
                    if (Curr != null)
                    {
                        opr = AGVDAccess.AGVClientDAccess.saveUSer(Curr, categoryCode);
                        MsgBox.Show(opr);
                        if (opr.ReturnCode == OperateCodeEnum.Success)
                        {
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
                return;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        internal void FrmAddUser_Shown(object sender, EventArgs e)
        {
            try
            {
                CenterToParent();
                UserCategory cate = new UserCategory();
                IList<UserCategory> cates = new List<UserCategory>();
                cates= AGVDAccess.AGVClientDAccess.get_Category();
                UserInfo NewUser = new UserInfo();
                IList<UserInfo> users = new List<UserInfo>();
                users = AGVDAccess.AGVClientDAccess.select_TheLastUserID();
                bsUsertoCategory.DataSource = cates;
                bsUser.ResetBindings(false);
                if (Set == 1)
                {
                    NewUser.UserID = UserID;
                    lookUpEdit1.EditValue = cates[Index].CategoryCode;
                    lookUpEdit1.SelectedText = cates[Index].CategoryName;
                    NewUser.UserName= txtUser.UserName;
                    NewUser.PassWord = txtUser.PassWord;

                }
                else
                {
                    if (users.Count > 0)
                    {
                        NewUser.UserID = (int.Parse(users[0].UserID) + 1).ToString().PadLeft(4, '0');
                        lookUpEdit1.EditValue = cates[0].CategoryCode;
                        lookUpEdit1.SelectedText = cates[0].CategoryName;
                    }
                    else
                    {
                        NewUser.UserID = "0001";
                        lookUpEdit1.EditValue = cates[0].CategoryCode;
                        lookUpEdit1.SelectedText = cates[0].CategoryName;
                    }
                }
               

                bsUser.DataSource = NewUser;
                bsUser.ResetBindings(false);
                NewUser.IsNew = true;
                bsUser.DataSource = NewUser;


            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void txtUserID_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void bsUser_CurrentChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 验证数据有效性
        /// </summary>
        /// <returns></returns>
        private bool Valid()
        {
            try
            {
                UserInfo users = bsUser.Current as UserInfo;
                UserCategory cate = bsUsertoCategory.Current as UserCategory;
                if (users == null) { return true; }
                if (string.IsNullOrEmpty(users.PassWord))
                {
                    MsgBox.ShowWarn("请维护用户密码！");
                    return false;
                }
                if (users.PassWord.Length > 6)
                {
                    MsgBox.ShowWarn("密码不能超过6位！");
                    return false;
                }
                if (string.IsNullOrEmpty(users.UserName))
                {
                    MsgBox.ShowWarn("请维护用户名！");
                    return false;
                }
                if (string.IsNullOrEmpty(users.PassWord))
                {
                    MsgBox.ShowWarn("请维护用户密码！");
                    return false;
                }
                if (cate==null)
                {
                    MsgBox.ShowWarn("请先创建用户分类!");
                    return false;
                }
                if (string.IsNullOrEmpty(cate.CategoryCode))
                {
                    MsgBox.ShowWarn("请先创建用户分类!");
                    return false;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
                return false;
            }
        }

        private void lookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}