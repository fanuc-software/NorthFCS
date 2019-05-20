using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFM.Common.DeviceAsset.Socket;

namespace BFM.Common.DeviceAsset.Socket
{
    /// <summary>
    /// 蔡司三坐标设备 
    /// </summary>
    class SocketFACS : ISocketCommDevice
    {
        public List<string> AnalysisServerData(byte[] inData, out string error)
        {
            error = "";
            List<string> messgeList = new List<string>();

            //ICMMSTAT>1-00-00   已连接            
            //ICMMSTAT>1-00-10   正在测量
            //IMEASFIN>1 合格
            //IMEASFIN>7 不合格

            try
            {
                string inDataString = System.Text.Encoding.Default.GetString(inData).Replace("\r\n", "|");
                string[] inDataList = inDataString.Split('|');
                string usefulData = "";
                foreach (var str in inDataList)
                {
                    if (str.Length >= ("ICMMSTAT>1").Length) usefulData = str;

                    if (usefulData.Contains("IMEASFIN"))  //结果重要
                    {
                        break;
                    }
                }

                string[] resultValues = usefulData.Split('>');

                if (resultValues.Length < 2)
                {
                    messgeList.Add(inDataString);
                    return messgeList;
                }
                string messageHead = resultValues[0];  //数据头
                string messageBody = resultValues[1];  //结果
                switch (messageHead)
                {
                    case "ICMMSTAT":  //指令结果
                        messgeList.Add("三坐标设备状态");//0：离线；1 工作中；2：故障；3：待机
                        if (messageBody.Length >= 7)
                        {
                            string result = messageBody.Substring(5);
                            if (result == "01" || result == "00" || result == "07")  //待机
                            {
                                messgeList.Add("3");
                            }
                            else if (result == "10" || result == "11")  //工作
                            {
                                messgeList.Add("1");
                            }
                            else
                            {
                                messgeList.Add("0");
                            }
                        }
                        else
                        {
                            messgeList.Add("2");  //故障状态
                        }
                        messgeList.Add(inDataString);
                        break;
                    case "IMEASFIN":  //结果
                        messgeList.Add("三坐标检测结果");
                        messgeList.Add(messageBody == "1" ? "1" : (messageBody == "-" ? "2" : "0"));  //合格，不合格
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
    }
}
