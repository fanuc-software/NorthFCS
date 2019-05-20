using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BFM.Common.Base;
using BFM.Common.Base.Helper;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.Data.PubData;
using BFM.Common.DeviceAsset;
using BFM.ContractModel;
using BFM.Server.DataAsset.EAMService;
using BFM.Server.DataAsset.FMSService;
using BFM.WPF.Base;
using BFM.WPF.Base.Helper;
using BFM.WPF.Base.Notification;
using BFM.WPF.FMS.BasicData.VirtualTag;
using DevExpress.Xpf.Grid;
using Microsoft.CSharp;

namespace BFM.WPF.FMS.BasicData
{
    /// <summary>
    /// AssetTagSetting_New.xaml 的交互逻辑
    /// </summary>
    public partial class AssetTagSetting_New : Page
    {
        private WcfClient<IFMSService> ws = new WcfClient<IFMSService>();
        private WcfClient<IEAMService> wsWAM = new WcfClient<IEAMService>();

        public AssetTagSetting_New()
        {
            InitializeComponent();

            GetBasicData();

            GetMainData();
        }

        //提取基础数据
        private void GetBasicData()
        {
            cmbInterfaceType.ItemsSource = EnumHelper.GetEnumsToList<DeviceCommInterface>().DefaultView; //通讯接口

            List<AmAssetMasterN> assets =
                wsWAM.UseService(s => s.GetAmAssetMasterNs($"USE_FLAG = {(int) EmUseFlag.Useful}"))
                    .OrderBy(c => c.ASSET_CODE).ToList(); //设备信息
            cmbAssetInfo.ItemsSource = assets;

            var valueTypes = EnumHelper.GetEnumsToList<TagDataType>().DefaultView; //标签值类型
            cmbValueType.ItemsSource = valueTypes;
            cmbValueType2.ItemsSource = valueTypes;
        }

        private void GetMainData()
        {
            List<ComboxHelper> AllTags = new List<ComboxHelper>();
            List<FmsAssetTagSetting> DeviceTags = ws.UseService(s => s.GetFmsAssetTagSettings($"USE_FLAG = 1"));

            #region 树形结构

            List<FmsAssetCommParam> commDevices = ws.UseService(s => s.GetFmsAssetCommParams("USE_FLAG = 1"))
                .OrderBy(c => c.ASSET_CODE).ToList();

            tvMain.View.Nodes.Clear();

            TreeListNode root = new TreeListNode
            {
                Content = new TreeHelper() {PKNO = "", Name = $"通讯设备 ({commDevices.Count})", Type = -1, Value = ""},
            };

            foreach (var device in commDevices)
            {
                string name = wsWAM
                    .UseService(s => s.GetAmAssetMasterNs($"ASSET_CODE = '{device.ASSET_CODE}' AND USE_FLAG = 1"))
                    .FirstOrDefault()
                    ?.ASSET_NAME;

                var deviceTags = DeviceTags.Where(c => c.ASSET_CODE == device.ASSET_CODE).OrderBy(c => c.TAG_NAME)
                    .ToList();

                TreeListNode viewItem = new TreeListNode
                {
                    Content = new TreeHelper()
                        {PKNO = device.ASSET_CODE, Name = $"{name} ({deviceTags.Count})", Type = 0, Value = device},
                    IsExpanded = false,
                };

                var realTags = deviceTags.Where(c => !string.IsNullOrEmpty(c.TAG_ADDRESS)).OrderBy(c => c.TAG_NAME)
                    .ToList();
                var virtualTags = deviceTags.Where(c => string.IsNullOrEmpty(c.TAG_ADDRESS)).OrderBy(c => c.TAG_NAME)
                    .ToList();

                TreeListNode realTagItem = new TreeListNode
                {
                    Content = new TreeHelper()
                        {PKNO = device.ASSET_CODE, Name = $"实际标签 ({realTags.Count})", Type = 1, Value = device},
                    IsExpanded = false,
                };

                TreeListNode virtualTagItem = new TreeListNode
                {
                    Content = new TreeHelper()
                        {PKNO = device.ASSET_CODE, Name = $"虚拟标签 ({virtualTags.Count})", Type = 2, Value = device},
                    IsExpanded = false,
                };

                foreach (FmsAssetTagSetting tag in realTags)
                {
                    TreeListNode item = new TreeListNode
                    {
                        Content = new TreeHelper() {PKNO = tag.PKNO, Name = tag.TAG_NAME, Type = 11, Value = tag},
                    };
                    realTagItem.Nodes.Add(item);

                    AllTags.Add(new ComboxHelper() {Value = tag.PKNO, Name = name + "." + tag.TAG_NAME + "(实际点)"});
                }

                foreach (FmsAssetTagSetting tag in virtualTags)
                {
                    TreeListNode item = new TreeListNode
                    {
                        Content = new TreeHelper() {PKNO = tag.PKNO, Name = tag.TAG_NAME, Type = 12, Value = tag},
                    };
                    virtualTagItem.Nodes.Add(item);

                    AllTags.Add(new ComboxHelper() {Value = tag.PKNO, Name = name + "." + tag.TAG_NAME + "(虚拟点)"});
                }

                viewItem.Nodes.Add(realTagItem);
                viewItem.Nodes.Add(virtualTagItem);

                viewItem.CollapseAll();
                root.IsExpanded = true;

                root.Nodes.Add(viewItem);
            }

            tvMain.View.Nodes.Add(root);

            #endregion

            this.VTagCalcu.DeviceTags = DeviceTags;
            this.VTagCalcu.AllShowTags = AllTags;
        }

        //显示信息
        private void tvMain_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            TreeHelper tree = tvMain.SelectedItem as TreeHelper;
            if (tree == null)
            {
                BtnAdd.IsEnabled = true;
                return;
            }

            string pkno = tree.PKNO;

            BtnAdd.IsEnabled = true;
            BtnMod.IsEnabled = true;
            BtnDel.IsEnabled = true;
            BtnSave.IsEnabled = false;
            BtnCancel.IsEnabled = false;

            #region  基础属性

            gbDevices.Visibility = Visibility.Collapsed;

            gbDevice.Visibility = Visibility.Collapsed;
            gbRealTag.Visibility = Visibility.Collapsed;
            gbVirtualTag.Visibility = Visibility.Collapsed;
            gbCalculation.Visibility = Visibility.Collapsed;

            gbTagInfo.Visibility = Visibility.Collapsed;
            gbVitualTagInfo.Visibility = Visibility.Collapsed;

            gbItem.IsCollapsed = true;

