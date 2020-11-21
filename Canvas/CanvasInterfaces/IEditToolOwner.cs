using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canvas.CanvasInterfaces
{
    /// <summary>
    /// 编辑工具提示信息接口
    /// </summary>
    public interface IEditToolOwner
    {
        void SetHint(string text);
    }
}
