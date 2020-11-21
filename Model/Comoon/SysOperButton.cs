using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Comoon
{
    /// <summary>
    /// 系统功能按钮
    /// </summary>
    public class SysOperButton
    {
        public SysOperButton()
        {
            ButtonName = "";
            ButtonCaption = "";
        }

        public string Button { get
            {
                if (ButtonType==1)
                {
                    return "服务端";
                }
                if (ButtonType==0)
                {
                    return "客户端";
                }
                return "";
            } }
        /// <summary>
        /// 类型[0 客户端   1 服务端]
        /// </summary>
        public int ButtonType { get; set; }

        public string ButtonTypeStr
        {
            get
            {
                if (ButtonType == 0)
                { return "客户端"; }
                else
                { return "服务端"; }
            }
        }

        /// <summary>
        /// 选择
        /// </summary>
        public bool IsSelect { get; set; }
        /// <summary>
        /// 按钮名称
        /// </summary>
        public string ButtonName { get; set; }

        /// <summary>
        /// 按钮标题
        /// </summary>
        public string ButtonCaption { get; set; }
    }
}
