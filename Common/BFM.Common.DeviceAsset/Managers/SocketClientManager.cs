using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using BFM.Common.Base.Log;
using BFM.Common.Base.PubData;
using BFM.Common.DeviceAsset.Socket;
using BFM.Common.DeviceAsset.Socket.Base;
using HslCommunication;

namespace BFM.Common.DeviceAsset
{
    /// <summary>
    /// 通过Socket通讯 - Client端
    /// 通讯地址格式 按照 | 分隔
    /// 格式：IP地址 | 端口号 | 自定义协议 | 协议参数
    /// </summary>
    public class SocketClientManager : IDeviceCore
    {
        public DeviceManager _DevcieComm;  //通讯设备

        #region 标准属性

        private Int64 pkid;  //唯一标识
        private IPAddress ServerIP;
        private int ServerPort;

        private string CustomProtocol; //自定义协议
        private string ProtocolVariable;  //自定义协议参数
        private List<DeviceTagParam> DeviceTags = new List<DeviceTagParam>();  //地址
        private DataChangeEventHandler Callback;  //结束数据的反馈

        #endregion

        #region Socket.Client 专用

        private object lockcreate = new object();
        private object lockwrite = new object();
        private object lockread = new object();
        private object lockanysis = new object();

        private bool bCloseSocket = false;

        private List<string> CurServerResult = new List<string>();  //服务器当前发送的值

        /// <summary>
        /// Socket通讯客户端
        /// </summary>
        private SocketClient socketClient;

        #endregion

        #region Socket

        private ISocketCommDevice SockeDevice = null;  //当前专用Socket接口

        private List<string> UnFinishWriteData = new List<string>(); //未完成写入的数据 

        #endregion

        public SocketClientManager(DeviceManager devcieCommunication, DataChangeEventHandler callback)
        {
            _DevcieComm = devcieCommunication;

            Initial(devcieCommunication.DevicePKID, devcieCommunication.CommParam.CommAddress,
                devcieCommunication.CommParam.CommInterface, devcieCommunication.DeviceTags, callback);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serverPKID"></param>
        /// <param name="address"></param>
        /// <param name="deviceInterface">设备接口</param>
        /// <param name="deviceTagParams"></param>
        /// <param name="callback"></param>
        public void Initial(Int64 serverPKID, string address, DeviceCommInterface deviceInterface,
            List<DeviceTagParam> deviceTagParams, DataChangeEventHandler callback)
        {
            pkid = serverPKID;

            #region Socket 通讯相关

            string ip = (address == "") ? CBaseData.LocalIP : address;
            int port = 2001;

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

            #region 添加自定义协议设备

            CustomProtocol = (addes.Length >= 3) ? addes[2] : ""; //自定义协议
            ProtocolVariable = (addes.Length >= 4) ? addes[3] : ""; //自定义协议

            SockeDevice = CustomSocket.GetDevice(deviceInterface, CustomProtocol, ProtocolVariable);

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
                    CreateAndRefreshClient();  //重新刷新
                    Console.WriteLine($"重新连接Socket Server {ServerIP} 结果 " + (socketClient.Connected ? "成功" : "失败"));
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

                #region 解析数据

                if (SockeDevice == null)
                {
                    string str = System.Text.Encoding.ASCII.GetString(bytes);
                    CurServerResult.Clear();
                    CurServerResult.Add(str);

                    EventLogger.Log($"Socket分析器没有初始化，解析错误：'{error}'");
                }
                else
                {
                    CurServerResult = SockeDevice?.AnalysisServerData(bytes, out error); //解析数据
                }

                #endregion

                if (error != "")
                {
                    EventLogger.Log($"已接收到服务端数据，解析错误：'{error}'");
                    return;
                }

                #region 按照特殊协议分别处理
                

                #endregion

                //解析的结果：CurServerResult：0：地址；1：值；2：其他自定义
                #region 反馈数据结果 - Callback

                if (CurServerResult.Count >= 2)
                {
                    string dataAddress = CurServerResult[0];
                    string dataValue = CurServerResult[1];

                    var tag = DeviceTags.FirstOrDefault(c => c.Address == dataAddress);

                    if (tag != null)
                    {
                        Callback?.Invoke(tag.PKNO, dataValue); //回调设备处理
                    }
                }

                #endregion
            }
        }
        
        #region 同步读写数据

        /// <summary>
        /// 同步写数据
        /// </summary>
        /// <param name="dataAddress"></param>
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
                return new OperateResult("自定义TCP协议 设备未初始化，请先初始化.");
            }

