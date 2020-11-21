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
    public partial class FrmTaskConfigInfo : BaseForm
    {
        public FrmTaskConfigInfo()
        {
            InitializeComponent();
        }

        private void FrmConfigInfo_Shown(object sender, EventArgs e)
        {
            try
            {
                bsDetail.DataSource = AGVDAccess.AGVClientDAccess.LoadAllTaskConfig();
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
                IList<TaskConfigInfo> AllTaskConfig = bsDetail.List as IList<TaskConfigInfo>;
                TaskConfigInfo NewTaskConfig = new TaskConfigInfo();
                NewTaskConfig.IsNew = true;
                using (FrmTaskConfigDetail frm = new FrmTaskConfigDetail(AllTaskConfig, NewTaskConfig))
                {
                    frm.ShowDialog();
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        bsDetail.DataSource = AGVDAccess.AGVClientDAccess.LoadAllTaskConfig();
                        bsDetail.ResetBindings(false);
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
                IList<TaskConfigInfo> AllTaskConfig = bsDetail.List as IList<TaskConfigInfo>;
                TaskConfigInfo NewTaskConfig = bsDetail.Current as TaskConfigInfo;
                if (NewTaskConfig != null)
                {
                    NewTaskConfig.IsNew = false;
                    NewTaskConfig.TaskConfigDetail = AGVDAccess.AGVClientDAccess.LoadTaskConfigDetails(NewTaskConfig);
                    using (FrmTaskConfigDetail frm = new FrmTaskConfigDetail(AllTaskConfig, NewTaskConfig))
                    {
                        frm.ShowDialog();
                        if (frm.DialogResult == DialogResult.OK)
                        {
                            bsDetail.DataSource = AGVDAccess.AGVClientDAccess.LoadAllTaskConfig();
                            bsDetail.ResetBindings(false);
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
                TaskConfigInfo NewTaskConfig = bsDetail.Current as TaskConfigInfo;
                if (NewTaskConfig != null)
                {
                    if (MsgBox.ShowQuestion("确定删除当前项?") == DialogResult.Yes)
                    {
                        OperateReturnInfo opr = AGVDAccess.AGVClientDAccess.DeleTaskConfig(NewTaskConfig);
                        MsgBox.Show(opr);
                        if (opr.ReturnCode == OperateCodeEnum.Success)
                        {
                            bsDetail.DataSource = AGVDAccess.AGVClientDAccess.LoadAllTaskConfig();
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
