using BFM.WPF.SHWMS.Service;
using BFM.WPF.SHWMS.ViewModel;
using BFM.WPF.SHWMS.ViewModel.Engineer;
using BFM.WPF.SHWMS.ViewModel.Finger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BFM.WPF.SHWMS
{
    /// <summary>
    /// 3C制造单元
    /// </summary>
    public partial class EngineerGraphic : Page
    {
        EngineerJobViewModel mainJobViewModel;
        JobService<EngineerOrderViewModel, Generate3CMachiningTask> jobService;
        private CancellationTokenSource token;
        public EngineerGraphic()
        {
            InitializeComponent();
            this.Loaded += EngineerGraphic_Loaded;
        }

        private void EngineerGraphic_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            mainJobViewModel = new EngineerJobViewModel();
            jobService = new JobService<EngineerOrderViewModel, Generate3CMachiningTask>(mainJobViewModel, new Generate3CMachiningTask());
            jobService.TaskJobFinishEvent += JobService_TaskJobFinishEvent;
            jobService.StartMachiningCountEvent += (s) => s.VMOne.StartMachiningCount();
            jobService.StopMachiningCountEvent += (s) => s.VMOne.StopMachiningCount();
            mainJobViewModel.StartJobEvent += MainJobViewModel_StartJobEvent;
            mainJobViewModel.JobOperationEvent += MainJobViewModel_JobOperationEvent;
            mainJobViewModel.MachineResetEvent += MainJobViewModel_MachineResetEvent;
            mainJobViewModel.GetOrderItemEvent += MainJobViewModel_GetOrderItemEvent;
            this.DataContext = mainJobViewModel;
            mainJobViewModel.CycleStart();
        }

        private OrderItemViewModel MainJobViewModel_GetOrderItemEvent()
        {
            var order = new OrderItemViewModel() { IconPath = "六方体", ItemID = new Guid().ToString("N"), Name = "六方体" };
            var orderWindow = new OrderWindow();
            orderWindow.OrderItemNumEvent += (s, type, name) =>
            {
                order.Count = s;
                order.Type = type;
                order.Name = name;
                order.IconPath = name;
            };
            orderWindow.ShowDialog();
            return order.Count > 0 ? order : null;
        }

        private void JobService_TaskJobFinishEvent(string obj)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                btnCycle.IsEnabled = true;

            }));
        }

        private void MainJobViewModel_MachineResetEvent()
        {
            Task.Factory.StartNew(() =>
            {
                var state = jobService.GetJobState();
                if (!state)
                {
                    Dispatcher.BeginInvoke(new Action(() => MessageBox.Show("请先清空产线任务列表")));
                    return;
                }
                jobService.SendJobTaskFinish();
            });

        }

        private void MainJobViewModel_JobOperationEvent(JobWorkEnum arg1, string arg2)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                btnCycle.IsEnabled = true;

            }));
        }

        private void MainJobViewModel_StartJobEvent(EngineerOrderViewModel obj)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                btnCycle.IsEnabled = false;

            }));
            token = new CancellationTokenSource();
            Task.Factory.StartNew(() => jobService.Start(token), token.Token);
        }
    }
}