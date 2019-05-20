using System;
using System.Windows;
using XL.CSharp.WPF.MainUI;

namespace BFM.WPF.MainUI
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        private WcfClient<ISDMService> ws = new WcfClient<ISDMService>();  //WCF 客户端

        public Login()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            RootFrame.MaxWidth = SystemParameters.WorkArea.Width;
            RootFrame.MaxHeight = SystemParameters.WorkArea.Height;
        }

        #region 窗口菜单

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            Environment.Exit(0);
        }

        #endregion

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string sUserNO = cmbUserName.Text;
            string sPwd = pbPassword.Password;

            SysUserInfo user = ws.UseService(s => s.GetAllSysUserInfo("")).FirstOrDefault(s => s.LoginName == sUserNO);

            if (user == null)
            {
                MyMessageBox.Show("用户名不存在！", "登录");
                pbPassword.Password = "";
                return;
            }
            if ((user.UserPassword == CPubFunc.GetMD5Value(sUserNO + sPwd))
                || (user.UserPassword == "" && sPwd == ""))
            {
                #region 登录成功

                CBaseData.LoginPKNO = user.UserPKNO;
                CBaseData.LoginNO = sUserNO;
                CBaseData.LoginName = user.UserName;
                CBaseData.BelongCompPKNO = user.BelongCompanyPKNO;

                MainWindow win = new MainWindow();
                win.Show();
                this.Close();

                #endregion
            }
            else
            {
                MyMessageBox.Show("密码错误！", "登录");
                pbPassword.Password = "";
                return;
            }


        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            //SysUserInfo user = new SysUserInfo();
            //user.PKNO = CBaseData.NewGuid();
            //user.BelongCompanyPKNO = "";
            //user.UserPKNO = "Admin";
            //user.UserName = "管理员";
            //user.LoginName = "admin";
            //user.UserPassword = CPubFunc.GetMD5Value(user.LoginName + "");
            //user.LoginCount = 0;
            //user.iState = 0;

            //ws.UseService(s => s.AddSysUserInfo(user));

            XL.CSharp.WPF.MainUI.TestUIPage win = new XL.CSharp.WPF.MainUI.TestUIPage();
            win.ShowDialog();
        }
    }
}
