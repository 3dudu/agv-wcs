using Canvas.CanvasInterfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Canvas.Layers
{
    /// <summary>
    /// 背景布局
    /// </summary>
    public class BackgroundLayer : ICanvasLayer, ISerialize
    {
        Font m_font = new System.Drawing.Font("Arial Black", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        SolidBrush m_brush = new SolidBrush(Color.FromArgb(50, 200, 200, 200));
        SolidBrush m_backgroundBrush;

        //画布背景颜色
        Color m_color = Color.LightGray;
        [XmlSerializable]
        public Color Color
        {
            get { return m_color; }
            set
            {
                m_color = value;
                m_backgroundBrush = new SolidBrush(m_color);
            }
        }

        public BackgroundLayer()
        {
            m_backgroundBrush = new SolidBrush(m_color);
            Enabled = true;
            Visible = true;
        }


        #region 实现接口ICanvasLayer
        /// <summary>
        /// 绘制画布背景
        /// </summary>
        public void Draw(ICanvas canvas, RectangleF unitrect)
        {
            try
            {
                RectangleF r = ScreenUtils.ToScreenNormalized(canvas, unitrect);
                //RectangleF ro = r;
                if (!float.IsNaN(r.X) || !float.IsInfinity(r.X) ||
                    !float.IsNaN(r.Y) || !float.IsInfinity(r.Y) ||
                    !float.IsNaN(r.Width) || !float.IsInfinity(r.Width) ||
                    !float.IsNaN(r.Height) || !float.IsInfinity(r.Height))
                { canvas.Graphics.FillRectangle(m_backgroundBrush, r); }
                //绘制东南西北
                //Image img_coording = global::Canvas.Properties.Resources.timg;
                //if (img_coording != null)
                //{
                //    Rectangle rt = new Rectangle((int)ro.X + 15, (int)ro.Y + 10, 150, 150);
                //    canvas.Graphics.DrawImage(img_coording, rt);
                //}
                canvas.Graphics.ResetTransform();
            }
            catch (Exception ex)
            { throw ex; }
        }


        public PointF SnapPoint(PointF unitmousepoint)
        {
            return PointF.Empty;
        }

        public string Id
        {
            get { return "BackGround"; }
        }

        ISnapPoint ICanvasLayer.SnapPoint(ICanvas canvas, UnitPoint point, List<IDrawObject> otherobj)
        {
            throw new Exception("背景布局不实现该方法!");
        }

        public IEnumerable<IDrawObject> Objects
        {
            get { return null; }
        }

        public bool Enabled { get; set; }

        public bool Visible { get; set; }
        #endregion

        #region 实现 ISerialize
        public void GetObjectData(XmlWriter wr)
        {
            wr.WriteStartElement("BackGroundLayer");
            XmlUtil.WriteProperties(this, wr);
            wr.WriteEndElement();
        }

        public void AfterSerializedIn()
        {}
        #endregion
    }//end Class
}
