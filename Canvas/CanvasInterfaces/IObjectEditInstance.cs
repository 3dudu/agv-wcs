using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas.CanvasInterfaces
{
    /// <summary>
    /// 获得一个图形元素
    /// </summary>
    public interface IObjectEditInstance
    {
        IDrawObject GetDrawObject();
    }
}
