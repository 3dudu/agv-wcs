using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserControls
{
    public class CircleButton : Button
    {
        private int radius;//半径 
        
        //圆形按钮的半径属性
        [CategoryAttribute("布局"), BrowsableAttribute(true), ReadOnlyAttribute(false)]
        public int Radius
        {
            set
            {
                radius = value;
                this.Height = this.Width = Radius;
            }
            get
            {
                return radius;
            }
        }
        
        private Image imageEnter;

        [CategoryAttribute("外观"), BrowsableAttribute(true), ReadOnlyAttribute(false)]
        public Image ImageEnter
        {
            set
            {
                imageEnter = value;
            }
            get
            {
                return imageEnter;
            }
        }

        private Image imageNormal;

        [CategoryAttribute("外观"), BrowsableAttribute(true), ReadOnlyAttribute(false)]
        public Image ImageNormal
        {
            set
            {
                imageNormal = value;
                BackgroundImage = imageNormal;
            }
            get
            {
                return imageNormal;
            }
        }
        
        //以下代码用于在VS中隐藏BackgroundImage属性，使得只能通过Diameter设置Height和Width
        [BrowsableAttribute(false)]
        public new Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = value;
            }
        }

        //以下代码用于在VS中隐藏Size属性，使得只能通过Diameter设置Height和Width
        [BrowsableAttribute(false)]
        public new Size Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                base.Size = value;
            }
        }
        
        public CircleButton()
        {
            Radius = 64;
            this.Height = this.Width = Radius;
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.BackgroundImage = imageEnter;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }
        
        //重写OnPaint
        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, Radius, Radius);
            this.Region = new Region(path);
        }

        //重写OnMouseEnter
        //protected override void OnMouseEnter(EventArgs e)
        //{
        //    Graphics g = this.CreateGraphics();
        //    g.DrawEllipse(new Pen(Color.Blue), 0, 0, this.Width, this.Height);
        //    g.Dispose();
        //}
        
        //重写OnSizeChanged
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (Height != Radius)
            {
                Radius = Width = Height;
            }
            else if (Width != Radius)
            {
                Radius = Height = Width;
            }
        }
        
        //重写OnMouseEnter
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            BackgroundImage = ImageEnter;
        }
        
        //重写OnMouseLeave
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            BackgroundImage = ImageNormal;
        }
    }
}
