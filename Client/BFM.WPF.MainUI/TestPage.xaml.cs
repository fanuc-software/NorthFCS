using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BFM.Common.Base;
using BFM.Common.Base.Extend;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.Data.PubData;
using BFM.Common.DeviceAsset;
using BFM.Server.DataAsset.FMSService;
using BFM.Server.DataAsset.RSMService;
using BFM.Server.DataAsset.SDMService;
using BFM.WPF.Base.Controls;
using BFM.WPF.FMS;
using BFM.WPF.SDM.TableNO;
using BFM.ContractModel;
using BFM.Server.DataAsset.EAMService;
using BFM.Server.DataAsset.PLMService;
using BFM.Web.Base.NetHelper;
using BFM.WPF.Base;
using BFM.WPF.MainUI.VersionCtrol;

namespace BFM.WPF.MainUI
{
    /// <summary>
    /// TestPage.xaml 的交互逻辑
    /// </summary>
    public partial class TestPage : Page
    {
        WcfClient<ISDMService> ws = new WcfClient<ISDMService>();
        private WcfClient<IFMSService> wsFMS = new WcfClient<IFMSService>();
        private WcfClient<IRSMService> ws2 = new WcfClient<IRSMService>();
        private WcfClient<IEAMService> wsEAM = new WcfClient<IEAMService>();
        public TestPage()
        {
            InitializeComponent();

            List<SysEnumMain> mains = ws.UseService(s => s.GetSysEnumMains("USE_FLAG >= 0"));
            cmbBasic.ItemsSource = mains;

            List<FmsAssetTagSetting> tags = wsFMS.UseService(s => s.GetFmsAssetTagSettings("USE_FLAG = 1")).OrderBy(c => c.ASSET_CODE).ThenBy(c => c.TAG_NAME).ToList();
            cmbTags.ItemsSource = tags;


            List<RsLine> lines = ws2.UseService(s => s.GetRsLines("USE_FLAG = 1"));
            cmbLine.ItemsSource = lines;
            cmbLine.SelectedValue = CBaseData.CurLinePKNO;

            cmbDevices.ItemsSource = wsEAM.UseService(s => s.GetAmAssetMasterNs("USE_FLAG = 1"));

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            tbVersion.Clear();
            foreach (SysAppInfo appInfo in VersionManage.AllModels)
            {
                tbVersion.Text += "App: " + appInfo.APP_NAME + " Version: " + appInfo.MODEL_INNER_VERSION +
                                  " VersionName: " + appInfo.MODEL_VERSION + Environment.NewLine;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SysEnumMain main = cmbBasic.SelectedItem as SysEnumMain;
            if (main == null)
            {
                return;
            }
            cmbTest.EnumIdentify = main.ENUM_IDENTIFY;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //tbResult.Text = cmbTest.Value;
            cmbTest.Save();
        }

        private void button_Click_2(object sender, RoutedEventArgs e)
        {
            string newAssetNO = TableNOHelper.GetNewNO("AM_ASSET_MASTER_N.ASSET_CODE");

            MessageBox.Show(newAssetNO);
        }

        private void Test_Click_2(object sender, RoutedEventArgs e)
        {
            DeviceMonitor device = new DeviceMonitor();
            device.Do();
            bGo.IsEnabled = false;
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            int ret = DeviceProcessControl.RunByLine(cmbLine.SelectedValue?.ToString());
            btnRun.IsEnabled = (ret == 0);
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            int ret = DeviceProcessControl.PauseByLine(cmbLine.SelectedValue?.ToString());
            btnPause.IsEnabled = (ret == 0);
        }

        private void 查看_Click(object sender, RoutedEventArgs e)
        {
            stateShow.Title = cmbTags.Text + "  ";
            stateShow.ShowDate = dpSearch.SelectedDate.Value;
            List<TimeFormat> timesFormats = new List<TimeFormat>();
            try
            {

                List<FmsStateResultRecord> records =
                    wsFMS.UseService(
                        s =>
                            s.GetFmsStateResultRecords(
                                $"TAG_SETTING_PKNO = '{cmbTags.SelectedValue}' AND END_TIME >= '{dpSearch.SelectedDate.Value}' " +
                                $"AND END_TIME < '{dpSearch.SelectedDate.Value.AddDays(1)}'"))
                        .OrderBy(c => c.CREATION_DATE).ThenBy(c => c.BEGINT_TIME).ThenBy(c => c.END_TIME)
                        .ToList();

                TimeSpan runTime = TimeSpan.Zero;
                TimeSpan errorTime = TimeSpan.Zero;
                TimeSpan standTime = TimeSpan.Zero;

                foreach (FmsStateResultRecord record in records)
                {
                    TimeFormat timeFormat = new TimeFormat()
                    {
                        BeginTime = record.BEGINT_TIME.Value,
                        EndTime = record.END_TIME.Value,
                        //ShowText = true,
                        Name = record.TAG_VALUE_NAME,
                    };
                    TimeSpan span = record.END_TIME.Value - record.BEGINT_TIME.Value;

                    if (record.TAG_VALUE == "1") //运行
                    {
                        timeFormat.StateText = "运行中";
                        timeFormat.StateColor = Brushes.Green;
                        timeFormat.BordBrush = Brushes.Green;
                        runTime += span;
                    }
                    else if (record.TAG_VALUE == "2") //故障
                    {
                        timeFormat.StateText = "故障";
                        timeFormat.StateColor = Brushes.Red;
                        timeFormat.BordBrush = Brushes.Red;
                        errorTime += span;
                    }
                    else if (record.TAG_VALUE == "3") //待机
                    {
                        timeFormat.StateText = "待机";
                        timeFormat.StateColor = Brushes.Yellow;
                        timeFormat.BordBrush = Brushes.Yellow;
                        standTime += span;
                    }
                    else
                    {
                        timeFormat.StateText = "未知";
                        timeFormat.ShowText = false;
                        timeFormat.StateColor = Brushes.Gray;
                        timeFormat.BordBrush = Brushes.Gray;
                    }
                    timesFormats.Add(timeFormat);
                }
                stateShow.StateData = timesFormats;

                TimeSpan allTime = runTime + errorTime + standTime;

                if (allTime.Seconds != 0)
                {
                    tbOEE.Text =
                        $"总监控时间（{allTime.Hours} 小时 {allTime.Minutes}分 {allTime.Seconds} 秒）" +
                        $" 运行时间 （{runTime.Hours} 小时 {runTime.Minutes}分 {runTime.Seconds} 秒）" +
                        $" 待机时间 （{standTime.Hours} 小时 {standTime.Minutes}分 {standTime.Seconds} 秒）" +
                        $" 故障时间 （{errorTime.Hours} 小时 {errorTime.Minutes}分 {errorTime.Seconds} 秒）" +
                        $" 利用率（开工率） {((runTime.TotalSeconds*100.0)/allTime.TotalSeconds).ToString("f2")}%" +
                        $" 故障率 {((errorTime.TotalSeconds * 100.0)/allTime.TotalSeconds).ToString("f2")}%";
                }
                else
                {
                    tbOEE.Text = "";
                }

            }
            catch (Exception)
            {

            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
        }

        private void bTestMessageBoxInfo_Click(object sender, RoutedEventArgs e)
        {
            WPFMessageBox.ShowInfo((sender as Button).Content.ToString(),  "测试");
        }

        private void bTestMessageBoxWarring_Click(object sender, RoutedEventArgs e)
        {
            WPFMessageBox.ShowWarring((sender as Button).Content.ToString(), "测试");
        }

        private void bTestMessageBoxError_Click(object sender, RoutedEventArgs e)
        {
            WPFMessageBox.ShowError((sender as Button).Content.ToString(), "测试");
        }

        private void bTestMessageBoxTips_Click(object sender, RoutedEventArgs e)
        {
            WPFMessageBox.ShowTips((sender as Button).Content.ToString(), "测试");
        }

        private void bTestMessageBoxConfirm_Click(object sender, RoutedEventArgs e)
        {
            WPFMessageBoxResult result = WPFMessageBox.ShowConfirm((sender as Button).Content.ToString(), "测试");

            WPFMessageBox.ShowInfo((result == WPFMessageBoxResult.OK ? "OK" : "Cancel"), "结果");
        }

        private void bTestMessageBoxQuestion_Click(object sender, RoutedEventArgs e)
        {
            WPFMessageBoxResult result = WPFMessageBox.ShowQuestion((sender as Button).Content.ToString(), "测试");

            WPFMessageBox.ShowInfo((result == WPFMessageBoxResult.Yes ? "Yes" :
                result == WPFMessageBoxResult.No ? "NO" : "Cancel"), "结果");
        }

        //读 - 线程
        private void bRead_Click(object sender, RoutedEventArgs e)
        {
            new Thread(ReadThread).Start(cmbTags.SelectedItem);
        }

        //界面读
        private void bRead2_Click(object sender, RoutedEventArgs e)
        {
            ReadThread(cmbTags.SelectedItem);
        }

        private void ReadThread(object obj)
        {
            FmsAssetTagSetting tag = obj as FmsAssetTagSetting;

            if (tag == null) return;
            string error = "";

            string value = DeviceMonitor.ReadTagFormDevice(tag.PKNO, out error);
            if (!error.IsEmpty())
            {
                value = error;
            }

            Dispatcher.BeginInvoke((Action)(delegate ()
            {
                tbTagResult.Text = value;
            }));
        }

        //写
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            FmsAssetTagSetting tag = cmbTags.SelectedItem as FmsAssetTagSetting;
            if (tag == null) return;

            string error = "";
            string sTagValue = tbTagResult.Text;
            DeviceMonitor.WriteTagToDevice(tag.PKNO, sTagValue, out error);  //写入执行的值
            if (!error.IsEmpty())
            {
                DeviceMonitor.WriteTagToDevice(tag.PKNO, sTagValue, out error);  //写入执行的值
                if (!error.IsEmpty())
                {
                    tbTagResult.Text = error;
                }
            }
        }

        //写并置位
        private void btnSetAndReset_Click(object sender, RoutedEventArgs e)
        {
            FmsAssetTagSetting tag = cmbTags.SelectedItem as FmsAssetTagSetting;
            if (tag == null) return;

            string error = "";
            string sTagValue = tbTagResult.Text;
            DeviceMonitor.WriteTagToDevice(tag.PKNO, sTagValue, out error);  //写入执行的值
            if (!error.IsEmpty())
            {
                DeviceMonitor.WriteTagToDevice(tag.PKNO, sTagValue, out error);  //写入执行的值
                if (!error.IsEmpty())
                {
                    tbTagResult.Text = error;
                }
            }
            else
            {
                Thread.Sleep(200);
                DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);  //写入执行的值 置0
                if (!error.IsEmpty())
                {
                    DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);  //写入执行的值 置0
                    if (!error.IsEmpty())
                    {
                        tbTagResult.Text = error;
                    }
                }
            }
        }

