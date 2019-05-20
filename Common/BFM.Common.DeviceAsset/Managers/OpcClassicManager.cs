using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BFM.Common.Base;
using BFM.Common.Base.Log;
using BFM.Common.Base.PubData;
using BFM.Common.Data.PubData;
using BFM.OPC.Client.Core;
using HD.OPC.Client.Core;
using HslCommunication;

namespace BFM.Common.DeviceAsset
{
    /// <summary>
    /// OPC Class DA 管理者
    /// 通讯地址格式 OPCServer名称
    /// </summary>
    public class OpcClassicManager : IDeviceCore
    {
        public static List<OpcServer> OpcServers = new List<OpcServer>(); //所有OPCServer

        public const string _RSLinxOPC = "RSLinx OPC Server";
        public const string _SimensOPC = "OPC.SimaticNET";

        public DeviceManager _DevcieComm;

        #region 标准属性

        private Int64 pkid;  //唯一标识
        private IPAddress ServerIP;
        private string CustomProtocol; //自定义协议
        private string ProtocolVariable;  //自定义协议参数
        private List<DeviceTagParam> DeviceTags = new List<DeviceTagParam>();  //地址
        private DataChangeEventHandler Callback;  //结束数据的反馈

        #endregion

        #region  OPC专用

        private OpcServer _opcServer = null;  //OPCServer
        private string OPCServerName;  //OPC 的名称

        #endregion

