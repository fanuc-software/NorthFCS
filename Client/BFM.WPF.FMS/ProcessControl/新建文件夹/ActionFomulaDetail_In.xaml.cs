using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using BFM.Common.Base;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.FMSService;
using BFM.WPF.Base;
using BFM.WPF.Base.Helper;
using DevExpress.Xpf.Grid;
using Cursors = System.Windows.Input.Cursors;

namespace BFM.WPF.FMS.ProcessControl
{
    /// <summary>
    /// ActionFomulaDetail_In.xaml 的交互逻辑
    /// </summary>
    public partial class ActionFomulaDetail_In : Page
    {
        private WcfClient<IFMSService> ws = new WcfClient<IFMSService>();

        public ActionFomulaDetail_In()
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

            //cmbMainInfo.ItemsSource = m_MainInfo;

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

            var tag = ws.UseService(s => s.GetFmsAssetTagSettings($"USE_FLAG = 1")).OrderBy(c => c.ASSET_CODE)
                .ThenBy(c => c.TAG_NAME).ToList();  //Tag点

            cmbCondition.ItemsSource = tag.Where(c => c.SAMPLING_MODE != 1).OrderBy(c => c.ASSET_CODE)
                .ThenBy(c => c.TAG_NAME).ToList();  //只写的不能作为条件和结果
            cmbFinish.ItemsSource = tag.Where(c => c.SAMPLING_MODE != 1).OrderBy(c => c.ASSET_CODE)
                .ThenBy(c => c.TAG_NAME).ToList();  //只写的不能作为条件和结果

            cmbExecute.ItemsSource = tag.Where(c => !string.IsNullOrEmpty(c.TAG_ADDRESS)).OrderBy(c => c.ASSET_CODE)
                .ThenBy(c => c.TAG_NAME).ToList();  //虚拟点暂时不能写
            cmbParam1.ItemsSource = tag.Where(c => !string.IsNullOrEmpty(c.TAG_ADDRESS)).OrderBy(c => c.ASSET_CODE)
                .ThenBy(c => c.TAG_NAME).ToList();  //虚拟点暂时不能写
            cmbParam2.ItemsSource = tag.Where(c => !string.IsNullOrEmpty(c.TAG_ADDRESS)).OrderBy(c => c.ASSET_CODE)
                .ThenBy(c => c.TAG_NAME).ToList();  //虚拟点暂时不能写
        }

        private void tvMain_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            FmsActionFormulaMain m_MainInfo = tvMain.SelectedItem as FmsActionFormulaMain;
            if ((m_MainInfo == null) || (string.IsNullOrEmpty(m_MainInfo.PKNO)))
            {
                gridItem.ItemsSource = null;
                return;
            }

            Cursor = Cursors.Wait;

            List<FmsActionFormulaDetail> mFormulaDetails =
                ws.UseService(s => s.GetFmsActionFormulaDetails($"FORMULA_CODE = {m_MainInfo.FORMULA_CODE} AND USE_FLAG >= 0"))
                .OrderBy(c => c.PROCESS_INDEX)
                .ToList();
            gridItem.ItemsSource = mFormulaDetails;

            Cursor = Cursors.Arrow;

            gbItem.IsCollapsed = true;
            gbItem.Visibility = Visibility.Collapsed;
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

            if (WPFMessageBox.ShowConfirm($"确定要删除该主配方[{main.FORMULA_NAME}]吗？", "删除") != WPFMessageBoxResult.OK)
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

                FmsActionControl action = new FmsActionControl()
                {
                    PKNO = CBaseData.NewGuid(),
                    ACTION_NAME = detail.FORMULA_DETAIL_NAME,
                    ACTION_TYPE = detail.PROCESS_ACTION_TYPE.ToString(),
                    START_CONDITION_TAG_PKNO = string.IsNullOrEmpty(cmbCondition.Text)? "": cmbCondition.SelectedValue.ToString(),
                    START_CONDITION_VALUE = tbConditionValue.Text,
                    EXECUTE_TAG_PKNO = string.IsNullOrEmpty(cmbExecute.Text) ? "" : cmbExecute.SelectedValue.ToString(),
                    EXECUTE_WRITE_VALUE = tbExecuteValue.Text,
                    EXECUTE_PARAM1_TAG_PKNO = string.IsNullOrEmpty(cmbParam1.Text) ? "" : cmbParam1.SelectedValue.ToString(),
                    EXECUTE_PARAM2_TAG_PKNO = string.IsNullOrEmpty(cmbParam2.Text) ? "" : cmbParam2.SelectedValue.ToString(),
                    FINISH_CONDITION_TAG_PKNO = string.IsNullOrEmpty(cmbFinish.Text) ? "" : cmbFinish.SelectedValue.ToString(),
                    FINISH_CONDITION_VALUE = tbFinishValue.Text,
                };

