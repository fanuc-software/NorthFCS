using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.Data.PubData;
using BFM.Server.DataAsset.QMSService;
using BFM.ContractModel;
using BFM.WPF.Base.Helper;

namespace BFM.WPF.PMC
{
    /// <summary>
    /// QmsTestPage.xaml 的交互逻辑
    /// </summary>
    public partial class QmsTestPage : Page
    {
        private WcfClient<IQMSService> ws = new WcfClient<IQMSService>();

        public QmsTestPage()
        {
            InitializeComponent();
            GetPage();
        }

        private void GetPage()
        {
            List<QmsTest> source = ws.UseService(s => s.GetQmsTests("USE_FLAG = 1"));
            //GetImage(source);
            gridItem.ItemsSource = source;
        }

        #region 按钮动作

        #region 编辑

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //新增

            #region

            //TODO: 校验

            #endregion

            QmsTest qmsTest = new QmsTest()
            {
                COMPANY_CODE = "",
                CREATION_DATE = DateTime.Now,
                USE_FLAG = 1, //启用
            };
            gbItem.DataContext = qmsTest;

            //dictBasic.Header = $"{HeaderName}  【新增】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
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
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            //删除
            QmsTest qmsTest = gridItem.SelectedItem as QmsTest;
            if (qmsTest == null)
            {
                return;
            }

            if (System.Windows.Forms.MessageBox.Show($"确定删除该测试质量信息【{qmsTest.TEST_NAME}】吗？",
                @"删除信息",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                ws.UseService(s => s.DelQmsTest(qmsTest.PKNO));

                //删除成功.
                GetPage();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //保存
            QmsTest qmsTest = gbItem.DataContext as QmsTest;
            if (qmsTest == null)
            {
                return;
            }

            #region  TODO: 校验

            //TODO: 校验

            #endregion

            if (string.IsNullOrEmpty(qmsTest.PKNO)) //新增
            {
                qmsTest.PKNO = Guid.NewGuid().ToString("N");
                qmsTest.CREATED_BY = CBaseData.LoginName;
                qmsTest.CREATION_DATE = DateTime.Now;

                ws.UseService(s => s.AddQmsTest(qmsTest));
            }
            else //修改
            {
                qmsTest.UPDATED_BY = CBaseData.LoginName;
                qmsTest.LAST_UPDATE_DATE = DateTime.Now;
                ws.UseService(s => s.UpdateQmsTest(qmsTest));
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
    }
}
