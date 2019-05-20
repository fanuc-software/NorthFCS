using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using BFM.WPF.FlowDesign.Controls;

namespace BFM.WPF.FlowDesign.MainCanvas
{
    public sealed partial class FlowContent : Canvas
    {
        #region 内部事件

        private bool _bSelectMode = false;

        private Point _SelectStartPoint = new Point();

        private List<Shape> SelectShapes = new List<Shape>();
        
        /// <summary>
        /// 清除所有选择的框
        /// </summary>
        private void ClearSelectShapes()
        {
            foreach (Shape shape in SelectShapes)
            {
                this.Children.Remove(shape);
            }
            SelectShapes.Clear();
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsReadOnly) return;

            if (_bSelectMode)
            {
                Point endPoint = e.GetPosition(this);
                SelectThumbByRect(endPoint);
            }
            else
            {
                ClearAllSelectedThumb();

                _bSelectMode = true;
                _SelectStartPoint = e.GetPosition(this);

                ClearSelectShapes();
            }
            this.Focus();
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (IsReadOnly) return;
            //选择范围内的
            if (_bSelectMode)
            {
                Point endPoint = e.GetPosition(this);
                SelectThumbByRect(endPoint);
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (IsReadOnly) return;
            ////选择范围内的
            //if (bSelectMode)
            //{
            //    Point endPoint = e.GetPosition(this);
            //    SelectThumbByRect(endPoint);
            //}
        }

        private void SelectThumbByRect(Point EndPoint)
        {
            if (IsReadOnly) return;
            if (!_bSelectMode)
            {
                return;
            }
            bMultiSelect = true;
            double minX = (_SelectStartPoint.X < EndPoint.X) ? _SelectStartPoint.X : EndPoint.X;
            double maxX = (_SelectStartPoint.X < EndPoint.X) ? EndPoint.X : _SelectStartPoint.X;

            double minY = (_SelectStartPoint.Y < EndPoint.Y) ? _SelectStartPoint.Y : EndPoint.Y;
            double maxY = (_SelectStartPoint.Y < EndPoint.Y) ? EndPoint.Y : _SelectStartPoint.Y;
            foreach (DragThumb thumb in DragThumbs)
            {
                if ((EndPoint.X < _SelectStartPoint.X) && (EndPoint.Y < _SelectStartPoint.Y)) //在范围内则选择
                {
                    if (((thumb.Position.X >= minX) && (thumb.Position.X <= maxX) &&
                         (thumb.Position.Y >= minY) && (thumb.Position.Y <= maxY)) ||

                        ((thumb.Position.X + thumb.Width >= minX) && (thumb.Position.X + thumb.Width <= maxX) &&
                         (thumb.Position.Y >= minY) && (thumb.Position.Y <= maxY)) ||

                        ((thumb.Position.X >= minX) && (thumb.Position.X <= maxX) &&
                         (thumb.Position.Y + thumb.Height >= minY) && (thumb.Position.Y + thumb.Height <= maxY)) ||

                        ((thumb.Position.X + thumb.Width >= minX) && (thumb.Position.X + thumb.Width <= maxX) &&
                         (thumb.Position.Y + thumb.Height >= minY) && (thumb.Position.Y + thumb.Height <= maxY)))
                    {
                        thumb.IsSelected = true;
                    }
                }
                else //全包含选择
                {
                    if ((thumb.Position.X >= minX) && (thumb.Position.X + thumb.Width <= maxX) &&
                        (thumb.Position.Y >= minY) && (thumb.Position.Y + thumb.Height <= maxY))
                    {
                        thumb.IsSelected = true;
                    }
                }
            }
            bMultiSelect = false;
            _bSelectMode = false;
            ClearSelectShapes();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (IsReadOnly) return;
            if (_bSelectMode)
            {
                Point endPoint = e.GetPosition(this);
                double width = Math.Abs(endPoint.X - _SelectStartPoint.X) - 1;
                double height = Math.Abs(endPoint.Y - _SelectStartPoint.Y) - 1;
                Rectangle rect = new Rectangle()
                {
                    Width = width < 0? 0: width,
                    Height = height < 0 ? 0 : height,
                    Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#880000FF")),
                    Fill = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#220000FF")),
                };
                Canvas.SetLeft(rect, (endPoint.X < _SelectStartPoint.X) ? endPoint.X + 1 : _SelectStartPoint.X);
                Canvas.SetTop(rect, (endPoint.Y < _SelectStartPoint.Y) ? endPoint.Y + 1 : _SelectStartPoint.Y);
                Canvas.SetZIndex(rect, this.Children.Count);

                ClearSelectShapes();
                this.Children.Add(rect);
                SelectShapes.Add(rect);
            }
            else
            {
                ClearSelectShapes();
            }
        }

