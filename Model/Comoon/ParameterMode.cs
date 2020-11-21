using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Comoon
{
    public class ParameterMode
    {
        public ParameterMode()
        {
            ParameterCode = "";
            ParameterName = "";
            DefaultValue = "";
            ChooseValues = "";
        }

        /// <summary>
        /// 系统参数编码
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 系统参数名称
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 系统参数值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 维护框类型(1文本;2选择框)
        /// </summary>
        public int ExitType { get; set; }

        /// <summary>
        /// 可选值(分号分割)
        /// </summary>
        public string ChooseValues { get; set; }
    }
}
