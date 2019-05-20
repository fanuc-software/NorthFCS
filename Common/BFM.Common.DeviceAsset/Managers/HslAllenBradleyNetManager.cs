using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BFM.Common.Base.Extend;
using BFM.Common.Base.PubData;
using HslCommunication;
using HslCommunication.Profinet.AllenBradley;

namespace BFM.Common.DeviceAsset.managers
{
    public class HslAllenBradleyNetManager : IDeviceCore
    {
        public DeviceManager _DevcieComm;

        public string CurThreadID { get; private set; } //当前线程ID
        //private object lockcreate = new object();
        //private object lockwrite = new object();
        //private object lockread = new object();
        //private object lockanysis = new object();
        #region 标准属性

        private Int64 pkid; //唯一标识

        private IPAddress ServerIP;
        private ushort ServerPort;

        private string CustomProtocol; //自定义协议
        private string ProtocolVariable; //自定义协议参数
        private List<DeviceTagParam> DeviceTags = new List<DeviceTagParam>(); //地址
        private DataChangeEventHandler Callback; //结束数据的反馈

        #endregion

        #region constructor

        public HslAllenBradleyNetManager(DeviceManager devcieCommunication, DataChangeEventHandler callback)
        {
            _DevcieComm = devcieCommunication;
            CurThreadID = Thread.CurrentThread.ManagedThreadId.ToString(); //当前线程ID

            Initial(devcieCommunication.DevicePKID, devcieCommunication.CommParam.CommAddress,
                devcieCommunication.CommParam.UpdateRate, devcieCommunication.DeviceTags, callback);
        }

        private void Initial(Int64 serverPKID, string address, int updateRate, List<DeviceTagParam> deviceTagParams,
            DataChangeEventHandler callback)
        {
            #region 初始化参数

            pkid = serverPKID;

            string ip = (address == "") ? CBaseData.LocalIP : address;
            int port = 44818;//ABPLC端口

            string[] addes = address.Split('|'); //分号隔开，前面是IP地址，后面是Port
            if (addes.Length > 1)
            {
                ip = addes[0];
                if ((ip.ToLower() == "local") || (ip.ToLower() == "localhost") || (ip.ToLower() == "."))
                {
                    ip = CBaseData.LocalIP; //本机IP
                }

                if (!string.IsNullOrEmpty(addes[1])) int.TryParse(addes[1], out port);
            }

            IPAddress remote;
            IPAddress.TryParse(ip, out remote);

            ServerIP = remote;
            ServerPort = (ushort)port;

            #endregion

            Callback = callback; //设置回调函数

            DeviceTags = deviceTagParams; //标签

            #region 自定义协议

            CustomProtocol = (addes.Length >= 3) ? addes[2] : ""; //自定义协议
            ProtocolVariable = (addes.Length >= 4) ? addes[3] : ""; //自定义协议

            #endregion
        }

        #endregion
        public void Dispose()
        {

        }
        /// <summary>
        /// 同步写数据，支持按照位写
        /// </summary>
        /// <param name="dataAddress"></param>
        /// <param name="dataValues"></param>
        /// <returns></returns>
        public OperateResult SyncWriteData(string dataAddress, string dataValues)
        {
            string dataValue = dataValues.Split('|')[0];
            if (string.IsNullOrEmpty(dataAddress) || (dataAddress.Length <= 1))
            {
                return new OperateResult<string>("传入的参数都不能为空");
            }

            if (ServerIP == null)
            {
                return new OperateResult<string>("设备未初始化，请先初始化。");
            }
            try
            {
                AllenBradleyNet allenBradleyNet = new AllenBradleyNet();
                allenBradleyNet.IpAddress = ServerIP.ToString();
                allenBradleyNet.Port = ServerPort;
                OperateResult connect = allenBradleyNet.ConnectServer();
                if (!connect.IsSuccess)
                {
                    return new OperateResult<string>("PLC连接失败");
                }
                //todo:读数据
                OperateResult isWrite=  allenBradleyNet.Write(dataAddress,Int16.Parse(dataValue));
                if (!isWrite.IsSuccess)
                {
                    return new OperateResult<string>("PLC写入失败");
                }
                allenBradleyNet.ConnectClose();


                return OperateResult.CreateSuccessResult();
            }
            catch (Exception ex)
            {
                string error = $"读取 设备IP({ServerIP}) 地址({dataAddress}) 失败，错误为({ex.Message.ToString()})";
                Console.WriteLine(error);
                return new OperateResult<string>(error);
            }
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="dataAddress"></param>
        /// <returns></returns>
        public OperateResult<string> SyncReadData(string dataAddress)
        {
            if (string.IsNullOrEmpty(dataAddress) || (dataAddress.Length <= 1))
            {
                return new OperateResult<string>("传入的参数都不能为空");
            }

            if (ServerIP == null)
            {
                return new OperateResult<string>("设备未初始化，请先初始化。");
            }
            try
            {
                AllenBradleyNet allenBradleyNet = new AllenBradleyNet();
                allenBradleyNet.IpAddress = ServerIP.ToString();
                allenBradleyNet.Port = ServerPort;
                OperateResult connect = allenBradleyNet.ConnectServer();
                if (!connect.IsSuccess)
                {
                    return new OperateResult<string>("PLC连接失败");
                }
                //todo:暂时支持bool类型读数据
                OperateResult<string>read=new OperateResult<string>();
                if (allenBradleyNet.ReadBool(dataAddress).Content)
                    read= OperateResult.CreateSuccessResult("1");
                else
                    read= OperateResult.CreateSuccessResult("0");
                

                allenBradleyNet.ConnectClose();
               

                return read;
            }
            catch (Exception ex)
            {
                string error = $"读取 设备IP({ServerIP}) 地址({dataAddress}) 失败，错误为({ex.Message.ToString()})";
                Console.WriteLine(error);
                return new OperateResult<string>(error);
            }
        }

        public OperateResult AsyncWriteData(string dataAddress, string dataValue)
        {
           return SyncWriteData(dataAddress, dataValue);
        }

        public OperateResult AsyncReadData(string dataAddress)
        {
          return  SyncReadData(dataAddress);
        }

      

    }
}
