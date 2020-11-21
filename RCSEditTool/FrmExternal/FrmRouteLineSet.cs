using System;
using System.Windows.Forms;
using RCSEditTool.FrmCommon;
using Model.MDM;
using Model.MSM;
using Model.Comoon;

namespace RCSEditTool.FrmExternal
{
    public partial class FrmRouteLineSet : BaseForm
    {
        /// <summary>
        /// 线路线段设置
        /// </summary>
        public FrmRouteLineSet()
        {
            InitializeComponent();
        }

        private void btnNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RouteFragmentConfigInfo frag = bsRouteLine.Current as RouteFragmentConfigInfo;
            using (FrmFragmentSet frm = new FrmFragmentSet("", 0))

            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    this.bsRouteLine.DataSource = AGVDAccess.AGVClientDAccess.get_RouteFragmentByFragment();
                    bsRouteLine.ResetBindings(false);
                    bsRouteLine.MoveLast();
                }
            }
        }

        private void btnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvFragment.CloseEditor();
                if (bsRouteLine.Count <= 0) { return; }
                if (MsgBox.ShowQuestion("确认删除当前行吗？")==DialogResult.Yes)
                {
                     RouteFragmentConfigInfo frag = bsRouteLine.Current as RouteFragmentConfigInfo;
                    if (frag!=null)
                    {
                        OperateReturnInfo opr = AGVDAccess.AGVClientDAccess.del_RouteFragment(frag);
                        if (opr.ReturnCode==OperateCodeEnum.Success)
                        {
                            this.bsRouteLine.DataSource = AGVDAccess.AGVClientDAccess.get_RouteFragmentByFragment();
                            bsRouteLine.ResetBindings(false);
                        }
                        MsgBox.Show(opr);
                    }

                }
            }
            catch (Exception ex) { MsgBox.ShowError(ex.Message); }
        }

        private void FrmRouteLineSet_Shown(object sender, EventArgs e)
        {
            try
            {
                CenterToParent();
                this.bsRouteLine.DataSource = AGVDAccess.AGVClientDAccess.get_RouteFragmentByFragment();
                bsRouteLine.ResetBindings(false);
            }
            catch (Exception ex) { MsgBox.ShowError(ex.Message); }
        }

        private void ttnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvFragment.CloseEditor();
                this.bsRouteLine.DataSource = AGVDAccess.AGVClientDAccess.get_RouteFragmentByFragment();
                bsRouteLine.ResetBindings(false);
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }

        }

        private void gvFragment_DoubleClick(object sender, EventArgs e)
        {
            RouteFragmentConfigInfo frag = bsRouteLine.Current as RouteFragmentConfigInfo;
            if (frag==null) { return; }
            using (FrmFragmentSet frm=new FrmFragmentSet(frag.Fragment,1))
            {
                if (frm.ShowDialog()==DialogResult.OK)
                {
                    this.bsRouteLine.DataSource = AGVDAccess.AGVClientDAccess.get_RouteFragmentByFragment();
                    bsRouteLine.ResetBindings(false);
                    
                }
            }
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
    }
}