        private void cmbTags_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbTagResult.Text = "";
        }

        private void 打开_Click(object sender, RoutedEventArgs e)
        {
            IEVersionHelper.SetIE(IeVersion.标准ie10);

            wbTarget.Source = new Uri(tbUrl.Text);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            wbTarget.NavigateToString(tbUrl.Text);
        }

        private void Button33_Click(object sender, RoutedEventArgs e)
        {
            WcfClient<IPLMService> ws = new WcfClient<IPLMService>();
            List<MesJobOrder> productProcesses = ws.UseService(s =>
                s.GetMesJobOrders(""));  //正在执行的产品信息
            MesJobOrder productProcess = productProcesses.FirstOrDefault();  //产品生产情况;

            productProcess.PKNO = "";
        }

        private void CmbDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string acessCode = cmbDevices.SelectedValue?.ToString();

            if (!string.IsNullOrEmpty(acessCode))
            {
                cmbTags.ItemsSource = wsFMS.UseService(s =>
                        s.GetFmsAssetTagSettings($"USE_FLAG = 1 AND ASSET_CODE = '{acessCode}'"))
                    .OrderBy(c => c.ASSET_CODE)
                    .ThenBy(c => c.TAG_NAME).ToList();
            }
            else
            {
                cmbTags.ItemsSource = wsFMS.UseService(s =>
                        s.GetFmsAssetTagSettings($"USE_FLAG = 1 "))
                    .Where(c => !string.IsNullOrEmpty(c.TAG_ADDRESS))
                    .OrderBy(c => c.ASSET_CODE)
                    .ThenBy(c => c.TAG_NAME).ToList();
            }
        }

        private void btnTestCombox_Click(object sender, RoutedEventArgs e)
        {
            string acessCode = cmbDevices.SelectedValue?.ToString();
            if (string.IsNullOrEmpty(cmbDevices.Text))
            {
                acessCode = "";
            }
            Console.WriteLine($"==========  {acessCode}  ==========");

            acessCode = cmbLine_Copy.SelectedValue?.ToString();
            Console.WriteLine($"==========  {acessCode}  ==========");
        }

        List<FmsAssetTagSetting> copyTags = new List<FmsAssetTagSetting>();

        private void btnTestCopy_Click(object sender, RoutedEventArgs e)
        {
            copyTags.Clear();
            #region 复制当前Tag值，保证数据一致性，作为判断条件用

            List<FmsAssetTagSetting> tags = DeviceMonitor.GetTagSettings("");

            foreach (var tag in tags)
            {
                FmsAssetTagSetting copyTag = new FmsAssetTagSetting();
                copyTag.CopyDataItem(tag);
                copyTags.Add(copyTag);
            }

            #endregion
        }

        private void btnTest2_Click(object sender, RoutedEventArgs e)
        {
            string pkno = cmbTags.SelectedValue?.ToString();

            tbValue.Text = copyTags.FirstOrDefault(c => c.PKNO == pkno)?.CUR_VALUE ?? "";

            tbValueOld.Text = DeviceMonitor.GetTagSettingById(pkno)?.CUR_VALUE ?? "";
        }
    }
}
