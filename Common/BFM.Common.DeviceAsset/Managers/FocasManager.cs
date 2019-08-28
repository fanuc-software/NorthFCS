using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BFM.Common.Base;
using BFM.Common.Base.PubData;
using HslCommunication;

namespace BFM.Common.DeviceAsset
{
    /// <summary>
    /// Fanuc CNC 管理者
    /// </summary>
    public class FocasManager : IDeviceCore
    {
        /**************************************************
         *
         *  通讯地址格式 按照 | 分隔
         *  格式：IP地址 | 端口号 | 自定义协议 | 协议参数
         *
         *  对于Tag地址设定的含义：
         *  第一位为：R、Y、D、E、G、X 为标准的PMC数值
         *  第一位为：#表示处理宏变量
         *
         *  状态、状态2、程序号、主程序名称、工件数、工件总数、进给倍率、进给速度、主轴负载、主轴转速、报警信息、写入刀补
         *
         *
         ***************************************************/

        public DeviceManager _DevcieComm;

        public string CurThreadID { get; private set; } //当前线程ID

        #region 标准属性

        private Int64 pkid; //唯一标识

        private IPAddress ServerIP;
        private ushort ServerPort;

        private string CustomProtocol; //自定义协议
        private string ProtocolVariable; //自定义协议参数
        private List<DeviceTagParam> DeviceTags = new List<DeviceTagParam>(); //地址
        private DataChangeEventHandler Callback; //结束数据的反馈

        #endregion

        #region Focas专用

        private ushort _Flibhndl = 0; //连接Handel

        private object lockHandle = new object();

        #endregion

        #region constructor

        public FocasManager(DeviceManager devcieCommunication, DataChangeEventHandler callback)
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
            int port = 8193;

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
            ServerPort = (ushort) port;

            #endregion

            Callback = callback; //设置回调函数

            DeviceTags = deviceTagParams; //标签

            #region 自定义协议

            CustomProtocol = (addes.Length >= 3) ? addes[2] : ""; //自定义协议
            ProtocolVariable = (addes.Length >= 4) ? addes[3] : ""; //自定义协议

            #endregion
        }

        #endregion

        #region 同步读写数据

