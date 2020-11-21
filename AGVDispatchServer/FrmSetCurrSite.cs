using AGVCore;
using Model.CarInfoExtend;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AGVDispatchServer
{
    public partial class FrmSetCurrSite : BaseForm
    {
        public FrmSetCurrSite()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                int CarID = 0;
                int SetSite = 0;
                try
                {
                    CarID = Convert.ToInt32(txtAGVID.Text.Trim());
                }
                catch
                {
                    MessageBox.Show("输入车号有误!", "提示", MessageBoxButtons.OK);
                    return;
                }

                try
                {
                    SetSite = Convert.ToInt32(txtCurrSite.Text.Trim());
                }
                catch
                {
                    MessageBox.Show("输入站点有误!", "提示", MessageBoxButtons.OK);
                    return;
                }
                CarInfo SetCar = CoreData.CarList.FirstOrDefault(q => q.AgvID == CarID);
                if (SetCar == null)
                {
                    MessageBox.Show("输入的当前车辆信息不存在!", "提示", MessageBoxButtons.OK);
                    return;
                }
                SetCar.CurrSite = SetSite;
                MessageBox.Show("设置成功!", "提示", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                return;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
    }
}
