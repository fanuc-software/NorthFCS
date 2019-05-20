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
    /// 实时信息采集表
    /// </summary>
    [DataContract]
    public class DAMachineRealTimeInfo
    {

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

        #region 采集时间

        private DateTime? _DA_TIME;
        /// <summary> 
        ///  采集时间
        /// </summary> 
        [DataMember]
        [Display(Name = "DA_TIME", Description = "采集时间")]
        public DateTime? DA_TIME
        {
            get { return _DA_TIME; }
            set { _DA_TIME = value; }
        }

        #endregion 采集时间

        #region 当前状态

        private Int32? _STATUS;
        /// <summary> 
        ///  当前状态
        /// </summary> 
        [DataMember]
        [Display(Name = "STATUS", Description = "当前状态")]
        public Int32? STATUS
        {
            get { return _STATUS; }
            set { _STATUS = value; }
        }

        #endregion 当前状态

        #region 主轴转速

        private string _SPINDLE_SPPED;
        /// <summary> 
        ///  主轴转速
        /// </summary> 
        [DataMember]
        [Display(Name = "SPINDLE_SPPED", Description = "主轴转速")]
        public string SPINDLE_SPPED
        {
            get { return _SPINDLE_SPPED; }
            set { _SPINDLE_SPPED = value; }
        }

        #endregion 主轴转速

        #region 主轴倍率

        private string _SPINDLE_OVERRIDE;
        /// <summary> 
        ///  主轴倍率
        /// </summary> 
        [DataMember]
        [Display(Name = "SPINDLE_OVERRIDE", Description = "主轴倍率")]
        public string SPINDLE_OVERRIDE
        {
            get { return _SPINDLE_OVERRIDE; }
            set { _SPINDLE_OVERRIDE = value; }
        }

        #endregion 主轴倍率

        #region 进给速度

        private string _FEED_SPEED;
        /// <summary> 
        ///  进给速度
        /// </summary> 
        [DataMember]
        [Display(Name = "FEED_SPEED", Description = "进给速度")]
        public string FEED_SPEED
        {
            get { return _FEED_SPEED; }
            set { _FEED_SPEED = value; }
        }

        #endregion 进给速度

        #region 进给倍率

        private string _FEED_RATE;
        /// <summary> 
        ///  进给倍率
        /// </summary> 
        [DataMember]
        [Display(Name = "FEED_RATE", Description = "进给倍率")]
        public string FEED_RATE
        {
            get { return _FEED_RATE; }
            set { _FEED_RATE = value; }
        }

        #endregion 进给倍率

        #region 程序号

        private string _MAIN_PROG;
        /// <summary> 
        ///  程序号
        /// </summary> 
        [DataMember]
        [Display(Name = "MAIN_PROG", Description = "程序号")]
        public string MAIN_PROG
        {
            get { return _MAIN_PROG; }
            set { _MAIN_PROG = value; }
        }

        #endregion 程序号
    }
}
