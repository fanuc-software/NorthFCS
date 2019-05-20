#region USINGs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#endregion

namespace BFM.WPF.Base.Controls
{
    /*******************************************
     *
     *  货架平面展示控件
     *  1.首先设置货架的TotalColumn（总列数）、TotalLayer(总层数)、Direction（方向 0：正向 => 1层在下面；1：反向 => 1层在上面）
     *  2.将货物的颜色或者图片写入到GoodsColors/GoodsImages中
     *
     *******************************************/

    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class SingleRowShelf : UserControl, IDisposable
    {
        /// <summary>
        /// 物料颜色管理
        /// </summary>
        public static Dictionary<string, Brush> GoodsColors = new Dictionary<string, Brush>();

        /// <summary>
        /// 物料图片管理
        /// </summary>
        public static Dictionary<string, ImageSource> GoodsImages = new Dictionary<string, ImageSource>();

        /// <summary>
        /// 货位单元
        /// </summary>
        private List<AllocationItem> AllocationItems = new List<AllocationItem>();
        
        /// <summary>
        /// 货架形状
        /// </summary>
        private DrawingGroup ShelfSharp = new DrawingGroup();

        /// <summary>
        /// 当前选中的货位
        /// </summary>
        public AllocationItem CurSelectedAllo { get; private set; }

        public SingleRowShelf()
        {
            this.InitializeComponent();

            PreviewMouseUp += this.Shelf_MouseUp;
        }

        #region Delegates

        public delegate void MyMouseClickEventHandler(
            object sender, ShelfClickMouseEventArg args);

        #endregion

        public event MyMouseClickEventHandler MyMouseClickEvent;

        #region Properties

        #region Properties.TotalColumn 总列数

        public static readonly DependencyProperty TotalColumnProperty =
            DependencyProperty.Register("TotalColumn",
                typeof(int),
                typeof(SingleRowShelf),
                new FrameworkPropertyMetadata(1, TotalColumnChanged));

        private static void TotalColumnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SingleRowShelf shelf = d as SingleRowShelf;
            shelf?.RefreshShelf();
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

        #region Properties.TotalLayer 总层数

        public static readonly DependencyProperty TotalLayerProperty =
            DependencyProperty.Register("TotalLayer",
                typeof(int),
                typeof(SingleRowShelf),
                new FrameworkPropertyMetadata(1, TotalLayerChanged));

        private static void TotalLayerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SingleRowShelf shelf = d as SingleRowShelf;
            shelf?.RefreshShelf();
        }

        /// <summary>
        /// 层
        /// </summary>
        public int TotalLayer
        {
            get { return (int)GetValue(TotalLayerProperty); }
            set { SetValue(TotalLayerProperty, value); }
        }

        #endregion

        #region Properties.BorderThick 厚度

        public static readonly DependencyProperty BorderThickProperty =
            DependencyProperty.Register("BorderThick",
                typeof(double),
                typeof(SingleRowShelf),
                new FrameworkPropertyMetadata(2.0, BorderThickChanged));

