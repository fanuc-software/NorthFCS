using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using BFM.Common.Base;
using BFM.Common.Base.Log;
using BFM.Common.Base.PubData;
using BFM.Common.DeviceAsset.Socket;
using BFM.Common.DeviceAsset.Socket.Base;
using HslCommunication;

namespace BFM.Common.DeviceAsset
{
    /// <summary>
    /// 
    /// 通过Modbus TCP 通讯
    /// 通讯地址格式 按照 | 分隔
    /// 格式：IP地址 | 端口号 | 自定义协议 | 协议参数
    ///
    /// Tag地址：s=站号（默认1）;f=功能号;d=地址;l=长度
    ///
    /// 
    /// </summary>
    public class ModbusTCPManager : IDeviceCore
    {
        private const short _minId = 10;  //最小的ID

        #region Modbus 协议常量

        /*****************************************************************************************
         * 
         *    本服务器和客户端支持的常用功能码
         * 
         *******************************************************************************************/
         
        /// <summary>
        /// 读取线圈
        /// </summary>
        public const byte ReadCoil = 0x01;

        /// <summary>
        /// 读取离散量
        /// </summary>
        public const byte ReadDiscrete = 0x02;

        /// <summary>
        /// 读取寄存器
        /// </summary>
        public const byte ReadRegister = 0x03;

        /// <summary>
        /// 读取输入寄存器
        /// </summary>
        public const byte ReadInputRegister = 0x04;

        /// <summary>
        /// 写单个线圈
        /// </summary>
        public const byte WriteOneCoil = 0x05;

        /// <summary>
        /// 写单个寄存器
        /// </summary>
        public const byte WriteOneRegister = 0x06;

        /// <summary>
        /// 写多个线圈
        /// </summary>
        public const byte WriteCoil = 0x0F;

        /// <summary>
        /// 写多个寄存器
        /// </summary>
        public const byte WriteRegister = 0x10;

        
        /*****************************************************************************************
         * 
         *    本服务器和客户端支持的异常返回
         * 
         *******************************************************************************************/
         
        /// <summary>
        /// 不支持该功能码
        /// </summary>
        public const byte FunctionCodeNotSupport = 0x01;
        /// <summary>
        /// 该地址越界
        /// </summary>
        public const byte FunctionCodeOverBound = 0x02;
        /// <summary>
        /// 读取长度超过最大值
        /// </summary>
        public const byte FunctionCodeQuantityOver = 0x03;
        /// <summary>
        /// 读写异常
        /// </summary>
        public const byte FunctionCodeReadWriteException = 0x04;

        #endregion 协议常量

        public DeviceManager _DevcieComm;

        #region 标准属性

        private Int64 pkid;  //唯一标识

        private IPAddress ServerIP;  //IP地址
        private int ServerPort;  //端口

        private string CustomProtocol; //自定义协议
        private string ProtocolVariable;  //自定义协议参数
        private List<DeviceTagParam> DeviceTags = new List<DeviceTagParam>();  //地址
        private DataChangeEventHandler Callback;  //结束数据的反馈

        #endregion

        #region Modbus TCP 专用

        private Dictionary<int, string> ModbusRebackPKNO = new Dictionary<int, string>();  //Modbus反馈用

        private object lockcreate = new object();
        private object lockwrite = new object();
        private object lockread = new object();
        private object lockanysis = new object();

        private bool bCloseSocket = false;

        private List<string> CurServerResult = new List<string>();  //服务器当前发送的值
        private short currentId = _minId;  //自增id，发送命令的标识

        #endregion

        #region Socket

        private List<string> UnFinishWriteData = new List<string>(); //未完成写入的数据 

        /// <summary>
        /// Socket通讯客户端
        /// </summary>
        private SocketClient socketClient;

        #endregion

