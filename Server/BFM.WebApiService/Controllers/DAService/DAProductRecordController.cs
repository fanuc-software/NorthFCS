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
    /// DAProductRecord ApiController
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("DAService/DAProductRecord")]
    public class DAProductRecordController : ApiController 
    {
        #region DAProductRecord分页查询

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
        public List<DAProductRecord> GetDAProductRecordByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<DAProductRecord, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<DAProductRecord>(sWhere); 

            using (IDAProductRecordBLL DAProductRecordBLL = BLLContainer.Resolve<IDAProductRecordBLL>())
            {
                List<DAProductRecord> models = DAProductRecordBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
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
        public int GetDAProductRecordCount(string sWhere)
        {
            Expression<Func<DAProductRecord, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<DAProductRecord>(sWhere); 
            using (IDAProductRecordBLL DAProductRecordBLL = BLLContainer.Resolve<IDAProductRecordBLL>())
            {
                return DAProductRecordBLL.GetCount(whereLamda);
            }
        }

        #endregion DAProductRecord分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mDAProductRecord">模型</param>
        /// <returns>是否成功</returns>
        [Route("Add")]
        [HttpPost]
        public bool AddDAProductRecord(DAProductRecord mDAProductRecord)
        {
            if (mDAProductRecord == null) return false;
            using (IDAProductRecordBLL DAProductRecordBLL = BLLContainer.Resolve<IDAProductRecordBLL>())
            {
                return DAProductRecordBLL.Add(mDAProductRecord);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mDAProductRecord">模型</param>
        /// <returns>是否成功</returns>
        [Route("Update")]
        [HttpPost]
        public bool UpdateDAProductRecord(DAProductRecord mDAProductRecord)
        {
            if (mDAProductRecord == null) return false;
            using (IDAProductRecordBLL DAProductRecordBLL = BLLContainer.Resolve<IDAProductRecordBLL>())
            {
                return DAProductRecordBLL.Update(mDAProductRecord);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [Route("Deletes")]
        [HttpPost]
        public bool DelDAProductRecords(string[] Ids)
        {
            using (IDAProductRecordBLL DAProductRecordBLL = BLLContainer.Resolve<IDAProductRecordBLL>())
            {
                try
                {
                    List<DAProductRecord> entitys = new List<DAProductRecord>();
                    foreach (string id in Ids)
                    {
                        DAProductRecord item = DAProductRecordBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return DAProductRecordBLL.Delete(entitys);
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
        public bool DelDAProductRecord(string Id)
        {
            using (IDAProductRecordBLL DAProductRecordBLL = BLLContainer.Resolve<IDAProductRecordBLL>())
            {
                try
                {
                    DAProductRecord item = DAProductRecordBLL.GetFirstOrDefault(Id);
                    return DAProductRecordBLL.Delete(item);
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
        public List<DAProductRecord> GetDAProductRecords([FromBody]string sWhere)
        {
            Expression<Func<DAProductRecord, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<DAProductRecord>(sWhere);
            using (IDAProductRecordBLL DAProductRecordBLL = BLLContainer.Resolve<IDAProductRecordBLL>())
            {
                List<DAProductRecord> models = DAProductRecordBLL.GetModels(whereLamda);
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
        public DAProductRecord GetDAProductRecordById(string Id)
        {
            using (IDAProductRecordBLL DAProductRecordBLL = BLLContainer.Resolve<IDAProductRecordBLL>())
            {
                DAProductRecord model = DAProductRecordBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
