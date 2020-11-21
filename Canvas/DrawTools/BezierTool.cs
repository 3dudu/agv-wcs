//using AGVDAccess;
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
    public enum BezierType
    {
        PointBezier,
        Bezier,
    }
    public class BezierTool : DrawObjectBase, IDrawObject, INodePoint, ISerialize
    {
        public enum eCurrentPoint
        {
            p1,
            p2,
            p3,
            p4,
            done,
        }

        #region 属性
        [XmlSerializable]
        [Description("长度")]
        public double Lenth { get; set; }
        protected static int ThresholdPixel = 0;

        [Browsable(false)]
        public static string ObjectType
        { get { return "BezierTool"; } }

        [Browsable(false)]
        public bool UseRoute { get; set; }

        [Browsable(false)]
        public virtual string Id
        { get { return ObjectType; } }

        UnitPoint m_p1 = UnitPoint.Empty;
        [Browsable(false)]
        [XmlSerializable]
        public UnitPoint P1
        {
            get { return m_p1; }
            set { m_p1 = value; }
        }

        UnitPoint m_p2 = UnitPoint.Empty;
        [Browsable(false)]
        [XmlSerializable]
        public UnitPoint P2
        {
            get { return m_p2; }
            set { m_p2 = value; }
        }

        UnitPoint m_p3 = UnitPoint.Empty;
        [Browsable(false)]
        [XmlSerializable]
        public UnitPoint P3
        {
            get { return m_p3; }
            set { m_p3 = value; }
        }


        UnitPoint m_p4 = UnitPoint.Empty;
        [Browsable(false)]
        [XmlSerializable]
        public UnitPoint P4
        {
            get { return m_p4; }
            set { m_p4 = value; }
        }

        protected eCurrentPoint m_curPoint = eCurrentPoint.p1;

        [Browsable(false)]
        public eCurrentPoint CurrentPoint
        {
            get { return m_curPoint; }
            set { m_curPoint = value; }
        }


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
            get { return m_p4; }
        }


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


        /// <summary>
        /// 当前选择的点
        /// </summary>
        public eCurrentPoint CurrSelectPoint
        {
            get { return m_pointId; }
        }

        BezierTool m_owner;
        BezierTool m_clone;
        eCurrentPoint m_pointId;
        UnitPoint m_originalPoint;
        UnitPoint m_endPoint;
        #endregion


        #region 函数
        public BezierTool() { UseRoute = false; }

        public BezierTool(BezierType type) { Type = type; UseRoute = false; }

        [Browsable(false)]
        [XmlSerializable]
        public BezierType Type { get; set; }

        public BezierTool(BezierTool owner, eCurrentPoint id)
        {
            try
            {
                m_owner = owner;
                m_clone = m_owner.Clone() as BezierTool;
                m_pointId = id;
                Type = owner.Type;
                m_originalPoint = GetPoint(m_pointId);
                UseRoute = false;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void InitializeFromModel(UnitPoint point, DrawingLayer layer, ISnapPoint snap)
        {
            try
            {
                Width = layer.Width;
                Color = layer.Color;
                m_p1 = m_p2 = m_p3 = m_p4 = point;
                Selected = true;
                OnMouseDown(null, point, snap);
            }
            catch (Exception ex)
            { throw ex; }
        }

        protected UnitPoint GetPoint(eCurrentPoint pointid)
        {
            try
            {
                if (pointid == eCurrentPoint.p1)
                    return m_clone.P1;
                if (pointid == eCurrentPoint.p1)
                    return m_clone.P2;
                if (pointid == eCurrentPoint.p1)
                    return m_clone.P3;
                if (pointid == eCurrentPoint.p1)
                    return m_clone.P4;
                return m_owner.P1;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void AfterSerializedIn()
        { }

        public void Cancel()
        { }

        public virtual void Copy(BezierTool acopy)
        {
            try
            {
                base.Copy(acopy);
                m_p1 = acopy.m_p1;
                m_p2 = acopy.m_p2;
                m_p3 = acopy.m_p3;
                m_p4 = acopy.m_p4;
                Selected = acopy.Selected;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public IDrawObject Clone()
        {
            try
            {
                BezierTool Bezier = new BezierTool(this.Type);
                Bezier.Copy(this);
                return Bezier;
            }
            catch (Exception ex)
            { throw ex; }
        }


        public void Draw(ICanvas canvas, RectangleF unitrect)
        {
            try
            {
                Pen pen = new Pen(Color, Width);
                if (UseRoute)
                {
                    pen.Color = Color.DeepPink;
                    pen.DashStyle = DashStyle.Custom;
                    pen.DashPattern = new float[] { 1f, 1f };
                }
                if (Type == BezierType.PointBezier)
                {
                    GraphicsPath hPath = new GraphicsPath();
                    float ScrtopY = canvas.ToScreen(0);
                    float ScrtopX = canvas.ToScreen(0);
                    float ScrRightX = canvas.ToScreen(0.03);
                    float ScrRightY = canvas.ToScreen(-0.08);
                    float ScrLeftX = canvas.ToScreen(-0.03);
                    float ScrLeftY = canvas.ToScreen(-0.08);
                    if (float.IsNaN(ScrtopY) || float.IsInfinity(ScrtopY) ||
                        float.IsNaN(ScrtopX) || float.IsInfinity(ScrtopX) ||
                        float.IsNaN(ScrRightX) || float.IsInfinity(ScrRightX) ||
                        float.IsNaN(ScrRightY) || float.IsInfinity(ScrRightY) ||
                        float.IsNaN(ScrLeftX) || float.IsInfinity(ScrLeftX) ||
                        float.IsNaN(ScrLeftY) || float.IsInfinity(ScrLeftY))
                    { return; }
                    hPath.AddLine(new PointF(ScrtopX, ScrLeftY), new PointF(ScrRightX, ScrRightY));
                    hPath.AddLine(new PointF(ScrRightX, ScrRightY), new PointF(ScrtopX, ScrtopY));
                    hPath.AddLine(new PointF(ScrtopX, ScrtopY), new PointF(ScrLeftX, ScrLeftY));
                    hPath.CloseFigure();
                    CustomLineCap HookCap;
                    try
                    {
                        HookCap = new CustomLineCap(hPath, null);
                    }
                    catch
                    { return; }
                    pen.CustomEndCap = HookCap;
                }
                else
                { pen.EndCap = System.Drawing.Drawing2D.LineCap.Round; }
                pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                if (Highlighted)
                {
                    Pen Choosepen = (Pen)DrawUtils.SelectedPen.Clone();
                    canvas.DrawBizer(canvas, Choosepen, m_p1, m_p4, m_p3, m_p2);
                }
                else if (Selected)
                {
                    Pen Choosepen = (Pen)DrawUtils.SelectedPen.Clone();
                    canvas.DrawBizer(canvas, Choosepen, m_p1, m_p4, m_p3, m_p2);
                    canvas.DrawLine(canvas, Choosepen, m_p1, m_p4);
                    canvas.DrawLine(canvas, Choosepen, m_p2, m_p3);
                    if (m_p2.IsEmpty == false)
                        DrawUtils.DrawNode(canvas, m_p2);
                    if (m_p3.IsEmpty == false)
                        DrawUtils.DrawNode(canvas, m_p3);
                    if (m_p1.IsEmpty == false)
                        DrawUtils.DrawNode(canvas, m_p1);
                    if (m_p4.IsEmpty == false)
                        DrawUtils.DrawNode(canvas, m_p4);
                }
                else
                { canvas.DrawBizer(canvas, pen, m_p1, m_p4, m_p3, m_p2); }
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
                m_owner.P3 = m_clone.P3;
                m_owner.P4 = m_clone.P4;
                m_clone = null;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 得到当前图形元素所在的举行区域
        /// </summary>
        public RectangleF GetBoundingRect(ICanvas canvas)
        {
            try
            {
                double minX = Math.Min(m_p1.X, m_p2.X);
                double maxX = Math.Max(m_p1.X, m_p2.X);
                double minY = Math.Min(m_p1.Y, m_p2.Y);
                double maxY = Math.Max(m_p1.Y, m_p2.Y);
                minX = Math.Min(minX, m_p3.X);
                maxX = Math.Max(maxX, m_p3.X);
                minY = Math.Min(minY, m_p3.Y);
                maxY = Math.Max(maxY, m_p3.Y);

                minX = Math.Min(minX, m_p4.X);
                maxX = Math.Max(maxX, m_p4.X);
                minY = Math.Min(minY, m_p4.Y);
                maxY = Math.Max(maxY, m_p4.Y);
                return new RectangleF(new PointF((float)minX, (float)minY), new SizeF((float)Math.Abs(maxX - minX), (float)Math.Abs(maxY - minY)));
            }
            catch (Exception ex)
            { throw ex; }
        }

        public IDrawObject GetClone()
        {
            return m_clone;
        }

        public string GetInfoAsString()
        {
            return "贝塞尔曲线";
        }

        public void GetObjectData(XmlWriter wr)
        {
            try
            {
                wr.WriteStartElement("BezierTool");
                XmlUtil.WriteProperties(this, wr);
                wr.WriteEndElement();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public IDrawObject GetOriginal()
        {
            return m_owner;
        }

        public void Move(UnitPoint offset)
        {
            try
            {
                m_p1.X += offset.X;
                m_p1.Y += offset.Y;
                m_p2.X += offset.X;
                m_p2.Y += offset.Y;
                m_p3.X += offset.X;
                m_p3.Y += offset.Y;
                m_p4.X += offset.X;
                m_p4.Y += offset.Y;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public INodePoint NodePoint(ICanvas canvas, UnitPoint point)
        {
            try
            {
                //float thWidth = LineTool.ThresholdWidth(canvas, Width, ThresholdPixel);
                //if (HitUtil.CircleHitPoint(m_p1, thWidth, point))
                //    return new BezierTool(this, eCurrentPoint.p1);
                //if (HitUtil.CircleHitPoint(m_p2, thWidth, point))
                //    return new BezierTool(this, eCurrentPoint.p2);
                //if (HitUtil.CircleHitPoint(m_p3, thWidth, point))
                //    return new BezierTool(this, eCurrentPoint.p3);
                //if (HitUtil.CircleHitPoint(m_p4, thWidth, point))
                //    return new BezierTool(this, eCurrentPoint.p4);
                return null;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 判断图形元素是否在一个矩形区域内
        /// </summary>
        public bool ObjectInRectangle(ICanvas canvas, RectangleF rect, bool anyPoint)
        {
            try
            {
                RectangleF boundingrect = GetBoundingRect(canvas);
                if (anyPoint)
                    return HitUtil.LineIntersectWithRect(m_p1, m_p2, rect);
                return rect.Contains(boundingrect);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void OnKeyDown(ICanvas canvas, KeyEventArgs e) { }

        public eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        {
            try
            {
                Selected = false;
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
                    m_curPoint = eCurrentPoint.p4;
                    return eDrawObjectMouseDownEnum.Continue;
                }
                if (m_curPoint == eCurrentPoint.p4)
                {
                    m_curPoint = eCurrentPoint.done;
                    return eDrawObjectMouseDownEnum.Done;
                }
                return eDrawObjectMouseDownEnum.Done;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void OnMouseMove(ICanvas canvas, UnitPoint point)
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
                    return;
                }
                if (m_curPoint == eCurrentPoint.p4)
                {
                    m_p4 = point;
                    return;
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void OnMouseUp(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        {
            try
            {
                //SysParameter sys = AGVClientDAccess.GetParameterByCode("DefaultLenth");
                //if (sys != null)
                //{
                //    double DefaultLenth = 0;
                //    try
                //    {
                //        DefaultLenth = Convert.ToDouble(sys.ParameterValue);
                //    }
                //    catch (Exception ex)
                //    { return; }
                //    this.Lenth = DefaultLenth;
                //}
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 判断一个点是否在一个元素上，即用来判断是否选中贝塞尔曲线
        /// </summary>
        public bool PointInObject(ICanvas canvas, UnitPoint point)
        {
            try
            {
                //RectangleF boundingrect = GetBoundingRect(canvas);
                //if (boundingrect.Contains(point.Point) == false)
                //    return false;
                //else
                //{ return true; }
                PointF[] pointList = new PointF[] { P1.Point, P4.Point, P3.Point, P2.Point };
                PointF[] aa = HitUtil.draw_bezier_curves(pointList, pointList.Length, 0.001F); // 在起点和终点之间
                if (aa.Count(p => Math.Abs(p.X - point.Point.X) <= 0.05 && Math.Abs(p.Y - point.Point.Y) <= 0.05) > 0)
                { return true; }
                else
                { return false; }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 恢复
        /// </summary>
        public void Redo()
        {
            try
            {
                SetPoint(m_pointId, m_endPoint, m_owner);
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

        public void SetPosition(UnitPoint pos)
        {
            try
            {
                SetPoint(m_pointId, pos, m_clone);
            }
            catch (Exception ex)
            { throw ex; }
        }

        protected void SetPoint(eCurrentPoint pointid, UnitPoint point, BezierTool Bezier)
        {
            try
            {
                if (pointid == eCurrentPoint.p1)
                    Bezier.P1 = point;
                if (pointid == eCurrentPoint.p2)
                    Bezier.P2 = point;
                if (pointid == eCurrentPoint.p3)
                    Bezier.P3 = point;
                if (pointid == eCurrentPoint.p4)
                    Bezier.P4 = point;
            }
            catch (Exception ex)
            { throw ex; }
        }


        public ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, List<IDrawObject> otherobj, Type[] runningsnaptypes, Type usersnaptype)
        {
            try
            {
                if (Type == BezierType.Bezier)
                {
                    if (HitUtil.Distance(point, m_p1, true) <= 0.2)
                    { return new VertextSnapPoint(canvas, this, m_p1); }
                    if (HitUtil.Distance(point, m_p2, true) <= 0.2)
                    { return new VertextSnapPoint(canvas, this, m_p2); }
                    if (HitUtil.Distance(point, m_p3, true) <= 0.2)
                    { return new VertextSnapPoint(canvas, this, m_p3); }
                    if (HitUtil.Distance(point, m_p4, true) <= 0.2)
                    { return new VertextSnapPoint(canvas, this, m_p4); }
                }
                return null;
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion
    }
}
