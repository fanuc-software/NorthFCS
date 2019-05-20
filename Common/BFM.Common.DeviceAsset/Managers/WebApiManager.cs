using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BFM.Common.Base;
using BFM.Common.DataBaseAsset;
using BFM.Common.DataBaseAsset.Enum;
using HslCommunication;

namespace BFM.Common.DeviceAsset
{
    /// <summary>
    /// WebApi方式
    /// 通讯地址格式 按照 | 分隔
    /// 地址格式：WebApi地址 | 自定义协议 | 协议参数
    /// </summary>
    public class WebApiManager : IDeviceCore
    {
        public DeviceManager _DevcieComm;

        #region 标准属性

        private Int64 pkid; //唯一标识

        private string CustomProtocol; //自定义协议
        private string ProtocolVariable; //自定义协议参数
        private List<DeviceTagParam> DeviceTags = new List<DeviceTagParam>();  //地址
        private DataChangeEventHandler Callback; //结束数据的反馈

        #endregion

        #region WebApi专用属性

        protected HttpClient client = null;

        #endregion

        public WebApiManager(DeviceManager devcieCommunication, DataChangeEventHandler callback)
        {
            _DevcieComm = devcieCommunication;

            Initial(devcieCommunication.DevicePKID, devcieCommunication.CommParam.CommAddress,
                devcieCommunication.CommParam.UpdateRate,devcieCommunication.DeviceTags, callback);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serverPKID">ID</param>
        /// <param name="address">地址：WebApi基地址 如 http://example.com/resources/ </param>
        /// <param name="updateRate"></param>
        /// <param name="deviceTagParams"></param>
        /// <param name="callback"></param>
        private void Initial(Int64 serverPKID, string address, int updateRate,
            List<DeviceTagParam> deviceTagParams, DataChangeEventHandler callback)
        {
            pkid = serverPKID;

            #region 初始化参数

            client = new HttpClient();
            string webapiurl = address;
            string[] addes = address.Split('|');  //分号隔开，前面是IP地址，后面是

            if (addes.Length > 1)
            {
                webapiurl = addes[0];
            }

            if (string.IsNullOrEmpty(webapiurl))
            {
                webapiurl = "http://localhost/BFM.WebApiService/";
            }
            client.BaseAddress = new Uri(webapiurl);  

            Callback = callback; //设置回调函数

            DeviceTags = deviceTagParams;  //标签

            #endregion

            #region 自定义协议

            CustomProtocol = (addes.Length >= 2) ? addes[1] : ""; //自定义协议
            ProtocolVariable = (addes.Length >= 3) ? addes[2] : ""; //自定义协议

            #endregion

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
            #region 检验

            if (string.IsNullOrEmpty(dataAddress))
            {
                return new OperateResult<string>("传入的参数都不能为空");
            }

            if (client == null)
            {
                return new OperateResult<string>("WebApi 接口设备，没有正确初始化");
            }

            #endregion

            string[] values = dataAddress.Split('|'); //WebApi地址，第一位为类型，第二位为地址，后面位参数

            try
            {
                object[] param = new object[values.Count() - 2];

                string sType = "PUT";
                string address = dataAddress;

                if (param.Length > 1)
                {
                    sType = SafeConverter.SafeToStr(param[0]).ToUpper();
                    address = SafeConverter.SafeToStr(param[1]);
                }

                for (int i = 2; i < values.Count(); i++)
                {
                    param[i - 2] = values[i];  //参数
                }

                string url = string.Format(address, param);

                HttpResponseMessage result = null;  //结果
                HttpContent content = new StringContent(dataValue);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json")
                {
                    CharSet = "utf-8"
                };

                if (sType == "GET")
                {
                    result = client.GetAsync(url).Result;
                }
                else if (sType == "PUT")
                {
                    result = client.PutAsync(url, content).Result;
                }
                else if (sType == "POST")
                {
                    result = client.PostAsync(url, content).Result;
                }
                else if (sType == "DELETE")
                {
                    result = client.DeleteAsync(url).Result;
                }

                if (result != null)
                {
                    string resultContent = result.Content.ReadAsStringAsync().Result;

                    return OperateResult.CreateSuccessResult<string>(resultContent);
                }

                return new OperateResult<string>("");
            }
            catch (Exception ex)
            {
                string error = $"---写入 WebApi 地址({dataAddress}) ) 失败，错误为({ex.Message.ToString()})";
                Console.WriteLine(error);
                return new OperateResult<string>(error);
            }
        }

        /// <summary>
        /// 同步读取数据
        /// </summary>
        /// <param name="dataAddress">WebApi地址，第一位为类型，第二位为地址，后面位参数</param>
        /// <returns></returns>
        public OperateResult<string> SyncReadData(string dataAddress)
        {
            #region 检验

            if (string.IsNullOrEmpty(dataAddress))
            {
                return new OperateResult<string>("传入的参数都不能为空");
            }

            if (client == null)
            {
                return new OperateResult<string>("WebApi 接口设备，没有正确初始化");
            }

            #endregion

            string[] values = dataAddress.Split('|'); //WebApi地址，第一位为类型，第二位为地址，后面位参数

            try
            {
                int count = values.Count() - 2;
                if (count <= 0)
                {
                    count = 0;

                }
                object[] param = new object[count];

                string sType = "GET";
                string address = dataAddress;

                if (param.Length > 1)
                {
                    sType = SafeConverter.SafeToStr(param[0]).ToUpper();
                    address = SafeConverter.SafeToStr(param[1]);
                }

                for (int i = 2; i < values.Count(); i++)
                {
                    param[i - 2] = values[i];  //参数
                }

                string url = string.Format(address, param);

                HttpResponseMessage result = null;  //结果
                HttpContent content = new StringContent("");
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json")
                {
                    CharSet = "utf-8"

                };

                if (sType == "GET")
                {
                    result = client.GetAsync(url).Result;
                }
                else if (sType == "PUT")
                {
                    result = client.PutAsync(url, content).Result;
                }
                else if (sType == "POST")
                {
                    result = client.PostAsync(url, content).Result;
                }
                else if (sType == "DELETE")
                {
                    result = client.DeleteAsync(url).Result;
                }

                if (result != null)
                {
                    string resultContent = result.Content.ReadAsStringAsync().Result;

                    return OperateResult.CreateSuccessResult<string>(resultContent);
                }

                return new OperateResult<string>("");
            }
            catch (Exception ex)
            {
                string error = $"---读取 WebApi 地址({dataAddress}) ) 失败，错误为({ex.Message.ToString()})";
                Console.WriteLine(error);
                return new OperateResult<string>(error);
            }
        }

        #endregion

        #region 异步读写数据

        public OperateResult AsyncWriteData(string dataAddress, string dataValue)
        {
            return SyncWriteData(dataAddress, dataValue);
        }

        public OperateResult AsyncReadData(string dataAddress)
        {
            return SyncReadData(dataAddress);
        }

        #endregion

        //释放
        public void Dispose()
        {
            client?.Dispose();
        }
    }

    enum WebApiOperate
    {
        /// <summary>
        /// GET：向指定的资源发出读取请求。该方法只用于读取资料，不应产生任何操作。
        /// </summary>
        [Description("GET")]
        GET = 0,
        /// <summary>
        /// POST：向指定资源提交数据，请求服务器进行处理。数据被包含在请求文本中。这个请求可能会建立新的资源或修改现有资源。
        /// </summary>
        [Description("POST")]
        POST = 1,
        /// <summary>
        /// PUT：向指定资源上传最新内容。
        /// </summary>
        [Description("PUT")]
        PUT = 2,
        /// <summary>
        /// DELETE：请求服务器删除资源
        /// </summary>
        [Description("DELETE")]
        DELETE = 3,
    }
}
