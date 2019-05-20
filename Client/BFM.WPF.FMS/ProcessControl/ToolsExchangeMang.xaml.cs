using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Forms;
using BFM.Common.Base.Helper;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.Data.PubData;
using BFM.Server.DataAsset.EAMService;
using BFM.Server.DataAsset.FMSService;
using BFM.Server.DataAsset.PLMService;
using BFM.Server.DataAsset.RSMService;
using BFM.Server.DataAsset.TMSService;
using BFM.Common.DeviceAsset;
using BFM.ContractModel;
using HslCommunication;

namespace BFM.WPF.FMS.ProcessControl
{
    /// <summary>
    /// ToolsExchangeMang.xaml 的交互逻辑
    /// </summary>
    public partial class ToolsExchangeMang : Page
    {
        private WcfClient<IPLMService> wsPLM = new WcfClient<IPLMService>(); //计划
        private WcfClient<IRSMService> wsRSM = new WcfClient<IRSMService>(); //工艺资源
        private WcfClient<IEAMService> wsEAM = new WcfClient<IEAMService>(); //设备
        private WcfClient<ITMSService> wsTMS = new WcfClient<ITMSService>(); //刀具
        private WcfClient<IFMSService> wsFMS = new WcfClient<IFMSService>(); //控制

        private string mainToolsNO = "";

        public ToolsExchangeMang()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            cmbTask.ItemsSource =
                wsPLM.UseService(s => s.GetMesJobOrders($"USE_FLAG = 1 AND RUN_STATE = 2 AND LINE_PKNO = '{CBaseData.CurLinePKNO}'")) //生产准备完成 尚未执行的任务
                    .OrderBy(c => c.CREATION_DATE)
                    .ToList(); //工单

            GetPage();

            GetMainToolsInfo();
        }

        private void GetPage()
        {
            gridChangeTools.ItemsSource = null;

            MesJobOrder jobOrder = cmbTask.SelectedItem as MesJobOrder;
            if (jobOrder == null) return;

            string itemPKNO = cmbProduct.SelectedValue.ToString();
            if (string.IsNullOrEmpty(itemPKNO)) return; //产品

            string processPKNO = cmbProcess.SelectedValue.ToString();
            RsRoutingDetail routingDetail = wsRSM.UseService(s => s.GetRsRoutingDetailById(processPKNO));
            if (routingDetail == null) return;

            AmAssetMasterN wcDevice = cmbDevice.SelectedItem as AmAssetMasterN;
            if (wcDevice == null) return;

            #region 换刀清单

            gridChangeTools.ItemsSource =
                    wsTMS.UseService(s => s.GetTmsDeviceToolsPoss($"USE_FLAG = 1 AND DEVICE_PKNO = '{wcDevice.PKNO}'"))
                        .OrderBy(c => c.TOOLS_POS_NO)
                        .ToList();
            #endregion

        }

        #region 获取查询条件

        private void cmbTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbProduct.SelectedIndex = -1;
            cmbProcess.SelectedIndex = -1;
            cmbDevice.SelectedIndex = -1;

            cmbProduct.ItemsSource = null;
            cmbProcess.ItemsSource = null;
            cmbDevice.ItemsSource = null;

            //选择生产线 => 获取产品
            MesJobOrder jobOrder = cmbTask.SelectedItem as MesJobOrder;
            if (jobOrder == null)
            {
                return;
            }

            cmbProduct.ItemsSource = wsRSM.UseService(s => s.GetRsItemMasters($"USE_FLAG = 1 AND PKNO = '{jobOrder.ITEM_PKNO}'"));

