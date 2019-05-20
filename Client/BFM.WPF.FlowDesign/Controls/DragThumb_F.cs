using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using BFM.WPF.FlowDesign.Common;
using BFM.WPF.FlowDesign.Controls.Base;
using BFM.WPF.FlowDesign.Enums;
using BFM.WPF.FlowDesign.MainCanvas;

namespace BFM.WPF.FlowDesign.Controls
{
    public partial class DragThumb : Thumb
    {
        #region 控件位置改变

        //按间距移动
        public int MoveBySpan(double leftSpan, double topSpan, bool isCheckBorder)
        {
            double newLeft = Canvas.GetLeft(this);
            double newTop = Canvas.GetTop(this);
            newLeft += leftSpan;
            newTop += topSpan;
            return MoveToNewPosition(newLeft, newTop, isCheckBorder);
        }

        public int MoveToNewPosition(Point position, bool isCheckBorder)
        {
            return MoveToNewPosition(position.X, position.Y, isCheckBorder);
        }

        public int MoveToNewPosition(double newLeft, double newTop, bool isCheckBorder)
        {
            if (newLeft <= 0) newLeft = 0;
            if (newTop <= 0) newTop = 0;

            int result = 0;

            #region 检验并设置边界

            if (isCheckBorder)
            {
                FlowContent parent = ControlsCommon.FindParentControl<FlowContent>(this);

                if (parent == null)
                {
                    return -1;
                }

                if (newLeft > parent.ActualWidth - this.ActualWidth)
                {
                    newLeft = parent.ActualWidth - this.ActualWidth;

                    result = 1;
                }

                if (newTop > parent.ActualHeight - this.ActualHeight)
                {
                    newTop = parent.ActualHeight - this.ActualHeight;

                    result += 10;
                }
            }

            #endregion

            Position = new Point(newLeft, newTop);

            if (BelongCanvas != null) BelongCanvas.bSaved = false;
            return result;
        }

        #endregion

        #region 改变控件大小

