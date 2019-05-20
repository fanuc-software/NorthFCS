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
    /// SysUserMenu Server
    /// </summary>
    public partial class SDMService : ISDMService
    {
        #region SysUserMenu分页查询

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        public List<SysUserMenu> GetSysUserMenuByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<SysUserMenu, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<SysUserMenu>(sWhere); 

            using (ISysUserMenuBLL SysUserMenuBLL = BLLContainer.Resolve<ISysUserMenuBLL>())
            {
                List<SysUserMenu> models = SysUserMenuBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        public int GetSysUserMenuCount(string sWhere)
        {
            Expression<Func<SysUserMenu, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<SysUserMenu>(sWhere); 
            using (ISysUserMenuBLL SysUserMenuBLL = BLLContainer.Resolve<ISysUserMenuBLL>())
            {
                return SysUserMenuBLL.GetCount(whereLamda);
            }
        }

        #endregion SysUserMenu分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mSysUserMenu">模型</param>
        /// <returns>是否成功</returns>
        public bool AddSysUserMenu(SysUserMenu mSysUserMenu)
        {
            if (mSysUserMenu == null) return false;
            using (ISysUserMenuBLL SysUserMenuBLL = BLLContainer.Resolve<ISysUserMenuBLL>())
            {
                return SysUserMenuBLL.Add(mSysUserMenu);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mSysUserMenu">模型</param>
        /// <returns>是否成功</returns>
        public bool UpdateSysUserMenu(SysUserMenu mSysUserMenu)
        {
            if (mSysUserMenu == null) return false;
            using (ISysUserMenuBLL SysUserMenuBLL = BLLContainer.Resolve<ISysUserMenuBLL>())
            {
                return SysUserMenuBLL.Update(mSysUserMenu);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        public bool DelSysUserMenus(string[] Ids)
        {
            using (ISysUserMenuBLL SysUserMenuBLL = BLLContainer.Resolve<ISysUserMenuBLL>())
            {
                try
                {
                    List<SysUserMenu> entitys = new List<SysUserMenu>();
                    foreach (string id in Ids)
                    {
                        SysUserMenu item = SysUserMenuBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return SysUserMenuBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        public bool DelSysUserMenu(string Id)
        {
            using (ISysUserMenuBLL SysUserMenuBLL = BLLContainer.Resolve<ISysUserMenuBLL>())
            {
                try
                {
                    SysUserMenu item = SysUserMenuBLL.GetFirstOrDefault(Id);
                    return SysUserMenuBLL.Delete(item);
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
        public List<SysUserMenu> GetSysUserMenus(string sWhere)
        {
            Expression<Func<SysUserMenu, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<SysUserMenu>(sWhere);
            using (ISysUserMenuBLL SysUserMenuBLL = BLLContainer.Resolve<ISysUserMenuBLL>())
            {
                List<SysUserMenu> models = SysUserMenuBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        public SysUserMenu GetSysUserMenuById(string Id)
        {
            using (ISysUserMenuBLL SysUserMenuBLL = BLLContainer.Resolve<ISysUserMenuBLL>())
            {
                SysUserMenu model = SysUserMenuBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
