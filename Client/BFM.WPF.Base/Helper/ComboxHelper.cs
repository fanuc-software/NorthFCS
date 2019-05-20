using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BFM.WPF.Base.Helper
{
    public class ComboxHelper: INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get { return _name;}
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private object _value;
        public object Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }


        #region mvvm 的回调事件

        //Set 中 增加 OnPropertyChanged("Value");

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