        public ModbusTCPManager(DeviceManager devcieCommunication, DataChangeEventHandler callback)
        {
            _DevcieComm = devcieCommunication;

            Initial(devcieCommunication.DevicePKID, devcieCommunication.CommParam.CommAddress,
                devcieCommunication.DeviceTags, callback);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serverPKID"></param>
        /// <param name="address"></param>
        /// <param name="deviceTagParams"></param>
        /// <param name="callback"></param>
        private void Initial(Int64 serverPKID, string address, 
            List<DeviceTagParam> deviceTagParams, DataChangeEventHandler callback)
        {
            pkid = serverPKID;

            #region Socket 通讯相关

            string ip = (address == "") ? CBaseData.LocalIP : address;
            int port = 502;  //Modbus默认502

            string[] addes = address.Split('|');  //分号隔开，前面是IP地址，后面是Port
            if (addes.Length > 1)
            {
                ip = addes[0];
                if ((ip.ToLower() == "local") || (ip.ToLower() == "localhost") || (ip.ToLower() == "."))
                {
                    ip = CBaseData.LocalIP;  //本机IP
                }
                int.TryParse(addes[1], out port);
            }

            Callback = callback;  //设置回调函数

            IPAddress remote;
            IPAddress.TryParse(ip, out remote);

            ServerIP = remote;
            ServerPort = port;

            ThreadPool.QueueUserWorkItem(s => { CreateAndRefreshClient(); }); //不断重连

            #endregion

            #region 设置标签信息

            DeviceTags = deviceTagParams;  //设置标签

            #endregion

            #region 自定义协议

            CustomProtocol = (addes.Length >= 3) ? addes[2] : ""; //自定义协议
            ProtocolVariable = (addes.Length >= 4) ? addes[3] : ""; //自定义协议

            #endregion
        }

        /// <summary>
        /// 创建和刷新Client
        /// </summary>
        private void CreateAndRefreshClient()
        {
            if (ServerIP == null) return;

            while (!CBaseData.AppClosing && !bCloseSocket)
            {
                Thread.Sleep(100);
                lock (lockcreate)
                {
                    if ((socketClient?._client != null) && (socketClient._client.Connected))
                    {
                        continue;
                    }
                    
                    #region 重新进行Socket 连接

                    socketClient?.DisConnect();

                    if (socketClient?._client == null)
                    {
                        if (socketClient != null)
                        {
                            socketClient.OnReadServerBytes -= ClientOnOnReadServerMessage;
                        }

                        socketClient = new SocketClient(ServerIP, ServerPort);
                        socketClient.OnReadServerBytes += ClientOnOnReadServerMessage; //读取数据
                    }

                    socketClient.Connect(); //连接服务器

                    #endregion

                    if (socketClient.Connected)  //连接成功
                    {
                        #region 发送缓存中的内容

                        for (int i = 0; i < UnFinishWriteData.Count; i++)
                        {
                            int result = socketClient.SyncSend(UnFinishWriteData[i]);
                            if (result != 0)
                            {
                                break;
                            }
                            UnFinishWriteData.RemoveAt(i);
                        }

                        #endregion
                    }
                    else  //连接失败，重新连接
                    {
                        Thread.Sleep(_DevcieComm.CommParam.UpdateRate);
                    }
                }
            }
        }

        #region Server 反馈

        /// <summary>
        /// Socket Server断了
        /// </summary>
        /// <param name="o"></param>
        /// <param name="isConnected"></param>
        private void ClientOnOnConnectChange(object o, bool isConnected)
        {
            if (CBaseData.AppClosing || bCloseSocket) return;

            if (!isConnected) //Server已退出
            {
                Thread.Sleep(500);
                if (!bCloseSocket)
                {
                    CreateAndRefreshClient();  //创建和刷新Client
                    Console.WriteLine($"重新连接Socket Server {ServerIP} 结果 " + (socketClient.Connected? "成功" : "失败"));
                    Thread.Sleep(100);
                }
            }
            else //连接成功
            {
                #region 发送缓存中的内容

                for (int i = 0; i < UnFinishWriteData.Count; i++)
                {
                    int result = socketClient.SyncSend(UnFinishWriteData[i]);
                    if (result != 0)
                    {
                        break;
                    }
                    UnFinishWriteData.RemoveAt(i);
                }

                #endregion
            }
        }

        /// <summary>
        /// 接收到服务器数据
        /// </summary>
        /// <param name="o"></param>
        /// <param name="bytes"></param>
        private void ClientOnOnReadServerMessage(object o, byte[] bytes)
        {
            if (CBaseData.AppClosing || bCloseSocket) return;

            lock (lockanysis)
            {
                string error = "";

                if (error != "")
                {
                    EventLogger.Log($"已接收到服务端数据，解析错误：'{error}'");
                    return;
                }

                #region 按照特殊协议分别处理

                #endregion

                if (bytes.Length >= 9)
                {
                    byte[] checkBytes = new byte[2];
                    checkBytes[0] = bytes[1];
                    checkBytes[1] = bytes[0];
                    short checkKey = BitConverter.ToInt16(checkBytes, 0);

                    byte[] valueBytes = new byte[bytes.Length - 9];
                    Array.Copy(bytes, 9, valueBytes, 0, valueBytes.Length);

                    int type = bytes[7];

                    #region 写入时不读取反馈

                    if ((type == WriteCoil) || (type == WriteOneCoil) || (type == WriteRegister) ||
                        (type == WriteOneRegister)) //写数据
                    {
                        if (ModbusRebackPKNO.ContainsKey(checkKey))
                        {
                            ModbusRebackPKNO.Remove(checkKey);
                        }

                        return; 
                    }

                    #endregion

                    string dataValue = "";

                    for (int i = 9; i < bytes.Length; i = i + 2)
                    {
                        if (i + 1 < bytes.Length)
                        {
                            byte[] tempValue = new byte[2];
                            tempValue[0] = bytes[i + 1];
                            tempValue[1] = bytes[i];
                            string value = BitConverter.ToInt16(tempValue, 0).ToString();  //值

                            #region 数据处理

                            if (type == ReadCoil) //线圈
                            {
                                string result = Convert.ToString(tempValue[0], 2).PadLeft(8, '0');
                                result += Convert.ToString(tempValue[1], 2).PadLeft(8, '0');
                                value = "";
                                for (int j = result.Length - 1; j >= 0; j--)
                                {
                                    value += (string.IsNullOrEmpty(value) ? "" : "|") + result[j];
                                }
                            }

                            #endregion

                            dataValue += (dataValue == "") ? value : ("|" + value);
                        }
                        else if (i + 1 == bytes.Length)  //最后只有1位
                        {
                            string value = bytes[i].ToString();

                            #region 数据处理

                            if (type == ReadCoil) //线圈
                            {
                                string result = Convert.ToString(bytes[i], 2).PadLeft(8, '0');
                                value = "";
                                for (int j = result.Length - 1; j >= 0 ; j--)
                                {
                                    value += (string.IsNullOrEmpty(value) ? "" : "|") + result[j];
                                }
                            }

                            #endregion

                            dataValue += (dataValue == "") ? value : ("|" + value);
                        }
                    }

                    #region 处理反馈结果

                    string rebackThisValue = "";

                    if (ModbusRebackPKNO.ContainsKey(checkKey))
                    {
                        rebackThisValue = ModbusRebackPKNO[checkKey];
                        ModbusRebackPKNO.Remove(checkKey);
                    }

                    if (checkKey <= _minId)  //当最小的
                    {
                        ModbusRebackPKNO.Clear();
                    }

                    List<int> removeRebacks = new List<int>();  //将<=当前id-2的 反馈置为空
                    foreach (int key in ModbusRebackPKNO.Keys)
                    {
                        if (key < checkKey - DeviceTags.Count)
                        {
                            removeRebacks.Add(key);
                        }
                    }

                    foreach (int reback in removeRebacks)
                    {
                        ModbusRebackPKNO.Remove(reback); //清除
                    }

                    Callback?.Invoke(rebackThisValue, dataValue); //反馈数据结果 - Callback 回调设备处理

                    if (string.IsNullOrEmpty(dataValue))
                    {
                        Console.WriteLine("获取的结果为空！");
                    }

                    #endregion
                }
            }
        }

        #endregion

        #region 同步读写数据

        /// <summary>
        /// 同步写数据
        /// </summary>
        /// <param name="dataAddress">格式为 ; 分开 s=Station;f=读取功能;d=地址;l=长度 </param>
        /// <param name="dataValues"></param>
        /// <returns></returns>
        public OperateResult SyncWriteData(string dataAddress, string dataValues)
        {
            #region 检验

            if (string.IsNullOrEmpty(dataAddress) || string.IsNullOrEmpty(dataValues))
            {
                return new OperateResult("传入的参数都不能为空");
            }

            if (socketClient == null)
            {
                return new OperateResult("Modbus 设备未初始化，请先初始化.");
            }

            if (!socketClient.Connected)
            {
                return new OperateResult("Modbus 设备连接未连接，请重试.");
            }
            #endregion

            lock (lockwrite)
            {

                //发送数据
                CurServerResult.Clear();

                var tag = DeviceTags.FirstOrDefault(c => c.Address == dataAddress);
                string pkno = (tag != null) ? tag.PKNO : "SyncWriteData";
                
                byte[] data = AnalysisWriteAddress(dataAddress, dataValues, WriteRegister);

                if (data == null)
                {
                    return new OperateResult("Modbus 同步写入数据时，地址转换失败!");
                }

                int result = socketClient.SyncSend(data);
                
                {
                    using (System.IO.StreamWriter wr = new System.IO.StreamWriter(@"d:\mod_log.txt", true))
                    {
                        wr.WriteLine("pkno:"+ pkno + "          data:" + data.ToString() + "      IP:" + this.ServerIP.ToString());
                    }
                }

                if (result != 0)
                {
                    return new OperateResult("Modbus 同步写入数据时，发送失败，错误：" + ((result == 1) ? "未连接" : "发送错误"));
                }

                #region 按照特殊协议分别处理


                #endregion
            }

            return OperateResult.CreateSuccessResult();
        }

        /// <summary>
        /// 同步读取数据
        /// </summary>
        /// <param name="dataAddress">格式为  s=Station;f=读取功能;d=地址;l=长度 </param>
        /// <returns></returns>
        public OperateResult<string> SyncReadData(string dataAddress)
        {
            #region 检验

            if (string.IsNullOrEmpty(dataAddress))
            {
                return new OperateResult<string>("传入的参数都不能为空");
            }

            if (socketClient == null)
            {
                return new OperateResult<string>("Modbus 设备未初始化，请先初始化.");
            }

            if (!socketClient.Connected)
            {
                return new OperateResult<string>("Modbus 设备连接未连接，请重试.");
            }

            #endregion

            lock (lockread)
            {
                CurServerResult.Clear();

                var tag = DeviceTags.FirstOrDefault(c => c.Address == dataAddress);
                string pkno = (tag != null) ? tag.PKNO : "SyncReadData";

                byte[] data = AnalysisReadAddress(dataAddress, ReadRegister, pkno);  //发送数据

                if (data == null)
                {
                    return new OperateResult<string>("Modbus 同步读取数据时，地址转换失败!");
                }

                int result = socketClient.SyncSend(data);  //发送数据
                if (result != 0)
                {
                    return new OperateResult<string>("Modbus 同步读取数据时，发送失败 " + ((result == 1) ? "未连接" : "发送错误"));
                }
            }

            return OperateResult.CreateSuccessResult("");   //没有获取到结果
        }

        #endregion
        
        #region 异步读写数据

        /// <summary>
        /// 异步写数据
        /// </summary>
        /// <param name="dataAddress">格式为 ; 分开 s=Station;f=读取功能;d=地址;l=长度 </param>
        /// <param name="dataValues"></param>
        /// <returns></returns>
        public OperateResult AsyncWriteData(string dataAddress, string dataValues)
        {
            #region 检验

            if (string.IsNullOrEmpty(dataValues))
            {
                return new OperateResult("传入的参数都不能为空.");
            }

            if (socketClient == null)
            {
                return new OperateResult("Modbus 设备未初始化，请先初始化.");
            }

            if (!socketClient.Connected)
            {
                return new OperateResult("Modbus 设备连接未连接，请重试.");
            }

            #endregion

            lock (lockwrite)
            {
                //发送数据
                CurServerResult.Clear();

                var tag = DeviceTags.FirstOrDefault(c => c.Address == dataAddress);
                string pkno = (tag != null) ? tag.PKNO : "AsyncWriteData";

                byte[] data = AnalysisWriteAddress(dataAddress, dataValues, WriteRegister);

                if (data == null)
                {
                    return new OperateResult("Modbus 异步写数据时，地址转换失败!");
                }

                int result = socketClient.AsyncSend(data);

                if (result != 0)
                {
                    return new OperateResult("Modbus 异步写数据时，发送失败，错误：" + ((result == 1) ? "未连接" : "发送错误"));
                }

                #region 按照特殊协议分别处理


                #endregion
            }

            return OperateResult.CreateSuccessResult();
        }

        /// <summary>
        /// 异步读取数据
        /// </summary>
        /// <param name="dataAddress">格式为  s=Station;f=读取功能;d=地址;l=长度 </param>
        /// <returns></returns>
        public OperateResult AsyncReadData(string dataAddress)
        {
            #region 检验

            if (string.IsNullOrEmpty(dataAddress))
            {
                return new OperateResult<string>("传入的参数都不能为空.");
            }

            if (socketClient == null)
            {
                return new OperateResult<string>("Modbus 设备未初始化，请先初始化.");
            }

            if (!socketClient.Connected)
            {
                return new OperateResult<string>("Modbus 设备连接未连接，请重试.");
            }

            #endregion

            lock (lockread)
            {
                CurServerResult.Clear();

                var tag = DeviceTags.FirstOrDefault(c => c.Address == dataAddress);
                string pkno = (tag != null) ? tag.PKNO : "AsyncReadData";

                byte[] data = AnalysisReadAddress(dataAddress, ReadRegister, pkno);  

                if (data == null)
                {
                    return new OperateResult<string>("Modbus 异步读取数据时，地址转换失败!");
                }

                //int result = _client.AsyncSend(data);   //异步读取数据时，则向Socket发送读取指令
                int result = socketClient.SyncSend(data);
                if (result != 0)
                {
                    return new OperateResult<string>("Modbus 异步读取数据时，发送失败，错误：" + ((result == 1) ? "未连接" : "发送错误"));
                }

                #region 按照特殊协议分别处理

                #endregion
            }

            return OperateResult.CreateSuccessResult();  
        }

        #endregion

        public void Dispose()
        {
            bCloseSocket = true;
            socketClient?.DisConnect();
        }

        /// <summary>
        /// 解析读取数据的地址
        /// </summary>
        /// <param name="address">s=Station;f=读取功能;d=地址;l=长度</param>
        /// <param name="defFuncton">默认的功能</param>
        /// <param name="spkno">地址的PKNO，需要读取反馈信息</param>
        private byte[] AnalysisReadAddress(string address, byte defFuncton, string spkno)
        {
            byte[] data = new byte[6];

            #region 解析Modbus地址

            try
            {
                int station = 1; //Station
                byte function = defFuncton; //
                int modAddress = 0; //地址
                int length = 1; //长度

                GetModbusInfo(address, ref station, ref function, ref modAddress, ref length);
                
                data[0] = (byte) station;
                data[1] = function;
                data[2] = BitConverter.GetBytes(modAddress)[1];
                data[3] = BitConverter.GetBytes(modAddress)[0];
                data[4] = BitConverter.GetBytes(length)[1];
                data[5] = BitConverter.GetBytes(length)[0];
            }
            catch
            {
                data = null;
            }

            #endregion

            return PackCommandToTcp(spkno, data);
        }

        /// <summary>
        /// 解析地址
        /// </summary>
        /// <param name="address">s=Station;f=读取功能;d=地址;l=长度</param>
        /// <param name="value">写入值</param>
        /// <param name="defFuncton">默认的功能</param>
        /// <param name="spkno">地址的PKNO，默认不需要获取反馈信息</param>
        private byte[] AnalysisWriteAddress(string address, string value, byte defFuncton, string spkno = "")
        {
            byte[] data = null;

            #region 解析Modbus地址

            try
            {
                int station = 1; //Station
                byte function = defFuncton; //
                int modAddress = 0; //地址
                int length = 1;

                GetModbusInfo(address, ref station, ref function, ref modAddress, ref length);


                if (length == 1)
                {
                    value = value.Split('|')[0];
                }

                if (function == WriteOneCoil)  //写单个线圈
                {
                    data = new byte[6];

                    data[0] = (byte)station;
                    data[1] = function;
                    data[2] = BitConverter.GetBytes(modAddress)[1];
                    data[3] = BitConverter.GetBytes(modAddress)[0];
                    data[4] = (byte)(SafeConverter.SafeToBool(SafeConverter.SafeToInt(value.Split('|')[0])) ? 0xFF : 0x00);
                    data[5] = 0x00;
                }
                else if (function == WriteOneRegister)  //写单个寄存器
                {
                    data = new byte[6];

                    data[0] = (byte)station;
                    data[1] = function;
                    data[2] = BitConverter.GetBytes(modAddress)[1];
                    data[3] = BitConverter.GetBytes(modAddress)[0];
                    data[4] = BitConverter.GetBytes(SafeConverter.SafeToInt(value.Split('|')[0]))[1];
                    data[5] = BitConverter.GetBytes(SafeConverter.SafeToInt(value.Split('|')[0]))[0];
                }
                else if (function == WriteCoil) //写多个线圈，目前只最多支持16位，注意需要设备支持 ++++++++++++++++需要测试++++++++++++++
                {
                    string[] values = value.Split('|');
                    string result = "";
                    for (int i = 0; i < length; i++)
                    {
                        if (i < values.Length)
                        {
                            result += SafeConverter.SafeToBool(values[i]) ? "1" : "0";
                        }
                        else
                        {
                            result += "0";
                        }
                    }

                    byte innerlength = (byte) (Math.Ceiling(length * 1.0 / 8));
                    data = new byte[7 + innerlength];

                    int buf = Convert.ToInt16(result, 2);

                    data[0] = (byte)station;
                    data[1] = function;
                    data[2] = BitConverter.GetBytes(modAddress)[1];
                    data[3] = BitConverter.GetBytes(modAddress)[0];
                    data[4] = BitConverter.GetBytes(length)[1];
                    data[5] = BitConverter.GetBytes(length)[0];
                    data[6] = innerlength;

                    if (innerlength == 1)
                    {
                        data[7] = BitConverter.GetBytes(buf)[0];
                    }
                    else if (innerlength == 2)
                    {
                        data[7] = BitConverter.GetBytes(buf)[1];
                        data[8] = BitConverter.GetBytes(buf)[0];
                    }
                }
                else if (function == WriteRegister)  //写多个寄存器
                {
                    string[] values = value.Split('|');
                    byte[] buf = new byte[length * 2];
                    int index = 0;
                    for (int i = 0; i < length; i++)  //按照写入值得长度来确定
                    {
                        string s = (values.Length > i) ? values[i] : "0";

                        buf[index++] = BitConverter.GetBytes(SafeConverter.SafeToInt(s))[1];
                        buf[index++] = BitConverter.GetBytes(SafeConverter.SafeToInt(s))[0];
                    }

                    data = new byte[7 + buf.Length];

                    data[0] = (byte)station;
                    data[1] = function;
                    data[2] = BitConverter.GetBytes(modAddress)[1];
                    data[3] = BitConverter.GetBytes(modAddress)[0];
                    data[4] = BitConverter.GetBytes(buf.Length / 2)[1];  
                    data[5] = BitConverter.GetBytes(buf.Length / 2)[0]; 
                    data[6] = (byte)(buf.Length);

                    buf.CopyTo(data, 7);
                }
            }
            catch
            {
                data = null;
            }

            #endregion

            return PackCommandToTcp(spkno, data);
        }

        /// <summary>
        /// 将modbus指令打包成Modbus-Tcp指令
        /// </summary>
        /// <param name="spkno">地址的PKNO</param>
        /// <param name="value">Modbus指令</param>
        /// <returns>Modbus-Tcp指令</returns>
        private byte[] PackCommandToTcp(string spkno, byte[] value)
        {
            if (value == null) return null;

            short id = ++currentId;
            if (currentId >= short.MaxValue) currentId = _minId;

            byte[] buffer = new byte[value.Length + 6];
            buffer[0] = BitConverter.GetBytes(id)[1];
            buffer[1] = BitConverter.GetBytes(id)[0];

            buffer[4] = BitConverter.GetBytes(value.Length)[1];
            buffer[5] = BitConverter.GetBytes(value.Length)[0];

            value.CopyTo(buffer, 6);

            if (!string.IsNullOrEmpty(spkno))
            {
                if (ModbusRebackPKNO.ContainsKey(id))
                {
                    ModbusRebackPKNO[id] = spkno;
                }
                else
                {
                    ModbusRebackPKNO.Add(id, spkno);
                }
            }

            return buffer;
        }

        /// <summary>
        /// 从地址字符串中获取Modbus的信息
        /// </summary>
        /// <param name="address">地址字符串</param>
        /// <param name="station"></param>
        /// <param name="function"></param>
        /// <param name="modAddress"></param>
        /// <param name="length"></param>
        public static void GetModbusInfo(string address, ref int station, ref byte function, ref int modAddress,
            ref int length)
        {
            if (address.IndexOf(';') < 0)
            {
                modAddress = SafeConverter.SafeToInt(address);  // 正常地址
            }
            else
            {
                string[] list = address.Split(';');  // 带功能码的地址
                for (int i = 0; i < list.Length; i++)
                {
                    if (string.IsNullOrEmpty(list[i])) continue;
                    if (list[i][0] == 's' || list[i][0] == 'S')  // 站号信息
                    {
                        station = SafeConverter.SafeToInt(list[i].Substring(2));
                    }
                    else if (list[i][0] == 'f' || list[i][0] == 'F')  //功能码
                    {
                        function = (byte)SafeConverter.SafeToInt(list[i].Substring(2));
                    }
                    else if (list[i][0] == 'd' || list[i][0] == 'D')  //数据
                    {
                        modAddress = SafeConverter.SafeToInt(list[i].Substring(2));
                    }
                    else if (list[i][0] == 'l' || list[i][0] == 'L')  //长度
                    {
                        length = SafeConverter.SafeToInt(list[i].Substring(2));
                    }
                    else
                    {
                        if (modAddress == 0) modAddress = SafeConverter.SafeToInt(list[i]);
                    }
                }
            }
        }
    }
}
