using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.Data.PubData;
using BFM.Server.DataAsset.WMSService;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Grid;
using BFM.ContractModel;
using BFM.WPF.Base.Helper;
using DevExpress.Data.Native;

namespace BFM.WPF.WMS.BaseData
{
    /// <summary>
    /// AllocationMang.xaml 的交互逻辑
    /// </summary>
    public partial class AllocationMang : Page
    {
        private WcfClient<IWMSService> ws = new WcfClient<IWMSService>();
        public AllocationMang()
        {
            InitializeComponent();

            GetMainData();

            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);

            gbItem.IsCollapsed = true;
            gbItem.Visibility = Visibility.Collapsed;
        }

        private void GetMainData()
        {
            string sWhere = "USE_FLAG = 1"; //
            List<WmsAreaInfo> mWmsAreaInfo = ws.UseService(s => s.GetWmsAreaInfos(sWhere));

            cmbAreaInfo.ItemsSource = mWmsAreaInfo;

            tvMain.View.Nodes.Clear();
            TreeListNode viewItem = new TreeListNode
            {
                Content = new WmsAreaInfo() {PKNO = "", AREA_NAME = "库区信息"},
                Tag = ""
            };

            foreach (WmsAreaInfo me in mWmsAreaInfo)
            {
                TreeListNode item = new TreeListNode
                {
                    Content = me,
                    Tag = me,
                };
                viewItem.Nodes.Add(item);
            }
            viewItem.IsExpanded = true;
            tvMain.View.Nodes.Add(viewItem);
        }

        private void tvMain_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            WmsAreaInfo m_WmsAreaInfo = tvMain.SelectedItem as WmsAreaInfo;
            if ((m_WmsAreaInfo == null) || (string.IsNullOrEmpty(m_WmsAreaInfo.PKNO)))
            {
                gridItem.ItemsSource = null;
                return;
            }

            List<WmsAllocationInfo> mWmsAllocationInfos =
                ws.UseService(s => s.GetWmsAllocationInfos($"AREA_PKNO = {m_WmsAreaInfo.PKNO} AND USE_FLAG >= 0"))
                .OrderBy(c => c.CREATION_DATE)
                .ToList();
            gridItem.ItemsSource = mWmsAllocationInfos;

