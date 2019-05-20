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
    /// 维护记录表
    /// </summary>
    [DataContract]
    public class RsMaintainRecord
    {

        #region 记录编号

        private string _PKNO;
        /// <summary> 
        ///  记录编号
        /// </summary> 
        [DataMember]
        [Display(Name = "PKNO", Description = "记录编号")]
        public string PKNO
        {
            get { return _PKNO; }
            set { _PKNO = value; }
        }

        #endregion 记录编号

        #region 设备编号

        private string _ASSET_CODE;
        /// <summary> 
        ///  设备编号
        /// </summary> 
        [DataMember]
        [Display(Name = "ASSET_CODE", Description = "设备编号")]
        public string ASSET_CODE
        {
            get { return _ASSET_CODE; }
            set { _ASSET_CODE = value; }
        }

        #endregion 设备编号

        #region 维护规程细节

        private string _STANDARD_DETAIL_ID;
        /// <summary> 
        ///  维护规程细节
        /// </summary> 
        [DataMember]
        [Display(Name = "STANDARD_DETAIL_ID", Description = "维护规程细节")]
        public string STANDARD_DETAIL_ID
        {
            get { return _STANDARD_DETAIL_ID; }
            set { _STANDARD_DETAIL_ID = value; }
        }

        #endregion 维护规程细节

        #region 维护人

        private string _MAINTAIN_USER;
        /// <summary> 
        ///  维护人
        /// </summary> 
        [DataMember]
        [Display(Name = "MAINTAIN_USER", Description = "维护人")]
        public string MAINTAIN_USER
        {
            get { return _MAINTAIN_USER; }
            set { _MAINTAIN_USER = value; }
        }

        #endregion 维护人

        #region 维护时长

        private string _MAINTAIN_TIME;
        /// <summary> 
        ///  维护时长
        /// </summary> 
        [DataMember]
        [Display(Name = "MAINTAIN_TIME", Description = "维护时长")]
        public string MAINTAIN_TIME
        {
            get { return _MAINTAIN_TIME; }
            set { _MAINTAIN_TIME = value; }
        }

        #endregion 维护时长

        #region 维护时间

        private string _MAINTAIN_DATE;
        /// <summary> 
        ///  维护时间
        /// </summary> 
        [DataMember]
        [Display(Name = "MAINTAIN_DATE", Description = "维护时间")]
        public string MAINTAIN_DATE
        {
            get { return _MAINTAIN_DATE; }
            set { _MAINTAIN_DATE = value; }
        }

        #endregion 维护时间

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
