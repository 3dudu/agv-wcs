using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Canvas.CanvasInterfaces;
using CommonTools;
using System.Drawing.Drawing2D;

namespace Canvas.CanvasCtrl
{
    /// <summary>
    /// 画布自定义控件
    /// </summary>
    public partial class CanvasCtrller : UserControl
    {
        #region 属性
        /// <summary>
        /// 默认选择命令
        /// </summary>
        public eCommandType m_commandType = eCommandType.select;
        /// <summary>
        /// 是否允许修改编辑
        /// </summary>
        public bool IsChooseSpecial = false;

        /// <summary>
        /// 当前画布
        /// </summary>
        public ICanvasOwner m_owner;

        /// <summary>
        /// 鼠标图标集合
        /// </summary>
        public CursorCollection m_cursors = new CursorCollection();

        /// <summary>
        /// 当前的画布下的数据模型
        /// </summary>
        public IModel m_model;

        /// <summary>
        /// 鼠标移动操作类
        /// </summary>
        private MoveHelper m_moveHelper = null;

        /// <summary>
        /// 节点移动类
        /// </summary>
        private NodeMoveHelper m_nodeMoveHelper = null;

        /// <summary>
        /// 画布包装类
        /// </summary>
        private CanvasWrapper m_canvaswrapper;

        /// <summary>
        /// 是否显示吸附点
        /// </summary>
        private bool m_runningSnaps = true;

        /// <summary>
        /// 吸附点类型(断点，中点，圆心。。。)
        /// </summary>
        private Type[] m_runningSnapTypes = null;

        /// <summary>
        /// 鼠标落下的坐标
        /// </summary>
        private PointF m_mousedownPoint;

        /// <summary>
        /// 新建的图形元素
        /// </summary>
        private IDrawObject m_newObject = null;

        /// <summary>
        /// 当前的编辑工具对象
        /// </summary>
        private IEditTool m_editTool = null;

        /// <summary>
        /// 当前鼠标左键按下拖动的选择矩形框
        /// </summary>
        private SelectionRectangle m_selection = null;//选择矩形

        /// <summary>
        /// 当前图形元素的ID
        /// </summary>
        private string m_drawObjectId = string.Empty;

        /// <summary>
        /// 当前选中工具的ID
        /// </summary>
        private string m_editToolId = string.Empty;

        /// <summary>
        /// 当前画布的图片
        /// </summary>
        private Bitmap m_staticImage = null;//当前绘画完的画面

        /// <summary>
        /// 是否刷新了画布
        /// </summary>
        private bool m_staticDirty = true;

        /// <summary>
        /// 最后一次中心点
        /// </summary>
        private UnitPoint m_lastCenterPoint;

        /// <summary>
        /// 像素的偏移量
        /// </summary>
        private PointF m_panOffset = new PointF(0, 0);
        /// <summary>
        /// 拖动的偏移量
        /// </summary>
        private PointF m_dragOffset = new PointF(0, 0);

        /// <summary>
        /// 像素  影响界面显示的元素数量
        /// </summary>
        private float m_screenResolution = 200;

        /// <summary>
        /// 当前的吸附点
        /// </summary>
        private ISnapPoint m_snappoint = null;

        public Type[] RunningSnaps
        {
            get { return m_runningSnapTypes; }
            set { m_runningSnapTypes = value; }
        }

        public bool RunningSnapsEnabled
        {
            get { return m_runningSnaps; }
            set { m_runningSnaps = value; }
        }

        /// <summary>
        /// 抗锯齿
        /// </summary>
        System.Drawing.Drawing2D.SmoothingMode m_smoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

        public System.Drawing.Drawing2D.SmoothingMode SmoothingMode
        {
            get { return m_smoothingMode; }
            set { m_smoothingMode = value; }
        }

        public IModel Model
        {
            get { return m_model; }
            set { m_model = value; }
        }

        public IDrawObject NewObject
        {
            get { return m_newObject; }
        }
        #endregion

        #region 画布方法

        protected override void OnLoad(EventArgs e)
        {
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.Opaque | ControlStyles.OptimizedDoubleBuffer, true);
        }

        public CanvasCtrller(ICanvasOwner owner, IModel datamodel)
        {
            m_canvaswrapper = new CanvasWrapper(this);
            m_owner = owner;
            m_model = datamodel;
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            m_commandType = eCommandType.select;
            m_cursors.AddCursor(eCommandType.select, Cursors.Arrow);
            m_cursors.AddCursor(eCommandType.draw, Cursors.Cross);
            m_cursors.AddCursor(eCommandType.pan, "hmove.cur");
            m_cursors.AddCursor(eCommandType.move, Cursors.SizeAll);
            m_cursors.AddCursor(eCommandType.edit, Cursors.Cross);
            UpdateCursor();

            m_moveHelper = new MoveHelper(this);
            m_nodeMoveHelper = new NodeMoveHelper(m_canvaswrapper);
        }

        /// <summary>
        /// 将鼠标屏幕坐标转换成画布坐标
        /// </summary>
        public UnitPoint GetMousePoint()
        {
            Point point = this.PointToClient(Control.MousePosition);
            return ToUnit(point);
        }


        /// <summary>
        /// 指定屏幕中心点位置
        /// </summary>
        /// <param name="unitPoint"></param>
        public void SetCenter(UnitPoint unitPoint)
        {
            PointF point = ToScreen(unitPoint);
            m_lastCenterPoint = unitPoint;
            SetCenterScreen(point, false);
        }

        /// <summary>
        /// 更新鼠标图标
        /// </summary>
        private void UpdateCursor()
        {
            Cursor = m_cursors.GetCursor(m_commandType);
        }

        /// <summary>
        /// 根据鼠标位置设定屏幕中心点
        /// </summary>
        public void SetCenter()
        {
            Point point = this.PointToClient(Control.MousePosition);
            SetCenterScreen(point, true);
        }

        /// <summary>
        /// 得到屏幕中心点位置
        /// </summary>
        /// <returns></returns>
        public UnitPoint GetCenter()
        {
            return ToUnit(new PointF(this.ClientRectangle.Width / 2, this.ClientRectangle.Height / 2));
        }

        /// <summary>
        /// 设置中心点
        /// </summary>
        /// <param name="screenPoint"></param>
        /// <param name="setCursor"></param>
        protected void SetCenterScreen(PointF screenPoint, bool setCursor)
        {
            float centerX = ClientRectangle.Width / 2;
            m_panOffset.X += centerX - screenPoint.X;

            float centerY = ClientRectangle.Height / 2;
            m_panOffset.Y += centerY - screenPoint.Y;

            if (setCursor)
            { Cursor.Position = this.PointToScreen(new Point((int)centerX, (int)centerY)); }
            DoInvalidate(true);
        }

        /// <summary>
        /// 将屏幕长度转换成画布长度
        /// </summary>
        public double ToUnit(float screenvalue)
        {
            return Math.Round((double)screenvalue / (double)(m_screenResolution * m_model.Zoom), 5);
        }

