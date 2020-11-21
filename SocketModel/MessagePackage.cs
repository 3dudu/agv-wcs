using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketModel
{
    public class MessagePackage
    {
        public MessagePackage()
        {
            Message = "";
            _messagetime = "";
            _contentlengh = "";
            MessageType = "0";
            Command = SocketCommand.CONTEST;
            _messagetime = DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        public MessagePackage(string message, string command)
            : this()
        {
            Message = message;
            MessageType = "0";
            Command = command;
        }
        public MessagePackage(string message, string messagetype, string command)
            : this()
        {
            Message = message;
            MessageType = messagetype;
            Command = command;
        }

        public override string ToString()
        {
            byte[] codemessage = System.Text.Encoding.UTF8.GetBytes(Message);
            _contentlengh = codemessage.Length.ToString().PadLeft(8, '0');
            return _contentlengh + MessageType + MessageTime + Command + Message;
        }

        public byte[] ToBuffer()
        {
            return System.Text.Encoding.UTF8.GetBytes(this.ToString());
        }

        public string Message { get; set; }

        public string MessageType { get; set; }

        public string Command { get; set; }

        public string MessageTime { get { return _messagetime; } }

        public string ContentLengh { get { return _contentlengh; } }

        private string _messagetime;

        private string _contentlengh;
    }
}
