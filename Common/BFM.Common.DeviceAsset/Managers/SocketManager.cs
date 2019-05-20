using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BFM.Common.Base.Log;
using BFM.Common.Base.PubData;
using BFM.Common.Data.PubData;
using BFM.Common.DeviceAsset.Socket;
using BFM.Common.DeviceAsset.Socket.Base;

namespace BFM.Common.DeviceAsset
{
    /// <summary>
    /// 通过Socket通讯
    /// </summary>
    public class SocketManager : IDeviceCore
    {

        public DeviceManager _DevcieComm;

        #region 标准属性

        private Int64 pkid;  //唯一标识
        private IPAddress ServerIP;
        private int ServerPort;

        private string CustomProtocol; //自定义协议
        private string ProtocolVariable;  //自定义协议参数
        private List<DeviceTagParam> DeviceTags = new List<DeviceTagParam>();  //地址
        private DataChangeEventHandler Callback;  //结束数据的反馈

        #endregion

        #region Modbus TCP 专用

        private object lockwrite = new object();
        private object lockread = new object();
        private object lockanysis = new object();

        private bool bCloseSocket = false;

        private List<string> CurServerResult = new List<string>();  //服务器当前发送的值

        #endregion

        #region Socket

        private ISocketCommDevice SockeDevice;  //专用Socket接口

        private List<string> UnFinishWriteData = new List<string>(); //未完成写入的数据 

        /// <summary>
        /// Socket通讯客户端
        /// </summary>
        private SocketClient _client;

        #endregion

        public SocketManager(DeviceManager devcieCommunication, DataChangeEventHandler callback)
        {
            _DevcieComm = devcieCommunication;

            Initial(devcieCommunication.DevicePKID, devcieCommunication.CommParam.CommAddress,
                devcieCommunication.CommParam.UpdateRate, devcieCommunication.CommParam.CustomProtocol,
                devcieCommunication.CommParam.ProtocolVariable, devcieCommunication.DeviceTags, callback);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serverPKID"></param>
        /// <param name="address"></param>
        /// <param name="updateRate"></param>
        /// <param name="protocolVariable"></param>
        /// <param name="deviceTagParams"></param>
        /// <param name="callback"></param>
        /// <param name="customProtocol"></param>
        public void Initial(Int64 serverPKID, string address, int updateRate,
            string customProtocol, string protocolVariable,
            List<DeviceTagParam> deviceTagParams, DataChangeEventHandler callback)
        {
            pkid = serverPKID;

            #region Socket 通讯相关

            string ip = (address == "") ? CBaseData.LocalIP : address;
            int port = 2001;

            string[] addes = address.Split(';', '；');  //分号隔开，前面是IP地址，后面是Port
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

            if (remote != null)
            {
                _client = new SocketClient(remote, port);
                _client.OnReadServerBytes -= ClientOnOnReadServerMessage;
                _client.OnReadServerBytes += ClientOnOnReadServerMessage; //读取数据
                _client.OnConnectChange -= ClientOnOnConnectChange;
                _client.OnConnectChange += ClientOnOnConnectChange; //连接成功

                _client.Connect();  //连接服务器
            }

            #endregion

            #region 设置标签信息

            DeviceTags = deviceTagParams;  //设置标签

            #endregion

            #region 自定义协议，创建不同的协议内容

            CustomProtocol = customProtocol; //自定义协议
            ProtocolVariable = protocolVariable;  //自定义协议

            #endregion
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
                    _client.Connect(); //重新连接
                    EventLogger.Log($"重新连接Socket Server {ServerIP}");
                }
            }
            else //连接成功
            {
                #region 发送缓存中的内容

                for (int i = 0; i < UnFinishWriteData.Count; i++)
                {
                    int result = _client.SyncSend(UnFinishWriteData[i]);
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
                    CurServerResult = SockeDevice.AnalysisServerData(bytes, out error); //解析数据
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
                        string sPKNO = tag.PKNO;

                        Callback?.Invoke(sPKNO, dataValue); //回调设备处理
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

            if (_client == null)
            {
                return new OperateResult("自定义TCP协议 设备未初始化，请先初始化.");
            }

            if (!_client.Connected)
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

                #region 按照特殊协议分别处理

                #endregion

                int result = _client.SyncSend(sendValue);

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

            if (_client == null)
            {
                return new OperateResult<string>("自定义TCP协议 设备未初始化，请先初始化.");
            }

            if (!_client.Connected)
            {
                return new OperateResult<string>("自定义TCP协议 设备未连接，请重试.");
            }

            #endregion

            lock (lockread)
            {
                CurServerResult.Clear();

                #region 按照特殊协议分别处理

                #endregion

                //异步读取数据时，则向Socket发送读取指令
                int result = _client.SyncSend(dataAddress);
                if (result != 0)
                {
                    return new OperateResult<string>("自定义TCP协议 同步读数据，发送失败 " + ((result == 1) ? "未连接" : "发送错误"));
                }

                //TODO 获取返回值
            }

            return OperateResult.CreateSuccessResult("测试");   //没有获取到结果++++++++++++++++++++++++++
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

            if (_client == null)
            {
                return new OperateResult("自定义TCP协议 设备未初始化，请先初始化.");
            }

            if (!_client.Connected)
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

                #region 按照特殊协议分别处理


                #endregion

                //int result = _client.AsyncSend(sendValue); //异步发送数据时，则向Socket发送读取指令
                int result = _client.SyncSend(sendValue);  //改为同步发送
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

            if (_client == null)
            {
                return new OperateResult("自定义TCP协议 设备未初始化，请先初始化.");
            }

            if (!_client.Connected)
            {
                return new OperateResult("自定义TCP协议 设备未连接，请重试.");
            }

            #endregion

            lock (lockread)
            {
                CurServerResult.Clear();

                #region 按照特殊协议分别处理

                #endregion
                
                //int result = _client.AsyncSend(dataAddress);  //异步读取数据时，则向Socket发送读取指令
                int result = _client.SyncSend(dataAddress);  //改为同步发送
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
            if (_client != null)
            {
                _client.DisConnect();
            }
        }
    }
}
