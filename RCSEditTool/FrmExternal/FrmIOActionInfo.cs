using AGVDAccess;
using Canvas.DrawTools;
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
    public partial class FrmIOActionInfo : BaseForm
    {
        public FrmIOActionInfo()
        {
            InitializeComponent();
        }

        private void FrmActionSet_Shown(object sender, EventArgs e)
        {
            try
            {
                IList<IOActionInfo> Allaction = AGVDAccess.AGVClientDAccess.LoadAllIOAction();
                bsDetail.DataSource = Allaction;
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
                int actionID = this.bsDetail.Count + 1;
                IOActionInfo NewActionInfo = new IOActionInfo();
                NewActionInfo.ActionID = actionID;
                using (FrmIOActionSet frm = new FrmIOActionSet(NewActionInfo, false))
                {
                    frm.ShowDialog();
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        bsDetail.DataSource = AGVDAccess.AGVClientDAccess.LoadAllIOAction();
                        bsDetail.ResetBindings(false);
                    }
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
                gvDetail.FocusedColumn = colActionID;
                gvDetail.FocusedColumn = colIsWait;
                if (gvDetail.FocusedRowHandle < 0) { return true; }
                IOActionInfo Curr = bsDetail[gvDetail.FocusedRowHandle] as IOActionInfo;
                if (Curr == null) { return false; }
                if (string.IsNullOrEmpty(Curr.ActionName) || string.IsNullOrEmpty(Curr.IsWaitStr) || string.IsNullOrEmpty(Curr.WaitTime.ToString()))
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
                IOActionInfo NewActionInfo = bsDetail.Current as IOActionInfo;
                if (NewActionInfo != null)
                {
                    if (MsgBox.ShowQuestion("确定删除当前项?") == DialogResult.Yes)
                    {
                        OperateReturnInfo opr = AGVDAccess.AGVClientDAccess.DeleIOAction(NewActionInfo);
                        MsgBox.Show(opr);
                        if (opr.ReturnCode == OperateCodeEnum.Success)
                        {
                            bsDetail.DataSource = AGVDAccess.AGVClientDAccess.LoadAllIOAction();
                            bsDetail.ResetBindings(false);
                        }
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                IList<IOActionInfo> AllAction = bsDetail.List as IList<IOActionInfo>;
                IOActionInfo NewActionInfo = bsDetail.Current as IOActionInfo;
                if (NewActionInfo != null)
                {
                    NewActionInfo.IsNew = false;
                    using (FrmIOActionSet frm = new FrmIOActionSet(NewActionInfo,false))
                    {
                        frm.ShowDialog();
                        if (frm.DialogResult == DialogResult.OK)
                        {
                            bsDetail.DataSource = AGVDAccess.AGVClientDAccess.LoadAllIOAction();
                            bsDetail.ResetBindings(false);
                        }
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

        private void gvDetail_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                btnEdit_ItemClick(null, null);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }
    }
}
