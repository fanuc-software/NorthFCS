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
using BFM.Server.DataAsset.EAMService;

namespace BFM.WPF.EAM.RM
{
    /// <summary>
    /// RepairMasterView.xaml 的交互逻辑
    /// </summary>
    public partial class RepairMasterView : Page
    {
        private WcfClient<IEAMService> _EAMClient;

        //RmRepairRecord m_RmRepairMaster;
        public RepairMasterView()
        {
            InitializeComponent();
            _EAMClient = new WcfClient<IEAMService>();
            Initialize();
        }
        public void Initialize()
        {
          
            List<AmAssetMasterN> source = _EAMClient.UseService(s => s.GetAmAssetMasterNs("USE_FLAG >= 1"));
            cmbErrorAsset.ItemsSource = source;
            RmRepairRecord m_RmRepairRecord = new RmRepairRecord();
            this.gridLayout.DataContext = m_RmRepairRecord;

        }
        #region 按钮动作

        #region 编辑

        private void BtnAdd_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //新增
        }

        private void BtnMod_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //修改
        }

        private void BtnDel_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //删除
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //保存
            if (this.cmbErrorAsset.SelectedItem == null)
            {
                return;
            }
            RmRepairRecord m_RmRepairRecord = this.gridLayout.DataContext as RmRepairRecord;
            try
            {

                m_RmRepairRecord.ASSET_CODE = ((AmAssetMasterN)this.cmbErrorAsset.SelectedItem).ASSET_CODE;
                m_RmRepairRecord.PKNO = Guid.NewGuid().ToString("N");
                m_RmRepairRecord.UPDATED_INTROD = this.ORDER_ID.Text;
                m_RmRepairRecord.FAULT_CODE = this.cmbCode.Text;
                _EAMClient.UseService(s => s.AddRmRepairRecord(m_RmRepairRecord));
                Initialize();
            
            }
            catch (Exception ex)
            {
                MessageUtil.ShowError(ex.Message);
            }

            //保存成功
            m_RmRepairRecord = new RmRepairRecord();
            this.gridLayout.DataContext = m_RmRepairRecord;
            this.ORDER_ID.Text = "";
            //int faultCount = _EAMClient.UseService(s => s.GetAllRmRepairRecordCount(""));  //记录个数
            //tbFaultCount.Text = faultCount.ToString();
        }

        private void BtnCancel_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //取消
        }

        #endregion

        #region 查询

        private void BtnSearch_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //查询
            Window win = new RmRepairRecordView();
            win.Height = 500;
            win.Width = 795;
            win.WindowStyle = WindowStyle.None;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //win.Closed += Win_Closed;
            win.Title = "故障查询";
            win.ShowDialog();            
        }

        private void BtnMoreSearch_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //高级查询
        }

        #endregion

        #region 导入导出
        private void BtnInPort_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //导入
        }

        private void BtnOutPort_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //导出
        }

        private void BtnReport_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //报表
        }

        #endregion

        #endregion
    }
}
