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
    /// 报警记录表
    /// </summary>
    [DataContract]
    public class DAAlarmRecord
    {

        #region 唯一编号

        private string _PKNO;
        /// <summary> 
        ///  唯一编号
        /// </summary> 
        [DataMember]
        [Display(Name = "PKNO", Description = "唯一编号")]
        public string PKNO
        {
            get { return _PKNO; }
            set { _PKNO = value; }
        }

        #endregion 唯一编号

        #region 设备编号

        private string _ASSET_CODE;
        /// <summary> 
        ///  设备编号
        /// </summary> 
        [DataMember]
        [Display(Name = "ASSET_CODE", Description = "设备编号")]
        public string ASSET_CODE
        {
            get { return _ASSET_CODE; }
            set { _ASSET_CODE = value; }
        }

        #endregion 设备编号

        #region 开始时间

        private DateTime? _START_TIME;
        /// <summary> 
        ///  开始时间
        /// </summary> 
        [DataMember]
        [Display(Name = "START_TIME", Description = "开始时间")]
        public DateTime? START_TIME
        {
            get { return _START_TIME; }
            set { _START_TIME = value; }
        }

        #endregion 开始时间

        #region 结束时间

        private DateTime? _END_TIME;
        /// <summary> 
        ///  结束时间
        /// </summary> 
        [DataMember]
        [Display(Name = "END_TIME", Description = "结束时间")]
        public DateTime? END_TIME
        {
            get { return _END_TIME; }
            set { _END_TIME = value; }
        }

        #endregion 结束时间

        #region 报警类型

        private string _ALARM_TYPE;
        /// <summary> 
        ///  报警类型
        /// </summary> 
        [DataMember]
        [Display(Name = "ALARM_TYPE", Description = "报警类型")]
        public string ALARM_TYPE
        {
            get { return _ALARM_TYPE; }
            set { _ALARM_TYPE = value; }
        }

        #endregion 报警类型

        #region 报警号

        private string _ALARM_NO;
        /// <summary> 
        ///  报警号
        /// </summary> 
        [DataMember]
        [Display(Name = "ALARM_NO", Description = "报警号")]
        public string ALARM_NO
        {
            get { return _ALARM_NO; }
            set { _ALARM_NO = value; }
        }

        #endregion 报警号

        #region 报警内容

        private string _ALARM_CONTENT;
        /// <summary> 
        ///  报警内容
        /// </summary> 
        [DataMember]
        [Display(Name = "ALARM_CONTENT", Description = "报警内容")]
        public string ALARM_CONTENT
        {
            get { return _ALARM_CONTENT; }
            set { _ALARM_CONTENT = value; }
        }

        #endregion 报警内容

        #region 报警状态

        private Int32? _ALARM_STATUS;
        /// <summary> 
        ///  报警状态
        /// </summary> 
        [DataMember]
        [Display(Name = "ALARM_STATUS", Description = "报警状态")]
        public Int32? ALARM_STATUS
        {
            get { return _ALARM_STATUS; }
            set { _ALARM_STATUS = value; }
        }

        #endregion 报警状态
    }
}
