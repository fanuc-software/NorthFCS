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
    /// 生产线任务表
    /// </summary>
    [DataContract]
    public class PmTaskLine
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

        #region 任务PKNO

        private string _TASK_PKNO;
        /// <summary> 
        ///  任务PKNO
        /// </summary> 
        [DataMember]
        [Display(Name = "TASK_PKNO", Description = "任务PKNO")]
        public string TASK_PKNO
        {
            get { return _TASK_PKNO; }
            set { _TASK_PKNO = value; }
        }

        #endregion 任务PKNO

        #region 产线PKNO

        private string _LINE_PKNO;
        /// <summary> 
        ///  产线PKNO
        /// </summary> 
        [DataMember]
        [Display(Name = "LINE_PKNO", Description = "产线PKNO")]
        public string LINE_PKNO
        {
            get { return _LINE_PKNO; }
            set { _LINE_PKNO = value; }
        }

        #endregion 产线PKNO

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

        #region 子任务编号

        private string _TASK_NO;
        /// <summary> 
        ///  子任务编号
        /// </summary> 
        [DataMember]
        [Display(Name = "TASK_NO", Description = "子任务编号")]
        public string TASK_NO
        {
            get { return _TASK_NO; }
            set { _TASK_NO = value; }
        }

        #endregion 子任务编号

        #region 预计开始

        private DateTime? _TASK_START_TIME;
        /// <summary> 
        ///  预计开始
        /// </summary> 
        [DataMember]
        [Display(Name = "TASK_START_TIME", Description = "预计开始")]
        public DateTime? TASK_START_TIME
        {
            get { return _TASK_START_TIME; }
            set { _TASK_START_TIME = value; }
        }

        #endregion 预计开始

        #region 预计完成

        private DateTime? _TASK_FINISH_TIME;
        /// <summary> 
        ///  预计完成
        /// </summary> 
        [DataMember]
        [Display(Name = "TASK_FINISH_TIME", Description = "预计完成")]
        public DateTime? TASK_FINISH_TIME
        {
            get { return _TASK_FINISH_TIME; }
            set { _TASK_FINISH_TIME = value; }
        }

        #endregion 预计完成

        #region 实际开始

        private DateTime? _ACT_START_TIME;
        /// <summary> 
        ///  实际开始
        /// </summary> 
        [DataMember]
        [Display(Name = "ACT_START_TIME", Description = "实际开始")]
        public DateTime? ACT_START_TIME
        {
            get { return _ACT_START_TIME; }
            set { _ACT_START_TIME = value; }
        }

        #endregion 实际开始

        #region 实际完成

        private DateTime? _ACT_FINISH_TIME;
        /// <summary> 
        ///  实际完成
        /// </summary> 
        [DataMember]
        [Display(Name = "ACT_FINISH_TIME", Description = "实际完成")]
        public DateTime? ACT_FINISH_TIME
        {
            get { return _ACT_FINISH_TIME; }
            set { _ACT_FINISH_TIME = value; }
        }

        #endregion 实际完成

        #region 任务数量

        private Decimal _TASK_QTY;
        /// <summary> 
        ///  任务数量
        /// </summary> 
        [DataMember]
        [Display(Name = "TASK_QTY", Description = "任务数量")]
        public Decimal TASK_QTY
        {
            get { return _TASK_QTY; }
            set { _TASK_QTY = value; }
        }

        #endregion 任务数量

        #region 生产准备完成数量

        private Decimal _PREPARED_QTY;
        /// <summary> 
        ///  生产准备完成数量
        /// </summary> 
        [DataMember]
        [Display(Name = "PREPARED_QTY", Description = "生产准备完成数量")]
        public Decimal PREPARED_QTY
        {
            get { return _PREPARED_QTY; }
            set { _PREPARED_QTY = value; }
        }

        #endregion 生产准备完成数量

        #region 完成数量

        private Decimal _COMPLETE_QTY;
        /// <summary> 
        ///  完成数量
        /// </summary> 
        [DataMember]
        [Display(Name = "COMPLETE_QTY", Description = "完成数量")]
        public Decimal COMPLETE_QTY
        {
            get { return _COMPLETE_QTY; }
            set { _COMPLETE_QTY = value; }
        }

        #endregion 完成数量

        #region 在线数量

        private Decimal _ONLINE_QTY;
        /// <summary> 
        ///  在线数量
        /// </summary> 
        [DataMember]
        [Display(Name = "ONLINE_QTY", Description = "在线数量")]
        public Decimal ONLINE_QTY
        {
            get { return _ONLINE_QTY; }
            set { _ONLINE_QTY = value; }
        }

        #endregion 在线数量

        #region 一次完成数量

        private Decimal _ONCE_QTY;
        /// <summary> 
        ///  一次完成数量
        /// </summary> 
        [DataMember]
        [Display(Name = "ONCE_QTY", Description = "一次完成数量")]
        public Decimal ONCE_QTY
        {
            get { return _ONCE_QTY; }
            set { _ONCE_QTY = value; }
        }

        #endregion 一次完成数量

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
