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
    /// RsRoutingItem ApiController
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("RSMService/RsRoutingItem")]
    public class RsRoutingItemController : ApiController 
    {
        #region RsRoutingItem分页查询

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
        public List<RsRoutingItem> GetRsRoutingItemByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<RsRoutingItem, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsRoutingItem>(sWhere); 

            using (IRsRoutingItemBLL RsRoutingItemBLL = BLLContainer.Resolve<IRsRoutingItemBLL>())
            {
                List<RsRoutingItem> models = RsRoutingItemBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
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
        public int GetRsRoutingItemCount(string sWhere)
        {
            Expression<Func<RsRoutingItem, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsRoutingItem>(sWhere); 
            using (IRsRoutingItemBLL RsRoutingItemBLL = BLLContainer.Resolve<IRsRoutingItemBLL>())
            {
                return RsRoutingItemBLL.GetCount(whereLamda);
            }
        }

        #endregion RsRoutingItem分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mRsRoutingItem">模型</param>
        /// <returns>是否成功</returns>
        [Route("Add")]
        [HttpPost]
        public bool AddRsRoutingItem(RsRoutingItem mRsRoutingItem)
        {
            if (mRsRoutingItem == null) return false;
            using (IRsRoutingItemBLL RsRoutingItemBLL = BLLContainer.Resolve<IRsRoutingItemBLL>())
            {
                return RsRoutingItemBLL.Add(mRsRoutingItem);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mRsRoutingItem">模型</param>
        /// <returns>是否成功</returns>
        [Route("Update")]
        [HttpPost]
        public bool UpdateRsRoutingItem(RsRoutingItem mRsRoutingItem)
        {
            if (mRsRoutingItem == null) return false;
            using (IRsRoutingItemBLL RsRoutingItemBLL = BLLContainer.Resolve<IRsRoutingItemBLL>())
            {
                return RsRoutingItemBLL.Update(mRsRoutingItem);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [Route("Deletes")]
        [HttpPost]
        public bool DelRsRoutingItems(string[] Ids)
        {
            using (IRsRoutingItemBLL RsRoutingItemBLL = BLLContainer.Resolve<IRsRoutingItemBLL>())
            {
                try
                {
                    List<RsRoutingItem> entitys = new List<RsRoutingItem>();
                    foreach (string id in Ids)
                    {
                        RsRoutingItem item = RsRoutingItemBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return RsRoutingItemBLL.Delete(entitys);
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
        public bool DelRsRoutingItem(string Id)
        {
            using (IRsRoutingItemBLL RsRoutingItemBLL = BLLContainer.Resolve<IRsRoutingItemBLL>())
            {
                try
                {
                    RsRoutingItem item = RsRoutingItemBLL.GetFirstOrDefault(Id);
                    return RsRoutingItemBLL.Delete(item);
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
        public List<RsRoutingItem> GetRsRoutingItems([FromBody]string sWhere)
        {
            Expression<Func<RsRoutingItem, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsRoutingItem>(sWhere);
            using (IRsRoutingItemBLL RsRoutingItemBLL = BLLContainer.Resolve<IRsRoutingItemBLL>())
            {
                List<RsRoutingItem> models = RsRoutingItemBLL.GetModels(whereLamda);
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
        public RsRoutingItem GetRsRoutingItemById(string Id)
        {
            using (IRsRoutingItemBLL RsRoutingItemBLL = BLLContainer.Resolve<IRsRoutingItemBLL>())
            {
                RsRoutingItem model = RsRoutingItemBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
