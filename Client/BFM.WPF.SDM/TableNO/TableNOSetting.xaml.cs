using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.Data.PubData;
using BFM.ContractModel;
using BFM.Server.DataAsset.SDMService;
using BFM.WPF.Base.Helper;

namespace BFM.WPF.SDM.TableNO
{
    /// <summary>
    /// TableNOSetting.xaml 的交互逻辑
    /// </summary>
    public partial class TableNOSetting : Page
    {
        private WcfClient<ISDMService> ws = new WcfClient<ISDMService>(); 
        public TableNOSetting()
        {
            InitializeComponent();

            GetPage();
        }

        private void GetPage()
        {
            List<SysTableNOSetting> tableNos = ws.UseService(s => s.GetSysTableNOSettings("USE_FLAG > 0"));
            gridItem.ItemsSource = tableNos;

            BFM.WPF.Base.Helper.BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }

        #region 按钮动作

        #region 编辑

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //新增
            dictInfo.Header = "系统表格编号设置详细  【新增】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;

            SysTableNOSetting tableNo = new SysTableNOSetting()
            {
                COMPANY_CODE = "",
                IDENTIFY_CODE = "",
                DATE_FORMATE = "yyMMdd",
                USE_FLAG = 1,
            };
            gbItem.DataContext = tableNo;
        }

        private void BtnMod_Click(object sender, RoutedEventArgs e)
        {
            SysTableNOSetting m_SysTableNOSetting = gridItem.SelectedItem as SysTableNOSetting;
            if (m_SysTableNOSetting == null)
            {
                return;
            }
            //修改
            dictInfo.Header = "系统表格编号设置详细  【修改】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            //删除
            SysTableNOSetting m_SysTableNOSetting = gridItem.SelectedItem as SysTableNOSetting;
            if (m_SysTableNOSetting == null)
            {
                return;
            }
            if (System.Windows.Forms.MessageBox.Show($"确定删除基础信息【{m_SysTableNOSetting.NO_INTROD}】吗？",
                    @"删除信息",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                m_SysTableNOSetting.USE_FLAG = -1;
                ws.UseService(s => s.UpdateSysTableNOSetting(m_SysTableNOSetting));

                //删除成功.
                GetPage();  //重新加载
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //保存
            SysTableNOSetting tableNo = gbItem.DataContext as SysTableNOSetting;
            if (tableNo == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(tableNo.PKNO)) //新增
            {
                tableNo.PKNO = Guid.NewGuid().ToString("N");
                tableNo.CREATION_DATE = DateTime.Now;
                tableNo.CREATED_BY = CBaseData.LoginName;
                tableNo.LAST_UPDATE_DATE = DateTime.Now;

                ws.UseService(s => s.AddSysTableNOSetting(tableNo));
            }
            else  //修改
            {
                tableNo.LAST_UPDATE_DATE = DateTime.Now;
                tableNo.UPDATED_BY = CBaseData.LoginName;
                ws.UseService(s => s.UpdateSysTableNOSetting(tableNo));
            }

            GetPage();  //重新加载

            gbItem.IsCollapsed = true;
            gbItem.Visibility = Visibility.Collapsed;
            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //取消
            gbItem.IsCollapsed = true;
            gbItem.Visibility = Visibility.Collapsed;
            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }

        #endregion

        #region 查询

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            //查询
        }

        private void BtnMoreSearch_Click(object sender, RoutedEventArgs e)
        {
            //高级查询
        }

        #endregion

        #region 导入导出
        private void BtnInPort_Click(object sender, RoutedEventArgs e)
        {
            //导入
        }

        private void BtnOutPort_Click(object sender, RoutedEventArgs e)
        {
            //导出
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            //报表
        }

        #endregion

        #endregion

        private void gridItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SysTableNOSetting m_SysTableNOSetting = gridItem.SelectedItem as SysTableNOSetting;
            if (m_SysTableNOSetting == null)
            {
                return;
            }
            //修改
            dictInfo.Header = "系统表格编号设置详细  【修改】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
        }
    }
}
