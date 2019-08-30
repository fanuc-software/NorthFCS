using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BFM.WPF.SHWMS.ViewModel
{
    public abstract class BaseJobViewModel<T> : ViewModelBase where T : BaseOrderViewModel
    {
        public ObservableCollection<T> OrderNodes { get; set; } = new ObservableCollection<T>();
        public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();
        public abstract event Action<JobWorkEnum, string> JobOperationEvent;

        public abstract event Action<T> StartJobEvent;

        public abstract event Action MachineResetEvent;

        public abstract ICommand CycleStartCommand { get; }


        public abstract ICommand AddCommand { get; }

        public virtual void Start(T model)
        {

        }

        public virtual void Finished(T model)
        {

        }
        public abstract ICommand MachineResetCommand { get; }

    }
}
