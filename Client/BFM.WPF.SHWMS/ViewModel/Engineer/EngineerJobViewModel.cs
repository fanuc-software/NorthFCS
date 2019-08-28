using BFM.WPF.SHWMS.ViewModel.PushOrder;
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

namespace BFM.WPF.SHWMS.ViewModel.Engineer
{
    public class EngineerJobViewModel : BaseJobViewModel<EngineerOrderViewModel>
    {
        private static string host;
        private string redisChannel;
        private RedisManagerPool managerPool;
        private AutoResetEvent autoReset = new AutoResetEvent(false);

        public override ICommand CycleStartCommand => new RelayCommand(() => StartJobEvent?.Invoke(null));

        public override ICommand AddCommand => new RelayCommand(AppendOrder);

        public override ICommand MachineResetCommand => new RelayCommand(() => MachineResetEvent?.Invoke());

        public override event Action<JobWorkEnum, string> JobOperationEvent;
        public override event Action<EngineerOrderViewModel> StartJobEvent;
        public override event Action MachineResetEvent;

        public event Func<OrderItemViewModel> GetOrderItemEvent;
        Dictionary<ProductTypeEnum, string> dict = new Dictionary<ProductTypeEnum, string>();

        static EngineerJobViewModel()
        {
            host = ConfigurationManager.AppSettings["RedisHost"];
        }
        public EngineerJobViewModel()
        {
            redisChannel = "Intellect";
            managerPool = new RedisManagerPool(host);
            dict.Add(ProductTypeEnum.Teacaddy, "茶叶罐");
            dict.Add(ProductTypeEnum.Brakedisc, "刹车盘");
            dict.Add(ProductTypeEnum.Phoneshell, "手机壳");
            dict.Add(ProductTypeEnum.Flange, "法兰盘(加工)");
            dict.Add(ProductTypeEnum.Seal, "图章");
        }
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

        public void CycleStart()
        {
            Task.Factory.StartNew(() =>
            {
                using (var redisClient = managerPool.GetClient())
                {
                    IRedisSubscription subscription = redisClient.CreateSubscription();
                    subscription.OnMessage = (channel, id) =>
                    {
                        string mes = id.Replace("\"", "").Trim();
                        var orderItem = GetOrderItem(mes);
                        if (orderItem == null)
                        {
                            return;
                        }
                        var order = OrderNodes.FirstOrDefault(d => d.OrderID == orderItem.Id);
                        if (order == null)
                        {

                            GetMesOrderViewModel(orderItem);
                        }

                    };
                    subscription.SubscribeToChannels(redisChannel);
                    autoReset.WaitOne();
                }

            });


        }

        private void GetMesOrderViewModel(OrderItem item)
        {

            string name = dict.ContainsKey(item.ProductType) ? dict[item.ProductType] : "六分体";
            var orderItem = new OrderItemViewModel()
            {
                Count = item.Quantity,
                ItemID = item.Id,
                Type = item.Type,
                Name = name,
                IconPath = name

            };
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


        }
        private OrderItem GetOrderItem(object id)
        {
            using (var redisClient = managerPool.GetClient())
            {
                var high = redisClient.As<OrderItem>();
                return high.GetById(id);

            }

        }

        public override void Start(EngineerOrderViewModel model)
        {
            var order = GetOrderItem(model.OrderID);
            if (order != null)
            {
                order.State = OrderItemStateEnum.DOWORK;
                order.StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                UpdateOrderItem(order);
            }
        }
        public override void Finished(EngineerOrderViewModel model)
        {
            var order = GetOrderItem(model.OrderID);
            if (order != null)
            {
                order.State = OrderItemStateEnum.DONE;
                order.FinishTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                UpdateOrderItem(order);
            }
        }
        private void UpdateOrderItem(OrderItem item)
        {
            using (var redisClient = managerPool.GetClient())
            {
                var high = redisClient.As<OrderItem>();
                high.Store(item);

            }
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
