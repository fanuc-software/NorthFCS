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
using BFM.Server.DataAsset.PLMService;

namespace BFM.WPF.MainUI
{
    /// <summary>
    /// TestMVVM.xaml 的交互逻辑
    /// </summary>
    public partial class TestMVVM : Page
    {
        public TestMVVM()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WcfClient<IPLMService> ws = new WcfClient<IPLMService>();
            List<MesJobOrder> productProcesses = ws.UseService(s =>
                s.GetMesJobOrders(""));  //正在执行的产品信息
            MesJobOrder productProcess = productProcesses.FirstOrDefault();  //产品生产情况;

            productProcess.PKNO = "";

        }
    }
}
