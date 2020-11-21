using Model.Comoon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace SQLServerOperator
{
    public class SQLServerDbOperator : IDbOperator
    {
        private const string CONNSTRING = "data source={0}; user id={1}; password={2}; database={3};Connect Timeout=3";
        SqlConnection conn;
        private DataBaseInfo dataBaseInfo = new DataBaseInfo();//保存连接信息

        /// <summary>
        /// sql文件路径
        /// </summary>
        string sqlpath;


        public SQLServerDbOperator(DataBaseInfo dataBaseInfo)
        {
            conn = new SqlConnection(string.Format(CONNSTRING, dataBaseInfo.DbSource, dataBaseInfo.Uid, dataBaseInfo.Pwd, dataBaseInfo.DataBaseName));
            this.dataBaseInfo = dataBaseInfo;
            sqlpath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\SQLXML\\SQL.xml";
        }

        public SQLServerDbOperator(DataBaseInfo dataBaseInfo, string _sqlpath)
        {
            conn = new SqlConnection(string.Format(CONNSTRING, dataBaseInfo.DbSource, dataBaseInfo.Uid, dataBaseInfo.Pwd, dataBaseInfo.DataBaseName));
            this.dataBaseInfo = dataBaseInfo;
            sqlpath = _sqlpath;
        }
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public void OpenConnection()
        {
            if (conn.State != ConnectionState.Open)
            { conn.Open(); }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void CloseConnection()
        {
            if (conn.State == ConnectionState.Open)
            { conn.Close(); }
        }

        /// <summary>
        /// 数据库是否连通
        /// </summary>
        /// <returns>bool</returns>
        public bool ServerIsThrough()
        {
            if (conn != null)
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        OpenConnection();
                        conn.Close();
                    }
                    return true;
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    CloseConnection();
                }
            }
            else
            { return false; }
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        public void SetDatas(string RuleCode)
        {
            string SQLStr = GetSQLStr(RuleCode, new Hashtable());
            OpenConnection();
            IDbCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            //SqlTransaction Trans = conn.BeginTransaction();
            //cmd.Transaction = Trans;
            try
            {
                cmd.CommandText = SQLStr;
                cmd.CommandTimeout = 1800;
                cmd.ExecuteNonQuery();
                //Trans.Commit();
            }
            catch (Exception ex)
            {
                //Trans.Rollback();
                throw ex;
            }
        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sqls"></param>
        /// <param name="bTran"></param>
        /// <param name="err"></param>
        /// <returns></returns>
        public void SetDatas(string RuleCode, Hashtable Dic)
        {
            string SQLStr = GetSQLStr(RuleCode, Dic);
            OpenConnection();
            IDbCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            //SqlTransaction Trans = conn.BeginTransaction();
            //cmd.Transaction = Trans;
            try
            {
                cmd.CommandText = SQLStr;
                cmd.CommandTimeout = 1800;
                cmd.ExecuteNonQuery();
                //Trans.Commit();
            }
            catch (Exception ex)
            {
                //Trans.Rollback();
                throw ex;
            }

        }

        public IDbCommand GetDbCommand()
        {
            IDbCommand dbc = conn.CreateCommand();
            dbc.CommandTimeout = 1800;
            return dbc;
        }

        //public SqlTransaction GetSqlTransaction()
        //{
        //    SqlTransaction tran = conn.BeginTransaction();
        //    return tran;
        //}

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <returns></returns>
        public DataTable LoadDatas(string RuleCode, Hashtable dic)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                IDbCommand dbc = GetDbCommand();
                da.SelectCommand = dbc as SqlCommand;
                string SQLStr = GetSQLStr(RuleCode, dic);
                dbc.CommandText = SQLStr;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            { throw; }
        }

        public DataTable LoadDatas(string RuleCode)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                IDbCommand dbc = GetDbCommand();
                da.SelectCommand = dbc as SqlCommand;
                string SQLStr = GetSQLStr(RuleCode, new Hashtable());
                dbc.CommandText = SQLStr;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            { throw; }
        }


        /// <summary>
        /// 根据规则取sql
        /// </summary>
        /// <param name="RuleCode"></param>
        /// <param name="hs"></param>
        /// <returns></returns>
        public string GetSQLStr(string RuleCode, Hashtable hs)
        {
            try
            {
                //存放规则号下的sql语句
                string ResultSQLStr = string.Empty;
                //根据规则号找到下面的sql标签下的sql语句
                XmlDocument doc = new XmlDocument();
                doc.Load(sqlpath);
                XmlElement root = doc.DocumentElement;
                XmlNodeList RuleNodes = root.GetElementsByTagName("rule");
                //找到将本规则下的sql语句
                foreach (XmlNode rule in RuleNodes)
                {
                    if (((XmlElement)rule).GetAttribute("RuleCode").ToString().Equals(RuleCode))
                    {
                        XmlNodeList subAgeNodes = ((XmlElement)rule).GetElementsByTagName("Sql");
                        if (subAgeNodes.Count == 1)
                        {
                            ResultSQLStr = subAgeNodes[0].InnerText;
                            break;
                        }
                    }
                }
                //解析替换条件
                ResultSQLStr = ExplanSQLStr(ResultSQLStr, hs);
                return ResultSQLStr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 解析sql
        /// </summary>
        /// <param name="OriginSQLStr"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public string ExplanSQLStr(string OriginSQLStr, Hashtable dic)
        {
            try
            {
                if (string.IsNullOrEmpty(OriginSQLStr))
                { return ""; }
                string AfterSQL = OriginSQLStr;
                foreach (string item in dic.Keys)
                {
                    string afterTag = "$" + item + "$";
                    AfterSQL = AfterSQL.Replace(afterTag, dic[item].ToString());
                }
                return AfterSQL;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 保存文件到数据库
        /// </summary>
        public void SaveFile(int New_ID, string FilePath, string FileName, float zoom)
        {
            try
            {
                FileStream fs = new FileStream(FilePath, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                Byte[] byData = br.ReadBytes((int)fs.Length);
                fs.Close();
                OpenConnection();
                IDbCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                //SqlTransaction Trans = conn.BeginTransaction();
                try
                {
                    string str = "insert into tbPlanset(ID,FileName,UpTime,FileContent,Zoom) values (" + New_ID + "," +
                                           "'" + FileName + "','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "',@Content," + zoom.ToString()+ ")";
                    cmd.CommandText = str;
                    cmd.CommandTimeout = 1800;
                    SqlParameter contentParam = new SqlParameter("@Content", System.Data.SqlDbType.Image);
                    contentParam.Value = byData;
                    cmd.Parameters.Add(contentParam);
                    cmd.ExecuteNonQuery();
                    //Trans.Commit();
                }
                catch (Exception ex)
                {
                    //Trans.Rollback();
                    //conn.Close();
                    throw ex;
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 加载保存的布局
        /// </summary>
        public Hashtable GetPlanSetXML()
        {
            try
            {
                Hashtable hs = new Hashtable();
                OpenConnection();
                try
                {
                    string str = "select top(1) fileName,FileContent,IsCreat2D,Zoom from tbPlanset order by ID desc";
                    SqlCommand cmd = new SqlCommand(str, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    string fileName = "";
                    if (reader.Read())
                    {
                        fileName = (string)reader.GetValue(0);
                        byte[] fileContent = (byte[])reader.GetValue(1);
                        string temFile = "";
                        temFile = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\temSet.agv";
                        if (File.Exists(temFile))
                        { File.Delete(temFile); }
                        FileStream fs = new FileStream(temFile, FileMode.Create);
                        BinaryWriter bw = new BinaryWriter(fs);
                        bw.Write(fileContent, 0, fileContent.Length);
                        bw.Close();
                        fs.Close();
                    }
                    hs["fileName"] = fileName;
                    if (reader.HasRows)
                    {
                        hs["IsCreat2D"] = (int)reader.GetValue(2);
                        hs["Zoom"] = System.Convert.ToDouble(reader.GetValue(3));
                    }
                    else
                    {
                        hs["IsCreat2D"] = 0;
                        hs["Zoom"] = 0;
                    }
                    return hs;
                }
                catch (Exception ex)
                { throw ex; }
                finally
                { CloseConnection(); }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { CloseConnection(); }
        }
    }
}
