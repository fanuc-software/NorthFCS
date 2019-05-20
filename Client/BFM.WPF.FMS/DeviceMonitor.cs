using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BFM.Common.Base;
using BFM.Common.Base.Helper;
using BFM.Common.Base.Log;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.Data.PubData;
using BFM.Server.DataAsset.FMSService;
using BFM.Common.DeviceAsset;
using BFM.ContractModel;
using BFM.Server.DataAsset.DAService;
using HslCommunication;
using Microsoft.CSharp;

namespace BFM.WPF.FMS
{
    /// <summary>
    /// 设备数据监控模块
    /// </summary>
    public class DeviceMonitor
    {
        /// <summary>
        /// 刷新计算的规则
        /// 设置一次true，刷新一次
        /// </summary>
        private static bool bRefreshAutoCal = true;  //刷新重新计算的信息

        /// <summary>
        /// 重新刷新计算规则
        /// </summary>
        public static void RefreshAutoCalSetting()
        {
            bRefreshAutoCal = true;
        }

        #region 静态变量，作用提高采集及控制速度

        #region private

        /// <summary>
        /// 是否为数据采集，只允许开启一个，防止多个采集
        /// </summary>
        private static bool bMonitor = false;

        /// <summary>
        /// 保存当前值的周期
        /// </summary>
        private static int SaveCurValueSpan = SafeConverter.SafeToInt(Configs.GetValue("SaveDataSpan"), 0);   //单位秒，<=0时不保存  60*n=n分钟;

        /// <summary>
        /// 超时清空数据
        /// </summary>
        private static int TimeOutClear = SafeConverter.SafeToInt(Configs.GetValue("TimeOutClear"), 3) * 60;   //单位秒，<=0时不保存  60*n=n分钟;3 * 60;  //超时清空数据

        /// <summary>
        /// 当前标签值是否更新
        /// </summary>
        //private static Dictionary<string, bool> CurValueChanged = new Dictionary<string, bool>();
        private static List<TagValueChange> CurValueChanged = new List<TagValueChange>();

        /// <summary>
        /// 设备Tag值，保存在内存中，提高效率
        /// </summary>
        private static List<FmsAssetTagSetting> _tagSettings = null;

        /// <summary>
        /// 设备通讯类，保存在内存中，提高效率
        /// </summary>
        private static List<FmsAssetCommParam> _devices = null;

        /// <summary>
        /// 记录值
        /// </summary>
        private static List<FmsSamplingRecord> AddSimplingRecords = new List<FmsSamplingRecord>();
        /// <summary>
        /// 更新状态记录
        /// </summary>
        private static List<FmsStateResultRecord> UpdateResultRecords = new List<FmsStateResultRecord>();
        /// <summary>
        /// 更新状态记录
        /// </summary>
        private static List<FmsStateResultRecord> AddResultRecords = new List<FmsStateResultRecord>();

        #endregion

        #region 公共函数

        /// <summary>
        /// 按条件获取Tag点
        /// 监控开启时直接获取内存的值，否则提取数据库的值
        /// </summary>
        public static List<FmsAssetTagSetting> GetTagSettings(string sWhere)
        {
            #region 整理查询条件

            if (string.IsNullOrEmpty(sWhere))
            {
                sWhere = "USE_FLAG = 1";
            }
            else
            {
                sWhere = "USE_FLAG = 1 AND " + sWhere;
            }

            #endregion

            if ((!bMonitor) || (_tagSettings == null)) //没有开启监控 || 空
            {
                WcfClient<IFMSService> wsthis = new WcfClient<IFMSService>();
                return wsthis.UseService(s => s.GetFmsAssetTagSettings(sWhere)); //获取Tag配置点
            }
            
            Expression<Func<FmsAssetTagSetting, bool>> whereLamda =
                SerializerHelper.ConvertParamWhereToLinq<FmsAssetTagSetting>(sWhere);

            return _tagSettings.Where(whereLamda.Compile()).ToList();
        }

        /// <summary>
        /// 按照PKNO获取Tag点
        /// 监控开启时直接获取内存的值，否则提取数据库的值
        /// </summary>
        /// <param name="tagPKNO">Tag点的PKNO</param>
        /// <returns>Tag点</returns>
        public static FmsAssetTagSetting GetTagSettingById(string tagPKNO)
        {
            if (string.IsNullOrEmpty(tagPKNO))
            {
                return null;
            }

            if ((!bMonitor) || (_tagSettings == null) || (!_tagSettings.Any())) //没有开启监控 || 空
            {
                WcfClient<IFMSService> wsThis = new WcfClient<IFMSService>();

                return wsThis.UseService(s => s.GetFmsAssetTagSettingById(tagPKNO));
            }

            return _tagSettings.FirstOrDefault(c => c.PKNO == tagPKNO);
        }

        /// <summary>
        /// 设置Tag点的值，结果OK
        /// </summary>
        /// <param name="tagSetting">Tag点</param>
        /// <param name="newValue">新的值</param>
        /// <returns>OK;错误代码</returns>
        public static string SetTagSettingValue(FmsAssetTagSetting tagSetting, string newValue)
        {
            string error = "OK";
            if (tagSetting == null)
            {
                return "标签参数为空";
            }

            try
            {
                if (bMonitor) //已经开启监控
                {
                    if (tagSetting.CUR_VALUE != newValue) //新的值 != 原值
                    {
                        TagValueChange tagValue = CurValueChanged.FirstOrDefault(c => c.PKNO == tagSetting.PKNO);
                        if (tagValue != null)
                        {
                            tagValue.CurValue = newValue;  //当前值已经更新了
                        }
                        else  //增加新增
                        {
                            CurValueChanged.Add(new TagValueChange(tagSetting.PKNO, newValue, tagSetting.SAMPLING_MODE, tagSetting.STATE_MARK_TYPE));
                        }

                        tagSetting.CUR_VALUE = newValue; //更新当前值
                    }
                }
                else  //没有开启监控
                {
                    tagSetting.CUR_VALUE = newValue;
                    WcfClient<IFMSService> wsThis = new WcfClient<IFMSService>();
                    wsThis.UseService(s => s.UpdateFmsAssetTagSetting(tagSetting));
                }

            }
            catch (Exception e)
            {
                error = "手动更新Tag点的值失败，具体 " + e.Message;
                Console.WriteLine(error);
            }

            return error;
        }

