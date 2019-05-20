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
    /// TmsDeviceToolsPos Server
    /// </summary>
    public partial class TMSService : ITMSService
    {
        #region TmsDeviceToolsPos分页查询

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        //[WebMethod(Description = "返回分页TmsDeviceToolsPos")]
        public List<TmsDeviceToolsPos> GetTmsDeviceToolsPosByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<TmsDeviceToolsPos, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<TmsDeviceToolsPos>(sWhere); 

            using (ITmsDeviceToolsPosBLL TmsDeviceToolsPosBLL = BLLContainer.Resolve<ITmsDeviceToolsPosBLL>())
            {
                List<TmsDeviceToolsPos> models = TmsDeviceToolsPosBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        //[WebMethod(Description = "返回TmsDeviceToolsPos行数")]
        public int GetTmsDeviceToolsPosCount(string sWhere)
        {
            Expression<Func<TmsDeviceToolsPos, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<TmsDeviceToolsPos>(sWhere); 
            using (ITmsDeviceToolsPosBLL TmsDeviceToolsPosBLL = BLLContainer.Resolve<ITmsDeviceToolsPosBLL>())
            {
                return TmsDeviceToolsPosBLL.GetCount(whereLamda);
            }
        }

        #endregion TmsDeviceToolsPos分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mTmsDeviceToolsPos">模型</param>
        /// <returns>是否成功</returns>
        //[WebMethod(Description = "新建TmsDeviceToolsPos")]
        public bool AddTmsDeviceToolsPos(TmsDeviceToolsPos mTmsDeviceToolsPos)
        {
            if (mTmsDeviceToolsPos == null) return false;
            using (ITmsDeviceToolsPosBLL TmsDeviceToolsPosBLL = BLLContainer.Resolve<ITmsDeviceToolsPosBLL>())
            {
                return TmsDeviceToolsPosBLL.Add(mTmsDeviceToolsPos);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mTmsDeviceToolsPos">模型</param>
        /// <returns>是否成功</returns>
        //[WebMethod(Description = "更新TmsDeviceToolsPos")]
        public bool UpdateTmsDeviceToolsPos(TmsDeviceToolsPos mTmsDeviceToolsPos)
        {
            if (mTmsDeviceToolsPos == null) return false;
            using (ITmsDeviceToolsPosBLL TmsDeviceToolsPosBLL = BLLContainer.Resolve<ITmsDeviceToolsPosBLL>())
            {
                return TmsDeviceToolsPosBLL.Update(mTmsDeviceToolsPos);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        //[WebMethod(Description = "删除（多个）TmsDeviceToolsPos")]
        public bool DelTmsDeviceToolsPoss(string[] Ids)
        {
            using (ITmsDeviceToolsPosBLL TmsDeviceToolsPosBLL = BLLContainer.Resolve<ITmsDeviceToolsPosBLL>())
            {
                try
                {
                    List<TmsDeviceToolsPos> entitys = new List<TmsDeviceToolsPos>();
                    foreach (string id in Ids)
                    {
                        TmsDeviceToolsPos item = TmsDeviceToolsPosBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return TmsDeviceToolsPosBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        //[WebMethod(Description = "删除TmsDeviceToolsPos")]
        public bool DelTmsDeviceToolsPos(string Id)
        {
            using (ITmsDeviceToolsPosBLL TmsDeviceToolsPosBLL = BLLContainer.Resolve<ITmsDeviceToolsPosBLL>())
            {
                try
                {
                    TmsDeviceToolsPos item = TmsDeviceToolsPosBLL.GetFirstOrDefault(Id);
                    return TmsDeviceToolsPosBLL.Delete(item);
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
        //[WebMethod(Description = "返回所有TmsDeviceToolsPos")]
        public List<TmsDeviceToolsPos> GetTmsDeviceToolsPoss(string sWhere)
        {
            Expression<Func<TmsDeviceToolsPos, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<TmsDeviceToolsPos>(sWhere);
            using (ITmsDeviceToolsPosBLL TmsDeviceToolsPosBLL = BLLContainer.Resolve<ITmsDeviceToolsPosBLL>())
            {
                List<TmsDeviceToolsPos> models = TmsDeviceToolsPosBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        //[WebMethod(Description = "按Id查找TmsDeviceToolsPos")]
        public TmsDeviceToolsPos GetTmsDeviceToolsPosById(string Id)
        {
            using (ITmsDeviceToolsPosBLL TmsDeviceToolsPosBLL = BLLContainer.Resolve<ITmsDeviceToolsPosBLL>())
            {
                TmsDeviceToolsPos model = TmsDeviceToolsPosBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
