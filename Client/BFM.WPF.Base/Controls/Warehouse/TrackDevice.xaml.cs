using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace BFM.WPF.Base.Controls
{
    /// <summary>
    /// TrackDevice.xaml 的交互逻辑
    /// </summary>
    public partial class TrackDevice : UserControl
    {
        private Action<double> _setdevicepos;

        public TrackDevice()
        {
            InitializeComponent();

            _setdevicepos = SetDevicePosByValue;
        }

        #region Properties

        #region Properties.TotalColumn

        public static readonly DependencyProperty TotalColumnProperty =
            DependencyProperty.Register("TotalColumn",
                typeof(int),
                typeof(TrackDevice),
                new FrameworkPropertyMetadata(1, TotalColumnChanged));

        private static void TotalColumnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrackDevice tack = d as TrackDevice;
            if (tack == null) return;

            //设置长度
            if (tack.TotalColumn <= 0)
            {
                tack.TotalColumn = 1;
            }

            if (tack.ActualWidth > 10)
            {
                tack.bDevice.Width = tack.ActualWidth / tack.TotalColumn;
            }

            tack.SetDevicePosByColumn(tack.CurColumn);
        }

        /// <summary>
        /// 总列数
        /// </summary>
        public int TotalColumn
        {
            get { return (int)GetValue(TotalColumnProperty); }
            set { SetValue(TotalColumnProperty, value); }
        }

        #endregion

        #region Properties.CurColumn

        public static readonly DependencyProperty CurColumnProperty =
            DependencyProperty.Register("CurColumn",
                typeof(int),
                typeof(TrackDevice),
                new FrameworkPropertyMetadata(0, CurColumnChanged));

        private static void CurColumnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrackDevice tack = d as TrackDevice;
            if (tack == null) return;

            tack.SetDevicePosByColumn(tack.CurColumn);

            tack.OldColumn = tack.CurColumn; 
        }

        /// <summary>
        /// 当前所在列数,0表示原点
        /// </summary>
        public int CurColumn
        {
            get { return (int)GetValue(CurColumnProperty); }
            set { SetValue(CurColumnProperty, value); }
        }

        /// <summary>
        /// 原始列，为了好的展现效果
        /// </summary>
        private int OldColumn = 0;

        #endregion

        #region Properties.DeviceZeroPosition

        public static readonly DependencyProperty DeviceZeroPositionProperty =
            DependencyProperty.Register("DeviceZeroPosition",
                typeof(double),
                typeof(TrackDevice),
                new FrameworkPropertyMetadata(0.0, DeviceZeroPositionChanged));

        private static void DeviceZeroPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrackDevice tack = d as TrackDevice;
            if (tack == null) return;


        }

        /// <summary>
        /// 设备原点坐标
        /// </summary>
        public double DeviceZeroPosition
        {
            get { return (double)GetValue(DeviceZeroPositionProperty); }
            set { SetValue(DeviceZeroPositionProperty, value); }
        }

        #endregion

        #region Properties.ShowPosWithEffect

        public static readonly DependencyProperty ShowPosWithEffectProperty =
            DependencyProperty.Register("ShowPosWithEffect",
                typeof(bool),
                typeof(TrackDevice),
                new FrameworkPropertyMetadata(true, ShowPosWithEffectChanged));

        private static void ShowPosWithEffectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrackDevice tack = d as TrackDevice;
            if (tack == null) return;
        }

        /// <summary>
        /// 显示位置时是否带展示效果
        /// </summary>
        public bool ShowPosWithEffect
        {
            get { return (bool)GetValue(ShowPosWithEffectProperty); }
            set { SetValue(ShowPosWithEffectProperty, value); }
        }

        #endregion

        #endregion


        /// <summary>
        /// 按照坐标显示设备的位置
        /// </summary>
        /// <param name="position"></param>
        public void SetDevicePosByValue(double position)
        {
            bDevice.Margin = new Thickness(position, 0, 0, 0);
        }

        /// <summary>
        /// 按照列数显示设备的位置
        /// </summary>
        /// <param name="column"></param>
        private void SetDevicePosByColumn(int column)
        {
            if (TotalColumn <= 0) TotalColumn = 1;
            double newposition = ((column == 0) ? DeviceZeroPosition : column * (this.ActualWidth) / TotalColumn);
            double oldposition = ((OldColumn == 0) ? DeviceZeroPosition : OldColumn * (this.ActualWidth) / TotalColumn);

            if (Math.Abs(newposition - oldposition) > 10)
            {
                if (ShowPosWithEffect)
                {
                    (new Thread(() => ThreadSetPos(oldposition, newposition))).Start();
                }
                else  //不带展示效果，直接显示位置
                {
                    SetDevicePosByValue(newposition);
                }
            }

        }

        /// <summary>
        /// 后台延时显示位置
        /// </summary>
        /// <param name="oldpos"></param>
        /// <param name="newpos"></param>
        private void ThreadSetPos(double oldpos, double newpos)
        {
            int RunCount = 9;
            for (int i = 0; i < RunCount; i++)  //2s秒内完成移动
            {
                double position = oldpos + (newpos - oldpos) * i / (RunCount - 1);

                Dispatcher.BeginInvoke(_setdevicepos, position);

                Thread.Sleep(200);
            }
        }
        
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //设置长度
            if (TotalColumn <= 0)
            {
                TotalColumn = 1;
            }

            if (ActualWidth > 10)
            {
                bDevice.Width = ActualWidth / TotalColumn;
            }

            SetDevicePosByColumn(CurColumn);
        }
    }
}
