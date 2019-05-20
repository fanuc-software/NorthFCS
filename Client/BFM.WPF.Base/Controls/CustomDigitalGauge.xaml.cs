using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BFM.WPF.Base.Controls
{
    /// <summary>
    /// CustomDigitalGauge.xaml 的交互逻辑
    /// </summary>
    public partial class CustomDigitalGauge : ContentControl
    {
        public CustomDigitalGauge()
        {
            InitializeComponent();
            var descriptor = DependencyPropertyDescriptor.FromProperty(ValueProperty, typeof(CustomDigitalGauge));
            descriptor.AddValueChanged(this, (s, e) =>
            {
                if (!string.IsNullOrEmpty(Value.ToString()))
                {
                    var nums = Value.ToString().ToArray();
                    root.ItemsSource = nums;
                }
            });
        }

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(CustomDigitalGauge), new PropertyMetadata(0));


    }
}
