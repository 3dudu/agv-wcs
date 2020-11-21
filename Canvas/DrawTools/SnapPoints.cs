using Canvas.CanvasInterfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas.DrawTools
{
    /// <summary>
    /// 所有的吸附点类
    /// </summary>
    public class SnapPointBase : ISnapPoint
    {
        protected UnitPoint m_snappoint;
        protected RectangleF m_boundingRect;
        protected IDrawObject m_owner;

        public IDrawObject Owner
        {get { return m_owner; } }

        public SnapPointBase(ICanvas canvas, IDrawObject owner, UnitPoint snappoint)
        {
            m_owner = owner;
            m_snappoint = snappoint;
            float size = (float)canvas.ToUnit(14);
            m_boundingRect.X = (float)(snappoint.X - size / 2);
            m_boundingRect.Y = (float)(snappoint.Y - size / 2);
            m_boundingRect.Width = size;
            m_boundingRect.Height = size;
        }

        #region 实现 ISnapPoint 接口
        public virtual UnitPoint SnapPoint
        {
            get { return m_snappoint; }
        }


        public virtual RectangleF BoundingRect
        {
            get { return m_boundingRect; }
        }

        public virtual void Draw(ICanvas canvas) { }

        /// <summary>
        /// 画吸附点
        /// </summary>
        protected void DrawPoint(ICanvas canvas, Pen pen, Brush fillBrush)
        {
            try
            {
                Rectangle screenrect = ScreenUtils.ConvertRect(ScreenUtils.ToScreenNormalized(canvas, m_boundingRect));
                canvas.Graphics.DrawRectangle(pen, screenrect);
                screenrect.X++;
                screenrect.Y++;
                screenrect.Width--;
                screenrect.Height--;
                if (fillBrush != null)
                { canvas.Graphics.FillRectangle(fillBrush, screenrect); }
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion
    }//end SnapPointBase
}
