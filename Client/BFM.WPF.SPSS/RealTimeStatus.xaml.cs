using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace BFM.WPF.SPSS
{
    /// <summary>
    /// Interaction logic for RealTimeStatus.xaml
    /// </summary>
    public partial class RealTimeStatus : Page
    {
        
        public RealTimeStatus()
        {
            InitializeComponent();

        }

        #region 按钮事件
        private void bSearch_click(object sender, RoutedEventArgs e)
        {
            DemoData demo = new DemoData();
            speedQuarter1.DataContext = demo;
            speedQuarter2.DataContext = demo;
            LoadMessage.DataContext = demo;
            coordinate.DataContext = demo;
            Message.DataContext = demo;
            ProgramShow.DataContext = demo;
            demo.Start();
        }
        #endregion
    }

    public class DemoData : BindableBase
    {
        #region 实体对应属性
        public double NeedleValue
        {
            get { return GetProperty(() => NeedleValue); }
            set { SetProperty(() => NeedleValue, value); }
        }
        public double MarkerValue
        {
            get { return GetProperty(() => MarkerValue); }
            set { SetProperty(() => MarkerValue, value); }
        }
        public double RangeBarValue
        {
            get { return GetProperty(() => RangeBarValue); }
            set { SetProperty(() => RangeBarValue, value); }
        }
        public double MechanicalX
        {
            get { return GetProperty(() => MechanicalX); }
            set { SetProperty(() => MechanicalX, value); }
        }
        public double MechanicalY
        {
            get { return GetProperty(() => MechanicalY); }
            set { SetProperty(() => MechanicalY, value); }
        }
        public double MechanicalZ
        {
            get { return GetProperty(() => MechanicalZ); }
            set { SetProperty(() => MechanicalZ, value); }
        }
        public double AbsoluteX
        {
            get { return GetProperty(() => AbsoluteX); }
            set { SetProperty(() => AbsoluteX, value); }
        }
        public double AbsoluteY
        {
            get { return GetProperty(() => AbsoluteY); }
            set { SetProperty(() => AbsoluteY, value); }
        }
        public double AbsoluteZ
        {
            get { return GetProperty(() => AbsoluteZ); }
            set { SetProperty(() => AbsoluteZ, value); }
        }
        public double RelativeX
        {
            get { return GetProperty(() => RelativeX); }
            set { SetProperty(() => RelativeX, value); }
        }
        public double RelativeY
        {
            get { return GetProperty(() => RelativeY); }
            set { SetProperty(() => RelativeY, value); }
        }
        public double RelativeZ
        {
            get { return GetProperty(() => RelativeZ); }
            set { SetProperty(() => RelativeZ, value); }
        }
        public double SurplusX
        {
            get { return GetProperty(() => SurplusX); }
            set { SetProperty(() => SurplusX, value); }
        }
        public double SurplusY
        {
            get { return GetProperty(() => SurplusY); }
            set { SetProperty(() => SurplusY, value); }
        }
        public double SurplusZ
        {
            get { return GetProperty(() => SurplusZ); }
            set { SetProperty(() => SurplusZ, value); }
        }
        public double MajorLoad
        {
            get { return GetProperty(() => MajorLoad); }
            set { SetProperty(() => MajorLoad, value); }
        }
        public double XLoad
        {
            get { return GetProperty(() => XLoad); }
            set { SetProperty(() => XLoad, value); }
        }
        public double YLoad
        {
            get { return GetProperty(() => YLoad); }
            set { SetProperty(() => YLoad, value); }
        }
        public double ZLoad
        {
            get { return GetProperty(() => ZLoad); }
            set { SetProperty(() => ZLoad, value); }
        }
        public string ProgramNumber
        {
            get { return GetProperty(() => ProgramNumber); }
            set { SetProperty(() => ProgramNumber, value); }
        }
        public string ChildProgramNumber
        {
            get { return GetProperty(() => ChildProgramNumber); }
            set { SetProperty(() => ChildProgramNumber, value); }
        }
        public string ProgramMessage
        {
            get { return GetProperty(() => ProgramMessage); }
            set { SetProperty(() => ProgramMessage, value); }
        }
        public string AlarmMessage
        {
            get { return GetProperty(() => AlarmMessage); }
            set { SetProperty(() => AlarmMessage, value); }
        }
        public string ModelMessage
        {
            get { return GetProperty(() => ModelMessage); }
            set { SetProperty(() => ModelMessage, value); }
        }
        public string ConnectionState
        {
            get { return GetProperty(() => ConnectionState); }
            set { SetProperty(() => ConnectionState, value); }
        }
        public string ConnectionMessage
        {
            get { return GetProperty(() => ConnectionMessage); }
            set { SetProperty(() => ConnectionMessage, value); }
        }
        #endregion

        DispatcherTimer timer;

        readonly Random random = new Random();

        readonly double minValue = 0;
        readonly double MaxValue = 100;

        double ValuesRange { get { return MaxValue - minValue; } }

        public  DemoData()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += OnTimerTick;
        }

        void OnTimerTick(object sender, EventArgs e)
        {
            NeedleValue = minValue + ValuesRange * random.NextDouble();
            RangeBarValue = minValue + ValuesRange * random.NextDouble();
            MarkerValue = minValue + ValuesRange * random.NextDouble();

            MechanicalX = minValue + ValuesRange * random.NextDouble();
            MechanicalY = minValue + ValuesRange * random.NextDouble();
            MechanicalZ = minValue + ValuesRange * random.NextDouble();

            AbsoluteX = minValue + ValuesRange * random.NextDouble();
            AbsoluteY = minValue + ValuesRange * random.NextDouble();
            AbsoluteZ = minValue + ValuesRange * random.NextDouble();

            RelativeX = minValue + ValuesRange * random.NextDouble();
            RelativeY = minValue + ValuesRange * random.NextDouble();
            RelativeZ = minValue + ValuesRange * random.NextDouble();

            SurplusX = minValue + ValuesRange * random.NextDouble();
            SurplusY = minValue + ValuesRange * random.NextDouble();
            SurplusZ = minValue + ValuesRange * random.NextDouble();

            MajorLoad = minValue + ValuesRange * random.NextDouble();
            XLoad = minValue + ValuesRange * random.NextDouble();
            YLoad = minValue + ValuesRange * random.NextDouble();
            ZLoad = minValue + ValuesRange * random.NextDouble();

            ProgramNumber = random.NextDouble().ToString();
            ChildProgramNumber = random.NextDouble().ToString();
            ProgramMessage = random.NextDouble().ToString();

            AlarmMessage = random.NextDouble().ToString();
            ModelMessage = random.NextDouble().ToString();
            ConnectionState = "未连接";
            ConnectionMessage = random.NextDouble().ToString();

        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }
    }
}
