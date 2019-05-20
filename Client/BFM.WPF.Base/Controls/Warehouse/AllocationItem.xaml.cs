using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BFM.Common.Base.PubData;

namespace BFM.WPF.Base.Controls
{
    /// <summary>
    /// AllocationItem.xaml 的交互逻辑
    /// </summary>
    public partial class AllocationItem : UserControl, IDisposable
    {
        private bool bDisposabled = false;
        private Action<Brush, string, Visibility> _showInfo;  //显示界面信息

        #region 自定义颜色

        /// <summary>
        /// 空货位颜色 =>白色
        /// </summary>
        public static Brush EmptyAlloColor = Brushes.White;

        /// <summary>
        /// 货位上存放物料少的颜色
        /// </summary>
        public static Brush LittleAlloColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9000FF00"));

        /// <summary>
        /// 货位上存放物料多的颜色
        /// </summary>
        public static Brush ManyAlloColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D0FFFF00"));

        /// <summary>
        /// 满货位颜色
        /// </summary>
        public static Brush FullAlloColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#80FF0000"));

        /// <summary>
        /// 不可用货位颜色
        /// </summary>
        public static Brush UnUseAlloColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AA808080"));

        /// <summary>
        /// 默认托盘颜色
        /// </summary>
        public static Brush DefPalletColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#900000FF"));

        /// <summary>
        /// 正在入库的颜色
        /// </summary>
        public static Brush WorkingInColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#60FFFF00"));

        /// <summary>
        /// 正在出库的颜色
        /// </summary>
        public static Brush WorkingOutColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#60FF00FF"));

        /// <summary>
        /// 入库锁定的颜色
        /// </summary>
        public static Brush InLockColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#60FFFF00"));
       
        /// <summary>
        /// 出库锁定的颜色
        /// </summary>
        public static Brush OutLockColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#60FF00FF"));

        #endregion

        #region construct

        public AllocationItem(bool bPalletManage = false)
        {
            InitializeComponent();

            PalletState = bPalletManage ? 0 : -1;
            ShowTitle = true;
            bTitle.Width = bdMain.ActualWidth > 2 ? bdMain.ActualWidth - 2 : 0;

            _showInfo = SetAllocStateColor;
        }

        #endregion

        #region private 事件

        private void ThreadShowInOutState()
        {
            bool bShowStateColor = false;
            
            while ((!CBaseData.AppClosing) && (!bDisposabled))
            {
                Thread.Sleep(800);

                if (UsefulState == 0)  //不可用情况下退出
                {
                    bShowStateColor = false;
                    continue;
                }

                if ((WorkState != 1) && (WorkState != 2))
                {
                    return;
                }

                bShowStateColor = !bShowStateColor;

                try
                {
                    #region 显示入出库状态 闪烁

                    if (!bShowStateColor)
                    {
                        Dispatcher.BeginInvoke(_showInfo, Brushes.Transparent, "", Visibility.Collapsed);
                    }
                    else if (WorkState == 1) //正在入库
                    {
                        Dispatcher.BeginInvoke(_showInfo, WorkingInColor, "正在入库", Visibility.Visible);
                    }
                    else if (WorkState == 2) //正在出库
                    {
                        Dispatcher.BeginInvoke(_showInfo, WorkingOutColor, "正在出库", Visibility.Visible);
                    }

                    #endregion
                }
                catch (Exception e)
                {
                    Console.WriteLine("AllocationItem.ThreadShowWorkState error:" + e);
                }

            }

        }

