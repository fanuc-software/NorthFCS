/*******************************************************************************
 * Copyright © 2018 代码生成器 版权所有
 * Author: LanGerp 
 * Description: 快速开发平台
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Http;
using BFM.BLL.Container;
using BFM.BLL.IBLL;
using BFM.Common.Base.Helper;
using BFM.Common.Base.Utils;
using BFM.ContractModel;

namespace BFM.WebApiService.Controllers 
{
    /// <summary>
    /// RsMaintainStandardsRelate ApiController
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("EAMService/RsMaintainStandardsRelate")]
    public class RsMaintainStandardsRelateController : ApiController 
    {
        #region RsMaintainStandardsRelate分页查询

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [Route("GetPageData")]
        [HttpGet]
        public List<RsMaintainStandardsRelate> GetRsMaintainStandardsRelateByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<RsMaintainStandardsRelate, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsMaintainStandardsRelate>(sWhere); 

            using (IRsMaintainStandardsRelateBLL RsMaintainStandardsRelateBLL = BLLContainer.Resolve<IRsMaintainStandardsRelateBLL>())
            {
                List<RsMaintainStandardsRelate> models = RsMaintainStandardsRelateBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [Route("GetCount")]
        [HttpGet]
        public int GetRsMaintainStandardsRelateCount(string sWhere)
        {
            Expression<Func<RsMaintainStandardsRelate, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsMaintainStandardsRelate>(sWhere); 
            using (IRsMaintainStandardsRelateBLL RsMaintainStandardsRelateBLL = BLLContainer.Resolve<IRsMaintainStandardsRelateBLL>())
            {
                return RsMaintainStandardsRelateBLL.GetCount(whereLamda);
            }
        }

        #endregion RsMaintainStandardsRelate分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mRsMaintainStandardsRelate">模型</param>
        /// <returns>是否成功</returns>
        [Route("Add")]
        [HttpPost]
        public bool AddRsMaintainStandardsRelate(RsMaintainStandardsRelate mRsMaintainStandardsRelate)
        {
            if (mRsMaintainStandardsRelate == null) return false;
            using (IRsMaintainStandardsRelateBLL RsMaintainStandardsRelateBLL = BLLContainer.Resolve<IRsMaintainStandardsRelateBLL>())
            {
                return RsMaintainStandardsRelateBLL.Add(mRsMaintainStandardsRelate);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mRsMaintainStandardsRelate">模型</param>
        /// <returns>是否成功</returns>
        [Route("Update")]
        [HttpPost]
        public bool UpdateRsMaintainStandardsRelate(RsMaintainStandardsRelate mRsMaintainStandardsRelate)
        {
            if (mRsMaintainStandardsRelate == null) return false;
            using (IRsMaintainStandardsRelateBLL RsMaintainStandardsRelateBLL = BLLContainer.Resolve<IRsMaintainStandardsRelateBLL>())
            {
                return RsMaintainStandardsRelateBLL.Update(mRsMaintainStandardsRelate);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [Route("Deletes")]
        [HttpPost]
        public bool DelRsMaintainStandardsRelates(string[] Ids)
        {
            using (IRsMaintainStandardsRelateBLL RsMaintainStandardsRelateBLL = BLLContainer.Resolve<IRsMaintainStandardsRelateBLL>())
            {
                try
                {
                    List<RsMaintainStandardsRelate> entitys = new List<RsMaintainStandardsRelate>();
                    foreach (string id in Ids)
                    {
                        RsMaintainStandardsRelate item = RsMaintainStandardsRelateBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return RsMaintainStandardsRelateBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [Route("Delete")]
        [HttpGet]
        public bool DelRsMaintainStandardsRelate(string Id)
        {
            using (IRsMaintainStandardsRelateBLL RsMaintainStandardsRelateBLL = BLLContainer.Resolve<IRsMaintainStandardsRelateBLL>())
            {
                try
                {
                    RsMaintainStandardsRelate item = RsMaintainStandardsRelateBLL.GetFirstOrDefault(Id);
                    return RsMaintainStandardsRelateBLL.Delete(item);
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
        [Route("GetByParam")]
        [HttpPost]
        public List<RsMaintainStandardsRelate> GetRsMaintainStandardsRelates([FromBody]string sWhere)
        {
            Expression<Func<RsMaintainStandardsRelate, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsMaintainStandardsRelate>(sWhere);
            using (IRsMaintainStandardsRelateBLL RsMaintainStandardsRelateBLL = BLLContainer.Resolve<IRsMaintainStandardsRelateBLL>())
            {
                List<RsMaintainStandardsRelate> models = RsMaintainStandardsRelateBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [Route("GetById")]
        [HttpGet]
        public RsMaintainStandardsRelate GetRsMaintainStandardsRelateById(string Id)
        {
            using (IRsMaintainStandardsRelateBLL RsMaintainStandardsRelateBLL = BLLContainer.Resolve<IRsMaintainStandardsRelateBLL>())
            {
                RsMaintainStandardsRelate model = RsMaintainStandardsRelateBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
