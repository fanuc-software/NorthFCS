using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BFM.Common.Data.Utils;
using BFM.WPF.SDM.MenuItem;
using BFM.WPF.SDM.SDMService;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.NavBar;

namespace BFM.WPF.MainUI
{
	public partial class MainWindow : Window
	{
        private WcfClient<ISDMService> _wsClient;
        private List<SysMenuItem> sysMenuItems;

        public MainWindow()
		{
			InitializeComponent();

            InitMenu();
            //SubscribeMediator.Subscribe("CreateAndShowPanel", this.ToString(), CreateAndShowPanel);
            //dicAssemblies = new Dictionary<string, Assembly>(0);

            //RibbonDefaultPageCategory pageCategory = this.ribbonControl.Categories.FirstOrDefault() as RibbonDefaultPageCategory;

            //foreach (var page in pageCategory.Pages)
            //{
            //	foreach (var group in page.Groups)
            //	{
            //		foreach (var item in group.Items)
            //		{
            //			if (item is BarButtonItem)
            //			{
            //				(item as BarButtonItem).ItemClick += MainWindow_ItemClick;
            //			}
            //		}
            //	}
            //}

            //dockLayoutManager.DockItemClosed -= DockLayoutManager_DockItemClosed;
            //dockLayoutManager.DockItemClosed += DockLayoutManager_DockItemClosed;
            //ShowStartUpPage();
		}

        private void InitMenu()
        {
            // ribbonControl.ShowApplicationButton = false;
            //读取菜单表
            sysMenuItems = _wsClient.UseService(s => s.GetAllSysMenuItem("")).ToList();
            var root = sysMenuItems.Where(s => s.PARENT_PKNO == "0").ToList();
            //barContainerControl.FontSize = 60;
            if (root.Count <= 0)
            {
                return;
            }
            foreach (var v in root)//主菜单
            {
                BarSubItem barButtonItem = new BarSubItem();
                barButtonItem.Content = v.ITEM_TITLE;
                barButtonItem.Tag = v.PKNO;

                barButtonItem.ItemClick += BarButtonItem_ItemClick;
                mainMenuControl.Items.Add(barButtonItem);
            }
        }

        private void BarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            navBarControl.Groups.Clear();

            BarButtonItem ss = sender as BarButtonItem;

            NavBarGroup navBarGroup = new NavBarGroup { Header = ss.Content };

            sysMenuItems = _wsClient.UseService(s => s.GetAllSysMenuItem("")).ToList();

            var secList = sysMenuItems.Where(s => s.PARENT_PKNO == ss.Tag.ToString()).ToList();//二级菜单
            foreach (var item in secList)
            {
                NavBarItem navBarItem = new NavBarItem { Content = item.ITEM_TITLE };
                navBarItem.Click += NavBarItem_Click;
                navBarGroup.Items.Add(navBarItem);
            }
            navBarControl.Groups.Add(navBarGroup);
        }

