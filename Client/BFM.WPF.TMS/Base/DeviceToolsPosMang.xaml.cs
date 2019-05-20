using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Server.DataAsset.EAMService;
using BFM.Server.DataAsset.TMSService;
using BFM.ContractModel;
using BFM.WPF.Base;
using BFM.WPF.FMS;

namespace BFM.WPF.TMS
{
    /// <summary>
    /// DeviceToolsPosMang.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceToolsPosMang : Page
    {
        private WcfClient<ITMSService> ws = new WcfClient<ITMSService>(); 
        private WcfClient<IEAMService> wsEAM = new WcfClient<IEAMService>(); 
        public DeviceToolsPosMang()
        {
            InitializeComponent();

            cmbDeviceInfo.ItemsSource = wsEAM.UseService(s => s.GetAmAssetMasterNs("USE_FLAG = 1 AND ASSET_TYPE = '机床'"));

            GetPage();
        }

        private void GetPage()
        {
            AmAssetMasterN asset = cmbDeviceInfo.SelectedItem as AmAssetMasterN;
            if (asset == null)
            {
                gridItem.ItemsSource = null;
            }
            else
            {
                gridItem.ItemsSource =
                    ws.UseService(s => s.GetTmsDeviceToolsPoss($"USE_FLAG = 1 AND DEVICE_PKNO = '{asset.PKNO}'"))
                        .OrderBy(c => c.TOOLS_POS_NO)
                        .ToList();
            }
        }

        private void bSearch4_Click(object sender, RoutedEventArgs e)
        {
            GetPage();
        }

        #region 按钮动作

        #region 编辑

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AmAssetMasterN asset = cmbDeviceInfo.SelectedItem as AmAssetMasterN;
            if (asset == null) return;
            gbItem.IsCollapsed = false;
            //新增
            dictInfo.Header = "设备刀位信息  【新增】";
            dictInfo.Visibility = Visibility.Visible;

            TmsDeviceToolsPos m_TmsDeviceToolsPos = new TmsDeviceToolsPos()
            {
                DEVICE_PKNO = asset.PKNO,
                TOOLS_PKNO = "",
                TOOLS_STATE = 0,  //空
                USE_FLAG = 1,
            };
            dictInfo.DataContext = m_TmsDeviceToolsPos;
        }
        
        private void BtnMod_Click(object sender, RoutedEventArgs e)
        {
            AmAssetMasterN asset = cmbDeviceInfo.SelectedItem as AmAssetMasterN;
            if (asset == null) return;
            TmsDeviceToolsPos m_TmsDeviceToolsPos = gridItem.SelectedItem as TmsDeviceToolsPos;
            if (m_TmsDeviceToolsPos == null)
            {
                return;
            }
            //修改
            gbItem.IsCollapsed = false;
            dictInfo.Header = "设备刀位信息  【修改】";
            dictInfo.Visibility = Visibility.Visible;

            dictInfo.DataContext = m_TmsDeviceToolsPos;
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            AmAssetMasterN asset = cmbDeviceInfo.SelectedItem as AmAssetMasterN;
            if (asset == null) return;
            //删除
            TmsDeviceToolsPos m_TmsDeviceToolsPos = gridItem.SelectedItem as TmsDeviceToolsPos;
            if (m_TmsDeviceToolsPos == null)
            {
                return;
            }
            if (WPFMessageBox.ShowConfirm($"确定删除设备刀位【{m_TmsDeviceToolsPos.TOOLS_POS_NO}】的信息吗？", @"删除信息") == WPFMessageBoxResult.OK)
            {
                m_TmsDeviceToolsPos.USE_FLAG = -1;
                ws.UseService(s => s.UpdateTmsDeviceToolsPos(m_TmsDeviceToolsPos));

                //删除成功.
                GetPage();  //重新加载
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            AmAssetMasterN asset = cmbDeviceInfo.SelectedItem as AmAssetMasterN;
            if (asset == null) return;
            //保存
            TmsDeviceToolsPos m_TmsDeviceToolsPos = dictInfo.DataContext as TmsDeviceToolsPos;
            if (m_TmsDeviceToolsPos == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(m_TmsDeviceToolsPos.TOOLS_POS_NO))
            {
                WPFMessageBox.ShowWarring("请输入刀号！", "保存");
                return;
            }

            if (string.IsNullOrEmpty(m_TmsDeviceToolsPos.PKNO)) //新增
            {
                TmsDeviceToolsPos check =
                    ws.UseService(s => s.GetTmsDeviceToolsPoss($"TOOLS_POS_NO = '{m_TmsDeviceToolsPos.TOOLS_POS_NO}' AND DEVICE_PKNO = '{asset.PKNO}' AND USE_FLAG > 0 "))
                        .FirstOrDefault();
                if (check != null)
                {
                    WPFMessageBox.ShowWarring("该设备的刀位号已经存在，不能新增相同的刀位号！", "保存");
                    return;
                }

                m_TmsDeviceToolsPos.PKNO = Guid.NewGuid().ToString("N");
                m_TmsDeviceToolsPos.CREATION_DATE = DateTime.Now;
                m_TmsDeviceToolsPos.CREATED_BY = CBaseData.LoginName;
                m_TmsDeviceToolsPos.LAST_UPDATE_DATE = DateTime.Now;

                ws.UseService(s => s.AddTmsDeviceToolsPos(m_TmsDeviceToolsPos));
            }
            else  //修改
            {
                TmsDeviceToolsPos check =
                    ws.UseService(
                        s =>
                            s.GetTmsDeviceToolsPoss(
                                $"TOOLS_POS_NO = '{m_TmsDeviceToolsPos.TOOLS_POS_NO}' AND PKNO <> '{m_TmsDeviceToolsPos.PKNO}' AND DEVICE_PKNO = '{asset.PKNO}' AND USE_FLAG > 0 "))
                        .FirstOrDefault();
                if (check != null)
                {
                    WPFMessageBox.ShowWarring("该设备的刀位号已经存在，不能修改为该刀位号！", "保存");
                    return;
                }
                m_TmsDeviceToolsPos.LAST_UPDATE_DATE = DateTime.Now;
                m_TmsDeviceToolsPos.UPDATED_BY = CBaseData.LoginName;
                ws.UseService(s => s.UpdateTmsDeviceToolsPos(m_TmsDeviceToolsPos));
            }

            GetPage();  //重新加载

            gbItem.IsCollapsed = true;

            dictInfo.Visibility = Visibility.Collapsed;
        }

        //取消
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            gbItem.IsCollapsed = true;

            dictInfo.Visibility = Visibility.Collapsed;
        }

