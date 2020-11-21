using Canvas.CanvasInterfaces;
using Canvas.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas.DrawTools
{
    /// <summary>
    /// 绘画元素的基类
    /// </summary>
    public abstract class DrawObjectBase
    {
        float m_width;
        Color m_color;
        DrawingLayer m_layer;
        int m_flag = (int)(eFlags.useLayerWidth | eFlags.useLayerColor);

        int autoObj = 0;
        enum eFlags
        {
            selected = 0x00000001,
            highlighted = 0x00000002,
            useLayerWidth = 0x00000004,
            useLayerColor = 0x00000008,
        }

        [XmlSerializable]
        [Browsable(false)]
        public int AutoObj
        {
            set { autoObj = value; }
            get { return autoObj; }
        }

        bool GetFlag(eFlags flag)
        {
            return ((int)m_flag & (int)flag) > 0;
        }

        void SetFlag(eFlags flag, bool enable)
        {
            if (enable)
            { m_flag |= (int)flag; }
            else
            { m_flag &= ~(int)flag; }
        }

        [XmlSerializable]
        [Browsable(false)]
        public bool UseLayerWidth
        {
            get { return false; }
            set { SetFlag(eFlags.useLayerWidth, value); }
        }

        [XmlSerializable]
        [Browsable(false)]
        public bool UseLayerColor
        {
            get { return false; }
            set { SetFlag(eFlags.useLayerColor, value); }
        }


        [XmlSerializable]
        [Browsable(false)]
        public float Width
        {
            set
            { m_width = value; }
            get
            { return m_width; }
        }

        [Browsable(false)]
        [XmlSerializable]
        public Color Color
        {
            set { m_color = value; }
            get { return m_color; }
        }


        [Browsable(false)]
        public DrawingLayer Layer
        {
            get
            { return m_layer; }
            set
            { m_layer = value; }
        }

        public abstract void InitializeFromModel(UnitPoint point, DrawingLayer layer, ISnapPoint snap);

        [Browsable(false)]
        public virtual bool Selected
        {
            get { return GetFlag(eFlags.selected); }
            set { SetFlag(eFlags.selected, value); }
        }

        [Browsable(false)]
        public virtual bool Highlighted
        {
            get { return GetFlag(eFlags.highlighted); }
            set { SetFlag(eFlags.highlighted, value); }
        }

        public virtual void Copy(DrawObjectBase acopy)
        {
            UseLayerColor = acopy.UseLayerColor;
            UseLayerWidth = acopy.UseLayerWidth;
            Width = acopy.Width;
            Color = acopy.Color;
        }
    }//end Class
}
