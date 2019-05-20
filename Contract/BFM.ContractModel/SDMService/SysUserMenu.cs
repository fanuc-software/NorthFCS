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
    /// 用户菜单表
    /// </summary>
    [DataContract]
    public class SysUserMenu
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

        #region 菜单编号

        private string _MENU_ITEM_PKNO;
        /// <summary> 
        ///  菜单编号
        /// </summary> 
        [DataMember]
        [Display(Name = "MENU_ITEM_PKNO", Description = "菜单编号")]
        public string MENU_ITEM_PKNO
        {
            get { return _MENU_ITEM_PKNO; }
            set { _MENU_ITEM_PKNO = value; }
        }

        #endregion 菜单编号

        #region 用户编号

        private string _USER_PKNO;
        /// <summary> 
        ///  用户编号
        /// </summary> 
        [DataMember]
        [Display(Name = "USER_PKNO", Description = "用户编号")]
        public string USER_PKNO
        {
            get { return _USER_PKNO; }
            set { _USER_PKNO = value; }
        }

        #endregion 用户编号

        #region 是否启用

        private Int32 _USE_FLAG;
        /// <summary> 
        ///  是否启用
        /// </summary> 
        [DataMember]
        [Display(Name = "USE_FLAG", Description = "是否启用")]
        public Int32 USE_FLAG
        {
            get { return _USE_FLAG; }
            set { _USE_FLAG = value; }
        }

        #endregion 是否启用

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
