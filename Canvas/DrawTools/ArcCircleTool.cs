using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.DrawTools
{
    interface IArc
    {
        UnitPoint Center { get; }
        float Radius { get; }
        float StartAngle { get; }
        float EndAngle { get; }
    }
    public class NodePointArcCenter : INodePoint
    {
        protected Arc m_owner;
        protected Arc m_clone;
        protected UnitPoint m_originalPoint;
        protected UnitPoint m_endPoint;
        public NodePointArcCenter(Arc owner)
        {
            m_owner = owner;
            m_clone = m_owner.Clone() as Arc;
            m_originalPoint = m_owner.Center;
        }
        #region INodePoint Members
        public IDrawObject GetClone()
        {
            return m_clone;
        }
        public IDrawObject GetOriginal()
        {
            return m_owner;
        }
        public virtual void SetPosition(UnitPoint pos)
        {
            m_clone.Center = pos;
        }
        public virtual void Finish()
        {
            m_endPoint = m_clone.Center;
            m_owner.Center = m_clone.Center;
            m_owner.Radius = m_clone.Radius;
            m_owner.Selected = true;
            m_clone = null;
        }
        public void Cancel()
        {
            m_owner.Selected = true;
        }
        public virtual void Undo()
        {
            m_owner.Center = m_originalPoint;
        }
        public virtual void Redo()
        {
            m_owner.Center = m_endPoint;
        }
        public void OnKeyDown(ICanvas canvas, KeyEventArgs e)
        {
        }
        #endregion
    }
    public class NodePointArcRadius : INodePoint
    {
        protected Arc m_owner;
        protected Arc m_clone;
        protected float m_originalValue;
        protected float m_endValue;
        public NodePointArcRadius(Arc owner)
        {
            m_owner = owner;
            m_clone = m_owner.Clone() as Arc;
            m_clone.CurrentPoint = m_owner.CurrentPoint;
            m_originalValue = m_owner.Radius;
        }
        #region INodePoint Members
        public IDrawObject GetClone()
        {
            return m_clone;
        }
        public IDrawObject GetOriginal()
        {
            return m_owner;
        }
        public virtual void SetPosition(UnitPoint pos)
        {
            m_clone.OnMouseMove(null, pos);
        }
        public virtual void Finish()
        {
            m_endValue = m_clone.Radius;
            m_owner.Radius = m_clone.Radius;
            m_owner.Selected = true;
            m_clone = null;
        }
        public void Cancel()
        {
            m_owner.Selected = true;
        }
        public virtual void Undo()
        {
            m_owner.Radius = m_originalValue;
        }
        public virtual void Redo()
        {
            m_owner.Radius = m_endValue;
        }
        public void OnKeyDown(ICanvas canvas, KeyEventArgs e)
        {
        }
        #endregion
    }
    class NodePointArcAngle : INodePoint
    {
        protected Arc m_owner;
        protected Arc m_clone;
        protected float m_originalA1;
        protected float m_endA1;
        protected float m_originalA2;
        protected float m_endA2;
        public NodePointArcAngle(Arc owner)
        {
            m_owner = owner;
            m_clone = m_owner.Clone() as Arc;
            m_clone.CurrentPoint = m_owner.CurrentPoint;
            m_originalA1 = m_owner.StartAngle;
            m_originalA2 = m_owner.EndAngle;
            m_owner.Selected = false;
        }
        #region INodePoint Members
        public IDrawObject GetClone()
        {
            return m_clone;
        }
        public IDrawObject GetOriginal()
        {
            return m_owner;
        }
        public virtual void SetPosition(UnitPoint pos)
        {
            m_clone.OnMouseMove(null, pos);
        }
        public virtual void Finish()
        {
            m_endA1 = m_clone.StartAngle;
            m_endA2 = m_clone.EndAngle;
            m_owner.Copy(m_clone);
            m_clone = null;
        }
        public void Cancel()
        {
            m_owner.Selected = true;
        }
        public virtual void Undo()
        {
            m_owner.StartAngle = m_originalA1;
            m_owner.EndAngle = m_originalA2;
        }
        public virtual void Redo()
        {
            m_owner.StartAngle = m_endA1;
            m_owner.EndAngle = m_endA2;
        }
        public void OnKeyDown(ICanvas canvas, KeyEventArgs e)
        {
        }
        #endregion
    }

    public class Arc : DrawObjectBase, IArc, IDrawObject, ISerialize
    {
        public enum eDirection
        {
            kCW,
            kCCW,
        }

        UnitPoint m_center;
        float m_radius;
        float m_startAngle = 0;
        float m_endAngle = 0;
        eDirection m_direction = eDirection.kCCW;
        [XmlSerializable]
        public UnitPoint Center
        {
            get { return m_center; }
            set { m_center = value; }
        }
        [XmlSerializable]
        public float Radius
        {
            get { return m_radius; }
            set { m_radius = value; }
        }
        [XmlSerializable]
        public float StartAngle
        {
            get { return m_startAngle; }
            set { m_startAngle = value; }
        }
        [XmlSerializable]
        public float EndAngle
        {
            get { return m_endAngle; }
            set { m_endAngle = value; }
        }
        [XmlSerializable]
        public eDirection Direction
        {
            get { return m_direction; }
            set { m_direction = value; }
        }
        public static string ObjectType
        {
            get { return "arc"; }
        }
        public Arc()
        {
        }
        public Arc(eArcType type)
        {
            m_arcType = type;
            m_curPoint = eCurrentPoint.p1;
            if (m_arcType == eArcType.typeCenterRadius)
                m_curPoint = eCurrentPoint.center;
        }
        public override void InitializeFromModel(UnitPoint point, DrawingLayer layer, ISnapPoint snap)
        {
            Width = layer.Width;
            Color = layer.Color;
            OnMouseDown(null, point, snap);
            Selected = true;
        }
        public void Copy(Arc acopy)
        {
            base.Copy(acopy);
            Center = acopy.Center;
            Radius = acopy.Radius;
            StartAngle = acopy.StartAngle;
            EndAngle = acopy.EndAngle;
            Selected = acopy.Selected;
            Direction = acopy.Direction;
            m_arcType = acopy.m_arcType;
            m_curPoint = acopy.m_curPoint;
        }
        #region IDrawObject Members
        public virtual string Id
        {
            get { return ObjectType; }
        }
        public virtual IDrawObject Clone()
        {
            Arc a = new Arc();
            a.Copy(this);
            return a;
        }
        public float SweepAngle
        {
            get
            {
                float sweep = 360;
                if (StartAngle == EndAngle)
                    return sweep;
                if (Direction == eDirection.kCCW)
                {
                    if (EndAngle >= StartAngle)
                        sweep = EndAngle - StartAngle;
                    else
                        sweep = 360 - (StartAngle - EndAngle);
                }
                if (Direction == eDirection.kCW)
                {
                    if (EndAngle >= StartAngle)
                        sweep = -(360 - (EndAngle - StartAngle));
                    else
                        sweep = -(StartAngle - EndAngle);
                }
                return sweep;
            }
        }
        public RectangleF GetBoundingRect(ICanvas canvas)
        {
            float thWidth = Line.ThresholdWidth(canvas, Width, ThresholdPixel);
            if (thWidth < Width)
                thWidth = Width;
            float r = m_radius + thWidth / 2;
            RectangleF rect = HitUtil.CircleBoundingRect(m_center, r);
            // if drawing either angle then include the mouse point in the ractangle - this is to redraw (erase) the line drawn
            // from center point to mouse point
            if (m_curPoint == eCurrentPoint.startAngle || m_curPoint == eCurrentPoint.endAngle)
                rect = RectangleF.Union(rect, new RectangleF(m_lastPoint.Point, new SizeF(0, 0)));
            return rect;
        }
        public virtual void Draw(ICanvas canvas, RectangleF unitrect)
        {
            float sweep = SweepAngle;
            Pen pen = canvas.CreatePen(Color);
            canvas.DrawArc(canvas, pen, m_center, m_radius, StartAngle, sweep);
            if (Selected)
            {
                canvas.DrawArc(canvas, DrawUtils.SelectedPen, m_center, m_radius, StartAngle, sweep);
                DrawUtils.DrawNode(canvas, m_center);
                DrawNodes(canvas);
            }
        }
        public virtual void OnMouseMove(ICanvas canvas, UnitPoint point)
        {
            m_lastPoint = point;
            if (m_curPoint == eCurrentPoint.p1)
            {
                m_p1 = point;
                return;
            }
            if (m_curPoint == eCurrentPoint.p2)
            {
                StartAngle = 0;
                EndAngle = 360;
                m_center = HitUtil.LineMidpoint(m_p1, point);
                m_radius = (float)HitUtil.Distance(m_center, point);
                return;
            }
            if (m_curPoint == eCurrentPoint.center)
            {
                m_center = point;
            }
            if (m_curPoint == eCurrentPoint.radius)
            {
                //StartAngle = 0;
                //EndAngle = 360;
                m_radius = (float)HitUtil.Distance(m_center, point);
            }

            double angleToRound = 0;
            if (Control.ModifierKeys == Keys.Control)
                angleToRound = HitUtil.DegressToRadians(45);

            if (m_curPoint == eCurrentPoint.startAngle)
            {
                StartAngle = (float)HitUtil.RadiansToDegrees(HitUtil.LineAngleR(m_center, point, angleToRound));
            }
            if (m_curPoint == eCurrentPoint.endAngle)
            {
                EndAngle = (float)HitUtil.RadiansToDegrees(HitUtil.LineAngleR(m_center, point, angleToRound));
            }
        }
        public virtual eDrawObjectMouseDown OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        {
            OnMouseMove(canvas, point);
            if (m_arcType == eArcType.type2point)
            {
                if (m_curPoint == eCurrentPoint.p1)
                {
                    m_curPoint = eCurrentPoint.p2;
                    return eDrawObjectMouseDown.Continue;
                }
                if (m_curPoint == eCurrentPoint.p2)
                {
                    m_curPoint = eCurrentPoint.startAngle;
                    return eDrawObjectMouseDown.Continue;
                }
                if (m_curPoint == eCurrentPoint.startAngle)
                {
                    m_curPoint = eCurrentPoint.endAngle;
                    return eDrawObjectMouseDown.Continue;
                }
                if (m_curPoint == eCurrentPoint.endAngle)
                {
                    m_curPoint = eCurrentPoint.done;
                    OnMouseMove(canvas, point);
                    Selected = false;
                    return eDrawObjectMouseDown.Done;
                }
            }
            if (m_arcType == eArcType.typeCenterRadius)
            {
                if (m_curPoint == eCurrentPoint.center)
                {
                    m_curPoint = eCurrentPoint.radius;
                    return eDrawObjectMouseDown.Continue;
                }
                if (m_curPoint == eCurrentPoint.radius)
                {
                    m_curPoint = eCurrentPoint.startAngle;
                    return eDrawObjectMouseDown.Continue;
                }
                if (m_curPoint == eCurrentPoint.startAngle)
                {
                    m_curPoint = eCurrentPoint.endAngle;
                    return eDrawObjectMouseDown.Continue;
                }
                if (m_curPoint == eCurrentPoint.endAngle)
                {
                    m_curPoint = eCurrentPoint.done;
                    OnMouseMove(canvas, point);
                    Selected = false;
                    return eDrawObjectMouseDown.Done;
                }
            }
            return eDrawObjectMouseDown.Done;
        }
        public virtual void OnMouseUp(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        {
        }
        public virtual void OnKeyDown(ICanvas canvas, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D && (m_curPoint == eCurrentPoint.startAngle || m_curPoint == eCurrentPoint.endAngle))
            {
                if (Direction == eDirection.kCW)
                    Direction = eDirection.kCCW;
                else
                    Direction = eDirection.kCW;
                e.Handled = true;
            }
            if (e.KeyCode == Keys.C) // switch to centerRadius mode
            {
                m_center = UnitPoint.Empty;
                m_radius = 0;
                m_startAngle = 0;
                m_endAngle = 0;
                m_arcType = eArcType.typeCenterRadius;
                m_curPoint = eCurrentPoint.center;
                e.Handled = true;
                canvas.Invalidate();
            }
            if (e.KeyCode == Keys.D2) // switch to 2 pointmode
            {
                m_center = UnitPoint.Empty;
                m_radius = 0;
                m_startAngle = 0;
                m_endAngle = 0;
                m_arcType = eArcType.type2point;
                m_curPoint = eCurrentPoint.p1;
                e.Handled = true;
                canvas.Invalidate();
            }
        }
        public virtual bool PointInObject(ICanvas canvas, UnitPoint point)
        {
            RectangleF boundingrect = GetBoundingRect(canvas);
            if (boundingrect.Contains(point.Point) == false)
                return false;
            float thWidth = Line.ThresholdWidth(canvas, Width, ThresholdPixel);
            if (HitUtil.PointInPoint(m_center, point, thWidth))
                return true;
            return HitUtil.IsPointInCircle(m_center, m_radius, point, thWidth / 2);
        }
        public virtual bool ObjectInRectangle(ICanvas canvas, RectangleF rect, bool anyPoint)
        {
            float r = m_radius + Width / 2;
            RectangleF boundingrect = HitUtil.CircleBoundingRect(m_center, r);
            if (anyPoint)
            {
                UnitPoint lp1 = new UnitPoint(rect.Left, rect.Top);
                UnitPoint lp2 = new UnitPoint(rect.Left, rect.Bottom);
                if (HitUtil.CircleIntersectWithLine(m_center, m_radius, lp1, lp2))
                    return true;
                lp1 = new UnitPoint(rect.Left, rect.Bottom);
                lp2 = new UnitPoint(rect.Right, rect.Bottom);
                if (HitUtil.CircleIntersectWithLine(m_center, m_radius, lp1, lp2))
                    return true;
                lp1 = new UnitPoint(rect.Left, rect.Top);
                lp2 = new UnitPoint(rect.Right, rect.Top);
                if (HitUtil.CircleIntersectWithLine(m_center, m_radius, lp1, lp2))
                    return true;
                lp1 = new UnitPoint(rect.Left, rect.Top);
                lp2 = new UnitPoint(rect.Right, rect.Top);
                if (HitUtil.CircleIntersectWithLine(m_center, m_radius, lp1, lp2))
                    return true;

                lp1 = new UnitPoint(rect.Right, rect.Top);
                lp2 = new UnitPoint(rect.Right, rect.Bottom);
                if (HitUtil.CircleIntersectWithLine(m_center, m_radius, lp1, lp2))
                    return true;
            }
            return rect.Contains(boundingrect);
        }
        public virtual UnitPoint RepeatStartingPoint
        {
            get { return UnitPoint.Empty; }
        }
        public virtual INodePoint NodePoint(ICanvas canvas, UnitPoint point)
        {
            float thWidth = Line.ThresholdWidth(canvas, Width, ThresholdPixel);
            if (HitUtil.PointInPoint(m_center, point, thWidth))
                return new NodePointArcCenter(this);
            if (HitUtil.PointInPoint(StartAnglePoint, point, thWidth))
            {
                m_curPoint = eCurrentPoint.startAngle;
                m_lastPoint = m_center;
                return new NodePointArcAngle(this);
            }
            if (HitUtil.PointInPoint(EndAnglePoint, point, thWidth))
            {
                m_curPoint = eCurrentPoint.endAngle;
                m_lastPoint = m_center;
                return new NodePointArcAngle(this);
            }
            if (HitUtil.PointInPoint(RadiusPoint, point, thWidth))
            {
                m_curPoint = eCurrentPoint.radius;
                m_lastPoint = m_center;
                return new NodePointArcRadius(this);
            }
            return null;
        }
        public virtual ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, List<IDrawObject> otherobj, Type[] runningsnaptypes, Type usersnaptype)
        {
            float thWidth = Line.ThresholdWidth(canvas, Width, ThresholdPixel);
            if (runningsnaptypes != null)
            {
                foreach (Type snaptype in runningsnaptypes)
                {
                    if (snaptype == typeof(QuadrantSnapPoint))
                    {
                        UnitPoint p = HitUtil.NearestPointOnCircle(m_center, m_radius, point, 90);
                        if (p != UnitPoint.Empty && HitUtil.PointInPoint(p, point, thWidth))
                            return new QuadrantSnapPoint(canvas, this, p);
                    }
                    if (snaptype == typeof(DivisionSnapPoint))
                    {
                        double angle = 360 / Divisions;
                        UnitPoint p = HitUtil.NearestPointOnCircle(m_center, m_radius, point, angle);
                        if (p != UnitPoint.Empty && HitUtil.PointInPoint(p, point, thWidth))
                            return new DivisionSnapPoint(canvas, this, p);
                    }
                    if (snaptype == typeof(CenterSnapPoint))
                    {
                        if (HitUtil.PointInPoint(m_center, point, thWidth))
                            return new CenterSnapPoint(canvas, this, m_center);
                    }
                    if (snaptype == typeof(VertextSnapPoint))
                    {
                        if (HitUtil.PointInPoint(StartAnglePoint, point, thWidth))
                            return new VertextSnapPoint(canvas, this, StartAnglePoint);
                        if (HitUtil.PointInPoint(EndAnglePoint, point, thWidth))
                            return new VertextSnapPoint(canvas, this, EndAnglePoint);
                    }
                }
                return null;
            }
            if (usersnaptype == typeof(NearestSnapPoint))
            {
                UnitPoint p = HitUtil.NearestPointOnCircle(m_center, m_radius, point, 0);
                if (p != UnitPoint.Empty)
                    return new NearestSnapPoint(canvas, this, p);
            }
            if (usersnaptype == typeof(PerpendicularSnapPoint))
            {
                UnitPoint p = HitUtil.NearestPointOnCircle(m_center, m_radius, point, 0);
                if (p != UnitPoint.Empty)
                    return new PerpendicularSnapPoint(canvas, this, p);
            }
            if (usersnaptype == typeof(QuadrantSnapPoint))
            {
                UnitPoint p = HitUtil.NearestPointOnCircle(m_center, m_radius, point, 90);
                if (p != UnitPoint.Empty)
                    return new QuadrantSnapPoint(canvas, this, p);
            }
            if (usersnaptype == typeof(DivisionSnapPoint))
            {
                double angle = 360 / Divisions;
                UnitPoint p = HitUtil.NearestPointOnCircle(m_center, m_radius, point, angle);
                if (p != UnitPoint.Empty)
                    return new DivisionSnapPoint(canvas, this, p);
            }
            if (usersnaptype == typeof(TangentSnapPoint))
            {
                IDrawObject drawingObject = canvas.CurrentObject;
                UnitPoint p = UnitPoint.Empty;
                if (drawingObject is LineEdit)
                {
                    UnitPoint mousepoint = point;
                    point = ((LineEdit)drawingObject).P1;
                    UnitPoint p1 = HitUtil.TangentPointOnCircle(m_center, m_radius, point, false);
                    UnitPoint p2 = HitUtil.TangentPointOnCircle(m_center, m_radius, point, true);
                    double d1 = HitUtil.Distance(mousepoint, p1);
                    double d2 = HitUtil.Distance(mousepoint, p2);
                    if (d1 <= d2)
                        return new TangentSnapPoint(canvas, this, p1);
                    else
                        return new TangentSnapPoint(canvas, this, p2);
                }
                //if (p != PointF.Empty)
                return new TangentSnapPoint(canvas, this, p);
            }
            if (usersnaptype == typeof(CenterSnapPoint))
            {
                return new CenterSnapPoint(canvas, this, m_center);
            }
            return null;
        }
        public virtual void Move(UnitPoint offset)
        {
            m_center.X += offset.X;
            m_center.Y += offset.Y;
            m_lastPoint = m_center;
        }
        public virtual string GetInfoAsString()
        {
            return string.Format("Arc@{0}, r={1:f4}, A1={2:f4}, A2={3:f4}", Center.PosAsString(), Radius, StartAngle, EndAngle);
        }
        #endregion
        #region ISerialize
        public virtual void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement(Id);
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }
        public void AfterSerializedIn()
        {
        }
        #endregion

        protected static int ThresholdPixel = 6;
        protected static int Divisions = 8;
        public enum eCurrentPoint
        {
            p1,
            p2,
            center,
            radius,
            startAngle,
            endAngle,
            done,
        }
        public eCurrentPoint CurrentPoint
        {
            get { return m_curPoint; }
            set { m_curPoint = value; }
        }
        public enum eArcType
        {
            typeCenterRadius,
            type2point,
        }
        protected virtual void DrawNodes(ICanvas canvas)
        {
            if (m_curPoint == eCurrentPoint.startAngle && m_lastPoint != UnitPoint.Empty)
                canvas.DrawLine(canvas, DrawUtils.SelectedPen, m_center, m_lastPoint);
            if (m_curPoint == eCurrentPoint.endAngle && m_lastPoint != UnitPoint.Empty)
                canvas.DrawLine(canvas, DrawUtils.SelectedPen, m_center, m_lastPoint);

            DrawUtils.DrawNode(canvas, StartAnglePoint);
            DrawUtils.DrawNode(canvas, EndAnglePoint);
            DrawUtils.DrawNode(canvas, RadiusPoint);
        }
        protected UnitPoint AnglePoint(float angle)
        {
            return HitUtil.PointOncircle(m_center, m_radius, HitUtil.DegressToRadians(angle));
        }
        protected UnitPoint RadiusPoint
        {
            get
            {
                float angle = StartAngle + SweepAngle / 2;
                return AnglePoint(angle);
            }
        }
        protected UnitPoint StartAnglePoint
        {
            get { return AnglePoint(StartAngle); }
        }
        protected UnitPoint EndAnglePoint
        {
            get { return AnglePoint(EndAngle); }
        }
        protected eArcType m_arcType = eArcType.type2point;
        protected eCurrentPoint m_curPoint = eCurrentPoint.p1;
        protected UnitPoint m_lastPoint;
        protected UnitPoint m_p1;
    }
    public class Circle : Arc
    {
        public Circle()
            : base()
        {
        }
        public Circle(Arc.eArcType type) : base(type)
        {
        }
        public new static string ObjectType
        {
            get { return "circle"; }
        }
        public override string Id
        {
            get { return ObjectType; }
        }
        public override IDrawObject Clone()
        {
            Circle a = new Circle();
            a.Copy(this);
            return a;
        }
        public override eDrawObjectMouseDown OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        {
            OnMouseMove(canvas, point);
            if (m_arcType == eArcType.type2point)
            {
                if (m_curPoint == eCurrentPoint.p1)
                {
                    m_curPoint = eCurrentPoint.p2;
                    return eDrawObjectMouseDown.Continue;
                }
                if (m_curPoint == eCurrentPoint.p2)
                {
                    m_curPoint = eCurrentPoint.done;
                    OnMouseMove(canvas, point);
                    Selected = false;
                    return eDrawObjectMouseDown.Done;
                }
            }
            if (m_arcType == eArcType.typeCenterRadius)
            {
                if (m_curPoint == eCurrentPoint.center)
                {
                    m_curPoint = eCurrentPoint.radius;
                    return eDrawObjectMouseDown.Continue;
                }
                if (m_curPoint == eCurrentPoint.radius)
                {
                    m_curPoint = eCurrentPoint.done;
                    OnMouseMove(canvas, point);
                    Selected = false;
                    return eDrawObjectMouseDown.Done;
                }
            }
            return eDrawObjectMouseDown.Done;
        }
        protected override void DrawNodes(ICanvas canvas)
        {
            DrawUtils.DrawNode(canvas, AnglePoint(0));
            DrawUtils.DrawNode(canvas, AnglePoint(90));
            DrawUtils.DrawNode(canvas, AnglePoint(180));
            DrawUtils.DrawNode(canvas, AnglePoint(270));
        }
        public override INodePoint NodePoint(ICanvas canvas, UnitPoint point)
        {
            float thWidth = Line.ThresholdWidth(canvas, Width, ThresholdPixel);
            if (HitUtil.PointInPoint(Center, point, thWidth))
                return new NodePointArcCenter(this);
            bool radiushit = HitUtil.PointInPoint(AnglePoint(0), point, thWidth);
            if (radiushit == false)
                radiushit = HitUtil.PointInPoint(AnglePoint(90), point, thWidth);
            if (radiushit == false)
                radiushit = HitUtil.PointInPoint(AnglePoint(180), point, thWidth);
            if (radiushit == false)
                radiushit = HitUtil.PointInPoint(AnglePoint(270), point, thWidth);
            if (radiushit)
            {
                m_curPoint = eCurrentPoint.radius;
                m_lastPoint = Center;
                return new NodePointArcRadius(this);
            }
            return null;
        }
        public override string GetInfoAsString()
        {
            return string.Format("Circle@{0}, r={1:f4}", Center.PosAsString(), Radius);
        }
    }
}
