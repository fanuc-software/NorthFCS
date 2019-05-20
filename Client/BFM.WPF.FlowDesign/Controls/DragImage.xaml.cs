using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using BFM.WPF.FlowDesign.Controls.Base;
using BFM.WPF.FlowDesign.Enums;

namespace BFM.WPF.FlowDesign.Controls
{
    /// <summary>
    /// DragImage.xaml 的交互逻辑
    /// </summary>
    public partial class DragImage : DragThumb
    {
        public DragImage()
        {
            InitializeComponent();
            CtrlType = EmFlowCtrlType.Image;  //图形控件 

            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            Background = Brushes.Transparent;
            BorderBrush = Brushes.White;
        }

        /// <summary>
        /// 改变大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (!(sender is ResizeThumb)) return;

            ResizeThumb tb = (sender as ResizeThumb);

            Resize_DragDelta(tb, e.HorizontalChange, e.VerticalChange);
        }

        private void ResizeThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (!(sender is ResizeThumb)) return;

            Resize_Completed();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            IsEditText = false;
        }

        private void tbText_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null) Text = textBox.Text;
        }

        private void tbText_MouseEnter(object sender, MouseEventArgs e)
        {
            var textBox = sender as TextBox;
            if ((textBox != null) && (!textBox.IsFocused))
            {
                textBox.Focus();
                textBox.SelectAll();
            }
        }

        private void DragThumb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (IsReadOnly)
            {
                return;
            }
            IsEditText = true;
        }
    }
}
