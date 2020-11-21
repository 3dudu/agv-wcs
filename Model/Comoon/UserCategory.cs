using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Comoon
{
    /// <summary>
    /// 用户分类
    /// </summary>
    [Serializable]
    public class UserCategory
    {
        public UserCategory()
        {
            CategoryCode = "";
            CategoryName = "";
            IsNew = true;
            CategoryList = new List<SysOprButtonToCategory>();
        }
        /// <summary>
        /// 权限集合
        /// </summary>
        public IList<SysOprButtonToCategory> CategoryList { get; set; }
        public bool IsNew { get; set; }
        /// <summary>
        /// 用户分类编码
        /// </summary>
        public string CategoryCode { get; set; }
        /// <summary>
        /// 用户分类名称
        /// </summary>
        public string CategoryName { get; set; }
    }
}
