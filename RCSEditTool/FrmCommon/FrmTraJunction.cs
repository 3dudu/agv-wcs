using AGVDAccess;
using DevExpress.Utils;
using System;
using System.Collections.Generic;
using RCSEditTool.FrmCommon;
using Model.MDM;
using Model.MSM;
using Model.Comoon;

namespace RCSEditTool.FrmCommon
{
    public partial class FrmTraJunction : BaseForm
    {
        public FrmTraJunction()
        {
            InitializeComponent();
        }

        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvDetail.CloseEditor();
                gvDetail.CloseEditForm();
                gvDetail.FocusedColumn = colID;
                if (bsDetail.Count > 0 && !Valid()) { return; }
                TraJunction newItem = new TraJunction();
                newItem.TraJunctionID = bsDetail.Count + 1;
                bsDetail.Add(newItem);
                bsDetail.ResetBindings(false);
                bsDetail.MoveLast();
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvDetail.CloseEditor();
                gvDetail.CloseEditForm();
                gvDetail.FocusedColumn = colID;
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
                gvDetail.FocusedColumn = colID;
                if (Valid())
                {
                    IList<TraJunction> traffics = bsDetail.List as IList<TraJunction>;
                    OperateReturnInfo opr;
                    using (WaitDialogForm dialog = new WaitDialogForm("正在保存,请稍后...", "提示"))
                    {
                        opr = AGVClientDAccess.SaveTraJunction(traffics);
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
        private bool Valid()
        {
            try
            {
                gvDetail.CloseEditor();
                gvDetail.CloseEditForm();
                gvDetail.FocusedColumn = colCarnumber;
                gvDetail.FocusedColumn = colID;
                if (gvDetail.FocusedRowHandle < 0) { return true; }
                TraJunction Curr = bsDetail[gvDetail.FocusedRowHandle] as TraJunction;
                if (Curr == null) { return false; }
                if (string.IsNullOrEmpty(Curr.JunctionLandMarkCodes))
                { return false; }
                if (!(Curr.JunctionLandMarkCodes.StartsWith(",") && Curr.JunctionLandMarkCodes.EndsWith(",")))
                {
                    MsgBox.ShowWarn("地标集未以逗号开头或结尾!");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); return false; }
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

        private void gvDetail_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void FrmTraJunction_Shown(object sender, EventArgs e)
        {
            try
            {
                IList<TraJunction> Traffics = AGVClientDAccess.GetTraJunction();
                this.bsDetail.DataSource = Traffics;
                bsDetail.ResetBindings(false);
                bsDetail.MoveLast();
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void gcDetail_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (gvDetail.FocusedRowHandle < 0) { return; }
                TraJunction Curr = bsDetail[gvDetail.FocusedRowHandle] as TraJunction;
                if (Curr != null)
                {
                    using (FrmJuctionSeg frm = new FrmJuctionSeg(Curr))
                    {
                        frm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }
    }
}
