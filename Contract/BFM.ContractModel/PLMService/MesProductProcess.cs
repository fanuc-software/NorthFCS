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
    /// 产品生产情况表
    /// </summary>
    [DataContract]
    public class MesProductProcess
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

        #region 产品项目PKNO

        private string _ITEM_PKNO;
        /// <summary> 
        ///  产品项目PKNO
        /// </summary> 
        [DataMember]
        [Display(Name = "ITEM_PKNO", Description = "产品项目PKNO")]
        public string ITEM_PKNO
        {
            get { return _ITEM_PKNO; }
            set { _ITEM_PKNO = value; }
        }

        #endregion 产品项目PKNO

        #region 工单PKNO

        private string _JOB_ORDER_PKNO;
        /// <summary> 
        ///  工单PKNO
        /// </summary> 
        [DataMember]
        [Display(Name = "JOB_ORDER_PKNO", Description = "工单PKNO")]
        public string JOB_ORDER_PKNO
        {
            get { return _JOB_ORDER_PKNO; }
            set { _JOB_ORDER_PKNO = value; }
        }

        #endregion 工单PKNO

        #region 工单编号

        private string _JOB_ORDER;
        /// <summary> 
        ///  工单编号
        /// </summary> 
        [DataMember]
        [Display(Name = "JOB_ORDER", Description = "工单编号")]
        public string JOB_ORDER
        {
            get { return _JOB_ORDER; }
            set { _JOB_ORDER = value; }
        }

        #endregion 工单编号

        #region 子工单编号

        private string _SUB_JOB_ORDER_NO;
        /// <summary> 
        ///  子工单编号
        /// </summary> 
        [DataMember]
        [Display(Name = "SUB_JOB_ORDER_NO", Description = "子工单编号")]
        public string SUB_JOB_ORDER_NO
        {
            get { return _SUB_JOB_ORDER_NO; }
            set { _SUB_JOB_ORDER_NO = value; }
        }

        #endregion 子工单编号

        #region 产品编码

        private string _PRODUCT_CODE;
        /// <summary> 
        ///  产品编码
        /// </summary> 
        [DataMember]
        [Display(Name = "PRODUCT_CODE", Description = "产品编码")]
        public string PRODUCT_CODE
        {
            get { return _PRODUCT_CODE; }
            set { _PRODUCT_CODE = value; }
        }

        #endregion 产品编码

        #region 产品当前位置

        private string _PRODUCT_POSITION;
        /// <summary> 
        ///  产品当前位置
        /// </summary> 
        [DataMember]
        [Display(Name = "PRODUCT_POSITION", Description = "产品当前位置")]
        public string PRODUCT_POSITION
        {
            get { return _PRODUCT_POSITION; }
            set { _PRODUCT_POSITION = value; }
        }

        #endregion 产品当前位置

        #region 当前物料

        private string _CUR_ITEM_PKNO;
        /// <summary> 
        ///  当前物料
        /// </summary> 
        [DataMember]
        [Display(Name = "CUR_ITEM_PKNO", Description = "当前物料")]
        public string CUR_ITEM_PKNO
        {
            get { return _CUR_ITEM_PKNO; }
            set { _CUR_ITEM_PKNO = value; }
        }

        #endregion 当前物料

        #region 当前生产过程

        private string _CUR_ROCESS_CTROL_PKNO;
        /// <summary> 
        ///  当前生产过程
        /// </summary> 
        [DataMember]
        [Display(Name = "CUR_ROCESS_CTROL_PKNO", Description = "当前生产过程")]
        public string CUR_ROCESS_CTROL_PKNO
        {
            get { return _CUR_ROCESS_CTROL_PKNO; }
            set { _CUR_ROCESS_CTROL_PKNO = value; }
        }

        #endregion 当前生产过程

        #region 原料数量

        private Decimal? _RAW_NUMBER;
        /// <summary> 
        ///  原料数量
        /// </summary> 
        [DataMember]
        [Display(Name = "RAW_NUMBER", Description = "原料数量")]
        public Decimal? RAW_NUMBER
        {
            get { return _RAW_NUMBER; }
            set { _RAW_NUMBER = value; }
        }

        #endregion 原料数量

        #region 产品数量

        private Decimal? _PRODUCT_NUMBER;
        /// <summary> 
        ///  产品数量
        /// </summary> 
        [DataMember]
        [Display(Name = "PRODUCT_NUMBER", Description = "产品数量")]
        public Decimal? PRODUCT_NUMBER
        {
            get { return _PRODUCT_NUMBER; }
            set { _PRODUCT_NUMBER = value; }
        }

        #endregion 产品数量

        #region 合格品数量

        private Decimal? _QUALIFIED_NUMBER;
        /// <summary> 
        ///  合格品数量
        /// </summary> 
        [DataMember]
        [Display(Name = "QUALIFIED_NUMBER", Description = "合格品数量")]
        public Decimal? QUALIFIED_NUMBER
        {
            get { return _QUALIFIED_NUMBER; }
            set { _QUALIFIED_NUMBER = value; }
        }

        #endregion 合格品数量

        #region 托盘号

        private string _PALLET_NO;
        /// <summary> 
        ///  托盘号
        /// </summary> 
        [DataMember]
        [Display(Name = "PALLET_NO", Description = "托盘号")]
        public string PALLET_NO
        {
            get { return _PALLET_NO; }
            set { _PALLET_NO = value; }
        }

        #endregion 托盘号

        #region 产品状态

        private Int32? _PRODUCT_STATE;
        /// <summary> 
        ///  产品状态
        /// </summary> 
        [DataMember]
        [Display(Name = "PRODUCT_STATE", Description = "产品状态")]
        public Int32? PRODUCT_STATE
        {
            get { return _PRODUCT_STATE; }
            set { _PRODUCT_STATE = value; }
        }

        #endregion 产品状态

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
