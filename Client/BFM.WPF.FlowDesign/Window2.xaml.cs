using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BFM.WPF.FlowDesign.Controls;

namespace CSharp.WPF.FlowDesign
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();

        }

        List<UIElement> _lines = new List<UIElement>();
        List<Point> _points = new List<Point>();
        private Point _drawPoint = new Point();

        private bool _bCross = false;
        
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //base.DragMove();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //添加
            Brush background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("White"));
            canvas1.AddDragImage("", new Size(100, 100), new Point(50, 50), new BitmapImage(new Uri(tbImage.Text)), background, background);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbContentWidth.Text = canvas1.ActualWidth.ToString();
            tbContentHeight.Text = canvas1.ActualHeight.ToString();
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            DragThumb di = canvas1.CurSelectedDragThumb;
            if (di == null) return;
            di.Source = new BitmapImage(new Uri(tbImage.Text));
        }

        private void button_Copy2_Click(object sender, RoutedEventArgs e)
        {
            if (canvas1.CurSelectedDragThumb == null) return;

            if (MessageBox.Show("确定要删除组件吗？", "删除控件", MessageBoxButton.OKCancel, MessageBoxImage.Question) !=
                MessageBoxResult.OK) return;

            canvas1.RemoveCurSelected();
        }


        private void button_Copy1_Click(object sender, RoutedEventArgs e)
        {
            canvas1.SendToBack(canvas1.CurSelectedDragThumb);
        }

        private void button_Copy3_Click(object sender, RoutedEventArgs e)
        {
            canvas1.BringToFront(canvas1.CurSelectedDragThumb);

        }

        private void button_Copy4_Click(object sender, RoutedEventArgs e)
        {
            canvas1.BackOnce(canvas1.CurSelectedDragThumb);

        }

        private void button_Copy5_Click(object sender, RoutedEventArgs e)
        {
            canvas1.FrontOnce(canvas1.CurSelectedDragThumb);

        }

        private void canvas1_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            Point p0 = new Point(0, 0);
            Point p1 = new Point(0, 0);
            Point p2 = new Point(0, 0);
            Point p3 = new Point(0, 0);

            int x1 = 3;
            int y1 = 6;
            int x2 = 9;
            int y2 = 12;

            p0.X = 0; p0.Y = 255;
            p1.X = x1; p1.Y = 255 - y1;
            p2.X = x2; p2.Y = 255 - y2;
            p3.X = 255; p3.Y = 0;

            Line lx = new Line();//X轴
            lx.X1 = 0; lx.X2 = 255; lx.Y1 = 255; lx.Y2 = 255;
            lx.StrokeThickness = 1;
            lx.Stroke = System.Windows.Media.Brushes.Black;
            Line ly = new Line();//Y轴
            ly.X1 = 0; ly.X2 = 0; ly.Y1 = 0; ly.Y2 = 255;
            ly.StrokeThickness = 1;
            ly.Stroke = System.Windows.Media.Brushes.Black;

            Polyline pl = new Polyline();//绘制折线
            PointCollection collection = new PointCollection();
            collection.Add(new Point(p0.X, p0.Y));
            collection.Add(new Point(p1.X, p1.Y));
            collection.Add(new Point(p2.X, p2.Y));
            collection.Add(new Point(p3.X, p3.Y));
            pl.Points = collection;
            pl.Stroke = new SolidColorBrush(Colors.Red);
            pl.StrokeThickness = 1;
            canvas1.Children.Add(lx);
            canvas1.Children.Add(ly);
            canvas1.Children.Add(pl);
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            Point p0 = new Point(0, 0);
            Polyline pl = new Polyline();//绘制折线
            PointCollection collection = new PointCollection();
            pl.Name = "Line" + canvas1.Children.Count;
            pl.Stroke = new SolidColorBrush(Colors.Red);
            pl.StrokeThickness = Convert.ToInt32(tbLineWidth.Text);
            collection.Add(p0);
            pl.Points = collection;
            canvas1.Children.Add(pl);

            _points.Clear();
            _points.Add(p0);
            _lines.Add(pl);
        }

        private void canvas1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_lines.Count <= 0) return;
            Polyline pl = (Polyline)_lines.Last();
            PointCollection collection = new PointCollection();
            Point lastPoint = new Point(0, 0);
            foreach (Point point in _points)
            {
                collection.Add(point);
                lastPoint = new Point(point.X, point.Y); ;
            }
            Point p = new Point(e.GetPosition(canvas1).X, e.GetPosition(canvas1).Y);
            if (_bCross) //正交
            {
                if (Math.Abs(p.X - lastPoint.X) > Math.Abs(p.Y - lastPoint.Y))  //横坐标比纵坐标大，则纵坐标不变
                {
                    p.Y = lastPoint.Y;
                }
                else if (Math.Abs(p.X - lastPoint.X) < Math.Abs(p.Y - lastPoint.Y))
                {
                    p.X = lastPoint.X;
                }
            }
            _drawPoint = new Point(p.X, p.Y);

            collection.Add(p);
            pl.Points = collection;
        }

        private void canvas1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_lines.Count <= 0) return;
            _points.Add(_drawPoint);
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            _bCross = !_bCross;
            canvas1.BOrthogonal = !canvas1.BOrthogonal;
            canvas1.LcDirection = 1;
        }
        private void button9_Click(object sender, RoutedEventArgs e)
        {
            _bCross = !_bCross;
            canvas1.BOrthogonal = !canvas1.BOrthogonal;
            canvas1.LcDirection = 0;
        }

        private void canvas1_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed) return;

            double newZoom = canvas1.Zoom;

            if (e.Delta > 0) //放大
            {
                newZoom *= e.Delta * 0.01;
            }
            else if (e.Delta < 0) //缩小
            {
                newZoom /= -e.Delta * 0.01;
            }

            if (Math.Abs(newZoom - 1) < 0.0001)
            {
                newZoom = 1;
            }

            canvas1.Zoom = newZoom;
            this._zoom.Value = newZoom * 10;
        }

        private void _zoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            canvas1.Zoom = e.NewValue / 10;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //保存
            if (canvas1.Save("config.xml"))
            {
                MessageBox.Show("保存成功.", "保存");
            }
            else
            {
                MessageBox.Show("保存失败！", "保存");
            }
        }

        private void btnIntial_Click(object sender, RoutedEventArgs e)
        {
            canvas1.RemoveAllDragThumb();
            canvas1.LoadFile("config.xml");
        }

        private void canvas1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void _zoom_2ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            canvas1.RenderTransform = new TranslateTransform(_zoom_Copy.Value, _zoom_Copy.Value);
        }
    }
}
