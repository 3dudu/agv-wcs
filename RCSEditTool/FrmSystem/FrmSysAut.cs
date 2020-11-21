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
using RCSEditTool.FrmCommon;
using Model.MSM;

namespace DXAGVClient.FormSystem
{
    public partial class FrmSysAut : BaseForm
    {
        public FrmSysAut()
        {
            InitializeComponent();
        }
        public static bool timetf = false;
        private string FocusGV = "gvCategor";


        //    textBox1.Text = "success";
        //    MessageBox.Show("成功在关闭子窗体时更新了父窗体内容！");
        //}
        public void FrmSysAut_Shown(object sender, EventArgs e)
        {
            try
            {

                this.bsSysOperButton.DataSource = AGVDAccess.AGVClientDAccess.load_Button();

                bsSysOperButton.ResetBindings(false);
                this.bsCategory.DataSource = AGVDAccess.AGVClientDAccess.get_Category();
                bsCategory.ResetBindings(false);
                IList<SysOprButtonToCategory> Sotc = AGVDAccess.AGVClientDAccess.load_UserCategoryToButton();
                IList<UserCategory> uc = bsCategory.List as List<UserCategory>;
                foreach (var item in Sotc)
                {
                    foreach (var a in uc)
                    {
                        if (a.CategoryCode == item.CategoryCode)
                        {
                            a.CategoryList.Add(new SysOprButtonToCategory()
                            {
                                ButtonName = item.ButtonName,
                                ButtonType = item.ButtonType,
                                CategoryCode = item.CategoryCode
                            });

                        }


                    }
                }
                IList<UserCategory> ucs = bsCategory.List as List<UserCategory>;
                bsSysOperButton.ResetBindings(false);
                bsCategory.ResetBindings(false);

            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void gridControl1_Click(object sender, EventArgs e)
        {


        }

        private void newCategory_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvCategory.BeforeLeaveRow -= gvCategory_BeforeLeaveRow;
                using (FrmAddCategory frm = new FrmAddCategory(bsCategory.Count + 1.ToString(), 0, ""))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        this.bsCategory.DataSource = AGVDAccess.AGVClientDAccess.get_Category();
                        bsCategory.ResetBindings(false);
                        bsCategory.MoveLast();
                        gvCategory_FoucsedRowChanged(null, null);
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
            finally
            { gvCategory.BeforeLeaveRow += gvCategory_BeforeLeaveRow; }
        }

        private void newUser_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            UserCategory uc = bsCategory.Current as UserCategory;
            IList<SysOperButton> bs = bsSysOperButton.List as List<SysOperButton>;

            int count = uc.CategoryList.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                if (uc.CategoryList[i].CategoryCode == uc.CategoryCode)
                {
                    uc.CategoryList.RemoveAt(i);
                }
            }
            foreach (var item in bs)
            {
                if (item.IsSelect == true)
                {
                    uc.CategoryList.Add(new SysOprButtonToCategory()
                    {
                        CategoryCode = uc.CategoryCode,
                        ButtonName = item.ButtonName,
                        ButtonType = item.ButtonType
                    });
                }
            }

            bsCategory.ResetBindings(false);
            UserCategory curr = bsCategory.Current as UserCategory;
            if (curr == null)
            {
                MsgBox.ShowWarn("请先创建用户分类!");
                return;
            }
            using (FrmAddUser frm = new FrmAddUser(bsUser.Count.ToString(), 0, bsCategory.IndexOf(bsCategory.Current)))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    gvCategory.RefreshData();
                    //this.bsUser.DataSource = AGVDAccess.AGVClientDAccess.LoadUserByCategoryCode(curr.CategoryCode);
                    bsCategory.ResetBindings(false);
                    //  bsCategory.MoveLast();

