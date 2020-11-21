using Model.Comoon;
using Model.MDM;
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
    public partial class FrmChargeInfo : BaseForm
    {
        public FrmChargeInfo()
        {
            InitializeComponent();

        }


        /// <summary>
        /// 加载界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmChargeInfo_Shown(object sender, EventArgs e)
        {
            try
            {
                //绑定数据源
                bsCharge.DataSource = AGVDAccess.AGVClientDAccess.LoadChargeInfo();
                bsCharge.ResetBindings(false);
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }


        /// <summary>
        /// 新增按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //移除行离开事件
                gvCharge.BeforeLeaveRow -= gvCharge_BeforeLeaveRow;
                //校检数据
                if (verify())
                {
                    //自动赋予新ID值
                    int ID = 0;
                    if (bsCharge.Count > 0)
                    {
                        ID = bsCharge.Count + 1;
                    }
                    //添加新行
                    ChargeStationInfo addRow = new ChargeStationInfo();
                    addRow.ID = ID;
                    bsCharge.Add(addRow);
                    bsCharge.ResetBindings(false);
                    bsCharge.MoveLast();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
            finally
            {
                gvCharge.BeforeLeaveRow += gvCharge_BeforeLeaveRow;
            }

        }


        /// <summary>
        /// 删除按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (bsCharge.Current != null)
                {
                    //删除选中行
                    gvCharge.CloseEditor();
                    bsCharge.Remove(bsCharge.Current);
                    bsCharge.ResetBindings(false);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }


        /// <summary>
        /// 保存按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvCharge.CloseEditor();
                if (verify())
                {
                    IList<ChargeStationInfo> Data = bsCharge.List as IList<ChargeStationInfo>;
                    if (Data.Count > 0)
                    {
                        OperateReturnInfo opr = AGVDAccess.AGVClientDAccess.SaveChargeInfo(Data);
                        MsgBox.Show(opr);
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }


        /// <summary>
        /// 退出按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try {
                this.Close();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }


        /// <summary>
        /// 校验数据
        /// </summary>
        /// <returns></returns>
        public bool verify()
        {
            try
            {
                gvCharge.CloseEditor();
                ChargeStationInfo Current = bsCharge.Current as ChargeStationInfo;
                if (Current != null)
                {
                    if (string.IsNullOrEmpty(Current.IP))
                    {
                        MsgBox.ShowError("请先维护IP");
                        return false;
                    }
                    if (string.IsNullOrEmpty(Current.Port))
                    {
                        MsgBox.ShowError("请先维护端口");
                        return false;
                    }
                    if (string.IsNullOrEmpty(Current.ChargeLandCode))
                    {
                        MsgBox.ShowError("请先维护充电桩地标");
                        return false;
                    }
                    IList<ChargeStationInfo> Data = bsCharge.List as IList<ChargeStationInfo>;
                    if (Data.Where(p => p.ID == Current.ID).Count() > 1)
                    {
                        MsgBox.ShowError("当前充电桩编号已占用");
                        return false;
                    }
                    if (Data.Where(p => p.IP == Current.IP).Count() > 1)
                    {
                        MsgBox.ShowError("当前充电桩IP已占用");
                        return false;
                    }
                    if (Data.Where(p => p.ChargeLandCode == Current.ChargeLandCode).Count() > 1)
                    {
                        MsgBox.ShowError("当前充电桩对照地标已占用");
                        return false;
                    }
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
                return false;
            }
        }


        /// <summary>
        /// 行移动前事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvCharge_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {
                e.Allow = verify();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }
    }
}