        protected void Resize_DragDelta(ResizeThumb resizeThumb, double horizontalChange, double verticalChange)
        {
            if (IsReadOnly)
            {
                return;
            }

            FlowContent dv = ControlsCommon.FindParentControl<FlowContent>(this);
            if (dv == null) return;
            this.BResizing = true; //设置为Resizing
            if (!dv.BCtrl) //不是Ctrl
            {
                if (Math.Abs(horizontalChange) > 0.0001)
                {
                    horizontalChange = horizontalChange - (horizontalChange % 5);
                }
                if (Math.Abs(verticalChange) > 0.0001)
                {
                    verticalChange = verticalChange - (verticalChange % 5);
                }
            }

            Point newPosition = new Point(Position.X, Position.Y);

            double addWidth = 0;
            double addHeight = 0;

            const double alignSpan = 10;

            double hSpan = alignSpan; //水平增大缩小间隔 - 停靠
            double vSpan = alignSpan; //垂直增大缩小间隔 - 停靠

            int iAutoResize = 0;  //自动大小方向 0 ：无；1：水平；2：垂直

            #region Resize

            switch (resizeThumb.EmDragDirection)
            {
                case EmDragDirection.TopLeft: //左上

                    #region 左上

                    newPosition.X = Position.X + horizontalChange;
                    newPosition.Y = Position.Y + verticalChange;

                    if (newPosition.X <= 0)
                    {
                        newPosition.X = 0;
                    }
                    else
                    {
                        addWidth = horizontalChange*(-1);
                    }
                    if (newPosition.Y <= 0)
                    {
                        newPosition.Y = 0;
                    }
                    else
                    {
                        addHeight = verticalChange*(-1);
                    }

                    #region 停靠 - 左上

                    foreach (DragThumb thumb in dv.DragThumbs)
                    {
                        if (thumb.Equals(this)) continue;

                        double span = 0;

                        #region 左边停靠

                        span = Math.Abs(newPosition.X - thumb.Position.X);

                        if (span <= hSpan) //左边
                        {
                            hSpan = span;
                            newPosition.X = thumb.Position.X;
                            addWidth = this.Position.X - thumb.Position.X;

                            iAutoResize = 1;
                        }

                        #endregion

                        #region 等宽

                        span = Math.Abs(this.Width + addWidth - thumb.Width);

                        if (span <= hSpan) //宽
                        {
                            hSpan = span;
                            addWidth = thumb.Width - this.Width;
                            newPosition.X = this.Position.X - addWidth;
                            iAutoResize = 1;
                        }

                        #endregion

                        #region 顶部停靠

                        span = Math.Abs(newPosition.Y - thumb.Position.Y);

                        if (span <= vSpan) //顶部
                        {
                            vSpan = span;
                            newPosition.Y = thumb.Position.Y;
                            addHeight = this.Position.Y - thumb.Position.Y;
                            iAutoResize = 2;
                        }

                        #endregion

                        #region 等高

                        span = Math.Abs(this.Height + addHeight - thumb.Height);

                        if (span <= vSpan) //高
                        {
                            vSpan = span;
                            addHeight = thumb.Height - this.Height;
                            newPosition.Y = this.Position.Y - addHeight;
                            iAutoResize = 2;
                        }

                        #endregion
                    }

                    #endregion

                    #region 格式拖动

                    if (BelongCanvas.bResizeFormat)  //格式化
                    {
                        if (iAutoResize == 1) //按照水平
                        {
                            addHeight = addWidth * this.Height / this.Width;
                            newPosition.Y = this.Position.Y - addHeight;
                        }
                        //else if (iAutoResize == 2)
                        //{

                        //}
                        else
                        {
                            addWidth = addHeight * this.Width / this.Height;
                            newPosition.X = this.Position.X - addWidth;
                        }
                    }

                    #endregion

                    #endregion

                    break;
                case EmDragDirection.TopCenter: //中上

                    #region 中上

                    newPosition.Y = Position.Y + verticalChange;

                    if (newPosition.Y <= 0)
                    {
                        newPosition.Y = 0;
                    }
                    else
                    {
                        addHeight = verticalChange*(-1);
                    }

                    #region 停靠 - 上

                    foreach (DragThumb thumb in dv.DragThumbs)
                    {
                        if (thumb.Equals(this)) continue;

                        double span = 0;

                        #region 顶部停靠

                        span = Math.Abs(newPosition.Y - thumb.Position.Y);

                        if (span <= vSpan) //顶部
                        {
                            vSpan = span;
                            newPosition.Y = thumb.Position.Y;
                            addHeight = this.Position.Y - thumb.Position.Y;
                        }

                        #endregion

                        #region 等高

                        span = Math.Abs(this.Height + addHeight - thumb.Height);

                        if (span <= vSpan) //高
                        {
                            vSpan = span;
                            addHeight = thumb.Height - this.Height;
                            newPosition.Y = this.Position.Y - addHeight;
                        }

                        #endregion
                    }

                    #endregion

                    #endregion 

                    break;
                case EmDragDirection.TopRight: //右上

                    #region 右上

                    newPosition.Y = Position.Y + verticalChange;
                    addWidth = horizontalChange;

                    if (newPosition.Y <= 0)
                    {
                        newPosition.Y = 0;
                    }
                    else
                    {
                        addHeight = verticalChange*(-1);
                    }
                    if (newPosition.Y > dv.ActualHeight - this.ActualHeight)
                    {
                        newPosition.Y = dv.ActualHeight - this.ActualHeight;
                    }

                    #region 停靠 - 右上

                    foreach (DragThumb thumb in dv.DragThumbs)
                    {
                        if (thumb.Equals(this)) continue;

                        double span = 0;

                        #region 右边停靠

                        span = Math.Abs(newPosition.X + this.Width + addWidth - (thumb.Position.X + thumb.Width));

                        if (span <= hSpan) //右边
                        {
                            hSpan = span;
                            addWidth = thumb.Position.X + thumb.Width - (newPosition.X + this.Width);

                            iAutoResize = 1;
                        }

                        #endregion

                        #region 等宽

                        span = Math.Abs(this.Width + addWidth - thumb.Width);

                        if (span <= hSpan) //宽
                        {
                            hSpan = span;
                            addWidth = thumb.Width - this.Width;
                            iAutoResize = 1;
                        }

                        #endregion

                        #region 顶部停靠

                        span = Math.Abs(newPosition.Y - thumb.Position.Y);

                        if (span <= vSpan) //顶部
                        {
                            vSpan = span;
                            newPosition.Y = thumb.Position.Y;
                            addHeight = this.Position.Y - thumb.Position.Y;
                            iAutoResize = 2;
                        }

                        #endregion

                        #region 等高

                        span = Math.Abs(this.Height + addHeight - thumb.Height);

                        if (span <= vSpan) //高
                        {
                            vSpan = span;
                            addHeight = thumb.Height - this.Height;
                            newPosition.Y = this.Position.Y - addHeight;
                            iAutoResize = 2;
                        }

                        #endregion
                    }

                    #endregion

                    #region 格式拖动

                    if (BelongCanvas.bResizeFormat)  //格式化
                    {
                        if (iAutoResize == 1) //按照水平
                        {
                            addHeight = addWidth * this.Height / this.Width;
                            newPosition.Y = this.Position.Y - addHeight;
                        }
                        //else if (iAutoResize == 2)
                        //{

                        //}
                        else
                        {
                            addWidth = addHeight * this.Width / this.Height;
                        }
                    }

                    #endregion

                    #endregion

                    break;
                case EmDragDirection.MiddleLeft: //中左

                    #region 中左

                    newPosition.X = Position.X + horizontalChange;
                    addWidth = horizontalChange*(-1);

                    if (newPosition.Y <= 0) newPosition.Y = 0;

                    #region 停靠 左

                    foreach (DragThumb thumb in dv.DragThumbs)
                    {
                        if (thumb.Equals(this)) continue;

                        double span = 0;

                        #region 左边停靠

                        span = Math.Abs(newPosition.X - thumb.Position.X);

                        if (span <= hSpan) //左边
                        {
                            hSpan = span;
                            newPosition.X = thumb.Position.X;
                            addWidth = this.Position.X - thumb.Position.X;
                        }

                        #endregion

                        #region 等宽

                        span = Math.Abs(this.Width + addWidth - thumb.Width);

                        if (span <= hSpan) //宽
                        {
                            hSpan = span;
                            addWidth = thumb.Width - this.Width;
                            newPosition.X = this.Position.X - addWidth;
                        }

                        #endregion
                    }

                    #endregion

                    #endregion

                    break;
                case EmDragDirection.MiddleRight: //中右

                    #region 中右

                    addWidth = horizontalChange;

                    #region 停靠 右

                    foreach (DragThumb thumb in dv.DragThumbs)
                    {
                        if (thumb.Equals(this)) continue;

                        double span = 0;

                        #region 右边停靠

                        span = Math.Abs(newPosition.X + this.Width + addWidth - (thumb.Position.X + thumb.Width));

                        if (span <= hSpan) //右边
                        {
                            hSpan = span;
                            addWidth = thumb.Position.X + thumb.Width - (newPosition.X + this.Width);
                        }

                        #endregion

                        #region 等宽

                        span = Math.Abs(this.Width + addWidth - thumb.Width);

                        if (span <= hSpan) //宽
                        {
                            hSpan = span;
                            addWidth = thumb.Width - this.Width;
                        }

                        #endregion
                    }

                    #endregion

                    #endregion

                    break;
                case EmDragDirection.BottomLeft: //下左

                    #region 下左

                    newPosition.X = Position.X + horizontalChange;
                    addWidth = horizontalChange*(-1);

                    addHeight = verticalChange;

                    if (newPosition.X <= 0) newPosition.X = 0;

                    #region 停靠 下左

                    foreach (DragThumb thumb in dv.DragThumbs)
                    {
                        if (thumb.Equals(this)) continue;

                        double span = 0;

                        #region 左边停靠

                        span = Math.Abs(newPosition.X - thumb.Position.X);

                        if (span <= hSpan) //左边
                        {
                            hSpan = span;
                            newPosition.X = thumb.Position.X;
                            addWidth = this.Position.X - thumb.Position.X;
                            iAutoResize = 1;
                        }

                        #endregion

                        #region 等宽

                        span = Math.Abs(this.Width + addWidth - thumb.Width);

                        if (span <= hSpan) //宽
                        {
                            hSpan = span;
                            addWidth = thumb.Width - this.Width;
                            newPosition.X = this.Position.X - addWidth;
                            iAutoResize = 1;
                        }

                        #endregion

                        #region 底部停靠

                        span = Math.Abs(newPosition.Y + this.Height + addHeight - (thumb.Position.Y + thumb.Height));

                        if (span <= vSpan) //底部
                        {
                            vSpan = span;
                            addHeight = thumb.Position.Y + thumb.Height - (newPosition.Y + this.Height);
                            iAutoResize = 2;
                        }

                        #endregion

                        #region 等高

                        span = Math.Abs(this.Height + addHeight - thumb.Height);

                        if (span <= vSpan) //高
                        {
                            vSpan = span;
                            addHeight = thumb.Height - this.Height;
                            iAutoResize = 2;
                        }

                        #endregion
                    }

                    #endregion

                    #region 格式拖动

                    if (BelongCanvas.bResizeFormat)  //格式化
                    {
                        if (iAutoResize == 1) //按照水平
                        {
                            addHeight = addWidth * this.Height / this.Width;
                            newPosition.X = this.Position.X - addWidth;
                        }
                        //else if (iAutoResize == 2)
                        //{

                        //}
                        else
                        {
                            addWidth = addHeight * this.Width / this.Height;
                        }
                    }

                    #endregion

                    #endregion

                    break;
                case EmDragDirection.BottomCenter: //下中

                    #region 下中

                    addHeight = verticalChange;

                    #region 停靠 下中

                    foreach (DragThumb thumb in dv.DragThumbs)
                    {
                        if (thumb.Equals(this)) continue;

                        double span = 0;

                        #region 底部停靠

                        span = Math.Abs(newPosition.Y + this.Height + addHeight - (thumb.Position.Y + thumb.Height));

                        if (span <= vSpan) //底部
                        {
                            vSpan = span;
                            addHeight = thumb.Position.Y + thumb.Height - (newPosition.Y + this.Height);
                        }

                        #endregion

                        #region 等高

                        span = Math.Abs(this.Height + addHeight - thumb.Height);

                        if (span <= vSpan) //高
                        {
                            vSpan = span;
                            addHeight = thumb.Height - this.Height;
                        }

                        #endregion
                    }

                    #endregion

                    #endregion

                    break;
                case EmDragDirection.BottomRight: //下右

                    #region 下右

                    addWidth = horizontalChange;
                    addHeight = verticalChange;

                    #region 停靠 下右

                    foreach (DragThumb thumb in dv.DragThumbs)
                    {
                        if (thumb.Equals(this)) continue;

                        double span = 0;

                        #region 右边停靠

                        span = Math.Abs(newPosition.X + this.Width + addWidth - (thumb.Position.X + thumb.Width));

                        if (span <= hSpan) //右边
                        {
                            hSpan = span;
                            addWidth = thumb.Position.X + thumb.Width - (newPosition.X + this.Width);
                            iAutoResize = 1;
                        }

                        #endregion

                        #region 等宽

                        span = Math.Abs(this.Width + addWidth - thumb.Width);

                        if (span <= hSpan) //宽
                        {
                            hSpan = span;
                            addWidth = thumb.Width - this.Width;
                            iAutoResize = 1;
                        }

                        #endregion

                        #region 底部停靠

                        span = Math.Abs(newPosition.Y + this.Height + addHeight - (thumb.Position.Y + thumb.Height));

                        if (span <= vSpan) //底部
                        {
                            vSpan = span;
                            addHeight = thumb.Position.Y + thumb.Height - (newPosition.Y + this.Height);
                            iAutoResize = 2;
                        }

                        #endregion


                        #region 等高

                        span = Math.Abs(this.Height + addHeight - thumb.Height);

                        if (span <= vSpan) //高
                        {
                            vSpan = span;
                            addHeight = thumb.Height - this.Height;
                            iAutoResize = 2;
                        }

                        #endregion
                    }

                    #endregion

                    #region 格式拖动

                    if (BelongCanvas.bResizeFormat)  //格式化
                    {
                        if (iAutoResize == 1) //按照水平
                        {
                            addHeight = addWidth * this.Height / this.Width;
                        }
                        //else if (iAutoResize == 2)
                        //{

                        //}
                        else
                        {
                            addWidth = addHeight * this.Width / this.Height;
                        }
                    }

                    #endregion

                    #endregion

                    break;
            }

            #endregion

            #region 限制最小宽度、高度

            if (this.Width + addWidth <= MinWidth)
            {
                addWidth = MinWidth - this.Width;
            }

            if (this.Height + addHeight <= MinHeight)
            {
                addHeight = MinHeight - this.Height;
            }

            #endregion

            this.Width += addWidth;
            this.Height += addHeight;

            #region 控制区域


            if (newPosition.X <= 0) newPosition.X = 0;
            if (newPosition.Y <= 0) newPosition.Y = 0;

            if (newPosition.X > dv.ActualWidth)
            {
                newPosition.X = dv.ActualWidth;
            }

            if (newPosition.Y > dv.ActualHeight)
            {
                newPosition.Y = dv.ActualHeight;
            }

            #endregion

            Position = newPosition; //设置位置

            #region 绘制 对齐和 等宽高线

            dv.ReDrawAlignLines(this,
                (resizeThumb.EmDragDirection == EmDragDirection.TopLeft ||
                 resizeThumb.EmDragDirection == EmDragDirection.TopRight ||
                 resizeThumb.EmDragDirection == EmDragDirection.TopCenter),
                (resizeThumb.EmDragDirection == EmDragDirection.BottomLeft ||
                 resizeThumb.EmDragDirection == EmDragDirection.BottomRight ||
                 resizeThumb.EmDragDirection == EmDragDirection.BottomCenter),
                false,
                (resizeThumb.EmDragDirection == EmDragDirection.TopLeft ||
                 resizeThumb.EmDragDirection == EmDragDirection.BottomLeft ||
                 resizeThumb.EmDragDirection == EmDragDirection.MiddleLeft),
                (resizeThumb.EmDragDirection == EmDragDirection.TopRight ||
                 resizeThumb.EmDragDirection == EmDragDirection.BottomRight ||
                 resizeThumb.EmDragDirection == EmDragDirection.MiddleRight),
                false
                );

            dv.ReDrawWidthHeightLines(this,
                (resizeThumb.EmDragDirection == EmDragDirection.TopLeft ||
                 resizeThumb.EmDragDirection == EmDragDirection.TopRight ||
                 resizeThumb.EmDragDirection == EmDragDirection.BottomLeft ||
                 resizeThumb.EmDragDirection == EmDragDirection.BottomRight ||

                 resizeThumb.EmDragDirection == EmDragDirection.MiddleLeft ||
                 resizeThumb.EmDragDirection == EmDragDirection.MiddleRight),

                (resizeThumb.EmDragDirection == EmDragDirection.TopLeft ||
                 resizeThumb.EmDragDirection == EmDragDirection.TopRight ||
                 resizeThumb.EmDragDirection == EmDragDirection.BottomLeft ||
                 resizeThumb.EmDragDirection == EmDragDirection.BottomRight ||

                 resizeThumb.EmDragDirection == EmDragDirection.TopCenter ||
                 resizeThumb.EmDragDirection == EmDragDirection.BottomCenter)
                );

            #endregion

            if (BelongCanvas != null) BelongCanvas.bSaved = false;
        }

