using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SimulateTcpCar
{
    /// <summary>
    /// 站点指令参数类
    /// </summary>
    public class SimulateCommand
    {
        public SimulateCommand()
        {
            Commands = new Hashtable();
            ParameterList = new List<Parameter>();
        }

        /// <summary>
        /// 站点
        /// </summary>
        public int Site { get; set; }

        /// <summary>
        /// 操作码
        /// </summary>
        public int OperatingCode { get; set; }

        /// <summary>
        /// 指令参数
        /// </summary>
        public Hashtable Commands { get; set; }

        /// <summary>
        /// 指令参数集合
        /// </summary>
        public List<Parameter> ParameterList { get; set; }
    }
}
