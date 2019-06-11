using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BFM.WPF.SHWMS.ViewModel
{
    public class BaseDeviceViewModel : ViewModelBase
    {
        public string ID { get; set; }

        public string IP { get; set; }

        public short CountPath { get; set; } = 1;

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

        protected CancellationTokenSource token = new CancellationTokenSource();

        public virtual void StartMachiningCount()
        {

            //获得加工数量的初始值
            int cout = 0;
            var resInit = GetTotalMachiningCount(IP, ref cout, CountPath);
            if (resInit == 0)
            {
                DeviceInitValue = cout;

            }

            Task.Factory.StartNew(() =>
            {

                while (Progress <= 100 && !token.IsCancellationRequested)
                {
                    Thread.Sleep(2000);
                    cout = 0;
                    var res = GetTotalMachiningCount(IP, ref cout, CountPath);
                    if (res == 0)
                    {
                        DeviceCurrentValue = cout;

                    }
                }
            }, token.Token);
        }


        public virtual void StopMachiningCount()
        {
            token.Cancel();
        }
        private short GetTotalMachiningCount(string ip, ref int cout, short cout_path)
        {
            ushort flib;
            short ret = Focas1.cnc_allclibhndl3(ip, 8193, 10, out flib);
            if (ret != 0) return ret;

            Focas1.cnc_setpath(flib, cout_path);

            Focas1.IODBPSD_1 buf = new Focas1.IODBPSD_1();
            ret = Focas1.cnc_rdparam(flib, 6712, 0, 8, buf);
            if (ret != 0)
            {
                Focas1.cnc_freelibhndl(flib);
                return ret;
            }

            cout = buf.idata;

            Focas1.cnc_freelibhndl(flib);
            return 0;

        }
    }
}
