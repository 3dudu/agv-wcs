using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Canvas
{
	// GDI class copied from http://www.codeproject.com/vcpp/gdiplus/gdiandgdiplusmixed.asp
	public enum PenStyles
    {
        PS_SOLID=0
        ,PS_DASH=1
        ,PS_DOT=2
        ,PS_DASHDOT=3
        ,PS_DASHDOTDOT=4
        ,PS_NULL=5
        ,PS_INSIDEFRAME=6
        ,PS_USERSTYLE=7
        ,PS_ALTERNATE=8
        ,PS_STYLE_MASK=0x0000000F

        ,PS_ENDCAP_ROUND=     0x00000000
        ,PS_ENDCAP_SQUARE=    0x00000100
        ,PS_ENDCAP_FLAT=      0x00000200
        ,PS_ENDCAP_MASK =     0x00000F00
        ,PS_JOIN_ROUND=       0x00000000
        ,PS_JOIN_BEVEL=       0x00001000
        ,PS_JOIN_MITER=       0x00002000
        ,PS_JOIN_MASK=        0x0000F000

        ,PS_COSMETIC=         0x00000000
        ,PS_GEOMETRIC=        0x00010000
        ,PS_TYPE_MASK=        0x000F0000
    }
    public enum drawingMode
    {
        R2_BLACK=            1   /*  0       */
        ,R2_NOTMERGEPEN=      2   /* DPon     */
        ,R2_MASKNOTPEN=       3   /* DPna     */
        ,R2_NOTCOPYPEN=       4   /* PN       */
        ,R2_MASKPENNOT=       5   /* PDna     */
        ,R2_NOT=              6   /* Dn       */
        ,R2_XORPEN=           7   /* DPx      */
        ,R2_NOTMASKPEN=       8   /* DPan     */
        ,R2_MASKPEN=          9   /* DPa      */
        ,R2_NOTXORPEN=        10  /* DPxn     */
        ,R2_NOP=              11  /* D        */
        ,R2_MERGENOTPEN=      12  /* DPno     */
        ,R2_COPYPEN=          13  /* P        */
        ,R2_MERGEPENNOT=      14  /* PDno     */
        ,R2_MERGEPEN=         15  /* DPo      */
        ,R2_WHITE=            16  /*  1       */
        ,R2_LAST=             16
    }

	public class GDI
    {
        private IntPtr hdc;
        private System.Drawing.Graphics grp;
        public void BeginGDI(System.Drawing.Graphics g)
        {
            grp=g;
            hdc=grp.GetHdc();    
        }
        public void EndGDI()
        {
            grp.ReleaseHdc(hdc);
        }
        public IntPtr CreatePEN(PenStyles fnPenStyle,  int nWidth, int crColor)
        {
           return CreatePen(fnPenStyle,nWidth,crColor);
        }
        public bool DeleteOBJECT(IntPtr hObject)
        {
          return DeleteObject(hObject);
        }
        public IntPtr SelectObject(IntPtr hgdiobj)
        {
             return SelectObject(hdc,hgdiobj);
        }

        public void MoveTo(int X,int Y)
        {
           MoveToEx(hdc,X,Y,0);
        }
        public void LineTo(int X,int Y)
        {
            LineTo(hdc,X,Y);
        }
        public int SetROP2(drawingMode fnDrawMode)
        {
           return SetROP2(hdc,fnDrawMode);
        }
		public void SetPixel(int x, int y, int color)
		{
			SetPixelV(hdc, x, y, color & 0x00ffffff);
		}

		[System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
		public static extern void SetPixelV(IntPtr hdc, int x, int y, int color);
		
		[System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
		public static extern int SetROP2(IntPtr hdc, drawingMode fnDrawMode);  
       
        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
		public static extern bool MoveToEx(IntPtr hdc, int X, int Y, int oldp);

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
		public static extern bool LineTo(IntPtr hdc, int nXEnd, int nYEnd);

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
		public static extern IntPtr CreatePen(PenStyles fnPenStyle, int nWidth, int crColor);

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
		public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

		[System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
		public static extern void Rectangle(IntPtr hdc, int X1, int Y1, int X2, int Y2);

		[System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
		public static extern IntPtr GetStockObject(int brStyle);

		[System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
		public static extern int SetBkMode(IntPtr hdc, int iBkMode);

        public static int RGB(int R,int G,int B)
        {
            return (R|(G<<8)|(B<<16));         
        }
	}

	public static class XorGdi
	{
		private static int NULL_BRUSH = 5;
		private static int TRANSPARENT = 1;
		public static void DrawLine(PenStyles penStyle, int penWidth, Color col, 
			Graphics grp, int X1, int Y1, int X2, int Y2)
		{
			// Extract the Win32 HDC from the Graphics object supplied.
			IntPtr hdc = grp.GetHdc();

			// Create a pen.
			IntPtr gdiPen = GDI.CreatePen(penStyle, penWidth, GDI.RGB(col.R, col.G, col.B));
			GDI.SetROP2(hdc, drawingMode.R2_XORPEN);
			GDI.SetBkMode(hdc, TRANSPARENT);

			// Set the ROP cdrawint mode to XOR.
			GDI.SetROP2(hdc, drawingMode.R2_XORPEN);

			// Select the pen into the device context.
			IntPtr oldPen = GDI.SelectObject(hdc, gdiPen);

			// Draw the line.
			GDI.MoveToEx(hdc, X1, Y1, 0);
			GDI.LineTo(hdc, X2, Y2);

			// Put the old stuff back where it was.
			GDI.SelectObject(hdc, oldPen);
			GDI.DeleteObject(gdiPen);

			// Return the device context to Windows.
			grp.ReleaseHdc(hdc);
		}
		public static void DrawRectangle(Graphics dc, PenStyles penStyle, int penWidth, Color col,
			int X1, int Y1, int X2, int Y2)
		{
			// Extract the Win32 HDC from the Graphics object supplied.
			IntPtr hdc = dc.GetHdc();

			// Create a pen.
			IntPtr gdiPen = GDI.CreatePen(penStyle, penWidth, GDI.RGB(col.R, col.G, col.B));
			GDI.SetROP2(hdc, drawingMode.R2_XORPEN);
			GDI.SetBkMode(hdc, TRANSPARENT);

			// Set the ROP cdrawint mode to XOR.
			GDI.SetROP2(hdc, drawingMode.R2_XORPEN);

			// Select the pen into the device context.
			IntPtr oldPen = GDI.SelectObject(hdc, gdiPen);

			// Create a stock NULL_BRUSH brush and select it into the device
			// context so that the rectangle isn't filled.
			IntPtr oldBrush = GDI.SelectObject(hdc, GDI.GetStockObject(NULL_BRUSH));

			// Now XOR the hollow rectangle on the Graphics object with
			// a dotted outline.
			GDI.Rectangle(hdc, X1, Y1, X2, Y2);

			// Put the old stuff back where it was.
			GDI.SelectObject(hdc, oldBrush); // no need to delete a stock object
			GDI.SelectObject(hdc, oldPen);
			GDI.DeleteObject(gdiPen);        // but we do need to delete the pen

			// Return the device context to Windows.
			dc.ReleaseHdc(hdc);
		}

		public static void DrawRectangle(Graphics dc, PenStyles penStyle, int penWidth, Color col,
			PointF topleft, PointF bottomright)
		{
			DrawRectangle(dc, penStyle, penWidth, col, (int)topleft.X, (int)topleft.Y,(int)bottomright.X, (int)bottomright.Y);
		}
	}
}
