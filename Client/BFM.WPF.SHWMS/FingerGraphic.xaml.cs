using BFM.WPF.SHWMS.ViewModel;
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

namespace BFM.WPF.SHWMS
{
    /// <summary>
    /// FingerGraphic.xaml 的交互逻辑
    /// </summary>
    public partial class FingerGraphic : Page
    {
        MainJobViewModel mainJobViewModel;
        public FingerGraphic()
        {
            InitializeComponent();
            mainJobViewModel = new MainJobViewModel();
            this.Loaded += FingerGraphic_Loaded;
            mainJobViewModel.StartJobEvent += MainJobViewModel_StartJobEvent;
            mainJobViewModel.JobOperationEvent += MainJobViewModel_JobOperationEvent;
        }

        private void MainJobViewModel_JobOperationEvent(JobWorkEnum arg1, string arg2)
        {

        }

        private void MainJobViewModel_StartJobEvent(OrderViewModel obj)
        {
        }

        private void FingerGraphic_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = mainJobViewModel;
        }
    }
}
