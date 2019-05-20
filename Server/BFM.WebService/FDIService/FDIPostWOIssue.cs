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
    /// FDIPostWOIssue Server
    /// </summary>
    public partial class FDIService : IFDIService
    {
        #region FDIPostWOIssue分页查询

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
        public List<FDIPostWOIssue> GetFDIPostWOIssueByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<FDIPostWOIssue, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostWOIssue>(sWhere); 

            using (IFDIPostWOIssueBLL FDIPostWOIssueBLL = BLLContainer.Resolve<IFDIPostWOIssueBLL>())
            {
                List<FDIPostWOIssue> models = FDIPostWOIssueBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIPostWOIssue记录总数 配合分页查询用")]
        public int GetFDIPostWOIssueCount(string sWhere)
        {
            Expression<Func<FDIPostWOIssue, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostWOIssue>(sWhere); 
            using (IFDIPostWOIssueBLL FDIPostWOIssueBLL = BLLContainer.Resolve<IFDIPostWOIssueBLL>())
            {
                return FDIPostWOIssueBLL.GetCount(whereLamda);
            }
        }

        #endregion FDIPostWOIssue分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIPostWOIssue">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIPostWOIssue")]
        public bool AddFDIPostWOIssue(FDIPostWOIssue mFDIPostWOIssue)
        {
            if (mFDIPostWOIssue == null) return false;
            using (IFDIPostWOIssueBLL FDIPostWOIssueBLL = BLLContainer.Resolve<IFDIPostWOIssueBLL>())
            {
                return FDIPostWOIssueBLL.Add(mFDIPostWOIssue);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIPostWOIssue">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIPostWOIssue")]
        public bool UpdateFDIPostWOIssue(FDIPostWOIssue mFDIPostWOIssue)
        {
            if (mFDIPostWOIssue == null) return false;
            using (IFDIPostWOIssueBLL FDIPostWOIssueBLL = BLLContainer.Resolve<IFDIPostWOIssueBLL>())
            {
                return FDIPostWOIssueBLL.Update(mFDIPostWOIssue);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIPostWOIssue")]
        public bool DelFDIPostWOIssues(string[] Ids)
        {
            using (IFDIPostWOIssueBLL FDIPostWOIssueBLL = BLLContainer.Resolve<IFDIPostWOIssueBLL>())
            {
                try
                {
                    List<FDIPostWOIssue> entitys = new List<FDIPostWOIssue>();
                    foreach (string id in Ids)
                    {
                        FDIPostWOIssue item = FDIPostWOIssueBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return FDIPostWOIssueBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIPostWOIssue")]
        public bool DelFDIPostWOIssue(string Id)
        {
            using (IFDIPostWOIssueBLL FDIPostWOIssueBLL = BLLContainer.Resolve<IFDIPostWOIssueBLL>())
            {
                try
                {
                    FDIPostWOIssue item = FDIPostWOIssueBLL.GetFirstOrDefault(Id);
                    return FDIPostWOIssueBLL.Delete(item);
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
        [WebMethod(Description = "根据查询条件获取记录集FDIPostWOIssue")]
        public List<FDIPostWOIssue> GetFDIPostWOIssues(string sWhere)
        {
            Expression<Func<FDIPostWOIssue, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostWOIssue>(sWhere);
            using (IFDIPostWOIssueBLL FDIPostWOIssueBLL = BLLContainer.Resolve<IFDIPostWOIssueBLL>())
            {
                List<FDIPostWOIssue> models = FDIPostWOIssueBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIPostWOIssue")]
        public FDIPostWOIssue GetFDIPostWOIssueById(string Id)
        {
            using (IFDIPostWOIssueBLL FDIPostWOIssueBLL = BLLContainer.Resolve<IFDIPostWOIssueBLL>())
            {
                FDIPostWOIssue model = FDIPostWOIssueBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
