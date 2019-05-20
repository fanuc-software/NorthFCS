using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.Base.Helper;
using BFM.Common.Data.PubData;
using BFM.Common.DeviceAsset;
using BFM.Server.DataAsset.FMSService;
using BFM.Server.DataAsset.EAMService;
using BFM.WPF.Base.Notification;
using BFM.ContractModel;
using BFM.WPF.Base.Helper;

namespace BFM.WPF.FMS.BasicData
{
    /// <summary>
    /// AssetCommParamSetting.xaml 的交互逻辑
    /// </summary>
    public partial class AssetCommParamSetting : Page
    {
        private WcfClient<IFMSService> _fmsClient = new WcfClient<IFMSService>();
        private WcfClient<IEAMService> _eamClient = new WcfClient<IEAMService>();
          
        private const string HeaderName = "设备通讯参数配置";

        public AssetCommParamSetting()
        {
            InitializeComponent();

            cmbInterfaceType.ItemsSource = EnumHelper.GetEnumsToList<DeviceCommInterface>().DefaultView;

            GetPage();

            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }

        private void GetPage()
        {
            List<AmAssetMasterN> assets = _eamClient.UseService(s => s.GetAmAssetMasterNs($"USE_FLAG = {(int)EmUseFlag.Useful}"));
            cmbAssetInfo.ItemsSource = assets;

            List<FmsAssetCommParam> assetComms =
                _fmsClient.UseService(s => s.GetFmsAssetCommParams($"USE_FLAG != {(int)EmUseFlag.Deleted}")).OrderBy(c => c.ASSET_CODE).ToList();
            gridItem.ItemsSource = assetComms;
        }

        #region 按钮动作

        #region 编辑

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //新增
            #region 

            //TODO: 校验，空的类绑定到界面的DataContent

            FmsAssetCommParam assetComm = new FmsAssetCommParam {PKNO = "", ASSET_CODE = "", USE_FLAG = 1};

            gbItem.DataContext = assetComm;

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
            FmsAssetCommParam assetComm = gridItem.SelectedItem as FmsAssetCommParam;
            if (assetComm == null)
            {
                return;
            }

            if (
                MessageBox.Show($"确定要删除 设备编号为【{assetComm.ASSET_CODE}】的通讯配置信息吗？", "删除", MessageBoxButton.OKCancel,
                    MessageBoxImage.Question) != MessageBoxResult.OK)
            {
                return;
            }
            assetComm.USE_FLAG = (int)EmUseFlag.Deleted;  //已删除
            assetComm.UPDATED_BY = CBaseData.LoginName;
            assetComm.LAST_UPDATE_DATE = DateTime.Now;
            assetComm.UPDATED_INTROD += "删除 ";
            _fmsClient.UseService(s => s.UpdateFmsAssetCommParam(assetComm));

            NotificationInvoke.NewNotification("删除提示", "设备通讯配置信息已删除！");

            GetPage();  //重新刷新数据，根据需求是否进行刷新数据
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //保存
            AmAssetMasterN asset = cmbAssetInfo.SelectedItem as AmAssetMasterN;
            FmsAssetCommParam assetComm = gbItem.DataContext as FmsAssetCommParam;

            if (assetComm == null)
            {
                return;
            }

            #region 

            //TODO: 校验；保存
            if (asset == null)
            {
                MessageBox.Show("请选择设备！", "保存", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (assetComm.PKNO == "")
            {
                assetComm.PKNO = CBaseData.NewGuid();
                assetComm.CREATED_BY = CBaseData.LoginName;
                assetComm.CREATION_DATE = DateTime.Now;
                assetComm.LAST_UPDATE_DATE = DateTime.Now;  //最后修改日期
                _fmsClient.UseService(s => s.AddFmsAssetCommParam(assetComm));
            }
            else
            {
                assetComm.UPDATED_BY = CBaseData.LoginName;
                assetComm.LAST_UPDATE_DATE = DateTime.Now;
                _fmsClient.UseService(s => s.UpdateFmsAssetCommParam(assetComm));
            }
            NotificationInvoke.NewNotification("保存", "设备通讯配置信息已保存。");

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
    }
}
