/*******************************************************************************
 * Copyright © 2018 代码生成器 版权所有
 * Author: LanGerp 
 * Description: 快速开发平台
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BFM.BLL.Container;
using BFM.BLL.IBLL;
using BFM.Common.Base.Helper;
using BFM.Common.Base.Utils;
using BFM.ContractModel;

namespace BFM.WCFService
{
    /// <summary>
    /// RmRepairRecord Server
    /// </summary>
    public partial class EAMService : IEAMService
    {
        #region RmRepairRecord分页查询

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        public List<RmRepairRecord> GetRmRepairRecordByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<RmRepairRecord, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RmRepairRecord>(sWhere); 

            using (IRmRepairRecordBLL RmRepairRecordBLL = BLLContainer.Resolve<IRmRepairRecordBLL>())
            {
                List<RmRepairRecord> models = RmRepairRecordBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        public int GetRmRepairRecordCount(string sWhere)
        {
            Expression<Func<RmRepairRecord, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RmRepairRecord>(sWhere); 
            using (IRmRepairRecordBLL RmRepairRecordBLL = BLLContainer.Resolve<IRmRepairRecordBLL>())
            {
                return RmRepairRecordBLL.GetCount(whereLamda);
            }
        }

        #endregion RmRepairRecord分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mRmRepairRecord">模型</param>
        /// <returns>是否成功</returns>
        public bool AddRmRepairRecord(RmRepairRecord mRmRepairRecord)
        {
            if (mRmRepairRecord == null) return false;
            using (IRmRepairRecordBLL RmRepairRecordBLL = BLLContainer.Resolve<IRmRepairRecordBLL>())
            {
                return RmRepairRecordBLL.Add(mRmRepairRecord);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mRmRepairRecord">模型</param>
        /// <returns>是否成功</returns>
        public bool UpdateRmRepairRecord(RmRepairRecord mRmRepairRecord)
        {
            if (mRmRepairRecord == null) return false;
            using (IRmRepairRecordBLL RmRepairRecordBLL = BLLContainer.Resolve<IRmRepairRecordBLL>())
            {
                return RmRepairRecordBLL.Update(mRmRepairRecord);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        public bool DelRmRepairRecords(string[] Ids)
        {
            using (IRmRepairRecordBLL RmRepairRecordBLL = BLLContainer.Resolve<IRmRepairRecordBLL>())
            {
                try
                {
                    List<RmRepairRecord> entitys = new List<RmRepairRecord>();
                    foreach (string id in Ids)
                    {
                        RmRepairRecord item = RmRepairRecordBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return RmRepairRecordBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        public bool DelRmRepairRecord(string Id)
        {
            using (IRmRepairRecordBLL RmRepairRecordBLL = BLLContainer.Resolve<IRmRepairRecordBLL>())
            {
                try
                {
                    RmRepairRecord item = RmRepairRecordBLL.GetFirstOrDefault(Id);
                    return RmRepairRecordBLL.Delete(item);
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
        public List<RmRepairRecord> GetRmRepairRecords(string sWhere)
        {
            Expression<Func<RmRepairRecord, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RmRepairRecord>(sWhere);
            using (IRmRepairRecordBLL RmRepairRecordBLL = BLLContainer.Resolve<IRmRepairRecordBLL>())
            {
                List<RmRepairRecord> models = RmRepairRecordBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        public RmRepairRecord GetRmRepairRecordById(string Id)
        {
            using (IRmRepairRecordBLL RmRepairRecordBLL = BLLContainer.Resolve<IRmRepairRecordBLL>())
            {
                RmRepairRecord model = RmRepairRecordBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
