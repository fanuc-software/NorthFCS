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
        private CancellationTokenSource token;

        public FingerGraphic()
        {
            InitializeComponent();
            mainJobViewModel = new MainJobViewModel();
            jobService = new JobService(mainJobViewModel);
            jobService.TaskJobFinishEvent += JobService_TaskJobFinishEvent;
            this.Loaded += FingerGraphic_Loaded;
            mainJobViewModel.StartJobEvent += MainJobViewModel_StartJobEvent;
            mainJobViewModel.JobOperationEvent += MainJobViewModel_JobOperationEvent;
        }

        private void JobService_TaskJobFinishEvent(string obj)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                btnCycle.IsEnabled = true;

            }));

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
            Dispatcher.BeginInvoke(new Action(() =>
            {
                btnCycle.IsEnabled = false;
                btnCancel.IsEnabled = true;
                btnStopCycle.IsEnabled = true;

            }));
            token = new CancellationTokenSource();
            Task.Factory.StartNew(() => jobService.Start(token), token.Token);
           // Task.Factory.StartNew(() => jobService.TestStart(token), token.Token);
        }

        private void FingerGraphic_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = mainJobViewModel;
            btnStopCycle.IsEnabled = false;
            btnCancel.IsEnabled = false;
        }

        private void BtnStopCycle_Click(object sender, RoutedEventArgs e)
        {
            btnStopCycle.IsEnabled = false;
            jobService.IsCycleStop = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确定所有订单任务吗？", "订单取消", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                btnCancel.IsEnabled = false;
                btnStopCycle.IsEnabled = false;
                token.Cancel();
            }

        }

    }
}
