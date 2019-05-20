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

namespace BFM.WPF.EAM.RS
{
    /// <summary>
    /// MaintainStandardEdit.xaml 的交互逻辑
    /// </summary>
    public partial class MaintainStandard_NingBo : Window
    {
        private bool IsNew;
        private WcfClient<IEAMService> _EAMClient;
        public MaintainStandard_NingBo()
        {
            InitializeComponent();
            IsNew = true;
            RsMaintainStandards m_MaintainStandars = new RsMaintainStandards();
            this.DataContext = m_MaintainStandars;
        }
        public MaintainStandard_NingBo(RsMaintainStandards m_MaintainStandarsN)
        {
            InitializeComponent();
            IsNew = false;
            this.DataContext = m_MaintainStandarsN;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
          

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
