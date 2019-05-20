using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFM.Common.DeviceAsset.Socket;

namespace BFM.Common.DeviceAsset.Socket
{
    /// <summary>
    /// 自动货柜 的 Socket 通讯协议解析
    /// </summary>
    public class SocketModula : ISocketCommDevice
    {
        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="inData"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public List<string> AnalysisServerData(byte[] inData, out string error)
        {
            error = "";
            List<string> messgeList = new List<string>();

            try
            {
                string inDataString = System.Text.Encoding.Default.GetString(inData);
                string[] resultValues = inDataString.Split('=');

                if (resultValues.Length < 2)
                {
                    messgeList.Add(inDataString);
                    return messgeList;
                }
                string messageHead = resultValues[0].ToUpper();
                string messageBody = resultValues[1];
                switch (messageHead)
                {
                    case "OKPART":  //成功出库下达指令
                        break;
                    case "OKRVAT":  //成功入库下达指令
                        break;
                    case "DPICK":
                        messgeList.Add("?DPICK");
                        messgeList.Add(DpickAnalysis(messageBody));
                        messgeList.Add(inDataString);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Socket报文数据解析出错");
                error = "Socket报文数据解析出错";
            }

            return messgeList;
        }

        /// <summary>
        /// 向设备写数据前获取发送的值
        /// </summary>
        /// <param name="sendValue">准备写入设备的主数据</param>
        /// <returns></returns>
        public byte[] GetSendValueBeforeWrite(string sendValue)
        {
            byte[] data = Encoding.Default.GetBytes(sendValue);

            return data;
        }

        /// <summary>
        /// 向设备读取数据前获取发送的值
        /// </summary>
        /// <param name="sendValue">准备读取设备的主数据</param>
        /// <returns></returns>
        public byte[] GetSendValueBeforeRead(string sendValue)
        {
            byte[] data = Encoding.Default.GetBytes(sendValue);

            return data;
        }

        //出入库返回结果
        public string DpickAnalysis(string messageBody)
        {
            List<string> messageList = new List<string>();
            string toolCode = (messageBody.Substring(0, 25)).Trim();//刀具编号
            string trayCode = (messageBody.Substring(25, 4)).Trim();//托盘编号
            string count = (messageBody.Substring(29, 4)).Trim();//数量
            string mode = (messageBody.Substring(33, 1)).Trim();//模式P:拿 V：取
            string position = (messageBody.Substring(34, 2)).Trim();//
            messageList.Add(toolCode);
            messageList.Add(trayCode);
            messageList.Add(count);
            messageList.Add(mode);
            messageList.Add(position);
            return toolCode + '|' + trayCode + '|' + mode + '|' + position;
        }
    }
}
