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
    /// 库区信息表
    /// </summary>
    [DataContract]
    public class WmsAreaInfo
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

        #region 库区编码

        private string _AREA_CODE;
        /// <summary> 
        ///  库区编码
        /// </summary> 
        [DataMember]
        [Display(Name = "AREA_CODE", Description = "库区编码")]
        public string AREA_CODE
        {
            get { return _AREA_CODE; }
            set { _AREA_CODE = value; }
        }

        #endregion 库区编码

        #region 库区名称

        private string _AREA_NAME;
        /// <summary> 
        ///  库区名称
        /// </summary> 
        [DataMember]
        [Display(Name = "AREA_NAME", Description = "库区名称")]
        public string AREA_NAME
        {
            get { return _AREA_NAME; }
            set { _AREA_NAME = value; }
        }

        #endregion 库区名称

        #region 库区类型

        private string _AREA_TYPE;
        /// <summary> 
        ///  库区类型
        /// </summary> 
        [DataMember]
        [Display(Name = "AREA_TYPE", Description = "库区类型")]
        public string AREA_TYPE
        {
            get { return _AREA_TYPE; }
            set { _AREA_TYPE = value; }
        }

        #endregion 库区类型

        #region 总排数

        private Int32? _TOTAL_ROW;
        /// <summary> 
        ///  总排数
        /// </summary> 
        [DataMember]
        [Display(Name = "TOTAL_ROW", Description = "总排数")]
        public Int32? TOTAL_ROW
        {
            get { return _TOTAL_ROW; }
            set { _TOTAL_ROW = value; }
        }

        #endregion 总排数

        #region 总列数

        private Int32? _TOTAL_COL;
        /// <summary> 
        ///  总列数
        /// </summary> 
        [DataMember]
        [Display(Name = "TOTAL_COL", Description = "总列数")]
        public Int32? TOTAL_COL
        {
            get { return _TOTAL_COL; }
            set { _TOTAL_COL = value; }
        }

        #endregion 总列数

        #region 总层数

        private Int32? _TOTAL_LAY;
        /// <summary> 
        ///  总层数
        /// </summary> 
        [DataMember]
        [Display(Name = "TOTAL_LAY", Description = "总层数")]
        public Int32? TOTAL_LAY
        {
            get { return _TOTAL_LAY; }
            set { _TOTAL_LAY = value; }
        }

        #endregion 总层数

        #region 库区描述

        private string _AREA_INTROD;
        /// <summary> 
        ///  库区描述
        /// </summary> 
        [DataMember]
        [Display(Name = "AREA_INTROD", Description = "库区描述")]
        public string AREA_INTROD
        {
            get { return _AREA_INTROD; }
            set { _AREA_INTROD = value; }
        }

        #endregion 库区描述

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
