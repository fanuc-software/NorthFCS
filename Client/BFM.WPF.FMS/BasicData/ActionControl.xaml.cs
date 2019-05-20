using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.Data.PubData;
using BFM.WPF.Base.Notification;
using BFM.ContractModel;
using BFM.Server.DataAsset.EAMService;
using BFM.Server.DataAsset.FMSService;
using BFM.WPF.Base;
using BFM.WPF.Base.Helper;

namespace BFM.WPF.FMS.BasicData
{
    /// <summary>
    /// ActionControl.xaml 的交互逻辑
    /// </summary>
    public partial class ActionControl : Page
    {
        private WcfClient<IFMSService> _fmsClient = new WcfClient<IFMSService>();
        private WcfClient<IEAMService> _eamClient = new WcfClient<IEAMService>();
          
        private const string HeaderName = "指令动作控制配置";

        public ActionControl()
        {
            InitializeComponent();

            GetPage();

            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }

        private void GetPage()
        {
            List<AmAssetMasterN> assets = _eamClient.UseService(s => s.GetAmAssetMasterNs($"USE_FLAG = {(int)EmUseFlag.Useful}"));
            List<FmsAssetCommParam> assetComm =
                _fmsClient.UseService(s => s.GetFmsAssetCommParams($"USE_FLAG != {(int)EmUseFlag.Deleted}"));

            var assets2 = from c in assets
                          join d in assetComm on c.ASSET_CODE equals d.ASSET_CODE
                          select c;
            cmbAssetInfo.ItemsSource = assets2;

            List<FmsActionControl> actionControls =
                _fmsClient.UseService(s => s.GetFmsActionControls($"")).OrderBy(c => c.ASSET_CODE).ToList();
            gridItem.ItemsSource = actionControls;
        }

        #region 按钮动作

        #region 编辑

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //新增
            #region 

            //TODO: 校验，空的类绑定到界面的DataContent

            FmsActionControl actionControl = new FmsActionControl
            {
                PKNO = "",
                ASSET_CODE = "",
                START_CONDITION_TAG_PKNO = "",
                START_CONDITION_VALUE = "",
                EXECUTE_TAG_PKNO = "",
                EXECUTE_WRITE_VALUE = "",
                EXECUTE_PARAM1_TAG_PKNO  = "",
                EXECUTE_PARAM2_TAG_PKNO = "",
                FINISH_CONDITION_TAG_PKNO = "",
                FINISH_CONDITION_VALUE = "",
            };

            gbItem.DataContext = actionControl;

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
            FmsActionControl actionControl = gridItem.SelectedItem as FmsActionControl;
            if (actionControl == null)
            {
                return;
            }

            if (
                WPFMessageBox.ShowConfirm($"确定要删除 指令动作【{actionControl.ACTION_NAME}】的配置信息吗？", "删除") != WPFMessageBoxResult.OK)
            {
                return;
            }
            _fmsClient.UseService(s => s.DelFmsActionControl(actionControl.PKNO));

            WPFMessageBox.ShowTips("指令动作配置已删除！", "删除提示");

            GetPage();  //重新刷新数据，根据需求是否进行刷新数据
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //保存
            AmAssetMasterN asset = cmbAssetInfo.SelectedItem as AmAssetMasterN;
            FmsActionControl actionControl = gbItem.DataContext as FmsActionControl;

            if (actionControl == null)
            {
                return;
            }

            #region 

            //TODO: 校验；保存
            if (asset == null)
            {
                WPFMessageBox.ShowError("请选择设备！", "保存");
                return;
            }
            if (string.IsNullOrEmpty(actionControl.ACTION_NAME))
            {
                WPFMessageBox.ShowError("请输入指令动作名称！", "保存");
                return;
            }

            if (actionControl.PKNO == "")
            {
                actionControl.PKNO = CBaseData.NewGuid();
                _fmsClient.UseService(s => s.AddFmsActionControl(actionControl));
            }
            else
            {
                _fmsClient.UseService(s => s.UpdateFmsActionControl(actionControl));
            }
            WPFMessageBox.ShowTips("指令动作配置信息已保存。", "保存");

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

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AmAssetMasterN asset = cmbAssetInfo.SelectedItem as AmAssetMasterN;
            if (asset != null)
            {
                cmbAssetInfo.Tag =
                    _fmsClient.UseService(
                        s => s.GetFmsAssetTagSettings($"USE_FLAG = 1 AND ASSET_CODE = '{asset.ASSET_CODE}'"));
            }
        }
    }
}
