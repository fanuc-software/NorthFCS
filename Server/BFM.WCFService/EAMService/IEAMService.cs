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
    public interface IEAMService
    {

        #region 设备台账表N 的服务接口

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
        List <AmAssetMasterN> GetAmAssetMasterNByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetAmAssetMasterNCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mAmAssetMasterN">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddAmAssetMasterN(AmAssetMasterN mAmAssetMasterN);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mAmAssetMasterN">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateAmAssetMasterN(AmAssetMasterN mAmAssetMasterN);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelAmAssetMasterNs(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelAmAssetMasterN(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<AmAssetMasterN> GetAmAssetMasterNs(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        AmAssetMasterN GetAmAssetMasterNById(string Id);

        #endregion 设备台账表N 的服务接口

        #region 备件台账表N 的服务接口

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
        List <AmPartsMasterN> GetAmPartsMasterNByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetAmPartsMasterNCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mAmPartsMasterN">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddAmPartsMasterN(AmPartsMasterN mAmPartsMasterN);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mAmPartsMasterN">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateAmPartsMasterN(AmPartsMasterN mAmPartsMasterN);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelAmPartsMasterNs(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelAmPartsMasterN(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<AmPartsMasterN> GetAmPartsMasterNs(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        AmPartsMasterN GetAmPartsMasterNById(string Id);

        #endregion 备件台账表N 的服务接口

        #region 维护规程表 的服务接口

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
        List <RsMaintainStandards> GetRsMaintainStandardsByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetRsMaintainStandardsCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mRsMaintainStandards">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddRsMaintainStandards(RsMaintainStandards mRsMaintainStandards);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mRsMaintainStandards">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateRsMaintainStandards(RsMaintainStandards mRsMaintainStandards);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelRsMaintainStandardss(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelRsMaintainStandards(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<RsMaintainStandards> GetRsMaintainStandardss(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        RsMaintainStandards GetRsMaintainStandardsById(string Id);

        #endregion 维护规程表 的服务接口

        #region 维护规程明细表 的服务接口

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
        List <RsMaintainStandardsDetail> GetRsMaintainStandardsDetailByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetRsMaintainStandardsDetailCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mRsMaintainStandardsDetail">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddRsMaintainStandardsDetail(RsMaintainStandardsDetail mRsMaintainStandardsDetail);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mRsMaintainStandardsDetail">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateRsMaintainStandardsDetail(RsMaintainStandardsDetail mRsMaintainStandardsDetail);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelRsMaintainStandardsDetails(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelRsMaintainStandardsDetail(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<RsMaintainStandardsDetail> GetRsMaintainStandardsDetails(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        RsMaintainStandardsDetail GetRsMaintainStandardsDetailById(string Id);

        #endregion 维护规程明细表 的服务接口

        #region 维护规程关系表 的服务接口

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
        List <RsMaintainStandardsRelate> GetRsMaintainStandardsRelateByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetRsMaintainStandardsRelateCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mRsMaintainStandardsRelate">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddRsMaintainStandardsRelate(RsMaintainStandardsRelate mRsMaintainStandardsRelate);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mRsMaintainStandardsRelate">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateRsMaintainStandardsRelate(RsMaintainStandardsRelate mRsMaintainStandardsRelate);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelRsMaintainStandardsRelates(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelRsMaintainStandardsRelate(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<RsMaintainStandardsRelate> GetRsMaintainStandardsRelates(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        RsMaintainStandardsRelate GetRsMaintainStandardsRelateById(string Id);

        #endregion 维护规程关系表 的服务接口

        #region 设备维修记录表 的服务接口

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
        List <RmRepairRecord> GetRmRepairRecordByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetRmRepairRecordCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mRmRepairRecord">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddRmRepairRecord(RmRepairRecord mRmRepairRecord);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mRmRepairRecord">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateRmRepairRecord(RmRepairRecord mRmRepairRecord);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelRmRepairRecords(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelRmRepairRecord(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<RmRepairRecord> GetRmRepairRecords(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        RmRepairRecord GetRmRepairRecordById(string Id);

        #endregion 设备维修记录表 的服务接口

        #region 维护记录表 的服务接口

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
        List <RsMaintainRecord> GetRsMaintainRecordByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetRsMaintainRecordCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mRsMaintainRecord">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddRsMaintainRecord(RsMaintainRecord mRsMaintainRecord);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mRsMaintainRecord">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateRsMaintainRecord(RsMaintainRecord mRsMaintainRecord);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelRsMaintainRecords(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelRsMaintainRecord(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<RsMaintainRecord> GetRsMaintainRecords(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        RsMaintainRecord GetRsMaintainRecordById(string Id);

        #endregion 维护记录表 的服务接口
        
    }
}
