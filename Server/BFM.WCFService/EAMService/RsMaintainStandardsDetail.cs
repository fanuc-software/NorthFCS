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
    /// RsMaintainStandardsDetail Server
    /// </summary>
    public partial class EAMService : IEAMService
    {
        #region RsMaintainStandardsDetail分页查询

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        public List<RsMaintainStandardsDetail> GetRsMaintainStandardsDetailByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<RsMaintainStandardsDetail, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsMaintainStandardsDetail>(sWhere); 

            using (IRsMaintainStandardsDetailBLL RsMaintainStandardsDetailBLL = BLLContainer.Resolve<IRsMaintainStandardsDetailBLL>())
            {
                List<RsMaintainStandardsDetail> models = RsMaintainStandardsDetailBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        public int GetRsMaintainStandardsDetailCount(string sWhere)
        {
            Expression<Func<RsMaintainStandardsDetail, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsMaintainStandardsDetail>(sWhere); 
            using (IRsMaintainStandardsDetailBLL RsMaintainStandardsDetailBLL = BLLContainer.Resolve<IRsMaintainStandardsDetailBLL>())
            {
                return RsMaintainStandardsDetailBLL.GetCount(whereLamda);
            }
        }

        #endregion RsMaintainStandardsDetail分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mRsMaintainStandardsDetail">模型</param>
        /// <returns>是否成功</returns>
        public bool AddRsMaintainStandardsDetail(RsMaintainStandardsDetail mRsMaintainStandardsDetail)
        {
            if (mRsMaintainStandardsDetail == null) return false;
            using (IRsMaintainStandardsDetailBLL RsMaintainStandardsDetailBLL = BLLContainer.Resolve<IRsMaintainStandardsDetailBLL>())
            {
                return RsMaintainStandardsDetailBLL.Add(mRsMaintainStandardsDetail);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mRsMaintainStandardsDetail">模型</param>
        /// <returns>是否成功</returns>
        public bool UpdateRsMaintainStandardsDetail(RsMaintainStandardsDetail mRsMaintainStandardsDetail)
        {
            if (mRsMaintainStandardsDetail == null) return false;
            using (IRsMaintainStandardsDetailBLL RsMaintainStandardsDetailBLL = BLLContainer.Resolve<IRsMaintainStandardsDetailBLL>())
            {
                return RsMaintainStandardsDetailBLL.Update(mRsMaintainStandardsDetail);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        public bool DelRsMaintainStandardsDetails(string[] Ids)
        {
            using (IRsMaintainStandardsDetailBLL RsMaintainStandardsDetailBLL = BLLContainer.Resolve<IRsMaintainStandardsDetailBLL>())
            {
                try
                {
                    List<RsMaintainStandardsDetail> entitys = new List<RsMaintainStandardsDetail>();
                    foreach (string id in Ids)
                    {
                        RsMaintainStandardsDetail item = RsMaintainStandardsDetailBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return RsMaintainStandardsDetailBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        public bool DelRsMaintainStandardsDetail(string Id)
        {
            using (IRsMaintainStandardsDetailBLL RsMaintainStandardsDetailBLL = BLLContainer.Resolve<IRsMaintainStandardsDetailBLL>())
            {
                try
                {
                    RsMaintainStandardsDetail item = RsMaintainStandardsDetailBLL.GetFirstOrDefault(Id);
                    return RsMaintainStandardsDetailBLL.Delete(item);
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
        public List<RsMaintainStandardsDetail> GetRsMaintainStandardsDetails(string sWhere)
        {
            Expression<Func<RsMaintainStandardsDetail, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsMaintainStandardsDetail>(sWhere);
            using (IRsMaintainStandardsDetailBLL RsMaintainStandardsDetailBLL = BLLContainer.Resolve<IRsMaintainStandardsDetailBLL>())
            {
                List<RsMaintainStandardsDetail> models = RsMaintainStandardsDetailBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        public RsMaintainStandardsDetail GetRsMaintainStandardsDetailById(string Id)
        {
            using (IRsMaintainStandardsDetailBLL RsMaintainStandardsDetailBLL = BLLContainer.Resolve<IRsMaintainStandardsDetailBLL>())
            {
                RsMaintainStandardsDetail model = RsMaintainStandardsDetailBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
