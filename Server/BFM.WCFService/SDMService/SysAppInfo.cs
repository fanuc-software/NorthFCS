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
    /// SysAppInfo Server
    /// </summary>
    public partial class SDMService : ISDMService
    {
        #region SysAppInfo分页查询

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        public List<SysAppInfo> GetSysAppInfoByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<SysAppInfo, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<SysAppInfo>(sWhere); 

            using (ISysAppInfoBLL SysAppInfoBLL = BLLContainer.Resolve<ISysAppInfoBLL>())
            {
                List<SysAppInfo> models = SysAppInfoBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        public int GetSysAppInfoCount(string sWhere)
        {
            Expression<Func<SysAppInfo, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<SysAppInfo>(sWhere); 
            using (ISysAppInfoBLL SysAppInfoBLL = BLLContainer.Resolve<ISysAppInfoBLL>())
            {
                return SysAppInfoBLL.GetCount(whereLamda);
            }
        }

        #endregion SysAppInfo分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mSysAppInfo">模型</param>
        /// <returns>是否成功</returns>
        public bool AddSysAppInfo(SysAppInfo mSysAppInfo)
        {
            if (mSysAppInfo == null) return false;
            using (ISysAppInfoBLL SysAppInfoBLL = BLLContainer.Resolve<ISysAppInfoBLL>())
            {
                return SysAppInfoBLL.Add(mSysAppInfo);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mSysAppInfo">模型</param>
        /// <returns>是否成功</returns>
        public bool UpdateSysAppInfo(SysAppInfo mSysAppInfo)
        {
            if (mSysAppInfo == null) return false;
            using (ISysAppInfoBLL SysAppInfoBLL = BLLContainer.Resolve<ISysAppInfoBLL>())
            {
                return SysAppInfoBLL.Update(mSysAppInfo);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        public bool DelSysAppInfos(string[] Ids)
        {
            using (ISysAppInfoBLL SysAppInfoBLL = BLLContainer.Resolve<ISysAppInfoBLL>())
            {
                try
                {
                    List<SysAppInfo> entitys = new List<SysAppInfo>();
                    foreach (string id in Ids)
                    {
                        SysAppInfo item = SysAppInfoBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return SysAppInfoBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        public bool DelSysAppInfo(string Id)
        {
            using (ISysAppInfoBLL SysAppInfoBLL = BLLContainer.Resolve<ISysAppInfoBLL>())
            {
                try
                {
                    SysAppInfo item = SysAppInfoBLL.GetFirstOrDefault(Id);
                    return SysAppInfoBLL.Delete(item);
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
        public List<SysAppInfo> GetSysAppInfos(string sWhere)
        {
            Expression<Func<SysAppInfo, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<SysAppInfo>(sWhere);
            using (ISysAppInfoBLL SysAppInfoBLL = BLLContainer.Resolve<ISysAppInfoBLL>())
            {
                List<SysAppInfo> models = SysAppInfoBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        public SysAppInfo GetSysAppInfoById(string Id)
        {
            using (ISysAppInfoBLL SysAppInfoBLL = BLLContainer.Resolve<ISysAppInfoBLL>())
            {
                SysAppInfo model = SysAppInfoBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
