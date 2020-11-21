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
    public class StorageTool : DrawObjectBase, IDrawObject, INodePoint, ISerialize
    {
        #region 属性
        protected string lankmarkcode = "";
        protected string additionalLandCode = "";
        protected string storagename = "";
        protected UnitPoint location = UnitPoint.Empty;
        protected static int ThresholdPixel = 0;
        protected bool allowModify = true;
        protected UnitPoint aqlocation = UnitPoint.Empty;
        protected int stcokid = 1;

        [XmlSerializable]
        [Browsable(false)]
        public UnitPoint Location
        {
            get { return location; }
            set { location = value; }
        }

        [XmlSerializable]
        public int StcokID
        {
            get { return stcokid; }
            set { stcokid = value; }
        }

        [XmlSerializable]
        [Description("储位地标号")]
        public string LankMarkCode
        {
            get { return lankmarkcode; }
            set
            {
                if (allowModify)
                { lankmarkcode = value; }
            }
        }

        [XmlSerializable]
        [Description("储位边地标号")]
        public string AdditionalLandCode
        {
            get { return additionalLandCode; }
            set
            { additionalLandCode = value; }
        }

        [XmlSerializable]
        [Description("储位名称")]
        public string StorageName
        {
            get { return storagename; }
            set { storagename = value; }
        }

        [XmlSerializable]
        [Browsable(false)]
        public int OwnArea { get; set; }

        [XmlSerializable]
        [Browsable(false)]
        public int SubOwnArea { get; set; }

        [XmlSerializable]
        [Browsable(false)]
        public int matterType { get; set; }

        [XmlSerializable]
        [Browsable(false)]
        public int MaterielType { get; set; }
        
        [Description("储位状态(0空|1有空料车|2有满料车)")]
        public int StorageState { get; set; }
        
        [Browsable(false)]
        public int LockState { get; set; }


        [Browsable(false)]
        public static string ObjectType
        {
            get { return "StorageTool"; }
        }

        [Browsable(false)]
        public string Id
        { get { return ObjectType; } }

        [Browsable(false)]
        public UnitPoint RepeatStartingPoint
        { get { return location; } }
        #endregion


        #region 函数方法
        public StorageTool() { }

        public void AfterSerializedIn() { }

        public void Cancel() { }

        public virtual IDrawObject Clone()
        {
            try
            {
                StorageTool Storage = new StorageTool();
                Storage.Copy(this);
                return Storage;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public virtual void Copy(StorageTool acopy)
        {
            try
            {
                base.Copy(acopy);
                location = acopy.location;
                StorageState = acopy.StorageState;
                LankMarkCode = acopy.LankMarkCode;
                StorageName = acopy.StorageName;
                StcokID = acopy.StcokID;
                LockState = acopy.LockState;
                OwnArea = acopy.OwnArea;
                SubOwnArea = acopy.SubOwnArea;
                matterType = acopy.matterType;
                Selected = acopy.Selected;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Draw(ICanvas canvas, RectangleF unitrect)
        {
            try
            {
                Brush Pen;
                if (StorageState == 2)
                { Pen = new SolidBrush(canvas.DataModel.FillShelfStorageColor); }
                else if (StorageState == 1)
                { Pen = new SolidBrush(canvas.DataModel.EmptyShelfStorageColor); }
                else
                { Pen = new SolidBrush(canvas.DataModel.NullStorageColor); }
                if (LockState == 1)
                { Pen = Brushes.DeepPink; }
                if (Selected)
                { Pen = Brushes.Magenta; }
                canvas.DrawStorage(canvas, Pen, StcokID.ToString(), Location);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void Finish()
        { }

        public RectangleF GetBoundingRect(ICanvas canvas)
        {
            try
            {
                //RectangleF rec = new RectangleF((float)Location.X, (float)Location.Y, 0.5F, -0.5F);
                //return rec;
                float PriWidth = canvas.ToScreen(0.5F);
                aqlocation = new UnitPoint(location.X + 0.5, location.Y - 0.5);
                return ScreenUtils.GetRect(location, aqlocation, 0);
            }
            catch (Exception ex)
            { throw ex; }
        }

        public IDrawObject GetClone()
        { return null; }

        public string GetInfoAsString()
        { return ""; }

        public void GetObjectData(XmlWriter wr)
        {
            try
            {
                wr.WriteStartElement("StorageTool");
                XmlUtil.WriteProperties(this, wr);
                wr.WriteEndElement();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public IDrawObject GetOriginal()
        { return null; }

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

        public eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint)
        {
            try
            {
                int MaxID = 1;
                if (canvas.DataModel.ActiveLayer.Objects.Count() > 0)
                {
                    if (canvas.DataModel.ActiveLayer.Objects.Where(p => p.Id == "StorageTool").Count() > 0)
                    {
                        MaxID = canvas.DataModel.ActiveLayer.Objects.Where(p => p.Id == "StorageTool").Max(p => (p as StorageTool).StcokID);
                        StcokID = MaxID + 1;
                    }
                }
                Selected = false;
                location = point;
                Brush Pen = Brushes.White;
                canvas.DrawStorage(canvas, Pen, StcokID.ToString(), Location);
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
            }catch(Exception ex)
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
        #endregion
    }
}
