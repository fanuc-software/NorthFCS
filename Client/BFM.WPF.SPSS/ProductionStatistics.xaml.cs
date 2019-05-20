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
using BFM.Server.DataAsset.DAService;
using BFM.Common.Base.Utils;
using BFM.ContractModel;
using DevExpress.Xpf.Charts;

namespace BFM.WPF.SPSS
{
    /// <summary>
    /// Interaction logic for ProductionStatistics.xaml
    /// </summary>
    public partial class ProductionStatistics : Page
    {

        private WcfClient<IDAService> wsDA = new WcfClient<IDAService>();

        // 工件计数柱形图数据源
        private List<SeriesPoint> lsBarPoint = new List<SeriesPoint>();

        // 工件总数柱形图数据源
        private List<SeriesPoint> lsBarPoint2 = new List<SeriesPoint>();

        // 饼状图数据源
        private List<SeriesPoint> lsPiePoint = new List<SeriesPoint>();


        public ProductionStatistics()
        {
            InitializeComponent();
            LoadingDateEdit();
            //LoadingAllDeviceNumber();
        }

        #region 加载日期控件显示格式
        private void LoadingDateEdit()
        {
            this.StartTime.DisplayFormatString = "yyyy-MM-dd";
            this.StartTime.Mask = "yyyy-MM-dd";
            this.EndTime.DisplayFormatString = "yyyy-MM-dd";
            this.EndTime.Mask = "yyyy-MM-dd";

        }
        #endregion

        #region 加载所有设备编号
        private void LoadingAllDeviceNumber()
        {
            List<MyClass1> DeviceList = new List<MyClass1>();
            DeviceList.Add(new MyClass1 { PKNO = "834e44295fb043bbb2b2cac70df09b9d", name = "text" });
            //DeviceList.Add(new MyClass1 { PKNO = "123456789", name = "text2" });
            DeviceNumber.Items.Clear();
            foreach (MyClass1 device in DeviceList)
            {
                DeviceNumber.Items.Add(device.PKNO);
            }
        }
        #endregion

        #region 绑定数据
        private void BindData(DateTime startTime, DateTime endTime)
        {
            productControl.ItemsSource = null;
            GetData(startTime, endTime);

            BarSideSerie.Points.Clear();
            BarSideSerie.Points.AddRange(lsBarPoint);
            BarSideSerie2.Points.Clear();
            BarSideSerie2.Points.AddRange(lsBarPoint2);
            BarSideSerie.Animate();
            BarSideSerie2.Animate();

            pieSerie.Points.Clear();
            pieSerie.Points.AddRange(lsPiePoint);
            pieSerie.Animate();

            barChart.UpdateData();
            pieChart.UpdateData();
        }

        private void BindData(DateTime startTime, DateTime endTime, string deviceNumber)
        {
            productControl.ItemsSource = null;
            GetData(startTime, endTime, deviceNumber);

            BarSideSerie.Points.Clear();
            BarSideSerie.Points.AddRange(lsBarPoint);
            BarSideSerie2.Points.Clear();
            BarSideSerie2.Points.AddRange(lsBarPoint2);
            BarSideSerie.Animate();
            BarSideSerie2.Animate();

            pieSerie.Points.Clear();
            pieSerie.Points.AddRange(lsPiePoint);
            pieSerie.Animate();

            barChart.UpdateData();
            pieChart.UpdateData();
        }

        #endregion

        #region 获取数据并加工
        private void GetData(DateTime startTime, DateTime endTime)
        {
            // 获取产量统计的数据
            List<DAProductRecord> daProductRecord = wsDA.UseService(s => s.GetDAProductRecords(" START_TIME > '" + startTime.ToString("yyyy-MM-dd HH:mm:ss") + "' AND END_TIME < '" + endTime.ToString("yyyy-MM-dd HH:mm:ss") + "'")).OrderBy(c => c.ASSET_CODE).ToList();

            try
            {
                lsBarPoint.Clear();
                lsBarPoint2.Clear();
                lsPiePoint.Clear();

                if (daProductRecord.Count == 0)
                {
                    MessageBox.Show("查询的数据不存在.");
                    return;
                }

                productControl.ItemsSource = daProductRecord;

                // 创建一个Dictionary,key是ASSET_CODE,value是中转类数据对象
                Dictionary<string, ProductionConvert> dicValue = new Dictionary<string, ProductionConvert>();
                for (int i = 0; i < daProductRecord.Count; i++)
                {
                    if (dicValue.ContainsKey(daProductRecord[i].ASSET_CODE))
                    {
                        dicValue[daProductRecord[i].ASSET_CODE].PART_NUM += daProductRecord[i].PART_NUM;
                        dicValue[daProductRecord[i].ASSET_CODE].TOTAL_PART_NUM += daProductRecord[i].TOTAL_PART_NUM;
                    }
                    else
                    {
                        ProductionConvert productionConvert = new ProductionConvert(daProductRecord[i].PART_NUM, daProductRecord[i].TOTAL_PART_NUM);

                        dicValue.Add(daProductRecord[i].ASSET_CODE, productionConvert);
                    }
                }

                foreach (var item in dicValue)
                {
                    lsBarPoint.Add(new SeriesPoint() { Argument = item.Key, Value = double.Parse(item.Value.PART_NUM.ToString()) });
                    lsBarPoint2.Add(new SeriesPoint() { Argument = item.Key, Value = double.Parse(item.Value.TOTAL_PART_NUM.ToString()) });
                    lsPiePoint.Add(new SeriesPoint() { Argument = item.Key, Value = double.Parse(item.Value.PART_NUM.ToString()) });

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void GetData(DateTime startTime, DateTime endTime, string deviceNumber)
        {
            // 获取产量统计的数据
            List<DAProductRecord> daProductRecord = wsDA.UseService(s => s.GetDAProductRecords(" START_TIME > '" + startTime.ToString("yyyy-MM-dd HH:mm:ss") + "' AND END_TIME < '" + endTime.ToString("yyyy-MM-dd HH:mm:ss") + "' AND ASSET_CODE = '" + deviceNumber + "'")).OrderBy(c => c.START_TIME).ToList();

            try
            {
                lsBarPoint.Clear();
                lsBarPoint2.Clear();
                lsPiePoint.Clear();

                if (daProductRecord.Count == 0)
                {
                    MessageBox.Show("查询的数据不存在.");
                    return;
                }
                productControl.ItemsSource = daProductRecord;

                foreach (var item in daProductRecord)
                {
                    lsBarPoint.Add(new SeriesPoint() { Argument = item.START_TIME.ToString(), Value = double.Parse(item.PART_NUM.ToString()) });
                    lsBarPoint2.Add(new SeriesPoint() { Argument = item.START_TIME.ToString(), Value = double.Parse(item.TOTAL_PART_NUM.ToString()) });
                    lsPiePoint.Add(new SeriesPoint() { Argument = item.START_TIME.ToString(), Value = double.Parse(item.PART_NUM.ToString()) });
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region 按钮动作
        private void bSearch4_click(object sender, RoutedEventArgs e)
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
            string deviceNumber = DeviceNumber.Text;
            if (deviceNumber == null || "".Equals(deviceNumber))
            {
                BindData(startTime, endTime);
            }
            else
            {
                BindData(startTime, endTime, deviceNumber);
            }

        }
        #endregion
    }

    class MyClass1
    {
        public string PKNO { get; set; }
        public string name { get; set; }
    }
}
