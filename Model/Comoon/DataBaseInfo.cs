using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Comoon
{
    [Serializable]
    public class DataBaseInfo
    {
        private string dbSource;
        /// <summary>
        /// 数据源(IP)
        /// </summary>
        public string DbSource
        {
            get { return dbSource; }
            set { dbSource = value; }
        }

        private string uid;
        /// <summary>
        /// 用户名
        /// </summary>
        public string Uid
        {
            get { return uid; }
            set { uid = value; }
        }

        private string pwd;
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd
        {
            get { return pwd; }
            set { pwd = value; }
        }

        private string dataBaseName;
        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DataBaseName
        {
            get { return dataBaseName; }
            set { dataBaseName = value; }
        }


        /// <summary>
        /// 端口
        /// </summary>
        public string Port
        {
            get { return dbSource.Substring(dbSource.IndexOf(':') + 1); }
        }

        private int dbType;
        /// <summary>
        /// 数据库类型
        /// </summary>
        public int DbType
        {
            get { return dbType; }
            set { dbType = value; }
        }

        private bool isNew;

        /// <summary>
        /// 是否新建
        /// </summary>
        public bool IsNew
        {
            get { return isNew; }
            set { isNew = value; }
        }

        private string defaultConnDbName;
        /// <summary>
        /// 默认连接数据库
        /// </summary>
        public string DefaultConnDbName
        {
            get
            {
                if (DbType == (int)DataBaseTypeEnum.SQLServer)
                { return "master"; }
                else
                { return DataBaseName; }
            }
            set { defaultConnDbName = value; }
        }

        private string dbTypeName;
        /// <summary>
        /// 数据库类型名称
        /// </summary>
        public string DbTypeName
        {
            get { return dbTypeName; }
            set
            {
                dbTypeName = value;
                if (dbTypeName.ToUpper().Contains("SQLSERVER"))
                { DbType = (int)DataBaseTypeEnum.SQLServer; }
                else if (dbTypeName.ToUpper().Contains("ORACLE"))
                { DbType = (int)DataBaseTypeEnum.Oracle; }
                else
                { DbType = (int)DataBaseTypeEnum.SQLServer; }
            }
        }

        private double version = 5.0;
        /// <summary>
        /// 版本
        /// </summary>
        public double Version
        {
            get { return version; }
            set { version = value; }
        }

        public DataBaseInfo()
        { }

        /// <summary>
        ///  DataBaseInfo构造函数
        /// </summary>
        /// <param name="serverName">服务器名</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <param name="dbName">数据库名</param>
        public DataBaseInfo(string serverName, string userName, string passWord, string dbName)
        {
            this.dbSource = serverName;
            this.uid = userName;
            this.pwd = passWord;
            this.dataBaseName = dbName;
        }


        /// <summary>
        ///  DataBaseInfo构造函数
        /// </summary>
        /// <param name="serverName">服务器名</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <param name="dbName">数据库名</param>
        /// <param name="dbType">数据库类型</param>
        public DataBaseInfo(string serverName, string userName, string passWord, string dbName, int dbType)
        {
            this.dbSource = serverName;
            this.uid = userName;
            this.pwd = passWord;
            this.dataBaseName = dbName;
            this.dbType = dbType;
        }


        /// <summary>
        ///  DataBaseInfo构造函数
        /// </summary>
        /// <param name="serverName">服务器名</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <param name="dbName">数据库名</param>
        public DataBaseInfo(DataBaseInfo dataBaseInfo, bool isNew)
        {
            this.dbSource = dataBaseInfo.DbSource;
            this.uid = dataBaseInfo.Uid;
            this.pwd = dataBaseInfo.Pwd;
            this.dataBaseName = dataBaseInfo.DataBaseName;
            this.dbType = dataBaseInfo.DbType;
            this.isNew = isNew;
        }


        /// <summary>
        ///  DataBaseInfo构造函数
        /// </summary>
        /// <param name="serverName">服务器名</param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        /// <param name="dbName">数据库名</param>
        public DataBaseInfo(DataBaseInfo dataBaseInfo)
        {
            this.dbSource = dataBaseInfo.DbSource;
            this.uid = dataBaseInfo.Uid;
            this.pwd = dataBaseInfo.Pwd;
            this.dataBaseName = dataBaseInfo.DataBaseName;
            this.isNew = dataBaseInfo.IsNew;
            this.DbType = dataBaseInfo.DbType;
        }
    }
}
