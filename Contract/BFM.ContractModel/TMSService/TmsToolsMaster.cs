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
    /// 刀具台账表
    /// </summary>
    [DataContract]
    public class TmsToolsMaster
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

        #region 刀具唯一编码

        private string _TOOLS_CODE;
        /// <summary> 
        ///  刀具唯一编码
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOLS_CODE", Description = "刀具唯一编码")]
        public string TOOLS_CODE
        {
            get { return _TOOLS_CODE; }
            set { _TOOLS_CODE = value; }
        }

        #endregion 刀具唯一编码

        #region 刀具类型

        private string _TOOLS_TYPE_PKNO;
        /// <summary> 
        ///  刀具类型
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOLS_TYPE_PKNO", Description = "刀具类型")]
        public string TOOLS_TYPE_PKNO
        {
            get { return _TOOLS_TYPE_PKNO; }
            set { _TOOLS_TYPE_PKNO = value; }
        }

        #endregion 刀具类型

        #region 刀具名称

        private string _TOOLS_NAME;
        /// <summary> 
        ///  刀具名称
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOLS_NAME", Description = "刀具名称")]
        public string TOOLS_NAME
        {
            get { return _TOOLS_NAME; }
            set { _TOOLS_NAME = value; }
        }

        #endregion 刀具名称

        #region 刀具寿命统计方式

        private Int32 _TOOLS_LIFE_METHOD;
        /// <summary> 
        ///  刀具寿命统计方式
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOLS_LIFE_METHOD", Description = "刀具寿命统计方式")]
        public Int32 TOOLS_LIFE_METHOD
        {
            get { return _TOOLS_LIFE_METHOD; }
            set { _TOOLS_LIFE_METHOD = value; }
        }

        #endregion 刀具寿命统计方式

        #region 刀具计划寿命

        private Int32 _TOOLS_LIFE_PLAN;
        /// <summary> 
        ///  刀具计划寿命
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOLS_LIFE_PLAN", Description = "刀具计划寿命")]
        public Int32 TOOLS_LIFE_PLAN
        {
            get { return _TOOLS_LIFE_PLAN; }
            set { _TOOLS_LIFE_PLAN = value; }
        }

        #endregion 刀具计划寿命

        #region 刀具已使用寿命

        private Int32 _TOOLS_LIFE_USED;
        /// <summary> 
        ///  刀具已使用寿命
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOLS_LIFE_USED", Description = "刀具已使用寿命")]
        public Int32 TOOLS_LIFE_USED
        {
            get { return _TOOLS_LIFE_USED; }
            set { _TOOLS_LIFE_USED = value; }
        }

        #endregion 刀具已使用寿命

        #region 刀具直径的形状补偿

        private Decimal _COMPENSATION_SHAPE_DIAMETER;
        /// <summary> 
        ///  刀具直径的形状补偿
        /// </summary> 
        [DataMember]
        [Display(Name = "COMPENSATION_SHAPE_DIAMETER", Description = "刀具直径的形状补偿")]
        public Decimal COMPENSATION_SHAPE_DIAMETER
        {
            get { return _COMPENSATION_SHAPE_DIAMETER; }
            set { _COMPENSATION_SHAPE_DIAMETER = value; }
        }

        #endregion 刀具直径的形状补偿

        #region 刀具直径的磨损补偿

        private Decimal _COMPENSATION_ABRASION_DIAMETER;
        /// <summary> 
        ///  刀具直径的磨损补偿
        /// </summary> 
        [DataMember]
        [Display(Name = "COMPENSATION_ABRASION_DIAMETER", Description = "刀具直径的磨损补偿")]
        public Decimal COMPENSATION_ABRASION_DIAMETER
        {
            get { return _COMPENSATION_ABRASION_DIAMETER; }
            set { _COMPENSATION_ABRASION_DIAMETER = value; }
        }

        #endregion 刀具直径的磨损补偿

        #region 刀具长度的形状补偿

        private Decimal _COMPENSATION_SHAPE_LENGTH;
        /// <summary> 
        ///  刀具长度的形状补偿
        /// </summary> 
        [DataMember]
        [Display(Name = "COMPENSATION_SHAPE_LENGTH", Description = "刀具长度的形状补偿")]
        public Decimal COMPENSATION_SHAPE_LENGTH
        {
            get { return _COMPENSATION_SHAPE_LENGTH; }
            set { _COMPENSATION_SHAPE_LENGTH = value; }
        }

        #endregion 刀具长度的形状补偿

        #region 刀具长度的磨损补偿

        private Decimal _COMPENSATION_ABRASION_LENGTH;
        /// <summary> 
        ///  刀具长度的磨损补偿
        /// </summary> 
        [DataMember]
        [Display(Name = "COMPENSATION_ABRASION_LENGTH", Description = "刀具长度的磨损补偿")]
        public Decimal COMPENSATION_ABRASION_LENGTH
        {
            get { return _COMPENSATION_ABRASION_LENGTH; }
            set { _COMPENSATION_ABRASION_LENGTH = value; }
        }

        #endregion 刀具长度的磨损补偿

        #region 刀具位置状态

        private Int32 _TOOLS_POSITION;
        /// <summary> 
        ///  刀具位置状态
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOLS_POSITION", Description = "刀具位置状态")]
        public Int32 TOOLS_POSITION
        {
            get { return _TOOLS_POSITION; }
            set { _TOOLS_POSITION = value; }
        }

        #endregion 刀具位置状态

        #region 刀具所在具体位置

        private string _TOOLS_POSITION_PKNO;
        /// <summary> 
        ///  刀具所在具体位置
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOLS_POSITION_PKNO", Description = "刀具所在具体位置")]
        public string TOOLS_POSITION_PKNO
        {
            get { return _TOOLS_POSITION_PKNO; }
            set { _TOOLS_POSITION_PKNO = value; }
        }

        #endregion 刀具所在具体位置

        #region 刀具描述

        private string _TOOLS_INTROD;
        /// <summary> 
        ///  刀具描述
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOLS_INTROD", Description = "刀具描述")]
        public string TOOLS_INTROD
        {
            get { return _TOOLS_INTROD; }
            set { _TOOLS_INTROD = value; }
        }

        #endregion 刀具描述

        #region 刀具图片

        private byte[] _TOOLS_PIC;
        /// <summary> 
        ///  刀具图片
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOLS_PIC", Description = "刀具图片")]
        public byte[] TOOLS_PIC
        {
            get { return _TOOLS_PIC; }
            set { _TOOLS_PIC = value; }
        }

        #endregion 刀具图片

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
