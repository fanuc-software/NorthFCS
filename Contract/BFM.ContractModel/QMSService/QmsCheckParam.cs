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
    /// 质量检测参数表
    /// </summary>
    [DataContract]
    public class QmsCheckParam
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

        #region 产品编号

        private string _ITEM_PKNO;
        /// <summary> 
        ///  产品编号
        /// </summary> 
        [DataMember]
        [Display(Name = "ITEM_PKNO", Description = "产品编号")]
        public string ITEM_PKNO
        {
            get { return _ITEM_PKNO; }
            set { _ITEM_PKNO = value; }
        }

        #endregion 产品编号

        #region 检测代码

        private string _CHECK_CODE;
        /// <summary> 
        ///  检测代码
        /// </summary> 
        [DataMember]
        [Display(Name = "CHECK_CODE", Description = "检测代码")]
        public string CHECK_CODE
        {
            get { return _CHECK_CODE; }
            set { _CHECK_CODE = value; }
        }

        #endregion 检测代码

        #region 检测名称

        private string _CHECK_NAME;
        /// <summary> 
        ///  检测名称
        /// </summary> 
        [DataMember]
        [Display(Name = "CHECK_NAME", Description = "检测名称")]
        public string CHECK_NAME
        {
            get { return _CHECK_NAME; }
            set { _CHECK_NAME = value; }
        }

        #endregion 检测名称

        #region 检测值

        private string _CHECK_VALUE;
        /// <summary> 
        ///  检测值
        /// </summary> 
        [DataMember]
        [Display(Name = "CHECK_VALUE", Description = "检测值")]
        public string CHECK_VALUE
        {
            get { return _CHECK_VALUE; }
            set { _CHECK_VALUE = value; }
        }

        #endregion 检测值

        #region 检测类别

        private string _CHECK_TYPE;
        /// <summary> 
        ///  检测类别
        /// </summary> 
        [DataMember]
        [Display(Name = "CHECK_TYPE", Description = "检测类别")]
        public string CHECK_TYPE
        {
            get { return _CHECK_TYPE; }
            set { _CHECK_TYPE = value; }
        }

        #endregion 检测类别

        #region 检测设备

        private string _CHECK_DEVICE;
        /// <summary> 
        ///  检测设备
        /// </summary> 
        [DataMember]
        [Display(Name = "CHECK_DEVICE", Description = "检测设备")]
        public string CHECK_DEVICE
        {
            get { return _CHECK_DEVICE; }
            set { _CHECK_DEVICE = value; }
        }

        #endregion 检测设备

        #region 检测设备图片

        private string _CHECK_DEVICE_PIC;
        /// <summary> 
        ///  检测设备图片
        /// </summary> 
        [DataMember]
        [Display(Name = "CHECK_DEVICE_PIC", Description = "检测设备图片")]
        public string CHECK_DEVICE_PIC
        {
            get { return _CHECK_DEVICE_PIC; }
            set { _CHECK_DEVICE_PIC = value; }
        }

        #endregion 检测设备图片

        #region 参数描述

        private string _CHECK_DESC;
        /// <summary> 
        ///  参数描述
        /// </summary> 
        [DataMember]
        [Display(Name = "CHECK_DESC", Description = "参数描述")]
        public string CHECK_DESC
        {
            get { return _CHECK_DESC; }
            set { _CHECK_DESC = value; }
        }

        #endregion 参数描述

        #region 工差下限

        private Decimal? _MIN_SIZE;
        /// <summary> 
        ///  工差下限
        /// </summary> 
        [DataMember]
        [Display(Name = "MIN_SIZE", Description = "工差下限")]
        public Decimal? MIN_SIZE
        {
            get { return _MIN_SIZE; }
            set { _MIN_SIZE = value; }
        }

        #endregion 工差下限

        #region 工差上限

        private Decimal? _MAX_SIZE;
        /// <summary> 
        ///  工差上限
        /// </summary> 
        [DataMember]
        [Display(Name = "MAX_SIZE", Description = "工差上限")]
        public Decimal? MAX_SIZE
        {
            get { return _MAX_SIZE; }
            set { _MAX_SIZE = value; }
        }

        #endregion 工差上限

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
