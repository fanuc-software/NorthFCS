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
    /// 工位信息表
    /// </summary>
    [DataContract]
    public class RsLineStation
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

        #region 生产线编号

        private string _LINE_PKNO;
        /// <summary> 
        ///  生产线编号
        /// </summary> 
        [DataMember]
        [Display(Name = "LINE_PKNO", Description = "生产线编号")]
        public string LINE_PKNO
        {
            get { return _LINE_PKNO; }
            set { _LINE_PKNO = value; }
        }

        #endregion 生产线编号

        #region 工位名称

        private string _STATION_NAME;
        /// <summary> 
        ///  工位名称
        /// </summary> 
        [DataMember]
        [Display(Name = "STATION_NAME", Description = "工位名称")]
        public string STATION_NAME
        {
            get { return _STATION_NAME; }
            set { _STATION_NAME = value; }
        }

        #endregion 工位名称

        #region 工位序号

        private Int32 _STATION_SEQ;
        /// <summary> 
        ///  工位序号
        /// </summary> 
        [DataMember]
        [Display(Name = "STATION_SEQ", Description = "工位序号")]
        public Int32 STATION_SEQ
        {
            get { return _STATION_SEQ; }
            set { _STATION_SEQ = value; }
        }

        #endregion 工位序号

        #region 工位类型

        private string _STATION_TYPE;
        /// <summary> 
        ///  工位类型
        /// </summary> 
        [DataMember]
        [Display(Name = "STATION_TYPE", Description = "工位类型")]
        public string STATION_TYPE
        {
            get { return _STATION_TYPE; }
            set { _STATION_TYPE = value; }
        }

        #endregion 工位类型

        #region 设备编号

        private string _EQUIP_CODE;
        /// <summary> 
        ///  设备编号
        /// </summary> 
        [DataMember]
        [Display(Name = "EQUIP_CODE", Description = "设备编号")]
        public string EQUIP_CODE
        {
            get { return _EQUIP_CODE; }
            set { _EQUIP_CODE = value; }
        }

        #endregion 设备编号

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

        #region IP地址

        private string _IP_ADDRESS;
        /// <summary> 
        ///  IP地址
        /// </summary> 
        [DataMember]
        [Display(Name = "IP_ADDRESS", Description = "IP地址")]
        public string IP_ADDRESS
        {
            get { return _IP_ADDRESS; }
            set { _IP_ADDRESS = value; }
        }

        #endregion IP地址

        #region MAC地址

        private string _MAC_ADDRESS;
        /// <summary> 
        ///  MAC地址
        /// </summary> 
        [DataMember]
        [Display(Name = "MAC_ADDRESS", Description = "MAC地址")]
        public string MAC_ADDRESS
        {
            get { return _MAC_ADDRESS; }
            set { _MAC_ADDRESS = value; }
        }

        #endregion MAC地址

        #region 首末标识

        private string _SF_FLAG;
        /// <summary> 
        ///  首末标识
        /// </summary> 
        [DataMember]
        [Display(Name = "SF_FLAG", Description = "首末标识")]
        public string SF_FLAG
        {
            get { return _SF_FLAG; }
            set { _SF_FLAG = value; }
        }

        #endregion 首末标识

        #region 父项工位

        private string _PARENT_STATION;
        /// <summary> 
        ///  父项工位
        /// </summary> 
        [DataMember]
        [Display(Name = "PARENT_STATION", Description = "父项工位")]
        public string PARENT_STATION
        {
            get { return _PARENT_STATION; }
            set { _PARENT_STATION = value; }
        }

        #endregion 父项工位

        #region 关键工位

        private string _KEY_FLAG;
        /// <summary> 
        ///  关键工位
        /// </summary> 
        [DataMember]
        [Display(Name = "KEY_FLAG", Description = "关键工位")]
        public string KEY_FLAG
        {
            get { return _KEY_FLAG; }
            set { _KEY_FLAG = value; }
        }

        #endregion 关键工位

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
