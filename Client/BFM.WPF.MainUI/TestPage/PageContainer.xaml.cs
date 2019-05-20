using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace BFM.WPF.MainUI
{
    /// <summary>
    /// 管理类界面的主窗体
    /// </summary>
    public partial class PageContainer : Page
    {
        public PageContainer()
        {
            InitializeComponent();

        }
        public PageContainer(string PagView, string PagEdit)
        {
            InitializeComponent();
            GetPage(PagView, PagEdit);
        }

        public void GetPage(string PagView, string PagEdit)
        {
            //PagView = "BFM.WPF.SDM.Purview.test1";
            Assembly assembly = Assembly.GetExecutingAssembly();
            dynamic obj = assembly.CreateInstance(PagView);
            dynamic obj2 = assembly.CreateInstance(PagEdit);
            List<UserControl> m_UserControl = new List<UserControl>();
            m_UserControl.Add(obj);
            m_UserControl.Add(obj2);
            this.flipView.ItemsSource = m_UserControl;
        }

        private void BarButtonItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

        }
    }
}
