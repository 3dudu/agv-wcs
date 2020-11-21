using Canvas.CanvasCtrl;
using Canvas.Layers;
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
    public partial class FrmOption : BaseForm
    {
        public FrmOption(bool gridEnable, GridLayer.eStyle gridStyle, Color gridColor, Color BgColor, Color penColor, float penWidth)
        {
            InitializeComponent();
            GridEnable = gridEnable;
            GridStyle = gridStyle;
            GridColor = gridColor;
            BackGroudColor = BgColor;
            PenColor = penColor;
            PenWidth = penWidth;
        }

        public bool GridEnable;
        public GridLayer.eStyle GridStyle;
        public Color GridColor;
        public Color BackGroudColor;
        public Color PenColor;
        public float PenWidth;

        private void FrmOption_Shown(object sender, EventArgs e)
        {
            try
            {
                clorPk_bg.Color = BackGroudColor;
                clorPk_grid.Color = GridColor;
                ckeEnableGrid.Checked = GridEnable;
                rdoLines.Checked = GridStyle == GridLayer.eStyle.Lines ? true : false;
                rdoPoints.Checked = GridStyle == GridLayer.eStyle.Lines ? false : true;
                clorPk_pen.Color = PenColor;
                txtPenSize.Text = PenWidth.ToString();
                labelControl1.Focus();
            }
            catch (Exception ex)
            { MsgBox.ShowError(ex.Message); }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                BackGroudColor = clorPk_bg.Color;
                GridEnable = ckeEnableGrid.Checked;
                GridColor = clorPk_grid.Color;
                GridStyle = rdoLines.Checked ? GridLayer.eStyle.Lines : GridLayer.eStyle.Dots;
                PenColor = clorPk_pen.Color;
                PenWidth = (float)Convert.ToDouble(txtPenSize.Text);
                this.DialogResult = DialogResult.OK;
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

        private void rdoLines_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rdoPoints_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
