/*******************************************************************************
 * Copyright © 2018 代码生成器 版权所有
 * Author: LanGerp 
 * Description: 快速开发平台
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BFM.Common.Base;

namespace BFM.Server.DataAsset
{
    public class SDMFacade<T> : IObjectFacade<T> where T : new() 
    {
        private string ModelName = "SDMService";
        protected HttpClient client;
        public SDMFacade()
        {
            client = new HttpClient();
            string webapiurl = StringWrapper.GetAppSettingValue("WebApiUrl");
            if (string.IsNullOrEmpty(webapiurl))
            {
                webapiurl = "http://localhost/BFM.WebApiService/";
            }
            client.BaseAddress = new Uri(webapiurl);  //测试地址：http://localhost/BFM.WebApiService/ 
        }

        #region SysDepartment分页查询

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <param name="controllerName">Controller名称 默认空 </param>
        /// <returns>符合查询条件的List结果</returns>
        public async Task<List<T>> GetPageData(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere, string controllerName = null)
        {
            if (string.IsNullOrEmpty(controllerName))
            {
                controllerName = typeof (T).Name;
            }

            var responseMsg = 
                await 
                    client.GetAsync( 
                    $"{ModelName}/{controllerName}/GetPageData/?pageSize={pageSize}&pageIndex={pageIndex}&isAsc={isAsc}&orderField={orderField}&sWhere={sWhere}"); 
            return await responseMsg.Content.ReadAsAsync<List<T>>(); 
        }

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <param name="controllerName">Controller名称 默认空 </param>
        /// <returns>符合查询条件的记录数</returns>
        public async Task<int> GetCount(string sWhere, string controllerName = null)
        {
            if (string.IsNullOrEmpty(controllerName))
            {
                controllerName = typeof (T).Name;
            }

            var responseMsg = 
                await 
                    client.GetAsync( 
                    $"{ModelName}/{controllerName}/GetCount/?sWhere={sWhere}"); 
            return await responseMsg.Content.ReadAsAsync<int>(); 
        }

        #endregion SysDepartment分页查询

        #region Add、Update、Delete

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="t">模型</param>
        /// <param name="controllerName">Controller名称 默认空 </param>
        /// <returns>是否成功</returns>
        public async Task<bool> Add(T t, string controllerName = null)
        {
            if (string.IsNullOrEmpty(controllerName))
            {
                controllerName = typeof (T).Name;
            }

            HttpResponseMessage responseMsg = 
                await client.PostAsJsonAsync($"{ModelName}/{controllerName}/Add/", t); 
            return await responseMsg.Content.ReadAsAsync<bool>();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="t">模型</param>
        /// <param name="controllerName">Controller名称 默认空 </param>
        /// <returns>是否成功</returns>
        public async Task<bool> Update(T t, string controllerName = null)
        {
            if (string.IsNullOrEmpty(controllerName))
            {
                controllerName = typeof (T).Name;
            }

            HttpResponseMessage responseMsg = 
                await client.PostAsJsonAsync($"{ModelName}/{controllerName}/Update/", t); 
            return await responseMsg.Content.ReadAsAsync<bool>();
        }

        /// <summary>
        /// 按照关键字段删除（多个）
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <param name="controllerName">Controller名称 默认空 </param>
        /// <returns>是否成功</returns>
        public async Task<bool> Deletes(string[] Ids, string controllerName = null)
        {
            if (string.IsNullOrEmpty(controllerName))
            {
                controllerName = typeof (T).Name;
            }

            HttpResponseMessage responseMsg = 
                await client.PostAsJsonAsync($"{ModelName}/{controllerName}/Deletes/", Ids); 
            return await responseMsg.Content.ReadAsAsync<bool>();
        }

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <param name="controllerName">Controller名称 默认空 </param>
        /// <returns>是否成功</returns>
        public async Task<bool> Delete(string Id, string controllerName = null)
        {
            if (string.IsNullOrEmpty(controllerName))
            {
                controllerName = typeof (T).Name;
            }

            HttpResponseMessage responseMsg = 
                await 
                    client.GetAsync( 
                    $"{ModelName}/{controllerName}/Delete/?Id={Id}"); 
            return await responseMsg.Content.ReadAsAsync<bool>(); 
        }

        #endregion Add、Update、Delete

        #region Search

        /// <summary>
        /// 按照查询条件查询
        /// </summary>
        /// <param name="sWhere">查询条件</param>
        /// <param name="controllerName">Controller名称 默认空 </param>
        /// <returns>是否成功</returns>
        public async Task<List<T>> GetByParam(string sWhere, string controllerName = null)
        {
            if (string.IsNullOrEmpty(controllerName))
            {
                controllerName = typeof (T).Name;
            }

            HttpResponseMessage responseMsg = 
                await 
                    client.PostAsJsonAsync( 
                    $"{ModelName}/{controllerName}/GetByParam/", sWhere); 
            return await responseMsg.Content.ReadAsAsync<List<T>>(); 
        }

        /// <summary>
        /// 按照关键字段查询
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <param name="controllerName">Controller名称 默认空 </param>
        /// <returns>是否成功</returns>
        public async Task<T> GetById(string Id, string controllerName = null)
        {
            if (string.IsNullOrEmpty(controllerName))
            {
                controllerName = typeof (T).Name;
            }

            HttpResponseMessage responseMsg = 
                await 
                    client.GetAsync( 
                    $"{ModelName}/{controllerName}/GetById/?Id={Id}"); 
            return await responseMsg.Content.ReadAsAsync<T>(); 
        }

        #endregion Search

        #region 其他

        #endregion 其他

    }
}
