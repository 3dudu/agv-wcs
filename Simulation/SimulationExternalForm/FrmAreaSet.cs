using DevExpress.Utils;
using Model.Comoon;
using Model.MDM;
using Model.MSM;
using Simulation.SimulationCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulation.SimulationExternalForm
{
    public partial class FrmAreaSet : BaseForm
    {
        public FrmAreaSet()
        {
            InitializeComponent();
        }

        private void FrmAreaSet_Shown(object sender, EventArgs e)
        {
            try
            {
                bsDetail.DataSource = AGVDAccess.AGVClientDAccess.LoadAllArea();
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
                gvDetail.FocusedColumn = colAreaName;
                if (bsDetail.Count > 0 && !Valid()) { return; }
                AreaInfo newItem = new AreaInfo();
                IList<AreaInfo> AllAreaInfo = bsDetail.List as IList<AreaInfo>;
                int OwnArea = 1;
                if (AllAreaInfo != null && AllAreaInfo.Count > 0)
                { OwnArea = AllAreaInfo.Max(p => p.OwnArea) + 1; }
                newItem.OwnArea = OwnArea;
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
                gvDetail.FocusedColumn = colOwnArea;
                gvDetail.FocusedColumn = colAreaName;
                if (gvDetail.FocusedRowHandle < 0) { return true; }
                AreaInfo Curr = bsDetail[gvDetail.FocusedRowHandle] as AreaInfo;
                if (Curr == null) { return false; }
                if (string.IsNullOrEmpty(Curr.AreaName))
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
                gvDetail.FocusedColumn = colOwnArea;
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
                gvDetail.FocusedColumn = colOwnArea;
                if (Valid())
                {
                    IList<AreaInfo> Area = bsDetail.List as IList<AreaInfo>;
                    OperateReturnInfo opr;
                    using (WaitDialogForm dialog = new WaitDialogForm("正在保存,请稍后...", "提示"))
                    {
                        opr = AGVDAccess.AGVClientDAccess.SaveArea(Area);
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
