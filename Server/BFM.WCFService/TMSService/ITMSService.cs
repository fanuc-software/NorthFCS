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
    public interface ITMSService
    {

        #region 刀具类型表 的服务接口

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
        List <TmsToolsType> GetTmsToolsTypeByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetTmsToolsTypeCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mTmsToolsType">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddTmsToolsType(TmsToolsType mTmsToolsType);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mTmsToolsType">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateTmsToolsType(TmsToolsType mTmsToolsType);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelTmsToolsTypes(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelTmsToolsType(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<TmsToolsType> GetTmsToolsTypes(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        TmsToolsType GetTmsToolsTypeById(string Id);

        #endregion 刀具类型表 的服务接口

        #region 刀具台账表 的服务接口

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
        List <TmsToolsMaster> GetTmsToolsMasterByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetTmsToolsMasterCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mTmsToolsMaster">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddTmsToolsMaster(TmsToolsMaster mTmsToolsMaster);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mTmsToolsMaster">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateTmsToolsMaster(TmsToolsMaster mTmsToolsMaster);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelTmsToolsMasters(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelTmsToolsMaster(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<TmsToolsMaster> GetTmsToolsMasters(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        TmsToolsMaster GetTmsToolsMasterById(string Id);

        #endregion 刀具台账表 的服务接口

        #region 设备刀位信息表 的服务接口

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
        List <TmsDeviceToolsPos> GetTmsDeviceToolsPosByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetTmsDeviceToolsPosCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mTmsDeviceToolsPos">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddTmsDeviceToolsPos(TmsDeviceToolsPos mTmsDeviceToolsPos);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mTmsDeviceToolsPos">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateTmsDeviceToolsPos(TmsDeviceToolsPos mTmsDeviceToolsPos);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelTmsDeviceToolsPoss(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelTmsDeviceToolsPos(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<TmsDeviceToolsPos> GetTmsDeviceToolsPoss(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        TmsDeviceToolsPos GetTmsDeviceToolsPosById(string Id);

        #endregion 设备刀位信息表 的服务接口
    }
}
