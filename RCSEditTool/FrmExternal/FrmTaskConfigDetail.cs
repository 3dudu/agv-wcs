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
using Tools;

namespace RCSEditTool.FrmExternal
{
    public partial class FrmTaskConfigDetail : BaseForm
    {
        IList<TaskConfigInfo> AllTaskConfigs = new List<TaskConfigInfo>();
        TaskConfigInfo NewTaskConfigs = new TaskConfigInfo();
        public FrmTaskConfigDetail(IList<TaskConfigInfo> AllTaskConfig, TaskConfigInfo NewTaskConfig)
        {
            InitializeComponent();
            if (AllTaskConfigs != null)
            { AllTaskConfigs = AllTaskConfig; }
            if (NewTaskConfigs != null)
            { NewTaskConfigs = NewTaskConfig; }
        }

        private void FrmTaskConfigDetail_Shown(object sender, EventArgs e)
        {
            if (NewTaskConfigs.IsNew)
            {
                this.txtTaskConditonCode.ReadOnly = false;
            }
            else
            {
                this.txtTaskConditonCode.ReadOnly = true;
            }
            bsConfig.DataSource = NewTaskConfigs;
            this.bsDetail.DataSource = AGVDAccess.AGVClientDAccess.LoadTaskConfigDetails(NewTaskConfigs);
            IList<AreaInfo> allAreas = AGVDAccess.AGVClientDAccess.LoadAllArea();
            AreaInfo _default = new AreaInfo() { OwnArea = -1, AreaName = "呼叫储位" };
            allAreas.Insert(0, _default);
            this.bsArea.DataSource = allAreas;
            this.bsMaterel.DataSource = AGVDAccess.AGVClientDAccess.LoadAllMaterial();
            bsDetail.ResetBindings(false);
            bsArea.ResetBindings(false);
            bsMaterel.ResetBindings(false);
            gvDetail_FocusedRowChanged(null, null);
        }

        private bool Valid()
        {
            try
            {
                gvDetail.CloseEditor();
                gvDetail.CloseEditForm();
                if (gvDetail.FocusedRowHandle < 0)
                {
                    return true;
                }
                if (string.IsNullOrEmpty(txtTaskConditonName.Text))
                {
                    MsgBox.ShowWarn("请维护任务条件名称!");
                    return false;
                }
                TaskConfigDetail Curr = bsDetail[gvDetail.FocusedRowHandle] as TaskConfigDetail;
                if (Curr == null) { return false; }
                if (string.IsNullOrEmpty(Curr.ArmOwnArea.ToString()) || string.IsNullOrEmpty(Curr.IsWaitPassUI) || string.IsNullOrEmpty(Curr.MaterialType.ToString()) || string.IsNullOrEmpty(Curr.IsWaitPassUI.ToString()))
                {
                    MsgBox.ShowWarn("请维护任务明细配置!");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); return false; }
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtTaskConditonName.Text) || string.IsNullOrEmpty(txtTaskConditonName.Text))
                {
                    MsgBox.ShowWarn("请维护任务条件编号或名称!");
                    return;
                }
                gvDetail.BeforeLeaveRow -= gvDetail_BeforeLeaveRow;
                labelControl1.Focus();
                gvDetail.CloseEditor();
                labelControl1.Focus();
                if (Valid())
                {
                    TaskConfigDetail newItem = new TaskConfigDetail();
                    newItem.TaskConditonCode = txtTaskConditonCode.Text;
                    IList<TaskConfigDetail> AllTaskConfigDetail = bsDetail.List as IList<TaskConfigDetail>;
                    int DetailID = 1;
                    if (AllTaskConfigDetail != null && AllTaskConfigDetail.Count > 0)
                    { DetailID = AllTaskConfigDetail.Max(p => p.DetailID) + 1; }
                    newItem.DetailID = DetailID;
                    bsDetail.Add(newItem);
                    bsDetail.ResetBindings(false);
                    bsDetail.MoveLast();
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
                if (bsDetail.Count <= 0) { return; }
                bsDetail.RemoveCurrent();
                bsDetail.ResetBindings(false);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
            finally
            { gvDetail.BeforeLeaveRow += gvDetail_BeforeLeaveRow; }
        }

