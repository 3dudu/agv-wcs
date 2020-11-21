using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Comoon
{
    /// <summary>
    /// 明细查询实体类
    /// </summary>
    [Serializable]
    public class DetailQueryInfo
    {
        public DetailQueryInfo()
        {
            sqlStr = "";
            QueryCode = "";
            QueryName = "";
            Condition = new List<DetailCondition>();
            Fileds = new List<DetailQueryFiled>();
        }

        /// <summary>
        /// 明细查询编码
        /// </summary>
        public string QueryCode { get; set; }

        /// <summary>
        /// 明细查询名称
        /// </summary>
        public string QueryName { get; set; }
        /// <summary>
        /// 执行SQL
        /// </summary>
        public string sqlStr { get; set; }

        /// <summary>
        /// 所有条件
        /// </summary>
        public IList<DetailCondition> Condition { get; set; }

        /// <summary>
        /// 字段
        /// </summary>
        public IList<DetailQueryFiled> Fileds { get; set; }
    }
}
