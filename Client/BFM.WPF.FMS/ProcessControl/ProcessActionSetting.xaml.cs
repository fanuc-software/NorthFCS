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
using BFM.Server.DataAsset.FMSService;
using BFM.ContractModel;

namespace BFM.WPF.FMS.ProcessControl
{
    /// <summary>
    /// ProcessActionSetting.xaml 的交互逻辑
    /// </summary>
    public partial class ProcessActionSetting : Window
    {
        private WcfClient<IEAMService> wsEAM = new WcfClient<IEAMService>(); //设备
        private WcfClient<IFMSService> wsFMS = new WcfClient<IFMSService>(); //设备

        public RsRoutingDetail RoutingDetail;

        public ProcessActionSetting(RsRoutingDetail routingDetail, string assetCode)
        {
            InitializeComponent();

            RoutingDetail = routingDetail;

            cmbAction.ItemsSource = wsFMS.UseService(s => s.GetFmsActionControls($"ASSET_CODE = '{assetCode}'"));

            gpAction.DataContext = routingDetail;  //原始的Detail；
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            RoutingDetail = gpAction.DataContext as RsRoutingDetail;
            if (RoutingDetail != null) RoutingDetail.REMARK = cmbAction.Text;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
