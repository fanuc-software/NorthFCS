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
    /// FDIPostSaleOrder Server
    /// </summary>
    public partial class FDIService : IFDIService
    {
        #region FDIPostSaleOrder分页查询

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
        public List<FDIPostSaleOrder> GetFDIPostSaleOrderByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<FDIPostSaleOrder, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostSaleOrder>(sWhere); 

            using (IFDIPostSaleOrderBLL FDIPostSaleOrderBLL = BLLContainer.Resolve<IFDIPostSaleOrderBLL>())
            {
                List<FDIPostSaleOrder> models = FDIPostSaleOrderBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIPostSaleOrder记录总数 配合分页查询用")]
        public int GetFDIPostSaleOrderCount(string sWhere)
        {
            Expression<Func<FDIPostSaleOrder, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostSaleOrder>(sWhere); 
            using (IFDIPostSaleOrderBLL FDIPostSaleOrderBLL = BLLContainer.Resolve<IFDIPostSaleOrderBLL>())
            {
                return FDIPostSaleOrderBLL.GetCount(whereLamda);
            }
        }

        #endregion FDIPostSaleOrder分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIPostSaleOrder">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIPostSaleOrder")]
        public bool AddFDIPostSaleOrder(FDIPostSaleOrder mFDIPostSaleOrder)
        {
            if (mFDIPostSaleOrder == null) return false;
            using (IFDIPostSaleOrderBLL FDIPostSaleOrderBLL = BLLContainer.Resolve<IFDIPostSaleOrderBLL>())
            {
                return FDIPostSaleOrderBLL.Add(mFDIPostSaleOrder);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIPostSaleOrder">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIPostSaleOrder")]
        public bool UpdateFDIPostSaleOrder(FDIPostSaleOrder mFDIPostSaleOrder)
        {
            if (mFDIPostSaleOrder == null) return false;
            using (IFDIPostSaleOrderBLL FDIPostSaleOrderBLL = BLLContainer.Resolve<IFDIPostSaleOrderBLL>())
            {
                return FDIPostSaleOrderBLL.Update(mFDIPostSaleOrder);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIPostSaleOrder")]
        public bool DelFDIPostSaleOrders(string[] Ids)
        {
            using (IFDIPostSaleOrderBLL FDIPostSaleOrderBLL = BLLContainer.Resolve<IFDIPostSaleOrderBLL>())
            {
                try
                {
                    List<FDIPostSaleOrder> entitys = new List<FDIPostSaleOrder>();
                    foreach (string id in Ids)
                    {
                        FDIPostSaleOrder item = FDIPostSaleOrderBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return FDIPostSaleOrderBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIPostSaleOrder")]
        public bool DelFDIPostSaleOrder(string Id)
        {
            using (IFDIPostSaleOrderBLL FDIPostSaleOrderBLL = BLLContainer.Resolve<IFDIPostSaleOrderBLL>())
            {
                try
                {
                    FDIPostSaleOrder item = FDIPostSaleOrderBLL.GetFirstOrDefault(Id);
                    return FDIPostSaleOrderBLL.Delete(item);
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
        [WebMethod(Description = "根据查询条件获取记录集FDIPostSaleOrder")]
        public List<FDIPostSaleOrder> GetFDIPostSaleOrders(string sWhere)
        {
            Expression<Func<FDIPostSaleOrder, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostSaleOrder>(sWhere);
            using (IFDIPostSaleOrderBLL FDIPostSaleOrderBLL = BLLContainer.Resolve<IFDIPostSaleOrderBLL>())
            {
                List<FDIPostSaleOrder> models = FDIPostSaleOrderBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIPostSaleOrder")]
        public FDIPostSaleOrder GetFDIPostSaleOrderById(string Id)
        {
            using (IFDIPostSaleOrderBLL FDIPostSaleOrderBLL = BLLContainer.Resolve<IFDIPostSaleOrderBLL>())
            {
                FDIPostSaleOrder model = FDIPostSaleOrderBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
