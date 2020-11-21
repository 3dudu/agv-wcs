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
    ///直线辅助点
    /// </summary>
    public class NodePointLine : INodePoint
    {
        public enum ePoint
        {
            P1,
            P2,
        }

        #region 属性
        static bool m_angleLocked = false;
        LineTool m_owner;
        LineTool m_clone;
        UnitPoint m_originalPoint;
        UnitPoint m_endPoint;
        ePoint m_pointId;
        #endregion


        #region 函数
        public NodePointLine(LineTool owner, ePoint id)
        {
            try
            {
                m_owner = owner;
                m_clone = m_owner.Clone() as LineTool;
                m_pointId = id;
                m_originalPoint = GetPoint(m_pointId);
            }
            catch (Exception ex)
            { throw ex; }
        }

        protected UnitPoint GetPoint(ePoint pointid)
        {
            try
            {
                if (pointid == ePoint.P1)
                    return m_clone.P1;
                if (pointid == ePoint.P2)
                    return m_clone.P2;
                return m_owner.P1;
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion

        #region 实现INodePoint
        public IDrawObject GetClone()
        { return m_clone; }

        public IDrawObject GetOriginal()
        { return m_owner; }

        public void SetPosition(UnitPoint pos)
        {
            try
            {
                if (Control.ModifierKeys == Keys.Control)
                    pos = HitUtil.OrthoPointD(OtherPoint(m_pointId), pos, 45);
                if (m_angleLocked || Control.ModifierKeys == (Keys)(Keys.Control | Keys.Shift))
                    pos = HitUtil.NearestPointOnLine(m_owner.P1, m_owner.P2, pos, true);
                SetPoint(m_pointId, pos, m_clone);
            }
            catch (Exception ex)
            { throw ex; }
        }

        protected UnitPoint OtherPoint(ePoint currentpointid)
        {
            try
            {
                if (currentpointid == ePoint.P1)
                    return GetPoint(ePoint.P2);
                return GetPoint(ePoint.P1);
            }
            catch (Exception ex)
            { throw ex; }
        }

        protected void SetPoint(ePoint pointid, UnitPoint point, LineTool line)
        {
            try
            {
                if (pointid == ePoint.P1)
                    line.P1 = point;
                if (pointid == ePoint.P2)
                    line.P2 = point;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Finish()
        {
            try
            {
                m_endPoint = GetPoint(m_pointId);
                m_owner.P1 = m_clone.P1;
                m_owner.P2 = m_clone.P2;
                m_clone = null;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Cancel() { }

        public void OnKeyDown(ICanvas canvas, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.L)
                {
                    m_angleLocked = !m_angleLocked;
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Undo()
        {
            try
            {
                SetPoint(m_pointId, m_originalPoint, m_owner);
            }
            catch (Exception ex)
            { throw ex; }
        }
        public void Redo()
        {
            try
            {
                SetPoint(m_pointId, m_endPoint, m_owner);
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion
    }
}
