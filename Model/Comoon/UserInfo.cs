using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Comoon
{
    /// <summary>
    /// 用户类
    /// </summary>
    [Serializable]
    public class UserInfo
    {
        public UserInfo()
        {
            UserID = "";
            UserName = "";
            PassWord = "";
            IsNew = true;
        }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string PassWord { get;  set; }
        public bool IsNew { get; set; }
    }
}
