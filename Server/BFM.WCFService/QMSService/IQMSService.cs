/*******************************************************************************
 * Copyright © 2018 代码生成器 版权所有
 * Author: LanGerp 
 * Description: 快速开发平台
*********************************************************************************/

using System.Collections.Generic;
using System.ServiceModel;
using BFM.ContractModel;

namespace BFM.WCFService
{
    [ServiceContract(Namespace = "http://fanuc.com.cn/")]
    public interface IQMSService
    {

        #region 质量测试表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [OperationContract]
        List <QmsTest> GetQmsTestByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetQmsTestCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mQmsTest">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddQmsTest(QmsTest mQmsTest);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mQmsTest">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateQmsTest(QmsTest mQmsTest);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelQmsTests(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelQmsTest(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<QmsTest> GetQmsTests(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        QmsTest GetQmsTestById(string Id);

        #endregion 质量测试表 的服务接口

        #region 质量检测参数表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [OperationContract]
        List <QmsCheckParam> GetQmsCheckParamByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetQmsCheckParamCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mQmsCheckParam">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddQmsCheckParam(QmsCheckParam mQmsCheckParam);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mQmsCheckParam">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateQmsCheckParam(QmsCheckParam mQmsCheckParam);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelQmsCheckParams(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelQmsCheckParam(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<QmsCheckParam> GetQmsCheckParams(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        QmsCheckParam GetQmsCheckParamById(string Id);

        #endregion 质量检测参数表 的服务接口

        #region 质量检验方案表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [OperationContract]
        List <QmsRoutingCheck> GetQmsRoutingCheckByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetQmsRoutingCheckCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mQmsRoutingCheck">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddQmsRoutingCheck(QmsRoutingCheck mQmsRoutingCheck);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mQmsRoutingCheck">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateQmsRoutingCheck(QmsRoutingCheck mQmsRoutingCheck);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelQmsRoutingChecks(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelQmsRoutingCheck(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<QmsRoutingCheck> GetQmsRoutingChecks(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        QmsRoutingCheck GetQmsRoutingCheckById(string Id);

        #endregion 质量检验方案表 的服务接口

        #region 质量检测主表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [OperationContract]
        List <QmsCheckMaster> GetQmsCheckMasterByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetQmsCheckMasterCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mQmsCheckMaster">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddQmsCheckMaster(QmsCheckMaster mQmsCheckMaster);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mQmsCheckMaster">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateQmsCheckMaster(QmsCheckMaster mQmsCheckMaster);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelQmsCheckMasters(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelQmsCheckMaster(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<QmsCheckMaster> GetQmsCheckMasters(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        QmsCheckMaster GetQmsCheckMasterById(string Id);

        #endregion 质量检测主表 的服务接口
    }
}
