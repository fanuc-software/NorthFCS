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
    /// RsRoutingEquip Server
    /// </summary>
    public partial class RSMService : IRSMService
    {
        #region RsRoutingEquip分页查询

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        public List<RsRoutingEquip> GetRsRoutingEquipByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<RsRoutingEquip, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsRoutingEquip>(sWhere); 

            using (IRsRoutingEquipBLL RsRoutingEquipBLL = BLLContainer.Resolve<IRsRoutingEquipBLL>())
            {
                List<RsRoutingEquip> models = RsRoutingEquipBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        public int GetRsRoutingEquipCount(string sWhere)
        {
            Expression<Func<RsRoutingEquip, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsRoutingEquip>(sWhere); 
            using (IRsRoutingEquipBLL RsRoutingEquipBLL = BLLContainer.Resolve<IRsRoutingEquipBLL>())
            {
                return RsRoutingEquipBLL.GetCount(whereLamda);
            }
        }

        #endregion RsRoutingEquip分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mRsRoutingEquip">模型</param>
        /// <returns>是否成功</returns>
        public bool AddRsRoutingEquip(RsRoutingEquip mRsRoutingEquip)
        {
            if (mRsRoutingEquip == null) return false;
            using (IRsRoutingEquipBLL RsRoutingEquipBLL = BLLContainer.Resolve<IRsRoutingEquipBLL>())
            {
                return RsRoutingEquipBLL.Add(mRsRoutingEquip);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mRsRoutingEquip">模型</param>
        /// <returns>是否成功</returns>
        public bool UpdateRsRoutingEquip(RsRoutingEquip mRsRoutingEquip)
        {
            if (mRsRoutingEquip == null) return false;
            using (IRsRoutingEquipBLL RsRoutingEquipBLL = BLLContainer.Resolve<IRsRoutingEquipBLL>())
            {
                return RsRoutingEquipBLL.Update(mRsRoutingEquip);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        public bool DelRsRoutingEquips(string[] Ids)
        {
            using (IRsRoutingEquipBLL RsRoutingEquipBLL = BLLContainer.Resolve<IRsRoutingEquipBLL>())
            {
                try
                {
                    List<RsRoutingEquip> entitys = new List<RsRoutingEquip>();
                    foreach (string id in Ids)
                    {
                        RsRoutingEquip item = RsRoutingEquipBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return RsRoutingEquipBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        public bool DelRsRoutingEquip(string Id)
        {
            using (IRsRoutingEquipBLL RsRoutingEquipBLL = BLLContainer.Resolve<IRsRoutingEquipBLL>())
            {
                try
                {
                    RsRoutingEquip item = RsRoutingEquipBLL.GetFirstOrDefault(Id);
                    return RsRoutingEquipBLL.Delete(item);
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
        public List<RsRoutingEquip> GetRsRoutingEquips(string sWhere)
        {
            Expression<Func<RsRoutingEquip, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<RsRoutingEquip>(sWhere);
            using (IRsRoutingEquipBLL RsRoutingEquipBLL = BLLContainer.Resolve<IRsRoutingEquipBLL>())
            {
                List<RsRoutingEquip> models = RsRoutingEquipBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        public RsRoutingEquip GetRsRoutingEquipById(string Id)
        {
            using (IRsRoutingEquipBLL RsRoutingEquipBLL = BLLContainer.Resolve<IRsRoutingEquipBLL>())
            {
                RsRoutingEquip model = RsRoutingEquipBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
