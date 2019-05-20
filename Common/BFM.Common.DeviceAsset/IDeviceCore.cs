using System;
using HslCommunication;

namespace BFM.Common.DeviceAsset
{
    public interface IDeviceCore : IDisposable
    {
        /// <summary>
        /// 同步写
        /// </summary>
        /// <param name="dataAddress"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        OperateResult SyncWriteData(string dataAddress, string dataValue);

        /// <summary>
        /// 同步读
        /// </summary>
        /// <param name="dataAddress"></param>
        /// <returns></returns>
        OperateResult<string> SyncReadData(string dataAddress);

        /// <summary>
        /// 异步写
        /// </summary>
        /// <param name="dataAddress"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        OperateResult AsyncWriteData(string dataAddress, string dataValue);

        /// <summary>
        /// 异步读取数据，没有返回值
        /// </summary>
        /// <param name="dataAddress"></param>
        /// <returns></returns>
        OperateResult AsyncReadData(string dataAddress);
    }
}
