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
using BFM.Common.Base;
using BFM.ContractModel;
using BFM.Server.DataAsset;
using DevExpress.Utils.Serializing.Helpers;

namespace BFM.WPF.MainUI
{
    /// <summary>
    /// TestWebApi.xaml 的交互逻辑
    /// </summary>
    public partial class TestWebApi : UserControl
    {
        private TestEFCodeFirstF testEfCodeFirstF = new TestEFCodeFirstF();
        private SysMenuItemF sysMenuItemF = new SysMenuItemF();
        public TestWebApi()
        {
            InitializeComponent();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            SysMenuItem m_SysUser = (await sysMenuItemF.GetById(tbIn.Text));

            tbResult.Text = SafeConverter.JsonSerializeObject(m_SysUser);
        }

        private async void btnGetPage_Click(object sender, RoutedEventArgs e)
        {
            //GetPage
            List<SysMenuItem> m_SysMenuItem = (await sysMenuItemF.GetPageData(10, 1, true, "CREATION_DATE", tbIn.Text));
            tbResult.Text = SafeConverter.JsonSerializeObject(m_SysMenuItem);
        }

        private async void btnCount_Click(object sender, RoutedEventArgs e)
        {
            //GetCount
            int i = (await sysMenuItemF.GetCount(tbIn.Text));
            tbResult.Text = SafeConverter.JsonSerializeObject(i);
        }

        private async void button2_Click(object sender, RoutedEventArgs e)
        {
            //GetByParam
            List<SysMenuItem> m_SysMenuItem = (await sysMenuItemF.GetByParam(tbIn.Text));
            tbResult.Text = SafeConverter.JsonSerializeObject(m_SysMenuItem);
        }

        private async void button3_Click(object sender, RoutedEventArgs e)
        {
            //Add
            TestEFCodeFirst test = new TestEFCodeFirst()
            {
                PKNO = tbIn.Text,
                USER_NAME = "Test1",
                PASSWORD = "1234",
            };

            string value = SafeConverter.JsonSerializeObject(test);
            TestEFCodeFirst test2 = SafeConverter.JsonDeserializeObject<TestEFCodeFirst>(value);

            bool m_SysUser = (await testEfCodeFirstF.Add(test));

            tbResult.Text = SafeConverter.JsonSerializeObject(m_SysUser);
        }

        private async void button4_Click(object sender, RoutedEventArgs e)
        {
            //DelById
            bool m_SysUser = (await testEfCodeFirstF.Delete(tbIn.Text));

            tbResult.Text = SafeConverter.JsonSerializeObject(m_SysUser);
        }

    }
}
