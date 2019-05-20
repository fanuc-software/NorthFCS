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
    /// DAAlarmRecord Server
    /// </summary>
    public partial class DAService : IDAService
    {
        #region DAAlarmRecord分页查询

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [WebMethod(Description = "返回分页DAAlarmRecord")]
        public List<DAAlarmRecord> GetDAAlarmRecordByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<DAAlarmRecord, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<DAAlarmRecord>(sWhere); 

            using (IDAAlarmRecordBLL DAAlarmRecordBLL = BLLContainer.Resolve<IDAAlarmRecordBLL>())
            {
                List<DAAlarmRecord> models = DAAlarmRecordBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [WebMethod(Description = "返回DAAlarmRecord记录总数 配合分页查询用")]
        public int GetDAAlarmRecordCount(string sWhere)
        {
            Expression<Func<DAAlarmRecord, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<DAAlarmRecord>(sWhere); 
            using (IDAAlarmRecordBLL DAAlarmRecordBLL = BLLContainer.Resolve<IDAAlarmRecordBLL>())
            {
                return DAAlarmRecordBLL.GetCount(whereLamda);
            }
        }

        #endregion DAAlarmRecord分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mDAAlarmRecord">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "新增DAAlarmRecord")]
        public bool AddDAAlarmRecord(DAAlarmRecord mDAAlarmRecord)
        {
            if (mDAAlarmRecord == null) return false;
            using (IDAAlarmRecordBLL DAAlarmRecordBLL = BLLContainer.Resolve<IDAAlarmRecordBLL>())
            {
                return DAAlarmRecordBLL.Add(mDAAlarmRecord);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mDAAlarmRecord">模型</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "修改DAAlarmRecord")]
        public bool UpdateDAAlarmRecord(DAAlarmRecord mDAAlarmRecord)
        {
            if (mDAAlarmRecord == null) return false;
            using (IDAAlarmRecordBLL DAAlarmRecordBLL = BLLContainer.Resolve<IDAAlarmRecordBLL>())
            {
                return DAAlarmRecordBLL.Update(mDAAlarmRecord);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "按照关键字段删除（多个）DAAlarmRecord")]
        public bool DelDAAlarmRecords(string[] Ids)
        {
            using (IDAAlarmRecordBLL DAAlarmRecordBLL = BLLContainer.Resolve<IDAAlarmRecordBLL>())
            {
                try
                {
                    List<DAAlarmRecord> entitys = new List<DAAlarmRecord>();
                    foreach (string id in Ids)
                    {
                        DAAlarmRecord item = DAAlarmRecordBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return DAAlarmRecordBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [WebMethod(Description = "删除DAAlarmRecord")]
        public bool DelDAAlarmRecord(string Id)
        {
            using (IDAAlarmRecordBLL DAAlarmRecordBLL = BLLContainer.Resolve<IDAAlarmRecordBLL>())
            {
                try
                {
                    DAAlarmRecord item = DAAlarmRecordBLL.GetFirstOrDefault(Id);
                    return DAAlarmRecordBLL.Delete(item);
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
        [WebMethod(Description = "根据查询条件获取记录集DAAlarmRecord")]
        public List<DAAlarmRecord> GetDAAlarmRecords(string sWhere)
        {
            Expression<Func<DAAlarmRecord, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<DAAlarmRecord>(sWhere);
            using (IDAAlarmRecordBLL DAAlarmRecordBLL = BLLContainer.Resolve<IDAAlarmRecordBLL>())
            {
                List<DAAlarmRecord> models = DAAlarmRecordBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [WebMethod(Description = "按Id查找DAAlarmRecord")]
        public DAAlarmRecord GetDAAlarmRecordById(string Id)
        {
            using (IDAAlarmRecordBLL DAAlarmRecordBLL = BLLContainer.Resolve<IDAAlarmRecordBLL>())
            {
                DAAlarmRecord model = DAAlarmRecordBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
