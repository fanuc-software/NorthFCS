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
    /// 状态持续结果记录表
    /// </summary>
    [DataContract]
    public class FmsStateResultRecord
    {

        #region 唯一编号

        private string _PKNO;
        /// <summary> 
        ///  唯一编号
        /// </summary> 
        [DataMember]
        [Display(Name = "PKNO", Description = "唯一编号")]
        public string PKNO
        {
            get { return _PKNO; }
            set { _PKNO = value; }
        }

        #endregion 唯一编号

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

        #region 设备通讯标签编号

        private string _TAG_SETTING_PKNO;
        /// <summary> 
        ///  设备通讯标签编号
        /// </summary> 
        [DataMember]
        [Display(Name = "TAG_SETTING_PKNO", Description = "设备通讯标签编号")]
        public string TAG_SETTING_PKNO
        {
            get { return _TAG_SETTING_PKNO; }
            set { _TAG_SETTING_PKNO = value; }
        }

        #endregion 设备通讯标签编号

        #region 标签值名称

        private string _TAG_VALUE_NAME;
        /// <summary> 
        ///  标签值名称
        /// </summary> 
        [DataMember]
        [Display(Name = "TAG_VALUE_NAME", Description = "标签值名称")]
        public string TAG_VALUE_NAME
        {
            get { return _TAG_VALUE_NAME; }
            set { _TAG_VALUE_NAME = value; }
        }

        #endregion 标签值名称

        #region 标签值

        private string _TAG_VALUE;
        /// <summary> 
        ///  标签值
        /// </summary> 
        [DataMember]
        [Display(Name = "TAG_VALUE", Description = "标签值")]
        public string TAG_VALUE
        {
            get { return _TAG_VALUE; }
            set { _TAG_VALUE = value; }
        }

        #endregion 标签值

        #region 开始时间

        private DateTime? _BEGINT_TIME;
        /// <summary> 
        ///  开始时间
        /// </summary> 
        [DataMember]
        [Display(Name = "BEGINT_TIME", Description = "开始时间")]
        public DateTime? BEGINT_TIME
        {
            get { return _BEGINT_TIME; }
            set { _BEGINT_TIME = value; }
        }

        #endregion 开始时间

        #region 结束时间

        private DateTime? _END_TIME;
        /// <summary> 
        ///  结束时间
        /// </summary> 
        [DataMember]
        [Display(Name = "END_TIME", Description = "结束时间")]
        public DateTime? END_TIME
        {
            get { return _END_TIME; }
            set { _END_TIME = value; }
        }

        #endregion 结束时间

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
