using BFM.WPF.SHWMS.Service;
using BFM.WPF.SHWMS.ViewModel;
using System;
using System.Collections.Generic;
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
        JobService jobService;
        public FingerGraphic()
        {
            InitializeComponent();
            mainJobViewModel = new MainJobViewModel();
            jobService = new JobService(mainJobViewModel);
            this.Loaded += FingerGraphic_Loaded;
            mainJobViewModel.StartJobEvent += MainJobViewModel_StartJobEvent;
            mainJobViewModel.JobOperationEvent += MainJobViewModel_JobOperationEvent;
        }

        private void MainJobViewModel_JobOperationEvent(JobWorkEnum arg1, string arg2)
        {
            infoLabel.Content = arg2;
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.AutoReset = false;
            timer.Enabled = true;
            timer.Interval = 2000;
            timer.Elapsed += (s, e) => { Dispatcher.BeginInvoke(new Action(() => infoLabel.Content = "")); };
            timer.Start();
        }

        private void MainJobViewModel_StartJobEvent(OrderViewModel obj)
        {
        }

        private void FingerGraphic_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = mainJobViewModel;
            Task.Factory.StartNew(() => jobService.Start());
        }
    }
}
