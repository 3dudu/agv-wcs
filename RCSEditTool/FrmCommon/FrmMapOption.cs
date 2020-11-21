using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RCSEditTool.FrmCommon
{
    public partial class FrmMapOption : BaseForm
    {
        public float width=10f;
        public float height=5f;
        public float space=0.5f;
        public float len = 1f;
        public int girdnum = 3;
        public bool removeall = false;
        public bool storageFull = false;

        public FrmMapOption()
        {
            InitializeComponent();
        }

        public void initData(float width,float height,float space,int girdnum)
        {
            textBox1.Text = Convert.ToString(width);
            textBox2.Text = Convert.ToString(height);
            textBox3.Text = Convert.ToString(space);
            textBox5.Text = Convert.ToString(girdnum);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btn_okclick(object sender, EventArgs e)
        {
            try
            {
                width = (float)Convert.ToDouble(textBox1.Text);
                height = (float)Convert.ToDouble(textBox2.Text); 
                space = (float)Convert.ToDouble(textBox3.Text);
                girdnum = Convert.ToInt32(textBox5.Text);
                removeall = checkBox1.Checked;
                storageFull = checkBox2.Checked;
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
