using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Canvas.Options
{
	public partial class GridPage : Form, CommonTools.IPropertyDialogPage
	{
		public GridPage()
		{
			InitializeComponent();
			m_gridBinding.CurrentChanged += new EventHandler(m_dataBinding_CurrentChanged);
			m_gridBinding.DataMemberChanged += new EventHandler(m_dataBinding_DataMemberChanged);
			/*
			this.m_lines.Tag = GridLayer.eStyle.Lines;
			this.m_dots.Tag = GridLayer.eStyle.Dots;
			
			this.m_lines.DataBindings.Add(new RadioButtonBinder("Value", m_dataBinding, "Style", this.m_lines.Tag));
			this.m_dots.DataBindings.Add(new RadioButtonBinder("Value", m_dataBinding, "Style", this.m_dots.Tag));
			*/
			this.m_lines.AddDatabinding(m_gridBinding, "Style", GridLayer.eStyle.Lines);
			this.m_dots.AddDatabinding(m_gridBinding, "Style", GridLayer.eStyle.Dots);

			this.m_gridHeight.DataBindings.Clear();
			this.m_gridHeight.DataBindings.Add(new CommonTools.BindingWithNotify("Value", this.m_gridBinding, "Spacing.Height"));

			this.m_gridWidth.DataBindings.Clear();
			this.m_gridWidth.DataBindings.Add(new CommonTools.BindingWithNotify("Value", this.m_gridBinding, "Spacing.Width"));
		}

		void m_dataBinding_CurrentChanged(object sender, EventArgs e)
		{
			//throw new Exception("The method or operation is not implemented.");
		}

		void m_dataBinding_DataMemberChanged(object sender, EventArgs e)
		{
			//throw new Exception("The method or operation is not implemented.");
		}

		#region IPropertyDialogPage Members
		public void BeforeDeactivated(object dataObject)
		{
		}

		public void BeforeActivated(object dataObject)
		{
			OptionsConfig config = dataObject as OptionsConfig;
			if (m_gridBinding.Count == 0)
			{
				m_gridBinding.Clear();
				m_gridBinding.Add(config.Grid);
			}
			if (m_backgroundBinding.Count == 0)
			{
				m_backgroundBinding.Clear();
				m_backgroundBinding.Add(config.Background);
			}
		}
		#endregion
	}
}