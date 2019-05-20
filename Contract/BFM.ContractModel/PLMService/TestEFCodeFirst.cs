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
    /// 测试EF框架
    /// </summary>
    [DataContract]
    public class TestEFCodeFirst
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

        #region 用户名

        private string _USER_NAME;
        /// <summary> 
        ///  用户名
        /// </summary> 
        [DataMember]
        [Display(Name = "USER_NAME", Description = "用户名")]
        public string USER_NAME
        {
            get { return _USER_NAME; }
            set { _USER_NAME = value; }
        }

        #endregion 用户名

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
    }
}
