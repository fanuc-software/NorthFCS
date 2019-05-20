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
    /// FDIGetSaleOrder Server
    /// </summary>
    public partial class FDIService : IFDIService
    {
        #region FDIGetSaleOrder分页查询

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
        public List<FDIGetSaleOrder> GetFDIGetSaleOrderByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<FDIGetSaleOrder, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIGetSaleOrder>(sWhere); 

            using (IFDIGetSaleOrderBLL FDIGetSaleOrderBLL = BLLContainer.Resolve<IFDIGetSaleOrderBLL>())
            {
                List<FDIGetSaleOrder> models = FDIGetSaleOrderBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIGetSaleOrder记录总数 配合分页查询用")]
        public int GetFDIGetSaleOrderCount(string sWhere)
        {
            Expression<Func<FDIGetSaleOrder, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIGetSaleOrder>(sWhere); 
            using (IFDIGetSaleOrderBLL FDIGetSaleOrderBLL = BLLContainer.Resolve<IFDIGetSaleOrderBLL>())
            {
                return FDIGetSaleOrderBLL.GetCount(whereLamda);
            }
        }

        #endregion FDIGetSaleOrder分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIGetSaleOrder">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIGetSaleOrder")]
        public bool AddFDIGetSaleOrder(FDIGetSaleOrder mFDIGetSaleOrder)
        {
            if (mFDIGetSaleOrder == null) return false;
            using (IFDIGetSaleOrderBLL FDIGetSaleOrderBLL = BLLContainer.Resolve<IFDIGetSaleOrderBLL>())
            {
                return FDIGetSaleOrderBLL.Add(mFDIGetSaleOrder);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIGetSaleOrder">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIGetSaleOrder")]
        public bool UpdateFDIGetSaleOrder(FDIGetSaleOrder mFDIGetSaleOrder)
        {
            if (mFDIGetSaleOrder == null) return false;
            using (IFDIGetSaleOrderBLL FDIGetSaleOrderBLL = BLLContainer.Resolve<IFDIGetSaleOrderBLL>())
            {
                return FDIGetSaleOrderBLL.Update(mFDIGetSaleOrder);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIGetSaleOrder")]
        public bool DelFDIGetSaleOrders(string[] Ids)
        {
            using (IFDIGetSaleOrderBLL FDIGetSaleOrderBLL = BLLContainer.Resolve<IFDIGetSaleOrderBLL>())
            {
                try
                {
                    List<FDIGetSaleOrder> entitys = new List<FDIGetSaleOrder>();
                    foreach (string id in Ids)
                    {
                        FDIGetSaleOrder item = FDIGetSaleOrderBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return FDIGetSaleOrderBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIGetSaleOrder")]
        public bool DelFDIGetSaleOrder(string Id)
        {
            using (IFDIGetSaleOrderBLL FDIGetSaleOrderBLL = BLLContainer.Resolve<IFDIGetSaleOrderBLL>())
            {
                try
                {
                    FDIGetSaleOrder item = FDIGetSaleOrderBLL.GetFirstOrDefault(Id);
                    return FDIGetSaleOrderBLL.Delete(item);
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
        [WebMethod(Description = "根据查询条件获取记录集FDIGetSaleOrder")]
        public List<FDIGetSaleOrder> GetFDIGetSaleOrders(string sWhere)
        {
            Expression<Func<FDIGetSaleOrder, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIGetSaleOrder>(sWhere);
            using (IFDIGetSaleOrderBLL FDIGetSaleOrderBLL = BLLContainer.Resolve<IFDIGetSaleOrderBLL>())
            {
                List<FDIGetSaleOrder> models = FDIGetSaleOrderBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIGetSaleOrder")]
        public FDIGetSaleOrder GetFDIGetSaleOrderById(string Id)
        {
            using (IFDIGetSaleOrderBLL FDIGetSaleOrderBLL = BLLContainer.Resolve<IFDIGetSaleOrderBLL>())
            {
                FDIGetSaleOrder model = FDIGetSaleOrderBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