        /// <summary>
        /// 设置显示
        /// </summary>
        /// <param name="brush"></param>
        /// <param name="content"></param>
        /// <param name="visibility"></param>
        private void SetAllocStateColor(Brush brush, string content, Visibility visibility)
        {
            bAlloState.Background = brush;
            lAlloState.Content = content;
            bAlloState.Visibility = visibility;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 唯一编号
        /// </summary>
        public string PKNO { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        public int Column { get; set; } 

        /// <summary>
        /// 层
        /// </summary>
        public int Layer { get; set; }

        /// <summary>
        /// 货位占比
        /// </summary>
        public int Proportion { get; set; } = 0;

        /// <summary>
        /// 0：不可用；1：可用
        /// </summary>
        public int UsefulState { get; set; } = 1;

        /// <summary>
        /// 工作状态；0：无工作；1：正在入库；2：正在出库；3：入库锁定；4：出库锁定
        /// </summary>
        public int WorkState { get; set; } = 0;

        #endregion

        #region Properties

        #region Properties.ShowTitle

        public static readonly DependencyProperty ShowTitleProperty =
            DependencyProperty.Register("ShowTitle",
                typeof(bool),
                typeof(AllocationItem),
                new FrameworkPropertyMetadata(true, ShowTitleChanged));

        private static void ShowTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AllocationItem allocationItem = d as AllocationItem;
            if (allocationItem == null) return;

            allocationItem.rdShowTitle.Height = allocationItem.ShowTitle ? new GridLength(23) : new GridLength(0);
        }

        /// <summary>
        /// 显示标题栏
        /// </summary>
        public bool ShowTitle
        {
            get { return (bool)GetValue(ShowTitleProperty); }
            set { SetValue(ShowTitleProperty, value); }
        }

        #endregion

        #region Properties.TitleColor

        public static readonly DependencyProperty TitleColorProperty =
            DependencyProperty.Register("TitleColor",
                typeof(Brush),
                typeof(AllocationItem),
                new FrameworkPropertyMetadata(EmptyAlloColor, TitleColorChanged));

        private static void TitleColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AllocationItem allocationItem = d as AllocationItem;
            if (allocationItem == null) return;

            allocationItem.bTitle.Background = allocationItem.TitleColor;
        }

        /// <summary>
        /// 标题颜色
        /// </summary>
        public Brush TitleColor
        {
            get { return (Brush)GetValue(TitleColorProperty); }
            set { SetValue(TitleColorProperty, value); }
        }

        #endregion

        #region Properties.TitleInfo

        public static readonly DependencyProperty TitleInfoProperty =
            DependencyProperty.Register("TitleInfo",
                typeof(string),
                typeof(AllocationItem),
                new FrameworkPropertyMetadata("空", TitleInfoChanged));

        private static void TitleInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AllocationItem allocationItem = d as AllocationItem;
            if (allocationItem == null) return;

            allocationItem.tbTitleInfo.Text = allocationItem.TitleInfo;
        }

        /// <summary>
        /// 标题信息
        /// </summary>
        public string TitleInfo
        {
            get { return (string)GetValue(TitleInfoProperty); }
            set { SetValue(TitleInfoProperty, value); }
        }

        #endregion
        
        #region Properties.AlloColor

        public static readonly DependencyProperty AlloColorProperty =
            DependencyProperty.Register("AlloColor",
                typeof(Brush),
                typeof(AllocationItem),
                new FrameworkPropertyMetadata(Brushes.White, AlloColorChanged));

        private static void AlloColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AllocationItem allocationItem = d as AllocationItem;
            if (allocationItem == null) return;