        #endregion
        
        #endregion

        private void gridItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AmAssetMasterN asset = cmbDeviceInfo.SelectedItem as AmAssetMasterN;
            if (asset == null) return;
            TmsDeviceToolsPos m_TmsDeviceToolsPos = gridItem.SelectedItem as TmsDeviceToolsPos;
            if (m_TmsDeviceToolsPos == null)
            {
                return;
            }
            //修改
            gbItem.IsCollapsed = false;
            dictInfo.Header = "设备刀位信息  【修改】";
            dictInfo.Visibility = Visibility.Visible;

            dictInfo.DataContext = m_TmsDeviceToolsPos;
        }

        private void cmbDeviceInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetPage();
        }

        private const int ReWriteCount = 3;  //数据重写次数

        //下发刀补
        private void btnWriteToolsSet_Click(object sender, RoutedEventArgs e)
        {
            AmAssetMasterN asset = cmbDeviceInfo.SelectedItem as AmAssetMasterN;
            if (asset == null) return;
            TmsDeviceToolsPos mTmsDeviceToolsPos = gridItem.SelectedItem as TmsDeviceToolsPos;
            if (mTmsDeviceToolsPos == null) return;
            TmsToolsMaster tools = ws.UseService(s => s.GetTmsToolsMasterById(mTmsDeviceToolsPos.TOOLS_PKNO));
            if (tools == null) return;

            if (WPFMessageBox.ShowConfirm($"确定要将机床【{asset.ASSET_NAME}】上刀号【{mTmsDeviceToolsPos.TOOLS_POS_NO}】的补偿值覆盖吗？",
                    "下发刀补") != WPFMessageBoxResult.OK)
            {
                return;
            }

            Cursor = Cursors.Wait;

            string tagName = "写入刀补";

            FmsAssetTagSetting tag = DeviceMonitor.GetTagSettings($"ASSET_CODE = '{asset.ASSET_CODE}' AND TAG_NAME = '{tagName}'").FirstOrDefault();

            if (tag == null)
            {
                Cursor = Cursors.Arrow;
                return;
            }

            //0：长度(形状)；1：长度(磨损)；2：半径(形状)；3：半径(磨损)
            List<string> values = new List<string>();
            values.Add(mTmsDeviceToolsPos.TOOLS_POS_NO);   //机床刀号
            values.Add(tools.COMPENSATION_SHAPE_LENGTH.ToString());  //长度(形状)
            //values.Add(tools.COMPENSATION_ABRASION_LENGTH.ToString()); //长度(磨损)
            values.Add(tools.COMPENSATION_SHAPE_DIAMETER.ToString());  //半径(形状)
            //values.Add(tools.COMPENSATION_ABRASION_DIAMETER.ToString());  //半径(磨损)

            string offSetValue = string.Join("|", values.ToArray());

            #region 下发刀补

            int iWrite = 0;
            int ret = 0;
            string error = "";

            while (iWrite < ReWriteCount)
            {
                ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, offSetValue, out error);
                if (ret == 0)
                {
                    break;
                }
                iWrite++;
                Thread.Sleep(100);
            }

            #endregion

            Cursor = Cursors.Arrow;

            if (ret == 0) WPFMessageBox.ShowTips("刀补值下发成功!", "下发刀补");
        }
    }
}