            gbDevice.Header = $"设备通讯信息";
            gbRealTag.Header = $"实体标签点信息";
            gbVirtualTag.Header = $"虚拟标签点信息";

            #endregion

            if (tree.Type == -1) //设备组
            {
                gbDevices.Visibility = Visibility.Visible;
                gridDevices.ItemsSource = ws.UseService(s => s.GetFmsAssetCommParams("USE_FLAG = 1"))
                    .OrderBy(c => c.ASSET_CODE).ToList();

                BindHelper.SetDictDataBindingGridItem(gbDevice, gridDevices); //设定绑定
            }
            else if (tree.Type == 0) //设备
            {
                BtnAdd.IsEnabled = false;
                gbDevice.Visibility = Visibility.Visible;
                gbDevice.DataContext = tree.Value; //设备
            }
            else if (tree.Type == 1) //实体标签 名
            {
                gbTagInfo.Visibility = Visibility.Visible;
                gridTagItems.ItemsSource =
                    ws.UseService(s => s.GetFmsAssetTagSettings($"ASSET_CODE = '{pkno}' AND USE_FLAG = 1"))
                        .Where(c => !string.IsNullOrEmpty(c.TAG_ADDRESS)).OrderBy(c => c.TAG_NAME).ToList();

                BindHelper.SetDictDataBindingGridItem(gbRealTag, gridTagItems); //设定绑定
            }
            else if (tree.Type == 2) //虚拟标签 名
            {
                gbVitualTagInfo.Visibility = Visibility.Visible;
                gridVitualTagItems.ItemsSource =
                    ws.UseService(s => s.GetFmsAssetTagSettings($"ASSET_CODE = '{pkno}' AND USE_FLAG = 1"))
                        .Where(c => string.IsNullOrEmpty(c.TAG_ADDRESS)).OrderBy(c => c.TAG_NAME).ToList();

                BindHelper.SetDictDataBindingGridItem(gbVirtualTag, gridVitualTagItems); //设定绑定
            }
            else if (tree.Type == 11) //实际标签 Tag
            {
                BtnAdd.IsEnabled = false;
                gbRealTag.Visibility = Visibility.Visible;
                gbRealTag.DataContext = tree.Value;

                FmsAssetTagSetting tagItem = tree.Value as FmsAssetTagSetting;
                if (tagItem == null)
                {
                    return;
                }

                string assetCode = tagItem?.ASSET_CODE;

                FmsAssetCommParam asset = ws.UseService(s => s.GetFmsAssetCommParams($"ASSET_CODE = '{assetCode}'"))
                    .FirstOrDefault();

                #region 方便编辑标签地址

                gFocas.Visibility = Visibility.Collapsed;
                gModbus.Visibility = Visibility.Collapsed;
                gZeiss.Visibility = Visibility.Collapsed;
                gModula.Visibility = Visibility.Collapsed;
                gFanucRobot.Visibility = Visibility.Collapsed;

                gAddress.Visibility = Visibility.Collapsed;

                byte function;
                int modAddress;
                int length;
                int station;
                switch (EnumHelper.ParserEnumByValue(asset?.INTERFACE_TYPE, DeviceCommInterface.TCP_Custom))
                {
                    case DeviceCommInterface.CNC_Fanuc:

                        #region 显示Fanuc CNC 的标准地址位

                        tbFocasAddress.Text = "";
                        cmbFocasFunc.SelectedValue = tagItem.TAG_ADDRESS;
                        if (cmbFocasFunc.SelectedIndex < 8)
                        {
                            cmbFocasFunc.SelectedValue = tagItem.TAG_ADDRESS.Substring(0, 1);
                            if (cmbFocasFunc.SelectedIndex >= 0) //地址位
                            {
                                tbFocasAddress.Text = tagItem.TAG_ADDRESS.Substring(1);
                            }
                            else
                            {
                                tbFocasAddress.Text = tagItem.TAG_ADDRESS;
                            }
                        }

                        #endregion

                        gFocas.Visibility = Visibility.Visible;
                        break;
                    case DeviceCommInterface.ZeissTCP:  //蔡司 三坐标
                        cmbZeiss.SelectedValue = tagItem.TAG_ADDRESS;
                        if (cmbZeiss.SelectedIndex < 0)
                        {
                            cmbZeiss.Text = tagItem.TAG_ADDRESS;
                        }

                        gZeiss.Visibility = Visibility.Visible;
                        break;
                    case DeviceCommInterface.ModulaTCP:  //Modula 自动货柜
                        cmbModula.SelectedValue = tagItem.TAG_ADDRESS;
                        if (cmbModula.SelectedIndex < 0)
                        {
                            cmbModula.Text = tagItem.TAG_ADDRESS;
                        }

                        gModula.Visibility = Visibility.Visible;
                        break;
                    case DeviceCommInterface.FanucRobot:  //Fanuc 机器人
                        station = 1;
                        function = 1;
                        modAddress = 0;
                        length = 1;

                        ModbusTCPManager.GetModbusInfo(tagItem.TAG_ADDRESS, ref station, ref function, ref modAddress,
                            ref length); //获取Modbus信息
                        cmbFanucRobot.SelectedValue = function.ToString();
                        tbFanucRobotAdd.Text = modAddress.ToString();
                        tbFanucRobotLen.Text = length.ToString();

                        gFanucRobot.Visibility = Visibility.Visible;
                        break;
                    case DeviceCommInterface.TCP_Modbus:
                        station = 1;
                        function = 1;
                        modAddress = 0;
                        length = 1;

                        ModbusTCPManager.GetModbusInfo(tagItem.TAG_ADDRESS, ref station, ref function, ref modAddress,
                            ref length); //获取Modbus信息
                        tbModbusStation.Text = station.ToString();
                        cmbModbusFunc.SelectedValue = function.ToString();
                        tbModbusAdd.Text = modAddress.ToString();
                        tbModbusLen.Text = length.ToString();

                        gModbus.Visibility = Visibility.Visible;
                        break;
                    default:
                        gAddress.Visibility = Visibility.Visible;
                        break;
                }

                #endregion
            }
            else if (tree.Type == 12) //虚拟标签 Tag
            {
                BtnAdd.IsEnabled = false;
                gbVirtualTag.Visibility = Visibility.Visible;
                gbCalculation.Visibility = Visibility.Visible;
                gbVirtualTag.DataContext = tree.Value;

                FmsAssetTagSetting assetTag = gbVirtualTag.DataContext as FmsAssetTagSetting;
                if (assetTag != null)
                {
                    FmsTagCalculation vCal = ws.UseService(s =>
                            s.GetFmsTagCalculations(
                                $"ASSET_CODE = '{assetTag.ASSET_CODE}' AND RESULT_TAG_PKNO = '{assetTag.PKNO}' AND USE_FLAG = 1"))
                        .FirstOrDefault();

                    InitialCalculation(vCal?.CALCULATION_TYPE, vCal?.CALCULATION_EXPRESSION);
                    gbCalculationInfo.DataContext = vCal;
                }

                VTagCalcu.IsReadOnly = true;
                gbCalculation.Visibility = Visibility.Visible; //虚拟点
            }
        }