        /// <summary>
        /// 是否按下Shift
        /// </summary>
        private bool BShift = false;
        /// <summary>
        /// 是否按下Ctrl
        /// </summary>
        public bool BCtrl = false;

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (IsReadOnly) return;
            switch (e.Key)
            {
                case Key.Escape:  //取消
                    BCtrl = false;
                    BShift = false;
                    bMultiSelect = false;
                    bResizeFormat = false;

                    if (_bSelectMode)  //已经选择
                    {
                        _bSelectMode = false;
                    }
                    ClearSelectShapes();
                    foreach (DragThumb thumb in DragThumbs)
                    {
                        thumb.IsEditText = false;
                    }
                    break;
                case Key.LeftCtrl:  //Ctrl
                    BCtrl = true;
                    bMultiSelect = true;
                    break;
                case Key.LeftShift: //
                    BShift = true;
                    bResizeFormat = true;
                    bMultiSelect = true;
                    break;
                case Key.Down:  //
                    double downSpan = BCtrl? 1: 5;
                    foreach (DragThumb thumb in SelectDragThumbs)
                    {
                        thumb.MoveBySpan(0, downSpan, false);
                    }
                    break;
                case Key.Up:
                    double upSpan = BCtrl ? -1 : -5;
                    foreach (DragThumb thumb in SelectDragThumbs)
                    {
                        thumb.MoveBySpan(0, upSpan, false);
                    }
                    break;
                case Key.Left:
                    double leftSpan = BCtrl ? -1 : -5;
                    foreach (DragThumb thumb in SelectDragThumbs)
                    {
                        thumb.MoveBySpan(leftSpan, 0, false);
                    }
                    break;
                case Key.Right:
                    double rightSpan = BCtrl ? 1 : 5;
                    foreach (DragThumb thumb in SelectDragThumbs)
                    {
                        thumb.MoveBySpan(rightSpan, 0, false);
                    }
                    break;
                case Key.Delete:
                    RemoveAllSelected();
                    break;
                case Key.A:
                    if (BCtrl)  //全选
                    {
                        foreach (DragThumb dt in DragThumbs)
                        {
                            dt.IsSelected = true;
                        }
                    }
                    break;
                default:
                    break;
            }
            //this.Focus();
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (IsReadOnly) return;
            switch (e.Key)
            {
                case Key.LeftCtrl:
                    BCtrl = false;
                    bMultiSelect = false;
                    break;
                case Key.LeftShift:
                    BShift = false;
                    bMultiSelect = false;
                    bResizeFormat = false;
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region 事件

        #region ItemClick

        public static readonly RoutedEvent ItemClickEvent =
            EventManager.RegisterRoutedEvent("ItemClick",
                        RoutingStrategy.Bubble,
                        typeof(RoutedPropertyChangedEventHandler<string>),
                        //typeof(RoutedEventHandler),
                        typeof(FlowContent));

        /// <summary>
        /// 元素点击事件 
        /// </summary>
        [Description("点击单个元素")]
        public event RoutedPropertyChangedEventHandler<string> ItemClick
        {
            //将路由事件添加路由事件处理程序
            add { AddHandler(ItemClickEvent, value); }
            //从路由事件处理程序中移除路由事件
            remove { RemoveHandler(ItemClickEvent, value); }
        }

        //public RoutedEventArgs : EventArgs

        #endregion
            
        #region SelectItemChanged

        public static readonly RoutedEvent SelectItemChangedEvent =
            EventManager.RegisterRoutedEvent("SelectItemChanged",
                        RoutingStrategy.Bubble,
                        typeof(RoutedPropertyChangedEventHandler<string>),
                        //typeof(RoutedEventHandler),
                        typeof(FlowContent));

        /// <summary>
        /// 当前选择的元素改变  
        /// </summary>
        [Description("当前选择的元素改变")]
        public event RoutedPropertyChangedEventHandler<string> SelectItemChanged
        {
            //将路由事件添加路由事件处理程序
            add { AddHandler(SelectItemChangedEvent, value); }
            //从路由事件处理程序中移除路由事件
            remove { RemoveHandler(SelectItemChangedEvent, value); }
        }

        //public RoutedEventArgs : EventArgs

        #endregion

        #endregion

    }
}
