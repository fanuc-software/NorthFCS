using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using BFM.WPF.FlowDesign.MainCanvas;

namespace BFM.WPF.FlowDesign.Controls
{
    //形状控件基础类，所有形状控件的父类
    public partial class DragThumb : Thumb
    {
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if ((IsReadOnly) && (ReadOnlyCanClick))
            {
                BelongCanvas.RaiseEvent(new RoutedPropertyChangedEventArgs<string>(this.CtrlName, CtrlName, FlowContent.ItemClickEvent));
            }
        }
    }
}
