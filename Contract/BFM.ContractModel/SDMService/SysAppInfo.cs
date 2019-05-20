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
    /// 系统程序信息表
    /// </summary>
    [DataContract]
    public class SysAppInfo
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

        #region 模块标识

        private string _MODEL_CODE;
        /// <summary> 
        ///  模块标识
        /// </summary> 
        [DataMember]
        [Display(Name = "MODEL_CODE", Description = "模块标识")]
        public string MODEL_CODE
        {
            get { return _MODEL_CODE; }
            set { _MODEL_CODE = value; }
        }

        #endregion 模块标识

        #region 模块名称

        private string _MODEL_NAME;
        /// <summary> 
        ///  模块名称
        /// </summary> 
        [DataMember]
        [Display(Name = "MODEL_NAME", Description = "模块名称")]
        public string MODEL_NAME
        {
            get { return _MODEL_NAME; }
            set { _MODEL_NAME = value; }
        }

        #endregion 模块名称

        #region 模块内部版本

        private Int32? _MODEL_INNER_VERSION;
        /// <summary> 
        ///  模块内部版本
        /// </summary> 
        [DataMember]
        [Display(Name = "MODEL_INNER_VERSION", Description = "模块内部版本")]
        public Int32? MODEL_INNER_VERSION
        {
            get { return _MODEL_INNER_VERSION; }
            set { _MODEL_INNER_VERSION = value; }
        }

        #endregion 模块内部版本

        #region 模块版本

        private string _MODEL_VERSION;
        /// <summary> 
        ///  模块版本
        /// </summary> 
        [DataMember]
        [Display(Name = "MODEL_VERSION", Description = "模块版本")]
        public string MODEL_VERSION
        {
            get { return _MODEL_VERSION; }
            set { _MODEL_VERSION = value; }
        }

        #endregion 模块版本

        #region 模块应用程序名称

        private string _APP_NAME;
        /// <summary> 
        ///  模块应用程序名称
        /// </summary> 
        [DataMember]
        [Display(Name = "APP_NAME", Description = "模块应用程序名称")]
        public string APP_NAME
        {
            get { return _APP_NAME; }
            set { _APP_NAME = value; }
        }

        #endregion 模块应用程序名称

        #region 应用程序相对路径

        private string _APP_RELATIVE_PATH;
        /// <summary> 
        ///  应用程序相对路径
        /// </summary> 
        [DataMember]
        [Display(Name = "APP_RELATIVE_PATH", Description = "应用程序相对路径")]
        public string APP_RELATIVE_PATH
        {
            get { return _APP_RELATIVE_PATH; }
            set { _APP_RELATIVE_PATH = value; }
        }

        #endregion 应用程序相对路径

        #region 版本描述

        private string _VERSION_INTROD;
        /// <summary> 
        ///  版本描述
        /// </summary> 
        [DataMember]
        [Display(Name = "VERSION_INTROD", Description = "版本描述")]
        public string VERSION_INTROD
        {
            get { return _VERSION_INTROD; }
            set { _VERSION_INTROD = value; }
        }

        #endregion 版本描述

        #region 模型内容

        private byte[] _MODEL_CONTENT;
        /// <summary> 
        ///  模型内容
        /// </summary> 
        [DataMember]
        [Display(Name = "MODEL_CONTENT", Description = "模型内容")]
        public byte[] MODEL_CONTENT
        {
            get { return _MODEL_CONTENT; }
            set { _MODEL_CONTENT = value; }
        }

        #endregion 模型内容

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