        /// <summary>
        /// 设置Tag点的值，结果OK
        /// </summary>
        /// <param name="tagPKNO">Tag点的PKNO</param>
        /// <param name="newValue">新的值</param>
        /// <returns>OK;错误代码</returns>
        public static string SetTagSettingValueById(string tagPKNO, string newValue)
        {
            FmsAssetTagSetting tag = DeviceMonitor.GetTagSettingById(tagPKNO);
            return SetTagSettingValue(tag, newValue);
        }

        /// <summary>
        /// 向设备读取标签值
        /// </summary>
        /// <param name="tagPKNO"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static string ReadTagFormDevice(string tagPKNO, out string error)
        {
            int ret = 0;
            error = "";
            if (string.IsNullOrEmpty(tagPKNO))
            {
                error = "标签PKNO为空";
                return "";
            }

            FmsAssetTagSetting tagSetting = GetTagSettingById(tagPKNO);
            if (tagSetting == null)
            {
                error = "标签不存在";
                return "";
            }

            if (_devices == null)
            {
                WcfClient<IFMSService> ws = new WcfClient<IFMSService>();
                _devices = ws.UseService(s => s.GetFmsAssetCommParams("USE_FLAG = 1"));    //可用的设备
            }

            FmsAssetCommParam deviceCommParam = _devices.FirstOrDefault(c => c.ASSET_CODE == tagSetting.ASSET_CODE);

            if (deviceCommParam == null)
            {
                error = "通讯设备不存在";
                return "";
            }

            OperateResult<string> result = DeviceHelper.ReadDataByAddress(deviceCommParam.ASSET_CODE,
                deviceCommParam.INTERFACE_TYPE, deviceCommParam.COMM_ADDRESS,
                tagSetting.PKNO, tagSetting.TAG_ADDRESS); //读取设备

            if (!result.IsSuccess)
            {
                error = "向设备写入失败，错误：" + result.Message;
                return "";
            }

            return result.Content;
        }

        /// <summary>
        /// 向设备写入标签值
        /// </summary>
        /// <param name="tagPKNO"></param>
        /// <param name="value"></param>
        /// <param name="error">输出错误信息</param>
        /// <returns>0:成功；1:标签PKNO为空；2：标签不存在;3:通讯设备不存在；10：写入设备失败</returns>
        public static int WriteTagToDevice(string tagPKNO, string value, out string error)
        {
            int ret = 0;
            error = "";
            if (string.IsNullOrEmpty(tagPKNO))
            {
                error = "标签PKNO为空";
                return 1;
            }

            FmsAssetTagSetting tagSetting = GetTagSettingById(tagPKNO);
            if (tagSetting == null)
            {
                error = "标签不存在";
                return 2;
            }

            if (_devices == null)
            {
                WcfClient<IFMSService> ws = new WcfClient<IFMSService>();
                _devices = ws.UseService(s => s.GetFmsAssetCommParams("USE_FLAG = 1"));    //可用的设备
            }

            FmsAssetCommParam deviceCommParam = _devices.FirstOrDefault(c => c.ASSET_CODE == tagSetting.ASSET_CODE);

            if (deviceCommParam == null)
            {
                error = "通讯设备不存在";
                return 3;
            }

            OperateResult result = DeviceHelper.WriteDataByAddress(deviceCommParam.ASSET_CODE,
                deviceCommParam.INTERFACE_TYPE, deviceCommParam.COMM_ADDRESS,
                tagSetting.PKNO, tagSetting.TAG_ADDRESS, value); //写入设备

            if (!result.IsSuccess)
            {
                error = "向设备写入失败，错误：" + result.Message;
                return 10;
            }

            //更新当前值
            SetTagSettingValue(tagSetting, value);

            return ret;
        }

        #endregion

        #endregion

        private WcfClient<IFMSService> ws = new WcfClient<IFMSService>();
        private WcfClient<IDAService> ws_DA = new WcfClient<IDAService>();

        object lockRecord = new object();  //记录锁
        object lockStateRecord = new object();  //状态记录锁
        object lockSave = new object();  //记录锁
        object lockUpdateCurValue = new object();  //更新当前值锁
        object locakDA = new object();  //DA锁

        /// <summary>
        /// 第一次结果
        /// </summary>
        private Dictionary<string, bool> ResultRecordFirsts = new Dictionary<string, bool>();

        private bool bPause = false;  //暂停

        public void Do()
        {
            if (bMonitor) return; //已开启监控
            bMonitor = true;  //开启监控

            _tagSettings = ws.UseService(s => s.GetFmsAssetTagSettings($"USE_FLAG = 1")); //获取所有Tag配置点
            _devices = ws.UseService(s => s.GetFmsAssetCommParams("USE_FLAG = 1"));    //可用的设备

            foreach (FmsAssetTagSetting setting in _tagSettings)
            {
                foreach (var device in _devices)
                {
                    if (device.ASSET_CODE== setting.ASSET_CODE&& device.USE_FLAG==1)
                    {
                        setting.CUR_VALUE = "";    //将所有Tag点的值清空
                        CurValueChanged.Add(new TagValueChange(setting.PKNO, "", setting.SAMPLING_MODE, setting.STATE_MARK_TYPE));
                    }
                  
                }
             
            }

            //启动设备线程
            foreach (FmsAssetCommParam device in _devices)
            {
                ThreadPool.QueueUserWorkItem(s => { TheadGetDeviceData(device); }); //线程池

                if (device.INTERFACE_TYPE == 1) //Focas通讯时，开启DA线程
                {
                    ThreadPool.QueueUserWorkItem(s => { ThreadGetDaMonitor(device); }); //线程池
                }
            }

            //启动自动保存当前值到数据库
            ThreadPool.QueueUserWorkItem(s => { ThreadSaveData(); }); //线程池

            //启动后台自动计算线程
            bRefreshAutoCal = true;
            ThreadPool.QueueUserWorkItem(s => { ThreadAutoCalculation(); }); //线程池

            //启动校验
            //ProcessCheckMoniter checkMoniter = new ProcessCheckMoniter();
            //checkMoniter.Do();
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            bPause = true;
        }

