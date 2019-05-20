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
    /// 菜单权限表
    /// </summary>
    [DataContract]
    public class SysMenuPurview
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

        #region 权限编码

        private string _PURVIEW_PKNO;
        /// <summary> 
        ///  权限编码
        /// </summary> 
        [DataMember]
        [Display(Name = "PURVIEW_PKNO", Description = "权限编码")]
        public string PURVIEW_PKNO
        {
            get { return _PURVIEW_PKNO; }
            set { _PURVIEW_PKNO = value; }
        }

        #endregion 权限编码

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
    }
}