        /// <summary>
        /// 拖动改变控件大小完成
        /// </summary>
        protected void Resize_Completed()
        {
            BResizing = false;
        }

        #endregion

        #region 绘制高度宽度显示线

        //宽度
        public void DrawWidthLine(FlowContent parent)
        {
            double beginX = Position.X;
            double beginY = Position.Y + this.Height + 5;
            double endX = Position.X + this.Width;

            Line line = new Line()
            {
                StrokeThickness = 1,
                Stroke = new SolidColorBrush(Colors.Green),
                Fill = new SolidColorBrush(Colors.Green),
                X1 = beginX,
                Y1 = beginY,
                X2 = beginX,
                Y2 = beginY + 20,
            };
            parent.WidthHeightLines.Add(line);
            Canvas.SetZIndex(line, parent.Children.Count);
            parent.Children.Add(line);

            Line line2 = new Line()
            {
                StrokeThickness = 1,
                Stroke = new SolidColorBrush(Colors.Green),
                Fill = new SolidColorBrush(Colors.Green),
                X1 = beginX,
                Y1 = beginY + 10,
                X2 = endX,
                Y2 = beginY + 10,
            };
            parent.WidthHeightLines.Add(line2);
            Canvas.SetZIndex(line2, parent.Children.Count);
            parent.Children.Add(line2);

            Line line3 = new Line()
            {
                StrokeThickness = 1,
                Stroke = new SolidColorBrush(Colors.Green),
                Fill = new SolidColorBrush(Colors.Green),
                X1 = endX,
                Y1 = beginY,
                X2 = endX,
                Y2 = beginY + 20,
            };
            parent.WidthHeightLines.Add(line3);
            Canvas.SetZIndex(line3, parent.Children.Count);
            parent.Children.Add(line3);

            Polygon polygon1 = new Polygon()
            {
                StrokeThickness = 1,
                Stroke = new SolidColorBrush(Colors.Green),
                Fill = new SolidColorBrush(Colors.Green),
            };
            polygon1.Points.Add(new Point(beginX, beginY + 10));
            polygon1.Points.Add(new Point(beginX + 6, beginY + 10 - 4));
            polygon1.Points.Add(new Point(beginX + 6, beginY +10 + 4));

            parent.WidthHeightLines.Add(polygon1);
            Canvas.SetZIndex(polygon1, parent.Children.Count);
            parent.Children.Add(polygon1);

            Polygon polygon2 = new Polygon()
            {
                StrokeThickness = 1,
                Stroke = new SolidColorBrush(Colors.Green),
                Fill = new SolidColorBrush(Colors.Green),
            };
            polygon2.Points.Add(new Point(endX, beginY + 10));
            polygon2.Points.Add(new Point(endX - 6, beginY + 10 - 4));
            polygon2.Points.Add(new Point(endX - 6, beginY + 10 + 4));

            parent.WidthHeightLines.Add(polygon2);
            Canvas.SetZIndex(polygon2, parent.Children.Count);
            parent.Children.Add(polygon2);
        }

