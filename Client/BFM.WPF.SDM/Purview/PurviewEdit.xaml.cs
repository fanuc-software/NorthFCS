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
using BFM.Server.DataAsset.SDMService;

namespace BFM.WPF.SDM.Purview
{
    /// <summary>
    /// PurviewEdit.xaml 的交互逻辑
    /// </summary>
    public partial class PurviewEdit : Window
    {
        private WcfClient<ISDMService> _SDMService;
        bool isNew;
        SysPurview m_SysPurview;
        public PurviewEdit()
        {
            InitializeComponent();
            _SDMService = new WcfClient<ISDMService>();
            m_SysPurview = new SysPurview();
            this.DataContext = m_SysPurview;
            isNew = true;
        }
        public PurviewEdit(SysPurview nm_SysPurview)
        {
            InitializeComponent();
            _SDMService = new WcfClient<ISDMService>();
            m_SysPurview = nm_SysPurview;
            this.DataContext = m_SysPurview;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (isNew)
            {
                SysPurview m_SysPurview = new SysPurview();
                m_SysPurview = this.DataContext as SysPurview;
                m_SysPurview.PKNO = Guid.NewGuid().ToString("n");
                _SDMService.UseService(s => s.AddSysPurview(m_SysPurview));
            }
            else
            {
                _SDMService.UseService(s => s.UpdateSysPurview(m_SysPurview));
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
