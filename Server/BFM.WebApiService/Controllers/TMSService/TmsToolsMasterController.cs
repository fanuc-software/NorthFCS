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
    /// TmsToolsMaster ApiController
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("TMSService/TmsToolsMaster")]
    public class TmsToolsMasterController : ApiController 
    {
        #region TmsToolsMaster分页查询

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
        public List<TmsToolsMaster> GetTmsToolsMasterByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<TmsToolsMaster, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<TmsToolsMaster>(sWhere); 

            using (ITmsToolsMasterBLL TmsToolsMasterBLL = BLLContainer.Resolve<ITmsToolsMasterBLL>())
            {
                List<TmsToolsMaster> models = TmsToolsMasterBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
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
        public int GetTmsToolsMasterCount(string sWhere)
        {
            Expression<Func<TmsToolsMaster, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<TmsToolsMaster>(sWhere); 
            using (ITmsToolsMasterBLL TmsToolsMasterBLL = BLLContainer.Resolve<ITmsToolsMasterBLL>())
            {
                return TmsToolsMasterBLL.GetCount(whereLamda);
            }
        }

        #endregion TmsToolsMaster分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mTmsToolsMaster">模型</param>
        /// <returns>是否成功</returns>
        [Route("Add")]
        [HttpPost]
        public bool AddTmsToolsMaster(TmsToolsMaster mTmsToolsMaster)
        {
            if (mTmsToolsMaster == null) return false;
            using (ITmsToolsMasterBLL TmsToolsMasterBLL = BLLContainer.Resolve<ITmsToolsMasterBLL>())
            {
                return TmsToolsMasterBLL.Add(mTmsToolsMaster);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mTmsToolsMaster">模型</param>
        /// <returns>是否成功</returns>
        [Route("Update")]
        [HttpPost]
        public bool UpdateTmsToolsMaster(TmsToolsMaster mTmsToolsMaster)
        {
            if (mTmsToolsMaster == null) return false;
            using (ITmsToolsMasterBLL TmsToolsMasterBLL = BLLContainer.Resolve<ITmsToolsMasterBLL>())
            {
                return TmsToolsMasterBLL.Update(mTmsToolsMaster);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [Route("Deletes")]
        [HttpPost]
        public bool DelTmsToolsMasters(string[] Ids)
        {
            using (ITmsToolsMasterBLL TmsToolsMasterBLL = BLLContainer.Resolve<ITmsToolsMasterBLL>())
            {
                try
                {
                    List<TmsToolsMaster> entitys = new List<TmsToolsMaster>();
                    foreach (string id in Ids)
                    {
                        TmsToolsMaster item = TmsToolsMasterBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return TmsToolsMasterBLL.Delete(entitys);
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
        public bool DelTmsToolsMaster(string Id)
        {
            using (ITmsToolsMasterBLL TmsToolsMasterBLL = BLLContainer.Resolve<ITmsToolsMasterBLL>())
            {
                try
                {
                    TmsToolsMaster item = TmsToolsMasterBLL.GetFirstOrDefault(Id);
                    return TmsToolsMasterBLL.Delete(item);
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
        public List<TmsToolsMaster> GetTmsToolsMasters([FromBody]string sWhere)
        {
            Expression<Func<TmsToolsMaster, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<TmsToolsMaster>(sWhere);
            using (ITmsToolsMasterBLL TmsToolsMasterBLL = BLLContainer.Resolve<ITmsToolsMasterBLL>())
            {
                List<TmsToolsMaster> models = TmsToolsMasterBLL.GetModels(whereLamda);
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
        public TmsToolsMaster GetTmsToolsMasterById(string Id)
        {
            using (ITmsToolsMasterBLL TmsToolsMasterBLL = BLLContainer.Resolve<ITmsToolsMasterBLL>())
            {
                TmsToolsMaster model = TmsToolsMasterBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
