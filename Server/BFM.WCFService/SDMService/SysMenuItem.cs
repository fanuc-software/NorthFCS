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
    /// SysMenuItem Server
    /// </summary>
    public partial class SDMService : ISDMService
    {
        #region SysMenuItem分页查询

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        public List<SysMenuItem> GetSysMenuItemByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<SysMenuItem, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<SysMenuItem>(sWhere); 

            using (ISysMenuItemBLL SysMenuItemBLL = BLLContainer.Resolve<ISysMenuItemBLL>())
            {
                List<SysMenuItem> models = SysMenuItemBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        public int GetSysMenuItemCount(string sWhere)
        {
            Expression<Func<SysMenuItem, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<SysMenuItem>(sWhere); 
            using (ISysMenuItemBLL SysMenuItemBLL = BLLContainer.Resolve<ISysMenuItemBLL>())
            {
                return SysMenuItemBLL.GetCount(whereLamda);
            }
        }

        #endregion SysMenuItem分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mSysMenuItem">模型</param>
        /// <returns>是否成功</returns>
        public bool AddSysMenuItem(SysMenuItem mSysMenuItem)
        {
            if (mSysMenuItem == null) return false;
            using (ISysMenuItemBLL SysMenuItemBLL = BLLContainer.Resolve<ISysMenuItemBLL>())
            {
                return SysMenuItemBLL.Add(mSysMenuItem);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mSysMenuItem">模型</param>
        /// <returns>是否成功</returns>
        public bool UpdateSysMenuItem(SysMenuItem mSysMenuItem)
        {
            if (mSysMenuItem == null) return false;
            using (ISysMenuItemBLL SysMenuItemBLL = BLLContainer.Resolve<ISysMenuItemBLL>())
            {
                return SysMenuItemBLL.Update(mSysMenuItem);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        public bool DelSysMenuItems(string[] Ids)
        {
            using (ISysMenuItemBLL SysMenuItemBLL = BLLContainer.Resolve<ISysMenuItemBLL>())
            {
                try
                {
                    List<SysMenuItem> entitys = new List<SysMenuItem>();
                    foreach (string id in Ids)
                    {
                        SysMenuItem item = SysMenuItemBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return SysMenuItemBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        public bool DelSysMenuItem(string Id)
        {
            using (ISysMenuItemBLL SysMenuItemBLL = BLLContainer.Resolve<ISysMenuItemBLL>())
            {
                try
                {
                    SysMenuItem item = SysMenuItemBLL.GetFirstOrDefault(Id);
                    return SysMenuItemBLL.Delete(item);
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
        public List<SysMenuItem> GetSysMenuItems(string sWhere)
        {
            Expression<Func<SysMenuItem, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<SysMenuItem>(sWhere);
            using (ISysMenuItemBLL SysMenuItemBLL = BLLContainer.Resolve<ISysMenuItemBLL>())
            {
                List<SysMenuItem> models = SysMenuItemBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        public SysMenuItem GetSysMenuItemById(string Id)
        {
            using (ISysMenuItemBLL SysMenuItemBLL = BLLContainer.Resolve<ISysMenuItemBLL>())
            {
                SysMenuItem model = SysMenuItemBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
