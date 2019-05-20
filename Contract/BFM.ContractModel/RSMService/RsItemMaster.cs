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
    /// 项目台账表
    /// </summary>
    [DataContract]
    public class RsItemMaster
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

        #region 图号

        private string _DRAWING_NO;
        /// <summary> 
        ///  图号
        /// </summary> 
        [DataMember]
        [Display(Name = "DRAWING_NO", Description = "图号")]
        public string DRAWING_NO
        {
            get { return _DRAWING_NO; }
            set { _DRAWING_NO = value; }
        }

        #endregion 图号

        #region 项目名称

        private string _ITEM_NAME;
        /// <summary> 
        ///  项目名称
        /// </summary> 
        [DataMember]
        [Display(Name = "ITEM_NAME", Description = "项目名称")]
        public string ITEM_NAME
        {
            get { return _ITEM_NAME; }
            set { _ITEM_NAME = value; }
        }

        #endregion 项目名称

        #region 项目规格

        private string _ITEM_SPECS;
        /// <summary> 
        ///  项目规格
        /// </summary> 
        [DataMember]
        [Display(Name = "ITEM_SPECS", Description = "项目规格")]
        public string ITEM_SPECS
        {
            get { return _ITEM_SPECS; }
            set { _ITEM_SPECS = value; }
        }

        #endregion 项目规格

        #region 项目型号

        private string _ITEM_NORM;
        /// <summary> 
        ///  项目型号
        /// </summary> 
        [DataMember]
        [Display(Name = "ITEM_NORM", Description = "项目型号")]
        public string ITEM_NORM
        {
            get { return _ITEM_NORM; }
            set { _ITEM_NORM = value; }
        }

        #endregion 项目型号

        #region 国标代码

        private string _GB_CODE;
        /// <summary> 
        ///  国标代码
        /// </summary> 
        [DataMember]
        [Display(Name = "GB_CODE", Description = "国标代码")]
        public string GB_CODE
        {
            get { return _GB_CODE; }
            set { _GB_CODE = value; }
        }

        #endregion 国标代码

        #region 责任部门

        private string _DEPT_CODE;
        /// <summary> 
        ///  责任部门
        /// </summary> 
        [DataMember]
        [Display(Name = "DEPT_CODE", Description = "责任部门")]
        public string DEPT_CODE
        {
            get { return _DEPT_CODE; }
            set { _DEPT_CODE = value; }
        }

        #endregion 责任部门

        #region ABC类

        private string _ABC_CLASS;
        /// <summary> 
        ///  ABC类
        /// </summary> 
        [DataMember]
        [Display(Name = "ABC_CLASS", Description = "ABC类")]
        public string ABC_CLASS
        {
            get { return _ABC_CLASS; }
            set { _ABC_CLASS = value; }
        }

        #endregion ABC类

        #region 分类标识

        private Int32? _NORM_CLASS;
        /// <summary> 
        ///  分类标识
        /// </summary> 
        [DataMember]
        [Display(Name = "NORM_CLASS", Description = "分类标识")]
        public Int32? NORM_CLASS
        {
            get { return _NORM_CLASS; }
            set { _NORM_CLASS = value; }
        }

        #endregion 分类标识

        #region 制造采购

        private string _MP_FLAG;
        /// <summary> 
        ///  制造采购
        /// </summary> 
        [DataMember]
        [Display(Name = "MP_FLAG", Description = "制造采购")]
        public string MP_FLAG
        {
            get { return _MP_FLAG; }
            set { _MP_FLAG = value; }
        }

        #endregion 制造采购

        #region 单耗定额

        private Decimal _ROUND_TIMES;
        /// <summary> 
        ///  单耗定额
        /// </summary> 
        [DataMember]
        [Display(Name = "ROUND_TIMES", Description = "单耗定额")]
        public Decimal ROUND_TIMES
        {
            get { return _ROUND_TIMES; }
            set { _ROUND_TIMES = value; }
        }

        #endregion 单耗定额

        #region 工艺路线号

        private string _ROUTING_NO;
        /// <summary> 
        ///  工艺路线号
        /// </summary> 
        [DataMember]
        [Display(Name = "ROUTING_NO", Description = "工艺路线号")]
        public string ROUTING_NO
        {
            get { return _ROUTING_NO; }
            set { _ROUTING_NO = value; }
        }

        #endregion 工艺路线号

        #region 项目简称

        private string _ITEM_ABV;
        /// <summary> 
        ///  项目简称
        /// </summary> 
        [DataMember]
        [Display(Name = "ITEM_ABV", Description = "项目简称")]
        public string ITEM_ABV
        {
            get { return _ITEM_ABV; }
            set { _ITEM_ABV = value; }
        }

        #endregion 项目简称

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
