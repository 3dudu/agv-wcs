using Canvas.CanvasInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Canvas.DrawTools
{
    /// <summary>
    /// 通过三点绘制圆弧的辅助点
    /// </summary>
    public class NodePointArc3PointPoint : INodePoint
    {
        #region 属性
        protected Arc3Point m_owner;
        protected Arc3Point m_clone;
        protected UnitPoint[] m_originalPoints = new UnitPoint[3];
        protected UnitPoint[] m_endPoints = new UnitPoint[3];
        public Arc3Point.eCurrentPoint m_curPoint;
        protected static int ThresholdPixel = 6;
        #endregion

        #region 实现方法和函数
        public NodePointArc3PointPoint(Arc3Point owner, Arc3Point.eCurrentPoint curpoint)
        {
            try
            {
                m_owner = owner;
                m_clone = m_owner.Clone() as Arc3Point;
                m_owner.Selected = false;
                m_clone.Selected = true;
                m_originalPoints[0] = m_owner.P1;
                m_originalPoints[1] = m_owner.P2;
                m_originalPoints[2] = m_owner.P3;
                m_curPoint = curpoint;
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion

        #region 内部类
        public enum eCurrentPoint
        {
            p1,
            p2,
            p3,
            startangle,
            endangle,
            radius,
            done,
        }
        #endregion

        #region INodePoint Members
        public IDrawObject GetClone()
        { return m_clone; }
        public IDrawObject GetOriginal()
        { return m_owner; }
        public virtual void SetPosition(UnitPoint pos)
        { SetPoint(m_clone, pos); }


        private void SetPoint(Arc3Point arc, UnitPoint pos)
        {
            try
            {
                if (m_curPoint == Arc3Point.eCurrentPoint.p1)
                { arc.P1 = pos; }
                if (m_curPoint == Arc3Point.eCurrentPoint.p2)
                { arc.P2 = pos; }
                if (m_curPoint == Arc3Point.eCurrentPoint.p3)
                { arc.P3 = pos; }
                double angleToRound = 0;
                if (Control.ModifierKeys == Keys.Control)
                { angleToRound = HitUtil.DegressToRadians(45); }
                double angleR = HitUtil.LineAngleR(arc.Center, pos, angleToRound);
                if (m_curPoint == Arc3Point.eCurrentPoint.startangle)
                { arc.P1 = HitUtil.PointOncircle(arc.Center, arc.Radius, angleR); }
                if (m_curPoint == Arc3Point.eCurrentPoint.endangle)
                { arc.P3 = HitUtil.PointOncircle(arc.Center, arc.Radius, angleR); }
                if (m_curPoint == Arc3Point.eCurrentPoint.radius)
                {
                    double radius = HitUtil.Distance(arc.Center, pos);
                    arc.P1 = HitUtil.PointOncircle(arc.Center, radius, HitUtil.DegressToRadians(arc.StartAngle));
                    arc.P2 = pos;
                    arc.P3 = HitUtil.PointOncircle(arc.Center, radius, HitUtil.DegressToRadians(arc.EndAngle));
                }
                arc.UpdateArcFrom3Points();
                if ((m_curPoint == Arc3Point.eCurrentPoint.startangle) || (m_curPoint == Arc3Point.eCurrentPoint.endangle))
                { arc.UpdateCenterNodeFromAngles(); }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual void Finish()
        {
            try
            {
                m_endPoints[0] = m_clone.P1;
                m_endPoints[1] = m_clone.P2;
                m_endPoints[2] = m_clone.P3;
                m_owner.Copy(m_clone);
                m_owner.Selected = true;
                m_clone = null;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Cancel()
        {
            m_owner.Selected = true;
        }


        public virtual void Undo()
        {
            try
            {
                m_owner.P1 = m_originalPoints[0];
                m_owner.P2 = m_originalPoints[1];
                m_owner.P3 = m_originalPoints[2];
                m_owner.UpdateArcFrom3Points();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual void Redo()
        {
            try
            {
                m_owner.P1 = m_endPoints[0];
                m_owner.P2 = m_endPoints[1];
                m_owner.P3 = m_endPoints[2];
                m_owner.UpdateArcFrom3Points();
            }catch(Exception ex)
            { throw ex; }
        }

        public void OnKeyDown(ICanvas canvas, KeyEventArgs e)
        {}
        #endregion

    }
}
