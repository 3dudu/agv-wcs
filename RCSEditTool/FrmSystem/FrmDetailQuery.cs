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
    public partial class FrmDetailQuery : BaseForm
    {
        public FrmDetailQuery()
        {
            InitializeComponent();
        }

        public FrmDetailQuery(bool isview) : this()
        {
            IsView = isview;
        }

        public FrmDetailQuery(string Query_Code) : this()
        {
            QueryCode = Query_Code;
        }

        private bool IsView;
        private string QueryCode = "";
        private DetailQueryInfo QueryInfo;

        private void FrmDetailQuery_Shown(object sender, EventArgs e)
        {
            try
            {
                if (IsView)
                {
                    this.btnQuery.Enabled = false;
                    this.btnExport.Enabled = false;
                    return;
                }
                QueryInfo = AGVDAccess.AGVClientDAccess.get_QueryByCode(QueryCode);
                if (QueryInfo == null)
                {
                    MsgBox.ShowWarn("报表" + QueryCode + "不完整!");
                    this.btnQuery.Enabled = false;
                    this.btnExport.Enabled = false;
                    return;
                }
                foreach (DetailCondition item in QueryInfo.Condition)
                {
                    Label lbl1 = new Label();
                    lbl1.Text = item.ConditionName + ":";
                    lbl1.AutoSize = true;
                    lbl1.Location = new Point(item.X, item.Y);
                    pnlCondition.Controls.Add(lbl1);
                    switch (item.control_type)
                    {
                        case 0:
                            TextBox txt1 = new TextBox();
                            txt1.Name = item.ConditionCode;
                            txt1.Location = new Point(item.X + lbl1.Width, item.Y - 3);
                            pnlCondition.Controls.Add(txt1);
                            break;
                        case 1:
                            DateTimePicker dtp1 = new DateTimePicker();
                            dtp1.Name = item.ConditionCode;
                            dtp1.Location = new Point(item.X + lbl1.Width, item.Y - 3);
                            pnlCondition.Controls.Add(dtp1);
                            break;
                        case 2:
                            ComboBox cbx1 = new ComboBox();
                            cbx1.DropDownStyle = ComboBoxStyle.DropDownList;
                            cbx1.Name = item.ConditionCode;
                            cbx1.Location = new Point(item.X + lbl1.Width, item.Y - 3);
                            string[] items = item.ConditionValue.Split(';');
                            foreach (string s in items)
                            {
                                if (!string.IsNullOrEmpty(s))
                                    cbx1.Items.Add(s);
                            }
                            pnlCondition.Controls.Add(cbx1);
                            break;
                        default: break;
                    }
                }
                int max_height = QueryInfo.Condition.Max(P => P.Y) + 20;
                pnlCondition.Height = max_height + 5;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnQuery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.gvDetail.Focus();
                if (QueryInfo == null)
                {
                    MsgBox.ShowWarn("报表" + QueryCode + "不完整!");
                    return;
                }
                try
                {
                    foreach (DetailCondition item in QueryInfo.Condition)
                    {
                        switch (item.control_type)
                        {
                            case 0:
                                TextBox txt = pnlCondition.Controls.Find(item.ConditionCode, true)[0] as TextBox;
                                item.RealyValue = txt.Text.Trim();
                                break;
                            case 1:
                                DateTimePicker dtp = pnlCondition.Controls.Find(item.ConditionCode, true)[0] as DateTimePicker;
                                item.RealyValue = dtp.Value.ToString("yyyy/MM/dd");
                                break;
                            case 2:
                                ComboBox cbx = pnlCondition.Controls.Find(item.ConditionCode, true)[0] as ComboBox;
                                string[] value = item.ConditionValue.Split(';');
                                string contian = value.Where(P => P.Contains(cbx.Text.ToString())).FirstOrDefault();
                                string reaValue = contian.Split(':')[0];
                                item.RealyValue = reaValue;
                                break;
                            default: break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowError("报表条件配置有误!\r\n" + ex.Message);
                }
                DataTable res = AGVDAccess.AGVClientDAccess.ExcuteBI(QueryInfo);
                if (res == null) { return; }
                DataTable Data = ConnectConfigTool.CreateDeepCopy<DataTable>(res);
                foreach (DataColumn col in res.Columns)
                {
                    int count = QueryInfo.Fileds.Where(P => P.FiledCode.Equals(col.ColumnName)).Count();
                    if (count == 0)
                    { Data.Columns.Remove(col.ColumnName); }
                }
                this.gcDetail.DataSource = Data;
                gcDetail.Refresh();
                //实现汇总方式
                ModiFyName();
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (gvDetail.RowCount <= 0) { return; }
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "导出Excel";
                saveFileDialog.Filter = "Excel文件(*.xls)|*.xls";
                DialogResult dialogResult = saveFileDialog.ShowDialog(this);
                if (dialogResult == DialogResult.OK)
                {
                    DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                    gcDetail.ExportToXls(saveFileDialog.FileName);
                    DevExpress.XtraEditors.XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
            return;
        }


        private void ModiFyName()
        {
            try
            {
                if (QueryInfo != null && gvDetail.RowCount > 0)
                {
                    foreach (DetailQueryFiled field in QueryInfo.Fileds)
                    {
                        gvDetail.Columns[field.FiledCode].Caption = field.FiledName;
                        switch (field.SummaryType)
                        {
                            case 1:
                                gvDetail.Columns[field.FiledCode].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
                                gvDetail.Columns[field.FiledCode].SummaryItem.DisplayFormat = "共:{0:0}条";
                                break;
                            case 2:
                                gvDetail.Columns[field.FiledCode].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                                gvDetail.Columns[field.FiledCode].SummaryItem.DisplayFormat = "合计:{0:0.00}";
                                break;
                            case 3:
                                gvDetail.Columns[field.FiledCode].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Average;
                                gvDetail.Columns[field.FiledCode].SummaryItem.DisplayFormat = "平均:{0:0.00}";
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }
    }
}
