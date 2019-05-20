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
    /// 工序参数表
    /// </summary>
    [DataContract]
    public class RsRoutingParam
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

        #region 工序编号

        private string _PROCESS_PKNO;
        /// <summary> 
        ///  工序编号
        /// </summary> 
        [DataMember]
        [Display(Name = "PROCESS_PKNO", Description = "工序编号")]
        public string PROCESS_PKNO
        {
            get { return _PROCESS_PKNO; }
            set { _PROCESS_PKNO = value; }
        }

        #endregion 工序编号

        #region 参数代码

        private string _PARAM_CODE;
        /// <summary> 
        ///  参数代码
        /// </summary> 
        [DataMember]
        [Display(Name = "PARAM_CODE", Description = "参数代码")]
        public string PARAM_CODE
        {
            get { return _PARAM_CODE; }
            set { _PARAM_CODE = value; }
        }

        #endregion 参数代码

        #region 参数名称

        private string _PARAM_NAME;
        /// <summary> 
        ///  参数名称
        /// </summary> 
        [DataMember]
        [Display(Name = "PARAM_NAME", Description = "参数名称")]
        public string PARAM_NAME
        {
            get { return _PARAM_NAME; }
            set { _PARAM_NAME = value; }
        }

        #endregion 参数名称

        #region 参数值

        private string _PARAM_VALUE;
        /// <summary> 
        ///  参数值
        /// </summary> 
        [DataMember]
        [Display(Name = "PARAM_VALUE", Description = "参数值")]
        public string PARAM_VALUE
        {
            get { return _PARAM_VALUE; }
            set { _PARAM_VALUE = value; }
        }

        #endregion 参数值

        #region 参数类别

        private string _PARAM_TYPE;
        /// <summary> 
        ///  参数类别
        /// </summary> 
        [DataMember]
        [Display(Name = "PARAM_TYPE", Description = "参数类别")]
        public string PARAM_TYPE
        {
            get { return _PARAM_TYPE; }
            set { _PARAM_TYPE = value; }
        }

        #endregion 参数类别

        #region 参数描述

        private string _PARAM_DESC;
        /// <summary> 
        ///  参数描述
        /// </summary> 
        [DataMember]
        [Display(Name = "PARAM_DESC", Description = "参数描述")]
        public string PARAM_DESC
        {
            get { return _PARAM_DESC; }
            set { _PARAM_DESC = value; }
        }

        #endregion 参数描述

        #region 最小尺寸

        private Decimal _MIN_SIZE;
        /// <summary> 
        ///  最小尺寸
        /// </summary> 
        [DataMember]
        [Display(Name = "MIN_SIZE", Description = "最小尺寸")]
        public Decimal MIN_SIZE
        {
            get { return _MIN_SIZE; }
            set { _MIN_SIZE = value; }
        }

        #endregion 最小尺寸

        #region 最大尺寸

        private Decimal _MAX_SIZE;
        /// <summary> 
        ///  最大尺寸
        /// </summary> 
        [DataMember]
        [Display(Name = "MAX_SIZE", Description = "最大尺寸")]
        public Decimal MAX_SIZE
        {
            get { return _MAX_SIZE; }
            set { _MAX_SIZE = value; }
        }

        #endregion 最大尺寸

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
