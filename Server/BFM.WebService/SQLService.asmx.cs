/*******************************************************************************
 * Copyright © 2018 代码生成器 版权所有
 * Author: LanGerp 
 * Description: 快速开发平台
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using BFM.Common.Base;
using BFM.Common.DataBaseAsset.EF;

namespace BFM.WebService
{
    /// <summary>
    /// SQLService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://fanuc.com.cn/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class SQLService : System.Web.Services.WebService
    {
        /// <summary>
        /// 获取应用服务器时间
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "获取应用服务器时间")]
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
        [WebMethod(Description = "获取数据库时间")]
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
        [WebMethod(Description = "获取记录总数")]
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
        [WebMethod(Description = "获取第一行第一列的数据")]
        public string GetScalarWithEF(string type, string strSql, string[] parameterNames, string[] parameterValues)
        {
            using (SqlBLL service = new SqlBLL())
            {
                Type t = Type.GetType(type);
                return service.GetScalarWithEF(t, strSql, parameterNames, parameterValues);
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
        [WebMethod(Description = "获取第一行的数据")]
        public List<string> GetFirstRowWithEF(string type, string strSql, string[] parameterNames, string[] parameterValues)
        {
            using (SqlBLL service = new SqlBLL())
            {
                Type t = Type.GetType(type);
                return service.GetFirstRowWithEF(t, strSql, parameterNames, parameterValues);
            }
        }

        /// <summary>
        /// 使用EF框架获取数据，返回Json
        /// </summary>
        /// <param name="type">Type  typeof(T).AssemblyQualifiedName</param>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        [WebMethod(Description = "使用EF框架获取数据，返回Json")]
        public string GetJsonDataWithEF(string type, string strSql, string[] parameterNames, string[] parameterValues)
        {
            DataTable dt = GetDataTableWithEF(type, strSql, parameterNames, parameterValues);

            return SafeConverter.JsonSerializeObject(dt);
        }

        /// <summary>
        /// 使用EF框架获取数据，返回DataTable
        /// </summary>
        /// <param name="type">Type  typeof(T).AssemblyQualifiedName</param>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        [WebMethod(Description = "使用EF框架获取数据，返回DataTable")]
        public DataTable GetDataTableWithEF(string type, string strSql, string[] parameterNames, string[] parameterValues)
        {
            using (SqlBLL service = new SqlBLL())
            {
                Type t = Type.GetType(type);
                if (t != null) return service.GetDataTableWithEF(t, strSql, parameterNames, parameterValues);

                return new DataTable("GetDataTableWithEF");
            }
        }

        /// <summary>
        /// 使用EF框架执行SQL语句
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        [WebMethod(Description = "使用EF框架执行SQL语句")]
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
        [WebMethod(Description = "使用EF框架执行SQL语句组，带事物")]
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
        [WebMethod(Description = "获取第一行第一列的数据")]
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
        /// <param name="strSQL"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        [WebMethod(Description = "获取第一行的数据到List中")]
        public List<string> GetFirstRow(string strSQL, string[] parameterNames, string[] parameterValues)
        {
            using (SqlBLL service = new SqlBLL())
            {
                return service.GetFirstRow(strSQL, parameterNames, parameterValues);
            }
        }

        /// <summary>
        /// 根据SQL语句获取数据集合，返回Json
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        [WebMethod(Description = "根据SQL语句获取数据集合，返回Json")]
        public string GetJsonData(string strSql)
        {
            DataTable dt = GetDataTable(strSql, null, null);

            return SafeConverter.JsonSerializeObject(dt);
        }

        /// <summary>
        /// 根据SQL语句获取数据集合（带分页），返回Json
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="pageIndex">表示第几页</param>
        /// <param name="pageSize">每页的记录数</param>
        /// <returns></returns>
        [WebMethod(Description = "根据SQL语句获取数据集合（带分页），返回Json")]
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
        [WebMethod(Description = "根据SQL语句获取数据集合，返回Json")]
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
        [WebMethod(Description = "根据SQL语句获取数据集合")]
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
        [WebMethod(Description = "根据SQL语句获取数据表格集合")]
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
        /// <param name="strSQL"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        [WebMethod(Description = "执行SQL语句")]
        public bool ExecuteSqlByParam(string strSQL, string[] parameterNames, string[] parameterValues)
        {
            using (SqlBLL service = new SqlBLL())
            {
                return service.ExecuteSql(strSQL, parameterNames, parameterValues);
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        [WebMethod(Description = "执行SQL语句")]
        public bool ExecuteSql(string strSQL)
        {
            using (SqlBLL service = new SqlBLL())
            {
                return service.ExecuteSql(strSQL, null, null);
            }
        }


        /// <summary>
        /// 执行SQL语句组
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "执行SQL语句组")]
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
