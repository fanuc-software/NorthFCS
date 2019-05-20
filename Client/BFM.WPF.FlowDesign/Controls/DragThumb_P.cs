using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using BFM.WPF.FlowDesign.Common;
using BFM.WPF.FlowDesign.MainCanvas;

namespace BFM.WPF.FlowDesign.Controls
{
    public partial class DragThumb : Thumb
    {
        #region Properties.BelongCanvas

        public static readonly DependencyProperty BelongCanvasProperty =
            DependencyProperty.Register("BelongCanvas",
                                       typeof(FlowContent),
                                       typeof(DragThumb),
                                       new FrameworkPropertyMetadata((FlowContent)null));
        public FlowContent BelongCanvas
        {
            get { return (FlowContent)GetValue(BelongCanvasProperty); }
            set { SetValue(BelongCanvasProperty, value); }
        }

        #endregion

        #region Properties.Source

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source",
                                       typeof(ImageSource),
                                       typeof(DragThumb),
                                       new FrameworkPropertyMetadata((ImageSource)null));
        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        #endregion

        #region Properties.IsSelected

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected",
                                       typeof(bool),
                                       typeof(DragThumb),
                                       new FrameworkPropertyMetadata(false, OnSelectedChanged));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DragThumb dragThumb = d as DragThumb;
            if (dragThumb == null) return;

            FlowContent parent = ControlsCommon.FindParentControl<FlowContent>(dragThumb);
            if (parent == null) return;

            bool selected = (bool)e.NewValue;
            if (!selected)  //不选择
            {
                dragThumb.IsEditText = false;

                if (parent.CurSelectedDragThumb != null && parent.CurSelectedDragThumb.Equals(dragThumb))
                {
                    parent.CurSelectedDragThumb = null;
                }

                parent.SelectDragThumbs.Remove(dragThumb);
                return;
            }

            if (parent.SelectDragThumbs.IndexOf(dragThumb) < 0)
            {
                parent.SelectDragThumbs.Add(dragThumb);
            }

            if (!parent.bMultiSelect) // //单选，将其他设置为取消选择
            {
                #region 将其他的DargImage 取消选择

                int childCount = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < childCount; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    if ((child.GetType() != typeof(DragImage)) &&
                        (child.GetType() != typeof(DragShape)) &&
                        (child.GetType() != typeof(DragCircle)) &&
                        (child.GetType() != typeof(DragVideo))) continue;

                    DragThumb di = child as DragThumb;
                    if ((di == null) || (di.Equals(d))) continue;

                    di.IsSelected = false;
                }

                #endregion 

                parent.CurSelectedDragThumb = dragThumb;
            }

            parent.Focus();  //设置Focus
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        #endregion

        #region Properties.IsReadOnly

        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly",
                                       typeof(bool),
                                       typeof(DragThumb),
                                       new FrameworkPropertyMetadata(false));


        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set
            {
                SetValue(IsReadOnlyProperty, value);
                if (value)
                {
                    IsSelected = false;
                    IsEditText = false;
                }
            }
        }

        #endregion

        #region Properties.ReadOnlyCanClick

        public static readonly DependencyProperty ReadOnlyCanClickProperty =
            DependencyProperty.Register("ReadOnlyCanClick",
                                       typeof(bool),
                                       typeof(DragThumb),
                                       new FrameworkPropertyMetadata(false));

        /// <summary>
        /// 只读模式下是否可以点击
        /// </summary>
        public bool ReadOnlyCanClick
        {
            get { return (bool) GetValue(ReadOnlyCanClickProperty); }
            set { SetValue(ReadOnlyCanClickProperty, value); }
        }

        #endregion

        #region Properties.MonitorItem

        public static readonly DependencyProperty MonitorItemProperty =
            DependencyProperty.Register("MonitorItem",
                                       typeof(bool),
                                       typeof(DragThumb),
                                       new FrameworkPropertyMetadata(true));

        /// <summary>
        /// 监控空间
        /// </summary>
        public bool MonitorItem
        {
            get { return (bool)GetValue(MonitorItemProperty); }
            set { SetValue(MonitorItemProperty, value); }
        }

        #endregion

        #region Properties.Position

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position",
                                       typeof(Point),
                                       typeof(DragThumb),
                                       new FrameworkPropertyMetadata((Point)new Point(0, 0)));


        public Point Position
        {
            get
            {
                return (Point)GetValue(PositionProperty);
            }
            set
            {
                SetValue(PositionProperty, value);

                Canvas.SetLeft(this, value.X);
                Canvas.SetTop(this, value.Y);
            }
        }

        #endregion

        #region Properties.Text

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text",
                                       typeof(string),
                                       typeof(DragThumb),
                                       new FrameworkPropertyMetadata(""));


        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        #endregion

        #region Properties.IsEditText

        public static readonly DependencyProperty IsEditTextProperty =
            DependencyProperty.Register("IsEditText",
                                       typeof(bool),
                                       typeof(DragThumb),
                                       new FrameworkPropertyMetadata(false));


        public bool IsEditText
        {
            get { return (bool)GetValue(IsEditTextProperty); }
            set
            {
                SetValue(IsEditTextProperty, value);
            }
        }

        #endregion

        #region Properties.ShapePoints

        public static readonly DependencyProperty ShapePointsProperty =
            DependencyProperty.Register("ShapePoints",
                                       typeof(PointCollection),
                                       typeof(DragThumb),
                                       new FrameworkPropertyMetadata(new PointCollection()));


        public PointCollection ShapePoints
        {
            get { return (PointCollection)GetValue(ShapePointsProperty); }
            set { SetValue(ShapePointsProperty, value); }
        }

        #endregion

        #region Properties.CircleSize

        public static readonly DependencyProperty CircleSizeProperty =
            DependencyProperty.Register("CircleSize",
                                       typeof(Size),
                                       typeof(DragThumb),
                                       new FrameworkPropertyMetadata(new Size()));


        public Size CircleSize
        {
            get { return (Size)GetValue(CircleSizeProperty); }
            set { SetValue(CircleSizeProperty, value); }
        }

        #endregion
    }
}
