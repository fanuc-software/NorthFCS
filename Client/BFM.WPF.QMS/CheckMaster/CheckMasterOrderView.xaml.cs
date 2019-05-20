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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.QMSService;
using BFM.Server.DataAsset.RSMService;

namespace BFM.WPF.QMS.CheckMaster
{
    /// <summary>
    /// CheckMasterOrderView.xaml 的交互逻辑
    /// </summary>
    public partial class CheckMasterOrderView : Page
    {
        private WcfClient<IQMSService> ws = new WcfClient<IQMSService>();
        private WcfClient<IRSMService> ws2 = new WcfClient<IRSMService>();
        public CheckMasterOrderView()
        {
            InitializeComponent();
            Getdata();
        }

        private void Getdata()
        {
            List<QmsCheckMaster>  qmsCheckMasters=  ws.UseService(s => s.GetQmsCheckMasters(" USE_FLAG >0 AND CHECK_STATUS != -1"));
            gridItem.ItemsSource = qmsCheckMasters;
        }

        private void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            //todo:质检页面
            if (gridItem.SelectedItem != null)
            {
                QmsCheckMaster qmsCheckMaster = gridItem.SelectedItem as QmsCheckMaster;
                CheckRecordView checkRecord = new CheckRecordView(qmsCheckMaster);
                checkRecord.Width = 800;
                checkRecord.Height = 700;
                checkRecord.Closed += CheckRecord_Closed;
                checkRecord.Show();
            }
        }

        private void CheckRecord_Closed(object sender, EventArgs e)
        {
            Getdata();
        }

        private void BtnCancle_Click(object sender, RoutedEventArgs e)
        {
           
            if (gridItem.SelectedItem!=null)
            {
                QmsCheckMaster qmsCheckMaster = gridItem.SelectedItem as QmsCheckMaster;

                if (qmsCheckMaster != null)
                {
                    qmsCheckMaster.CHECK_STATUS = "-1";
                    qmsCheckMaster.USE_FLAG = -1;
                }

                ws.UseService(s => s.UpdateQmsCheckMaster(qmsCheckMaster));
            }

            Getdata();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Getdata();
        }
    }
}
