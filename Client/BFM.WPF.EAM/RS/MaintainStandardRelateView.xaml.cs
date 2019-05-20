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
using BFM.ContractModel;

namespace BFM.WPF.EAM.RS
{
    /// <summary>
    /// MaintainStandardRelateView.xaml 的交互逻辑
    /// </summary>
    public partial class MaintainStandardRelateView : Page
    {
        private WcfClient<IEAMService> _EAMClient;
        public MaintainStandardRelateView()
        {
            InitializeComponent();
            _EAMClient = new WcfClient<IEAMService>();
            Initialize();
        }
        private void Initialize()
        {
            List<AmAssetMasterN> m_AssetMaster = new List<AmAssetMasterN>();
            m_AssetMaster = _EAMClient.UseService(s => s.GetAmAssetMasterNs(""));            
            this.GridControl.ItemsSource = m_AssetMaster;
            List<RsMaintainStandards> m_MaintainStandars = new List<RsMaintainStandards>();
            m_MaintainStandars = _EAMClient.UseService(s => s.GetRsMaintainStandardss(""));
            ComStandard.ItemsSource = m_MaintainStandars;
            ComStandard.ValueMember = "PKNO";
            ComStandard.DisplayMember = "STANDARD_NAME";

        }
      
        private void GridControl_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            List<RsMaintainStandardsRelate> m_RsMaintainStandardsRelate = new List<RsMaintainStandardsRelate>();
            AmAssetMasterN m_AssetMaster = new AmAssetMasterN();
            m_AssetMaster = GridControl.SelectedItem as AmAssetMasterN;
            List<RsMaintainStandardsDetail> m_RsMaintainStandardsDetail = new List<RsMaintainStandardsDetail>();
            m_RsMaintainStandardsRelate = _EAMClient.UseService(s => s.GetRsMaintainStandardsRelates("ASSET_CODE = "+ m_AssetMaster .ASSET_CODE+ ""));
            if (m_RsMaintainStandardsRelate.Count>0)
            {
                m_RsMaintainStandardsDetail = _EAMClient.UseService(s => s.GetRsMaintainStandardsDetails("PKNO = " + m_RsMaintainStandardsRelate[0].STANDARD_DETAIL_PKNO + ""));
                //GridControl_Standards.ItemsSource = m_RsMaintainStandardsDetail;
                if (m_RsMaintainStandardsDetail.Count>0)
                {
                    RsMaintainStandards a_RSMaintainStandars = _EAMClient.UseService(s => s.GetRsMaintainStandardsById(m_RsMaintainStandardsDetail[0].STANDARD_PKNO));

                    for (int i = 0; i < ComStandard.Items.Count; i++)
                    {

                        RsMaintainStandards temp = ComStandard.Items[i] as RsMaintainStandards;
                        if (temp.STANDARD_NAME== a_RSMaintainStandars.STANDARD_NAME)
                        {
                            ComStandard.SelectedIndex = 0;
                            ComStandard.SelectedIndex = i;
                        }
                    }
                    this.ComStandard.SelectedText = a_RSMaintainStandars.STANDARD_NAME;
                }
            
               
                //this.ComStandard.SelectedIndex();
            }
            else
            {
                this.ComStandard.Text = "";
                GridControl_Standards.ItemsSource = null;
            }

        }
        #region 按钮动作

        #region 编辑

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            MaintainStandard_NingBo d= new MaintainStandard_NingBo();
            d.Show();
            return;
            //绑定
            //todo： 未对判重进行逻辑操作。
            RsMaintainStandardsRelate m_RsMaintainStandardsRelate = new RsMaintainStandardsRelate();

            AmAssetMasterN m_AssetMaster = new AmAssetMasterN();
            m_AssetMaster = GridControl.SelectedItem as AmAssetMasterN;
            RsMaintainStandards a_RSMaintainStandars = ComStandard.SelectedItem as RsMaintainStandards;
            List<RsMaintainStandardsDetail> m_RsMaintainStandardsDetail = new List<RsMaintainStandardsDetail>();
            m_RsMaintainStandardsDetail = GridControl_Standards.ItemsSource as List<RsMaintainStandardsDetail>;
            if (m_RsMaintainStandardsDetail == null)
            {
                System.Windows.Forms.MessageBox.Show("请选择维护规程！！！");
                return;
            }
            //解除之前绑定的规程
            List<RsMaintainStandardsRelate> del_RsMaintainStandardsRelate = new List<RsMaintainStandardsRelate>();
            del_RsMaintainStandardsRelate = _EAMClient.UseService(s => s.GetRsMaintainStandardsRelates(""));
            //List<string> delstr = new List<string>();
            foreach (RsMaintainStandardsRelate item in del_RsMaintainStandardsRelate)
            {
                if (item.ASSET_CODE == m_AssetMaster.ASSET_CODE)
                {
                    //delstr.Add(item.PKNO);
                    _EAMClient.UseService(s => s.DelRsMaintainStandardsRelate(item.PKNO));
                }

            }
          
            int i = 0;
            foreach (RsMaintainStandardsDetail item in m_RsMaintainStandardsDetail)
            {
                m_RsMaintainStandardsRelate.PKNO = Guid.NewGuid().ToString("N");
                m_RsMaintainStandardsRelate.ASSET_CODE = m_AssetMaster.ASSET_CODE;
                m_RsMaintainStandardsRelate.STANDARD_DETAIL_PKNO = item.PKNO;
                m_RsMaintainStandardsRelate.LAST_MAINTAIN_TIME = DateTime.Now;
                m_RsMaintainStandardsRelate.CREATION_DATE = DateTime.Now;
                m_RsMaintainStandardsRelate.LAST_UPDATE_DATE = DateTime.Now;
                _EAMClient.UseService(s => s.AddRsMaintainStandardsRelate(m_RsMaintainStandardsRelate));
                i++;
            }
            System.Windows.Forms.MessageBox.Show("绑定完成");
        }

        private void BtnMod_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //修改
        }

        private void BtnDel_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //删除
        }
        private void BtnSave_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //保存
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

        private void ComStandard_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            List<RsMaintainStandardsDetail> m_RsMaintainStandardsDetail = new List<RsMaintainStandardsDetail>();

            RsMaintainStandards a_RSMaintainStandars = ComStandard.SelectedItem as RsMaintainStandards;
            if (a_RSMaintainStandars==null)
            {
                return;
            }
            m_RsMaintainStandardsDetail = _EAMClient.UseService(s => s.GetRsMaintainStandardsDetails("STANDARD_PKNO = " + a_RSMaintainStandars.PKNO + ""));
            GridControl_Standards.ItemsSource = m_RsMaintainStandardsDetail;
        }

    }
}