                detail.PROCESS_ACTION_PKNO = action.PKNO;

                ws.UseService(s => s.AddFmsActionControl(action));

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
                FmsActionControl action = ws.UseService(s => s.GetFmsActionControlById(detail.PROCESS_ACTION_PKNO));
                if (action == null)
                {
                    action = new FmsActionControl()
                    {
                        PKNO = CBaseData.NewGuid(),
                        ACTION_NAME = detail.FORMULA_DETAIL_NAME,
                        ACTION_TYPE = detail.PROCESS_ACTION_TYPE.ToString(),
                        START_CONDITION_TAG_PKNO = string.IsNullOrEmpty(cmbCondition.Text) ? "" : cmbCondition.SelectedValue.ToString(),
                        START_CONDITION_VALUE = tbConditionValue.Text,
                        EXECUTE_TAG_PKNO = string.IsNullOrEmpty(cmbExecute.Text) ? "" : cmbExecute.SelectedValue.ToString(),
                        EXECUTE_WRITE_VALUE = tbExecuteValue.Text,
                        EXECUTE_PARAM1_TAG_PKNO = string.IsNullOrEmpty(cmbParam1.Text) ? "" : cmbParam1.SelectedValue.ToString(),
                        EXECUTE_PARAM2_TAG_PKNO = string.IsNullOrEmpty(cmbParam2.Text) ? "" : cmbParam2.SelectedValue.ToString(),
                        FINISH_CONDITION_TAG_PKNO = string.IsNullOrEmpty(cmbFinish.Text) ? "" : cmbFinish.SelectedValue.ToString(),
                        FINISH_CONDITION_VALUE = tbFinishValue.Text,
                    };

                    detail.PROCESS_ACTION_PKNO = action.PKNO;

                    ws.UseService(s => s.AddFmsActionControl(action));
                }
                else //修改控制
                {
                    action.ACTION_NAME = detail.FORMULA_DETAIL_NAME;
                    action.ACTION_TYPE = detail.PROCESS_ACTION_TYPE.ToString();
                    action.START_CONDITION_TAG_PKNO = string.IsNullOrEmpty(cmbCondition.Text)
                        ? ""
                        : cmbCondition.SelectedValue.ToString();
                    action.START_CONDITION_VALUE = tbConditionValue.Text;
                    action.EXECUTE_TAG_PKNO =
                        string.IsNullOrEmpty(cmbExecute.Text) ? "" : cmbExecute.SelectedValue.ToString();
                    action.EXECUTE_WRITE_VALUE = tbExecuteValue.Text;
                    action.EXECUTE_PARAM1_TAG_PKNO =
                        string.IsNullOrEmpty(cmbParam1.Text) ? "" : cmbParam1.SelectedValue.ToString();
                    action.EXECUTE_PARAM2_TAG_PKNO =
                        string.IsNullOrEmpty(cmbParam2.Text) ? "" : cmbParam2.SelectedValue.ToString();
                    action.FINISH_CONDITION_TAG_PKNO =
                        string.IsNullOrEmpty(cmbFinish.Text) ? "" : cmbFinish.SelectedValue.ToString();
                    action.FINISH_CONDITION_VALUE = tbFinishValue.Text;

                    ws.UseService(s => s.UpdateFmsActionControl(action));
                }

