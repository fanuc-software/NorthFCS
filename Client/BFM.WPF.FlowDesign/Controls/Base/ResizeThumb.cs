using System.Windows;
using System.Windows.Controls.Primitives;
using BFM.WPF.FlowDesign.Enums;

namespace BFM.WPF.FlowDesign.Controls.Base
{
    public class ResizeThumb : Thumb
    {
        public EmDragDirection EmDragDirection { get; set; }

        static ResizeThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizeThumb), new FrameworkPropertyMetadata(typeof(ResizeThumb)));
        }
    }
}
