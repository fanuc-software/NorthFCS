using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using BFM.Common.Data.Utils;
using DevExpress.Utils;
using DevExpress.Utils.Design;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Ribbon;
using BFM.WPF.SDM.Purview;
using BFM.WPF.SDM;
using DevExpress.Xpf.NavBar;
using BFM.ContractModel;
using BFM.Server.DataAsset.SDMService;

namespace BFM.WPF.MainUI
{
    /// <summary>
    /// Interaction logic for MainUI.xaml
    /// </summary>
    public partial class MainUI : Window
    {

        private WcfClient<ISDMService> _wsClient;
        private List<SysMenuItem> sysMenuItems;


        public MainUI()
        {
            InitializeComponent();
            _wsClient = new WcfClient<ISDMService>();
            InitMenu();
        }

        private void InitMenu()
        {
            // ribbonControl.ShowApplicationButton = false;
            //读取菜单表
            sysMenuItems = _wsClient.UseService(s => s.GetSysMenuItems("")).OrderBy(c => c.ITEM_SEQ).ToList();
            var root = sysMenuItems.Where(s => s.PARENT_PKNO == "0").ToList();
            //barContainerControl.FontSize = 60;
            if (root.Count<=0)
            {
                return;
            }
            foreach (var v in root)//主菜单
            {
               
                BarSubItem barButtonItem = new BarSubItem
                {
                    Content = v.ITEM_TITLE,
                    Tag = v.PKNO,
                    Glyph = new BitmapImage(new Uri(@"images/icon/PurviewMenuView.png", UriKind.Relative)),
                    LargeGlyph = new BitmapImage(new Uri(@"images/icon/PurviewMenuView.png", UriKind.Relative)),
                    BarItemDisplayMode =BarItemDisplayMode.ContentAndGlyph
                };

                barButtonItem.ItemClick += BarButtonItem_ItemClick;
                mainMenuControl.Items.Add(barButtonItem);
            }
        }

        private void BarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            navBarControl.Groups.Clear();

            BarButtonItem ss = sender as BarButtonItem;
          
            NavBarGroup navBarGroup = new NavBarGroup
            {
                Header =ss.Content,
            };
            sysMenuItems = _wsClient.UseService(s => s.GetSysMenuItems("")).OrderBy(c => c.ITEM_SEQ).ToList();
          
            var secList = sysMenuItems.Where(s => s.PARENT_PKNO == ss.Tag.ToString()).ToList();//二级菜单

            foreach (var item in secList)
            {
                
                NavBarItem navBarItem = new NavBarItem
                {
                    Content = item.ITEM_TITLE,
                    Tag = item
                };
                navBarItem.Click += NavBarItem_Click;
                navBarGroup.Items.Add(navBarItem);
            }
            navBarControl.Groups.Add(navBarGroup);
        }

        private void NavBarItem_Click(object sender, EventArgs e)
        {
            NavBarItem ss = sender as NavBarItem;

            //消除对tab的多次创建
            foreach (DXTabItem variable in dxTabControl.Items)
            {
                if (ss == null || variable.Header.ToString() != ss.Content.ToString()) continue;
                variable.IsSelected = true;
                return;
            }
            //创建新的TabItem
            DXTabItem dxTabItem = new DXTabItem();
            dxTabItem.Header = ss.Content;
            dxTabItem.IsSelected = true;
            dxTabItem.AllowHide = DefaultBoolean.True;

            SysMenuItem sysMenu = ss.Tag as SysMenuItem;
            dxTabItem.FontSize = 13;
            Frame tabFrame = new Frame();

            //PurviewMenuView menuItem = new PurviewMenuView();
            //tabFrame.Content = menuItem;

            if (string.IsNullOrEmpty(sysMenu?.ASSEMBLY_NAME))
            {
                return;
            }

            Assembly assembly = Assembly.Load(sysMenu.ASSEMBLY_NAME); // 获取当前程序集 
            Page obj = (Page) assembly.CreateInstance(sysMenu.PAGE_ID, true); // 创建类的实例，返回为 object 类型
            if (obj == null)
            {
                return;
            };

            //MenuItemView obj = new MenuItemView();
            tabFrame.Content = obj;
            dxTabItem.Content = tabFrame;
            dxTabControl.Items.Add(dxTabItem);
        }

        private void dxTabControl_TabHidden(object sender, TabControlTabHiddenEventArgs e)
        {
            dxTabControl.RemoveTabItem(e.TabIndex);
        }
    }
}
