using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BFM.WPF.FlowDesign.Controls;

namespace BFM.WPF.FlowDesign.MainCanvas
{
    public sealed partial class FlowContent : Canvas
    {
        static FlowContent()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(
                typeof(FlowContent), new FrameworkPropertyMetadata(typeof(FlowContent)));
        }

        public FlowContent()
        {
            Background = Brushes.White;
            Focusable = true;
            _gridPen = CreateGridPen();
            Zoom = 1;
            Margin = new Thickness(0);
            this.MouseLeftButtonDown += OnMouseLeftButtonDown;
            this.MouseMove += OnMouseMove;
            this.MouseUp += OnMouseUp;
            this.MouseLeave += OnMouseLeave;
            this.KeyDown += OnKeyDown;
            this.KeyUp += OnKeyUp;
        }
        

        private Pen _gridPen;

        private bool isReadOnly = false;
        public bool IsReadOnly
        {
            get { return isReadOnly; }
            set
            {
                isReadOnly = value;

                foreach (DragImage di in DragThumbs)
                {
                    di.IsReadOnly = !di.IsReadOnly;
                }
            }
        }
        
        /// <summary>
        /// 是否多选
        /// </summary>
        public bool bMultiSelect = false;

        /// <summary>
        /// 是否保存
        /// </summary>
        public bool bSaved = true;

        /// <summary>
        /// 保存的路径
        /// </summary>
        public string SavePath = "";

        /// <summary>
        /// 宽高限制拖动
        /// </summary>
        public bool bResizeFormat = false;

        /// <summary>
        /// 是否在拖动
        /// </summary>
        public bool IsDragging = false;

        /// <summary>
        /// 所有元素
        /// </summary>
        public List<DragThumb> DragThumbs = new List<DragThumb>();

        
        private DragThumb _CurSelectedDragThumb = null;

        /// <summary>
        /// 当前选中的
        /// </summary>
        public DragThumb CurSelectedDragThumb
        {
            get { return _CurSelectedDragThumb; }
            set
            {
                string oldValue = _CurSelectedDragThumb?.CtrlName;
                _CurSelectedDragThumb = value; 
                string newValue = _CurSelectedDragThumb?.CtrlName;
                RaiseEvent(new RoutedPropertyChangedEventArgs<string>(oldValue, newValue, FlowContent.SelectItemChangedEvent)); //触发变化
            }
        }

        public List<DragThumb> SelectDragThumbs = new List<DragThumb>(); 

        /// <summary>
        /// 正交
        /// </summary>
        public bool BOrthogonal = false;
        private Point _orthogonalBeginPoint = new Point();
        
        /// <summary>
        /// 锁定方向 0 水平 1 高度
        /// </summary>
        public int LcDirection = 0; //锁定方向

        protected override void OnRender(DrawingContext dc)
        {
            var rect = new Rect(20, 20, RenderSize.Width, RenderSize.Height);
            dc.DrawRectangle(Background, null, rect);
            if (ShowGrid && GridCellSize.Width > 0 && GridCellSize.Height > 0)
                DrawGrid(dc, rect);
        }

        #region 绘制背景表格

        private void DrawGrid(DrawingContext dc, Rect rect)
        {
            //using .5 forces wpf to draw a single pixel line
            for (var i = 0.5; i < rect.Height; i += GridCellSize.Height)
                dc.DrawLine(_gridPen, new Point(0, i), new Point(rect.Width, i));
            for (var i = 0.5; i < rect.Width; i += GridCellSize.Width)
                dc.DrawLine(_gridPen, new Point(i, 0), new Point(i, rect.Height));
        }

        private Pen CreateGridPen()
        {
            return new Pen(Brushes.LightGray, (1 / Zoom));
        }

        #endregion
    }

}
