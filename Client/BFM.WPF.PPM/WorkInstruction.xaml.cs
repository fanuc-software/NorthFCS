using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using System.Windows.Shapes;
using BFM.Common.Base;
using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset;
using BFM.Server.DataAsset.EAMService;
using BFM.Server.DataAsset.PLMService;
using BFM.Server.DataAsset.RSMService;
using BFM.Server.DataAsset.SDMService;
using BFM.WPF.Base;
using BFM.WPF.Base.Controls;

namespace BFM.WPF.PPM
{
    /// <summary>
    /// PlanMasterMangEdit.xaml 的交互逻辑
    /// </summary>
    public partial class WorkInstruction : Window
    {     
        private WcfClient<IPLMService> ws = new WcfClient<IPLMService>();
        private WcfClient<IRSMService> ws2 = new WcfClient<IRSMService>();
       
        public WorkInstruction(PmTaskLine master)
        {
            InitializeComponent();         
            BitmapImage nBitmapImage = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\images\\手机壳.png", UriKind.RelativeOrAbsolute));
            textImage.Source = nBitmapImage;
            if (!string.IsNullOrEmpty(master.ITEM_PKNO))
            {
                if (File.Exists(Environment.CurrentDirectory + "\\images\\" + master.ITEM_PKNO))
                {
                    nBitmapImage = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\images\\" + master.ITEM_PKNO, UriKind.RelativeOrAbsolute));
                    textImage.Source = nBitmapImage;
                }
            }

            //if (nBitmapImage != null)
            //{
            //    textImage.Source = nBitmapImage;
            //}

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
          
        
           
        }

        private void treeList_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {

        }
    }
}
