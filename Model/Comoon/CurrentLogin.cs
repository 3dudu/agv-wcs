using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Comoon
{
    public static class CurrentLogin
    {
        /// <summary>
        /// 当前登录用户
        /// </summary>
        public static UserInfo CurrentUser { get; set; }
        /// <summary>
        /// 可操作的按钮
        /// </summary>
        public static IList<SysOprButtonToCategory> SysOprButtons{ get;set;}
    }
}