        #region 功能菜单

        //添加
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            TreeHelper tree = tvMain.SelectedItem as TreeHelper;

            if (tree == null || tree.Type == -1) //设备组 => 增加设备
            {
                FmsAssetCommParam assetComm = new FmsAssetCommParam
                {
                    PKNO = "",
                    ASSET_CODE = "",
                    COMM_ADDRESS = "",
                    USE_FLAG = 1
                };

                gbDevice.DataContext = assetComm;
                gbDevice.Header = $"设备通讯信息 【新增】";
                gbDevice.Visibility = Visibility.Visible;
            }
            else if (tree.Type == 1) //实体点
            {
                FmsAssetCommParam asset = tree.Value as FmsAssetCommParam;
                if (asset == null) return;

                FmsAssetTagSetting assetTag = new FmsAssetTagSetting
                {
                    PKNO = "",
                    ASSET_CODE = asset.ASSET_CODE,
                    STATE_MARK_TYPE = 0,
                    SAMPLING_MODE = 0,
                    RECORD_TYPE = 0, //默认不记录
                    USE_FLAG = 1
                };

                gbRealTag.DataContext = assetTag;
                gbRealTag.Header = $"实体标签点信息  【新增】";
                gbRealTag.Visibility = Visibility.Visible;

                #region 方便编辑标签地址

                gFocas.Visibility = Visibility.Collapsed;
                gModbus.Visibility = Visibility.Collapsed;
                gZeiss.Visibility = Visibility.Collapsed;
                gModula.Visibility = Visibility.Collapsed;
                gFanucRobot.Visibility = Visibility.Collapsed;

                gAddress.Visibility = Visibility.Collapsed;

                switch (EnumHelper.ParserEnumByValue(asset?.INTERFACE_TYPE, DeviceCommInterface.TCP_Custom))
                {
                    case DeviceCommInterface.CNC_Fanuc:
                        cmbFocasFunc.SelectedIndex = 0;
                        tbFocasAddress.Text = "";

                        gFocas.Visibility = Visibility.Visible;
                        break;
                    case DeviceCommInterface.ZeissTCP:  //蔡司 三坐标
                        cmbZeiss.SelectedIndex = 0;

                        gZeiss.Visibility = Visibility.Visible;
                        break;
                    case DeviceCommInterface.ModulaTCP:  //Modula 自动货柜
                        cmbModula.SelectedIndex = 0;

                        gModula.Visibility = Visibility.Visible;
                        break;
                    case DeviceCommInterface.FanucRobot:  //Fanuc 机器人
                        cmbFanucRobot.SelectedIndex = 0;  //默认读取所有UO
                        tbFanucRobotAdd.Text = "";
                        tbFanucRobotLen.Text = "1";

                        gFanucRobot.Visibility = Visibility.Visible;
                        break;
                    case DeviceCommInterface.TCP_Modbus:
                        tbModbusStation.Text = "1";
                        cmbModbusFunc.SelectedIndex = 0;
                        tbModbusAdd.Text = "";
                        tbModbusLen.Text = "1";

                        gModbus.Visibility = Visibility.Visible;
                        break;
                    default:
                        gAddress.Visibility = Visibility.Visible;
                        break;
                }

                #endregion
            }
            else if (tree.Type == 2) //虚拟点
            {
                FmsAssetCommParam asset = tree.Value as FmsAssetCommParam;
                if (asset == null) return;

                FmsAssetTagSetting assetTag = new FmsAssetTagSetting
                {
                    PKNO = "",
                    ASSET_CODE = asset.ASSET_CODE,
                    STATE_MARK_TYPE = 0,
                    SAMPLING_MODE = 0,
                    RECORD_TYPE = 0, //默认不记录
                    USE_FLAG = 1
                };

                FmsTagCalculation tagCalculation = new FmsTagCalculation
                {
                    PKNO = "",
                    ASSET_CODE = asset.ASSET_CODE,
                    RESULT_TAG_PKNO = "",
                    CALCULATION_TYPE = 1,
                    USE_FLAG = 1
                };
                gbCalculationInfo.DataContext = tagCalculation;

                gbVirtualTag.DataContext = assetTag;
                gbVirtualTag.Header = $"虚拟标签点信息  【新增】";
                gbVirtualTag.Visibility = Visibility.Visible;

                VTagCalcu.IsReadOnly = false;
                gbCalculation.Visibility = Visibility.Visible;
            }

            gbItem.IsCollapsed = false;

