using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using BFM.WPF.FlowDesign.Controls;

namespace BFM.WPF.FlowDesign.MainCanvas
{
    public sealed partial class FlowContent : Canvas
    {
        private List<Line> AlignLines = new List<Line>();  //对齐线
        public List<Shape> WidthHeightLines = new List<Shape>();  //宽高展示线 

        #region 移动控件元素

        private void DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            DragThumb tb = (sender as DragThumb);
            if ((tb == null) || (tb.BResizing) || (tb.IsReadOnly)) return;
            
            ClearAlignLies();
            Point oldPosition = new Point(tb.Position.X, tb.Position.Y);
            Point newPosition = new Point(tb.Position.X, tb.Position.Y);

            double addV = e.VerticalChange;
            double addH = e.HorizontalChange;

            #region 对齐到网格

            if (!BCtrl) //不是Ctrl
            {
                if (Math.Abs(addV) > 0.0001)
                {
                    addV = addV  - (addV%5);
                }
                if (Math.Abs(addH) > 0.0001)
                {
                    addH = addH - (addH % 5);
                }
            }

            #endregion 

            #region 设置新的坐标值

            if (BOrthogonal) //正交
            {
                if (LcDirection == 1) //锁定高度方向
                {
                    newPosition.Y += addV;
                }
                else
                {
                    newPosition.X += addH;

                }
            }
            else
            {
                newPosition.X += addH;
                newPosition.Y += addV;
            }

            #endregion

            #region 获取对齐的坐标

            const double AlignSpan = 10;

            double theNewTop = newPosition.Y;
            double theNewLeft = newPosition.X;

            double HSpan = AlignSpan; //水平移动间隔 - 停靠
            double VSpan = AlignSpan; //垂直移动间隔 - 停靠

            foreach (DragThumb thumb in DragThumbs)
            {
                if (thumb.Equals(tb)) continue;

                double theLeft = thumb.Position.X;
                double theTop = thumb.Position.Y;
                double theRight = theLeft + thumb.Width;
                double theButtom = theTop + thumb.Height;
                double theHCenter = theTop + (thumb.Height / 2);  //水平中心
                double theVCenter = theLeft + (thumb.Width / 2); //垂直中心

                #region 水平 对齐 Top，Buttom, HCenter

                if (Math.Abs(newPosition.Y - theTop) <= HSpan)  //查找水平方向最小间隔的控件
                {
                    HSpan = Math.Abs(newPosition.Y - theTop);
                    theNewTop = theTop;
                }

                if (Math.Abs(newPosition.Y + tb.Height - theButtom) <= HSpan)  //查找水平方向最小间隔的控件
                {
                    HSpan = Math.Abs(newPosition.Y + tb.Height - theButtom);
                    theNewTop = theButtom - tb.Height;
                }

                if (Math.Abs(newPosition.Y + (tb.Height / 2) - theHCenter) <= HSpan)  //查找水平方向最小间隔的控件
                {
                    HSpan = Math.Abs(newPosition.Y + (tb.Height / 2) - theHCenter);
                    theNewTop = theHCenter - (tb.Height / 2);
                }
                #endregion

                #region 垂直 对齐

                #region 水平 对齐 Left，Right, VCenter


                if (Math.Abs(newPosition.X - theLeft) <= VSpan) //查找垂直方向最小间隔的控件 
                {
                    VSpan = Math.Abs(newPosition.X - theLeft);
                    theNewLeft = theLeft;
                }

                if (Math.Abs(newPosition.X + tb.Width - theRight) <= VSpan)  //查找垂直方向最小间隔的控件
                {
                    VSpan = Math.Abs(newPosition.X + tb.Width - theRight);
                    theNewLeft = theRight - tb.Width;
                }

                if (Math.Abs(newPosition.X + (tb.Width / 2) - theVCenter) <= VSpan)  //查找垂直方向最小间隔的控件
                {
                    VSpan = Math.Abs(newPosition.X + (tb.Width / 2) - theVCenter);
                    theNewLeft = theVCenter - (tb.Width / 2);
                }
                #endregion 

                #endregion 
            }

            #endregion

            newPosition.X = theNewLeft;
            newPosition.Y = theNewTop;

            #region 更新控件的位置

            tb.MoveToNewPosition(newPosition.X, newPosition.Y, true);

            #endregion

            #region 更新所有选择的控件的位置

            foreach (var dragThumb in SelectDragThumbs)
            {
                if (dragThumb.Equals(tb)) continue;

                dragThumb.MoveBySpan(tb.Position.X - oldPosition.X, tb.Position.Y - oldPosition.Y, true);
            }

            #endregion

            ReDrawAlignLines(tb, true, true, true, true, true, true);

