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
    /// 基础信息主表
    /// </summary>
    [DataContract]
    public class SysEnumMain
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

        #region 基础信息标识

        private string _ENUM_IDENTIFY;
        /// <summary> 
        ///  基础信息标识
        /// </summary> 
        [DataMember]
        [Display(Name = "ENUM_IDENTIFY", Description = "基础信息标识")]
        public string ENUM_IDENTIFY
        {
            get { return _ENUM_IDENTIFY; }
            set { _ENUM_IDENTIFY = value; }
        }

        #endregion 基础信息标识

        #region 信息分类

        private string _ENUM_SORT;
        /// <summary> 
        ///  信息分类
        /// </summary> 
        [DataMember]
        [Display(Name = "ENUM_SORT", Description = "信息分类")]
        public string ENUM_SORT
        {
            get { return _ENUM_SORT; }
            set { _ENUM_SORT = value; }
        }

        #endregion 信息分类

        #region 名称

        private string _ENUM_NAME;
        /// <summary> 
        ///  名称
        /// </summary> 
        [DataMember]
        [Display(Name = "ENUM_NAME", Description = "名称")]
        public string ENUM_NAME
        {
            get { return _ENUM_NAME; }
            set { _ENUM_NAME = value; }
        }

        #endregion 名称

        #region 描述

        private string _ENUM_INTROD;
        /// <summary> 
        ///  描述
        /// </summary> 
        [DataMember]
        [Display(Name = "ENUM_INTROD", Description = "描述")]
        public string ENUM_INTROD
        {
            get { return _ENUM_INTROD; }
            set { _ENUM_INTROD = value; }
        }

        #endregion 描述

        #region 类型

        private Int32 _ENUM_TYPE;
        /// <summary> 
        ///  类型
        /// </summary> 
        [DataMember]
        [Display(Name = "ENUM_TYPE", Description = "类型")]
        public Int32 ENUM_TYPE
        {
            get { return _ENUM_TYPE; }
            set { _ENUM_TYPE = value; }
        }

        #endregion 类型

        #region Value值的字段类型

        private Int32 _VALUE_FIELD;
        /// <summary> 
        ///  Value值的字段类型
        /// </summary> 
        [DataMember]
        [Display(Name = "VALUE_FIELD", Description = "Value值的字段类型")]
        public Int32 VALUE_FIELD
        {
            get { return _VALUE_FIELD; }
            set { _VALUE_FIELD = value; }
        }

        #endregion Value值的字段类型

        #region 枚举明细代号意义

        private string _ENUM_CODE_INFO;
        /// <summary> 
        ///  枚举明细代号意义
        /// </summary> 
        [DataMember]
        [Display(Name = "ENUM_CODE_INFO", Description = "枚举明细代号意义")]
        public string ENUM_CODE_INFO
        {
            get { return _ENUM_CODE_INFO; }
            set { _ENUM_CODE_INFO = value; }
        }

        #endregion 枚举明细代号意义

        #region 枚举明细代号描述

        private string _ENUM_CODE_INTROD;
        /// <summary> 
        ///  枚举明细代号描述
        /// </summary> 
        [DataMember]
        [Display(Name = "ENUM_CODE_INTROD", Description = "枚举明细代号描述")]
        public string ENUM_CODE_INTROD
        {
            get { return _ENUM_CODE_INTROD; }
            set { _ENUM_CODE_INTROD = value; }
        }

        #endregion 枚举明细代号描述

        #region 枚举明细代号非空校验

        private Int32 _ENUM_CODE_CHECKNULL;
        /// <summary> 
        ///  枚举明细代号非空校验
        /// </summary> 
        [DataMember]
        [Display(Name = "ENUM_CODE_CHECKNULL", Description = "枚举明细代号非空校验")]
        public Int32 ENUM_CODE_CHECKNULL
        {
            get { return _ENUM_CODE_CHECKNULL; }
            set { _ENUM_CODE_CHECKNULL = value; }
        }

        #endregion 枚举明细代号非空校验

        #region 枚举明细代号唯一校验

        private Int32 _ENUM_CODE_CHECKKEY;
        /// <summary> 
        ///  枚举明细代号唯一校验
        /// </summary> 
        [DataMember]
        [Display(Name = "ENUM_CODE_CHECKKEY", Description = "枚举明细代号唯一校验")]
        public Int32 ENUM_CODE_CHECKKEY
        {
            get { return _ENUM_CODE_CHECKKEY; }
            set { _ENUM_CODE_CHECKKEY = value; }
        }

        #endregion 枚举明细代号唯一校验

        #region 枚举明细代号数字检测

        private Int32 _ENUM_CODE_CHECKNUM;
        /// <summary> 
        ///  枚举明细代号数字检测
        /// </summary> 
        [DataMember]
        [Display(Name = "ENUM_CODE_CHECKNUM", Description = "枚举明细代号数字检测")]
        public Int32 ENUM_CODE_CHECKNUM
        {
            get { return _ENUM_CODE_CHECKNUM; }
            set { _ENUM_CODE_CHECKNUM = value; }
        }

        #endregion 枚举明细代号数字检测

        #region 状态

        private Int32 _ENUM_STATE;
        /// <summary> 
        ///  状态
        /// </summary> 
        [DataMember]
        [Display(Name = "ENUM_STATE", Description = "状态")]
        public Int32 ENUM_STATE
        {
            get { return _ENUM_STATE; }
            set { _ENUM_STATE = value; }
        }

        #endregion 状态

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
