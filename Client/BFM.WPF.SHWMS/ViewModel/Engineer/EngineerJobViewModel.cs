using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BFM.WPF.SHWMS.ViewModel.Engineer
{
    public class EngineerJobViewModel : BaseJobViewModel<EngineerOrderViewModel>
    {
        public override ICommand CycleStartCommand => new RelayCommand(() => StartJobEvent?.Invoke(null));

        public override ICommand AddCommand => new RelayCommand(AppendOrder);

        public override ICommand MachineResetCommand => new RelayCommand(() => MachineResetEvent?.Invoke());

        public override event Action<JobWorkEnum, string> JobOperationEvent;
        public override event Action<EngineerOrderViewModel> StartJobEvent;
        public override event Action MachineResetEvent;

        public event Func<OrderItemViewModel> GetOrderItemEvent;

        private void AppendOrder()
        {
            var orderItem = GetOrderItemEvent?.Invoke();
            if (orderItem == null)
            {
                return;
            }

            var order = new EngineerOrderViewModel()
            {
                CreateTime = DateTime.Now.ToString("HH:mm:ss"),
                Sate = OrderStateEnum.Create,
                Name = orderItem.Name,
                OrderID = Guid.NewGuid().ToString().Substring(0, 6),
                VMOne = new BaseDeviceViewModel() { ID = "Lathe1", IP = "192.168.0.232" },
                Items = new List<OrderItemViewModel>() { orderItem }
            };
            orderItem.MainOrder = order;
            order.OrderCommandEvent += Order_OrderCommandEvent;
            OrderNodes.Add(order);
            order.VMOne.Count = order.Items.Sum(d => d.Count);
            JobOperationEvent?.Invoke(JobWorkEnum.Success, "订单添加成功！");
        }

        private void Order_OrderCommandEvent(OrderCommandEnum arg1, BaseOrderViewModel arg2)
        {
            if (arg1 == OrderCommandEnum.Remove)
            {
                OrderNodes.Remove(arg2 as EngineerOrderViewModel);
                return;
            }
            if (arg1 == OrderCommandEnum.Start)
            {
                StartJobEvent?.Invoke(arg2 as EngineerOrderViewModel);

            }
        }
    }
}
