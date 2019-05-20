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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BFM.Common.Base.Utils;
using BFM.Common.Data.PubData;
using BFM.Server.DataAsset.PLMService;
using BFM.Server.DataAsset.RSMService;
using BFM.Server.DataAsset.SDMService;
using BFM.WPF.Report;
using BFM.WPF.Report.PPM;
using BFM.ContractModel;
using BFM.WPF.Base.Helper;

namespace BFM.WPF.PPM
{
    /// <summary>
    /// TaskLineSearch.xaml 的交互逻辑
    /// </summary>
    public partial class TaskLineSearch : Page
    {
        private WcfClient<IPLMService> ws = new WcfClient<IPLMService>();
        private WcfClient<IRSMService> wsRsm = new WcfClient<IRSMService>();
        private WcfClient<ISDMService> wsSdm = new WcfClient<ISDMService>();

        public TaskLineSearch()
        {
            InitializeComponent();

            GetPage();
        }

        private void GetPage()
        {
            List<PmTaskLine> taskLines =
                ws.UseService(s => s.GetPmTaskLines("USE_FLAG = 1")).OrderBy(c => c.CREATION_DATE).ToList(); 
            gridItem.ItemsSource = taskLines;

            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }
    }
}
