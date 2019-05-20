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
    /// FDIPostWOCloseDetail Server
    /// </summary>
    public partial class FDIService : IFDIService
    {
        #region FDIPostWOCloseDetail分页查询

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
        public List<FDIPostWOCloseDetail> GetFDIPostWOCloseDetailByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<FDIPostWOCloseDetail, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostWOCloseDetail>(sWhere); 

            using (IFDIPostWOCloseDetailBLL FDIPostWOCloseDetailBLL = BLLContainer.Resolve<IFDIPostWOCloseDetailBLL>())
            {
                List<FDIPostWOCloseDetail> models = FDIPostWOCloseDetailBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIPostWOCloseDetail记录总数 配合分页查询用")]
        public int GetFDIPostWOCloseDetailCount(string sWhere)
        {
            Expression<Func<FDIPostWOCloseDetail, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostWOCloseDetail>(sWhere); 
            using (IFDIPostWOCloseDetailBLL FDIPostWOCloseDetailBLL = BLLContainer.Resolve<IFDIPostWOCloseDetailBLL>())
            {
                return FDIPostWOCloseDetailBLL.GetCount(whereLamda);
            }
        }

        #endregion FDIPostWOCloseDetail分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIPostWOCloseDetail">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIPostWOCloseDetail")]
        public bool AddFDIPostWOCloseDetail(FDIPostWOCloseDetail mFDIPostWOCloseDetail)
        {
            if (mFDIPostWOCloseDetail == null) return false;
            using (IFDIPostWOCloseDetailBLL FDIPostWOCloseDetailBLL = BLLContainer.Resolve<IFDIPostWOCloseDetailBLL>())
            {
                return FDIPostWOCloseDetailBLL.Add(mFDIPostWOCloseDetail);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIPostWOCloseDetail">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIPostWOCloseDetail")]
        public bool UpdateFDIPostWOCloseDetail(FDIPostWOCloseDetail mFDIPostWOCloseDetail)
        {
            if (mFDIPostWOCloseDetail == null) return false;
            using (IFDIPostWOCloseDetailBLL FDIPostWOCloseDetailBLL = BLLContainer.Resolve<IFDIPostWOCloseDetailBLL>())
            {
                return FDIPostWOCloseDetailBLL.Update(mFDIPostWOCloseDetail);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIPostWOCloseDetail")]
        public bool DelFDIPostWOCloseDetails(string[] Ids)
        {
            using (IFDIPostWOCloseDetailBLL FDIPostWOCloseDetailBLL = BLLContainer.Resolve<IFDIPostWOCloseDetailBLL>())
            {
                try
                {
                    List<FDIPostWOCloseDetail> entitys = new List<FDIPostWOCloseDetail>();
                    foreach (string id in Ids)
                    {
                        FDIPostWOCloseDetail item = FDIPostWOCloseDetailBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return FDIPostWOCloseDetailBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIPostWOCloseDetail")]
        public bool DelFDIPostWOCloseDetail(string Id)
        {
            using (IFDIPostWOCloseDetailBLL FDIPostWOCloseDetailBLL = BLLContainer.Resolve<IFDIPostWOCloseDetailBLL>())
            {
                try
                {
                    FDIPostWOCloseDetail item = FDIPostWOCloseDetailBLL.GetFirstOrDefault(Id);
                    return FDIPostWOCloseDetailBLL.Delete(item);
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
        [WebMethod(Description = "根据查询条件获取记录集FDIPostWOCloseDetail")]
        public List<FDIPostWOCloseDetail> GetFDIPostWOCloseDetails(string sWhere)
        {
            Expression<Func<FDIPostWOCloseDetail, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostWOCloseDetail>(sWhere);
            using (IFDIPostWOCloseDetailBLL FDIPostWOCloseDetailBLL = BLLContainer.Resolve<IFDIPostWOCloseDetailBLL>())
            {
                List<FDIPostWOCloseDetail> models = FDIPostWOCloseDetailBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIPostWOCloseDetail")]
        public FDIPostWOCloseDetail GetFDIPostWOCloseDetailById(string Id)
        {
            using (IFDIPostWOCloseDetailBLL FDIPostWOCloseDetailBLL = BLLContainer.Resolve<IFDIPostWOCloseDetailBLL>())
            {
                FDIPostWOCloseDetail model = FDIPostWOCloseDetailBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
