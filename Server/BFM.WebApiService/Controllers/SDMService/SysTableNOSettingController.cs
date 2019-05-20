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
    /// SysTableNOSetting ApiController
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("SDMService/SysTableNOSetting")]
    public class SysTableNOSettingController : ApiController 
    {
        #region SysTableNOSetting分页查询

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
        public List<SysTableNOSetting> GetSysTableNOSettingByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<SysTableNOSetting, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<SysTableNOSetting>(sWhere); 

            using (ISysTableNOSettingBLL SysTableNOSettingBLL = BLLContainer.Resolve<ISysTableNOSettingBLL>())
            {
                List<SysTableNOSetting> models = SysTableNOSettingBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
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
        public int GetSysTableNOSettingCount(string sWhere)
        {
            Expression<Func<SysTableNOSetting, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<SysTableNOSetting>(sWhere); 
            using (ISysTableNOSettingBLL SysTableNOSettingBLL = BLLContainer.Resolve<ISysTableNOSettingBLL>())
            {
                return SysTableNOSettingBLL.GetCount(whereLamda);
            }
        }

        #endregion SysTableNOSetting分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mSysTableNOSetting">模型</param>
        /// <returns>是否成功</returns>
        [Route("Add")]
        [HttpPost]
        public bool AddSysTableNOSetting(SysTableNOSetting mSysTableNOSetting)
        {
            if (mSysTableNOSetting == null) return false;
            using (ISysTableNOSettingBLL SysTableNOSettingBLL = BLLContainer.Resolve<ISysTableNOSettingBLL>())
            {
                return SysTableNOSettingBLL.Add(mSysTableNOSetting);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mSysTableNOSetting">模型</param>
        /// <returns>是否成功</returns>
        [Route("Update")]
        [HttpPost]
        public bool UpdateSysTableNOSetting(SysTableNOSetting mSysTableNOSetting)
        {
            if (mSysTableNOSetting == null) return false;
            using (ISysTableNOSettingBLL SysTableNOSettingBLL = BLLContainer.Resolve<ISysTableNOSettingBLL>())
            {
                return SysTableNOSettingBLL.Update(mSysTableNOSetting);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [Route("Deletes")]
        [HttpPost]
        public bool DelSysTableNOSettings(string[] Ids)
        {
            using (ISysTableNOSettingBLL SysTableNOSettingBLL = BLLContainer.Resolve<ISysTableNOSettingBLL>())
            {
                try
                {
                    List<SysTableNOSetting> entitys = new List<SysTableNOSetting>();
                    foreach (string id in Ids)
                    {
                        SysTableNOSetting item = SysTableNOSettingBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return SysTableNOSettingBLL.Delete(entitys);
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
        public bool DelSysTableNOSetting(string Id)
        {
            using (ISysTableNOSettingBLL SysTableNOSettingBLL = BLLContainer.Resolve<ISysTableNOSettingBLL>())
            {
                try
                {
                    SysTableNOSetting item = SysTableNOSettingBLL.GetFirstOrDefault(Id);
                    return SysTableNOSettingBLL.Delete(item);
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
        public List<SysTableNOSetting> GetSysTableNOSettings([FromBody]string sWhere)
        {
            Expression<Func<SysTableNOSetting, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<SysTableNOSetting>(sWhere);
            using (ISysTableNOSettingBLL SysTableNOSettingBLL = BLLContainer.Resolve<ISysTableNOSettingBLL>())
            {
                List<SysTableNOSetting> models = SysTableNOSettingBLL.GetModels(whereLamda);
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
        public SysTableNOSetting GetSysTableNOSettingById(string Id)
        {
            using (ISysTableNOSettingBLL SysTableNOSettingBLL = BLLContainer.Resolve<ISysTableNOSettingBLL>())
            {
                SysTableNOSetting model = SysTableNOSettingBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