            if (!socketClient.Connected)
            {
                return new OperateResult("自定义TCP协议 设备未连接，请重试.");
            }

            #endregion

            string[] dataValue = dataValues.Split('|');

            lock (lockwrite)
            {
                //发送数据
                CurServerResult.Clear();

                string sendValue = string.Format(dataAddress, dataValue);

                byte[] sendData = SockeDevice?.GetSendValueBeforeWrite(sendValue) ?? Encoding.Default.GetBytes(sendValue);   //根据协议转换

                int result = socketClient.SyncSend(sendData);  //向设备写入数据

                if (result != 0)
                {
                    return new OperateResult("自定义TCP协议 同步写数据时，发送失败 " + ((result == 1) ? "未连接" : "发送错误"));
                }
            }

            return OperateResult.CreateSuccessResult();
        }

        /// <summary>
        /// 同步读数据
        /// </summary>
        /// <param name="dataAddress"></param>
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
                return new OperateResult<string>("自定义TCP协议 设备未初始化，请先初始化.");
            }

            if (!socketClient.Connected)
            {
                return new OperateResult<string>("自定义TCP协议 设备未连接，请重试.");
            }


            #endregion

            lock (lockread)
            {
                CurServerResult.Clear();

                byte[] sendData = SockeDevice?.GetSendValueBeforeWrite(dataAddress) ?? Encoding.Default.GetBytes(dataAddress);   //根据协议转换

                int result = socketClient.SyncSend(sendData);  //向Socket发送读取指令

                if (result != 0)
                {
                    return new OperateResult<string>("自定义TCP协议 同步读数据，发送失败 " + ((result == 1) ? "未连接" : "发送错误"));
                }

                //TODO 获取返回值
            }

            return OperateResult.CreateSuccessResult("哦同步协议值");   //没有获取到结果
        }

        #endregion

        #region 异步读写数据

        /// <summary>
        /// 异步写数据
        /// </summary>
        /// <param name="dataAddress"></param>
        /// <param name="dataValues"></param>
        /// <returns></returns>
        public OperateResult AsyncWriteData(string dataAddress, string dataValues)
        {
            #region 检验

            if (string.IsNullOrEmpty(dataAddress) || string.IsNullOrEmpty(dataValues))
            {
                return new OperateResult("传入的参数都不能为空");
            }

            if (socketClient == null)
            {
                return new OperateResult("自定义TCP协议 设备未初始化，请先初始化.");
            }

            if (!socketClient.Connected)
            {
                return new OperateResult("自定义TCP协议 设备未连接，请重试.");
            }

            #endregion

            string[] dataValue = dataValues.Split('|');

            lock (lockwrite)
            {
                //发送数据
                CurServerResult.Clear();

                string sendValue = string.Format(dataAddress, dataValue);

                byte[] sendData = SockeDevice?.GetSendValueBeforeWrite(sendValue) ?? Encoding.Default.GetBytes(sendValue);   //根据协议转换

                int result = socketClient.SyncSend(sendData);  //向设备写入数据
                if (result != 0)
                {
                    return new OperateResult("自定义TCP协议 异步写数据时，发送失败 " + ((result == 1) ? "未连接" : "发送错误"));
                }
            }
            return OperateResult.CreateSuccessResult();
        }

        /// <summary>
        /// 异步读数据
        /// </summary>
        /// <param name="dataAddress"></param>
        /// <returns></returns>
        public OperateResult AsyncReadData(string dataAddress)
        {
            #region 检验

            if (string.IsNullOrEmpty(dataAddress))
            {
                return new OperateResult("传入的参数都不能为空");
            }

            if (socketClient == null)
            {
                return new OperateResult("自定义TCP协议 设备未初始化，请先初始化.");
            }
            if (!socketClient.Connected)
            {
                return new OperateResult("自定义TCP协议 设备未连接，请重试.");
            }


            #endregion

            lock (lockread)
            {
                CurServerResult.Clear();

                byte[] sendData = SockeDevice?.GetSendValueBeforeWrite(dataAddress) ?? Encoding.Default.GetBytes(dataAddress);   //根据协议转换

                int result = socketClient.SyncSend(sendData);  //向Socket发送读取指令

                if (result != 0)
                {
                    return new OperateResult("自定义TCP协议 异步读取数据时，发送失败 " + ((result == 1) ? "未连接" : "发送错误"));
                }
            }
            return OperateResult.CreateSuccessResult();   //没有获取到结果
        }

        #endregion

        public void Dispose()
        {
            bCloseSocket = true;
            socketClient?.DisConnect();
        }
    }
}
