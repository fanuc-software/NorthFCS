using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BFM.WPF.SHWMS.ViewModel
{
    public class OrderItemViewModel : ViewModelBase, ICloneable
    {
        public BaseOrderViewModel MainOrder { get; set; }

        private const int MaxCount = 10;

        public string ItemID { get; set; }

        public string Name { get; set; }


        public int Type { get; set; }

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
                Name = Name,
                MainOrder = MainOrder
            };
        }
    }

}
