using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DipatchModel
{
    public class CommandToValue
    {
        /// <summary>
        /// 命令
        /// </summary>
        public AGVCommandEnum Command { get; set; }

        /// <summary>
        /// 命令值
        /// </summary>
        public object CommandValue { get; set; }


        public CommandToValue(AGVCommandEnum cmd)
        {
            Command = cmd;
        }

        public CommandToValue(AGVCommandEnum cmd, object cmdvalue)
        {
            Command = cmd;
            CommandValue = cmdvalue;
        }
    }
}