        //高度
        public void DrawHeightLine(FlowContent parent)
        {
            double beginX = Position.X + this.Width + 5;
            double beginY = Position.Y;
            double endY = Position.Y + this.Height;

            Line line = new Line()
            {
                StrokeThickness = 1,
                Stroke = new SolidColorBrush(Colors.Green),
                Fill = new SolidColorBrush(Colors.Green),
                X1 = beginX,
                Y1 = beginY,
                X2 = beginX + 20,
                Y2 = beginY,
            };
            parent.WidthHeightLines.Add(line);
            Canvas.SetZIndex(line, parent.Children.Count);
            parent.Children.Add(line);

            //中线
            Line line2 = new Line()
            {
                StrokeThickness = 1,
                Stroke = new SolidColorBrush(Colors.Green),
                Fill = new SolidColorBrush(Colors.Green),
                X1 = beginX + 10,
                Y1 = beginY,
                X2 = beginX + 10,
                Y2 = endY,
            };
            parent.WidthHeightLines.Add(line2);
            Canvas.SetZIndex(line2, parent.Children.Count);
            parent.Children.Add(line2);

            Line line3 = new Line()
            {
                StrokeThickness = 1,
                Stroke = new SolidColorBrush(Colors.Green),
                Fill = new SolidColorBrush(Colors.Green),
                X1 = beginX,
                Y1 = endY,
                X2 = beginX + 20,
                Y2 = endY,
            };
            parent.WidthHeightLines.Add(line3);
            Canvas.SetZIndex(line3, parent.Children.Count);
            parent.Children.Add(line3);

            Polygon polygon1 = new Polygon()
            {
                StrokeThickness = 1,
                Stroke = new SolidColorBrush(Colors.Green),
                Fill = new SolidColorBrush(Colors.Green),
            };
            polygon1.Points.Add(new Point(beginX + 10, beginY));
            polygon1.Points.Add(new Point(beginX + 10 - 4 , beginY + 6));
            polygon1.Points.Add(new Point(beginX + 10 + 4 , beginY + 6));

            parent.WidthHeightLines.Add(polygon1);
            Canvas.SetZIndex(polygon1, parent.Children.Count);
            parent.Children.Add(polygon1);

            Polygon polygon2 = new Polygon()
            {
                StrokeThickness = 1,
                Stroke = new SolidColorBrush(Colors.Green),
                Fill = new SolidColorBrush(Colors.Green),
            };
            polygon2.Points.Add(new Point(beginX + 10, endY));
            polygon2.Points.Add(new Point(beginX + 10 - 4, endY - 6));
            polygon2.Points.Add(new Point(beginX + 10 + 4, endY - 6));

            parent.WidthHeightLines.Add(polygon2);
            Canvas.SetZIndex(polygon2, parent.Children.Count);
            parent.Children.Add(polygon2);
        }

        #endregion
    }
}
