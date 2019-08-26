using GalaSoft.MvvmLight.Command;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BFM.WPF.SHWMS.ViewModel.PushOrder
{
    public class PushJobViewModel : BaseJobViewModel<PushOrderViewModel>
    {
        public override ICommand CycleStartCommand => new RelayCommand(CycleStart);

        public ICommand CycleStopCommand => new RelayCommand(CycleStop);
        public override ICommand AddCommand => new RelayCommand(new Action(() => AddOrder(GetOrderViewModel)));

        public override ICommand MachineResetCommand => new RelayCommand(new Action(() => { }));

        public override event Action<JobWorkEnum, string> JobOperationEvent;
        public override event Action<PushOrderViewModel> StartJobEvent;
        public override event Action MachineResetEvent;
        static string host;
        private RedisManagerPool managerPool;
        public event Func<OrderItemViewModel> GetOrderItemEvent;
        string redisChannel = "";

        private AutoResetEvent autoReset = new AutoResetEvent(false);
        static PushJobViewModel()
        {
            host = ConfigurationManager.AppSettings["RedisHost"];
        }
        public PushJobViewModel(string channel)
        {
            redisChannel = channel;
            managerPool = new RedisManagerPool(host);

        }

        private PushOrderViewModel GetMesOrderViewModel(OrderItem item)
        {
            var orderItem = new OrderItemViewModel()
            {

            };
            var order = new PushOrderViewModel()
            {
                CreateTime = DateTime.Now.ToString("HH:mm:ss"),
                Sate = OrderStateEnum.Create,
                Name = "",
                OrderID = Guid.NewGuid().ToString().Substring(0, 6),
                VMOne = new BaseDeviceViewModel() { ID = "Lathe1", IP = "192.168.0.232" },
                Items = new List<OrderItemViewModel>() { orderItem }
            };
            orderItem.MainOrder = order;

            return order;
        }
        private PushOrderViewModel GetOrderViewModel()
        {
            var orderItem = GetOrderItemEvent?.Invoke();
            if (orderItem == null)
            {
                return null;
            }           
            var order = new PushOrderViewModel()
            {
                CreateTime = DateTime.Now.ToString("HH:mm:ss"),
                Sate = OrderStateEnum.Create,
                Name = orderItem.Name,
                OrderID = Guid.NewGuid().ToString().Substring(0, 6),
                VMOne = new BaseDeviceViewModel() { ID = "Lathe1", IP = "192.168.0.232" },
                Items = new List<OrderItemViewModel>() { orderItem }
            };
            orderItem.MainOrder = order;

            return order;
        }
        private void AddOrder(Func<PushOrderViewModel> action)
        {
            var order = action();
            if (order == null)
            {
                return;
            }
            order.OrderCommandEvent += Order_OrderCommandEvent;
            OrderNodes.Add(order);
            
            order.VMOne.Count = order.Items.Sum(d => d.Count);
            try
            {
                using (var redisClient = managerPool.GetClient())
                {
                    var redisTodo = redisClient.As<OrderItem>();

                    redisTodo.Store(new OrderItem()
                    {
                        ActualQuantity = 0,
                        Id = order.OrderID,
                        Quantity = order.Items.Sum(d => d.Count),
                        State = OrderItemStateEnum.NEW,
                        Type = order.Items[0].Type,
                        CreateDateTime = DateTime.Now
                    });
                    redisClient.PublishMessage(redisChannel, order.OrderID);

                }
            }
            catch (Exception ex)
            {

                JobOperationEvent?.Invoke(JobWorkEnum.Error, "Redis 错误！" + ex.Message);
                return;

            }

            JobOperationEvent?.Invoke(JobWorkEnum.Success, "订单添加成功！");
        }

        private void Order_OrderCommandEvent(OrderCommandEnum arg1, BaseOrderViewModel arg2)
        {
            if (arg1 == OrderCommandEnum.Remove)
            {
                OrderNodes.Remove(arg2 as PushOrderViewModel);
                return;
            }
            if (arg1 == OrderCommandEnum.Start)
            {
                CycleStart();
            }
        }


        private void CycleStop()
        {
            using (var redisClient = managerPool.GetClient())
            {
                IRedisSubscription subscription = redisClient.CreateSubscription();
                subscription.UnSubscribeFromChannels(redisChannel);
                autoReset.Set();
            }
        }
        private void CycleStart()
        {
            Task.Factory.StartNew(() =>
            {
                using (var redisClient = managerPool.GetClient())
                {
                    IRedisSubscription subscription = redisClient.CreateSubscription();
                    subscription.OnMessage = (channel, mes) =>
                    {
                        var orderItem = GetOrderItem(mes);
                        var order = OrderNodes.FirstOrDefault(d => d.OrderID == orderItem.Id);
                        if (order != null)
                        {
                            if (orderItem.State == OrderItemStateEnum.DOWORK)
                            {
                                order.StartJob();
                                order.CurrentTotal = orderItem.ActualQuantity;
                                order.Progress = Convert.ToInt32(orderItem.ActualQuantity * 100.0 / order.TotalProgress);
                            }
                            else if (orderItem.State == OrderItemStateEnum.DONE)
                            {
                                order.Sate = OrderStateEnum.Finish;
                                order.Progress = 100;
                            }
                            else if (orderItem.State == OrderItemStateEnum.NEW)
                            {
                                AddOrder(() => GetMesOrderViewModel(orderItem));
                            }
                        }
                    };
                    subscription.SubscribeToChannels(redisChannel);
                    autoReset.WaitOne();
                }

            });


        }


        private OrderItem GetOrderItem(object id)
        {
            using (var redisClient = managerPool.GetClient())
            {
                var high = redisClient.As<OrderItem>();
                return high.GetById(id);

            }

        }

    }
}
