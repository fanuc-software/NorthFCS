using BFM.WPF.Base.Controls;
using DevExpress.Xpf.Bars;
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
    /// MaintainStandardView.xaml 的交互逻辑
    /// </summary>
    public partial class MaintainStandardView : Page
    {
        private WcfClient<IEAMService> ws;
        public MaintainStandardView()
        {
            InitializeComponent();
            ws = new WcfClient<IEAMService>();
            GetPage();
        }
        private void GetPage()
        {
            List<RsMaintainStandards> source = ws.UseService(s => s.GetRsMaintainStandardss("USE_FLAG >= 0"));
            gridItem.ItemsSource = source;
        }


        #region 按钮动作

        #region 编辑

        private void BtnAdd_Click(object sender, RoutedEventArgs e )
        {
            ImageButtonWithIcon item = sender as ImageButtonWithIcon;

            //新增
            if (item.Content.ToString().Contains("主项"))
            {
                RsMaintainStandards MaintainStandards = new RsMaintainStandards()
                {
                 
                    CREATION_DATE = DateTime.Now,
                    LAST_UPDATE_DATE = DateTime.Now,  //最后修改日期
                    USE_FLAG = 1,  //启用
                };
                gbItem.DataContext = MaintainStandards;

             
                gbItem.IsCollapsed = false;
                gbItem.Visibility = Visibility.Visible;
            }
            else
            {
                if (this.gridItem .SelectedItem == null) return;
                RsMaintainStandards selectItem = this.gridItem.SelectedItem as RsMaintainStandards;

                Window win = new MaintainStandardsDetailEdit( true ,selectItem);
                win.Height = 500;
                win.Width = 795;
                win.WindowStyle = WindowStyle.None;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Closed += Win_Closed;
                win.Title = "新增明细";
                win.ShowDialog();
            }
        }

        private void Win_Closed(object sender, EventArgs e)
        {
            GetPage();
        }

        private void BtnMod_Click(object sender, RoutedEventArgs e )
        {
            ImageButtonWithIcon item = sender as ImageButtonWithIcon;
            //修改
           
            if (item.Content.ToString().Contains("主项"))
            {
                gbItem.DataContext = gridItem.SelectedItem;
                gbItem.IsCollapsed = false;
                gbItem.Visibility = Visibility.Visible;
            }
            else
            {
                if (this.gridItem.SelectedItem == null) return;
                RsMaintainStandards selectItem = this.gridItem.SelectedItem as RsMaintainStandards;

                Window win = new MaintainStandardsDetailEdit(false, selectItem);
                win.Height = 500;
                win.Width = 795;
                win.WindowStyle = WindowStyle.None;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Closed += Win_Closed;
                win.Title = "修改明细";
                win.ShowDialog();
            }
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e )
        {
            //删除
            if (this.gridItem.SelectedItem == null) return;
            RsMaintainStandards selectItem = this.gridItem.SelectedItem as RsMaintainStandards;
            if (this.gridItem.SelectedItem == null)
            {
                MessageUtil.ShowError("请选择删除数据！");
                return;
            }
            if (!MessageUtil.ConfirmYesNo("是否确定删除选择的数据？！")) return;
            List<string> deList = new List<string>();
            RsMaintainStandards m_RsMaintainStandards = this.gridItem.SelectedItem as RsMaintainStandards;
            m_RsMaintainStandards.USE_FLAG = -1;
            ws.UseService(s => s.UpdateRsMaintainStandards(m_RsMaintainStandards));
            GetPage();
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e )
        {
            //保存
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e )
        {
            //取消
        }

        #endregion

        #region 查询

        private void BtnSearch_Click(object sender, RoutedEventArgs e )
        {
            //查询
        }

        private void BtnMoreSearch_Click(object sender, RoutedEventArgs e )
        {
            //高级查询
        }

        /// <summary>
        /// 双击表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (gridItem.SelectedItem == null)
            {
                return;
            }

            gbItem.DataContext = gridItem.SelectedItem;
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
            //修改

        }
        #endregion



        #endregion


    }
}
