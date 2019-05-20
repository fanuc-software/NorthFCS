using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BFM.WPF.FlowDesign.MainCanvas
{
    public sealed partial class FlowContent : Canvas
    {
        #region GridCellSize

        public static readonly DependencyProperty GridCellSizeProperty =
            DependencyProperty.Register("GridCellSize",
                                       typeof(Size),
                                       typeof(FlowContent),
                                       new FrameworkPropertyMetadata(new Size(10, 10)));

        public Size GridCellSize
        {
            get { return (Size)GetValue(GridCellSizeProperty); }
            set { SetValue(GridCellSizeProperty, value); }
        }

        #endregion

        #region ShowGrid

        public static readonly DependencyProperty ShowGridProperty =
            DependencyProperty.Register("ShowGrid",
                                       typeof(bool),
                                       typeof(FlowContent),
                                       new FrameworkPropertyMetadata(true));

        public bool ShowGrid
        {
            get { return (bool)GetValue(ShowGridProperty); }
            set
            {
                SetValue(ShowGridProperty, value);
                this.InvalidateVisual();
            }
        }

        #endregion

        #region DocumentSize

        public static readonly DependencyProperty DocumentSizeProperty =
            DependencyProperty.Register("DocumentSize",
                                       typeof(Size),
                                       typeof(FlowContent),
                                       new FrameworkPropertyMetadata(new Size(2000, 2000)));

        public Size DocumentSize
        {
            get { return (Size)GetValue(DocumentSizeProperty); }
            set { SetValue(DocumentSizeProperty, value); }
        }

        #endregion

        #region Zoom

        public static readonly DependencyProperty ZoomProperty =
            DependencyProperty.Register("Zoom",
                                       typeof(double),
                                       typeof(FlowContent),
                                       new FrameworkPropertyMetadata(1.0, new PropertyChangedCallback(OnZoomChanged)));

        private static void OnZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as FlowContent;
            if (view == null) return;
            var zoom = (double)e.NewValue;
            view._gridPen = view.CreateGridPen();
            if (Math.Abs(zoom - 1) < 0.0001)
                view.LayoutTransform = null;
            else
                view.LayoutTransform = new ScaleTransform(zoom, zoom);  //缩放
        }

        public double Zoom
        {
            get { return (double)GetValue(ZoomProperty); }
            set
            {
                if (Math.Abs(Zoom - value) < 0.00001)
                {
                    return;
                }
                
                if ((value < 0.1) || (value > 10))
                {
                    return;
                }
                if (Math.Abs(value - 1) < 0.0001)
                {
                    value = 1;
                }

                SetValue(ZoomProperty, value);
            }
        }

        #endregion

        protected override Size MeasureOverride(Size constraint)
        {
            base.MeasureOverride(DocumentSize);
            return DocumentSize;
        }
        
    }

}