            allocationItem.bAllo.Background = allocationItem.AlloColor;
        }

        /// <summary>
        /// 货位颜色
        /// </summary>
        public Brush AlloColor
        {
            get { return (Brush)GetValue(AlloColorProperty); }
            set { SetValue(AlloColorProperty, value); }
        }

        #endregion

        #region Properties.AllocImage

        public static readonly DependencyProperty AlloImageProperty =
            DependencyProperty.Register("AlloImage",
                typeof(ImageSource),
                typeof(AllocationItem),
                new FrameworkPropertyMetadata(null, AlloImageChanged));

        private static void AlloImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AllocationItem allocationItem = d as AllocationItem;
            if (allocationItem == null) return;

            allocationItem.imgAllo.Source = allocationItem.AlloImage;
        }

        /// <summary>
        /// 货位图片
        /// </summary>
        public ImageSource AlloImage
        {
            get { return (ImageSource)GetValue(AlloImageProperty); }
            set { SetValue(AlloImageProperty, value); }
        }

        #endregion

        #region Properties.PalletState

        public static readonly DependencyProperty PalletStateProperty =
            DependencyProperty.Register("PalletState",
                typeof(int),
                typeof(AllocationItem),
                new FrameworkPropertyMetadata(0, PalletStateChanged));

        private static void PalletStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AllocationItem allocationItem = d as AllocationItem;
            if (allocationItem == null) return;

            allocationItem.rdPalletHeight.Height = (allocationItem.PalletState >= 0) ? new GridLength(27) : new GridLength(0);

            allocationItem.PalletColor = (allocationItem.PalletState == 1) ? DefPalletColor : EmptyAlloColor;
        }

        /// <summary>
        /// 托盘状态；-1：不显示托盘管理；0：无托盘；1：有托盘
        /// </summary>
        public int PalletState
        {
            get { return (int)GetValue(PalletStateProperty); }
            set { SetValue(PalletStateProperty, value); }
        }

        #endregion

        #region Properties.PalletInfo

        public static readonly DependencyProperty PalletInfoProperty =
            DependencyProperty.Register("PalletInfo",
                typeof(string),
                typeof(AllocationItem),
                new FrameworkPropertyMetadata("空", PalletInfoChanged));

        private static void PalletInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AllocationItem allocationItem = d as AllocationItem;
            if (allocationItem == null) return;

            allocationItem.tbPalletInfo.Text = allocationItem.PalletInfo;
        }

        /// <summary>
        /// 托盘信息
        /// </summary>
        public string PalletInfo
        {
            get { return (string)GetValue(PalletInfoProperty); }
            set { SetValue(PalletInfoProperty, value); }
        }

        #endregion

        #region Properties.PalletColor

        public static readonly DependencyProperty PalletColorProperty =
            DependencyProperty.Register("PalletColor",
                typeof(Brush),
                typeof(AllocationItem),
                new FrameworkPropertyMetadata(DefPalletColor, PalletColorChanged));

        private static void PalletColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AllocationItem allocationItem = d as AllocationItem;
            if (allocationItem == null) return;

            allocationItem.bPallet.Background = allocationItem.PalletColor;

            if (allocationItem.PalletColor == Brushes.White) //白色
            {
                allocationItem.rdPalletHeight.Height = new GridLength(23);
                allocationItem.bPallet.Visibility = Visibility.Collapsed;
                //allocationItem.bPalletContent.Margin = new Thickness(0);
            }
        }

        /// <summary>
        /// 托盘颜色
        /// </summary>
        public Brush PalletColor
        {
            get { return (Brush)GetValue(PalletColorProperty); }
            set { SetValue(PalletColorProperty, value); }
        }

        #endregion
        
        #region Properties.IsSelected

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected",
                typeof(bool),
                typeof(AllocationItem),
                new FrameworkPropertyMetadata(false, IsSelectedChanged));

        private static void IsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AllocationItem allocationItem = d as AllocationItem;
            if (allocationItem == null) return;

            allocationItem.bdMain.BorderThickness = allocationItem.IsSelected ? new Thickness(2) : new Thickness(0);
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        #endregion

        #endregion

        #region ** 设置占比 含 入出库状态 **

        /// <summary>
        /// 设置占比，最大100
        /// 按照1000取余
        /// 1000-1999表示正在入库；2000-2999表示正在出库
        /// 3000-3999表示入库锁定；4000-4999表示出库锁定
        /// </summary>
        /// <param proportion="">占比</param>
        public void SetProportion(int proportion)
        {
            Proportion = proportion;

            int workState = Proportion / 1000;  //设置入出库状态

            #region 设置工作颜色

            if (workState != WorkState)  
            {
                WorkState = workState;

                if (workState == 0)  //无工作状态
                {
                    SetUseAllo();
                }
                else if((workState == 1) || (workState == 2))  //正在入库、正在出库
                {
                    ThreadPool.QueueUserWorkItem(s => { ThreadShowInOutState(); }); //设置显示出入库状态的线程
                }
                else if (workState == 3) //入库锁定 
                {
                    SetAllocStateColor(InLockColor, "入库锁定", Visibility.Visible);
                }
                else if (workState == 4) //出库锁定
                {
                    SetAllocStateColor(OutLockColor, "出库锁定", Visibility.Visible);
                }
            }

            #endregion

            if (proportion < 0) //禁用
            {
                SetUnUseAllo();
            }
            else if (proportion == 0) //空
            {

                TitleInfo = "空";
                TitleColor = EmptyAlloColor;
                bTitle.Width = bdMain.ActualWidth > 2 ? bdMain.ActualWidth - 2 : 0;
            }
            else
            {
                #region 设置占比

                int prop = proportion % 1000;
                if (prop >= 100)
                {
                    TitleColor = FullAlloColor;
                    bTitle.Width = bdMain.ActualWidth > 2 ? bdMain.ActualWidth - 2 : 0;
                }
                else
                {
                    TitleColor = prop >= 80 ? ManyAlloColor : LittleAlloColor;

                    bTitle.Width = prop * (bdMain.ActualWidth > 2 ? bdMain.ActualWidth - 2 : 0) / 100;
                }

                #endregion
            }
        }

        #endregion

        public void SetValue(string alloinfo, string palletinfo = null)
        {
            TitleInfo = string.IsNullOrEmpty(alloinfo) ? "空" : alloinfo;

            if (!string.IsNullOrEmpty(palletinfo))
            {
                PalletInfo = palletinfo;

                if (palletinfo == "空")
                {
                    PalletColor = EmptyAlloColor;
                }
                else
                {
                    PalletColor = DefPalletColor;
                }
            }
        }

        /// <summary>
        /// 按照颜色更新货位信息
        /// </summary>
        /// <param name="alloinfo"></param>
        /// <param name="allocolor"></param>
        /// <param name="palletinfo">默认不管理，空 表示没有托盘</param>
        public void SetValueByColor(string alloinfo, Brush allocolor, string palletinfo = null)
        {
            SetValue(alloinfo, palletinfo);

            imgAllo.Source = null;
            AlloColor = allocolor;
        }

        /// <summary>
        /// 按照颜色更新货位信息
        /// </summary>
        /// <param name="alloinfo"></param>
        /// <param name="proportion"></param>
        /// <param name="allocolor"></param>
        /// <param name="palletinfo">默认不管理，空 表示没有托盘</param>
        public void SetValueByColor(string alloinfo, int proportion, Brush allocolor, string palletinfo = null)
        {
            SetValueByColor(alloinfo, allocolor, palletinfo);
            SetProportion(proportion);
        }

        /// <summary>
        /// 按照图片更新货位信息
        /// </summary>
        /// <param name="alloinfo">货位标题</param>
        /// <param name="image">货位图片</param>
        /// <param name="palletinfo">默认不管理，空 表示没有托盘</param>
        public void SetValueByImage(string alloinfo, ImageSource image, string palletinfo = null)
        {
            SetValue(alloinfo, palletinfo);
            AlloImage = image;
        }

        /// <summary>
        /// 按照图片更新货位信息
        /// </summary>
        /// <param name="alloinfo">货位标题</param>
        /// <param name="image">货位图片</param>
        /// <param name="palletinfo">默认不管理，空 表示没有托盘</param>
        public void SetValueByImage(string alloinfo, int proportion, ImageSource image, string palletinfo = null)
        {
            SetValueByImage(alloinfo, image, palletinfo);
            SetProportion(proportion);
        }

        #region 货位的禁用/启用

        /// <summary>
        /// 禁用货位
        /// </summary>
        public void SetUnUseAllo()
        {
            UsefulState = 0; //已禁用

            SetAllocStateColor(UnUseAlloColor, "已禁用", Visibility.Visible);
        }

        /// <summary>
        /// 解除禁用
        /// </summary>
        public void SetUseAllo()
        {
            UsefulState = 1;

            SetAllocStateColor(Brushes.Transparent, "", Visibility.Collapsed);
        }

        #endregion

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetProportion(Proportion);
        }

        public void Dispose()
        {
            bDisposabled = true;
        }
    }
}
