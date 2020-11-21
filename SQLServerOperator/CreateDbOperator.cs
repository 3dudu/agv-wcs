using Model.Comoon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLServerOperator
{
    public static class CreateDbOperator
    {
        /// <summary>
        /// 创建连接
        /// </summary> 
        /// <param name="dataBaseInfo">数据库信息</param>
        /// <returns></returns>
        public static IDbOperator DbOperatorInstance(DataBaseInfo dataBaseInfo)
        {
            switch (dataBaseInfo.DbType)
            {
                case (int)DataBaseTypeEnum.SQLServer: return new SQLServerDbOperator(dataBaseInfo);
                default: return new SQLServerDbOperator(dataBaseInfo);
            }
        }
    }
}
