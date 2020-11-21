//using AGVDAccess;
using AGVDAccess;
using Canvas.CanvasInterfaces;
using Canvas.Layers;
using Canvas.Utils;
using Model.Comoon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.DrawTools
{
    public enum LineType
    {
        PointLine,
        Line,
        Dote,
    }

    public class LineTool : DrawObjectBase, IDrawObject, ISerialize
    {
        #region 属性
        [XmlSerializable]
        [Description("长度")]
        public double Lenth { get; set; }

        [Browsable(false)]
        public bool UseRoute { get; set; }

        protected UnitPoint m_p1, m_p2;
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

        [Browsable(false)]
        public static string ObjectType
        {
            get { return "LineTool"; }
        }

        [Browsable(false)]
        [XmlSerializable]
        public LineType Type { get; set; }

        protected int excutAngle = -1;
        /// <summary>
        /// 强制车头方向
        /// </summary>
        [XmlSerializable]
        [Description("强制车头方向:不强制旋转请设置为:-1")]
        public int ExcuteAngle
        {
            get { return excutAngle; }
            set { excutAngle = value; }
        }

        protected int excutmovdirect = -1;
        /// <summary>
        /// 强制车行进方向
        /// </summary>
        [XmlSerializable]
        [Description("强制行进方向:0前进1后退,不强制请设置为:-1")]
        public int ExcuteMoveDirect
        {
            get { return excutmovdirect; }
            set { excutmovdirect = value; }
        }

        protected int excuteturndirect = -1;
        /// <summary>
        /// 强制车行进方向
        /// </summary>
        [XmlSerializable]
        [Description("强制拐弯方向:0右拐1左拐,不强制请设置为:-1")]
        public int ExcuteTurnDirect
        {
            get { return excuteturndirect; }
            set { excuteturndirect = value; }
        }

        protected int excuteavoidance = -1;
        /// <summary>
        /// 强制避障值
        /// </summary>
        [XmlSerializable]
        [Description("强制避障值(整型):填写相应值,不强制请设置为:-1")]
        public int ExcuteAvoidance
        {
            get { return excuteavoidance; }
            set { excuteavoidance = value; }
        }

        protected int excutespeed = -1;
        /// <summary>
        /// 强制速度
        /// </summary>
        [XmlSerializable]
        [Description("强制速度(整型):填写相应值,不强制请设置为:-1")]
        public int ExcuteSpeed
        {
            get { return excutespeed; }
            set { excutespeed = value; }
        }

        [Browsable(false)]
        public UnitPoint RepeatStartingPoint
        {
            get { return m_p2; }
        }
        #endregion

        protected int planRouteLevel = 0;
        /// <summary>
        /// 线段优先级
        /// </summary>
        [XmlSerializable]
        [Description("线段优先级")]
        public int PlanRouteLevel
        {
            get { return planRouteLevel; }
            set { planRouteLevel = value; }
        }

        #region 方法
        public LineTool(LineType type)
        {
            Type = type;
            UseRoute = false;
        }

        public LineTool() { UseRoute = false;}

        public LineTool(UnitPoint point, UnitPoint endpoint, float width, Color color)
        {
            P1 = point;
            P2 = endpoint;
            Width = width;
            Color = color;
            Selected = false;
            UseRoute = false;
        }


        public override void InitializeFromModel(UnitPoint point, DrawingLayer layer, ISnapPoint snap)
        {
            P1 = P2 = point;
            Width = layer.Width;
            Color = layer.Color;
            Selected = true;
        }

        private static int ThresholdPixel = 6;

        public static float ThresholdWidth(ICanvas canvas, float objectwidth)
        {
            return ThresholdWidth(canvas, objectwidth, ThresholdPixel);
        }

        public static float ThresholdWidth(ICanvas canvas, float objectwidth, float pixelwidth)
        {
            double minWidth = canvas.ToUnit(pixelwidth);
            double width = Math.Max(canvas.ToUnit(objectwidth / 2), minWidth);
            return (float)width;
        }

        public virtual void Copy(LineTool acopy)
        {
            base.Copy(acopy);
            m_p1 = acopy.m_p1;
            m_p2 = acopy.m_p2;
            Selected = acopy.Selected;
            Lenth = acopy.Lenth;
        }
        #endregion

        #region 实现 IDrawObject
        [Browsable(false)]
        public virtual string Id
        {
            get { return ObjectType; }
        }

        public virtual IDrawObject Clone()
        {
            LineTool l = new LineTool(Type);
            l.Copy(this);
            return l;
        }

        public RectangleF GetBoundingRect(ICanvas canvas)
        {
            try
            {
                float thWidth = ThresholdWidth(canvas, Width);
                return ScreenUtils.GetRect(m_p1, m_p2, thWidth);
            }
            catch (Exception ex)
            { throw ex; }
        }

        UnitPoint MidPoint(ICanvas canvas, UnitPoint p1, UnitPoint p2, UnitPoint hitpoint)
        {
            try
            {
                //UnitPoint mid = HitUtil.LineMidpoint(p1, p2);
                //float thWidth = ThresholdWidth(canvas, Width);
                //if (HitUtil.CircleHitPoint(mid, thWidth, hitpoint))
                //    return mid;
                return UnitPoint.Empty;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public bool PointInObject(ICanvas canvas, UnitPoint point)
        {
            try
            {
                float thWidth = ThresholdWidth(canvas, Width);
                return HitUtil.IsPointInLine(m_p1, m_p2, point, thWidth);
                //if (Type == LineType.Line)
                //{ return HitUtil.IsPointInLine(m_p1, m_p2, point, thWidth); }
                //else
                //{
                //    bool PointInLine = HitUtil.IsPointInLine(m_p1, m_p2, point, thWidth);
                //    if(!PointInLine)
                //    {
                //        GraphicsPath path = new GraphicsPath();
                //        float ScrtopY = canvas.ToScreen(m_p2.X);
                //        float ScrtopX = canvas.ToScreen(m_p2.Y);
                //        float ScrRightX = canvas.ToScreen(0.03);
                //        float ScrRightY = canvas.ToScreen(-0.08);
                //        float ScrLeftX = canvas.ToScreen(-0.03);
                //        float ScrLeftY = canvas.ToScreen(-0.08);
                //        path.AddLine(new PointF(ScrtopX, ScrLeftY), new PointF(ScrRightX, ScrRightY));
                //        path.AddLine(new PointF(ScrRightX, ScrRightY), new PointF(ScrtopX, ScrtopY));
                //        path.AddLine(new PointF(ScrtopX, ScrtopY), new PointF(ScrLeftX, ScrLeftY));
                //        path.CloseFigure();
                //        RectangleF boundingrect = path.GetBounds();
                //        if (boundingrect != RectangleF.Empty)
                //        {
                //            return boundingrect.Contains((float)point.X,(float)point.Y);
                //        }
                //    }
                //    return PointInLine;
                //}
            }
            catch (Exception ex)
            { throw ex; }
        }

        public bool ObjectInRectangle(ICanvas canvas, RectangleF rect, bool anyPoint)
        {
            try
            {
                RectangleF boundingrect = GetBoundingRect(canvas);
                if (anyPoint)
                { return HitUtil.LineIntersectWithRect(m_p1, m_p2, rect); }
                return rect.Contains(boundingrect);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual void Draw(ICanvas canvas, RectangleF unitrect)
        {
            try
            {
                Pen pen = null;
                if (Type == LineType.PointLine)
                { pen = new Pen(Color, Width); }
                else if (Type == LineType.Line)
                { pen = new Pen(Color, Width); }
                 else
                {
                    pen = new Pen(Color, 1);
                    pen.DashStyle = DashStyle.Custom;
                    pen.DashPattern = new float[] { 10f, 10f };
                }
                if (UseRoute)
                {
                    pen.Color = Color.DeepPink;
                    pen.DashStyle = DashStyle.Custom;
                    pen.DashPattern = new float[] { 1f, 1f };
                }
                if (Type == LineType.PointLine)
                {
                    GraphicsPath hPath = new GraphicsPath();
                    float ScrtopY = canvas.ToScreen(0);
                    float ScrtopX = canvas.ToScreen(0);
                    float ScrRightX = canvas.ToScreen(0.03);
                    float ScrRightY = canvas.ToScreen(-0.08);
                    float ScrLeftX = canvas.ToScreen(-0.03);
                    float ScrLeftY = canvas.ToScreen(-0.08);
                    hPath.AddLine(new PointF(ScrtopX, ScrLeftY), new PointF(ScrRightX, ScrRightY));
                    hPath.AddLine(new PointF(ScrRightX, ScrRightY), new PointF(ScrtopX, ScrtopY));
                    hPath.AddLine(new PointF(ScrtopX, ScrtopY), new PointF(ScrLeftX, ScrLeftY));
                    hPath.CloseFigure();
                    CustomLineCap HookCap = new CustomLineCap(hPath, null);
                    pen.CustomEndCap = HookCap;
                }
                else
                { pen.EndCap = System.Drawing.Drawing2D.LineCap.Round; }
                pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                canvas.DrawLine(canvas, pen, m_p1, m_p2);
                if (Highlighted)
                    canvas.DrawLine(canvas, DrawUtils.SelectedPen, m_p1, m_p2);
                if (Selected)
                {
                    canvas.DrawLine(canvas, DrawUtils.SelectedPen, m_p1, m_p2);
                    if (m_p1.IsEmpty == false)
                        DrawUtils.DrawNode(canvas, m_p1);
                    if (m_p2.IsEmpty == false)
                        DrawUtils.DrawNode(canvas, m_p2);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }


        public virtual void OnMouseMove(ICanvas canvas, UnitPoint point)
        {
            try
            {
                if (Control.ModifierKeys == Keys.Control)
                    point = HitUtil.OrthoPointD(m_p1, point, 45);
                m_p2 = point;
            }
            catch (Exception ex)
            { throw ex; }
        }


        public virtual eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        {
            try
            {
                Selected = false;
                if (snappoint is PerpendicularSnapPoint && snappoint.Owner is LineTool)
                {
                    LineTool src = snappoint.Owner as LineTool;
                    m_p2 = HitUtil.NearestPointOnLine(src.P1, src.P2, m_p1, true);
                    return eDrawObjectMouseDownEnum.DoneRepeat;
                }
                if (Control.ModifierKeys == Keys.Control)
                    point = HitUtil.OrthoPointD(m_p1, point, 45);
                m_p2 = point;
                return eDrawObjectMouseDownEnum.DoneRepeat;
            }
            catch (Exception ex)
            { throw ex; }
        }


        public void OnMouseUp(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        {
            try
            {
                SysParameter sys = AGVClientDAccess.GetParameterByCode("DefaultLenth");
                if (sys != null)
                {
                    double DefaultLenth = 0;
                    try
                    {
                        DefaultLenth = Convert.ToDouble(sys.ParameterValue);
                    }
                    catch (Exception ex)
                    { return; }
                    this.Lenth = DefaultLenth;
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual void OnKeyDown(ICanvas canvas, KeyEventArgs e)
        { }

        public INodePoint NodePoint(ICanvas canvas, UnitPoint point)
        {
            try
            {
                float thWidth = ThresholdWidth(canvas, Width);
                if (HitUtil.CircleHitPoint(m_p1, thWidth, point))
                    return new NodePointLine(this, NodePointLine.ePoint.P1);
                if (HitUtil.CircleHitPoint(m_p2, thWidth, point))
                    return new NodePointLine(this, NodePointLine.ePoint.P2);
                return null;
            }
            catch (Exception ex)
            { throw ex; }
        }


        public ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, List<IDrawObject> otherobjs, Type[] runningsnaptypes, Type usersnaptype)
        {
            try
            {
                if (Type == LineType.Line || Type == LineType.Dote)
                {
                    float thWidth = ThresholdWidth(canvas, Width);
                    if (runningsnaptypes != null)
                    {
                        foreach (Type snaptype in runningsnaptypes)
                        {
                            if (snaptype == typeof(VertextSnapPoint))
                            {
                                if (HitUtil.CircleHitPoint(m_p1, thWidth, point))
                                    return new VertextSnapPoint(canvas, this, m_p1);
                                if (HitUtil.CircleHitPoint(m_p2, thWidth, point))
                                    return new VertextSnapPoint(canvas, this, m_p2);
                            }
                            //if (snaptype == typeof(MidpointSnapPoint))
                            //{
                            //    UnitPoint p = MidPoint(canvas, m_p1, m_p2, point);
                            //    if (p != UnitPoint.Empty)
                            //        return new MidpointSnapPoint(canvas, this, p);
                            //}
                            if (snaptype == typeof(IntersectSnapPoint))
                            {
                                LineTool otherline = Utiles.FindObjectTypeInList(this, otherobjs, typeof(LineTool)) as LineTool;
                                if (otherline == null)
                                    continue;
                                UnitPoint p = HitUtil.LinesIntersectPoint(m_p1, m_p2, otherline.m_p1, otherline.m_p2);
                                if (p != UnitPoint.Empty)
                                    return new IntersectSnapPoint(canvas, this, p);
                            }
                        }
                        return null;
                    }

                    //if (usersnaptype == typeof(MidpointSnapPoint))
                    //    return new MidpointSnapPoint(canvas, this, HitUtil.LineMidpoint(m_p1, m_p2));
                    if (usersnaptype == typeof(IntersectSnapPoint))
                    {
                        LineTool otherline = Utiles.FindObjectTypeInList(this, otherobjs, typeof(LineTool)) as LineTool;
                        if (otherline == null)
                            return null;
                        UnitPoint p = HitUtil.LinesIntersectPoint(m_p1, m_p2, otherline.m_p1, otherline.m_p2);
                        if (p != UnitPoint.Empty)
                            return new IntersectSnapPoint(canvas, this, p);
                    }
                    if (usersnaptype == typeof(VertextSnapPoint))
                    {
                        double d1 = HitUtil.Distance(point, m_p1);
                        double d2 = HitUtil.Distance(point, m_p2);
                        if (d1 <= d2)
                            return new VertextSnapPoint(canvas, this, m_p1);
                        return new VertextSnapPoint(canvas, this, m_p2);
                    }
                    if (usersnaptype == typeof(NearestSnapPoint))
                    {
                        UnitPoint p = HitUtil.NearestPointOnLine(m_p1, m_p2, point);
                        if (p != UnitPoint.Empty)
                            return new NearestSnapPoint(canvas, this, p);
                    }
                    if (usersnaptype == typeof(PerpendicularSnapPoint))
                    {
                        UnitPoint p = HitUtil.NearestPointOnLine(m_p1, m_p2, point);
                        if (p != UnitPoint.Empty)
                            return new PerpendicularSnapPoint(canvas, this, p);
                    }
                }
                return null;
            }
            catch (Exception ex)
            { throw ex; }
        }


        public void Move(UnitPoint offset)
        {
            try
            {
                m_p1.X += offset.X;
                m_p1.Y += offset.Y;
                m_p2.X += offset.X;
                m_p2.Y += offset.Y;
            }
            catch (Exception ex)
            { throw ex; }
        }


        public string GetInfoAsString()
        {
            try
            {
                return string.Format("Line@{0},{1} - L={2:f4}<{3:f4}",
                    P1.PosAsString(),
                    P2.PosAsString(),
                    HitUtil.Distance(P1, P2),
                    HitUtil.RadiansToDegrees(HitUtil.LineAngleR(P1, P2, 0)));
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion

        #region ISerialize实现
        public void GetObjectData(XmlWriter wr)
        {
            try
            {
                wr.WriteStartElement("LineTool");
                XmlUtil.WriteProperties(this, wr);
                wr.WriteEndElement();
            }
            catch (Exception ex)
            { throw ex; }
        }


        public void AfterSerializedIn()
        { }

        /// <summary>
        /// 延长直线
        /// </summary>
        public void ExtendLineToPoint(UnitPoint newpoint)
        {
            try
            {
                UnitPoint newlinepoint = HitUtil.NearestPointOnLine(P1, P2, newpoint, true);
                if (HitUtil.Distance(newlinepoint, P1) < HitUtil.Distance(newlinepoint, P2))
                { P1 = newlinepoint; }
                else
                { P2 = newlinepoint; }
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion
    }
}
