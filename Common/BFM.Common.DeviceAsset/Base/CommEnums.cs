using System.ComponentModel;

namespace BFM.Common.DeviceAsset
{
    /// <summary>
    /// 设备通讯接口类型 
    /// 目前支持1：Fanuc CNC;21:OPC Classic;22:OPC UA;30:自定义TCP;31:Modbus TCP;100:数据库
    /// </summary>
    public enum DeviceCommInterface
    {
        /// <summary>
        /// 机床 Fanuc CNC 
        /// </summary>
        [Description("机床 Fanuc CNC")]
        CNC_Fanuc = 1,

        /// <summary>
        /// Fanuc机器人协议
        /// </summary>
        [Description("Fanuc机器人")]
        FanucRobot = 301,

        /// <summary>
        /// 西斯特姆 Modula 自动货柜
        /// </summary>
        [Description("Modula 自动货柜")]
        ModulaTCP = 302,

        /// <summary>
        /// 蔡司三坐标协议
        /// </summary>
        [Description("蔡司 三坐标")]
        ZeissTCP = 303,

        /// <summary>
        /// OPC DA 方式
        /// </summary>
        [Description("OPC Classic 方式")]
        OPC_Classic = 21,

        /// <summary>
        /// OPC UA 方式
        /// 需要 .net framework 4.6.1以上
        /// </summary>
        [Description("OPC UA 方式")]
        OPC_UA = 22,

        /// <summary>
        /// Hsl ABPLC 方式
        /// 需要 .net framework 4.6.1以上
        /// </summary>
        [Description("Hsl ABPLC 方式")]
        ABPLC = 23,

        /// <summary>
        /// TCP自定义协议 Client
        /// </summary>
        [Description("TCP自定义协议")]
        TCP_Custom = 30,

        /// <summary>
        /// Modbus TCP Cliet
        /// </summary>
        [Description("TCP Modbus协议")]
        TCP_Modbus = 31,

        /// <summary>
        /// TCP 服务器
        /// </summary>
        [Description("TCP 服务器")]
        TCP_Server = 32,

        /// <summary>
        /// WebApi RestApi
        /// </summary>
        [Description("WebApi")]
        WebApi = 40,

        /// <summary>
        /// 数据库
        /// 目前只支持 MySQL、SQL Server、Oracle 
        /// </summary>
        [Description("数据库")]
        DataBase = 100,

        ///// <summary>
        ///// 共享文件形式
        ///// 支持远程文件共享形式
        ///// </summary>
        [Description("共享文件形式")]
        ShareFile = 110,
    }

    /// <summary>
    /// 设备状态
    /// </summary>
    public enum DeviceState
    {
        /// <summary>
        /// 离线
        /// </summary>
        [Description("离线")]
        Offline = 0,

        /// <summary>
        /// 待机非工作
        /// </summary>
        [Description("待机")]
        Standby = 1,

        /// <summary>
        /// 工作中
        /// </summary>
        [Description("工作中")]
        Working = 2,

        /// <summary>
        /// 报警
        /// </summary>
        [Description("报警")]
        Alarm = 10,

        /// <summary>
        /// 急停
        /// </summary>
        [Description("急停")]
        Emergency = 20,
    }

    /// <summary>
    /// 设备标签值类型
    /// </summary>
    public enum TagDataType
    {
        /// <summary>
        /// 默认类型
        /// </summary>
        [Description("默认类型")]
        Default = 0,

        /// <summary>
        /// bool/bit/位类型
        /// </summary>
        [Description("bool/bit/位类型")]
        Bool = 1,

        /// <summary>
        /// byte(int8)类型
        /// </summary>
        [Description("byte(int8)类型")]
        Byte = 2,

        /// <summary>
        /// short(int16)类型
        /// </summary>
        [Description("short(int16)类型")]
        Short = 3,

        /// <summary>
        /// ushort类型
        /// </summary>
        [Description("ushort类型")]
        UShort = 4,

        /// <summary>
        /// int(int32)类型
        /// </summary>
        [Description("int(int32)类型")]
        Int32 = 5,

        /// <summary>
        /// uint类型
        /// </summary>
        [Description("uint类型")]
        UInt32 = 6,

        /// <summary>
        /// long(int64)类型
        /// </summary>
        [Description("long(int64)类型")]
        Long = 7,

        /// <summary>
        /// ulong类型
        /// </summary>
        [Description("ulong类型")]
        ULong = 8,

        /// <summary>
        /// float类型
        /// </summary>
        [Description("float类型")]
        Float = 9,

        /// <summary>
        /// double类型
        /// </summary>
        [Description("double类型")]
        Double = 10,

        /// <summary>
        /// string型
        /// </summary>
        [Description("string型")]
        String = 20,

        /// <summary>
        /// 数组型(Tag地址+[])
        /// </summary>
        [Description("数组型(Tag地址+[])")]
        Array = 101,

        /// <summary>
        /// 连续地址型(Tag地址+序号)
        /// </summary>
        [Description("连续地址型(Tag地址+序号)")]
        ContinuousAddr = 102,
    }

    /// <summary>
    /// 数据采集模式
    /// </summary>
    public enum DataSimplingMode
    {
        /// <summary>
        /// 主动读取设备数据
        /// 会按照设备周期读取数据
        /// </summary>
        [Description("主动读取设备数据")]
        AutoReadDevice = 0,

        /// <summary>
        /// 只能写入不能读取数据
        /// 不能作为判断条件
        /// </summary>
        [Description("只写入不读取")]
        OnlyWrite = 1,

        /// <summary>
        /// 订阅方式 针对OPC设备用
        /// </summary>
        [Description("OPC订阅")]
        Subscribe = 2,

        /// <summary>
        /// DA数据采集
        /// 开启CNC的特殊采集
        /// </summary>
        [Description("DA数据采集")]
        DASimpling = 3,

        /// <summary>
        /// 不主动读取数据
        /// 可作为判断条件
        /// </summary>
        [Description("不主动读取数据")]
        NoAutoRead = 4,

        /// <summary>
        /// 读（动作完成后自动关闭）
        /// </summary>
        [Description("读（动作完成后自动关闭）")]
        ReadAutoStop = 10,

        /// <summary>
        /// 不读（动作开始后自动开启）
        /// </summary>
        [Description("不读（动作开始后自动开启）")]
        ReadAutoStart = 11,
    }

    /// <summary>
    /// 消息通讯方向
    /// </summary>
    public enum MessageDirection
    {
        /// <summary>
        /// 发送
        /// </summary>
        [Description("发送")]
        Send = 1,

        /// <summary>
        /// 接收
        /// </summary>
        [Description("接收")]
        Receive = 2,
    }
}
