using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.FMSService;
using BFM.WPF.Base;
using BFM.WPF.Base.Helper;
using DevExpress.Xpf.Grid;

namespace BFM.WPF.FMS.ProcessControl
{
    /// <summary>
    /// ActionFomulaDetail.xaml 的交互逻辑
    /// </summary>
    public partial class ActionFomulaDetail : Page
    {
        private WcfClient<IFMSService> ws = new WcfClient<IFMSService>();
        public ActionFomulaDetail()
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
            List<FmsActionFormulaMain> m_MainInfo = ws.UseService(s => s.GetFmsActionFormulaMains(sWhere))
                .OrderBy(c => c.FORMULA_NAME).ToList();

            tvMain.View.Nodes.Clear();
            TreeListNode viewItem = new TreeListNode
            {
                Content = new FmsActionFormulaMain() {PKNO = "", FORMULA_NAME = "配方主信息"},
                Tag = ""
            };

            foreach (var me in m_MainInfo)
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

            var actions = ws.UseService(s => s.GetFmsActionControls($"")).OrderBy(c => c.ASSET_CODE).ToList();
            cmbActionControl.ItemsSource = actions;
        }

        private void tvMain_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            FmsActionFormulaMain m_MainInfo = tvMain.SelectedItem as FmsActionFormulaMain;
            if ((m_MainInfo == null) || (string.IsNullOrEmpty(m_MainInfo.PKNO)))
            {
                gridItem.ItemsSource = null;
                return;
            }

            List<FmsActionFormulaDetail> mFormulaDetails =
                ws.UseService(s => s.GetFmsActionFormulaDetails($"FORMULA_CODE = {m_MainInfo.FORMULA_CODE} AND USE_FLAG >= 0"))
                .OrderBy(c => c.PROCESS_INDEX)
                .ToList();
            gridItem.ItemsSource = mFormulaDetails;

