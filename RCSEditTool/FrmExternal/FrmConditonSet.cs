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

namespace DXAGVClient.FrmExternal
{
    public partial class FrmConditonSet : BaseForm
    {
        public FrmConditonSet(IList<DetailCondition> has_Conditons)
        {
            InitializeComponent();
            HasConditons = ConnectConfigTool.CreateDeepCopy<IList<DetailCondition>>(has_Conditons);
        }

        IList<DetailCondition> HasConditons = new List<Model.Comoon.DetailCondition>();
        DetailCondition Curr_Condition = new DetailCondition();

        private void FrmConditonSet_Shown(object sender, EventArgs e)
        {
            try
            {
                cbxcontrol_type.SelectedIndex = 0;
                if (Curr_Condition != null && !string.IsNullOrEmpty(Curr_Condition.ConditionCode))
                {
                    txtCode.Text = Curr_Condition.ConditionCode;
                    txtName.Text = Curr_Condition.ConditionName;
                    cbxcontrol_type.SelectedIndex = Curr_Condition.control_type;
                    txtValue.Text = Curr_Condition.ConditionValue;
                    txtLeft.Text = Curr_Condition.X.ToString();
                    txtUp.Text = Curr_Condition.Y.ToString();
                    labelControl1.Focus();
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                labelControl1.Focus();
                if (!Valid()) { return; }
                using (FrmDetailQuery frm = new FrmDetailQuery(true))
                {
                    HasConditons.Add(Curr_Condition);
                    foreach (DetailCondition item in HasConditons)
                    {
                        Label lbl1 = new Label();
                        lbl1.Text = item.ConditionName + ":";
                        lbl1.AutoSize = true;
                        lbl1.Location = new Point(item.X, item.Y);
                        frm.pnlCondition.Controls.Add(lbl1);
                        switch (item.control_type)
                        {
                            case 0:
                                TextBox txt1 = new TextBox();
                                txt1.Name = item.ConditionCode;
                                txt1.Location = new Point(item.X + lbl1.Width, item.Y - 3);
                                frm.pnlCondition.Controls.Add(txt1);
                                break;
                            case 1:
                                DateTimePicker dtp1 = new DateTimePicker();
                                dtp1.Name = item.ConditionCode;
                                dtp1.Location = new Point(item.X + lbl1.Width, item.Y - 3);
                                frm.pnlCondition.Controls.Add(dtp1);
                                break;
                            case 2:
                                ComboBox cbx1 = new ComboBox();
                                cbx1.Name = item.ConditionCode;
                                cbx1.Location = new Point(item.X + lbl1.Width, item.Y - 3);
                                string[] items = item.ConditionValue.Split(';');
                                foreach (string s in items)
                                { cbx1.Items.Add(s); }
                                frm.pnlCondition.Controls.Add(cbx1);
                                break;
                            default: break;
                        }
                    }
                    frm.pnlCondition.Refresh();
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Valid()) { return; }
                this.mid_obj = Curr_Condition;
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                return;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }


        private bool Valid()
        {
            try
            {
                string ConditionCode = this.txtCode.Text.Trim();
                string ConditionName = this.txtName.Text.Trim();
                int control_type = this.cbxcontrol_type.SelectedIndex;
                string ConditionValue = this.txtValue.Text.Trim();
                string X = txtLeft.Text.Trim();
                string Y = txtUp.Text.Trim();
                if (string.IsNullOrEmpty(ConditionCode))
                {
                    MsgBox.ShowWarn("请输入条码编码!");
                    return false;
                }
                if (string.IsNullOrEmpty(ConditionName))
                {
                    MsgBox.ShowWarn("请输入条件名称!");
                    return false;
                }
                if (control_type == 2 && string.IsNullOrEmpty(ConditionValue))
                {
                    MsgBox.ShowWarn("请输入条件值!");
                    return false;
                }
                if (string.IsNullOrEmpty(X))
                {
                    MsgBox.ShowWarn("请输入左边距!");
                    return false;
                }
                if (string.IsNullOrEmpty(Y))
                {
                    MsgBox.ShowWarn("请输入上边距!");
                    return false;
                }

                int XX, YY = 0;
                try
                {
                    XX = Convert.ToInt16(X);
                    YY = Convert.ToInt16(Y);
                }
                catch
                { MsgBox.ShowWarn("请输入有效值!"); return false; }
                Curr_Condition.ConditionCode = ConditionCode;
                Curr_Condition.ConditionName = ConditionName;
                Curr_Condition.ConditionValue = ConditionValue;
                Curr_Condition.control_type = control_type;
                Curr_Condition.X = XX;
                Curr_Condition.Y = YY;
                return true;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); return false; }
        }
    }
}
