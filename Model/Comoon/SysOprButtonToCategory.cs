using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Comoon
{
    /// <summary>
    /// 功能按钮对照类
    /// </summary>
    public class SysOprButtonToCategory
    {
        public SysOprButtonToCategory()
        {
            CategoryCode = "";
            ButtonType = 0;
            ButtonName = "";
        }
        //用户分类编码
        public string CategoryCode { get; set; }
        //类型 0客户端 1服务端
        public int ButtonType { get; set; }
        //按钮名称
        public string ButtonName{get;set;}
        //用户分类对应的按钮集
        public IList<UserCategory> ButtonOfCategory { get; set; }
    }
}