        private static void BorderThickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SingleRowShelf shelf = d as SingleRowShelf;
            shelf?.RefreshShelf();
        }

        /// <summary>
        /// 货架厚度
        /// </summary>
        public double BorderThick
        {
            get { return (double)GetValue(BorderThickProperty); }
            set { SetValue(BorderThickProperty, value); }
        }

        #endregion

        #region Properties.BorderColor 货架颜色

        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.Register("BorderColor",
                typeof(Brush),
                typeof(SingleRowShelf),
                new FrameworkPropertyMetadata(Brushes.SaddleBrown, BorderColorChanged));

        private static void BorderColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SingleRowShelf shelf = d as SingleRowShelf;
            shelf?.RefreshShelf();
        }

        /// <summary>
        /// 货架颜色
        /// </summary>
        public Brush BorderColor
        {
            get { return (Brush)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        #endregion

        #region Properties.Direction  方向

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction",
                typeof(int),
                typeof(SingleRowShelf),
                new FrameworkPropertyMetadata(0, DirectionChanged));

        private static void DirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SingleRowShelf shelf = d as SingleRowShelf;
            shelf?.RefreshShelf();
        }

        /// <summary>
        /// 方向 0：正向 => 1层在下面；1：反向 => 1层在上面
        /// </summary>
        public int Direction
        {
            get { return (int)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        #endregion

        #endregion

        #region 属性

        public Thickness InnerPadding { get; set; } = new Thickness(2);

        private double LayerHeight { get; set; }

        private double ColumnWidth { get; set; }

        #endregion

        #region 刷新货架本身

        /// <summary>
        /// 刷新货架
        /// </summary>
        private void RefreshShelf()
        {
            this.ColumnWidth = (ActualWidth - this.InnerPadding.Top -
                                this.InnerPadding.Bottom) / this.TotalColumn;
            this.LayerHeight = (ActualHeight - this.InnerPadding.Left -
                                this.InnerPadding.Right) / this.TotalLayer;
            if (ColumnWidth <= 0 || LayerHeight <= 0)
            {
                return;
            }
            this.DrawShelf();
            DrawEmptyAllocation();
            this.GeometryToImage();
        }

        private void GeometryToImage()
        {
            DrawingImage geoimage = new DrawingImage();
            geoimage.Drawing = ShelfSharp;
            Image image = new Image()
            {
                Source = geoimage, Stretch = Stretch.None, HorizontalAlignment = HorizontalAlignment.Left
            };
            cvMain.Children.Clear();
            cvMain.Children.Add(image);

            List<AllocationItem> delItems = new List<AllocationItem>();

            foreach (var item in AllocationItems)
            {
                if ((item.Column > TotalColumn) || (item.Layer > TotalLayer))
                {
                    delItems.Add(item);
                }
                else
                {
                    cvMain.Children.Add(item);
                }
            }

            foreach (var delItem in delItems)
            {
                AllocationItems.Remove(delItem);
            }
        }
        
        private void DrawShelf()
        {
            GeometryDrawing gdshelf = new GeometryDrawing();
            GeometryGroup ggshelf = new GeometryGroup();
            LineGeometry lng;
            Point pt = new Point();

            //hori
            for (int i = 0; i <= this.TotalLayer; i++)
            {
                lng = new LineGeometry();
                pt.X = this.InnerPadding.Left;
                pt.Y = this.InnerPadding.Top + (i * this.LayerHeight);
                lng.StartPoint = pt;
                pt.X = ActualWidth - this.InnerPadding.Right;
                lng.EndPoint = pt;

                ggshelf.Children.Add(lng);
            }

            //vtch
            for (int i = 0; i <= this.TotalColumn; i++)
            {
                lng = new LineGeometry();
                pt.X = this.InnerPadding.Left + (i * this.ColumnWidth);
                pt.Y = this.InnerPadding.Top;
                lng.StartPoint = pt;
                pt.Y = ActualHeight - this.InnerPadding.Bottom;
                lng.EndPoint = pt;

                ggshelf.Children.Add(lng);
            }

            gdshelf.Geometry = ggshelf;
            gdshelf.Pen = new Pen(BorderColor, BorderThick);

            ShelfSharp.Children.Clear();
            ShelfSharp.Children.Add(gdshelf);
        }

        private void DrawEmptyAllocation()
        {
            for (int i = 0; i < this.TotalColumn; i++) //列
            {
                for (int j = 0; j < this.TotalLayer; j++)  //层
                {
                    int column = i + 1;
                    int layer = (Direction == 0)? (TotalLayer - j) : (j + 1); //方向 0：正向 => 1层在下面；1：反向 => 1层在上面
                    var item = AllocationItems.FirstOrDefault(c => c.Column == column && c.Layer == layer);
                    if (item == null)
                    {
                        item = new AllocationItem()
                        {
                            Column = column, Layer = layer, 
                            PalletInfo = column + "-" + layer
                        };

                        AllocationItems.Add(item);
                    }

                    item.Width = ColumnWidth <= 2 ? 1 : ColumnWidth - 2;
                    item.Height = LayerHeight <= 2 ? 1 : LayerHeight - 2;

                    Canvas.SetLeft(item, this.InnerPadding.Left + (i * this.ColumnWidth));
                    Canvas.SetTop(item, this.InnerPadding.Top + (j * this.LayerHeight));
                }
            }
        }

        #endregion 

        /// <summary>
        /// 获取货位
        /// </summary>
        /// <param name="column"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public AllocationItem GetAlloItemByAddr(int column, int layer)
        {
            return AllocationItems.FirstOrDefault(c => c.Column == column && c.Layer == layer);
        }

        /// <summary>
        /// 设置刷新状态信息
        /// </summary>
        /// <param name="usefulState">0：不可用；1：可用</param>
        public void SetAutoRefreshState(int usefulState)
        {
            foreach (var item in AllocationItems)
            {
                item.UsefulState = usefulState;
            }
        }

        #region 刷新货物

        /// <summary>
        /// 按照货物名称进行更新
        /// </summary>
        /// <param name="column">列</param>
        /// <param name="layer">层</param>
        /// <param name="alloinfo">货位描述</param>
        /// <param name="goodsno">货物编号</param>
        /// <param name="alloproportion">占比 按照1000取余 最大100</param>
        /// <param name="palletinfo">托盘描述；空 时表示没有托盘</param>
        public void RefreshAlloInfo(int column, int layer, string alloinfo, string goodsno, int alloproportion, string palletinfo = null)
        {
            var item = AllocationItems.FirstOrDefault(c => c.Column == column && c.Layer == layer);
            if (item == null) return;
            
            if (GoodsImages.ContainsKey(goodsno)) //优先显示图片
            {
                item.SetValueByImage(alloinfo, GoodsImages[goodsno], palletinfo);
            }
            else if (GoodsColors.ContainsKey(goodsno)) //按照颜色进行更新
            {
                item.SetValueByColor(alloinfo, GoodsColors[goodsno], palletinfo);
            }
             
            else
            {
                item.SetValueByColor(alloinfo, (alloinfo=="" || alloinfo=="空")? AllocationItem.EmptyAlloColor : AllocationItem.FullAlloColor, palletinfo);
            }

            item.SetProportion(alloproportion);
        }
        
        /// <summary>
        /// 按照图片进行更新
        /// </summary>
        /// <param name="column"></param>
        /// <param name="layer"></param>
        /// <param name="alloinfo"></param>
        /// <param name="image"></param>
        /// <param name="alloproportion">占比 按照1000取余 最大100</param>
        /// <param name="palletinfo"></param>
        public void RefreshAlloInfo(int column, int layer, string alloinfo, ImageSource image, int alloproportion, string palletinfo = null)
        {
            var item = AllocationItems.FirstOrDefault(c => c.Column == column && c.Layer == layer);
            if (item == null) return;

            if (palletinfo == null)
            {
                palletinfo = item.PalletInfo;
            }
            item.SetValueByImage(alloinfo, image, palletinfo);
            item.SetProportion(alloproportion);
        }

        #endregion

        #region 事件

        private void Shelf_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //get column and floor
            Point pt = e.GetPosition(this);
            int column = 0, layer = 0;
            if (pt.X > this.InnerPadding.Left &&
                pt.X < (ActualWidth - this.InnerPadding.Right))
            {
                column = 1 + (int) ((pt.X - this.InnerPadding.Left) / this.ColumnWidth);
            }
            if (pt.Y > this.InnerPadding.Top &&
                pt.Y < (ActualHeight - this.InnerPadding.Bottom))
            {
                layer = 1 + (int) ((pt.Y - this.InnerPadding.Top) / this.LayerHeight);
            }

            if (Direction == 0) //方向 0：正向 => 1层在下面；1：反向 => 1层在上面
            {
                layer = TotalLayer + 1 - layer;
                if (layer < 0) layer = 0;
            }

            ShelfClickMouseEventArg se = new ShelfClickMouseEventArg(
                e.MouseDevice, e.Timestamp, MouseButton.Left);
            se.TotalColumn = column; 
            se.TotalLayer = layer;

            foreach (AllocationItem item in AllocationItems)
            {
                if ((item.Column == column) && (item.Layer == layer))
                {
                    CurSelectedAllo = item;
                    item.IsSelected = true;
                }
                else
                {
                    item.IsSelected = false;
                }
            }
            
            this.MyMouseClickEvent?.Invoke(this, se); //raise the event
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RefreshShelf();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshShelf();
        }

        #endregion

        public void Dispose()
        {
            foreach (var item in AllocationItems)
            {
                item.Dispose();
            }
        }
    }

    #region 单击事件参数

    public class ShelfClickMouseEventArg : MouseButtonEventArgs
    {
        public ShelfClickMouseEventArg(MouseDevice md, int i, MouseButton mb)
            : base(md, i, mb)
        {
        }

        public ShelfClickMouseEventArg(
            MouseDevice md, int i, MouseButton mb, StylusDevice sd) : base(md, i, mb, sd)
        {
        }

        /// <summary>
        /// 总列
        /// </summary>
        public int TotalColumn { get; set; }

        /// <summary>
        /// 总层
        /// </summary>
        public int TotalLayer { get; set; }
    }

   #endregion
}