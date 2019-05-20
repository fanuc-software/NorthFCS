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
    /// 工序检验方案表
    /// </summary>
    [DataContract]
    public class RsRoutingCheck
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

        #region 工序编号

        private string _PROCESS_PKNO;
        /// <summary> 
        ///  工序编号
        /// </summary> 
        [DataMember]
        [Display(Name = "PROCESS_PKNO", Description = "工序编号")]
        public string PROCESS_PKNO
        {
            get { return _PROCESS_PKNO; }
            set { _PROCESS_PKNO = value; }
        }

        #endregion 工序编号

        #region 检验对象单位

        private string _CHK_OBJ_UNIT;
        /// <summary> 
        ///  检验对象单位
        /// </summary> 
        [DataMember]
        [Display(Name = "CHK_OBJ_UNIT", Description = "检验对象单位")]
        public string CHK_OBJ_UNIT
        {
            get { return _CHK_OBJ_UNIT; }
            set { _CHK_OBJ_UNIT = value; }
        }

        #endregion 检验对象单位

        #region 检验频次值

        private string _CHK_FREQ_VALUE;
        /// <summary> 
        ///  检验频次值
        /// </summary> 
        [DataMember]
        [Display(Name = "CHK_FREQ_VALUE", Description = "检验频次值")]
        public string CHK_FREQ_VALUE
        {
            get { return _CHK_FREQ_VALUE; }
            set { _CHK_FREQ_VALUE = value; }
        }

        #endregion 检验频次值

        #region 检验频次单位

        private string _CHK_FREQ_UNIT;
        /// <summary> 
        ///  检验频次单位
        /// </summary> 
        [DataMember]
        [Display(Name = "CHK_FREQ_UNIT", Description = "检验频次单位")]
        public string CHK_FREQ_UNIT
        {
            get { return _CHK_FREQ_UNIT; }
            set { _CHK_FREQ_UNIT = value; }
        }

        #endregion 检验频次单位

        #region 记录频次值

        private Int32 _REC_FREQ_VALUE;
        /// <summary> 
        ///  记录频次值
        /// </summary> 
        [DataMember]
        [Display(Name = "REC_FREQ_VALUE", Description = "记录频次值")]
        public Int32 REC_FREQ_VALUE
        {
            get { return _REC_FREQ_VALUE; }
            set { _REC_FREQ_VALUE = value; }
        }

        #endregion 记录频次值

        #region 记录频次单位

        private Decimal _REC_FREQ_UNIT;
        /// <summary> 
        ///  记录频次单位
        /// </summary> 
        [DataMember]
        [Display(Name = "REC_FREQ_UNIT", Description = "记录频次单位")]
        public Decimal REC_FREQ_UNIT
        {
            get { return _REC_FREQ_UNIT; }
            set { _REC_FREQ_UNIT = value; }
        }

        #endregion 记录频次单位

        #region 记录方式

        private string _REC_MODE;
        /// <summary> 
        ///  记录方式
        /// </summary> 
        [DataMember]
        [Display(Name = "REC_MODE", Description = "记录方式")]
        public string REC_MODE
        {
            get { return _REC_MODE; }
            set { _REC_MODE = value; }
        }

        #endregion 记录方式

        #region 采集标识

        private Int32 _COLLECTION_FLAG;
        /// <summary> 
        ///  采集标识
        /// </summary> 
        [DataMember]
        [Display(Name = "COLLECTION_FLAG", Description = "采集标识")]
        public Int32 COLLECTION_FLAG
        {
            get { return _COLLECTION_FLAG; }
            set { _COLLECTION_FLAG = value; }
        }

        #endregion 采集标识

        #region 检测类别

        private Decimal _CHECK_TYPE;
        /// <summary> 
        ///  检测类别
        /// </summary> 
        [DataMember]
        [Display(Name = "CHECK_TYPE", Description = "检测类别")]
        public Decimal CHECK_TYPE
        {
            get { return _CHECK_TYPE; }
            set { _CHECK_TYPE = value; }
        }

        #endregion 检测类别

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
