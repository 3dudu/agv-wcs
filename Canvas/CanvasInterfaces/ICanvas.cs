using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas.CanvasInterfaces
{
    /// <summary>
    /// 画布接口类
    /// </summary>
    public interface ICanvas
    {
        IModel DataModel { get; }
        UnitPoint ScreenTopLeftToUnitPoint();
        UnitPoint ScreenBottomRightToUnitPoint();
        PointF ToScreen(UnitPoint unitpoint);
        float ToScreen(double unitvalue);
        double ToUnit(float screenvalue);
        UnitPoint ToUnit(PointF screenpoint);
        void Invalidate();
        IDrawObject CurrentObject { get; }
        Rectangle ClientRectangle { get; }
        Graphics Graphics { get; }
        Pen CreatePen(Color color);
        void DrawLine(ICanvas canvas, Pen pen, UnitPoint p1, UnitPoint p2);
        //void DrawArc(ICanvas canvas, Pen pen, UnitPoint center, float radius, float beginangle, float angle);
        void DrawLandMark(ICanvas canvas, Brush pen, string code, UnitPoint Point);
        void DrawImge(ICanvas canvas, Pen pen, UnitPoint Location, float Widht, float Hight, Image img, string values);
        void DrawTxt(ICanvas canvas, string code, UnitPoint Point, int FontSize, Color fontColor);
        void DrawBizer(ICanvas canvas, Pen pen, UnitPoint p1, UnitPoint p2, UnitPoint p3, UnitPoint p4);
        void DrawStorage(ICanvas canvas, Brush Pen, string code, UnitPoint Point);
        void DrawPosition(ICanvas canvas, Brush Pen, string code, UnitPoint Point);
        void DrawBtnBox(ICanvas canvas, float Radius, UnitPoint Point,bool Selected);
        void DrawAGV(ICanvas canvas, Pen pen, UnitPoint p, string Code);
        void DrawArc(ICanvas canvas, Pen pen, UnitPoint center, float radius, float beginangle, float angle);
        SizeF GetStrValueSize(string Str, Font f);
    }
}
