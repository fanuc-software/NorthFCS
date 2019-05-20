/*******************************************************************************
 * Copyright © 2018 代码生成器 版权所有
 * Author: LanGerp 
 * Description: 快速开发平台
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Http;
using BFM.BLL.Container;
using BFM.BLL.IBLL;
using BFM.Common.Base.Helper;
using BFM.Common.Base.Utils;
using BFM.ContractModel;

namespace BFM.WebApiService.Controllers 
{
    /// <summary>
    /// SysUser ApiController
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("SDMService/SysUser")]
    public class SysUserController : ApiController 
    {
        #region SysUser分页查询

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [Route("GetPageData")]
        [HttpGet]
        public List<SysUser> GetSysUserByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<SysUser, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<SysUser>(sWhere); 

            using (ISysUserBLL SysUserBLL = BLLContainer.Resolve<ISysUserBLL>())
            {
                List<SysUser> models = SysUserBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [Route("GetCount")]
        [HttpGet]
        public int GetSysUserCount(string sWhere)
        {
            Expression<Func<SysUser, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<SysUser>(sWhere); 
            using (ISysUserBLL SysUserBLL = BLLContainer.Resolve<ISysUserBLL>())
            {
                return SysUserBLL.GetCount(whereLamda);
            }
        }

        #endregion SysUser分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mSysUser">模型</param>
        /// <returns>是否成功</returns>
        [Route("Add")]
        [HttpPost]
        public bool AddSysUser(SysUser mSysUser)
        {
            if (mSysUser == null) return false;
            using (ISysUserBLL SysUserBLL = BLLContainer.Resolve<ISysUserBLL>())
            {
                return SysUserBLL.Add(mSysUser);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mSysUser">模型</param>
        /// <returns>是否成功</returns>
        [Route("Update")]
        [HttpPost]
        public bool UpdateSysUser(SysUser mSysUser)
        {
            if (mSysUser == null) return false;
            using (ISysUserBLL SysUserBLL = BLLContainer.Resolve<ISysUserBLL>())
            {
                return SysUserBLL.Update(mSysUser);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [Route("Deletes")]
        [HttpPost]
        public bool DelSysUsers(string[] Ids)
        {
            using (ISysUserBLL SysUserBLL = BLLContainer.Resolve<ISysUserBLL>())
            {
                try
                {
                    List<SysUser> entitys = new List<SysUser>();
                    foreach (string id in Ids)
                    {
                        SysUser item = SysUserBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return SysUserBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [Route("Delete")]
        [HttpGet]
        public bool DelSysUser(string Id)
        {
            using (ISysUserBLL SysUserBLL = BLLContainer.Resolve<ISysUserBLL>())
            {
                try
                {
                    SysUser item = SysUserBLL.GetFirstOrDefault(Id);
                    return SysUserBLL.Delete(item);
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
        [Route("GetByParam")]
        [HttpPost]
        public List<SysUser> GetSysUsers([FromBody]string sWhere)
        {
            Expression<Func<SysUser, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<SysUser>(sWhere);
            using (ISysUserBLL SysUserBLL = BLLContainer.Resolve<ISysUserBLL>())
            {
                List<SysUser> models = SysUserBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [Route("GetById")]
        [HttpGet]
        public SysUser GetSysUserById(string Id)
        {
            using (ISysUserBLL SysUserBLL = BLLContainer.Resolve<ISysUserBLL>())
            {
                SysUser model = SysUserBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
