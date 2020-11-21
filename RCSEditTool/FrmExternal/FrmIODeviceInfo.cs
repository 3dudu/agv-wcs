using AGVDAccess;
using Model.Comoon;
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
    public partial class FrmIODeviceInfo : BaseForm
    {
        public FrmIODeviceInfo()
        {
            InitializeComponent();
        }

        private void FrmIODeviceInfo_Shown(object sender, EventArgs e)
        {
            try
            {
                this.CenterToParent();
                bsDetail.DataSource = AGVDAccess.AGVClientDAccess.LoadIODeviceInfo();
                bsDetail.ResetBindings(false);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvDetail.CloseEditor();
                IODeviceInfo CurrentIOInfo = this.bsDetail.Current as IODeviceInfo;
                if (bsDetail.Count <= 0 || (CurrentIOInfo != null && Valid(CurrentIOInfo)))
                {
                    IODeviceInfo newItem = new IODeviceInfo();
                    newItem.ID = bsDetail.Count + 1;
                    bsDetail.Add(newItem);
                    bsDetail.ResetBindings(false);
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        /// <summary>
        /// 删行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDele_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                IODeviceInfo CurrentIOInfo = this.bsDetail.Current as IODeviceInfo;
                if (CurrentIOInfo != null)
                {
                    if (MsgBox.ShowQuestion("是否确认删除当前行") != DialogResult.No)
                    {
                        OperateReturnInfo opr = AGVClientDAccess.Delete_IODeviceInfo(CurrentIOInfo);
                        if (opr.ReturnCode == OperateCodeEnum.Success)
                        {
                            bsDetail.DataSource = AGVDAccess.AGVClientDAccess.LoadIODeviceInfo();
                            bsDetail.ResetBindings(false);
                        }
                        else
                        { MsgBox.Show(opr); }
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvDetail.CloseEditor();
                IODeviceInfo CurrentDevice = bsDetail.Current as IODeviceInfo;
                if (Valid(CurrentDevice))
                {
                    IList<IODeviceInfo> AllDevices = this.bsDetail.List as IList<IODeviceInfo>;
                    if (AllDevices != null && AllDevices.Count > 0)
                    {
                        OperateReturnInfo opr = AGVDAccess.AGVClientDAccess.SaveIO(AllDevices);
                        MsgBox.Show(opr);
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 验证行信息有效性
        /// </summary>
        private bool Valid(IODeviceInfo DeviceInfo)
        {
            try
            {
                if (DeviceInfo == null) { return false; }
                if (string.IsNullOrEmpty(DeviceInfo.DeviceName))
                {
                    MsgBox.ShowWarn("请维护IO设备名称!");
                    return false;
                }
                if (string.IsNullOrEmpty(DeviceInfo.IP))
                {
                    MsgBox.ShowWarn("请维护IO设备IP地址!");
                    return false;
                }
                if (string.IsNullOrEmpty(DeviceInfo.Port))
                {
                    MsgBox.ShowWarn("请维护IO设备端口号!");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
                return false;
            }
        }
    }//end Form
}
