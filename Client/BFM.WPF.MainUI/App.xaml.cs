using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using BFM.Common.Base.Log;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.SDMService;
using BFM.WPF.Base;
using BFM.WPF.MainUI.VersionCtrol;

namespace BFM.WPF.MainUI
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        //程序启动
        protected override void OnStartup(StartupEventArgs e)
        {
            #region 检测进程是否存在

            string sName = Assembly.GetExecutingAssembly().FullName;
            bool runone = true;

            System.Threading.Mutex run = new System.Threading.Mutex(true, sName, out runone);

            if (!runone)
            {
                MessageBox.Show("系统已经在运行中，请查看任务管理器并退出系统【BFM.WPF.MainUI.exe】.", "系统启动", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                System.Environment.Exit(0);
                return;
            }

            run.ReleaseMutex();  //必须需要

            #endregion

            NewMainForm mainForm;

#if !DEBUG
            NetLog.Write("============系统启动=========");
            mainForm = new NewMainForm();
#endif

            System.Threading.ThreadPool.SetMaxThreads(2000, 800);

            #region 获取用户

            try
            {
                startRedis();
                WcfClient<ISDMService> _SDMService = new WcfClient<ISDMService>();
                List<SysUser> mSysUsers = _SDMService.UseService(s => s.GetSysUsers("USE_FLAG = 1")).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据库访问失败，请核实系统配置是否正确。\r\n错误为：" + ex.Message, "系统启动", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Environment.Exit(0);
                return;
            }

            #endregion

            try
            {
                #region 自动升级程序

                //1.检验版本并提示升级
                //VersionManage.UpdateAppVersion(true);  //检验版本
                //2.开启后台检查版本
                //VersionManage.AutoUpdateVersion(); //自动升级版本

                #endregion

#if DEBUG
                NetLog.Write("系统为DEBUG，请使用Release...");
                mainForm = new NewMainForm();
#endif

                mainForm.ShowDialog();
            }
            catch (Exception ex)
            {
                NetLog.Error("系统错误，", ex);

                WPFMessageBox.ShowError(
                        $"系统错误，请核实。" + Environment.NewLine +
                        $"错误为：" + ex.Message, "系统错误");
            }
        }

        private void startRedis()
        {
            try
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
                p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
                p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
                p.StartInfo.CreateNoWindow = true;//不显示程序窗口
                p.Start();//启动程序

                //向cmd窗口发送输入信息
                var startcmd = @"net start redis";

                p.StandardInput.WriteLine(startcmd);

                p.StandardInput.AutoFlush = true;
                p.WaitForExit(1000);//等待程序执行完退出进程
                p.Close();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

        }

        protected override void OnExit(ExitEventArgs e)
        {
            AppExit(e.ApplicationExitCode);

#if !DEBUG
            NetLog.Write("============系统 " + ((e.ApplicationExitCode == 0) ? "正常" : "异常(" + e.ApplicationExitCode + ")") + " 退出=========");
#endif

        }

        /// <summary>
        /// 系统退出
        /// </summary>
        /// <param name="exitCode"></param>
        public static void AppExit(int exitCode)
        {
            CBaseData.AppClosing = true;  //系统退出

            VersionManage.ExecUpdateBat();

            System.Environment.Exit(exitCode);
        }
    }
}
