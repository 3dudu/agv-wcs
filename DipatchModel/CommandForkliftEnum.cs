using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DipatchModel
{
    public class CommandForkliftEnum
    {
        public CommandForkliftEnum()
        {
            ForkAction = 1;
            height = 0;
        }
        /// <summary>
        /// 叉举动作触发 1降 2升
        /// </summary>
        public int ForkAction { get; set; }
        /// <summary>
        /// 叉举高度
        /// </summary>
        public double height { get; set; }
    }
}
