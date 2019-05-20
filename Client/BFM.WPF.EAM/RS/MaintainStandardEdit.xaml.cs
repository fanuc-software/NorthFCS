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
    public partial class MaintainStandardEdit : Window
    {
        private bool IsNew;
        private WcfClient<IEAMService> _EAMClient;
        public MaintainStandardEdit()
        {
            InitializeComponent();
            IsNew = true;
            RsMaintainStandards m_MaintainStandars = new RsMaintainStandards();
            this.DataContext = m_MaintainStandars;
        }
        public MaintainStandardEdit(RsMaintainStandards m_MaintainStandarsN)
        {
            InitializeComponent();
            IsNew = false;
            this.DataContext = m_MaintainStandarsN;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            _EAMClient = new WcfClient<IEAMService>();
            RsMaintainStandards m_MaintainStandars = this.DataContext as RsMaintainStandards;

            if (IsNew)
            {
                if (comboBoxISENABLE.Text == "启用")

                    m_MaintainStandars.USE_FLAG = 1;

                else
                    m_MaintainStandars.USE_FLAG = 0;

                try
                {
                    m_MaintainStandars.PKNO = Guid.NewGuid().ToString("N");
                    m_MaintainStandars.CREATION_DATE = DateTime.Now;
                    m_MaintainStandars.LAST_UPDATE_DATE = DateTime.Now;  //最后修改日期
                    _EAMClient.UseService(s => s.AddRsMaintainStandards(m_MaintainStandars));
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
                    _EAMClient.UseService(s => s.UpdateRsMaintainStandards(m_MaintainStandars));
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
