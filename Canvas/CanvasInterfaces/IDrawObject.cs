using Canvas.CanvasCtrl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Canvas.CanvasInterfaces
{
    /// <summary>
    /// 图形元素类接口
    /// </summary>
    public interface IDrawObject
    {
        string Id { get; }

        int AutoObj { get; }

        IDrawObject Clone();
        bool PointInObject(ICanvas canvas, UnitPoint point);
        bool ObjectInRectangle(ICanvas canvas, RectangleF rect, bool anyPoint);
        void Draw(ICanvas canvas, RectangleF unitrect);
        RectangleF GetBoundingRect(ICanvas canvas);
        void OnMouseMove(ICanvas canvas, UnitPoint point);
        eDrawObjectMouseDownEnum OnMouseDown(ICanvas canvas, UnitPoint point, ISnapPoint snappoint);
        void OnMouseUp(ICanvas canvas, UnitPoint point, ISnapPoint snappoint);
        void OnKeyDown(ICanvas canvas, KeyEventArgs e);
        UnitPoint RepeatStartingPoint { get; }
        INodePoint NodePoint(ICanvas canvas, UnitPoint point);
        ISnapPoint SnapPoint(ICanvas canvas, UnitPoint point, List<IDrawObject> otherobj, Type[] runningsnaptypes, Type usersnaptype);
        void Move(UnitPoint offset);
        string GetInfoAsString();
    }
}
