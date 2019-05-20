using DevExpress.Xpf.Accordion;
using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using BFM.Common.Base;
using BFM.Common.Base.Log;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.Data.PubData;
using BFM.Server.DataAsset.SDMService;
using BFM.ContractModel;
using BFM.WPF.Base.ControlStyles;
using BFM.WPF.MainUI.VersionCtrol;
using DevExpress.Printing.Core;

namespace BFM.WPF.MainUI
{
    /// <summary>
    /// NewMainForm.xaml 的交互逻辑
    /// </summary>
    public partial class NewMainForm : Window
    {
        private WcfClient<ISDMService> ws = new WcfClient<ISDMService>();
        public Dictionary<string, Assembly> dicAssemblies { get; set; }


        public NewMainForm()
        {
            InitializeComponent();
            this.Loaded += NewMainForm_Loaded;
           
            dicAssemblies = new Dictionary<string, Assembly>(0);
            initAccordion();

            tbAppTitle.Text = Configs.GetValue("AppTitle");
            tbMainFormTitle.Text = Configs.GetValue("MainFormTitle");

            CBaseData.MainWindow = this;  //主窗体
        }

        private void NewMainForm_Loaded(object sender, RoutedEventArgs e)
        {
           // DXSplashScreen.Close();
        }

        public void initAccordion()
        {
            accordion.Items.Clear();
            List<SysMenuItem> menu = ws.UseService(s => s.GetSysMenuItems("TARGET_NAME = 'WPF'")).OrderBy(c => c.ITEM_SEQ).ToList();

            var roots = menu.Where(s => s.PARENT_PKNO == "0").OrderBy(c => c.ITEM_SEQ).ToList();
            foreach (SysMenuItem item in roots)
            {
                AccordionItem accordionitem = new AccordionItem();
                accordionitem.Tag = item;
                accordionitem.Header = item.ITEM_TITLE;
                //accordionitem.Glyph = new BitmapImage(new Uri(@"images/icon/DocumentManageView.png", UriKind.Relative));
                var childitem = menu.Where(s => s.PARENT_PKNO == item.PKNO).OrderBy(c => c.ITEM_SEQ).ToList();
                foreach (var iitem in childitem)
                {
                    AccordionItem child_accordionitem = new AccordionItem();
                    child_accordionitem.Tag = iitem;
                    child_accordionitem.Header = iitem.ITEM_TITLE;
                    accordionitem.Items.Add(child_accordionitem);

                    string path = "";
                    if (!string.IsNullOrEmpty(iitem.PAGE_ID))
                    {
                        string[] count = iitem.PAGE_ID.Split('.');
                        path = count.Last();
                    }

                    string sFilePath = @"/BFM.WPF.MainUI;component/images/icon/" +
                                       (string.IsNullOrEmpty(path) ? "default" : path) + ".png";

                    try
                    {
                        Application.GetResourceStream(new Uri(sFilePath, UriKind.Relative));
                    }
                    catch
                    {
                        sFilePath = @"/BFM.WPF.MainUI;component/images/icon/default.png";
                    }

                    child_accordionitem.Glyph = new BitmapImage(new Uri(sFilePath, UriKind.Relative));


                    child_accordionitem.PreviewMouseDown += Child_accordionitem_PreviewMouseDown;
                }
                accordion.Items.Add(accordionitem);
            }
        }

        private void AddIndexPage()
        {
            IndexPage index = new IndexPage();
            Frame framItem = new Frame();
            framItem.Navigate(index);
            DXTabItem m_TabItem = new DXTabItem()
            {
                Content = index,
                Header = "首页",
            };
            
            tab_Control.Items.Add(m_TabItem);
            tab_Control.SelectedIndex = tab_Control.Items.Count - 1;
        }

        private void Child_accordionitem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            AccordionItem m_AccordionItem = sender as AccordionItem;
            SysMenuItem m_SysMenuItem = m_AccordionItem.Tag as SysMenuItem;

            string assembly = m_SysMenuItem.ASSEMBLY_NAME;
            string fulName = m_SysMenuItem.PAGE_ID;
            if (string.IsNullOrEmpty(fulName) || string.IsNullOrEmpty(assembly)) return;

            CreateAndShowPanel(assembly, fulName, m_SysMenuItem.ITEM_TITLE);
        }
        
        public void CreateAndShowPanel(string assName, string fullName, string title)
        {
            foreach (DXTabItem item in tab_Control.Items)
            {
                if (title != null && item.Header == title)
                {
                    tab_Control.SelectedItem = item;
                    return;
                }
            }

            this.Cursor = Cursors.Wait;

            Page page = ShowCurrentPage(assName, fullName);
            if (page == null)
            {
                this.Cursor = Cursors.Arrow;
                return;
            }

            Frame fram_item = new Frame();
            fram_item.Navigate(page);
            DXTabItem m_TabItem = new DXTabItem();
            m_TabItem.Content = page;
            m_TabItem.Header = title;
            tab_Control.Items.Add(m_TabItem);
            tab_Control.SelectedIndex = tab_Control.Items.Count - 1;

            this.Cursor = Cursors.Arrow;
        }

        private Page ShowCurrentPage(string assName, string fullName)
        {
            try
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


                Page obj = null;
                try
                {
                    obj = assembly.GetType(fullName).InvokeMember("GetInstance",
                        BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod, null, null,
                        null) as Page;
                }
                catch
                {
                    obj = null;
                }
                if (obj == null) obj = (Page)assembly.CreateInstance(fullName, true); // 创建类的实例，返回为 object 类型
                return obj;
            }
            catch (Exception ex)
            {
                NetLog.Error("MainForm.ShowCurrentPage", ex);
                return null;
            }
        }

        #region 窗口操作

        private void BtMin_OnClick(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtMax_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            SetWindowState(WindowState);
        }

        private void SetWindowState(WindowState state)
        {

            BtMax.Visibility = state == WindowState.Maximized ? Visibility.Collapsed : Visibility.Visible;
            BtRestore.Visibility = state == WindowState.Normal ? Visibility.Collapsed : Visibility.Visible;

        }

        private void BtClose_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); //系统退出，App中能够捕捉到
            //Environment.Exit(0);  //不友好的退出，建议不用
        }

        public override void OnApplyTemplate()
        {
            RootFrame.MaxWidth = SystemParameters.WorkArea.Width;
            RootFrame.MaxHeight = SystemParameters.WorkArea.Height;
        }

        #endregion
        
    }
}
