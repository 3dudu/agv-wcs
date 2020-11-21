using Canvas.CanvasInterfaces;
using Canvas.DrawTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Canvas.Layers
{
    public class DrawingLayer : ICanvasLayer, ISerialize
    {

        #region 属性
        private string m_id;
        private Color m_color;
        private float m_width = 0.00f;
        bool m_enabled = true;
        bool m_visible = true;
        List<IDrawObject> m_objects = new List<IDrawObject>();
        Dictionary<IDrawObject, bool> m_objectMap = new Dictionary<IDrawObject, bool>();

        public string Id
        {
            get { return m_id; }
        }

        public IEnumerable<IDrawObject> Objects
        {
            get { return m_objects; }
        }

        [XmlSerializable]
        public Color Color
        {
            get { return m_color; }
            set { m_color = value; }
        }

        [XmlSerializable]
        public float Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        [XmlSerializable]
        public bool Enabled
        {
            get { return m_enabled && m_visible; }
            set { m_enabled = value; }
        }
        [XmlSerializable]
        public bool Visible
        {
            get { return m_visible; }
            set { m_visible = value; }
        }

        public int Count
        {
            get { return m_objects.Count; }
        }
        #endregion

        #region 方法
        public DrawingLayer(string id, Color color, float width)
        {
            m_id = id;
            m_color = color;
            m_width = width;
        }

        /// <summary>
        /// 添加一种图形元素
        /// </summary>
        public void AddObject(IDrawObject drawobject)
        {
            try
            {
                if (m_objectMap.ContainsKey(drawobject))
                { return; }
                if (drawobject is DrawTools.DrawObjectBase)
                { ((DrawTools.DrawObjectBase)drawobject).Layer = this; }
                m_objects.Add(drawobject);
                m_objectMap[drawobject] = true;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 删除元素
        /// </summary>
        public List<IDrawObject> DeleteObjects(IEnumerable<IDrawObject> objects)
        {
            try
            {
                if (Enabled == false)
                { return null; }
                List<IDrawObject> removedobjects = new List<IDrawObject>();
                foreach (IDrawObject obj in objects)
                {
                    if (m_objectMap.ContainsKey(obj))
                    {
                        removedobjects.Add(obj);
                        m_objectMap.Remove(obj);
                    }
                }
                if (removedobjects.Count == 0)
                { return null; }
                if (removedobjects.Count < 10)
                {
                    foreach (IDrawObject obj in removedobjects)
                    { m_objects.Remove(obj); }
                }
                else
                {
                    List<IDrawObject> newlist = new List<IDrawObject>();
                    foreach (IDrawObject obj in m_objects)
                    {
                        if (m_objectMap.ContainsKey(obj))
                        { newlist.Add(obj); }
                    }
                    m_objects.Clear();
                    m_objects = newlist;
                }
                return removedobjects;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 实现接口
        /// </summary>
        public void Draw(ICanvas canvas, RectangleF unitrect)
        {
            try
            {
                CommonTools.Tracing.StartTrack(CommonTools.Tracing.TracePaint);
                int cnt = 0;
                foreach (IDrawObject drawobject in m_objects)
                {
                    DrawTools.DrawObjectBase obj = drawobject as DrawTools.DrawObjectBase;
                    if (obj is IDrawObject && ((IDrawObject)obj).ObjectInRectangle(canvas, unitrect, true) == false)
                    { continue; }
                    bool sel = obj.Selected;
                    bool high = obj.Highlighted;
                    obj.Selected = false;
                    try
                    {
                        drawobject.Draw(canvas, unitrect);
                    }
                    catch (Exception ex)
                    { throw ex; }
                    obj.Selected = sel;
                    obj.Highlighted = high;
                    cnt++;
                }
                CommonTools.Tracing.EndTrack(CommonTools.Tracing.TracePaint, "Draw Layer {0}, ObjCount {1}, Painted ObjCount {2}", Id, m_objects.Count, cnt);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public PointF SnapPoint(PointF unitmousepoint)
        {
            return PointF.Empty;
        }

        public ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, List<IDrawObject> otherobj)
        {
            foreach (IDrawObject obj in m_objects)
            {
                ISnapPoint sp = obj.SnapPoint(canvas, point, otherobj, null, null);
                if (sp != null)
                    return sp;
            }
            return null;
        }
        #endregion


        #region 实现 XML Serialize
        /// <summary>
        /// 将当前布局写入XMl文件
        /// </summary>
        public void GetObjectData(XmlWriter wr)
        {
            try
            {
                wr.WriteStartElement("layer");
                wr.WriteAttributeString("Id", m_id);
                XmlUtil.WriteProperties(this, wr);
                wr.WriteStartElement("items");
                foreach (IDrawObject drawobj in m_objects)
                {
                    if (drawobj is ISerialize)
                        ((ISerialize)drawobj).GetObjectData(wr);
                }
                wr.WriteEndElement();
                wr.WriteEndElement();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void AfterSerializedIn()
        { }

        /// <summary>
        /// 打开还原布局
        /// </summary>
        public static DrawingLayer NewLayer(XmlElement xmlelement,IList<string> ContainElement)
        {
            string id = xmlelement.GetAttribute("Id");
            if (id.Length == 0)
                id = Guid.NewGuid().ToString();
            DrawingLayer layer = new DrawingLayer(id, Color.White, 0.0f);
            foreach (XmlElement node in xmlelement.ChildNodes)
            {
                XmlUtil.ParseProperty(node, layer);
                if (node.Name == "items")
                {
                    foreach (XmlElement itemnode in node.ChildNodes)
                    {
                        object item = DataModel.NewDrawObject(itemnode.Name);
                        if (item == null)
                        { continue; }
                        if (ContainElement != null)
                        {
                            if (ContainElement.Count(p => p == itemnode.Name) <= 0)
                            { continue; }
                        }
                        if (item is IDrawObject)
                        { layer.AddObject(item as IDrawObject); }
                        if (item != null)
                        { XmlUtil.ParseProperties(itemnode, item); }
                        if (item is ISerialize)
                        { ((ISerialize)item).AfterSerializedIn(); }
                    }
                }
            }
            return layer;
        }
        #endregion
        public void removeAll()
        {
            this.m_objects.Clear();
        }

        public void InitLayer(float width, float height,float space,float len,bool storagefull, int girdnum)
        {
            string id = Guid.NewGuid().ToString();
            List<IDrawObject> autoObject = this.Objects.Where(p => p.AutoObj == 1).ToList() ;
            this.DeleteObjects(autoObject);
            int code = 1;
            int hw_depth = girdnum*2+1;
            int rows = 0;
           
            bool isStore = false;
            for(float i = 1.0f; i < width;)
            {
                rows = 0;
                for (float j = 1.0f; j < height;)
                {
                    isStore = false;
                    Canvas.DrawTools.LandMarkTool landMark = new LandMarkTool(code+"");
                    landMark.SetPosition(new UnitPoint(i-0.1,j+0.1));
                    landMark.AutoObj = 1;
                    landMark.Width = 0.1F;
                    landMark.Color = Color.Green;
                    this.AddObject(landMark);

                    if(rows % hw_depth!=0 && i > 1.0f && i < width - space && j>1.0f && j < height - space)
                    {
                        isStore = true;
                        StorageTool storageTool = new StorageTool();
                        storageTool.AutoObj = 1;
                        storageTool.SetPosition(new UnitPoint(i, j));
                        storageTool.Width = 0.1F;
                        storageTool.Color = Color.Green;
                        storageTool.StcokID = code;
                        storageTool.LankMarkCode = code + "";
                        storageTool.OwnArea = 1;
                        if (storagefull)
                        {
                            storageTool.StorageState = 2;
                            storageTool.LockState = 1;
                        }
                        this.AddObject(storageTool);
                    }
                        rows++;
       

                    //画线
                    UnitPoint startPoint = new UnitPoint(i, j);
                

                    if (i < width-space && !isStore && (rows % hw_depth == 1&&(j <=1.0f || ((rows / hw_depth) % 2 == 0&&j< height - space))))
                    {
                        LineTool line = new LineTool(startPoint, new UnitPoint(i + space, j), 0.1f, Color.Green);
                        line.AutoObj = 1;
                        line.Lenth = len;
                        line.PlanRouteLevel = 1;
                        this.AddObject(line);
                    }

                    if (i > 1.0f && !isStore && (j >= height - space || ( rows/hw_depth%2==1 && rows% hw_depth == 1)))
                    {
                        LineTool line = new LineTool(startPoint, new UnitPoint(i - space, j), 0.1f, Color.Green);
                        line.AutoObj = 1;
                        line.Lenth = len;
                        line.PlanRouteLevel = 1;
                        this.AddObject(line);
                    }


                    if (j < height- space && i > 1.0f && ((rows % hw_depth) != girdnum+1 || i<=1.0f || i >= width - space))
                    {
                        LineTool line = new LineTool(startPoint, new UnitPoint(i, j + space), 0.1f, Color.Green);
                        line.AutoObj = 1;
                        line.Lenth = len;
                        if (!isStore && i<=1.0f)
                        {
                            line.PlanRouteLevel = 1;
                        }
                        this.AddObject(line);
                    }

                    if (j > 1.0f  && i < width- space && ((rows % hw_depth) != girdnum+2 || i <= 1.0f || i >= width - space))
                    {
                        LineTool line = new LineTool(startPoint, new UnitPoint(i, j - space), 0.1f, Color.Green);
                        line.AutoObj = 1;
                        line.Lenth = len;
                        if (!isStore && i >= width - space)
                        {
                            line.PlanRouteLevel = 1;
                        }
                        this.AddObject(line);
                    }

                    code++;
                    j += space;
                }
                i += space;
            }
        }
    }//end
}