        public OpcClassicManager(DeviceManager devcieCommunication, DataChangeEventHandler callback)
        {
            _DevcieComm = devcieCommunication;

            Initial(devcieCommunication.DevicePKID, devcieCommunication.CommParam.CommAddress,
                devcieCommunication.CommParam.UpdateRate, devcieCommunication.DeviceTags, callback);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serverPKID"></param>
        /// <param name="address"></param>
        /// <param name="updateRate"></param>
        /// <param name="deviceTagParams"></param>
        /// <param name="callback"></param>
        private void Initial(Int64 serverPKID, string address, int updateRate, 
            List<DeviceTagParam> deviceTagParams, DataChangeEventHandler callback)
        {
            pkid = serverPKID;

            #region 初始化参数

            string ip = (address == "") ? CBaseData.LocalIP : address;
            string serverName = address;

            string[] addes = address.Split('|', ';');  //分号隔开，前面是IP地址，后面是OPCServer名称
            if (addes.Length > 1)
            {
                ip = addes[0];
                if ((ip.ToLower() == "local") || (ip.ToLower() == "localhost") || (ip.ToLower() == "."))
                {
                    ip = CBaseData.LocalIP; //本机IP
                }
                serverName = addes[1];
            }
            else  //默认为本机
            {
                ip = CBaseData.LocalIP;
            }

            IPAddress remote;
            IPAddress.TryParse(ip, out remote);

            ServerIP = remote;
            OPCServerName = serverName;
            Callback = callback;  //设置回调函数

            #endregion
            
            DeviceTags = deviceTagParams;  //标签

            #region 自定义协议

            CustomProtocol = (addes.Length >= 3) ? addes[2] : ""; //自定义协议
            ProtocolVariable = (addes.Length >= 4) ? addes[3] : ""; //自定义协议

            #endregion

            #region OPC 设定

            string error = String.Empty;

            _opcServer = OpcServers.FirstOrDefault(c => c.PKID == serverPKID);

            if ((_opcServer == null) || (!_opcServer.IsConnected))
            {
                _opcServer = OPCConnector.ConnectOPCServer(serverPKID, ip, serverName, ref error);
            }
            else
            {
                EventLogger.Log($"【{serverPKID}】OPC服务器已连接");
            }

            if (_opcServer != null && _opcServer.IsConnected)
            {
                List<Guid> subHandels = new List<Guid>();  //Handel
                List<string> subItemIds = new List<string>();  //标签地址

                List<Guid> normalHandels = new List<Guid>();  //Handel
                List<string> normalItemIds = new List<string>();  //标签地址
                
                if (subHandels.Count > 0) //添加订阅
                {
                    OpcGroup subGroup = _opcServer.AddGroup("GP" + serverPKID + "S", updateRate, true);  //订阅的Group

                    subGroup.AddItems(subItemIds.ToArray(), subHandels.ToArray());

                    subGroup.DataChanged -= OPCDataChanged;
                    subGroup.DataChanged += OPCDataChanged;
                }

                if (normalHandels.Count > 0) //添加正常
                {
                    OpcGroup normalGroup = _opcServer.AddGroup("GP" + serverPKID + "N", updateRate, false);  //正常的Group

                    normalGroup.AddItems(normalItemIds.ToArray(), normalHandels.ToArray());
                }
                
                EventLogger.Log($"【{serverPKID}】OPC服务器连接成功。");
            }
            else
            {
                error = $"【{serverPKID}】OPC服务器连接失败，具体为：" + error;
                EventLogger.Log(error);
            }

            OpcServers.Add(_opcServer);  //将OPCServer添加到系统中

            #endregion 
        }
        
        #region 同步读写数据

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataAddress"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public OperateResult SyncWriteData(string dataAddress, string dataValue)
        {
            #region 检验

            if (string.IsNullOrEmpty(dataAddress) || string.IsNullOrEmpty(dataValue))
            {
                return new OperateResult("传入的参数都不能为空");
            }

            if (_opcServer == null)
            {
                return new OperateResult("OPCServer未能正确初始化！");
            }

            if (!_opcServer.IsConnected)
            {
                return new OperateResult("OPCServer连接错误！");
            }

            #endregion

            string groupName = "SyncWriteData";

            OpcGroup group = _opcServer.FindGroupByName(groupName);
            if (group != null)
            {
                _opcServer.RemoveGroup(group);
            }
            group = _opcServer.AddGroup(groupName, 1, false);  //添加组

            string datavalue = dataValue.Split('|')[0];

            WriteCompleteEventHandler writeFinishHandler = WriteFinishHandler;
            IRequest request;

            #region 添加Item及写入Item

            if ((dataAddress.Contains(',')) && (OPCServerName == _RSLinxOPC)) //RSLinx 数组的写入
            {
                //地址前缀,类型,起始位置,长度

                #region RSLinx 数组及地址串

                string[] tags = dataAddress.Split(',');

                if (tags.Length < 3)
                {
                    return new OperateResult("RSLinx 地址错误");
                }

                string mainAddress = tags[0]; //主地址
                string type = tags[1]; //类型
                int beginIndex = SafeConverter.SafeToInt(tags[2]); //开始索引
                int length = SafeConverter.SafeToInt(tags[3]); //长度
                string endStr = (tags.Length > 4) ? tags[4] : "";

                Guid[] handels = new Guid[length];
                string[] address = new string[length];
                string[] values = new string[length];
                for (int i = 0; i < length; i++)
                {
                    handels[i] = Guid.NewGuid();
                    values[i] = datavalue.Length > i ? datavalue[i].ToString() : "0"; //默认为0
                    if (type.ToLower() == "array") //数组型
                    {
                        address[i] = mainAddress + "[" + (i + beginIndex).ToString() + "]";
                    }
                    else //连续地址
                    {
                        address[i] = mainAddress + type + (i + beginIndex).ToString() + endStr;
                    }
                }

                ItemResult[] itemResults = group.AddItems(address, handels);

                ItemValue[] itemValues = new ItemValue[length];

                for (int i = 0; i < length; i++)
                {
                    itemValues[i] = new ItemValue(itemResults[i]) {Value = values[i]};
                }

                group.AsyncWrite(itemValues, handels, writeFinishHandler, out request);

                #endregion
            }
            else  //正常类型
            {
                Guid[] handel = new[] { Guid.NewGuid() };
                ItemResult[] itemResults = group.AddItems(new[] { dataAddress }, handel);
                
                ItemValue itemValue = new ItemValue(itemResults[0])
                {
                    Value = datavalue,
                };
                group.AsyncWrite(new ItemValue[] { itemValue }, handel, writeFinishHandler, out request);

            }


            #endregion
            
            return OperateResult.CreateSuccessResult();
        }


        /// <summary>
        /// 同步读
        /// </summary>
        /// <param name="dataAddress"></param>
        /// <returns></returns>
        public OperateResult<string> SyncReadData(string dataAddress)
        {
            #region 检验

            if (string.IsNullOrEmpty(dataAddress) )
            {
                return new OperateResult<string>("传入的参数都不能为空");
            }

            if (_opcServer == null)
            {
                return new OperateResult<string>("OPCServer未能正确初始化！");
            }

            if (!_opcServer.IsConnected)
            {
                return new OperateResult<string>("OPCServer连接错误！");
            }

            #endregion
            
            string groupName = "AsyncWriteData";

            OpcGroup group = _opcServer.FindGroupByName(groupName);
            if (group != null)
            {
                _opcServer.RemoveGroup(group);
            }
            group = _opcServer.AddGroup(groupName, 1, false);  //1.添加组

            IRequest request;

            #region 添加Item及写入Item

            if ((dataAddress.Contains(',')) && (OPCServerName == _RSLinxOPC)) //RSLinx 数组的写入
            {
                //地址前缀,类型,起始位置,长度
            }
            else  //正常类型
            {
                Guid[] handel = new[] { Guid.NewGuid() };
                ItemResult[] itemResults = group.AddItems(new[] { dataAddress }, handel);  //2.添加Items

                object[] objhandel = new object[] { handel[0] };
                object _requestHandle = Guid.NewGuid().ToString("N");
                //group.(objhandel, _requestHandle, )

            }

            #endregion

            return OperateResult.CreateSuccessResult("测试");   //没有获取到结果++++++++++++++++++++++++++
        }

        #endregion

        #region 异步读写数据

        /// <summary>
        /// 异步写数据
        /// </summary>
        /// <param name="dataAddress"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public OperateResult AsyncWriteData(string dataAddress, string dataValue)
        {
            #region 检验

            if (string.IsNullOrEmpty(dataAddress) || string.IsNullOrEmpty(dataValue))
            {
                return new OperateResult("传入的参数都不能为空");
            }

            if (_opcServer == null)
            {
                return new OperateResult("OPCServer未能正确初始化！");
            }

            if (!_opcServer.IsConnected)
            {
                return new OperateResult("OPCServer连接错误！");
            }

            #endregion

            string groupName = "AsyncWriteData";

            OpcGroup group = _opcServer.FindGroupByName(groupName);
            if (group != null)
            {
                _opcServer.RemoveGroup(group);
            }
            group = _opcServer.AddGroup(groupName, 1, false);  //添加组

            string datavalue = dataValue.Split('|')[0];

            WriteCompleteEventHandler writeFinishHandler = WriteFinishHandler;
            IRequest request;

            #region 添加Item及写入Item

            if ((dataAddress.Contains(',')) && (OPCServerName == _RSLinxOPC)) //RSLinx 数组的写入
            {
                //地址前缀,类型,起始位置,长度

                #region RSLinx 数组及地址串

                string[] tags = dataAddress.Split(',');

                if (tags.Length < 3)
                {
                    return new OperateResult("RSLinx 地址错误");
                }

                string mainAddress = tags[0]; //主地址
                string type = tags[1]; //类型
                int beginIndex = SafeConverter.SafeToInt(tags[2]); //开始索引
                int length = SafeConverter.SafeToInt(tags[3]); //长度
                string endStr = (tags.Length > 4) ? tags[4] : "";

                Guid[] handels = new Guid[length];
                string[] address = new string[length];
                string[] values = new string[length];
                for (int i = 0; i < length; i++)
                {
                    handels[i] = Guid.NewGuid();
                    values[i] = datavalue.Length > i ? datavalue[i].ToString() : "0"; //默认为0
                    if (type.ToLower() == "array") //数组型
                    {
                        address[i] = mainAddress + "[" + (i + beginIndex).ToString() + "]";
                    }
                    else //连续地址
                    {
                        address[i] = mainAddress + type + (i + beginIndex).ToString() + endStr;
                    }
                }

                ItemResult[] itemResults = group.AddItems(address, handels);

                ItemValue[] itemValues = new ItemValue[length];

                for (int i = 0; i < length; i++)
                {
                    itemValues[i] = new ItemValue(itemResults[i]) { Value = values[i] };
                }

                group.AsyncWrite(itemValues, handels, writeFinishHandler, out request);

                #endregion
            }
            else  //正常类型
            {
                Guid[] handel = new[] { Guid.NewGuid() };
                ItemResult[] itemResults = group.AddItems(new[] { dataAddress }, handel);

                ItemValue itemValue = new ItemValue(itemResults[0])
                {
                    Value = datavalue,
                };
                group.AsyncWrite(new ItemValue[] { itemValue }, handel, writeFinishHandler, out request);

            }

            #endregion

            return OperateResult.CreateSuccessResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestHandle"></param>
        /// <param name="results"></param>
        private void WriteFinishHandler(object requestHandle, IdentifiedResult[] results)
        {

        }

        public OperateResult AsyncReadData(string dataAddress)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="error"></param>
        public OperateResult RefreshData()
        {

            #region 检验

            if (_opcServer == null)
            {
                return new OperateResult("OPCServer未能正确初始化!");
            }

            if (!_opcServer.IsConnected)
            {
                return new OperateResult("OPCServer连接错误!");
            }

            #endregion

            try
            {
                _opcServer.RefreshOpc();  //刷新数据
                return OperateResult.CreateSuccessResult();
            }
            catch (Exception ex)
            {
                return new OperateResult("error " + ex.Message);
            }
        }

        /// <summary>
        /// OPC的DataChange事件
        /// </summary>
        /// <param name="subscriptionHandle"></param>
        /// <param name="requestHandle"></param>
        /// <param name="values"></param>
        private void OPCDataChanged(object subscriptionHandle, object requestHandle, ItemValueResult[] values)
        {
            foreach (ItemValueResult value in values)
            {
                if (value.ClientHandle == null) continue;

                if (Callback != null) Callback.Invoke(Guid.Parse(value.ClientHandle.ToString()).ToString("N"), value.Value.ToString());

                Thread.Sleep(100);
            }
        }

        public void Dispose()
        {
            //释放
            OpcServer opcServer = OpcServers.FirstOrDefault(c => c.PKID == pkid);
            if (opcServer != null)
            {
                opcServer.Disconnect();
            }
        }
    }
}
