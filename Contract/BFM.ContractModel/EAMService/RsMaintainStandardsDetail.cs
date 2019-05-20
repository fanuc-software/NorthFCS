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
    /// 维护规程明细表
    /// </summary>
    [DataContract]
    public class RsMaintainStandardsDetail
    {

        #region 唯一标识

        private string _PKNO;
        /// <summary> 
        ///  唯一标识
        /// </summary> 
        [DataMember]
        [Display(Name = "PKNO", Description = "唯一标识")]
        public string PKNO
        {
            get { return _PKNO; }
            set { _PKNO = value; }
        }

        #endregion 唯一标识

        #region 维护规程PKNO

        private string _STANDARD_PKNO;
        /// <summary> 
        ///  维护规程PKNO
        /// </summary> 
        [DataMember]
        [Display(Name = "STANDARD_PKNO", Description = "维护规程PKNO")]
        public string STANDARD_PKNO
        {
            get { return _STANDARD_PKNO; }
            set { _STANDARD_PKNO = value; }
        }

        #endregion 维护规程PKNO

        #region 维护周期

        private string _STANDARD_CYCLE;
        /// <summary> 
        ///  维护周期
        /// </summary> 
        [DataMember]
        [Display(Name = "STANDARD_CYCLE", Description = "维护周期")]
        public string STANDARD_CYCLE
        {
            get { return _STANDARD_CYCLE; }
            set { _STANDARD_CYCLE = value; }
        }

        #endregion 维护周期

        #region 维护内容文件

        private string _STANDARD_FILE;
        /// <summary> 
        ///  维护内容文件
        /// </summary> 
        [DataMember]
        [Display(Name = "STANDARD_FILE", Description = "维护内容文件")]
        public string STANDARD_FILE
        {
            get { return _STANDARD_FILE; }
            set { _STANDARD_FILE = value; }
        }

        #endregion 维护内容文件

        #region 维护内容

        private string _STANDARD_CONTENT;
        /// <summary> 
        ///  维护内容
        /// </summary> 
        [DataMember]
        [Display(Name = "STANDARD_CONTENT", Description = "维护内容")]
        public string STANDARD_CONTENT
        {
            get { return _STANDARD_CONTENT; }
            set { _STANDARD_CONTENT = value; }
        }

        #endregion 维护内容

        #region 预计消耗时长

        private Decimal _ESTIMATED_TIME;
        /// <summary> 
        ///  预计消耗时长
        /// </summary> 
        [DataMember]
        [Display(Name = "ESTIMATED_TIME", Description = "预计消耗时长")]
        public Decimal ESTIMATED_TIME
        {
            get { return _ESTIMATED_TIME; }
            set { _ESTIMATED_TIME = value; }
        }

        #endregion 预计消耗时长

        #region 人次

        private Decimal _MAN_TIME;
        /// <summary> 
        ///  人次
        /// </summary> 
        [DataMember]
        [Display(Name = "MAN_TIME", Description = "人次")]
        public Decimal MAN_TIME
        {
            get { return _MAN_TIME; }
            set { _MAN_TIME = value; }
        }

        #endregion 人次

        #region 工种

        private string _WORK_TYPE;
        /// <summary> 
        ///  工种
        /// </summary> 
        [DataMember]
        [Display(Name = "WORK_TYPE", Description = "工种")]
        public string WORK_TYPE
        {
            get { return _WORK_TYPE; }
            set { _WORK_TYPE = value; }
        }

        #endregion 工种

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
