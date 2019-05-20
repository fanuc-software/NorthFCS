using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using BFM.Common.Base.Log;
using BFM.Common.DataBaseAsset.Enum;

namespace BFM.Common.DataBaseAsset
{
    /// <summary>
    /// 数据库的通用访问类
    /// 目前支持：MsSQLServer；Oracle；MySQL；Access 四种数据库
    /// <remarks>
    ///     用法一（主数据库访问）：1.初始化参数 调用 SqlHelper.Initial
    ///           2.根据实际需要调用 ExecuteNonQuery (执行SQL)；ExecuteNonQuerys (执行SQL组，带事务)
    ///             执行查询返回 DataReader（Connection不能释放）、DataSet、GetFirstValue（第一行第一列数据）、GetFirstRow（第一行的数据到List中）
    ///           3.主连接更改后，需要重新条用 SqlHelper.Initial进行初始化
    ///     用法二（临时/数据库设备用）：将数据库类型和连接语句传入执行语句参数。根据实际需要调用 ExecuteNonQuery (执行SQL)；ExecuteNonQuerys (执行SQL组，带事务)
    ///             执行查询返回 DataReader（Connection不能释放）、DataSet、GetFirstValue（第一行第一列数据）、GetFirstRow（第一行的数据到List中）
    /// </remarks>
    /// </summary>
    public sealed class SqlHelper
    {
        #region 主数据库 - private

        /// <summary>
        /// 主 连接字符串
        /// </summary>
        private static string MainConnectionString; //= ConfigurationManager.ConnectionStrings["SQLDirectDB"].connString;

        /// <summary>
        /// 主 数据库连接
        /// </summary>
        private static DbConnection MainConn; //= new SqlConnection(ConfigurationManager.ConnectionStrings["SQLDirectDB"].connString);

        /// <summary>
        /// 主 数据库类型
        /// </summary>
        public static EmDbType MainDbType = EmDbType.None; //主 数据库类型

        #endregion

        #region 0.1 主数据初始化

        /// <summary>
        /// 初始化主连接
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="datasource">数据源</param>
        /// <param name="database">数据库</param>
        /// <param name="userid">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static bool InitialMainConn(EmDbType dbType, string datasource, string database, string userid, string password)
        {
            string connnString = DBFactory.BuildConnStr(dbType, datasource, database, userid, password);

            MainConnectionString = connnString;
            MainDbType = dbType;
            MainConn = DBFactory.NewConnection(dbType, connnString);

            return InitialMainConn(dbType, connnString);
        }
        
        /// <summary>
        /// 初始化主连接
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="connnString">连接字符串</param>
        /// <returns></returns>
        public static bool InitialMainConn(EmDbType dbType, string connnString)
        {
            if (string.IsNullOrEmpty(connnString)) throw new ArgumentNullException("connnString");

            MainConnectionString = connnString;
            MainDbType = dbType;
            MainConn = DBFactory.NewConnection(dbType, connnString);

            return RefreshMainSqlConnection();
        }

        /// <summary>
        /// 刷新主连接
        /// </summary>
        /// <returns></returns>
        public static bool RefreshMainSqlConnection()
        {
            return OpenConnection(MainDbType, MainConnectionString);
        }

        #endregion

        #region 0.2 打开数据库连接

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="connString">数据库连接语句</param>
        /// <returns></returns>
        public static bool OpenConnection(EmDbType dbType, string connString)
        {
            try
            {
                DbConnection conn = DBFactory.NewConnection(dbType, connString);
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                string error = $"打开链接失败，连接语句 [{connString}].";
                EventLogger.Log(error, ex);

                return false;
            }
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="conn">连接语句</param>
        /// <returns></returns>
        public static bool OpenConnection(EmDbType dbType, DbConnection conn)
        {
            try
            {
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                string error = $"打开链接失败，连接语句 [{conn.ConnectionString}].";
                EventLogger.Log(error, ex);

                return false;
            }
        }

        #endregion
        
        #region 1. 带参数的执行SQL语句

        public static string ExecuteNonQuery(string cmdText)
        {
            return ExecuteNonQuery(MainConn, MainDbType, CommandType.Text, cmdText);
        }

        public static string ExecuteNonQuery(string cmdText, params DbParameter[] commandParameters)
        {
            return ExecuteNonQuery(MainConn, MainDbType, CommandType.Text, cmdText, commandParameters);
        }

        public static string ExecuteNonQuery(CommandType cmdType, string cmdText,
            params DbParameter[] commandParameters)
        {
            return ExecuteNonQuery(MainConn, MainDbType, cmdType, cmdText, commandParameters);
        }

        /// <summary>
        /// 执行带参数的SQL语句
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="dbType"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns>OK；错误信息</returns>
        public static string ExecuteNonQuery(DbConnection conn, EmDbType dbType, CommandType cmdType, string cmdText,
            params DbParameter[] commandParameters)
        {
            if (conn == null) throw new ArgumentNullException("conn");
            if (string.IsNullOrEmpty(cmdText)) throw new ArgumentNullException("cmdText");

            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                var cmd = DBFactory.NewCommand(dbType, conn);
                cmd.CommandType = cmdType;
                cmd.CommandText = cmdText;
                if (commandParameters != null)
                {
                    foreach (var param in commandParameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                cmd.ExecuteNonQuery(); //执行
                cmd.Parameters.Clear();
                conn.Close();
                return "OK";
            }
            catch (Exception ex)
            {
                EventLogger.Log($"SqlServerHelper.ExecuteNonQuery SQL[{cmdText}] 错误.", ex);
                OpenConnection(dbType, conn);  //重新连接
                return ex.Message;
            }
        }

        /// <summary>
        /// 执行带参数的SQL语句
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="connString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static string ExecuteNonQuery(EmDbType dbType, string connString, CommandType cmdType, string cmdText,
            params DbParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connString)) throw new ArgumentNullException("connString");
            if (string.IsNullOrEmpty(cmdText)) throw new ArgumentNullException("cmdText");

