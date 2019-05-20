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
    /// 货位信息表
    /// </summary>
    [DataContract]
    public class WmsAllocationInfo
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

        #region 货位编码

        private string _ALLOCATION_CODE;
        /// <summary> 
        ///  货位编码
        /// </summary> 
        [DataMember]
        [Display(Name = "ALLOCATION_CODE", Description = "货位编码")]
        public string ALLOCATION_CODE
        {
            get { return _ALLOCATION_CODE; }
            set { _ALLOCATION_CODE = value; }
        }

        #endregion 货位编码

        #region 货位名称

        private string _ALLOCATION_NAME;
        /// <summary> 
        ///  货位名称
        /// </summary> 
        [DataMember]
        [Display(Name = "ALLOCATION_NAME", Description = "货位名称")]
        public string ALLOCATION_NAME
        {
            get { return _ALLOCATION_NAME; }
            set { _ALLOCATION_NAME = value; }
        }

        #endregion 货位名称

        #region 所属库区

        private string _AREA_PKNO;
        /// <summary> 
        ///  所属库区
        /// </summary> 
        [DataMember]
        [Display(Name = "AREA_PKNO", Description = "所属库区")]
        public string AREA_PKNO
        {
            get { return _AREA_PKNO; }
            set { _AREA_PKNO = value; }
        }

        #endregion 所属库区

        #region 货位描述

        private string _ALLOCATION_INTROD;
        /// <summary> 
        ///  货位描述
        /// </summary> 
        [DataMember]
        [Display(Name = "ALLOCATION_INTROD", Description = "货位描述")]
        public string ALLOCATION_INTROD
        {
            get { return _ALLOCATION_INTROD; }
            set { _ALLOCATION_INTROD = value; }
        }

        #endregion 货位描述

        #region 排

        private Int32? _ALLOCATION_ROW;
        /// <summary> 
        ///  排
        /// </summary> 
        [DataMember]
        [Display(Name = "ALLOCATION_ROW", Description = "排")]
        public Int32? ALLOCATION_ROW
        {
            get { return _ALLOCATION_ROW; }
            set { _ALLOCATION_ROW = value; }
        }

        #endregion 排

        #region 列

        private Int32? _ALLOCATION_COL;
        /// <summary> 
        ///  列
        /// </summary> 
        [DataMember]
        [Display(Name = "ALLOCATION_COL", Description = "列")]
        public Int32? ALLOCATION_COL
        {
            get { return _ALLOCATION_COL; }
            set { _ALLOCATION_COL = value; }
        }

        #endregion 列

        #region 层

        private Int32? _ALLOCATION_LAY;
        /// <summary> 
        ///  层
        /// </summary> 
        [DataMember]
        [Display(Name = "ALLOCATION_LAY", Description = "层")]
        public Int32? ALLOCATION_LAY
        {
            get { return _ALLOCATION_LAY; }
            set { _ALLOCATION_LAY = value; }
        }

        #endregion 层

        #region 货位容量

        private Decimal? _ALLOCATION_CAPACITY;
        /// <summary> 
        ///  货位容量
        /// </summary> 
        [DataMember]
        [Display(Name = "ALLOCATION_CAPACITY", Description = "货位容量")]
        public Decimal? ALLOCATION_CAPACITY
        {
            get { return _ALLOCATION_CAPACITY; }
            set { _ALLOCATION_CAPACITY = value; }
        }

        #endregion 货位容量

        #region 货位接口名称

        private string _INTERFACE_NAME;
        /// <summary> 
        ///  货位接口名称
        /// </summary> 
        [DataMember]
        [Display(Name = "INTERFACE_NAME", Description = "货位接口名称")]
        public string INTERFACE_NAME
        {
            get { return _INTERFACE_NAME; }
            set { _INTERFACE_NAME = value; }
        }

        #endregion 货位接口名称

        #region 当前托盘号

        private string _CUR_PALLET_NO;
        /// <summary> 
        ///  当前托盘号
        /// </summary> 
        [DataMember]
        [Display(Name = "CUR_PALLET_NO", Description = "当前托盘号")]
        public string CUR_PALLET_NO
        {
            get { return _CUR_PALLET_NO; }
            set { _CUR_PALLET_NO = value; }
        }

        #endregion 当前托盘号

        #region 货位状态

        private Int32? _ALLOCATION_STATE;
        /// <summary> 
        ///  货位状态
        /// </summary> 
        [DataMember]
        [Display(Name = "ALLOCATION_STATE", Description = "货位状态")]
        public Int32? ALLOCATION_STATE
        {
            get { return _ALLOCATION_STATE; }
            set { _ALLOCATION_STATE = value; }
        }

        #endregion 货位状态

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
