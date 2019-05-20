/*******************************************************************************
 * Copyright © 2018 代码生成器 版权所有
 * Author: LanGerp 
 * Description: 快速开发平台
*********************************************************************************/

using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BFM.ContractModel 
{
    /// <summary>
    /// 工艺路线明细表（工序表）
    /// </summary>
    [DataContract]
    public class RsRoutingDetail: INotifyPropertyChanged
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
            get
            {
                return _PKNO;
            }
            set
            {
                _PKNO = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PKNO"));
            }
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
            get
            {
                return _COMPANY_CODE;
            }
            set
            {
                _COMPANY_CODE = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("COMPANY_CODE"));
            }
        }

        #endregion 公司代码

        #region 工艺路线编号

        private string _ROUTING_PKNO;
        /// <summary> 
        ///  工艺路线编号
        /// </summary> 
        [DataMember]
        [Display(Name = "ROUTING_PKNO", Description = "工艺路线编号")]
        public string ROUTING_PKNO
        {
            get
            {
                return _ROUTING_PKNO;
            }
            set
            {
                _ROUTING_PKNO = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ROUTING_PKNO"));
            }
        }

        #endregion 工艺路线编号

        #region 工序号

        private string _OP_NO;
        /// <summary> 
        ///  工序号
        /// </summary> 
        [DataMember]
        [Display(Name = "OP_NO", Description = "工序号")]
        public string OP_NO
        {
            get
            {
                return _OP_NO;
            }
            set
            {
                _OP_NO = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OP_NO"));
            }
        }

        #endregion 工序号

        #region 工序名称

        private string _OP_NAME;
        /// <summary> 
        ///  工序名称
        /// </summary> 
        [DataMember]
        [Display(Name = "OP_NAME", Description = "工序名称")]
        public string OP_NAME
        {
            get
            {
                return _OP_NAME;
            }
            set
            {
                _OP_NAME = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OP_NAME"));
            }
        }

        #endregion 工序名称

        #region 工序说明

        private string _OP_NOTE;
        /// <summary> 
        ///  工序说明
        /// </summary> 
        [DataMember]
        [Display(Name = "OP_NOTE", Description = "工序说明")]
        public string OP_NOTE
        {
            get
            {
                return _OP_NOTE;
            }
            set
            {
                _OP_NOTE = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OP_NOTE"));
            }
        }

        #endregion 工序说明

        #region 加工中心

        private string _WC_CODE;
        /// <summary> 
        ///  加工中心
        /// </summary> 
        [DataMember]
        [Display(Name = "WC_CODE", Description = "加工中心")]
        public string WC_CODE
        {
            get
            {
                return _WC_CODE;
            }
            set
            {
                _WC_CODE = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WC_CODE"));
            }
        }

        #endregion 加工中心

        #region 加工中心简称

        private string _WC_ABV;
        /// <summary> 
        ///  加工中心简称
        /// </summary> 
        [DataMember]
        [Display(Name = "WC_ABV", Description = "加工中心简称")]
        public string WC_ABV
        {
            get
            {
                return _WC_ABV;
            }
            set
            {
                _WC_ABV = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WC_ABV"));
            }
        }

        #endregion 加工中心简称

        #region 工种

        private string _JOB_KIND;
        /// <summary> 
        ///  工种
        /// </summary> 
        [DataMember]
        [Display(Name = "JOB_KIND", Description = "工种")]
        public string JOB_KIND
        {
            get
            {
                return _JOB_KIND;
            }
            set
            {
                _JOB_KIND = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("JOB_KIND"));
            }
        }

        #endregion 工种

        #region 加工时间

        private Decimal _RUN_TIME;
        /// <summary> 
        ///  加工时间
        /// </summary> 
        [DataMember]
        [Display(Name = "RUN_TIME", Description = "加工时间")]
        public Decimal RUN_TIME
        {
            get
            {
                return _RUN_TIME;
            }
            set
            {
                _RUN_TIME = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RUN_TIME"));
            }
        }

        #endregion 加工时间

        #region 加工时间单位

        private string _RUN_UNIT;
        /// <summary> 
        ///  加工时间单位
        /// </summary> 
        [DataMember]
        [Display(Name = "RUN_UNIT", Description = "加工时间单位")]
        public string RUN_UNIT
        {
            get
            {
                return _RUN_UNIT;
            }
            set
            {
                _RUN_UNIT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RUN_UNIT"));
            }
        }

        #endregion 加工时间单位

        #region 首末标记

        private Int32 _FIRST_LAST;
        /// <summary> 
        ///  首末标记
        /// </summary> 
        [DataMember]
        [Display(Name = "FIRST_LAST", Description = "首末标记")]
        public Int32 FIRST_LAST
        {
            get
            {
                return _FIRST_LAST;
            }
            set
            {
                _FIRST_LAST = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FIRST_LAST"));
            }
        }

        #endregion 首末标记

        #region 准备工时

        private Decimal _SETUP_TIME;
        /// <summary> 
        ///  准备工时
        /// </summary> 
        [DataMember]
        [Display(Name = "SETUP_TIME", Description = "准备工时")]
        public Decimal SETUP_TIME
        {
            get
            {
                return _SETUP_TIME;
            }
            set
            {
                _SETUP_TIME = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SETUP_TIME"));
            }
        }

        #endregion 准备工时

        #region 准备工时单位

        private string _SETUP_UNIT;
        /// <summary> 
        ///  准备工时单位
        /// </summary> 
        [DataMember]
        [Display(Name = "SETUP_UNIT", Description = "准备工时单位")]
        public string SETUP_UNIT
        {
            get
            {
                return _SETUP_UNIT;
            }
            set
            {
                _SETUP_UNIT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SETUP_UNIT"));
            }
        }

        #endregion 准备工时单位

        #region 外协标识

        private Int32 _COOP_FLAG;
        /// <summary> 
        ///  外协标识
        /// </summary> 
        [DataMember]
        [Display(Name = "COOP_FLAG", Description = "外协标识")]
        public Int32 COOP_FLAG
        {
            get
            {
                return _COOP_FLAG;
            }
            set
            {
                _COOP_FLAG = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("COOP_FLAG"));
            }
        }

        #endregion 外协标识

        #region 外协时间

        private Decimal _COOP_TIME;
        /// <summary> 
        ///  外协时间
        /// </summary> 
        [DataMember]
        [Display(Name = "COOP_TIME", Description = "外协时间")]
        public Decimal COOP_TIME
        {
            get
            {
                return _COOP_TIME;
            }
            set
            {
                _COOP_TIME = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("COOP_TIME"));
            }
        }

        #endregion 外协时间

        #region 质量控制标识

        private Int32 _QC_FLAG;
        /// <summary> 
        ///  质量控制标识
        /// </summary> 
        [DataMember]
        [Display(Name = "QC_FLAG", Description = "质量控制标识")]
        public Int32 QC_FLAG
        {
            get
            {
                return _QC_FLAG;
            }
            set
            {
                _QC_FLAG = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("QC_FLAG"));
            }
        }

        #endregion 质量控制标识

        #region 工装

        private string _TOOL_DESC;
        /// <summary> 
        ///  工装
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOL_DESC", Description = "工装")]
        public string TOOL_DESC
        {
            get
            {
                return _TOOL_DESC;
            }
            set
            {
                _TOOL_DESC = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TOOL_DESC"));
            }
        }

        #endregion 工装

        #region 工装标识

        private string _TOOL_FLAG;
        /// <summary> 
        ///  工装标识
        /// </summary> 
        [DataMember]
        [Display(Name = "TOOL_FLAG", Description = "工装标识")]
        public string TOOL_FLAG
        {
            get
            {
                return _TOOL_FLAG;
            }
            set
            {
                _TOOL_FLAG = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TOOL_FLAG"));
            }
        }

        #endregion 工装标识

        #region NC程序名称

        private string _NC_PRO_NAME;
        /// <summary> 
        ///  NC程序名称
        /// </summary> 
        [DataMember]
        [Display(Name = "NC_PRO_NAME", Description = "NC程序名称")]
        public string NC_PRO_NAME
        {
            get
            {
                return _NC_PRO_NAME;
            }
            set
            {
                _NC_PRO_NAME = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NC_PRO_NAME"));
            }
        }

        #endregion NC程序名称

        #region NC程序内容

        private byte[] _NC_PRO_INFO;
        /// <summary> 
        ///  NC程序内容
        /// </summary> 
        [DataMember]
        [Display(Name = "NC_PRO_INFO", Description = "NC程序内容")]
        public byte[] NC_PRO_INFO
        {
            get
            {
                return _NC_PRO_INFO;
            }
            set
            {
                _NC_PRO_INFO = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NC_PRO_INFO"));
            }
        }

        #endregion NC程序内容

        #region 工序内容

        private string _OP_DESC_XY;
        /// <summary> 
        ///  工序内容
        /// </summary> 
        [DataMember]
        [Display(Name = "OP_DESC_XY", Description = "工序内容")]
        public string OP_DESC_XY
        {
            get
            {
                return _OP_DESC_XY;
            }
            set
            {
                _OP_DESC_XY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OP_DESC_XY"));
            }
        }

        #endregion 工序内容

        #region 量具

        private string _MEASURE_EQUIP_XY;
        /// <summary> 
        ///  量具
        /// </summary> 
        [DataMember]
        [Display(Name = "MEASURE_EQUIP_XY", Description = "量具")]
        public string MEASURE_EQUIP_XY
        {
            get
            {
                return _MEASURE_EQUIP_XY;
            }
            set
            {
                _MEASURE_EQUIP_XY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MEASURE_EQUIP_XY"));
            }
        }

        #endregion 量具

        #region 刀具

        private string _CUTLER_EQUIP_XY;
        /// <summary> 
        ///  刀具
        /// </summary> 
        [DataMember]
        [Display(Name = "CUTLER_EQUIP_XY", Description = "刀具")]
        public string CUTLER_EQUIP_XY
        {
            get
            {
                return _CUTLER_EQUIP_XY;
            }
            set
            {
                _CUTLER_EQUIP_XY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CUTLER_EQUIP_XY"));
            }
        }

        #endregion 刀具

        #region 工序内容简称

        private string _OP_ABV_XY;
        /// <summary> 
        ///  工序内容简称
        /// </summary> 
        [DataMember]
        [Display(Name = "OP_ABV_XY", Description = "工序内容简称")]
        public string OP_ABV_XY
        {
            get
            {
                return _OP_ABV_XY;
            }
            set
            {
                _OP_ABV_XY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OP_ABV_XY"));
            }
        }

        #endregion 工序内容简称

        #region 工序类型

        private string _OP_TYPE;
        /// <summary> 
        ///  工序类型
        /// </summary> 
        [DataMember]
        [Display(Name = "OP_TYPE", Description = "工序类型")]
        public string OP_TYPE
        {
            get
            {
                return _OP_TYPE;
            }
            set
            {
                _OP_TYPE = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OP_TYPE"));
            }
        }

        #endregion 工序类型

        #region 工序顺序

        private Int32 _OP_INDEX;
        /// <summary> 
        ///  工序顺序
        /// </summary> 
        [DataMember]
        [Display(Name = "OP_INDEX", Description = "工序顺序")]
        public Int32 OP_INDEX
        {
            get
            {
                return _OP_INDEX;
            }
            set
            {
                _OP_INDEX = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OP_INDEX"));
            }
        }

        #endregion 工序顺序

        #region 工序动作类型

        private Int32? _PROCESS_ACTION_TYPE;
        /// <summary> 
        ///  工序动作类型
        /// </summary> 
        [DataMember]
        [Display(Name = "PROCESS_ACTION_TYPE", Description = "工序动作类型")]
        public Int32? PROCESS_ACTION_TYPE
        {
            get
            {
                return _PROCESS_ACTION_TYPE;
            }
            set
            {
                _PROCESS_ACTION_TYPE = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PROCESS_ACTION_TYPE"));
            }
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
            get
            {
                return _PROCESS_ACTION_PKNO;
            }
            set
            {
                _PROCESS_ACTION_PKNO = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PROCESS_ACTION_PKNO"));
            }
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
            get
            {
                return _PROCESS_ACTION_PARAM1_VALUE;
            }
            set
            {
                _PROCESS_ACTION_PARAM1_VALUE = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PROCESS_ACTION_PARAM1_VALUE"));
            }
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
            get
            {
                return _PROCESS_ACTION_PARAM2_VALUE;
            }
            set
            {
                _PROCESS_ACTION_PARAM2_VALUE = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PROCESS_ACTION_PARAM2_VALUE"));
            }
        }

        #endregion 工序动作参数2

        #region 创建日期

        private DateTime? _CREATION_DATE;
        /// <summary> 
        ///  创建日期
        /// </summary> 
        [DataMember]
        [Display(Name = "CREATION_DATE", Description = "创建日期")]
        public DateTime? CREATION_DATE
        {
            get
            {
                return _CREATION_DATE;
            }
            set
            {
                _CREATION_DATE = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CREATION_DATE"));
            }
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
            get
            {
                return _CREATED_BY;
            }
            set
            {
                _CREATED_BY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CREATED_BY"));
            }
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
            get
            {
                return _LAST_UPDATE_DATE;
            }
            set
            {
                _LAST_UPDATE_DATE = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LAST_UPDATE_DATE"));
            }
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
            get
            {
                return _UPDATED_BY;
            }
            set
            {
                _UPDATED_BY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UPDATED_BY"));
            }
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
            get
            {
                return _UPDATED_INTROD;
            }
            set
            {
                _UPDATED_INTROD = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UPDATED_INTROD"));
            }
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
            get
            {
                return _USE_FLAG;
            }
            set
            {
                _USE_FLAG = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("USE_FLAG"));
            }
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
            get
            {
                return _REMARK;
            }
            set
            {
                _REMARK = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("REMARK"));
            }
        }

        #endregion 备注

        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
