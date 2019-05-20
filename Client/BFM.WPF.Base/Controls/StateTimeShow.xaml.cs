using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BFM.WPF.Base.Controls
{
    /// <summary>
    /// StateTimeShow.xaml 的交互逻辑
    /// </summary>
    public partial class StateTimeShow : UserControl
    {
        public StateTimeShow()
        {
            InitializeComponent();
        }

        private void StateTimeShow_OnLoaded(object sender, RoutedEventArgs e)
        {
            RefreshDraw();
        }

        #region Properties

        #region Properties 标题

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title",
                typeof(string),
                typeof(StateTimeShow),
                new FrameworkPropertyMetadata("Title", TitleChanged));

        private static void TitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StateTimeShow;
            if (control != null)
            {
                control.tbTitle.Text = (string)e.NewValue;
                control.RefreshDraw();
            }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        #endregion

        #region Properties 时间标题是否显示

        public static readonly DependencyProperty TimeBottomVisibleProperty =
            DependencyProperty.Register("TimeBottomVisible",
                typeof(Visibility),
                typeof(StateTimeShow),
                new FrameworkPropertyMetadata(System.Windows.Visibility.Visible, TimeBottomVisibleChanged));

        private static void TimeBottomVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StateTimeShow;
            if (control != null)
            {
                control.cvBottomTime.Visibility = (Visibility) e.NewValue;
                control.DrawBottomTime();
            }
        }

        public Visibility TimeBottomVisible
        {
            get { return (Visibility)GetValue(TimeBottomVisibleProperty); }
            set { SetValue(TimeBottomVisibleProperty, value); }
        }

        public static readonly DependencyProperty TimeTopVisibleProperty =
            DependencyProperty.Register("TimeTopVisible",
                typeof(Visibility),
                typeof(StateTimeShow),
                new FrameworkPropertyMetadata(System.Windows.Visibility.Collapsed, TimeTopVisibleChanged));

        private static void TimeTopVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StateTimeShow;
            if (control != null)
            {
                control.cvTopTime.Visibility = (Visibility)e.NewValue;
                control.DrawTopTime();
            }
        }

        public Visibility TimeTopVisible
        {
            get { return (Visibility)GetValue(TimeTopVisibleProperty); }
            set { SetValue(TimeTopVisibleProperty, value); }
        }

        #endregion

        #region Properties 时间与主显示间隔

        public static readonly DependencyProperty TimeTitleSpanProperty =
            DependencyProperty.Register("TimeTitleSpan",
                typeof(double),
                typeof(StateTimeShow),
                new FrameworkPropertyMetadata((Double)0, TimeTitleSpanChanged));

        private static void TimeTitleSpanChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StateTimeShow;
            if (control != null)
            {
                control.DrawTopTime();
                control.DrawBottomTime();
            }
        }

        public double TimeTitleSpan
        {
            get { return (double)GetValue(TimeTitleSpanProperty); }
            set { SetValue(TimeTitleSpanProperty, value); }
        }

        #endregion

        #region Properties 时间标题高度

        public static readonly DependencyProperty TimeTitleHeightProperty =
            DependencyProperty.Register("TimeTitleHeight",
                typeof(double),
                typeof(StateTimeShow),
                new FrameworkPropertyMetadata((Double)20, TimeTitleHeightChanged));

        private static void TimeTitleHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StateTimeShow;
            if (control != null)
            {
                control.cvBottomTime.Height = (double) e.NewValue;
                control.cvTopTime.Height = (double)e.NewValue;
                control.RefreshDraw();
            }
        }

        public double TimeTitleHeight
        {
            get { return (double)GetValue(TimeTitleSpanProperty); }
            set { SetValue(TimeTitleSpanProperty, value); }
        }

        #endregion
        
        #region Properties 是否显示最小的刻度

        public static readonly DependencyProperty ShowMinTimeProperty =
            DependencyProperty.Register("ShowMinTime",
                typeof(bool),
                typeof(StateTimeShow),
                new FrameworkPropertyMetadata((bool)true, ShowMinTimeChanged));

        private static void ShowMinTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StateTimeShow;
            if (control != null)
            {
                control.DrawTopTime();
                control.DrawBottomTime();
            }
        }

        public bool ShowMinTime
        {
            get { return (bool)GetValue(ShowMinTimeProperty); }
            set { SetValue(ShowMinTimeProperty, value); }
        }

        #endregion

        #region Properties 是否显示中间的刻度

        public static readonly DependencyProperty ShowMiddleTimeProperty =
            DependencyProperty.Register("ShowMiddleTime",
                typeof(bool),
                typeof(StateTimeShow),
                new FrameworkPropertyMetadata((bool)true, ShowMiddleTimeChanged));

        private static void ShowMiddleTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StateTimeShow;
            if (control != null)
            {
                control.DrawTopTime();
                control.DrawBottomTime();
            }
        }

        public bool ShowMiddleTime
        {
            get { return (bool)GetValue(ShowMiddleTimeProperty); }
            set { SetValue(ShowMiddleTimeProperty, value); }
        }

        #endregion

        #region Properties 时间标长刻度高度

        public static readonly DependencyProperty TimeMaxHeightProperty =
            DependencyProperty.Register("TimeMaxHeight",
                typeof(double),
                typeof(StateTimeShow),
                new FrameworkPropertyMetadata((Double)1, TimeMaxHeightChanged));

        private static void TimeMaxHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StateTimeShow;
            if (control != null)
            {
                control.DrawTopTime();
                control.DrawBottomTime();
            }
        }

        public double TimeMaxHeight
        {
            get { return (double)GetValue(TimeMaxHeightProperty); }
            set { SetValue(TimeMaxHeightProperty, value); }
        }

        #endregion

        #region Properties 时间标中间刻度高度

        public static readonly DependencyProperty TimeMiddleHeightProperty =
            DependencyProperty.Register("TimeMiddleHeight",
                typeof(double),
                typeof(StateTimeShow),
                new FrameworkPropertyMetadata((Double)0.5, TimeMiddleHeightChanged));

        private static void TimeMiddleHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StateTimeShow;
            if (control != null)
            {
                control.DrawTopTime();
                control.DrawBottomTime();
            }
        }

        public double TimeMiddleHeight
        {
            get { return (double)GetValue(TimeMiddleHeightProperty); }
            set { SetValue(TimeMiddleHeightProperty, value); }
        }

        #endregion

        #region Properties 时间标短刻度高度

        public static readonly DependencyProperty TimeMinHeightProperty =
            DependencyProperty.Register("TimeMinHeight",
                typeof(double),
                typeof(StateTimeShow),
                new FrameworkPropertyMetadata((Double)0.25, TimeMinHeightChanged));

        private static void TimeMinHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StateTimeShow;
            if (control != null)
            {
                control.DrawTopTime();
                control.DrawBottomTime();
            }
        }

        public double TimeMinHeight
        {
            get { return (double)GetValue(TimeMinHeightProperty); }
            set { SetValue(TimeMinHeightProperty, value); }
        }

        #endregion
        
        #region Properties 时间标时间位置

        public static readonly DependencyProperty TimePositionProperty =
            DependencyProperty.Register("TimePosition",
                typeof(Point),
                typeof(StateTimeShow),
                new FrameworkPropertyMetadata((Point)(new Point(10,10)), TimePositionChanged));

        private static void TimePositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StateTimeShow;
            if (control != null)
            {
                control.DrawTopTime();
                control.DrawBottomTime();
            }
        }

        public Point TimePosition
        {
            get { return (Point)GetValue(TimePositionProperty); }
            set { SetValue(TimePositionProperty, value); }
        }

        #endregion

        #region Properties 选择数据日期

        public static readonly DependencyProperty ShowDateProperty =
            DependencyProperty.Register("ShowDate",
                typeof(DateTime),
                typeof(StateTimeShow),
                new FrameworkPropertyMetadata(DateTime.Now.Date, ShowDateChanged));

        private static void ShowDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StateTimeShow;
            if (control != null)
            {
                control.DrawState();
            }
        }

        public DateTime ShowDate
        {
            get { return ((DateTime)GetValue(ShowDateProperty)).Date; }
            set { SetValue(ShowDateProperty, value); }
        }

        #endregion

        #region Properties 状态数据背景色

        public static readonly DependencyProperty StateBackgroundProperty =
            DependencyProperty.Register("StateBackground",
                typeof(Brush),
                typeof(StateTimeShow),
                new FrameworkPropertyMetadata(Brushes.Gray, StateBackgroundChanged));

        private static void StateBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StateTimeShow;
            if (control != null)
            {
                control.cvMain.Background = (Brush) e.NewValue;
            }
        }

        public Brush StateBackground
        {
            get { return (Brush)GetValue(StateBackgroundProperty); }
            set { SetValue(StateBackgroundProperty, value); }
        }

        #endregion

        #region Properties 状态数据边框色

        public static readonly DependencyProperty StateBorderProperty =
            DependencyProperty.Register("StateBorder",
                typeof(Brush),
                typeof(StateTimeShow),
                new FrameworkPropertyMetadata(Brushes.Gray, StateBorderChanged));

        private static void StateBorderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StateTimeShow;
            if (control != null)
            {
                control.bdMain.BorderBrush = (Brush)e.NewValue;
            }
        }

        public Brush StateBorder
        {
            get { return (Brush)GetValue(StateBorderProperty); }
            set { SetValue(StateBorderProperty, value); }
        }

        #endregion

        #region Properties 状态数据边框样式

        public static readonly DependencyProperty StateBorderThinknessProperty =
            DependencyProperty.Register("StateBorderThinkness",
                typeof(Thickness),
                typeof(StateTimeShow),
                new FrameworkPropertyMetadata(new Thickness(1), StateBorderThinknessChanged));

        private static void StateBorderThinknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StateTimeShow;
            if (control != null)
            {
                control.bdMain.BorderThickness = (Thickness)e.NewValue;
            }
        }

        public Thickness StateBorderThinkness
        {
            get { return (Thickness)GetValue(StateBorderThinknessProperty); }
            set { SetValue(StateBorderThinknessProperty, value); }
        }

        #endregion

        #region Properties 状态数据

        public static readonly DependencyProperty StateDataProperty =
            DependencyProperty.Register("StateData",
                typeof(List<TimeFormat>),
                typeof(StateTimeShow),
                new FrameworkPropertyMetadata(new List<TimeFormat>(), StateDataChanged));

        private static void StateDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StateTimeShow;
            if (control != null)
            {
                control.DrawState();
            }
        }

        public List<TimeFormat> StateData
        {
            get { return (List<TimeFormat>)GetValue(StateDataProperty); }
            set { SetValue(StateDataProperty, value); }
        }

        #endregion

        #region Properties 是否鼠标移动显示

        public static readonly DependencyProperty IsMouseMoveShowProperty =
            DependencyProperty.Register("IsMouseMoveShow",
                typeof(bool),
                typeof(StateTimeShow),
                new FrameworkPropertyMetadata(false, IsMouseMoveShowChanged));

        private static void IsMouseMoveShowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StateTimeShow;
            if (control != null)
            {
            }
        }

        public bool IsMouseOverShow
        {
            get { return (bool)GetValue(IsMouseMoveShowProperty); }
            set { SetValue(IsMouseMoveShowProperty, value); }
        }

        #endregion

        #region Properties 显示提示信息格式

        public static readonly DependencyProperty StateToolTipFormatProperty =
            DependencyProperty.Register("StateToolTipFormat",
                typeof(string),
                typeof(StateTimeShow),
                new FrameworkPropertyMetadata("", StateToolTipFormatChanged));

        private static void StateToolTipFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StateTimeShow;
            if (control != null)
            {
            }
        }

        public string StateToolTipFormat
        {
            get { return (string)GetValue(StateToolTipFormatProperty); }
            set { SetValue(StateToolTipFormatProperty, value); }
        }

        #endregion

        #endregion

        #region 事件

        #region StateItemMouseMove

        public static readonly RoutedEvent StateItemMouseMoveEvent =
            EventManager.RegisterRoutedEvent("StateItemMouseMove",
                        RoutingStrategy.Bubble,
                        typeof(RoutedEventHandler),
                        typeof(StateTimeShow));

        /// <summary>
        /// 处理各种路由事件的方法 
        /// </summary>
        public event RoutedEventHandler StateItemMouseMove
        {
            //将路由事件添加路由事件处理程序
            add { AddHandler(StateItemMouseMoveEvent, value); }
            //从路由事件处理程序中移除路由事件
            remove { RemoveHandler(StateItemMouseMoveEvent, value); }
        }

        #endregion

        #region StateItemMouseDown

        public static readonly RoutedEvent StateItemMouseDownEvent =
            EventManager.RegisterRoutedEvent("StateItemMouseDown",
                        RoutingStrategy.Bubble,
                        typeof(RoutedEventHandler),
                        typeof(StateTimeShow));

        /// <summary>
        /// 处理各种路由事件的方法 
        /// </summary>
        public event RoutedEventHandler StateItemMouseDown
        {
            //将路由事件添加路由事件处理程序
            add { AddHandler(StateItemMouseDownEvent, value); }
            //从路由事件处理程序中移除路由事件
            remove { RemoveHandler(StateItemMouseDownEvent, value); }
        }

        #endregion

        #endregion

        public void RefreshDraw()
        {
            DrawTopTime();
            DrawBottomTime();
            DrawState();
        }

        //顶部Time
        private void DrawTopTime()
        {
            cvTopTime.Children.Clear();
            if (TimeTopVisible != Visibility.Visible)
            {
                return;
            }
            double allWidth = cvTopTime.ActualWidth;
            if ((allWidth <= 0) && (!double.IsNaN(cvTopTime.Width)))
            {
                allWidth = cvTopTime.Width;
            }
            if (allWidth <= 0)
            {
                return;
            }

            double baseHeight = cvTopTime.ActualHeight;
            if ((baseHeight <= 0) && (!double.IsNaN(cvTopTime.Height)))
            {
                baseHeight = cvTopTime.Height;
            }
            double beginY = TimeTitleSpan * (-1);  //起始高度

            double bottom = beginY + baseHeight;  //底部

            double middleTop = beginY + baseHeight * (1 - TimeMiddleHeight);   //中间
            double minTop = beginY + baseHeight * (1 - TimeMinHeight);   //最短

            beginY = beginY + baseHeight * (1 - TimeMaxHeight);  //开始

            for (int i = 0; i < 24; i++)
            {
                TextBlock tb = new TextBlock()
                {
                    Text = i.ToString() + ":00",
                    FontSize = 10,
                };

                Canvas.SetLeft(tb, i * allWidth / 24 + TimePosition.X);
                Canvas.SetTop(tb, TimeTitleSpan * (-1) + TimePosition.Y - 13);
                cvTopTime.Children.Add(tb);

                Line line = new Line()
                {
                    X1 = (i*allWidth/24),
                    Y1 = beginY,
                    X2 = (i*allWidth/24),
                    Y2 = bottom,
                    StrokeThickness = 1,
                    Stroke = Brushes.Gray,
                    //StrokeDashArray = new DoubleCollection() {2, 3},
                };
                cvTopTime.Children.Add(line); //长刻度

                
                for (int j = 0; j < 9; j++)
                {
                    if ((!ShowMinTime) && (j != 4))
                    {
                        continue;
                    }

                    if ((!ShowMiddleTime) && (j == 4))
                    {
                        continue;
                    }
                    Line line2 = new Line()
                    {
                        X1 = ((i + ((j + 1.0)/10))*allWidth/24),
                        Y1 = ((j == 4) ? middleTop : minTop),
                        X2 = ((i + ((j + 1.0)/10))*allWidth/24),
                        Y2 = bottom,
                        StrokeThickness = 1,
                        Stroke = Brushes.Gray,
                        //StrokeDashArray = new DoubleCollection() { 2, 3 },
                    };
                    cvTopTime.Children.Add(line2); //中间刻度
                }
            }

            //绘制最后一个长刻度
            Line last = new Line()
            {
                X1 = allWidth,
                Y1 = beginY,
                X2 = allWidth,
                Y2 = bottom,
                StrokeThickness = 1,
                Stroke = Brushes.Gray,
                //StrokeDashArray = new DoubleCollection() {2, 3},
            };
            cvTopTime.Children.Add(last);
        }

        //底部Time
        private void DrawBottomTime()
        {
            cvBottomTime.Children.Clear();
            if (TimeBottomVisible != Visibility.Visible)
            {
                return;
            }
            double allWidth = cvBottomTime.ActualWidth;
            if ((allWidth <= 0) && (!double.IsNaN(cvBottomTime.Width)))
            {
                allWidth = cvBottomTime.Width;
            }
            if (allWidth <= 0)
            {
                return;
            }
            double baseHeight = cvTopTime.ActualHeight;
            if ((baseHeight <= 0) && (!double.IsNaN(cvBottomTime.Height)))
            {
                baseHeight = cvBottomTime.Height;
            }

            double maxHeight = baseHeight * TimeMaxHeight;
            double middleHeight = baseHeight * TimeMiddleHeight;   //中间
            double minHeight = baseHeight * TimeMinHeight;     //最短
            double beginY = TimeTitleSpan;  //起始高度

            for (int i = 0; i < 24; i++)
            {
                TextBlock tb = new TextBlock()
                {
                    Text = i.ToString() + ":00",
                    FontSize = 10,
                };

                Canvas.SetLeft(tb, i*allWidth/24 + TimePosition.X);
                Canvas.SetTop(tb, beginY + maxHeight - TimePosition.Y);
                cvBottomTime.Children.Add(tb);

                Line line = new Line()
                {
                    X1 = (i*allWidth/24),
                    Y1 = beginY,
                    X2 = (i*allWidth/24),
                    Y2 = beginY + maxHeight,
                    StrokeThickness = 1,
                    Stroke = Brushes.Gray,
                    //StrokeDashArray = new DoubleCollection() {2, 3},
                };
                cvBottomTime.Children.Add(line); //长刻度

                for (int j = 0; j < 9; j++)
                {
                    if ((!ShowMinTime) && (j != 4))
                    {
                        continue;
                    }

                    if ((!ShowMiddleTime) && (j == 4))
                    {
                        continue;
                    }
                    Line line2 = new Line()
                    {
                        X1 = ((i + ((j + 1.0)/10))*allWidth/24),
                        Y1 = beginY,
                        X2 = ((i + ((j + 1.0)/10))*allWidth/24),
                        Y2 = beginY + ((j == 4) ? middleHeight : minHeight),
                        StrokeThickness = 1,
                        Stroke = Brushes.Gray,
                        //StrokeDashArray = new DoubleCollection() { 2, 3 },
                    };
                    cvBottomTime.Children.Add(line2); //中间刻度
                }
            }

            //绘制最后一个线
            Line last = new Line()
            {
                X1 = allWidth,
                Y1 = beginY,
                X2 = allWidth,
                Y2 = beginY + maxHeight,
                StrokeThickness = 1,
                Stroke = Brushes.Gray,
                //StrokeDashArray = new DoubleCollection() {2, 3},
            };
            cvBottomTime.Children.Add(last);
        }

        private void DrawState()
        {
            cvMain.Children.Clear();
            double allWidth = cvMain.ActualWidth;
            double baseHeight = cvMain.ActualHeight;
            if ((allWidth <= 0) && (!double.IsNaN(cvMain.Width)))
            {
                allWidth = cvBottomTime.Width;
            }
            if (allWidth <= 0)
            {
                return;
            }

            foreach (TimeFormat data in StateData)
            {
                DateTime minTime = (DateTime)data.BeginTime;
                DateTime maxTime = (DateTime)data.EndTime;

                if (minTime < ShowDate)
                {
                    minTime = ShowDate;
                }
                if (maxTime > ShowDate.AddDays(1))
                {
                    maxTime = ShowDate.AddDays(1);
                }

                if (minTime > maxTime)
                {
                    continue;
                }

                Double left = 0;

                if (minTime > ShowDate)
                {
                    double beginSpan = minTime.Subtract(ShowDate).TotalSeconds; //按照秒计算
                    left = allWidth * beginSpan / (60 * 60 * 24);
                }

                double span = maxTime.Subtract(minTime).TotalSeconds + 0.001; //按照秒计算

                double thisWidth = allWidth * span / (60 * 60 * 24);
                Brush brush = data.StateColor;
                Rectangle retc = new Rectangle()
                {
                    Width = thisWidth,
                    Height = baseHeight,
                    Fill = brush,
                    Stroke = data.BordBrush ?? brush, 
                    StrokeThickness = 1
                };

                Canvas.SetLeft(retc, left);
                Canvas.SetTop(retc, 0);

                data.Left = left;
                data.Right = left + thisWidth;
                data.Rect = retc;
                data.Height = baseHeight;
                data.Widht = thisWidth;

                cvMain.Children.Add(retc);

                if ((data.ShowText) && (!string.IsNullOrEmpty(data.StateText)))
                {
                    TextBlock tb = new TextBlock()
                    {
                        Text = data.StateText,
                        Height = baseHeight,
                        Width = thisWidth,
                        TextAlignment = TextAlignment.Center,
                        Foreground = data.TextColor ?? this.Foreground,
                    };
                    Canvas.SetLeft(tb, left);
                    Canvas.SetTop(tb, baseHeight / 2 - 12);
                    cvMain.Children.Add(tb);
                }
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RefreshDraw();
        }

        private void StateTimeShow_OnMouseLeave(object sender, MouseEventArgs e)
        {
            foreach (TimeFormat thisItem in StateData)
            {
                if (thisItem.Rect == null) continue;
                if (thisItem.Rect.Margin.Left > 0)
                {
                    thisItem.Rect.Margin = new Thickness(0);
                    thisItem.Rect.Width = thisItem.Widht;
                    thisItem.Rect.Height = thisItem.Height;
                }
            }
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)  //鼠标移动
        {
            foreach (TimeFormat thisItem in StateData)
            {
                if (thisItem.Rect == null) continue;
                if (thisItem.Rect.Margin.Left > 0)
                {
                    thisItem.Rect.Margin = new Thickness(0);
                    thisItem.Rect.Width = thisItem.Widht;
                    thisItem.Rect.Height = thisItem.Height;
                }
            }
            double x = e.GetPosition(cvMain).X;
            double y = e.GetPosition(cvMain).Y;
            if ((x < 0) || (x > cvMain.ActualWidth) ||
                (y < 0) || (y > cvMain.ActualHeight))
            {
                ToolTipService.SetIsEnabled(cvMain, false);
                return;
            }

            TimeFormat item = StateData.FirstOrDefault(c => (c.Left <= x) && (c.Right >= x));

            if (item == null)
            {
                ToolTipService.SetIsEnabled(cvMain, false);
                return;
            }
            ToolTipService.SetIsEnabled(cvMain, !string.IsNullOrEmpty(StateToolTipFormat));

            if (IsMouseOverShow)  //鼠标移上后显示
            {
                item.Rect.Margin = new Thickness(2);
                item.Rect.Width = item.Widht - 4;
                item.Rect.Height = item.Height - 4;
            }

            if (!string.IsNullOrEmpty(StateToolTipFormat))
            {
                string tipString = StateToolTipFormat;
                tipString = tipString.Replace("(Name)", item.Name??"");
                tipString = tipString.Replace("(BeginTime)", item.BeginTime.ToString());
                tipString = tipString.Replace("(EndTime)", item.EndTime.ToString());
                tipString = tipString.Replace("(StateText)", item.StateText??"");
                tipString = tipString.Replace("\\n", Environment.NewLine);
                tbToolTip.Text = tipString;
            }

            this.RaiseEvent(new RoutedEventArgs(StateItemMouseMoveEvent, item));
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            double x = e.GetPosition(cvMain).X;
            double y = e.GetPosition(cvMain).Y;
            if ((x < 0) || (x > cvMain.ActualWidth) ||
                (y < 0) || (y > cvMain.ActualHeight))
            {
                return;
            }

            TimeFormat item = StateData.FirstOrDefault(c => (c.Left <= x) && (c.Right >= x));

            if (item == null)
            {
                return;
            }

            this.RaiseEvent(new RoutedEventArgs(StateItemMouseDownEvent, item));
        }

    }

    public class TimeFormat
    {
        public string Name { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 状态内容
        /// </summary>
        public string StateText { get; set; }

        /// <summary>
        /// 状态颜色
        /// </summary>
        public virtual Brush StateColor { get; set; }

        /// <summary>
        /// 边框颜色
        /// </summary>
        public virtual Brush BordBrush { get; set; }

        /// <summary>
        /// 是否显示状态内容
        /// </summary>
        public bool ShowText { get; set; }

        public Brush TextColor { get; set; }
        
        public double Left;
        public double Right;
        public double Height;
        public double Widht;
        public Rectangle Rect;
    }

    public class TimeStateFormat : TimeFormat
    {
        public EmStateType StateType { get; set; }

        public override Brush StateColor
        {
            get
            {
                switch (StateType)
                {
                    case EmStateType.StandBy: //待机
                        return StandardBrush.StandByBrush();
                    case EmStateType.Working: //工作
                        return StandardBrush.WorkingBrush();
                    case EmStateType.Error: //故障
                        return StandardBrush.ErrorBrush();
                    case EmStateType.OffLine:
                        return StandardBrush.OffLineBrush();
                    default:
                        return StandardBrush.OffLineBrush();
                }
            }
        }

        public override Brush BordBrush
        {
            get
            {
                switch (StateType)
                {

                    case EmStateType.StandBy: //待机
                        return StandardBrush.StandByBrush();
                    case EmStateType.Working: //工作
                        return StandardBrush.WorkingBrush();
                    case EmStateType.Error: //故障
                        return StandardBrush.ErrorBrush();
                    case EmStateType.OffLine:
                        return StandardBrush.OffLineBrush();
                    default:
                        return StandardBrush.OffLineBrush();
                        //case EmStateType.StandBy: //待机
                        //    return Brushes.Yellow;
                        //case EmStateType.Working: //工作
                        //    return Brushes.Green;
                        //case EmStateType.Error: //故障
                        //    return Brushes.Red;
                        //case EmStateType.OffLine:
                        //    return Brushes.Gray;
                        //default:
                        //    return Brushes.Gray;
                }
            }
        }
    }

    public enum EmStateType
    {
        /// <summary>
        /// 离线
        /// </summary>
        OffLine = 0,
        /// <summary>
        /// 待机
        /// </summary>
        StandBy = 1,
        /// <summary>
        /// 工作
        /// </summary>
        Working = 2,
        /// <summary>
        /// 故障
        /// </summary>
        Error = 3,
    }

    /// <summary>
    /// 标准刷子
    /// </summary>
    public class StandardBrush
    {

        public static Brush OffLineBrush()
        {
            GradientStopCollection colors = new GradientStopCollection();
            GradientStop min = new GradientStop()
            {
                Color = (Color) ColorConverter.ConvertFromString("#FF4D4D4D"),
                Offset = 0,
            };
            colors.Add(min);
            GradientStop max = new GradientStop()
            {
                Color = (Color) ColorConverter.ConvertFromString("#FF979797"),
                Offset = 1,
            };
            colors.Add(max);

            LinearGradientBrush brush = new LinearGradientBrush(colors)
            {
                StartPoint = new Point(0.5, 0),
                EndPoint = new Point(0.5, 1),
            };
            return brush;
        }

        public static Brush StandByBrush()
        {
            GradientStopCollection colors = new GradientStopCollection();
            GradientStop min = new GradientStop()
            {
                Color = (Color)ColorConverter.ConvertFromString("Yellow"),
                Offset = 0,
            };
            colors.Add(min);
            GradientStop max = new GradientStop()
            {
                Color = (Color)ColorConverter.ConvertFromString("White"),
                Offset = 1,
            };
            colors.Add(max);

            LinearGradientBrush brush = new LinearGradientBrush(colors)
            {
                StartPoint = new Point(0.5, 0),
                EndPoint = new Point(0.5, 1),
            };
            return brush;
        }

        public static Brush WorkingBrush()
        {
            GradientStopCollection colors = new GradientStopCollection();
            GradientStop min = new GradientStop()
            {
                Color = (Color)ColorConverter.ConvertFromString("Green"),
                Offset = 0,
            };
            colors.Add(min);
            GradientStop max = new GradientStop()
            {
                Color = (Color)ColorConverter.ConvertFromString("White"),
                Offset = 1,
            };
            colors.Add(max);

            LinearGradientBrush brush = new LinearGradientBrush(colors)
            {
                StartPoint = new Point(0.5, 0),
                EndPoint = new Point(0.5, 1),
            };
            return brush;
        }
        public static Brush ErrorBrush()
        {
            GradientStopCollection colors = new GradientStopCollection();
            GradientStop min = new GradientStop()
            {
                Color = (Color)ColorConverter.ConvertFromString("Red"),
                Offset = 0,
            };
            colors.Add(min);
            GradientStop max = new GradientStop()
            {
                Color = (Color)ColorConverter.ConvertFromString("White"),
                Offset = 1,
            };
            colors.Add(max);

            LinearGradientBrush brush = new LinearGradientBrush(colors)
            {
                StartPoint = new Point(0.5, 0),
                EndPoint = new Point(0.5, 1),
            };
            return brush;
        }
    }
}
