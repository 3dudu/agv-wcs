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
    public partial class FrmCallBoxSet : BaseForm
    {
        public FrmCallBoxSet()
        {
            InitializeComponent();
        }

        private void FrmCallBoxSet_Shown(object sender, EventArgs e)
        {
            try
            {
                bsDetail.DataSource = AGVDAccess.AGVClientDAccess.LoadAllCallBoxs();
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
                IList<CallBoxInfo> AllCallBox = bsDetail.List as IList<CallBoxInfo>;
                int NewBoxID = 1;
                if (AllCallBox != null && AllCallBox.Count > 0)
                { NewBoxID = AllCallBox.Max(p => p.CallBoxID) + 1; }
                CallBoxInfo NewBoxInfo = new CallBoxInfo();
                NewBoxInfo.CallBoxID = NewBoxID;
                NewBoxInfo.IsNew = true;
                using (FrmCallBoxDetail frm = new FrmCallBoxDetail(AllCallBox, NewBoxInfo))
                {
                    frm.ShowDialog();
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        bsDetail.DataSource = AGVDAccess.AGVClientDAccess.LoadAllCallBoxs();
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
                IList<CallBoxInfo> AllCallBox = bsDetail.List as IList<CallBoxInfo>;
                CallBoxInfo NewBoxInfo = bsDetail.Current as CallBoxInfo;
                if (NewBoxInfo != null)
                {
                    NewBoxInfo.IsNew = false;
                    NewBoxInfo.CallBoxDetails = AGVDAccess.AGVClientDAccess.LoadCallBoxDetails(NewBoxInfo);
                    using (FrmCallBoxDetail frm = new FrmCallBoxDetail(AllCallBox, NewBoxInfo))
                    {
                        frm.ShowDialog();
                        if (frm.DialogResult == DialogResult.OK)
                        {
                            bsDetail.DataSource = AGVDAccess.AGVClientDAccess.LoadAllCallBoxs();
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
                CallBoxInfo NewBoxInfo = bsDetail.Current as CallBoxInfo;
                if (NewBoxInfo != null)
                {
                    if (MsgBox.ShowQuestion("确定删除当前项?") == DialogResult.Yes)
                    {
                        OperateReturnInfo opr = AGVDAccess.AGVClientDAccess.DeleCallBox(NewBoxInfo);
                        MsgBox.Show(opr);
                        if (opr.ReturnCode == OperateCodeEnum.Success)
                        {
                            bsDetail.DataSource = AGVDAccess.AGVClientDAccess.LoadAllCallBoxs();
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
