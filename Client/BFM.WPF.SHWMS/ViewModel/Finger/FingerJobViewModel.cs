using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BFM.WPF.SHWMS.ViewModel.Finger
{
    public class FingerJobViewModel : BaseJobViewModel<FingerOrderViewModel>
    {

        public override ICommand AddCommand => new RelayCommand(AddCommandAction);

        public override ICommand CycleStartCommand => new RelayCommand(()=>StartJobEvent?.Invoke(null));

        public override ICommand MachineResetCommand => new RelayCommand(() => MachineResetEvent?.Invoke());

        public override event Action<JobWorkEnum, string> JobOperationEvent;
        public override event Action<FingerOrderViewModel> StartJobEvent;
        public override event Action MachineResetEvent;


        private void AddCommandAction()
        {
            var items = OrderItems.Where(d => d.Count > 0).ToList();
            if (items.Count == 0)
            {

                JobOperationEvent?.Invoke(JobWorkEnum.Error, "请选择要加工的产品！");
                return;
            }
            var order = new FingerOrderViewModel()
            {
                CreateTime = DateTime.Now.ToString("HH:mm:ss"),
                Sate = OrderStateEnum.Create,
                OrderID = Guid.NewGuid().ToString().Substring(0, 6),
                VMOne = new BaseDeviceViewModel() { ID = "Lathe1", IP = "192.168.0.232" },
                LatheTwo = new BaseDeviceViewModel() { ID = "Lathe2", IP = "192.168.0.231", CountPath = 2 }
            };
            order.OrderCommandEvent += Order_OrderCommandEvent; 
            OrderNodes.Add(order);
            items.ForEach(d =>
            {
                var item = d.Clone() as OrderItemViewModel;
                item.MainOrder = order;
                order.Items.Add(item);
            });
            order.VMOne.Count = order.Items.Sum(d => d.Count);
            order.LatheTwo.Count = order.Items.Sum(d => d.Count);
            order.ToString();
            JobOperationEvent?.Invoke(JobWorkEnum.Success, "订单添加成功！");
            OrderItems.ForEach(d => d.Init());
        }

        private void Order_OrderCommandEvent(OrderCommandEnum arg1, BaseOrderViewModel arg2)
        {
            if (arg1 == OrderCommandEnum.Remove)
            {
                OrderNodes.Remove(arg2 as FingerOrderViewModel);
                return;
            }
            if (arg1 == OrderCommandEnum.Start)
            {
                StartJobEvent?.Invoke(arg2 as FingerOrderViewModel);

            }
        }
        public FingerJobViewModel()
        {
            string[] data = { "狗", "猴", "虎", "鸡", "龙", "马", "蛇", "牛", "鼠", "兔", "羊", "猪" };
            data.ToList().ForEach(d =>
            {
                OrderItems.Add(new OrderItemViewModel()
                {

                    Name = d,
                    ItemID = Guid.NewGuid().ToString(),
                    IconPath = $"./SHImage/{d}.png"
                });
            });
        }
    }
}
