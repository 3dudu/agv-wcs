using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Display
{
    public partial class FrmFindLand : Form
    {

        public string LandCode { get; set; }
        public FrmFindLand()
        {
            InitializeComponent();
        }

        private void FrmFindLand_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (string.IsNullOrEmpty(txtLandCode.Text.Trim()))
                    {
                        MessageBox.Show("请输入地标号");
                        return;
                    }
                    LandCode = txtLandCode.Text.Trim();
                    this.DialogResult = DialogResult.OK;
                } 
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
    }
}
