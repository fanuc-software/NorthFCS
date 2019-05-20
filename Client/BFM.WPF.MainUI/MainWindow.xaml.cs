using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BFM.Common.Data.EventLogger;
using BFM.Common.Data.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Docking.Base;
using DevExpress.Xpf.Ribbon;
using BFM.ContractModel;
using BFM.Server.DataAsset.SDMService;

namespace BFM.WPF.MainUI
{
	public partial class MainWindow : Window
	{
	    private WcfClient<ISDMService> ws = new WcfClient<ISDMService>();

        public Dictionary<string, Assembly> dicAssemblies { get; set; }
        public MainWindow()
        {
            EventLogger.Setting(true, "BFM_MES3.0", "", 365, -1, -1, 5, "", "");

            InitializeComponent();

            dicAssemblies = new Dictionary<string, Assembly>(0);

			RibbonDefaultPageCategory pageCategory = this.ribbonControl.Categories.FirstOrDefault() as RibbonDefaultPageCategory;
            BuildRibbonMenu(pageCategory);
            
            dockLayoutManager.DockItemClosed -= DockLayoutManager_DockItemClosed;
			dockLayoutManager.DockItemClosed += DockLayoutManager_DockItemClosed;

            dockLayoutManager.DockItemClosing -= DockLayoutManagerOnDockItemClosing;
            dockLayoutManager.DockItemClosing += DockLayoutManagerOnDockItemClosing;
            ShowStartUpPage();
            
		}

	    private void BuildRibbonMenu(RibbonDefaultPageCategory pageCategory)
	    {
	        pageCategory.Pages.Clear();

            List<SysMenuItem> menu = ws.UseService(s => s.GetSysMenuItems("")).OrderBy(c => c.ITEM_SEQ).ToList();
            var roots = menu.Where(s => s.PARENT_PKNO == "0").OrderBy(c => c.ITEM_SEQ).ToList();
	        foreach (var root in roots) //大类，根目录
	        {
                RibbonPage ribbonPage = new RibbonPage();
                ribbonPage.Caption = root.ITEM_TITLE;

                List<string> groups = menu.Where(c => c.PARENT_PKNO == root.PKNO).Select(c => c.ITEM_TYPE).Distinct().ToList();

                foreach (var group in groups)  //增加组
                {
                    RibbonPageGroup ribbonGroup = new RibbonPageGroup();
                    ribbonGroup.Caption = group;

                    var items = menu.Where(c => c.PARENT_PKNO == root.PKNO && c.ITEM_TYPE == group).ToList();

                    foreach (SysMenuItem item in items)
                    {
                        string path="";
                        if(!string.IsNullOrEmpty(item.PAGE_ID))
                        { 
                         string[] count = item.PAGE_ID.Split('.');
                         path = count.Last();
                        }
                        BarButtonItem newBtn = new BarButtonItem()
                        {
                            Content = item.ITEM_TITLE,
                            RibbonStyle = RibbonItemStyles.Large,
                            Description = item.PAGE_ID,
                            Tag = item.ASSEMBLY_NAME,
                      
                           // Glyph = new BitmapImage(new Uri(@"dm.png", UriKind.Relative)),
                            LargeGlyph = new BitmapImage(new Uri(@"images/icon/DocumentManageView.png", UriKind.Relative)),
                            BarItemDisplayMode = BarItemDisplayMode.ContentAndGlyph,   
                        };
                        if (path != "")
                        {
                            string sFilePath = @"images/icon/" + path + ".png";
                            if (File.Exists(sFilePath))
                            {
                                newBtn.Glyph = new BitmapImage(new Uri(sFilePath, UriKind.Relative));
                                newBtn.LargeGlyph = new BitmapImage(new Uri(sFilePath, UriKind.Relative));
                            }
                        }
                        newBtn.ItemClick += MainWindow_ItemClick;
                        ribbonGroup.Items.Add(newBtn);
                    }

                    ribbonPage.Groups.Add(ribbonGroup);
                }

                pageCategory.Pages.Add(ribbonPage);
            }
        }

		private void MainWindow_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (e.Item.Tag == null) return;
			string assembly = e.Item.Tag.ToString();
			string fulName = e.Item.Description;
			if (string.IsNullOrEmpty(fulName) || string.IsNullOrEmpty(assembly)) return;

			CreateAndShowPanel(assembly, fulName, e.Item.Content.ToString());
		}

		private void CreateAndShowPanel(object obj)
		{
			string[] data = obj.ToString().Split('|');
			if (data.Length != 3) return;

			string assName = data[0];
			string fullName = data[1];
			string title = data[2];

			CreateAndShowPanel(assName, fullName, title);
		}

		public void CreateAndShowPanel(string assName, string fullName, string title)
		{
			Assembly assem = Assembly.Load(assName);
			Type type = assem.GetType(fullName);

			DocumentGroup docGroup = null;
			DocumentPanel panelFunc = null;
			if (layoutGroup.Items.Any())
			{
				docGroup = layoutGroup.Items.FirstOrDefault() as DocumentGroup;
				panelFunc = docGroup.Items.FirstOrDefault(p => p.Tag.ToString() == fullName) as DocumentPanel;
			}
			else
			{
				docGroup = new DocumentGroup();
                docGroup.ShowCloseButton = true;
				docGroup.ClosePageButtonShowMode = ClosePageButtonShowMode.InAllTabPagesAndTabControlHeader;
                docGroup.SelectedItemChanged -= DocGroup_SelectedItemChanged;
                docGroup.SelectedItemChanged += DocGroup_SelectedItemChanged;
                layoutGroup.Items.Add(docGroup);
			}

		    if (panelFunc == null)
		    {
		        Page page = ShowCurrentPage(assName, fullName);
		        if (page == null)
		        {
		            return;
		        }
		        panelFunc = new DocumentPanel()
		        {
		            Caption = title,
		            CaptionAlignMode = CaptionAlignMode.Custom,
		            CaptionWidth = 180,
                    AllowClose = true,
		            Tag = page,
		            Content = page.Content,
		        };
                page.Tag = panelFunc;
		        docGroup.Items.Add(panelFunc);
		    }

		    panelFunc.IsActive = true;
		}

