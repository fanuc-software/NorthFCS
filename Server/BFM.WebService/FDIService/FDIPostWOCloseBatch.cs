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
    /// FDIPostWOCloseBatch Server
    /// </summary>
    public partial class FDIService : IFDIService
    {
        #region FDIPostWOCloseBatch分页查询

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
        public List<FDIPostWOCloseBatch> GetFDIPostWOCloseBatchByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<FDIPostWOCloseBatch, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostWOCloseBatch>(sWhere); 

            using (IFDIPostWOCloseBatchBLL FDIPostWOCloseBatchBLL = BLLContainer.Resolve<IFDIPostWOCloseBatchBLL>())
            {
                List<FDIPostWOCloseBatch> models = FDIPostWOCloseBatchBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIPostWOCloseBatch记录总数 配合分页查询用")]
        public int GetFDIPostWOCloseBatchCount(string sWhere)
        {
            Expression<Func<FDIPostWOCloseBatch, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostWOCloseBatch>(sWhere); 
            using (IFDIPostWOCloseBatchBLL FDIPostWOCloseBatchBLL = BLLContainer.Resolve<IFDIPostWOCloseBatchBLL>())
            {
                return FDIPostWOCloseBatchBLL.GetCount(whereLamda);
            }
        }

        #endregion FDIPostWOCloseBatch分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIPostWOCloseBatch">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIPostWOCloseBatch")]
        public bool AddFDIPostWOCloseBatch(FDIPostWOCloseBatch mFDIPostWOCloseBatch)
        {
            if (mFDIPostWOCloseBatch == null) return false;
            using (IFDIPostWOCloseBatchBLL FDIPostWOCloseBatchBLL = BLLContainer.Resolve<IFDIPostWOCloseBatchBLL>())
            {
                return FDIPostWOCloseBatchBLL.Add(mFDIPostWOCloseBatch);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIPostWOCloseBatch">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIPostWOCloseBatch")]
        public bool UpdateFDIPostWOCloseBatch(FDIPostWOCloseBatch mFDIPostWOCloseBatch)
        {
            if (mFDIPostWOCloseBatch == null) return false;
            using (IFDIPostWOCloseBatchBLL FDIPostWOCloseBatchBLL = BLLContainer.Resolve<IFDIPostWOCloseBatchBLL>())
            {
                return FDIPostWOCloseBatchBLL.Update(mFDIPostWOCloseBatch);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIPostWOCloseBatch")]
        public bool DelFDIPostWOCloseBatchs(string[] Ids)
        {
            using (IFDIPostWOCloseBatchBLL FDIPostWOCloseBatchBLL = BLLContainer.Resolve<IFDIPostWOCloseBatchBLL>())
            {
                try
                {
                    List<FDIPostWOCloseBatch> entitys = new List<FDIPostWOCloseBatch>();
                    foreach (string id in Ids)
                    {
                        FDIPostWOCloseBatch item = FDIPostWOCloseBatchBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return FDIPostWOCloseBatchBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIPostWOCloseBatch")]
        public bool DelFDIPostWOCloseBatch(string Id)
        {
            using (IFDIPostWOCloseBatchBLL FDIPostWOCloseBatchBLL = BLLContainer.Resolve<IFDIPostWOCloseBatchBLL>())
            {
                try
                {
                    FDIPostWOCloseBatch item = FDIPostWOCloseBatchBLL.GetFirstOrDefault(Id);
                    return FDIPostWOCloseBatchBLL.Delete(item);
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
        [WebMethod(Description = "根据查询条件获取记录集FDIPostWOCloseBatch")]
        public List<FDIPostWOCloseBatch> GetFDIPostWOCloseBatchs(string sWhere)
        {
            Expression<Func<FDIPostWOCloseBatch, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostWOCloseBatch>(sWhere);
            using (IFDIPostWOCloseBatchBLL FDIPostWOCloseBatchBLL = BLLContainer.Resolve<IFDIPostWOCloseBatchBLL>())
            {
                List<FDIPostWOCloseBatch> models = FDIPostWOCloseBatchBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIPostWOCloseBatch")]
        public FDIPostWOCloseBatch GetFDIPostWOCloseBatchById(string Id)
        {
            using (IFDIPostWOCloseBatchBLL FDIPostWOCloseBatchBLL = BLLContainer.Resolve<IFDIPostWOCloseBatchBLL>())
            {
                FDIPostWOCloseBatch model = FDIPostWOCloseBatchBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
