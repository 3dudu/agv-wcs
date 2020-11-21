using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Canvas.Options
{
	public partial class LayersPage : Form, CommonTools.IPropertyDialogPage
	{
		public LayersPage()
		{
			InitializeComponent();
		}
		#region IPropertyDialogPage Members
		public void BeforeDeactivated(object dataObject)
		{
		}
		public void BeforeActivated(object dataObject)
		{
		}
		#endregion
	}
}