		private Page ShowCurrentPage(string assName, string fullName)
		{
			Assembly assembly = null;
			if (dicAssemblies.ContainsKey(assName))
			{
                assembly = dicAssemblies[assName];
			}
			else
			{
                assembly = Assembly.Load(assName);
			}
            Page obj = (Page)assembly.CreateInstance(fullName, true); // 创建类的实例，返回为 object 类型
		    return obj;
		}

	    private void DockLayoutManagerOnDockItemClosing(object sender, ItemCancelEventArgs e)
	    {
	        #region 提示是否可以关闭
            //如果窗体正在编辑，则不能关闭
	        //e.Cancel = true;

	        #endregion
	    }

	    private void DockLayoutManager_DockItemClosed(object sender, DockItemClosedEventArgs e)
		{

		}

		public void ShowStartUpPage()
		{
			DocumentGroup docGroup = null;
			DocumentPanel panelFunc = null;
			if (layoutGroup.Items.Any())
			{
				docGroup = layoutGroup.Items.FirstOrDefault() as DocumentGroup;
				panelFunc = docGroup.Items.FirstOrDefault(p => p.Tag.ToString() == "StartUpPage") as DocumentPanel;
			}
			else
			{
				docGroup = new DocumentGroup();
				docGroup.SelectedItemChanged -= DocGroup_SelectedItemChanged;
				docGroup.SelectedItemChanged += DocGroup_SelectedItemChanged;
				docGroup.ShowCloseButton = true;
				docGroup.ClosePageButtonShowMode = ClosePageButtonShowMode.InAllTabPagesAndTabControlHeader;
				layoutGroup.Items.Add(docGroup);
			}

			if (panelFunc == null)
            {
                Page page = new Desktop();
                panelFunc = new DocumentPanel()
			    {
                    Caption = "系统首页",
                    CaptionAlignMode = CaptionAlignMode.Custom,
                    CaptionWidth = 180,
                    AllowClose = false,
                    Tag = page,
                    Content = page.Content,
                };
                page.Tag = panelFunc;
                docGroup.Items.Add(panelFunc);
			}

			panelFunc.IsActive = true;
		}

		private void DocGroup_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
		{
			if (ribbonControl.Categories.Count > 1)
			{
				ribbonControl.Categories.RemoveAt(1);
			}

			DocumentPanel panel = e.Item as DocumentPanel;
			if (panel == null) return;

            Page win = panel.Tag as Page;
			if (win == null) return;
            
            #region 合并ribbon菜单

            RibbonControl ribCtor = win.FindName("ribbonControl") as RibbonControl;
			if (ribCtor != null)
			{
				RibbonDefaultPageCategory pageCategory = ribCtor.Categories.FirstOrDefault() as RibbonDefaultPageCategory;
				RibbonPageCategory ribPageCategory = new RibbonPageCategory();
				ribPageCategory.Color = Colors.Orange;
				ribPageCategory.Caption = pageCategory.Caption;
				ribbonControl.Categories.Add(ribPageCategory);

				foreach (RibbonPage page in pageCategory.Pages)
				{
					RibbonPage newPage = new RibbonPage();
					foreach (RibbonPageGroup group in page.Groups)
					{
						RibbonPageGroup newGroup = new RibbonPageGroup() {Caption = group.Caption};
						foreach (var item in group.Items)
						{
							if (item is BarButtonItem)
							{
								BarButtonItem btn = item as BarButtonItem;
							    BarButtonItem newBtn = new BarButtonItem()
							    {
                                    Content = btn.Content,
                                    RibbonStyle = btn.RibbonStyle,
                                    Glyph = btn.Glyph,
                                    LargeGlyph = btn.LargeGlyph,
                                    KeyGesture = btn.KeyGesture,
                                };
                                newBtn.ItemClick += (s, arg) =>
                                {
                                    btn.Focusable = true;
                                    btn.Focus();       //TextBox 失去焦点后才反馈给绑定值
                                    btn.PerformClick();
                                };

                                Binding binding = new Binding() {Source = btn, Path = new PropertyPath("IsEnabled")};
							    BindingOperations.SetBinding(newBtn, IsEnabledProperty, binding);

                                binding = new Binding() { Source = btn, Path = new PropertyPath("Visibility") };
                                BindingOperations.SetBinding(newBtn, VisibilityProperty, binding);

                                newGroup.Items.Add(newBtn);
							}
						}
						newPage.Groups.Add(newGroup);
					}

					newPage.Caption = "工作栏";
					newPage.IsSelected = true;
					ribPageCategory.Pages.Add(newPage);
				}

				ribPageCategory.IsVisible = true;
			}

			ribbonControl.UpdateLayout();

            #endregion
        }
    }
}
