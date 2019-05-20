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
    /// 部门信息表
    /// </summary>
    [DataContract]
    public class SysDepartment
    {

        #region 唯一编码

        private string _PKNO;
        /// <summary> 
        ///  唯一编码
        /// </summary> 
        [DataMember]
        [Display(Name = "PKNO", Description = "唯一编码")]
        public string PKNO
        {
            get { return _PKNO; }
            set { _PKNO = value; }
        }

        #endregion 唯一编码

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

        #region 部门代码

        private string _DEPARTMENT_NO;
        /// <summary> 
        ///  部门代码
        /// </summary> 
        [DataMember]
        [Display(Name = "DEPARTMENT_NO", Description = "部门代码")]
        public string DEPARTMENT_NO
        {
            get { return _DEPARTMENT_NO; }
            set { _DEPARTMENT_NO = value; }
        }

        #endregion 部门代码

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

        #region 父部门

        private string _PARENT_DEPARTMENT_PKNO;
        /// <summary> 
        ///  父部门
        /// </summary> 
        [DataMember]
        [Display(Name = "PARENT_DEPARTMENT_PKNO", Description = "父部门")]
        public string PARENT_DEPARTMENT_PKNO
        {
            get { return _PARENT_DEPARTMENT_PKNO; }
            set { _PARENT_DEPARTMENT_PKNO = value; }
        }

        #endregion 父部门

        #region 部门路径

        private string _DEPARTMENT_PATH;
        /// <summary> 
        ///  部门路径
        /// </summary> 
        [DataMember]
        [Display(Name = "DEPARTMENT_PATH", Description = "部门路径")]
        public string DEPARTMENT_PATH
        {
            get { return _DEPARTMENT_PATH; }
            set { _DEPARTMENT_PATH = value; }
        }

        #endregion 部门路径

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

        #region 创建时间

        private DateTime? _CREATION_DATE;
        /// <summary> 
        ///  创建时间
        /// </summary> 
        [DataMember]
        [Display(Name = "CREATION_DATE", Description = "创建时间")]
        public DateTime? CREATION_DATE
        {
            get { return _CREATION_DATE; }
            set { _CREATION_DATE = value; }
        }

        #endregion 创建时间

        #region 版本类型

        private Int32? _VERSION_TYPE;
        /// <summary> 
        ///  版本类型
        /// </summary> 
        [DataMember]
        [Display(Name = "VERSION_TYPE", Description = "版本类型")]
        public Int32? VERSION_TYPE
        {
            get { return _VERSION_TYPE; }
            set { _VERSION_TYPE = value; }
        }

        #endregion 版本类型

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
