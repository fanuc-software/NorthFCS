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
    /// CheckRecordView.xaml 的交互逻辑
    /// </summary>
    public partial class CheckRecordView : Window
    {
        private QmsCheckMaster m_qmsCheckMaster=new QmsCheckMaster();
        private WcfClient<IQMSService> ws = new WcfClient<IQMSService>();
        private WcfClient<IRSMService> ws2 = new WcfClient<IRSMService>();
        public CheckRecordView()
        {
            InitializeComponent();
        }

        public CheckRecordView(QmsCheckMaster qmsCheckMaster)
        {
            InitializeComponent();
            if (qmsCheckMaster!=null)
            {
                m_qmsCheckMaster = qmsCheckMaster;
                GetPage();
            }

        
        }

        private void GetPage()
        {
           
            QmsCheckParam qmsCheckParam = ws.UseService(s => s.GetQmsCheckParamById(m_qmsCheckMaster.CHECK_PARAM_PKNO));
            QmsRoutingCheck qmsRoutingCheck = ws.UseService(s => s.GetQmsRoutingCheckById(m_qmsCheckMaster.ROUTING_CHECK_PKNO));
            RsItemMaster rsItemMaster = ws2.UseService(s => s.GetRsItemMasterById(qmsCheckParam.ITEM_PKNO));
            RsRoutingDetail rsRoutingDetail =
                ws2.UseService(s => s.GetRsRoutingDetailById(qmsRoutingCheck.PROCESS_PKNO));
            TextCheckMode.Text=  m_qmsCheckMaster.CHK_MODE;
            TextName.Text = m_qmsCheckMaster.CHECK_NO + System.Environment.NewLine  + rsItemMaster.ITEM_NAME;
            TextCheckSize.Text = qmsCheckParam.MIN_SIZE + "-" + qmsCheckParam.MAX_SIZE;
            TextRoutingName.Text = rsRoutingDetail.OP_NO + "  " + rsRoutingDetail.OP_NAME + " / " +
                                   qmsCheckParam.CHECK_NAME;
            TextDevice.Text = qmsCheckParam.CHECK_DEVICE;
        }
        private void BtCommit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
