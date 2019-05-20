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
    /// 生产过程表
    /// </summary>
    [DataContract]
    public class MesProcessCtrol
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

        #region 工序编号（工艺路线明细）

        private string _ROUTING_DETAIL_PKNO;
        /// <summary> 
        ///  工序编号（工艺路线明细）
        /// </summary> 
        [DataMember]
        [Display(Name = "ROUTING_DETAIL_PKNO", Description = "工序编号（工艺路线明细）")]
        public string ROUTING_DETAIL_PKNO
        {
            get { return _ROUTING_DETAIL_PKNO; }
            set { _ROUTING_DETAIL_PKNO = value; }
        }

        #endregion 工序编号（工艺路线明细）

        #region 过程控制名称

        private string _PROCESS_CTROL_NAME;
        /// <summary> 
        ///  过程控制名称
        /// </summary> 
        [DataMember]
        [Display(Name = "PROCESS_CTROL_NAME", Description = "过程控制名称")]
        public string PROCESS_CTROL_NAME
        {
            get { return _PROCESS_CTROL_NAME; }
            set { _PROCESS_CTROL_NAME = value; }
        }

        #endregion 过程控制名称

        #region 加工设备

        private string _PROCESS_DEVICE_PKNO;
        /// <summary> 
        ///  加工设备
        /// </summary> 
        [DataMember]
        [Display(Name = "PROCESS_DEVICE_PKNO", Description = "加工设备")]
        public string PROCESS_DEVICE_PKNO
        {
            get { return _PROCESS_DEVICE_PKNO; }
            set { _PROCESS_DEVICE_PKNO = value; }
        }

        #endregion 加工设备

        #region 加工程序号

        private string _PROCESS_PROGRAM_NO;
        /// <summary> 
        ///  加工程序号
        /// </summary> 
        [DataMember]
        [Display(Name = "PROCESS_PROGRAM_NO", Description = "加工程序号")]
        public string PROCESS_PROGRAM_NO
        {
            get { return _PROCESS_PROGRAM_NO; }
            set { _PROCESS_PROGRAM_NO = value; }
        }

        #endregion 加工程序号

        #region 加工程序内容

        private string _PROCESS_PROGRAM_CONTENT;
        /// <summary> 
        ///  加工程序内容
        /// </summary> 
        [DataMember]
        [Display(Name = "PROCESS_PROGRAM_CONTENT", Description = "加工程序内容")]
        public string PROCESS_PROGRAM_CONTENT
        {
            get { return _PROCESS_PROGRAM_CONTENT; }
            set { _PROCESS_PROGRAM_CONTENT = value; }
        }

        #endregion 加工程序内容

        #region 工序顺序

        private Int32? _PROCESS_INDEX;
        /// <summary> 
        ///  工序顺序
        /// </summary> 
        [DataMember]
        [Display(Name = "PROCESS_INDEX", Description = "工序顺序")]
        public Int32? PROCESS_INDEX
        {
            get { return _PROCESS_INDEX; }
            set { _PROCESS_INDEX = value; }
        }

        #endregion 工序顺序

        #region 生产前项目PKNO

        private string _BEGIN_ITEM_PKNO;
        /// <summary> 
        ///  生产前项目PKNO
        /// </summary> 
        [DataMember]
        [Display(Name = "BEGIN_ITEM_PKNO", Description = "生产前项目PKNO")]
        public string BEGIN_ITEM_PKNO
        {
            get { return _BEGIN_ITEM_PKNO; }
            set { _BEGIN_ITEM_PKNO = value; }
        }

        #endregion 生产前项目PKNO

        #region 生产后项目PKNO

        private string _FINISH_ITEM_PKNO;
        /// <summary> 
        ///  生产后项目PKNO
        /// </summary> 
        [DataMember]
        [Display(Name = "FINISH_ITEM_PKNO", Description = "生产后项目PKNO")]
        public string FINISH_ITEM_PKNO
        {
            get { return _FINISH_ITEM_PKNO; }
            set { _FINISH_ITEM_PKNO = value; }
        }

        #endregion 生产后项目PKNO

        #region 生产前位置

        private string _BEGIN_POSITION;
        /// <summary> 
        ///  生产前位置
        /// </summary> 
        [DataMember]
        [Display(Name = "BEGIN_POSITION", Description = "生产前位置")]
        public string BEGIN_POSITION
        {
            get { return _BEGIN_POSITION; }
            set { _BEGIN_POSITION = value; }
        }

        #endregion 生产前位置

        #region 生产后位置

        private string _FINISH_POSITION;
        /// <summary> 
        ///  生产后位置
        /// </summary> 
        [DataMember]
        [Display(Name = "FINISH_POSITION", Description = "生产后位置")]
        public string FINISH_POSITION
        {
            get { return _FINISH_POSITION; }
            set { _FINISH_POSITION = value; }
        }

        #endregion 生产后位置

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

        #region 工序动作类型

        private Int32? _PROCESS_ACTION_TYPE;
        /// <summary> 
        ///  工序动作类型
        /// </summary> 
        [DataMember]
        [Display(Name = "PROCESS_ACTION_TYPE", Description = "工序动作类型")]
        public Int32? PROCESS_ACTION_TYPE
        {
            get { return _PROCESS_ACTION_TYPE; }
            set { _PROCESS_ACTION_TYPE = value; }
        }

        #endregion 工序动作类型

        #region 工序动作控制PKNO

        private string _PROCESS_ACTION_PKNO;
        /// <summary> 
        ///  工序动作控制PKNO
        /// </summary> 
        [DataMember]
        [Display(Name = "PROCESS_ACTION_PKNO", Description = "工序动作控制PKNO")]
        public string PROCESS_ACTION_PKNO
        {
            get { return _PROCESS_ACTION_PKNO; }
            set { _PROCESS_ACTION_PKNO = value; }
        }

        #endregion 工序动作控制PKNO

        #region 工序动作参数1

        private string _PROCESS_ACTION_PARAM1_VALUE;
        /// <summary> 
        ///  工序动作参数1
        /// </summary> 
        [DataMember]
        [Display(Name = "PROCESS_ACTION_PARAM1_VALUE", Description = "工序动作参数1")]
        public string PROCESS_ACTION_PARAM1_VALUE
        {
            get { return _PROCESS_ACTION_PARAM1_VALUE; }
            set { _PROCESS_ACTION_PARAM1_VALUE = value; }
        }

        #endregion 工序动作参数1

        #region 工序动作参数2

        private string _PROCESS_ACTION_PARAM2_VALUE;
        /// <summary> 
        ///  工序动作参数2
        /// </summary> 
        [DataMember]
        [Display(Name = "PROCESS_ACTION_PARAM2_VALUE", Description = "工序动作参数2")]
        public string PROCESS_ACTION_PARAM2_VALUE
        {
            get { return _PROCESS_ACTION_PARAM2_VALUE; }
            set { _PROCESS_ACTION_PARAM2_VALUE = value; }
        }

        #endregion 工序动作参数2

        #region 当前生产加工的产品编码PKNO

        private string _CUR_PRODUCT_CODE_PKNO;
        /// <summary> 
        ///  当前生产加工的产品编码PKNO
        /// </summary> 
        [DataMember]
        [Display(Name = "CUR_PRODUCT_CODE_PKNO", Description = "当前生产加工的产品编码PKNO")]
        public string CUR_PRODUCT_CODE_PKNO
        {
            get { return _CUR_PRODUCT_CODE_PKNO; }
            set { _CUR_PRODUCT_CODE_PKNO = value; }
        }

        #endregion 当前生产加工的产品编码PKNO

        #region 加工数量（上线数量）

        private Decimal? _PROCESS_QTY;
        /// <summary> 
        ///  加工数量（上线数量）
        /// </summary> 
        [DataMember]
        [Display(Name = "PROCESS_QTY", Description = "加工数量（上线数量）")]
        public Decimal? PROCESS_QTY
        {
            get { return _PROCESS_QTY; }
            set { _PROCESS_QTY = value; }
        }

        #endregion 加工数量（上线数量）

        #region 完成数量（下线数量）

        private Decimal? _COMPLETE_QTY;
        /// <summary> 
        ///  完成数量（下线数量）
        /// </summary> 
        [DataMember]
        [Display(Name = "COMPLETE_QTY", Description = "完成数量（下线数量）")]
        public Decimal? COMPLETE_QTY
        {
            get { return _COMPLETE_QTY; }
            set { _COMPLETE_QTY = value; }
        }

        #endregion 完成数量（下线数量）

        #region 合格数量

        private Decimal? _QUALIFIED_QTY;
        /// <summary> 
        ///  合格数量
        /// </summary> 
        [DataMember]
        [Display(Name = "QUALIFIED_QTY", Description = "合格数量")]
        public Decimal? QUALIFIED_QTY
        {
            get { return _QUALIFIED_QTY; }
            set { _QUALIFIED_QTY = value; }
        }

        #endregion 合格数量

        #region 工序状态

        private Int32? _PROCESS_STATE;
        /// <summary> 
        ///  工序状态
        /// </summary> 
        [DataMember]
        [Display(Name = "PROCESS_STATE", Description = "工序状态")]
        public Int32? PROCESS_STATE
        {
            get { return _PROCESS_STATE; }
            set { _PROCESS_STATE = value; }
        }

        #endregion 工序状态

        #region 工序开始时间

        private DateTime? _PROCESS_START_TIME;
        /// <summary> 
        ///  工序开始时间
        /// </summary> 
        [DataMember]
        [Display(Name = "PROCESS_START_TIME", Description = "工序开始时间")]
        public DateTime? PROCESS_START_TIME
        {
            get { return _PROCESS_START_TIME; }
            set { _PROCESS_START_TIME = value; }
        }

        #endregion 工序开始时间

        #region 工序完成时间

        private DateTime? _PROCESS_END_TIME;
        /// <summary> 
        ///  工序完成时间
        /// </summary> 
        [DataMember]
        [Display(Name = "PROCESS_END_TIME", Description = "工序完成时间")]
        public DateTime? PROCESS_END_TIME
        {
            get { return _PROCESS_END_TIME; }
            set { _PROCESS_END_TIME = value; }
        }

        #endregion 工序完成时间

        #region 工序开始方式

        private Int32? _PROCESS_START_TYPE;
        /// <summary> 
        ///  工序开始方式
        /// </summary> 
        [DataMember]
        [Display(Name = "PROCESS_START_TYPE", Description = "工序开始方式")]
        public Int32? PROCESS_START_TYPE
        {
            get { return _PROCESS_START_TYPE; }
            set { _PROCESS_START_TYPE = value; }
        }

        #endregion 工序开始方式

        #region 工序完成方式

        private Int32? _PROCESS_END_TYPE;
        /// <summary> 
        ///  工序完成方式
        /// </summary> 
        [DataMember]
        [Display(Name = "PROCESS_END_TYPE", Description = "工序完成方式")]
        public Int32? PROCESS_END_TYPE
        {
            get { return _PROCESS_END_TYPE; }
            set { _PROCESS_END_TYPE = value; }
        }

        #endregion 工序完成方式

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
