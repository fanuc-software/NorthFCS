using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Diagnostics;
//Ms Sql Server
using Oracle.ManagedDataAccess.Client;   //Oracle
using MySql.Data.MySqlClient;    //MySQL
using BFM.Common.DataBaseAsset.Enum;

namespace BFM.Common.DataBaseAsset
{
    public static class DBFactory
    {
        #region 数据库类型 及 连接语句

        private static EmDbType _dbType = EmDbType.None; //数据库类型
        private static string _dbConnectName = "DB_Service";  //数据库连接语句
        private static string _dbDefaultSchema = "UNV";  //默认命名表空间

        /// <summary>
        /// 数据库类型
        /// </summary>
        public static EmDbType DbType
        {
            get { return _dbType; }
            set { _dbType = value; }
        }

        /// <summary>
        /// 数据库连接语句的访问名称
        /// </summary>
        public static string DbConnectName
        {
            get { return _dbConnectName; }
            set { _dbConnectName = value; }
        }

        /// <summary>
        /// 默认命名表空间
        /// </summary>
        public static string DBDefaultSchema
        {
            get { return _dbDefaultSchema; }
            set { _dbDefaultSchema = value; }
        }

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        static DBFactory()
        {
            RefreshDBSetting();
        }

        /// <summary>
        /// 初始化SQL语句，初始化一次
        /// </summary>
        public static void RefreshDBSetting()
        {
            if ((ConfigurationManager.ConnectionStrings == null) ||
                (ConfigurationManager.ConnectionStrings[_dbConnectName] == null))
            {
                return;
            }

            string connectString = ConfigurationManager.ConnectionStrings[_dbConnectName].ConnectionString;   //获取
            
            #region 解析连接语句

            string[] _Attrib = connectString.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries);

            Dictionary<string, string> _AttribList = new Dictionary<string, string>();

            for (int i = 0; i < _Attrib.Length; i++)
            {
                string[] _AttribValue = _Attrib[i].Split('=');
                _AttribList.Add(_AttribValue[0].ToLower(), _AttribValue[1]);
            }

            #endregion

            if (ConfigurationManager.ConnectionStrings[_dbConnectName].ProviderName ==
                "Oracle.ManagedDataAccess.Client")
            {
                _dbType = EmDbType.Oracle;
                _dbDefaultSchema = _AttribList["user id"];
            }
            else if (ConfigurationManager.ConnectionStrings[_dbConnectName].ProviderName == "MySql.Data.MySqlClient")
            {
                _dbType = EmDbType.MySql;
            }
            else if (ConfigurationManager.ConnectionStrings[_dbConnectName].ProviderName == "System.Data.SqlClient")
            {
                _dbType = EmDbType.SqlServer;
                _dbDefaultSchema = "dbo";
            }
            else if (ConfigurationManager.ConnectionStrings[_dbConnectName].ProviderName ==
                     "JetEntityFrameworkProvider")
            {
                _dbType = EmDbType.Access;
            }

            SqlHelper.InitialMainConn(_dbType, connectString);
        }

        #region SQL语句 参数

        public static string SqlSign
        {
            get
            {
                switch (DbType)
                {
                    case EmDbType.SqlServer:
                        return "@";
                    case EmDbType.Oracle:
                        return ":";
                    case EmDbType.MySql:
                        return ":";
                    default:
                        throw new Exception("配置文件未指定数据库");
                }
            }
        }

        public static string ParamSign
        {
            get
            {
                switch (DbType)
                {
                    case EmDbType.SqlServer:
                        return "";
                    case EmDbType.Oracle:
                        return ":";
                    case EmDbType.MySql:
                        return ":";
                    default:
                        throw new Exception("配置文件未指定数据库");
                }
            }
        }

        #endregion 

        #region 数据库处理事件

        #region 数据库连接相关

        public static string BuildConnStr(string datasource, string database, string userid, string password)
        {
            return BuildConnStr(DbType, datasource, database, userid, password);
        }

        public static string BuildConnStr(EmDbType dbtype, string datasource, string database, string userid, string password)
        {
            string connStr = "";
            switch (dbtype)
            {
                case EmDbType.SqlServer:
                    connStr =
                        String.Format(
                            "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}",
                            datasource, database, userid, password);
                    break;
                case EmDbType.Oracle:
                    connStr =
                        String.Format("DATA SOURCE={0}:1521/{1};PASSWORD={3};PERSIST SECURITY INFO=True;USER ID={2}",
                            datasource, database, userid, password);  //Oracle.ManagedDataAccess
                    //connStr = string.Format("Host={0};Port=1521;User ID={2};Password={3}; Service Name={1}",
                    //        datasource, database, userid, password);   //DDTek.Oracle
                    break;
                case EmDbType.MySql:
                    connStr = String.Format("server={0};user id={2};password={3};database={1}",
                        datasource, database, userid, password);
                    break;
                case EmDbType.Access:
                    if (String.IsNullOrEmpty(userid))
                    {
                        connStr = String.Format("Provider=Microsoft.Jet.OleDb.4.0;Data Source={0};Persist Security Info=False;",
                            datasource);
                    }
                    else
                    {
                        connStr = String.Format("Provider=Microsoft.Jet.OleDb.4.0;Data Source={0};Persist Security Info=True;" +
                                                "user id={1};Jet OleDb:DataBase Password={2};",
                            datasource, userid, password);
                    }
                    break;
                default: //其他
                    connStr = String.Format("data source={0};database={1};user id={2};password={3}",
                        datasource, database, userid, password);
                    break;
            }
            return connStr;
        }
        
