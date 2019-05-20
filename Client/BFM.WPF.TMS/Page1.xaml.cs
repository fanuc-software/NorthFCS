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

namespace BFM.WPF.TMS
{
    /// <summary>
    /// Page1.xaml 的交互逻辑
    /// </summary>
    public partial class Page1 : Page
    {
        List<ToolLifeer> ToolLifeers;
        public Page1()
        {
            InitializeComponent();
            initClass();
            gridView.ItemsSource = ToolLifeers;
        }

        public void initClass()
        {
            ToolLifeers = new List<ToolLifeer>();
            for (int i = 0; i < 6; i++)
            {
                Random rd = new Random(i);
                ToolLifeer tool = new ToolLifeer();
                tool.ToolName = Enum.GetNames(typeof(ToolName))[i];
                tool.Picture = "";
                tool.SetUpLife = 100;
                tool.UseLife = rd.Next(0, 100);
                tool.UseRate = Math.Round((double)((double)(tool.UseLife) / tool.SetUpLife), 2);
                tool.PlanCount = rd.Next(180, 200);
                tool.Count = rd.Next(0, 180);
                this.char1.AddPoint(tool.ToolName, tool.PlanCount);
                this.char2.AddPoint(tool.ToolName, tool.Count);
                if (tool.UseLife < 100 && tool.UseLife > 50)
                {
                    tool.LifeStatu = "正常";

                    tool.StockStatu = "预警";
                }
                else if (tool.UseLife < 50 && tool.UseLife > 10)
                {
                    tool.LifeStatu = "预警";
                    tool.StockStatu = "充足";
                }
                else
                {
                    tool.LifeStatu = "报废";
                    tool.StockStatu = "充足";
                }
                ToolLifeers.Add(tool);
            }


        }

        private void ImageButtonWithIcon_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            //可能要获取的路径名
            string localFilePath = "", fileNameExt = "", newFileName = "", FilePath = "";

            //设置文件类型
            //书写规则例如：txt files(*.txt)|*.txt
            saveFileDialog.Filter = " xls files(*.xls)|*.xls";
            //设置默认文件名（可以不设置）
            saveFileDialog.FileName = "RootElements";
            //主设置默认文件extension（可以不设置）
            //saveFileDialog.DefaultExt = "txt";
            //获取或设置一个值，该值指示如果用户省略扩展名，文件对话框是否自动在文件名中添加扩展名。（可以不设置）
            //saveFileDialog.AddExtension = true;

            //设置默认文件类型显示顺序（可以不设置）
            //saveFileDialog.FilterIndex = 2;

            //保存对话框是否记忆上次打开的目录
            saveFileDialog.RestoreDirectory = true;

            // Show save file dialog box
            bool? result = saveFileDialog.ShowDialog();
            //点了保存按钮进入
            if (result == true)
            {
                //获得文件路径
                localFilePath = saveFileDialog.FileName.ToString();

                //获取文件名，不带路径
                fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1);

                //获取文件路径，不带文件名
                FilePath = localFilePath.Substring(0, localFilePath.LastIndexOf("\\"));

                //给文件名前加上时间
                newFileName = fileNameExt;
                newFileName = FilePath + "\\" + newFileName;

                //在文件名里加字符
                //saveFileDialog.FileName.Insert(1,"dameng");
                //为用户使用 SaveFileDialog 选定的文件名创建读/写文件流。
                //System.IO.File.WriteAllText(newFileName, wholestring); //这里的文件名其实是含有路径的。
                TableViewgridView.ExportToXls(newFileName);
            }
        }
    }

}
public class ToolLifeer
{
    //设备名称
    public string ToolName { get; set; }
    //图片
    public string Picture { get; set; }
    //设定寿命

    public int SetUpLife { get; set; }

    //使用寿命

    public int UseLife { get; set; }
    //使用率

    public double UseRate { get; set; }

    //寿命状态

    public string LifeStatu { get; set; }

    //库存状态

    public string StockStatu { get; set; }
    //库存状态

    public int PlanCount { get; set; }

    //库存状态

    public int Count { get; set; }



}
public enum ToolName
{
    铣刀01 = 0,
    铣刀02,
    铣刀03,
    车刀01,
    车刀02,
    车刀03,
}