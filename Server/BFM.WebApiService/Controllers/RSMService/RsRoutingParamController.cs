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
using BFM.Common.Base.Utils;
using BFM.ContractModel;

namespace BFM.WebApiService.Controllers 
{
    /// <summary>
    /// RsRoutingParam ApiController
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("RSMService/RsRoutingParam")]
    public class RsRoutingParamController : ApiController 
    {
        #region RsRoutingParam分页查询

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
        public List<RsRoutingParam> GetRsRoutingParamByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<RsRoutingParam, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsRoutingParam>(sWhere); 

            using (IRsRoutingParamBLL RsRoutingParamBLL = BLLContainer.Resolve<IRsRoutingParamBLL>())
            {
                List<RsRoutingParam> models = RsRoutingParamBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
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
        public int GetRsRoutingParamCount(string sWhere)
        {
            Expression<Func<RsRoutingParam, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsRoutingParam>(sWhere); 
            using (IRsRoutingParamBLL RsRoutingParamBLL = BLLContainer.Resolve<IRsRoutingParamBLL>())
            {
                return RsRoutingParamBLL.GetCount(whereLamda);
            }
        }

        #endregion RsRoutingParam分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mRsRoutingParam">模型</param>
        /// <returns>是否成功</returns>
        [Route("Add")]
        [HttpPost]
        public bool AddRsRoutingParam(RsRoutingParam mRsRoutingParam)
        {
            if (mRsRoutingParam == null) return false;
            using (IRsRoutingParamBLL RsRoutingParamBLL = BLLContainer.Resolve<IRsRoutingParamBLL>())
            {
                return RsRoutingParamBLL.Add(mRsRoutingParam);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mRsRoutingParam">模型</param>
        /// <returns>是否成功</returns>
        [Route("Update")]
        [HttpPost]
        public bool UpdateRsRoutingParam(RsRoutingParam mRsRoutingParam)
        {
            if (mRsRoutingParam == null) return false;
            using (IRsRoutingParamBLL RsRoutingParamBLL = BLLContainer.Resolve<IRsRoutingParamBLL>())
            {
                return RsRoutingParamBLL.Update(mRsRoutingParam);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [Route("Deletes")]
        [HttpPost]
        public bool DelRsRoutingParams(string[] Ids)
        {
            using (IRsRoutingParamBLL RsRoutingParamBLL = BLLContainer.Resolve<IRsRoutingParamBLL>())
            {
                try
                {
                    List<RsRoutingParam> entitys = new List<RsRoutingParam>();
                    foreach (string id in Ids)
                    {
                        RsRoutingParam item = RsRoutingParamBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return RsRoutingParamBLL.Delete(entitys);
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
        public bool DelRsRoutingParam(string Id)
        {
            using (IRsRoutingParamBLL RsRoutingParamBLL = BLLContainer.Resolve<IRsRoutingParamBLL>())
            {
                try
                {
                    RsRoutingParam item = RsRoutingParamBLL.GetFirstOrDefault(Id);
                    return RsRoutingParamBLL.Delete(item);
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
        public List<RsRoutingParam> GetRsRoutingParams([FromBody]string sWhere)
        {
            Expression<Func<RsRoutingParam, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsRoutingParam>(sWhere);
            using (IRsRoutingParamBLL RsRoutingParamBLL = BLLContainer.Resolve<IRsRoutingParamBLL>())
            {
                List<RsRoutingParam> models = RsRoutingParamBLL.GetModels(whereLamda);
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
        public RsRoutingParam GetRsRoutingParamById(string Id)
        {
            using (IRsRoutingParamBLL RsRoutingParamBLL = BLLContainer.Resolve<IRsRoutingParamBLL>())
            {
                RsRoutingParam model = RsRoutingParamBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
