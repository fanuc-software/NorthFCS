using System.Windows;
using System.Drawing;
using System.Windows.Forms;

namespace BFM.WPF.MainUI
{
    /// <summary>
    /// MainWindow_New.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow_New : Window
    {
        public MainWindow_New()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            //System.Drawing.Point point = new System.Drawing.Point((int)Left, (int)Top);
            //RootFrame.MaxWidth = Screen.GetWorkingArea(point).Width;
            //RootFrame.MaxHeight = Screen.GetWorkingArea(point).Height;
            //RootFrame.MaxWidth = SystemParameters.WorkArea.Width;
            //RootFrame.MaxHeight = SystemParameters.WorkArea.Height;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
            
        }

        private void ButtonMax_Click(object sender, RoutedEventArgs e)
        {
            OnApplyTemplate();
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }
    }
}