            gbItem.IsCollapsed = true;
            gbItem.Visibility = Visibility.Collapsed;
        }

        private void CmbActionControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gbItem.Visibility != Visibility.Visible)
            {
                return;
            }
            FmsActionFormulaDetail detail = gbItem.DataContext as FmsActionFormulaDetail;
            if ((detail == null) || (!string.IsNullOrEmpty(detail.PROCESS_DEVICE_PKNO)))
            {
                return;
            }

            FmsActionControl action = cmbActionControl.SelectedItem as FmsActionControl;
            detail.PROCESS_DEVICE_PKNO = action?.ASSET_CODE;
        }

        #region 功能菜单

        #region 主信息

        private void BtnMainAdd_Click(object sender, RoutedEventArgs e)
        {
            FmsActionFormulaMain main = new FmsActionFormulaMain()
            {
                PKNO = "",
                //COMPANY_CODE = "",
                USE_FLAG = 1,
            };
            ActionFomulaMain mainEdit = new ActionFomulaMain(main);
            mainEdit.ShowDialog();

            GetMainData();
        }

        private void BtnMainMod_Click(object sender, RoutedEventArgs e)
        {
            FmsActionFormulaMain main = tvMain.SelectedItem as FmsActionFormulaMain;
            if ((main == null) || (string.IsNullOrEmpty(main.PKNO)))
            {
                return;
            }
            ActionFomulaMain mainEdit = new ActionFomulaMain(main);
            mainEdit.ShowDialog();

            GetMainData();
        }

        //删除主信息，已禁用
        private void BtnMainDel_Click(object sender, RoutedEventArgs e)
        {
            FmsActionFormulaMain main = tvMain.SelectedItem as FmsActionFormulaMain;
            if ((main == null) || (string.IsNullOrEmpty(main.PKNO)))
            {
                return;
            }

            main.LAST_UPDATE_DATE = DateTime.Now;
            main.UPDATED_BY = CBaseData.LoginNO;
            main.UPDATED_INTROD = "删除";
            main.USE_FLAG = -1;  //已删除

            ws.UseService(s => s.UpdateFmsActionFormulaMain(main));

            List<FmsActionFormulaDetail> details = gridItem.ItemsSource as List<FmsActionFormulaDetail>;
            if (details != null)
            {
                foreach (var detail in details)
                {
                    detail.USE_FLAG = -1; //已删除
                    ws.UseService(s => s.UpdateFmsActionFormulaDetail(detail));
                }
            }

            GetMainData();
        }

        #endregion

        private void BtnItemAdd_Click(object sender, RoutedEventArgs e)
        {
            FmsActionFormulaMain main = tvMain.SelectedItem as FmsActionFormulaMain;
            if ((main == null) || (string.IsNullOrEmpty(main.PKNO)))
            {
                return;
            }
            
            //添加明细
            dictBasic.Header = "动作配方明细信息  【新增】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;

            List<FmsActionFormulaDetail> details = gridItem.ItemsSource as List<FmsActionFormulaDetail>;

            FmsActionFormulaDetail detail = new FmsActionFormulaDetail()
            {
                FORMULA_CODE = main.FORMULA_CODE,
                PROCESS_ACTION_TYPE = 0,
                PROCESS_INDEX = (details?.Count + 1) ?? 1,
                USE_FLAG = 1,
            };
            gbItem.DataContext = detail;
        }

        private void BtnItemMod_Click(object sender, RoutedEventArgs e)
        {
            FmsActionFormulaMain main = tvMain.SelectedItem as FmsActionFormulaMain;
            if ((main == null) || (string.IsNullOrEmpty(main.PKNO)))
            {
                return;
            }

            if (gridItem.SelectedItem == null)
            {
                return;
            }
            //修改明细
            dictBasic.Header = "动作配方明细信息  【修改】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
        }

        private void BtnItemDel_Click(object sender, RoutedEventArgs e)
        {
            FmsActionFormulaMain main = tvMain.SelectedItem as FmsActionFormulaMain;
            if ((main == null) || (string.IsNullOrEmpty(main.PKNO)))
            {
                return;
            }
            //删除明细
            FmsActionFormulaDetail detail = gridItem.SelectedItem as FmsActionFormulaDetail;
            if (detail == null)
            {
                return;
            }

            if (WPFMessageBox.ShowConfirm($"确定删除动作配方明细信息【{detail.FORMULA_DETAIL_NAME}】吗？", @"删除信息")== WPFMessageBoxResult.OK)
            {
                //detail.USE_FLAG = -1;
                //ws.UseService(s => s.UpdateFmsActionFormulaDetail(detail));

                ws.UseService(s => s.DelFmsActionFormulaDetail(detail.PKNO));

                //删除成功.
                List<FmsActionFormulaDetail> details =
                    ws.UseService(s => s.GetFmsActionFormulaDetails($"FORMULA_CODE = {main.FORMULA_CODE} AND USE_FLAG >= 0"))
                        .OrderBy(c => c.PROCESS_INDEX)
                        .ToList();
                gridItem.ItemsSource = details;

                BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
            }
        }

        private void BtnItemUse_Click(object sender, RoutedEventArgs e)
        {
            FmsActionFormulaMain main = tvMain.SelectedItem as FmsActionFormulaMain;
            if ((main == null) || (string.IsNullOrEmpty(main.PKNO)))
            {
                return;
            }
            //可用明细
            FmsActionFormulaDetail detail = gridItem.SelectedItem as FmsActionFormulaDetail;
            if (detail == null)
            {
                return;
            }

            if (WPFMessageBox.ShowConfirm($"确定[{BtnItemUse.Content}]动作配方明细信息【{detail.FORMULA_DETAIL_NAME}】吗？", @"删除信息") == WPFMessageBoxResult.OK)
            {
                detail.USE_FLAG = (detail.USE_FLAG == 1) ? 0 : 1;
                ws.UseService(s => s.UpdateFmsActionFormulaDetail(detail));

                //可用成功.
                List<FmsActionFormulaDetail> details =
                    ws.UseService(s => s.GetFmsActionFormulaDetails($"FORMULA_CODE = {main.FORMULA_CODE} AND USE_FLAG >= 0"))
                        .OrderBy(c => c.PROCESS_INDEX)
                        .ToList();
                gridItem.ItemsSource = details;

                BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
            }
        }

        private void BtnItemSave_Click(object sender, RoutedEventArgs e)
        {
            FmsActionFormulaMain main = tvMain.SelectedItem as FmsActionFormulaMain;
            if ((main == null) || (string.IsNullOrEmpty(main.PKNO)))
            {
                return;
            }
            FmsActionFormulaDetail detail = gbItem.DataContext as FmsActionFormulaDetail;
            if (detail == null)
            {
                return;
            }

            #region  校验

            if (string.IsNullOrEmpty(detail.FORMULA_CODE))
            {
                WPFMessageBox.ShowWarring("请选择配方主信息。", "保存");
                return;
            }

            if (string.IsNullOrEmpty(detail.FORMULA_DETAIL_NAME))
            {
                WPFMessageBox.ShowWarring("请输入配方明细名称。", "保存");
                return;
            }

            #endregion

            if (string.IsNullOrEmpty(detail.PKNO)) //新增
            {
                detail.PKNO = CBaseData.NewGuid();

                ws.UseService(s => s.AddFmsActionFormulaDetail(detail));

                //重新刷新数据
                List<FmsActionFormulaDetail> details =
                    ws.UseService(s => s.GetFmsActionFormulaDetails($"FORMULA_CODE = {main.FORMULA_CODE} AND USE_FLAG >= 0"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();
                gridItem.ItemsSource = details;
            }
            else  //修改
            {
                ws.UseService(s => s.UpdateFmsActionFormulaDetail(detail));
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
            FmsActionFormulaMain main = tvMain.SelectedItem as FmsActionFormulaMain;
            if ((main == null) || (string.IsNullOrEmpty(main.PKNO)))
            {
                return;
            }

            if (gridItem.SelectedItem == null)
            {
                return;
            }

            //修改明细
            dictBasic.Header = "动作配方明细信息  【修改】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
        }

        private void GridItem_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            FmsActionFormulaMain main = tvMain.SelectedItem as FmsActionFormulaMain;
            if ((main == null) || (string.IsNullOrEmpty(main.PKNO)))
            {
                return;
            }

            FmsActionFormulaDetail detail = gridItem.SelectedItem as FmsActionFormulaDetail;
            if (detail == null)
            {
                return;
            }

            BtnItemUse.Content = (detail.USE_FLAG == 1) ? "禁用" : "启用";
        }

        private void BtnItemUp_Click(object sender, RoutedEventArgs e)
        {
            FmsActionFormulaDetail detail = gridItem.SelectedItem as FmsActionFormulaDetail;
            if ((detail == null) || (detail.PROCESS_INDEX <= 1))
            {
                return;
            }

            List<FmsActionFormulaDetail> details = gridItem.ItemsSource as List<FmsActionFormulaDetail>;
            FmsActionFormulaDetail down = details?.FirstOrDefault(c => c.PROCESS_INDEX == detail.PROCESS_INDEX - 1);
            if (down == null)
            {
                return;
            }

            down.PROCESS_INDEX = detail.PROCESS_INDEX;  //上一个 + 1
            detail.PROCESS_INDEX -= 1;  //当前 - 1

            ws.UseService(s => s.UpdateFmsActionFormulaDetail(down));
            ws.UseService(s => s.UpdateFmsActionFormulaDetail(detail));

            tvMain_SelectedItemChanged(null, null);
            gridItem.SelectedItem = detail;
        }

        private void BtnItemDown_Click(object sender, RoutedEventArgs e)
        {
            FmsActionFormulaDetail detail = gridItem.SelectedItem as FmsActionFormulaDetail;
            if (detail == null)
            {
                return;
            }

            List<FmsActionFormulaDetail> details = gridItem.ItemsSource as List<FmsActionFormulaDetail>;
            FmsActionFormulaDetail up = details?.FirstOrDefault(c => c.PROCESS_INDEX == detail.PROCESS_INDEX + 1);
            if (up == null)
            {
                return;
            }

            up.PROCESS_INDEX = detail.PROCESS_INDEX;  //上一个 - 1
            detail.PROCESS_INDEX += 1;  //当前 - 1

            ws.UseService(s => s.UpdateFmsActionFormulaDetail(up));
            ws.UseService(s => s.UpdateFmsActionFormulaDetail(detail));

            tvMain_SelectedItemChanged(null, null);
            gridItem.SelectedItem = detail;
        }
    }
}
