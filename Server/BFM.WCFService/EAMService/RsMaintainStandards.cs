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
    /// RsMaintainStandards Server
    /// </summary>
    public partial class EAMService : IEAMService
    {
        #region RsMaintainStandards分页查询

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        public List<RsMaintainStandards> GetRsMaintainStandardsByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<RsMaintainStandards, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsMaintainStandards>(sWhere); 

            using (IRsMaintainStandardsBLL RsMaintainStandardsBLL = BLLContainer.Resolve<IRsMaintainStandardsBLL>())
            {
                List<RsMaintainStandards> models = RsMaintainStandardsBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        public int GetRsMaintainStandardsCount(string sWhere)
        {
            Expression<Func<RsMaintainStandards, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsMaintainStandards>(sWhere); 
            using (IRsMaintainStandardsBLL RsMaintainStandardsBLL = BLLContainer.Resolve<IRsMaintainStandardsBLL>())
            {
                return RsMaintainStandardsBLL.GetCount(whereLamda);
            }
        }

        #endregion RsMaintainStandards分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mRsMaintainStandards">模型</param>
        /// <returns>是否成功</returns>
        public bool AddRsMaintainStandards(RsMaintainStandards mRsMaintainStandards)
        {
            if (mRsMaintainStandards == null) return false;
            using (IRsMaintainStandardsBLL RsMaintainStandardsBLL = BLLContainer.Resolve<IRsMaintainStandardsBLL>())
            {
                return RsMaintainStandardsBLL.Add(mRsMaintainStandards);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mRsMaintainStandards">模型</param>
        /// <returns>是否成功</returns>
        public bool UpdateRsMaintainStandards(RsMaintainStandards mRsMaintainStandards)
        {
            if (mRsMaintainStandards == null) return false;
            using (IRsMaintainStandardsBLL RsMaintainStandardsBLL = BLLContainer.Resolve<IRsMaintainStandardsBLL>())
            {
                return RsMaintainStandardsBLL.Update(mRsMaintainStandards);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        public bool DelRsMaintainStandardss(string[] Ids)
        {
            using (IRsMaintainStandardsBLL RsMaintainStandardsBLL = BLLContainer.Resolve<IRsMaintainStandardsBLL>())
            {
                try
                {
                    List<RsMaintainStandards> entitys = new List<RsMaintainStandards>();
                    foreach (string id in Ids)
                    {
                        RsMaintainStandards item = RsMaintainStandardsBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return RsMaintainStandardsBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        public bool DelRsMaintainStandards(string Id)
        {
            using (IRsMaintainStandardsBLL RsMaintainStandardsBLL = BLLContainer.Resolve<IRsMaintainStandardsBLL>())
            {
                try
                {
                    RsMaintainStandards item = RsMaintainStandardsBLL.GetFirstOrDefault(Id);
                    return RsMaintainStandardsBLL.Delete(item);
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
        public List<RsMaintainStandards> GetRsMaintainStandardss(string sWhere)
        {
            Expression<Func<RsMaintainStandards, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsMaintainStandards>(sWhere);
            using (IRsMaintainStandardsBLL RsMaintainStandardsBLL = BLLContainer.Resolve<IRsMaintainStandardsBLL>())
            {
                List<RsMaintainStandards> models = RsMaintainStandardsBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        public RsMaintainStandards GetRsMaintainStandardsById(string Id)
        {
            using (IRsMaintainStandardsBLL RsMaintainStandardsBLL = BLLContainer.Resolve<IRsMaintainStandardsBLL>())
            {
                RsMaintainStandards model = RsMaintainStandardsBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
