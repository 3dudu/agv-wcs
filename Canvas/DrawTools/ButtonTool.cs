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
    public class ButtonTool : DrawObjectBase, IDrawObject, INodePoint, ISerialize
    {
        #region 属性
        protected UnitPoint location = UnitPoint.Empty;
        protected UnitPoint centerpoint = UnitPoint.Empty;
        protected int ThresholdPixel = 0;
        protected float radius =1;
        protected int BoxID = 0;

        [XmlSerializable]
        public int CallBoxID
        {
            get { return BoxID; }
            set { BoxID = value; }
        }

        [XmlSerializable]
        [Browsable(false)]
        public UnitPoint Location
        {
            get { return location; }
            set { location = value; }
        }

        [Browsable(false)]
        public UnitPoint Centerpoint
        {
            get { return centerpoint; }
            set { centerpoint = value; }
        }

        [XmlSerializable]
        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        [Browsable(false)]
        public static string ObjectType
        {
            get { return "ButtonTool"; }
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
        #endregion

        #region 重载和方法
        public ButtonTool()
        { Width = 0.1F; Selected = false; }

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
            wr.WriteStartElement("ButtonTool");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public void AfterSerializedIn() { }

        public virtual IDrawObject Clone()
        {
            ButtonTool btnTool = new ButtonTool();
            btnTool.Copy(this);
            return btnTool;
        }

        public virtual void Copy(ButtonTool acopy)
        {
            base.Copy(acopy);
            Location = acopy.Location;
            Radius = acopy.Radius;
            Selected = acopy.Selected;
        }

        public RectangleF GetBoundingRect(ICanvas canvas)
        {
            try
            {
                double thWidth = canvas.ToUnit(ThresholdPixel);
                float PriWidth = canvas.ToScreen(this.radius);
                UnitPoint aqlocation = new UnitPoint(Location.X + canvas.ToUnit(PriWidth), Location.Y - canvas.ToUnit(PriWidth));
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
                {
                    float PriWidth = canvas.ToScreen(this.radius);
                    UnitPoint aqlocation = new UnitPoint(Location.X + canvas.ToUnit(PriWidth), Location.Y - canvas.ToUnit(PriWidth));
                    return HitUtil.LineIntersectWithRect(Location, aqlocation, rect);
                }
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
                canvas.DrawBtnBox(canvas, Radius, Location, Selected);
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
                Selected = false;
                location = point;
                Brush pen = Brushes.DarkRed;
                float PriWidth = canvas.ToScreen(this.radius);
                UnitPoint offset = new UnitPoint(-radius/2F, radius / 2F);
                Move(offset);
                canvas.DrawBtnBox(canvas, radius, Location, Selected);
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
            //float PriWidth = canvas.ToScreen(this.radius);
            //return new MidpointSnapPoint(canvas, this, new UnitPoint(Location.X + PriWidth, Location.Y - PriWidth));
            return null;
        }

        public void Move(UnitPoint offset)
        {
            location.X += offset.X;
            location.Y += offset.Y;
        }

        public string GetInfoAsString()
        {
            //return string.Format("LandMark:@{0}",
            //    LandCode);
            return "";
        }
        #endregion
    }
}
