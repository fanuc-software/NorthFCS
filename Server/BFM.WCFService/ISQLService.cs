/*******************************************************************************
 * Copyright © 2018 代码生成器 版权所有
 * Author: LanGerp 
 * Description: 快速开发平台
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;

namespace BFM.WCFService
{
    [ServiceContract(Namespace = "http://fanuc.com.cn/")]
    public interface ISQLService
    {

        /// <summary>
        /// 获取应用服务器时间
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        DateTime GetServerTime();

        /// <summary>
        /// 获取数据库时间
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        DateTime GetDataBaseTime();

        /// <summary>
        /// 获取记录总数
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        [OperationContract]
        int RowCountWithEF(string strSql, string[] parameterNames, string[] parameterValues);

        /// <summary>
        /// 获取第一行第一列的数据
        /// </summary>
        /// <param name="type">Type typeof(T).AssemblyQualifiedName</param>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        [OperationContract]
        string GetScalarWithEF(string type, string strSql, string[] parameterNames, string[] parameterValues);

        /// <summary>
        /// 获取第一行的数据
        /// </summary>
        /// <param name="type">Type typeof(T).AssemblyQualifiedName</param>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        [OperationContract]
        List<string> GetFirstRowWithEF(string type, string strSql, string[] parameterNames, string[] parameterValues);

        /// <summary>
        /// 使用EF框架获取数据，返回Json
        /// </summary>
        /// <param name="type">Type typeof(T).AssemblyQualifiedName</param>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        [OperationContract]
        string GetJsonDataWithEF(string type, string strSql, string[] parameterNames, string[] parameterValues);

        /// <summary>
        /// 使用EF框架获取数据， 返回DataTable
        /// </summary>
        /// <param name="type">Type typeof(T).AssemblyQualifiedName</param>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        [OperationContract]
        DataTable GetDataTableWithEF(string type, string strSql, string[] parameterNames, string[] parameterValues);

        /// <summary>
        /// 使用EF框架执行SQL语句
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        [OperationContract]
        bool ExecuteSqlWithEF(string strSql, string[] parameterNames, string[] parameterValues);

        /// <summary>
        /// 使用EF框架执行SQL语句组，带事物
        /// </summary>
        /// <param name="strSqls"></param>
        /// <returns></returns>
        [OperationContract]
        bool ExecuteSqlsWithEF(string[] strSqls);


        #region 使用SqlHelper访问数据库

        /// <summary>
        /// 获取第一行第一列的数据
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        [OperationContract]
        string GetScalar(string strSql, string[] parameterNames, string[] parameterValues);

        /// <summary>
        /// 获取第一行的数据到List中
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        [OperationContract]
        List<string> GetFirstRow(string strSql, string[] parameterNames, string[] parameterValues);

        /// <summary>
        /// 根据SQL语句获取数据集合，返回Json
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        [OperationContract]
        string GetJsonData(string strSql);

        /// <summary>
        /// 根据SQL语句获取数据集合（带分页），返回Json
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [OperationContract]
        string GetJsonDataWithPage(string strSql, int pageIndex, int pageSize);

        /// <summary>
        /// 根据SQL语句获取数据集合，返回Json
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        [OperationContract]
        string GetJsonDataByParam(string strSql, string[] parameterNames, string[] parameterValues);

        /// <summary>
        /// 根据SQL语句获取数据集合
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        [OperationContract]
        DataSet GetDataSet(string strSql, string[] parameterNames, string[] parameterValues);

        /// <summary>
        /// 根据SQL语句获取数据表格集合
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        [OperationContract]
        DataTable GetDataTable(string strSql, string[] parameterNames, string[] parameterValues);

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="parameterNames"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        [OperationContract]
        bool ExecuteSqlByParam(string strSql, string[] parameterNames, string[] parameterValues);

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        [OperationContract]
        bool ExecuteSql(string strSql);

        /// <summary>
        /// 执行SQL语句组
        /// </summary>
        /// <param name="strSqLs"></param>
        /// <returns></returns>
        [OperationContract]
        bool ExecuteSqls(string[] strSqLs);

        #endregion
    }
}
