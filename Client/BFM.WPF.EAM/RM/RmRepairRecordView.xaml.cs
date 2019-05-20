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
using BFM.Server.DataAsset.EAMService;

namespace BFM.WPF.EAM.RM
{
    /// <summary>
    /// RmRepairRecordView.xaml 的交互逻辑
    /// </summary>
    public partial class RmRepairRecordView : Window
    {
        private WcfClient<IEAMService> _EAMClient;
        public RmRepairRecordView()
        {
            InitializeComponent();
            _EAMClient = new WcfClient<IEAMService>();

            Initialize();
        }
        private void Initialize()
        {

            List<RmRepairRecord> lsRmRepairReocord = _EAMClient.UseService(s => s.GetRmRepairRecords(""));

            ////List <RmRepairRecord> lsRmRepairReocord = _EAMClient.UseService(s => s.GetRmRepairRecordByPage(DataPagerSize, pageIndex, 
            ////           true, "FAULT_OCCURRENCE_TIME", jsWhere));
            //List<AssetMaster> lsAssetMaster = _EAMClient.UseService(s => s.GetAllAssetMaster());

            this.gridView.ItemsSource = lsRmRepairReocord;

            //this.DataContext = this;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
