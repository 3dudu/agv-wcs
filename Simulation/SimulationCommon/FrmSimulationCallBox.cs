using AGVDAccess;
using Model.MDM;
using SimulationModel;
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
    public partial class FrmSimulationCallBox : Form
    {
        public int CallBoxID = -1;
        Simulator Simula = null;
        public FrmSimulationCallBox(int BoxID,Simulator Simulator)
        {
            InitializeComponent();
            CallBoxID = BoxID;
            Simula = Simulator;
            this.lblBoxID.Text = CallBoxID.ToString() + "号按钮盒";
        }

        #region 窗体属性

        #endregion


        #region 窗体事件
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Simula == null) { return; }
                int BtnID = Convert.ToInt16(button1.Tag);
                string result = Simula.CreatTask(CallBoxID, BtnID);
                this.txtMesWarn.Text = result;
            }
            catch(Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (Simula == null) { return; }
                int BtnID = Convert.ToInt16(button2.Tag);
                string result = Simula.CreatTask(CallBoxID, BtnID);
                this.txtMesWarn.Text = result;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (Simula == null) { return; }
                int BtnID = Convert.ToInt16(button3.Tag);
                string result = Simula.CreatTask(CallBoxID, BtnID);
                this.txtMesWarn.Text = result;
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }
        #endregion

    }
}
