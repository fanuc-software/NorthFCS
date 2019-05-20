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
    /// 库存信息表
    /// </summary>
    [DataContract]
    public class WmsInventory
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

        #region 物料编号

        private string _MATERIAL_PKNO;
        /// <summary> 
        ///  物料编号
        /// </summary> 
        [DataMember]
        [Display(Name = "MATERIAL_PKNO", Description = "物料编号")]
        public string MATERIAL_PKNO
        {
            get { return _MATERIAL_PKNO; }
            set { _MATERIAL_PKNO = value; }
        }

        #endregion 物料编号

        #region 货位地址

        private string _ALLOCATION_PKNO;
        /// <summary> 
        ///  货位地址
        /// </summary> 
        [DataMember]
        [Display(Name = "ALLOCATION_PKNO", Description = "货位地址")]
        public string ALLOCATION_PKNO
        {
            get { return _ALLOCATION_PKNO; }
            set { _ALLOCATION_PKNO = value; }
        }

        #endregion 货位地址

        #region 库区PKNO

        private string _AREA_PKNO;
        /// <summary> 
        ///  库区PKNO
        /// </summary> 
        [DataMember]
        [Display(Name = "AREA_PKNO", Description = "库区PKNO")]
        public string AREA_PKNO
        {
            get { return _AREA_PKNO; }
            set { _AREA_PKNO = value; }
        }

        #endregion 库区PKNO

        #region 批次号

        private string _BATCH_NO;
        /// <summary> 
        ///  批次号
        /// </summary> 
        [DataMember]
        [Display(Name = "BATCH_NO", Description = "批次号")]
        public string BATCH_NO
        {
            get { return _BATCH_NO; }
            set { _BATCH_NO = value; }
        }

        #endregion 批次号

        #region 库存数量

        private Decimal? _INVENTORY_NUM;
        /// <summary> 
        ///  库存数量
        /// </summary> 
        [DataMember]
        [Display(Name = "INVENTORY_NUM", Description = "库存数量")]
        public Decimal? INVENTORY_NUM
        {
            get { return _INVENTORY_NUM; }
            set { _INVENTORY_NUM = value; }
        }

        #endregion 库存数量

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
