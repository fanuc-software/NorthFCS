using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.Data.PubData;
using BFM.Common.DeviceAsset;
using BFM.Common.Base.Helper;
using BFM.Server.DataAsset.EAMService;
using BFM.Server.DataAsset.FMSService;
using BFM.WPF.Base.Notification;
using BFM.ContractModel;
using BFM.WPF.Base;
using BFM.WPF.Base.Helper;

namespace BFM.WPF.FMS.BasicData
{
    /// <summary>
    /// AssetTagSetting.xaml 的交互逻辑
    /// </summary>
    public partial class AssetTagSetting : Page
    {
        private WcfClient<IFMSService> _fmsClient = new WcfClient<IFMSService>();
        private WcfClient<IEAMService> _eamClient = new WcfClient<IEAMService>();
          
        private const string HeaderName = "设备通讯标签配置";

        public AssetTagSetting()
        {
            InitializeComponent();

            cmbValueType.ItemsSource = EnumHelper.GetEnumsToList<TagDataType>().DefaultView;

            GetPage();
        }

        private void GetPage()
        {
            List<AmAssetMasterN> assets = _eamClient.UseService(s => s.GetAmAssetMasterNs($"USE_FLAG = {(int)EmUseFlag.Useful}"));

            List<FmsAssetCommParam> assetComm =
                _fmsClient.UseService(s => s.GetFmsAssetCommParams($"USE_FLAG != {(int) EmUseFlag.Deleted}"));

            var assets2 = from c in assets
                join d in assetComm on c.ASSET_CODE equals d.ASSET_CODE
                select c;
            cmbAssetInfo.ItemsSource = assets2;

            List<FmsAssetTagSetting> assetTags =
                _fmsClient.UseService(s => s.GetFmsAssetTagSettings($"USE_FLAG != {(int)EmUseFlag.Deleted}")).OrderBy(c => c.ASSET_CODE).ToList();
            gridItem.ItemsSource = assetTags;

            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }

        private void cmbAssetInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AmAssetMasterN asset = cmbAssetInfo.SelectedItem as AmAssetMasterN;
            if (asset == null)
            {
                return;
            }

            FmsAssetCommParam comm = _fmsClient
                .UseService(s => s.GetFmsAssetCommParams($"ASSET_CODE = '{asset.ASSET_CODE}' AND USE_FLAG = 1"))
                .FirstOrDefault();
            tbAddressIntrod.DataContext = comm;

        }

        #region 按钮动作

        #region 编辑

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //新增
            #region 

            //TODO: 校验，空的类绑定到界面的DataContent

            FmsAssetTagSetting assetTag = new FmsAssetTagSetting
            {
                PKNO = "",
                ASSET_CODE = "",
                STATE_MARK_TYPE = 0,
                SAMPLING_MODE = 0,
                RECORD_TYPE = 0, //默认不记录
                USE_FLAG = 1
            };

            gbItem.DataContext = assetTag;

            #endregion

            dictBasic.Header = $"{HeaderName}  【新增】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
        }

        private void BtnMod_Click(object sender, RoutedEventArgs e)
        {
            ModItem();
        }

        /// <summary>
        /// 修改主体
        /// </summary>
        private void ModItem()
        {
            //修改
            #region 

            //TODO: 校验

            #endregion

            dictBasic.Header = $"{HeaderName} 【修改】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            //删除
            FmsAssetTagSetting assetTag = gridItem.SelectedItem as FmsAssetTagSetting;
            if (assetTag == null)
            {
                return;
            }

            if (
                MessageBox.Show($"确定要删除 标签名称【{assetTag.TAG_NAME}】的通讯标签配置信息吗？", "删除", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }
            assetTag.USE_FLAG = (int)EmUseFlag.Deleted;  //已删除
            assetTag.UPDATED_BY = CBaseData.LoginName;
            assetTag.LAST_UPDATE_DATE = DateTime.Now;
            assetTag.UPDATED_INTROD += "删除 ";
            _fmsClient.UseService(s => s.UpdateFmsAssetTagSetting(assetTag));

            NotificationInvoke.NewNotification("删除提示", "设备通讯标签配置已删除！");

            GetPage();  //重新刷新数据，根据需求是否进行刷新数据
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //保存
            AmAssetMasterN asset = cmbAssetInfo.SelectedItem as AmAssetMasterN;
            FmsAssetTagSetting assetTag = gbItem.DataContext as FmsAssetTagSetting;

            if (assetTag == null)
            {
                return;
            }

            #region 校验

            if (asset == null)
            {
                MessageBox.Show("请选择设备！", "保存", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (!string.IsNullOrEmpty(assetTag.TAG_CODE)) //标签编码
            {
                List<FmsAssetTagSetting> existTags = _fmsClient.UseService(s =>
                    s.GetFmsAssetTagSettings(
                        $"USE_FLAG = 1 AND TAG_CODE = '{assetTag.TAG_CODE}'"));
                if (!string.IsNullOrEmpty(assetTag.PKNO))  //修改
                {
                    existTags = existTags.Where(c => c.PKNO != assetTag.PKNO).ToList();
                }

                if (existTags.Any())
                {
                    WPFMessageBox.ShowWarring(
                        $"该标签编码【{assetTag.TAG_CODE}】已存在不能" + (string.IsNullOrEmpty(assetTag.PKNO) ? "添加" : "修改") + "为这个编码！",
                        "保存");
                    return;
                }
            }

            #endregion

            #region 保存

            if (string.IsNullOrEmpty(assetTag.PKNO))
            {
                assetTag.PKNO = CBaseData.NewGuid();
                assetTag.CREATED_BY = CBaseData.LoginName;
                assetTag.CREATION_DATE = DateTime.Now;
                assetTag.LAST_UPDATE_DATE = DateTime.Now; //最后修改日期
                _fmsClient.UseService(s => s.AddFmsAssetTagSetting(assetTag));
            }
            else
            {
                assetTag.UPDATED_BY = CBaseData.LoginName;
                assetTag.LAST_UPDATE_DATE = DateTime.Now;
                _fmsClient.UseService(s => s.UpdateFmsAssetTagSetting(assetTag));
            }
            NotificationInvoke.NewNotification("保存", "设备通讯标签配置信息已保存。");

            #endregion

            GetPage();  //重新刷新数据，根据需求是否进行刷新数据

            //保存成功
            gbItem.IsCollapsed = true;
            gbItem.Visibility = Visibility.Collapsed;
            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //取消
            gbItem.IsCollapsed = true;
            gbItem.Visibility = Visibility.Collapsed;
            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }

        #endregion

        #endregion

        /// <summary>
        /// 双击表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (gridItem.VisibleRowCount <= 0)
            {
                return;
            }
            //修改
            ModItem();
        }

        private void bRefresh_Click(object sender, RoutedEventArgs e)
        {
            //刷新
            BtnCancel_Click(null, null);
            GetPage();
        }
    }
}
