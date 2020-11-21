using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace RCSEditTool
{
    public partial class BaseForm : DevExpress.XtraEditors.XtraForm
    {
        public BaseForm()
        {
            InitializeComponent();
        }

        [Browsable(false)]
        public object mid_obj
        { get; set; }
    }
}