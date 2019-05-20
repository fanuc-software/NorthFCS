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
    /// 设备台账表N
    /// </summary>
    [DataContract]
    public class AmAssetMasterN
    {

        #region 资产编号

        private string _ASSET_CODE;
        /// <summary> 
        ///  资产编号
        /// </summary> 
        [DataMember]
        [Display(Name = "ASSET_CODE", Description = "资产编号")]
        public string ASSET_CODE
        {
            get { return _ASSET_CODE; }
            set { _ASSET_CODE = value; }
        }

        #endregion 资产编号

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

        #region 父项代码

        private string _PARENT_CODE;
        /// <summary> 
        ///  父项代码
        /// </summary> 
        [DataMember]
        [Display(Name = "PARENT_CODE", Description = "父项代码")]
        public string PARENT_CODE
        {
            get { return _PARENT_CODE; }
            set { _PARENT_CODE = value; }
        }

        #endregion 父项代码

        #region 父项名称

        private string _PARENT_NAME;
        /// <summary> 
        ///  父项名称
        /// </summary> 
        [DataMember]
        [Display(Name = "PARENT_NAME", Description = "父项名称")]
        public string PARENT_NAME
        {
            get { return _PARENT_NAME; }
            set { _PARENT_NAME = value; }
        }

        #endregion 父项名称

        #region 父项类型

        private string _PARENT_TYPE;
        /// <summary> 
        ///  父项类型
        /// </summary> 
        [DataMember]
        [Display(Name = "PARENT_TYPE", Description = "父项类型")]
        public string PARENT_TYPE
        {
            get { return _PARENT_TYPE; }
            set { _PARENT_TYPE = value; }
        }

        #endregion 父项类型

        #region 原资产编号

        private string _OLD_ASSET_CODE;
        /// <summary> 
        ///  原资产编号
        /// </summary> 
        [DataMember]
        [Display(Name = "OLD_ASSET_CODE", Description = "原资产编号")]
        public string OLD_ASSET_CODE
        {
            get { return _OLD_ASSET_CODE; }
            set { _OLD_ASSET_CODE = value; }
        }

        #endregion 原资产编号

        #region 资产名称

        private string _ASSET_NAME;
        /// <summary> 
        ///  资产名称
        /// </summary> 
        [DataMember]
        [Display(Name = "ASSET_NAME", Description = "资产名称")]
        public string ASSET_NAME
        {
            get { return _ASSET_NAME; }
            set { _ASSET_NAME = value; }
        }

        #endregion 资产名称

        #region 资产标签

        private string _ASSET_LABEL;
        /// <summary> 
        ///  资产标签
        /// </summary> 
        [DataMember]
        [Display(Name = "ASSET_LABEL", Description = "资产标签")]
        public string ASSET_LABEL
        {
            get { return _ASSET_LABEL; }
            set { _ASSET_LABEL = value; }
        }

        #endregion 资产标签

        #region 资产类别

        private string _ASSET_TYPE;
        /// <summary> 
        ///  资产类别
        /// </summary> 
        [DataMember]
        [Display(Name = "ASSET_TYPE", Description = "资产类别")]
        public string ASSET_TYPE
        {
            get { return _ASSET_TYPE; }
            set { _ASSET_TYPE = value; }
        }

        #endregion 资产类别

        #region 资产小类

        private string _ASSET_CLASS;
        /// <summary> 
        ///  资产小类
        /// </summary> 
        [DataMember]
        [Display(Name = "ASSET_CLASS", Description = "资产小类")]
        public string ASSET_CLASS
        {
            get { return _ASSET_CLASS; }
            set { _ASSET_CLASS = value; }
        }

        #endregion 资产小类

        #region 规格

        private string _ASSET_NORM;
        /// <summary> 
        ///  规格
        /// </summary> 
        [DataMember]
        [Display(Name = "ASSET_NORM", Description = "规格")]
        public string ASSET_NORM
        {
            get { return _ASSET_NORM; }
            set { _ASSET_NORM = value; }
        }

        #endregion 规格

        #region ABC类

        private string _ABC_TYPE;
        /// <summary> 
        ///  ABC类
        /// </summary> 
        [DataMember]
        [Display(Name = "ABC_TYPE", Description = "ABC类")]
        public string ABC_TYPE
        {
            get { return _ABC_TYPE; }
            set { _ABC_TYPE = value; }
        }

        #endregion ABC类

        #region 部门编码

        private string _DEPARTMENT_CODE;
        /// <summary> 
        ///  部门编码
        /// </summary> 
        [DataMember]
        [Display(Name = "DEPARTMENT_CODE", Description = "部门编码")]
        public string DEPARTMENT_CODE
        {
            get { return _DEPARTMENT_CODE; }
            set { _DEPARTMENT_CODE = value; }
        }

        #endregion 部门编码

        #region 部门名称

        private string _DEPARTMENT_NAME;
        /// <summary> 
        ///  部门名称
        /// </summary> 
        [DataMember]
        [Display(Name = "DEPARTMENT_NAME", Description = "部门名称")]
        public string DEPARTMENT_NAME
        {
            get { return _DEPARTMENT_NAME; }
            set { _DEPARTMENT_NAME = value; }
        }

        #endregion 部门名称

        #region 使用部门编号

        private string _USE_DEPT_CODE;
        /// <summary> 
        ///  使用部门编号
        /// </summary> 
        [DataMember]
        [Display(Name = "USE_DEPT_CODE", Description = "使用部门编号")]
        public string USE_DEPT_CODE
        {
            get { return _USE_DEPT_CODE; }
            set { _USE_DEPT_CODE = value; }
        }

        #endregion 使用部门编号

        #region 使用部门名称

        private string _USE_DEPT_NAME;
        /// <summary> 
        ///  使用部门名称
        /// </summary> 
        [DataMember]
        [Display(Name = "USE_DEPT_NAME", Description = "使用部门名称")]
        public string USE_DEPT_NAME
        {
            get { return _USE_DEPT_NAME; }
            set { _USE_DEPT_NAME = value; }
        }

        #endregion 使用部门名称

        #region 资产组

        private string _ASSET_GROUP;
        /// <summary> 
        ///  资产组
        /// </summary> 
        [DataMember]
        [Display(Name = "ASSET_GROUP", Description = "资产组")]
        public string ASSET_GROUP
        {
            get { return _ASSET_GROUP; }
            set { _ASSET_GROUP = value; }
        }

        #endregion 资产组

        #region 制造厂商

        private string _MANUCATURER;
        /// <summary> 
        ///  制造厂商
        /// </summary> 
        [DataMember]
        [Display(Name = "MANUCATURER", Description = "制造厂商")]
        public string MANUCATURER
        {
            get { return _MANUCATURER; }
            set { _MANUCATURER = value; }
        }

        #endregion 制造厂商

        #region 制造日期

        private DateTime? _MANUCATURE_DATE;
        /// <summary> 
        ///  制造日期
        /// </summary> 
        [DataMember]
        [Display(Name = "MANUCATURE_DATE", Description = "制造日期")]
        public DateTime? MANUCATURE_DATE
        {
            get { return _MANUCATURE_DATE; }
            set { _MANUCATURE_DATE = value; }
        }

        #endregion 制造日期

        #region 出厂年份

        private DateTime? _OUT_DATE;
        /// <summary> 
        ///  出厂年份
        /// </summary> 
        [DataMember]
        [Display(Name = "OUT_DATE", Description = "出厂年份")]
        public DateTime? OUT_DATE
        {
            get { return _OUT_DATE; }
            set { _OUT_DATE = value; }
        }

        #endregion 出厂年份

        #region 安装日期

        private DateTime? _INSTALL_DATE;
        /// <summary> 
        ///  安装日期
        /// </summary> 
        [DataMember]
        [Display(Name = "INSTALL_DATE", Description = "安装日期")]
        public DateTime? INSTALL_DATE
        {
            get { return _INSTALL_DATE; }
            set { _INSTALL_DATE = value; }
        }

        #endregion 安装日期

        #region 启用日期

        private DateTime? _START_USE_DATE;
        /// <summary> 
        ///  启用日期
        /// </summary> 
        [DataMember]
        [Display(Name = "START_USE_DATE", Description = "启用日期")]
        public DateTime? START_USE_DATE
        {
            get { return _START_USE_DATE; }
            set { _START_USE_DATE = value; }
        }

        #endregion 启用日期

        #region 资产图片

        private string _ASSET_PIC;
        /// <summary> 
        ///  资产图片
        /// </summary> 
        [DataMember]
        [Display(Name = "ASSET_PIC", Description = "资产图片")]
        public string ASSET_PIC
        {
            get { return _ASSET_PIC; }
            set { _ASSET_PIC = value; }
        }

        #endregion 资产图片

        #region 出厂编号

        private string _INITIAL_NO;
        /// <summary> 
        ///  出厂编号
        /// </summary> 
        [DataMember]
        [Display(Name = "INITIAL_NO", Description = "出厂编号")]
        public string INITIAL_NO
        {
            get { return _INITIAL_NO; }
            set { _INITIAL_NO = value; }
        }

        #endregion 出厂编号

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
