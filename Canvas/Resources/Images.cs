using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace Canvas
{
	class ImagesUtil
	{
		static public ImageList GetToolbarImageList(Type type, string resourceName, Size imageSize, Color transparentColor)
		{
			System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(type, resourceName);
			ImageList imageList = new ImageList();
			imageList.ImageSize = imageSize;
			imageList.TransparentColor = transparentColor;
			imageList.Images.AddStrip(bitmap);
			imageList.ColorDepth = ColorDepth.Depth24Bit;
			return imageList;
		}
	}

	public class MenuImages16x16
	{
		static private ImageList m_imageList = null;
		
		public enum eIndexes
		{
			Undo = 0,
			Redo,
			NewDocument,
			OpenDocument,
			SaveDocument,
		}
		static public ImageList ImageList()
		{
			Type t = typeof(MenuImages16x16);
			if (m_imageList == null)
				m_imageList = ImagesUtil.GetToolbarImageList(t, "Resources.menuimages.bmp", new Size(16, 16), Color.White);
			return m_imageList;
		}
		static public Image Image(eIndexes index)
		{
			return ImageList().Images[(int)index];
		}
	}
	public class DrawToolsImages16x16
	{
		static private ImageList m_imageList = null;

		public enum eIndexes
		{
			Select,
			Pan,
			Move,
			Line,
			CircleCR,
			Circle2P,
			ArcCR,
			Arc2P,
			Arc3P132,
			Arc3P123,
		}
		static public ImageList ImageList()
		{
			Type t = typeof(MenuImages16x16);
			if (m_imageList == null)
				m_imageList = ImagesUtil.GetToolbarImageList(t, "Resources.drawtoolimages.bmp", new Size(16, 16), Color.White);
			return m_imageList;
		}
		static public Image Image(eIndexes index)
		{
			return ImageList().Images[(int)index];
		}
	}
	public class EditToolsImages16x16
	{
		static private ImageList m_imageList = null;

		public enum eIndexes
		{
			Meet2Lines,
			LineSrhinkExtend,
		}
		static public ImageList ImageList()
		{
			Type t = typeof(MenuImages16x16);
			if (m_imageList == null)
				m_imageList = ImagesUtil.GetToolbarImageList(t, "Resources.edittoolimages.bmp", new Size(16, 16), Color.White);
			return m_imageList;
		}
		static public Image Image(eIndexes index)
		{
			return ImageList().Images[(int)index];
		}
	}
}
