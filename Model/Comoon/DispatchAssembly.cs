using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Comoon
{
    public class DispatchAssembly
    {
        public DispatchAssembly()
        {
            AssemblyType = 0;
            ClassName = "";
            Discript = "";
        }
        /// <summary>
        /// 类操作类型[0 AGV通信 1 自动充电桩通信 2 调度逻辑处理 3 外围逻辑处理 4非标逻辑处理]
        /// </summary>
        public int AssemblyType { get; set; }

        /// <summary>
        /// 类名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Discript { get; set; }
    }
}
