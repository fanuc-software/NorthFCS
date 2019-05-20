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
    /// SysRolePurview Server
    /// </summary>
    public partial class SDMService : ISDMService
    {
        #region SysRolePurview分页查询

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        public List<SysRolePurview> GetSysRolePurviewByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<SysRolePurview, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<SysRolePurview>(sWhere); 

            using (ISysRolePurviewBLL SysRolePurviewBLL = BLLContainer.Resolve<ISysRolePurviewBLL>())
            {
                List<SysRolePurview> models = SysRolePurviewBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        public int GetSysRolePurviewCount(string sWhere)
        {
            Expression<Func<SysRolePurview, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<SysRolePurview>(sWhere); 
            using (ISysRolePurviewBLL SysRolePurviewBLL = BLLContainer.Resolve<ISysRolePurviewBLL>())
            {
                return SysRolePurviewBLL.GetCount(whereLamda);
            }
        }

        #endregion SysRolePurview分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mSysRolePurview">模型</param>
        /// <returns>是否成功</returns>
        public bool AddSysRolePurview(SysRolePurview mSysRolePurview)
        {
            if (mSysRolePurview == null) return false;
            using (ISysRolePurviewBLL SysRolePurviewBLL = BLLContainer.Resolve<ISysRolePurviewBLL>())
            {
                return SysRolePurviewBLL.Add(mSysRolePurview);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mSysRolePurview">模型</param>
        /// <returns>是否成功</returns>
        public bool UpdateSysRolePurview(SysRolePurview mSysRolePurview)
        {
            if (mSysRolePurview == null) return false;
            using (ISysRolePurviewBLL SysRolePurviewBLL = BLLContainer.Resolve<ISysRolePurviewBLL>())
            {
                return SysRolePurviewBLL.Update(mSysRolePurview);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        public bool DelSysRolePurviews(string[] Ids)
        {
            using (ISysRolePurviewBLL SysRolePurviewBLL = BLLContainer.Resolve<ISysRolePurviewBLL>())
            {
                try
                {
                    List<SysRolePurview> entitys = new List<SysRolePurview>();
                    foreach (string id in Ids)
                    {
                        SysRolePurview item = SysRolePurviewBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return SysRolePurviewBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        public bool DelSysRolePurview(string Id)
        {
            using (ISysRolePurviewBLL SysRolePurviewBLL = BLLContainer.Resolve<ISysRolePurviewBLL>())
            {
                try
                {
                    SysRolePurview item = SysRolePurviewBLL.GetFirstOrDefault(Id);
                    return SysRolePurviewBLL.Delete(item);
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
        public List<SysRolePurview> GetSysRolePurviews(string sWhere)
        {
            Expression<Func<SysRolePurview, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<SysRolePurview>(sWhere);
            using (ISysRolePurviewBLL SysRolePurviewBLL = BLLContainer.Resolve<ISysRolePurviewBLL>())
            {
                List<SysRolePurview> models = SysRolePurviewBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        public SysRolePurview GetSysRolePurviewById(string Id)
        {
            using (ISysRolePurviewBLL SysRolePurviewBLL = BLLContainer.Resolve<ISysRolePurviewBLL>())
            {
                SysRolePurview model = SysRolePurviewBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
