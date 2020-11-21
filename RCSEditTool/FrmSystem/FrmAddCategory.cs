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
    public partial class FrmAddCategory : BaseForm
    {
        string categoryCode = "";
        int Set = 0;
        string categoryName = "";
        public UserCategory newCate = null;
        public FrmAddCategory(String CategoryCode,int set,string CategoryName)
        {
            InitializeComponent();
            categoryCode = CategoryCode;
            Set = set;
            categoryName = CategoryName;
        }
        public FrmSysAut Gz_fm;
        //public FrmAddCategory(FrmSysAut t_FrmSysAut)
        //{
        //    InitializeComponent();
        //    Gz_fm = t_FrmSysAut;
        //}
        public void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (Valid())
                {
                    this.labelControl1.Focus();
                    OperateReturnInfo opr = null;
                    if (bsCategory.Count <= 0) { return; }
                    UserCategory Curr = bsCategory.Current as UserCategory;
                    if (Curr != null)
                    {
                        IList<UserCategory> cate = new List<UserCategory>();
                        cate.Add(Curr);
                        opr = AGVDAccess.AGVClientDAccess.saveCategory(cate);
                        this.mid_obj = cate;
                        if (opr.ReturnCode == OperateCodeEnum.Success)
                        {
                            this.DialogResult = DialogResult.OK;
                            MsgBox.ShowWarn("保存成功!");
                            this.Close();
                        }
                        else { MsgBox.ShowWarn(opr.ReturnInfo.ToString()); }
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
            { MsgBox.ShowError(ex.Message); }
        }

        private void FrmAddCategory_Shown(object sender, EventArgs e)
        {
            try
            {
                    CenterToParent();
                    UserCategory newUercate = new UserCategory();
                    IList<UserCategory> listcate = new List<UserCategory>();
                    listcate = AGVDAccess.AGVClientDAccess.select_TheLastFromCategory();
                    if (Set == 1)
                    {
                        newUercate.CategoryCode = categoryCode;
                    newUercate.CategoryName = categoryName;
                    }
                    else
                    {
                        if (listcate.Count > 0)
                        {
                        newUercate.CategoryCode = (int.Parse(listcate[0].CategoryCode) + 1).ToString().PadLeft(4, '0');
                        }
                        else
                        {
                            newUercate.CategoryCode = "0001";
                        }
                    }
                    bsCategory.DataSource = newUercate;
                    bsCategory.ResetBindings(false);

            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
          
        }
        /// <summary>
        /// 验证数据有效性
        /// </summary>
        /// <returns></returns>
        private bool Valid()
        {
            try
            {
                UserCategory users = bsCategory.Current as UserCategory;
                if (string.IsNullOrEmpty(users.CategoryName))
                {
                    MsgBox.ShowWarn("用户分类名不能为空！");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message); return false;
            }
        }
    }
    
}