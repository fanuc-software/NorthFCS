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
    /// 状态记录表
    /// </summary>
    [DataContract]
    public class DAStatusRecord
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

        #region 状态

        private Int32? _RUN_STATUS;
        /// <summary> 
        ///  状态
        /// </summary> 
        [DataMember]
        [Display(Name = "RUN_STATUS", Description = "状态")]
        public Int32? RUN_STATUS
        {
            get { return _RUN_STATUS; }
            set { _RUN_STATUS = value; }
        }

        #endregion 状态

        #region 主程序号

        private string _MAIN_PROG;
        /// <summary> 
        ///  主程序号
        /// </summary> 
        [DataMember]
        [Display(Name = "MAIN_PROG", Description = "主程序号")]
        public string MAIN_PROG
        {
            get { return _MAIN_PROG; }
            set { _MAIN_PROG = value; }
        }

        #endregion 主程序号

        #region CT时间

        private string _CT_TIME;
        /// <summary> 
        ///  CT时间
        /// </summary> 
        [DataMember]
        [Display(Name = "CT_TIME", Description = "CT时间")]
        public string CT_TIME
        {
            get { return _CT_TIME; }
            set { _CT_TIME = value; }
        }

        #endregion CT时间
    }
}
