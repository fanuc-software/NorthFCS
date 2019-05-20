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
using BFM.Server.DataAsset.EAMService;
using BFM.WPF.SDM.TableNO;
using BFM.ContractModel;

namespace BFM.WPF.EAM.AM
{
    /// <summary>
    /// AmAssetMasterNEdit.xaml 的交互逻辑
    /// </summary>
    public partial class AmAssetMasterNEdit : Window
    {
        private bool IsNew;
        private WcfClient<IEAMService> _EAMClient;      
        AmAssetMasterN m_AssetMaster;
        public AmAssetMasterNEdit()
        {
            InitializeComponent();
            m_AssetMaster = new AmAssetMasterN() {ASSET_CODE = TableNOHelper.GetNewNO("AM_ASSET_MASTER_N.ASSET_CODE") };
            this.gridLayout.DataContext = m_AssetMaster;
            IsNew = true;
        }

        public AmAssetMasterNEdit(AmAssetMasterN m_AmAssetMasterN)
        {

            InitializeComponent();
            m_AssetMaster = m_AmAssetMasterN;
            this.gridLayout.DataContext = m_AssetMaster;
            IsNew = false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            _EAMClient = new WcfClient<IEAMService>();

            if (IsNew)
            {
                try
                {
                    m_AssetMaster = this.gridLayout.DataContext as AmAssetMasterN;
                    m_AssetMaster.PKNO = Guid.NewGuid().ToString();
                    _EAMClient.UseService(s => s.AddAmAssetMasterN(m_AssetMaster));
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
                    m_AssetMaster = this.gridLayout.DataContext as AmAssetMasterN;
                    _EAMClient.UseService(s => s.UpdateAmAssetMasterN(m_AssetMaster));
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
