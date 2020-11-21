using Canvas.CanvasInterfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas.Utils
{
    /// <summary>
    /// 绘画工具类
    /// </summary>
    public class DrawUtils
    {
        /// <summary>
        /// 获取选择元素时的画笔
        /// </summary>
        private static Pen m_selectedPen = null;
        public static Pen SelectedPen
        {
            get
            {
                if (m_selectedPen == null)
                {
                    m_selectedPen = new Pen(Color.Magenta, 1);
                    m_selectedPen.DashStyle = DashStyle.Dash;
                }
                return m_selectedPen;
            }
        }

        /// <summary>
        /// 画一个圆
        /// </summary>
        public static void DrawNode(ICanvas canvas, UnitPoint nodepoint)
        {
            try
            {
                RectangleF r = new RectangleF(canvas.ToScreen(nodepoint), new SizeF(0, 0));
                r.Inflate(3, 3);
                if (r.Right < 0 || r.Left > canvas.ClientRectangle.Width)
                { return; }
                if (r.Top < 0 || r.Bottom > canvas.ClientRectangle.Height)
                { return; }
                canvas.Graphics.FillRectangle(Brushes.White, r);
                r.Inflate(1, 1);
                canvas.Graphics.DrawRectangle(Pens.Black, ScreenUtils.ConvertRect(r));
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 画一个三角形
        /// </summary>
        public static void DrawTriangleNode(ICanvas canvas, UnitPoint nodepoint)
        {
            try
            {
                PointF screenpoint = canvas.ToScreen(nodepoint);
                float size = 4;
                PointF[] p = new PointF[]
                {
                new PointF(screenpoint.X - size, screenpoint.Y),
                new PointF(screenpoint.X, screenpoint.Y + size),
                new PointF(screenpoint.X + size, screenpoint.Y),
                new PointF(screenpoint.X, screenpoint.Y - size),
                };
                canvas.Graphics.FillPolygon(Brushes.White, p);
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