            gbItem.IsCollapsed = true;
            gbItem.Visibility = Visibility.Collapsed;
        }

        #region 功能菜单

        #region 主信息

        private void BtnMainAdd_Click(object sender, RoutedEventArgs e)
        {
            WmsAreaInfo main = new WmsAreaInfo()
            {
                PKNO = "",
                COMPANY_CODE = "",
                USE_FLAG = 1,
            };
            AreaMang mainEdit = new AreaMang(main);
            mainEdit.ShowDialog();

            GetMainData();
        }

        private void BtnMainMod_Click(object sender, RoutedEventArgs e)
        {
            WmsAreaInfo m_WmsAreaInfo = tvMain.SelectedItem as WmsAreaInfo;
            if ((m_WmsAreaInfo == null) || (string.IsNullOrEmpty(m_WmsAreaInfo.PKNO)))
            {
                return;
            }
            AreaMang mainEdit = new AreaMang(m_WmsAreaInfo);
            mainEdit.ShowDialog();

            GetMainData();
        }

        //删除主信息，已禁用
        private void BtnMainDel_Click(object sender, RoutedEventArgs e)
        {
            WmsAreaInfo m_WmsAreaInfo = tvMain.SelectedItem as WmsAreaInfo;
            if ((m_WmsAreaInfo == null) || (string.IsNullOrEmpty(m_WmsAreaInfo.PKNO)))
            {
                return;
            }

            ws.UseService(s => s.DelWmsAreaInfo(m_WmsAreaInfo.PKNO));

            GetMainData();
        }

        #endregion

        private void BtnItemAdd_Click(object sender, RoutedEventArgs e)
        {
            WmsAreaInfo m_WmsAreaInfo = tvMain.SelectedItem as WmsAreaInfo;
            if ((m_WmsAreaInfo == null) || (string.IsNullOrEmpty(m_WmsAreaInfo.PKNO)))
            {
                return;
            }
            
            //添加明细
            dictBasic.Header = "货位信息维护  【新增】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;

            WmsAllocationInfo m_AllocationInfos = new WmsAllocationInfo()
            {
                COMPANY_CODE = "",
                AREA_PKNO = m_WmsAreaInfo.PKNO,
                ALLOCATION_STATE = 0,
                USE_FLAG = 1,
            };
            gbItem.DataContext = m_AllocationInfos;
        }

        private void BtnItemMod_Click(object sender, RoutedEventArgs e)
        {
            WmsAreaInfo m_WmsAreaInfo = tvMain.SelectedItem as WmsAreaInfo;
            if ((m_WmsAreaInfo == null) || (string.IsNullOrEmpty(m_WmsAreaInfo.PKNO)))
            {
                return;
            }

            if (gridItem.SelectedItem == null)
            {
                return;
            }
            //修改明细
            dictBasic.Header = "货位信息维护  【修改】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
        }

        private void BtnItemDel_Click(object sender, RoutedEventArgs e)
        {
            WmsAreaInfo m_WmsAreaInfo = tvMain.SelectedItem as WmsAreaInfo;
            if ((m_WmsAreaInfo == null) || (string.IsNullOrEmpty(m_WmsAreaInfo.PKNO)))
            {
                return;
            }
            //删除明细
            WmsAllocationInfo m_AllocationInfo = gridItem.SelectedItem as WmsAllocationInfo;
            if (m_AllocationInfo == null)
            {
                return;
            }

            if (System.Windows.Forms.MessageBox.Show($"确定删除货位信息【{m_AllocationInfo.ALLOCATION_NAME}】吗？",
                @"删除信息",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                m_AllocationInfo.USE_FLAG = -1;
                ws.UseService(s => s.UpdateWmsAllocationInfo(m_AllocationInfo));

                //删除成功.
                List<WmsAllocationInfo> mWmsAllocationInfos =
                    ws.UseService(s => s.GetWmsAllocationInfos($"AREA_PKNO = {m_WmsAreaInfo.PKNO} AND USE_FLAG >= 0"))
                        .OrderBy(c => c.CREATION_DATE)
                        .ToList();
                gridItem.ItemsSource = mWmsAllocationInfos;

                BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
            }
        }

        private void BtnItemSave_Click(object sender, RoutedEventArgs e)
        {
            WmsAreaInfo m_WmsAreaInfo = tvMain.SelectedItem as WmsAreaInfo;
            if ((m_WmsAreaInfo == null) || (string.IsNullOrEmpty(m_WmsAreaInfo.PKNO)))
            {
                return;
            }
            WmsAllocationInfo m_AllocationInfo = gbItem.DataContext as WmsAllocationInfo;
            if (m_AllocationInfo == null)
            {
                return;
            }

            #region  校验

            if (string.IsNullOrEmpty(m_AllocationInfo.ALLOCATION_NAME))
            {
                System.Windows.Forms.MessageBox.Show("请输入货位名称。", "保存", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            #endregion

            if (string.IsNullOrEmpty(m_AllocationInfo.PKNO)) //新增
            {
                m_AllocationInfo.PKNO = Guid.NewGuid().ToString("N");
                m_AllocationInfo.CREATED_BY = CBaseData.LoginName;
                m_AllocationInfo.CREATION_DATE = DateTime.Now;
                m_AllocationInfo.LAST_UPDATE_DATE = DateTime.Now;

                ws.UseService(s => s.AddWmsAllocationInfo(m_AllocationInfo));

                //重新刷新数据
                List<WmsAllocationInfo> mAllocationInfos =
                    ws.UseService(s => s.GetWmsAllocationInfos($"AREA_PKNO = {m_WmsAreaInfo.PKNO} AND USE_FLAG >= 0"))
                    .OrderBy(c => c.CREATION_DATE)
                    .ToList();
                gridItem.ItemsSource = mAllocationInfos;
            }
            else  //修改
            {
                m_AllocationInfo.UPDATED_BY = CBaseData.LoginName;
                m_AllocationInfo.LAST_UPDATE_DATE = DateTime.Now;
                ws.UseService(s => s.UpdateWmsAllocationInfo(m_AllocationInfo));
            }
            //提示保存成功

            gbItem.IsCollapsed = true;
            gbItem.Visibility = Visibility.Collapsed;
            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }

        private void BtnItemCancel_Click(object sender, RoutedEventArgs e)
        {
            gbItem.IsCollapsed = true;
            gbItem.Visibility = Visibility.Collapsed;
            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }

        #endregion

        //双击修改
        private void gridItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            WmsAreaInfo m_WmsAreaInfo = tvMain.SelectedItem as WmsAreaInfo;
            if ((m_WmsAreaInfo == null) || (string.IsNullOrEmpty(m_WmsAreaInfo.PKNO)))
            {
                return;
            }

            if (gridItem.SelectedItem == null)
            {
                return;
            }
            //修改明细
            dictBasic.Header = "货位信息维护  【修改】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
        }
    }
}
