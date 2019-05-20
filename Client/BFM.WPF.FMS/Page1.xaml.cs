using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.Data.PubData;
using BFM.Server.DataAsset.FMSService;
using BFM.Server.DataAsset.SQLService;
using BFM.ContractModel;

namespace BFM.WPF.FMS
{
    /// <summary>
    /// Page1.xaml 的交互逻辑
    /// </summary>
    public partial class Page1 : Page
    {
        private System.Timers.Timer timer;
        private System.Timers.Timer timer2;
        private System.Timers.Timer timer3;
        private System.Timers.Timer timer4;
        private System.Timers.Timer timer5;
        private System.Timers.Timer timer6;
        private WcfClient<IFMSService> ws2 = new WcfClient<IFMSService>();

        private object objLock = new object();


        public Page1()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            timer = new System.Timers.Timer(100);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }


        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.timer.Enabled = false;

            WcfClient<IFMSService> ws = new WcfClient<IFMSService>();
            //try
            //{
            lock (objLock)
            {
                FmsAssetCommParam comm = ws.UseService(s => s.GetFmsAssetCommParams("")).FirstOrDefault();
                comm.LAST_UPDATE_DATE = DateTime.Now;
                ws.UseService(s => s.UpdateFmsAssetCommParam(comm));
            }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            Console.WriteLine($"Thread Write{iWriteTest++}");
            this.timer.Enabled = true;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            DateTime dt1 = DateTime.Now;
            WcfClient<IFMSService> ws = new WcfClient<IFMSService>();
            List<FmsAssetCommParam> comm = ws.UseService(s => s.GetFmsAssetCommParams(""));
            gridItem.ItemsSource = comm;
            DateTime dt2 = DateTime.Now;
            TimeSpan span = dt2 - dt1;
            MessageBox.Show("共耗时 " + span.TotalSeconds + " 秒");
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            timer2 = new System.Timers.Timer(100);
            timer2.Elapsed += Timer2OnElapsed;
            timer2.Start();
        }

        private Int64 iReadTest = 0;
        private Int64 iWriteTest = 0;

        private void Timer2OnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            this.timer2.Enabled = false;

            WcfClient<IFMSService> ws = new WcfClient<IFMSService>();
            List<FmsAssetCommParam> comm = ws.UseService(s => s.GetFmsAssetCommParams(""));
            Dispatcher.BeginInvoke((Action) (delegate()
            {
                gridItem.ItemsSource = comm;
            }));

            Console.WriteLine($"Thread Read{iReadTest++}");

            this.timer2.Enabled = true;
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            DateTime dt1 = DateTime.Now;
            WcfClient<IFMSService> ws = new WcfClient<IFMSService>();
            List<FmsStateResultRecord> comm = ws.UseService(s => s.GetFmsStateResultRecords(""));
            Dispatcher.BeginInvoke((Action) (delegate()
            {
                gridItem2.ItemsSource = comm;
            }));
            DateTime dt2 = DateTime.Now;
            TimeSpan span = dt2 - dt1;
            MessageBox.Show("共耗时 " + span.TotalSeconds + " 秒");
        }

        private void button22_Click(object sender, RoutedEventArgs e)
        {
            timer3 = new System.Timers.Timer(100);
            timer3.Elapsed += Timer3_Elapsed;
            timer3.Start();
        }

        private void button32_Click(object sender, RoutedEventArgs e)
        {
            timer4 = new System.Timers.Timer(100);
            timer4.Elapsed += Timer4_Elapsed;
            timer4.Start();
        }

        private Int64 iReadTest2 = 0;
        private Int64 iWriteTest2 = 0;

        private void Timer3_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.timer3.Enabled = false;

            WcfClient<IFMSService> ws = new WcfClient<IFMSService>();
            //try
            //{
            lock (objLock)
            {
                FmsStateResultRecord comm = new FmsStateResultRecord()
                {
                    PKNO = CBaseData.NewGuid(),
                    ASSET_CODE = "A20002",
                    BEGINT_TIME = DateTime.Now,
                    END_TIME = DateTime.Now,
                    CREATED_BY = CBaseData.LoginName,
                    CREATION_DATE = DateTime.Now,
                    REMARK = "",
                    TAG_SETTING_PKNO = "fe0e40d4bb57424088c1876bba50f229",
                    TAG_VALUE = (new Random()).Next(200).ToString(),
                    TAG_VALUE_NAME = "测试",
                };
                ws.UseService(s => s.AddFmsStateResultRecord(comm));
            }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            Console.WriteLine($"2Thread Write{iWriteTest2++}");
            this.timer3.Enabled = true;
        }


        private void Timer4_Elapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            this.timer4.Enabled = false;

            WcfClient<IFMSService> ws = new WcfClient<IFMSService>();
            List<FmsStateResultRecord> comm =
                ws.UseService(s => s.GetFmsStateResultRecords("")).OrderByDescending(c => c.CREATION_DATE).ToList();
            Dispatcher.BeginInvoke((Action) (delegate()
            {
                gridItem2.ItemsSource = comm;
            }));

            Console.WriteLine($"2Thread Read{iReadTest2++}");

            this.timer4.Enabled = true;
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            WcfClient<ISQLService> ws = new WcfClient<ISQLService>();
            int a =
                ws.UseService(
                    s =>
                        s.RowCountWithEF("SELECT COUNT(*) FROM FMS_STATERESULT_RECORD", new List<string>(), new List<string>()));

            Console.WriteLine(a);
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            WcfClient<ISQLService> ws = new WcfClient<ISQLService>();
            FmsStateResultRecord record = gridItem2.SelectedItem as FmsStateResultRecord;
            if (record != null)
            {
                ws.UseService(
                    s =>
                        s.ExecuteSql(
                            $"UPDATE FMS_STATERESULT_RECORD SET TAG_VALUE = '0' WHERE PKNO = '{record.PKNO}'"));
            }
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            timer5 = new System.Timers.Timer(100);
            timer5.Elapsed += Timer5_Elapsed;
            timer5.Start();
        }

        private void button9_Click(object sender, RoutedEventArgs e)
        {
            timer6 = new System.Timers.Timer(100);
            timer6.Elapsed += Timer6_Elapsed;
            timer6.Start();
        }


        private void Timer5_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.timer5.Enabled = false;

            WcfClient<IFMSService> ws = new WcfClient<IFMSService>();
            //try
            //{
            lock (objLock)
            {
                FmsSamplingRecord record = new FmsSamplingRecord()
                {
                    PKNO = CBaseData.NewGuid(),
                    ASSET_CODE = "A20002",
                    SAMPLING_TIME = DateTime.Now,
                    TAG_SETTING_PKNO = "fe0e40d4bb57424088c1876bba50f229",
                    TAG_VALUE_NAME = "测试",
                    TAG_VALUE = (new Random()).Next(100).ToString(),
                    CREATED_BY = CBaseData.LoginName,
                    CREATION_DATE = DateTime.Now,
                    REMARK = "",
                };
                ws.UseService(s => s.AddFmsSamplingRecord(record));
            }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            Console.WriteLine($"3Thread Write{iWriteTest2++}");
            this.timer5.Enabled = true;
        }


        private void Timer6_Elapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            this.timer6.Enabled = false;

            WcfClient<IFMSService> ws = new WcfClient<IFMSService>();
            List<FmsSamplingRecord> comm =
                ws.UseService(s => s.GetFmsSamplingRecords("")).OrderByDescending(c => c.CREATION_DATE).ToList();
            Dispatcher.BeginInvoke((Action)(delegate ()
            {
                gridItem3.ItemsSource = comm;
            }));

            Console.WriteLine($"3Thread Read{iReadTest2++}");

            this.timer6.Enabled = true;
        }
    }
}
