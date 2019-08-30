using BFM.Common.DeviceAsset;
using GalaSoft.MvvmLight.Command;
using HslCommunication;
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
    public class PushDeviceViewModel : BaseJobViewModel<PushOrderViewModel>
    {
        public override ICommand CycleStartCommand => new RelayCommand(CycleStart);

        public ICommand CycleStopCommand => new RelayCommand(CycleStop);
        public override ICommand AddCommand => new RelayCommand(new Action(() => AddOrder(GetOrderViewModel)));

        public override ICommand MachineResetCommand => new RelayCommand(new Action(() => { }));

        private static string CncHost;

        public override event Action<JobWorkEnum, string> JobOperationEvent;
        public override event Action<PushOrderViewModel> StartJobEvent;
        public override event Action MachineResetEvent;
        public event Func<OrderItemViewModel> GetOrderItemEvent;
        string redisChannel = "";

        private AutoResetEvent autoReset = new AutoResetEvent(false);
        CancellationTokenSource tokenSource;
        static PushDeviceViewModel()
        {
            CncHost = ConfigurationManager.AppSettings["CncHost"];
        }
        public PushDeviceViewModel(string channel)
        {

            tokenSource = new CancellationTokenSource();
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
            tokenSource.Cancel();

        }

        private bool FoceasAdd(PushOrderViewModel order)
        {
            try
            {
                // 发订单
                FocasManager focasManager = new FocasManager(new DeviceManager(DeviceCommInterface.CNC_Fanuc, CncHost), (s, e) =>
                {

                });
                FocasManager focasManager2 = new FocasManager(new DeviceManager(DeviceCommInterface.CNC_Fanuc, "192.168.1.42"), (s, e) =>
                {

                });
                //发送订单
                var aaa3 = focasManager.AsyncReadData("E501.0");

                var aaa4 = OperateResult.CreateSuccessResult(aaa3);
                var info = (aaa4.Content as dynamic).Content;
                if (info != "1")
                {
                    return false;
                }

                // 下单
                var ret = focasManager.SetPath(2);
                if (ret.IsSuccess)
                {
                    focasManager.AsyncWriteData("#550", order.Items[0].Count.ToString());
                    focasManager2.AsyncWriteData("#550", order.Items[0].Count.ToString());

                    focasManager.AsyncWriteData("E500.0", "1");
                    focasManager2.AsyncWriteData("E500.0", "1");
                    Thread.Sleep(1000);
                    focasManager.AsyncWriteData("E500.0", "0");
                    focasManager2.AsyncWriteData("E500.0", "0");
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {

                JobOperationEvent?.Invoke(JobWorkEnum.Error, "下单 错误！" + ex.Message);
                return false;

            }

        }
        private void CycleStart()
        {
            Task.Factory.StartNew(() =>
            {
                PushOrderViewModel pushDevice = null;
                while (!tokenSource.IsCancellationRequested)
                {
                    var obj = OrderNodes.FirstOrDefault(d => d.Sate == OrderStateEnum.Create);
                    if (obj != null)
                    {
                        var state = FoceasAdd(obj);
                        if (state)
                        {
                            obj.Sate = OrderStateEnum.Executing;
                            obj.Progress = 10;
                            obj.CurrentTotal = 1;
                            pushDevice = obj;
                        }

                    }
                    else
                    {
                        if (pushDevice != null)
                        {
                            pushDevice.CurrentTotal = pushDevice.Items[0].Count;
                            pushDevice.Progress = 100;
                            pushDevice.Sate = OrderStateEnum.Finish;
                        }

                    }

                    Thread.Sleep(1000);
                }

            }, tokenSource.Token);


        }



    }

    public class OperationResult
    {
        public string Content { get; set; }
    }
}

