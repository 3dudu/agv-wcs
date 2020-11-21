using Canvas.CanvasInterfaces;
using Canvas.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Canvas.DrawTools
{
    public class TextTool : DrawObjectBase, IDrawObject, INodePoint, ISerialize
    {
        #region 属性
        private int font_size = 9;
        private string str_value = "输入文字...";
        private UnitPoint location = UnitPoint.Empty;
        private string targetobjcode = "";
        protected static int ThresholdPixel = 0;


        [XmlSerializable]
        [Description("字体大小")]
        public int FontSize
        {
            get { return font_size; }
            set { font_size = value; }
        }

        [XmlSerializable]
        [Description("显示文字")]
        public string StrValue
        {
            get { return str_value; }
            set { str_value = value; }
        }

        [XmlSerializable]
        [Browsable(false)]
        public UnitPoint Location
        {
            get { return location; }
            set { location = value; }
        }

        [Browsable(false)]
        public static string ObjectType
        {
            get { return "TextTool"; }
        }

        [Browsable(false)]
        public virtual string Id
        {
            get { return ObjectType; }
        }

        [Browsable(false)]
        public UnitPoint RepeatStartingPoint
        {
            get { return location; }
        }

        [XmlSerializable]
        [Browsable(false)]
        [Description("所描述对象编码")]
        public string TargetObjCode
        {
            get { return targetobjcode; }
            set { targetobjcode = value; }
        }
        #endregion

        #region 函数方法
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

        /// <summary>
        /// 还原xml文件内的数据
        /// </summary>
        public void GetObjectData(XmlWriter wr)
        {
            try
            {
                wr.WriteStartElement("TextTool");
                XmlUtil.WriteProperties(this, wr);
                wr.WriteEndElement();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void AfterSerializedIn()
        { }

        /// <summary>
        /// 克隆一个对象
        /// </summary>
        public virtual IDrawObject Clone()
        {
            try
            {
                TextTool txtTool = new TextTool();
                txtTool.Copy(this);
                return txtTool;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 拷贝对象
        /// </summary>
        public virtual void Copy(TextTool acopy)
        {
            try
            {
                base.Copy(acopy);
                Location = acopy.Location;
                FontSize = acopy.FontSize;
                StrValue = acopy.StrValue;
                Selected = acopy.Selected;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 得到当前图形元素的矩形区域
        /// </summary>
        public RectangleF GetBoundingRect(ICanvas canvas)
        {
            try
            {
                //Font m_font = new System.Drawing.Font("Arial Black", FontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                //SizeF size= canvas.GetStrValueSize(StrValue, m_font);
                //UnitPoint aqlocation = new UnitPoint(location.X + (float)canvas.ToUnit(size.Width), location.Y - (float)canvas.ToUnit(size.Height));
                //return ScreenUtils.GetRect(location, aqlocation, 0);


                float thWidth = LineTool.ThresholdWidth(canvas, Width, ThresholdPixel);
                if (thWidth < Width)
                    thWidth = Width;
                UnitPoint Aqlocation = new UnitPoint(location.X + canvas.ToUnit((float)10 * str_value.Length), location.Y - canvas.ToUnit(10F));
                return ScreenUtils.GetRect(location, Aqlocation, thWidth);
            }
            catch (Exception ex)
            { throw ex; }
        }

        UnitPoint MidPoint(ICanvas canvas, UnitPoint p1, UnitPoint p2, UnitPoint hitpoint)
        {
            return UnitPoint.Empty;
        }

        /// <summary>
        /// 用于判断当前元素是否被选中
        /// </summary>
        public virtual bool PointInObject(ICanvas canvas, UnitPoint point)
        {
            try
            {
                RectangleF boundingrect = GetBoundingRect(canvas);
                if (boundingrect.Contains(point.Point) == false)
                { return false; }
                else
                { return true; }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 判断当前元素是否被选中
        /// </summary>
        public bool ObjectInRectangle(ICanvas canvas, RectangleF rect, bool anyPoint)
        {
            try
            {
                RectangleF boundingrect = GetBoundingRect(canvas);
                if (anyPoint)
                {
                    UnitPoint Aqlocation = new UnitPoint(location.X + canvas.ToUnit((float)10 * str_value.Length), location.Y);
                    if (HitUtil.LineIntersectWithRect(location, Aqlocation, rect))
                    { return true; }
                    Aqlocation = new UnitPoint(location.X, location.Y - canvas.ToUnit((float)10));
                    return HitUtil.LineIntersectWithRect(location, Aqlocation, rect);
                }
                return rect.Contains(boundingrect);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual void Draw(ICanvas canvas, RectangleF unitrect)
        {
            try
            {
                canvas.DrawTxt(canvas, StrValue, Location, FontSize, Color);
                if (Selected)
                {
                    canvas.DrawTxt(canvas, StrValue, Location, FontSize, Utils.DrawUtils.SelectedPen.Color);
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
                Selected = false;
                location = point;
                canvas.DrawTxt(canvas, StrValue, location, FontSize, Color);
                if (Selected)
                {
                    canvas.DrawTxt(canvas, StrValue, location, FontSize, Utils.DrawUtils.SelectedPen.Color);
                }
                return eDrawObjectMouseDownEnum.Done;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void OnMouseUp(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        { }

        public INodePoint NodePoint(ICanvas canvas, UnitPoint point)
        { return null; }

        public ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, List<IDrawObject> otherobjs, Type[] runningsnaptypes, Type usersnaptype)
        { return null; }

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

        public string GetInfoAsString()
        {
            return string.Format("TextTool:@{0}",
                 StrValue);
        }

        public void SetPosition(UnitPoint pos)
        {
            try
            {
                this.Location = pos;
            }
            catch (Exception ex)
            { throw ex; }
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
        public void OnKeyDown(ICanvas canvas, System.Windows.Forms.KeyEventArgs e)
        { }
        #endregion
    }
}