            try
            {
                DbConnection conn = DBFactory.NewConnection(dbType, connString);

                return ExecuteNonQuery(conn, dbType, cmdType, cmdText, commandParameters);
            }
            catch (Exception ex)
            {
                EventLogger.Log($"SqlServerHelper.ExecuteNonQuery SQL[{cmdText}] 错误.", ex);
                OpenConnection(dbType, connString);  //重新连接
                return ex.Message;
            }

        }

        #endregion

        #region 2. 执行SQL语句组，带事务回滚

        public static string ExecuteNonQuerys(params string[] sqls)
        {
            List<string> sqlList = new List<string>(sqls);
            return ExecuteNonQuerys(sqlList);
        }

        public static string ExecuteNonQuerys(List<string> sqlList)
        {
            return ExecuteNonQuerys(MainConn, MainDbType, sqlList);
        }

        public static string ExecuteNonQuerys(DbConnection conn, EmDbType dbType, params string[] sqls)
        {
            List<string> sqlList = new List<string>(sqls);
            return ExecuteNonQuerys(conn, dbType, sqlList);
        }

        public static string ExecuteNonQuerys(EmDbType dbType, string connString, List<string> sqlList)
        {
            if (string.IsNullOrEmpty(connString)) throw new ArgumentNullException("connString");

            try
            {
                DbConnection conn = DBFactory.NewConnection(dbType, connString);
                return ExecuteNonQuerys(conn, dbType, sqlList);
            }
            catch (Exception ex)
            {
                EventLogger.Log("SqlHelper.ExecuteNonQuerys 错误.", ex);
                OpenConnection(dbType, connString);  //重新连接
                return ex.Message;
            }
        }

        public static string ExecuteNonQuerys(DbConnection conn, EmDbType dbType, List<string> sqlList)
        {
            if (conn == null) throw new ArgumentNullException("conn");

            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var cmd = DBFactory.NewCommand(dbType, conn);

                using (var trans = conn.BeginTransaction())
                {
                    cmd.Transaction = trans;
                    try
                    {
                        foreach (string sql in sqlList)
                        {
                            if (string.IsNullOrWhiteSpace(sql)) continue;

                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        EventLogger.Log("SqlHelper.ExecuteNonQuerys SQL【" + string.Join(";", sqlList.ToArray()) + "】 ", ex);
                        trans.Rollback(); //回滚
                        OpenConnection(dbType, conn);  //重新连接
                        return ex.Message;
                    }
                }

                conn.Close();
                return "OK";
            }
            catch (Exception ex)
            {
                EventLogger.Log("SqlHelper.ExecuteNonQuery 错误.", ex);
                OpenConnection(dbType, conn);  //重新连接
                return ex.Message;
            }
        }
        
        #endregion

        #region 3. 查询语句-返回 DataReader，占资源，一般不传入connection

        public static DbDataReader ExecuterReader(string cmdText)
        {
            return ExecuterReader(MainDbType, MainConnectionString, CommandType.Text, cmdText, null);
        }

        public static DbDataReader ExecuterReader(CommandType cmdType, string cmdText,
            params DbParameter[] commandParameters)
        {
            return ExecuterReader(MainDbType, MainConnectionString, cmdType, cmdText, commandParameters);
        }

        public static DbDataReader ExecuterReader(string cmdText, params DbParameter[] commandParameters)
        {
            return ExecuterReader(MainDbType, MainConnectionString, CommandType.Text, cmdText, commandParameters);
        }

