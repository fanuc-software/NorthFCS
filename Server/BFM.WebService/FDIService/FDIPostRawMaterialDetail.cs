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
    /// FDIPostRawMaterialDetail Server
    /// </summary>
    public partial class FDIService : IFDIService
    {
        #region FDIPostRawMaterialDetail分页查询

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
        public List<FDIPostRawMaterialDetail> GetFDIPostRawMaterialDetailByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<FDIPostRawMaterialDetail, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostRawMaterialDetail>(sWhere); 

            using (IFDIPostRawMaterialDetailBLL FDIPostRawMaterialDetailBLL = BLLContainer.Resolve<IFDIPostRawMaterialDetailBLL>())
            {
                List<FDIPostRawMaterialDetail> models = FDIPostRawMaterialDetailBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIPostRawMaterialDetail记录总数 配合分页查询用")]
        public int GetFDIPostRawMaterialDetailCount(string sWhere)
        {
            Expression<Func<FDIPostRawMaterialDetail, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostRawMaterialDetail>(sWhere); 
            using (IFDIPostRawMaterialDetailBLL FDIPostRawMaterialDetailBLL = BLLContainer.Resolve<IFDIPostRawMaterialDetailBLL>())
            {
                return FDIPostRawMaterialDetailBLL.GetCount(whereLamda);
            }
        }

        #endregion FDIPostRawMaterialDetail分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIPostRawMaterialDetail">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIPostRawMaterialDetail")]
        public bool AddFDIPostRawMaterialDetail(FDIPostRawMaterialDetail mFDIPostRawMaterialDetail)
        {
            if (mFDIPostRawMaterialDetail == null) return false;
            using (IFDIPostRawMaterialDetailBLL FDIPostRawMaterialDetailBLL = BLLContainer.Resolve<IFDIPostRawMaterialDetailBLL>())
            {
                return FDIPostRawMaterialDetailBLL.Add(mFDIPostRawMaterialDetail);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIPostRawMaterialDetail">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIPostRawMaterialDetail")]
        public bool UpdateFDIPostRawMaterialDetail(FDIPostRawMaterialDetail mFDIPostRawMaterialDetail)
        {
            if (mFDIPostRawMaterialDetail == null) return false;
            using (IFDIPostRawMaterialDetailBLL FDIPostRawMaterialDetailBLL = BLLContainer.Resolve<IFDIPostRawMaterialDetailBLL>())
            {
                return FDIPostRawMaterialDetailBLL.Update(mFDIPostRawMaterialDetail);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIPostRawMaterialDetail")]
        public bool DelFDIPostRawMaterialDetails(string[] Ids)
        {
            using (IFDIPostRawMaterialDetailBLL FDIPostRawMaterialDetailBLL = BLLContainer.Resolve<IFDIPostRawMaterialDetailBLL>())
            {
                try
                {
                    List<FDIPostRawMaterialDetail> entitys = new List<FDIPostRawMaterialDetail>();
                    foreach (string id in Ids)
                    {
                        FDIPostRawMaterialDetail item = FDIPostRawMaterialDetailBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return FDIPostRawMaterialDetailBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIPostRawMaterialDetail")]
        public bool DelFDIPostRawMaterialDetail(string Id)
        {
            using (IFDIPostRawMaterialDetailBLL FDIPostRawMaterialDetailBLL = BLLContainer.Resolve<IFDIPostRawMaterialDetailBLL>())
            {
                try
                {
                    FDIPostRawMaterialDetail item = FDIPostRawMaterialDetailBLL.GetFirstOrDefault(Id);
                    return FDIPostRawMaterialDetailBLL.Delete(item);
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
        [WebMethod(Description = "根据查询条件获取记录集FDIPostRawMaterialDetail")]
        public List<FDIPostRawMaterialDetail> GetFDIPostRawMaterialDetails(string sWhere)
        {
            Expression<Func<FDIPostRawMaterialDetail, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostRawMaterialDetail>(sWhere);
            using (IFDIPostRawMaterialDetailBLL FDIPostRawMaterialDetailBLL = BLLContainer.Resolve<IFDIPostRawMaterialDetailBLL>())
            {
                List<FDIPostRawMaterialDetail> models = FDIPostRawMaterialDetailBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIPostRawMaterialDetail")]
        public FDIPostRawMaterialDetail GetFDIPostRawMaterialDetailById(string Id)
        {
            using (IFDIPostRawMaterialDetailBLL FDIPostRawMaterialDetailBLL = BLLContainer.Resolve<IFDIPostRawMaterialDetailBLL>())
            {
                FDIPostRawMaterialDetail model = FDIPostRawMaterialDetailBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
