using DevExpress.Xpf.Grid;
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
using System.Windows.Shapes;
using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.SDMService;
using BFM.WPF.Base;

namespace BFM.WPF.SDM.MenuItem
{
    /// <summary>
    /// MenuItemView.xaml 的交互逻辑
    /// </summary>
    public partial class MenuItemView : Page
    {
        private WcfClient<ISDMService> _SDMService;
        public MenuItemView()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            _SDMService = new WcfClient<ISDMService>();
            List<SysMenuItem> m_SysMenuItems =
                _SDMService.UseService(s => s.GetSysMenuItems("TARGET_NAME = 'WPF'")).OrderBy(c => c.ITEM_SEQ).ToList();
            this.treeList.ItemsSource = m_SysMenuItems;
        }

        private void MenuAdd_Click(object sender, RoutedEventArgs e)
        {
            if (treeList.SelectedItem == null)
            {
                SysMenuItem m_SysMenuItem = new SysMenuItem();
                m_SysMenuItem.PKNO = Guid.NewGuid().ToString("N");
                m_SysMenuItem.TARGET_NAME = "WPF";
                m_SysMenuItem.PARENT_PKNO = "0";
                bool isSuccss = _SDMService.UseService(s => s.AddSysMenuItem(m_SysMenuItem));

            }
            else
            {
                SysMenuItem m_SysMenuItem = new SysMenuItem();
                m_SysMenuItem.PKNO = Guid.NewGuid().ToString("N");
                m_SysMenuItem.TARGET_NAME = "WPF";
                m_SysMenuItem.PARENT_PKNO = (treeList.SelectedItem as SysMenuItem).PKNO;
                bool isSuccss = _SDMService.UseService(s => s.AddSysMenuItem(m_SysMenuItem));
            }
            List<SysMenuItem> m_SysMenuItems =
                _SDMService.UseService(s => s.GetSysMenuItems("TARGET_NAME = 'WPF'")).OrderBy(c => c.ITEM_SEQ).ToList();
            this.treeList.ItemsSource = m_SysMenuItems;
           
        }

        private void MenuDelete_Click(object sender, RoutedEventArgs e)
        {
            if (this.treeList.SelectedItem!=null)
            {
                SysMenuItem m_SysMenuItem = this.treeList.SelectedItem as SysMenuItem;
                List<SysMenuItem> d_SysMenuItems = _SDMService.UseService(s => s.GetSysMenuItems("")).
                    Where(c => c.PARENT_PKNO == m_SysMenuItem.PKNO).ToList();

                if (WPFMessageBox.ShowConfirm("是否删除该菜单与子项？", "删除菜单") != WPFMessageBoxResult.OK) return;

                foreach (SysMenuItem item in d_SysMenuItems)
                {
                    _SDMService.UseService(s => s.DelSysMenuItem(item.PKNO));
                }
                bool isSuccss = _SDMService.UseService(s => s.DelSysMenuItem(m_SysMenuItem.PKNO));
                if (isSuccss)
                {
                    System.Windows.Forms.MessageBox.Show("删除完成。");
                }
            }
            List<SysMenuItem> m_SysMenuItems =
                _SDMService.UseService(s => s.GetSysMenuItems("TARGET_NAME = 'WPF'")).OrderBy(c => c.ITEM_SEQ).ToList();
            this.treeList.ItemsSource = m_SysMenuItems;
        }

        private void treeList_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            if (this.treeList.SelectedItem != null && this.treeList.SelectedItem.ToString() != "False" &&
                this.treeList.SelectedItem.ToString() != "True")
            {
                gbMenuContent.DataContext = this.treeList.SelectedItem as SysMenuItem;
                if (
                    _SDMService.UseService(
                        s =>
                            s.GetSysMenuItems("TARGET_NAME = 'WPF'")
                                .Where(c => c.PKNO == (this.treeList.SelectedItem as SysMenuItem).PARENT_PKNO))
                        .ToList()
                        .Count > 0)
                {
                    SysMenuItem m_SysMenuItem =
                        _SDMService.UseService(
                            s =>
                                s.GetSysMenuItems("TARGET_NAME = 'WPF'")
                                    .Where(c => c.PKNO == (this.treeList.SelectedItem as SysMenuItem).PARENT_PKNO))
                            .ToList()[0];
                    parentname.Text = m_SysMenuItem.ITEM_TITLE;
                }
            }
        }

        private void BarItem_OnItemClick(object sender, RoutedEventArgs e)
        {
            SysMenuItem m_SysMenuItem = gbMenuContent.DataContext as SysMenuItem;
            _SDMService.UseService(s => s.UpdateSysMenuItem(m_SysMenuItem));
        }
    }
}
