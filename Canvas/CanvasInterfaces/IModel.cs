using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas.CanvasInterfaces
{
    /// <summary>
    /// 画布模型接口
    /// </summary>
    public interface IModel
    {
        float Zoom { get; set; }
        ICanvasLayer BackgroundLayer { get; }
        ICanvasLayer GridLayer { get; }
        ICanvasLayer[] Layers { get; }
        ICanvasLayer ActiveLayer { get; set; }
        ICanvasLayer GetLayer(string id);

        #region StateColor
        Color NullStorageColor { get; set; }
        Color EmptyShelfStorageColor { get; set; }
        Color FillShelfStorageColor { get; set; }
        void SetStateColor(int State, Color color);
        Color GetStateColor(int State);
        #endregion
        IDrawObject CreateObject(string type, UnitPoint point, ISnapPoint snappoint);
        void AddObject(ICanvasLayer layer, IDrawObject drawobject);
        void DeleteObjects(IEnumerable<IDrawObject> objects);
        void MoveObjects(UnitPoint offset, IEnumerable<IDrawObject> objects);
        void CopyObjects(UnitPoint offset, IEnumerable<IDrawObject> objects);
        void MoveNodes(UnitPoint position, IEnumerable<INodePoint> nodes);
        IEditTool GetEditTool(string id);
        void AfterEditObjects(IEditTool edittool);
        List<IDrawObject> GetHitObjects(ICanvas canvas, RectangleF selection, bool anyPoint);
        List<IDrawObject> GetHitObjects(ICanvas canvas, UnitPoint point);
        bool IsSelected(IDrawObject drawobject);
        void AddSelectedObject(IDrawObject drawobject);
        void RemoveSelectedObject(IDrawObject drawobject);
        IEnumerable<IDrawObject> SelectedObjects { get; }
        int SelectedCount { get; }
        void ClearSelectedObjects();
        ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, Type[] runningsnaptypes, Type usersnaptype);
    }
}
