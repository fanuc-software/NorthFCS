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
    /// RsWorkSchedule ApiController
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("SDMService/RsWorkSchedule")]
    public class RsWorkScheduleController : ApiController 
    {
        #region RsWorkSchedule分页查询

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
        public List<RsWorkSchedule> GetRsWorkScheduleByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<RsWorkSchedule, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsWorkSchedule>(sWhere); 

            using (IRsWorkScheduleBLL RsWorkScheduleBLL = BLLContainer.Resolve<IRsWorkScheduleBLL>())
            {
                List<RsWorkSchedule> models = RsWorkScheduleBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
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
        public int GetRsWorkScheduleCount(string sWhere)
        {
            Expression<Func<RsWorkSchedule, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsWorkSchedule>(sWhere); 
            using (IRsWorkScheduleBLL RsWorkScheduleBLL = BLLContainer.Resolve<IRsWorkScheduleBLL>())
            {
                return RsWorkScheduleBLL.GetCount(whereLamda);
            }
        }

        #endregion RsWorkSchedule分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mRsWorkSchedule">模型</param>
        /// <returns>是否成功</returns>
        [Route("Add")]
        [HttpPost]
        public bool AddRsWorkSchedule(RsWorkSchedule mRsWorkSchedule)
        {
            if (mRsWorkSchedule == null) return false;
            using (IRsWorkScheduleBLL RsWorkScheduleBLL = BLLContainer.Resolve<IRsWorkScheduleBLL>())
            {
                return RsWorkScheduleBLL.Add(mRsWorkSchedule);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mRsWorkSchedule">模型</param>
        /// <returns>是否成功</returns>
        [Route("Update")]
        [HttpPost]
        public bool UpdateRsWorkSchedule(RsWorkSchedule mRsWorkSchedule)
        {
            if (mRsWorkSchedule == null) return false;
            using (IRsWorkScheduleBLL RsWorkScheduleBLL = BLLContainer.Resolve<IRsWorkScheduleBLL>())
            {
                return RsWorkScheduleBLL.Update(mRsWorkSchedule);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [Route("Deletes")]
        [HttpPost]
        public bool DelRsWorkSchedules(string[] Ids)
        {
            using (IRsWorkScheduleBLL RsWorkScheduleBLL = BLLContainer.Resolve<IRsWorkScheduleBLL>())
            {
                try
                {
                    List<RsWorkSchedule> entitys = new List<RsWorkSchedule>();
                    foreach (string id in Ids)
                    {
                        RsWorkSchedule item = RsWorkScheduleBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return RsWorkScheduleBLL.Delete(entitys);
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
        public bool DelRsWorkSchedule(string Id)
        {
            using (IRsWorkScheduleBLL RsWorkScheduleBLL = BLLContainer.Resolve<IRsWorkScheduleBLL>())
            {
                try
                {
                    RsWorkSchedule item = RsWorkScheduleBLL.GetFirstOrDefault(Id);
                    return RsWorkScheduleBLL.Delete(item);
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
        public List<RsWorkSchedule> GetRsWorkSchedules([FromBody]string sWhere)
        {
            Expression<Func<RsWorkSchedule, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsWorkSchedule>(sWhere);
            using (IRsWorkScheduleBLL RsWorkScheduleBLL = BLLContainer.Resolve<IRsWorkScheduleBLL>())
            {
                List<RsWorkSchedule> models = RsWorkScheduleBLL.GetModels(whereLamda);
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
        public RsWorkSchedule GetRsWorkScheduleById(string Id)
        {
            using (IRsWorkScheduleBLL RsWorkScheduleBLL = BLLContainer.Resolve<IRsWorkScheduleBLL>())
            {
                RsWorkSchedule model = RsWorkScheduleBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
