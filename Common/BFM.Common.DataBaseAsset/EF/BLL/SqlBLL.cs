using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFM.Common.DataBaseAsset.EF
{
    public class SqlBLL : IDisposable
    {
		private SqlDAL _dal = SqlDAL.GetSqlDal();

        /// <summary>
        /// 获取应用服务器时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            return _dal.GetServerTime();
        }

        /// <summary>
        /// 获取数据库时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetDataBaseTime()
        {
            return _dal.GetDataBaseTime();
        }

        /// <summary>
        /// 根据SQL语句获取记录总数
        /// SQL语句为记录的总数
        /// </summary>
        /// <param name="strSql">获取记录总数的SQL语句 SELECT COUNT(*)</param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public int RowCountWithEF(string strSql, string[] parameterNames, string[] parameterValues)
        {
            return _dal.RowCountWithEF(strSql, parameterNames, parameterValues);
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
            return _dal.GetScalarWithEF(type, strSql, parameterNames, parameterValues);
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
            return _dal.GetFirstRowWithEF(type, strSql, parameterNames, parameterValues);
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
            if (type != null) return _dal.GetDataTableWithEF(type, strSql, parameterNames, parameterValues);
            return new DataTable("DataTableWithEF");
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
            return _dal.ExecuteSqlWithEF(strSql, parameterNames, parameterValues);
        }

        /// <summary>
        /// 使用EF框架执行SQL语句组，带事物
        /// </summary>
        /// <param name="strSqls"></param>
        /// <returns></returns>
        public bool ExecuteSqlsWithEF(string[] strSqls)
        {
            return _dal.ExecuteSqlsWithEF(strSqls);
        }

        #region 使用SqlHelper访问数据库

        /// <summary>
        /// 获取第一行第一列数据
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public string GetScalar(string strSql, string[] parameterNames, string[] parameterValues)
        {
            return _dal.GetScalar(strSql, parameterNames, parameterValues);
        }

        /// <summary>
        /// 获取第一行数据
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public List<string> GetFirstRow(string strSql, string[] parameterNames, string[] parameterValues)
        {
            return _dal.GetFirstRow(strSql, parameterNames, parameterValues);
        }

        public DataSet GetDataSet(string strSql, string[] parameterNames, string[] parameterValues)
        {
            return _dal.GetDataSet(strSql, parameterNames, parameterValues);
        }

        public DataTable GetDataTable(string strSql, string[] parameterNames, string[] parameterValues)
        {
            return _dal.GetDataTable(strSql, parameterNames, parameterValues);
        }

        public bool ExecuteSql(string strSql, string[] parameterNames, string[] parameterValues)
        {
            return _dal.ExecuteSql(strSql, parameterNames, parameterValues);
        }

        public bool ExecuteSqls(string[] strSqLs)
        {
            return _dal.ExecuteSqls(strSqLs);
        }

        #endregion

        public void Dispose()
        {
            _dal = null;
        }
    }
}
