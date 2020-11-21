using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Linq;
using Canvas.CanvasInterfaces;
using Canvas.Layers;
using Canvas.DrawTools;
using System.Collections;

namespace Canvas
{
    public class DataModel : IModel
    {

        /// <summary>
        /// 创建objecttype的绘画实体
        /// </summary>
        static Dictionary<string, Type> m_toolTypes = new Dictionary<string, Type>();
        static public IDrawObject NewDrawObject(string objecttype)
        {
            if (m_toolTypes.ContainsKey(objecttype))
            {
                string type = m_toolTypes[objecttype].ToString();
                return Assembly.GetExecutingAssembly().CreateInstance(type) as IDrawObject;
            }
            return null;
        }

        /// <summary>
        /// 创建绘画元素对象
        /// </summary>
        Dictionary<string, IDrawObject> m_drawObjectTypes = new Dictionary<string, IDrawObject>();
        DrawObjectBase CreateObject(string objecttype)
        {
            if (m_drawObjectTypes.ContainsKey(objecttype))
            {
                return m_drawObjectTypes[objecttype].Clone() as DrawTools.DrawObjectBase;
            }
            return null;
        }


        Dictionary<string, IEditTool> m_editTools = new Dictionary<string, IEditTool>();
        public void AddEditTool(string key, IEditTool tool)
        {
            m_editTools.Add(key, tool);
        }

        public bool IsDirty
        {
            get { return m_undoBuffer.Dirty; }
        }
        UndoRedoBuffer m_undoBuffer = new UndoRedoBuffer();

        UnitPoint m_centerPoint = UnitPoint.Empty;
        public UnitPoint CenterPoint
        {
            get { return m_centerPoint; }
            set { m_centerPoint = value; }
        }

        float m_zoom = 0.5f;
        GridLayer m_gridLayer = new GridLayer();
        BackgroundLayer m_backgroundLayer = new BackgroundLayer();
        List<ICanvasLayer> m_layers = new List<ICanvasLayer>();
        ICanvasLayer m_activeLayer;
        Dictionary<IDrawObject, bool> m_selection = new Dictionary<IDrawObject, bool>();
        public DataModel()
        {
            m_toolTypes.Clear();
            m_toolTypes[DrawTools.LineTool.ObjectType] = typeof(DrawTools.LineTool);
            m_toolTypes[DrawTools.LandMarkTool.ObjectType] = typeof(DrawTools.LandMarkTool);
            m_toolTypes[DrawTools.ImgeTool.ObjectType] = typeof(DrawTools.ImgeTool);
            m_toolTypes[DrawTools.TextTool.ObjectType] = typeof(DrawTools.TextTool);
            m_toolTypes[DrawTools.BezierTool.ObjectType] = typeof(DrawTools.BezierTool);
            m_toolTypes[DrawTools.StorageTool.ObjectType] = typeof(DrawTools.StorageTool);
            m_toolTypes[DrawTools.PositionTool.ObjectType] = typeof(DrawTools.PositionTool);
            m_toolTypes[DrawTools.ButtonTool.ObjectType] = typeof(DrawTools.ButtonTool);
            m_toolTypes[DrawTools.Arc3Point.ObjectType] = typeof(DrawTools.Arc3Point);
            DefaultLayer();
            m_centerPoint = new UnitPoint(0, 0);
        }

        /// <summary>
        /// 把工具看的按钮添加到键值对中方便调取
        /// </summary>
        /// <param name="key"></param>
        /// <param name="drawtool"></param>
        public void AddDrawTool(string key, IDrawObject drawtool)
        {
            m_drawObjectTypes[key] = drawtool;
        }

