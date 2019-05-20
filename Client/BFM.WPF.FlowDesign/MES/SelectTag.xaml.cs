using System.Linq;
using System.Windows;
using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.EAMService;
using BFM.Server.DataAsset.FMSService;

namespace BFM.WPF.FlowDesign.MES
{
    /// <summary>
    /// SelectTag.xaml 的交互逻辑
    /// </summary>
    public partial class SelectTag : Window
    {
        public string SelectName = "";
        private WcfClient<IFMSService> ws = new WcfClient<IFMSService>();
        private WcfClient<IEAMService> wsEAM = new WcfClient<IEAMService>();

        public SelectTag()
        {
            InitializeComponent();
        }

        private void SelectTag_OnLoaded(object sender, RoutedEventArgs e)
        {
            cmbDevices.ItemsSource = wsEAM.UseService(s => s.GetAmAssetMasterNs("USE_FLAG = 1"));

            cmbTag.ItemsSource = ws.UseService(s => s.GetFmsAssetTagSettings("USE_FLAG = 1"));

            cmbTag.SelectedValue = SelectName;
        }

        //确定选择
        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (cmbTag.SelectedValue != null)
            {
                SelectName = cmbTag.SelectedValue.ToString();
                Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CmbDevices_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string acessCode = cmbDevices.SelectedValue?.ToString();

            if (!string.IsNullOrEmpty(acessCode))
            {
                cmbTag.ItemsSource = ws.UseService(s =>
                        s.GetFmsAssetTagSettings($"USE_FLAG = 1 AND ASSET_CODE = '{acessCode}'"))
                    .OrderBy(c => c.ASSET_CODE)
                    .ThenBy(c => c.TAG_NAME).ToList();
            }
            else
            {
                cmbTag.ItemsSource = ws.UseService(s =>
                        s.GetFmsAssetTagSettings($"USE_FLAG = 1 "))
                    .OrderBy(c => c.ASSET_CODE)
                    .ThenBy(c => c.TAG_NAME).ToList();
            }
        }

        private void CmbTag_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            FmsAssetTagSetting tag = cmbTag.SelectedItem as FmsAssetTagSetting;
            if (tag == null)
            {
                tbTagType.Text = "请选择Tag";
                return;
            }

            string stateName = "未知";
            int state = tag.STATE_MARK_TYPE??0;
            if (state == 0)
            {
                stateName = "普通";
            }
            else if (state == 1)
            {
                stateName = "状态-脱机";
            }
            else if (state == 3)
            {
                stateName = "状态-工作";
            }
            else if (state == 4)
            {
                stateName = "状态-故障";
            }
            else if (state == 10)
            {
                stateName = "状态";
            }
            else if (state == 11)
            {
                stateName = "位移";
            }
            tbTagType.Text = stateName;
        }
    }
}
