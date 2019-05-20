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
    /// RsRoutingCheck Server
    /// </summary>
    public partial class RSMService : IRSMService
    {
        #region RsRoutingCheck分页查询

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        public List<RsRoutingCheck> GetRsRoutingCheckByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<RsRoutingCheck, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsRoutingCheck>(sWhere); 

            using (IRsRoutingCheckBLL RsRoutingCheckBLL = BLLContainer.Resolve<IRsRoutingCheckBLL>())
            {
                List<RsRoutingCheck> models = RsRoutingCheckBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        public int GetRsRoutingCheckCount(string sWhere)
        {
            Expression<Func<RsRoutingCheck, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsRoutingCheck>(sWhere); 
            using (IRsRoutingCheckBLL RsRoutingCheckBLL = BLLContainer.Resolve<IRsRoutingCheckBLL>())
            {
                return RsRoutingCheckBLL.GetCount(whereLamda);
            }
        }

        #endregion RsRoutingCheck分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mRsRoutingCheck">模型</param>
        /// <returns>是否成功</returns>
        public bool AddRsRoutingCheck(RsRoutingCheck mRsRoutingCheck)
        {
            if (mRsRoutingCheck == null) return false;
            using (IRsRoutingCheckBLL RsRoutingCheckBLL = BLLContainer.Resolve<IRsRoutingCheckBLL>())
            {
                return RsRoutingCheckBLL.Add(mRsRoutingCheck);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mRsRoutingCheck">模型</param>
        /// <returns>是否成功</returns>
        public bool UpdateRsRoutingCheck(RsRoutingCheck mRsRoutingCheck)
        {
            if (mRsRoutingCheck == null) return false;
            using (IRsRoutingCheckBLL RsRoutingCheckBLL = BLLContainer.Resolve<IRsRoutingCheckBLL>())
            {
                return RsRoutingCheckBLL.Update(mRsRoutingCheck);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        public bool DelRsRoutingChecks(string[] Ids)
        {
            using (IRsRoutingCheckBLL RsRoutingCheckBLL = BLLContainer.Resolve<IRsRoutingCheckBLL>())
            {
                try
                {
                    List<RsRoutingCheck> entitys = new List<RsRoutingCheck>();
                    foreach (string id in Ids)
                    {
                        RsRoutingCheck item = RsRoutingCheckBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return RsRoutingCheckBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        public bool DelRsRoutingCheck(string Id)
        {
            using (IRsRoutingCheckBLL RsRoutingCheckBLL = BLLContainer.Resolve<IRsRoutingCheckBLL>())
            {
                try
                {
                    RsRoutingCheck item = RsRoutingCheckBLL.GetFirstOrDefault(Id);
                    return RsRoutingCheckBLL.Delete(item);
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
        public List<RsRoutingCheck> GetRsRoutingChecks(string sWhere)
        {
            Expression<Func<RsRoutingCheck, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsRoutingCheck>(sWhere);
            using (IRsRoutingCheckBLL RsRoutingCheckBLL = BLLContainer.Resolve<IRsRoutingCheckBLL>())
            {
                List<RsRoutingCheck> models = RsRoutingCheckBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        public RsRoutingCheck GetRsRoutingCheckById(string Id)
        {
            using (IRsRoutingCheckBLL RsRoutingCheckBLL = BLLContainer.Resolve<IRsRoutingCheckBLL>())
            {
                RsRoutingCheck model = RsRoutingCheckBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
