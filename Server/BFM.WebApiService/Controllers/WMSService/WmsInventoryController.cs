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
using BFM.ContractModel;

namespace BFM.WebApiService.Controllers 
{
    /// <summary>
    /// WmsInventory ApiController
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("WMSService/WmsInventory")]
    public class WmsInventoryController : ApiController 
    {
        #region WmsInventory分页查询

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
        public List<WmsInventory> GetWmsInventoryByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<WmsInventory, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<WmsInventory>(sWhere); 

            using (IWmsInventoryBLL WmsInventoryBLL = BLLContainer.Resolve<IWmsInventoryBLL>())
            {
                List<WmsInventory> models = WmsInventoryBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
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
        public int GetWmsInventoryCount(string sWhere)
        {
            Expression<Func<WmsInventory, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<WmsInventory>(sWhere); 
            using (IWmsInventoryBLL WmsInventoryBLL = BLLContainer.Resolve<IWmsInventoryBLL>())
            {
                return WmsInventoryBLL.GetCount(whereLamda);
            }
        }

        #endregion WmsInventory分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mWmsInventory">模型</param>
        /// <returns>是否成功</returns>
        [Route("Add")]
        [HttpPost]
        public bool AddWmsInventory(WmsInventory mWmsInventory)
        {
            if (mWmsInventory == null) return false;
            using (IWmsInventoryBLL WmsInventoryBLL = BLLContainer.Resolve<IWmsInventoryBLL>())
            {
                return WmsInventoryBLL.Add(mWmsInventory);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mWmsInventory">模型</param>
        /// <returns>是否成功</returns>
        [Route("Update")]
        [HttpPost]
        public bool UpdateWmsInventory(WmsInventory mWmsInventory)
        {
            if (mWmsInventory == null) return false;
            using (IWmsInventoryBLL WmsInventoryBLL = BLLContainer.Resolve<IWmsInventoryBLL>())
            {
                return WmsInventoryBLL.Update(mWmsInventory);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [Route("Deletes")]
        [HttpPost]
        public bool DelWmsInventorys(string[] Ids)
        {
            using (IWmsInventoryBLL WmsInventoryBLL = BLLContainer.Resolve<IWmsInventoryBLL>())
            {
                try
                {
                    List<WmsInventory> entitys = new List<WmsInventory>();
                    foreach (string id in Ids)
                    {
                        WmsInventory item = WmsInventoryBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return WmsInventoryBLL.Delete(entitys);
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
        public bool DelWmsInventory(string Id)
        {
            using (IWmsInventoryBLL WmsInventoryBLL = BLLContainer.Resolve<IWmsInventoryBLL>())
            {
                try
                {
                    WmsInventory item = WmsInventoryBLL.GetFirstOrDefault(Id);
                    return WmsInventoryBLL.Delete(item);
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
        public List<WmsInventory> GetWmsInventorys([FromBody]string sWhere)
        {
            Expression<Func<WmsInventory, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<WmsInventory>(sWhere);
            using (IWmsInventoryBLL WmsInventoryBLL = BLLContainer.Resolve<IWmsInventoryBLL>())
            {
                List<WmsInventory> models = WmsInventoryBLL.GetModels(whereLamda);
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
        public WmsInventory GetWmsInventoryById(string Id)
        {
            using (IWmsInventoryBLL WmsInventoryBLL = BLLContainer.Resolve<IWmsInventoryBLL>())
            {
                WmsInventory model = WmsInventoryBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