                    gvCategory_FoucsedRowChanged(null, null);
                }

            }
        }

        public void btnOK_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvCategory.CloseEditor();
                gvOperButton.CloseEditor();
                IList<SysOperButton> bs = bsSysOperButton.List as List<SysOperButton>;
                UserCategory uc = bsCategory.Current as UserCategory;
                int count = uc.CategoryList.Count;
                for (int i = count - 1; i >= 0; i--)
                {
                    if (uc.CategoryList[i].CategoryCode == uc.CategoryCode)
                    {
                        uc.CategoryList.RemoveAt(i);
                    }
                }
                foreach (var item in bs)
                {
                    if (item.IsSelect == true)
                    {
                        uc.CategoryList.Add(new SysOprButtonToCategory()
                        {
                            CategoryCode = uc.CategoryCode,
                            ButtonName = item.ButtonName,
                            ButtonType = item.ButtonType
                        });

                    }
                }
                OperateReturnInfo opr = null;
                if (MsgBox.ShowQuestion("确定保存?") == DialogResult.Yes)
                {
                    IList<UserCategory> ucList = bsCategory.List as IList<UserCategory>;
                    IList<SysOprButtonToCategory> a = new List<SysOprButtonToCategory>();


                    foreach (var item in ucList)
                    {
                        a.Clear();
                        if (item.CategoryList.Count == 0)
                        {
                            a.Add(new SysOprButtonToCategory()
                            {
                                ButtonName = "",
                                ButtonType = 0,
                                CategoryCode = item.CategoryCode
                            });
                        }
                        else
                        {
                            a = item.CategoryList;
                        }
                        //IList<SysOprButtonToCategory> allButton  = item.CategoryList;
                        opr = AGVDAccess.AGVClientDAccess.saveButton(a);
                    }
                    MsgBox.Show(opr);
                    bsCategory.ResetBindings(false);

                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }
        private void gvCategory_Click(object sender, EventArgs e)
        {
            FocusGV = "gvCategor";
        }
        private void gcUser_Click(object sender, EventArgs e)
        {

        }

        private void gcOperButton_Click(object sender, EventArgs e)
        {

        }
        private bool Valid()
        {
            try
            {
                gvCategory.CloseEditor();
                gvUser.CloseEditor();
                gvOperButton.CloseEditor();
                if (bsCategory.Count > 0)
                {
                    UserCategory currCategory = bsCategory[gvCategory.FocusedRowHandle] as UserCategory;
                    if (currCategory == null)
                    {
                        return false;
                    }
                    if (String.IsNullOrEmpty(currCategory.CategoryName))
                    {
                        MsgBox.ShowWarn("请维护分类名称！");
                        return false;
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
                return false;
            }
        }

        private void gvCategory_FoucsedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                UserCategory curr = bsCategory.Current as UserCategory;
                if (curr == null) { return; }
                IList<SysOprButtonToCategory> ButtontoCategiryList = AGVDAccess.AGVClientDAccess.loadButtonByCategoryCode(curr.CategoryCode);
                if (ButtontoCategiryList != null)
                {
                    foreach (SysOperButton item in bsSysOperButton.List)
                    {
                        item.IsSelect = false;
                        foreach (SysOprButtonToCategory itemButn in ButtontoCategiryList)
                        {
                            if (item.ButtonName == itemButn.ButtonName)
                            { item.IsSelect = true; }
                        }
                    }
                }
                bsSysOperButton.ResetBindings(false);
                bsUser.DataSource = AGVDAccess.AGVClientDAccess.LoadUserByCategoryCode(curr.CategoryCode);
                bsUser.ResetBindings(false);




                //gvCategory.BeforeLeaveRow -= gvCategory_BeforeLeaveRow;
                //UserCategory curr = bsCategory.Current as UserCategory;
                //SysOprButtonToCategory buttontoCategory = new SysOprButtonToCategory();
                //IList<SysOprButtonToCategory> ButtontoCategiryList = AGVDAccess.AGVClientDAccess.loadButtonByCategoryCode(curr.CategoryCode);
                //IList<string> button = new List<string>();

                //foreach (var item in ButtontoCategiryList)
                //{
                //    if (!buttontoCategory.CategoryCode.Contains(item.CategoryCode))
                //    {
                //        curr.CategoryList.Add(item);
                //    }
                //}
                //IList<SysOperButton> operButton = new List<SysOperButton>();
                //foreach (SysOperButton item in bsSysOperButton.List)
                //{
                //    item.IsSelect = false;
                //    if (ButtontoCategiryList != null)
                //    {
                //        foreach (var a in curr.CategoryList)
                //        {
                //            if (item.ButtonName == a.ButtonName)
                //            {
                //                item.IsSelect = true;
                //            }
                //        }

                //    }
                //}
                //bsSysOperButton.ResetBindings(false);
                //gvCategory.BeforeLeaveRow += gvCategory_BeforeLeaveRow;
                //gvOperButton.RefreshData();
                //UserCategory cate = bsCategory.Current as UserCategory;
                //if (cate == null) return;
                //bsUser.DataSource = AGVDAccess.AGVClientDAccess.LoadUserByCategoryCode(cate.CategoryCode);
                //bsUser.ResetBindings(false);


            }
            //}
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void Cancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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

        private void gvCategory_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {
                IList<SysOperButton> bs = bsSysOperButton.List as List<SysOperButton>;
                UserCategory uc = bsCategory.Current as UserCategory;
                if (uc == null) { return; }
                int count = uc.CategoryList.Count;
                for (int i = count - 1; i >= 0; i--)
                {
                    if (uc.CategoryList[i].CategoryCode == uc.CategoryCode)
                    {
                        uc.CategoryList.RemoveAt(i);
                    }
                }
                foreach (var item in bs)
                {
                    if (item.IsSelect == true)
                    {
                        uc.CategoryList.Add(new SysOprButtonToCategory()
                        {
                            CategoryCode = uc.CategoryCode,
                            ButtonName = item.ButtonName,
                            ButtonType = item.ButtonType
                        });

                    }
                }
                bsCategory.ResetBindings(false);
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        public void delCategory_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvCategory.CloseEditor();
                gvUser.CloseEditor();
                if (bsCategory.Count <= 0)
                { return; }
                if (MsgBox.ShowQuestion("确定删除当前用户分类吗?") == DialogResult.Yes)
                {
                    UserCategory curr = bsCategory.Current as UserCategory;
                    if (curr != null)
                    {
                        OperateReturnInfo opr = AGVDAccess.AGVClientDAccess.DelCategory(curr);
                        if (opr.ReturnCode == OperateCodeEnum.Success)
                        {
                            this.bsCategory.DataSource = AGVDAccess.AGVClientDAccess.get_Category();
                            bsCategory.ResetBindings(false);
                            this.bsUser.DataSource = AGVDAccess.AGVClientDAccess.LoadUserByCategoryCode(curr.CategoryCode);
                            bsUser.ResetBindings(false);
                            bsSysOperButton.DataSource = AGVDAccess.AGVClientDAccess.load_Button();
                            bsSysOperButton.ResetBindings(false);
                            gvCategory_FoucsedRowChanged(null, null);
                        }
                        MsgBox.Show(opr);
                    }

                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void delUser_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvCategory.CloseEditor();
                gvUser.CloseEditor();
                if (bsUser.Count <= 0)
                { return; }
                if (MsgBox.ShowQuestion("确定删除当前用户吗?") == DialogResult.Yes)
                {
                    UserInfo curr = bsUser.Current as UserInfo;
                    if (curr != null)
                    {
                        OperateReturnInfo opr = AGVDAccess.AGVClientDAccess.DelUser(curr);
                        if (opr.ReturnCode == OperateCodeEnum.Success)
                        {
                            gvCategory.RefreshData();
                            bsCategory.ResetBindings(false);
                            gvCategory_FoucsedRowChanged(null, null);
                        }
                        MsgBox.Show(opr);
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void gvCategory_DoubleClick(object sender, EventArgs e)
        {
            UserCategory cate = bsCategory.Current as UserCategory;
            if (cate == null) { return; }
            using (FrmAddCategory frm = new FrmAddCategory(cate.CategoryCode, 1, cate.CategoryName))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    this.bsCategory.DataSource = AGVDAccess.AGVClientDAccess.get_Category();
                    bsCategory.ResetBindings(false);
                    bsCategory.MoveLast();
                    gvCategory_FoucsedRowChanged(null, null);
                }
            }
        }

        private void gvUser_DoubleClick(object sender, EventArgs e)
        {
            UserInfo user = bsUser.Current as UserInfo;
            if (user == null) { return; }
            using (FrmAddUser frm = new FrmAddUser(user.UserID, 1, bsCategory.IndexOf(bsCategory.Current)))
            {
                frm.txtUser = bsUser.Current as UserInfo;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    this.bsCategory.DataSource = AGVDAccess.AGVClientDAccess.get_Category();
                    bsCategory.ResetBindings(false);
                    bsCategory.MoveLast();
                    gvCategory_FoucsedRowChanged(null, null);
                }
            }
        }

        private void AllSelect_Click(object sender, EventArgs e)
        {
            gvOperButton.CloseEditor();
            IList<SysOperButton> button = bsSysOperButton.List as List<SysOperButton>;
            foreach (var item in button)
            {
                item.IsSelect = true;
            }
            bsSysOperButton.ResetBindings(false);
        }

        private void CancelSelect_Click(object sender, EventArgs e)
        {
            gvCategory.CloseEditor();
            gvOperButton.CloseEditor();
            IList<UserCategory> uca = bsCategory.List as List<UserCategory>;
            IList<SysOperButton> button = bsSysOperButton.List as List<SysOperButton>;
            foreach (var item in button)
            {
                item.IsSelect = false;
            }
            IList<UserCategory> uc = bsCategory.List as List<UserCategory>;
            bsCategory.ResetBindings(false);
            bsSysOperButton.ResetBindings(false);
        }
    }
}