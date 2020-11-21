using Canvas.CanvasInterfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas.CanvasCtrl
{
    /// <summary>
    /// 画布包装类
    /// </summary>
    public class CanvasWrapper : ICanvas
    {
        #region 属性
        CanvasCtrller m_canvas;
        Graphics m_graphics;
        Rectangle m_rect;
        public IModel Model
        {
            get { return m_canvas.Model; }
        }

        public IModel DataModel
        {
            get { return m_canvas.Model; }
        }

        public CanvasCtrller CanvasCtrl
        {
            get { return m_canvas; }
        }

        public UnitPoint ScreenTopLeftToUnitPoint()
        {
            return m_canvas.ScreenTopLeftToUnitPoint();
        }
        public UnitPoint ScreenBottomRightToUnitPoint()
        {
            return m_canvas.ScreenBottomRightToUnitPoint();
        }
        #endregion
        public CanvasWrapper(CanvasCtrller canvas)
        {
            m_canvas = canvas;
            m_graphics = null;
            m_rect = new Rectangle();
        }
        public CanvasWrapper(CanvasCtrller canvas, Graphics graphics, Rectangle clientrect)
        {
            m_canvas = canvas;
            m_graphics = graphics;
            m_rect = clientrect;
        }

        /// <summary>
        /// UnitPoint转换成当前屏幕点
        /// </summary>
        public PointF ToScreen(UnitPoint unitpoint)
        {
            return m_canvas.ToScreen(unitpoint);
        }

        /// <summary>
        /// 将UnitPoint值转换成当前屏幕值
        /// </summary>
        public float ToScreen(double unitvalue)
        {
            return m_canvas.ToScreen(unitvalue);
        }

        /// <summary>
        /// 将当前屏幕值转换成UnitPoint值
        /// </summary>
        public double ToUnit(float screenvalue)
        {
            return m_canvas.ToUnit(screenvalue);
        }

        /// <summary>
        /// 将当前屏幕坐标转换成UnitPoint坐标
        /// </summary>
        public UnitPoint ToUnit(PointF screenpoint)
        {
            return m_canvas.ToUnit(screenpoint);
        }

        /// <summary>
        /// 当前画布
        /// </summary>
        public Graphics Graphics
        {
            get { return m_graphics; }
        }

        /// <summary>
        /// 当前画布矩形尺寸
        /// </summary>
        public Rectangle ClientRectangle
        {
            get { return m_rect; }
            set { m_rect = value; }
        }

        /// <summary>
        /// 创建画笔
        /// </summary>
        public Pen CreatePen(Color color)
        {
            return m_canvas.CreatePen(color, 0.05F);
        }


        /// <summary>
        /// 刷新画布
        /// </summary>
        public void Invalidate()
        {
            m_canvas.DoInvalidate(false);
        }

        /// <summary>
        /// 当前选中元素
        /// </summary>
        public IDrawObject CurrentObject
        {
            get { return m_canvas.NewObject; }
        }
        public void Dispose()
        {
            m_graphics = null;
        }


        public void DrawLine(ICanvas canvas, Pen pen, UnitPoint p1, UnitPoint p2)
        {
            try
            {
                m_canvas.DrawLine(canvas, pen, p1, p2);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void DrawLandMark(ICanvas canvas, Brush pen, string code, UnitPoint Point)
        {
            try
            {
                m_canvas.DrawLandMark(canvas, pen, code, Point);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void DrawImge(ICanvas canvas, Pen pen, UnitPoint Location, float Widht, float Hight, Image img, string values)
        {
            try
            {
                m_canvas.DrawImge(canvas, pen, Location, Widht, Hight, img, values);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void DrawTxt(ICanvas canvas, string code, UnitPoint Point, int FontSize, Color fontColor)
        {
            try
            {
                m_canvas.DrawTxt(canvas, code, Point, FontSize, fontColor);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void DrawBizer(ICanvas canvas, Pen pen, UnitPoint p1, UnitPoint p2, UnitPoint p3, UnitPoint p4)
        {
            try
            {
                m_canvas.DrawBizer(canvas, pen, p1, p2, p3, p4);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void DrawStorage(ICanvas canvas, Brush Pen, string code, UnitPoint Point)
        {
            try
            {
                m_canvas.DrawStorage(canvas, Pen, code, Point);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void DrawPosition(ICanvas canvas, Brush Pen, string code, UnitPoint Point)
        {
            try
            {
                m_canvas.DrawPosition(canvas, Pen, code, Point);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void DrawBtnBox(ICanvas canvas, float Radius, UnitPoint Point, bool Selected)
        {
            try
            {
                m_canvas.DrawBtnBox(canvas, Radius, Point, Selected);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void DrawAGV(ICanvas canvas, Pen pen, UnitPoint p, string Code)
        {
            try
            {
                m_canvas.DrawAGV(canvas, pen, p, Code);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void DrawArc(ICanvas canvas, Pen pen, UnitPoint center, float radius, float beginangle, float angle)
        {
            try
            {
                m_canvas.DrawArc(canvas, pen, center, radius, beginangle, angle);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public SizeF GetStrValueSize(string Str, Font f)
        {
            return m_canvas.GetStrValueSize(Str,f);
        }
    }
}
