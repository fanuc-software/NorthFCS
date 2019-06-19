using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BFM.WPF.SHWMS
{
    /// <summary>
    /// OrderWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OrderWindow : Window
    {
        public string ImagePath { set; get; }

        public event Action<int, int> OrderItemNumEvent;
        public OrderWindow()
        {
            InitializeComponent();
            this.Loaded += OrderWindow_Loaded;
        }

        private void OrderWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
        }

        private void Btn_minus_Click(object sender, RoutedEventArgs e)
        {
            var num = Convert.ToInt32(label_count.Content);
            if (num <= 1)
            {
                label_count.Content = 0;
                btn_ok.IsEnabled = false;
                return;
            }
            label_count.Content = num - 1;
        }

        private void Btn_plus_Click(object sender, RoutedEventArgs e)
        {
            btn_ok.IsEnabled = true;
            var num = Convert.ToInt32(label_count.Content);
            label_count.Content = num + 1;


        }

        private void Btn_ok_Click(object sender, RoutedEventArgs e)
        {
            OrderItemNumEvent?.Invoke(Convert.ToInt32(label_count.Content), comboxBox.SelectedIndex + 1);
            this.Close();
        }

        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
