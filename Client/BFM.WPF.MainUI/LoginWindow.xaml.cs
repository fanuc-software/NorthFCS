using System.Windows;
using System.Windows.Input;
using BFM.Common.Base.Log;

namespace BFM.WPF.MainUI
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            this.Loaded += LoginWindow_Loaded;
            
            EventLogger.Setting(true, "BFM_MES", "Log", 10, -1, -1, 5, "BFM_MES", "");
        }

        private void LoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TbUser.Focus();
            //cmbTest.Items.Clear();
            //cmbTest.Items.AddRange(ass.Select(c => c.ACCOUNT_NO).ToArray());
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            EventLogger.Log("登录成功.");

            //MainWindow mainWindow=new MainWindow();
            //Application.Current.MainWindow = mainWindow;
            //mainWindow.Loaded += MainWindow_Loaded;
            //mainWindow.ShowDialog();
           
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
