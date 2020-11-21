using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Canvas.Options
{
	public class SizeFWrapper
	{
		// data binding needs object, not struct, therefore this wrapper
		SizeF m_size = SizeF.Empty;
		public object Owner;
		public SizeFWrapper(SizeF size)
		{
			m_size = size;
		}
		public SizeFWrapper(float x, float y)
		{
			m_size = new SizeF(x, y);
		}
		public SizeF SizeF
		{
			get { return m_size; }
			set { m_size = value; }
		}
		public float Width
		{
			get { return m_size.Width; }
			set 
			{ 
				if (value < 0.001f)
					value = 0.001f;
				if (value > 1000f)
					value = 1000f;
				m_size.Width= value; 
			}
		}
		public float Height
		{
			get { return m_size.Height; }
			set 
			{ 
				if (value < 0.001f)
					value = 0.001f;
				if (value > 1000f)
					value = 1000f;
				m_size.Height = value; 
			}
		}
	}
	public class OptionsBackground
	{
		public void CopyFromLayer(BackgroundLayer layer)
		{
			m_color = layer.Color;
		}
		public void CopyToLayer(BackgroundLayer layer)
		{
			layer.Color = m_color;
		}

		Color	m_color = Color.Black;
		public Color Color
		{
			get { return m_color; }
			set { m_color = value; }
		}
	}
	public class OptionsGrid
	{
		public OptionsGrid()
		{
			m_spacing = new SizeFWrapper(1, 1);
			m_spacing.Owner = this;
		}
		SizeFWrapper m_spacing;
		GridLayer m_layer = new GridLayer();
		public void CopyFromLayer(GridLayer layer)
		{
			m_layer.Copy(layer);
			m_spacing.SizeF = m_layer.Spacing;
		}
		public void CopyToLayer(GridLayer layer)
		{
			layer.Copy(m_layer);
			layer.Spacing = m_spacing.SizeF;
		}
		public bool Enabled
		{
			get { return m_layer.Enabled; }
			set { m_layer.Enabled = value;}
		}
		public GridLayer.eStyle Style
		{
			get { return m_layer.GridStyle; }
			set { m_layer.GridStyle = value; }
		}
		public Color Color
		{
			get { return m_layer.Color; }
			set { m_layer.Color = value; }
		}
		public SizeFWrapper Spacing
		{
			get { return m_spacing; }
		}
	}
	public class OptionsLayer
	{
		DrawingLayer m_layer = new DrawingLayer("", "", Color.Black, 0);
		int	m_count = 0;
		public OptionsLayer(DrawingLayer layer)
		{
			CopyFromLayer(layer);
		}
		public void CopyFromLayer(DrawingLayer layer)
		{
			m_layer.Copy(layer, false);
			m_count = layer.Count;
		}
		public void CopyToLayer(DrawingLayer layer)
		{
			layer.Copy(m_layer, false);
		}
		public DrawingLayer Layer
		{
			get { return m_layer; }
			set { m_layer = value; }
		}
		public int Count
		{
			get { return m_count; }
		}
	}
	public class OptionsConfig
	{
		OptionsBackground m_background = new OptionsBackground();
		OptionsGrid m_grid = new OptionsGrid();
		List<OptionsLayer>	m_layers = new List<OptionsLayer>();
		
		public OptionsBackground Background
		{
			get { return m_background; }
		}
		public OptionsGrid Grid
		{
			get { return m_grid; }
		}
		public List<OptionsLayer> Layers
		{
			get { return m_layers; }
		}
	}
}
