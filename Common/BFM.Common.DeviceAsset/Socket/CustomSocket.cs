using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFM.Common.DeviceAsset.Socket
{
    public class CustomSocket
    {
        private static Dictionary<string, ISocketCommDevice> CustomSocketDevices =
            new Dictionary<string, ISocketCommDevice>();  //自定义解析设备

        static CustomSocket()
        {
            CustomSocketDevices.Add("Modula", new SocketModula());  //增加Modula的解析设备
            CustomSocketDevices.Add("FACS", new SocketFACS());  //增加蔡司FACS的解析设备
        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <param name="deviceInterface">设备接口</param>
        /// <param name="customProtocol"></param>
        /// <param name="protocolVariable"></param>
        /// <returns></returns>
        public static ISocketCommDevice GetDevice(DeviceCommInterface deviceInterface, string customProtocol,
            string protocolVariable)
        {
            ISocketCommDevice sockeDevice = null;

            string DeviceName = "";

            switch (deviceInterface)
            {
                case DeviceCommInterface.TCP_Custom:  //自定义协议
                    DeviceName = customProtocol;
                    break;
                case DeviceCommInterface.TCP_Server:  //自定义协议
                    DeviceName = customProtocol;
                    break;
                case DeviceCommInterface.ModulaTCP:
                    DeviceName = "Modula";
                    break;
                case DeviceCommInterface.ZeissTCP:
                    DeviceName = "FACS";
                    break;
            }

            if (CustomSocketDevices.ContainsKey(DeviceName))
            {
                sockeDevice = CustomSocketDevices[DeviceName];
            }
            return sockeDevice;
        }
    }
}
