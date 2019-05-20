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
    /// 生产线产品关联表
    /// </summary>
    [DataContract]
    public class RsLineProduct
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

        #region 产品编号

        private string _ITEM_PKNO;
        /// <summary> 
        ///  产品编号
        /// </summary> 
        [DataMember]
        [Display(Name = "ITEM_PKNO", Description = "产品编号")]
        public string ITEM_PKNO
        {
            get { return _ITEM_PKNO; }
            set { _ITEM_PKNO = value; }
        }

        #endregion 产品编号

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

        #region 顺序号

        private Int32 _SEQ_NO;
        /// <summary> 
        ///  顺序号
        /// </summary> 
        [DataMember]
        [Display(Name = "SEQ_NO", Description = "顺序号")]
        public Int32 SEQ_NO
        {
            get { return _SEQ_NO; }
            set { _SEQ_NO = value; }
        }

        #endregion 顺序号

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

        #region 默认标记

        private Int32 _DEFAULT_FLAG;
        /// <summary> 
        ///  默认标记
        /// </summary> 
        [DataMember]
        [Display(Name = "DEFAULT_FLAG", Description = "默认标记")]
        public Int32 DEFAULT_FLAG
        {
            get { return _DEFAULT_FLAG; }
            set { _DEFAULT_FLAG = value; }
        }

        #endregion 默认标记

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
