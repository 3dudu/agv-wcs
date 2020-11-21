using AGVDAccess;
using DevExpress.Utils;
using Model.Comoon;
using Model.MDM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RCSEditTool.FrmCommon
{
    public partial class FrmJuctionSeg : Form
    {

        private TraJunction TraInfo { get; set; }

        public FrmJuctionSeg(TraJunction trajuc)
        {
            InitializeComponent();
            TraInfo = trajuc;
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvDetail.CloseEditor();
                gvDetail.CloseEditForm();
                gvDetail.FocusedColumn = colSegmentID;
                if (bsDetail.Count > 0 && !Valid()) { return; }
                TraJunctionSegInfo newItem = new TraJunctionSegInfo();
                newItem.SegmentID = bsDetail.Count + 1;
                bsDetail.Add(newItem);
                bsDetail.ResetBindings(false);
                bsDetail.MoveLast();
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvDetail.CloseEditor();
                gvDetail.CloseEditForm();
                gvDetail.FocusedColumn = colSegmentID;
                gvDetail.BeforeLeaveRow -= gvDetail_BeforeLeaveRow;
                bsDetail.RemoveCurrent();
                bsDetail.ResetBindings(false);
                bsDetail.MoveLast();
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvDetail.CloseEditor();
                gvDetail.CloseEditForm();
                gvDetail.FocusedColumn = colSegmentID;
                if (Valid())
                {
                    IList<TraJunctionSegInfo> traffics = bsDetail.List as IList<TraJunctionSegInfo>;
                    if (traffics != null)
                    {
                        TraInfo.Segments = traffics;
                        OperateReturnInfo opr;
                        using (WaitDialogForm dialog = new WaitDialogForm("正在保存,请稍后...", "提示"))
                        {
                            opr = AGVClientDAccess.SaveTraJunctionByOne(TraInfo);
                        }
                        MsgBox.Show(opr);
                    }
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

        private void FrmJuctionSeg_Shown(object sender, EventArgs e)
        {
            try
            {
                if (TraInfo != null)
                {
                    this.bsDetail.DataSource = AGVDAccess.AGVClientDAccess.LoadTraSegByTraID(TraInfo.TraJunctionID);
                    this.bsDetail.ResetBindings(false);
                }
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
                gvDetail.FocusedColumn = colLandCodes;
                gvDetail.FocusedColumn = colSegmentID;
                if (gvDetail.FocusedRowHandle < 0) { return true; }
                TraJunctionSegInfo Curr = bsDetail[gvDetail.FocusedRowHandle] as TraJunctionSegInfo;
                if (Curr == null) { return false; }
                if (string.IsNullOrEmpty(Curr.LandCodes))
                { return false; }
                if (!(Curr.LandCodes.StartsWith(",") && Curr.LandCodes.EndsWith(",")))
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
    }
}
