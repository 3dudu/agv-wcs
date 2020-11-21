using Canvas.CanvasInterfaces;
using Canvas.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Canvas.DrawTools
{
    public class LineEdit : LineTool, IObjectEditInstance
    {
        #region 属性
        protected PerpendicularSnapPoint m_perSnap;
        protected TangentSnapPoint m_tanSnap;
        protected bool m_tanReverse = false;
        protected bool m_singleLineSegment = true;
        protected LineType Type;

        [Browsable(false)]
        public override string Id
        {
            get
            {
                if (m_singleLineSegment)
                    return "Line";
                return "Lines";
            }
        }
        #endregion

        #region 函数
        public LineEdit(bool singleLine)
          : base()
        {
            m_singleLineSegment = singleLine;
        }

        public override void InitializeFromModel(UnitPoint point, DrawingLayer layer, ISnapPoint snap)
        {
            base.InitializeFromModel(point, layer, snap);
            m_perSnap = snap as PerpendicularSnapPoint;
            m_tanSnap = snap as TangentSnapPoint;
        }

        public override void OnMouseMove(ICanvas canvas, UnitPoint point)
        {
            base.OnMouseMove(canvas, point);
        }


        public override eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        {
            if (m_perSnap != null || m_tanSnap != null)
            {
                if (snappoint != null)
                    point = snappoint.SnapPoint;
                OnMouseMove(canvas, point);
                if (m_singleLineSegment)
                    return eDrawObjectMouseDownEnum.Done;
                return eDrawObjectMouseDownEnum.DoneRepeat;
            }
            eDrawObjectMouseDownEnum result = base.OnMouseDown(canvas, point, snappoint);
            if (m_singleLineSegment)
                return eDrawObjectMouseDownEnum.Done;
            return eDrawObjectMouseDownEnum.DoneRepeat;
        }


        protected virtual void MouseMovePerpendicular(ICanvas canvas, UnitPoint point)
        {
            if (m_perSnap.Owner is LineTool)
            {
                LineTool src = m_perSnap.Owner as LineTool;
                m_p1 = HitUtil.NearestPointOnLine(src.P1, src.P2, point, true);
                m_p2 = point;
            }
        }


        public void Copy(LineEdit acopy)
        {
            base.Copy(acopy);
            m_perSnap = acopy.m_perSnap;
            m_tanSnap = acopy.m_tanSnap;
            m_tanReverse = acopy.m_tanReverse;
            m_singleLineSegment = acopy.m_singleLineSegment;
        }

        public override IDrawObject Clone()
        {
            LineEdit l = new LineEdit(false);
            l.Copy(this);
            return l;
        }

        public IDrawObject GetDrawObject()
        {
            return new LineTool(P1, P2, Width, Color);
        }
        #endregion

    }
}