            BtnAdd.IsEnabled = false;
            BtnMod.IsEnabled = false;
            BtnDel.IsEnabled = false;
            BtnSave.IsEnabled = true;
            BtnCancel.IsEnabled = true;

        }

        //修改
        private void BtnMod_Click(object sender, RoutedEventArgs e)
        {
            ModItem(); //修改
        }

        //修改
        private void ModItem()
        {
            TreeHelper tree = tvMain.SelectedItem as TreeHelper;
            if (tree == null)
            {
                return;
            }
            else if (tree.Type == -1) //设备组
            {
                FmsAssetCommParam assetComm = gridDevices.SelectedItem as FmsAssetCommParam;
                if (assetComm == null)
                {
                    return;
                }

                gbDevice.DataContext = assetComm;
                gbDevice.Header = $"设备通讯信息 【编辑】";
                gbDevice.Visibility = Visibility.Visible;

                gbItem.IsCollapsed = false;
            }
            else if (tree.Type == 0) //设备
            {
                gbDevice.Header = $"设备通讯信息 【编辑】";
                gbDevice.Visibility = Visibility.Visible;

                gbItem.IsCollapsed = false;
            }
            else if (tree.Type == 1) //实体标签 组
            {
                FmsAssetTagSetting tagItem = gridTagItems.SelectedItem as FmsAssetTagSetting;
                if (tagItem == null)
                {
                    return;
                }

                gbRealTag.DataContext = tagItem;
                gbRealTag.Header = $"实体标签点信息  【编辑】";
                gbRealTag.Visibility = Visibility.Visible;

                gbItem.IsCollapsed = false;

                FmsAssetCommParam asset = tree.Value as FmsAssetCommParam;

                #region 方便编辑标签地址

                gFocas.Visibility = Visibility.Collapsed;
                gModbus.Visibility = Visibility.Collapsed;
                gZeiss.Visibility = Visibility.Collapsed;
                gModula.Visibility = Visibility.Collapsed;
                gFanucRobot.Visibility = Visibility.Collapsed;

                gAddress.Visibility = Visibility.Collapsed;

                int station;
                byte function;
                int modAddress;
                int length;
                switch (EnumHelper.ParserEnumByValue(asset?.INTERFACE_TYPE, DeviceCommInterface.TCP_Custom))
                {
                    case DeviceCommInterface.CNC_Fanuc:

                        #region 显示Fanuc CNC 的标准地址位

                        tbFocasAddress.Text = "";
                        cmbFocasFunc.SelectedValue = tagItem.TAG_ADDRESS;
                        if (cmbFocasFunc.SelectedIndex < 8)
                        {
                            cmbFocasFunc.SelectedValue = tagItem.TAG_ADDRESS.Substring(0, 1);
                            if (cmbFocasFunc.SelectedIndex >= 0) //地址位
                            {
                                tbFocasAddress.Text = tagItem.TAG_ADDRESS.Substring(1);
                            }
                            else
                            {
                                tbFocasAddress.Text = tagItem.TAG_ADDRESS;
                            }
                        }

                        #endregion

                        gFocas.Visibility = Visibility.Visible;
                        break;
                    case DeviceCommInterface.ZeissTCP:  //蔡司 三坐标
                        cmbZeiss.SelectedValue = tagItem.TAG_ADDRESS;
                        if (cmbZeiss.SelectedIndex < 0)
                        {
                            cmbZeiss.Text = tagItem.TAG_ADDRESS;
                        }

                        gZeiss.Visibility = Visibility.Visible;
                        break;
                    case DeviceCommInterface.ModulaTCP:  //Modula 自动货柜
                        cmbModula.SelectedValue = tagItem.TAG_ADDRESS;
                        if (cmbModula.SelectedIndex < 0)
                        {
                            cmbModula.Text = tagItem.TAG_ADDRESS;
                        }

                        gModula.Visibility = Visibility.Visible;
                        break;
                    case DeviceCommInterface.FanucRobot:  //Fanuc 机器人
                        station = 1;
                        function = 1;
                        modAddress = 0;
                        length = 1;

                        ModbusTCPManager.GetModbusInfo(tagItem.TAG_ADDRESS, ref station, ref function, ref modAddress,
                            ref length); //获取Modbus信息
                        cmbFanucRobot.SelectedValue = function.ToString();
                        tbFanucRobotAdd.Text = modAddress.ToString();
                        tbFanucRobotLen.Text = length.ToString();

                        gFanucRobot.Visibility = Visibility.Visible;
                        break;
                    case DeviceCommInterface.TCP_Modbus:
                        station = 1;
                        function = 1;
                        modAddress = 0;
                        length = 1;

                        ModbusTCPManager.GetModbusInfo(tagItem.TAG_ADDRESS, ref station, ref function, ref modAddress,
                            ref length); //获取Modbus信息
                        tbModbusStation.Text = station.ToString();
                        cmbModbusFunc.SelectedValue = function.ToString();
                        tbModbusAdd.Text = modAddress.ToString();
                        tbModbusLen.Text = length.ToString();

                        gModbus.Visibility = Visibility.Visible;
                        break;
                    default:
                        gAddress.Visibility = Visibility.Visible;
                        break;
                }

                #endregion
            }
            else if (tree.Type == 2) //虚拟标签 组
            {
                FmsAssetTagSetting tagItem = gridVitualTagItems.SelectedItem as FmsAssetTagSetting;
                if (tagItem == null)
                {
                    return;
                }

                FmsTagCalculation tagCalculation = ws.UseService(s =>
                        s.GetFmsTagCalculations(
                            $"ASSET_CODE = '{tagItem.ASSET_CODE}' AND RESULT_TAG_PKNO = '{tagItem.PKNO}' AND USE_FLAG = 1"))
                    .FirstOrDefault();

                if (tagCalculation == null)
                {
                    tagCalculation = new FmsTagCalculation
                    {
                        PKNO = "",
                        ASSET_CODE = tagItem.ASSET_CODE,
                        RESULT_TAG_PKNO = tagItem.PKNO,
                        CALCULATION_TYPE = 1,
                        USE_FLAG = 1
                    };
                }

                gbCalculationInfo.DataContext = tagCalculation;
                int calculationType = SafeConverter.SafeToInt(tagCalculation.CALCULATION_TYPE, 1);
                InitialCalculation(calculationType, tagCalculation.CALCULATION_EXPRESSION);

                gbVirtualTag.DataContext = tagItem;
                gbVirtualTag.Header = $"虚拟标签点信息  【编辑】";
                gbVirtualTag.Visibility = Visibility.Visible;

                gbItem.IsCollapsed = false;
                VTagCalcu.IsReadOnly = false;

                gbCalculation.Visibility = Visibility.Visible;
            }
            else if (tree.Type == 11) //实际点
            {
                gbRealTag.Header = $"实体标签点信息  【编辑】";
                gbRealTag.Visibility = Visibility.Visible;

                gbItem.IsCollapsed = false;

                FmsAssetTagSetting tagItem = tree.Value as FmsAssetTagSetting;
                if (tagItem == null)
                {
                    return;
                }

                string assetCode = tagItem?.ASSET_CODE;

                FmsAssetCommParam asset = ws.UseService(s => s.GetFmsAssetCommParams($"ASSET_CODE = '{assetCode}'"))
                    .FirstOrDefault();

                #region 方便编辑标签地址

                gFocas.Visibility = Visibility.Collapsed;
                gModbus.Visibility = Visibility.Collapsed;
                gZeiss.Visibility = Visibility.Collapsed;
                gModula.Visibility = Visibility.Collapsed;
                gFanucRobot.Visibility = Visibility.Collapsed;

                gAddress.Visibility = Visibility.Collapsed;

                int station;
                byte function;
                int modAddress;
                int length;
                switch (EnumHelper.ParserEnumByValue(asset?.INTERFACE_TYPE, DeviceCommInterface.TCP_Custom))
                {
                    case DeviceCommInterface.CNC_Fanuc:

                        #region 显示Fanuc CNC 的标准地址位

                        tbFocasAddress.Text = "";
                        cmbFocasFunc.SelectedValue = tagItem.TAG_ADDRESS;
                        if (cmbFocasFunc.SelectedIndex < 8)
                        {
                            cmbFocasFunc.SelectedValue = tagItem.TAG_ADDRESS.Substring(0, 1);
                            if (cmbFocasFunc.SelectedIndex >= 0) //地址位
                            {
                                tbFocasAddress.Text = tagItem.TAG_ADDRESS.Substring(1);
                            }
                            else
                            {
                                tbFocasAddress.Text = tagItem.TAG_ADDRESS;
                            }
                        }

                        #endregion

                        gFocas.Visibility = Visibility.Visible;
                        break;
                    case DeviceCommInterface.ZeissTCP:  //蔡司 三坐标
                        cmbZeiss.SelectedValue = tagItem.TAG_ADDRESS;
                        if (cmbZeiss.SelectedIndex < 0)
                        {
                            cmbZeiss.Text = tagItem.TAG_ADDRESS;
                        }

                        gZeiss.Visibility = Visibility.Visible;
                        break;
                    case DeviceCommInterface.ModulaTCP:  //Modula 自动货柜
                        cmbModula.SelectedValue = tagItem.TAG_ADDRESS;
                        if (cmbModula.SelectedIndex < 0)
                        {
                            cmbModula.Text = tagItem.TAG_ADDRESS;
                        }

                        gModula.Visibility = Visibility.Visible;
                        break;
                    case DeviceCommInterface.FanucRobot:  //Fanuc 机器人
                        station = 1;
                        function = 1;
                        modAddress = 0;
                        length = 1;

                        ModbusTCPManager.GetModbusInfo(tagItem.TAG_ADDRESS, ref station, ref function, ref modAddress,
                            ref length); //获取Modbus信息
                        cmbFanucRobot.SelectedValue = function.ToString();
                        tbFanucRobotAdd.Text = modAddress.ToString();
                        tbFanucRobotLen.Text = length.ToString();

                        gFanucRobot.Visibility = Visibility.Visible;
                        break;

                    case DeviceCommInterface.TCP_Modbus:
                        station = 1;
                        function = 1;
                        modAddress = 0;
                        length = 1;

                        ModbusTCPManager.GetModbusInfo(tagItem.TAG_ADDRESS, ref station, ref function, ref modAddress,
                            ref length); //获取Modbus信息
                        tbModbusStation.Text = station.ToString();
                        cmbModbusFunc.SelectedValue = function.ToString();
                        tbModbusAdd.Text = modAddress.ToString();
                        tbModbusLen.Text = length.ToString();

                        gModbus.Visibility = Visibility.Visible;
                        break;
                    default:
                        gAddress.Visibility = Visibility.Visible;
                        break;
                }

                #endregion
            }
            else if (tree.Type == 12) //虚拟点
            {
                FmsAssetTagSetting tagItem = gbVirtualTag.DataContext as FmsAssetTagSetting;
                if (tagItem == null) return;
                gbVirtualTag.Header = $"虚拟标签点信息  【编辑】";
                gbVirtualTag.Visibility = Visibility.Visible;

                FmsTagCalculation tagCalculation = gbCalculationInfo.DataContext as FmsTagCalculation;
                if (tagCalculation == null)
                {
                    tagCalculation = new FmsTagCalculation
                    {
                        PKNO = "",
                        ASSET_CODE = tagItem.ASSET_CODE,
                        RESULT_TAG_PKNO = tagItem.PKNO,
                        CALCULATION_TYPE = 1,
                        USE_FLAG = 1
                    };

                    InitialCalculation(1, "");
                }

                gbCalculationInfo.DataContext = tagCalculation;

                gbItem.IsCollapsed = false;
                VTagCalcu.IsReadOnly = false;
            }

            BtnAdd.IsEnabled = false;
            BtnMod.IsEnabled = false;
            BtnDel.IsEnabled = false;
            BtnSave.IsEnabled = true;
            BtnCancel.IsEnabled = true;

        }

        //删除
        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            TreeHelper tree = tvMain.SelectedItem as TreeHelper;
            if (tree == null)
            {
                return;
            }

            if ((tree.Type == -1) || (tree.Type == 0)) //设备组
            {
                #region 删除设备

                FmsAssetCommParam assetComm = null;
                if (tree.Type == -1)
                {
                    assetComm = gridDevices.SelectedItem as FmsAssetCommParam;
                }
                else
                {
                    assetComm = gbDevice.DataContext as FmsAssetCommParam;
                }

                if ((assetComm == null) || (string.IsNullOrEmpty(assetComm.PKNO)))
                {
                    return;
                }

                if (WPFMessageBox.ShowConfirm($"确定要删除 该设备的通讯信息吗？删除后通讯点的信息也将不会采集", "删除") != WPFMessageBoxResult.OK)
                {
                    return;
                }

                assetComm.USE_FLAG = (int) EmUseFlag.Deleted; //已删除
                assetComm.UPDATED_BY = CBaseData.LoginName;
                assetComm.LAST_UPDATE_DATE = DateTime.Now;
                assetComm.UPDATED_INTROD += "删除 ";
                ws.UseService(s => s.UpdateFmsAssetCommParam(assetComm));

                NotificationInvoke.NewNotification("删除提示", "设备通讯配置信息已删除！");

                #endregion
            }
            else if ((tree.Type == 1) || (tree.Type == 11)) //实际点
            {
                #region 删除实际点 

                FmsAssetTagSetting tagItem = null;

                if (tree.Type == 1)
                {
                    tagItem = gridTagItems.SelectedItem as FmsAssetTagSetting;
                }
                else
                {
                    tagItem = gbRealTag.DataContext as FmsAssetTagSetting;
                }

                if ((tagItem == null) || (string.IsNullOrEmpty(tagItem.PKNO)))
                {
                    return;
                }

                if (WPFMessageBox.ShowConfirm($"确定要删除 该虚拟标签信息吗？", "删除") != WPFMessageBoxResult.OK)
                {
                    return;
                }

                tagItem.USE_FLAG = (int) EmUseFlag.Deleted; //已删除
                tagItem.UPDATED_BY = CBaseData.LoginName;
                tagItem.LAST_UPDATE_DATE = DateTime.Now;
                tagItem.UPDATED_INTROD += "删除 ";
                ws.UseService(s => s.UpdateFmsAssetTagSetting(tagItem));

                NotificationInvoke.NewNotification("删除提示", "设备通讯标签配置已删除！");


                #endregion

            }
            else if ((tree.Type == 2) || (tree.Type == 12)) //虚拟点
            {
                #region 删除虚拟点

                FmsAssetTagSetting tagItem = null;

                if (tree.Type == 1)
                {
                    tagItem = gridTagItems.SelectedItem as FmsAssetTagSetting;
                }
                else
                {
                    tagItem = gbVirtualTag.DataContext as FmsAssetTagSetting;
                }

                if ((tagItem == null) || (string.IsNullOrEmpty(tagItem.PKNO)))
                {
                    return;
                }

                if (WPFMessageBox.ShowConfirm($"确定要删除 该实际标签信息吗？", "删除") != WPFMessageBoxResult.OK)
                {
                    return;
                }

                tagItem.USE_FLAG = (int) EmUseFlag.Deleted; //已删除
                tagItem.UPDATED_BY = CBaseData.LoginName;
                tagItem.LAST_UPDATE_DATE = DateTime.Now;
                tagItem.UPDATED_INTROD += "删除 ";
                ws.UseService(s => s.UpdateFmsAssetTagSetting(tagItem));

                FmsTagCalculation tagCalculation = gbCalculationInfo.DataContext as FmsTagCalculation;
                if (tagCalculation != null)
                {
                    tagCalculation.USE_FLAG = (int) EmUseFlag.Deleted; //已删除
                    tagCalculation.UPDATED_BY = CBaseData.LoginName;
                    tagCalculation.LAST_UPDATE_DATE = DateTime.Now;
                    tagCalculation.UPDATED_INTROD += "删除 ";
                    ws.UseService(s => s.UpdateFmsTagCalculation(tagCalculation));
                }

                NotificationInvoke.NewNotification("删除提示", "设备通讯标签配置已删除！");


                #endregion
            }

            GetMainData();

            tvMain_SelectedItemChanged(null, null);
        }

        //保存
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            bool bRefreshAll = false;

            TreeHelper tree = tvMain.SelectedItem as TreeHelper;

            if ((tree == null) || (tree.Type == -1) || (tree.Type == 0))
            {
                #region 保存通讯设备

                AmAssetMasterN asset1 = cmbAssetInfo.SelectedItem as AmAssetMasterN;
                FmsAssetCommParam assetComm = gbDevice.DataContext as FmsAssetCommParam;

                if (assetComm == null)
                {
                    return;
                }

                //TODO: 校验；保存
                if (asset1 == null)
                {
                    WPFMessageBox.ShowConfirm("请选择设备！", "保存");
                    return;
                }

                if (assetComm.PKNO == "") //新增
                {
                    bRefreshAll = true;
                    assetComm.PKNO = CBaseData.NewGuid();
                    assetComm.CREATED_BY = CBaseData.LoginName;
                    assetComm.CREATION_DATE = DateTime.Now;
                    assetComm.LAST_UPDATE_DATE = DateTime.Now; //最后修改日期
                    ws.UseService(s => s.AddFmsAssetCommParam(assetComm));

                    GetMainData(); //刷新所有
                }
                else //修改
                {
                    assetComm.UPDATED_BY = CBaseData.LoginName;
                    assetComm.LAST_UPDATE_DATE = DateTime.Now;
                    ws.UseService(s => s.UpdateFmsAssetCommParam(assetComm));
                }

                NotificationInvoke.NewNotification("保存", "设备通讯配置信息已保存。");

                //保存成功
                gbItem.IsCollapsed = true;

                #endregion
            }
            else if (tree.Type == 1 || tree.Type == 11)
            {
                #region 保存实体标签

                FmsAssetTagSetting assetTag = gbRealTag.DataContext as FmsAssetTagSetting;

                if (assetTag == null)
                {
                    return;
                }

                #region 校验

                if (string.IsNullOrEmpty(assetTag.TAG_NAME))
                {
                    WPFMessageBox.ShowWarring($"请输入标签名称！", "保存");
                    return;
                }

                if (string.IsNullOrEmpty(assetTag.TAG_ADDRESS))
                {
                    WPFMessageBox.ShowWarring($"请输入正确的标签地址！", "保存");
                    return;
                }

                if (!string.IsNullOrEmpty(assetTag.TAG_CODE)) //标签编码
                {
                    List<FmsAssetTagSetting> existTags = ws.UseService(s =>
                        s.GetFmsAssetTagSettings(
                            $"USE_FLAG = 1 AND TAG_CODE = '{assetTag.TAG_CODE}'"));
                    if (!string.IsNullOrEmpty(assetTag.PKNO)) //修改
                    {
                        existTags = existTags.Where(c => c.PKNO != assetTag.PKNO).ToList();
                    }

                    if (existTags.Any())
                    {
                        WPFMessageBox.ShowWarring(
                            $"该标签编码【{assetTag.TAG_CODE}】已存在，不能" + (string.IsNullOrEmpty(assetTag.PKNO) ? "添加" : "修改") +
                            "为这个编码！",
                            "保存");
                        return;
                    }
                }

                #endregion

                #region 保存

                if (string.IsNullOrEmpty(assetTag.PKNO)) //新增
                {
                    bRefreshAll = true;
                    assetTag.PKNO = CBaseData.NewGuid();
                    assetTag.CREATED_BY = CBaseData.LoginName;
                    assetTag.CREATION_DATE = DateTime.Now;
                    assetTag.LAST_UPDATE_DATE = DateTime.Now; //最后修改日期
                    ws.UseService(s => s.AddFmsAssetTagSetting(assetTag));

                    GetMainData();
                }
                else //修改
                {
                    assetTag.UPDATED_BY = CBaseData.LoginName;
                    assetTag.LAST_UPDATE_DATE = DateTime.Now;
                    ws.UseService(s => s.UpdateFmsAssetTagSetting(assetTag));
                }

                NotificationInvoke.NewNotification("保存", "设备实体标签配置信息已保存。");

                #endregion

                #endregion
            }
            else if (tree.Type == 2 || tree.Type == 12)
            {
                #region 保存虚拟标签

                FmsAssetTagSetting assetTag = gbVirtualTag.DataContext as FmsAssetTagSetting;

                FmsTagCalculation calculation = gbCalculationInfo.DataContext as FmsTagCalculation;

                if ((assetTag == null) || (calculation == null))
                {
                    return;
                }

                #region 校验

                if (string.IsNullOrEmpty(assetTag.TAG_NAME))
                {
                    WPFMessageBox.ShowWarring($"请输入标签名称！", "保存");
                    return;
                }

                if (!string.IsNullOrEmpty(assetTag.TAG_CODE)) //标签编码
                {
                    List<FmsAssetTagSetting> existTags = ws.UseService(s =>
                        s.GetFmsAssetTagSettings(
                            $"USE_FLAG = 1 AND TAG_CODE = '{assetTag.TAG_CODE}'"));
                    if (!string.IsNullOrEmpty(assetTag.PKNO)) //修改
                    {
                        existTags = existTags.Where(c => c.PKNO != assetTag.PKNO).ToList();
                    }

                    if (existTags.Any())
                    {
                        WPFMessageBox.ShowWarring(
                            $"该标签编码【{assetTag.TAG_CODE}】已存在，不能" + (string.IsNullOrEmpty(assetTag.PKNO) ? "添加" : "修改") +
                            "为这个编码！",
                            "保存");
                        return;
                    }
                }

                if (calculation.CALCULATION_TYPE == null)
                {
                    WPFMessageBox.ShowWarring($"请选择虚拟标签的计算类型！", "保存");
                    return;
                }
                
                calculation.CALCULATION_EXPRESSION = VTagCalcu.RefreshCalculationText();
                if (string.IsNullOrEmpty(calculation.CALCULATION_EXPRESSION))
                {
                    WPFMessageBox.ShowWarring($"请维护虚拟标签的计算表达式！", "保存");
                    return;
                }

                #endregion

                #region 保存

                if (string.IsNullOrEmpty(assetTag.PKNO)) //新增虚拟点
                {
                    assetTag.PKNO = CBaseData.NewGuid();
                    assetTag.CREATED_BY = CBaseData.LoginName;
                    assetTag.CREATION_DATE = DateTime.Now;
                    assetTag.LAST_UPDATE_DATE = DateTime.Now; //最后修改日期
                    ws.UseService(s => s.AddFmsAssetTagSetting(assetTag));

                    GetMainData();
                }
                else //修改
                {
                    assetTag.UPDATED_BY = CBaseData.LoginName;
                    assetTag.LAST_UPDATE_DATE = DateTime.Now;
                    ws.UseService(s => s.UpdateFmsAssetTagSetting(assetTag));
                }

                calculation.RESULT_TAG_PKNO = assetTag.PKNO;

                if (string.IsNullOrEmpty(calculation.PKNO)) //新增规则
                {
                    calculation.PKNO = CBaseData.NewGuid();
                    calculation.CREATED_BY = CBaseData.LoginName;
                    calculation.CREATION_DATE = DateTime.Now;
                    calculation.LAST_UPDATE_DATE = DateTime.Now; //最后修改日期
                    ws.UseService(s => s.AddFmsTagCalculation(calculation));
                }
                else //修改
                {
                    calculation.UPDATED_BY = CBaseData.LoginName;
                    calculation.LAST_UPDATE_DATE = DateTime.Now;
                    ws.UseService(s => s.UpdateFmsTagCalculation(calculation));
                }

                NotificationInvoke.NewNotification("保存", "设备虚拟标签配置信息已保存。");

                #endregion

                DeviceMonitor.RefreshAutoCalSetting(); //设置重新刷新一次计算

                #endregion
            }

            tvMain_SelectedItemChanged(null, null);
        }

        //取消
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            tvMain_SelectedItemChanged(null, null); //取消
        }

        //刷新
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            tvMain_SelectedItemChanged(null, null); //取消
            Thread.Sleep(300);
            Cursor = Cursors.Arrow;
        }

        #endregion

        private void gridItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //修改
            if ((sender as GridControl)?.VisibleRowCount <= 0)
            {
                return;
            }

            ModItem();
        }

        //运算类型
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FmsTagCalculation calculation = cmbCalculationType.DataContext as FmsTagCalculation;
            if (calculation == null)
            {
                return;
            }

            int calculationType = SafeConverter.SafeToInt(calculation.CALCULATION_TYPE, 1);

            if (calculationType == 1) //逻辑运算
            {
                gbCalculation.Header = "逻辑运算";
            }
            else if (calculationType == 2) //数值运算
            {
                gbCalculation.Header = "数值运算";
            }
            else if (calculationType == 3) //字符运算
            {
                gbCalculation.Header = "字符运算";
            }
            else if (calculationType == 12) //条件数值运算
            {
                gbCalculation.Header = "条件数值运算";
            }
            else if (calculationType == 13) //条件字符运算
            {
                gbCalculation.Header = "条件字符运算";
            }
            else if (calculationType == 100) //C#脚本
            {
                gbCalculation.Header = "C#脚本";
            }

            InitialCalculation(calculationType, calculation.CALCULATION_EXPRESSION);
        }

        //初始化计算公式
        private void InitialCalculation(int? calculationType, string calculationText)
        {
            if (VTagCalcu == null) return;

            #region 清空

            VTagCalcu.LogicText = "";
            VTagCalcu.NormalText = "";
            VTagCalcu.ConditionCals.Clear();
            VTagCalcu.ConditionCals.Add(new ConditionCal());
            VTagCalcu.CSharpText = "";

            #endregion

            VTagCalcu.CalculationType = calculationType ?? 1;

            if (string.IsNullOrEmpty(calculationText))
            {
                return;
            }

            string newcalculationText = calculationText;

            foreach (var item in VTagCalcu.AllShowTags)
            {
                newcalculationText = newcalculationText.Replace("{" + item.Value + "}", "{" + item.Name + "}");
            }

            if (calculationType == 1) //逻辑运算
            {
                VTagCalcu.LogicText = newcalculationText;
            }
            else if ((calculationType == 2) || (calculationType == 3)) //数值运算 字符运算
            {
                VTagCalcu.NormalText = newcalculationText;
            }
            else if ((calculationType == 12) || (calculationType == 13)) //条件数值运算 条件字符运算
            {
                VTagCalcu.ConditionCals.Clear();
                List<string> conditions = newcalculationText.Split(';').ToList();

                foreach (var condition in conditions)
                {
                    if (condition.Split(':').Length < 2) continue;
                    VTagCalcu.ConditionCals.Add(new ConditionCal()
                        {Condition = condition.Split(':')[0], Result = condition.Split(':')[1]});
                }

                if (VTagCalcu.ConditionCals.Count == 0)
                {
                    VTagCalcu.ConditionCals.Add(new ConditionCal());
                }
            }
            else if (calculationType == 21)  //数组计算
            {
                List<string> exps = calculationText.Split(';').ToList();
                VTagCalcu.ArrayTagPKNO = "";
                VTagCalcu.ArrayIndex = "";
                if (exps.Count >= 2)
                {
                    int arrayIndex = SafeConverter.SafeToInt(exps[1].Trim(), 0);
                    VTagCalcu.ArrayTagPKNO = (exps[0].Trim().Length > 2 ? exps[0].Trim().Substring(1, exps[0].Trim().Length - 2) : "");
                    VTagCalcu.ArrayIndex = arrayIndex.ToString();
                }
            }
            else if (calculationType == 100) //C#脚本
            {
                VTagCalcu.CSharpText = newcalculationText;
            }
        }

        #region 编辑Fanuc CNC 地址

        private void TbFocasAddress_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            FocasAddressChange();
        }

        private void CmbFocasFunc_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FocasAddressChange();
        }

        private void FocasAddressChange()
        {
            if (gFocas.Visibility != Visibility.Visible) return;

            FmsAssetTagSetting tagItem = gbRealTag.DataContext as FmsAssetTagSetting;
            if (tagItem == null) return;

            tagItem.TAG_ADDRESS = cmbFocasFunc.SelectedValue + tbFocasAddress.Text;
        }

        #endregion

        #region 编辑 Modbus 地址

        private void TbModbusStation_TextChanged(object sender, TextChangedEventArgs e)
        {
            ModbusAddressChange();
        }

        private void CmbModbusFunc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ModbusAddressChange();
        }

        private void TbModbusAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            ModbusAddressChange();
        }

        private void TbModbusLength_TextChanged(object sender, TextChangedEventArgs e)
        {
            ModbusAddressChange();
        }

        private void ModbusAddressChange()
        {
            if (gModbus.Visibility != Visibility.Visible) return;

            FmsAssetTagSetting tagItem = gbRealTag.DataContext as FmsAssetTagSetting;
            if (tagItem == null) return;

            if (string.IsNullOrEmpty(tbModbusAdd.Text))
            {
                tagItem.TAG_ADDRESS = "";
                return;
            }

            tagItem.TAG_ADDRESS = "s=" + (string.IsNullOrEmpty(tbModbusStation.Text) ? "1" : tbModbusStation.Text) + ";" +
                                  "f=" + cmbModbusFunc.SelectedValue + ";" +
                                  "d=" + tbModbusAdd.Text + ";" +
                                  "l=" + (string.IsNullOrEmpty(tbModbusLen.Text) ? "1" : tbModbusLen.Text);
        }

        #endregion

        #region 编辑Fanuc 机器人

        private void cmbFanucRobot_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FanucRobotAddressChange();
        }

        private void TbFanucRobotAdd_TextChanged(object sender, TextChangedEventArgs e)
        {
            FanucRobotAddressChange();
        }

        private void TbFanucRobotLen_TextChanged(object sender, TextChangedEventArgs e)
        {
            FanucRobotAddressChange();
        }

        private void FanucRobotAddressChange()
        {
            if (gFanucRobot.Visibility != Visibility.Visible) return;

            FmsAssetTagSetting tagItem = gbRealTag.DataContext as FmsAssetTagSetting;
            if (tagItem == null) return;

            if (string.IsNullOrEmpty(tbFanucRobotAdd.Text))
            {
                tagItem.TAG_ADDRESS = "";
                return;
            }

            tagItem.TAG_ADDRESS = "f=" + cmbFanucRobot.SelectedValue + ";" +
                                  "d=" + tbFanucRobotAdd.Text + ";" +
                                  "l=" + (string.IsNullOrEmpty(tbFanucRobotLen.Text) ? "1" : tbFanucRobotLen.Text);
        }

        #endregion

        #region 编辑 蔡司三坐标

        private void CmbZeiss_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ZeissAddressChange();
        }

        private void ZeissAddressChange()
        {
            if (gZeiss.Visibility != Visibility.Visible) return;

            FmsAssetTagSetting tagItem = gbRealTag.DataContext as FmsAssetTagSetting;
            if (tagItem == null) return;

            if (string.IsNullOrEmpty(cmbZeiss.Text))
            {
                tagItem.TAG_ADDRESS = "";
                return;
            }

            tagItem.TAG_ADDRESS = cmbZeiss.SelectedValue?.ToString() ?? cmbZeiss.Text;
        }

        #endregion

        #region 编辑 Modula 自动货柜

        private void CmbModula_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ModulaAddressChange();
        }

        private void ModulaAddressChange()
        {
            if (gModula.Visibility != Visibility.Visible) return;

            FmsAssetTagSetting tagItem = gbRealTag.DataContext as FmsAssetTagSetting;
            if (tagItem == null) return;

            if (string.IsNullOrEmpty(cmbModula.Text))
            {
                tagItem.TAG_ADDRESS = "";
                return;
            }

            tagItem.TAG_ADDRESS = cmbModula.SelectedValue?.ToString() ?? cmbModula.Text;
        }
        #endregion

        //通讯协议
        private void CmbInterfaceType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string message = "";

            if (cmbInterfaceType.SelectedValue != null)
            {
                DeviceCommInterface deviceiInterface =
                    EnumHelper.ParserEnumByValue(SafeConverter.SafeToInt(cmbInterfaceType.SelectedValue),
                        DeviceCommInterface.CNC_Fanuc);

                switch (deviceiInterface)
                {
                    case DeviceCommInterface.CNC_Fanuc: //Fanuc CNC
                        message = "通讯地址格式：IP地址；需要特殊端口号时采用 | 分隔";
                        break;
                    case DeviceCommInterface.FanucRobot: //Fanuc机器人
                        message = "通讯地址格式：IP地址；需要特殊端口号时采用 | 分隔";
                        break;
                    case DeviceCommInterface.ModulaTCP: //Modula 自动货柜
                        message = "通讯地址格式：IP地址 | 端口号；";
                        break;
                    case DeviceCommInterface.ZeissTCP: //蔡司 三坐标
                        message = "通讯地址格式：设备IP地址 | 设备端口号 | 本地IP地址 | 本地端口号 ";
                        break;
                    case DeviceCommInterface.OPC_Classic: //OPC Server
                        message = "通讯地址格式：OPCServer名称（RSLinx OPC Server / OPC.SimaticNET）";
                        break;
                    case DeviceCommInterface.TCP_Custom: //自定义TCP协议 
                        message = "通讯地址格式：IP地址 | 端口号 | 自定义协议 | 自定义协议参数 ";
                        break;
                    case DeviceCommInterface.TCP_Modbus: //Modbus TCP
                        message = "通讯地址格式：IP地址；需要特殊端口号时采用 | 分隔";
                        break;
                    case DeviceCommInterface.TCP_Server: //TCP 服务器
                        message = "通讯地址格式：本地IP地址 | 端口号 ";
                        break;
                    case DeviceCommInterface.WebApi: //WebApi
                        message = "通讯地址格式：WebApi地址 ";
                        break;
                    case DeviceCommInterface.DataBase: //数据库通讯
                        message = "通讯地址格式：数据库类型（sqlserver、mysql、oracle、access）| 数据库连接语句 ";
                        break;
                }
            }

            imgMessage.ToolTip = message;
        }

    }
}
