/*******************************************************************************
 * Copyright © 2018 代码生成器 版权所有
 * Author: LanGerp 
 * Description: 快速开发平台
*********************************************************************************/

using System.Collections.Generic;
using System.ServiceModel;
using BFM.ContractModel;

namespace BFM.WCFService
{
    [ServiceContract(Namespace = "http://fanuc.com.cn/")]
    public interface IFMSService
    {

        #region 设备通讯参数配置表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [OperationContract]
        List <FmsAssetCommParam> GetFmsAssetCommParamByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetFmsAssetCommParamCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFmsAssetCommParam">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddFmsAssetCommParam(FmsAssetCommParam mFmsAssetCommParam);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFmsAssetCommParam">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateFmsAssetCommParam(FmsAssetCommParam mFmsAssetCommParam);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelFmsAssetCommParams(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelFmsAssetCommParam(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<FmsAssetCommParam> GetFmsAssetCommParams(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        FmsAssetCommParam GetFmsAssetCommParamById(string Id);

        #endregion 设备通讯参数配置表 的服务接口

        #region 设备通讯标签配置表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [OperationContract]
        List <FmsAssetTagSetting> GetFmsAssetTagSettingByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetFmsAssetTagSettingCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFmsAssetTagSetting">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddFmsAssetTagSetting(FmsAssetTagSetting mFmsAssetTagSetting);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFmsAssetTagSetting">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateFmsAssetTagSetting(FmsAssetTagSetting mFmsAssetTagSetting);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelFmsAssetTagSettings(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelFmsAssetTagSetting(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<FmsAssetTagSetting> GetFmsAssetTagSettings(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        FmsAssetTagSetting GetFmsAssetTagSettingById(string Id);

        #endregion 设备通讯标签配置表 的服务接口

        #region 状态持续结果记录表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [OperationContract]
        List <FmsStateResultRecord> GetFmsStateResultRecordByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetFmsStateResultRecordCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFmsStateResultRecord">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddFmsStateResultRecord(FmsStateResultRecord mFmsStateResultRecord);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFmsStateResultRecord">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateFmsStateResultRecord(FmsStateResultRecord mFmsStateResultRecord);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelFmsStateResultRecords(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelFmsStateResultRecord(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<FmsStateResultRecord> GetFmsStateResultRecords(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        FmsStateResultRecord GetFmsStateResultRecordById(string Id);

        #endregion 状态持续结果记录表 的服务接口

        #region 设备标签采集记录表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [OperationContract]
        List <FmsSamplingRecord> GetFmsSamplingRecordByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetFmsSamplingRecordCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFmsSamplingRecord">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddFmsSamplingRecord(FmsSamplingRecord mFmsSamplingRecord);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFmsSamplingRecord">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateFmsSamplingRecord(FmsSamplingRecord mFmsSamplingRecord);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelFmsSamplingRecords(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelFmsSamplingRecord(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<FmsSamplingRecord> GetFmsSamplingRecords(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        FmsSamplingRecord GetFmsSamplingRecordById(string Id);

        #endregion 设备标签采集记录表 的服务接口

        #region 指令动作控制表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [OperationContract]
        List <FmsActionControl> GetFmsActionControlByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetFmsActionControlCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFmsActionControl">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddFmsActionControl(FmsActionControl mFmsActionControl);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFmsActionControl">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateFmsActionControl(FmsActionControl mFmsActionControl);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelFmsActionControls(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelFmsActionControl(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<FmsActionControl> GetFmsActionControls(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        FmsActionControl GetFmsActionControlById(string Id);

        #endregion 指令动作控制表 的服务接口

        #region 设备标签计算表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [OperationContract]
        List <FmsTagCalculation> GetFmsTagCalculationByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetFmsTagCalculationCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFmsTagCalculation">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddFmsTagCalculation(FmsTagCalculation mFmsTagCalculation);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFmsTagCalculation">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateFmsTagCalculation(FmsTagCalculation mFmsTagCalculation);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelFmsTagCalculations(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelFmsTagCalculation(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<FmsTagCalculation> GetFmsTagCalculations(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        FmsTagCalculation GetFmsTagCalculationById(string Id);

        #endregion 设备标签计算表 的服务接口

        #region 动作配方主表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [OperationContract]
        List <FmsActionFormulaMain> GetFmsActionFormulaMainByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetFmsActionFormulaMainCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFmsActionFormulaMain">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddFmsActionFormulaMain(FmsActionFormulaMain mFmsActionFormulaMain);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFmsActionFormulaMain">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateFmsActionFormulaMain(FmsActionFormulaMain mFmsActionFormulaMain);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelFmsActionFormulaMains(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelFmsActionFormulaMain(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<FmsActionFormulaMain> GetFmsActionFormulaMains(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        FmsActionFormulaMain GetFmsActionFormulaMainById(string Id);

        #endregion 动作配方主表 的服务接口

        #region 动作配方明细表 的服务接口

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize">单页的记录数</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="isAsc">是否正序排序</param>
        /// <param name="orderField">排序字段 为空时默认为 CREATION_DATE </param>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List结果</returns>
        [OperationContract]
        List <FmsActionFormulaDetail> GetFmsActionFormulaDetailByPage(int pageSize, int pageIndex, bool isAsc, string orderField, string sWhere);

        /// <summary>
        /// 返回记录总数 配合分页查询用
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的记录数</returns>
        [OperationContract]
        int GetFmsActionFormulaDetailCount(string sWhere); 

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="mFmsActionFormulaDetail">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool AddFmsActionFormulaDetail(FmsActionFormulaDetail mFmsActionFormulaDetail);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="mFmsActionFormulaDetail">模型</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool UpdateFmsActionFormulaDetail(FmsActionFormulaDetail mFmsActionFormulaDetail);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Ids">关键字段数组</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelFmsActionFormulaDetails(string[] Ids);

        /// <summary>
        /// 按照关键字段删除
        /// </summary>
        /// <param name="Id">关键字段</param>
        /// <returns>是否成功</returns>
        [OperationContract]
        bool DelFmsActionFormulaDetail(string Id);

        /// <summary>
        /// 根据查询条件获取记录
        /// </summary>
        /// <param name="sWhere">查询条件；多条件是目前只支持 and；表达式 System.Linq.Dynamic的形式</param>
        /// <returns>符合查询条件的List</returns>
        [OperationContract]
        List<FmsActionFormulaDetail> GetFmsActionFormulaDetails(string sWhere);

        /// <summary>
        /// 根据关键字段的值获取记录
        /// </summary>
        /// <param name="Id">关键字段的值</param>
        /// <returns>符合查询条件的记录</returns>
        [OperationContract]
        FmsActionFormulaDetail GetFmsActionFormulaDetailById(string Id);

        #endregion 动作配方明细表 的服务接口
    }
}
