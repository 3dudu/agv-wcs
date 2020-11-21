using Model.MDM;
using RCSEditTool.FrmCommon;
using System;
using System.Windows.Forms;

namespace RCSEditTool.FrmSystem
{
    public partial class FrmAGVCommod : BaseForm
    {
        int CmdCode = 0;
        int FormType = 0;
        public CmdInfo agvNewAction = null;

        public FrmAGVCommod(int formType,int cmdcode)
        {
            InitializeComponent();
            FormType = formType;
            CmdCode = cmdcode;
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                labelControl1.Focus();
                CmdInfo action = bsAction.Current as CmdInfo;
                if (action == null) { return; }
                if (string.IsNullOrEmpty(action.CmdName))
                {
                    MsgBox.ShowWarn("请维护动作名称!");
                    return;
                }
                if (string.IsNullOrEmpty(action.CmdOrder))
                {
                    MsgBox.ShowWarn("请维护动作消息体!");
                    return;
                }
                mid_obj = action;
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void FrmAGVCommod_Shown(object sender, EventArgs e)
        {
            try
            {
                if (FormType == 0)
                {
                    agvNewAction = new CmdInfo();
                    agvNewAction.CmdCode = CmdCode;
                }
                //if (agvNewAction.CmdCode >= 1 && agvNewAction.CmdCode <= 8)
                //{
                //    this.txtActionCode.ReadOnly = true;
                //    this.txtActionName.ReadOnly = true;
                //    this.rtxtMessge.ReadOnly = true;
                //}
                //else
                //{
                //    this.txtActionCode.ReadOnly = false;
                //    this.txtActionName.ReadOnly = false;
                //    this.rtxtMessge.ReadOnly = false;
                //}
                this.bsAction.DataSource = agvNewAction;
                bsAction.ResetBindings(false);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }
    }//end
}
