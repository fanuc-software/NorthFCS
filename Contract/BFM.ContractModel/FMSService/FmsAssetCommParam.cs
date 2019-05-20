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
    /// 设备通讯参数配置表
    /// </summary>
    [DataContract]
    public class FmsAssetCommParam
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

        #region 设备属性

        private string _ASSET_ATTRIBUTE;
        /// <summary> 
        ///  设备属性
        /// </summary> 
        [DataMember]
        [Display(Name = "ASSET_ATTRIBUTE", Description = "设备属性")]
        public string ASSET_ATTRIBUTE
        {
            get { return _ASSET_ATTRIBUTE; }
            set { _ASSET_ATTRIBUTE = value; }
        }

        #endregion 设备属性

        #region 通讯接口类型

        private Int32 _INTERFACE_TYPE;
        /// <summary> 
        ///  通讯接口类型
        /// </summary> 
        [DataMember]
        [Display(Name = "INTERFACE_TYPE", Description = "通讯接口类型")]
        public Int32 INTERFACE_TYPE
        {
            get { return _INTERFACE_TYPE; }
            set { _INTERFACE_TYPE = value; }
        }

        #endregion 通讯接口类型

        #region 通讯地址

        private string _COMM_ADDRESS;
        /// <summary> 
        ///  通讯地址
        /// </summary> 
        [DataMember]
        [Display(Name = "COMM_ADDRESS", Description = "通讯地址")]
        public string COMM_ADDRESS
        {
            get { return _COMM_ADDRESS; }
            set { _COMM_ADDRESS = value; }
        }

        #endregion 通讯地址

        #region 指定采样计算机编码

        private string _SAMPLING_COMPUTER_CODE;
        /// <summary> 
        ///  指定采样计算机编码
        /// </summary> 
        [DataMember]
        [Display(Name = "SAMPLING_COMPUTER_CODE", Description = "指定采样计算机编码")]
        public string SAMPLING_COMPUTER_CODE
        {
            get { return _SAMPLING_COMPUTER_CODE; }
            set { _SAMPLING_COMPUTER_CODE = value; }
        }

        #endregion 指定采样计算机编码

        #region 采样周期(s)

        private Decimal? _SAMPLING_PERIOD;
        /// <summary> 
        ///  采样周期(s)
        /// </summary> 
        [DataMember]
        [Display(Name = "SAMPLING_PERIOD", Description = "采样周期(s)")]
        public Decimal? SAMPLING_PERIOD
        {
            get { return _SAMPLING_PERIOD; }
            set { _SAMPLING_PERIOD = value; }
        }

        #endregion 采样周期(s)

        #region 通讯标签地址格式说明

        private string _TAG_ADDRESS_INTROD;
        /// <summary> 
        ///  通讯标签地址格式说明
        /// </summary> 
        [DataMember]
        [Display(Name = "TAG_ADDRESS_INTROD", Description = "通讯标签地址格式说明")]
        public string TAG_ADDRESS_INTROD
        {
            get { return _TAG_ADDRESS_INTROD; }
            set { _TAG_ADDRESS_INTROD = value; }
        }

        #endregion 通讯标签地址格式说明

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

        private Int32? _USE_FLAG;
        /// <summary> 
        ///  启用标识
        /// </summary> 
        [DataMember]
        [Display(Name = "USE_FLAG", Description = "启用标识")]
        public Int32? USE_FLAG
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
