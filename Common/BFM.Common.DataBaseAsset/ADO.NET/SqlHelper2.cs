using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using BFM.Common.Base.Log;

namespace BFM.Common.DataBaseAsset
{
    /// <summary>
    /// 数据库的通用访问类
    /// 目前支持：MsSQLServer；Oracle；MySQL；Access 四种数据库
    /// <remarks>
    ///     用法：1.初始化参数 调用 SqlHelper.Initial
    ///           2.根据实际需要调用 ExecuteNonQuery (执行SQL)；ExecuteNonQuerys (执行SQL组，带事务)
    ///             执行查询返回 DataReader（Connection不能释放）、DataSet、GetFirstValue（第一行第一列数据）、GetFirstRow（第一行的数据到List中）
    /// </remarks>
    /// </summary>
    public sealed class SqlHelper
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        private static string MainConnectionString; //= ConfigurationManager.ConnectionStrings["SQLDirectDB"].ConnectionString;

        /// <summary>
        /// 主 数据库连接
        /// </summary>
        private static DbConnection MainConn; //= new SqlConnection(ConfigurationManager.ConnectionStrings["SQLDirectDB"].ConnectionString);
        
        #region 连接

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="dbtype">数据库类型</param>
        /// <param name="datasource">数据源</param>
        /// <param name="database">数据库</param>
        /// <param name="userid">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static bool Initial(string datasource, string database, string userid, string password)
        {
            string connnString = DBFactory.BuildConnStr(datasource, database, userid, password);
            return Initial(connnString);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="dbtype">数据库类型</param>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        public static bool Initial(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString");
            
            MainConnectionString = connectionString;

            return RefreshSqlConnection(connectionString);
        }

        /// <summary>
        /// 刷新连接
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static bool RefreshSqlConnection()
        {
            return RefreshSqlConnection(MainConnectionString);
        }
        /// <summary>
        /// 刷新连接
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static bool RefreshSqlConnection(string connectionString)
        {
            try
            {
                if (!string.IsNullOrEmpty(connectionString))
                {
                    MainConnectionString = connectionString;
                }

                MainConn = DBFactory.NewConnection(MainConnectionString);
                MainConn.Open();
                return true;
            }
            catch (Exception ex)
            {
                string error = $"打开链接失败，连接语句 [{connectionString}].";
                EventLogger.Log(error, ex);

                return false;
            }
        }

        #endregion

        #region 带参数的执行SQL语句

        public static string ExecuteNonQuery(string cmdText)
        {
            return ExecuteNonQuery(MainConn, CommandType.Text, cmdText, null);
        }

        public static string ExecuteNonQuery(string cmdText, params DbParameter[] commandParameters)
        {
            return ExecuteNonQuery(MainConn, CommandType.Text, cmdText, commandParameters);
        }

        public static string ExecuteNonQuery(CommandType cmdType, string cmdText,
            params DbParameter[] commandParameters)
        {
            return ExecuteNonQuery(MainConn, cmdType, cmdText, commandParameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns>OK；错误信息</returns>
        public static string ExecuteNonQuery(DbConnection conn, CommandType cmdType, string cmdText,
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

                var cmd = DBFactory.NewCommand(conn);
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
                RefreshSqlConnection();
                return ex.Message;
            }
        }

        #endregion

        #region 执行SQL语句组，带事务回滚

        public static string ExecuteNonQuerys(params string[] sqls)
        {
            List<string> sqlList = new List<string>(sqls);
            return ExecuteNonQuerys(sqlList);
        }

        public static string ExecuteNonQuerys(List<string> sqlList)
        {
            return ExecuteNonQuerys(MainConn, sqlList);
        }

        public static string ExecuteNonQuerys(DbConnection conn, params string[] sqls)
        {
            List<string> sqlList = new List<string>(sqls);
            return ExecuteNonQuerys(conn, sqlList);
        }

        public static string ExecuteNonQuerys(DbConnection conn, List<string> sqlList)
        {
            if (conn == null) throw new ArgumentNullException("conn");

            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                var cmd = DBFactory.NewCommand(conn);

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
                        RefreshSqlConnection();
                        return ex.Message;
                    }
                }
                conn.Close();
                return "OK";
            }
            catch (Exception ex)
            {
                EventLogger.Log("SqlHelper.ExecuteNonQuery 错误.", ex);
                RefreshSqlConnection();
                return ex.Message;
            }
        }


        #endregion

        #region 查询语句-返回 DataReader，占资源

        public static DbDataReader ExecuterReader(string cmdText)
        {
            return ExecuterReader(MainConnectionString, CommandType.Text, cmdText, null);
        }

        public static DbDataReader ExecuterReader(CommandType cmdType, string cmdText,
            params DbParameter[] commandParameters)
        {
            return ExecuterReader(MainConnectionString, cmdType, cmdText, commandParameters);
        }

        public static DbDataReader ExecuterReader(string cmdText, params DbParameter[] commandParameters)
        {
            return ExecuterReader(MainConnectionString, CommandType.Text, cmdText, commandParameters);
        }

        public static DbDataReader ExecuterReader(string connectionString, CommandType cmdType, string cmdText,
            params DbParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString");
            if (string.IsNullOrEmpty(cmdText)) throw new ArgumentNullException("cmdText");

            try
            {
                var conn = DBFactory.NewConnection(connectionString);
                conn.Open();

                var cmd = DBFactory.NewCommand(conn);
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
                RefreshSqlConnection();
                return null;
            }
        }

        #endregion

        #region 查询语句-返回 DataSet

        public static DataSet GetDataSet(string cmdText)
        {
            return GetDataSet(MainConn, CommandType.Text, cmdText, null);
        }

        public static DataSet GetDataSet(string cmdText, params DbParameter[] commandParameters)
        {
            return GetDataSet(MainConn, CommandType.Text, cmdText, commandParameters);
        }

        public static DataSet GetDataSet(CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            return GetDataSet(MainConn, cmdType, cmdText, commandParameters);
        }

        public static DataSet GetDataSet(DbConnection conn, CommandType cmdType, string cmdText,
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

                var cmd = DBFactory.NewCommand(conn);
                cmd.CommandType = cmdType;
                cmd.CommandText = cmdText;

                if (commandParameters != null)
                {
                    foreach (var param in commandParameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                var da = DBFactory.NewDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds); //填充数据
                cmd.Parameters.Clear();
                conn.Close();
                return ds;
            }
            catch (Exception ex)
            {
                EventLogger.Log("SqlHelper.GetDataSet 【" + cmdText + "】", ex);
                RefreshSqlConnection();
                return null;
            }
        }

        #endregion

        #region 查询语句-返回 DataTable

        public static DataTable GetDataTable(string cmdText)
        {
            return GetDataTable(MainConn, CommandType.Text, cmdText, null);
        }

        public static DataTable GetDataTable(string cmdText, params DbParameter[] commandParameters)
        {
            return GetDataTable(MainConn, CommandType.Text, cmdText, commandParameters);
        }

        public static DataTable GetDataTable(CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            return GetDataTable(MainConn, cmdType, cmdText, commandParameters);
        }

        public static DataTable GetDataTable(DbConnection conn, CommandType cmdType, string cmdText,
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

                var cmd = DBFactory.NewCommand(conn);
                cmd.CommandType = cmdType;
                cmd.CommandText = cmdText;

                if (commandParameters != null)
                {
                    foreach (var param in commandParameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                var da = DBFactory.NewDataAdapter(cmd);
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
                RefreshSqlConnection();
                return null;
            }
        }

        #endregion

        #region 查询语句 - 返回第一行第一列数据

        public static string GetFirstValue(string cmdText)
        {
            return GetFirstValue(MainConn, CommandType.Text, cmdText, null);
        }

        public static string GetFirstValue(string cmdText, params DbParameter[] commandParameters)
        {
            return GetFirstValue(MainConn, CommandType.Text, cmdText, commandParameters);
        }

        public static string GetFirstValue(CommandType cmdType, string cmdText, params DbParameter[] commandParameters)
        {
            return GetFirstValue(MainConn, cmdType, cmdText, commandParameters);
        }

        public static string GetFirstValue(DbConnection conn, CommandType cmdType, string cmdText,
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

                var cmd = DBFactory.NewCommand(conn);
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
                RefreshSqlConnection();
                return null;
            }
        }

        #endregion

        #region 查询语句 - 返回第一行的数据到List中

        public static List<string> GetFirstRow(string cmdText)
        {
            return GetFirstRow(MainConn, CommandType.Text, cmdText, null);
        }

        public static List<string> GetFirstRow(CommandType cmdType, string cmdText,
            params DbParameter[] commandParameters)
        {
            return GetFirstRow(MainConn, cmdType, cmdText, commandParameters);
        }

        public static List<string> GetFirstRow(string cmdText, params DbParameter[] commandParameters)
        {
            return GetFirstRow(MainConn, CommandType.Text, cmdText, commandParameters);
        }

        public static List<string> GetFirstRow(DbConnection conn, CommandType cmdType, string cmdText,
            params DbParameter[] commandParameters)
        {
            if (conn == null) throw new ArgumentNullException("conn");
            if (string.IsNullOrEmpty(cmdText)) throw new ArgumentNullException("cmdText");

            try
            {
                List<string> result = new List<string>();
                DataSet ds = GetDataSet(conn, cmdType, cmdText, commandParameters);
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
                RefreshSqlConnection();
                return null;
            }
        }

        #endregion
    }
}