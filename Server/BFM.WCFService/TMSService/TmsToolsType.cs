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
    /// TmsToolsType Server
    /// </summary>
    public partial class TMSService : ITMSService
    {
        #region TmsToolsType分页查询

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        //[WebMethod(Description = "返回分页TmsToolsType")]
        public List<TmsToolsType> GetTmsToolsTypeByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<TmsToolsType, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<TmsToolsType>(sWhere); 

            using (ITmsToolsTypeBLL TmsToolsTypeBLL = BLLContainer.Resolve<ITmsToolsTypeBLL>())
            {
                List<TmsToolsType> models = TmsToolsTypeBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        //[WebMethod(Description = "返回TmsToolsType行数")]
        public int GetTmsToolsTypeCount(string sWhere)
        {
            Expression<Func<TmsToolsType, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<TmsToolsType>(sWhere); 
            using (ITmsToolsTypeBLL TmsToolsTypeBLL = BLLContainer.Resolve<ITmsToolsTypeBLL>())
            {
                return TmsToolsTypeBLL.GetCount(whereLamda);
            }
        }

        #endregion TmsToolsType分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mTmsToolsType">模型</param>
        /// <returns>是否成功</returns>
        //[WebMethod(Description = "新建TmsToolsType")]
        public bool AddTmsToolsType(TmsToolsType mTmsToolsType)
        {
            if (mTmsToolsType == null) return false;
            using (ITmsToolsTypeBLL TmsToolsTypeBLL = BLLContainer.Resolve<ITmsToolsTypeBLL>())
            {
                return TmsToolsTypeBLL.Add(mTmsToolsType);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mTmsToolsType">模型</param>
        /// <returns>是否成功</returns>
        //[WebMethod(Description = "更新TmsToolsType")]
        public bool UpdateTmsToolsType(TmsToolsType mTmsToolsType)
        {
            if (mTmsToolsType == null) return false;
            using (ITmsToolsTypeBLL TmsToolsTypeBLL = BLLContainer.Resolve<ITmsToolsTypeBLL>())
            {
                return TmsToolsTypeBLL.Update(mTmsToolsType);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        //[WebMethod(Description = "删除（多个）TmsToolsType")]
        public bool DelTmsToolsTypes(string[] Ids)
        {
            using (ITmsToolsTypeBLL TmsToolsTypeBLL = BLLContainer.Resolve<ITmsToolsTypeBLL>())
            {
                try
                {
                    List<TmsToolsType> entitys = new List<TmsToolsType>();
                    foreach (string id in Ids)
                    {
                        TmsToolsType item = TmsToolsTypeBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return TmsToolsTypeBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        //[WebMethod(Description = "删除TmsToolsType")]
        public bool DelTmsToolsType(string Id)
        {
            using (ITmsToolsTypeBLL TmsToolsTypeBLL = BLLContainer.Resolve<ITmsToolsTypeBLL>())
            {
                try
                {
                    TmsToolsType item = TmsToolsTypeBLL.GetFirstOrDefault(Id);
                    return TmsToolsTypeBLL.Delete(item);
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
        //[WebMethod(Description = "返回所有TmsToolsType")]
        public List<TmsToolsType> GetTmsToolsTypes(string sWhere)
        {
            Expression<Func<TmsToolsType, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<TmsToolsType>(sWhere);
            using (ITmsToolsTypeBLL TmsToolsTypeBLL = BLLContainer.Resolve<ITmsToolsTypeBLL>())
            {
                List<TmsToolsType> models = TmsToolsTypeBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        //[WebMethod(Description = "按Id查找TmsToolsType")]
        public TmsToolsType GetTmsToolsTypeById(string Id)
        {
            using (ITmsToolsTypeBLL TmsToolsTypeBLL = BLLContainer.Resolve<ITmsToolsTypeBLL>())
            {
                TmsToolsType model = TmsToolsTypeBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
