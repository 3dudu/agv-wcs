using Canvas.CanvasInterfaces;
using Canvas.Layers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Canvas.DrawTools
{
    /// <summary>
    /// 货位图形
    /// </summary>
    public class PositionTool : DrawObjectBase, IDrawObject, INodePoint, ISerialize
    {
        #region 属性
        private int zoneid = 0;//库区号
        private int landid = 0;//巷道号
        private int rowid = 0;//排号
        private Int32 colid = 0;//列号
        private int layerid = 0;//层号
        private int maxtrays = 0;//最大托盘数
        private string description = "";//描述
        protected UnitPoint location = UnitPoint.Empty;//位置
        protected Int64 positionid = 1;

        protected UnitPoint aqlocation = UnitPoint.Empty;

        public int ZoneID
        {
            get { return zoneid; }
            set { zoneid = value; }
        }

        public int LandID
        {
            get { return landid; }
            set { landid = value; }
        }

        public int RowID
        {
            get { return rowid; }
            set { rowid = value; }
        }

        public Int32 ColId
        {
            get { return colid; }
            set { colid = value; }
        }

        public int LayerId
        {
            get { return layerid; }
            set { layerid = value; }
        }

        public int MaxTrays
        {
            get { return maxtrays; }
            set { maxtrays = value; }
        }

        public string DesCription
        {
            get { return description; }
            set { description = value; }
        }

        public UnitPoint Location
        {
            get { return location; }
            set { location = value; }
        }

        public Int64 PositionID
        {
            get { return positionid; }
            set { positionid = value; }
        }

        public int State { get; set; }

        public static string ObjectType
        {
            get { return "PositionTool"; }
        }

        public string Id
        { get { return ObjectType; } }

        public UnitPoint RepeatStartingPoint
        { get { return location; } }
        #endregion

        #region 其他属性

        #endregion


        #region 方法函数
        public PositionTool() { }
        public virtual void Copy(PositionTool acopy)
        {
            try
            {
                base.Copy(acopy);
                Location = acopy.Location;
                State = acopy.State;
                ZoneID = acopy.ZoneID;
                LandID = acopy.LandID;
                RowID = acopy.RowID;
                ColId = acopy.ColId;
                LayerId = acopy.LayerId;
                MaxTrays = acopy.MaxTrays;
                DesCription = acopy.DesCription;
                PositionID = acopy.PositionID;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual IDrawObject Clone()
        {
            try
            {
                PositionTool Storage = new PositionTool();
                Storage.Copy(this);
                return Storage;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Draw(ICanvas canvas, RectangleF unitrect)
        {
            try
            {
                Brush Pen = new SolidBrush(Color.White);
                Color StateColor = canvas.DataModel.GetStateColor(State);
                if (Selected)
                { Pen = Brushes.Magenta; }
                canvas.DrawPosition(canvas, Pen, PositionID.ToString(), Location);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public RectangleF GetBoundingRect(ICanvas canvas)
        {
            try
            {
                float PriWidth = canvas.ToScreen(0.5F);
                float PriHeight = canvas.ToScreen(0.3F);
                aqlocation = new UnitPoint(location.X + canvas.ToUnit(PriWidth), location.Y - canvas.ToUnit(PriHeight));
                return ScreenUtils.GetRect(location, aqlocation, 0);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public override void InitializeFromModel(UnitPoint point, DrawingLayer layer, ISnapPoint snap)
        {
            try
            {
                Width = layer.Width;
                Color = layer.Color;
                Selected = true;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Move(UnitPoint offset)
        {
            try
            {
                location.X += offset.X;
                location.Y += offset.Y;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public INodePoint NodePoint(ICanvas canvas, UnitPoint point)
        { return null; }

        public IDrawObject GetOriginal()
        { return null; }

        public bool ObjectInRectangle(ICanvas canvas, RectangleF rect, bool anyPoint)
        {
            try
            {
                RectangleF boundingrect = GetBoundingRect(canvas);
                if (anyPoint)
                { return HitUtil.LineIntersectWithRect(location, aqlocation, rect); }
                return rect.Contains(boundingrect);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void OnKeyDown(ICanvas canvas, KeyEventArgs e) { }

        public IDrawObject GetClone()
        { return null; }

        public string GetInfoAsString()
        { return ""; }

        public void Finish()
        { }

        public eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        {
            try
            {
                Int64 MaxID = 1;
                if (canvas.DataModel.ActiveLayer.Objects.Count() > 0)
                {
                    if (canvas.DataModel.ActiveLayer.Objects.Where(p => p.Id == "PositionTool").Count() > 0)
                    {
                        MaxID = canvas.DataModel.ActiveLayer.Objects.Where(p => p.Id == "PositionTool").Max(p => (p as PositionTool).PositionID);
                        PositionID = MaxID + 1;
                    }
                }
                Selected = false;
                location = point;
                Brush Pen = Brushes.White;
                canvas.DrawPosition(canvas, Pen, PositionID.ToString(), Location);
                return eDrawObjectMouseDownEnum.Done;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void OnMouseMove(ICanvas canvas, UnitPoint point)
        { }

        public void OnMouseUp(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        { }

        public bool PointInObject(ICanvas canvas, UnitPoint point)
        {
            try
            {
                RectangleF boundingrect = GetBoundingRect(canvas);
                if (boundingrect.Contains(point.Point) == false)
                    return false;
                else
                { return true; }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Redo() { }

        public void SetPosition(UnitPoint pos)
        {
            this.Location = pos;
        }

        public ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, List<IDrawObject> otherobj, Type[] runningsnaptypes, Type usersnaptype)
        { return null; }

        public void Undo()
        { }

        public void Cancel() { }

        public void AfterSerializedIn() { }

        public void GetObjectData(XmlWriter wr)
        {
            try
            {
                //wr.WriteStartElement("PositionTool");
                //XmlUtil.WriteProperties(this, wr);
                //wr.WriteEndElement();
            }
            catch (Exception ex)
            { throw ex; }
        }
        #endregion
    }
}
