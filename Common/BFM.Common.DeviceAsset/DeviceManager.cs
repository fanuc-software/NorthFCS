using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BFM.Common.Base.Helper;
using BFM.Common.DeviceAsset.managers;
using HslCommunication;

namespace BFM.Common.DeviceAsset
{
    /****************************************
     *
     *  设备操作管理总类
     *
     ****************************************/

    public class DeviceManager : IDisposable
    {
        #region 设备标志

        /// <summary>
        /// 设备唯一编号
        /// </summary>
        public Int64 DevicePKID { get; }  //通讯设备唯一标识

        /// <summary>
        /// 设备唯一PKNO
        /// </summary>
        public string DevicePKNO { get; }  //服务PKNO = 设备通讯的PKNO

        #endregion

        /// <summary>
        /// 设备通讯参数
        /// </summary>
        public DeviceCommParam CommParam { get; }

        /// <summary>
        /// 设备标签
        /// </summary>
        public List<DeviceTagParam> DeviceTags { get; private set; }

        /// <summary>
        /// 主通讯器
        /// </summary>
        private List<IDeviceCore> DeviceCores { get; set; } //主通讯设备

        private DataChangeEventHandler CallBack = null;

        #region 1. Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commType">通讯接口</param>
        /// <param name="deviceAddress">通讯地址</param>
        public DeviceManager(DeviceCommInterface commType, string deviceAddress)
        {
            DevicePKID = DeviceHelper.GetNewPKID();

            CommParam = new DeviceCommParam(DevicePKID.ToString(), commType, deviceAddress);  //设备通讯信息

            DeviceCores = new List<IDeviceCore>();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="devicePKNO">设备PKNO，正常采集用AssetCode</param>
        /// <param name="commType">通讯类型 1：Fanuc CNC;21:OPC DA;22:OPC UA;30:自定义TCP;31:Modbus TCP;100:数据库；</param>
        /// <param name="deviceAddress">通讯地址</param>
        /// <param name="updateRate">更新率ms；为SamplingPeriod</param>
        public DeviceManager(string devicePKNO, DeviceCommInterface commType, string deviceAddress, 
            int updateRate = 1000)
        {
            DevicePKID = DeviceHelper.GetNewPKID();  //PKID
            
            DevicePKNO = string.IsNullOrEmpty(devicePKNO) ? DevicePKID.ToString() : devicePKNO;  //PKNO

            CommParam = new DeviceCommParam(DevicePKNO, commType, deviceAddress, updateRate);  //设备通讯信息

            DeviceCores = new List<IDeviceCore>();
        }

        #endregion

        /// <summary>
        /// 初始化设备
        /// </summary>
        /// <param name="tagPKNO"></param>
        /// <param name="tagAddress"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IDeviceCore InitialDevice(string tagPKNO, string tagAddress, DataChangeEventHandler callback = null) //初始化设备
        {
            List<DeviceTagParam> deviceTags = new List<DeviceTagParam>();
            DeviceTagParam deviceTag = new DeviceTagParam(tagPKNO, tagAddress, this);  //通讯参数
            deviceTags.Add(deviceTag);  //添加节点

            return InitialDevice(deviceTags, callback);
        }

        /// <summary>
        /// 初始化设备
        /// </summary>
        /// <param name="divDeviceTagParams"></param>
        /// <param name="callback"></param>
        public IDeviceCore InitialDevice(List<DeviceTagParam> divDeviceTagParams, DataChangeEventHandler callback) //初始化设备
        {
            DeviceTags = divDeviceTagParams;
            CallBack = callback;

            IDeviceCore deviceCore = null;

            #region 初始化主通讯设备

            switch (CommParam.CommInterface)
            {
                case DeviceCommInterface.CNC_Fanuc: //Fanuc CNC
                    deviceCore = new FocasManager(this, callback);
                    break;
                case DeviceCommInterface.FanucRobot:  //Fanuc机器人
                    deviceCore = new ModbusTCPManager(this, callback);
                    break;
                case DeviceCommInterface.ModulaTCP:  //Modula 自动货柜
                    deviceCore = new SocketClientManager(this, callback);
                    break;
                case DeviceCommInterface.ABPLC:  //Modula 自动货柜
                    deviceCore = new HslAllenBradleyNetManager(this, callback);
                    break;
                case DeviceCommInterface.ZeissTCP:  //蔡司 三坐标
                    int serverIndex = this.CommParam.CommAddress.IndexOf('|', 0);
                    string serverAdd = this.CommParam.CommAddress.Substring(serverIndex + 1);
                    serverIndex = serverAdd.IndexOf('|', 0);
                    serverAdd = serverAdd.Substring(serverIndex + 1);
                    DeviceManager deviceComm = new DeviceManager(this.DevicePKNO + "_S", DeviceCommInterface.ZeissTCP, serverAdd);
                    deviceCore = new SocketClientManager(this, callback);
                    var serverManager = new SocketServerManager(deviceComm, callback);  //新的Server端
                    break;
                case DeviceCommInterface.OPC_Classic: //OPC Server
                    //deviceCore = new OpcClassicManager(this, callback);
                    break;
                case DeviceCommInterface.TCP_Custom:  //自定义TCP协议 
                    deviceCore = new SocketClientManager(this, callback);
                    break;
                case DeviceCommInterface.TCP_Modbus:  //Modbus TCP
                    deviceCore = new ModbusTCPManager(this, callback);
                    break;
                case DeviceCommInterface.TCP_Server:  //TCP 服务器
                    deviceCore = new SocketServerManager(this, callback);
                    break;
                case DeviceCommInterface.WebApi:  //WebApi
                    deviceCore = new WebApiManager(this, callback);
                    break;
                case DeviceCommInterface.DataBase: //数据库通讯
                    deviceCore = new DatabaseManager(this, callback);
                    break;
                case DeviceCommInterface.ShareFile: //数据库通讯
                    deviceCore = new CopyFileManager();
                    break;
            }

            #endregion

            if (deviceCore != null)
            {
                if (!DeviceHelper.CommDevices.Contains(this)) DeviceHelper.CommDevices.Add(this); //添加到通讯设备中

                DeviceCores.Add(deviceCore);
            }

            return deviceCore;
        }

        /// <summary>
        /// 获取当前的通讯设备
        /// </summary>
        /// <returns></returns>
        public IDeviceCore GetCurDeviceCore()
        {
            IDeviceCore deviceCore = DeviceCores.FirstOrDefault();

            if (CommParam.CommInterface == DeviceCommInterface.CNC_Fanuc) //Faunc CNC 线程
            {
                string threadID = Thread.CurrentThread.ManagedThreadId.ToString();  //按照线程分别建立ID

                foreach (var device in DeviceCores)
                {
                    FocasManager focasDevice = device as FocasManager;

                    if ((focasDevice != null) && (focasDevice.CurThreadID == threadID))  //线程相同
                    {
                        return device;
                    }
                }

                for (int i = DeviceCores.Count - 1; i >= 3; i--)
                {
                    DeviceCores[i].Dispose();
                    DeviceCores.RemoveAt(i);
                }

                #region 创建新的通讯

                deviceCore = new FocasManager(this, CallBack);

                if (!DeviceHelper.CommDevices.Contains(this)) DeviceHelper.CommDevices.Add(this); //添加到通讯设备中

                DeviceCores.Add(deviceCore);

                #endregion
            }

            return deviceCore;
        }
        
        #region 同步读写数据

        /// <summary>
        /// 同步写数据
        /// </summary>
        /// <param name="dataAddress"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public OperateResult SyncWriteData(string dataAddress, string dataValue)
        {
            IDeviceCore deivceCore = GetCurDeviceCore();
            if (deivceCore == null)
            {
                return  new OperateResult("设备未初始化");
            }

            dataAddress = dataAddress.Replace("#13", "\n");
            dataValue = dataValue.Replace("#13", "\n");
            return deivceCore.SyncWriteData(dataAddress, dataValue);
        }

        /// <summary>
        /// 同步读数据
        /// </summary>
        /// <param name="dataAddress"></param>
        /// <returns></returns>
        public OperateResult<string> SyncReadData(string dataAddress)
        {
            IDeviceCore deivceCore = GetCurDeviceCore();
            if (deivceCore == null)
            {
                return new OperateResult<string>("设备未初始化");
            }

            dataAddress = dataAddress.Replace("#13", "\n");
            return deivceCore.SyncReadData(dataAddress);
        }

        /// <summary>
        /// 同步读取数据，直接返回结果
        /// </summary>
        /// <param name="deviceaddress"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public string SyncReadData(string deviceaddress, out string error)
        {
            error = "";
            OperateResult<string> ret = SyncReadData(deviceaddress);
            if (!ret.IsSuccess)
            {
                error = ret.Message;
                return "";
            }

            return ret.Content;
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
            IDeviceCore deivceCore = GetCurDeviceCore();
            if (deivceCore == null)
            {
                return new OperateResult("设备未初始化");
            }

            dataAddress = dataAddress.Replace("#13", "\n");
            dataValue = dataValue.Replace("#13", "\n");
            return deivceCore.AsyncWriteData(dataAddress, dataValue);
        }

        /// <summary>
        /// 异步读数据
        /// </summary>
        /// <param name="dataAddress"></param>
        /// <returns></returns>
        public OperateResult AsyncReadData(string dataAddress)
        {
            IDeviceCore deivceCore = GetCurDeviceCore();
            if (deivceCore == null)
            {
                return new OperateResult<string>("设备未初始化");
            }

            dataAddress = dataAddress.Replace("#13", "\n");
            return deivceCore.AsyncReadData(dataAddress);
        }

        #endregion

        /// <inheritdoc />
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            DeviceHelper.CommDevices.RemoveAll(c => c.DevicePKID == DevicePKID);   //移除设备

            for (int i = 0; i < DeviceCores.Count; i++)
            {
                DeviceCores[i].Dispose();
            }

            DeviceCores.Clear();
        }
    }
}
