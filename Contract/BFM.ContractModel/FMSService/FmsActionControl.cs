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
    /// 指令动作控制表
    /// </summary>
    [DataContract]
    public class FmsActionControl
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

        #region 指令动作名称

        private string _ACTION_NAME;
        /// <summary> 
        ///  指令动作名称
        /// </summary> 
        [DataMember]
        [Display(Name = "ACTION_NAME", Description = "指令动作名称")]
        public string ACTION_NAME
        {
            get { return _ACTION_NAME; }
            set { _ACTION_NAME = value; }
        }

        #endregion 指令动作名称

        #region 指令动作类型

        private string _ACTION_TYPE;
        /// <summary> 
        ///  指令动作类型
        /// </summary> 
        [DataMember]
        [Display(Name = "ACTION_TYPE", Description = "指令动作类型")]
        public string ACTION_TYPE
        {
            get { return _ACTION_TYPE; }
            set { _ACTION_TYPE = value; }
        }

        #endregion 指令动作类型

        #region 指令动作开始条件的TagPKNO

        private string _START_CONDITION_TAG_PKNO;
        /// <summary> 
        ///  指令动作开始条件的TagPKNO
        /// </summary> 
        [DataMember]
        [Display(Name = "START_CONDITION_TAG_PKNO", Description = "指令动作开始条件的TagPKNO")]
        public string START_CONDITION_TAG_PKNO
        {
            get { return _START_CONDITION_TAG_PKNO; }
            set { _START_CONDITION_TAG_PKNO = value; }
        }

        #endregion 指令动作开始条件的TagPKNO

        #region 指令动作开始条件值

        private string _START_CONDITION_VALUE;
        /// <summary> 
        ///  指令动作开始条件值
        /// </summary> 
        [DataMember]
        [Display(Name = "START_CONDITION_VALUE", Description = "指令动作开始条件值")]
        public string START_CONDITION_VALUE
        {
            get { return _START_CONDITION_VALUE; }
            set { _START_CONDITION_VALUE = value; }
        }

        #endregion 指令动作开始条件值

        #region 指令动作执行的TagPKNO

        private string _EXECUTE_TAG_PKNO;
        /// <summary> 
        ///  指令动作执行的TagPKNO
        /// </summary> 
        [DataMember]
        [Display(Name = "EXECUTE_TAG_PKNO", Description = "指令动作执行的TagPKNO")]
        public string EXECUTE_TAG_PKNO
        {
            get { return _EXECUTE_TAG_PKNO; }
            set { _EXECUTE_TAG_PKNO = value; }
        }

        #endregion 指令动作执行的TagPKNO

        #region 指令动作执行写入的值

        private string _EXECUTE_WRITE_VALUE;
        /// <summary> 
        ///  指令动作执行写入的值
        /// </summary> 
        [DataMember]
        [Display(Name = "EXECUTE_WRITE_VALUE", Description = "指令动作执行写入的值")]
        public string EXECUTE_WRITE_VALUE
        {
            get { return _EXECUTE_WRITE_VALUE; }
            set { _EXECUTE_WRITE_VALUE = value; }
        }

        #endregion 指令动作执行写入的值

        #region 执行参数1的TagPKNO

        private string _EXECUTE_PARAM1_TAG_PKNO;
        /// <summary> 
        ///  执行参数1的TagPKNO
        /// </summary> 
        [DataMember]
        [Display(Name = "EXECUTE_PARAM1_TAG_PKNO", Description = "执行参数1的TagPKNO")]
        public string EXECUTE_PARAM1_TAG_PKNO
        {
            get { return _EXECUTE_PARAM1_TAG_PKNO; }
            set { _EXECUTE_PARAM1_TAG_PKNO = value; }
        }

        #endregion 执行参数1的TagPKNO

        #region 执行参数2的TagPKNO

        private string _EXECUTE_PARAM2_TAG_PKNO;
        /// <summary> 
        ///  执行参数2的TagPKNO
        /// </summary> 
        [DataMember]
        [Display(Name = "EXECUTE_PARAM2_TAG_PKNO", Description = "执行参数2的TagPKNO")]
        public string EXECUTE_PARAM2_TAG_PKNO
        {
            get { return _EXECUTE_PARAM2_TAG_PKNO; }
            set { _EXECUTE_PARAM2_TAG_PKNO = value; }
        }

        #endregion 执行参数2的TagPKNO

        #region 指令动作完成条件的TagPKNO

        private string _FINISH_CONDITION_TAG_PKNO;
        /// <summary> 
        ///  指令动作完成条件的TagPKNO
        /// </summary> 
        [DataMember]
        [Display(Name = "FINISH_CONDITION_TAG_PKNO", Description = "指令动作完成条件的TagPKNO")]
        public string FINISH_CONDITION_TAG_PKNO
        {
            get { return _FINISH_CONDITION_TAG_PKNO; }
            set { _FINISH_CONDITION_TAG_PKNO = value; }
        }

        #endregion 指令动作完成条件的TagPKNO

        #region 指令动作完成条件的值

        private string _FINISH_CONDITION_VALUE;
        /// <summary> 
        ///  指令动作完成条件的值
        /// </summary> 
        [DataMember]
        [Display(Name = "FINISH_CONDITION_VALUE", Description = "指令动作完成条件的值")]
        public string FINISH_CONDITION_VALUE
        {
            get { return _FINISH_CONDITION_VALUE; }
            set { _FINISH_CONDITION_VALUE = value; }
        }

        #endregion 指令动作完成条件的值

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