                ws.UseService(s => s.UpdateFmsActionFormulaDetail(detail));
            }

            //保存成功
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

            FmsActionControl action = ws.UseService(s => s.GetFmsActionControlById(detail.PROCESS_ACTION_PKNO));

            if (action == null)
            {
                cmbCondition.SelectedValue = null;
                tbConditionValue.Text = "";
                cmbExecute.SelectedValue = null;
                tbExecuteValue.Text = "";
                cmbParam1.SelectedValue = null;
                cmbParam2.SelectedValue = null;
                cmbFinish.SelectedValue = null;
                tbFinishValue.Text = "";
            }
            else
            {
                cmbCondition.SelectedValue = action.START_CONDITION_TAG_PKNO;
                tbConditionValue.Text = action.START_CONDITION_VALUE;
                cmbExecute.SelectedValue = action.EXECUTE_TAG_PKNO;
                tbExecuteValue.Text = action.EXECUTE_WRITE_VALUE;
                cmbParam1.SelectedValue = action.EXECUTE_PARAM1_TAG_PKNO;
                cmbParam2.SelectedValue = action.EXECUTE_PARAM2_TAG_PKNO;
                cmbFinish.SelectedValue = action.FINISH_CONDITION_TAG_PKNO;
                tbFinishValue.Text = action.FINISH_CONDITION_VALUE;
            }
        }

        //向上
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

        //向下
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

        private void BtnItemCopy_Click(object sender, RoutedEventArgs e)
        {
            FmsActionFormulaMain m_MainInfo = tvMain.SelectedItem as FmsActionFormulaMain;
            if ((m_MainInfo == null) || (string.IsNullOrEmpty(m_MainInfo.PKNO)))
            {
                return;
            }
            List<FmsActionFormulaDetail> details = ws.UseService(s => s.GetFmsActionFormulaDetails("FORMULA_CODE = " + m_MainInfo.FORMULA_CODE + ""));

            List<FmsActionFormulaDetail> newFormulaDetails = new List<FmsActionFormulaDetail>();
            List<FmsActionControl> newFmsActionControls = new List<FmsActionControl>();
            FmsActionFormulaMain newFormulaMain = m_MainInfo;
            newFormulaMain.PKNO = Guid.NewGuid().ToString("N");
            newFormulaMain.FORMULA_NAME = "轮毂生产5（只下料，中间件检测）";
            newFormulaMain.FORMULA_CODE = "轮毂生产5 new";
         
            foreach (FmsActionFormulaDetail item in details)
            {
                FmsActionControl action = ws.UseService(s => s.GetFmsActionControlById(item.PROCESS_ACTION_PKNO));
                FmsActionControl newActionControlaction = action;
                #region 处理actionControl
                newActionControlaction.PKNO= Guid.NewGuid().ToString("N");
                newActionControlaction.START_CONDITION_TAG_PKNO= GetTagPkno(newActionControlaction.START_CONDITION_TAG_PKNO);
                newActionControlaction.EXECUTE_PARAM1_TAG_PKNO = GetTagPkno(newActionControlaction.EXECUTE_PARAM1_TAG_PKNO);
                newActionControlaction.EXECUTE_PARAM2_TAG_PKNO = GetTagPkno(newActionControlaction.EXECUTE_PARAM2_TAG_PKNO);
                newActionControlaction.EXECUTE_TAG_PKNO = GetTagPkno(newActionControlaction.EXECUTE_TAG_PKNO);
                newActionControlaction.FINISH_CONDITION_TAG_PKNO = GetTagPkno(newActionControlaction.FINISH_CONDITION_TAG_PKNO);
                #endregion

                FmsActionFormulaDetail newActionFormulaDetail = item;
                newActionFormulaDetail.PKNO= Guid.NewGuid().ToString("N");
                newActionFormulaDetail.FORMULA_CODE = newFormulaMain.FORMULA_CODE;
                newActionFormulaDetail.PROCESS_ACTION_PKNO = newActionControlaction.PKNO;
                newFormulaDetails.Add(newActionFormulaDetail);
                newFmsActionControls.Add(newActionControlaction);
            }
            foreach (var variable2 in newFormulaDetails)
            {
                ws.UseService(s => s.AddFmsActionFormulaDetail(variable2));
            }
            ws.UseService(s => s.AddFmsActionFormulaMain(newFormulaMain));
            foreach (var variable1 in newFmsActionControls)
            {
                ws.UseService(s => s.AddFmsActionControl(variable1));
            }

          
         
        }
        private void BtnItemExport_Click(object sender, RoutedEventArgs e)
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
                TableView.ExportToXls(newFileName);
            }
        }
        public string GetTagPkno(string  tagpkno)
        {
            if (tagpkno=="")
            {
                return tagpkno;
            }
            FmsAssetTagSetting tageTagSetting= ws.UseService(s => s.GetFmsAssetTagSettingById(tagpkno));
            if (tageTagSetting.ASSET_CODE== "SH00001")
            {
                FmsAssetTagSetting tageTagSettinglike = ws.UseService(s => s.GetFmsAssetTagSettings("TAG_ADDRESS = "+ tageTagSetting.TAG_ADDRESS + " AND ASSET_CODE = 'SH00003'")).FirstOrDefault();
                if (tageTagSettinglike!=null)
                {
                    return tageTagSettinglike.PKNO;
                    //return tagpkno;
                }
            }

            return tagpkno;
        }
    }
}
