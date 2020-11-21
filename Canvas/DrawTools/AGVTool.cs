using Canvas.CanvasInterfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Canvas.CanvasCtrl;
using System.Drawing.Drawing2D;
using System.Xml;
using Canvas.Layers;

namespace Canvas.DrawTools
{
    public class AGVTool : DrawObjectBase, IDrawObject, INodePoint, ISerialize
    {
        #region 属性
        protected UnitPoint position;
        protected string agv_id = "";
        private bool isviewable = true;

        protected static int ThresholdPixel = 15;

        public int HandState { get; set; }

        public string Agv_id
        {
            get { return agv_id; }
            set { agv_id = value; }
        }

        public static string ObjectType
        {
            get { return "AGVTool"; }
        }

        public string Id
        {
            get { return ObjectType; }
        }

        public UnitPoint Position
        {
            get { return position; }
            set { position = value; }
        }

        public UnitPoint RepeatStartingPoint
        {
            get { return position; }
        }

        public bool IsViewable
        {
            get { return isviewable; }
            set { isviewable = value; }
        }
        #endregion


        public void AfterSerializedIn()
        { }

        public void Cancel()
        { }

        public IDrawObject Clone()
        {
            AGVTool AGV = new AGVTool();
            AGV.Copy(this);
            return AGV;
        }

        public void Draw(ICanvas canvas, RectangleF unitrect)
        {
            try
            {
                Pen pen;
                if (!isviewable) { return; }
                if (HandState == 0)
                {
                    pen = new Pen(Color.Gray);
                }
                else
                {
                    pen = new Pen(Color.Red);
                }
               // pen.DashStyle = DashStyle.Custom;
                canvas.DrawAGV(canvas, pen, position, Agv_id);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Finish()
        { }

        public RectangleF GetBoundingRect(ICanvas canvas)
        {
            float thWidth = LineTool.ThresholdWidth(canvas, Width, ThresholdPixel);
            return ScreenUtils.GetRect(new UnitPoint(position.X - 0.2, position.Y + 0.2), new UnitPoint(position.X + 0.2, position.Y - 0.2), thWidth);
        }

        public IDrawObject GetClone()
        { return null; }

        public string GetInfoAsString()
        { return ""; }

        public void GetObjectData(XmlWriter wr)
        { }

        public IDrawObject GetOriginal()
        { return null; }

        public override void InitializeFromModel(UnitPoint point, DrawingLayer layer, ISnapPoint snap)
        {
            Width = layer.Width;
            Color = layer.Color;
            Selected = true;
        }

        public void Move(UnitPoint offset)
        {
            try
            {
                position.X += offset.X;
                position.Y += offset.Y;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public INodePoint NodePoint(ICanvas canvas, UnitPoint point)
        { return null; }

        public bool ObjectInRectangle(ICanvas canvas, RectangleF rect, bool anyPoint)
        {
            RectangleF boundingrect = GetBoundingRect(canvas);
            if (anyPoint)
                return HitUtil.LineIntersectWithRect(new UnitPoint(position.X - 0.2, position.Y + 0.2), new UnitPoint(position.X + 0.2, position.Y - 0.2), rect);
            return rect.Contains(boundingrect);
        }

        public void OnKeyDown(ICanvas canvas, KeyEventArgs e)
        { }

        public eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        { return eDrawObjectMouseDownEnum.Done; }

        public void OnMouseMove(ICanvas canvas, UnitPoint point)
        { }

        public void OnMouseUp(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        { }

        public bool PointInObject(ICanvas canvas, UnitPoint point)
        {
            RectangleF boundingrect = GetBoundingRect(canvas);
            if (boundingrect.Contains(point.Point) == false)
                return false;
            else
            { return true; }
        }

        public void Redo()
        { }

        public void SetPosition(UnitPoint pos)
        { }

        public ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, List<IDrawObject> otherobj, Type[] runningsnaptypes, Type usersnaptype)
        { return null; }

        public void Undo()
        { }
    }
}
