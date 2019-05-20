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
    /// 工序刀具清单表
    /// </summary>
    [DataContract]
    public class RsRoutingTools
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

        private string _ROUTING_DETAIL_PKNO;
        /// <summary> 
        ///  工序编号
        /// </summary> 
        [DataMember]
        [Display(Name = "ROUTING_DETAIL_PKNO", Description = "工序编号")]
        public string ROUTING_DETAIL_PKNO
        {
            get { return _ROUTING_DETAIL_PKNO; }
            set { _ROUTING_DETAIL_PKNO = value; }
        }

        #endregion 工序编号

        #region NC程序刀号

        private string _TOOLS_NC_CODE;
        /// <summary> 
        ///  NC程序刀号
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOLS_NC_CODE", Description = "NC程序刀号")]
        public string TOOLS_NC_CODE
        {
            get { return _TOOLS_NC_CODE; }
            set { _TOOLS_NC_CODE = value; }
        }

        #endregion NC程序刀号

        #region 刀具类型PKNO

        private string _TOOLS_TYPE_PKNO;
        /// <summary> 
        ///  刀具类型PKNO
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOLS_TYPE_PKNO", Description = "刀具类型PKNO")]
        public string TOOLS_TYPE_PKNO
        {
            get { return _TOOLS_TYPE_PKNO; }
            set { _TOOLS_TYPE_PKNO = value; }
        }

        #endregion 刀具类型PKNO

        #region 说明

        private string _INTROD;
        /// <summary> 
        ///  说明
        /// </summary> 
        [DataMember]
        [Display(Name = "INTROD", Description = "说明")]
        public string INTROD
        {
            get { return _INTROD; }
            set { _INTROD = value; }
        }

        #endregion 说明

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
