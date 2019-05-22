using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
namespace BFM.WPF.SHWMS.ViewModel
{
    public enum JobWorkEnum
    {
        Error,
        Success
    }
    public class MainJobViewModel : ViewModelBase
    {

        public ObservableCollection<OrderViewModel> OrderNodes { get; set; } = new ObservableCollection<OrderViewModel>();

        public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();

        public event Action<JobWorkEnum, string> JobOperationEvent;

        public event Action<OrderViewModel> StartJobEvent;

        public ICommand AddCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var items = OrderItems.Where(d => d.Count > 0).ToList();
                    if (items.Count == 0)
                    {
                        JobOperationEvent?.Invoke(JobWorkEnum.Error, "请选择要加工的产品！");
                        return;
                    }
                    var order = new OrderViewModel()
                    {
                        CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        Sate = OrderStateEnum.Create,
                        OrderID = Guid.NewGuid().ToString()
                    };
                    order.OrderCommandEvent += Order_OrderCommandEvent;
                    OrderNodes.Add(order);
                    items.ForEach(d =>
                    {
                        order.Items.Add(d.Clone() as OrderItemViewModel);
                    });
                    order.ToString();

                    JobOperationEvent?.Invoke(JobWorkEnum.Success, "订单添加成功！");
                    OrderItems.ForEach(d => d.Init());
                });
            }
        }

        private void Order_OrderCommandEvent(OrderCommandEnum arg1, OrderViewModel arg2)
        {
            if (arg1 == OrderCommandEnum.Remove)
            {
                OrderNodes.Remove(arg2);
                return;
            }
            if (arg1 == OrderCommandEnum.Start)
            {
                StartJobEvent?.Invoke(arg2);

            }
        }

        public MainJobViewModel()
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
