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
    /// 动作配方明细表
    /// </summary>
    [DataContract]
    public class FmsActionFormulaDetail: INotifyPropertyChanged
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
            get
            {
                return _PKNO;
            }
            set
            {
                _PKNO = value;
                OnPropertyChanged("PKNO");
            }
        }

        #endregion 唯一编号

        #region 配方编码

        private string _FORMULA_CODE;
        /// <summary> 
        ///  配方编码
        /// </summary> 
        [DataMember]
        [Display(Name = "FORMULA_CODE", Description = "配方编码")]
        public string FORMULA_CODE
        {
            get
            {
                return _FORMULA_CODE;
            }
            set
            {
                _FORMULA_CODE = value;
                OnPropertyChanged("FORMULA_CODE");
            }
        }

        #endregion 配方编码

        #region 配方明细名称

        private string _FORMULA_DETAIL_NAME;
        /// <summary> 
        ///  配方明细名称
        /// </summary> 
        [DataMember]
        [Display(Name = "FORMULA_DETAIL_NAME", Description = "配方明细名称")]
        public string FORMULA_DETAIL_NAME
        {
            get
            {
                return _FORMULA_DETAIL_NAME;
            }
            set
            {
                _FORMULA_DETAIL_NAME = value;
                OnPropertyChanged("FORMULA_DETAIL_NAME");
            }
        }

        #endregion 配方明细名称

        #region 工序编号（工艺路线明细）

        private string _ROUTING_DETAIL_PKNO;
        /// <summary> 
        ///  工序编号（工艺路线明细）
        /// </summary> 
        [DataMember]
        [Display(Name = "ROUTING_DETAIL_PKNO", Description = "工序编号（工艺路线明细）")]
        public string ROUTING_DETAIL_PKNO
        {
            get
            {
                return _ROUTING_DETAIL_PKNO;
            }
            set
            {
                _ROUTING_DETAIL_PKNO = value;
                OnPropertyChanged("ROUTING_DETAIL_PKNO");
            }
        }

        #endregion 工序编号（工艺路线明细）

        #region 生产设备

        private string _PROCESS_DEVICE_PKNO;
        /// <summary> 
        ///  生产设备
        /// </summary> 
        [DataMember]
        [Display(Name = "PROCESS_DEVICE_PKNO", Description = "生产设备")]
        public string PROCESS_DEVICE_PKNO
        {
            get
            {
                return _PROCESS_DEVICE_PKNO;
            }
            set
            {
                _PROCESS_DEVICE_PKNO = value;
                OnPropertyChanged("PROCESS_DEVICE_PKNO");
            }
        }

        #endregion 生产设备

        #region 加工程序号

        private string _PROCESS_PROGRAM_NO;
        /// <summary> 
        ///  加工程序号
        /// </summary> 
        [DataMember]
        [Display(Name = "PROCESS_PROGRAM_NO", Description = "加工程序号")]
        public string PROCESS_PROGRAM_NO
        {
            get
            {
                return _PROCESS_PROGRAM_NO;
            }
            set
            {
                _PROCESS_PROGRAM_NO = value;
                OnPropertyChanged("PROCESS_PROGRAM_NO");
            }
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
            get
            {
                return _PROCESS_PROGRAM_CONTENT;
            }
            set
            {
                _PROCESS_PROGRAM_CONTENT = value;
                OnPropertyChanged("PROCESS_PROGRAM_CONTENT");
            }
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
            get
            {
                return _PROCESS_INDEX;
            }
            set
            {
                _PROCESS_INDEX = value;
                OnPropertyChanged("PROCESS_INDEX");
            }
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
            get
            {
                return _BEGIN_ITEM_PKNO;
            }
            set
            {
                _BEGIN_ITEM_PKNO = value;
                OnPropertyChanged("BEGIN_ITEM_PKNO");
            }
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
            get
            {
                return _FINISH_ITEM_PKNO;
            }
            set
            {
                _FINISH_ITEM_PKNO = value;
                OnPropertyChanged("FINISH_ITEM_PKNO");
            }
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
            get
            {
                return _BEGIN_POSITION;
            }
            set
            {
                _BEGIN_POSITION = value;
                OnPropertyChanged("BEGIN_POSITION");
            }
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
            get
            {
                return _FINISH_POSITION;
            }
            set
            {
                _FINISH_POSITION = value;
                OnPropertyChanged("FINISH_POSITION");
            }
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
            get
            {
                return _PALLET_NO;
            }
            set
            {
                _PALLET_NO = value;
                OnPropertyChanged("PALLET_NO");
            }
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
            get
            {
                return _PROCESS_ACTION_TYPE;
            }
            set
            {
                _PROCESS_ACTION_TYPE = value;
                OnPropertyChanged("PROCESS_ACTION_TYPE");
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
                OnPropertyChanged("PROCESS_ACTION_PKNO");
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
                OnPropertyChanged("PROCESS_ACTION_PARAM1_VALUE");
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
                OnPropertyChanged("PROCESS_ACTION_PARAM2_VALUE");
            }
        }

        #endregion 工序动作参数2

        #region 启用标识

        private Int32? _USE_FLAG;
        /// <summary> 
        ///  启用标识
        /// </summary> 
        [DataMember]
        [Display(Name = "USE_FLAG", Description = "启用标识")]
        public Int32? USE_FLAG
        {
            get
            {
                return _USE_FLAG;
            }
            set
            {
                _USE_FLAG = value;
                OnPropertyChanged("USE_FLAG");
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
                OnPropertyChanged("REMARK");
            }
        }

        #endregion 备注

        #region mvvm

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion mvvm

    }
}
