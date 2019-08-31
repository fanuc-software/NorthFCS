using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BFM.WPF.SHWMS.ViewModel
{
    public class BaseOrderViewModel : ViewModelBase
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
                StateInfo = state == OrderStateEnum.Create ? "新订单" : state == OrderStateEnum.Executing ? "执行中" : state == OrderStateEnum.Cancel ? "已取消" : "已完成";
                RemoveVisibility = state == OrderStateEnum.Create ? Visibility.Visible : Visibility.Collapsed;
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

        private Visibility removeVisibility;

        public Visibility RemoveVisibility
        {
            get { return removeVisibility; }
            set
            {
                if (removeVisibility != value)
                {
                    removeVisibility = value;
                    RaisePropertyChanged(() => RemoveVisibility);
                }
            }
        }

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

        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();

        public bool IsStart { get; set; }
        public virtual void StartJob()
        {
            TotalProgress = Items.Sum(d => d.Count);
            Sate = OrderStateEnum.Executing;
            IsStart = true;


        }

        public virtual void FinishJob()
        {
            Sate = OrderStateEnum.Finish;

        }

        public virtual bool WorkItem(OrderItemViewModel model)
        {
            if (model.CurrentCount == model.Count)
            {

                return false;
            }
            model.CurrentCount++;
            CurrentTotal++;
            Progress = Convert.ToInt32(CurrentTotal * 100.0 / TotalProgress);
            //if (CurrentTotal == TotalProgress)
            //{
            //    Sate = OrderStateEnum.Finish;
            //}
            return true;
        }
        public event Action<OrderCommandEnum, BaseOrderViewModel> OrderCommandEvent;
    }
}
