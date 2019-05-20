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
    /// 附件信息表
    /// </summary>
    [DataContract]
    public class SysAttachInfo
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

        #region 所属公司

        private string _COMPANY_CODE;
        /// <summary> 
        ///  所属公司
        /// </summary> 
        [DataMember]
        [Display(Name = "COMPANY_CODE", Description = "所属公司")]
        public string COMPANY_CODE
        {
            get { return _COMPANY_CODE; }
            set { _COMPANY_CODE = value; }
        }

        #endregion 所属公司

        #region 所属模块

        private string _BELONGFUNCTION;
        /// <summary> 
        ///  所属模块
        /// </summary> 
        [DataMember]
        [Display(Name = "BELONGFUNCTION", Description = "所属模块")]
        public string BELONGFUNCTION
        {
            get { return _BELONGFUNCTION; }
            set { _BELONGFUNCTION = value; }
        }

        #endregion 所属模块

        #region 所属模块PKNO

        private string _FUNCTIONPKNO;
        /// <summary> 
        ///  所属模块PKNO
        /// </summary> 
        [DataMember]
        [Display(Name = "FUNCTIONPKNO", Description = "所属模块PKNO")]
        public string FUNCTIONPKNO
        {
            get { return _FUNCTIONPKNO; }
            set { _FUNCTIONPKNO = value; }
        }

        #endregion 所属模块PKNO

        #region 分组编号

        private string _GROUPNO;
        /// <summary> 
        ///  分组编号
        /// </summary> 
        [DataMember]
        [Display(Name = "GROUPNO", Description = "分组编号")]
        public string GROUPNO
        {
            get { return _GROUPNO; }
            set { _GROUPNO = value; }
        }

        #endregion 分组编号

        #region 附件名称

        private string _ATTACHNAME;
        /// <summary> 
        ///  附件名称
        /// </summary> 
        [DataMember]
        [Display(Name = "ATTACHNAME", Description = "附件名称")]
        public string ATTACHNAME
        {
            get { return _ATTACHNAME; }
            set { _ATTACHNAME = value; }
        }

        #endregion 附件名称

        #region 附件管理方式

        private Int32 _ATTACHMANAGEMODE;
        /// <summary> 
        ///  附件管理方式
        /// </summary> 
        [DataMember]
        [Display(Name = "ATTACHMANAGEMODE", Description = "附件管理方式")]
        public Int32 ATTACHMANAGEMODE
        {
            get { return _ATTACHMANAGEMODE; }
            set { _ATTACHMANAGEMODE = value; }
        }

        #endregion 附件管理方式

        #region 附件格式

        private string _ATTACHFORMATE;
        /// <summary> 
        ///  附件格式
        /// </summary> 
        [DataMember]
        [Display(Name = "ATTACHFORMATE", Description = "附件格式")]
        public string ATTACHFORMATE
        {
            get { return _ATTACHFORMATE; }
            set { _ATTACHFORMATE = value; }
        }

        #endregion 附件格式

        #region 附件存储类型

        private Int32 _ATTACHSTORETYPE;
        /// <summary> 
        ///  附件存储类型
        /// </summary> 
        [DataMember]
        [Display(Name = "ATTACHSTORETYPE", Description = "附件存储类型")]
        public Int32 ATTACHSTORETYPE
        {
            get { return _ATTACHSTORETYPE; }
            set { _ATTACHSTORETYPE = value; }
        }

        #endregion 附件存储类型

        #region 附件内容描述

        private string _ATTACHINTROD;
        /// <summary> 
        ///  附件内容描述
        /// </summary> 
        [DataMember]
        [Display(Name = "ATTACHINTROD", Description = "附件内容描述")]
        public string ATTACHINTROD
        {
            get { return _ATTACHINTROD; }
            set { _ATTACHINTROD = value; }
        }

        #endregion 附件内容描述

        #region 附件存储的文件路径

        private string _ATTACHSTOREFILE;
        /// <summary> 
        ///  附件存储的文件路径
        /// </summary> 
        [DataMember]
        [Display(Name = "ATTACHSTOREFILE", Description = "附件存储的文件路径")]
        public string ATTACHSTOREFILE
        {
            get { return _ATTACHSTOREFILE; }
            set { _ATTACHSTOREFILE = value; }
        }

        #endregion 附件存储的文件路径

        #region 附件内容

        private byte[] _ATTACHINFO;
        /// <summary> 
        ///  附件内容
        /// </summary> 
        [DataMember]
        [Display(Name = "ATTACHINFO", Description = "附件内容")]
        public byte[] ATTACHINFO
        {
            get { return _ATTACHINFO; }
            set { _ATTACHINFO = value; }
        }

        #endregion 附件内容

        #region 附件图标

        private byte[] _ATTACHICON;
        /// <summary> 
        ///  附件图标
        /// </summary> 
        [DataMember]
        [Display(Name = "ATTACHICON", Description = "附件图标")]
        public byte[] ATTACHICON
        {
            get { return _ATTACHICON; }
            set { _ATTACHICON = value; }
        }

        #endregion 附件图标

        #region 排序

        private Int32 _ISEQ;
        /// <summary> 
        ///  排序
        /// </summary> 
        [DataMember]
        [Display(Name = "ISEQ", Description = "排序")]
        public Int32 ISEQ
        {
            get { return _ISEQ; }
            set { _ISEQ = value; }
        }

        #endregion 排序

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

        #region 修改人

        private string _UPDATED_BY;
        /// <summary> 
        ///  修改人
        /// </summary> 
        [DataMember]
        [Display(Name = "UPDATED_BY", Description = "修改人")]
        public string UPDATED_BY
        {
            get { return _UPDATED_BY; }
            set { _UPDATED_BY = value; }
        }

        #endregion 修改人

        #region 修改时间

        private DateTime? _LAST_UPDATE_DATE;
        /// <summary> 
        ///  修改时间
        /// </summary> 
        [DataMember]
        [Display(Name = "LAST_UPDATE_DATE", Description = "修改时间")]
        public DateTime? LAST_UPDATE_DATE
        {
            get { return _LAST_UPDATE_DATE; }
            set { _LAST_UPDATE_DATE = value; }
        }

        #endregion 修改时间

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

        #region 状态

        private Int32 _ISTATE;
        /// <summary> 
        ///  状态
        /// </summary> 
        [DataMember]
        [Display(Name = "ISTATE", Description = "状态")]
        public Int32 ISTATE
        {
            get { return _ISTATE; }
            set { _ISTATE = value; }
        }

        #endregion 状态

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
