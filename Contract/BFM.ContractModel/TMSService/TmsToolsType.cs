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
    /// 刀具类型表
    /// </summary>
    [DataContract]
    public class TmsToolsType
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

        #region 刀具类型编号

        private string _TOOLS_TYPE_CODE;
        /// <summary> 
        ///  刀具类型编号
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOLS_TYPE_CODE", Description = "刀具类型编号")]
        public string TOOLS_TYPE_CODE
        {
            get { return _TOOLS_TYPE_CODE; }
            set { _TOOLS_TYPE_CODE = value; }
        }

        #endregion 刀具类型编号

        #region 刀具类型名称

        private string _TOOLS_TYPE_NAME;
        /// <summary> 
        ///  刀具类型名称
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOLS_TYPE_NAME", Description = "刀具类型名称")]
        public string TOOLS_TYPE_NAME
        {
            get { return _TOOLS_TYPE_NAME; }
            set { _TOOLS_TYPE_NAME = value; }
        }

        #endregion 刀具类型名称

        #region 基本类型

        private string _TOOLS_TYPE_BASIC;
        /// <summary> 
        ///  基本类型
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOLS_TYPE_BASIC", Description = "基本类型")]
        public string TOOLS_TYPE_BASIC
        {
            get { return _TOOLS_TYPE_BASIC; }
            set { _TOOLS_TYPE_BASIC = value; }
        }

        #endregion 基本类型

        #region 指定类型

        private string _TOOLS_TYPE_SPECIFIED;
        /// <summary> 
        ///  指定类型
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOLS_TYPE_SPECIFIED", Description = "指定类型")]
        public string TOOLS_TYPE_SPECIFIED
        {
            get { return _TOOLS_TYPE_SPECIFIED; }
            set { _TOOLS_TYPE_SPECIFIED = value; }
        }

        #endregion 指定类型

        #region 刀具参数

        private string _TOOLS_TYPE_PARAM;
        /// <summary> 
        ///  刀具参数
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOLS_TYPE_PARAM", Description = "刀具参数")]
        public string TOOLS_TYPE_PARAM
        {
            get { return _TOOLS_TYPE_PARAM; }
            set { _TOOLS_TYPE_PARAM = value; }
        }

        #endregion 刀具参数

        #region 辅助说明

        private string _TOOLS_TYPE_ASSIST;
        /// <summary> 
        ///  辅助说明
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOLS_TYPE_ASSIST", Description = "辅助说明")]
        public string TOOLS_TYPE_ASSIST
        {
            get { return _TOOLS_TYPE_ASSIST; }
            set { _TOOLS_TYPE_ASSIST = value; }
        }

        #endregion 辅助说明

        #region 对应的T代码

        private string _TOOLS_TYPE_TCODE;
        /// <summary> 
        ///  对应的T代码
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOLS_TYPE_TCODE", Description = "对应的T代码")]
        public string TOOLS_TYPE_TCODE
        {
            get { return _TOOLS_TYPE_TCODE; }
            set { _TOOLS_TYPE_TCODE = value; }
        }

        #endregion 对应的T代码

        #region 刀具图片

        private byte[] _TOOLS_PIC;
        /// <summary> 
        ///  刀具图片
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOLS_PIC", Description = "刀具图片")]
        public byte[] TOOLS_PIC
        {
            get { return _TOOLS_PIC; }
            set { _TOOLS_PIC = value; }
        }

        #endregion 刀具图片

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
