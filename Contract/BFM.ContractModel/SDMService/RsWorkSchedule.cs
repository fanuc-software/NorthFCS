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
    /// 工作日历表
    /// </summary>
    [DataContract]
    public class RsWorkSchedule
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

        #region 标记日期

        private DateTime? _MARK_DATE;
        /// <summary> 
        ///  标记日期
        /// </summary> 
        [DataMember]
        [Display(Name = "MARK_DATE", Description = "标记日期")]
        public DateTime? MARK_DATE
        {
            get { return _MARK_DATE; }
            set { _MARK_DATE = value; }
        }

        #endregion 标记日期

        #region 标记开始日期

        private DateTime? _MARK_STARTTIME;
        /// <summary> 
        ///  标记开始日期
        /// </summary> 
        [DataMember]
        [Display(Name = "MARK_STARTTIME", Description = "标记开始日期")]
        public DateTime? MARK_STARTTIME
        {
            get { return _MARK_STARTTIME; }
            set { _MARK_STARTTIME = value; }
        }

        #endregion 标记开始日期

        #region 标记结束日期

        private DateTime? _MARK_ENDTIME;
        /// <summary> 
        ///  标记结束日期
        /// </summary> 
        [DataMember]
        [Display(Name = "MARK_ENDTIME", Description = "标记结束日期")]
        public DateTime? MARK_ENDTIME
        {
            get { return _MARK_ENDTIME; }
            set { _MARK_ENDTIME = value; }
        }

        #endregion 标记结束日期

        #region 日期标识

        private string _WORK_FLAG;
        /// <summary> 
        ///  日期标识
        /// </summary> 
        [DataMember]
        [Display(Name = "WORK_FLAG", Description = "日期标识")]
        public string WORK_FLAG
        {
            get { return _WORK_FLAG; }
            set { _WORK_FLAG = value; }
        }

        #endregion 日期标识

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
    }
}
