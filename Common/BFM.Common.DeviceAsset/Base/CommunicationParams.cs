using System;
using System.ComponentModel;

namespace BFM.Common.DeviceAsset
{
    /// <summary>
    /// 设备通讯标签参数
    /// </summary>
    public class DeviceTagParam
    {
        #region 属性

        /// <summary>
        /// 标签唯一编号
        /// </summary>
        public string PKNO { get; set; }

        /// <summary>
        /// Handel
        /// </summary>
        public string Handel { get; set; }

        /// <summary>
        /// 标签编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public TagDataType DataType { get; set; } = TagDataType.Default;

        /// <summary>
        /// 采用模式
        /// </summary>
        public DataSimplingMode SimplingMode { get; set; } = DataSimplingMode.AutoReadDevice;

        #endregion
        
        public string ReadHandel { get; set; } //当前读取数据的Handel
        public string WriteHandel { get; set; } //当前写入数据的Handel
        public DateTime ReadTime { get; set; } //读取时间
        public DateTime WriteTime { get; set; } //写入时间

        public string CurValue;  //当前值

        /// <summary>
        /// 通讯设备
        /// </summary>
        public DeviceManager _DeviceManager;

        #region Constructor

        public DeviceTagParam(string tagPKNO, string tagAddress, DeviceManager deviceManager)
        {
            PKNO = tagPKNO;
            Handel = PKNO;
            Name = "临时读写";
            Address = tagAddress;
            _DeviceManager = deviceManager;
        }

        public DeviceTagParam(string tagPKNO, string tagCode, string tagName, string tagAddress, TagDataType tagDataType,
            DataSimplingMode simplingMode, DeviceManager deviceManager)
        {
            PKNO = tagPKNO;
            Handel = PKNO;
            Code = tagCode;
            Name = tagName;
            Address = tagAddress;
            DataType = tagDataType;
            SimplingMode = simplingMode;
            _DeviceManager = deviceManager;
        }

        #endregion

        public void AddNewReadRecord(string msg)
        {

        }

        public void AddNewWriteRecord(string msg)
        {

        }
    }

    /// <summary>
    /// 设备通讯参数
    /// </summary>
    public class DeviceCommParam
    {
        #region 属性

        /// <summary>
        /// 设备唯一编号
        /// </summary>
        public string PKNO { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 通讯接口类型
        /// </summary>
        public DeviceCommInterface CommInterface { get; set; }

        /// <summary>
        /// 通讯地址
        /// </summary>
        public string CommAddress { get; set; }

        /// <summary>
        /// 数据刷新率 单位：ms
        /// </summary>
        public int UpdateRate { get; set; } = 1000;

        /// <summary>
        /// 设备的状态
        /// </summary>
        public DeviceState State { get; set; } = DeviceState.Offline;

        #endregion

        #region Constructor

        public DeviceCommParam(DeviceCommInterface commType, string deviceAddress)
        {
            PKNO = Guid.NewGuid().ToString("N");
            CommInterface = commType;
            CommAddress = deviceAddress;

        }

        public DeviceCommParam(string devicePKNO, DeviceCommInterface commType, string deviceAddress)
        {
            PKNO = devicePKNO;
            CommInterface = commType;
            CommAddress = deviceAddress;
        }

        public DeviceCommParam(string devicePKNO, DeviceCommInterface commType, string deviceAddress, int updateRate)
        {
            PKNO = devicePKNO;
            CommInterface = commType;
            CommAddress = deviceAddress;
            UpdateRate = updateRate;
        }
        
        #endregion

    }
}
