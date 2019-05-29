using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
namespace BFM.WPF.SHWMS.ViewModel
{
    public enum OrderStateEnum
    {
        Create,
        Executing,
        Finish
    }

    public enum OrderCommandEnum
    {
        Remove,
        Cancel,
        Start,
        Pause,
        Continue

    }
    public class OrderViewModel : ViewModelBase
    {
        public string OrderID { get; set; }

        public string CreateTime { get; set; }

        private OrderStateEnum state;

        public OrderStateEnum Sate
        {
            get { return state; }
            set
            {
                state = value;
                StateInfo = state == OrderStateEnum.Create ? "新订单" : state == OrderStateEnum.Executing ? "执行中" : "已完成";
            }
        }



        private string context;

        public string Context
        {
            get { return context; }
            set
            {
                if (context != value)
                {
                    context = value;
                    RaisePropertyChanged(() => Context);
                }
            }
        }

        private string stateInfo;

        public string StateInfo
        {
            get { return stateInfo; }
            set
            {
                if (stateInfo != value)
                {
                    stateInfo = value;
                    RaisePropertyChanged(() => StateInfo);
                }
            }
        }


        private int progress;

        public int Progress
        {
            get { return progress; }
            set
            {
                if (progress != value)
                {
                    progress = value;
                    RaisePropertyChanged(() => Progress);
                }
            }
        }


        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();

        public OrderViewModel Self { get { return this; } }

        public override string ToString()
        {
            Context = Items.Aggregate("", (s, n) => $"{s} {n}");
            return Context;
        }


        public LatheViewModel LatheOne { get; set; }

        public LatheViewModel LatheTwo { get; set; }
        public ICommand OrderCommand
        {
            get
            {
                return new RelayCommand<OrderCommandEnum>(s =>
                {
                    OrderCommandEvent?.Invoke(s, this);
                });
            }
        }
        public int CurrentTotal { get; set; }
        public int TotalProgress { get; set; }
        public void StartJob()
        {
            TotalProgress = Items.Sum(d => d.Count);
            Sate = OrderStateEnum.Executing;

            OrderCommandEvent?.Invoke(OrderCommandEnum.Start, this);
            

        }

        public bool WorkItem(OrderItemViewModel model)
        {
            if (model.CurrentCount == model.Count)
            {

                return false;
            }
            model.CurrentCount++;
            CurrentTotal++;
            Progress = Convert.ToInt32(CurrentTotal * 100.0 / TotalProgress);
            if (CurrentTotal == TotalProgress)
            {
                Sate = OrderStateEnum.Finish;
            }
            return true;
        }
        public event Action<OrderCommandEnum, OrderViewModel> OrderCommandEvent;

    }


    public class OrderItemViewModel : ViewModelBase, ICloneable
    {
        private const int MaxCount = 10;

        public string ItemID { get; set; }

        public string Name { get; set; }


        public string IconPath { get; set; }


        private int count;

        public int Count
        {
            get { return count; }
            set
            {
                if (count != value)
                {
                    count = value;
                    RaisePropertyChanged(() => Count);
                }
            }
        }


        private int currentCount;

        public int CurrentCount
        {
            get { return currentCount; }
            set
            {
                if (currentCount != value)
                {
                    currentCount = value;
                    RaisePropertyChanged(() => CurrentCount);
                }
            }
        }

        public ICommand ItemOperaCommand
        {
            get
            {
                return new RelayCommand<object>(d =>
                {
                    var s = Convert.ToInt32(d);
                    var temp = Count;
                    temp += s;
                    if (temp > MaxCount)
                    {
                        Count = MaxCount;
                    }
                    else if (temp < 0)
                    {
                        Count = 0;
                    }
                    else
                    {
                        Count += s;
                    }

                });
            }
        }


        public void Init()
        {
            Count = 0;
        }

        public override string ToString()
        {
            return $"{Name}: {CurrentCount}/{Count}";
        }

        public object Clone()
        {
            return new OrderItemViewModel()
            {
                Count = Count,
                ItemID = ItemID,
                IconPath = IconPath,
                Name = Name
            };
        }
    }


    public class LatheViewModel : ViewModelBase
    {
        public string ID { get; set; }

        public int DeviceInitValue { get; set; }


        private int deviceCurrentValue;

        public int DeviceCurrentValue
        {
            get { return deviceCurrentValue; }
            set
            {
                deviceCurrentValue = value;
                CurrentValue = deviceCurrentValue - DeviceInitValue;
            }
        }



        private int currentValue;

        public int CurrentValue
        {
            get { return currentValue; }
            set
            {
                if (currentValue != value)
                {
                    currentValue = value;
                    Progress = (int)(currentValue * 100.0 / Count);
                    RaisePropertyChanged(() => CurrentValue);

                }
            }
        }


        private int progress;

        public int Progress
        {
            get { return progress; }
            set
            {
                if (progress != value)
                {
                    progress = value;
                    RaisePropertyChanged(() => Progress);

                }
            }
        }


        public int Count { get; set; }


    }


    public class LatheNode
    {
        public string ID { get; set; }

        public int InitValue { get; set; }

        public int CurrentValue { get; set; }
    }
}
