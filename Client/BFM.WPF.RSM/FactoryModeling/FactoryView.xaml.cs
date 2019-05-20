using BFM.Common.Data.PubData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Server.DataAsset.RSMService;
using BFM.ContractModel;
using BFM.WPF.Base.Helper;

namespace BFM.WPF.RSM.FactoryModeling
{
    /// <summary>
    /// FactoryView.xaml 的交互逻辑
    /// </summary>
    public partial class FactoryView : Page
    {
        private WcfClient<IRSMService> ws = new WcfClient<IRSMService>();
        public FactoryView()
        {
            InitializeComponent();
            Initialize();
        }

        public void Initialize()
        {
            List<RsFactory> m_RsFactory = ws.UseService(s => s.GetRsFactorys(" USE_FLAG > 0"));
            gridItem.ItemsSource = m_RsFactory;

            BFM.WPF.Base.Helper.BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }
        #region 事件


        private void gridItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RsFactory m_RsFactory = gridItem.SelectedItem as RsFactory;
            if (m_RsFactory == null)
            {
                return;
            }
            //修改
            dictInfo.Header = "工厂信息【修改】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //新增
            dictInfo.Header = "工厂信息  【新增】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;

            RsFactory m_RsFactory = new RsFactory()
            {
                COMPANY_CODE = "",           
                USE_FLAG = 1,
            };
            gbItem.DataContext = m_RsFactory;
        }

        private void BtnMod_Click(object sender, RoutedEventArgs e)
        {
            RsFactory m_RsFactory = gridItem.SelectedItem as RsFactory;
            if (m_RsFactory == null)
            {
                return;
            }
            //修改
            dictInfo.Header = "工厂信息 【修改】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            //删除
            RsFactory m_RsFactory = gridItem.SelectedItem as RsFactory;
            if (m_RsFactory == null)
            {
                return;
            }
            if (System.Windows.Forms.MessageBox.Show($"确定删除基础信息【{m_RsFactory.FACTORY_NAME}】吗？",
                    @"删除信息",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                m_RsFactory.USE_FLAG = -1;
                ws.UseService(s => s.UpdateRsFactory(m_RsFactory));

                //删除成功.
                Initialize();  //重新加载
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //保存
            RsFactory m_RsFactory = gbItem.DataContext as RsFactory;
            if (m_RsFactory == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(m_RsFactory.PKNO)) //新增
            {
                m_RsFactory.PKNO = Guid.NewGuid().ToString("N");
                m_RsFactory.CREATION_DATE = DateTime.Now;
                m_RsFactory.CREATED_BY = CBaseData.LoginName;
                m_RsFactory.LAST_UPDATE_DATE = DateTime.Now;  //最后修改日期

                ws.UseService(s => s.AddRsFactory(m_RsFactory));
            }
            else  //修改
            {
                m_RsFactory.LAST_UPDATE_DATE = DateTime.Now;
                m_RsFactory.UPDATED_BY = CBaseData.LoginName;
                ws.UseService(s => s.UpdateRsFactory(m_RsFactory));
            }

            Initialize();  //重新加载

            gbItem.IsCollapsed = true;
            gbItem.Visibility = Visibility.Collapsed;
            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {     //取消
            gbItem.IsCollapsed = true;
            gbItem.Visibility = Visibility.Collapsed;
            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);

        }
        #endregion
    }
}
