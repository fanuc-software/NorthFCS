using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFM.Common.DeviceAsset.Socket
{
    //Socket通讯特殊接口用
    public interface ISocketCommDevice
    {
        /// <summary>
        /// 解析服务端数据
        /// 标准：0：Tag地址；1：Tag值；2：其他自定义
        /// </summary>
        /// <returns></returns>
        List<string> AnalysisServerData(byte[] inData, out string error);

        /// <summary>
        /// 向设备写数据前获取发送的值
        /// </summary>
        /// <param name="sendValue">准备写入设备的主数据</param>
        /// <returns></returns>
        byte[] GetSendValueBeforeWrite(string sendValue);

        /// <summary>
        /// 向设备读取数据前获取发送的值
        /// </summary>
        /// <param name="sendValue">准备读取设备的主数据</param>
        /// <returns></returns>
        byte[] GetSendValueBeforeRead(string sendValue);

    }
}
