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
    public interface IDAService
    {

        #region 实时信息采集表 的服务接口

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
        List <DAMachineRealTimeInfo> GetDAMachineRealTimeInfoByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetDAMachineRealTimeInfoCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mDAMachineRealTimeInfo">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddDAMachineRealTimeInfo(DAMachineRealTimeInfo mDAMachineRealTimeInfo);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mDAMachineRealTimeInfo">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateDAMachineRealTimeInfo(DAMachineRealTimeInfo mDAMachineRealTimeInfo);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelDAMachineRealTimeInfos(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelDAMachineRealTimeInfo(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<DAMachineRealTimeInfo> GetDAMachineRealTimeInfos(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        DAMachineRealTimeInfo GetDAMachineRealTimeInfoById(string Id);

        #endregion 实时信息采集表 的服务接口

        #region 产量记录表 的服务接口

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
        List <DAProductRecord> GetDAProductRecordByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetDAProductRecordCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mDAProductRecord">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddDAProductRecord(DAProductRecord mDAProductRecord);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mDAProductRecord">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateDAProductRecord(DAProductRecord mDAProductRecord);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelDAProductRecords(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelDAProductRecord(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<DAProductRecord> GetDAProductRecords(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        DAProductRecord GetDAProductRecordById(string Id);

        #endregion 产量记录表 的服务接口

        #region 报警记录表 的服务接口

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
        List <DAAlarmRecord> GetDAAlarmRecordByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetDAAlarmRecordCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mDAAlarmRecord">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddDAAlarmRecord(DAAlarmRecord mDAAlarmRecord);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mDAAlarmRecord">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateDAAlarmRecord(DAAlarmRecord mDAAlarmRecord);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelDAAlarmRecords(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelDAAlarmRecord(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<DAAlarmRecord> GetDAAlarmRecords(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        DAAlarmRecord GetDAAlarmRecordById(string Id);

        #endregion 报警记录表 的服务接口

        #region 状态记录表 的服务接口

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
        List <DAStatusRecord> GetDAStatusRecordByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetDAStatusRecordCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mDAStatusRecord">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddDAStatusRecord(DAStatusRecord mDAStatusRecord);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mDAStatusRecord">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateDAStatusRecord(DAStatusRecord mDAStatusRecord);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelDAStatusRecords(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelDAStatusRecord(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<DAStatusRecord> GetDAStatusRecords(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        DAStatusRecord GetDAStatusRecordById(string Id);

        #endregion 状态记录表 的服务接口
    }
}
