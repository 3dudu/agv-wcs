using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Comoon
{
    /// <summary>
    /// 系统参数
    /// </summary>
    public class SysParameter
    {
        public SysParameter()
        {
            ParameterCode = "";
            ParameterValue = "";
            ParameterName = "";
        }
        /// <summary>
        /// 参数编码
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string ParameterValue { get; set; }
        public string ParameterName { get; set; }
    }
}
