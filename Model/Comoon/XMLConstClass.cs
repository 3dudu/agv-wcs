using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Comoon
{
    public class XMLConstClass
    {
        public const string NODE_DB = "nodedbconfig";
        public const string NODE_SERVER = "serverconfig";

        /// <summary>
        /// 数据可地址 
        /// </summary>
        public const string DBADDRESS = "dbaddress";

        /// <summary>
        /// 数据库名
        /// </summary>
        public const string DBNAME = "dbname";

        /// <summary>
        /// 用户名
        /// </summary>
        public const string USERNAME = "username";

        /// <summary>
        /// 密码
        /// </summary>
        public const string PWD = "password";


        /// <summary>
        /// 外围服务监听端口
        /// </summary>
        public const string PORT = "port";
        /// <summary>
        /// 外围设备最大链接数
        /// </summary>
        public const string MAXCONNECTCOUNT = "maxclient";
        /// <summary>
        /// 链接超时
        /// </summary>
        public const string RECTIMEOUT = "rcvtimeout";
        /// <summary>
        /// 发送超时
        /// </summary>
        public const string SENDTIMEOUT = "sendtimeout";
        /// <summary>
        /// 接受缓存
        /// </summary>
        public const string BUFFERSIZE = "buffersize";
    }
}
