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
    /// 产量记录表
    /// </summary>
    [DataContract]
    public class DAProductRecord
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

        #region 工件计数

        private Int32? _PART_NUM;
        /// <summary> 
        ///  工件计数
        /// </summary> 
        [DataMember]
        [Display(Name = "PART_NUM", Description = "工件计数")]
        public Int32? PART_NUM
        {
            get { return _PART_NUM; }
            set { _PART_NUM = value; }
        }

        #endregion 工件计数

        #region 工件总数

        private Int32? _TOTAL_PART_NUM;
        /// <summary> 
        ///  工件总数
        /// </summary> 
        [DataMember]
        [Display(Name = "TOTAL_PART_NUM", Description = "工件总数")]
        public Int32? TOTAL_PART_NUM
        {
            get { return _TOTAL_PART_NUM; }
            set { _TOTAL_PART_NUM = value; }
        }

        #endregion 工件总数
    }
}