            if (cmbProduct.Items.Count > 0) cmbProduct.SelectedIndex = 0; //选择第一个
        }

        private void cmbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //选择产品 => 获取工艺路线
            cmbProcess.SelectedIndex = -1;
            cmbDevice.SelectedIndex = -1;
            cmbProcess.ItemsSource = null;
            cmbDevice.ItemsSource = null;


            if (cmbProduct.SelectedValue == null) return;
            MesJobOrder jobOrder = cmbTask.SelectedItem as MesJobOrder;
            if (jobOrder == null)
            {
                return;
            }
            string itemPKNO = cmbProduct.SelectedValue.ToString();

            if (string.IsNullOrEmpty(itemPKNO))
            {
                return;
            }

            cmbProcess.ItemsSource =
                wsPLM.UseService(s => s.GetMesProcessCtrols($"USE_FLAG = 1 AND JOB_ORDER_PKNO = '{jobOrder.PKNO}' AND ITEM_PKNO = '{itemPKNO}' "))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .Join(wsRSM.UseService(s => s.GetRsRoutingDetails("USE_FLAG = 1")).OrderBy(c => c.OP_INDEX), c => c.ROUTING_DETAIL_PKNO,
                        d => d.PKNO,
                        (c, d) => new { d.PKNO, d.ROUTING_PKNO, OP_INFO = d.OP_NO + " " + d.OP_NAME });

            //需增加状态表，已经完成准备的工序不再显示
            if (cmbProcess.Items.Count > 0) cmbProcess.SelectedIndex = 0; //选择第一个
        }

        private void cmbProcess_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //选择工序 => 获取设备
            cmbDevice.SelectedIndex = -1;
            cmbDevice.ItemsSource = null;

            if (cmbProcess.SelectedValue == null) return;
            if (cmbProduct.SelectedValue == null) return;
            MesJobOrder jobOrder = cmbTask.SelectedItem as MesJobOrder;
            if (jobOrder == null)
            {
                return;
            }

            string processPKNO = cmbProcess.SelectedValue.ToString();
            MesProcessCtrol processCtrol = wsPLM
                .UseService(s =>
                    s.GetMesProcessCtrols(
                        $"USE_FLAG = 1 AND JOB_ORDER_PKNO = '{jobOrder.PKNO}' AND ROUTING_DETAIL_PKNO = '{processPKNO}'"))
                .OrderBy(c => c.PROCESS_INDEX).FirstOrDefault();

            if (processCtrol == null)
            {
                return;
            }

            cmbDevice.ItemsSource =
                wsEAM.UseService(s => s.GetAmAssetMasterNs($"USE_FLAG = 1 AND ASSET_CODE = '{processCtrol.PROCESS_DEVICE_PKNO}'"));

            if (cmbDevice.Items.Count > 0) cmbDevice.SelectedIndex = 0; //选择第一个

            GetPage();
        }

        #endregion

        private void bToolsChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //换刀 向CNC发送换刀动作
            TmsDeviceToolsPos toolsPos = gridChangeTools.SelectedItem as TmsDeviceToolsPos;
            if (toolsPos == null)
            {
                return;
            }
            //获取主轴刀号
            FmsAssetTagSetting tagSetting =
                wsFMS.UseService(
                    s =>
                        s.GetFmsAssetTagSettings(
                            $"USE_FLAG = 1 AND ASSET_CODE = '{toolsPos.DEVICE_PKNO}' AND TAG_NAME = '机床换刀'"))
                    .FirstOrDefault();

            if (tagSetting == null)
            {
                tbMainToolsNO.Text = "【机床换刀】的Tag地址未设置！";
                return;
            }
            FmsAssetCommParam device =
                wsFMS.UseService(s => s.GetFmsAssetCommParams($"USE_FLAG = 1 AND ASSET_CODE = '{toolsPos.DEVICE_PKNO}'"))
                    .FirstOrDefault();

            if (device == null)
            {
                tbMainToolsNO.Text = "设备未设置通讯信息！";
                return;
            }
            string commAddress = device.COMM_ADDRESS;
            int interfaceType = device.INTERFACE_TYPE;
            int period = Convert.ToInt32(device.SAMPLING_PERIOD);  //采样周期

            OperateResult ret = DeviceHelper.WriteDataByAddress(device.ASSET_CODE, interfaceType, commAddress, 
                tagSetting.PKNO, tagSetting.TAG_ADDRESS, toolsPos.TOOLS_POS_NO);
            
            tbMainToolsNO.Text = (ret.IsSuccess) ? "机床换刀准备 " + toolsPos.TOOLS_POS_NO : "发送换刀指令错误，代码 " + ret.Message;

            if (ret.IsSuccess) mainToolsNO = "";  //开始换刀
        }

        private void bRefresh_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            GetMainToolsInfo();
        }

        /// <summary>
        /// 获取主轴刀号信息
        /// </summary>
        private void GetMainToolsInfo()
        {
            TmsDeviceToolsPos toolsPos = gridChangeTools.SelectedItem as TmsDeviceToolsPos;
            if (toolsPos == null)
            {
                return;
            }
            //获取主轴刀号
            FmsAssetTagSetting tagSetting =
                wsFMS.UseService(
                    s =>
                        s.GetFmsAssetTagSettings(
                            $"USE_FLAG = 1 AND ASSET_CODE = '{toolsPos.DEVICE_PKNO}' AND TAG_NAME = '主轴刀号'"))
                    .FirstOrDefault();

            if (tagSetting == null)
            {
                tbMainToolsNO.Text = "【主轴刀号】的Tag地址未设置！";
                return;
            }
            FmsAssetCommParam device =
                wsFMS.UseService(s => s.GetFmsAssetCommParams($"USE_FLAG = 1 AND ASSET_CODE = '{toolsPos.DEVICE_PKNO}'"))
                    .FirstOrDefault();

            if (device == null)
            {
                tbMainToolsNO.Text = "设备未设置通讯信息！";
                return;
            }
            string commAddress = device.COMM_ADDRESS;
            DeviceCommInterface interfaceType = EnumHelper.ParserEnumByValue(device.INTERFACE_TYPE, DeviceCommInterface.CNC_Fanuc);
            int period = Convert.ToInt32(device.SAMPLING_PERIOD);  //采样周期

            DeviceManager deviceCommunication = new DeviceManager(device.PKNO, interfaceType, commAddress, period * 1000);

            List<DeviceTagParam> deviceTags = new List<DeviceTagParam>();
            DeviceTagParam deviceTag = new DeviceTagParam(tagSetting.PKNO, tagSetting.TAG_CODE,
                tagSetting.TAG_NAME, tagSetting.TAG_ADDRESS,
                EnumHelper.ParserEnumByValue(tagSetting.VALUE_TYPE, TagDataType.Default),
                EnumHelper.ParserEnumByValue(tagSetting.SAMPLING_MODE, DataSimplingMode.AutoReadDevice),
                deviceCommunication);  //通讯参数

            deviceTags.Add(deviceTag);  //添加

            deviceCommunication.InitialDevice(deviceTags, null);

            string error = "";
            string sResult = deviceCommunication.SyncReadData(tagSetting.TAG_ADDRESS, out error);

            if (error != "") mainToolsNO = sResult;

            tbMainToolsNO.Text = (error == "") ? "主轴刀号：T" + sResult : "读取主轴刀号错误，代码 " + error;
        }

        #region 换刀管理

        //卸刀
        private void bChangeDown_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            TmsDeviceToolsPos toolsPos = gridChangeTools.SelectedItem as TmsDeviceToolsPos;
            if (toolsPos == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(mainToolsNO))  //主轴刀号为空
            {
                MessageBox.Show($"未获取到主轴上刀号信息，请先获取主轴刀号信息。", "卸刀",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (mainToolsNO != toolsPos.TOOLS_POS_NO) //选中的刀号信息
            {
                MessageBox.Show($"主轴上刀号为【{mainToolsNO}】，当前选中的需要卸刀为【{toolsPos.TOOLS_POS_NO}】，不能卸下该刀。", "卸刀",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (toolsPos.TOOLS_STATE == 3) //等待卸刀
            {
                if (MessageBox.Show($"刀位[{toolsPos.TOOLS_POS_NO}]已经是等待卸刀状态，卸刀完成后不需要装刀，确定要！卸刀！吗？", "卸刀",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) return;

                toolsPos.TOOLS_STATE = 0;  //空位置
                
            }
            else if (toolsPos.TOOLS_STATE == 10) //等待更换
            {
                if (MessageBox.Show($"刀位[{toolsPos.TOOLS_POS_NO}]已经是换刀状态，卸刀完成还需要进行装刀处理，确定要！卸刀！吗？", "卸刀",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) return;

                toolsPos.TOOLS_STATE = 2;  //更换完成，等待装刀
            }
            else   //其他
            {
                MessageBox.Show($"刀位[{toolsPos.TOOLS_POS_NO}]的状态不正确，不能进行卸刀处理！", "卸刀",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            #region 刀具台账出库非机床

            TmsToolsMaster toolsMaster = wsTMS.UseService(s => s.GetTmsToolsMasterById(toolsPos.TOOLS_PKNO));
            if (toolsMaster != null)
            {
                toolsMaster.TOOLS_POSITION = 10; //出库，非机床
                toolsMaster.TOOLS_POSITION_PKNO = ""; //已出库
                wsTMS.UseService(s => s.UpdateTmsToolsMaster(toolsMaster)); //修改库存
            }

            #endregion

            toolsPos.TOOLS_PKNO = "";  //将旧刀卸下
            //TODO:与CNC通讯，卸刀处理
            //TODO：修改刀补
            wsTMS.UseService(s => s.UpdateTmsDeviceToolsPos(toolsPos));

            
            GetPage();
        }

        //装刀
        private void bChangeUp_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            TmsDeviceToolsPos toolsPos = gridChangeTools.SelectedItem as TmsDeviceToolsPos;
            if (toolsPos == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(mainToolsNO))  //主轴刀号为空
            {
                MessageBox.Show($"未获取到主轴上刀号信息，请先获取主轴刀号信息。", "装刀",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (mainToolsNO != toolsPos.TOOLS_POS_NO) //选中的刀号信息
            {
                MessageBox.Show($"主轴上刀号为【{mainToolsNO}】，当前选中的需要装刀为【{toolsPos.TOOLS_POS_NO}】，不能装上该刀。", "装刀",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (toolsPos.TOOLS_STATE == 2) //等待装刀
            {
                if (MessageBox.Show($"刀位[{toolsPos.TOOLS_POS_NO}]已经是等待装刀状态，确定要！装刀！吗？", "装刀",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) return;

                toolsPos.TOOLS_STATE = 1;  //空位置
                toolsPos.TOOLS_PKNO = toolsPos.NEW_TOOLS_PKNO;  //将新刀装上
                toolsPos.NEW_TOOLS_PKNO = "";  //清除新刀信息
            }
            else   //其他
            {
                MessageBox.Show($"刀位[{toolsPos.TOOLS_POS_NO}]的状态不正确，不能进行装刀处理！", "装刀",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            #region 刀具台账出库到机床

            TmsToolsMaster toolsMaster = wsTMS.UseService(s => s.GetTmsToolsMasterById(toolsPos.TOOLS_PKNO));
            if (toolsMaster != null)
            {
                toolsMaster.TOOLS_POSITION = 2; //出库到机床
                toolsMaster.TOOLS_POSITION_PKNO = toolsPos.DEVICE_PKNO; //机床
                wsTMS.UseService(s => s.UpdateTmsToolsMaster(toolsMaster)); //修改库存
            }

            #endregion

            //TODO:与CNC通讯，装刀处理
            //TODO：修改刀补
            wsTMS.UseService(s => s.UpdateTmsDeviceToolsPos(toolsPos));  //完成换刀

            GetPage();
        }

        #endregion

    }
}
