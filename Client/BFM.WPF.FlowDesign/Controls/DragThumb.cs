using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using BFM.WPF.FlowDesign.Enums;

namespace BFM.WPF.FlowDesign.Controls
{
    //形状控件基础类，所有形状控件的父类
    public partial class DragThumb : Thumb
    {
        static DragThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DragThumb), new FrameworkPropertyMetadata(typeof(DragThumb)));

        }

        private new const double MinWidth = 5;
        private new const double MinHeight = 5;

        
        /// <summary>
        /// 是否是缩放
        /// </summary>
        public bool BResizing = false;

        private string _ctrlname;

        /// <summary>
        /// 控件名称
        /// </summary>
        public string CtrlName
        {
            get { return _ctrlname; }
            set
            {
                _ctrlname = value;
                if (BelongCanvas != null) BelongCanvas.bSaved = false;
            }
        }

        /// <summary>
        /// 控件类型
        /// </summary>
        public EmFlowCtrlType CtrlType { get; protected set; }

        private int zIndex = 0;
        /// <summary>
        /// 顺序
        /// </summary>
        public int ZIndex
        {
            get { return zIndex; }
            set
            {
                zIndex = value;
                if (BelongCanvas != null) BelongCanvas.bSaved = false;
                Canvas.SetZIndex(this, zIndex);
            }
        }
        
    }
}
