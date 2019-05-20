/*******************************************************************************
 * Copyright © 2018 代码生成器 版权所有
 * Author: LanGerp 
 * Description: 快速开发平台
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Services;
using BFM.BLL.Container;
using BFM.BLL.IBLL;
using BFM.Common.Base.Helper;
using BFM.ContractModel;

namespace BFM.WebService
{
    /// <summary>
    /// FDIPostSaleOrderDetail Server
    /// </summary>
    public partial class FDIService : IFDIService
    {
        #region FDIPostSaleOrderDetail分页查询

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
        public List<FDIPostSaleOrderDetail> GetFDIPostSaleOrderDetailByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
        {
            string orderStr = "";
            if (string.IsNullOrEmpty(orderField))
            {
                orderStr = "CREATION_DATE";
            }
            else
            {
                orderStr = orderField;
            }
            Expression<Func<FDIPostSaleOrderDetail, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostSaleOrderDetail>(sWhere); 

            using (IFDIPostSaleOrderDetailBLL FDIPostSaleOrderDetailBLL = BLLContainer.Resolve<IFDIPostSaleOrderDetailBLL>())
            {
                List<FDIPostSaleOrderDetail> models = FDIPostSaleOrderDetailBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIPostSaleOrderDetail记录总数 配合分页查询用")]
        public int GetFDIPostSaleOrderDetailCount(string sWhere)
        {
            Expression<Func<FDIPostSaleOrderDetail, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostSaleOrderDetail>(sWhere); 
            using (IFDIPostSaleOrderDetailBLL FDIPostSaleOrderDetailBLL = BLLContainer.Resolve<IFDIPostSaleOrderDetailBLL>())
            {
                return FDIPostSaleOrderDetailBLL.GetCount(whereLamda);
            }
        }

        #endregion FDIPostSaleOrderDetail分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIPostSaleOrderDetail">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIPostSaleOrderDetail")]
        public bool AddFDIPostSaleOrderDetail(FDIPostSaleOrderDetail mFDIPostSaleOrderDetail)
        {
            if (mFDIPostSaleOrderDetail == null) return false;
            using (IFDIPostSaleOrderDetailBLL FDIPostSaleOrderDetailBLL = BLLContainer.Resolve<IFDIPostSaleOrderDetailBLL>())
            {
                return FDIPostSaleOrderDetailBLL.Add(mFDIPostSaleOrderDetail);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIPostSaleOrderDetail">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIPostSaleOrderDetail")]
        public bool UpdateFDIPostSaleOrderDetail(FDIPostSaleOrderDetail mFDIPostSaleOrderDetail)
        {
            if (mFDIPostSaleOrderDetail == null) return false;
            using (IFDIPostSaleOrderDetailBLL FDIPostSaleOrderDetailBLL = BLLContainer.Resolve<IFDIPostSaleOrderDetailBLL>())
            {
                return FDIPostSaleOrderDetailBLL.Update(mFDIPostSaleOrderDetail);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIPostSaleOrderDetail")]
        public bool DelFDIPostSaleOrderDetails(string[] Ids)
        {
            using (IFDIPostSaleOrderDetailBLL FDIPostSaleOrderDetailBLL = BLLContainer.Resolve<IFDIPostSaleOrderDetailBLL>())
            {
                try
                {
                    List<FDIPostSaleOrderDetail> entitys = new List<FDIPostSaleOrderDetail>();
                    foreach (string id in Ids)
                    {
                        FDIPostSaleOrderDetail item = FDIPostSaleOrderDetailBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return FDIPostSaleOrderDetailBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIPostSaleOrderDetail")]
        public bool DelFDIPostSaleOrderDetail(string Id)
        {
            using (IFDIPostSaleOrderDetailBLL FDIPostSaleOrderDetailBLL = BLLContainer.Resolve<IFDIPostSaleOrderDetailBLL>())
            {
                try
                {
                    FDIPostSaleOrderDetail item = FDIPostSaleOrderDetailBLL.GetFirstOrDefault(Id);
                    return FDIPostSaleOrderDetailBLL.Delete(item);
                }
                catch { return false; }
            }
        }

        #endregion Add、Update、Delete

        #region Search

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [WebMethod(Description = "根据查询条件获取记录集FDIPostSaleOrderDetail")]
        public List<FDIPostSaleOrderDetail> GetFDIPostSaleOrderDetails(string sWhere)
        {
            Expression<Func<FDIPostSaleOrderDetail, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostSaleOrderDetail>(sWhere);
            using (IFDIPostSaleOrderDetailBLL FDIPostSaleOrderDetailBLL = BLLContainer.Resolve<IFDIPostSaleOrderDetailBLL>())
            {
                List<FDIPostSaleOrderDetail> models = FDIPostSaleOrderDetailBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIPostSaleOrderDetail")]
        public FDIPostSaleOrderDetail GetFDIPostSaleOrderDetailById(string Id)
        {
            using (IFDIPostSaleOrderDetailBLL FDIPostSaleOrderDetailBLL = BLLContainer.Resolve<IFDIPostSaleOrderDetailBLL>())
            {
                FDIPostSaleOrderDetail model = FDIPostSaleOrderDetailBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
