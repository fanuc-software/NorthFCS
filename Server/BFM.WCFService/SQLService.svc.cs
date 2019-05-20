using System;
using System.Collections.Generic;
using System.Data;
using BFM.Common.Base;
using BFM.Common.DataBaseAsset.EF;

namespace BFM.WCFService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“SQLService”。
    public class SQLService : ISQLService
    {
        /// <summary>
        /// 获取应用服务器时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            using (SqlBLL service = new SqlBLL())
            {
                return service.GetServerTime();
            }
        }

        /// <summary>
        /// 获取数据库时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetDataBaseTime()
        {
            using (SqlBLL service = new SqlBLL())
            {
                return service.GetDataBaseTime();
            }
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public int RowCountWithEF(string strSql, string[] parameterNames, string[] parameterValues)
        {
            using (SqlBLL service = new SqlBLL())
            {
                return service.RowCountWithEF(strSql, parameterNames, parameterValues);
            }
        }

        /// <summary>
        /// 获取第一行第一列的数据
        /// </summary>
        /// <param name="type">Type typeof(T).AssemblyQualifiedName</param>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public string GetScalarWithEF(string type, string strSql, string[] parameterNames, string[] parameterValues)
        {
            using (SqlBLL service = new SqlBLL())
            {
                Type t = Type.GetType(type);
                return service.GetScalarWithEF(t, strSql, parameterNames, parameterValues);
            }
        }

        /// <summary>
        /// 获取第一行的数据
        /// </summary>
        /// <param name="type">Type typeof(T).AssemblyQualifiedName</param>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public List<string> GetFirstRowWithEF(string type, string strSql, string[] parameterNames, string[] parameterValues)
        {
            using (SqlBLL service = new SqlBLL())
            {
                Type t = Type.GetType(type);
                return service.GetFirstRowWithEF(t, strSql, parameterNames, parameterValues);
            }
        }

        /// <summary>
        /// 使用EF框架获取数据，按照Json返回
        /// </summary>
        /// <param name="type">Type typeof(T).AssemblyQualifiedName</param>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public string GetJsonDataWithEF(string type, string strSql, string[] parameterNames, string[] parameterValues)
        {
            DataTable dt = GetDataTableWithEF(type, strSql, parameterNames, parameterValues);

            return SafeConverter.JsonSerializeObject(dt);
        }

        /// <summary>
        /// 使用EF框架获取数据，返回DataTable
        /// </summary>
        /// <param name="type">Type typeof(T).AssemblyQualifiedName</param>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public DataTable GetDataTableWithEF(string type, string strSql, string[] parameterNames, string[] parameterValues)
        {
            using (SqlBLL service = new SqlBLL())
            {
                DataTable dt = new DataTable("DataTableWithEF");

                Type t = Type.GetType(type);
                if (t != null)
                {
                    dt = service.GetDataTableWithEF(t, strSql, parameterNames, parameterValues);
                }

                return dt;
            }
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
            using (SqlBLL service = new SqlBLL())
            {
                return service.ExecuteSqlWithEF(strSql, parameterNames, parameterValues);
            }
        }

        /// <summary>
        /// 使用EF框架执行SQL语句组，带事物
        /// </summary>
        /// <param name="strSqls"></param>
        /// <returns></returns>
        public bool ExecuteSqlsWithEF(string[] strSqls)
        {
            using (SqlBLL service = new SqlBLL())
            {
                return service.ExecuteSqlsWithEF(strSqls);
            }
        }

        #region 使用SqlHelper访问数据库

        /// <summary>
        /// 获取第一行第一列的数据
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public string GetScalar(string strSql, string[] parameterNames, string[] parameterValues)
        {
            using (SqlBLL service = new SqlBLL())
            {
                return service.GetScalar(strSql, parameterNames, parameterValues);
            }
        }

        /// <summary>
        /// 获取第一行的数据到List中
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public List<string> GetFirstRow(string strSql, string[] parameterNames, string[] parameterValues)
        {
            using (SqlBLL service = new SqlBLL())
            {
                return service.GetFirstRow(strSql, parameterNames, parameterValues);
            }
        }

        /// <summary>
        /// 根据SQL语句获取数据集合，返回Json
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public string GetJsonData(string strSql)
        {
            DataTable dt = GetDataTable(strSql, null, null);

            return SafeConverter.JsonSerializeObject(dt);
        }

        /// <summary>
        /// 根据SQL语句获取数据集合，返回Json
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public string GetJsonDataWithPage(string strSql, int pageIndex, int pageSize)
        {
            DataTable dt = GetDataTable(strSql, null, null);
            DataTable newdt = dt.Copy();  //copy dt的框架
            if (pageIndex > 0)
            {
                newdt.Clear();
                int rowbegin = (pageIndex - 1) * pageSize;
                int rowend = pageIndex * pageSize;

                if (rowbegin < dt.Rows.Count)
                {
                    if (rowend > dt.Rows.Count) rowend = dt.Rows.Count;

                    for (int i = rowbegin; i <= rowend - 1; i++)
                    {
                        DataRow newdr = newdt.NewRow();
                        DataRow dr = dt.Rows[i];
                        foreach (DataColumn column in dt.Columns)
                        {
                            newdr[column.ColumnName] = dr[column.ColumnName];
                        }

                        newdt.Rows.Add(newdr);
                    }
                }
            }
            return SafeConverter.JsonSerializeObject(newdt);
        }

        /// <summary>
        /// 根据SQL语句获取数据集合，返回Json
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public string GetJsonDataByParam(string strSql, string[] parameterNames, string[] parameterValues)
        {
            DataTable dt = GetDataTable(strSql, parameterNames, parameterValues);

            return SafeConverter.JsonSerializeObject(dt);
        }

        /// <summary>
        /// 根据SQL语句获取数据集合
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string strSql, string[] parameterNames, string[] parameterValues)
        {
            using (SqlBLL service = new SqlBLL())
            {
                return service.GetDataSet(strSql, parameterNames, parameterValues);
            }
        }

        /// <summary>
        /// 根据SQL语句获取数据表格集合
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string strSql, string[] parameterNames, string[] parameterValues)
        {
            using (SqlBLL service = new SqlBLL())
            {
                return service.GetDataTable(strSql, parameterNames, parameterValues);
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public bool ExecuteSqlByParam(string strSql, string[] parameterNames, string[] parameterValues)
        {
            using (SqlBLL service = new SqlBLL())
            {
                return service.ExecuteSql(strSql, parameterNames, parameterValues);
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public bool ExecuteSql(string strSql)
        {
            using (SqlBLL service = new SqlBLL())
            {
                return service.ExecuteSql(strSql, null, null);
            }
        }

        /// <summary>
        /// 执行SQL语句组
        /// </summary>
        /// <param name="strSqLs"></param>
        /// <returns></returns>
        public bool ExecuteSqls(string[] strSqLs)
        {
            using (SqlBLL service = new SqlBLL())
            {
                return service.ExecuteSqls(strSqLs);
            }
        }

        #endregion

    }
}
