using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFM.WPF.SHWMS.ViewModel.Finger
{
    public class FingerOrderViewModel : BaseOrderViewModel
    {
        public BaseDeviceViewModel VMOne { get; set; }

        public BaseDeviceViewModel LatheTwo { get; set; }
    }
}
