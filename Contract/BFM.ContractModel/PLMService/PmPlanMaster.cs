/*******************************************************************************
 * Copyright © 2018 代码生成器 版权所有
 * Author: LanGerp 
 * Description: 快速开发平台
*********************************************************************************/

using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace BFM.ContractModel 
{
    /// <summary>
    /// 生产计划表
    /// </summary>
    [DataContract]
    public class PmPlanMaster
    {

        #region 唯一性字段

        private string _PKNO;
        /// <summary> 
        ///  唯一性字段
        /// </summary> 
        [DataMember]
        [Display(Name = "PKNO", Description = "唯一性字段")]
        public string PKNO
        {
            get { return _PKNO; }
            set { _PKNO = value; }
        }

        #endregion 唯一性字段

        #region 公司代码

        private string _COMPANY_CODE;
        /// <summary> 
        ///  公司代码
        /// </summary> 
        [DataMember]
        [Display(Name = "COMPANY_CODE", Description = "公司代码")]
        public string COMPANY_CODE
        {
            get { return _COMPANY_CODE; }
            set { _COMPANY_CODE = value; }
        }

        #endregion 公司代码

        #region 计划名称

        private string _PLAN_NAME;
        /// <summary> 
        ///  计划名称
        /// </summary> 
        [DataMember]
        [Display(Name = "PLAN_NAME", Description = "计划名称")]
        public string PLAN_NAME
        {
            get { return _PLAN_NAME; }
            set { _PLAN_NAME = value; }
        }

        #endregion 计划名称

        #region 计划类别

        private string _PLAN_TYPE;
        /// <summary> 
        ///  计划类别
        /// </summary> 
        [DataMember]
        [Display(Name = "PLAN_TYPE", Description = "计划类别")]
        public string PLAN_TYPE
        {
            get { return _PLAN_TYPE; }
            set { _PLAN_TYPE = value; }
        }

        #endregion 计划类别

        #region 计划日期

        private DateTime? _PLAN_DATE;
        /// <summary> 
        ///  计划日期
        /// </summary> 
        [DataMember]
        [Display(Name = "PLAN_DATE", Description = "计划日期")]
        public DateTime? PLAN_DATE
        {
            get { return _PLAN_DATE; }
            set { _PLAN_DATE = value; }
        }

        #endregion 计划日期

        #region 项目PKNO

        private string _ITEM_PKNO;
        /// <summary> 
        ///  项目PKNO
        /// </summary> 
        [DataMember]
        [Display(Name = "ITEM_PKNO", Description = "项目PKNO")]
        public string ITEM_PKNO
        {
            get { return _ITEM_PKNO; }
            set { _ITEM_PKNO = value; }
        }

        #endregion 项目PKNO

        #region 计划数量

        private Decimal _PLAN_QTY;
        /// <summary> 
        ///  计划数量
        /// </summary> 
        [DataMember]
        [Display(Name = "PLAN_QTY", Description = "计划数量")]
        public Decimal PLAN_QTY
        {
            get { return _PLAN_QTY; }
            set { _PLAN_QTY = value; }
        }

        #endregion 计划数量

        #region 需求数

        private Decimal _NEED_QTY;
        /// <summary> 
        ///  需求数
        /// </summary> 
        [DataMember]
        [Display(Name = "NEED_QTY", Description = "需求数")]
        public Decimal NEED_QTY
        {
            get { return _NEED_QTY; }
            set { _NEED_QTY = value; }
        }

        #endregion 需求数

        #region 执行状态

        private Int32 _RUN_STATE;
        /// <summary> 
        ///  执行状态
        /// </summary> 
        [DataMember]
        [Display(Name = "RUN_STATE", Description = "执行状态")]
        public Int32 RUN_STATE
        {
            get { return _RUN_STATE; }
            set { _RUN_STATE = value; }
        }

        #endregion 执行状态

        #region 分配数量

        private Decimal _DISPATCH_QTY;
        /// <summary> 
        ///  分配数量
        /// </summary> 
        [DataMember]
        [Display(Name = "DISPATCH_QTY", Description = "分配数量")]
        public Decimal DISPATCH_QTY
        {
            get { return _DISPATCH_QTY; }
            set { _DISPATCH_QTY = value; }
        }

        #endregion 分配数量

        #region 计划开始

        private DateTime? _PLAN_START_TIME;
        /// <summary> 
        ///  计划开始
        /// </summary> 
        [DataMember]
        [Display(Name = "PLAN_START_TIME", Description = "计划开始")]
        public DateTime? PLAN_START_TIME
        {
            get { return _PLAN_START_TIME; }
            set { _PLAN_START_TIME = value; }
        }

        #endregion 计划开始

        #region 计划结束

        private DateTime? _PLAN_END_TIME;
        /// <summary> 
        ///  计划结束
        /// </summary> 
        [DataMember]
        [Display(Name = "PLAN_END_TIME", Description = "计划结束")]
        public DateTime? PLAN_END_TIME
        {
            get { return _PLAN_END_TIME; }
            set { _PLAN_END_TIME = value; }
        }

        #endregion 计划结束

        #region 创建日期

        private DateTime? _CREATION_DATE;
        /// <summary> 
        ///  创建日期
        /// </summary> 
        [DataMember]
        [Display(Name = "CREATION_DATE", Description = "创建日期")]
        public DateTime? CREATION_DATE
        {
            get { return _CREATION_DATE; }
            set { _CREATION_DATE = value; }
        }

        #endregion 创建日期

        #region 创建人

        private string _CREATED_BY;
        /// <summary> 
        ///  创建人
        /// </summary> 
        [DataMember]
        [Display(Name = "CREATED_BY", Description = "创建人")]
        public string CREATED_BY
        {
            get { return _CREATED_BY; }
            set { _CREATED_BY = value; }
        }

        #endregion 创建人

        #region 最后修改日期

        private DateTime? _LAST_UPDATE_DATE;
        /// <summary> 
        ///  最后修改日期
        /// </summary> 
        [DataMember]
        [Display(Name = "LAST_UPDATE_DATE", Description = "最后修改日期")]
        public DateTime? LAST_UPDATE_DATE
        {
            get { return _LAST_UPDATE_DATE; }
            set { _LAST_UPDATE_DATE = value; }
        }

        #endregion 最后修改日期

        #region 更新人

        private string _UPDATED_BY;
        /// <summary> 
        ///  更新人
        /// </summary> 
        [DataMember]
        [Display(Name = "UPDATED_BY", Description = "更新人")]
        public string UPDATED_BY
        {
            get { return _UPDATED_BY; }
            set { _UPDATED_BY = value; }
        }

        #endregion 更新人

        #region 修改说明

        private string _UPDATED_INTROD;
        /// <summary> 
        ///  修改说明
        /// </summary> 
        [DataMember]
        [Display(Name = "UPDATED_INTROD", Description = "修改说明")]
        public string UPDATED_INTROD
        {
            get { return _UPDATED_INTROD; }
            set { _UPDATED_INTROD = value; }
        }

        #endregion 修改说明

        #region 启用标识

        private Int32 _USE_FLAG;
        /// <summary> 
        ///  启用标识
        /// </summary> 
        [DataMember]
        [Display(Name = "USE_FLAG", Description = "启用标识")]
        public Int32 USE_FLAG
        {
            get { return _USE_FLAG; }
            set { _USE_FLAG = value; }
        }

        #endregion 启用标识

        #region 备注

        private string _REMARK;
        /// <summary> 
        ///  备注
        /// </summary> 
        [DataMember]
        [Display(Name = "REMARK", Description = "备注")]
        public string REMARK
        {
            get { return _REMARK; }
            set { _REMARK = value; }
        }

        #endregion 备注
    }
}
