using AGVDAccess;
using Model.Comoon;
using Model.MSM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulation.SimulationCommon
{
    public partial class FrmDirectionSet : BaseForm
    {
        public FrmDirectionSet()
        {
            InitializeComponent();
        }
        private void FrmDirectionSet_Shown(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = AGVClientDAccess.LoadAGVCoordinate();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        switch (Convert.ToInt16(dr["Direction"]))
                        {
                            case 0:
                                cbxBei.SelectedItem = dr["Angle"].ToString();
                                break;
                            case 1:
                                cbxDong.SelectedItem = dr["Angle"].ToString();
                                break;
                            case 2:
                                cbxNan.SelectedItem = dr["Angle"].ToString(); ;
                                break;
                            case 3:
                                cbxXi.SelectedItem = dr["Angle"].ToString();
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

        private void btnClear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (Valid())
                {
                    List<string> Parameters = new List<string>();
                    Parameters.Add("0," + cbxBei.SelectedItem.ToString());
                    Parameters.Add("1," + cbxDong.SelectedItem.ToString());
                    Parameters.Add("2," + cbxNan.SelectedItem.ToString());
                    Parameters.Add("3," + cbxXi.SelectedItem.ToString());
                    OperateReturnInfo opr = AGVClientDAccess.SaveAGVCoordinate(Parameters);
                    MsgBox.Show(opr);
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


        private bool Valid()
        {
            try
            {
                labelControl1.Focus();
                if (cbxBei.SelectedIndex + cbxDong.SelectedIndex + cbxNan.SelectedIndex + cbxXi.SelectedIndex != 6)
                {
                    MsgBox.ShowWarn("角度有重复!");
                    return false;
                }
                else
                { return true; }
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); return false; }
        }
    }
}
