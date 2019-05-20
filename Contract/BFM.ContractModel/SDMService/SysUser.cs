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
    /// 用户信息表
    /// </summary>
    [DataContract]
    public class SysUser
    {

        #region 唯一标识

        private string _PKNO;
        /// <summary> 
        ///  唯一标识
        /// </summary> 
        [DataMember]
        [Display(Name = "PKNO", Description = "唯一标识")]
        public string PKNO
        {
            get { return _PKNO; }
            set { _PKNO = value; }
        }

        #endregion 唯一标识

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

        #region 部门编号

        private string _DEPARTMENT_CODE;
        /// <summary> 
        ///  部门编号
        /// </summary> 
        [DataMember]
        [Display(Name = "DEPARTMENT_CODE", Description = "部门编号")]
        public string DEPARTMENT_CODE
        {
            get { return _DEPARTMENT_CODE; }
            set { _DEPARTMENT_CODE = value; }
        }

        #endregion 部门编号

        #region 工号

        private string _STAFF_NO;
        /// <summary> 
        ///  工号
        /// </summary> 
        [DataMember]
        [Display(Name = "STAFF_NO", Description = "工号")]
        public string STAFF_NO
        {
            get { return _STAFF_NO; }
            set { _STAFF_NO = value; }
        }

        #endregion 工号

        #region 用户姓名

        private string _USER_NAME;
        /// <summary> 
        ///  用户姓名
        /// </summary> 
        [DataMember]
        [Display(Name = "USER_NAME", Description = "用户姓名")]
        public string USER_NAME
        {
            get { return _USER_NAME; }
            set { _USER_NAME = value; }
        }

        #endregion 用户姓名

        #region 密码

        private string _PASSWORD;
        /// <summary> 
        ///  密码
        /// </summary> 
        [DataMember]
        [Display(Name = "PASSWORD", Description = "密码")]
        public string PASSWORD
        {
            get { return _PASSWORD; }
            set { _PASSWORD = value; }
        }

        #endregion 密码

        #region 失效日期

        private DateTime? _EXPIRE_DATE;
        /// <summary> 
        ///  失效日期
        /// </summary> 
        [DataMember]
        [Display(Name = "EXPIRE_DATE", Description = "失效日期")]
        public DateTime? EXPIRE_DATE
        {
            get { return _EXPIRE_DATE; }
            set { _EXPIRE_DATE = value; }
        }

        #endregion 失效日期

        #region 电话号码

        private string _TEL_NO;
        /// <summary> 
        ///  电话号码
        /// </summary> 
        [DataMember]
        [Display(Name = "TEL_NO", Description = "电话号码")]
        public string TEL_NO
        {
            get { return _TEL_NO; }
            set { _TEL_NO = value; }
        }

        #endregion 电话号码

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
    }
}
