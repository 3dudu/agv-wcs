using Canvas.CanvasInterfaces;
using Canvas.Layers;
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
    public class LandMarkTool : DrawObjectBase, IDrawObject, INodePoint, ISerialize
    {
        #region 属性
        protected UnitPoint location = UnitPoint.Empty;
        protected UnitPoint aqlocation = UnitPoint.Empty;
        protected UnitPoint midpoint = UnitPoint.Empty;
        protected string landcode = "1";
        protected string landname = "";
        protected static int ThresholdPixel =0;
        protected bool allowModify = true;

        [XmlSerializable]
        [Browsable(false)]
        public UnitPoint Location
        {
            get{return location;}
            set{ location = value; }
        }

        /// <summary>
        /// 当前坐标
        /// </summary>
        [Description("地标坐标")]
        public UnitPoint CoorDinate
        {
            get { return location; }
        }


        [XmlSerializable]
        [Description("地标编码")]
        public string LandCode
        {
            get { return landcode; }
            set { landcode = value; }
        }


        [XmlSerializable]
        [Description("地标名称")]
        public string LandName
        {
            get { return landname; }
            set
            { landname = value; }
        }

        [Browsable(false)]
        public static string ObjectType
        {
            get { return "LandMark"; }
        }

        [Browsable(false)]
        public virtual string Id
        {
            get { return ObjectType; }
        }

        [Browsable(false)]
        public UnitPoint RepeatStartingPoint
        {
            get { return Location; }
        }

        [Description("地标中心点")]
        public UnitPoint MidPoint
        {
            get
            {
                midpoint = new UnitPoint(Location.X + 0.1, Location.Y - 0.1);
                return midpoint;
            }
        }
        #endregion

        #region 重载和方法
        public LandMarkTool()
        { Width = 0.2F; }

        public LandMarkTool(string code)
        {
            landcode = code;
            Selected = false;
        }

        public override void InitializeFromModel(UnitPoint point, DrawingLayer layer, ISnapPoint snap)
        {
            Width = layer.Width;
            Color = layer.Color;
            Selected = true;
        }

        public void SetPosition(UnitPoint pos)
        {
            this.Location = pos;
        }
        public IDrawObject GetClone()
        { return null; }

        public IDrawObject GetOriginal()
        { return null; }

        public void Finish()
        { }
        public void Cancel()
        { }
        public void Undo()
        { }
        public void Redo()
        { }
        public void OnKeyDown(ICanvas canvas, KeyEventArgs e)
        { }

        public void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement("LandMark");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public void AfterSerializedIn() { }

        public virtual IDrawObject Clone()
        {
            LandMarkTool land = new LandMarkTool();
            land.Copy(this);
            return land;
        }

        public virtual void Copy(LandMarkTool acopy)
        {
            base.Copy(acopy);
            Location = acopy.Location;
            LandCode = acopy.LandCode;
            LandName = acopy.LandName;
            Selected = acopy.Selected;
        }

        public RectangleF GetBoundingRect(ICanvas canvas)
        {
            try
            {
                double thWidth = canvas.ToUnit(ThresholdPixel);
                //if (thWidth < Width)
                //    thWidth = Width;
                float PriWidth = canvas.ToScreen(0.2F);
                aqlocation = new UnitPoint(Location.X + canvas.ToUnit(PriWidth), Location.Y - canvas.ToUnit(PriWidth));
                return ScreenUtils.GetRect(Location, aqlocation, thWidth);
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 判断一个点是否在一个元素上
        /// </summary>
        public virtual bool PointInObject(ICanvas canvas, UnitPoint point)
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

        /// <summary>
        /// 判断一个元素是否在一个举行区域内
        /// </summary>
        public bool ObjectInRectangle(ICanvas canvas, RectangleF rect, bool anyPoint)
        {
            try
            {
                RectangleF boundingrect = GetBoundingRect(canvas);
                if (anyPoint)
                    return HitUtil.LineIntersectWithRect(Location, aqlocation, rect);
                return rect.Contains(boundingrect);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Draw(ICanvas canvas, RectangleF unitrect)
        {
            try
            {
                Brush pen = Brushes.DarkRed;
                canvas.DrawLandMark(canvas, pen, LandCode, Location);
                if (Selected)
                {
                    Brush Choosepen = Brushes.Magenta;
                    canvas.DrawLandMark(canvas, Choosepen, LandCode, Location);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual void OnMouseMove(ICanvas canvas, UnitPoint point)
        { }

        public virtual eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        {
            try
            {
                int MaxValue = 1;
                if (canvas.DataModel.ActiveLayer.Objects.Count() > 0)
                {
                    if (canvas.DataModel.ActiveLayer.Objects.Where(p => p.Id == "LandMark").Count() > 0)
                    {
                        MaxValue = canvas.DataModel.ActiveLayer.Objects.Where(p => p.Id == "LandMark").Max(p => Convert.ToInt32((p as LandMarkTool).LandCode));
                        LandCode = (MaxValue + 1).ToString();
                    }
                }
                Selected = false;
                location = point;
                Brush pen = Brushes.DarkRed;
                UnitPoint offset = new UnitPoint(-0.1, 0.1);
                Move(offset);
                canvas.DrawLandMark(canvas, pen, LandCode, Location);
                return eDrawObjectMouseDownEnum.Done;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void OnMouseUp(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        { }


        public INodePoint NodePoint(ICanvas canvas, UnitPoint point)
        {
            return null;
        }

        public ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, List<IDrawObject> otherobjs, Type[] runningsnaptypes, Type usersnaptype)
        {
            return new MidpointSnapPoint(canvas, this, new UnitPoint(Location.X + 0.1, Location.Y - 0.1));
        }

        public void Move(UnitPoint offset)
        {
            location.X += offset.X;
            location.Y += offset.Y;
        }


        public string GetInfoAsString()
        {
            return string.Format("LandMark:@{0}",
                LandCode);
        }
        #endregion
    }//end
}
