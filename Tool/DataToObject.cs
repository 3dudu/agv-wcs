using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;
using System.IO;
using System.Windows.Forms;
using SocketClient;
using Model.Comoon;

namespace Tools
{
    public class DataToObject
    {
        //当前连接数据库信息实体
        public static DataBaseInfo DBase { get; set; }
        //客户端通信参数
        public static ClientConfig serverconfig { get; set; }

        //得到DataTable结构体和数据
        private static DataTable GetDataTableStruct()
        {
            try
            {
                DataTable dt = new DataTable();
                DataColumn dcIP = new DataColumn("DBIP", typeof(string));
                dt.Columns.Add(dcIP);
                DataColumn dcName = new DataColumn("DBName", typeof(string));
                dt.Columns.Add(dcName);
                DataColumn dcUser = new DataColumn("DBUser", typeof(string));
                dt.Columns.Add(dcUser);
                DataColumn dcPass = new DataColumn("DBPass", typeof(string));
                dt.Columns.Add(dcPass);
                DataColumn dcServerIP = new DataColumn("ServerIP", typeof(string));
                dt.Columns.Add(dcServerIP);
                DataColumn dcServerPort = new DataColumn("ServerPort", typeof(string));
                dt.Columns.Add(dcServerPort);
                return dt;
            }
            catch (Exception ex)
            { throw ex; }
        }

        //将文本转换成DataTable
        private static DataTable TxtToDT()
        {
            string Content = "";
            DataTable dt = null;
            try
            {
                if (File.Exists(Application.StartupPath + @"\DataSource.txt"))
                {
                    Content = File.ReadAllText(Application.StartupPath + @"\DataSource.txt");
                }
                if (!string.IsNullOrEmpty(Content))
                {
                    dt = GetDataTableStruct();
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    string[] clomnsStr = Content.Split(',');
                    for (int i = 0; i < clomnsStr.Length; i++)
                    {
                        string CloumnName = clomnsStr[i].Split(':')[0].ToString();
                        string CloumnConten = clomnsStr[i].Split(':')[1].ToString();
                        dt.Rows[0][CloumnName] = CloumnConten;
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }
            return dt;
        }

        //初始化数据库
        public static void setDBase()
        {
            try
            {
                DataTable dt = TxtToDT();
                if (dt != null && TxtToDT().Rows.Count > 0)
                {
                    DataBaseInfo db = new DataBaseInfo();
                    db.DataBaseName = dt.Rows[0]["DBName"].ToString();
                    db.DbSource = dt.Rows[0]["DBIP"].ToString();
                    db.Pwd = dt.Rows[0]["DBPass"].ToString();
                    db.Uid = dt.Rows[0]["DBUser"].ToString();
                    DBase = DataToObject.CreateDeepCopy<DataBaseInfo>(db);
                    serverconfig = new ClientConfig() { ServerIP = dt.Rows[0]["ServerIP"].ToString(), Port = System.Convert.ToInt32(dt.Rows[0]["ServerPort"].ToString()), TimeOut = 6, ReceiveBufferSize = 128 };
                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        public static IList<T> TableToEntity<T>(DataTable dt) where T : class, new()
        {
            Type type = typeof(T);
            List<T> list = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                PropertyInfo[] pArray = type.GetProperties();
                T entity = new T();
                foreach (PropertyInfo p in pArray)
                {
                    if (!row.Table.Columns.Contains(p.Name))
                    { continue; }
                    object obj = row[p.Name];
                    if (p.PropertyType == typeof(string))
                    {
                        if (obj != null)
                        {
                            string v = obj.ToString();
                            v = v.Trim();
                            obj = v;
                        }
                        else
                        {
                            obj = string.Empty;
                        }
                    }
                    else if (p.PropertyType == typeof(bool))
                    {
                        int i = Convert.ToInt32(obj);
                        obj = i == 0 ? false : true;
                    }
                    else if (p.PropertyType == typeof(double))
                    {
                        double d = Convert.ToDouble(obj);
                        obj = d;
                    }
                    else if (p.PropertyType == typeof(int))
                    {
                        int i = Convert.ToInt32(obj);
                        obj = i;
                    }
                    else if (p.PropertyType == typeof(Int64))
                    {
                        Int64 i = Convert.ToInt64(obj);
                        obj = i;
                    }
                    else if (row[p.Name] is Int64)
                    {
                        p.SetValue(entity, Convert.ToInt32(row[p.Name]), null);
                        continue;
                    }
                    p.SetValue(entity, obj, null);
                }
                list.Add(entity);
            }
            return list;
        }

        /// <summary>
        /// 实现对象的深拷贝
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns>T</returns>
        public static T CreateDeepCopy<T>(T obj)
        {
            T t;

            MemoryStream memoryStream = new MemoryStream();

            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(memoryStream, obj);

            memoryStream.Position = 0;

            t = (T)formatter.Deserialize(memoryStream);

            return t;

        }
    }
}
