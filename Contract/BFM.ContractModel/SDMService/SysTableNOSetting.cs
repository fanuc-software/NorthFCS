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
    /// 系统表格编号设置表
    /// </summary>
    [DataContract]
    public class SysTableNOSetting
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

        #region 表格编号代码

        private string _IDENTIFY_CODE;
        /// <summary> 
        ///  表格编号代码
        /// </summary> 
        [DataMember]
        [Display(Name = "IDENTIFY_CODE", Description = "表格编号代码")]
        public string IDENTIFY_CODE
        {
            get { return _IDENTIFY_CODE; }
            set { _IDENTIFY_CODE = value; }
        }

        #endregion 表格编号代码

        #region 表名

        private string _TABLE_NAME;
        /// <summary> 
        ///  表名
        /// </summary> 
        [DataMember]
        [Display(Name = "TABLE_NAME", Description = "表名")]
        public string TABLE_NAME
        {
            get { return _TABLE_NAME; }
            set { _TABLE_NAME = value; }
        }

        #endregion 表名

        #region 表格名称描述

        private string _TABLE_INTROD;
        /// <summary> 
        ///  表格名称描述
        /// </summary> 
        [DataMember]
        [Display(Name = "TABLE_INTROD", Description = "表格名称描述")]
        public string TABLE_INTROD
        {
            get { return _TABLE_INTROD; }
            set { _TABLE_INTROD = value; }
        }

        #endregion 表格名称描述

        #region 字段名称

        private string _FIELD_NAME;
        /// <summary> 
        ///  字段名称
        /// </summary> 
        [DataMember]
        [Display(Name = "FIELD_NAME", Description = "字段名称")]
        public string FIELD_NAME
        {
            get { return _FIELD_NAME; }
            set { _FIELD_NAME = value; }
        }

        #endregion 字段名称

        #region 字段最大长度

        private Int32 _MAX_LENGTH;
        /// <summary> 
        ///  字段最大长度
        /// </summary> 
        [DataMember]
        [Display(Name = "MAX_LENGTH", Description = "字段最大长度")]
        public Int32 MAX_LENGTH
        {
            get { return _MAX_LENGTH; }
            set { _MAX_LENGTH = value; }
        }

        #endregion 字段最大长度

        #region 前缀符

        private string _PREFIX_STR;
        /// <summary> 
        ///  前缀符
        /// </summary> 
        [DataMember]
        [Display(Name = "PREFIX_STR", Description = "前缀符")]
        public string PREFIX_STR
        {
            get { return _PREFIX_STR; }
            set { _PREFIX_STR = value; }
        }

        #endregion 前缀符

        #region 前缀符之后的日期格式

        private string _DATE_FORMATE;
        /// <summary> 
        ///  前缀符之后的日期格式
        /// </summary> 
        [DataMember]
        [Display(Name = "DATE_FORMATE", Description = "前缀符之后的日期格式")]
        public string DATE_FORMATE
        {
            get { return _DATE_FORMATE; }
            set { _DATE_FORMATE = value; }
        }

        #endregion 前缀符之后的日期格式

        #region 后缀符

        private string _POSTFIX_STR;
        /// <summary> 
        ///  后缀符
        /// </summary> 
        [DataMember]
        [Display(Name = "POSTFIX_STR", Description = "后缀符")]
        public string POSTFIX_STR
        {
            get { return _POSTFIX_STR; }
            set { _POSTFIX_STR = value; }
        }

        #endregion 后缀符

        #region 初始顺序号

        private string _FIRST_NO;
        /// <summary> 
        ///  初始顺序号
        /// </summary> 
        [DataMember]
        [Display(Name = "FIRST_NO", Description = "初始顺序号")]
        public string FIRST_NO
        {
            get { return _FIRST_NO; }
            set { _FIRST_NO = value; }
        }

        #endregion 初始顺序号

        #region 当前顺序号

        private string _CUR_NO;
        /// <summary> 
        ///  当前顺序号
        /// </summary> 
        [DataMember]
        [Display(Name = "CUR_NO", Description = "当前顺序号")]
        public string CUR_NO
        {
            get { return _CUR_NO; }
            set { _CUR_NO = value; }
        }

        #endregion 当前顺序号

        #region 代号描述

        private string _NO_INTROD;
        /// <summary> 
        ///  代号描述
        /// </summary> 
        [DataMember]
        [Display(Name = "NO_INTROD", Description = "代号描述")]
        public string NO_INTROD
        {
            get { return _NO_INTROD; }
            set { _NO_INTROD = value; }
        }

        #endregion 代号描述

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
