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
    /// 设备维修记录表
    /// </summary>
    [DataContract]
    public class RmRepairRecord
    {

        #region 记录编号

        private string _PKNO;
        /// <summary> 
        ///  记录编号
        /// </summary> 
        [DataMember]
        [Display(Name = "PKNO", Description = "记录编号")]
        public string PKNO
        {
            get { return _PKNO; }
            set { _PKNO = value; }
        }

        #endregion 记录编号

        #region 设备编号

        private string _ASSET_CODE;
        /// <summary> 
        ///  设备编号
        /// </summary> 
        [DataMember]
        [Display(Name = "ASSET_CODE", Description = "设备编号")]
        public string ASSET_CODE
        {
            get { return _ASSET_CODE; }
            set { _ASSET_CODE = value; }
        }

        #endregion 设备编号

        #region 故障代号

        private string _FAULT_CODE;
        /// <summary> 
        ///  故障代号
        /// </summary> 
        [DataMember]
        [Display(Name = "FAULT_CODE", Description = "故障代号")]
        public string FAULT_CODE
        {
            get { return _FAULT_CODE; }
            set { _FAULT_CODE = value; }
        }

        #endregion 故障代号

        #region 故障名称

        private string _FAULT_NAME;
        /// <summary> 
        ///  故障名称
        /// </summary> 
        [DataMember]
        [Display(Name = "FAULT_NAME", Description = "故障名称")]
        public string FAULT_NAME
        {
            get { return _FAULT_NAME; }
            set { _FAULT_NAME = value; }
        }

        #endregion 故障名称

        #region 故障分类

        private string _FAULT_SORT;
        /// <summary> 
        ///  故障分类
        /// </summary> 
        [DataMember]
        [Display(Name = "FAULT_SORT", Description = "故障分类")]
        public string FAULT_SORT
        {
            get { return _FAULT_SORT; }
            set { _FAULT_SORT = value; }
        }

        #endregion 故障分类

        #region 故障内容

        private string _FAULT_CONTENT;
        /// <summary> 
        ///  故障内容
        /// </summary> 
        [DataMember]
        [Display(Name = "FAULT_CONTENT", Description = "故障内容")]
        public string FAULT_CONTENT
        {
            get { return _FAULT_CONTENT; }
            set { _FAULT_CONTENT = value; }
        }

        #endregion 故障内容

        #region 故障发生时间

        private string _FAULT_OCCURRENCE_TIME;
        /// <summary> 
        ///  故障发生时间
        /// </summary> 
        [DataMember]
        [Display(Name = "FAULT_OCCURRENCE_TIME", Description = "故障发生时间")]
        public string FAULT_OCCURRENCE_TIME
        {
            get { return _FAULT_OCCURRENCE_TIME; }
            set { _FAULT_OCCURRENCE_TIME = value; }
        }

        #endregion 故障发生时间

        #region 故障保修人

        private string _FAULT_WARRANTY;
        /// <summary> 
        ///  故障保修人
        /// </summary> 
        [DataMember]
        [Display(Name = "FAULT_WARRANTY", Description = "故障保修人")]
        public string FAULT_WARRANTY
        {
            get { return _FAULT_WARRANTY; }
            set { _FAULT_WARRANTY = value; }
        }

        #endregion 故障保修人

        #region 故障修复人

        private string _FAULT_REPAIRER;
        /// <summary> 
        ///  故障修复人
        /// </summary> 
        [DataMember]
        [Display(Name = "FAULT_REPAIRER", Description = "故障修复人")]
        public string FAULT_REPAIRER
        {
            get { return _FAULT_REPAIRER; }
            set { _FAULT_REPAIRER = value; }
        }

        #endregion 故障修复人

        #region 故障修复时间

        private DateTime? _FAULT_REPAIR_TIME;
        /// <summary> 
        ///  故障修复时间
        /// </summary> 
        [DataMember]
        [Display(Name = "FAULT_REPAIR_TIME", Description = "故障修复时间")]
        public DateTime? FAULT_REPAIR_TIME
        {
            get { return _FAULT_REPAIR_TIME; }
            set { _FAULT_REPAIR_TIME = value; }
        }

        #endregion 故障修复时间

        #region 故障修复过程描述

        private string _FAULT_REPAIR_CONTENT;
        /// <summary> 
        ///  故障修复过程描述
        /// </summary> 
        [DataMember]
        [Display(Name = "FAULT_REPAIR_CONTENT", Description = "故障修复过程描述")]
        public string FAULT_REPAIR_CONTENT
        {
            get { return _FAULT_REPAIR_CONTENT; }
            set { _FAULT_REPAIR_CONTENT = value; }
        }

        #endregion 故障修复过程描述

        #region 故障修复结果

        private string _FAULT_REPAIR_RESULT;
        /// <summary> 
        ///  故障修复结果
        /// </summary> 
        [DataMember]
        [Display(Name = "FAULT_REPAIR_RESULT", Description = "故障修复结果")]
        public string FAULT_REPAIR_RESULT
        {
            get { return _FAULT_REPAIR_RESULT; }
            set { _FAULT_REPAIR_RESULT = value; }
        }

        #endregion 故障修复结果

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
