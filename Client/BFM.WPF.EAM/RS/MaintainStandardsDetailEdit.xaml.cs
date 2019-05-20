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
using BFM.Server.DataAsset.EAMService;
using BFM.ContractModel;

namespace BFM.WPF.EAM.RS
{
    /// <summary>
    /// MaintainStandardsDetailEdit.xaml 的交互逻辑
    /// </summary>
    public partial class MaintainStandardsDetailEdit : Window
    {
        private WcfClient<IEAMService> ws;
        bool isNew;
        RsMaintainStandards m_RsMaintainStandards;
        public MaintainStandardsDetailEdit(bool misNew, RsMaintainStandards nm_RsMaintainStandards)
        {
            InitializeComponent();
            isNew = misNew;
            m_RsMaintainStandards = nm_RsMaintainStandards;
            if (isNew)
            {
                view.Visibility = Visibility.Collapsed;
            }
            ws = new WcfClient<IEAMService>();
            Initialize(m_RsMaintainStandards);
        }
        public MaintainStandardsDetailEdit()
        {
            InitializeComponent();
        }

        public void Initialize(RsMaintainStandards m_RsMaintainStandards)
        {
            List<RsMaintainStandardsDetail> m_RsMaintainStandardsDetails = ws.UseService(s => s.GetRsMaintainStandardsDetails("STANDARD_PKNO = " + m_RsMaintainStandards.PKNO + ""));
            this.gridView.ItemsSource = m_RsMaintainStandardsDetails;
            RsMaintainStandardsDetail a = new RsMaintainStandardsDetail();
            gridLayout.DataContext = a;
        }

        private void gridView_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            gridLayout.DataContext = e.NewItem as RsMaintainStandardsDetail;
            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (isNew)
            {
             
                RsMaintainStandardsDetail m_RsMaintainStandardsDetail = new RsMaintainStandardsDetail();
             
                m_RsMaintainStandardsDetail = gridLayout.DataContext as RsMaintainStandardsDetail;
                m_RsMaintainStandardsDetail.PKNO = Guid.NewGuid().ToString("N");
                m_RsMaintainStandardsDetail.STANDARD_PKNO = m_RsMaintainStandards.PKNO;
                ws.UseService(s => s.AddRsMaintainStandardsDetail(m_RsMaintainStandardsDetail));
            }
            else
            {
                RsMaintainStandardsDetail m_RsMaintainStandardsDetail = new RsMaintainStandardsDetail();
                m_RsMaintainStandardsDetail = gridLayout.DataContext as RsMaintainStandardsDetail;
                ws.UseService(s => s.UpdateRsMaintainStandardsDetail(m_RsMaintainStandardsDetail));
            }
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
