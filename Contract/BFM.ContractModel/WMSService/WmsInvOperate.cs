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
    /// 库存作业明细表
    /// </summary>
    [DataContract]
    public class WmsInvOperate
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

        #region 物料编号

        private string _MATERIAL_PKNO;
        /// <summary> 
        ///  物料编号
        /// </summary> 
        [DataMember]
        [Display(Name = "MATERIAL_PKNO", Description = "物料编号")]
        public string MATERIAL_PKNO
        {
            get { return _MATERIAL_PKNO; }
            set { _MATERIAL_PKNO = value; }
        }

        #endregion 物料编号

        #region 货位地址

        private string _ALLOCATION_PKNO;
        /// <summary> 
        ///  货位地址
        /// </summary> 
        [DataMember]
        [Display(Name = "ALLOCATION_PKNO", Description = "货位地址")]
        public string ALLOCATION_PKNO
        {
            get { return _ALLOCATION_PKNO; }
            set { _ALLOCATION_PKNO = value; }
        }

        #endregion 货位地址

        #region 库区PKNO

        private string _AREA_PKNO;
        /// <summary> 
        ///  库区PKNO
        /// </summary> 
        [DataMember]
        [Display(Name = "AREA_PKNO", Description = "库区PKNO")]
        public string AREA_PKNO
        {
            get { return _AREA_PKNO; }
            set { _AREA_PKNO = value; }
        }

        #endregion 库区PKNO

        #region 作业来源

        private Int32 _OPERATE_SOURCE;
        /// <summary> 
        ///  作业来源
        /// </summary> 
        [DataMember]
        [Display(Name = "OPERATE_SOURCE", Description = "作业来源")]
        public Int32 OPERATE_SOURCE
        {
            get { return _OPERATE_SOURCE; }
            set { _OPERATE_SOURCE = value; }
        }

        #endregion 作业来源

        #region 批次号

        private string _BATCH_NO;
        /// <summary> 
        ///  批次号
        /// </summary> 
        [DataMember]
        [Display(Name = "BATCH_NO", Description = "批次号")]
        public string BATCH_NO
        {
            get { return _BATCH_NO; }
            set { _BATCH_NO = value; }
        }

        #endregion 批次号

        #region 作业数量

        private Decimal _OPERATE_NUM;
        /// <summary> 
        ///  作业数量
        /// </summary> 
        [DataMember]
        [Display(Name = "OPERATE_NUM", Description = "作业数量")]
        public Decimal OPERATE_NUM
        {
            get { return _OPERATE_NUM; }
            set { _OPERATE_NUM = value; }
        }

        #endregion 作业数量

        #region 作业类型

        private Int32 _OPERATE_TYPE;
        /// <summary> 
        ///  作业类型
        /// </summary> 
        [DataMember]
        [Display(Name = "OPERATE_TYPE", Description = "作业类型")]
        public Int32 OPERATE_TYPE
        {
            get { return _OPERATE_TYPE; }
            set { _OPERATE_TYPE = value; }
        }

        #endregion 作业类型

        #region 作业人员

        private string _OPERATE_PERSON;
        /// <summary> 
        ///  作业人员
        /// </summary> 
        [DataMember]
        [Display(Name = "OPERATE_PERSON", Description = "作业人员")]
        public string OPERATE_PERSON
        {
            get { return _OPERATE_PERSON; }
            set { _OPERATE_PERSON = value; }
        }

        #endregion 作业人员

        #region 出库去向

        private Int32 _OUT_TARGET;
        /// <summary> 
        ///  出库去向
        /// </summary> 
        [DataMember]
        [Display(Name = "OUT_TARGET", Description = "出库去向")]
        public Int32 OUT_TARGET
        {
            get { return _OUT_TARGET; }
            set { _OUT_TARGET = value; }
        }

        #endregion 出库去向

        #region 安装位置

        private string _INSTALL_POS;
        /// <summary> 
        ///  安装位置
        /// </summary> 
        [DataMember]
        [Display(Name = "INSTALL_POS", Description = "安装位置")]
        public string INSTALL_POS
        {
            get { return _INSTALL_POS; }
            set { _INSTALL_POS = value; }
        }

        #endregion 安装位置

        #region 作业时间

        private DateTime? _OPERATE_TIME;
        /// <summary> 
        ///  作业时间
        /// </summary> 
        [DataMember]
        [Display(Name = "OPERATE_TIME", Description = "作业时间")]
        public DateTime? OPERATE_TIME
        {
            get { return _OPERATE_TIME; }
            set { _OPERATE_TIME = value; }
        }

        #endregion 作业时间

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
