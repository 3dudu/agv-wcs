using AGVDAccess;
using Model.Comoon;
using Model.MDM;
using Model.MSM;
using RCSEditTool.FrmCommon;
using System;
using System.Collections.Generic;

namespace RCSEditTool.FrmSystem
{
    public partial class FrmAGVAchive : BaseForm
    {
        public FrmAGVAchive()
        {
            InitializeComponent();
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvDetail.CloseEditor();
                if (bsDetail.Count > 0)
                {
                    if (!Valid()) { return; }
                }
                CarBaseStateInfo newCar = new CarBaseStateInfo() { AgvID = bsDetail.Count + 1 };
                bsDetail.Add(newCar);
                bsDetail.MoveLast();
                bsDetail.ResetBindings(false);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnDele_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvDetail.BeforeLeaveRow -= gvDetail_BeforeLeaveRow;
                if (bsDetail.Current != null)
                    bsDetail.RemoveCurrent();
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
                if (Valid())
                {
                    IList<CarBaseStateInfo> cars = bsDetail.List as IList<CarBaseStateInfo>;
                    OperateReturnInfo opr = AGVDAccess.AGVClientDAccess.SaveAGVAchive(cars);
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
            catch (Exception x)
            { MsgBox.ShowError(x.Message); }
        }

        private void FrmAGVAchive_Shown(object sender, EventArgs e)
        {
            try
            {
                IList<CarBaseStateInfo> cars = AGVClientDAccess.LoadAGVAchive();
                if (cars == null) { return; }
                if (cars.Count > 0)
                {
                    this.bsDetail.DataSource = cars;
                    bsDetail.ResetBindings(false);

                    IList<AreaInfo> allAreas = AGVDAccess.AGVClientDAccess.LoadAllArea();
                    allAreas.Insert(0,new AreaInfo {  OwnArea=0,AreaName="全局"});
                    this.bsArea.DataSource = allAreas;
                    this.bsArea.ResetBindings(false);
                }
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


        private bool Valid()
        {
            try
            {
                if (gvDetail.FocusedRowHandle < 0) { return true; }
                CarBaseStateInfo currCar = bsDetail[gvDetail.FocusedRowHandle] as CarBaseStateInfo;
                if (currCar == null) { return false; }
                if (string.IsNullOrEmpty(currCar.CarName))
                {
                    MsgBox.ShowWarn("请维护车辆名称!");
                    return false;
                }
                if (string.IsNullOrEmpty(currCar.CarIP.Trim()))
                {
                    MsgBox.ShowWarn("请维护车辆IP!");
                    return false;
                }
                if (string.IsNullOrEmpty(currCar.CarPort.Trim()))
                {
                    MsgBox.ShowWarn("请维护车辆端口号!");
                    return false;
                }
                if (string.IsNullOrEmpty(currCar.StandbyLandMark.Trim()))
                {
                    MsgBox.ShowWarn("请维护车辆待命地标!");
                    return false;
                }
                //LandmarkInfo land = AGVClientDAccess.LoadLandByCode(currCar.StandbyLandMark.Trim());
                //if (land == null)
                //{
                //    MsgBox.ShowWarn("车辆待命地标不存在!");
                //    return false;
                //}
                return true;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message);return false; }
        }


    }
}
