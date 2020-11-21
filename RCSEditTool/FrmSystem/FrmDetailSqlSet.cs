using AGVDAccess;
using Model.Comoon;
using RCSEditTool;
using RCSEditTool.FrmCommon;
using SQLServerOperator;
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
    public partial class FrmDetailSqlSet : BaseForm
    {
        public FrmDetailSqlSet(DetailQueryInfo queryinfo)
        {
            InitializeComponent();
            QueryInfo = ConnectConfigTool.CreateDeepCopy<DetailQueryInfo>(queryinfo);
        }

        DetailQueryInfo QueryInfo = new DetailQueryInfo();

        private void FrmDetailSqlSet_Shown(object sender, EventArgs e)
        {
            try
            {
                this.bsCondtions.DataSource = QueryInfo.Condition;
                DetailCondition SysFuc = new DetailCondition();
                SysFuc.IsSystem = true;
                SysFuc.ConditionCode = "Empty_Null{|}";
                SysFuc.ConditionName = "该函数指对应的条件没有值\r\n则查询所有,|前面为条件字段\r\n,后面为自定义条件,例:Empty_Null{CarCode|'[CarCode]'}";
                bsCondtions.Add(SysFuc);
                bsCondtions.ResetBindings(false);
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void FrmDetailSqlSet_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void gvCondition_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                DetailCondition Curr_Condtion = bsCondtions.Current as DetailCondition;
                if (Curr_Condtion != null)
                {
                    string ConditionCode = Curr_Condtion.ConditionCode;
                    if (!string.IsNullOrEmpty(ConditionCode))
                    {
                        string s = rtbSQL.Text;
                        int idx = rtbSQL.SelectionStart;
                        string res = s.Insert(idx, Curr_Condtion.IsSystem ? ConditionCode : "[" + ConditionCode + "]");
                        rtbSQL.Text = res;
                        rtbSQL.SelectionStart = idx + s.Length;
                        rtbSQL.Focus();
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void gvCondition_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                DetailCondition Curr_Condtion = bsCondtions.Current as DetailCondition;
                if (Curr_Condtion != null)
                {
                    this.rtxtShowExam.Text = Curr_Condtion.ConditionName;
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                QueryInfo.sqlStr = this.rtbSQL.Text;
                DataTable dt = AGVDAccess.AGVClientDAccess.ExcuteBI(QueryInfo);
                if (dt == null) { return; }
                DataTable dt_filed = new DataTable();
                DataColumn clo = new DataColumn("所有字段");
                dt_filed.Columns.Add(clo);
                foreach (DataColumn col in dt.Columns)
                {
                    DataRow dr = dt_filed.NewRow();
                    dr["所有字段"] = col.ColumnName;
                    dt_filed.Rows.Add(dr.ItemArray);
                }
                using (FrmDetailQueryFiled frmfiled = new FrmDetailQueryFiled(dt_filed, QueryInfo))
                {
                    if (frmfiled.ShowDialog() == DialogResult.OK)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        return;
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                return;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }




    }
}
