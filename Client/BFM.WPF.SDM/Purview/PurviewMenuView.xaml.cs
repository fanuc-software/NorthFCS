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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.SDMService;

namespace BFM.WPF.SDM.Purview
{


    /// <summary>
    /// PurviewMenuView.xaml 的交互逻辑
    /// </summary>
    public partial class PurviewMenuView : Page
    {
        private WcfClient<ISDMService> _SDMService;
        List<SysMenuItem> m_SysMenuItems;
        public PurviewMenuView()
        {
            InitializeComponent();
            Initialize();
        }

        public void Initialize()
        {
            _SDMService = new WcfClient<ISDMService>();
            List<SysPurview> m_SysPurviews = _SDMService.UseService(s => s.GetSysPurviews(""));
            this.GridControl1.ItemsSource = m_SysPurviews;
            _SDMService = new WcfClient<ISDMService>();
            m_SysMenuItems = _SDMService.UseService(s => s.GetSysMenuItems(""));
            this.treeList.ItemsSource = m_SysMenuItems;
        }

        /// <summary>
        /// 递归获取checked状态
        /// </summary>
        /// <param name="node"></param>
        /// <param name="m_SysMenuPurviews"></param>
        public void LoopTreeNodes(TreeListNode node, List<SysMenuPurview> m_SysMenuPurviews)
        {
            if (node == null) return;
            foreach (TreeListNode _childNode in node.Nodes )
            {
                SysMenuItem t_SysMenuItem = _childNode.Content as SysMenuItem;


                if (m_SysMenuPurviews.Where(c => c.MENU_ITEM_PKNO == t_SysMenuItem.PKNO).ToList().Count > 0)
                {
                    _childNode.IsChecked = true;

                }
                else
                {
                    _childNode.IsChecked = false;
                }
                LoopTreeNodes(_childNode, m_SysMenuPurviews);
            }
        }

        /// <summary>
        ///  递归保存
        /// </summary>
        /// <param name="node"></param>
        /// <param name="m_SysMenuPurviews"></param>
        public void SaveLoopTreeNodes(TreeListNode node, List<SysMenuPurview> m_SysMenuPurviews, SysPurview m_SysPurview)
        {
            //todo: 采用之前有权限->无权限则删除，无权限->有权限添加，其他不动作。
            if (node == null) return;
            List<string> delitem = new List<string>();
            //foreach (var item in m_SysMenuPurviews)
            //{
            //    delitem.Add(item.PKNO);
            //}
            //_SDMService.UseService(s => s.DelSysMenuPurviews(delitem));

            foreach (var menuItem in m_SysMenuItems)
            {
                //if (menuItem.IsChecked) //有权限
                {
                    if (m_SysMenuPurviews.Where(c => c.MENU_ITEM_PKNO == menuItem.PKNO).ToList().Count == 0)
                    {
                        SysMenuPurview a_SysMenuPurview = new SysMenuPurview()
                        {
                            PKNO = Guid.NewGuid().ToString("N"),
                            MENU_ITEM_PKNO = menuItem.PKNO,
                            PURVIEW_PKNO = m_SysPurview.PKNO,
                        };
                        _SDMService.UseService(s => s.AddSysMenuPurview(a_SysMenuPurview));
                    }
                }
                //else
                //{
                //    SysMenuPurview delMenuPurview =
                //        m_SysMenuPurviews.FirstOrDefault(c => c.MENU_ITEM_PKNO == menuItem.PKNO);

                //    if (delMenuPurview != null)
                //    {
                //        delitem.Add(delMenuPurview.PKNO);
                //    }
                //}
            }

            if (delitem.Count > 0)
            {
                _SDMService.UseService(s => s.DelSysMenuPurviews(delitem));
            }

            //foreach (TreeListNode _childNode in node.Nodes)
            //{
            //    if (_childNode.IsChecked==true)
            //    {
            //        SysMenuItem t_SysMenuItem = _childNode.Content as SysMenuItem;
            //        SysMenuPurview a_SysMenuPurview = new SysMenuPurview();
            //        a_SysMenuPurview.PKNO = Guid.NewGuid().ToString("N");
            //        a_SysMenuPurview.MENU_ITEM_PKNO = t_SysMenuItem.PKNO;
            //        a_SysMenuPurview.PURVIEW_PKNO = m_SysPurview.PKNO;
            //        _SDMService.UseService(s => s.AddSysMenuPurview(a_SysMenuPurview));
            //    }
             
            //    SaveLoopTreeNodes(_childNode, m_SysMenuPurviews, m_SysPurview);
            //}
        }
        private void GridControl1_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            SysPurview m_SysPurview = e.NewItem as SysPurview;
            if (m_SysPurview==null)
            {
                return;
            }

             List<SysMenuPurview> m_SysMenuPurviews= _SDMService.UseService(s => s.GetSysMenuPurviews("")).Where(s=>s.PURVIEW_PKNO== m_SysPurview.PKNO).ToList();
            if (this.treeList.View.Nodes.Count == 0) return;
            
            foreach (var menuItem in m_SysMenuItems)
            {
                //menuItem.IsChecked = m_SysMenuPurviews.Where(c => c.MENU_ITEM_PKNO == menuItem.PKNO).ToList().Count > 0;
            }
            //LoopTreeNodes(this.treeList.View.Nodes[0], m_SysMenuPurviews);
        }

        private void BarItem_OnItemClick(object sender, RoutedEventArgs e)
        {
            //
            SysPurview m_SysPurview = this.GridControl1.SelectedItem as SysPurview;
            List<SysMenuPurview> m_SysMenuPurviews = _SDMService.UseService(s => s.GetSysMenuPurviews("")).Where(s => s.PURVIEW_PKNO == m_SysPurview.PKNO).ToList();
            SaveLoopTreeNodes(this.treeList.View.Nodes[0], m_SysMenuPurviews, m_SysPurview);
            //
        }

    
    }
}
