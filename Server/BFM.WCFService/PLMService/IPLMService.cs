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
    public interface IPLMService
    {

        #region 生产计划表 的服务接口

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
        List <PmPlanMaster> GetPmPlanMasterByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetPmPlanMasterCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mPmPlanMaster">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddPmPlanMaster(PmPlanMaster mPmPlanMaster);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mPmPlanMaster">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdatePmPlanMaster(PmPlanMaster mPmPlanMaster);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelPmPlanMasters(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelPmPlanMaster(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<PmPlanMaster> GetPmPlanMasters(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        PmPlanMaster GetPmPlanMasterById(string Id);

        #endregion 生产计划表 的服务接口

        #region 生产任务表 的服务接口

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
        List <PmTaskMaster> GetPmTaskMasterByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetPmTaskMasterCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mPmTaskMaster">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddPmTaskMaster(PmTaskMaster mPmTaskMaster);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mPmTaskMaster">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdatePmTaskMaster(PmTaskMaster mPmTaskMaster);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelPmTaskMasters(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelPmTaskMaster(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<PmTaskMaster> GetPmTaskMasters(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        PmTaskMaster GetPmTaskMasterById(string Id);

        #endregion 生产任务表 的服务接口

        #region 生产线任务表 的服务接口

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
        List <PmTaskLine> GetPmTaskLineByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetPmTaskLineCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mPmTaskLine">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddPmTaskLine(PmTaskLine mPmTaskLine);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mPmTaskLine">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdatePmTaskLine(PmTaskLine mPmTaskLine);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelPmTaskLines(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelPmTaskLine(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<PmTaskLine> GetPmTaskLines(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        PmTaskLine GetPmTaskLineById(string Id);

        #endregion 生产线任务表 的服务接口

        #region 工单表 的服务接口

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
        List <MesJobOrder> GetMesJobOrderByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetMesJobOrderCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mMesJobOrder">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddMesJobOrder(MesJobOrder mMesJobOrder);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mMesJobOrder">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateMesJobOrder(MesJobOrder mMesJobOrder);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelMesJobOrders(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelMesJobOrder(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<MesJobOrder> GetMesJobOrders(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        MesJobOrder GetMesJobOrderById(string Id);

        #endregion 工单表 的服务接口

        #region 生产过程表 的服务接口

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
        List <MesProcessCtrol> GetMesProcessCtrolByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetMesProcessCtrolCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mMesProcessCtrol">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddMesProcessCtrol(MesProcessCtrol mMesProcessCtrol);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mMesProcessCtrol">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateMesProcessCtrol(MesProcessCtrol mMesProcessCtrol);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelMesProcessCtrols(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelMesProcessCtrol(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<MesProcessCtrol> GetMesProcessCtrols(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        MesProcessCtrol GetMesProcessCtrolById(string Id);

        #endregion 生产过程表 的服务接口

        #region 测试EF框架 的服务接口

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
        List <TestEFCodeFirst> GetTestEFCodeFirstByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetTestEFCodeFirstCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mTestEFCodeFirst">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddTestEFCodeFirst(TestEFCodeFirst mTestEFCodeFirst);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mTestEFCodeFirst">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateTestEFCodeFirst(TestEFCodeFirst mTestEFCodeFirst);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelTestEFCodeFirsts(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelTestEFCodeFirst(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<TestEFCodeFirst> GetTestEFCodeFirsts(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        TestEFCodeFirst GetTestEFCodeFirstById(string Id);

        #endregion 测试EF框架 的服务接口

        #region 产品生产情况表 的服务接口

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
        List <MesProductProcess> GetMesProductProcessByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetMesProductProcessCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mMesProductProcess">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddMesProductProcess(MesProductProcess mMesProductProcess);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mMesProductProcess">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateMesProductProcess(MesProductProcess mMesProductProcess);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelMesProductProcesss(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelMesProductProcess(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<MesProductProcess> GetMesProductProcesss(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        MesProductProcess GetMesProductProcessById(string Id);

        #endregion 产品生产情况表 的服务接口
    }
}
