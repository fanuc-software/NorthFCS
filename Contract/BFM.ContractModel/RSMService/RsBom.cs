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
    /// BOM信息表
    /// </summary>
    [DataContract]
    public class RsBom
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

        #region 父唯一性字段

        private string _PARENT_PKNO;
        /// <summary> 
        ///  父唯一性字段
        /// </summary> 
        [DataMember]
        [Display(Name = "PARENT_PKNO", Description = "父唯一性字段")]
        public string PARENT_PKNO
        {
            get { return _PARENT_PKNO; }
            set { _PARENT_PKNO = value; }
        }

        #endregion 父唯一性字段

        #region 项目PKNO

        private string _ITEM_PKNO;
        /// <summary> 
        ///  项目PKNO
        /// </summary> 
        [DataMember]
        [Display(Name = "ITEM_PKNO", Description = "项目PKNO")]
        public string ITEM_PKNO
        {
            get { return _ITEM_PKNO; }
            set { _ITEM_PKNO = value; }
        }

        #endregion 项目PKNO

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

        #region 父类名称

        private string _PARENT_NAME;
        /// <summary> 
        ///  父类名称
        /// </summary> 
        [DataMember]
        [Display(Name = "PARENT_NAME", Description = "父类名称")]
        public string PARENT_NAME
        {
            get { return _PARENT_NAME; }
            set { _PARENT_NAME = value; }
        }

        #endregion 父类名称

        #region 子类名称

        private string _CHILD_NAME;
        /// <summary> 
        ///  子类名称
        /// </summary> 
        [DataMember]
        [Display(Name = "CHILD_NAME", Description = "子类名称")]
        public string CHILD_NAME
        {
            get { return _CHILD_NAME; }
            set { _CHILD_NAME = value; }
        }

        #endregion 子类名称

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

        #region 子项型号

        private string _CHILD_MODEL;
        /// <summary> 
        ///  子项型号
        /// </summary> 
        [DataMember]
        [Display(Name = "CHILD_MODEL", Description = "子项型号")]
        public string CHILD_MODEL
        {
            get { return _CHILD_MODEL; }
            set { _CHILD_MODEL = value; }
        }

        #endregion 子项型号

        #region 子项规格

        private string _CHILD_NORM;
        /// <summary> 
        ///  子项规格
        /// </summary> 
        [DataMember]
        [Display(Name = "CHILD_NORM", Description = "子项规格")]
        public string CHILD_NORM
        {
            get { return _CHILD_NORM; }
            set { _CHILD_NORM = value; }
        }

        #endregion 子项规格

        #region 工序编号

        private string _OP_NO;
        /// <summary> 
        ///  工序编号
        /// </summary> 
        [DataMember]
        [Display(Name = "OP_NO", Description = "工序编号")]
        public string OP_NO
        {
            get { return _OP_NO; }
            set { _OP_NO = value; }
        }

        #endregion 工序编号

        #region 子项数量

        private string _CHILD_QTY;
        /// <summary> 
        ///  子项数量
        /// </summary> 
        [DataMember]
        [Display(Name = "CHILD_QTY", Description = "子项数量")]
        public string CHILD_QTY
        {
            get { return _CHILD_QTY; }
            set { _CHILD_QTY = value; }
        }

        #endregion 子项数量

        #region 顺序编号

        private Int32 _SEQ_NO;
        /// <summary> 
        ///  顺序编号
        /// </summary> 
        [DataMember]
        [Display(Name = "SEQ_NO", Description = "顺序编号")]
        public Int32 SEQ_NO
        {
            get { return _SEQ_NO; }
            set { _SEQ_NO = value; }
        }

        #endregion 顺序编号

        #region 工位编号

        private string _STATION_PKNO;
        /// <summary> 
        ///  工位编号
        /// </summary> 
        [DataMember]
        [Display(Name = "STATION_PKNO", Description = "工位编号")]
        public string STATION_PKNO
        {
            get { return _STATION_PKNO; }
            set { _STATION_PKNO = value; }
        }

        #endregion 工位编号

        #region 工序偏移

        private Int32 _MOVE_DAYS;
        /// <summary> 
        ///  工序偏移
        /// </summary> 
        [DataMember]
        [Display(Name = "MOVE_DAYS", Description = "工序偏移")]
        public Int32 MOVE_DAYS
        {
            get { return _MOVE_DAYS; }
            set { _MOVE_DAYS = value; }
        }

        #endregion 工序偏移

        #region 基础件标识

        private string _BASIC_FLAG;
        /// <summary> 
        ///  基础件标识
        /// </summary> 
        [DataMember]
        [Display(Name = "BASIC_FLAG", Description = "基础件标识")]
        public string BASIC_FLAG
        {
            get { return _BASIC_FLAG; }
            set { _BASIC_FLAG = value; }
        }

        #endregion 基础件标识

        #region 装配组元

        private string _ASSEMBLY_GROUP;
        /// <summary> 
        ///  装配组元
        /// </summary> 
        [DataMember]
        [Display(Name = "ASSEMBLY_GROUP", Description = "装配组元")]
        public string ASSEMBLY_GROUP
        {
            get { return _ASSEMBLY_GROUP; }
            set { _ASSEMBLY_GROUP = value; }
        }

        #endregion 装配组元

        #region 生产组元

        private string _PRODUCT_GROUP;
        /// <summary> 
        ///  生产组元
        /// </summary> 
        [DataMember]
        [Display(Name = "PRODUCT_GROUP", Description = "生产组元")]
        public string PRODUCT_GROUP
        {
            get { return _PRODUCT_GROUP; }
            set { _PRODUCT_GROUP = value; }
        }

        #endregion 生产组元

        #region 外直送

        private string _STRAIGHT_SEND_FLAG;
        /// <summary> 
        ///  外直送
        /// </summary> 
        [DataMember]
        [Display(Name = "STRAIGHT_SEND_FLAG", Description = "外直送")]
        public string STRAIGHT_SEND_FLAG
        {
            get { return _STRAIGHT_SEND_FLAG; }
            set { _STRAIGHT_SEND_FLAG = value; }
        }

        #endregion 外直送

        #region 原父项

        private string _ORI_PARENT_CODE;
        /// <summary> 
        ///  原父项
        /// </summary> 
        [DataMember]
        [Display(Name = "ORI_PARENT_CODE", Description = "原父项")]
        public string ORI_PARENT_CODE
        {
            get { return _ORI_PARENT_CODE; }
            set { _ORI_PARENT_CODE = value; }
        }

        #endregion 原父项

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
