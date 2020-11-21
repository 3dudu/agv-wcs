using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SQLServerOperator
{
    public interface IDbOperator
    {

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sqls"></param>
        /// <param name="bTran"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        void SetDatas(string RuleCode, Hashtable Dic);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="RuleCode"></param>
        void SetDatas(string RuleCode);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="dbc"></param>
        /// <param name="sSQL"></param>
        /// <returns></returns>
        DataTable LoadDatas(string RuleCode, Hashtable Dic);
        DataTable LoadDatas(string RuleCode);

        /// <summary>
        /// 数据库是否连通
        /// </summary>
        /// <returns>bool</returns>
        bool ServerIsThrough();

        /// <summary>
        /// 获取事务
        /// </summary>
        /// <returns></returns>
        //SqlTransaction GetSqlTransaction();

        void SaveFile(int New_ID,string FilePath, string FileName,float Zoom);

        /// <summary>
        /// 加载保存的布局
        /// </summary>
        Hashtable GetPlanSetXML();
    }
}
