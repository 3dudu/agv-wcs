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

namespace RCSEditTool.FrmSystem
{
    public partial class FrmLandCooraSet : BaseForm
    {
        public FrmLandCooraSet(string PreLandLocationInfo)
        {
            InitializeComponent();
            this.PreLandLocationInfo = PreLandLocationInfo;
        }

        public string ReferLand { get; set; }
        public double Lenth { get; set; }
        public int RelativePos { get; set; }
        public float LandX { get; set; }
        public float LandY { get; set; }
        private string PreLandLocationInfo { get; set; }



        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                //string ReferLand = txtReferLand.Text.Trim();
                //if (string.IsNullOrEmpty(ReferLand))
                //{
                //    MsgBox.ShowWarn("请维护参照地标");
                //    return;
                //}
                //string Lenth = txtLenth.Text.Trim();
                //if (string.IsNullOrEmpty(Lenth))
                //{
                //    MsgBox.ShowWarn("请维护距离长度");
                //    return;
                //}
                //try
                //{
                //    this.Lenth = Convert.ToDouble(Lenth);
                //}
                //catch
                //{
                //    MsgBox.ShowWarn("请输入正确的距离长度");
                //    return;
                //}
                //if (cbxRelativePos.SelectedIndex < 0)
                //{
                //    MsgBox.ShowWarn("请选择相对参照地标的位置");
                //    return;
                //}
                //this.ReferLand = ReferLand;
                //RelativePos = cbxRelativePos.SelectedIndex;

                string CorX = this.txtCorX.Text.ToString().Trim();
                if (string.IsNullOrEmpty(CorX))
                {
                    MsgBox.ShowWarn("请维护X坐标");
                    return;
                }
                try
                {
                    this.LandX = Convert.ToSingle(CorX);
                }
                catch
                {
                    MsgBox.ShowWarn("请输入正确的X坐标");
                    return;
                }

                string CorY = this.txtCorY.Text.ToString().Trim();
                if (string.IsNullOrEmpty(CorY))
                {
                    MsgBox.ShowWarn("请维护Y坐标");
                    return;
                }
                try
                {
                    this.LandY = Convert.ToSingle(CorY);
                }
                catch
                {
                    MsgBox.ShowWarn("请输入正确的Y坐标");
                    return;
                }
                this.DialogResult = DialogResult.OK;
                return;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
                return;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void FrmLandCooraSet_Shown(object sender, EventArgs e)
        {
            try
            {
                //if (!string.IsNullOrEmpty(PreLandLocationInfo))
                //{
                //    string[] Condition = PreLandLocationInfo.Split(',');
                //    if (Condition != null && Condition.Length > 1)
                //    {
                //        this.txtReferLand.Text = Condition[0].ToString();
                //        this.cbxRelativePos.SelectedIndex = Convert.ToInt16(Condition[1].ToString());
                //    }
                //    this.txtLenth.Focus();
                //}
                //else
                //{ this.txtReferLand.Focus(); }

                this.txtCorX.Focus();
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }
    }
}
