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
    /// 维护规程关系表
    /// </summary>
    [DataContract]
    public class RsMaintainStandardsRelate
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

        #region 维护规程明细PKNO

        private string _STANDARD_DETAIL_PKNO;
        /// <summary> 
        ///  维护规程明细PKNO
        /// </summary> 
        [DataMember]
        [Display(Name = "STANDARD_DETAIL_PKNO", Description = "维护规程明细PKNO")]
        public string STANDARD_DETAIL_PKNO
        {
            get { return _STANDARD_DETAIL_PKNO; }
            set { _STANDARD_DETAIL_PKNO = value; }
        }

        #endregion 维护规程明细PKNO

        #region 设备资产编码

        private string _ASSET_CODE;
        /// <summary> 
        ///  设备资产编码
        /// </summary> 
        [DataMember]
        [Display(Name = "ASSET_CODE", Description = "设备资产编码")]
        public string ASSET_CODE
        {
            get { return _ASSET_CODE; }
            set { _ASSET_CODE = value; }
        }

        #endregion 设备资产编码

        #region 上一次维护时间

        private DateTime? _LAST_MAINTAIN_TIME;
        /// <summary> 
        ///  上一次维护时间
        /// </summary> 
        [DataMember]
        [Display(Name = "LAST_MAINTAIN_TIME", Description = "上一次维护时间")]
        public DateTime? LAST_MAINTAIN_TIME
        {
            get { return _LAST_MAINTAIN_TIME; }
            set { _LAST_MAINTAIN_TIME = value; }
        }

        #endregion 上一次维护时间

        #region 下一次维护时间

        private DateTime? _NEXT_MAINTAIN_TIME;
        /// <summary> 
        ///  下一次维护时间
        /// </summary> 
        [DataMember]
        [Display(Name = "NEXT_MAINTAIN_TIME", Description = "下一次维护时间")]
        public DateTime? NEXT_MAINTAIN_TIME
        {
            get { return _NEXT_MAINTAIN_TIME; }
            set { _NEXT_MAINTAIN_TIME = value; }
        }

        #endregion 下一次维护时间

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
