using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.DAService;
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
using System.Windows.Shapes;

namespace BFM.WPF.SPSS
{
    /// <summary>
    /// Interaction logic for AlarmStatistics.xaml
    /// </summary>
    public partial class AlarmStatistics : Page
    {
        // 调用WCF服务端的对象
        private WcfClient<IDAService> wsDA = new WcfClient<IDAService>();

        // 报警信息饼状图数据源
        private List<SeriesPoint> lsPiePoint = new List<SeriesPoint>();

        // 报警信息柱状图数据源
        private List<SeriesPoint> lsLinePoint = new List<SeriesPoint>();

        public AlarmStatistics()
        {
            InitializeComponent();
            LoadingDateEdit();
        }

        #region 绑定数据
        private void BindData(DateTime startTime, DateTime endTime)
        {
            getData(startTime, endTime);

            LineSideSerie.Points.Clear();
            LineSideSerie.Points.AddRange(lsLinePoint);
            LineSideSerie.Animate();

            pieSerie.Points.Clear();
            pieSerie.Points.AddRange(lsPiePoint);
            pieSerie.Animate();

            barChart.UpdateData();
            pieChart.UpdateData();


        }

        private void BindData(DateTime startTime, DateTime endTime, string assetNumber)
        {
            getData(startTime, endTime, assetNumber);

            LineSideSerie.Points.Clear();
            LineSideSerie.Points.AddRange(lsLinePoint);
            LineSideSerie.Animate();

            pieSerie.Points.Clear();
            pieSerie.Points.AddRange(lsPiePoint);
            pieSerie.Animate();

            barChart.UpdateData();
            pieChart.UpdateData();
        }
        #endregion


        #region 加载日期控件显示格式
        private void LoadingDateEdit()
        {
            this.StartTime.DisplayFormatString = "yyyy-MM-dd";
            this.StartTime.Mask = "yyyy-MM-dd";
            this.EndTime.DisplayFormatString = "yyyy-MM-dd";
            this.EndTime.Mask = "yyyy-MM-dd";

        }
        #endregion


        #region 获取数据并处理数据
        private void getData(DateTime startTime, DateTime endTime)
        {


            // 获取报警记录
            List<DAAlarmRecord> daAlarmRecord = wsDA.UseService(s => s.GetDAAlarmRecords(" START_TIME > '" + startTime.ToString("yyyy-MM-dd HH:mm:ss") + "' AND END_TIME < '" + endTime.ToString("yyyy-MM-dd HH:mm:ss") + "'")).OrderBy(c => c.ASSET_CODE).ToList();

            lsLinePoint.Clear();
            lsPiePoint.Clear();
            if (daAlarmRecord.Count < 1)
            {
                MessageBox.Show("查询结果不存才！");
                return;
            }
            alarmControl.ItemsSource = daAlarmRecord;

            Dictionary<string, int> dicValue = new Dictionary<string, int>();
            for (int i = 0; i < daAlarmRecord.Count; i++)
            {
                if (dicValue.ContainsKey(daAlarmRecord[i].ASSET_CODE))
                {
                    dicValue[daAlarmRecord[i].ASSET_CODE] += 1;
                }
                else
                {
                    dicValue.Add(daAlarmRecord[i].ASSET_CODE, 1);
                }
            }
            foreach (var item in dicValue)
            {
                lsLinePoint.Add(new SeriesPoint() { Argument = item.Key, Value = item.Value });
                lsPiePoint.Add(new SeriesPoint() { Argument = item.Key, Value = item.Value });
            }

        }

        private void getData(DateTime startTime, DateTime endTime, string AssetNumber)
        {
            List<DAAlarmRecord> daAlarmRecord = wsDA.UseService(s =>
                   s.GetDAAlarmRecords(" START_TIME > '" + startTime.ToString("yyyy-MM-dd HH:mm:ss") +
                                       "' AND END_TIME < '" + endTime.ToString("yyyy-MM-dd HH:mm:ss") +
                                       "' AND ASSET_CODE = '" + AssetNumber + "'"));//.OrderBy(c => c.START_TIME).ToList();
            lsLinePoint.Clear();
            lsPiePoint.Clear();

            if (daAlarmRecord.Count < 1)
            {
                MessageBox.Show("查询结果不存在");
                return;
            }
            alarmControl.ItemsSource = daAlarmRecord;

            Dictionary<string, int> dicValue = new Dictionary<string, int>();
            for (int i = 0; i < daAlarmRecord.Count; i++)
            {
                if (dicValue.ContainsKey(daAlarmRecord[i].ALARM_TYPE))
                {
                    dicValue[daAlarmRecord[i].ALARM_TYPE] += 1;
                }
                else
                {
                    dicValue.Add(daAlarmRecord[i].ALARM_TYPE, 1);
                }
            }

            foreach (var item in dicValue)
            {
                lsLinePoint.Add(new SeriesPoint() { Argument = item.Key, Value = item.Value });
                lsPiePoint.Add(new SeriesPoint() { Argument = item.Key, Value = item.Value });
            }
        }
        #endregion


        private void AlartSerachClick(object sender, RoutedEventArgs e)
        {
            if (StartTime.Text == null || StartTime.Text == "")
            {
                MessageBox.Show("请输入开始时间！");
                return;
            }

            if (EndTime.Text == null || EndTime.Text == "")
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

            if (DeviceNumber.Text == null || DeviceNumber.Text == "")
            {

                BindData(startTime, endTime);
            }
            else
            {
                BindData(startTime, endTime, DeviceNumber.Text);
            }


        }
    }
}
