using DXAGVClient.FrmExternal;
using Model.Comoon;
using Model.MSM;
using RCSEditTool;
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

namespace DXAGVClient
{
    public partial class FromDetailQuerySet : BaseForm
    {
        public FromDetailQuerySet()
        {
            InitializeComponent();
        }

        private string FocusGV = "gvDetail";

        private void FromDetailQuerySet_Shown(object sender, EventArgs e)
        {
            try
            {
                this.bsDetailQuery.DataSource = AGVDAccess.AGVClientDAccess.get_Querys();
                bsDetailQuery.ResetBindings(false);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void gvDetail_BeforeLeaveRow(object sender, DevExpress.XtraGrid.Views.Base.RowAllowEventArgs e)
        {
            try
            {
                e.Allow = Valid();
                DetailQueryInfo CurrQueryInfo = bsDetailQuery[e.RowHandle] as DetailQueryInfo;
                if (CurrQueryInfo == null) return;
                CurrQueryInfo.Condition = bsCondition.List as IList<DetailCondition>;
                CurrQueryInfo.sqlStr = rbtSQL.Text;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void gvDetail_Click(object sender, EventArgs e)
        {
            FocusGV = "gvDetail";
        }

        private void gvDetail_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                DetailQueryInfo Curr_Detail = bsDetailQuery.Current as DetailQueryInfo;
                if (Curr_Detail == null) { return; }
                bsCondition.DataSource = Curr_Detail.Condition;
                bsCondition.ResetBindings(false);
                this.rbtSQL.Text = Curr_Detail.sqlStr;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void gvConditions_Click(object sender, EventArgs e)
        {
            FocusGV = "gvConditions";
        }
        private void btnAddRow_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvDetail.CloseEditor();
                gvConditions.CloseEditor();
                if (FocusGV == "gvDetail")
                {
                    DetailQueryInfo new_Query = new DetailQueryInfo();
                    new_Query.QueryCode = (bsDetailQuery.Count + 1).ToString().PadLeft(4, '0');
                    bsDetailQuery.Add(new_Query);
                    bsDetailQuery.ResetBindings(false);
                    bsDetailQuery.MoveLast();
                    //}
                }
                else if (FocusGV == "gvConditions")
                {
                    IList<DetailCondition> has_conditon = this.bsCondition.List as IList<DetailCondition>;
                    if (has_conditon == null) { has_conditon = new List<DetailCondition>(); }
                    using (FrmConditonSet frmSet = new FrmConditonSet(has_conditon))
                    {
                        if (frmSet.ShowDialog() == DialogResult.OK)
                        {
                            if (frmSet.mid_obj != null)
                            {
                                int count = has_conditon.Where(P => P.ConditionCode.Equals((frmSet.mid_obj as DetailCondition).ConditionCode)).Count();
                                if (count > 0)
                                {
                                    MsgBox.ShowWarn("该条件已存在!");
                                    return;
                                }
                                bsCondition.Add(frmSet.mid_obj);
                            }
                        }
                    }
                    bsCondition.ResetBindings(false);
                    bsCondition.MoveLast();
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvDetail.CloseEditor();
                gvConditions.CloseEditor();
                gvDetail.BeforeLeaveRow -= gvDetail_BeforeLeaveRow;
                if (FocusGV == "gvDetail")
                {
                    DetailQueryInfo Curr_Detail = bsDetailQuery.Current as DetailQueryInfo;
                    if (Curr_Detail == null) { return; }
                    bsDetailQuery.RemoveCurrent();
                    bsDetailQuery.ResetBindings(false);
                }
                else if (FocusGV == "gvConditions")
                {
                    DetailCondition Curr_Condition = bsCondition.Current as DetailCondition;
                    if (Curr_Condition == null) { return; }
                    bsCondition.RemoveCurrent();
                    bsCondition.ResetBindings(false);
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
            finally
            { gvDetail.BeforeLeaveRow += gvDetail_BeforeLeaveRow; }
        }

        private void btnModifi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvDetail.CloseEditor();
                gvConditions.CloseEditor();
                DetailQueryInfo Curr_Detail = bsDetailQuery.Current as DetailQueryInfo;
                if (Curr_Detail == null) { return; }
                IList<DetailCondition> has_conditon = this.bsCondition.List as IList<DetailCondition>;
                if (has_conditon == null) { has_conditon = new List<DetailCondition>(); }
                Curr_Detail.Condition = has_conditon;
                using (FrmDetailSqlSet frmSet = new FrmDetailSqlSet(Curr_Detail))
                {
                    frmSet.rtbSQL.Text = rbtSQL.Text;
                    if (frmSet.ShowDialog() == DialogResult.OK)
                    {
                        this.bsDetailQuery.DataSource = AGVDAccess.AGVClientDAccess.get_Querys();
                        bsDetailQuery.ResetBindings(false);
                        gvDetail_FocusedRowChanged(null, null);
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }
        //public static OperateReturnInfo SaveDetailQuery(IList<DetailQueryInfo> Querys)
        //{
        //    try
        //    {
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            Hashtable hs = new Hashtable();
        //            OperateReturnInfo opr;
        //            foreach (DetailQueryInfo item in Querys)
        //            {
        //                opr = SaveDetailQuery(item);
        //                if (opr.ReturnCode == OperateCodeEnum.Failed)
        //                { return opr;
        //                }
        //            }
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gvDetail.CloseEditor();
                gvConditions.CloseEditor();
                IList<DetailQueryInfo> Querys = bsDetailQuery.List as IList<DetailQueryInfo>;
                if (Querys == null) { return; }
                if (Querys.Count <= 0) { return; }
                this.rbtSQL.Focus();
                gvDetail.Focus();
                if (!Valid()) { return; }
                DetailQueryInfo CurrQueryInfo = bsDetailQuery.Current as DetailQueryInfo;
                if (CurrQueryInfo == null) return;
                CurrQueryInfo.Condition = bsCondition.List as IList<DetailCondition>;
                CurrQueryInfo.sqlStr = rbtSQL.Text;
                OperateReturnInfo opr = AGVDAccess.AGVClientDAccess.SaveDetailQuery(Querys);
                MsgBox.Show(opr);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
            return;
        }

        private bool Valid()
        {
            try
            {
                gvDetail.CloseEditor();
                gvConditions.CloseEditor();
                if (bsDetailQuery.Count > 0)
                {
                    DetailQueryInfo CurrQueryInfo = bsDetailQuery[gvDetail.FocusedRowHandle] as DetailQueryInfo;
                    if (CurrQueryInfo == null) { return false; }
                    if (string.IsNullOrEmpty(CurrQueryInfo.QueryName))
                    {
                        MsgBox.ShowWarn("请维护报表名称!");
                        return false;
                    }
                    if (string.IsNullOrEmpty(rbtSQL.Text.Trim()))
                    {
                        MsgBox.ShowWarn("请维护报表查询语句!");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); return false; }
        }

        private void gcConditions_Click(object sender, EventArgs e)
        {

        }

        private void gcDetail_Click(object sender, EventArgs e)
        {

        }
    }
}
