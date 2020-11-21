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
    public partial class FrmDetailQueryFiled : BaseForm
    {
        public FrmDetailQueryFiled(DataTable dt_filed, DetailQueryInfo query)
        {
            InitializeComponent();
            DTFiled = dt_filed.Copy();
            Query = query;
        }

        private DataTable DTFiled = new DataTable();
        private DetailQueryInfo Query = new DetailQueryInfo();

        private void FrmDetailQueryFiled_Shown(object sender, EventArgs e)
        {
            try
            {
                this.gcFiled.DataSource = DTFiled;
                gcFiled.Refresh();
                if (Query.Fileds != null)
                {
                    bsFileds.DataSource = Query.Fileds;
                    bsFileds.ResetBindings(false);
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnOneRight_Click(object sender, EventArgs e)
        {
            try
            {
                string CurrCellValue = gvFiled.FocusedValue.ToString();
                if (!string.IsNullOrEmpty(CurrCellValue))
                {
                    int count = (bsFileds.List as IList<DetailQueryFiled>).Where(P => P.FiledCode.EndsWith(CurrCellValue)).Count();
                    if (count == 0)
                    {
                        DetailQueryFiled filedInfo = new DetailQueryFiled();
                        filedInfo.FiledCode = CurrCellValue;
                        filedInfo.FiledName = CurrCellValue;
                        this.bsFileds.Add(filedInfo);
                        bsFileds.ResetBindings(false);
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnAllRight_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataRow dr in DTFiled.Rows)
                {
                    string CurrCellValue = dr[0].ToString();
                    if (!string.IsNullOrEmpty(CurrCellValue))
                    {
                        int count = (bsFileds.List as IList<DetailQueryFiled>).Where(P => P.FiledCode.EndsWith(CurrCellValue)).Count();
                        if (count == 0)
                        {
                            DetailQueryFiled filedInfo = new DetailQueryFiled();
                            filedInfo.FiledCode = CurrCellValue;
                            filedInfo.FiledName = CurrCellValue;
                            this.bsFileds.Add(filedInfo);
                            bsFileds.ResetBindings(false);
                        }
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnOneLeft_Click(object sender, EventArgs e)
        {
            try
            {
                if (bsFileds.Count <= 0) { return; }
                DetailQueryFiled CurrField = bsFileds.Current as DetailQueryFiled;
                if (CurrField != null)
                {
                    bsFileds.RemoveCurrent();
                    bsFileds.ResetBindings(false);
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnAllLeft_Click(object sender, EventArgs e)
        {
            try
            {
                if (bsFileds.Count > 0)
                {
                    bsFileds.Clear();
                    bsFileds.ResetBindings(false);
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            try
            {
                IList<DetailQueryFiled> fields = bsFileds.List as IList<DetailQueryFiled>;
                if (fields != null)
                {
                    Query.Fileds = fields;
                    OperateReturnInfo opr = AGVDAccess.AGVClientDAccess.SaveDetailQuery(Query);
                    MsgBox.Show(opr);
                    if (opr.ReturnCode == OperateCodeEnum.Success)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            return;
        }

        private void FrmDetailQueryFiled_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                IList<DetailQueryFiled> fields = bsFileds.List as IList<DetailQueryFiled>;
                if (fields == null) { fields = new List<DetailQueryFiled>(); }
                this.mid_obj = fields;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }
    }
}
