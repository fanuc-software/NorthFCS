using BFM.Common.Data.PubData;
using BFM.Common.DeviceAsset;
using BFM.ContractModel;
using BFM.Server.DataAsset.DAService;
using BFM.Server.DataAsset.FMSService;
using BFM.Server.DataAsset.PLMService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Server.DataAsset.EAMService;
using BFM.Server.DataAsset.RSMService;
using HslCommunication;

namespace BFM.WPF.FMS
{
    /// <summary>
    /// 生产过程校验
    /// </summary>
    public class ProcessCheckMoniter
    {
        private WcfClient<IFMSService> ws = new WcfClient<IFMSService>();
        private WcfClient<IPLMService> wsPlm = new WcfClient<IPLMService>();
        private WcfClient<IRSMService> wsRSM = new WcfClient<IRSMService>();
        private WcfClient<IEAMService> wsEAM = new WcfClient<IEAMService>();

        public void Do()
        {
            Thread thSaveValue = new Thread(TheadCheckData);
            thSaveValue.Start();
        }

        /// <summary>
        /// 读取设备线程
        /// </summary>
        private void TheadCheckData()
        {
            while (!CBaseData.AppClosing)
            {
                Thread.Sleep(200); //暂停
                try
                {
                    for (int i = 1; i <= 5; i++)
                    {
                        string basicAddress = "[fuzhou]MES.JG.SB_" + i.ToString(); //基本地址
                        string qingqiuAddress = ".Qing_qiu"; //请求
                        string gongyiIDAddress = ".Gongyi_ID"; //工艺ID
                        string chanpinIDAddress = ".chanpin_ID"; //产品ID
                        string querenAddress = ".que_ren"; //确认地址
                        querenAddress.Substring(0, 1);
                        FmsAssetTagSetting tag_qingqiu =
                            DeviceMonitor.GetTagSettings($"TAG_ADDRESS = '{basicAddress}{qingqiuAddress}'")
                                .FirstOrDefault();

                        if ((tag_qingqiu == null) || (tag_qingqiu.CUR_VALUE != "1")) continue;

                        #region 触发请求

                        FmsAssetTagSetting tag_gongyi =
                            DeviceMonitor.GetTagSettings($"TAG_ADDRESS = '{basicAddress}{gongyiIDAddress}'")
                                .FirstOrDefault(); //工艺ID
                        if (tag_gongyi == null) continue;

                        FmsAssetTagSetting tag_chanpin =
                            DeviceMonitor.GetTagSettings($"TAG_ADDRESS = '{basicAddress}{chanpinIDAddress}'")
                                .FirstOrDefault(); //产品ID
                        if (tag_chanpin == null) continue;

                        FmsAssetTagSetting tag_queren =
                            DeviceMonitor.GetTagSettings($"TAG_ADDRESS = '{basicAddress}{querenAddress}'")
                                .FirstOrDefault(); //确认
                        if (tag_queren == null) continue;

                        EmCheckResult checkResult = EmCheckResult.NoCheck; //无校验

                        #region 1.校验程序号

                        AmAssetMasterN checkDevice =
                            wsEAM.UseService(s => s.GetAmAssetMasterNs($"USE_FLAG = 1 AND ASSET_TYPE = '机床' AND ASSET_LABEL = '{i.ToString()}'"))
                                .FirstOrDefault();

                        if (checkDevice == null)
                        {
                            Console.WriteLine("...error:没有相应的机床设备.");
                            continue;
                        }

                        FmsAssetTagSetting tag_focas =
                            DeviceMonitor.GetTagSettings(
                                $"ASSET_CODE = '{checkDevice.ASSET_CODE}' AND TAG_ADDRESS = '程序号'")
                                .FirstOrDefault(); //获取程序号

                        if ((tag_focas != null) &&
                            (tag_focas.CUR_VALUE == tag_gongyi.CUR_VALUE)) //程序号相同
                        {
                            checkResult = EmCheckResult.Success;
                        }
                        else
                        {
                            checkResult = EmCheckResult.ProgramError;
                        }

                        #endregion

                        #region 2.校验产品

                        MesJobOrder curJob =
                            wsPlm.UseService(s => s.GetMesJobOrders($"RUN_STATE = 3 AND LINE_PKNO = '{CBaseData.CurLinePKNO}'")).FirstOrDefault(); //开工确认完成的

                        if (curJob != null)
                        {
                            RsItemMaster itemMaster =
                                wsRSM.UseService(s => s.GetRsItemMasterById(curJob.ITEM_PKNO));

                            if ((itemMaster != null) && (itemMaster.ITEM_ABV == tag_chanpin.CUR_VALUE))
                            {
                                if (checkResult == EmCheckResult.Success)  //程序校验成功
                                {
                                    checkResult = EmCheckResult.Success;
                                }
                            }
                            else
                            {
                                checkResult = EmCheckResult.ProductError;
                            }
                        }
                        else
                        {
                            checkResult = EmCheckResult.ProductError;
                        }

                        #endregion

                        #region 向PLC反馈

                        FmsAssetCommParam deviceComm =
                            ws.UseService(
                                s => s.GetFmsAssetCommParams(
                                    $"ASSET_CODE = '{tag_qingqiu.ASSET_CODE}' AND USE_FLAG = 1")).FirstOrDefault();  //反馈设备

                        string error = "";
                        OperateResult ret = DeviceHelper.WriteDataByAddress(deviceComm.ASSET_CODE,
                            deviceComm.INTERFACE_TYPE, deviceComm.COMM_ADDRESS,
                            tag_queren.PKNO, tag_queren.TAG_ADDRESS,
                            ((int) checkResult).ToString()); //反馈

                        #endregion

                        if (!ret.IsSuccess)
                        {
                            Console.WriteLine("...error:向设备" + tag_queren.ASSET_CODE + "反馈确认结果失败。");
                        }

                        #endregion

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("...error：生产过程校验错误，错误为：" + ex.Message);
                }
            }

        }

    }

    /// <summary>
    /// 校验结果
    /// </summary>
    enum EmCheckResult
    {
        /// <summary>
        /// 无校验
        /// </summary>
        NoCheck = 0,
        /// <summary>
        /// 校验通过
        /// </summary>
        Success = 1,
        /// <summary>
        /// 程序号错误
        /// </summary>
        ProgramError = 2,
        /// <summary>
        /// 产品错误
        /// </summary>
        ProductError = 3,
    }
}