        public static DbConnection NewConnection(EmDbType dbtype, string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString");

            switch (dbtype)
            {
                case EmDbType.SqlServer:
                    return new SqlConnection(connectionString);
                case EmDbType.Oracle:
                    return new OracleConnection(connectionString);
                case EmDbType.MySql:
                    return new MySqlConnection(connectionString);
                case EmDbType.Access:
                    return new OleDbConnection(connectionString);
                default:
                    throw new ArgumentException("不支持的数据库类型", dbtype.ToString());
            }
        }

        #endregion

        #region Command 相关
        
        public static DbCommand NewCommand(EmDbType dbtype, DbConnection conn)
        {
            switch (dbtype)
            {
                case EmDbType.SqlServer:
                    if (conn is SqlConnection)
                    {
                        return ((SqlConnection)conn).CreateCommand();
                    }
                    throw new ArgumentException("数据库连接的类型不一致", "conn");
                case EmDbType.Oracle:
                    if (conn is OracleConnection)
                    {
                        return ((OracleConnection)conn).CreateCommand();
                    }
                    throw new ArgumentException("数据库连接的类型不一致", "conn");
                case EmDbType.MySql:
                    if (conn is MySqlConnection)
                    {
                        return ((MySqlConnection)conn).CreateCommand();
                    }
                    throw new ArgumentException("数据库连接的类型不一致", "conn");
                case EmDbType.Access:
                    if (conn is OleDbConnection)
                    {
                        return ((OleDbConnection)conn).CreateCommand();
                    }
                    throw new ArgumentException("数据库连接的类型不一致", "conn");
                default:
                    throw new ArgumentException("不支持的数据库类型", "dbtype");
            }

        }

        #endregion

        #region bDataAdapter 相关

        public static DbDataAdapter NewDataAdapter(EmDbType dbtype, DbCommand cmd)
        {
            switch (dbtype)
            {
                case EmDbType.SqlServer:
                    if (cmd is SqlCommand)
                    {
                        return new SqlDataAdapter((SqlCommand)cmd);
                    }
                    throw new ArgumentException("数据库Command的类型不一致", "cmd");
                case EmDbType.Oracle:
                    if (cmd is OracleCommand)
                    {
                        return new OracleDataAdapter((OracleCommand)cmd);
                    }
                    throw new ArgumentException("数据库Command的类型不一致", "cmd");
                case EmDbType.MySql:
                    if (cmd is MySqlCommand)
                    {
                        return new MySqlDataAdapter((MySqlCommand)cmd);
                    }
                    throw new ArgumentException("数据库Command的类型不一致", "cmd");
                case EmDbType.Access:
                    if (cmd is OleDbCommand)
                    {
                        return new OleDbDataAdapter((OleDbCommand)cmd);
                    }
                    throw new ArgumentException("数据库Command的类型不一致", "cmd");
                default:
                    throw new ArgumentException("不支持的数据库类型", "dbtype");
            }
        }

        #endregion

        #region  Parameter

        public static DbParameter[] ChangeToDbParams(string[] parameterNames, string[] parameterValues)
        {
            return ChangeToDbParams(DbType, parameterNames, parameterValues);
        }


        /// <summary>
        /// SQL语句的参数类型转换
        /// </summary>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public static DbParameter[] ChangeToDbParams(EmDbType dbtype, string[] parameterNames, string[] parameterValues)
        {
            List<DbParameter> parameterItems = new List<DbParameter>();
            if ((parameterNames == null) || (parameterValues == null))
            {
                return parameterItems.ToArray();
            }
            for (int i = 0; i < parameterNames.Length; i++)
            {
                DbParameter dbParam = null;
                switch (dbtype)
                {
                    case EmDbType.SqlServer:
                        dbParam = new SqlParameter(parameterNames[i], parameterValues[i]);
                        break;
                    case EmDbType.Oracle:
                        dbParam = new OracleParameter(parameterNames[i], parameterValues[i]);
                        break;
                    case EmDbType.MySql:
                        dbParam = new MySqlParameter(parameterNames[i], parameterValues[i]);
                        break;
                    case EmDbType.Access:
                        dbParam = new OleDbParameter(parameterNames[i], parameterValues[i]);
                        break;
                }
                parameterItems.Add(dbParam);
            }
            return parameterItems.ToArray();
        }

        #endregion

        #endregion
    }
}
