using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using BFM.Common.Base;
using BFM.Common.DataBaseAsset.Enum;

namespace BFM.Common.DataBaseAsset.EF
{
    public class SqlDAL
    {
        private DB_Service dbContext => DBContextFactory.GetService();  //每次新获取一个EF缓存

        private static SqlDAL _currentObject = new SqlDAL();

        static SqlDAL()  //静态gou
        {
            DBContextFactory.GetService();  //
        }

        /// <summary>
        /// 获取Sql的数据库操作类
        /// </summary>
        /// <returns></returns>
        public static SqlDAL GetSqlDal()  
        {
            return _currentObject;
        }

        /// <summary>
        /// 获取应用服务器时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// 使用EF获取数据库时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetDataBaseTime()
        {
            string sql = "";
            switch (DBFactory.DbType)
            {
                case EmDbType.Oracle:
                    sql = "select sysdate from dual";
                    break;
                case EmDbType.SqlServer:
                    sql = "select getdate()";
                    break;
                case EmDbType.MySql:
                    sql = "select sysdate()";
                    break;
                case EmDbType.Access:
                    sql = "select now()";
                    break;
            }
            if (string.IsNullOrEmpty(sql))
            {
                return GetServerTime();
            }
            else
            {
                var time = dbContext.Database.SqlQuery<DateTime>(sql);
                return time.FirstOrDefault();
            }
        }

        /// <summary>
        /// 使用EF框架根据SQL语句获取记录总数
        /// SQL语句为记录的总数
        /// </summary>
        /// <param name="strSql">获取记录总数的SQL语句 SELECT COUNT(*)</param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public int RowCountWithEF(string strSql, string[] parameterNames, string[] parameterValues)
        {
            return dbContext.Database.SqlQuery<int>(strSql, DBFactory.ChangeToDbParams(parameterNames, parameterValues)).FirstOrDefault();
        }

        /// <summary>
        /// 使用EF框架获取第一行第一列数据
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public string GetScalarWithEF(Type type, string strSql, string[] parameterNames, string[] parameterValues)
        {
            if (type == null) return "";

            var value = GetFirstRowWithEF(type, strSql, parameterNames, parameterValues);
            if (value.Count > 0) return value[0];

            return "";
        }

        /// <summary>
        /// 使用EF框架获取第一行第一列数据
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public List<string> GetFirstRowWithEF(Type type, string strSql, string[] parameterNames, string[] parameterValues)
        {
            List<string> result = new List<string>();
            if (type == null) return result;

            DbRawSqlQuery datas = dbContext.Database.SqlQuery(type, strSql, DBFactory.ChangeToDbParams(parameterNames, parameterValues));

            foreach (var data in datas)
            {
                foreach (var info in type.GetProperties())
                {
                    var value = info.GetValue(data, null) ?? DBNull.Value;

                    result.Add(SafeConverter.SafeToStr(value));
                }

                return result;
            }

            return result;
        }

        /// <summary>
        /// 使用EF框架获取数据，返回DataTable
        /// </summary>
        /// <param name="type"></param>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public DataTable GetDataTableWithEF(Type type, string strSql, string[] parameterNames, string[] parameterValues)
        {
            var table = new DataTable("DataTableWithEF");
            DbRawSqlQuery datas =
                dbContext.Database.SqlQuery(type, strSql, DBFactory.ChangeToDbParams(parameterNames, parameterValues));
            foreach (var data in datas)
            {
                var row = table.NewRow();

                foreach (var info in type.GetProperties())
                {
                    if (table.Rows.Count <= 0)
                    {
                        table.Columns.Add(info.Name);
                    }

                    var value = info.GetValue(data, null) ?? DBNull.Value;

                    row[info.Name] = SafeConverter.SafeToStr(value);
                }

                table.Rows.Add(row);
            }

            return table;
        }

        /// <summary>
        /// 使用EF框架执行SQL语句
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public bool ExecuteSqlWithEF(string strSql, string[] parameterNames, string[] parameterValues)
        {
            var result = dbContext.Database.ExecuteSqlCommand(strSql, DBFactory.ChangeToDbParams(parameterNames, parameterValues));
            return (result > 0);
        }

        /// <summary>
        /// 使用EF框架执行SQL语句组，带事物
        /// </summary>
        /// <param name="strSqls"></param>
        /// <returns></returns>
        public bool ExecuteSqlsWithEF(string[] strSqls)
        {
            using (var trans = dbContext.Database.BeginTransaction())
            {
                try
                {
                    foreach (var strSql in strSqls)
                    {
                        dbContext.Database.ExecuteSqlCommand(strSql);
                    }

                    dbContext.SaveChanges();

                    trans.Commit();
                    return true;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    trans.Rollback();
                    return false;
                }
            }
        }

        #region 采用SqlHelper访问数据库

        public string GetScalar(string strSql, string[] parameterNames, string[] parameterValues)
        {
            string str = SqlHelper.GetFirstValue(strSql, DBFactory.ChangeToDbParams(parameterNames, parameterValues));
            return str;
        }

        public List<string> GetFirstRow(string strSql, string[] parameterNames, string[] parameterValues)
        {
            return SqlHelper.GetFirstRow(strSql, DBFactory.ChangeToDbParams(parameterNames, parameterValues));
        }

        public DataSet GetDataSet(string strSql, string[] parameterNames, string[] parameterValues)
        {
            return SqlHelper.GetDataSet(strSql, DBFactory.ChangeToDbParams(parameterNames, parameterValues));
        }

        public DataTable GetDataTable(string strSql, string[] parameterNames, string[] parameterValues)
        {
            return SqlHelper.GetDataTable(strSql, DBFactory.ChangeToDbParams(parameterNames, parameterValues));
        }

        public bool ExecuteSql(string strSql, string[] parameterNames, string[] parameterValues)
        {
            return (SqlHelper.ExecuteNonQuery(strSql, DBFactory.ChangeToDbParams(parameterNames, parameterValues)) == "OK");
        }

        public bool ExecuteSqls(string[] strSqls)
        {
            return (SqlHelper.ExecuteNonQuerys(strSqls) == "OK");
        }

        #endregion
    }
}
