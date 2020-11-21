using AGVDAccess;
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
    public partial class FrmIOActionSet : BaseForm
    {
        IOActionInfo ActionInfo = null;
        bool IsView = false;
        public FrmIOActionSet(IOActionInfo actionInfo, bool isView)
        {
            InitializeComponent();
            ActionInfo = actionInfo;
            IsView = isView;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void FrmActionDetail_Shown(object sender, EventArgs e)
        {
            try
            {
                this.bsIOActionSet.DataSource = ActionInfo;
                this.bsIOActionSet.ResetBindings(false);
                if (IsView)
                {
                    txtActionID.ReadOnly = true;
                    txtActionName.ReadOnly = true;
                    txtDeviceID.ReadOnly = true;
                    txtTerminalID.ReadOnly = true;
                    cbxTerminalType.ReadOnly = true;
                    cbxIOUseState.ReadOnly = true;
                    txtTerminalData.ReadOnly = true;
                    cbxIsPass.ReadOnly = true;
                    txtWaitTime.ReadOnly = true;
                    cbxIsWait.ReadOnly = true;
                    btnClear.Enabled = false;
                    btnSave.Enabled = false;
                }
                else
                {
                    txtActionID.ReadOnly = false;
                    txtActionName.ReadOnly = false;
                    txtDeviceID.ReadOnly = false;
                    txtTerminalID.ReadOnly = false;
                    cbxTerminalType.ReadOnly = false;
                    cbxIOUseState.ReadOnly = false;
                    txtTerminalData.ReadOnly = false;
                    cbxIsPass.ReadOnly = false;
                    txtWaitTime.ReadOnly = false;
                    cbxIsWait.ReadOnly = false;
                    btnClear.Enabled = true;
                    btnSave.Enabled = true;
                }
                this.bsIODevices.DataSource = AGVClientDAccess.LoadIODeviceInfo();
                bsIODevices.ResetBindings(false);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                labelControl1.Focus();
                if (!Valid()) { return; }
                IOActionInfo CurrentActionInfo = this.bsIOActionSet.Current as IOActionInfo;
                OperateReturnInfo opr = AGVClientDAccess.SaveIOAction(CurrentActionInfo);
                MsgBox.Show(opr);
                if (opr.ReturnCode == OperateCodeEnum.Success)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
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

        private void btnClear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                IOActionInfo CurrentAction = bsIOActionSet.Current as IOActionInfo;
                if (CurrentAction != null)
                {
                    int actionID = CurrentAction.ActionID;
                    IOActionInfo newItem = new IOActionInfo();
                    newItem.ActionID = actionID;
                    this.bsIOActionSet.DataSource = newItem;
                    this.bsIOActionSet.ResetBindings(false);
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }


        /// <summary>
        /// 验证数据有效性
        /// </summary>
        /// <returns></returns>
        private bool Valid()
        {
            try
            {
                IOActionInfo CurrentActionInfo = this.bsIOActionSet.Current as IOActionInfo;
                if (CurrentActionInfo == null)
                { return false; }
                if (string.IsNullOrEmpty(CurrentActionInfo.ActionName))
                {
                    MsgBox.ShowWarn("请维护IO动作名称!");
                    return false;
                }

                if (cbxIsPass.SelectedIndex != 0 || cbxTerminalType.SelectedIndex != 0)
                {
                    if (cbxTerminalType.SelectedIndex != 0 && CurrentActionInfo.DeviceID == -1)
                    {
                        MsgBox.ShowWarn("请维护IO设备编号");
                        return false;
                    }
                    if (cbxIsPass.SelectedIndex != 0 && cbxTerminalType.SelectedIndex == 0)
                    {
                        MsgBox.ShowWarn("请维护IO控制类型");
                        return false;
                    }

                    if (CurrentActionInfo.TerminalID == -1)
                    {
                        MsgBox.ShowWarn("请维护控制字节索引!");
                        return false;
                    }
                    if (string.IsNullOrEmpty(CurrentActionInfo.TerminalData.Trim()))
                    {
                        MsgBox.ShowWarn("请维护控制数据!");
                        return false;
                    }
                }
                if (cbxIsWait.SelectedIndex == 0 && cbxTerminalType.SelectedIndex == 0)
                {
                    MsgBox.ShowWarn("请维护具体控制信息");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); return false; }
        }

        private void cbxTerminalType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cbxIsWait.SelectedIndexChanged -= cbxIsWait_SelectedIndexChanged;
                if (cbxTerminalType.SelectedIndex != 0)
                {
                    cbxIsWait.SelectedIndex = 0;
                    cbxIsWait.ReadOnly = true;
                    txtWaitTime.ReadOnly = true;
                }
                else
                {
                    cbxIsWait.ReadOnly = false;
                    txtWaitTime.ReadOnly = false;
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
            finally
            { cbxIsWait.SelectedIndexChanged += cbxIsWait_SelectedIndexChanged; }
        }

        private void labelControl3_Click(object sender, EventArgs e)
        {

        }

        private void labelControl7_Click(object sender, EventArgs e)
        {

        }

        private void cbxIsPass_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbxIsWait_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                cbxIsWait.SelectedIndexChanged -= cbxIsWait_SelectedIndexChanged;
                cbxTerminalType.SelectedIndexChanged -= cbxTerminalType_SelectedIndexChanged;
                txtDeviceID.EditValueChanged -= txtDeviceID_EditValueChanged;
                if (cbxIsWait.SelectedIndex ==1)
                {
                    cbxTerminalType.SelectedIndex = 0;
                    cbxTerminalType.ReadOnly = true;
                    cbxIOUseState.ReadOnly = true;
                    txtTerminalID.ReadOnly = true;
                    cbxIsPass.ReadOnly = true;
                    txtDeviceID.ReadOnly = true;
                    txtTerminalData.ReadOnly = true;
                    IOActionInfo CurrentActionInfo = this.bsIOActionSet.Current as IOActionInfo;
                    if (CurrentActionInfo != null)
                    {
                        CurrentActionInfo.IsWaitStr = cbxIsWait.EditValue.ToString();
                        CurrentActionInfo.DeviceID = -1;
                        CurrentActionInfo.IOUseState = 0;
                        CurrentActionInfo.DeviceName = "";
                        bsIOActionSet.ResetBindings(true);
                    }
                }
                else
                {
                    cbxTerminalType.ReadOnly = false;
                    cbxIOUseState.ReadOnly = false;
                    txtTerminalID.ReadOnly = false;
                    cbxIsPass.ReadOnly = false;
                    txtTerminalData.ReadOnly = false;
                    txtDeviceID.ReadOnly = false;
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
            finally
            {
                cbxTerminalType.SelectedIndexChanged += cbxTerminalType_SelectedIndexChanged;
                cbxIsWait.SelectedIndexChanged += cbxIsWait_SelectedIndexChanged;
                txtDeviceID.EditValueChanged += txtDeviceID_EditValueChanged;
            }
        }

        private void txtDeviceID_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                txtDeviceID.EditValueChanged -= txtDeviceID_EditValueChanged;
                IOActionInfo CurrentActionInfo = this.bsIOActionSet.Current as IOActionInfo;
                if (CurrentActionInfo != null&&!string.IsNullOrEmpty(((DevExpress.XtraEditors.LookUpEdit)sender).Text))
                {
                    CurrentActionInfo.DeviceID = Convert.ToInt16(((DevExpress.XtraEditors.LookUpEdit)sender).Text);
                    IODeviceInfo CurrDevice = (bsIODevices.DataSource as IList<IODeviceInfo>).FirstOrDefault(p=>p.ID== CurrentActionInfo.DeviceID);
                    if (CurrDevice != null)
                    { CurrentActionInfo.DeviceName = CurrDevice.DeviceName; }
                    bsIOActionSet.ResetBindings(false);
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
            finally
            { txtDeviceID.EditValueChanged += txtDeviceID_EditValueChanged; }
        }
    }
}
