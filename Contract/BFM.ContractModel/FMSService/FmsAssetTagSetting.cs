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
    /// 设备通讯标签配置表
    /// </summary>
    [DataContract]
    public class FmsAssetTagSetting
    {

        #region 唯一编号

        private string _PKNO;
        /// <summary> 
        ///  唯一编号
        /// </summary> 
        [DataMember]
        [Display(Name = "PKNO", Description = "唯一编号")]
        public string PKNO
        {
            get { return _PKNO; }
            set { _PKNO = value; }
        }

        #endregion 唯一编号

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

        #region 标签名称

        private string _TAG_NAME;
        /// <summary> 
        ///  标签名称
        /// </summary> 
        [DataMember]
        [Display(Name = "TAG_NAME", Description = "标签名称")]
        public string TAG_NAME
        {
            get { return _TAG_NAME; }
            set { _TAG_NAME = value; }
        }

        #endregion 标签名称

        #region 标签编码

        private string _TAG_CODE;
        /// <summary> 
        ///  标签编码
        /// </summary> 
        [DataMember]
        [Display(Name = "TAG_CODE", Description = "标签编码")]
        public string TAG_CODE
        {
            get { return _TAG_CODE; }
            set { _TAG_CODE = value; }
        }

        #endregion 标签编码

        #region 标签值标识

        private Int32? _STATE_MARK_TYPE;
        /// <summary> 
        ///  标签值标识
        /// </summary> 
        [DataMember]
        [Display(Name = "STATE_MARK_TYPE", Description = "标签值标识")]
        public Int32? STATE_MARK_TYPE
        {
            get { return _STATE_MARK_TYPE; }
            set { _STATE_MARK_TYPE = value; }
        }

        #endregion 标签值标识

        #region 标签值名称

        private string _TAG_VALUE_NAME;
        /// <summary> 
        ///  标签值名称
        /// </summary> 
        [DataMember]
        [Display(Name = "TAG_VALUE_NAME", Description = "标签值名称")]
        public string TAG_VALUE_NAME
        {
            get { return _TAG_VALUE_NAME; }
            set { _TAG_VALUE_NAME = value; }
        }

        #endregion 标签值名称

        #region 标签地址

        private string _TAG_ADDRESS;
        /// <summary> 
        ///  标签地址
        /// </summary> 
        [DataMember]
        [Display(Name = "TAG_ADDRESS", Description = "标签地址")]
        public string TAG_ADDRESS
        {
            get { return _TAG_ADDRESS; }
            set { _TAG_ADDRESS = value; }
        }

        #endregion 标签地址

        #region 数值类型

        private Int32? _VALUE_TYPE;
        /// <summary> 
        ///  数值类型
        /// </summary> 
        [DataMember]
        [Display(Name = "VALUE_TYPE", Description = "数值类型")]
        public Int32? VALUE_TYPE
        {
            get { return _VALUE_TYPE; }
            set { _VALUE_TYPE = value; }
        }

        #endregion 数值类型

        #region 数值单位

        private string _VALUE_UNIT;
        /// <summary> 
        ///  数值单位
        /// </summary> 
        [DataMember]
        [Display(Name = "VALUE_UNIT", Description = "数值单位")]
        public string VALUE_UNIT
        {
            get { return _VALUE_UNIT; }
            set { _VALUE_UNIT = value; }
        }

        #endregion 数值单位

        #region 数值描述

        private string _VALUE_INTROD;
        /// <summary> 
        ///  数值描述
        /// </summary> 
        [DataMember]
        [Display(Name = "VALUE_INTROD", Description = "数值描述")]
        public string VALUE_INTROD
        {
            get { return _VALUE_INTROD; }
            set { _VALUE_INTROD = value; }
        }

        #endregion 数值描述

        #region 数值采样模式

        private Int32? _SAMPLING_MODE;
        /// <summary> 
        ///  数值采样模式
        /// </summary> 
        [DataMember]
        [Display(Name = "SAMPLING_MODE", Description = "数值采样模式")]
        public Int32? SAMPLING_MODE
        {
            get { return _SAMPLING_MODE; }
            set { _SAMPLING_MODE = value; }
        }

        #endregion 数值采样模式

        #region 数值记录方式

        private Int32? _RECORD_TYPE;
        /// <summary> 
        ///  数值记录方式
        /// </summary> 
        [DataMember]
        [Display(Name = "RECORD_TYPE", Description = "数值记录方式")]
        public Int32? RECORD_TYPE
        {
            get { return _RECORD_TYPE; }
            set { _RECORD_TYPE = value; }
        }

        #endregion 数值记录方式

        #region 当前值

        private string _CUR_VALUE;
        /// <summary> 
        ///  当前值
        /// </summary> 
        [DataMember]
        [Display(Name = "CUR_VALUE", Description = "当前值")]
        public string CUR_VALUE
        {
            get { return _CUR_VALUE; }
            set { _CUR_VALUE = value; }
        }

        #endregion 当前值

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

        private Int32? _USE_FLAG;
        /// <summary> 
        ///  启用标识
        /// </summary> 
        [DataMember]
        [Display(Name = "USE_FLAG", Description = "启用标识")]
        public Int32? USE_FLAG
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