        private void NavBarItem_Click(object sender, System.EventArgs e)
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
            Frame tabFrame = new Frame();
            MenuItemView menuItem = new MenuItemView();
            tabFrame.Content = menuItem;
            dxTabItem.Content = tabFrame;
            dxTabControl.Items.Add(dxTabItem);
        }

        private void dxTabControl_TabHidden(object sender, TabControlTabHiddenEventArgs e)
        {
            dxTabControl.RemoveTabItem(e.TabIndex);
        }
        //private void MainWindow_ItemClick(object sender, ItemClickEventArgs e)
        //{
        //	if (e.Item.Tag == null) return;
        //	string assembly = e.Item.Tag.ToString();
        //	string fulName = e.Item.Description;
        //	if (string.IsNullOrEmpty(fulName) || string.IsNullOrEmpty(assembly)) return;

        //	CreateAndShowPanel(assembly, fulName, e.Item.Content.ToString());
        //}

        //private void CreateAndShowPanel(object obj)
        //{
        //	string[] data = obj.ToString().Split('|');
        //	if (data.Length != 3) return;

        //	string assName = data[0];
        //	string fullName = data[1];
        //	string title = data[2];

        //	CreateAndShowPanel(assName, fullName, title);
        //}

        //public void CreateAndShowPanel(string assName, string fullName, string title)
        //{
        //	Assembly assem = Assembly.Load(assName);
        //	Type type = assem.GetType(fullName);

        //	DocumentGroup docGroup = null;
        //	DocumentPanel panelFunc = null;
        //	if (layoutGroup.Items.Any())
        //	{
        //		docGroup = layoutGroup.Items.FirstOrDefault() as DocumentGroup;
        //		panelFunc = docGroup.Items.FirstOrDefault(p => p.Tag.ToString() == fullName) as DocumentPanel;
        //	}
        //	else
        //	{
        //		docGroup = new DocumentGroup();
        //		docGroup.ShowCloseButton = true;
        //		docGroup.ClosePageButtonShowMode = ClosePageButtonShowMode.InAllTabPagesAndTabControlHeader;
        //		layoutGroup.Items.Add(docGroup);
        //	}

        //	if (panelFunc == null)
        //	{
        //		panelFunc = new DocumentPanel();
        //		panelFunc.Caption = title;
        //		panelFunc.CaptionAlignMode = CaptionAlignMode.Custom;
        //		panelFunc.CaptionWidth = 180;
        //		panelFunc.Tag = fullName;
        //		docGroup.SelectedItemChanged -= DocGroup_SelectedItemChanged;
        //		docGroup.SelectedItemChanged += DocGroup_SelectedItemChanged;
        //		Window win = ShowCurrentPage(assName, fullName);

        //		panelFunc.Content = win.Content;
        //		panelFunc.Tag = win;
        //		win.Tag = panelFunc;
        //		docGroup.Items.Add(panelFunc);
        //	}

        //	panelFunc.IsActive = true;
        //}

        //public Dictionary<string, Assembly> dicAssemblies { get; set; }
        //private Window ShowCurrentPage(string assName, string fullName)
        //{
        //	Assembly currentAssembly = null;
        //	if (dicAssemblies.ContainsKey(assName))
        //	{
        //		currentAssembly = dicAssemblies[assName];
        //	}
        //	else
        //	{
        //		currentAssembly = Assembly.Load(assName);
        //	}

        //	Window content = currentAssembly.GetType(fullName).InvokeMember("GetInstance", BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, null, null, new object[] { }) as Window;
        //	return content;
        //}

        //private void DockLayoutManager_DockItemClosed(object sender, DevExpress.Xpf.Docking.Base.DockItemClosedEventArgs e)
        //{

        //}

        //public void ShowStartUpPage()
        //{
        //	DocumentGroup docGroup = null;
        //	DocumentPanel panelFunc = null;
        //	if (layoutGroup.Items.Any())
        //	{
        //		docGroup = layoutGroup.Items.FirstOrDefault() as DocumentGroup;
        //		panelFunc = docGroup.Items.FirstOrDefault(p => p.Tag.ToString() == "StartUpPage") as DocumentPanel;
        //	}
        //	else
        //	{
        //		docGroup = new DocumentGroup();
        //		docGroup.SelectedItemChanged -= DocGroup_SelectedItemChanged;
        //		docGroup.SelectedItemChanged += DocGroup_SelectedItemChanged;
        //		docGroup.ShowCloseButton = true;
        //		docGroup.ClosePageButtonShowMode = ClosePageButtonShowMode.InAllTabPagesAndTabControlHeader;
        //		layoutGroup.Items.Add(docGroup);
        //	}

        //	if (panelFunc == null)
        //	{
        //		panelFunc = new DocumentPanel();
        //		panelFunc.Caption = "系统首页";
        //		panelFunc.CaptionAlignMode = CaptionAlignMode.Custom;
        //		panelFunc.CaptionWidth = 180;
        //		panelFunc.Tag = "StartUpPage";
        //		Window win = new Desktop();
        //		panelFunc.Content = win.Content;
        //		docGroup.Items.Add(panelFunc);
        //	}

        //	panelFunc.IsActive = true;
        //}

        //private void DocGroup_SelectedItemChanged(object sender, DevExpress.Xpf.Docking.Base.SelectedItemChangedEventArgs e)
        //{
        //	if (ribbonControl.Categories.Count > 1)
        //	{
        //		ribbonControl.Categories.RemoveAt(1);
        //	}

        //	DocumentPanel panel = e.Item as DocumentPanel;
        //	if (panel == null) return;

        //	Window win = panel.Tag as Window;
        //	if (win == null) return;

        //	RibbonControl ribCtor = win.FindName("ribbonControl") as RibbonControl;
        //	if (ribCtor != null)
        //	{
        //		RibbonDefaultPageCategory pageCategory = ribCtor.Categories.FirstOrDefault() as RibbonDefaultPageCategory;
        //		RibbonPageCategory ribPageCategory = new RibbonPageCategory();
        //		ribPageCategory.Color = Colors.Orange;
        //		ribPageCategory.Caption = "操作区域";
        //		ribbonControl.Categories.Add(ribPageCategory);

        //		foreach (RibbonPage page in pageCategory.Pages)
        //		{
        //			RibbonPage newPage = new RibbonPage();
        //			foreach (RibbonPageGroup group in page.Groups)
        //			{
        //				RibbonPageGroup newGroup = new RibbonPageGroup();
        //				foreach (var item in group.Items)
        //				{
        //					if (item is BarButtonItem)
        //					{
        //						BarButtonItem btn = item as BarButtonItem;
        //						BarButtonItem newBtn = new BarButtonItem();
        //						newBtn.Content = btn.Content;
        //						newBtn.RibbonStyle = btn.RibbonStyle;
        //						newBtn.ItemClick += (s, arg) =>
        //						{
        //							btn.PerformClick();
        //						};
        //						newBtn.Glyph = btn.Glyph;
        //						newGroup.Items.Add(newBtn);
        //					}
        //				}
        //				newPage.Groups.Add(newGroup);
        //			}

        //			newPage.Caption = "工作栏";
        //			newPage.IsSelected = true;
        //			ribPageCategory.Pages.Add(newPage);
        //		}

        //		ribPageCategory.IsVisible = true;
        //	}

        //	ribbonControl.UpdateLayout();
        //}

        #region Add by leihj
        private void BtMin_OnClick(object sender, RoutedEventArgs e)
	    {
            this.WindowState = WindowState.Minimized; 
	    }

	    private void BtMax_OnClick(object sender, RoutedEventArgs e)
	    {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
	        BtMax.Visibility = WindowState == WindowState.Maximized ? Visibility.Collapsed : Visibility.Visible;
            BtRestore.Visibility = WindowState == WindowState.Normal ? Visibility.Collapsed : Visibility.Visible;
        }

        private void BtClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            Environment.Exit(0);
        }

        public override void OnApplyTemplate()
        {
            // MaxWidth = SystemParameters.WorkArea.Width;
            //MaxHeight = SystemParameters.WorkArea.Height;
            RootFrame.MaxWidth = SystemParameters.WorkArea.Width;
            RootFrame.MaxHeight = SystemParameters.WorkArea.Height;
        }
        #endregion
    }
}