        /// <summary>
        /// 将屏幕坐标转换成画布坐标
        /// </summary>
        public UnitPoint ToUnit(PointF screenpoint)
        {
            float panoffsetX = m_panOffset.X + m_dragOffset.X;
            float panoffsetY = m_panOffset.Y + m_dragOffset.Y;
            float xpos = (screenpoint.X - panoffsetX) / (m_screenResolution * m_model.Zoom);
            float ypos = ScreenHeight() - ((screenpoint.Y - panoffsetY)) / (m_screenResolution * m_model.Zoom);
            return new UnitPoint(xpos, ypos);
        }

        /// <summary>
        /// 将画布坐标转换成屏幕坐标
        /// </summary>
        public PointF ToScreen(UnitPoint point)
        {
            try
            {
                PointF transformedPoint = Translate(point);
                transformedPoint.Y = ScreenHeight() - transformedPoint.Y;
                transformedPoint.Y *= m_screenResolution * m_model.Zoom;
                transformedPoint.X *= m_screenResolution * m_model.Zoom;

                transformedPoint.X += m_panOffset.X + m_dragOffset.X;
                transformedPoint.Y += m_panOffset.Y + m_dragOffset.Y;
                return transformedPoint;
            }
            catch (Exception ex)
            { throw ex; }
        }

        PointF Translate(UnitPoint point)
        {
            return point.Point;
        }

        /// <summary>
        /// 获取屏幕的画布高度
        /// </summary>
        float ScreenHeight()
        {
            return (float)(ToUnit(this.ClientRectangle.Height) / m_model.Zoom);
        }