            bSaved = false;
        }

        
        private void DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            DragThumb tb = (sender as DragThumb);
            if ((tb == null) || (tb.IsReadOnly)) return;

            IsDragging = true;

            if (bMultiSelect) //多选情况下
            {
                tb.IsSelected = !tb.IsSelected;
            }
            else
            {
                tb.IsSelected = true;
            }
            if (tb.IsSelected)
            {
                _CurSelectedDragThumb = tb;  //将当前的设置为选择
            }

            _orthogonalBeginPoint = new Point(tb.Position.X, tb.Position.Y);
        }

        private void DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            IsDragging = false;

            ClearAlignLies();
            ClearWidthHeightLines();
        }

        #endregion

        /// <summary>
        /// 清空对齐线
        /// </summary>
        public void ClearAlignLies()
        {
            foreach (Line line in AlignLines)
            {
                this.Children.Remove(line);
            }
            AlignLines.Clear();
            
        }

        /// <summary>
        /// 重新绘制对齐线
        /// </summary>
        /// <param name="curThumb">当前的控件</param>
        /// <param name="bDrawTop">是否绘制Top</param>
        /// <param name="bDrawBottom">是否绘制Bottom</param>
        /// <param name="bDrawHCenter">是否绘制水平中线</param>
        /// <param name="bDrawLeft">是否绘制Left</param>
        /// <param name="bDrawRight">是否绘制Right</param>
        /// <param name="bDrawVCenter">是否绘制垂直中线</param>
        public void ReDrawAlignLines(DragThumb curThumb, bool bDrawTop, bool bDrawBottom, bool bDrawHCenter, bool bDrawLeft, bool bDrawRight, bool bDrawVCenter)
        {
            ClearAlignLies();

            #region 绘制对齐线

            foreach (DragThumb dt in DragThumbs)
            {
                if (dt.Equals(curThumb)) continue;
                if (SelectDragThumbs.Contains(dt)) continue;  //内部的线不绘制

                double theLeft = dt.Position.X;
                double theRight = theLeft + dt.Width;
                double theVCenter = theLeft + (dt.Width / 2);  //垂直中心

                double theTop = dt.Position.Y;
                double theBottom = theTop + dt.Height;
                double theHCenter = theTop + (dt.Height / 2); //水平中心

                //Top Buttom HCenter Left Rigth VCenter
                double TOLERANCE = 0.00001;

                #region Draw Top

                if (bDrawTop)
                {
                    if (Math.Abs(theTop - curThumb.Position.Y) < TOLERANCE)
                    {
                        Line line = NewAlignLine();

                        if (theLeft + dt.Width < curThumb.Position.X) //基准在左边
                        {
                            line.X1 = theLeft + dt.Width;
                        }
                        else
                        {
                            line.X1 = theLeft;
                        }
                        if (curThumb.Position.X + curThumb.Width < theLeft) //基准在右边
                        {
                            line.X2 = curThumb.Position.X + curThumb.Width;
                        }
                        else
                        {
                            line.X2 = curThumb.Position.X;
                        }

                        line.Y1 = curThumb.Position.Y;
                        line.Y2 = curThumb.Position.Y;

                        AlignLines.Add(line);
                        this.Children.Add(line);
                    }
                }

                #endregion

                #region Draw Bottom

                if (bDrawBottom)
                {
                    if (Math.Abs(curThumb.Position.Y + curThumb.Height - theBottom) < TOLERANCE)
                    {
                        Line line = NewAlignLine();
                        if (theLeft + dt.Width < curThumb.Position.X) //基准在左边
                        {
                            line.X1 = theLeft + dt.Width;
                        }
                        else
                        {
                            line.X1 = theLeft;
                        }
                        if (curThumb.Position.X + curThumb.Width < theLeft) //基准在右边
                        {
                            line.X2 = curThumb.Position.X + curThumb.Width;
                        }
                        else
                        {
                            line.X2 = curThumb.Position.X;
                        }

                        line.Y1 = theBottom;
                        line.Y2 = theBottom;

                        AlignLines.Add(line);
                        this.Children.Add(line);
                    }
                }

                #endregion

                #region Draw theHCenter

                if (bDrawHCenter)
                {
                    if (Math.Abs(curThumb.Position.Y + (curThumb.Height / 2) - theHCenter) < TOLERANCE)
                    {
                        Line line = NewAlignLine();

                        if (theLeft + dt.Width < curThumb.Position.X) //基准在左边
                        {
                            line.X1 = theLeft + dt.Width;
                        }
                        else
                        {
                            line.X1 = theLeft;
                        }
                        if (curThumb.Position.X + curThumb.Width < theLeft) //基准在右边
                        {
                            line.X2 = curThumb.Position.X + curThumb.Width;
                        }
                        else
                        {
                            line.X2 = curThumb.Position.X;
                        }

                        line.Y1 = theHCenter;
                        line.Y2 = theHCenter;

                        AlignLines.Add(line);
                        this.Children.Add(line);
                    }
                }

                #endregion

                #region Draw Left

                if (bDrawLeft)
                {
                    if (Math.Abs(theLeft - curThumb.Position.X) < TOLERANCE)
                    {
                        Line line = NewAlignLine();

                        if (theTop + dt.Height < curThumb.Position.Y) //基准在上边
                        {
                            line.Y1 = theTop + dt.Height;
                        }
                        else
                        {
                            line.Y1 = theTop;
                        }
                        if (curThumb.Position.Y + curThumb.Height < theTop) //基准在下边
                        {
                            line.Y2 = curThumb.Position.Y + curThumb.Height;
                        }
                        else
                        {
                            line.Y2 = curThumb.Position.Y;
                        }

                        line.X1 = curThumb.Position.X;
                        line.X2 = curThumb.Position.X;

                        AlignLines.Add(line);
                        this.Children.Add(line);
                    }
                }

                #endregion

                #region Draw Right

                if (bDrawRight)
                {
                    if (Math.Abs(curThumb.Position.X + curThumb.Width - theRight) < TOLERANCE)
                    {
                        Line line = NewAlignLine();

                        if (theTop + dt.Height < curThumb.Position.Y) //基准在上边
                        {
                            line.Y1 = theTop + dt.Height;
                        }
                        else
                        {
                            line.Y1 = theTop;
                        }
                        if (curThumb.Position.Y + curThumb.Height < theTop) //基准在下边
                        {
                            line.Y2 = curThumb.Position.Y + curThumb.Height;
                        }
                        else
                        {
                            line.Y2 = curThumb.Position.Y;
                        }

                        line.X1 = theRight;
                        line.X2 = theRight;

                        AlignLines.Add(line);
                        this.Children.Add(line);
                    }
                }

                #endregion

                #region Draw theVCenter

                if (bDrawVCenter)
                {
                    if (Math.Abs(curThumb.Position.X + (curThumb.Width / 2) - theVCenter) < TOLERANCE)
                    {
                        Line line = NewAlignLine();

                        if (theTop + dt.Height < curThumb.Position.Y) //基准在上边
                        {
                            line.Y1 = theTop + dt.Height;
                        }
                        else
                        {
                            line.Y1 = theTop;
                        }
                        if (curThumb.Position.Y + curThumb.Height < theTop) //基准在下边
                        {
                            line.Y2 = curThumb.Position.Y + curThumb.Height;
                        }
                        else
                        {
                            line.Y2 = curThumb.Position.Y;
                        }

                        line.X1 = theVCenter;
                        line.X2 = theVCenter;

                        AlignLines.Add(line);
                        this.Children.Add(line);
                    }
                }

                #endregion
            }

            #endregion
        }

        public void ClearWidthHeightLines()
        {
            foreach (Shape shape in WidthHeightLines)
            {
                this.Children.Remove(shape);
            }
            WidthHeightLines.Clear();
        }

        /// <summary>
        /// 重新绘制宽度、高度等高线
        /// </summary>
        /// <param name="curThumb">当前的控件</param>
        /// <param name="bDrawWidth">是否绘制宽度</param>
        /// <param name="bDrawHeight">是否绘制高度</param>
        public void ReDrawWidthHeightLines(DragThumb curThumb, bool bDrawWidth, bool bDrawHeight)
        {
            ClearWidthHeightLines();

            //Top Buttom HCenter Left Rigth VCenter
            double TOLERANCE = 0.00001;

            foreach (DragThumb dt in DragThumbs)
            {
                if (dt.Equals(curThumb)) continue;

                #region 绘制宽度

                if (bDrawWidth)
                {
                    if (Math.Abs(curThumb.Width - dt.Width) < TOLERANCE)
                    {
                        curThumb.DrawWidthLine(this);
                        dt.DrawWidthLine(this);
                    }
                }

                #endregion

                #region 绘制高度

                if (bDrawHeight)
                {
                    if (Math.Abs(curThumb.Height - dt.Height) < TOLERANCE)
                    {
                        curThumb.DrawHeightLine(this);
                        dt.DrawHeightLine(this);
                    }
                }

                #endregion
            }
        }
        
        #region 绘制对齐线

        public Line NewAlignLine(double x1, double y1, double x2, double y2)
        {
            Line line = NewAlignLine();
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            return line;
        }

        public Line NewAlignLine()
        {
            Line line = new Line()
            {
                StrokeThickness = 1,
                Stroke = new SolidColorBrush(Colors.Green),
                StrokeDashArray = new DoubleCollection() { 4, 2 },
            };
            Canvas.SetZIndex(line, this.Children.Count);
            return line;
        }

        #endregion
    }

}
