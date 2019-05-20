using BFM.Server.DataAsset.EAMService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BFM.Common.Base.Utils;
using BFM.ContractModel;

namespace BFM.WPF.FMS.ProcessControl
{
    /// <summary>
    /// DeviceSelect.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceSelect : Window
    {
        private WcfClient<IEAMService> wsEAM = new WcfClient<IEAMService>(); //设备
        public DeviceSelect(RsRoutingDetail routingDetail)
        {
            InitializeComponent();
            gridItem.ItemsSource = wsEAM.UseService(s => s.GetAmAssetMasterNs($"ASSET_GROUP = '{routingDetail.WC_CODE}'"));
        }

        private void gridItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AmAssetMasterN amAssetMasterN = gridItem.SelectedItem as AmAssetMasterN;
            this.Tag = amAssetMasterN;
            this.Close();
        }
    }
}
