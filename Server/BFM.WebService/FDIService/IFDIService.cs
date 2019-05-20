/*******************************************************************************
 * Copyright © 2018 代码生成器 版权所有
 * Author: LanGerp 
 * Description: 快速开发平台
*********************************************************************************/

using System.Collections.Generic;
using System.ServiceModel;
using System.Web.Services;
using BFM.ContractModel;

namespace BFM.WebService
{
    [ServiceContract(Namespace = "http://fanuc.com.cn/")]
    public interface IFDIService
    {

        #region 原材料入库接受单_主表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页FDIGetMaterial")]
        List <FDIGetMaterial> GetFDIGetMaterialByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIGetMaterial记录总数 配合分页查询用")]
        int GetFDIGetMaterialCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIGetMaterial">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIGetMaterial")]
        bool AddFDIGetMaterial(FDIGetMaterial mFDIGetMaterial);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIGetMaterial">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIGetMaterial")]
        bool UpdateFDIGetMaterial(FDIGetMaterial mFDIGetMaterial);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIGetMaterial")]
        bool DelFDIGetMaterials(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIGetMaterial")]
        bool DelFDIGetMaterial(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIGetMaterial")]
        List<FDIGetMaterial> GetFDIGetMaterials(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIGetMaterial")]
        FDIGetMaterial GetFDIGetMaterialById(string Id);

        #endregion 原材料入库接受单_主表 的服务接口

        #region 原材料入库接受单_明细表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页FDIGetMaterialDetail")]
        List <FDIGetMaterialDetail> GetFDIGetMaterialDetailByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIGetMaterialDetail记录总数 配合分页查询用")]
        int GetFDIGetMaterialDetailCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIGetMaterialDetail">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIGetMaterialDetail")]
        bool AddFDIGetMaterialDetail(FDIGetMaterialDetail mFDIGetMaterialDetail);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIGetMaterialDetail">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIGetMaterialDetail")]
        bool UpdateFDIGetMaterialDetail(FDIGetMaterialDetail mFDIGetMaterialDetail);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIGetMaterialDetail")]
        bool DelFDIGetMaterialDetails(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIGetMaterialDetail")]
        bool DelFDIGetMaterialDetail(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIGetMaterialDetail")]
        List<FDIGetMaterialDetail> GetFDIGetMaterialDetails(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIGetMaterialDetail")]
        FDIGetMaterialDetail GetFDIGetMaterialDetailById(string Id);

        #endregion 原材料入库接受单_明细表 的服务接口

        #region 原材料入库接受单_主表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页FDIGetRawMaterial")]
        List <FDIGetRawMaterial> GetFDIGetRawMaterialByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIGetRawMaterial记录总数 配合分页查询用")]
        int GetFDIGetRawMaterialCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIGetRawMaterial">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIGetRawMaterial")]
        bool AddFDIGetRawMaterial(FDIGetRawMaterial mFDIGetRawMaterial);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIGetRawMaterial">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIGetRawMaterial")]
        bool UpdateFDIGetRawMaterial(FDIGetRawMaterial mFDIGetRawMaterial);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIGetRawMaterial")]
        bool DelFDIGetRawMaterials(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIGetRawMaterial")]
        bool DelFDIGetRawMaterial(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIGetRawMaterial")]
        List<FDIGetRawMaterial> GetFDIGetRawMaterials(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIGetRawMaterial")]
        FDIGetRawMaterial GetFDIGetRawMaterialById(string Id);

        #endregion 原材料入库接受单_主表 的服务接口

        #region 原材料入库接受单_明细表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页FDIGetRawMaterialDetail")]
        List <FDIGetRawMaterialDetail> GetFDIGetRawMaterialDetailByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIGetRawMaterialDetail记录总数 配合分页查询用")]
        int GetFDIGetRawMaterialDetailCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIGetRawMaterialDetail">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIGetRawMaterialDetail")]
        bool AddFDIGetRawMaterialDetail(FDIGetRawMaterialDetail mFDIGetRawMaterialDetail);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIGetRawMaterialDetail">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIGetRawMaterialDetail")]
        bool UpdateFDIGetRawMaterialDetail(FDIGetRawMaterialDetail mFDIGetRawMaterialDetail);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIGetRawMaterialDetail")]
        bool DelFDIGetRawMaterialDetails(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIGetRawMaterialDetail")]
        bool DelFDIGetRawMaterialDetail(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIGetRawMaterialDetail")]
        List<FDIGetRawMaterialDetail> GetFDIGetRawMaterialDetails(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIGetRawMaterialDetail")]
        FDIGetRawMaterialDetail GetFDIGetRawMaterialDetailById(string Id);

        #endregion 原材料入库接受单_明细表 的服务接口

        #region 生产订单_主表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页FDIGetWOrder")]
        List <FDIGetWOrder> GetFDIGetWOrderByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIGetWOrder记录总数 配合分页查询用")]
        int GetFDIGetWOrderCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIGetWOrder">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIGetWOrder")]
        bool AddFDIGetWOrder(FDIGetWOrder mFDIGetWOrder);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIGetWOrder">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIGetWOrder")]
        bool UpdateFDIGetWOrder(FDIGetWOrder mFDIGetWOrder);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIGetWOrder")]
        bool DelFDIGetWOrders(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIGetWOrder")]
        bool DelFDIGetWOrder(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIGetWOrder")]
        List<FDIGetWOrder> GetFDIGetWOrders(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIGetWOrder")]
        FDIGetWOrder GetFDIGetWOrderById(string Id);

        #endregion 生产订单_主表 的服务接口

        #region 生产订单_明细表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页FDIGetWOrderDetail")]
        List <FDIGetWOrderDetail> GetFDIGetWOrderDetailByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIGetWOrderDetail记录总数 配合分页查询用")]
        int GetFDIGetWOrderDetailCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIGetWOrderDetail">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIGetWOrderDetail")]
        bool AddFDIGetWOrderDetail(FDIGetWOrderDetail mFDIGetWOrderDetail);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIGetWOrderDetail">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIGetWOrderDetail")]
        bool UpdateFDIGetWOrderDetail(FDIGetWOrderDetail mFDIGetWOrderDetail);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIGetWOrderDetail")]
        bool DelFDIGetWOrderDetails(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIGetWOrderDetail")]
        bool DelFDIGetWOrderDetail(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIGetWOrderDetail")]
        List<FDIGetWOrderDetail> GetFDIGetWOrderDetails(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIGetWOrderDetail")]
        FDIGetWOrderDetail GetFDIGetWOrderDetailById(string Id);

        #endregion 生产订单_明细表 的服务接口

        #region 销售发货单草稿_主表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页FDIGetSaleOrder")]
        List <FDIGetSaleOrder> GetFDIGetSaleOrderByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIGetSaleOrder记录总数 配合分页查询用")]
        int GetFDIGetSaleOrderCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIGetSaleOrder">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIGetSaleOrder")]
        bool AddFDIGetSaleOrder(FDIGetSaleOrder mFDIGetSaleOrder);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIGetSaleOrder">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIGetSaleOrder")]
        bool UpdateFDIGetSaleOrder(FDIGetSaleOrder mFDIGetSaleOrder);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIGetSaleOrder")]
        bool DelFDIGetSaleOrders(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIGetSaleOrder")]
        bool DelFDIGetSaleOrder(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIGetSaleOrder")]
        List<FDIGetSaleOrder> GetFDIGetSaleOrders(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIGetSaleOrder")]
        FDIGetSaleOrder GetFDIGetSaleOrderById(string Id);

        #endregion 销售发货单草稿_主表 的服务接口

        #region 销售发货单草稿_明细 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页FDIGetSaleOrderDetail")]
        List <FDIGetSaleOrderDetail> GetFDIGetSaleOrderDetailByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIGetSaleOrderDetail记录总数 配合分页查询用")]
        int GetFDIGetSaleOrderDetailCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIGetSaleOrderDetail">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIGetSaleOrderDetail")]
        bool AddFDIGetSaleOrderDetail(FDIGetSaleOrderDetail mFDIGetSaleOrderDetail);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIGetSaleOrderDetail">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIGetSaleOrderDetail")]
        bool UpdateFDIGetSaleOrderDetail(FDIGetSaleOrderDetail mFDIGetSaleOrderDetail);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIGetSaleOrderDetail")]
        bool DelFDIGetSaleOrderDetails(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIGetSaleOrderDetail")]
        bool DelFDIGetSaleOrderDetail(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIGetSaleOrderDetail")]
        List<FDIGetSaleOrderDetail> GetFDIGetSaleOrderDetails(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIGetSaleOrderDetail")]
        FDIGetSaleOrderDetail GetFDIGetSaleOrderDetailById(string Id);

        #endregion 销售发货单草稿_明细 的服务接口

        #region 物料主数据同步 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页FDIGetMaterialInfo")]
        List <FDIGetMaterialInfo> GetFDIGetMaterialInfoByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIGetMaterialInfo记录总数 配合分页查询用")]
        int GetFDIGetMaterialInfoCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIGetMaterialInfo">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIGetMaterialInfo")]
        bool AddFDIGetMaterialInfo(FDIGetMaterialInfo mFDIGetMaterialInfo);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIGetMaterialInfo">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIGetMaterialInfo")]
        bool UpdateFDIGetMaterialInfo(FDIGetMaterialInfo mFDIGetMaterialInfo);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIGetMaterialInfo")]
        bool DelFDIGetMaterialInfos(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIGetMaterialInfo")]
        bool DelFDIGetMaterialInfo(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIGetMaterialInfo")]
        List<FDIGetMaterialInfo> GetFDIGetMaterialInfos(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIGetMaterialInfo")]
        FDIGetMaterialInfo GetFDIGetMaterialInfoById(string Id);

        #endregion 物料主数据同步 的服务接口

        #region 生产订单报工_主表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页FDIPostWOClose")]
        List <FDIPostWOClose> GetFDIPostWOCloseByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIPostWOClose记录总数 配合分页查询用")]
        int GetFDIPostWOCloseCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIPostWOClose">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIPostWOClose")]
        bool AddFDIPostWOClose(FDIPostWOClose mFDIPostWOClose);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIPostWOClose">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIPostWOClose")]
        bool UpdateFDIPostWOClose(FDIPostWOClose mFDIPostWOClose);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIPostWOClose")]
        bool DelFDIPostWOCloses(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIPostWOClose")]
        bool DelFDIPostWOClose(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIPostWOClose")]
        List<FDIPostWOClose> GetFDIPostWOCloses(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIPostWOClose")]
        FDIPostWOClose GetFDIPostWOCloseById(string Id);

        #endregion 生产订单报工_主表 的服务接口

        #region 生产订单报工_明细表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页FDIPostWOCloseDetail")]
        List <FDIPostWOCloseDetail> GetFDIPostWOCloseDetailByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIPostWOCloseDetail记录总数 配合分页查询用")]
        int GetFDIPostWOCloseDetailCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIPostWOCloseDetail">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIPostWOCloseDetail")]
        bool AddFDIPostWOCloseDetail(FDIPostWOCloseDetail mFDIPostWOCloseDetail);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIPostWOCloseDetail">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIPostWOCloseDetail")]
        bool UpdateFDIPostWOCloseDetail(FDIPostWOCloseDetail mFDIPostWOCloseDetail);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIPostWOCloseDetail")]
        bool DelFDIPostWOCloseDetails(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIPostWOCloseDetail")]
        bool DelFDIPostWOCloseDetail(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIPostWOCloseDetail")]
        List<FDIPostWOCloseDetail> GetFDIPostWOCloseDetails(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIPostWOCloseDetail")]
        FDIPostWOCloseDetail GetFDIPostWOCloseDetailById(string Id);

        #endregion 生产订单报工_明细表 的服务接口

        #region 生产订单报工_批次 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页FDIPostWOCloseBatch")]
        List <FDIPostWOCloseBatch> GetFDIPostWOCloseBatchByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIPostWOCloseBatch记录总数 配合分页查询用")]
        int GetFDIPostWOCloseBatchCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIPostWOCloseBatch">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIPostWOCloseBatch")]
        bool AddFDIPostWOCloseBatch(FDIPostWOCloseBatch mFDIPostWOCloseBatch);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIPostWOCloseBatch">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIPostWOCloseBatch")]
        bool UpdateFDIPostWOCloseBatch(FDIPostWOCloseBatch mFDIPostWOCloseBatch);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIPostWOCloseBatch")]
        bool DelFDIPostWOCloseBatchs(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIPostWOCloseBatch")]
        bool DelFDIPostWOCloseBatch(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIPostWOCloseBatch")]
        List<FDIPostWOCloseBatch> GetFDIPostWOCloseBatchs(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIPostWOCloseBatch")]
        FDIPostWOCloseBatch GetFDIPostWOCloseBatchById(string Id);

        #endregion 生产订单报工_批次 的服务接口

        #region 生产订单发料_主表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页FDIPostWOIssue")]
        List <FDIPostWOIssue> GetFDIPostWOIssueByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIPostWOIssue记录总数 配合分页查询用")]
        int GetFDIPostWOIssueCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIPostWOIssue">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIPostWOIssue")]
        bool AddFDIPostWOIssue(FDIPostWOIssue mFDIPostWOIssue);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIPostWOIssue">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIPostWOIssue")]
        bool UpdateFDIPostWOIssue(FDIPostWOIssue mFDIPostWOIssue);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIPostWOIssue")]
        bool DelFDIPostWOIssues(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIPostWOIssue")]
        bool DelFDIPostWOIssue(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIPostWOIssue")]
        List<FDIPostWOIssue> GetFDIPostWOIssues(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIPostWOIssue")]
        FDIPostWOIssue GetFDIPostWOIssueById(string Id);

        #endregion 生产订单发料_主表 的服务接口

        #region 生产订单发料_明细表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页FDIPostWOIssueDetail")]
        List <FDIPostWOIssueDetail> GetFDIPostWOIssueDetailByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIPostWOIssueDetail记录总数 配合分页查询用")]
        int GetFDIPostWOIssueDetailCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIPostWOIssueDetail">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIPostWOIssueDetail")]
        bool AddFDIPostWOIssueDetail(FDIPostWOIssueDetail mFDIPostWOIssueDetail);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIPostWOIssueDetail">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIPostWOIssueDetail")]
        bool UpdateFDIPostWOIssueDetail(FDIPostWOIssueDetail mFDIPostWOIssueDetail);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIPostWOIssueDetail")]
        bool DelFDIPostWOIssueDetails(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIPostWOIssueDetail")]
        bool DelFDIPostWOIssueDetail(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIPostWOIssueDetail")]
        List<FDIPostWOIssueDetail> GetFDIPostWOIssueDetails(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIPostWOIssueDetail")]
        FDIPostWOIssueDetail GetFDIPostWOIssueDetailById(string Id);

        #endregion 生产订单发料_明细表 的服务接口

        #region 生产订单发料_批次 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页FDIPostWOIssueBatch")]
        List <FDIPostWOIssueBatch> GetFDIPostWOIssueBatchByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIPostWOIssueBatch记录总数 配合分页查询用")]
        int GetFDIPostWOIssueBatchCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIPostWOIssueBatch">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIPostWOIssueBatch")]
        bool AddFDIPostWOIssueBatch(FDIPostWOIssueBatch mFDIPostWOIssueBatch);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIPostWOIssueBatch">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIPostWOIssueBatch")]
        bool UpdateFDIPostWOIssueBatch(FDIPostWOIssueBatch mFDIPostWOIssueBatch);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIPostWOIssueBatch")]
        bool DelFDIPostWOIssueBatchs(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIPostWOIssueBatch")]
        bool DelFDIPostWOIssueBatch(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIPostWOIssueBatch")]
        List<FDIPostWOIssueBatch> GetFDIPostWOIssueBatchs(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIPostWOIssueBatch")]
        FDIPostWOIssueBatch GetFDIPostWOIssueBatchById(string Id);

        #endregion 生产订单发料_批次 的服务接口

        #region 销售发货单_主表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页FDIPostSaleOrder")]
        List <FDIPostSaleOrder> GetFDIPostSaleOrderByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIPostSaleOrder记录总数 配合分页查询用")]
        int GetFDIPostSaleOrderCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIPostSaleOrder">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIPostSaleOrder")]
        bool AddFDIPostSaleOrder(FDIPostSaleOrder mFDIPostSaleOrder);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIPostSaleOrder">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIPostSaleOrder")]
        bool UpdateFDIPostSaleOrder(FDIPostSaleOrder mFDIPostSaleOrder);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIPostSaleOrder")]
        bool DelFDIPostSaleOrders(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIPostSaleOrder")]
        bool DelFDIPostSaleOrder(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIPostSaleOrder")]
        List<FDIPostSaleOrder> GetFDIPostSaleOrders(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIPostSaleOrder")]
        FDIPostSaleOrder GetFDIPostSaleOrderById(string Id);

        #endregion 销售发货单_主表 的服务接口

        #region 销售发货单_明细 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页FDIPostSaleOrderDetail")]
        List <FDIPostSaleOrderDetail> GetFDIPostSaleOrderDetailByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIPostSaleOrderDetail记录总数 配合分页查询用")]
        int GetFDIPostSaleOrderDetailCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIPostSaleOrderDetail">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIPostSaleOrderDetail")]
        bool AddFDIPostSaleOrderDetail(FDIPostSaleOrderDetail mFDIPostSaleOrderDetail);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIPostSaleOrderDetail">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIPostSaleOrderDetail")]
        bool UpdateFDIPostSaleOrderDetail(FDIPostSaleOrderDetail mFDIPostSaleOrderDetail);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIPostSaleOrderDetail")]
        bool DelFDIPostSaleOrderDetails(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIPostSaleOrderDetail")]
        bool DelFDIPostSaleOrderDetail(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIPostSaleOrderDetail")]
        List<FDIPostSaleOrderDetail> GetFDIPostSaleOrderDetails(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIPostSaleOrderDetail")]
        FDIPostSaleOrderDetail GetFDIPostSaleOrderDetailById(string Id);

        #endregion 销售发货单_明细 的服务接口

        #region 销售发货单_批次 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页FDIPostSaleOrderBatch")]
        List <FDIPostSaleOrderBatch> GetFDIPostSaleOrderBatchByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIPostSaleOrderBatch记录总数 配合分页查询用")]
        int GetFDIPostSaleOrderBatchCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIPostSaleOrderBatch">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIPostSaleOrderBatch")]
        bool AddFDIPostSaleOrderBatch(FDIPostSaleOrderBatch mFDIPostSaleOrderBatch);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIPostSaleOrderBatch">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIPostSaleOrderBatch")]
        bool UpdateFDIPostSaleOrderBatch(FDIPostSaleOrderBatch mFDIPostSaleOrderBatch);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIPostSaleOrderBatch")]
        bool DelFDIPostSaleOrderBatchs(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIPostSaleOrderBatch")]
        bool DelFDIPostSaleOrderBatch(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIPostSaleOrderBatch")]
        List<FDIPostSaleOrderBatch> GetFDIPostSaleOrderBatchs(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIPostSaleOrderBatch")]
        FDIPostSaleOrderBatch GetFDIPostSaleOrderBatchById(string Id);

        #endregion 销售发货单_批次 的服务接口

        #region 原材料入库_主表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页FDIPostRawMaterial")]
        List <FDIPostRawMaterial> GetFDIPostRawMaterialByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIPostRawMaterial记录总数 配合分页查询用")]
        int GetFDIPostRawMaterialCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIPostRawMaterial">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIPostRawMaterial")]
        bool AddFDIPostRawMaterial(FDIPostRawMaterial mFDIPostRawMaterial);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIPostRawMaterial">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIPostRawMaterial")]
        bool UpdateFDIPostRawMaterial(FDIPostRawMaterial mFDIPostRawMaterial);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIPostRawMaterial")]
        bool DelFDIPostRawMaterials(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIPostRawMaterial")]
        bool DelFDIPostRawMaterial(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIPostRawMaterial")]
        List<FDIPostRawMaterial> GetFDIPostRawMaterials(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIPostRawMaterial")]
        FDIPostRawMaterial GetFDIPostRawMaterialById(string Id);

        #endregion 原材料入库_主表 的服务接口

        #region 原材料入库_明细表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页FDIPostRawMaterialDetail")]
        List <FDIPostRawMaterialDetail> GetFDIPostRawMaterialDetailByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIPostRawMaterialDetail记录总数 配合分页查询用")]
        int GetFDIPostRawMaterialDetailCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIPostRawMaterialDetail">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIPostRawMaterialDetail")]
        bool AddFDIPostRawMaterialDetail(FDIPostRawMaterialDetail mFDIPostRawMaterialDetail);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIPostRawMaterialDetail">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIPostRawMaterialDetail")]
        bool UpdateFDIPostRawMaterialDetail(FDIPostRawMaterialDetail mFDIPostRawMaterialDetail);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIPostRawMaterialDetail")]
        bool DelFDIPostRawMaterialDetails(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIPostRawMaterialDetail")]
        bool DelFDIPostRawMaterialDetail(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIPostRawMaterialDetail")]
        List<FDIPostRawMaterialDetail> GetFDIPostRawMaterialDetails(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIPostRawMaterialDetail")]
        FDIPostRawMaterialDetail GetFDIPostRawMaterialDetailById(string Id);

        #endregion 原材料入库_明细表 的服务接口

        #region 原材料入库_明细表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页FDIPostRawMaterialBatch")]
        List <FDIPostRawMaterialBatch> GetFDIPostRawMaterialBatchByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIPostRawMaterialBatch记录总数 配合分页查询用")]
        int GetFDIPostRawMaterialBatchCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIPostRawMaterialBatch">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIPostRawMaterialBatch")]
        bool AddFDIPostRawMaterialBatch(FDIPostRawMaterialBatch mFDIPostRawMaterialBatch);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIPostRawMaterialBatch">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIPostRawMaterialBatch")]
        bool UpdateFDIPostRawMaterialBatch(FDIPostRawMaterialBatch mFDIPostRawMaterialBatch);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIPostRawMaterialBatch")]
        bool DelFDIPostRawMaterialBatchs(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIPostRawMaterialBatch")]
        bool DelFDIPostRawMaterialBatch(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIPostRawMaterialBatch")]
        List<FDIPostRawMaterialBatch> GetFDIPostRawMaterialBatchs(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIPostRawMaterialBatch")]
        FDIPostRawMaterialBatch GetFDIPostRawMaterialBatchById(string Id);

        #endregion 原材料入库_明细表 的服务接口

        #region 库存同步 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页FDIInventorySyn")]
        List <FDIInventorySyn> GetFDIInventorySynByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIInventorySyn记录总数 配合分页查询用")]
        int GetFDIInventorySynCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIInventorySyn">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIInventorySyn")]
        bool AddFDIInventorySyn(FDIInventorySyn mFDIInventorySyn);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIInventorySyn">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIInventorySyn")]
        bool UpdateFDIInventorySyn(FDIInventorySyn mFDIInventorySyn);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIInventorySyn")]
        bool DelFDIInventorySyns(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIInventorySyn")]
        bool DelFDIInventorySyn(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIInventorySyn")]
        List<FDIInventorySyn> GetFDIInventorySyns(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIInventorySyn")]
        FDIInventorySyn GetFDIInventorySynById(string Id);

        #endregion 库存同步 的服务接口

        #region 库存同步 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页FDIInventorySynBacth")]
        List <FDIInventorySynBacth> GetFDIInventorySynBacthByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIInventorySynBacth记录总数 配合分页查询用")]
        int GetFDIInventorySynBacthCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIInventorySynBacth">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIInventorySynBacth")]
        bool AddFDIInventorySynBacth(FDIInventorySynBacth mFDIInventorySynBacth);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIInventorySynBacth">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIInventorySynBacth")]
        bool UpdateFDIInventorySynBacth(FDIInventorySynBacth mFDIInventorySynBacth);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIInventorySynBacth")]
        bool DelFDIInventorySynBacths(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIInventorySynBacth")]
        bool DelFDIInventorySynBacth(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIInventorySynBacth")]
        List<FDIInventorySynBacth> GetFDIInventorySynBacths(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIInventorySynBacth")]
        FDIInventorySynBacth GetFDIInventorySynBacthById(string Id);

        #endregion 库存同步 的服务接口
    }
}
