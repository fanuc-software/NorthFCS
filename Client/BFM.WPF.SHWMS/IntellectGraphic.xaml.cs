using BFM.WPF.SHWMS.ViewModel;
using BFM.WPF.SHWMS.ViewModel.PushOrder;
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
    /// IntellectGraphic.xaml 智能制造单元
    /// </summary>
    public partial class IntellectGraphic : Page
    {
        PushJobViewModel mainJobViewModel;

        public IntellectGraphic()
        {
            InitializeComponent();
            this.Loaded += IntellectGraphic_Loaded;
        }

        private void IntellectGraphic_Loaded(object sender, RoutedEventArgs e)
        {
            mainJobViewModel = new PushJobViewModel("Intellect");
            mainJobViewModel.StartJobEvent += MainJobViewModel_StartJobEvent;
            mainJobViewModel.JobOperationEvent += MainJobViewModel_JobOperationEvent;
            mainJobViewModel.MachineResetEvent += MainJobViewModel_MachineResetEvent;
            mainJobViewModel.GetOrderItemEvent += MainJobViewModel_GetOrderItemEvent;
            this.DataContext = mainJobViewModel;
        }

        private OrderItemViewModel MainJobViewModel_GetOrderItemEvent()
        {
            var order = new OrderItemViewModel() { IconPath = "六方体", ItemID = new Guid().ToString("N"), Name = "六方体" };
            var orderWindow = new OrderWindow() { ImagePath = "六方体" };
            orderWindow.OrderItemNumEvent += (s, type) =>
            {
                order.Count = s;
                order.Type = type;

            };
            orderWindow.ShowDialog();
            return order.Count > 0 ? order : null;
        }

        private void MainJobViewModel_MachineResetEvent()
        {
        }

        private void MainJobViewModel_JobOperationEvent(JobWorkEnum arg1, string arg2)
        {
        }

        private void MainJobViewModel_StartJobEvent(PushOrderViewModel obj)
        {

        }
    }
}
