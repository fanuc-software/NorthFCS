using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BFM.Common.Base.PubData;

namespace BFM.WPF.Base.Helper
{
    public class TextBoxHelper
    {
        #region 附加属性 IsClearButton
        /// <summary>
        /// 附加属性，是否带清空按钮
        /// </summary>
        public static readonly DependencyProperty IsClearButtonProperty =
            DependencyProperty.RegisterAttached("IsClearButton", typeof(bool), typeof(TextBoxHelper),
                new PropertyMetadata(false, ClearText));
        public static bool GetIsClearButton(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsClearButtonProperty);
        }

        public static void SetIsClearButton(DependencyObject obj, bool value)
        {
            obj.SetValue(IsClearButtonProperty, value);
        }
        #endregion

        #region 回调函数和清空输入框内容的实现
        /// <summary>
        /// 回调函数若附加属性IsClearButton值为True则挂载清空TextBox内容的函数
        /// </summary>
        /// <param name="d">属性所属依赖对象</param>
        /// <param name="e">属性改变事件参数</param>
        private static void ClearText(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Button btn = d as Button;
            if (d != null && e.OldValue != e.NewValue)
            {
                btn.Click -= ClearTextClicked;
                if ((bool)e.NewValue)
                {
                    btn.Click += ClearTextClicked;
                }
            }
        }

        /// <summary>
        /// 清空应用该附加属性的父TextBox内容函数
        /// </summary>
        /// <param name="sender">发送对象</param>
        /// <param name="e">路由事件参数</param>
        public static void ClearTextClicked(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                var parent = VisualTreeHelper.GetParent(btn);
                while ((!CBaseData.AppClosing) && (!(parent is TextBox)))
                {
                    if (parent != null) parent = VisualTreeHelper.GetParent(parent);
                    Thread.Sleep(10);
                }
                TextBox txt = parent as TextBox;
                txt.Clear();
            }
        }

        #endregion
    }
}