        /// <summary>
        /// 创建画笔
        /// </summary>
        public Pen CreatePen(Color color, float unitWidth)
        {
            return new Pen(color, unitWidth);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="dostatic"></param>
        public void DoInvalidate(bool dostatic)
        {
            if (dostatic)
            { m_staticDirty = true; }
            Invalidate();
        }

        /// <summary>
        /// 重载画布的onPaint事件
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                CommonTools.Tracing.StartTrack(Tracing.TracePaint);
                //设置画线是否抗锯齿
                e.Graphics.SmoothingMode = m_smoothingMode;
                //设置画布
                CanvasWrapper dc = new CanvasWrapper(this, e.Graphics, ClientRectangle);
                //获取当前绘画区域矩形
                Rectangle cliprectangle = e.ClipRectangle;
                if (m_staticImage == null)
                {
                    cliprectangle = ClientRectangle;
                    m_staticImage = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
                    m_staticDirty = true;
                }
                RectangleF r = ScreenUtils.ToUnitNormalized(dc, cliprectangle);
                if (float.IsNaN(r.Width) || float.IsInfinity(r.Width))
                { return; }
                if (m_staticDirty)//刷新画布
                {
                    m_staticDirty = false;
                    CanvasWrapper dcStatic = new CanvasWrapper(this, Graphics.FromImage(m_staticImage), ClientRectangle);
                    dcStatic.Graphics.SmoothingMode = m_smoothingMode;
                    try
                    {
                        m_model.BackgroundLayer.Draw(dcStatic, r);
                    }
                    catch (Exception ex)
                    { throw ex; }
                    //判断是否绘制背景栅格背景表格
                    try
                    {
                        if (m_model.GridLayer.Enabled)
                        { m_model.GridLayer.Draw(dcStatic, r); }
                    }
                    catch (Exception ex)
                    { throw ex; }
                    //绘制画布中心十字
                    PointF nullPoint = ToScreen(new UnitPoint(0, 0));
                    try
                    {
                        GraphicsPath hPath = new GraphicsPath();
                        float ScrtopY = ToScreen(0);
                        float ScrtopX = ToScreen(0);
                        float ScrRightX = (float)3;
                        float ScrRightY = (float)-6;
                        float ScrLeftX = (float)-3;
                        float ScrLeftY = (float)-6;
                        hPath.AddLine(new PointF(ScrtopX, ScrLeftY), new PointF(ScrRightX, ScrRightY));
                        hPath.AddLine(new PointF(ScrRightX, ScrRightY), new PointF(ScrtopX, ScrtopY));
                        hPath.AddLine(new PointF(ScrtopX, ScrtopY), new PointF(ScrLeftX, ScrLeftY));
                        hPath.CloseFigure();
                        Pen pen = new Pen(Color.Blue, 1);
                        CustomLineCap HookCap = new CustomLineCap(hPath, null);
                        pen.CustomEndCap = HookCap;
                        dcStatic.Graphics.DrawLine(pen, nullPoint.X/* - 50*/, nullPoint.Y, nullPoint.X + 100, nullPoint.Y);
                        dcStatic.Graphics.DrawLine(pen, nullPoint.X, nullPoint.Y/* - 50*/, nullPoint.X, nullPoint.Y - 100);
                    }
                    catch (Exception ex)
                    { }
                    //刷新
                    try
                    {
                        ICanvasLayer[] layers = m_model.Layers;
                        for (int layerindex = layers.Length - 1; layerindex >= 0; layerindex--)
                        {
                            if (layers[layerindex] != m_model.ActiveLayer && layers[layerindex].Visible)
                                layers[layerindex].Draw(dcStatic, r);
                        }

                        if (m_model.ActiveLayer != null)
                        { m_model.ActiveLayer.Draw(dcStatic, r); }
                        dcStatic.Dispose();
                    }
                    catch (Exception ex)
                    { throw ex; }
                }
                try
                {
                    e.Graphics.DrawImage(m_staticImage, cliprectangle, cliprectangle, GraphicsUnit.Pixel);
                }
                catch (Exception ex)
                { throw ex; }
                //绘制元素被选中
                foreach (IDrawObject drawobject in m_model.SelectedObjects)
                { drawobject.Draw(dc, r); }
                //绘制图形元素
                try
                {
                    if (m_newObject != null)
                    { m_newObject.Draw(dc, r); }
                }
                catch (Exception ex)
                { throw ex; }
                //绘制吸附点
                try
                {
                    if (m_snappoint != null)
                    { m_snappoint.Draw(dc); }
                }
                catch (Exception ex)
                { throw ex; }
                //绘制鼠标拖拽的选择框
                if (m_selection != null)
                {
                    m_selection.Reset();
                    m_selection.SetMousePoint(e.Graphics, this.PointToClient(Control.MousePosition));
                }
                //绘制移动过程元素
                if (m_moveHelper.IsEmpty == false)
                { m_moveHelper.DrawObjects(dc, r); }
                if (m_nodeMoveHelper.IsEmpty == false)
                { m_nodeMoveHelper.DrawObjects(dc, r); }
                dc.Dispose();
                CommonTools.Tracing.EndTrack(Tracing.TracePaint, "OnPaint complete");
            }
            catch (Exception ex)
            { /*throw ex; */}
        }

        /// <summary>
        /// 重绘矩形区域内容
        /// </summary>
        private void RepaintStatic(Rectangle r)
        {
            if (m_staticImage == null)
                return;
            Graphics dc = Graphics.FromHwnd(Handle);
            if (r.X < 0) r.X = 0;
            if (r.X > m_staticImage.Width) r.X = 0;
            if (r.Y < 0) r.Y = 0;
            if (r.Y > m_staticImage.Height) r.Y = 0;

            if (r.Width > m_staticImage.Width || r.Width < 0)
                r.Width = m_staticImage.Width;
            if (r.Height > m_staticImage.Height || r.Height < 0)
                r.Height = m_staticImage.Height;
            dc.DrawImage(m_staticImage, r, r, GraphicsUnit.Pixel);
            dc.Dispose();
        }

        /// <summary>
        /// 重绘吸附点
        /// </summary>
        private void RepaintSnappoint(ISnapPoint snappoint)
        {
            if (snappoint == null)
                return;
            CanvasWrapper dc = new CanvasWrapper(this, Graphics.FromHwnd(Handle), ClientRectangle);
            snappoint.Draw(dc);
            dc.Graphics.Dispose();
            dc.Dispose();
        }

        /// <summary>
        /// 重画元素
        /// </summary>
        void RepaintObject(IDrawObject obj)
        {
            if (obj == null)
                return;
            CanvasWrapper dc = new CanvasWrapper(this, Graphics.FromHwnd(Handle), ClientRectangle);
            RectangleF invalidaterect = ScreenUtils.ConvertRect(ScreenUtils.ToScreenNormalized(dc, obj.GetBoundingRect(dc)));
            obj.Draw(dc, invalidaterect);
            dc.Graphics.Dispose();
            dc.Dispose();
        }

        /// <summary>
        /// 刷新矩形区域
        /// </summary>
        public void DoInvalidate(bool dostatic, RectangleF rect)
        {
            if (dostatic)
            { m_staticDirty = true; }
            Invalidate(ScreenUtils.ConvertRect(rect));
        }

        /// <summary>
        /// 处理元素选择
        /// </summary>
        protected void HandleSelection(List<IDrawObject> selected)
        {
            bool add = Control.ModifierKeys == Keys.Shift;
            bool toggle = Control.ModifierKeys == Keys.Control;
            bool invalidate = false;
            bool anyoldsel = false;
            int selcount = 0;
            if (selected != null)
            { selcount = selected.Count; }
            if (m_model.SelectedObjects.Count() > 0)
            { anyoldsel = true; }
            //按住Control键可以多选
            if (toggle && selcount > 0)
            {
                invalidate = true;
                foreach (IDrawObject obj in selected)
                {
                    if (IsChooseSpecial)
                    {
                        if (obj.Id != "ButtonTool")
                        { m_model.RemoveSelectedObject(obj); }
                        else
                        { m_model.AddSelectedObject(obj); }
                    }
                    else
                    {
                        if (m_model.IsSelected(obj))
                        { m_model.RemoveSelectedObject(obj); }
                        else
                        { m_model.AddSelectedObject(obj); }
                    }
                }
            }
            if (add && selcount > 0)
            {
                invalidate = true;
                foreach (IDrawObject obj in selected)
                {
                    if (IsChooseSpecial)
                    {
                        if (obj.Id != "ButtonTool")
                        { m_model.RemoveSelectedObject(obj); }
                        else
                        { m_model.AddSelectedObject(obj); }
                    }
                    else
                    { m_model.AddSelectedObject(obj); }
                }
            }
            //如果没按住键盘，则单选
            if (add == false && toggle == false && selcount > 0)
            {
                invalidate = true;
                m_model.ClearSelectedObjects();
                foreach (IDrawObject obj in selected)
                {
                    if (IsChooseSpecial)
                    {
                        if (obj.Id != "ButtonTool")
                        { m_model.RemoveSelectedObject(obj); }
                        else
                        { m_model.AddSelectedObject(obj); }
                    }
                    else
                    { m_model.AddSelectedObject(obj); }
                }
            }
            if (add == false && toggle == false && selcount == 0 && anyoldsel)
            {
                invalidate = true;
                m_model.ClearSelectedObjects();
            }

            if (invalidate)
            { DoInvalidate(false); }
        }

        private void FinishNodeEdit()
        {
            m_commandType = eCommandType.select;
            m_snappoint = null;
        }

        /// <summary>
        /// 处理绘制事鼠标落下事件
        /// </summary>
        protected virtual void HandleMouseDownWhenDrawing(UnitPoint mouseunitpoint, ISnapPoint snappoint)
        {
            try
            {
                if (m_commandType == eCommandType.draw)
                {
                    if (m_newObject == null)
                    {
                        m_newObject = m_model.CreateObject(m_drawObjectId, mouseunitpoint, snappoint);
                        DoInvalidate(false, m_newObject.GetBoundingRect(m_canvaswrapper));
                        if (m_newObject.Id == "LandMark" || m_newObject.Id == "ImgeTool"
                       || m_newObject.Id == "TextTool" || m_newObject.Id == "StorageTool"
                       || m_newObject.Id == "AGVTool" || m_newObject.Id == "ButtonTool"
                       || m_newObject.Id == "PositionTool")
                        {
                            m_newObject = m_model.CreateObject(m_drawObjectId, mouseunitpoint, snappoint);
                            m_newObject.OnMouseDown(m_canvaswrapper, mouseunitpoint, snappoint);
                            m_model.AddObject(m_model.ActiveLayer, m_newObject);
                            m_newObject = null;
                            DoInvalidate(true);
                        }
                        if (m_drawObjectId == "PointBezier")
                        {
                            if ((m_newObject as Canvas.DrawTools.BezierTool).CurrentPoint == Canvas.DrawTools.BezierTool.eCurrentPoint.p1 ||
                                (m_newObject as Canvas.DrawTools.BezierTool).CurrentPoint == Canvas.DrawTools.BezierTool.eCurrentPoint.p2)
                            {
                                if (m_snappoint == null || (m_snappoint != null &&
                              (m_snappoint.Owner != null && m_snappoint.Owner.Id != "LandMark" ||
                              (m_snappoint.Owner == null))))
                                {
                                    m_newObject = null;
                                    DoInvalidate(true);
                                    return;
                                }
                            }
                        }
                        if (m_drawObjectId == "arc3p")
                        {
                            if ((m_newObject as Canvas.DrawTools.Arc3Point).CurrentPoint == Canvas.DrawTools.Arc3Point.eCurrentPoint.p1 ||
                                (m_newObject as Canvas.DrawTools.Arc3Point).CurrentPoint == Canvas.DrawTools.Arc3Point.eCurrentPoint.p3)
                            {
                                if (m_snappoint == null || (m_snappoint != null &&
                             (m_snappoint.Owner != null && m_snappoint.Owner.Id != "LandMark" ||
                             (m_snappoint.Owner == null))) || ((m_newObject as Canvas.DrawTools.Arc3Point).P1.X == (m_newObject as Canvas.DrawTools.Arc3Point).P3.X && (m_newObject as Canvas.DrawTools.Arc3Point).P1.Y == (m_newObject as Canvas.DrawTools.Arc3Point).P3.Y))
                                {
                                    m_newObject = null;
                                    DoInvalidate(true);
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (m_drawObjectId == "PointBezier")
                        {
                            if ((m_newObject as Canvas.DrawTools.BezierTool).CurrentPoint == Canvas.DrawTools.BezierTool.eCurrentPoint.p1 ||
                                (m_newObject as Canvas.DrawTools.BezierTool).CurrentPoint == Canvas.DrawTools.BezierTool.eCurrentPoint.p2)
                            {
                                if (m_snappoint == null || (m_snappoint != null &&
                              (m_snappoint.Owner != null && m_snappoint.Owner.Id != "LandMark" ||
                              (m_snappoint.Owner == null))))
                                {
                                    m_newObject = null;
                                    DoInvalidate(true);
                                    return;
                                }
                            }
                        }
                        if (m_drawObjectId == "arc3p")
                        {
                            if ((m_newObject as Canvas.DrawTools.Arc3Point).CurrentPoint == Canvas.DrawTools.Arc3Point.eCurrentPoint.p1 ||
                                (m_newObject as Canvas.DrawTools.Arc3Point).CurrentPoint == Canvas.DrawTools.Arc3Point.eCurrentPoint.p3)
                            {
                                if (m_snappoint == null || (m_snappoint != null &&
                             (m_snappoint.Owner != null && m_snappoint.Owner.Id != "LandMark" ||
                             (m_snappoint.Owner == null))) || ((m_newObject as Canvas.DrawTools.Arc3Point).P1.X == (m_newObject as Canvas.DrawTools.Arc3Point).P3.X && (m_newObject as Canvas.DrawTools.Arc3Point).P1.Y == (m_newObject as Canvas.DrawTools.Arc3Point).P3.Y))
                                {
                                    m_newObject = null;
                                    DoInvalidate(true);
                                    return;
                                }
                            }
                        }
                        eDrawObjectMouseDownEnum result = m_newObject.OnMouseDown(m_canvaswrapper, mouseunitpoint, snappoint);
                        switch (result)
                        {
                            case eDrawObjectMouseDownEnum.Done:
                                m_model.AddObject(m_model.ActiveLayer, m_newObject);
                                m_newObject = null;
                                DoInvalidate(true);
                                break;
                            case eDrawObjectMouseDownEnum.DoneRepeat:
                                m_model.AddObject(m_model.ActiveLayer, m_newObject);
                                m_newObject = m_model.CreateObject(m_newObject.Id, m_newObject.RepeatStartingPoint, null);
                                DoInvalidate(true);
                                break;
                            case eDrawObjectMouseDownEnum.Continue:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            { /*throw ex;*/ }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            try
            {
                if (m_commandType == eCommandType.move && e.Button == MouseButtons.Right)
                {
                    m_commandType = eCommandType.select;
                    UpdateCursor();
                    return;
                }
                if (m_commandType == eCommandType.select && e.Button == MouseButtons.Right)
                {
                    m_commandType = eCommandType.pan;
                    UpdateCursor();
                }
                m_mousedownPoint = new PointF(e.X, e.Y);
                m_dragOffset = new PointF(0, 0);

                UnitPoint mousepoint = ToUnit(m_mousedownPoint);
                if (m_snappoint != null)
                { mousepoint = m_snappoint.SnapPoint; }
                if (m_commandType == eCommandType.editNode)
                {
                    bool handled = false;
                    if (m_nodeMoveHelper.HandleMouseDown(mousepoint, ref handled))
                    {
                        if (m_drawObjectId == "PointLine")
                        {
                            if (m_snappoint == null || (m_snappoint != null &&
                           (m_snappoint.Owner != null && m_snappoint.Owner.Id != "LandMark" ||
                           (m_snappoint.Owner == null))))
                            {
                                m_newObject = null;
                                DoInvalidate(true);
                                return;
                            }
                        }
                        FinishNodeEdit();
                        base.OnMouseDown(e);
                        return;
                    }
                }
                if (m_commandType == eCommandType.select && e.Button == MouseButtons.Left)
                {
                    bool handled = false;
                    if (m_nodeMoveHelper.HandleMouseDown(mousepoint, ref handled))
                    {
                        m_commandType = eCommandType.editNode;
                        m_snappoint = null;
                        base.OnMouseDown(e);
                        return;
                    }
                    if (!IsChooseSpecial)
                    {
                        m_selection = new SelectionRectangle(m_mousedownPoint);
                    }
                }
                if (m_commandType == eCommandType.move)
                {
                    m_moveHelper.HandleMouseDownForMove(mousepoint, m_snappoint);
                }
                if (m_commandType == eCommandType.draw)
                {
                    if (e.Button == MouseButtons.Right)//鼠标右键结束绘画
                    {
                        m_newObject = null;
                        DoInvalidate(true);
                        return;
                    }
                    if (m_drawObjectId == "PointLine")
                    {
                        if (m_snappoint == null || (m_snappoint != null &&
                       (m_snappoint.Owner != null && m_snappoint.Owner.Id != "LandMark" ||
                       (m_snappoint.Owner == null))))
                        {
                            m_newObject = null;
                            DoInvalidate(true);
                            return;
                        }
                    }
                    HandleMouseDownWhenDrawing(mousepoint, null);
                    DoInvalidate(true);
                }
                if (m_commandType == eCommandType.edit)
                {
                    if (m_editTool == null)
                        m_editTool = m_model.GetEditTool(m_editToolId);
                    if (m_editTool != null)
                    {
                        if (m_editTool.SupportSelection)
                            m_selection = new SelectionRectangle(m_mousedownPoint);

                        eDrawObjectMouseDownEnum mouseresult = m_editTool.OnMouseDown(m_canvaswrapper, mousepoint, m_snappoint);
                        if (mouseresult == eDrawObjectMouseDownEnum.Done)
                        {
                            m_editTool.Finished();
                            m_editTool = m_model.GetEditTool(m_editToolId);
                            if (m_editTool.SupportSelection)
                                m_selection = new SelectionRectangle(m_mousedownPoint);
                        }
                    }
                    DoInvalidate(true);
                    UpdateCursor();
                }
                base.OnMouseDown(e);
            }
            catch (Exception ex)
            {/* throw ex;*/ }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            try
            {
                if (m_commandType == eCommandType.pan && e.Button == MouseButtons.Right)
                {
                    m_commandType = eCommandType.select;
                    UpdateCursor();
                }
                if (m_commandType == eCommandType.pan || e.Button == MouseButtons.Right)
                {
                    m_panOffset.X += m_dragOffset.X;
                    m_panOffset.Y += m_dragOffset.Y;
                    m_dragOffset = new PointF(0, 0);
                }
                List<IDrawObject> hitlist = null;
                Rectangle screenSelRect = Rectangle.Empty;
                if (m_selection != null)
                {
                    screenSelRect = m_selection.ScreenRect();
                    RectangleF selectionRect = m_selection.Selection(m_canvaswrapper);
                    if (selectionRect != RectangleF.Empty)
                    {
                        //选择框选择元素
                        hitlist = m_model.GetHitObjects(m_canvaswrapper, selectionRect, m_selection.AnyPoint());
                        DoInvalidate(true);
                    }
                    else
                    {
                        // 用鼠标点击来选择元素
                        UnitPoint mousepoint = ToUnit(new PointF(e.X, e.Y));
                        hitlist = m_model.GetHitObjects(m_canvaswrapper, mousepoint);
                    }
                    m_selection = null;
                }
                else
                {
                    // 用鼠标点击来选择元素
                    UnitPoint mousepoint = ToUnit(new PointF(e.X, e.Y));
                    hitlist = m_model.GetHitObjects(m_canvaswrapper, mousepoint);
                }
                if (m_commandType == eCommandType.select && e.Button == MouseButtons.Left)
                {
                    if (hitlist != null)
                    {
                        HandleSelection(hitlist);
                    }
                }
                if (m_commandType == eCommandType.edit && m_editTool != null)
                {
                    UnitPoint mousepoint = ToUnit(m_mousedownPoint);
                    if (m_snappoint != null)
                    { mousepoint = m_snappoint.SnapPoint; }
                    if (screenSelRect != Rectangle.Empty)
                    { m_editTool.SetHitObjects(mousepoint, hitlist); }
                    m_editTool.OnMouseUp(m_canvaswrapper, mousepoint, m_snappoint);
                }
                if (m_commandType == eCommandType.draw && m_newObject != null)
                {
                    UnitPoint mousepoint = ToUnit(m_mousedownPoint);
                    if (m_snappoint != null)
                    { mousepoint = m_snappoint.SnapPoint; }
                    m_newObject.OnMouseUp(m_canvaswrapper, mousepoint, m_snappoint);
                }
                base.OnMouseUp(e);
            }
            catch (Exception ex)
            {/* throw ex*/; }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            try
            {
                base.OnMouseMove(e);
                if (m_selection != null)
                {
                    Graphics dc = Graphics.FromHwnd(Handle);
                    m_selection.SetMousePoint(dc, new PointF(e.X, e.Y));
                    dc.Dispose();
                    return;
                }

                if ((m_commandType == eCommandType.pan && e.Button == MouseButtons.Left) || (e.Button == MouseButtons.Right))
                {
                    m_dragOffset.X = -(m_mousedownPoint.X - e.X);
                    m_dragOffset.Y = -(m_mousedownPoint.Y - e.Y);
                    m_lastCenterPoint = CenterPointUnit();
                    DoInvalidate(true);
                }

                UnitPoint mousepoint;
                UnitPoint unitpoint = ToUnit(new PointF(e.X, e.Y));
                if (m_commandType == eCommandType.draw || m_commandType == eCommandType.move || m_nodeMoveHelper.IsEmpty == false)
                {
                    Rectangle invalidaterect = Rectangle.Empty;
                    ISnapPoint newsnap = null;
                    mousepoint = GetMousePoint();
                    if (RunningSnapsEnabled)
                    { newsnap = m_model.SnapPoint(m_canvaswrapper, mousepoint, m_runningSnapTypes, null); }
                    if (newsnap == null)
                    { newsnap = m_model.GridLayer.SnapPoint(m_canvaswrapper, mousepoint, null); }
                    if ((m_snappoint != null) && ((newsnap == null) || (newsnap.SnapPoint != m_snappoint.SnapPoint) || m_snappoint.GetType() != newsnap.GetType()))
                    {
                        invalidaterect = ScreenUtils.ConvertRect(ScreenUtils.ToScreenNormalized(m_canvaswrapper, m_snappoint.BoundingRect));
                        invalidaterect.Inflate(2, 2);
                        RepaintStatic(invalidaterect);
                        m_snappoint = newsnap;
                    }
                    if (m_commandType == eCommandType.move)
                    { Invalidate(invalidaterect); }
                    if (m_snappoint == null)
                    { m_snappoint = newsnap; }
                }
                m_owner.SetPositionInfo(unitpoint);
                m_owner.SetSnapInfo(m_snappoint);

                if (m_snappoint != null)
                { mousepoint = m_snappoint.SnapPoint; }
                else
                { mousepoint = GetMousePoint(); }

                if (m_newObject != null)
                {
                    Rectangle invalidaterect = ScreenUtils.ConvertRect(ScreenUtils.ToScreenNormalized(m_canvaswrapper, m_newObject.GetBoundingRect(m_canvaswrapper)));
                    invalidaterect.Inflate(2, 2);
                    RepaintStatic(invalidaterect);

                    m_newObject.OnMouseMove(m_canvaswrapper, mousepoint);
                    RepaintObject(m_newObject);
                }

                if (m_snappoint != null)
                { RepaintSnappoint(m_snappoint); }

                if (m_moveHelper.HandleMouseMoveForMove(mousepoint))
                { Refresh(); }

                RectangleF rNoderect = m_nodeMoveHelper.HandleMouseMoveForNode(mousepoint);

                if (rNoderect != RectangleF.Empty)
                {
                    Rectangle invalidaterect = ScreenUtils.ConvertRect(ScreenUtils.ToScreenNormalized(m_canvaswrapper, rNoderect));
                    RepaintStatic(invalidaterect);
                    CanvasWrapper dc = new CanvasWrapper(this, Graphics.FromHwnd(Handle), ClientRectangle);
                    dc.Graphics.Clip = new Region(ClientRectangle);
                    m_nodeMoveHelper.DrawObjects(dc, rNoderect);
                    if (m_snappoint != null)
                    { RepaintSnappoint(m_snappoint); }
                    dc.Graphics.Dispose();
                    dc.Dispose();
                }
            }
            catch (Exception ex)
            {}
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            try
            {
                UnitPoint p = GetMousePoint();
                //设置鼠标一个齿轮的数值
                float wheeldeltatick = 120;
                float zoomdelta = (1.1f * (Math.Abs(e.Delta) / wheeldeltatick));
                float zoom = 0;
                if (e.Delta < 0)
                { zoom = m_model.Zoom / zoomdelta; }
                else
                { zoom = m_model.Zoom * zoomdelta; }
                if (float.IsNaN(zoom) || float.IsInfinity(zoom) || Math.Round(zoom, 6) <= 0.000001 || zoom > 20000)
                { return; }
                m_model.Zoom = zoom;
                SetCenterScreen(ToScreen(p), true);
                //DoInvalidate(true);
                DoInvalidate(true, ClientRectangle);
                base.OnMouseWheel(e);
            }
            catch (Exception ex)
            { /*throw ex;*/ }
        }

        protected override void OnResize(EventArgs e)
        {
            try
            {
                base.OnResize(e);
                SetCenterScreen(ToScreen(m_lastCenterPoint), false);
                m_lastCenterPoint = CenterPointUnit();
                m_staticImage = null;
                DoInvalidate(true);
            }
            catch (Exception ex)
            { /*throw ex;*/ }
        }
        #endregion

        #region 绘画图形元素
        public void DrawLine(ICanvas canvas, Pen pen, UnitPoint p1, UnitPoint p2)
        {
            try
            {
                PointF tmpp1 = ToScreen(p1);
                PointF tmpp2 = ToScreen(p2);
                canvas.Graphics.DrawLine(pen, tmpp1, tmpp2);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void DrawLandMark(ICanvas canvas, Brush pen, string code, UnitPoint Point)
        {
            try
            {
                if (Point.IsEmpty) { return; }
                if (canvas.Graphics == null) { return; }
                PointF pointScr = ToScreen(Point);
                float EllipsewidthScr = ToScreen(0.5);
                float EllipsewidthScrOffset = ToScreen(0.15);
                // canvas.Graphics.FillEllipse(pen, (float)pointScr.X, (float)pointScr.Y, EllipsewidthScr, EllipsewidthScr);

                Rectangle rec1 = new Rectangle((int)(pointScr.X - EllipsewidthScrOffset), (int)(pointScr.Y - EllipsewidthScrOffset), (int)EllipsewidthScr, (int)EllipsewidthScr);
                SolidBrush m_brush = new SolidBrush(Color.FromArgb(50, Color.White));


                canvas.Graphics.FillRectangle(m_brush, rec1);
                canvas.Graphics.DrawRectangle(new Pen(Color.Gray, 2), rec1);

                

                pen = Brushes.DarkBlue;
                //绘制地标编码
                float fontSize = ToScreen(0.1F);
                float recwidthScr = fontSize * (code.Length + 1);
                float recHeitScr = ToScreen(0.175F);
                float rechalfScr = EllipsewidthScr / 2 - recwidthScr / 2;
                StringFormat f = new StringFormat();
                f.Alignment = StringAlignment.Center;
                Rectangle rec = new Rectangle((int)(pointScr.X - EllipsewidthScrOffset + rechalfScr), (int)(pointScr.Y - recHeitScr), (int)recwidthScr, (int)recHeitScr);
                Font font = new Font("宋体", fontSize);
                canvas.Graphics.DrawString(code, font, pen, rec, f);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void DrawImge(ICanvas canvas, Pen pen, UnitPoint Location, float Widht, float Hight, Image img, string values)
        {
            try
            {
                if (Location.IsEmpty) { return; }
                if (canvas.Graphics != null)
                {
                    PointF point = ToScreen(Location);
                    float width = ToScreen(Widht / 96);
                    float hight = ToScreen(Hight / 96);
                    canvas.Graphics.DrawRectangle(pen, point.X, point.Y, width, hight);
                    canvas.Graphics.DrawImage(img, new RectangleF(point, new Size((int)width, (int)hight)));
                    if (!string.IsNullOrEmpty(values))
                    {
                        StringFormat f = new StringFormat();
                        f.Alignment = StringAlignment.Center;
                        float fontSize = 0;
                        float deviation_x = 0;
                        float deviation_y = 0;
                        if (values.Length == 1)
                        {
                            fontSize = ToScreen(30F / 96);
                            deviation_x = -ToScreen(25F / 96);
                        }
                        else if (values.Length == 2)
                        {
                            fontSize = ToScreen(30F / 96);
                            deviation_x = -ToScreen(27F / 96);
                        }
                        else
                        {
                            fontSize = ToScreen(20F / 96);
                            deviation_x = -ToScreen(27F / 96);
                            deviation_y = -ToScreen(10F / 96);
                        }
                        Font m_font = new System.Drawing.Font("Arial Black", fontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        SolidBrush m_brush = new SolidBrush(Color.White);
                        canvas.Graphics.DrawString(values, m_font, m_brush, point.X - deviation_x, point.Y - deviation_y, f);
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void DrawTxt(ICanvas canvas, string code, UnitPoint Point, int FontSize, Color fontColor)
        {
            try
            {
                if (Point.IsEmpty) { return; }
                if (canvas.Graphics != null)
                {
                    PointF point = ToScreen(Point);
                    StringFormat f = new StringFormat();
                    f.Alignment = StringAlignment.Near;
                    float fontSize = ToScreen((float)FontSize / 96);
                    Font m_font = new System.Drawing.Font("Arial Black", fontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    SolidBrush m_brush = new SolidBrush(fontColor);
                    canvas.Graphics.DrawString(code, m_font, m_brush, point.X, point.Y, f);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void DrawBizer(ICanvas canvas, Pen pen, UnitPoint p1, UnitPoint p2, UnitPoint p3, UnitPoint p4)
        {
            try
            {
                PointF point1 = ToScreen(p1);
                PointF point2 = ToScreen(p2);
                PointF point3 = ToScreen(p3);
                PointF point4 = ToScreen(p4);
                if (canvas.Graphics != null)
                {
                    canvas.Graphics.DrawBezier(pen, point1, point2, point3, point4);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void DrawStorage(ICanvas canvas, Brush Pen, string code, UnitPoint Point)
        {
            try
            {
                if (Point.IsEmpty) { return; }
                if (canvas.Graphics == null) { return; }
                PointF pointScr = ToScreen(Point);
                float EllipsewidthScr = ToScreen(0.5);
                float EllipsewidthScrOffset = ToScreen(0.25);
                Rectangle rec = new Rectangle((int)(pointScr.X - EllipsewidthScrOffset), (int)(pointScr.Y - EllipsewidthScrOffset), (int)EllipsewidthScr, (int)EllipsewidthScr);
                canvas.Graphics.FillRectangle(Pen, rec);

                //画十字
                int x1 = (int)(pointScr.X - EllipsewidthScrOffset);
                int y1 = (int)(pointScr.Y - EllipsewidthScrOffset);
                Pen xPen = new System.Drawing.Pen(Color.Black,4);
                float linesrc = ToScreen(0.49);
                canvas.Graphics.DrawLine(xPen, x1, y1,x1+ (int)linesrc, y1+ (int)linesrc);
                canvas.Graphics.DrawLine(xPen, x1, y1 + (int)linesrc, x1 + (int)linesrc, y1 );


                //绘制储位编码
                float fontSize = ToScreen(0.1F);
                float recwidthScr = fontSize * code.Length + fontSize;
                float recheightScr = ToScreen(0.15F);
                float rechalfScr = EllipsewidthScr / 2 - recwidthScr / 2;
                StringFormat f = new StringFormat();
                f.Alignment = StringAlignment.Center;
                Font font = new Font("宋体", fontSize);
                Brush StrPen = Brushes.DarkGray;
                Rectangle recText = new Rectangle((int)(x1 + rechalfScr), (int)(pointScr.Y + ToScreen(0.125F)), (int)recwidthScr, (int)recheightScr);
                canvas.Graphics.DrawString(code, font, StrPen, recText, f);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void DrawPosition(ICanvas canvas, Brush Pen, string code, UnitPoint Point)
        {
            try
            {
                if (Point.IsEmpty) { return; }
                if (canvas.Graphics == null) { return; }
                PointF pointScr = ToScreen(Point);
                float EllipsewidthScr = ToScreen(0.5);
                float EllipseheightScr = ToScreen(0.3);
                Rectangle rec = new Rectangle((int)pointScr.X, (int)pointScr.Y, (int)EllipsewidthScr, (int)EllipseheightScr);
                canvas.Graphics.FillRectangle(Pen, rec);
                //绘制储位编码
                float fontSize = ToScreen(0.1F);
                float recwidthScr = fontSize * code.Length + fontSize;
                float recheightScr = ToScreen(0.15F);
                float rechalfScr = EllipsewidthScr / 2 - recwidthScr / 2;
                StringFormat f = new StringFormat();
                f.Alignment = StringAlignment.Center;
                Font font = new Font("宋体", fontSize);
                Brush StrPen = Brushes.Gray;
                Rectangle recText = new Rectangle((int)(pointScr.X + rechalfScr), (int)(pointScr.Y + ToScreen(0.1F)), (int)recwidthScr, (int)recheightScr);
                canvas.Graphics.DrawString(code, font, StrPen, recText, f);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void DrawBtnBox(ICanvas canvas, float Radius, UnitPoint Point, bool Selected)
        {
            try
            {
                if (Point.IsEmpty) { return; }
                if (canvas.Graphics == null) { return; }
                PointF pointScr = ToScreen(Point);
                float EllipsewidthScr = ToScreen(Radius);
                Color color = Color.FromArgb(47, 79, 79);
                Brush pen = new SolidBrush(color);
                canvas.Graphics.FillEllipse(pen, (float)pointScr.X, (float)pointScr.Y, EllipsewidthScr, EllipsewidthScr);
                color = Color.FromArgb(105, 105, 105);
                pen = new SolidBrush(color);
                if (Selected)
                { pen = Brushes.Magenta; }
                EllipsewidthScr -= ToScreen(0.2F);
                if (EllipsewidthScr > 0)
                { canvas.Graphics.FillEllipse(pen, (float)pointScr.X + ToScreen(0.1F), (float)pointScr.Y + ToScreen(0.1F), EllipsewidthScr, EllipsewidthScr); }

                pen = Brushes.DarkGray;
                float fontSize = ToScreen((Radius >= 1 ? 0.2F * Radius : 0.14F) - 0.05F);
                StringFormat f = new StringFormat();
                f.Alignment = StringAlignment.Center;
                Rectangle rec = new Rectangle((int)(ToScreen(Point).X + ToScreen(Radius / 3F)), (int)(ToScreen(Point).Y + ToScreen(Radius / 3F)), (int)ToScreen(Radius / 3F), (int)ToScreen(Radius / 3F * 2));
                Font font = new Font("宋体", fontSize);
                canvas.Graphics.DrawString("呼叫", font, pen, rec, f);

            }
            catch (Exception ex)
            { throw ex; }
        }

        public void DrawAGV(ICanvas canvas, Pen pen, UnitPoint p, string Code)
        {
            try
            {
                PointF tmpp1 = ToScreen(p);
                SolidBrush brush = new System.Drawing.SolidBrush(Color.Turquoise);
                RectangleF rect = new RectangleF((float)tmpp1.X - ToScreen(0.14), (float)tmpp1.Y - ToScreen(0.14), ToScreen(0.47), ToScreen(0.47));
                if (!rect.Location.IsEmpty && !float.IsInfinity(rect.X) && !float.IsInfinity(rect.Y))
                {
                    Bitmap bmp = new Bitmap(canvas.ClientRectangle.Width, canvas.ClientRectangle.Height);
                    Graphics g = Graphics.FromImage(bmp);
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
                    using (GraphicsPath path = CreateRoundedRectanglePath(rect, (int)ToScreen(0.06)))
                    {

                        float fontSize = ToScreen(0.3F);
                        StringFormat f = new StringFormat();
                        f.Alignment = StringAlignment.Center;
                        Font font = new Font("宋体", fontSize);
                        Brush StrPen = Brushes.White;
                        float StrHeit = ToScreen(0.05) + (Code.Length - 1) * ToScreen(0.1);
                        float StrWidth = ToScreen(0.1);
                        {
                            if (path != null)
                            { g.FillPath(brush, path); }
                            
                            SolidBrush brushInner = new System.Drawing.SolidBrush(pen.Color);
                            rect = new RectangleF((float)tmpp1.X - ToScreen(0.12), (float)tmpp1.Y - ToScreen(0.12), ToScreen(0.42), ToScreen(0.42));
                            if (!rect.Location.IsEmpty && !float.IsInfinity(rect.X) && !float.IsInfinity(rect.Y))
                            {
                                g.FillRectangle(brushInner, rect);
                                //using (GraphicsPath pathInner = CreateRoundedRectanglePath(rect, (int)ToScreen(0)))
                                //{
                                //    if (pathInner != null)
                                //    { g.FillPath(brushInner, pathInner); }
                                //}
                                g.DrawString(Code, font, StrPen, new PointF((int)(tmpp1.X - StrHeit), (int)(tmpp1.Y - StrWidth)));
                            }
                        }
                        g.Dispose();
                        g = null;
                        canvas.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
                        canvas.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                        canvas.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
                        canvas.Graphics.DrawImage(bmp, 0, 0);
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void DrawArc(ICanvas canvas, Pen pen, UnitPoint center, float radius, float startAngle, float sweepAngle)
        {
            try
            {
                PointF p1 = ToScreen(center);
                radius = (float)Math.Round(ToScreen(radius));
                RectangleF r = new RectangleF(p1, new SizeF());
                r.Inflate(radius, radius);
                if (radius > 0 && radius < 1e8f)
                { canvas.Graphics.DrawArc(pen, r, -startAngle, -sweepAngle); }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 得到一个圆角矩形路径
        /// </summary>
        private GraphicsPath CreateRoundedRectanglePath(RectangleF rect, int cornerRadius)
        {
            try
            {
                GraphicsPath roundedRect = new GraphicsPath();
                roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
                roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
                roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
                roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
                roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
                roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
                roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
                roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
                roundedRect.CloseFigure();
                return roundedRect;
            }
            catch
            { }
            return null;
            //{ throw ex; }
        }
        #endregion

        #region ICanvas接口实现
        /// <summary>
        /// 将屏幕中点坐标转换成画布重点坐标
        /// </summary>
        public UnitPoint CenterPointUnit()
        {
            try
            {
                UnitPoint p1 = ScreenTopLeftToUnitPoint();
                UnitPoint p2 = ScreenBottomRightToUnitPoint();
                UnitPoint center = new UnitPoint();
                center.X = (p1.X + p2.X) / 2;
                center.Y = (p1.Y + p2.Y) / 2;
                return center;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 屏幕左上角的画布坐标
        /// </summary>
        public UnitPoint ScreenTopLeftToUnitPoint()
        {
            return ToUnit(new PointF(0, 0));
        }

        /// <summary>
        /// 屏幕右下角的画布坐标
        /// </summary>
        public UnitPoint ScreenBottomRightToUnitPoint()
        {
            return ToUnit(new PointF(this.ClientRectangle.Width, this.ClientRectangle.Height));
        }

        /// <summary>
        /// 将屏幕长度转换成画布长度
        /// </summary>
        public float ToScreen(double value)
        {
            return (float)Math.Round((value * m_screenResolution * m_model.Zoom), 5);
        }

        /// <summary>
        /// 添加快捷方式
        /// </summary>
        Dictionary<Keys, Type> m_QuickSnap = new Dictionary<Keys, Type>();
        public void AddQuickSnapType(Keys key, Type snaptype)
        {
            m_QuickSnap.Add(key, snaptype);
        }

        /// <summary>
        /// 执行绘画命令
        /// </summary>
        public void CommandSelectDrawTool(string drawobjectid)
        {
            try
            {
                CommandEscape();
                m_model.ClearSelectedObjects();
                m_commandType = eCommandType.draw;
                m_drawObjectId = drawobjectid;
                UpdateCursor();
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// ESC命令
        /// </summary>
        public void CommandEscape()
        {
            try
            {
                bool dirty = (m_newObject != null) || (m_snappoint != null);
                m_newObject = null;
                m_snappoint = null;
                if (m_editTool != null)
                    m_editTool.Finished();
                m_editTool = null;
                m_commandType = eCommandType.select;
                m_moveHelper.HandleCancelMove();
                m_nodeMoveHelper.HandleCancelMove();
                DoInvalidate(dirty);
                UpdateCursor();
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 抓手命令
        /// </summary>
        public void CommandPan()
        {
            try
            {
                //if (m_commandType == eCommandType.select || m_commandType == eCommandType.move)
                m_commandType = eCommandType.pan;
                UpdateCursor();
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 移动命令
        /// </summary>
        public void CommandMove(bool handleImmediately)
        {
            try
            {
                if (m_model.SelectedCount > 0)
                {
                    if (handleImmediately && m_commandType == eCommandType.move)
                    { m_moveHelper.HandleMouseDownForMove(GetMousePoint(), m_snappoint); }
                    m_commandType = eCommandType.move;
                    UpdateCursor();
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void CommandDeleteSelected()
        {
            try
            {
                m_model.DeleteObjects(m_model.SelectedObjects);
                m_model.ClearSelectedObjects();
                DoInvalidate(true);
                UpdateCursor();
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 编辑命令
        /// </summary>
        public void CommandEdit(string editid)
        {
            try
            {
                CommandEscape();
                m_model.ClearSelectedObjects();
                m_commandType = eCommandType.edit;
                m_editToolId = editid;
                m_editTool = m_model.GetEditTool(m_editToolId);
                UpdateCursor();
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 根据快捷键处理吸附焦点
        /// </summary>
        public void HandleQuickSnap(KeyEventArgs e)
        {
            try
            {
                if (m_commandType == eCommandType.select || m_commandType == eCommandType.pan)
                    return;
                ISnapPoint p = null;
                UnitPoint mousepoint = GetMousePoint();
                if (m_QuickSnap.ContainsKey(e.KeyCode))
                    p = m_model.SnapPoint(m_canvaswrapper, mousepoint, null, m_QuickSnap[e.KeyCode]);
                if (p != null)
                {
                    if (m_commandType == eCommandType.draw)
                    {
                        HandleMouseDownWhenDrawing(p.SnapPoint, p);
                        if (m_newObject != null)
                            m_newObject.OnMouseMove(m_canvaswrapper, GetMousePoint());
                        DoInvalidate(true);
                        e.Handled = true;
                    }
                    if (m_commandType == eCommandType.move)
                    {
                        m_moveHelper.HandleMouseDownForMove(p.SnapPoint, p);
                        e.Handled = true;
                    }
                    if (m_nodeMoveHelper.IsEmpty == false)
                    {
                        bool handled = false;
                        m_nodeMoveHelper.HandleMouseDown(p.SnapPoint, ref handled);
                        FinishNodeEdit();
                        e.Handled = true;
                    }
                    if (m_commandType == eCommandType.edit)
                    {
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 重载控件键盘事件
        /// </summary>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (IsChooseSpecial) { return; }
            HandleQuickSnap(e);
            if (m_nodeMoveHelper.IsEmpty == false)
            {
                m_nodeMoveHelper.OnKeyDown(m_canvaswrapper, e);
                if (e.Handled)
                    return;
            }
            base.OnKeyDown(e);
            if (e.Handled)
            {
                UpdateCursor();
                return;
            }
            if (m_editTool != null)
            {
                m_editTool.OnKeyDown(m_canvaswrapper, e);
                if (e.Handled)
                    return;
            }
            if (m_newObject != null)
            {
                m_newObject.OnKeyDown(m_canvaswrapper, e);
                if (e.Handled)
                    return;
            }
            foreach (IDrawObject obj in m_model.SelectedObjects)
            {
                obj.OnKeyDown(m_canvaswrapper, e);
                if (e.Handled)
                    return;
            }

            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                if (e.KeyCode == Keys.G)
                {
                    m_model.GridLayer.Enabled = !m_model.GridLayer.Enabled;
                    DoInvalidate(true);
                }
                if (e.KeyCode == Keys.C)
                {
                    RunningSnapsEnabled = !RunningSnapsEnabled;
                    if (!RunningSnapsEnabled)
                        m_snappoint = null;
                    DoInvalidate(false);
                }
                return;
            }

            if (e.KeyCode == Keys.Escape)
            {
                CommandEscape();
            }
            if (e.KeyCode == Keys.P)
            {
                CommandPan();
            }
            if (e.KeyCode == Keys.S)
            { CommandEscape(); }
            if (e.KeyCode == Keys.M)
            { CommandMove(true); }
            if (e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D9)
            {
                int layerindex = (int)e.KeyCode - (int)Keys.D1;
                if (layerindex >= 0 && layerindex < m_model.Layers.Length)
                {
                    m_model.ActiveLayer = m_model.Layers[layerindex];
                    DoInvalidate(true);
                }
            }
            if (e.KeyCode == Keys.Delete)
            {
                CommandDeleteSelected();
            }
            if (e.KeyCode == Keys.O)
            {
                CommandEdit("linesmeet");
            }
            UpdateCursor();
        }
        #endregion

        #region 自定义函数
        public SizeF GetStrValueSize(string Str, Font f)
        {
            try
            {
                Bitmap bmp = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
                Graphics graph = Graphics.FromImage(bmp);
                SizeF size = graph.MeasureString(Str, f);
                return size;
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion
    }
}