        public static DbDataReader ExecuterReader(EmDbType dbType, string connString, CommandType cmdType, string cmdText,
            params DbParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connString)) throw new ArgumentNullException("connString");
            if (string.IsNullOrEmpty(cmdText)) throw new ArgumentNullException("cmdText");

            try
            {
                var conn = DBFactory.NewConnection(dbType, connString);
                conn.Open();

                var cmd = DBFactory.NewCommand(dbType, conn);
                cmd.CommandType = cmdType;
                cmd.CommandText = cmdText;

                if (commandParameters != null)
                {
                    foreach (var param in commandParameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();

                return dr;
            }
            catch (Exception ex)
            {
                EventLogger.Log("SqlHelper.ExecuterReader 【" + cmdText + "】", ex);
                OpenConnection(dbType, connString);  //重新刷新
                return null;
            }
        }

        #endregion

        #region 4. 查询语句-返回 DataSet

        public static DataSet GetDataSet(string cmdText)
        {
            return GetDataSet(MainConn, MainDbType, CommandType.Text, cmdText, null);
        }

        public static DataSet GetDataSet(string cmdText, params DbParameter[] commandParameters)
        {
            return GetDataSet(MainConn, MainDbType, CommandType.Text, cmdText, commandParameters);
        }

        public static DataSet GetDataSet(CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            return GetDataSet(MainConn, MainDbType, cmdType, cmdText, commandParameters);
        }

        public static DataSet GetDataSet(DbConnection conn, EmDbType dbType, CommandType cmdType, string cmdText,
            params DbParameter[] commandParameters)
        {
            if (conn == null) throw new ArgumentNullException("conn");
            if (string.IsNullOrEmpty(cmdText)) throw new ArgumentNullException("cmdText");

            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                var cmd = DBFactory.NewCommand(dbType, conn);
                cmd.CommandType = cmdType;
                cmd.CommandText = cmdText;

                if (commandParameters != null)
                {
                    foreach (var param in commandParameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                var da = DBFactory.NewDataAdapter(dbType, cmd);
                DataSet ds = new DataSet();
                da.Fill(ds); //填充数据
                cmd.Parameters.Clear();
                conn.Close();
                return ds;
            }
            catch (Exception ex)
            {
                EventLogger.Log("SqlHelper.GetDataSet 【" + cmdText + "】", ex);
                OpenConnection(dbType, conn);  //重新连接
                return null;
            }
        }

        public static DataSet GetDataSet(EmDbType dbType, string connString, CommandType cmdType, string cmdText,
            params DbParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connString)) throw new ArgumentNullException("connString");
            if (string.IsNullOrEmpty(cmdText)) throw new ArgumentNullException("cmdText");

            try
            {
                var conn = DBFactory.NewConnection(dbType, connString);
                conn.Open();

                return GetDataSet(conn, dbType, cmdType, cmdText, commandParameters);
            }
            catch (Exception ex)
            {
                EventLogger.Log("SqlHelper.GetDataSet 【" + cmdText + "】", ex);
                OpenConnection(dbType, connString);  //重新刷新
                return null;
            }
        }

        #endregion

        #region 5. 查询语句-返回 DataTable

        public static DataTable GetDataTable(string cmdText)
        {
            return GetDataTable(MainConn, MainDbType, CommandType.Text, cmdText, null);
        }

        public static DataTable GetDataTable(string cmdText, params DbParameter[] commandParameters)
        {
            return GetDataTable(MainConn, MainDbType, CommandType.Text, cmdText, commandParameters);
        }

        public static DataTable GetDataTable(CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            return GetDataTable(MainConn, MainDbType, cmdType, cmdText, commandParameters);
        }

        public static DataTable GetDataTable(DbConnection conn, EmDbType dbType, CommandType cmdType, string cmdText,
            params DbParameter[] commandParameters)
        {
            if (conn == null) throw new ArgumentNullException("conn");
            if (string.IsNullOrEmpty(cmdText)) throw new ArgumentNullException("cmdText");

            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                var cmd = DBFactory.NewCommand(dbType, conn);
                cmd.CommandType = cmdType;
                cmd.CommandText = cmdText;

                if (commandParameters != null)
                {
                    foreach (var param in commandParameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                var da = DBFactory.NewDataAdapter(dbType, cmd);
                DataSet ds = new DataSet();
                da.Fill(ds); //填充数据
                cmd.Parameters.Clear();
                conn.Close();
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                EventLogger.Log("SqlHelper.GetDataSet 【" + cmdText + "】", ex);
                OpenConnection(dbType, conn);  //重新连接
                return null;
            }
        }

        public static DataTable GetDataTable(EmDbType dbType, string connString, CommandType cmdType, string cmdText,
            params DbParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connString)) throw new ArgumentNullException("connString");
            if (string.IsNullOrEmpty(cmdText)) throw new ArgumentNullException("cmdText");

            try
            {
                var conn = DBFactory.NewConnection(dbType, connString);
                conn.Open();

                return GetDataTable(conn, dbType, cmdType, cmdText, commandParameters);
            }
            catch (Exception ex)
            {
                EventLogger.Log("SqlHelper.GetDataTable 【" + cmdText + "】", ex);
                OpenConnection(dbType, connString);  //重新刷新
                return null;
            }
        }

        #endregion

        #region 6. 查询语句 - 返回第一行第一列数据

        public static string GetFirstValue(string cmdText)
        {
            return GetFirstValue(MainConn, MainDbType, CommandType.Text, cmdText, null);
        }

        public static string GetFirstValue(string cmdText, params DbParameter[] commandParameters)
        {
            return GetFirstValue(MainConn, MainDbType, CommandType.Text, cmdText, commandParameters);
        }

        public static string GetFirstValue(CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            return GetFirstValue(MainConn, MainDbType, cmdType, cmdText, commandParameters);
        }

        public static string GetFirstValue(DbConnection conn, EmDbType dbType, CommandType cmdType, string cmdText,
            params DbParameter[] commandParameters)
        {
            if (conn == null) throw new ArgumentNullException("conn");
            if (string.IsNullOrEmpty(cmdText)) throw new ArgumentNullException("cmdText");

            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                var cmd = DBFactory.NewCommand(dbType, conn);
                cmd.CommandType = cmdType;
                cmd.CommandText = cmdText;

                if (commandParameters != null)
                {
                    foreach (var param in commandParameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                conn.Close();
                return val?.ToString();
            }
            catch (Exception ex)
            {
                EventLogger.Log("SqlHelper.GetFirstValue 【" + cmdText + "】", ex);
                OpenConnection(dbType, conn);  //重新连接
                return null;
            }
        }


        public static string GetFirstValue(EmDbType dbType, string connString, CommandType cmdType, string cmdText,
            params DbParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connString)) throw new ArgumentNullException("connString");
            if (string.IsNullOrEmpty(cmdText)) throw new ArgumentNullException("cmdText");

            try
            {
                var conn = DBFactory.NewConnection(dbType, connString);
                conn.Open();

                return GetFirstValue(conn, dbType, cmdType, cmdText, commandParameters);
            }
            catch (Exception ex)
            {
                EventLogger.Log("SqlHelper.GetFirstValue 【" + cmdText + "】", ex);
                OpenConnection(dbType, connString);  //重新刷新
                return null;
            }
        }

        #endregion

        #region 7. 查询语句 - 返回第一行的数据到List中

        public static List<string> GetFirstRow(string cmdText)
        {
            return GetFirstRow(MainConn, MainDbType, CommandType.Text, cmdText, null);
        }

        public static List<string> GetFirstRow(CommandType cmdType, string cmdText,
            params DbParameter[] commandParameters)
        {
            return GetFirstRow(MainConn, MainDbType, cmdType, cmdText, commandParameters);
        }

        public static List<string> GetFirstRow(string cmdText, params DbParameter[] commandParameters)
        {
            return GetFirstRow(MainConn, MainDbType, CommandType.Text, cmdText, commandParameters);
        }

        public static List<string> GetFirstRow(DbConnection conn, EmDbType dbType, CommandType cmdType, string cmdText,
            params DbParameter[] commandParameters)
        {
            if (conn == null) throw new ArgumentNullException("conn");
            if (string.IsNullOrEmpty(cmdText)) throw new ArgumentNullException("cmdText");

            try
            {
                List<string> result = new List<string>();
                DataSet ds = GetDataSet(conn, dbType, cmdType, cmdText, commandParameters);
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables[0].Rows.Count > 0))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        result.Add(dr[i].ToString());
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                EventLogger.Log("SqlHelper.GetFirstRow 【" + cmdText + "】", ex);
                OpenConnection(dbType, conn);  //重新连接
                return null;
            }
        }
        public static List<string> GetFirstRow(EmDbType dbType, string connString, CommandType cmdType, string cmdText,
            params DbParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connString)) throw new ArgumentNullException("connString");
            if (string.IsNullOrEmpty(cmdText)) throw new ArgumentNullException("cmdText");

            try
            {
                var conn = DBFactory.NewConnection(dbType, connString);
                conn.Open();

                return GetFirstRow(conn, dbType, cmdType, cmdText, commandParameters);
            }
            catch (Exception ex)
            {
                EventLogger.Log("SqlHelper.GetFirstRow 【" + cmdText + "】", ex);
                OpenConnection(dbType, connString);  //重新刷新
                return null;
            }
        }

        #endregion
    }
}