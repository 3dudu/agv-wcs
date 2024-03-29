﻿using Canvas.CanvasInterfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas.DrawTools
{
    /// <summary>
    /// 中心点吸附点
    /// </summary>
    public class CenterSnapPoint: SnapPointBase
    {
        public CenterSnapPoint(ICanvas canvas, IDrawObject owner, UnitPoint snappoint)
           : base(canvas, owner, snappoint)
        { }

        public override void Draw(ICanvas canvas)
        {
            DrawPoint(canvas, Pens.White, Brushes.YellowGreen);
        }
    }
}