        private void btnAddb_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //TaskConfigDetail NewTaskConfig = bsDetail.Current as TaskConfigDetail;
                //if (NewTaskConfig != null)
                //{
                //    using (FrmAddTaskConfigMustPass frm = new FrmAddTaskConfigMustPass(NewTaskConfig))
                //    {
                //        frm.ShowDialog();
                //        if (frm.DialogResult == DialogResult.OK)
                //        {
                //            gvDetail_FocusedRowChanged(null, null);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }


        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvPass.BeforeLeaveRow -= gvPass_BeforeLeaveRow;
                gvDetail.CloseEditor();
                labelControl1.Focus();
                if (!Valid()) { return; }
                if (gvPass.FocusedRowHandle >= 0)
                {
                    TaskConfigMustPass CurrConfigPass = baPass[gvPass.FocusedRowHandle] as TaskConfigMustPass;
                    if (CurrConfigPass != null)
                    {
                        if (string.IsNullOrEmpty(CurrConfigPass.MustPassLandCode))
                        { baPass.RemoveCurrent(); }
                        IList<IOActionInfo> IOActions = bsIOActions.List as List<IOActionInfo>;
                        if (IOActions != null)
                        { CurrConfigPass.MustPassIOAction = DataToObject.CreateDeepCopy<IList<IOActionInfo>>(IOActions); }
                    }
                }
                if (gvDetail.FocusedRowHandle >= 0)
                {
                    TaskConfigDetail CurrDetail = bsDetail[gvDetail.FocusedRowHandle] as TaskConfigDetail;
                    if (CurrDetail != null)
                    {
                        IList<TaskConfigMustPass> ConfigMustPasss = baPass.List as List<TaskConfigMustPass>;
                        if (ConfigMustPasss != null)
                        { CurrDetail.TaskConfigMustPass = DataToObject.CreateDeepCopy<IList< TaskConfigMustPass >>(ConfigMustPasss); }
                    }
                }
                TaskConfigInfo CurrTaskConfig = bsConfig.Current as TaskConfigInfo;
                IList<TaskConfigDetail> AllTaskConfigDetail = bsDetail.List as IList<TaskConfigDetail>;
                if (CurrTaskConfig != null && AllTaskConfigDetail != null && AllTaskConfigDetail.Count > 0)
                {
                    if (CurrTaskConfig.IsNew && AllTaskConfigs.Where(p => p.TaskConditonCode == CurrTaskConfig.TaskConditonCode).Count() > 0)
                    {
                        MsgBox.ShowWarn("任务条件配置编号已存在!");
                        return;
                    }
                    CurrTaskConfig.TaskConfigDetail = DataToObject.CreateDeepCopy< IList < TaskConfigDetail >>(AllTaskConfigDetail);
                    OperateReturnInfo opr = AGVDAccess.AGVClientDAccess.SaveTaskConfig(CurrTaskConfig);
                    MsgBox.Show(opr);
                    if (opr.ReturnCode == OperateCodeEnum.Success)
                    { this.DialogResult = DialogResult.OK; }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
            finally
            { gvPass.BeforeLeaveRow += gvPass_BeforeLeaveRow; }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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

        private void gvDetail_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {
                gvPass.BeforeLeaveRow -= gvPass_BeforeLeaveRow;
                labelControl1.Focus();
                gvDetail.CloseEditor();
                labelControl1.Focus();
                e.Allow = Valid();
                if (!e.Allow) return;
                if (gvDetail.FocusedRowHandle >= 0)
                {
                    TaskConfigDetail CurrDetail = bsDetail[gvDetail.FocusedRowHandle] as TaskConfigDetail;
                    if (CurrDetail != null)
                    {
                        if (gvPass.FocusedRowHandle >= 0)
                        {
                            TaskConfigMustPass CurrConfigPass = baPass[gvPass.FocusedRowHandle] as TaskConfigMustPass;
                            if (CurrConfigPass != null)
                            {
                                if (string.IsNullOrEmpty(CurrConfigPass.MustPassLandCode))
                                { baPass.RemoveCurrent(); }
                            }
                            IList<IOActionInfo> Actions = bsIOActions.List as IList<IOActionInfo>;
                            CurrConfigPass.MustPassIOAction = Actions;
                        }
                        IList<TaskConfigMustPass> ConfigMustPasss = baPass.List as List<TaskConfigMustPass>;
                        if (ConfigMustPasss != null)
                        { CurrDetail.TaskConfigMustPass = DataToObject.CreateDeepCopy<IList<TaskConfigMustPass>>(ConfigMustPasss); }
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
            finally
            { gvPass.BeforeLeaveRow += gvPass_BeforeLeaveRow; }
        }

        private void gvDetail_Click(object sender, EventArgs e)
        {

        }

        private void gvDetail_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                gvPass.BeforeLeaveRow -= gvPass_BeforeLeaveRow;
                TaskConfigDetail curr = bsDetail.Current as TaskConfigDetail;
                if (curr == null) { return; }
                //IList<TaskConfigMustPass> TaskPass = AGVDAccess.AGVClientDAccess.LoadMustPassByConfigDetail(curr.TaskConditonCode, curr.DetailID);
                //if (TaskPass != null)
                //{
                baPass.DataSource = curr.TaskConfigMustPass;
                baPass.ResetBindings(false);
                baPass.MoveFirst();
                //}
                txtTaskConditonCode.Focus();
                gvPass_FocusedRowChanged(null,null);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
            finally
            { gvPass.BeforeLeaveRow += gvPass_BeforeLeaveRow; }
        }

        private void gvPass_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {
                if (gvPass.FocusedRowHandle < 0) { return; }
                TaskConfigMustPass CurrConfigPass = baPass[gvPass.FocusedRowHandle] as TaskConfigMustPass;
                if (CurrConfigPass != null)
                {
                    IList<TaskConfigMustPass> AllPassLands = baPass.List as IList<TaskConfigMustPass>;
                    if (AllPassLands != null && AllPassLands.Count(p => p.MustPassLandCode == CurrConfigPass.MustPassLandCode) > 1)
                    {
                        gvPass.FocusedColumn = colMustPassLandCode;
                        gvPass.ShowEditor();
                        e.Allow = false;
                        return;
                    }
                    if (string.IsNullOrEmpty(CurrConfigPass.MustPassLandCode))
                    {
                        gvPass.FocusedColumn = colMustPassLandCode;
                        gvPass.ShowEditor();
                        e.Allow = false;
                        return;
                    }
                    IList<IOActionInfo> IOActions = bsIOActions.List as List<IOActionInfo>;
                    if (IOActions != null)
                    { CurrConfigPass.MustPassIOAction = DataToObject.CreateDeepCopy<IList<IOActionInfo>>(IOActions); }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void gvPass_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (gvPass.FocusedRowHandle < 0)
                {
                    bsIOActions.Clear();
                    bsIOActions.ResetBindings(false);
                    return;
                }
                TaskConfigMustPass CurrConfigPass = baPass[gvPass.FocusedRowHandle] as TaskConfigMustPass;
                if (CurrConfigPass != null)
                {
                    this.bsIOActions.DataSource = CurrConfigPass.MustPassIOAction;
                    this.bsIOActions.ResetBindings(false);
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void gvDetail_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //btnAddb_ItemClick(null, null);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnViewDetail_Click(object sender, EventArgs e)
        {
            try
            {
                gvPassLandToAction.CloseEditor();
                IOActionInfo CurrIOAction = bsIOActions.Current as IOActionInfo;
                if (CurrIOAction != null)
                {
                    using (FrmIOActionSet frm = new FrmIOActionSet(CurrIOAction, true))
                    {
                        frm.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnAddAction_Click(object sender, EventArgs e)
        {
            try
            {
                using (FormSelectIOAction frm = new FormSelectIOAction())
                {
                    if (frm.ShowDialog() == DialogResult.OK && frm.SelectIOActions.Count > 0)
                    {
                        if (this.bsIOActions.Count > 0 && MsgBox.ShowQuestion("是否清除现有IO动作信息!") == DialogResult.Yes)
                        {
                            bsIOActions.Clear();
                        }
                        IList<IOActionInfo> HadIOActions = bsIOActions.List as IList<IOActionInfo>;
                        foreach (IOActionInfo newitem in frm.SelectIOActions)
                        {
                            if (HadIOActions != null && HadIOActions.Count(p => p.ActionID == newitem.ActionID) <= 0)
                            {
                                bsIOActions.List.Add(newitem);
                            }
                        }
                        bsIOActions.ResetBindings(false);
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnDeleAction_Click(object sender, EventArgs e)
        {
            try
            {
                gvPassLandToAction.CloseEditor();
                IOActionInfo CurrIOAction = bsIOActions.Current as IOActionInfo;
                if (CurrIOAction != null)
                {
                    this.bsIOActions.Remove(CurrIOAction);
                    bsIOActions.ResetBindings(false);
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }


        private void btnUP_Click(object sender, EventArgs e)
        {
            try
            {
                gvPassLandToAction.CloseEditor();
                IList<IOActionInfo> ToIOActions = bsIOActions.List as IList<IOActionInfo>;
                if (ToIOActions != null && ToIOActions.Count > 0)
                {
                    int dsRowIndex = gvPassLandToAction.GetFocusedDataSourceRowIndex();
                    if (gvPassLandToAction.IsFirstRow)
                    { return; }
                    IOActionInfo Current = gvPassLandToAction.GetRow(dsRowIndex) as IOActionInfo;
                    if (Current != null)
                    {
                        ToIOActions[dsRowIndex] = ToIOActions[dsRowIndex - 1];
                        ToIOActions[dsRowIndex - 1] = Current;
                        this.bsIOActions.DataSource = ToIOActions;
                        bsIOActions.ResetBindings(false);
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            try
            {
                gvPassLandToAction.CloseEditor();
                IList<IOActionInfo> ToIOActions = bsIOActions.List as IList<IOActionInfo>;
                if (ToIOActions != null && ToIOActions.Count > 0)
                {
                    int dsRowIndex = gvPassLandToAction.GetFocusedDataSourceRowIndex();
                    if (gvPassLandToAction.IsLastRow)
                    { return; }
                    IOActionInfo Current = gvPassLandToAction.GetRow(dsRowIndex) as IOActionInfo;
                    if (Current != null)
                    {
                        ToIOActions[dsRowIndex] = ToIOActions[dsRowIndex + 1];
                        ToIOActions[dsRowIndex + 1] = Current;
                        this.bsIOActions.DataSource = ToIOActions;
                        bsIOActions.ResetBindings(false);
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnAddPassLand_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvDetail.FocusedRowHandle < 0)
                { return; }

                TaskConfigMustPass CurrConfigPass = baPass.Current as TaskConfigMustPass;
                if (CurrConfigPass != null)
                {
                    IList<TaskConfigMustPass> AllPassLands = baPass.List as IList<TaskConfigMustPass>;
                    if (AllPassLands != null && AllPassLands.Count(p => p.MustPassLandCode == CurrConfigPass.MustPassLandCode) > 1)
                    {
                        gvPass.FocusedColumn = colMustPassLandCode;
                        gvPass.ShowEditor();
                        return;
                    }
                    if (string.IsNullOrEmpty(CurrConfigPass.MustPassLandCode))
                    {
                        gvPass.FocusedColumn = colMustPassLandCode;
                        gvPass.ShowEditor();
                        return;
                    }
                }

                TaskConfigDetail CurrConfigDetail = bsDetail[gvDetail.FocusedRowHandle] as TaskConfigDetail;
                if (CurrConfigDetail != null)
                {
                    TaskConfigMustPass newitem = new TaskConfigMustPass();
                    newitem.TaskConditonCode = CurrConfigDetail.TaskConditonCode;
                    newitem.TaskConfigDetailID = CurrConfigDetail.DetailID;
                    newitem.MustPassIOAction = new List<IOActionInfo>();
                    this.baPass.Add(newitem);
                    this.baPass.ResetBindings(false);
                    this.baPass.MoveLast();
                    gvPass.FocusedColumn = colMustPassLandCode;
                    gvPass.ShowEditor();
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnDelePassLand_Click(object sender, EventArgs e)
        {
            try
            {
                gvPass.BeforeLeaveRow -= gvPass_BeforeLeaveRow;
                if (gvPass.FocusedRowHandle >= 0)
                {
                    gvPass.DeleteRow(gvPass.FocusedRowHandle);
                    gvPass.RefreshData();
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
            finally
            { gvPass.BeforeLeaveRow += gvPass_BeforeLeaveRow; }
        }
    }//endForm
}
