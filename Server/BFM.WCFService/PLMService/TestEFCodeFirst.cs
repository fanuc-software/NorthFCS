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
    /// TestEFCodeFirst Server
    /// </summary>
    public partial class PLMService : IPLMService
    {
        #region TestEFCodeFirst分页查询

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        public List<TestEFCodeFirst> GetTestEFCodeFirstByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere)
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
            Expression<Func<TestEFCodeFirst, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<TestEFCodeFirst>(sWhere); 

            using (ITestEFCodeFirstBLL TestEFCodeFirstBLL = BLLContainer.Resolve<ITestEFCodeFirstBLL>())
            {
                List<TestEFCodeFirst> models = TestEFCodeFirstBLL.GetModelsByPage(pageSize, pageIndex, isAsc, orderStr, whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        public int GetTestEFCodeFirstCount(string sWhere)
        {
            Expression<Func<TestEFCodeFirst, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<TestEFCodeFirst>(sWhere); 
            using (ITestEFCodeFirstBLL TestEFCodeFirstBLL = BLLContainer.Resolve<ITestEFCodeFirstBLL>())
            {
                return TestEFCodeFirstBLL.GetCount(whereLamda);
            }
        }

        #endregion TestEFCodeFirst分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mTestEFCodeFirst">模型</param>
        /// <returns>是否成功</returns>
        public bool AddTestEFCodeFirst(TestEFCodeFirst mTestEFCodeFirst)
        {
            if (mTestEFCodeFirst == null) return false;
            using (ITestEFCodeFirstBLL TestEFCodeFirstBLL = BLLContainer.Resolve<ITestEFCodeFirstBLL>())
            {
                return TestEFCodeFirstBLL.Add(mTestEFCodeFirst);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mTestEFCodeFirst">模型</param>
        /// <returns>是否成功</returns>
        public bool UpdateTestEFCodeFirst(TestEFCodeFirst mTestEFCodeFirst)
        {
            if (mTestEFCodeFirst == null) return false;
            using (ITestEFCodeFirstBLL TestEFCodeFirstBLL = BLLContainer.Resolve<ITestEFCodeFirstBLL>())
            {
                return TestEFCodeFirstBLL.Update(mTestEFCodeFirst);
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        public bool DelTestEFCodeFirsts(string[] Ids)
        {
            using (ITestEFCodeFirstBLL TestEFCodeFirstBLL = BLLContainer.Resolve<ITestEFCodeFirstBLL>())
            {
                try
                {
                    List<TestEFCodeFirst> entitys = new List<TestEFCodeFirst>();
                    foreach (string id in Ids)
                    {
                        TestEFCodeFirst item = TestEFCodeFirstBLL.GetFirstOrDefault(id);
                        entitys.Add(item);
                    }
                    return TestEFCodeFirstBLL.Delete(entitys);
                }
                catch { return false; }
            }
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        public bool DelTestEFCodeFirst(string Id)
        {
            using (ITestEFCodeFirstBLL TestEFCodeFirstBLL = BLLContainer.Resolve<ITestEFCodeFirstBLL>())
            {
                try
                {
                    TestEFCodeFirst item = TestEFCodeFirstBLL.GetFirstOrDefault(Id);
                    return TestEFCodeFirstBLL.Delete(item);
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
        public List<TestEFCodeFirst> GetTestEFCodeFirsts(string sWhere)
        {
            Expression<Func<TestEFCodeFirst, bool>> whereLamda = SerializerHelper.ConvertParamWhereToLinq<TestEFCodeFirst>(sWhere);
            using (ITestEFCodeFirstBLL TestEFCodeFirstBLL = BLLContainer.Resolve<ITestEFCodeFirstBLL>())
            {
                List<TestEFCodeFirst> models = TestEFCodeFirstBLL.GetModels(whereLamda);
                return models;
            }
        }

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        public TestEFCodeFirst GetTestEFCodeFirstById(string Id)
        {
            using (ITestEFCodeFirstBLL TestEFCodeFirstBLL = BLLContainer.Resolve<ITestEFCodeFirstBLL>())
            {
                TestEFCodeFirst model = TestEFCodeFirstBLL.GetFirstOrDefault(Id);
                return model;
            }
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
