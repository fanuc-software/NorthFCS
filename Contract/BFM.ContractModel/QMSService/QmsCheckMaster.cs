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
    /// 质量检测主表
    /// </summary>
    [DataContract]
    public class QmsCheckMaster
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

        #region 检测单编号

        private string _CHECK_NO;
        /// <summary> 
        ///  检测单编号
        /// </summary> 
        [DataMember]
        [Display(Name = "CHECK_NO", Description = "检测单编号")]
        public string CHECK_NO
        {
            get { return _CHECK_NO; }
            set { _CHECK_NO = value; }
        }

        #endregion 检测单编号

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

        #region 任务编号

        private string _TASKLINE_PKNO;
        /// <summary> 
        ///  任务编号
        /// </summary> 
        [DataMember]
        [Display(Name = "TASKLINE_PKNO", Description = "任务编号")]
        public string TASKLINE_PKNO
        {
            get { return _TASKLINE_PKNO; }
            set { _TASKLINE_PKNO = value; }
        }

        #endregion 任务编号

        #region 检测参数编号

        private string _CHECK_PARAM_PKNO;
        /// <summary> 
        ///  检测参数编号
        /// </summary> 
        [DataMember]
        [Display(Name = "CHECK_PARAM_PKNO", Description = "检测参数编号")]
        public string CHECK_PARAM_PKNO
        {
            get { return _CHECK_PARAM_PKNO; }
            set { _CHECK_PARAM_PKNO = value; }
        }

        #endregion 检测参数编号

        #region 质检方案编号

        private string _ROUTING_CHECK_PKNO;
        /// <summary> 
        ///  质检方案编号
        /// </summary> 
        [DataMember]
        [Display(Name = "ROUTING_CHECK_PKNO", Description = "质检方案编号")]
        public string ROUTING_CHECK_PKNO
        {
            get { return _ROUTING_CHECK_PKNO; }
            set { _ROUTING_CHECK_PKNO = value; }
        }

        #endregion 质检方案编号

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

        #region 检测方式

        private string _CHK_MODE;
        /// <summary> 
        ///  检测方式
        /// </summary> 
        [DataMember]
        [Display(Name = "CHK_MODE", Description = "检测方式")]
        public string CHK_MODE
        {
            get { return _CHK_MODE; }
            set { _CHK_MODE = value; }
        }

        #endregion 检测方式

        #region 检查状态

        private string _CHECK_STATUS;
        /// <summary> 
        ///  检查状态
        /// </summary> 
        [DataMember]
        [Display(Name = "CHECK_STATUS", Description = "检查状态")]
        public string CHECK_STATUS
        {
            get { return _CHECK_STATUS; }
            set { _CHECK_STATUS = value; }
        }

        #endregion 检查状态

        #region 检测类型

        private string _CHECK_TYPE;
        /// <summary> 
        ///  检测类型
        /// </summary> 
        [DataMember]
        [Display(Name = "CHECK_TYPE", Description = "检测类型")]
        public string CHECK_TYPE
        {
            get { return _CHECK_TYPE; }
            set { _CHECK_TYPE = value; }
        }

        #endregion 检测类型

        #region 检测结果

        private string _CHECK_RESULT;
        /// <summary> 
        ///  检测结果
        /// </summary> 
        [DataMember]
        [Display(Name = "CHECK_RESULT", Description = "检测结果")]
        public string CHECK_RESULT
        {
            get { return _CHECK_RESULT; }
            set { _CHECK_RESULT = value; }
        }

        #endregion 检测结果

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

        private Int32? _USE_FLAG;
        /// <summary> 
        ///  启用标识
        /// </summary> 
        [DataMember]
        [Display(Name = "USE_FLAG", Description = "启用标识")]
        public Int32? USE_FLAG
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
