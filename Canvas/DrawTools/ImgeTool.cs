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
    public class ImgeTool : DrawObjectBase, IDrawObject, INodePoint, ISerialize
    {
        #region 属性
        private UnitPoint location = UnitPoint.Empty;
        private string imageStr = "";
        private Image img;
        private Color transcolor = Color.Transparent;
        private string upValue = "";
        private float height = 100;
        private float width = 100;
        private bool isviewable = true;
        protected static int ThresholdPixel = 0;

        [XmlSerializable]
        [Description("位置坐标")]
        /// <summary>
        public UnitPoint Location
        {
            get { return location; }
            set { location = value; }
        }

        [Browsable(false)]
        [XmlSerializable]
        public string ImageStr
        {
            get
            {
                return imageStr;
            }
            set
            {
                imageStr = value;
                if (!string.IsNullOrEmpty(ImageStr))
                {
                    byte[] imageBytes = Convert.FromBase64String(ImageStr);
                    System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(imageBytes, 0, imageBytes.Length);
                    memoryStream.Write(imageBytes, 0, imageBytes.Length);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(memoryStream);
                    img = image;
                }
            }
        }

        [Description("图片")]
        public Image Imge
        {
            get
            {
                return img;
            }
            set
            {
                img = value;
                if (img != null)
                {
                    System.IO.MemoryStream mstream = new System.IO.MemoryStream();
                    img.Save(mstream, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byData = new Byte[mstream.Length];
                    mstream.Position = 0;
                    mstream.Read(byData, 0, byData.Length);
                    mstream.Close();
                    imageStr = Convert.ToBase64String(byData);
                }
            }
        }


        [Description("图片透明背景色")]
        [XmlSerializable]
        public Color TransColor
        {
            get
            { return transcolor; }
            set
            { transcolor = value; }
        }

        [Browsable(false)]
        public string UpValue
        {
            get { return upValue; }
            set { upValue = value; }
        }

        [XmlSerializable]
        [Description("高度")]
        public float Height
        {
            get { return height; }
            set
            {
                height = value;
            }
        }

        [XmlSerializable]
        [Description("宽度")]
        public float Wideth
        {
            get { return width; }
            set
            { width = value; }
        }

        [Browsable(false)]
        public bool IsViewable
        {
            get { return isviewable; }
            set { isviewable = value; }
        }

        [Browsable(false)]
        /// <summary>
        /// 当前对象的类型
        /// </summary>
        public static string ObjectType
        {
            get { return "ImgeTool"; }
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

        public void GetObjectData(XmlWriter wr)
        {
            try
            {
                wr.WriteStartElement("ImgeTool");
                XmlUtil.WriteProperties(this, wr);
                wr.WriteEndElement();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void AfterSerializedIn()
        { }


        public virtual IDrawObject Clone()
        {
            try
            {
                ImgeTool imgeTool = new ImgeTool();
                imgeTool.Copy(this);
                return imgeTool;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual void Copy(ImgeTool acopy)
        {
            try
            {
                base.Copy(acopy);
                location = acopy.location;
                img = acopy.img;
                imageStr = acopy.imageStr;
                width = acopy.width;
                height = acopy.height;
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
                float thWidth = LineTool.ThresholdWidth(canvas, Width, ThresholdPixel);
                if (thWidth < Width)
                    thWidth = Width;
                float ProduceWidth = canvas.ToScreen(width / 96);
                float ProduceHight = canvas.ToScreen(height / 96);
                UnitPoint Aqlocation = new UnitPoint(location.X + canvas.ToUnit(ProduceWidth), location.Y - canvas.ToUnit(ProduceHight));
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
        /// 判断左边点是否在图形元素上
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
        /// 判断当前图形元素是否在一个矩形区域内
        /// </summary>
        public bool ObjectInRectangle(ICanvas canvas, RectangleF rect, bool anyPoint)
        {
            try
            {
                RectangleF boundingrect = GetBoundingRect(canvas);
                if (anyPoint)
                {
                    float ProduceWidth = canvas.ToScreen(width / 96);
                    float ProduceHight = canvas.ToScreen(height / 96);
                    UnitPoint Aqlocation = new UnitPoint(location.X + canvas.ToUnit(ProduceWidth), location.Y - canvas.ToUnit(ProduceHight));
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
                if (!isviewable) { return; }
                Pen pen = new Pen(Color.Transparent, 1);
                if (img == null)
                {
                    img = global::Canvas.Properties.Resources._default;
                    return;
                }
                (img as Bitmap).MakeTransparent(transcolor);
                canvas.DrawImge(canvas, pen, location, Wideth, height, img, UpValue);
                if (Selected)
                {
                    canvas.DrawImge(canvas, Utils.DrawUtils.SelectedPen, location, Wideth, height, img, UpValue);
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
                if (!isviewable) { return eDrawObjectMouseDownEnum.Done; }
                Selected = false;
                location = point;
                Pen pen = new Pen(Color.Transparent, 1);
                if (img == null)
                {
                    img = global::Canvas.Properties.Resources._default;
                    //img = new Bitmap(imgpath);
                    return eDrawObjectMouseDownEnum.Done;
                }
                (img as Bitmap).MakeTransparent(transcolor);
                canvas.DrawImge(canvas, pen, location, Wideth, height, img, UpValue);
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
        { return "ImageTool"; }

        public void SetPosition(UnitPoint pos)
        { this.Location = pos; }

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
        #endregion
    }
}
