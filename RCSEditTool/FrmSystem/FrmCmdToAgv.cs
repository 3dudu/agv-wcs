using Model.Comoon;
using Model.MDM;
using Model.MSM;
using RCSEditTool.FrmCommon;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RCSEditTool.FrmSystem
{
    public partial class FrmCmdToAgv : BaseForm
    {
        public FrmCmdToAgv()
        {
            InitializeComponent();
        }

        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvAction.CloseEditor();
                using (FrmAGVCommod frm = new FrmAGVCommod(0, bsActions.Count + 1))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        if (frm.mid_obj != null)
                        {
                            this.bsActions.Add(frm.mid_obj as CmdInfo);
                            bsActions.MoveLast();
                            bsActions.ResetBindings(false);
                        }
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnDele_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvAction.CloseEditor();
                CmdInfo currAction = bsActions.Current as CmdInfo;
                if (currAction == null) { return; }
                bsActions.Remove(currAction);
                bsActions.ResetBindings(false);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvAction.CloseEditor();
                IList<CmdInfo> acts = bsActions.List as IList<CmdInfo>;
                if (acts == null) { return; }
                if (acts.Count <= 0) { return; }
                OperateReturnInfo opr = AGVDAccess.AGVClientDAccess.Save_agv_Cmd(acts);
                MsgBox.Show(opr);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
            return;
        }

        private void FrmCmdToAgv_Shown(object sender, EventArgs e)
        {
            try
            {
                //OperateReturnInfo opr = AGVDAccess.AGVClientDAccess.InitSysCmd();
                //if (opr.ReturnCode == OperateCodeEnum.Failed)
                //{ MsgBox.Show(opr); }
                IList<CmdInfo> res = AGVDAccess.AGVClientDAccess.LoadAction();
                bsActions.DataSource = res;
                bsActions.ResetBindings(false);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void gvAction_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                CmdInfo currAction = bsActions.Current as CmdInfo;
                if (currAction == null) { return; }
                using (FrmAGVCommod frmSet = new FrmAGVCommod(1, currAction.CmdCode))
                {
                    frmSet.agvNewAction = currAction;
                    if (frmSet.ShowDialog() == DialogResult.OK)
                    {
                        currAction = frmSet.mid_obj as CmdInfo;
                        bsActions.ResetBindings(false);
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void gvAction_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {

        }

        private void gvAction_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

        }
    }//end
}
