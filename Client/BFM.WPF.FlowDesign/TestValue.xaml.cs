using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BFM.Common.Data.PubData;
using BFM.Common.Data.Utils;
using BFM.WPF.Base.Controls;
using CSharp.WPF.FlowDesign.MESService;
using MessageBox = System.Windows.MessageBox;

namespace CSharp.WPF.FlowDesign
{
    /// <summary>
    /// TestValue.xaml 的交互逻辑
    /// </summary>
    public partial class TestValue : Window
    {
        private readonly System.Timers.Timer timer;
        private readonly System.Timers.Timer timer2;

        private bool bFirstRecord = true;

        private WcfClient<IMESService> ws2 = new WcfClient<IMESService>();

        public TestValue()
        {
            InitializeComponent();

            //this.timer2 = new System.Timers.Timer(2000);
            //timer2.Elapsed += Timer2_Elapsed;
            //timer2.Start();

            //this.timer = new System.Timers.Timer(3000);
            //timer.Elapsed += Timer_Elapsed;
            //timer.Start();
        }

        private void Timer2_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.timer2.Enabled = false;

            try
            {
                string deviceNO = "";
                string deviceState = "";
                double allWidth = 0;
                double height = 30;
                double hSpan = 10;

                DateTime searchDate = DateTime.Now.Date;

                Dispatcher.BeginInvoke((Action)(delegate ()
                {
                    deviceNO = this.tbDeviceNO.Text;
                    deviceState = this.tbDeviceState.Text;
                    allWidth = canvas1.ActualWidth;
                    var selectedDate = this.dpSearch.SelectedDate;
                    if (selectedDate != null) searchDate = selectedDate.Value;

                    canvas1.Children.Clear();

                    #region 绘制日期

                    for (int i = 0; i < 24; i++)
                    {
                        TextBlock tb = new TextBlock()
                        {
                            Text = i.ToString() + ":00",
                        };

                        Canvas.SetLeft(tb, (i + 0.3) * allWidth / 24);
                        Canvas.SetTop(tb, 0);
                        canvas1.Children.Add(tb);

                        Line line = new Line()
                        {
                            X1 = (i * allWidth / 24),
                            Y1 = 10,
                            X2 = (i * allWidth / 24),
                            Y2 = canvas1.ActualHeight,
                            StrokeThickness = 1,
                            Stroke = Brushes.Gray,
                            //StrokeDashArray = new DoubleCollection() { 2, 3},
                        };
                        canvas1.Children.Add(line);

                        for (int j = 0; j < 9; j++)
                        {
                            Line line2 = new Line()
                            {
                                X1 = ((i + (j + 1.0)/10)*allWidth/24),
                                Y1 = (j == 4) ? 12 : 15,
                                X2 = ((i + (j + 1.0) / 10) *allWidth/24),
                                Y2 = 20, //
                                StrokeThickness = 1,
                                Stroke = Brushes.Gray,
                                //StrokeDashArray = new DoubleCollection() { 2, 3 },
                            };
                            canvas1.Children.Add(line2);  //中间刻度
                        }
                    }

                    Line last = new Line()
                    {
                        X1 = allWidth,
                        Y1 = 0,
                        X2 = allWidth,
                        Y2 = canvas1.ActualHeight,
                        StrokeThickness = 1,
                        Stroke = Brushes.Gray,
                        //StrokeDashArray = new DoubleCollection() { 2, 3 },
                    };
                    canvas1.Children.Add(last);

                    Brush brush = Brushes.LightSlateGray;
                    Rectangle retc = new Rectangle()
                    {
                        Width = allWidth,
                        Height = height,
                        Fill = brush,
                        Stroke = brush,
                        StrokeThickness = 1
                    };

                    Canvas.SetLeft(retc, 0);
                    Canvas.SetTop(retc, 10 + hSpan);

                    canvas1.Children.Add(retc);

                    #endregion

                    //Rectangle retc = new Rectangle()
                    //{
                    //    Width = allWidth,
                    //    Height = height,
                    //    Fill = Brushes.White,
                    //    Stroke = Brushes.Black,
                    //    StrokeThickness = 1
                    //};
                    //Canvas.SetLeft(retc, 0);

                    //canvas1.Children.Add(retc);
                }));

                #region 加载数据

                Dictionary<string, string> where = new Dictionary<string, string>();
                where.Add("DevicePKNO", deviceNO);
                string jsWhere = JsonSerializer.GetJsonString(where);
                List<MesStateResultRecord> records =
                    ws2.UseService(s => s.GetAllMesStateResultRecord(jsWhere))
                        .Where(c => ((c.BeginTime >= searchDate && c.BeginTime < searchDate.AddDays(1)) ||
                                     (c.EndTime >= searchDate && c.EndTime < searchDate.AddDays(1)) ||
                                     (c.BeginTime < searchDate && c.EndTime >= searchDate.AddDays(1))))
                        //开始时间或结束时间在查询的当天，或者时间包含查询的查询天
                        .OrderBy(c => c.BeginTime)
                        .ToList();
                MesStateResultRecord first = records.FirstOrDefault();
                if (first != null)
                {
                    if (first.BeginTime < searchDate)
                    {
                        first.BeginTime = searchDate;
                    }
                }


                foreach (MesStateResultRecord record in records)
                {
                    DateTime minTime = (DateTime) record.BeginTime;
                    DateTime maxTime = (DateTime) record.EndTime;
                    if (minTime < searchDate)
                    {
                        minTime = searchDate;
                    }
                    if (maxTime > searchDate.AddDays(1))
                    {
                        maxTime = searchDate.AddDays(1);
                    }

                    Double left = 0;

                    if (minTime > searchDate)
                    {
                        double beginSpan = minTime.Subtract(searchDate).TotalSeconds; //按照秒计算
                        left = allWidth*beginSpan/(60*60*24);
                    }

                    double span = maxTime.Subtract(minTime).TotalSeconds; //按照秒计算

                    double thisWidth = allWidth*span/(60*60*24);
                    Brush brush = (record.TagValue == "1")
                        ? Brushes.Gray
                        : ((record.TagValue == "2")
                            ? Brushes.Blue
                            : ((record.TagValue == "3")
                                ? Brushes.Yellow
                                : ((record.TagValue == "4") ? Brushes.Red : Brushes.White)));
                    Dispatcher.BeginInvoke((Action) (delegate()
                    {
                        Rectangle retc = new Rectangle()
                        {
                            Width = thisWidth,
                            Height = height,
                            Fill = brush,
                            Stroke = brush,
                            StrokeThickness = 1
                        };

                        Canvas.SetLeft(retc, left);
                        Canvas.SetTop(retc, 10 + hSpan);

                        canvas1.Children.Add(retc);
                    }));
                }

                #endregion

            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }

