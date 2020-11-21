using Model.Comoon;
using Model.MDM;
using Model.MSM;
using RCSEditTool.FrmCommon;
using RCSEditTool.FrmExternal;
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
    public partial class FrmCallBoxDetail : BaseForm
    {
        IList<CallBoxInfo> AllCallBox = new List<CallBoxInfo>();
        CallBoxInfo NewCallBox = new CallBoxInfo();
        public FrmCallBoxDetail(IList<CallBoxInfo> AllCallBoxs, CallBoxInfo NewCallBoxInfo)
        {
            InitializeComponent();
            if (AllCallBoxs != null)
            { AllCallBox = AllCallBoxs; }
            if (NewCallBoxInfo != null)
            { NewCallBox = NewCallBoxInfo; }
        }

        private void FrmCallBoxDetail_Shown(object sender, EventArgs e)
        {
            try
            {
                if (!NewCallBox.IsNew)
                { this.txtCallBoxID.ReadOnly = true; }
                else
                { this.txtCallBoxID.ReadOnly = false; }
                //if (NewCallBox.CallBoxType == 0)
                //{
                //    this.colLocationID.OptionsColumn.AllowEdit = false;
                //    this.colLocationState.OptionsColumn.AllowEdit = false;
                //}
                //else
                //{
                //    this.colLocationID.OptionsColumn.AllowEdit = true;
                //    this.colLocationState.OptionsColumn.AllowEdit = true;
                //}
                bsCallBox.DataSource = NewCallBox;
                bsCallDetail.DataSource = NewCallBox.CallBoxDetails;
                bsTaskConfigs.DataSource = AGVDAccess.AGVClientDAccess.LoadAllTaskConfig();
                bsCallBox.ResetBindings(false);
                bsCallDetail.ResetBindings(false);
                bsTaskConfigs.ResetBindings(false);
                txtCallBoxName.Focus();
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void gvDetail_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {
                labelControl1.Focus();
                gvDetail.CloseEditor();
                labelControl1.Focus();
                e.Allow = Valid();
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
                if (gvDetail.FocusedRowHandle < 0) { return true; }
                CallBoxDetail Curr = bsCallDetail[gvDetail.FocusedRowHandle] as CallBoxDetail;
                if (Curr == null)
                { return false; }

                if (Curr.OperaType == 0 && string.IsNullOrEmpty(Curr.TaskConditonCode))
                {
                    MsgBox.ShowWarn("请维护任务条件!");
                    return false;
                }
                if (Curr.LocationID==0)
                {
                    MsgBox.ShowWarn("请维护监控储位ID!");
                    return false;
                }
                //CallBoxDetail CurrDetail = bsCallDetail.Current as CallBoxDetail;
                //if (CurrDetail != null)
                //{
                //    IList<CallBoxDetail> CallBoxToLands = bsCallDetail.List as IList<CallBoxDetail>;
                //    if (CallBoxToLands != null && CallBoxToLands.Count > 0)
                //    {
                //        if (CallBoxToLands.Where(p => p.ButtonID == CurrDetail.ButtonID).Count() > 1)
                //        {
                //            MsgBox.ShowWarn("当前按钮号重复!");
                //            return false;
                //        }
                //    }
                //}
                return true;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); return false; }
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvDetail.BeforeLeaveRow -= gvDetail_BeforeLeaveRow;
                labelControl1.Focus();
                gvDetail.CloseEditor();
                labelControl1.Focus();
                if (Valid())
                {
                    CallBoxDetail newItem = new CallBoxDetail();
                    bsCallDetail.Add(newItem);
                    bsCallDetail.ResetBindings(false);
                    bsCallDetail.MoveLast();
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
            finally
            { gvDetail.BeforeLeaveRow += gvDetail_BeforeLeaveRow; }
        }

        private void btnDele_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvDetail.BeforeLeaveRow -= gvDetail_BeforeLeaveRow;
                bsCallDetail.RemoveCurrent();
                bsCallDetail.ResetBindings(false);
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
                labelControl1.Focus();
                if (!Valid()) { return; }
                CallBoxInfo CurrCallBox = bsCallBox.Current as CallBoxInfo;
                IList<CallBoxDetail> AllCallBoxDetail = bsCallDetail.List as IList<CallBoxDetail>;
                if (CurrCallBox != null && AllCallBoxDetail != null && AllCallBoxDetail.Count > 0)
                {
                    if (CurrCallBox.IsNew && AllCallBox.Where(p => p.CallBoxID == CurrCallBox.CallBoxID).Count() > 0)
                    {
                        MsgBox.ShowWarn("呼叫器ID已存在!");
                        return;
                    }
                    if (CurrCallBox.IsNew && AllCallBox.Where(p => p.CallBoxIP == CurrCallBox.CallBoxIP).Count() > 0)
                    {
                        MsgBox.ShowWarn("呼叫器IP已存在!");
                        return;
                    }
                    CurrCallBox.CallBoxDetails = AllCallBoxDetail;
                    OperateReturnInfo opr = AGVDAccess.AGVClientDAccess.SaveCallBox(CurrCallBox);
                    MsgBox.Show(opr);
                    this.DialogResult = DialogResult.OK;
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

        private void cbxCallBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (cbxCallBoxType.EditValue.ToString() == "呼叫")
                //{
                //    this.colLocationID.OptionsColumn.AllowEdit = false;
                //    this.colLocationState.OptionsColumn.AllowEdit = false;
                //}
                //else
                //{
                //    this.colLocationID.OptionsColumn.AllowEdit = true;
                //    this.colLocationState.OptionsColumn.AllowEdit = true;
                //}
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

    }
}
