using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFM.Common.Base.Helper;
using HslCommunication;

namespace BFM.Common.DeviceAsset
{
    /// <summary>
    /// 数据改变事件，反馈到内存中
    /// </summary>
    /// <param name="tagPKNO"></param>
    /// <param name="sReadResult"></param>
    public delegate void DataChangeEventHandler(string tagPKNO, string sReadResult);

    /****************************************
     *
     *   设备通讯辅助类
     *
     ***************************************/

    public static class DeviceHelper
    {
        /// <summary>
        /// 当前设备编号
        /// </summary>
        private static Int64 CurDevicePKID = 0;

        /// <summary>
        /// 通讯设备
        /// </summary>
        public static List<DeviceManager> CommDevices = new List<DeviceManager>();

        /// <summary>
        /// 获取新的设备PKID
        /// </summary>
        /// <returns></returns>
        public static Int64 GetNewPKID()
        {
            CurDevicePKID++;
            if (CurDevicePKID > 999999999999) DeviceHelper.CurDevicePKID = 100000;

            return CurDevicePKID;
        }

        #region 直接写入信息

        /// <summary>
        /// 写入信息
        /// </summary>
        /// <param name="devicePKNO"></param>
        /// <param name="commType"></param>
        /// <param name="deviceAddress"></param>
        /// <param name="tagPKNO"></param>
        /// <param name="tagAddress"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public static OperateResult WriteDataByAddress(string devicePKNO, int commType,
            string deviceAddress, string tagPKNO, string tagAddress, string dataValue)
        {
            DeviceCommInterface interfaceType = EnumHelper.ParserEnumByValue(commType, DeviceCommInterface.CNC_Fanuc);
            return WriteDataByAddress(devicePKNO, interfaceType, deviceAddress, 1000, tagPKNO, tagAddress, dataValue);
        }

        /// <summary>
        /// 向设备写值
        /// </summary>
        /// <param name="devicePKNO">设备PKNO，一般为Asset_Code</param>
        /// <param name="commType"></param>
        /// <param name="deviceAddress"></param>
        /// <param name="updateRate"></param>
        /// <param name="tagPKNO"></param>
        /// <param name="tagAddress"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        public static OperateResult WriteDataByAddress(string devicePKNO, DeviceCommInterface commType,
            string deviceAddress, int updateRate, string tagPKNO, string tagAddress, string dataValue)
        {
            DeviceManager deviceComm =
                CommDevices.FirstOrDefault(c => c.DevicePKNO == devicePKNO && c.DeviceTags.Any(d => d.PKNO == tagPKNO));

            bool bDispose = false;

            if (deviceComm == null) //没有添加监视，表示临时的写入，则需要释放
            {
                deviceComm = new DeviceManager(devicePKNO, commType, deviceAddress, updateRate);

                deviceComm.InitialDevice(tagPKNO, tagAddress);

                CommDevices.Add(deviceComm);
                bDispose = true;
            }

            OperateResult ret = deviceComm.SyncWriteData(tagAddress, dataValue); //写入执行值

            if (bDispose)
            {
                deviceComm.Dispose();  //释放
            }

            return ret;
        }

        #endregion

        #region 直接读取信息

        /// <summary>
        /// 读取信息
        /// </summary>
        /// <param name="devicePKNO"></param>
        /// <param name="commType"></param>
        /// <param name="deviceAddress"></param>
        /// <param name="tagPKNO"></param>
        /// <param name="tagAddress"></param>
        /// <returns></returns>
        public static string ReadDataByAddress(string devicePKNO, int commType,
            string deviceAddress, string tagPKNO, string tagAddress, out string error)
        {
            DeviceCommInterface interfaceType = EnumHelper.ParserEnumByValue(commType, DeviceCommInterface.CNC_Fanuc);
            return ReadDataByAddress(devicePKNO, interfaceType, deviceAddress, 1000, tagPKNO, tagAddress, out error);
        }

        /// <summary>
        /// 读取信息
        /// </summary>
        /// <param name="devicePKNO"></param>
        /// <param name="commType"></param>
        /// <param name="deviceAddress"></param>
        /// <param name="updateRate"></param>
        /// <param name="tagPKNO"></param>
        /// <param name="tagAddress"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static string ReadDataByAddress(string devicePKNO, DeviceCommInterface commType,
            string deviceAddress, int updateRate, string tagPKNO, string tagAddress, out string error)
        {
            error = "";
            OperateResult<string> ret = ReadDataByAddress(devicePKNO, commType, deviceAddress, 1000, tagPKNO, tagAddress);
            if (!ret.IsSuccess)
            {
                error = ret.Message;
                return "";
            }

            return ret.Content;
        }

        /// <summary>
        /// 读取信息
        /// </summary>
        /// <param name="devicePKNO"></param>
        /// <param name="commType"></param>
        /// <param name="deviceAddress"></param>
        /// <param name="tagPKNO"></param>
        /// <param name="tagAddress"></param>
        /// <returns></returns>
        public static OperateResult<string> ReadDataByAddress(string devicePKNO, int commType,
            string deviceAddress, string tagPKNO, string tagAddress)
        {
            DeviceCommInterface interfaceType = EnumHelper.ParserEnumByValue(commType, DeviceCommInterface.CNC_Fanuc);
            return ReadDataByAddress(devicePKNO, interfaceType, deviceAddress, 1000, tagPKNO, tagAddress);
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="devicePKNO">设备PKNO，一般为Asset_Code</param>
        /// <param name="commType"></param>
        /// <param name="deviceAddress"></param>
        /// <param name="updateRate"></param>
        /// <param name="tagPKNO"></param>
        /// <param name="tagAddress"></param>
        /// <returns></returns>
        public static OperateResult<string> ReadDataByAddress(string devicePKNO, DeviceCommInterface commType,
            string deviceAddress, int updateRate, string tagPKNO, string tagAddress)
        {
            DeviceManager deviceComm =
                  CommDevices.FirstOrDefault(c => c.DevicePKNO == devicePKNO && c.DeviceTags.Any(d => d.PKNO == tagPKNO));

            bool bDispose = false;

            if (deviceComm == null) //没有添加监视
            {
                deviceComm = new DeviceManager(devicePKNO, commType, deviceAddress, updateRate);

                deviceComm.InitialDevice(tagPKNO, tagAddress);
                
                CommDevices.Add(deviceComm);
                bDispose = true;
            }

            OperateResult<string> ret = deviceComm.SyncReadData(tagAddress);

            if (bDispose)
            {
                deviceComm.Dispose();  //释放
            }

            return ret;
        }

        #endregion 
    }
}
