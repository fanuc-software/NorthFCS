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
    /// 设备状态表
    /// </summary>
    [DataContract]
    public class RsEquipMaster
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

        #region 加工中心

        private string _WC_PKNO;
        /// <summary> 
        ///  加工中心
        /// </summary> 
        [DataMember]
        [Display(Name = "WC_PKNO", Description = "加工中心")]
        public string WC_PKNO
        {
            get { return _WC_PKNO; }
            set { _WC_PKNO = value; }
        }

        #endregion 加工中心

        #region 设备名称

        private string _EQUIP_NAME;
        /// <summary> 
        ///  设备名称
        /// </summary> 
        [DataMember]
        [Display(Name = "EQUIP_NAME", Description = "设备名称")]
        public string EQUIP_NAME
        {
            get { return _EQUIP_NAME; }
            set { _EQUIP_NAME = value; }
        }

        #endregion 设备名称

        #region 设备类型

        private string _EQUIP_TYPE;
        /// <summary> 
        ///  设备类型
        /// </summary> 
        [DataMember]
        [Display(Name = "EQUIP_TYPE", Description = "设备类型")]
        public string EQUIP_TYPE
        {
            get { return _EQUIP_TYPE; }
            set { _EQUIP_TYPE = value; }
        }

        #endregion 设备类型

        #region 设备规格

        private string _EQUIP_NORM;
        /// <summary> 
        ///  设备规格
        /// </summary> 
        [DataMember]
        [Display(Name = "EQUIP_NORM", Description = "设备规格")]
        public string EQUIP_NORM
        {
            get { return _EQUIP_NORM; }
            set { _EQUIP_NORM = value; }
        }

        #endregion 设备规格

        #region 设备型号

        private string _EQUIP_MODEL;
        /// <summary> 
        ///  设备型号
        /// </summary> 
        [DataMember]
        [Display(Name = "EQUIP_MODEL", Description = "设备型号")]
        public string EQUIP_MODEL
        {
            get { return _EQUIP_MODEL; }
            set { _EQUIP_MODEL = value; }
        }

        #endregion 设备型号

        #region 图标路径

        private string _IMG_PATH;
        /// <summary> 
        ///  图标路径
        /// </summary> 
        [DataMember]
        [Display(Name = "IMG_PATH", Description = "图标路径")]
        public string IMG_PATH
        {
            get { return _IMG_PATH; }
            set { _IMG_PATH = value; }
        }

        #endregion 图标路径

        #region 设备状态

        private Int32 _EQUIP_STATUS;
        /// <summary> 
        ///  设备状态
        /// </summary> 
        [DataMember]
        [Display(Name = "EQUIP_STATUS", Description = "设备状态")]
        public Int32 EQUIP_STATUS
        {
            get { return _EQUIP_STATUS; }
            set { _EQUIP_STATUS = value; }
        }

        #endregion 设备状态

        #region 开机检验

        private Int32 _BEGIN_CHECK_FLAG;
        /// <summary> 
        ///  开机检验
        /// </summary> 
        [DataMember]
        [Display(Name = "BEGIN_CHECK_FLAG", Description = "开机检验")]
        public Int32 BEGIN_CHECK_FLAG
        {
            get { return _BEGIN_CHECK_FLAG; }
            set { _BEGIN_CHECK_FLAG = value; }
        }

        #endregion 开机检验

        #region 维修检验

        private Int32 _REPAIR_CHECK_FLAG;
        /// <summary> 
        ///  维修检验
        /// </summary> 
        [DataMember]
        [Display(Name = "REPAIR_CHECK_FLAG", Description = "维修检验")]
        public Int32 REPAIR_CHECK_FLAG
        {
            get { return _REPAIR_CHECK_FLAG; }
            set { _REPAIR_CHECK_FLAG = value; }
        }

        #endregion 维修检验

        #region 换型检验

        private Int32 _CHANGE_CHECK_FLAG;
        /// <summary> 
        ///  换型检验
        /// </summary> 
        [DataMember]
        [Display(Name = "CHANGE_CHECK_FLAG", Description = "换型检验")]
        public Int32 CHANGE_CHECK_FLAG
        {
            get { return _CHANGE_CHECK_FLAG; }
            set { _CHANGE_CHECK_FLAG = value; }
        }

        #endregion 换型检验

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
