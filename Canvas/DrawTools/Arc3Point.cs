using Canvas.CanvasInterfaces;
using Canvas.Layers;
using Canvas.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.DrawTools
{
    public class Arc3Point : DrawObjectBase, IArc, IDrawObject, ISerialize
    {
        #region 属性
        public enum eArcType
        {
            kArc3P132,
            kArc3P123,
        }
        eArcType m_type = eArcType.kArc3P132;
        public enum eDirection
        {
            kCW,
            kCCW,
        }
        UnitPoint m_p1 = UnitPoint.Empty;
        UnitPoint m_p2 = UnitPoint.Empty;
        UnitPoint m_p3 = UnitPoint.Empty;
        protected static int ThresholdPixel = 6;

        [XmlSerializable]
        public UnitPoint P1
        {
            get { return m_p1; }
            set { m_p1 = value; }
        }

        [XmlSerializable]
        public UnitPoint P2
        {
            get { return m_p2; }
            set { m_p2 = value; }
        }

        [XmlSerializable]
        public UnitPoint P3
        {
            get { return m_p3; }
            set { m_p3 = value; }
        }

        UnitPoint m_center;
        float m_radius;
        float m_startAngle = 0;
        float m_endAngle = 0;
        [XmlSerializable]
        public UnitPoint Center
        {
            get { return m_center; }
            set { m_center = value; }
        }

        [Browsable(false)]
        [XmlSerializable]
        public float Radius
        {
            get { return m_radius; }
            set { m_radius = value; }
        }
        [Browsable(false)]
        [XmlSerializable]
        public float StartAngle
        {
            get { return m_startAngle; }
            set { m_startAngle = value; }
        }
        [Browsable(false)]
        [XmlSerializable]
        public float EndAngle
        {
            get { return m_endAngle; }
            set { m_endAngle = value; }
        }
        eDirection m_direction = eDirection.kCCW;

        [XmlSerializable]
        [Browsable(false)]
        public eDirection Direction
        {
            get { return m_direction; }
            set { m_direction = value; }
        }
        [XmlSerializable]
        public static string ObjectType
        {
            get { return "arc3p"; }
        }
        [Browsable(false)]
        public eCurrentPoint CurrentPoint
        {
            get { return m_curPoint; }
            set { m_curPoint = value; }
        }
        protected eCurrentPoint m_curPoint = eCurrentPoint.done;
        protected UnitPoint m_lastPoint = UnitPoint.Empty;
        #endregion

        #region 函数方法
        public Arc3Point()
        { }

        public Arc3Point(eArcType type)
        {
            m_type = type;
            m_curPoint = eCurrentPoint.p1;
        }

        public override void InitializeFromModel(UnitPoint point, DrawingLayer layer, ISnapPoint snap)
        {
            try
            {
                Width = layer.Width;
                Color = layer.Color;
                Selected = true;
                OnMouseDown(null, point, snap);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Copy(Arc3Point acopy)
        {
            try
            {
                base.Copy(acopy);
                m_p1 = acopy.m_p1;
                m_p2 = acopy.m_p2;
                m_p3 = acopy.m_p3;
                m_center = acopy.m_center;
                m_radius = acopy.m_radius;
                m_startAngle = acopy.m_startAngle;
                m_endAngle = acopy.m_endAngle;
                m_direction = acopy.m_direction;
                //m_curPoint = acopy.m_curPoint;
            }
            catch (Exception ex)
            { throw ex; }
        }

        private double GetSweep(double start, double end, eDirection direction)
        {
            double sweep = 360;
            if (start == end)
                return sweep;
            if (direction == eDirection.kCCW)
            {
                if (end >= start)
                    sweep = end - start;
                else
                    sweep = 360 - (start - end);
            }
            if (direction == eDirection.kCW)
            {
                if (end >= start)
                    sweep = -(360 - (end - start));
                else
                    sweep = -(start - end);
            }
            return sweep;
        }

        [Browsable(false)]
        public float SweepAngle
        {
            get { return (float)GetSweep(StartAngle, EndAngle, Direction); }
        }
        #endregion

        #region IDrawObject Members
        [Browsable(false)]
        public virtual string Id
        {
            get { return ObjectType; }
        }
        public virtual IDrawObject Clone()
        {
            Arc3Point a = new Arc3Point(m_type);
            a.Copy(this);
            return a;
        }

        public RectangleF GetBoundingRect(ICanvas canvas)
        {
            try
            {
                float thWidth = LineTool.ThresholdWidth(canvas, Width, ThresholdPixel);
                if (thWidth < Width)
                { thWidth = Width; }

                RectangleF rect = RectangleF.Empty;
                if (m_p2.IsEmpty || m_p3.IsEmpty)
                { rect = ScreenUtils.GetRect(m_p1, m_lastPoint, thWidth); }

                if (rect.IsEmpty)
                {
                    float r = m_radius + thWidth / 2;
                    rect = HitUtil.CircleBoundingRect(m_center, r);
                    if (Selected)
                    {
                        float w = (float)canvas.ToUnit(20);
                        rect.Inflate(w, w);
                    }
                }
                if (m_lastPoint.IsEmpty == false)
                { rect = RectangleF.Union(rect, new RectangleF(m_lastPoint.Point, new SizeF(0, 0))); }
                return rect;
            }
            catch (Exception ex)
            { throw ex; }
        }

        private void DrawArc(ICanvas canvas, RectangleF unitrect)
        {
            try
            {
                Pen pen = new Pen(Color, Width);
                bool inline = PointsInLine();
                double sweep = GetSweep(StartAngle, EndAngle, Direction);

                if (inline == false)
                { canvas.DrawArc(canvas, pen, m_center, m_radius, StartAngle, (float)sweep); }
                else
                {
                    canvas.DrawLine(canvas, pen, P1, P2);
                    canvas.DrawLine(canvas, pen, P1, P3);
                }

                if (Selected)
                {
                    if (inline == false)
                    { canvas.DrawArc(canvas, DrawUtils.SelectedPen, m_center, m_radius, StartAngle, (float)sweep); }
                    else
                    {
                        canvas.DrawLine(canvas, DrawUtils.SelectedPen, P1, P2);
                        canvas.DrawLine(canvas, DrawUtils.SelectedPen, P1, P3);
                    }
                    if (m_p1.IsEmpty == false)
                    {
                        DrawUtils.DrawNode(canvas, P1);
                        UnitPoint anglepoint = StartAngleNodePoint(canvas);
                        if (!anglepoint.IsEmpty)
                            DrawUtils.DrawTriangleNode(canvas, anglepoint);
                        anglepoint = EndAngleNodePoint(canvas);
                        if (!anglepoint.IsEmpty)
                            DrawUtils.DrawTriangleNode(canvas, anglepoint);
                        anglepoint = RadiusNodePoint(canvas);
                        if (!anglepoint.IsEmpty)
                            DrawUtils.DrawTriangleNode(canvas, anglepoint);
                    }
                    if (m_p2.IsEmpty == false)
                        DrawUtils.DrawNode(canvas, P2);
                    if (m_p3.IsEmpty == false)
                        DrawUtils.DrawNode(canvas, P3);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        bool PointsInLine()
        {
            try
            {
                double slope1 = HitUtil.LineSlope(P1, P2);
                double slope2 = HitUtil.LineSlope(P1, P3);
                return slope1 == slope2;
            }
            catch (Exception ex)
            { throw ex; }
        }

        private void Draw3P132(ICanvas canvas, RectangleF unitrect)
        {
            try
            {
                if (m_curPoint == eCurrentPoint.p3)
                {
                    Pen pen = new Pen(Color, Width);
                    canvas.DrawLine(canvas, pen, m_p1, m_p3);
                }
                if (m_curPoint == eCurrentPoint.p2 || m_curPoint == eCurrentPoint.done)
                {
                    DrawArc(canvas, unitrect);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        void Draw3P123(ICanvas canvas, RectangleF unitrect)
        {
            try
            {
                if (m_curPoint == eCurrentPoint.p2)
                {
                    Pen pen = new Pen(Color, Width);
                    canvas.DrawLine(canvas, pen, m_p1, m_p2);
                }
                if (m_curPoint == eCurrentPoint.p3 || m_curPoint == eCurrentPoint.done)
                {
                    DrawArc(canvas, unitrect);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }


        public virtual void Draw(ICanvas canvas, RectangleF unitrect)
        {
            try
            {
                if (m_type == eArcType.kArc3P132)
                { Draw3P132(canvas, unitrect); }
                if (m_type == eArcType.kArc3P123)
                { Draw3P123(canvas, unitrect); }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void UpdateCenterNodeFromAngles()
        {
            try
            {
                float angle = StartAngle + SweepAngle / 2;
                P2 = HitUtil.PointOncircle(m_center, m_radius, HitUtil.DegressToRadians(angle));
            }
            catch (Exception ex)
            { throw ex; }
        }


        public void UpdateArcFrom3Points()
        {
            try
            {
                m_center = HitUtil.CenterPointFrom3Points(m_p1, m_p2, m_p3);
                m_radius = (float)HitUtil.Distance(m_center, m_p1);
                StartAngle = (float)HitUtil.RadiansToDegrees(HitUtil.LineAngleR(m_center, m_p1, 0));
                EndAngle = (float)HitUtil.RadiansToDegrees(HitUtil.LineAngleR(m_center, m_p3, 0));
                double p1p3angle = HitUtil.RadiansToDegrees(HitUtil.LineAngleR(m_p1, m_p3, 0));
                double p1p2angle = HitUtil.RadiansToDegrees(HitUtil.LineAngleR(m_p1, m_p2, 0));
                double diff = p1p3angle - p1p2angle;
                Direction = eDirection.kCCW;
                if (diff < 0 || diff > 180)
                { Direction = eDirection.kCW; }

                if (p1p3angle == 0)
                {
                    if (diff < -180)
                    { Direction = eDirection.kCCW; }
                    else
                    { Direction = eDirection.kCW; }
                }
                if (p1p3angle == 90)
                {
                    if (diff < -180)
                    { Direction = eDirection.kCCW; }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }


        private void MoveMouse3P132(ICanvas canvas, UnitPoint point)
        {
            try
            {
                if (m_curPoint == eCurrentPoint.p1)
                {
                    m_p1 = point;
                    return;
                }
                if (m_curPoint == eCurrentPoint.p3)
                {
                    m_p3 = point;
                    return;
                }
                if (m_curPoint == eCurrentPoint.p2)
                {
                    m_p2 = point;
                    UpdateArcFrom3Points();
                   // m_curPoint = eCurrentPoint.done;
                    return;
                }
            }
            catch (Exception ex)
            { throw ex; }
        }


        private void MoveMouse3P123(ICanvas canvas, UnitPoint point)
        {
            try
            {
                if (m_curPoint == eCurrentPoint.p1)
                {
                    m_p1 = point;
                    return;
                }
                if (m_curPoint == eCurrentPoint.p2)
                {
                    m_p2 = point;
                    return;
                }
                if (m_curPoint == eCurrentPoint.p3)
                {
                    m_p3 = point;
                    UpdateArcFrom3Points();
                    //m_curPoint = eCurrentPoint.done;
                    return;
                }
            }
            catch (Exception ex)
            { throw ex; }
        }


        private eDrawObjectMouseDownEnum MouseDown3P132(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        {
            try
            {
                OnMouseMove(canvas, point);
                if (m_curPoint == eCurrentPoint.p1)
                {
                    m_curPoint = eCurrentPoint.p3;
                    return eDrawObjectMouseDownEnum.Continue;
                }
                if (m_curPoint == eCurrentPoint.p3)
                {
                    m_curPoint = eCurrentPoint.p2;
                    return eDrawObjectMouseDownEnum.Continue;
                }
                if (m_curPoint == eCurrentPoint.p2)
                {
                    m_curPoint = eCurrentPoint.done;
                    Selected = false;
                    return eDrawObjectMouseDownEnum.Done;
                }
                return eDrawObjectMouseDownEnum.Done;
            }
            catch (Exception ex)
            { throw ex; }
        }


        private eDrawObjectMouseDownEnum MouseDown3P123(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        {
            try
            {
                OnMouseMove(canvas, point);
                if (m_curPoint == eCurrentPoint.p1)
                {
                    m_curPoint = eCurrentPoint.p2;
                    return eDrawObjectMouseDownEnum.Continue;
                }
                if (m_curPoint == eCurrentPoint.p2)
                {
                    m_curPoint = eCurrentPoint.p3;
                    return eDrawObjectMouseDownEnum.Continue;
                }
                if (m_curPoint == eCurrentPoint.p3)
                {
                    m_curPoint = eCurrentPoint.done;
                    Selected = false;
                    return eDrawObjectMouseDownEnum.Done;
                }
                return eDrawObjectMouseDownEnum.Done;
            }
            catch (Exception ex)
            { throw ex; }
        }

        private UnitPoint StartAngleNodePoint(ICanvas canvas)
        {
            try
            {
                double r = Radius + canvas.ToUnit(8);
                return HitUtil.PointOncircle(m_center, r, HitUtil.DegressToRadians(StartAngle));
            }
            catch (Exception ex)
            { throw ex; }
        }

        private UnitPoint EndAngleNodePoint(ICanvas canvas)
        {
            try
            {
                double r = Radius + canvas.ToUnit(8);
                return HitUtil.PointOncircle(m_center, r, HitUtil.DegressToRadians(EndAngle));
            }
            catch (Exception ex)
            { throw ex; }
        }

        private UnitPoint RadiusNodePoint(ICanvas canvas)
        {
            try
            {
                double r = Radius + canvas.ToUnit(8);
                float angle = StartAngle + SweepAngle / 2;
                return HitUtil.PointOncircle(m_center, r, HitUtil.DegressToRadians(angle));
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual void OnMouseMove(ICanvas canvas, UnitPoint point)
        {
            try
            {
                m_lastPoint = point;
                if (m_type == eArcType.kArc3P132)
                { MoveMouse3P132(canvas, point); }
                if (m_type == eArcType.kArc3P123)
                { MoveMouse3P123(canvas, point); }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        {
            try
            {
                if (m_type == eArcType.kArc3P132)
                    return MouseDown3P132(canvas, point, snappoint);
                if (m_type == eArcType.kArc3P123)
                    return MouseDown3P123(canvas, point, snappoint);
                return eDrawObjectMouseDownEnum.Done;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual void OnMouseUp(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        { }

        public virtual void OnKeyDown(ICanvas canvas, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.D)
                {
                    if (Direction == eDirection.kCW)
                        Direction = eDirection.kCCW;
                    else
                        Direction = eDirection.kCW;
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual bool PointInObject(ICanvas canvas, UnitPoint point)
        {
            try
            {
                RectangleF boundingrect = GetBoundingRect(canvas);
                if (boundingrect.Contains(point.Point) == false)
                    return false;
                float thWidth = LineTool.ThresholdWidth(canvas, Width, ThresholdPixel);
                if (HitUtil.PointInPoint(m_center, point, thWidth))
                    return true;
                return HitUtil.IsPointInCircle(m_center, m_radius, point, thWidth / 2);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual bool ObjectInRectangle(ICanvas canvas, RectangleF rect, bool anyPoint)
        {
            try
            {
                float r = m_radius + Width / 2;
                RectangleF boundingrect = HitUtil.CircleBoundingRect(m_center, r);
                if (anyPoint)
                {
                    UnitPoint lp1 = new UnitPoint(rect.Left, rect.Top);
                    UnitPoint lp2 = new UnitPoint(rect.Left, rect.Bottom);
                    if (HitUtil.CircleIntersectWithLine(m_center, m_radius, lp1, lp2))
                    { return true; }
                    lp1 = new UnitPoint(rect.Left, rect.Bottom);
                    lp2 = new UnitPoint(rect.Right, rect.Bottom);
                    if (HitUtil.CircleIntersectWithLine(m_center, m_radius, lp1, lp2))
                    { return true; }
                    lp1 = new UnitPoint(rect.Left, rect.Top);
                    lp2 = new UnitPoint(rect.Right, rect.Top);
                    if (HitUtil.CircleIntersectWithLine(m_center, m_radius, lp1, lp2))
                    { return true; }
                    lp1 = new UnitPoint(rect.Left, rect.Top);
                    lp2 = new UnitPoint(rect.Right, rect.Top);
                    if (HitUtil.CircleIntersectWithLine(m_center, m_radius, lp1, lp2))
                        return true;
                    lp1 = new UnitPoint(rect.Right, rect.Top);
                    lp2 = new UnitPoint(rect.Right, rect.Bottom);
                    if (HitUtil.CircleIntersectWithLine(m_center, m_radius, lp1, lp2))
                    { return true; }
                }
                return rect.Contains(boundingrect);
            }
            catch (Exception ex)
            { throw ex; }
        }

        [Browsable(false)]
        public virtual UnitPoint RepeatStartingPoint
        {
            get { return UnitPoint.Empty; }
        }

        public virtual INodePoint NodePoint(ICanvas canvas, UnitPoint point)
        {
            try
            {
                //float thWidth = LineTool.ThresholdWidth(canvas, Width, ThresholdPixel);
                //if (HitUtil.PointInPoint(P1, point, thWidth))
                //{
                //    m_lastPoint = P1;
                //    return new NodePointArc3PointPoint(this, eCurrentPoint.p1);
                //}
                //if (HitUtil.PointInPoint(P2, point, thWidth))
                //{
                //    m_lastPoint = P2;
                //    return new NodePointArc3PointPoint(this, eCurrentPoint.p2);
                //}
                //if (HitUtil.PointInPoint(P3, point, thWidth))
                //{
                //    m_lastPoint = P3;
                //    return new NodePointArc3PointPoint(this, eCurrentPoint.p3);
                //}
                //UnitPoint p = StartAngleNodePoint(canvas);
                //if (HitUtil.PointInPoint(p, point, thWidth))
                //{
                //    m_lastPoint = p;
                //    return new NodePointArc3PointPoint(this, eCurrentPoint.startangle);
                //}
                //p = EndAngleNodePoint(canvas);
                //if (HitUtil.PointInPoint(p, point, thWidth))
                //{
                //    m_lastPoint = p;
                //    return new NodePointArc3PointPoint(this, eCurrentPoint.endangle);
                //}
                //p = RadiusNodePoint(canvas);
                //if (HitUtil.PointInPoint(p, point, thWidth))
                //{
                //    m_lastPoint = p;
                //    return new NodePointArc3PointPoint(this, eCurrentPoint.radius);
                //}
                return null;
            }
            catch (Exception ex)
            { throw ex; }
        }


        public virtual ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, List<IDrawObject> otherobj, Type[] runningsnaptypes, Type usersnaptype)
        {
            try
            {
                //float thWidth = LineTool.ThresholdWidth(canvas, Width, ThresholdPixel);
                //if (runningsnaptypes != null)
                //{
                //    foreach (Type snaptype in runningsnaptypes)
                //    {
                //        if (snaptype == typeof(VertextSnapPoint))
                //        {
                //            if (HitUtil.PointInPoint(P1, point, thWidth))
                //            { return new VertextSnapPoint(canvas, this, P1); }
                //            if (HitUtil.PointInPoint(P3, point, thWidth))
                //            { return new VertextSnapPoint(canvas, this, P3); }
                //        }
                //    }
                //    return null;
                //}
                //if (usersnaptype == typeof(NearestSnapPoint))
                //{
                //    UnitPoint p = HitUtil.NearestPointOnCircle(m_center, m_radius, point, 0);
                //    if (p != UnitPoint.Empty)
                //    { return new NearestSnapPoint(canvas, this, p); }
                //}
                //if (usersnaptype == typeof(PerpendicularSnapPoint))
                //{
                //    UnitPoint p = HitUtil.NearestPointOnCircle(m_center, m_radius, point, 0);
                //    if (p != UnitPoint.Empty)
                //    { return new PerpendicularSnapPoint(canvas, this, p); }
                //}
                //if (usersnaptype == typeof(QuadrantSnapPoint))
                //{
                //    UnitPoint p = HitUtil.NearestPointOnCircle(m_center, m_radius, point, 90);
                //    if (p != UnitPoint.Empty)
                //    { return new QuadrantSnapPoint(canvas, this, p); }
                //}
                //if (usersnaptype == typeof(TangentSnapPoint))
                //{
                //    IDrawObject drawingObject = canvas.CurrentObject;
                //    UnitPoint p = UnitPoint.Empty;
                //    if (drawingObject is LineEdit)
                //    {
                //        UnitPoint mousepoint = point;
                //        point = ((LineEdit)drawingObject).P1;
                //        UnitPoint p1 = HitUtil.TangentPointOnCircle(m_center, m_radius, point, false);
                //        UnitPoint p2 = HitUtil.TangentPointOnCircle(m_center, m_radius, point, true);
                //        double d1 = HitUtil.Distance(mousepoint, p1);
                //        double d2 = HitUtil.Distance(mousepoint, p2);
                //        if (d1 <= d2)
                //        { return new TangentSnapPoint(canvas, this, p1); }
                //        else
                //        { return new TangentSnapPoint(canvas, this, p2); }
                //    }
                //    return new TangentSnapPoint(canvas, this, p);
                //}
                //if (usersnaptype == typeof(CenterSnapPoint))
                //{
                //    return new CenterSnapPoint(canvas, this, m_center);
                //}
                return null;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual void Move(UnitPoint offset)
        {
            try
            {
                P1 += offset;
                P2 += offset;
                P3 += offset;
                UpdateArcFrom3Points();
                m_lastPoint = m_center;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual string GetInfoAsString()
        {
            return string.Format("Arc@{0}, r={1:f4}, A1={2:f4}, A2={3:f4}", Center.PosAsString(), Radius, StartAngle, EndAngle);
        }
        #endregion

        #region ISerialize
        public virtual void GetObjectData(XmlWriter wr)
        {
            try
            {
                wr.WriteStartElement(Id);
                XmlUtil.WriteProperties(this, wr);
                wr.WriteEndElement();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void AfterSerializedIn()
        {
            try
            {
                UpdateArcFrom3Points();
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
    }
}
