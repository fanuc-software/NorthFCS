using GalaSoft.MvvmLight;
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
        public event Action<int, int, string> OrderItemNumEvent;

        static string orderImage;

        ImageNodeViewModel currentNode;
        public List<ImageNodeViewModel> ImageNodes { get; set; } = new List<ImageNodeViewModel>();
        static OrderWindow()
        {

            orderImage = System.Configuration.ConfigurationManager.AppSettings["OrderImage"];
        }
        public OrderWindow()
        {
            InitializeComponent();
            this.Loaded += OrderWindow_Loaded;
        }

        private void OrderWindow_Loaded(object sender, RoutedEventArgs e)
        {
            int index = 1;
            orderImage.Split('|').ToList().ForEach(d =>
            {
                var node = new ImageNodeViewModel() { Name = d, Tag = index++ };
                node.ClickChangeEvent += Node_ClickChangeEvent;
                ImageNodes.Add(node);
            });
            this.DataContext = this;
            if (ImageNodes.Count > 0)
            {
                Node_ClickChangeEvent(ImageNodes[0]);
            }
        }

        private void Node_ClickChangeEvent(ImageNodeViewModel obj)
        {
            imagePath.Source = new BitmapImage(new Uri($"pack://application:,,,/BFM.WPF.SHWMS;component/SHImage/{obj.Name}.png"));
            ImageNodes.ForEach(d => d.IsActive = false);
            obj.IsActive = true;
            currentNode = obj;
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
            OrderItemNumEvent?.Invoke(Convert.ToInt32(label_count.Content), currentNode.Tag, currentNode.Name);
            this.Close();
        }

        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }

    public class ImageNodeViewModel : ViewModelBase
    {

        public string Name { get; set; }

        public int Tag { get; set; }

        private bool isActive;

        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                if (isActive)
                {
                    ActiveShow = Visibility.Visible;
                    DefaultShow = Visibility.Collapsed;
                }
                else
                {
                    ActiveShow = Visibility.Collapsed;
                    DefaultShow = Visibility.Visible;
                }
            }
        }
        public ImageNodeViewModel()
        {
            IsActive = false;
        }
        private Visibility activeShow;

        public Visibility ActiveShow
        {
            get { return activeShow; }
            set
            {
                if (activeShow != value)
                {
                    activeShow = value;
                    RaisePropertyChanged(() => ActiveShow);
                }

            }
        }


        private Visibility defaultShow;

        public Visibility DefaultShow
        {
            get { return defaultShow; }
            set
            {
                if (defaultShow != value)
                {
                    defaultShow = value;
                    RaisePropertyChanged(() => DefaultShow);
                }

            }
        }

        public event Action<ImageNodeViewModel> ClickChangeEvent;
        public ICommand ClickCommand
        {
            get
            {
                return new GalaSoft.MvvmLight.Command.RelayCommand<ImageNodeViewModel>((s) =>
                {
                    ClickChangeEvent?.Invoke(s);
                });
            }
        }
    }
}
