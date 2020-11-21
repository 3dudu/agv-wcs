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

namespace Simulation.SimulationSysForm
{
    public partial class FrmTestPointToPoint : BaseForm
    {

        public string BeginLandCode = "";
        public string EndLandCode = "";
        public FrmTestPointToPoint()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.labelControl1.Focus();
                BeginLandCode = txtBeginLandCode.Text.Trim();
                EndLandCode = txtEndLandCode.Text.Trim();
                if (string.IsNullOrEmpty(BeginLandCode) || string.IsNullOrEmpty(EndLandCode))
                {
                    MsgBox.ShowWarn("请输入起止地标号!");
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
                this.labelControl1.Focus();
                this.DialogResult = DialogResult.Cancel;
                return;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }
    }
}
