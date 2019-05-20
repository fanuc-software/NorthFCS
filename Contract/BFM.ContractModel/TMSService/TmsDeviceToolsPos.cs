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
    /// 设备刀位信息表
    /// </summary>
    [DataContract]
    public class TmsDeviceToolsPos
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

        #region 设备PKNO

        private string _DEVICE_PKNO;
        /// <summary> 
        ///  设备PKNO
        /// </summary> 
        [DataMember]
        [Display(Name = "DEVICE_PKNO", Description = "设备PKNO")]
        public string DEVICE_PKNO
        {
            get { return _DEVICE_PKNO; }
            set { _DEVICE_PKNO = value; }
        }

        #endregion 设备PKNO

        #region 刀具PKNO

        private string _TOOLS_PKNO;
        /// <summary> 
        ///  刀具PKNO
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOLS_PKNO", Description = "刀具PKNO")]
        public string TOOLS_PKNO
        {
            get { return _TOOLS_PKNO; }
            set { _TOOLS_PKNO = value; }
        }

        #endregion 刀具PKNO

        #region 刀位号

        private string _TOOLS_POS_NO;
        /// <summary> 
        ///  刀位号
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOLS_POS_NO", Description = "刀位号")]
        public string TOOLS_POS_NO
        {
            get { return _TOOLS_POS_NO; }
            set { _TOOLS_POS_NO = value; }
        }

        #endregion 刀位号

        #region 刀位描述

        private string _POS_INTROD;
        /// <summary> 
        ///  刀位描述
        /// </summary> 
        [DataMember]
        [Display(Name = "POS_INTROD", Description = "刀位描述")]
        public string POS_INTROD
        {
            get { return _POS_INTROD; }
            set { _POS_INTROD = value; }
        }

        #endregion 刀位描述

        #region 刀具状态

        private Int32 _TOOLS_STATE;
        /// <summary> 
        ///  刀具状态
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOLS_STATE", Description = "刀具状态")]
        public Int32 TOOLS_STATE
        {
            get { return _TOOLS_STATE; }
            set { _TOOLS_STATE = value; }
        }

        #endregion 刀具状态

        #region 新刀号

        private string _NEW_TOOLS_PKNO;
        /// <summary> 
        ///  新刀号
        /// </summary> 
        [DataMember]
        [Display(Name = "NEW_TOOLS_PKNO", Description = "新刀号")]
        public string NEW_TOOLS_PKNO
        {
            get { return _NEW_TOOLS_PKNO; }
            set { _NEW_TOOLS_PKNO = value; }
        }

        #endregion 新刀号

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
