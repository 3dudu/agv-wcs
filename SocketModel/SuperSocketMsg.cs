using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketModel
{
    public class SuperSocketMsg
    {
        public const string msg = "{0}MFLAG{1}ENDFLAG\r\n";

        public SuperSocketMsg(string content, string command)
        {
            Content = content;
            Command = command;
        }
        /// <summary>
        /// 命令
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// UTF8
        /// </summary>
        /// <returns></returns>
        public byte[] ToBuffer()
        {
            return System.Text.Encoding.UTF8.GetBytes(string.Format(msg, Command, Content));
        }
    }
}
