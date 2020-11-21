using Canvas.CanvasInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Canvas.EditTools
{
    public class LinesMeetEditTool : IEditTool
    {
        IEditToolOwner m_owner;
        LinePoints m_l1Original = new LinePoints();
        LinePoints m_l2Original = new LinePoints();
        LinePoints m_l1NewPoint = new LinePoints();
        LinePoints m_l2NewPoint = new LinePoints();

        public bool SupportSelection
        {
            get { return false; }
        }

        public LinesMeetEditTool(IEditToolOwner owner)
        {
            m_owner = owner;
            SetHint("选择第一个直线");
        }

        void SetHint(string text)
        {
            try
            {
                if (m_owner != null)
                {
                    if (text.Length > 0)
                    { m_owner.SetHint("提示:" + text); }
                    else
                    { m_owner.SetHint(""); }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public IEditTool Clone()
        {
            try
            {
                LinesMeetEditTool t = new LinesMeetEditTool(m_owner);
                return t;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void SetHitObjects(UnitPoint point, List<IDrawObject> list) { }

        public void OnMouseMove(ICanvas canvas, UnitPoint point) { }

        public eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        {
            try
            {
                List<IDrawObject> items = canvas.DataModel.GetHitObjects(canvas, point);
                DrawTools.LineTool line = null;
                foreach (IDrawObject item in items)
                {
                    if (item is DrawTools.LineTool)
                    {
                        line = item as DrawTools.LineTool;
                        if (line.Type == DrawTools.LineType.PointLine) { return eDrawObjectMouseDownEnum.Done; }
                        if (line != m_l1Original.Line)
                            break;
                    }
                }
                if (line == null)
                {
                    if (m_l1Original.Line == null)
                    { SetHint("请选择第一个直线"); }
                    else
                    { SetHint("请选择第二个直线"); }
                    return eDrawObjectMouseDownEnum.Continue;
                }
                if (m_l1Original.Line == null)
                {
                    line.Highlighted = true;
                    m_l1Original.SetLine(line);
                    m_l1Original.MousePoint = point;
                    SetHint("请选择第二个直线");
                    return eDrawObjectMouseDownEnum.Continue;
                }
                if (m_l2Original.Line == null)
                {
                    line.Highlighted = true;
                    m_l2Original.SetLine(line);
                    m_l2Original.MousePoint = point;

                    UnitPoint intersectpoint = HitUtil.LinesIntersectPoint(
                        m_l1Original.Line.P1,
                        m_l1Original.Line.P2,
                        m_l2Original.Line.P1,
                        m_l2Original.Line.P2);

                    //如果两条实现没有相交，则将两条直线延长到交点
                    if (intersectpoint == UnitPoint.Empty)
                    {
                        UnitPoint apprarentISPoint = HitUtil.FindApparentIntersectPoint(m_l1Original.Line.P1, m_l1Original.Line.P2, m_l2Original.Line.P1, m_l2Original.Line.P2);
                        if (apprarentISPoint == UnitPoint.Empty)
                            return eDrawObjectMouseDownEnum.Done;
                        m_l1Original.Line.ExtendLineToPoint(apprarentISPoint);
                        m_l2Original.Line.ExtendLineToPoint(apprarentISPoint);
                        m_l1NewPoint.SetLine(m_l1Original.Line);
                        m_l2NewPoint.SetLine(m_l2Original.Line);
                        canvas.DataModel.AfterEditObjects(this);
                        return eDrawObjectMouseDownEnum.Done;
                    }

                    m_l1NewPoint.SetNewPoints(m_l1Original.Line, m_l1Original.MousePoint, intersectpoint);
                    m_l2NewPoint.SetNewPoints(m_l2Original.Line, m_l2Original.MousePoint, intersectpoint);
                    canvas.DataModel.AfterEditObjects(this);
                    return eDrawObjectMouseDownEnum.Done;
                }
                return eDrawObjectMouseDownEnum.Done;
            }
            catch (Exception ex)
            { throw ex; }
        }


        public void OnMouseUp(ICanvas canvas, UnitPoint point, ISnapPoint snappoint) { }

        public void OnKeyDown(ICanvas canvas, KeyEventArgs e) { }

        public void Finished()
        {
            try
            {
                SetHint("");
                if (m_l1Original.Line != null)
                { m_l1Original.Line.Highlighted = false; }
                if (m_l2Original.Line != null)
                { m_l2Original.Line.Highlighted = false; }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Undo()
        {
            try
            {
                m_l1Original.ResetLine();
                m_l2Original.ResetLine();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Redo()
        {
            try
            {
                m_l1NewPoint.ResetLine();
                m_l2NewPoint.ResetLine();
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