            this.timer2.Enabled = true;

        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.timer.Enabled = false;

            string deviceNO = "1";
            string deviceState = "2";
            try
            {

                Dispatcher.BeginInvoke((Action)(delegate ()
               {
                   deviceNO = this.tbDeviceNO.Text;
                   deviceState = this.tbDeviceState.Text;
               }));
            }
            catch (Exception ex)
            {

            }

            WcfClient<IMESService> ws = new WcfClient<IMESService>();
            try
            {
                //数据采集
                #region 写入数据库

                if (bFirstRecord)
                {
                    MesStateResultRecord record = new MesStateResultRecord()
                    {
                        PKNO = CBaseData.NewGuid(),
                        DevicePKNO = deviceNO,
                        TagValue = deviceState,
                        BeginTime = DateTime.Now,
                        EndTime = DateTime.Now,
                    };
                    Console.WriteLine(record.PKNO);
                    ws.UseService(s => s.AddMesStateResultRecord(record));
                }
                else
                {
                    List<MesStateResultRecord> records = ws.UseService(s => s.GetAllMesStateResultRecord(""));
                    MesStateResultRecord record = records.Where(c => c.DevicePKNO == deviceNO)
                        .OrderByDescending(c => c.EndTime)
                        .FirstOrDefault();
                    if ((record == null) || (record.EndTime <= DateTime.Now.AddMinutes(-10)))    //10分钟之前的数据则重新增加
                    {
                        record = new MesStateResultRecord()
                        {
                            PKNO = CBaseData.NewGuid(),
                            DevicePKNO = deviceNO,
                            TagValue = deviceState,
                            BeginTime = DateTime.Now,
                            EndTime = DateTime.Now,
                        };
                        Console.WriteLine(record.PKNO);
                        ws.UseService(s => s.AddMesStateResultRecord(record));
                    }
                    else
                    {
                        DateTime nowTime = DateTime.Now;
                        record.EndTime = nowTime;
                        ws.UseService(s => s.UpdateMesStateResultRecord(record));

                        if (record.TagValue != deviceState) //不相同情况下新增
                        {
                            MesStateResultRecord newRecord = new MesStateResultRecord()
                            {
                                PKNO = CBaseData.NewGuid(),
                                DevicePKNO = deviceNO,
                                TagValue = deviceState,
                                BeginTime = nowTime,
                                EndTime = nowTime.AddSeconds(1),  //提取最新数据
                            };
                            Console.WriteLine(newRecord.PKNO);
                            ws.UseService(s => s.AddMesStateResultRecord(newRecord));
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            bFirstRecord = false;

            this.timer.Enabled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            List<MesStateResultRecord> records =
                ws2.UseService(s => s.GetAllMesStateResultRecord("")).OrderBy(c => c.BeginTime).ToList();
            dataGrid.ItemsSource = records;
            
        }

        private void canvas1_Copy_StateItemMouseDown(object sender, RoutedEventArgs e)
        {
            TimeFormat item = (TimeFormat)sender;
            MessageBox.Show(item.BeginTime + "   " + item.EndTime + "   " + item.StateText);
        }

        private void canvas1_Copy_StateItemMouseMove(object sender, RoutedEventArgs e)
        {

        }

        private void canvas1_Copy_StateItemMouseDown_1(object sender, RoutedEventArgs e)
        {
            TimeFormat item = (TimeFormat)e.OriginalSource;
            MessageBox.Show(item.BeginTime + "   " + item.EndTime + "   " + item.StateText);

        }

        private void canvas1_Copy_StateItemMouseMove_1(object sender, RoutedEventArgs e)
        {
            //TimeFormat item = (TimeFormat)e.OriginalSource;
            //canvas1_Copy.ToolTip = item.BeginTime + "   " + item.EndTime + "   " + item.StateText;
        }
    }
}
