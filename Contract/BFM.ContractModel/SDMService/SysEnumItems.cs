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
    /// 基础信息明细表
    /// </summary>
    [DataContract]
    public class SysEnumItems
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

        #region 明细编号

        private string _ITEM_NO;
        /// <summary> 
        ///  明细编号
        /// </summary> 
        [DataMember]
        [Display(Name = "ITEM_NO", Description = "明细编号")]
        public string ITEM_NO
        {
            get { return _ITEM_NO; }
            set { _ITEM_NO = value; }
        }

        #endregion 明细编号

        #region 代号

        private string _ITEM_CODE;
        /// <summary> 
        ///  代号
        /// </summary> 
        [DataMember]
        [Display(Name = "ITEM_CODE", Description = "代号")]
        public string ITEM_CODE
        {
            get { return _ITEM_CODE; }
            set { _ITEM_CODE = value; }
        }

        #endregion 代号

        #region 名称

        private string _ITEM_NAME;
        /// <summary> 
        ///  名称
        /// </summary> 
        [DataMember]
        [Display(Name = "ITEM_NAME", Description = "名称")]
        public string ITEM_NAME
        {
            get { return _ITEM_NAME; }
            set { _ITEM_NAME = value; }
        }

        #endregion 名称

        #region 名称拼音首字母

        private string _NAME_PINY;
        /// <summary> 
        ///  名称拼音首字母
        /// </summary> 
        [DataMember]
        [Display(Name = "NAME_PINY", Description = "名称拼音首字母")]
        public string NAME_PINY
        {
            get { return _NAME_PINY; }
            set { _NAME_PINY = value; }
        }

        #endregion 名称拼音首字母

        #region 明细项描述

        private string _ITEM_INTROD;
        /// <summary> 
        ///  明细项描述
        /// </summary> 
        [DataMember]
        [Display(Name = "ITEM_INTROD", Description = "明细项描述")]
        public string ITEM_INTROD
        {
            get { return _ITEM_INTROD; }
            set { _ITEM_INTROD = value; }
        }

        #endregion 明细项描述

        #region 排列顺序

        private Int32 _ITEM_INDEX;
        /// <summary> 
        ///  排列顺序
        /// </summary> 
        [DataMember]
        [Display(Name = "ITEM_INDEX", Description = "排列顺序")]
        public Int32 ITEM_INDEX
        {
            get { return _ITEM_INDEX; }
            set { _ITEM_INDEX = value; }
        }

        #endregion 排列顺序

        #region 明细项类型

        private Int32 _ITEM_TYPE;
        /// <summary> 
        ///  明细项类型
        /// </summary> 
        [DataMember]
        [Display(Name = "ITEM_TYPE", Description = "明细项类型")]
        public Int32 ITEM_TYPE
        {
            get { return _ITEM_TYPE; }
            set { _ITEM_TYPE = value; }
        }

        #endregion 明细项类型

        #region 所属用户

        private string _USER_PKNO;
        /// <summary> 
        ///  所属用户
        /// </summary> 
        [DataMember]
        [Display(Name = "USER_PKNO", Description = "所属用户")]
        public string USER_PKNO
        {
            get { return _USER_PKNO; }
            set { _USER_PKNO = value; }
        }

        #endregion 所属用户

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
