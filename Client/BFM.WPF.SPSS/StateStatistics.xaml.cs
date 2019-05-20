using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.DAService;
using BFM.WPF.Base.Controls;
using DevExpress.Xpf.Charts;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BFM.WPF.SPSS
{
    /// <summary>
    /// Interaction logic for StateStatistics.xaml
    /// </summary>
    public partial class StateStatistics : Page
    {

        private WcfClient<IDAService> wsDA = new WcfClient<IDAService>();

        // 柱形图数据源
        private List<SeriesPoint> lsBarPoint = new List<SeriesPoint>();

        // StepLine数据源
        private List<SeriesPoint> lsLinePoint = new List<SeriesPoint>();

        // 时间戳数据源
        private List<TimeFormat> timesFormats = new List<TimeFormat>();

        // 状态枚举转化类型
        private enum state
        {
            运行 = 1,
            故障 = 2,
            待机 = 3,
            未知 = 4
        }

        public StateStatistics()
        {
            InitializeComponent();
        }

        #region 绑定数据
        private void BindData(DateTime startTime, DateTime endTime)
        {
            stateControl.ItemsSource = null;
            getData(startTime, endTime);

            //BarSideSerie.Points.Clear();
            //BarSideSerie.Points.AddRange(lsBarPoint);
            //BarSideSerie.Animate();
            LineSideSerie.Points.Clear();
            LineSideSerie.Points.AddRange(lsLinePoint);
            LineSideSerie.Animate();

            barChart.UpdateData();         
            
        }
        private void BindData(DateTime startTime, DateTime endTime, string deviceNumber)
        {
            stateControl.ItemsSource = null;
            getData(startTime, endTime, deviceNumber);

            //BarSideSerie.Points.Clear();
            //BarSideSerie.Points.AddRange(lsBarPoint);
            //BarSideSerie.Animate();
            LineSideSerie.Points.Clear();
            LineSideSerie.Points.AddRange(lsLinePoint);
            LineSideSerie.Animate();

            barChart.UpdateData();
          
        }
        #endregion

        #region 获取数据并加工

        private void getData(DateTime startTime, DateTime endTime)
        {
            List<DAStatusRecord> daStatusRecord = wsDA.UseService(s => s.GetDAStatusRecords(" START_TIME > '" + startTime.ToString("yyyy-MM-dd HH:mm:ss") + "' AND END_TIME < '" + endTime.ToString("yyyy-MM-dd HH:mm:ss") + "'")).OrderBy(c => c.ASSET_CODE).ToList();

            try
            {
                lsBarPoint.Clear();
                lsLinePoint.Clear();
                timesFormats.Clear();
                stateShow.StateData.Clear();
                stateShow.Title = "  ";
                stateShow.RefreshDraw();
                if (daStatusRecord.Count == 0)
                {
                    MessageBox.Show("查询的数据不存在.");
                    return;
                }

                stateControl.ItemsSource = daStatusRecord;

                Dictionary<string, int> dictionary = new Dictionary<string, int>();
                for (int i = 0; i < daStatusRecord.Count; i++)
                {
                    if (dictionary.ContainsKey(Enum.GetName(typeof(state), daStatusRecord[i].RUN_STATUS)))
                    {
                        TimeSpan time = Convert.ToDateTime(daStatusRecord[i].END_TIME) - Convert.ToDateTime(daStatusRecord[i].START_TIME);
                        dictionary[Enum.GetName(typeof(state), daStatusRecord[i].RUN_STATUS)] += Convert.ToInt32(time.TotalHours);
                    }
                    else
                    {
                        TimeSpan time = Convert.ToDateTime(daStatusRecord[i].END_TIME) - Convert.ToDateTime(daStatusRecord[i].START_TIME);
                        int hours = Convert.ToInt32(time.TotalHours);
                        dictionary.Add(Enum.GetName(typeof(state), daStatusRecord[i].RUN_STATUS), hours);
                    }
                }

                foreach (var item in dictionary)
                {
                    //lsBarPoint.Add(new SeriesPoint() { Argument = item.Key, Value = item.Value });
                    lsLinePoint.Add(new SeriesPoint() { Argument = item.Key, Value = item.Value });
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void getData(DateTime startTime, DateTime endTime, string deviceNumber)
        {
            stateShow.Title = deviceNumber;
            stateShow.ShowDate = startTime;

            List<DAStatusRecord> daStatusRecord = wsDA.UseService(s => s.GetDAStatusRecords(" START_TIME > '" + startTime.ToString("yyyy-MM-dd HH:mm:ss") + "' AND END_TIME < '" + endTime.ToString("yyyy-MM-dd HH:mm:ss") + "' AND ASSET_CODE = '" + deviceNumber + "'")).OrderBy(c => c.START_TIME).ToList();
            try
            {
                lsBarPoint.Clear();
                lsLinePoint.Clear();
                timesFormats.Clear();
                stateShow.StateData.Clear();

                if (daStatusRecord.Count == 0)
                {
                    MessageBox.Show("查询的数据不存在.");
                    return;
                }

                stateControl.ItemsSource = daStatusRecord;

                Dictionary<string, int> dictionary = new Dictionary<string, int>();
                for (int i = 0; i < daStatusRecord.Count; i++)
                {
                    if (dictionary.ContainsKey(Enum.GetName(typeof(state), daStatusRecord[i].RUN_STATUS)))
                    {
                        TimeSpan time = Convert.ToDateTime(daStatusRecord[i].END_TIME) - Convert.ToDateTime(daStatusRecord[i].START_TIME);
                        dictionary[Enum.GetName(typeof(state), daStatusRecord[i].RUN_STATUS)] += Convert.ToInt32(time.TotalHours);
                    }
                    else
                    {
                        TimeSpan time = Convert.ToDateTime(daStatusRecord[i].END_TIME) - Convert.ToDateTime(daStatusRecord[i].START_TIME);
                        int hours = Convert.ToInt32(time.TotalHours);
                        dictionary.Add(Enum.GetName(typeof(state), daStatusRecord[i].RUN_STATUS), hours);
                    }
                }

                foreach (var item in dictionary)
                {
                    //lsBarPoint.Add(new SeriesPoint() { Argument = item.Key, Value = item.Value });
                    lsLinePoint.Add(new SeriesPoint() { Argument = item.Key, Value = item.Value });
                }

                TimeSpan runTime = TimeSpan.Zero;
                TimeSpan errorTime = TimeSpan.Zero;
                TimeSpan standTime = TimeSpan.Zero;
                

                foreach (DAStatusRecord record in daStatusRecord)
                {
                    TimeFormat timeFormat = new TimeFormat()
                    {
                        BeginTime = record.START_TIME.Value,
                        EndTime = record.END_TIME.Value,
                        //ShowText = true,
                        Name = Enum.GetName(typeof(state), record.RUN_STATUS),
                    };
                    TimeSpan span = record.END_TIME.Value - record.START_TIME.Value;

                    if (record.RUN_STATUS.ToString() == "1") //运行
                    {
                        timeFormat.StateText = "运行中";
                        timeFormat.StateColor = Brushes.MediumSeaGreen;
                        timeFormat.BordBrush = Brushes.MediumSeaGreen;
                        runTime += span;
                    }
                    else if (record.RUN_STATUS.ToString() == "2") //故障
                    {
                        timeFormat.StateText = "故障";
                        timeFormat.StateColor = Brushes.Firebrick;
                        timeFormat.BordBrush = Brushes.Firebrick;
                        errorTime += span;
                    }
                    else if (record.RUN_STATUS.ToString() == "3") //待机
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
                stateShow.RefreshDraw();

                TimeSpan allTime = runTime + errorTime + standTime;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
        #endregion

        #region 按钮动作
        private void bSearch4_click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(StartTime.Text))
            {
                MessageBox.Show("请输入开始时间！");
                return;
            }

            if (string.IsNullOrEmpty(EndTime.Text))
            {
                MessageBox.Show("请输入结束时间！");
                return;
            }
            // 获取前台传来的时间
            DateTime startTime = Convert.ToDateTime(StartTime.Text);
            DateTime endTime = Convert.ToDateTime(EndTime.Text);
            // 对时间进行判断
            if (startTime > endTime)
            {
                MessageBox.Show("开始时间不能大于结束时间！");
                return;
            }
            string deviceNumber = DeviceNumber.Text;
            if (deviceNumber == null || "".Equals(deviceNumber))
            {
                BindData(startTime, endTime);
            }
            else
            {
                if ((endTime - startTime).TotalDays > 1)
                {
                    MessageBox.Show("请选择一天的时差!");
                    return;
                }
                BindData(startTime, endTime, deviceNumber);
            }
        }
        #endregion
    }
}
