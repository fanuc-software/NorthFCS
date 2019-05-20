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
    /// FDIPostWOIssueBatch Server
    /// </summary>
    public partial class FDIService : IFDIService
    {
        #region FDIPostWOIssueBatch分页查询

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
        public List<FDIPostWOIssueBatch> GetFDIPostWOIssueBatchByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<FDIPostWOIssueBatch, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostWOIssueBatch>(sWhere); 

            using (IFDIPostWOIssueBatchBLL FDIPostWOIssueBatchBLL = BLLContainer.Resolve<IFDIPostWOIssueBatchBLL>())
            {
                List<FDIPostWOIssueBatch> models = FDIPostWOIssueBatchBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIPostWOIssueBatch记录总数 配合分页查询用")]
        public int GetFDIPostWOIssueBatchCount(string sWhere)
        {
            Expression<Func<FDIPostWOIssueBatch, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostWOIssueBatch>(sWhere); 
            using (IFDIPostWOIssueBatchBLL FDIPostWOIssueBatchBLL = BLLContainer.Resolve<IFDIPostWOIssueBatchBLL>())
            {
                return FDIPostWOIssueBatchBLL.GetCount(whereLamda);
            }
        }

        #endregion FDIPostWOIssueBatch分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIPostWOIssueBatch">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIPostWOIssueBatch")]
        public bool AddFDIPostWOIssueBatch(FDIPostWOIssueBatch mFDIPostWOIssueBatch)
        {
            if (mFDIPostWOIssueBatch == null) return false;
            using (IFDIPostWOIssueBatchBLL FDIPostWOIssueBatchBLL = BLLContainer.Resolve<IFDIPostWOIssueBatchBLL>())
            {
                return FDIPostWOIssueBatchBLL.Add(mFDIPostWOIssueBatch);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIPostWOIssueBatch">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIPostWOIssueBatch")]
        public bool UpdateFDIPostWOIssueBatch(FDIPostWOIssueBatch mFDIPostWOIssueBatch)
        {
            if (mFDIPostWOIssueBatch == null) return false;
            using (IFDIPostWOIssueBatchBLL FDIPostWOIssueBatchBLL = BLLContainer.Resolve<IFDIPostWOIssueBatchBLL>())
            {
                return FDIPostWOIssueBatchBLL.Update(mFDIPostWOIssueBatch);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIPostWOIssueBatch")]
        public bool DelFDIPostWOIssueBatchs(string[] Ids)
        {
            using (IFDIPostWOIssueBatchBLL FDIPostWOIssueBatchBLL = BLLContainer.Resolve<IFDIPostWOIssueBatchBLL>())
            {
                try
                {
                    List<FDIPostWOIssueBatch> entitys = new List<FDIPostWOIssueBatch>();
                    foreach (string id in Ids)
                    {
                        FDIPostWOIssueBatch item = FDIPostWOIssueBatchBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return FDIPostWOIssueBatchBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIPostWOIssueBatch")]
        public bool DelFDIPostWOIssueBatch(string Id)
        {
            using (IFDIPostWOIssueBatchBLL FDIPostWOIssueBatchBLL = BLLContainer.Resolve<IFDIPostWOIssueBatchBLL>())
            {
                try
                {
                    FDIPostWOIssueBatch item = FDIPostWOIssueBatchBLL.GetFirstOrDefault(Id);
                    return FDIPostWOIssueBatchBLL.Delete(item);
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
        [WebMethod(Description = "根据查询条件获取记录集FDIPostWOIssueBatch")]
        public List<FDIPostWOIssueBatch> GetFDIPostWOIssueBatchs(string sWhere)
        {
            Expression<Func<FDIPostWOIssueBatch, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostWOIssueBatch>(sWhere);
            using (IFDIPostWOIssueBatchBLL FDIPostWOIssueBatchBLL = BLLContainer.Resolve<IFDIPostWOIssueBatchBLL>())
            {
                List<FDIPostWOIssueBatch> models = FDIPostWOIssueBatchBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIPostWOIssueBatch")]
        public FDIPostWOIssueBatch GetFDIPostWOIssueBatchById(string Id)
        {
            using (IFDIPostWOIssueBatchBLL FDIPostWOIssueBatchBLL = BLLContainer.Resolve<IFDIPostWOIssueBatchBLL>())
            {
                FDIPostWOIssueBatch model = FDIPostWOIssueBatchBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
