using System.Windows;
using System.Windows.Data;

namespace BFM.WPF.Base.Helper
{
    public class BindHelper
    {
        /// <summary>
        /// 设定内容绑定到表格选择行
        /// </summary>
        /// <param name="target">DataContent的内容控件</param>
        /// <param name="grid">表格</param>
        public static void SetDictDataBindingGridItem(DependencyObject target, DependencyObject grid)
        {
            Binding binding = new Binding() { Source = grid, Path = new PropertyPath("SelectedItem") };
            BindingOperations.SetBinding(target, FrameworkElement.DataContextProperty, binding);
        }
    }
}
