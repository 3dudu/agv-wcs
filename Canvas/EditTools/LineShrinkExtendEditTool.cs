using Canvas.CanvasInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Canvas.EditTools
{
    public class LineShrinkExtendEditTool : IEditTool
    {
        IEditToolOwner m_owner;
        Dictionary<DrawTools.LineTool, LinePoints> m_originalLines = new Dictionary<DrawTools.LineTool, LinePoints>();
        Dictionary<DrawTools.LineTool, LinePoints> m_modifiedLines = new Dictionary<DrawTools.LineTool, LinePoints>();
        public bool SupportSelection
        {
            get { return true; }
        }

        public LineShrinkExtendEditTool(IEditToolOwner owner)
        {
            m_owner = owner;
            SetHint("选择一条直线延伸");
        }

        void SetHint(string text)
        {
            if (m_owner != null)
            {
                if (text.Length > 0)
                { m_owner.SetHint("延长直线: " + text); }
                else
                { m_owner.SetHint(""); }
            }
        }

        public IEditTool Clone()
        {
            LineShrinkExtendEditTool t = new LineShrinkExtendEditTool(m_owner);
            return t;
        }

        public void OnMouseMove(ICanvas canvas, UnitPoint point) { }

        void ClearAll()
        {
            try
            {
                foreach (LinePoints p in m_originalLines.Values)
                    p.Line.Highlighted = false;
                m_originalLines.Clear();
            }
            catch (Exception ex)
            { throw ex; }
        }

        void AddLine(UnitPoint point, DrawTools.LineTool line)
        {
            try
            {
                if (m_originalLines.ContainsKey(line) == false)
                {
                    line.Highlighted = true;
                    LinePoints lp = new LinePoints();
                    lp.SetLine(line);
                    lp.MousePoint = point;
                    m_originalLines.Add(line, lp);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        void RemoveLine(DrawTools.LineTool line)
        {
            try
            {
                if (m_originalLines.ContainsKey(line))
                {
                    m_originalLines[line].Line.Highlighted = false;
                    m_originalLines.Remove(line);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void SetHitObjects(UnitPoint point, List<IDrawObject> list)
        {
            try
            {
                if (list == null)
                    return;
                List<DrawTools.LineTool> lines = GetLines(list);
                if (lines.Count == 0)
                    return;

                bool shift = Control.ModifierKeys == Keys.Shift;
                bool ctrl = Control.ModifierKeys == Keys.Control;

                if (shift == false && ctrl == false)
                    ClearAll();

                if (ctrl == false)
                {
                    foreach (DrawTools.LineTool line in lines)
                    { AddLine(point, line); }
                }
                if (ctrl)
                {
                    foreach (DrawTools.LineTool line in lines)
                    {
                        if (m_originalLines.ContainsKey(line))
                            RemoveLine(line);
                        else
                            AddLine(point, line);
                    }
                }
                SetSelectHint();
            }
            catch (Exception ex)
            { throw ex; }
        }

        void SetSelectHint()
        {
            if (m_originalLines.Count == 0)
            { SetHint("选择延伸的直线"); }
            else
            { SetHint("Control+单击可以多选"); }
        }

        List<DrawTools.LineTool> GetLines(List<IDrawObject> objs)
        {
            try
            {
                List<DrawTools.LineTool> lines = new List<DrawTools.LineTool>();
                foreach (IDrawObject obj in objs)
                {
                    if (obj is DrawTools.LineTool)
                    { lines.Add((DrawTools.LineTool)obj); }
                }
                return lines;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        {
            try
            {
                List<IDrawObject> drawitems = canvas.DataModel.GetHitObjects(canvas, point);
                List<DrawTools.LineTool> lines = GetLines(drawitems);
                if (m_originalLines.Count == 0 || Control.ModifierKeys == Keys.Shift)
                {
                    foreach (DrawTools.LineTool line in lines)
                    { AddLine(point, line); }
                    SetSelectHint();
                    return eDrawObjectMouseDownEnum.Continue;
                }
                if (m_originalLines.Count == 0 || Control.ModifierKeys == Keys.Control)
                {
                    foreach (DrawTools.LineTool line in lines)
                    {
                        if (m_originalLines.ContainsKey(line))
                        { RemoveLine(line); }
                        else
                        { AddLine(point, line); }
                    }
                    SetSelectHint();
                    return eDrawObjectMouseDownEnum.Continue;
                }

                if (drawitems.Count == 0)
                { return eDrawObjectMouseDownEnum.Continue; }

                if (drawitems[0] is DrawTools.LineTool)
                {
                    DrawTools.LineTool edge = (DrawTools.LineTool)drawitems[0];
                    if (edge.Type == DrawTools.LineType.PointLine) { return eDrawObjectMouseDownEnum.Done; }
                    bool modified = false;
                    foreach (LinePoints originalLp in m_originalLines.Values)
                    {
                        UnitPoint intersectpoint = HitUtil.LinesIntersectPoint(edge.P1, edge.P2, originalLp.Line.P1, originalLp.Line.P2);
                        if (intersectpoint != UnitPoint.Empty)
                        {
                            LinePoints lp = new LinePoints();
                            lp.SetLine(originalLp.Line);
                            lp.MousePoint = originalLp.MousePoint;
                            m_modifiedLines.Add(lp.Line, lp);
                            lp.SetNewPoints(lp.Line, lp.MousePoint, intersectpoint);
                            modified = true;
                            continue;
                        }
                        if (intersectpoint == UnitPoint.Empty)
                        {
                            UnitPoint apprarentISPoint = HitUtil.FindApparentIntersectPoint(
                                edge.P1,
                                edge.P2,
                                originalLp.Line.P1,
                                originalLp.Line.P2,
                                false,
                                true);
                            if (apprarentISPoint == UnitPoint.Empty)
                            { continue; }

                            modified = true;
                            originalLp.Line.ExtendLineToPoint(apprarentISPoint);

                            LinePoints lp = new LinePoints();
                            lp.SetLine(originalLp.Line);
                            lp.MousePoint = point;
                            m_modifiedLines.Add(lp.Line, lp);
                        }
                    }
                    if (modified)
                    { canvas.DataModel.AfterEditObjects(this); }
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
                foreach (LinePoints originalLp in m_originalLines.Values)
                { originalLp.Line.Highlighted = false; }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Undo()
        {
            try
            {
                foreach (LinePoints lp in m_originalLines.Values)
                { lp.ResetLine(); }
            }
            catch (Exception ex)
            { throw ex; }
        }
        public void Redo()
        {
            try
            {
                foreach (LinePoints lp in m_modifiedLines.Values)
                { lp.ResetLine(); }
            }catch(Exception ex)
            { throw ex; }
        }
    }
}