        /// <summary>
        /// 同步写数据，支持按照位写
        /// </summary>
        /// <param name="dataAddress">地址位；R2000</param>
        /// <param name="dataValues"></param>
        /// <returns></returns>
        public OperateResult SyncWriteData(string dataAddress, string dataValues)
        {
            if (string.IsNullOrEmpty(dataAddress) || string.IsNullOrEmpty(dataValues))
            {
                return new OperateResult("传入的参数都不能为空");
            }

            if (dataAddress.Length <= 1)
            {
                return new OperateResult("Tag地址长度不正确");
            }

            string dataValue = dataValues.Split('|')[0];

            #region 特殊地址解析

            string[] addresses = dataAddress.Split('|');

            if ((addresses.Length > 1) && (!string.IsNullOrEmpty(addresses[1])))
            {
                dataValue = string.Format(addresses[1].Replace("#13", "\n"), dataValues.Split('|'));
                dataAddress = addresses[0];
            }

            #endregion

            string sInfo = $"写入 Fanuc CNC 设备IP({ServerIP}) 地址({dataAddress}) 值({dataValue}) ";

            try
            {
                string sValueType = dataAddress.Substring(0, 1); //类型

                short valueType = 0; //参数类型

                string sAddr = dataAddress.Substring(1); //内部地址
                string startAddr = sAddr;
                int iGetPos = -1; //按位进行读时的位。

                if (sAddr.Contains('.')) //按位写
                {
                    startAddr = sAddr.Substring(0, sAddr.IndexOf('.'));
                    iGetPos = SafeConverter.SafeToInt(sAddr.Substring(sAddr.IndexOf('.') + 1));
                }

                #region 获取类型

                valueType = ValueTypeByStr(sValueType);

                if ((valueType == 0) && (dataAddress != "写入刀补")) //不支持的类型
                {
                    string errorType = $"Fanuc CNC 设备 IP({ServerIP}) 地址({dataAddress}) 值({dataValue})，写入失败，不支持的类型.";
                    Console.WriteLine(errorType);
                    return new OperateResult(errorType);
                }

                #endregion

                #region Focas 写值 

                Console.WriteLine($"---Fanuc CNC 设备 IP({ServerIP}) 地址({dataAddress}) 值({dataValue})");

                string error = "";
                ushort handel = GetFocasHandle(out error); //获取 连接Focas的Handel
                short ret;

                if (!string.IsNullOrEmpty(error))
                {
                    Console.WriteLine(sInfo + $"失败，连接错误代码({error})");
                    return new OperateResult(handel, sInfo + "失败，连接错误代码(" + error + ")");
                }

                ushort nAddr = SafeConverter.SafeToUshort(startAddr); //地址
                short nRValue = SafeConverter.SafeToShort(dataValue); //写入值

                if (dataAddress == "写入刀补")
                {
                    #region 写入刀补

                    ret = DownToolOffset(handel, dataValue, out error);

                    if (ret != Focas1.EW_OK) //失败，两次写入
                    {
                        FreeFocasHandle(out error); //释放
                        handel = GetFocasHandle(out error); //重新连接Focas的Handel
                        if (!string.IsNullOrEmpty(error))
                        {
                            Console.WriteLine(sInfo + $"失败，连接错误代码({error})");
                            return new OperateResult(ret, sInfo + "失败，连接错误代码(" + error + ")");
                        }

                        ret = DownToolOffset(handel, dataValue, out error);
                    }

                    #endregion
                }
                else if (sValueType == "#") //写宏变量
                {
                    #region 写宏变量

                    ret = SetMacroData(handel, SafeConverter.SafeToShort(nAddr), SafeConverter.SafeToDouble(dataValue),
                        out error);

                    if (ret != Focas1.EW_OK) //失败，两次写入
                    {
                        FreeFocasHandle(out error); //释放
                        handel = GetFocasHandle(out error); //重新连接Focas的Handel
                        if (!string.IsNullOrEmpty(error))
                        {
                            Console.WriteLine(sInfo + $"失败，连接错误代码({error})");
                            return new OperateResult(ret, sInfo + "失败，连接错误代码(" + error + ")");
                        }

                        ret = SetMacroData(handel, SafeConverter.SafeToShort(nAddr),
                            SafeConverter.SafeToDouble(dataValue), out error);
                    }

                    #endregion
                }
                else if (valueType < 100)
                {
                    #region 写PMC，先读后写

                    Focas1.IODBPMC0 pmcdata1 = new Focas1.IODBPMC0(); // for 1 Byte
                    ret = Focas1.pmc_rdpmcrng(handel, valueType, 0, nAddr, nAddr, 9,
                        pmcdata1); // D data of 1 Byte

                    if (ret != Focas1.EW_OK) //失败，两次写入
                    {
                        FreeFocasHandle(out error); //释放
                        handel = GetFocasHandle(out error); //重新连接Focas的Handel
                        if (!string.IsNullOrEmpty(error))
                        {
                            Console.WriteLine(sInfo + $"失败，连接错误代码({error})");
                            return new OperateResult(ret, sInfo + "失败，连接错误代码(" + error + ")");
                        }

                        ret = Focas1.pmc_rdpmcrng(handel, valueType, 0, nAddr, nAddr, 9, pmcdata1); // D data of 1 Byte
                        if (ret != Focas1.EW_OK)
                        {
                            Console.WriteLine(sInfo + "失败(读取时)，错误代码(" + ret + ")");
                            return new OperateResult(ret, sInfo + "失败(读取时)，错误代码(" + ret + ")");
                        }
                    }

                    if ((iGetPos >= 0) && (iGetPos <= 7)) //按位读取数据
                    {
                        int oldValue = pmcdata1.cdata[0];
                        byte newValue = (byte) (1 << iGetPos);
                        if (nRValue > 0) //置位
                        {
                            pmcdata1.cdata[0] = (byte) (oldValue | newValue); //按位或
                        }
                        else //复位
                        {
                            pmcdata1.cdata[0] = (byte) (oldValue & (newValue ^ 255)); //按位与
                        }
                    }
                    else //正常读写
                    {
                        pmcdata1.cdata[0] = Convert.ToByte(nRValue);
                    }

                    ret = Focas1.pmc_wrpmcrng(handel, nAddr, pmcdata1);

                    #endregion
                }
                 
                else
                {
                    return new OperateResult(handel, "写Fanuc CNC 数据失败！错误的类型！");
                }

                if (ret != Focas1.EW_OK)
                {
                    Console.WriteLine(sInfo + "失败(写入时)，错误代码(" + ret + ")");
                    return new OperateResult(ret, sInfo + "失败(写入时)，错误代码(" + ret + ")");
                }

                Console.WriteLine("-----" + sInfo + "成功---");

                #endregion

                return OperateResult.CreateSuccessResult(); //返回成功
            }
            catch (Exception ex)
            {
                string error = sInfo + $"失败，错误为({ex.Message})";
                Console.WriteLine(error);
                return new OperateResult(error);
            }
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="dataAddress">地址位；R2000；或者直接 状态 等特殊；R200.1；R200,3,2（地址，长度，类型）</param>
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
                string error = "";
                ushort handel = GetFocasHandle(out error); //获取 连接Focas的Handel

                if (!string.IsNullOrEmpty(error))
                {
                    return new OperateResult<string>(handel, error);
                }

                OperateResult<string> read = ReadFocasData(handel, dataAddress);
                if (!read.IsSuccess) //错误，重新读取一次
                {
                    FreeFocasHandle(out error);
                    handel = GetFocasHandle(out error); //重新连接Focas的Handel
                    if (!string.IsNullOrEmpty(error))
                    {
                        return new OperateResult<string>(handel, error);
                    }

                    read = ReadFocasData(handel, dataAddress); //再次读数据
                }

                return read;
            }
            catch (Exception ex)
            {
                string error = $"读取 设备IP({ServerIP}) 地址({dataAddress}) 失败，错误为({ex.Message.ToString()})";
                Console.WriteLine(error);
                return new OperateResult<string>(error);
            }
        }

        #endregion

        #region 异步读写数据 ====  数据库的异步读写和同步读写一样

        public OperateResult AsyncWriteData(string dataAddress, string dataValue)
        {
            return SyncWriteData(dataAddress, dataValue);
        }

        public OperateResult AsyncReadData(string dataAddress)
        {
            return SyncReadData(dataAddress);
        }

        #endregion

        public OperateResult SetPath(short pathnum)
        {
           
            if (ServerIP == null)
            {
                return new OperateResult<string>("设备未初始化，请先初始化。");
            }

            try
            {
                string error = "";
                ushort handel = GetFocasHandle(out error); //获取 连接Focas的Handel

                if (!string.IsNullOrEmpty(error))
                {
                    return new OperateResult<string>(handel, error);
                }

                OperateResult<string> setresult = SelectPath(handel, pathnum);
              
                return setresult;
            }
            catch (Exception ex)
            {
                string error = $"读取 设备IP({ServerIP}) 通道为({pathnum}) 失败，错误为({ex.Message.ToString()})";
                Console.WriteLine(error);
                return new OperateResult<string>(error);
            }
        }

        //public OperateResult GetPath(out short pahtnum,  out short pathcount)
        //{

        //    if (ServerIP == null)
        //    {
        //        return new OperateResult<string>("设备未初始化，请先初始化。");
        //    }

        //    try
        //    {
        //        string error = "";
        //        ushort handel = GetFocasHandle(out error); //获取 连接Focas的Handel
              
        //        if (!string.IsNullOrEmpty(error))
        //        {
        //            return new OperateResult<string>(handel, error);
        //        }

        //        OperateResult<string> setresult = GetPath(handel, pathnum);

        //        return setresult;
        //    }
        //    catch (Exception ex)
        //    {
        //        string error = $"读取 设备IP({ServerIP}) 通道为({pathnum}) 失败，错误为({ex.Message.ToString()})";
        //        Console.WriteLine(error);
        //        return new OperateResult<string>(error);
        //    }
        //}
        //释放
        public void Dispose()
        {
            FreeFocasHandle(out _);
        }

        #region  private

        //读取Focas的值
        private OperateResult<string> ReadFocasData(ushort handle, string dataAddress)
        {
            string error = ""; //错误信息
            string dataValue = "";

            string sValueType = dataAddress.Substring(0, 1); //类型

            #region 获取参数值

            if ((sValueType == "R") || (sValueType == "Y") ||
                (sValueType == "D") || (sValueType == "E") ||
                (sValueType == "G") || (sValueType == "X"))
            {
                #region 获取PMC变量值

                short valueType = ValueTypeByStr(sValueType); //内部参数类型
                string sAddr = dataAddress.Substring(1); //内部地址
                ushort nStarAddr = 0; //起始地址
                ushort nEndAddr = 0;
                int iDataType = 0; //0:字节Byte(CNC中每一个都是一个Byte，如R2000的2000表示一个Byte）；1:字型；2:双字型；
                int iGetPos = -1; //按位进行读时的位。
                ushort len = 1; //长度为1

                string[] addrsStrings = sAddr.Split(',');
                if (addrsStrings[0].Contains(".")) //字节
                {
                    iDataType = 0;
                    double dAddr = Double.Parse(addrsStrings[0]);
                    nStarAddr = (ushort) dAddr;
                    iGetPos = SafeConverter.SafeToInt(addrsStrings[0].Substring(sAddr.IndexOf('.') + 1));
                }
                else
                {
                    nStarAddr = Convert.ToUInt16(addrsStrings[0]);
                    if (addrsStrings.Count() > 1) len = Convert.ToUInt16(addrsStrings[1]); //长度
                    if (addrsStrings.Count() > 2) iDataType = Convert.ToUInt16(addrsStrings[2]); //获取的类型
                }

                nEndAddr = (ushort) (nStarAddr + (len) * Math.Pow(2, iDataType) - 1);

                ushort cncLen = (ushort) (8 + (len) * Math.Pow(2, iDataType));

                int[] values = new int[len];

                short ret = 0;

                switch (iDataType)
                {
                    case 0: //Byte
                        Focas1.IODBPMC0 pmcdata0 = new Focas1.IODBPMC0(); // 按照Byte
                        ret = Focas1.pmc_rdpmcrng(handle, valueType, 0, nStarAddr, nEndAddr, cncLen, pmcdata0);
                        if (ret == Focas1.EW_OK) //成功
                        {
                            for (int i = 0; i < len; i++)
                            {
                                values[i] = pmcdata0.cdata[i];
                            }
                        }
                        else
                        {
                            error = $"读取失败，错误代码[{ret}]";
                        }

                        break;
                    case 1:
                        Focas1.IODBPMC1 pmcdata1 = new Focas1.IODBPMC1(); // 按照字型
                        ret = Focas1.pmc_rdpmcrng(handle, valueType, 0, nStarAddr, nEndAddr, cncLen, pmcdata1);
                        if (ret == Focas1.EW_OK) //成功
                        {
                            for (int i = 0; i < len; i++)
                            {
                                values[i] = pmcdata1.idata[i];
                            }
                        }
                        else
                        {
                            error = $"读取失败，错误代码[{ret}]";
                        }

                        break;
                    case 2:
                        Focas1.IODBPMC2 pmcdata2 = new Focas1.IODBPMC2(); // 按照双字
                        ret = Focas1.pmc_rdpmcrng(handle, valueType, 0, nStarAddr, nEndAddr, cncLen, pmcdata2);
                        if (ret == Focas1.EW_OK) //成功
                        {
                            for (int i = 0; i < len; i++)
                            {
                                values[i] = pmcdata2.ldata[i];
                            }
                        }
                        else
                        {
                            error = $"读取失败，错误代码[{ret}]";
                        }

                        break;
                    default:
                        return new OperateResult<string>(error);
                }

                if ((iGetPos >= 0) && (iGetPos <= 7)) //按位读取数据
                {
                    string results = Convert.ToString(values[0], 2).PadLeft(8, '0');

                    if (results.Length < iGetPos)
                    {
                        dataValue = "0";
                    }
                    else
                    {
                        dataValue = results[7 - iGetPos].ToString();
                    }
                }
                else //正常读取
                {
                    dataValue = values[0].ToString();
                }

                #endregion
            }
            else if (sValueType == "#") //读取宏变量
            {
                string sAddr = dataAddress.Substring(1); //内部地址

                short number = SafeConverter.SafeToShort(sAddr);

                if (number == 0)
                {
                    return new OperateResult<string>("宏变量内部地址错误，地址为：" + sAddr);
                }

                dataValue = GetMacroData(handle, number, out error);
                if (error != "")
                {
                    return new OperateResult<string>($"读取宏变量[{sAddr}]，错误为：" + error);
                }
            }
            else if (dataAddress == "状态") //其他参数，按照参数类型进行读取
            {
                #region  获取状态

                dataValue = GetStatus(handle, out error).ToString();

                #endregion
            }
            else if (dataAddress == "状态2") //其他参数，按照参数类型进行读取
            {
                #region  获取状态

                dataValue = GetStatus2(handle, out error).ToString();

                #endregion
            }
            else if (dataAddress == "程序号") //读正在运行的程序信息
            {
                #region  程序号

                dataValue = GetProgNum(handle, out error);

                #endregion
            }
            else if (dataAddress == "主程序名称") //其他参数，按照参数类型进行读取
            {
                #region  主程序名称

                dataValue = GetProgComment(handle, out error);

                #endregion
            }
            else if (dataAddress == "工件数") //工件数
            {
                #region  工件数

                short num = 3901; //num = 3901 工件数   3902 总工件数

                dataValue = GetMacro(handle, num, out error).ToString();

                #endregion
            }
            else if (dataAddress == "工件总数") //工件计数
            {
                #region  工件计数

                short num = 3902; //num = 3901 工件数   3902 总工件数

                dataValue = GetMacro(handle, num, out error).ToString();

                #endregion
            }
            else if (dataAddress == "进给倍率") //进给倍率
            {
                #region  进给倍率

                dataValue = GetFeedRateOverride(handle, out error).ToString();

                #endregion
            }
            else if (dataAddress == "进给速度") //进给速度
            {
                #region  进给速度

                dataValue = GetFeedRateValue(handle, out error).ToString();

                #endregion
            }
            else if (dataAddress == "主轴负载") //主轴负载
            {
                #region  主轴负载

                dataValue = GetSPLoad(handle, out error).ToString();

                #endregion
            }
            else if (dataAddress == "主轴转速") //主轴转速
            {
                #region  主轴转速

                dataValue = GetSPSpeed(handle, out error).ToString();

                #endregion
            }
            else if (dataAddress == "报警信息") //主轴转速
            {
                #region  报警信息

                dataValue = ReadAlarm(handle, out error).ToString();

                #endregion
            }

            #endregion

            if (string.IsNullOrEmpty(error))
            {
                return OperateResult.CreateSuccessResult<string>(dataValue);
            }

            return new OperateResult<string>(error);
        }

        #region 通道切换
        private OperateResult<string> SelectPath(ushort handle, short pathnum)
        {
            string error = ""; //错误信息
            string dataValue = "";

            short nRet = Focas1.cnc_setpath(handle, pathnum);
         

            if (string.IsNullOrEmpty(error)&& nRet == Focas1.EW_OK)
            {
                return OperateResult.CreateSuccessResult<string>(dataValue);
            }

            return new OperateResult<string>(error);
        }

        private OperateResult<string> GetPath(ushort handle, out short pathnum,short pathcount)
        {
            string error = ""; //错误信息
            string dataValue = "";
          
            short nRet = Focas1.cnc_getpath(handle, out pathnum, out pathcount);


            if (string.IsNullOrEmpty(error) && nRet == Focas1.EW_OK)
            {
                return OperateResult.CreateSuccessResult<string>(dataValue);
            }

            return new OperateResult<string>(error);
        }
        #endregion
        #region Handle 相关

        /// <summary>
        /// 获取Handle
        /// </summary>
        /// <returns>Handel</returns>
        private ushort GetFocasHandle(out string error)
        {
            error = "";
            lock (lockHandle)
            {
                if (_Flibhndl == 0) //Handle 不存在时
                {
                    ushort flibhnd1 = 0;

                    try
                    {
                        short ret = Focas1.cnc_allclibhndl3(ServerIP.ToString(), ServerPort, 3, out flibhnd1); //建立连接

                        if (ret != Focas1.EW_OK) //连接失败
                        {
                            error = $"连接 Fanuc CNC 失败，设备IP({ServerIP}), ret=" + ret;
                            return (ushort) ret;
                        }
                    }
                    catch (Exception ex)
                    {
                        error = "连接 Fanuc CNC 失败，设备IP({ServerIP})，错误为：" + ex.Message;
                        return 0;
                    }

                    _Flibhndl = flibhnd1;
                }
            }

            return _Flibhndl; //返回Handel
        }

        //释放Handel
        private bool FreeFocasHandle(out string error)
        {
            bool result = true;
            error = "";
            if (_Flibhndl != 0)
            {
                try
                {
                    short ret = Focas1.cnc_freelibhndl(_Flibhndl); //释放连接
                    if (ret != Focas1.EW_OK) //连接失败
                    {
                        result = false;
                        error = "释放 Focas 失败. ret=" + ret;
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    error = "释放 Focas 失败，错误为：" + ex.Message;
                }
            }

            _Flibhndl = 0;
            return result;
        }

        #endregion

        #region 类型处理

        /// <summary>
        /// 获取类型号
        /// </summary>
        /// <param name="sValueType"></param>
        /// <returns></returns>
        private short ValueTypeByStr(string sValueType)
        {
            short valueType = 0;

            if (sValueType == "R")
            {
                valueType = 5;
            }
            else if (sValueType == "G")
            {
                valueType = 0;
            }
            else if (sValueType == "F")
            {
                valueType = 1;
            }
            else if (sValueType == "Y")
            {
                valueType = 2;
            }
            else if (sValueType == "X")
            {
                valueType = 3;
            }
            else if (sValueType == "C")
            {
                valueType = 8;
            }
            else if (sValueType == "D")
            {
                valueType = 9;
            }
            else if (sValueType == "E")
            {
                valueType = 12;
            }
            else if (sValueType == "#")
            {
                valueType = 101;
            }

            return valueType;
        }

        #endregion

        #region 读写 宏变量

        //获取宏变量数值，支持小数
        private string GetMacroData(ushort handle, short number, out string error)
        {
            error = "";
            Focas1.ODBM macro = new Focas1.ODBM();
            short nRet = Focas1.cnc_rdmacro(handle, number, 10, macro);
            if (nRet == Focas1.EW_OK)
            {
                return (macro.mcr_val * Math.Pow(10, -macro.dec_val)).ToString();
            }

            error = $"读取错误！错误代号[{nRet}]";
            return "";

        }

        //设置宏变量的值，支持小数输入
        private short SetMacroData(ushort handle, short number, double value, out string error)
        {
            error = "";
            short dec_val = (short) (value.ToString().Length - value.ToString().IndexOf('.') - 1);
            int mcr_val = (int) (value * Math.Pow(10, dec_val));
            short nRet = Focas1.cnc_wrmacro(handle, number, 10, mcr_val, dec_val);
            if (nRet != Focas1.EW_OK)
            {
                error = $"读取错误！错误代号[{nRet}]";
            }

            return nRet;
        }

        #endregion

        #region 写刀补

        private short DownToolOffset(ushort handle, string value, out string error)
        {
            error = "";

            //0：NC程序（NC program)；1：工件偏移（Tool offset data）；2：参数Parameter；3：螺距补偿Pitch error compensation data；4：用户宏变量Custom macro variables；5：Work zero offset data
            short type = 1;
            short nRet = Focas1.cnc_dwnstart4(handle, type, null);
            if (nRet != Focas1.EW_OK)
            {
                error = $"下载刀补值失败！错误代号[{nRet}]，参数[{value}]";
                return nRet;
            }

            byte[] buf = new byte[257];

            byte[] beginBuf = new byte[2];
            beginBuf[0] = 0x25;
            beginBuf[1] = 0x0A;
            Array.Copy(beginBuf, buf, beginBuf.Length);

            byte[] tempbuf = System.Text.Encoding.Default.GetBytes(value); // for String of CNC program
            Array.Copy(tempbuf, 0, buf, beginBuf.Length, tempbuf.Length);

            byte[] endBuf = new byte[1];
            endBuf[0] = 0x25;
            Array.Copy(endBuf, 0, buf, beginBuf.Length + tempbuf.Length, endBuf.Length);

            int len = beginBuf.Length + tempbuf.Length + endBuf.Length;
            do
            {
                nRet = Focas1.cnc_download4(handle, ref len, buf);
            } while (nRet == 10); //// Focas1.focas_ret.EW_BUFFER  );

            if (nRet != Focas1.EW_OK)
            {
                nRet = Focas1.cnc_dwnend3(handle);
                error = $"下载刀补值失败！错误代号[{nRet}]，参数[{value}]";
                return nRet;
            }

            nRet = Focas1.cnc_dwnend3(handle);


            if (nRet == 5)
            {
                Focas1.ODBERR odberr = new Focas1.ODBERR();
                nRet = Focas1.cnc_getdtailerr(handle, odberr);
                short dataError = odberr.err_no;
                ////1 : A character which is unavailable for NC program is detected. 
                ////2 : When TV check is effective, a block which includes odd number of characters (including 'LF' at the end of the block) is detected. 
                ////3 : The registered program count is full. 
                ////4 : The same program number has already been registered. 
                ////5 : The same program number is selected on CNC. 
            }

            if (nRet != Focas1.EW_OK)
            {
                error = $"下载刀补值失败！错误代号[{nRet}]，参数[{value}]";
                return nRet;
            }

            return nRet;
        }

        #endregion

        #region 采集信息

        /// <summary>
        /// 1 运行 2待机 3报警 4急停  
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private int GetStatus(ushort handle, out string error)
        {
            error = "";
            Focas1.ODBST statbuf = new Focas1.ODBST();
            short nRet = Focas1.cnc_statinfo(handle, statbuf);

            if (nRet != Focas1.EW_OK)
            {
                error = $"读取错误！错误代号[{nRet}]";
                return -1;
            }

            int iState = 0;

            if (statbuf.emergency > 0) //急停 1 : EMerGency 2 : ReSET 
            {
                iState = 4;
            }
            else if (statbuf.alarm > 0) //故障 3报警  1 : ALarM  
            {
                iState = 3;
            }
            //运行中 1 : STOP 2 : HOLD 3 : STaRT  4 : MSTR(during retraction and re-positioning of tool retraction and recovery, and operation of JOG MDI) 
            else if (statbuf.run > 0) 
            {
                iState = 1;
            }
            else //待机
            {
                iState = 2;
            }

            return iState;
        }

        /// <summary>
        /// 1 运行 2待机 3报警 4急停  
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private int GetStatus2(ushort handle, out string error)
        {
            error = "";
            Focas1.ODBST2 statbuf = new Focas1.ODBST2();
            short nRet = Focas1.cnc_statinfo2(handle, statbuf);

            if (nRet != Focas1.EW_OK)
            {
                error = $"读取错误！错误代号[{nRet}]";
                return -1;
            }

            int iState = 0;

            if (statbuf.emergency > 0) //急停 1 : EMerGency 2 : ReSET 
            {
                iState = 4;
            }
            else if (statbuf.alarm == 1) //故障 3报警  1 : ALarM  2 : BATtery low 
            {
                iState = 3;
            }
            else if (statbuf.run > 0
            ) //运行中 1 : STOP 2 : HOLD 3 : STaRT  4 : MSTR(during retraction and re-positioning of tool retraction and recovery, and operation of JOG MDI) 
            {
                iState = 1;
            }
            else //待机
            {
                iState = 2;
            }

            return iState;
        }

        //获得程序号
        private string GetProgNum(ushort handle, out string error)
        {
            error = "";
            Focas1.ODBEXEPRG pname = new Focas1.ODBEXEPRG();
            short nRet = Focas1.cnc_exeprgname(handle, pname);
            if (nRet != Focas1.EW_OK)
            {
                error = $"读取错误！错误代号[{nRet}]";
                return "";
            }

            return "O" + pname.o_num.ToString();
        }

        //主程序名称
        private string GetProgComment(ushort handle, out string error)
        {
            error = "";
            //获得当前主程序号
            Focas1.ODBM macro = new Focas1.ODBM();
            short nRet = Focas1.cnc_rdmacro(handle, 4000, 10, macro);
            if (nRet == Focas1.EW_OK)
            {
                Focas1.PRGDIR3 buf1 = new Focas1.PRGDIR3();
                double dSmall = macro.dec_val * 1.0;
                double top_prog = macro.mcr_val / Math.Pow(10.0, dSmall);
                int ntop_prog = Convert.ToInt32(top_prog);
                short num_prog = 1;
                nRet = Focas1.cnc_rdprogdir3(handle, 1, ref ntop_prog, ref num_prog, buf1);

                if (nRet == Focas1.EW_OK)
                {
                    return buf1.dir1.comment;
                }
            }

            error = $"读取错误！错误代号[{nRet}]";
            return "";
        }
        
        //工件计数  num = 3901 工件数   3902 总工件数
        private long GetMacro(ushort handle, short num, out string error)
        {
            error = "";
            Focas1.IODBMR macror = new Focas1.IODBMR();
            short s_number = num;
            short e_number = num;
            short length = 100;

            long MacroValue = 0;
            short nRet = Focas1.cnc_rdmacror(handle, s_number, e_number, length, macror);

            //short nRet2 = Focas1.cnc_rdparam(handle, 6712, -1, 4 + Focas1.MAX_AXIS, Param);

            if (nRet == Focas1.EW_OK)
            {
                MacroValue = macror.data.data1.mcr_val;
                for (int i = 0; i < macror.data.data1.dec_val; i++)
                    MacroValue = MacroValue / 10;
            }

            if ((MacroValue == 0) && (num == 3902))  
            {
                //方法二工件总数
                Focas1.IODBPSD_1 Param = new Focas1.IODBPSD_1();
                nRet = Focas1.cnc_rdparam(handle, 6712, -1, 4 + Focas1.MAX_AXIS, Param);
                if (nRet == Focas1.EW_OK)
                {
                    MacroValue = Param.ldata;
                }
            }

            if (nRet != Focas1.EW_OK) error = $"读取错误！错误代号[{nRet}]";

            return MacroValue;
        }

        //进给倍率
        private int GetFeedRateOverride(ushort handle, out string error)
        {
            error = "";
            //进给倍率,攻进倍率：0-200% 10%为1档   进给G01 取地址G12
            Focas1.IODBPMC0 info = new Focas1.IODBPMC0();
            short nRet = Focas1.pmc_rdpmcrng(handle, 0, 0, 12, 12, 9, info);
            if (nRet == Focas1.EW_OK)
            {
                int nFeedRate = 255 - info.cdata[0];
                return nFeedRate;
            }

            error = $"读取错误！错误代号[{nRet}]";
            return 0;
        }

        //进给速度
        private int GetFeedRateValue(ushort handle, out string error)
        {
            error = "";
            //进给速度
            Focas1.ODBACT actf = new Focas1.ODBACT();
            short nRet = Focas1.cnc_actf(handle, actf);

            if (nRet == Focas1.EW_OK)
            {
                return actf.data;
            }

            error = $"读取错误！错误代号[{nRet}]";
            return 0;
        }

        //主轴负载
        private int GetSPLoad(ushort handle, out string error)
        {
            error = "";
            //主轴负载
            Focas1.ODBSPLOAD odbspload = new Focas1.ODBSPLOAD();
            short data_num = 1;
            short nRet = Focas1.cnc_rdspmeter(handle, 0, ref data_num, odbspload);
            if (nRet == Focas1.EW_OK)
            {
                int processValue =
                    (int) (odbspload.spload1.spload.data * System.Math.Pow(10, -odbspload.spload1.spload.dec));

                return (processValue > 200) ? 200 : processValue;
            }

            error = $"读取错误！错误代号[{nRet}]";
            return 0;
        }

        //主轴转速
        private string GetSPSpeed(ushort handle, out string error)
        {
            error = "";
            //主轴转速
            Focas1.ODBACT odbspeed = new Focas1.ODBACT();

            short nRet = Focas1.cnc_acts(handle, odbspeed);
            if (nRet == Focas1.EW_OK)
            {
                return odbspeed.data.ToString();
            }

            error = $"读取错误！错误代号[{nRet}]";
            return "";
        }

        /// <summary>
        /// 报警信息采集
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="error"></param>
        /// <returns>以@区分，分别[0]报警编号[1]报警类型[2]警报信息</returns>
        private string ReadAlarm(ushort handle, out string error)
        {
            error = "";
            short ret1;
            short type = -1;
            short num = 100; //设大一些没关系
            string[] alarm_type =
            {
                "SW", "PW", "IO", "PS", "OT", "OH", "SV", "SR", "MC", "SP", "DS", "IE", "BG", "SN", "", "EX", "", "",
                "", "PC"
            };

            Focas1.ODBALMMSG2 odbalmmsg2 = new Focas1.ODBALMMSG2();
            ret1 = Focas1.cnc_rdalmmsg2(handle, type, ref num, odbalmmsg2);
            string strAlmNO = "";
            string strAlmcode = "";
            string strAlmmsg = "";
            if (ret1 == Focas1.EW_OK)
            {
                strAlmNO = odbalmmsg2.msg1.alm_no.ToString();
                strAlmcode = alarm_type[odbalmmsg2.msg1.type];
                strAlmmsg = System.Text.Encoding.Default.GetString(
                    System.Text.Encoding.Default.GetBytes(odbalmmsg2.msg1.alm_msg), 0, odbalmmsg2.msg1.msg_len);
            }
            else
            {
                error = $"读取错误！原因{ret1}";
            }

            return strAlmNO + "@" + strAlmcode + "@" + strAlmmsg;
        }

        #endregion

        #region 特殊设置信息

        #region 程序相关

        /// <summary>
        /// 设置主程序 => 按照程序号
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="error"></param>
        /// <returns>0:成功；其他失败</returns>
        private int SetMainProgram(ushort handle, short programNO, out string error)
        {
            error = "";
            short nRet = Focas1.cnc_search(handle, programNO);
            if (nRet != Focas1.EW_OK)
            {
                error = $"程序号{programNO}设置主程序错误！错误代码{nRet}";
            }

            return nRet;
        }

        /// <summary>
        /// 删除指定的程序
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="programNO"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private int DelProgram(ushort handle, short programNO, out string error)
        {
            error = "";
            short nRet = Focas1.cnc_delete(handle, programNO);
            if (nRet != Focas1.EW_OK)
            {
                error = $"程序号{programNO}删除错误！错误代码{nRet}";
            }

            return nRet;
        }

        #endregion

        #endregion

        #region  直接获取信息 - 测试用

        //获得状态
        /// <summary>
        /// 获取CNC运行状态
        /// </summary>
        /// <param name="deviceAddress"></param>
        /// <param name="error"></param>
        /// <returns>1 运行 2报警 3急停 4 待机</returns>
        private static int GetStatus(string deviceAddress, out string error)
        {
            error = "";
            Focas1.ODBST statbuf = new Focas1.ODBST();
            ushort m_handle;
            short nRet = Focas1.cnc_allclibhndl3(deviceAddress, 8193, 2, out m_handle);
            if (nRet == Focas1.EW_OK)
            {
                nRet = Focas1.cnc_statinfo(m_handle, statbuf);
            }

            if (nRet == Focas1.EW_OK)
            {
                int iState = 0;

                if (statbuf.run > 0) //运行中
                {
                    iState = 1;
                }
                else if (statbuf.alarm > 0) //故障
                {
                    iState = 2;
                }
                else if (statbuf.emergency > 0) //急停
                {
                    iState = 3;
                }
                else //待机
                {
                    iState = 4;
                }

                Focas1.cnc_freelibhndl(m_handle); //释放连接
                return iState;
            }

            error = "连接失败";
            return -1;
        }

        //获得程序号
        private static string GetProgNum(string deviceAddress, out string error)
        {
            error = "";
            Focas1.ODBEXEPRG pname = new Focas1.ODBEXEPRG();
            ushort m_handle;
            short nRet = Focas1.cnc_allclibhndl3(deviceAddress, 8193, 2, out m_handle);
            if (nRet == Focas1.EW_OK)
            {
                nRet = Focas1.cnc_exeprgname(m_handle, pname);
            }

            if (nRet == Focas1.EW_OK)
            {
                Focas1.cnc_freelibhndl(m_handle); //释放连接
                return "O" + pname.o_num.ToString();
            }

            error = "连接失败";
            return "O0";
        }

        //主程序名称
        private static string GetProgComment(string deviceAddress, out string error)
        {
            error = "";
            //获得当前主程序号
            Focas1.ODBM macro = new Focas1.ODBM();
            Focas1.PRGDIR3 buf1 = new Focas1.PRGDIR3();
            ushort m_handle;
            short nRet = Focas1.cnc_allclibhndl3(deviceAddress, 8193, 2, out m_handle);
            if (nRet == Focas1.EW_OK)
            {
                nRet = Focas1.cnc_rdmacro(m_handle, 4000, 10, macro);
            }

            if (nRet == Focas1.EW_OK)
            {
                //****************
                double dSmall = macro.dec_val * 1.0;
                double top_prog = macro.mcr_val / Math.Pow(10.0, dSmall);
                int ntop_prog = Convert.ToInt32(top_prog);
                short num_prog = 1;
                nRet = Focas1.cnc_rdprogdir3(m_handle, 1, ref ntop_prog, ref num_prog, buf1);
            }

            if (nRet == Focas1.EW_OK)
            {
                Focas1.cnc_freelibhndl(m_handle); //释放连接
                return buf1.dir1.comment;
            }

            error = "连接失败";
            return "";
        }

        //工件计数
        private static long GetMacro(string deviceAddress, short num, out string error)
        {
            error = "";
            Focas1.IODBMR macror = new Focas1.IODBMR();
            ushort m_handle;
            short nRet = Focas1.cnc_allclibhndl3(deviceAddress, 8193, 2, out m_handle);
            short s_number = num;
            short e_number = num;
            short length = 100;
            long MacroValue = 0;
            if (nRet == Focas1.EW_OK)
            {
                nRet = Focas1.cnc_rdmacror(m_handle, s_number, e_number, length, macror);
            }

            if (nRet == Focas1.EW_OK)
            {
                MacroValue = macror.data.data1.mcr_val;
                for (int i = 0; i < macror.data.data1.dec_val; i++)
                    MacroValue = MacroValue / 10;

                Focas1.cnc_freelibhndl(m_handle); //释放连接
                return MacroValue;
            }

            error = "连接失败";
            return 0;
        }

        //获得4字节整数
        private static int GetPmc(string deviceAddress, string type, ushort num, out string error)
        {
            error = "";
            short nType = 12;
            if (type == "D")
            {
                nType = 9;
            }

            if (type == "E")
            {
                nType = 12;
            }

            Focas1.IODBPMC2 info = new Focas1.IODBPMC2();
            ushort m_handle;
            short nRet = Focas1.cnc_allclibhndl3(deviceAddress, 8193, 2, out m_handle);

            if (nRet == Focas1.EW_OK)
            {
                ushort nstart = num;
                ushort offense = 3;
                ushort nend = Convert.ToUInt16(num + offense);
                nRet = Focas1.pmc_rdpmcrng(m_handle, nType, 2, nstart, nend, 12, info);
            }

            if (nRet == Focas1.EW_OK)
            {
                int value = info.ldata[0];

                Focas1.cnc_freelibhndl(m_handle); //释放连接
                return value;
            }

            error = "连接失败";
            return -16;
        }

        //进给倍率
        private static int GetFeedRateOverride(string deviceAddress, out string error)
        {
            error = "";
            //进给倍率,攻进倍率：0-200% 10%为1档   进给G01 取地址G12
            ushort m_handle;
            Focas1.IODBPMC0 info = new Focas1.IODBPMC0();
            short nRet = Focas1.cnc_allclibhndl3(deviceAddress, 8193, 2, out m_handle);
            if (nRet == Focas1.EW_OK)
            {
                //  Focas1.pmc_rdpmcrng(Handle, 0, 1, 12, 12, 10, info); 
                nRet = Focas1.pmc_rdpmcrng(m_handle, 0, 0, 12, 12, 9, info);
            }

            if (nRet == Focas1.EW_OK)
            {
                int nFeedRate = 255 - info.cdata[0];

                Focas1.cnc_freelibhndl(m_handle); //释放连接
                return nFeedRate;
            }

            error = "连接失败";
            return 0;
        }

        //进给速度
        private static int GetFeedRateValue(string deviceAddress, out string error)
        {
            error = "";
            //进给速度
            ushort m_handle;
            Focas1.ODBACT actf = new Focas1.ODBACT();
            short nRet = Focas1.cnc_allclibhndl3(deviceAddress, 8193, 2, out m_handle);
            if (nRet == Focas1.EW_OK)
            {
                nRet = Focas1.cnc_actf(m_handle, actf);
            }

            if (nRet == Focas1.EW_OK)
            {
                Focas1.cnc_freelibhndl(m_handle); //释放连接
                return actf.data;
            }

            error = "连接失败";
            return 0;
        }

        //主轴负载
        private static int GetSPLoad(string deviceAddress, out string error)
        {
            error = "";
            //主轴负载
            ushort m_handle;
            Focas1.ODBSPLOAD odbspload = new Focas1.ODBSPLOAD();
            short nRet = Focas1.cnc_allclibhndl3(deviceAddress, 8193, 2, out m_handle);
            if (nRet == Focas1.EW_OK)
            {
                short data_num = 1;
                nRet = Focas1.cnc_rdspmeter(m_handle, 0, ref data_num, odbspload);
            }

            if (nRet == Focas1.EW_OK)
            {
                int processValue =
                    (int) (odbspload.spload1.spload.data * System.Math.Pow(10, -odbspload.spload1.spload.dec));

                Focas1.cnc_freelibhndl(m_handle); //释放连接
                return (processValue > 200) ? 200 : processValue;
            }

            error = "连接失败";
            return 0;
        }

        #endregion

        #endregion
    }
}
