using BFM.Common.Data.PubData;
using BFM.WPF.SDM.DM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Server.DataAsset.EAMService;
using BFM.ContractModel;
using BFM.WPF.Base;
using BFM.WPF.Base.Helper;

namespace BFM.WPF.EAM.AM
{
    /// <summary>
    /// AmAssetMasterNView.xaml 的交互逻辑
    /// </summary>
    public partial class AmAssetMasterNView : Page
    {
        private WcfClient<IEAMService> ws;

        public AmAssetMasterNView()
        {
            InitializeComponent();
            ws = new WcfClient<IEAMService>();
            GetPage();
        }

        private void GetPage()
        {
            List<AmAssetMasterN> source = ws.UseService(s => s.GetAmAssetMasterNs(""));
            //GetImage(source);
            gridItem.ItemsSource = source.OrderBy(s => s.CREATION_DATE);
        }

        #region 按钮动作



        private void Win_Closed(object sender, EventArgs e)
        {
            GetPage();
        }

        #endregion


        #region 按钮动作

        #region 编辑

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //新增

            #region

            //TODO: 校验

            #endregion

            AmAssetMasterN AssetMasterN = new AmAssetMasterN()
            {
                COMPANY_CODE = "",
                CREATION_DATE = DateTime.Now,
                LAST_UPDATE_DATE = DateTime.Now, //最后修改日期
                USE_FLAG = 1, //启用
            };
            gbItem.DataContext = AssetMasterN;

            //dictBasic.Header = $"{HeaderName}  【新增】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
            tbAssetCode.IsReadOnly = false;
        }

        private void BtnMod_Click(object sender, RoutedEventArgs e)
        {
            if (gridItem.SelectedItem == null)
            {
                return;
            }

            ModItem();
        }

        /// <summary>
        /// 修改主体
        /// </summary>
        private void ModItem()
        {
            gbItem.DataContext = gridItem.SelectedItem;
            //dictBasic.Header = $"{HeaderName} 【修改】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
            tbAssetCode.IsReadOnly = true;
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            //删除
            AmAssetMasterN AssetMasterN = gridItem.SelectedItem as AmAssetMasterN;
            if (AssetMasterN == null)
            {
                return;
            }

            if (WPFMessageBox.ShowConfirm($"确定删除设备【{AssetMasterN.ASSET_NAME}】吗？", "删除信息") == WPFMessageBoxResult.OK)
            {
                ws.UseService(s => s.DelAmAssetMasterN(AssetMasterN.ASSET_CODE));

                //删除成功.
                GetPage();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //保存
            AmAssetMasterN AssetMasterN = gbItem.DataContext as AmAssetMasterN;
            if (AssetMasterN == null)
            {
                return;
            }

            #region  TODO: 校验

            //TODO: 校验

            #endregion

            if (string.IsNullOrEmpty(AssetMasterN.PKNO)) //新增
            {
                AssetMasterN.PKNO = Guid.NewGuid().ToString("N");
                AssetMasterN.CREATED_BY = CBaseData.LoginName;
                AssetMasterN.CREATION_DATE = DateTime.Now;
                AssetMasterN.LAST_UPDATE_DATE = DateTime.Now; //最后修改日期

                ws.UseService(s => s.AddAmAssetMasterN(AssetMasterN));
            }
            else //修改
            {
                AssetMasterN.UPDATED_BY = CBaseData.LoginName;
                AssetMasterN.LAST_UPDATE_DATE = DateTime.Now;
                ws.UseService(s => s.UpdateAmAssetMasterN(AssetMasterN));
            }

            GetPage(); //重新刷新数据，根据需求是否进行刷新数据

            //保存成功
            gbItem.IsCollapsed = true;
            gbItem.Visibility = Visibility.Collapsed;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //取消
            gbItem.IsCollapsed = true;
            gbItem.Visibility = Visibility.Collapsed;
            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }

        /// <summary>
        /// 双击表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (gridItem.SelectedItem == null)
            {
                return;
            }

            //修改
            ModItem();
        }

        #endregion



        #endregion

        private void Btn_File(object sender, RoutedEventArgs e)
        {
            if (this.gridItem.SelectedItem == null) return;
            AmAssetMasterN selectItem = this.gridItem.SelectedItem as AmAssetMasterN;
            DocumentManageInvoke.NewDocumentManage("设备管理", selectItem.PKNO, "", 0, DocumentMangMode.CanUpLoad);
        }

        private void Btn_Export(object sender, RoutedEventArgs e)
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
                view.ExportToXls(newFileName);
            }
        }
    }
}
