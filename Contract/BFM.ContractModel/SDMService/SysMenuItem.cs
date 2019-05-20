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
    /// 菜单信息表
    /// </summary>
    [DataContract]
    public class SysMenuItem
    {

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

        #region 父节点编号

        private string _PARENT_PKNO;
        /// <summary> 
        ///  父节点编号
        /// </summary> 
        [DataMember]
        [Display(Name = "PARENT_PKNO", Description = "父节点编号")]
        public string PARENT_PKNO
        {
            get { return _PARENT_PKNO; }
            set { _PARENT_PKNO = value; }
        }

        #endregion 父节点编号

        #region 序号

        private Int32 _ITEM_SEQ;
        /// <summary> 
        ///  序号
        /// </summary> 
        [DataMember]
        [Display(Name = "ITEM_SEQ", Description = "序号")]
        public Int32 ITEM_SEQ
        {
            get { return _ITEM_SEQ; }
            set { _ITEM_SEQ = value; }
        }

        #endregion 序号

        #region 标题

        private string _ITEM_TITLE;
        /// <summary> 
        ///  标题
        /// </summary> 
        [DataMember]
        [Display(Name = "ITEM_TITLE", Description = "标题")]
        public string ITEM_TITLE
        {
            get { return _ITEM_TITLE; }
            set { _ITEM_TITLE = value; }
        }

        #endregion 标题

        #region 类型

        private string _ITEM_TYPE;
        /// <summary> 
        ///  类型
        /// </summary> 
        [DataMember]
        [Display(Name = "ITEM_TYPE", Description = "类型")]
        public string ITEM_TYPE
        {
            get { return _ITEM_TYPE; }
            set { _ITEM_TYPE = value; }
        }

        #endregion 类型

        #region 程序集

        private string _ASSEMBLY_NAME;
        /// <summary> 
        ///  程序集
        /// </summary> 
        [DataMember]
        [Display(Name = "ASSEMBLY_NAME", Description = "程序集")]
        public string ASSEMBLY_NAME
        {
            get { return _ASSEMBLY_NAME; }
            set { _ASSEMBLY_NAME = value; }
        }

        #endregion 程序集

        #region 所属页面ID

        private string _PAGE_ID;
        /// <summary> 
        ///  所属页面ID
        /// </summary> 
        [DataMember]
        [Display(Name = "PAGE_ID", Description = "所属页面ID")]
        public string PAGE_ID
        {
            get { return _PAGE_ID; }
            set { _PAGE_ID = value; }
        }

        #endregion 所属页面ID

        #region 页面参数

        private string _PAGE_PARAM;
        /// <summary> 
        ///  页面参数
        /// </summary> 
        [DataMember]
        [Display(Name = "PAGE_PARAM", Description = "页面参数")]
        public string PAGE_PARAM
        {
            get { return _PAGE_PARAM; }
            set { _PAGE_PARAM = value; }
        }

        #endregion 页面参数

        #region 是否启用

        private Int32 _USE_FLAG;
        /// <summary> 
        ///  是否启用
        /// </summary> 
        [DataMember]
        [Display(Name = "USE_FLAG", Description = "是否启用")]
        public Int32 USE_FLAG
        {
            get { return _USE_FLAG; }
            set { _USE_FLAG = value; }
        }

        #endregion 是否启用

        #region 图标路径

        private string _IMG_PATH;
        /// <summary> 
        ///  图标路径
        /// </summary> 
        [DataMember]
        [Display(Name = "IMG_PATH", Description = "图标路径")]
        public string IMG_PATH
        {
            get { return _IMG_PATH; }
            set { _IMG_PATH = value; }
        }

        #endregion 图标路径

        #region 目标框架

        private string _TARGET_NAME;
        /// <summary> 
        ///  目标框架
        /// </summary> 
        [DataMember]
        [Display(Name = "TARGET_NAME", Description = "目标框架")]
        public string TARGET_NAME
        {
            get { return _TARGET_NAME; }
            set { _TARGET_NAME = value; }
        }

        #endregion 目标框架

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
        
    }
}
