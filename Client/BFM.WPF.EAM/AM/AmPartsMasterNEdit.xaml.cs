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

namespace BFM.WPF.EAM.AM
{
    /// <summary>
    /// AmPartsMasterNEdit.xaml 的交互逻辑
    /// </summary>
    public partial class AmPartsMasterNEdit : Window
    {
        private bool IsNew;
        private WcfClient<IEAMService> _EAMClient;
        AmPartsMasterN m_PartMaster;
        public AmPartsMasterNEdit()
        {
            InitializeComponent();
            m_PartMaster = new AmPartsMasterN();
            this.gridLayout.DataContext = m_PartMaster;
            IsNew = true;
        }

        public AmPartsMasterNEdit(AmPartsMasterN m_AmPartsMasterN)
        {
            InitializeComponent();
            m_PartMaster = m_AmPartsMasterN;
            this.gridLayout.DataContext = m_PartMaster;
            IsNew = false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            _EAMClient = new WcfClient<IEAMService>();



            if (IsNew)
            {
                try
                {
                    m_PartMaster = this.gridLayout.DataContext as AmPartsMasterN;
                    m_PartMaster.PKNO = Guid.NewGuid().ToString();
                    _EAMClient.UseService(s => s.AddAmPartsMasterN(m_PartMaster));
                }
                catch (Exception ex)
                {
                    MessageUtil.ShowError(ex.Message);
                }
            }
            else
            {

                try
                {
                    m_PartMaster = this.gridLayout.DataContext as AmPartsMasterN;
                    _EAMClient.UseService(s => s.UpdateAmPartsMasterN(m_PartMaster));
                }
                catch (Exception ex)
                {
                    MessageUtil.ShowError(ex.Message);
                }
            }

            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
