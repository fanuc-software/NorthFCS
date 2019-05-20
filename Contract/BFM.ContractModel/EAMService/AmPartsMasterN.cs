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
    /// 备件台账表N
    /// </summary>
    [DataContract]
    public class AmPartsMasterN
    {

        #region 备件编号

        private string _PARTS_CODE;
        /// <summary> 
        ///  备件编号
        /// </summary> 
        [DataMember]
        [Display(Name = "PARTS_CODE", Description = "备件编号")]
        public string PARTS_CODE
        {
            get { return _PARTS_CODE; }
            set { _PARTS_CODE = value; }
        }

        #endregion 备件编号

        #region 唯一标识

        private string _PKNO;
        /// <summary> 
        ///  唯一标识
        /// </summary> 
        [DataMember]
        [Display(Name = "PKNO", Description = "唯一标识")]
        public string PKNO
        {
            get { return _PKNO; }
            set { _PKNO = value; }
        }

        #endregion 唯一标识

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

        #region 备件名称

        private string _PARTS_NAME;
        /// <summary> 
        ///  备件名称
        /// </summary> 
        [DataMember]
        [Display(Name = "PARTS_NAME", Description = "备件名称")]
        public string PARTS_NAME
        {
            get { return _PARTS_NAME; }
            set { _PARTS_NAME = value; }
        }

        #endregion 备件名称

        #region 备件规格

        private string _PARTS_NORM;
        /// <summary> 
        ///  备件规格
        /// </summary> 
        [DataMember]
        [Display(Name = "PARTS_NORM", Description = "备件规格")]
        public string PARTS_NORM
        {
            get { return _PARTS_NORM; }
            set { _PARTS_NORM = value; }
        }

        #endregion 备件规格

        #region 备件型号

        private string _PARTS_MODEL;
        /// <summary> 
        ///  备件型号
        /// </summary> 
        [DataMember]
        [Display(Name = "PARTS_MODEL", Description = "备件型号")]
        public string PARTS_MODEL
        {
            get { return _PARTS_MODEL; }
            set { _PARTS_MODEL = value; }
        }

        #endregion 备件型号

        #region 类别编号

        private string _TYPE_NO;
        /// <summary> 
        ///  类别编号
        /// </summary> 
        [DataMember]
        [Display(Name = "TYPE_NO", Description = "类别编号")]
        public string TYPE_NO
        {
            get { return _TYPE_NO; }
            set { _TYPE_NO = value; }
        }

        #endregion 类别编号

        #region 类别名称

        private string _TYPE_NAME;
        /// <summary> 
        ///  类别名称
        /// </summary> 
        [DataMember]
        [Display(Name = "TYPE_NAME", Description = "类别名称")]
        public string TYPE_NAME
        {
            get { return _TYPE_NAME; }
            set { _TYPE_NAME = value; }
        }

        #endregion 类别名称

        #region 备件类别

        private string _PARTS_TYPE;
        /// <summary> 
        ///  备件类别
        /// </summary> 
        [DataMember]
        [Display(Name = "PARTS_TYPE", Description = "备件类别")]
        public string PARTS_TYPE
        {
            get { return _PARTS_TYPE; }
            set { _PARTS_TYPE = value; }
        }

        #endregion 备件类别

        #region 项目图号

        private string _DRAWING_NO;
        /// <summary> 
        ///  项目图号
        /// </summary> 
        [DataMember]
        [Display(Name = "DRAWING_NO", Description = "项目图号")]
        public string DRAWING_NO
        {
            get { return _DRAWING_NO; }
            set { _DRAWING_NO = value; }
        }

        #endregion 项目图号

        #region 备件号

        private string _STANDARD_NO;
        /// <summary> 
        ///  备件号
        /// </summary> 
        [DataMember]
        [Display(Name = "STANDARD_NO", Description = "备件号")]
        public string STANDARD_NO
        {
            get { return _STANDARD_NO; }
            set { _STANDARD_NO = value; }
        }

        #endregion 备件号

        #region 设备类型

        private string _EQUIP_TYPE;
        /// <summary> 
        ///  设备类型
        /// </summary> 
        [DataMember]
        [Display(Name = "EQUIP_TYPE", Description = "设备类型")]
        public string EQUIP_TYPE
        {
            get { return _EQUIP_TYPE; }
            set { _EQUIP_TYPE = value; }
        }

        #endregion 设备类型

        #region 部位

        private string _USE_PART;
        /// <summary> 
        ///  部位
        /// </summary> 
        [DataMember]
        [Display(Name = "USE_PART", Description = "部位")]
        public string USE_PART
        {
            get { return _USE_PART; }
            set { _USE_PART = value; }
        }

        #endregion 部位

        #region 供应商编号

        private string _SUPPLIER_ID;
        /// <summary> 
        ///  供应商编号
        /// </summary> 
        [DataMember]
        [Display(Name = "SUPPLIER_ID", Description = "供应商编号")]
        public string SUPPLIER_ID
        {
            get { return _SUPPLIER_ID; }
            set { _SUPPLIER_ID = value; }
        }

        #endregion 供应商编号

        #region 供应商名称

        private string _SUPPLIER_NAME;
        /// <summary> 
        ///  供应商名称
        /// </summary> 
        [DataMember]
        [Display(Name = "SUPPLIER_NAME", Description = "供应商名称")]
        public string SUPPLIER_NAME
        {
            get { return _SUPPLIER_NAME; }
            set { _SUPPLIER_NAME = value; }
        }

        #endregion 供应商名称

        #region 更换周期

        private string _EXCHANGE_PERIOD;
        /// <summary> 
        ///  更换周期
        /// </summary> 
        [DataMember]
        [Display(Name = "EXCHANGE_PERIOD", Description = "更换周期")]
        public string EXCHANGE_PERIOD
        {
            get { return _EXCHANGE_PERIOD; }
            set { _EXCHANGE_PERIOD = value; }
        }

        #endregion 更换周期

        #region 数量

        private Int32 _ITEM_QTY;
        /// <summary> 
        ///  数量
        /// </summary> 
        [DataMember]
        [Display(Name = "ITEM_QTY", Description = "数量")]
        public Int32 ITEM_QTY
        {
            get { return _ITEM_QTY; }
            set { _ITEM_QTY = value; }
        }

        #endregion 数量

        #region 单价

        private Decimal _UNIT_PRICE;
        /// <summary> 
        ///  单价
        /// </summary> 
        [DataMember]
        [Display(Name = "UNIT_PRICE", Description = "单价")]
        public Decimal UNIT_PRICE
        {
            get { return _UNIT_PRICE; }
            set { _UNIT_PRICE = value; }
        }

        #endregion 单价

        #region 父项代码

        private string _PARENT_CODE;
        /// <summary> 
        ///  父项代码
        /// </summary> 
        [DataMember]
        [Display(Name = "PARENT_CODE", Description = "父项代码")]
        public string PARENT_CODE
        {
            get { return _PARENT_CODE; }
            set { _PARENT_CODE = value; }
        }

        #endregion 父项代码

        #region 父项名称

        private string _PARENT_NAME;
        /// <summary> 
        ///  父项名称
        /// </summary> 
        [DataMember]
        [Display(Name = "PARENT_NAME", Description = "父项名称")]
        public string PARENT_NAME
        {
            get { return _PARENT_NAME; }
            set { _PARENT_NAME = value; }
        }

        #endregion 父项名称

        #region 计量单位

        private string _ASSET_UNIT;
        /// <summary> 
        ///  计量单位
        /// </summary> 
        [DataMember]
        [Display(Name = "ASSET_UNIT", Description = "计量单位")]
        public string ASSET_UNIT
        {
            get { return _ASSET_UNIT; }
            set { _ASSET_UNIT = value; }
        }

        #endregion 计量单位

        #region 部门编码

        private string _DEPARTMENT_CODE;
        /// <summary> 
        ///  部门编码
        /// </summary> 
        [DataMember]
        [Display(Name = "DEPARTMENT_CODE", Description = "部门编码")]
        public string DEPARTMENT_CODE
        {
            get { return _DEPARTMENT_CODE; }
            set { _DEPARTMENT_CODE = value; }
        }

        #endregion 部门编码

        #region 部门名称

        private string _DEPARTMENT_NAME;
        /// <summary> 
        ///  部门名称
        /// </summary> 
        [DataMember]
        [Display(Name = "DEPARTMENT_NAME", Description = "部门名称")]
        public string DEPARTMENT_NAME
        {
            get { return _DEPARTMENT_NAME; }
            set { _DEPARTMENT_NAME = value; }
        }

        #endregion 部门名称

        #region 使用部门编号

        private string _USE_DEPT_CODE;
        /// <summary> 
        ///  使用部门编号
        /// </summary> 
        [DataMember]
        [Display(Name = "USE_DEPT_CODE", Description = "使用部门编号")]
        public string USE_DEPT_CODE
        {
            get { return _USE_DEPT_CODE; }
            set { _USE_DEPT_CODE = value; }
        }

        #endregion 使用部门编号

        #region 使用部门名称

        private string _USE_DEPT_NAME;
        /// <summary> 
        ///  使用部门名称
        /// </summary> 
        [DataMember]
        [Display(Name = "USE_DEPT_NAME", Description = "使用部门名称")]
        public string USE_DEPT_NAME
        {
            get { return _USE_DEPT_NAME; }
            set { _USE_DEPT_NAME = value; }
        }

        #endregion 使用部门名称

        #region 存放地点

        private string _LAY_LOCATION;
        /// <summary> 
        ///  存放地点
        /// </summary> 
        [DataMember]
        [Display(Name = "LAY_LOCATION", Description = "存放地点")]
        public string LAY_LOCATION
        {
            get { return _LAY_LOCATION; }
            set { _LAY_LOCATION = value; }
        }

        #endregion 存放地点

        #region 安装区域

        private string _LAY_REGION;
        /// <summary> 
        ///  安装区域
        /// </summary> 
        [DataMember]
        [Display(Name = "LAY_REGION", Description = "安装区域")]
        public string LAY_REGION
        {
            get { return _LAY_REGION; }
            set { _LAY_REGION = value; }
        }

        #endregion 安装区域

        #region 登记人

        private string _REGISTER_NAME;
        /// <summary> 
        ///  登记人
        /// </summary> 
        [DataMember]
        [Display(Name = "REGISTER_NAME", Description = "登记人")]
        public string REGISTER_NAME
        {
            get { return _REGISTER_NAME; }
            set { _REGISTER_NAME = value; }
        }

        #endregion 登记人

        #region 安装日期

        private DateTime? _INSTALL_DATE;
        /// <summary> 
        ///  安装日期
        /// </summary> 
        [DataMember]
        [Display(Name = "INSTALL_DATE", Description = "安装日期")]
        public DateTime? INSTALL_DATE
        {
            get { return _INSTALL_DATE; }
            set { _INSTALL_DATE = value; }
        }

        #endregion 安装日期

        #region 启用日期

        private DateTime? _START_USE_DATE;
        /// <summary> 
        ///  启用日期
        /// </summary> 
        [DataMember]
        [Display(Name = "START_USE_DATE", Description = "启用日期")]
        public DateTime? START_USE_DATE
        {
            get { return _START_USE_DATE; }
            set { _START_USE_DATE = value; }
        }

        #endregion 启用日期

        #region 资产原值

        private Decimal _NET_VALUE;
        /// <summary> 
        ///  资产原值
        /// </summary> 
        [DataMember]
        [Display(Name = "NET_VALUE", Description = "资产原值")]
        public Decimal NET_VALUE
        {
            get { return _NET_VALUE; }
            set { _NET_VALUE = value; }
        }

        #endregion 资产原值

        #region 折旧值

        private Decimal _DEPRECIATION_VALUE;
        /// <summary> 
        ///  折旧值
        /// </summary> 
        [DataMember]
        [Display(Name = "DEPRECIATION_VALUE", Description = "折旧值")]
        public Decimal DEPRECIATION_VALUE
        {
            get { return _DEPRECIATION_VALUE; }
            set { _DEPRECIATION_VALUE = value; }
        }

        #endregion 折旧值

        #region 重估值

        private Decimal _RECKON_VALUE;
        /// <summary> 
        ///  重估值
        /// </summary> 
        [DataMember]
        [Display(Name = "RECKON_VALUE", Description = "重估值")]
        public Decimal RECKON_VALUE
        {
            get { return _RECKON_VALUE; }
            set { _RECKON_VALUE = value; }
        }

        #endregion 重估值

        #region 折旧日期

        private DateTime? _DEPRECIATE_DATE;
        /// <summary> 
        ///  折旧日期
        /// </summary> 
        [DataMember]
        [Display(Name = "DEPRECIATE_DATE", Description = "折旧日期")]
        public DateTime? DEPRECIATE_DATE
        {
            get { return _DEPRECIATE_DATE; }
            set { _DEPRECIATE_DATE = value; }
        }

        #endregion 折旧日期

        #region 使用状态

        private string _USE_STATE;
        /// <summary> 
        ///  使用状态
        /// </summary> 
        [DataMember]
        [Display(Name = "USE_STATE", Description = "使用状态")]
        public string USE_STATE
        {
            get { return _USE_STATE; }
            set { _USE_STATE = value; }
        }

        #endregion 使用状态

        #region 台账状态

        private string _ACCT_STATE;
        /// <summary> 
        ///  台账状态
        /// </summary> 
        [DataMember]
        [Display(Name = "ACCT_STATE", Description = "台账状态")]
        public string ACCT_STATE
        {
            get { return _ACCT_STATE; }
            set { _ACCT_STATE = value; }
        }

        #endregion 台账状态

        #region 盘点状态

        private string _CHECK_STATE;
        /// <summary> 
        ///  盘点状态
        /// </summary> 
        [DataMember]
        [Display(Name = "CHECK_STATE", Description = "盘点状态")]
        public string CHECK_STATE
        {
            get { return _CHECK_STATE; }
            set { _CHECK_STATE = value; }
        }

        #endregion 盘点状态

        #region 交接日期

        private DateTime? _HAND_OVER_DATE;
        /// <summary> 
        ///  交接日期
        /// </summary> 
        [DataMember]
        [Display(Name = "HAND_OVER_DATE", Description = "交接日期")]
        public DateTime? HAND_OVER_DATE
        {
            get { return _HAND_OVER_DATE; }
            set { _HAND_OVER_DATE = value; }
        }

        #endregion 交接日期

        #region 登记日期

        private DateTime? _REG_DATE;
        /// <summary> 
        ///  登记日期
        /// </summary> 
        [DataMember]
        [Display(Name = "REG_DATE", Description = "登记日期")]
        public DateTime? REG_DATE
        {
            get { return _REG_DATE; }
            set { _REG_DATE = value; }
        }

        #endregion 登记日期

        #region 负责人

        private string _MANAGER_NAME;
        /// <summary> 
        ///  负责人
        /// </summary> 
        [DataMember]
        [Display(Name = "MANAGER_NAME", Description = "负责人")]
        public string MANAGER_NAME
        {
            get { return _MANAGER_NAME; }
            set { _MANAGER_NAME = value; }
        }

        #endregion 负责人

        #region 用途

        private string _ASSET_USE;
        /// <summary> 
        ///  用途
        /// </summary> 
        [DataMember]
        [Display(Name = "ASSET_USE", Description = "用途")]
        public string ASSET_USE
        {
            get { return _ASSET_USE; }
            set { _ASSET_USE = value; }
        }

        #endregion 用途

        #region 关键工位

        private Int32 _KEY_FLAG;
        /// <summary> 
        ///  关键工位
        /// </summary> 
        [DataMember]
        [Display(Name = "KEY_FLAG", Description = "关键工位")]
        public Int32 KEY_FLAG
        {
            get { return _KEY_FLAG; }
            set { _KEY_FLAG = value; }
        }

        #endregion 关键工位

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