        #region 保存加载画图数据
        public void Save(string filename)
        {
            try
            {
                if (File.Exists(filename))
                { File.Delete(filename); }
                XmlTextWriter wr = new XmlTextWriter(filename, null);
                wr.Formatting = Formatting.Indented;
                wr.WriteStartElement("CanvasDataModel");
                m_backgroundLayer.GetObjectData(wr);
                m_gridLayer.GetObjectData(wr);
                foreach (ICanvasLayer layer in m_layers)
                {
                    if (layer is ISerialize)
                        ((ISerialize)layer).GetObjectData(wr);
                }
                XmlUtil.WriteProperties(this, wr);
                wr.WriteEndElement();
                wr.Close();
                m_undoBuffer.Dirty = false;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void reinitLay(float width, float height, float space,float len,bool storagefull,int girdnum)
        {
            foreach (ICanvasLayer layer in m_layers)
            {
                if (layer is DrawingLayer)
                    ((DrawingLayer)layer).InitLayer(width,height,space, len, storagefull, girdnum);
            }
        }

        public void removeAll()
        {
            foreach (ICanvasLayer layer in m_layers)
            {
                if (layer is DrawingLayer)
                    ((DrawingLayer)layer).removeAll();
            }
        }

        public bool Load(string filename, IList<string> ContainElement)
        {
            try
            {
                StreamReader sr = new StreamReader(filename);
                //XmlTextReader rd = new XmlTextReader(sr);
                XmlDocument doc = new XmlDocument();
                doc.Load(sr);
                sr.Dispose();
                XmlElement root = doc.DocumentElement;
                if (root.Name != "CanvasDataModel")
                    return false;

                m_layers.Clear();
                m_undoBuffer.Clear();
                m_undoBuffer.Dirty = false;
                foreach (XmlElement childnode in root.ChildNodes)
                {
                    if (childnode.Name == "backgroundlayer")
                    {
                        XmlUtil.ParseProperties(childnode, m_backgroundLayer);
                        continue;
                    }
                    if (childnode.Name == "gridlayer")
                    {
                        XmlUtil.ParseProperties(childnode, m_gridLayer);
                        continue;
                    }
                    if (childnode.Name == "layer")
                    {
                        DrawingLayer l = DrawingLayer.NewLayer(childnode as XmlElement, ContainElement);
                        m_layers.Add(l);
                    }
                    if (childnode.Name == "property")
                        XmlUtil.ParseProperty(childnode, this);
                }
                return true;
            }
            catch (Exception e)
            {
                DefaultLayer();
                Console.WriteLine("Load exception - {0}", e.Message);
            }
            return false;
        }
        #endregion
        void DefaultLayer()
        {
            m_layers.Clear();
            m_layers.Add(new DrawingLayer("1", Color.Green, 0.1f));
        }
        public IDrawObject GetFirstSelected()
        {
            if (m_selection.Count > 0)
            {
                Dictionary<IDrawObject, bool>.KeyCollection.Enumerator e = m_selection.Keys.GetEnumerator();
                e.MoveNext();
                return e.Current;
            }
            return null;
        }
        #region IModel Members
        public float Zoom
        {
            get { return m_zoom; }
            set { m_zoom = value; }
        }
        public ICanvasLayer BackgroundLayer
        {
            get { return m_backgroundLayer; }
        }
        public ICanvasLayer GridLayer
        {
            get { return m_gridLayer; }
        }
        public ICanvasLayer[] Layers
        {
            get { return m_layers.ToArray(); }
        }
        public ICanvasLayer ActiveLayer
        {
            get
            {
                if (m_activeLayer == null)
                    m_activeLayer = m_layers[0];
                return m_activeLayer;
            }
            set
            {
                m_activeLayer = value;
            }
        }
        public ICanvasLayer GetLayer(string id)
        {
            foreach (ICanvasLayer layer in m_layers)
            {
                if (layer.Id == id)
                    return layer;
            }
            return null;
        }

        private Color nullStorageColor = Color.White;
        public Color NullStorageColor
        {
            get { return nullStorageColor; }
            set { nullStorageColor = value; }
        }

        private Color emptyShelfStorageColor = Color.LightBlue;
        public Color EmptyShelfStorageColor
        {
            get { return emptyShelfStorageColor; }
            set { emptyShelfStorageColor = value; }
        }

        private Color fillShelfStorageColor = Color.DarkGreen;
        public Color FillShelfStorageColor
        {
            get { return fillShelfStorageColor; }
            set { fillShelfStorageColor = value; }
        }

        private IDictionary dicColor = new Hashtable();

        public void SetStateColor(int State, Color color)
        {
            try
            {
                dicColor[State] = color;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public Color GetStateColor(int State)
        {
            try
            {
                object colorobj = dicColor[State];
                if (colorobj == null) { return Color.White; }
                return (Color)dicColor[State];
            }
            catch (Exception ex)
            { throw ex; }
        }

        public IDrawObject CreateObject(string type, UnitPoint point, ISnapPoint snappoint)
        {
            DrawingLayer layer = ActiveLayer as DrawingLayer;
            if (layer.Enabled == false)
                return null;
            DrawTools.DrawObjectBase newobj = CreateObject(type);
            if (newobj != null)
            {
                newobj.Layer = layer;
                newobj.InitializeFromModel(point, layer, snappoint);
            }
            return newobj as IDrawObject;
        }
        public void AddObject(ICanvasLayer layer, IDrawObject drawobject)
        {
            if (drawobject is Canvas.CanvasInterfaces.IObjectEditInstance)
                drawobject = ((Canvas.CanvasInterfaces.IObjectEditInstance)drawobject).GetDrawObject();
            if (m_undoBuffer.CanCapture)
                m_undoBuffer.AddCommand(new EditCommandAdd(layer, drawobject));
            ((DrawingLayer)layer).AddObject(drawobject);
        }
        public void DeleteObjects(IEnumerable<IDrawObject> objects)
        {
            EditCommandRemove undocommand = null;
            if (m_undoBuffer.CanCapture)
                undocommand = new EditCommandRemove();
            foreach (ICanvasLayer layer in m_layers)
            {
                List<IDrawObject> removedobjects = ((DrawingLayer)layer).DeleteObjects(objects);
                if (removedobjects != null && undocommand != null)
                    undocommand.AddLayerObjects(layer, removedobjects);
            }
            if (undocommand != null)
                m_undoBuffer.AddCommand(undocommand);
        }

        /// <summary>
        /// 循环所有选中的元素移动
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="objects"></param>
        public void MoveObjects(UnitPoint offset, IEnumerable<IDrawObject> objects)
        {
            if (m_undoBuffer.CanCapture)
                m_undoBuffer.AddCommand(new EditCommandMove(offset, objects));
            foreach (IDrawObject obj in objects)
                obj.Move(offset);
        }
        public void CopyObjects(UnitPoint offset, IEnumerable<IDrawObject> objects)
        {
            ClearSelectedObjects();
            List<IDrawObject> newobjects = new List<IDrawObject>();
            foreach (IDrawObject obj in objects)
            {
                IDrawObject newobj = obj.Clone();
                newobjects.Add(newobj);
                newobj.Move(offset);
                ((DrawingLayer)ActiveLayer).AddObject(newobj);
                AddSelectedObject(newobj);
            }
            if (m_undoBuffer.CanCapture)
                m_undoBuffer.AddCommand(new EditCommandAdd(ActiveLayer, newobjects));
        }
        public void AfterEditObjects(IEditTool edittool)
        {
            edittool.Finished();
            if (m_undoBuffer.CanCapture)
                m_undoBuffer.AddCommand(new EditCommandEditTool(edittool));
        }
        public IEditTool GetEditTool(string edittoolid)
        {
            if (m_editTools.ContainsKey(edittoolid))
                return m_editTools[edittoolid].Clone();
            return null;
        }
        public void MoveNodes(UnitPoint position, IEnumerable<INodePoint> nodes)
        {
            if (m_undoBuffer.CanCapture)
                m_undoBuffer.AddCommand(new EditCommandNodeMove(nodes));
            foreach (INodePoint node in nodes)
            {
                //判断一下，如果是连接地标的线段不允许拖动延长或更改
                //IDrawObject obj = node.GetOriginal();
                //if (((obj as LineTool) != null && (obj as LineTool).Type == LineType.PointLine) 
                //    || ((obj as BezierTool) != null && (obj as BezierTool).Type == BezierType.PointBezier 
                //    && ((node as BezierTool).CurrSelectPoint == BezierTool.eCurrentPoint.p1 
                //    || (node as BezierTool).CurrSelectPoint == BezierTool.eCurrentPoint.p2))
                //    ||((obj as Arc3Point) != null&&(((node as NodePointArc3PointPoint).m_curPoint== Arc3Point.eCurrentPoint.p1)|| (node as NodePointArc3PointPoint).m_curPoint == Arc3Point.eCurrentPoint.p3)))
                //{
                //if ((obj as LineTool) != null)
                //{
                //    if (Math.Abs(Math.Round((obj as LineTool).P1.X, 2, MidpointRounding.AwayFromZero) - Math.Round(position.X, 2, MidpointRounding.AwayFromZero)) <= 0.02 && Math.Abs(Math.Round((obj as LineTool).P1.Y, 2, MidpointRounding.AwayFromZero) - Math.Round(position.Y, 2, MidpointRounding.AwayFromZero)) <= 0.02)
                //    { continue; }
                //    if (Math.Abs(Math.Round((obj as LineTool).P2.X, 2, MidpointRounding.AwayFromZero) - Math.Round(position.X, 2, MidpointRounding.AwayFromZero)) <= 0.02 && Math.Abs(Math.Round((obj as LineTool).P2.Y, 2, MidpointRounding.AwayFromZero) - Math.Round(position.Y, 2, MidpointRounding.AwayFromZero)) <= 0.02)
                //    { continue; }
                //}
                //if ((obj as BezierTool) != null)
                //{
                //    if (Math.Abs(Math.Round((obj as BezierTool).P1.X, 2, MidpointRounding.AwayFromZero) - Math.Round(position.X, 2, MidpointRounding.AwayFromZero)) <= 0.02 && Math.Abs(Math.Round((obj as BezierTool).P1.Y, 2, MidpointRounding.AwayFromZero) - Math.Round(position.Y, 2, MidpointRounding.AwayFromZero)) <= 0.02)
                //    { continue; }
                //    if (Math.Abs(Math.Round((obj as BezierTool).P2.X, 2, MidpointRounding.AwayFromZero) - Math.Round(position.X, 2, MidpointRounding.AwayFromZero)) <= 0.02 && Math.Abs(Math.Round((obj as BezierTool).P2.Y, 2, MidpointRounding.AwayFromZero) - Math.Round(position.Y, 2, MidpointRounding.AwayFromZero)) <= 0.02)
                //    { continue; }
                //}
                //if ((obj as Arc3Point) != null)
                //{
                //    if (Math.Abs(Math.Round((obj as Arc3Point).P1.X, 2, MidpointRounding.AwayFromZero) - Math.Round(position.X, 2, MidpointRounding.AwayFromZero)) <= 0.02 && Math.Abs(Math.Round((obj as Arc3Point).P1.Y, 2, MidpointRounding.AwayFromZero) - Math.Round(position.Y, 2, MidpointRounding.AwayFromZero)) <= 0.02)
                //    { continue; }
                //    if (Math.Abs(Math.Round((obj as Arc3Point).P2.X, 2, MidpointRounding.AwayFromZero) - Math.Round(position.X, 2, MidpointRounding.AwayFromZero)) <= 0.02 && Math.Abs(Math.Round((obj as Arc3Point).P2.Y, 2, MidpointRounding.AwayFromZero) - Math.Round(position.Y, 2, MidpointRounding.AwayFromZero)) <= 0.02)
                //    { continue; }
                //}



                //IDrawObject defaultLand = ActiveLayer.Objects.FirstOrDefault(p => p.Id == "LandMark" && Math.Abs(Math.Round((p as LandMarkTool).MidPoint.X, 2, MidpointRounding.AwayFromZero) - Math.Round(position.X, 2, MidpointRounding.AwayFromZero)) <= 0.02 && Math.Abs(Math.Round((p as LandMarkTool).MidPoint.Y, 2, MidpointRounding.AwayFromZero) - Math.Round(position.Y, 2, MidpointRounding.AwayFromZero)) <= 0.02);
                //if (defaultLand == null)
                //{ continue; }
                //}

                //if ((node != null && (node as LineTool) != null && (node as LineTool).Type == LineType.PointLine)||
                //    (node != null && (node as BezierTool) != null && (node as BezierTool).Type == BezierType.PointBezier
                //    && (((node as BezierTool).CurrSelectPoint == BezierTool.eCurrentPoint.p1)|| (node as BezierTool).CurrSelectPoint == BezierTool.eCurrentPoint.p2)))
                //{ continue; }



                IDrawObject obj = node.GetOriginal();
                if ((obj as LineTool) != null && (obj as LineTool).Type == LineType.PointLine)
                {
                    IDrawObject defaultLand = ActiveLayer.Objects.FirstOrDefault(p => p.Id == "LandMark" && Math.Abs(Math.Round((p as LandMarkTool).MidPoint.X, 2, MidpointRounding.AwayFromZero) - Math.Round(position.X, 2, MidpointRounding.AwayFromZero)) <= 0.02 && Math.Abs(Math.Round((p as LandMarkTool).MidPoint.Y, 2, MidpointRounding.AwayFromZero) - Math.Round(position.Y, 2, MidpointRounding.AwayFromZero)) <= 0.02);
                    if (defaultLand == null)
                    { continue; }
                }
                node.SetPosition(position);
                node.Finish();
            }
        }
        public List<IDrawObject> GetHitObjects(ICanvas canvas, RectangleF selection, bool anyPoint)
        {
            List<IDrawObject> selected = new List<IDrawObject>();
            foreach (ICanvasLayer layer in m_layers)
            {
                if (layer.Visible == false)
                    continue;
                foreach (IDrawObject drawobject in layer.Objects)
                {
                    if (drawobject.ObjectInRectangle(canvas, selection, anyPoint))
                        selected.Add(drawobject);
                }
            }
            return selected;
        }

        //通过鼠标当前坐标获取选中的图形
        public List<IDrawObject> GetHitObjects(ICanvas canvas, UnitPoint point)
        {
            List<IDrawObject> selected = new List<IDrawObject>();
            foreach (ICanvasLayer layer in m_layers)
            {
                if (layer.Visible == false)
                { continue; }
                foreach (IDrawObject drawobject in layer.Objects)
                {
                    try
                    {
                        if (drawobject.PointInObject(canvas, point))
                        { selected.Add(drawobject); }
                    }
                    catch (Exception ex)
                    { }
                }
                //如果选中该的元素有多个且包含地标则默认选择地标
                if (selected.Count > 1)
                {
                    IDrawObject obj = selected.FirstOrDefault(p=>p.Id== "LandMark");
                    if (obj != null)
                    {
                        selected.Clear();
                        selected.Add(obj);
                    }
                }
            }
            return selected;
        }
        public bool IsSelected(IDrawObject drawobject)
        {
            return m_selection.ContainsKey(drawobject);
        }
        public void AddSelectedObject(IDrawObject drawobject)
        {
            DrawTools.DrawObjectBase obj = drawobject as DrawTools.DrawObjectBase;
            RemoveSelectedObject(drawobject);
            m_selection[drawobject] = true;
            obj.Selected = true;
        }
        public void RemoveSelectedObject(IDrawObject drawobject)
        {
            if (m_selection.ContainsKey(drawobject))
            {
                DrawTools.DrawObjectBase obj = drawobject as DrawTools.DrawObjectBase;
                obj.Selected = false;
                m_selection.Remove(drawobject);
            }
        }
        public IEnumerable<IDrawObject> SelectedObjects
        {
            get
            {
                return m_selection.Keys;
            }
        }
        public int SelectedCount
        {
            get { return m_selection.Count; }
        }
        public void ClearSelectedObjects()
        {
            IEnumerable<IDrawObject> x = SelectedObjects;
            foreach (IDrawObject drawobject in x)
            {
                DrawTools.DrawObjectBase obj = drawobject as DrawTools.DrawObjectBase;
                obj.Selected = false;
            }
            m_selection.Clear();
        }
        public ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, Type[] runningsnaptypes, Type usersnaptype)
        {
            List<IDrawObject> objects = GetHitObjects(canvas, point);
            if (objects.Count == 0)
                return null;

            foreach (IDrawObject obj in objects)
            {
                ISnapPoint snap = obj.SnapPoint(canvas, point, objects, runningsnaptypes, usersnaptype);
                if (snap != null)
                    return snap;
            }
            return null;
        }

        public bool CanUndo()
        {
            return m_undoBuffer.CanUndo;
        }
        public bool DoUndo()
        {
            return m_undoBuffer.DoUndo(this);
        }
        public bool CanRedo()
        {
            return m_undoBuffer.CanRedo;

        }
        public bool DoRedo()
        {
            return m_undoBuffer.DoRedo(this);
        }
        #endregion
    }
}
