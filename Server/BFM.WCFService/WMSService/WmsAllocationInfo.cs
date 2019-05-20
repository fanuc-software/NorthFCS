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
    /// WmsAllocationInfo Server
    /// </summary>
    public partial class WMSService : IWMSService
    {
        #region WmsAllocationInfo分页查询

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        public List<WmsAllocationInfo> GetWmsAllocationInfoByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<WmsAllocationInfo, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<WmsAllocationInfo>(sWhere); 

            using (IWmsAllocationInfoBLL WmsAllocationInfoBLL = BLLContainer.Resolve<IWmsAllocationInfoBLL>())
            {
                List<WmsAllocationInfo> models = WmsAllocationInfoBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        public int GetWmsAllocationInfoCount(string sWhere)
        {
            Expression<Func<WmsAllocationInfo, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<WmsAllocationInfo>(sWhere); 
            using (IWmsAllocationInfoBLL WmsAllocationInfoBLL = BLLContainer.Resolve<IWmsAllocationInfoBLL>())
            {
                return WmsAllocationInfoBLL.GetCount(whereLamda);
            }
        }

        #endregion WmsAllocationInfo分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mWmsAllocationInfo">模型</param>
        /// <returns>是否成功</returns>
        public bool AddWmsAllocationInfo(WmsAllocationInfo mWmsAllocationInfo)
        {
            if (mWmsAllocationInfo == null) return false;
            using (IWmsAllocationInfoBLL WmsAllocationInfoBLL = BLLContainer.Resolve<IWmsAllocationInfoBLL>())
            {
                return WmsAllocationInfoBLL.Add(mWmsAllocationInfo);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mWmsAllocationInfo">模型</param>
        /// <returns>是否成功</returns>
        public bool UpdateWmsAllocationInfo(WmsAllocationInfo mWmsAllocationInfo)
        {
            if (mWmsAllocationInfo == null) return false;
            using (IWmsAllocationInfoBLL WmsAllocationInfoBLL = BLLContainer.Resolve<IWmsAllocationInfoBLL>())
            {
                return WmsAllocationInfoBLL.Update(mWmsAllocationInfo);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        public bool DelWmsAllocationInfos(string[] Ids)
        {
            using (IWmsAllocationInfoBLL WmsAllocationInfoBLL = BLLContainer.Resolve<IWmsAllocationInfoBLL>())
            {
                try
                {
                    List<WmsAllocationInfo> entitys = new List<WmsAllocationInfo>();
                    foreach (string id in Ids)
                    {
                        WmsAllocationInfo item = WmsAllocationInfoBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return WmsAllocationInfoBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        public bool DelWmsAllocationInfo(string Id)
        {
            using (IWmsAllocationInfoBLL WmsAllocationInfoBLL = BLLContainer.Resolve<IWmsAllocationInfoBLL>())
            {
                try
                {
                    WmsAllocationInfo item = WmsAllocationInfoBLL.GetFirstOrDefault(Id);
                    return WmsAllocationInfoBLL.Delete(item);
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
        public List<WmsAllocationInfo> GetWmsAllocationInfos(string sWhere)
        {
            Expression<Func<WmsAllocationInfo, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<WmsAllocationInfo>(sWhere);
            using (IWmsAllocationInfoBLL WmsAllocationInfoBLL = BLLContainer.Resolve<IWmsAllocationInfoBLL>())
            {
                List<WmsAllocationInfo> models = WmsAllocationInfoBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        public WmsAllocationInfo GetWmsAllocationInfoById(string Id)
        {
            using (IWmsAllocationInfoBLL WmsAllocationInfoBLL = BLLContainer.Resolve<IWmsAllocationInfoBLL>())
            {
                WmsAllocationInfo model = WmsAllocationInfoBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