        /// <summary>
        /// 继续
        /// </summary>
        public void GoOn()
        {
            bPause = false;
        }

        /// <summary>
        /// 读取设备线程
        /// </summary>
        /// <param name="device">设备</param>
        private void TheadGetDeviceData(FmsAssetCommParam device)
        {
            if ((!bMonitor) || (device == null)) return;  //未开启监控

            #region 获取基础参数

            int period = Convert.ToInt32(device.SAMPLING_PERIOD * 10);  //采样周期, 单位s => 100ms
            if (period <= 0) return;    //采样周期<=0时不采样

            string deviceCode = device.ASSET_CODE;
            DeviceCommInterface interfaceType = EnumHelper.ParserEnumByValue(device.INTERFACE_TYPE, DeviceCommInterface.CNC_Fanuc);
            string commAddress = device.COMM_ADDRESS;

            List<FmsAssetTagSetting> tagSettings = _tagSettings
                .Where(c => c.ASSET_CODE == deviceCode && !string.IsNullOrEmpty(c.TAG_ADDRESS)).ToList();  //实际点,虚拟点不采集

            if (tagSettings.Count <= 0) return;

            #endregion

            DeviceManager deviceCommunication = new DeviceManager(device.ASSET_CODE, interfaceType, commAddress, period * 100);

            List<DeviceTagParam> deviceTags = new List<DeviceTagParam>();

            foreach (var tagSetting in tagSettings)
            {
                DeviceTagParam deviceTag = new DeviceTagParam(tagSetting.PKNO, tagSetting.TAG_CODE, 
                    tagSetting.TAG_NAME, tagSetting.TAG_ADDRESS,
                    EnumHelper.ParserEnumByValue(tagSetting.VALUE_TYPE, TagDataType.Default),
                    EnumHelper.ParserEnumByValue(tagSetting.SAMPLING_MODE, DataSimplingMode.AutoReadDevice),
                    deviceCommunication);  //通讯参数

                deviceTags.Add(deviceTag);  //设备采集点
            }

            deviceCommunication.InitialDevice(deviceTags, SaveData);

            int readSpan = period - 1;  //设定采样周期的变量，一开始就采集

            while (!CBaseData.AppClosing)
            {
                #region 暂停

                if (bPause)
                {
                    System.Threading.Thread.Sleep(200);
                    continue;
                }

                #endregion

                try
                {
                    readSpan++;

                    if ((period > 0) && (readSpan % period == 0)) //读取数据
                    {
                        #region 定期刷新数据 - 目前针对OPC订阅方式

                        IDeviceCore deviceCore = deviceCommunication.GetCurDeviceCore();

                        //if (deviceCore is OpcClassicManager)  //如果是OPC读取方式
                        //{
                        //    var ret = ((OpcClassicManager)deviceCore).RefreshData();
                        //    if (!ret.IsSuccess) //错误时
                        //    {
                        //        EventLogger.Log("数据刷新失败，错误为：" + ret.Message);
                        //        Thread.Sleep(10);
                        //        continue;
                        //    }
                        //}

                        #endregion

                        #region 正常读取

                        List<FmsAssetTagSetting> readTags =
                           tagSettings.Where(c => c.SAMPLING_MODE == 0 || c.SAMPLING_MODE == 10).ToList();  //这些需要读取的。

                        foreach (FmsAssetTagSetting curSetting in readTags)
                        {
                            try
                            {
                                if (curSetting.SAMPLING_MODE == 11)
                                {
                                    Thread.Sleep(10);
                                    continue; //暂时不读
                                }

                                Thread.Sleep(100);  //防止读取过快

                                OperateResult read = deviceCommunication.AsyncReadData(curSetting.TAG_ADDRESS); //异步读取数据

                                if (!read.IsSuccess) //错误时
                                {
                                    continue;
                                }

                                //读取成功，读取数据有结果时，一般为同步读取数据时
                                OperateResult<string> result = read as OperateResult<string>;
                                if (result != null)
                                {
                                    SaveData(curSetting.PKNO, result.Content);
                                }
                            }
                            catch (Exception ex)
                            {
                                EventLogger.Log($"读取设备[{device.ASSET_CODE}]的值错误，错误为：", ex);
                            }
                        }

                        #endregion

                        readSpan = 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("读取失败, 错误为：" + ex.Message);
                    System.Threading.Thread.Sleep(100);
                }

                System.Threading.Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 读取DA数据
        /// </summary>
        /// <param name="device"></param>
        private void ThreadGetDaMonitor(FmsAssetCommParam device)
        {
            if ((!bMonitor) || (device == null)) return;  //未开启监控
            int readSpan = 0;  //设定采样周期的变量

            #region 获取基础参数

            string deviceCode = device.ASSET_CODE;
            DeviceCommInterface interfaceType = EnumHelper.ParserEnumByValue(device.INTERFACE_TYPE, DeviceCommInterface.CNC_Fanuc);
            string commAddress = device.COMM_ADDRESS;
            int period = Convert.ToInt32(device.SAMPLING_PERIOD);  //采样周期

            List<FmsAssetTagSetting> tagSettings =
                _tagSettings.Where(c => c.ASSET_CODE == deviceCode && c.SAMPLING_MODE == 3).ToList();  //DA采集模式

            if (tagSettings.Count <= 0) return;

            #endregion

            DeviceManager deviceCommunication = new DeviceManager(CBaseData.NewGuid(),
                interfaceType, commAddress, period * 1000);

            List<DeviceTagParam> deviceTags = new List<DeviceTagParam>();

            foreach (var tagSetting in tagSettings)
            {
                DeviceTagParam deviceTag = new DeviceTagParam(tagSetting.PKNO, tagSetting.TAG_CODE,
                    tagSetting.TAG_NAME, tagSetting.TAG_ADDRESS,
                    EnumHelper.ParserEnumByValue(tagSetting.VALUE_TYPE, TagDataType.Default),
                    EnumHelper.ParserEnumByValue(tagSetting.SAMPLING_MODE, DataSimplingMode.AutoReadDevice),
                    deviceCommunication);  //通讯参数

                deviceTags.Add(deviceTag);  //添加
            }

            deviceCommunication.InitialDevice(deviceTags, SaveData);

            while (!CBaseData.AppClosing)
            {
                #region 暂停

                if (bPause)
                {
                    System.Threading.Thread.Sleep(200);
                    continue;
                }

                #endregion

                try
                {
                    readSpan++;

                    if ((period > 0) && (readSpan%(period*10) == 0)) //读取数据
                    {
                        string error = "";

                        lock (locakDA)
                        {
                            List<FmsAssetTagSetting> daTags =
                                tagSettings.Where(c => c.SAMPLING_MODE == 3).ToList(); //这些为Focas的

                            if (daTags.Any())
                            {
                                #region 采集实时信息

                                DAMachineRealTimeInfo realTimeInfo =
                                    ws_DA.UseService(
                                            s => s.GetDAMachineRealTimeInfos("ASSET_CODE = " + device.ASSET_CODE + ""))
                                        .FirstOrDefault();

                                if (realTimeInfo == null)
                                {
                                    realTimeInfo = new DAMachineRealTimeInfo();
                                }
                                realTimeInfo.DA_TIME = DateTime.Now; //采集时间

                                //实况信息采集和录入
                                realTimeInfo.STATUS = SafeConverter.SafeToInt(deviceCommunication.SyncReadData("状态", out error));

                                if (string.IsNullOrEmpty(error))  //获取状态错误，则
                                {
                                    realTimeInfo.MAIN_PROG = deviceCommunication.SyncReadData("程序号", out error);
                                    realTimeInfo.SPINDLE_OVERRIDE = deviceCommunication.SyncReadData("主轴负载", out error);
                                    if (!string.IsNullOrEmpty(error))
                                    {
                                        realTimeInfo.SPINDLE_OVERRIDE = error;
                                    }
                                    realTimeInfo.SPINDLE_SPPED = deviceCommunication.SyncReadData("主轴转速", out error);
                                    if (!string.IsNullOrEmpty(error))
                                    {
                                        realTimeInfo.SPINDLE_SPPED = error;
                                    }
                                    realTimeInfo.FEED_SPEED = deviceCommunication.SyncReadData("进给速度", out error);
                                    if (!string.IsNullOrEmpty(error))
                                    {
                                        realTimeInfo.FEED_SPEED = error;
                                    }
                                    realTimeInfo.FEED_RATE = deviceCommunication.SyncReadData("进给倍率", out error);
                                    if (!string.IsNullOrEmpty(error))
                                    {
                                        realTimeInfo.FEED_RATE = error;
                                    }

                                    if (string.IsNullOrEmpty(realTimeInfo.ASSET_CODE))
                                    {
                                        realTimeInfo.ASSET_CODE = device.ASSET_CODE;
                                        ws_DA.UseService(s => s.AddDAMachineRealTimeInfo(realTimeInfo)); //新增
                                    }
                                    else
                                    {
                                        ws_DA.UseService(s => s.UpdateDAMachineRealTimeInfo(realTimeInfo)); //实时更新
                                    }
                                }

                                #endregion

                                #region 采集产量信息

                                //设备产量记录采集
                                DAProductRecord productRecord =
                                    ws_DA.UseService(s => s.GetDAProductRecords($"ASSET_CODE = '{device.ASSET_CODE}'"))
                                        .OrderByDescending(c => c.END_TIME)
                                        .FirstOrDefault(); //获取生产记录

                                bool bProductRecord = true;

                                int partNum = SafeConverter.SafeToInt(deviceCommunication.SyncReadData("工件数", out error));
                                if (!string.IsNullOrEmpty(error))
                                {
                                    bProductRecord = false;
                                }
                                int totalNum = SafeConverter.SafeToInt(deviceCommunication.SyncReadData("工件总数", out error));
                                if (!string.IsNullOrEmpty(error))
                                {
                                    bProductRecord = false;
                                }

                                if (bProductRecord)
                                {
                                    if (productRecord == null)
                                    {
                                        productRecord = new DAProductRecord()
                                        {
                                            PKNO = Guid.NewGuid().ToString("N"),
                                            ASSET_CODE = device.ASSET_CODE,
                                            START_TIME = DateTime.Now,
                                            END_TIME = DateTime.Now.AddMilliseconds(100),
                                            PART_NUM = partNum,
                                            TOTAL_PART_NUM = totalNum,
                                        };
                                        ws_DA.UseService(s => s.AddDAProductRecord(productRecord));
                                    }
                                    else
                                    {
                                        if (partNum == productRecord.PART_NUM)
                                        {
                                            if (productRecord.END_TIME <= DateTime.Now.AddMinutes(-5)) //每5分钟，更新数据库
                                            {
                                                productRecord.END_TIME = DateTime.Now;
                                                ws_DA.UseService(s => s.UpdateDAProductRecord(productRecord));
                                            }
                                        }
                                        else //不一致
                                        {
                                            productRecord.END_TIME = DateTime.Now;
                                            ws_DA.UseService(s => s.UpdateDAProductRecord(productRecord));

                                            //插入新的纪录
                                            DAProductRecord newProductRecord = new DAProductRecord()
                                            {
                                                PKNO = Guid.NewGuid().ToString("N"),
                                                ASSET_CODE = deviceCode,
                                                START_TIME = DateTime.Now,
                                                END_TIME = DateTime.Now.AddMilliseconds(100),
                                                PART_NUM = partNum,
                                                TOTAL_PART_NUM = totalNum,
                                            };
                                            ws_DA.UseService(s => s.AddDAProductRecord(newProductRecord)); //新增当前值到数据库
                                        }
                                    }
                                }

                                #endregion

                                #region 采集报警信息

                                string a = deviceCommunication.SyncReadData("报警信息", out error);

                                #endregion
                            }
                        }

                        readSpan = 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ThreadGetDAMonitor, 错误为：" + ex.Message);
                }

                System.Threading.Thread.Sleep(100);
            }
        }

        #region 处理结果

        private void SaveData(string pkno, string sReadResult)
        {
            if (string.IsNullOrEmpty(pkno))
            {
                return;
            }

            try
            {
                FmsAssetTagSetting deviceTagSetting = _tagSettings.FirstOrDefault(c => c.PKNO == pkno);
                if (deviceTagSetting == null)
                {
                    return;
                }
                //if (string.IsNullOrEmpty(sReadResult))
                //{
                //    Console.WriteLine("获取的结果为空！");
                //}
                SaveData(deviceTagSetting, sReadResult);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void SaveData(FmsAssetTagSetting deviceTagSetting, string sReadResult)
        {
            if (deviceTagSetting == null)
            {
                return;
            }

            lock (lockSave)
            {
                try
                {
                    #region 按条件保存到内存中，便于更新数据库

                    if (deviceTagSetting.RECORD_TYPE == 10) //持续结果的记录
                    {
                        FmsStateResultRecord updateRecord = null;
                        FmsStateResultRecord addRecord = NewTagValueResultRecord(deviceTagSetting, sReadResult, out updateRecord);

                        FmsStateResultRecord record =
                            UpdateResultRecords.FirstOrDefault(c => c.PKNO == updateRecord?.PKNO);
                        if (record == null)
                        {
                            if (updateRecord != null) UpdateResultRecords.Add(updateRecord);
                        }
                        else
                        {
                            record.END_TIME = updateRecord.END_TIME;
                        }
                        if (addRecord != null) AddResultRecords.Add(addRecord);
                    }
                    else if ((deviceTagSetting.RECORD_TYPE == 2) ||
                             ((deviceTagSetting.RECORD_TYPE == 1) && (deviceTagSetting.CUR_VALUE != sReadResult))
                    ) //一个周期记录一次或者有改变则记录
                    {
                        FmsSamplingRecord record = NewTagValueRecord(deviceTagSetting, sReadResult);

                        if (record != null) AddSimplingRecords.Add(record);
                    }

                    #endregion

                    #region 将当前值反馈到改变中，便于更新数据库

                    if (deviceTagSetting.CUR_VALUE != sReadResult) //新的值 != 原值
                    {
                        TagValueChange tagValue = CurValueChanged.FirstOrDefault(c => c.PKNO == deviceTagSetting.PKNO);
                        if (tagValue != null)
                        {
                            tagValue.CurValue = sReadResult; //当前值已经更新了
                        }
                        else  //增加新增
                        {
                            CurValueChanged.Add(new TagValueChange(deviceTagSetting.PKNO, sReadResult, deviceTagSetting.SAMPLING_MODE, deviceTagSetting.STATE_MARK_TYPE));
                        }

                        deviceTagSetting.CUR_VALUE = sReadResult; //更新当前值
                    }

                    #endregion

                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        #endregion

        /// <summary>
        /// 自动保存结果，超时清空
        /// </summary>
        private void ThreadSaveData()
        {
            int runSpan = 0; //循环变量

            while (!CBaseData.AppClosing)
            {
                #region 暂停

                if (bPause)
                {
                    System.Threading.Thread.Sleep(200);
                    continue;
                }

                #endregion

                runSpan++;

                if ((SaveCurValueSpan > 0) && (runSpan % (SaveCurValueSpan * 10) == 0)) //到达周期
                {
                    WcfClient<IFMSService> wsThis = new WcfClient<IFMSService>();

                    #region 自动保存当前值到Tag点

                    lock (lockUpdateCurValue)
                    {
                        try
                        {
                            foreach (FmsAssetTagSetting deviceTagSetting in _tagSettings)
                            {
                                TagValueChange tagValue =
                                    CurValueChanged.FirstOrDefault(c => c.PKNO == deviceTagSetting.PKNO);
                                if (tagValue != null)
                                {
                                    if (!tagValue.Changed) continue; //当前值没有更新则退出
                                }
                                else //增加新增
                                {
                                    tagValue = new TagValueChange(deviceTagSetting.PKNO, deviceTagSetting.CUR_VALUE,
                                        deviceTagSetting.SAMPLING_MODE, deviceTagSetting.STATE_MARK_TYPE);
                                    CurValueChanged.Add(tagValue);
                                }

                                FmsAssetTagSetting changeValue = wsThis.UseService(s =>
                                    s.GetFmsAssetTagSettingById(deviceTagSetting.PKNO));
                                if (changeValue == null) continue;

                                changeValue.CUR_VALUE = deviceTagSetting.CUR_VALUE;

                                tagValue.CurValue = deviceTagSetting.CUR_VALUE; //更新值，则改变
                                tagValue.Changed = false;  //没有改变了

                                try
                                {
                                    wsThis.UseService(s => s.UpdateFmsAssetTagSetting(changeValue)); //只更新当前值到数据库
                                }
                                catch (Exception e)
                                {
                                    tagValue.Changed = true;  //
                                }
                            }
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }

                    #endregion
                    
                    #region 保存 标签记录值

                    lock (lockRecord)
                    {
                        List<FmsSamplingRecord> delRecords = new List<FmsSamplingRecord>();
                        try
                        {
                            for (int i = 0; i < AddSimplingRecords.Count; i++)
                            {
                                var record = AddSimplingRecords[i];

                                if (record != null)
                                {
                                    wsThis.UseService(s => s.AddFmsSamplingRecord(record));
                                    delRecords.Add(record);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                        if (delRecords.Count > 0) AddSimplingRecords.RemoveAll(c => delRecords.Contains(c));
                    }

                    #endregion

                    #region 保存 状态记录值

                    lock (lockStateRecord)
                    {
                        List<FmsStateResultRecord> delRecords = new List<FmsStateResultRecord>();
                        try
                        {
                            for (int i = 0; i < UpdateResultRecords.Count; i++)
                            {
                                var record = UpdateResultRecords[i];

                                if (record != null)
                                {
                                    wsThis.UseService(s => s.UpdateFmsStateResultRecord(record));
                                    delRecords.Add(record);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                        if (delRecords.Count > 0) UpdateResultRecords.RemoveAll(c => delRecords.Contains(c));

                        delRecords.Clear();
                        try
                        {
                            for (int i = 0; i < AddResultRecords.Count; i++)
                            {
                                var record = AddResultRecords[i];

                                if (record != null)

                                {
                                    wsThis.UseService(s => s.AddFmsStateResultRecord(record));
                                    delRecords.Add(record);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                        if (delRecords.Count > 0) AddResultRecords.RemoveAll(c => delRecords.Contains(c));
                    }

                    #endregion

                    runSpan = 0;
                }

                #region 超时清空数据

                if (_tagSettings != null)
                {
                    DateTime timeOutTime = DateTime.Now.AddSeconds(-1 * (SaveCurValueSpan + TimeOutClear)); //超时未更新数据的时间

                    List<TagValueChange> clearValues =
                        CurValueChanged.Where(c => !c.Changed && c.LastChangedTime <= timeOutTime
                                                              && (c.SimplingMode != 1)
                                                              && (c.StateMarkType != 100))
                            .ToList(); //超时 未改变 的 非只写点 非系统变量

                    foreach (var clearValue in clearValues)
                    {
                        var tag = _tagSettings.FirstOrDefault(c => c.PKNO == clearValue.PKNO);
                        tag.CUR_VALUE = "";

                        clearValue.CurValue = "";   //当前值已经更新了
                    }
                }

                #endregion

                System.Threading.Thread.Sleep(100);
            }
        }
        
        /// <summary>
        /// 自动计算标签值
        /// 采用动态代码执行方式，编译一次，采用参数传入的方式执行
        /// </summary>
        private void ThreadAutoCalculation()
        {
            Type dynamicCode = null;  //获取编译后代码，调用该类用
            List<FmsTagCalculation> TagCalculations = null;  //计算规则
            Dictionary<string, Dictionary<string, string>> FuncAndParamTagPKNO = new Dictionary<string, Dictionary<string, string>>();  //函数和对应参数的Tag的PKNO

            while (!CBaseData.AppClosing)
            {
                #region 暂停

                if (bPause)
                {
                    System.Threading.Thread.Sleep(500);
                    continue;
                }

                #endregion

                try
                {
                    int index;

                    if (bRefreshAutoCal) //刷新计算规则
                    {
                        WcfClient<IFMSService> wsThis = new WcfClient<IFMSService>();
                        TagCalculations = wsThis.UseService(s => s.GetFmsTagCalculations("USE_FLAG = 1"));

                        bRefreshAutoCal = false;

                        if ((TagCalculations == null) || (!TagCalculations.Any()))
                        {
                            Thread.Sleep(500);
                            continue;
                        }

                        string className = "C" + Guid.NewGuid().ToString("N");

                        #region 形成执行的代码

                        string execCode = "using System; \r\n" +
                                          "using System.Text; \r\n" +
                                          "using System.Collections.Generic; \r\n" +
                                          "using BFM.Common.Base; \r\n\r\n";

                        execCode += "public class " + className + "\r\n" +
                                    "{ \r\n";

                        string basicFuc = "AutoCalculation";

                        index = 1;
                        FuncAndParamTagPKNO.Clear();

                        foreach (FmsTagCalculation calculation in TagCalculations)
                        {
                            FmsAssetTagSetting tagResult = GetTagSettingById(calculation.RESULT_TAG_PKNO);  //结果

                            if (tagResult == null)
                            {
                                continue;
                            }

                            string exp = calculation.CALCULATION_EXPRESSION;  //表达式

                            string funcname = basicFuc + index.ToString();  //函数名称
                            Dictionary<string, string> paramTas = new Dictionary<string, string>();     //参数对应的标签的PKNO, param

                            List<string> funcParam = new List<string>();  //带类型的参数
                            
                            string code = "";
                            string resultType = "string";

                            //将标签替换成参数名
                            foreach (var line in exp.Split(new string[] {"\r\n"}, StringSplitOptions.None))
                            {
                                string ret = line;

                                #region 替换标签值，将标签替换成参数名

                                string[] expTags = line.Split('{');

                                for (int i = 0; i < expTags.Length; i++)
                                {
                                    string str = expTags[i];
                                    int length = str.IndexOf('}');

                                    if (length < 0) //没有找到  }
                                    {
                                        continue;
                                    }

                                    string tagPKNO = str.Substring(0, length); //{ } 内为PKNO

                                    string param = "{" + tagPKNO + "}";

                                    if (paramTas.ContainsKey(tagPKNO))  //已经添加了该参数
                                    {
                                        param = paramTas[tagPKNO];
                                    }
                                    else
                                    {
                                        FmsAssetTagSetting tag = GetTagSettingById(tagPKNO);

                                        if (tag == null) continue;

                                        param = "param" + paramTas.Count;
                                        paramTas.Add(tagPKNO, param);
                                        string paramType = "string";  //所有参数传入都是string型
                                        //string paramType =
                                        //    ((calculation.CALCULATION_TYPE == 2) ||
                                        //     (tag.VALUE_TYPE > 0 && tag.VALUE_TYPE < 20))
                                        //        ? "double"
                                        //        : "string";
                                        funcParam.Add(paramType + " " + param);
                                    }

                                    ret = ret.Replace("{" + tagPKNO + "}", param);
                                }

                                #endregion

                                if (string.IsNullOrEmpty(code))
                                {
                                    code = "    " + ret;
                                }
                                else
                                {
                                    code += Environment.NewLine + "    " + ret;
                                }
                            }

                            //确定返回结果类型，将code语句转换成C#的语句
                            if (calculation.CALCULATION_TYPE == 1) //逻辑运算
                            {
                                //（结果为1,0):({标签1}==1)&&({标签2}==1)&&({标签3}==0||{标签4}==0)&&({标签5}==1)
                                code = code.Replace("AND", "&&").Replace("and", "&&").Replace("OR", "||").Replace("or", "||");
                                resultType = "bool";
                            }
                            else if (calculation.CALCULATION_TYPE == 2) //数值运算
                            {
                                //{标签1}+3+{标签2}+4
                                resultType = "double";
                            }
                            else if (calculation.CALCULATION_TYPE == 3) //字符运算
                            {
                                //{标签1}+"123"
                            }
                            else if (calculation.CALCULATION_TYPE == 12) //条件数值运算
                            {
                                //{标签1}==3:{标签2}+1;{标签1}==4:{标签2}+2;{标签1}==5:{标签2}+3
                                resultType = "double";
                                List<string> exps = code.Split(';').ToList();
                                string temp = "";
                                foreach (var exp1 in exps)
                                {
                                    if (exp1.Split(':').Length < 2)
                                    {
                                        continue;
                                    }
                                    temp += "        if (" + exp1.Split(':')[0] + ") { return (" + exp1.Split(':')[1] + "); } \r\n";
                                }

                                temp += "        return 0; \r\n";

                                code = temp;
                            }
                            else if (calculation.CALCULATION_TYPE == 13) //条件字符运算
                            {
                                //{标签1}==3:{标签1}+"123";{标签1}==4:{标签1}+"123"
                                List<string> exps = code.Split(';').ToList();
                                string temp = "";
                                foreach (var exp1 in exps)
                                {
                                    if (exp1.Split(':').Length < 2)
                                    {
                                        continue;
                                    }
                                    temp += "        if (" + exp1.Split(':')[0] + ") { return (" + exp1.Split(':')[1] + ").ToString(); } \r\n";
                                }

                                temp += "        return \"\"; \r\n";

                                code = temp;
                            }
                            else if (calculation.CALCULATION_TYPE == 21) //标签数组序号
                            {
                                resultType = "string";//{标签1};3
                                List<string> exps = code.Split(';').ToList();
                                string temp = "";
                                if (exps.Count >= 2)
                                {
                                    int arrayIndex = SafeConverter.SafeToInt(exps[1].Trim(), 0);
                                    temp += "            if ( " + exps[0].Trim() + ".Split('|').Length > " + arrayIndex + ") { return " + exps[0].Trim() + ".Split('|')[" + arrayIndex + "]; } \r\n";
                                }

                                temp += "        return \"\"; \r\n";

                                code = temp;
                            }
                            else if (calculation.CALCULATION_TYPE == 100) //C#脚本
                            {
                                //支持C#语法，最后返回值（Double/String)
                                resultType = "string";
                            }
                            else  //不支持的类型
                            {
                                code = $"        return \"计算类型[{calculation.CALCULATION_TYPE}]，不支持的类型。\"; \r\n";
                            }

                            execCode += DynamicCode.BuildExecFunc(funcname, resultType, code, funcParam);  //增加一个函数

                            index++;

                            FuncAndParamTagPKNO.Add(funcname, paramTas); //添加
                        }

                        execCode += "}\r\n";

                        #endregion

                        #region 编译代码

                        CodeDomProvider compiler = new CSharpCodeProvider();
                        CompilerParameters cp = new CompilerParameters() { GenerateExecutable = false, GenerateInMemory = true, };
                        cp.ReferencedAssemblies.Add("BFM.Common.Base.dll");
                        CompilerResults cr = compiler.CompileAssemblyFromSource(cp, execCode);
                        if (cr.Errors.HasErrors)
                        {
                            NetLog.Error("DeviceMonitor.ThreadAutoCalculation Invaild Code: :" + execCode);
                        }

                        dynamicCode = cr.CompiledAssembly.GetType(className);  //获取

                        #endregion

                    }

                    if ((TagCalculations == null) || (!TagCalculations.Any()) || (dynamicCode == null) || (FuncAndParamTagPKNO.Count <= 0))
                    {
                        Thread.Sleep(800);
                        bRefreshAutoCal = true;
                        continue;
                    }
                    
                    #region 复制当前Tag值，保证数据一致性，作为判断条件用

                    List<FmsAssetTagSetting> tags = GetTagSettings("");
                    List<FmsAssetTagSetting> copyTags = new List<FmsAssetTagSetting>();

                    foreach (var tag in tags)
                    {
                        FmsAssetTagSetting copyTag = new FmsAssetTagSetting();
                        copyTag.CopyDataItem(tag);
                        copyTags.Add(copyTag);
                    }

                    #endregion

                    #region  获取值

                    index = 0;
                    foreach (FmsTagCalculation calculation in TagCalculations)
                    {
                        FmsAssetTagSetting tagResult = GetTagSettingById(calculation.RESULT_TAG_PKNO);  //结果

                        if (tagResult == null)
                        {
                            continue;
                        }

                        if (FuncAndParamTagPKNO.Count < index)
                        {
                            break;
                        }

                        string funcName = FuncAndParamTagPKNO.Keys.ToList()[index];
                        var tagParms = FuncAndParamTagPKNO.Values.ToList()[index];
                        List<object> paramValues = new List<object>();  //参数值

                        foreach (var tagpkno in tagParms)  //参数 
                        {
                            object value = null;
                            FmsAssetTagSetting tagParam = copyTags.FirstOrDefault(c => c.PKNO == tagpkno.Key);

                            value = SafeConverter.SafeToStr(tagParam?.CUR_VALUE);

                            paramValues.Add(value);
                        }

                        object obj = dynamicCode.InvokeMember(funcName,
                            BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod,
                            System.Type.DefaultBinder, null, paramValues.ToArray());

                        string newValue = "";  //新的计算结果

                        #region 更新结果

                        if (calculation.CALCULATION_TYPE == 1)  //逻辑运算
                        {
                            newValue = SafeConverter.SafeToBool(obj) ? "1" : "0";
                        }
                        else
                        {
                            newValue = SafeConverter.SafeToStr(obj);
                        }

                        SaveData(tagResult.PKNO, newValue);  //保存更新值

                        #endregion

                        index++;
                    }

                    #endregion
                }
                catch (Exception e)
                {
                    NetLog.Error("DeviceMonitor.ThreadAutoCalculation error:", e);
                    Thread.Sleep(1000);
                    bRefreshAutoCal = true;
                }

                Thread.Sleep(30);
            }
        }

        #region 形成Record信息

        /// <summary>
        /// 记录标签值
        /// </summary>
        /// <param name="tagSetting">标签配置信息</param>
        /// <param name="tagValue">标签值</param>
        private FmsSamplingRecord NewTagValueRecord(FmsAssetTagSetting tagSetting, string tagValue)
        {
            FmsSamplingRecord record = null;

            try
            {
                record = new FmsSamplingRecord
                {
                    PKNO = CBaseData.NewGuid(),
                    ASSET_CODE = tagSetting.ASSET_CODE,
                    TAG_SETTING_PKNO = tagSetting.PKNO,
                    TAG_VALUE_NAME = tagSetting.TAG_VALUE_NAME,
                    TAG_VALUE = tagValue,
                    SAMPLING_TIME = DateTime.Now,
                    CREATION_DATE = DateTime.Now,
                    CREATED_BY = CBaseData.LoginName,
                    REMARK = "",
                };
                //ws.UseService(s => s.AddFmsSamplingRecord(record));
            }
            catch (Exception ex)
            {
                EventLogger.Log($"!!!!!!写入【{tagSetting.TAG_VALUE_NAME}】Record失败，原因:{ex.Message}!!!!!!");
            }

            return record;
        }

        /// <summary>
        /// 状态类标签值保存结果
        /// </summary>
        /// <param name="tagSetting"></param>
        /// <param name="tagValue"></param>
        private FmsStateResultRecord NewTagValueResultRecord(FmsAssetTagSetting tagSetting, string tagValue, out FmsStateResultRecord updateRecord)
        {
            const int addMinutes = -10;  //分钟
            const int spanSaveSec = -5;   //每个5秒更新结束时间
            FmsStateResultRecord record = null;
            updateRecord = null;

            try
            {
                DateTime dtNow = DateTime.Now;

                List<FmsStateResultRecord> records =
                    ws.UseService(
                        s =>
                            s.GetFmsStateResultRecords(
                                $"ASSET_CODE = '{tagSetting.ASSET_CODE}' AND TAG_SETTING_PKNO = '{tagSetting.PKNO}' " +
                                $"AND END_TIME >= '{dtNow.AddMinutes(addMinutes * 2)}' ")) //2倍时间以上时不再继续续写
                        .OrderByDescending(c => c.END_TIME)
                        .ThenByDescending(c => c.BEGINT_TIME)
                        .ThenByDescending(c => c.CREATION_DATE)
                        .ToList();

                FmsStateResultRecord oldRecord = records.FirstOrDefault();
                bool bFirstRecord = !ResultRecordFirsts.ContainsKey(tagSetting.PKNO) ||
                                    ResultRecordFirsts[tagSetting.PKNO];

                bool isAddNewResult = (bFirstRecord) || (oldRecord == null) || (oldRecord.TAG_VALUE != tagValue)|| 
                                      (oldRecord.END_TIME?.Date != dtNow.Date ||       //跨天数据则新增加
                                      (oldRecord.END_TIME <= dtNow.AddMinutes(addMinutes)) );  //10分钟之前的数据则重新增加;


                if ((!bFirstRecord) && (oldRecord != null)) //不是第一次运行则修改之前的时间，一个周期更新一次结束时间+++++
                {
                    if ((isAddNewResult) || (oldRecord.END_TIME <= dtNow.AddSeconds(spanSaveSec)))  //新增记录时，修改原值
                    {
                        oldRecord.END_TIME = dtNow; //更新状态时间
                        updateRecord = oldRecord;
                    }
                }

                if (isAddNewResult) //新增值
                {
                    #region 新增值

                    record = new FmsStateResultRecord
                    {
                        PKNO = CBaseData.NewGuid(),
                        ASSET_CODE = tagSetting.ASSET_CODE,
                        TAG_SETTING_PKNO = tagSetting.PKNO,
                        TAG_VALUE_NAME = tagSetting.TAG_VALUE_NAME,
                        TAG_VALUE = tagValue,
                        BEGINT_TIME = dtNow,
                        END_TIME = DateTime.Now,
                        CREATION_DATE = DateTime.Now,
                        CREATED_BY = CBaseData.LoginName,
                        REMARK = "",
                    };

                    #endregion 
                }

                if (!ResultRecordFirsts.ContainsKey(tagSetting.PKNO))
                {
                    ResultRecordFirsts.Add(tagSetting.PKNO, false); //不是第一次
                }
                else
                {
                    ResultRecordFirsts[tagSetting.PKNO] = false;
                }
            }
            catch (Exception ex)
            {
                EventLogger.Log($"!!!!!!写入【{tagSetting.TAG_VALUE_NAME}】Result失败，原因:{ex.Message}!!!!!!");
            }

            return record;
        }

        #endregion
    }

    class TagValueChange
    {
        public TagValueChange(string pkno, string curValue, int? simplingmode, int? statemarktype)
        {
            PKNO = pkno;
            SimplingMode = simplingmode?? 0;
            StateMarkType = statemarktype ?? 0;

            _curValue = curValue;
            Changed = true;
            LastChangedTime = DateTime.Now;
        }

        /// <summary>
        /// TagSetting.PKNO
        /// </summary>
        public string PKNO { get; set; }

        private string _curValue;
        /// <summary>
        /// 当前值
        /// </summary>
        public string CurValue
        {
            get { return _curValue; }
            set
            {
                if (_curValue != value)
                {
                    Changed = true;
                    _curValue = value;
                }
                LastChangedTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 是否改变
        /// </summary>
        public bool Changed { get; set; } 

        /// <summary>
        /// 数据采集模式
        /// </summary>
        public int SimplingMode { get; set; }

        /// <summary>
        /// 标签标识
        /// </summary>
        public int StateMarkType { get; set; }

        /// <summary>
        /// 上次更新的时间
        /// </summary>
        public DateTime LastChangedTime { get; set; }
    }
}
