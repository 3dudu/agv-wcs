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
            this.bsDetail.DataSource = AGVDAccess.AGVClientDAccess.load_TaskDetail(NewTaskConfigs.TaskConditonCode);
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
               
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }


        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvDetail.CloseEditor();
                labelControl1.Focus();
                if (!Valid()) { return; }
                TaskConfigInfo CurrTaskConfig = bsConfig.Current as TaskConfigInfo;
                IList<TaskConfigDetail> AllTaskConfigDetail = bsDetail.List as IList<TaskConfigDetail>;
                if (CurrTaskConfig != null && AllTaskConfigDetail != null && AllTaskConfigDetail.Count > 0)
                {
                    if (CurrTaskConfig.IsNew && AllTaskConfigs.Where(p => p.TaskConditonCode == CurrTaskConfig.TaskConditonCode).Count() > 0)
                    {
                        MsgBox.ShowWarn("任务条件配置编号已存在!");
                        return;
                    }
                    CurrTaskConfig.TaskConfigDetail = AllTaskConfigDetail;
                    OperateReturnInfo opr = AGVDAccess.AGVClientDAccess.SaveTaskConfig(CurrTaskConfig);
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
                labelControl1.Focus();
                gvDetail.CloseEditor();
                labelControl1.Focus();
                e.Allow = Valid();
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void gvDetail_Click(object sender, EventArgs e)
        {

        }

        private void gvDetail_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //if (string.IsNullOrEmpty(txtTaskConditonName.Text) || string.IsNullOrEmpty(txtTaskConditonName.Text))
            //{
            //    MsgBox.ShowWarn("请维护任务条件编号或名称!");
            //    return;
            //}
            TaskConfigDetail curr = bsDetail.Current as TaskConfigDetail;
            if (curr == null) { return; }
            IList<TaskConfigMustPass> TaskPass = AGVDAccess.AGVClientDAccess.LoadMustPassByConfigDetail(curr.TaskConditonCode, curr.DetailID);
            if (TaskPass != null)
            {
                baPass.DataSource = TaskPass;
                baPass.ResetBindings(false);
                baPass.MoveFirst();
            }
            txtTaskConditonCode.Focus();
        }

        private void gvPass_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {

        }

        private void gvPass_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

        }

        private void gvDetail_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                btnAddb_ItemClick(null, null);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }
    }
}
