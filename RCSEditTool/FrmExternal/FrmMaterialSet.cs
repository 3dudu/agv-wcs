using DevExpress.Utils;
using Model.Comoon;
using Model.MDM;
using Model.MSM;
using RCSEditTool.FrmCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RCSEditTool.FrmExternal
{
    public partial class FrmMaterialSet : BaseForm
    {
        public FrmMaterialSet()
        {
            InitializeComponent();
        }

        private void FrmMaterialSet_Shown(object sender, EventArgs e)
        {
            try
            {
                bsDetail.DataSource = AGVDAccess.AGVClientDAccess.LoadAllMaterial();
                bsDetail.ResetBindings(false);
                bsDetail.MoveFirst();
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvDetail.CloseEditor();
                gvDetail.CloseEditForm();
                gvDetail.FocusedColumn = colMaterialName;
                if (bsDetail.Count > 0 && !Valid()) { return; }
                MaterialInfo newItem = new MaterialInfo();
                newItem.MaterialType = bsDetail.Count + 1;
                bsDetail.Add(newItem);
                bsDetail.ResetBindings(false);
                bsDetail.MoveLast();
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }
        private bool Valid()
        {
            try
            {
                gvDetail.CloseEditor();
                gvDetail.CloseEditForm();
                gvDetail.FocusedColumn = colMaterialType;
                gvDetail.FocusedColumn = colMaterialName;
                if (gvDetail.FocusedRowHandle < 0) { return true; }
                MaterialInfo Curr = bsDetail[gvDetail.FocusedRowHandle] as MaterialInfo;
                if (Curr == null) { return false; }
                if (string.IsNullOrEmpty(Curr.MaterialName))
                { return false; }
                return true;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); return false; }
        }
        private void btnDele_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvDetail.CloseEditor();
                gvDetail.CloseEditForm();
                gvDetail.FocusedColumn = colMaterialType;
                gvDetail.BeforeLeaveRow -= gvDetail_BeforeLeaveRow;
                bsDetail.RemoveCurrent();
                bsDetail.ResetBindings(false);
                bsDetail.MoveLast();
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
            finally
            { gvDetail.BeforeLeaveRow += gvDetail_BeforeLeaveRow; }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvDetail.CloseEditor();
                gvDetail.CloseEditForm();
                gvDetail.FocusedColumn = colMaterialType;
                if (Valid())
                {
                    IList<MaterialInfo> Material = bsDetail.List as IList<MaterialInfo>;
                    OperateReturnInfo opr;
                    using (WaitDialogForm dialog = new WaitDialogForm("正在保存,请稍后...", "提示"))
                    {
                        opr = AGVDAccess.AGVClientDAccess.SaveMaterial(Material);
                    }
                    MsgBox.Show(opr);
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Close();
                return;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void gvDetail_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {
                e.Allow = Valid();
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }
    }
}
