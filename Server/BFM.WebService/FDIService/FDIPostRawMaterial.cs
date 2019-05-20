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
    /// FDIPostRawMaterial Server
    /// </summary>
    public partial class FDIService : IFDIService
    {
        #region FDIPostRawMaterial分页查询

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
        public List<FDIPostRawMaterial> GetFDIPostRawMaterialByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<FDIPostRawMaterial, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostRawMaterial>(sWhere); 

            using (IFDIPostRawMaterialBLL FDIPostRawMaterialBLL = BLLContainer.Resolve<IFDIPostRawMaterialBLL>())
            {
                List<FDIPostRawMaterial> models = FDIPostRawMaterialBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回FDIPostRawMaterial记录总数 配合分页查询用")]
        public int GetFDIPostRawMaterialCount(string sWhere)
        {
            Expression<Func<FDIPostRawMaterial, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostRawMaterial>(sWhere); 
            using (IFDIPostRawMaterialBLL FDIPostRawMaterialBLL = BLLContainer.Resolve<IFDIPostRawMaterialBLL>())
            {
                return FDIPostRawMaterialBLL.GetCount(whereLamda);
            }
        }

        #endregion FDIPostRawMaterial分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFDIPostRawMaterial">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增FDIPostRawMaterial")]
        public bool AddFDIPostRawMaterial(FDIPostRawMaterial mFDIPostRawMaterial)
        {
            if (mFDIPostRawMaterial == null) return false;
            using (IFDIPostRawMaterialBLL FDIPostRawMaterialBLL = BLLContainer.Resolve<IFDIPostRawMaterialBLL>())
            {
                return FDIPostRawMaterialBLL.Add(mFDIPostRawMaterial);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFDIPostRawMaterial">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改FDIPostRawMaterial")]
        public bool UpdateFDIPostRawMaterial(FDIPostRawMaterial mFDIPostRawMaterial)
        {
            if (mFDIPostRawMaterial == null) return false;
            using (IFDIPostRawMaterialBLL FDIPostRawMaterialBLL = BLLContainer.Resolve<IFDIPostRawMaterialBLL>())
            {
                return FDIPostRawMaterialBLL.Update(mFDIPostRawMaterial);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）FDIPostRawMaterial")]
        public bool DelFDIPostRawMaterials(string[] Ids)
        {
            using (IFDIPostRawMaterialBLL FDIPostRawMaterialBLL = BLLContainer.Resolve<IFDIPostRawMaterialBLL>())
            {
                try
                {
                    List<FDIPostRawMaterial> entitys = new List<FDIPostRawMaterial>();
                    foreach (string id in Ids)
                    {
                        FDIPostRawMaterial item = FDIPostRawMaterialBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return FDIPostRawMaterialBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除FDIPostRawMaterial")]
        public bool DelFDIPostRawMaterial(string Id)
        {
            using (IFDIPostRawMaterialBLL FDIPostRawMaterialBLL = BLLContainer.Resolve<IFDIPostRawMaterialBLL>())
            {
                try
                {
                    FDIPostRawMaterial item = FDIPostRawMaterialBLL.GetFirstOrDefault(Id);
                    return FDIPostRawMaterialBLL.Delete(item);
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
        [WebMethod(Description = "根据查询条件获取记录集FDIPostRawMaterial")]
        public List<FDIPostRawMaterial> GetFDIPostRawMaterials(string sWhere)
        {
            Expression<Func<FDIPostRawMaterial, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<FDIPostRawMaterial>(sWhere);
            using (IFDIPostRawMaterialBLL FDIPostRawMaterialBLL = BLLContainer.Resolve<IFDIPostRawMaterialBLL>())
            {
                List<FDIPostRawMaterial> models = FDIPostRawMaterialBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找FDIPostRawMaterial")]
        public FDIPostRawMaterial GetFDIPostRawMaterialById(string Id)
        {
            using (IFDIPostRawMaterialBLL FDIPostRawMaterialBLL = BLLContainer.Resolve<IFDIPostRawMaterialBLL>())
            {
                FDIPostRawMaterial model = FDIPostRawMaterialBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
