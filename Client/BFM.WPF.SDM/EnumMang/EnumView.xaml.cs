using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.Data.PubData;
using BFM.ContractModel;
using BFM.Server.DataAsset.SDMService;
using BFM.WPF.Base.Helper;
using DevExpress.Data.Native;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Grid;
using Cursors = System.Windows.Input.Cursors;

namespace BFM.WPF.SDM.EnumMang
{
    /// <summary>
    /// EnumView.xaml 的交互逻辑
    /// </summary>
    public partial class EnumView : Page
    {
        private WcfClient<ISDMService> ws = new WcfClient<ISDMService>();
        public EnumView()
        {
            InitializeComponent();

            GetMainData();

            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }

        private void GetMainData()
        {
            string sWhere = "";   //
            if (CBaseData.LoginNO != CBaseData.ADMINPKNO)
            {
                sWhere = "ENUM_STATE < 3";
            }
            List<SysEnumMain> m_SysEnumMain = ws.UseService(s => s.GetSysEnumMains(sWhere));
            List<string> mainText = m_SysEnumMain.Select(c => c.ENUM_SORT).Distinct().ToList();

            tvMain.View.Nodes.Clear();

            foreach (string s in mainText)
            {
                TreeListNode viewItem = new TreeListNode
                {
                    Content = new SysEnumMain() {PKNO = "", ENUM_NAME = s },
                    Tag = ""
                };
                List<SysEnumMain> mainEnum = m_SysEnumMain.Where(c => c.ENUM_SORT == s).ToList();

                foreach (SysEnumMain me in mainEnum)
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
        }

        private void tvMain_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            //TreeListNode item = tvMain.SelectedItem as TreeListNode;
            //if ((item == null) || (item.Tag.ToString() == ""))
            //{
            //    gridItem.ItemsSource = null;
            //    return;
            //}
            //SysEnumMain m_SysEnumMain = item.Tag as SysEnumMain;

            SysEnumMain m_SysEnumMain = tvMain.SelectedItem as SysEnumMain;
            if ((m_SysEnumMain == null) || (string.IsNullOrEmpty(m_SysEnumMain.PKNO)))
            {
                gridItem.ItemsSource = null;
                return;
            }
            if (string.IsNullOrEmpty(m_SysEnumMain.ENUM_CODE_INFO))
            {
                this.lbCodeName.Label = "代号";
                this.gridCodeName.Header = "代号";
                this.lbCodeIntrod.Text = "";
            }
            else
            {
                this.lbCodeName.Label = m_SysEnumMain.ENUM_CODE_INFO;
                this.gridCodeName.Header = m_SysEnumMain.ENUM_CODE_INFO;
                this.lbCodeIntrod.Text = m_SysEnumMain.ENUM_CODE_INTROD;
            }

            List<SysEnumItems> mSysEnumItemses =
                ws.UseService(s => s.GetSysEnumItemss($"ENUM_IDENTIFY = {m_SysEnumMain.ENUM_IDENTIFY} AND USE_FLAG >= 0"))
                .OrderBy(c => c.ITEM_INDEX)
                .ToList();
            gridItem.ItemsSource = mSysEnumItemses;
        }

        #region 功能菜单

        #region 主信息

        private void BtnMainAdd_Click(object sender, RoutedEventArgs e)
        {
            SysEnumMain main = new SysEnumMain()
            {
                PKNO = "",
                COMPANY_CODE = "",
                ENUM_TYPE = 0,
                VALUE_FIELD = 0,
                USE_FLAG = 0,
            };
            EnumMainEdit mainEdit = new EnumMainEdit(main);
            mainEdit.ShowDialog();

            GetMainData();
        }

        private void BtnMainMod_Click(object sender, RoutedEventArgs e)
        {
            //TreeListNode item = tvMain.SelectedItem as TreeListNode;
            //if ((item == null) || (item.Tag.ToString() == ""))
            //{
            //    gridItem.ItemsSource = null;
            //    return;
            //}
            //SysEnumMain m_SysEnumMain = item.Tag as SysEnumMain;

            SysEnumMain m_SysEnumMain = tvMain.SelectedItem as SysEnumMain;
            if ((m_SysEnumMain == null) || (string.IsNullOrEmpty(m_SysEnumMain.PKNO)))
            {
                return;
            }

            if (CBaseData.LoginNO != CBaseData.ADMINPKNO)
            {
                if (m_SysEnumMain.ENUM_STATE != 0)
                {
                    System.Windows.Forms.MessageBox.Show("非管理员不能更改该基础信息主信息", "维护主信息", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }
            }

            EnumMainEdit mainEdit = new EnumMainEdit(m_SysEnumMain);
            mainEdit.ShowDialog();

            GetMainData();
        }

        //删除主信息，已禁用
        private void BtnMainDel_Click(object sender, RoutedEventArgs e)
        {
            if (this.tvMain.SelectedItem != null)
            {
                SysEnumMain m_SysEnumMain = this.tvMain.SelectedItem as SysEnumMain;
                if (m_SysEnumMain == null)
                {
                    return;
                }

                ws.UseService(s => s.DelSysEnumMain(m_SysEnumMain.PKNO));
            }
        }

        #endregion

        private void BtnItemAdd_Click(object sender, RoutedEventArgs e)
        {
            //TreeListNode item = tvMain.SelectedItem as TreeListNode;
            //if ((item == null) || (item.Tag.ToString() == ""))
            //{
            //    gridItem.ItemsSource = null;
            //    return;
            //}
            //SysEnumMain m_SysEnumMain = item.Tag as SysEnumMain;

            SysEnumMain m_SysEnumMain = tvMain.SelectedItem as SysEnumMain;
            if ((m_SysEnumMain == null) || (string.IsNullOrEmpty(m_SysEnumMain.PKNO)))
            {
                return;
            }

            if (CBaseData.LoginNO != CBaseData.ADMINPKNO)
            {
                if ((m_SysEnumMain.ENUM_STATE == 2) || (m_SysEnumMain.ENUM_STATE == 3))
                {
                    System.Windows.Forms.MessageBox.Show("非管理员不能更改该基础信息明细信息", "维护明细信息", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }
            }

            #region 动画效果

            //dictBasic.Header = "基础信息明细项  【新增】";
            //TranslateTransform tt = new TranslateTransform();
            //DoubleAnimation da = new DoubleAnimation();
            ////动画时间
            //Duration duration = new Duration(TimeSpan.FromSeconds(1));
            ////设置按钮的转换效果
            //gbItem.RenderTransform = tt;
            //tt.Y = 200;
            //da.To = 0;
            //da.SpeedRatio = 1.3;
            //da.By = 0.5;
            //// da.Duration = duration;
            //tt.BeginAnimation(TranslateTransform.YProperty, da);

            #endregion
            
            //添加明细
            dictBasic.Header = "基础信息明细项  【新增】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;

            SysEnumItems m_SysEnumItems = new SysEnumItems()
            {
                COMPANY_CODE = "",
                ENUM_IDENTIFY = m_SysEnumMain.ENUM_IDENTIFY,  //基础信息标识
                ITEM_INDEX = gridItem.VisibleRowCount,
                ITEM_TYPE = 0,
                USE_FLAG = 1,
            };
            gbItem.DataContext = m_SysEnumItems;
        }

        private void BtnItemMod_Click(object sender, RoutedEventArgs e)
        {
            //TreeListNode item = tvMain.SelectedItem as TreeListNode;
            //if ((item == null) || (item.Tag.ToString() == ""))
            //{
            //    gridItem.ItemsSource = null;
            //    return;
            //}
            //SysEnumMain m_SysEnumMain = item.Tag as SysEnumMain;

            SysEnumMain m_SysEnumMain = tvMain.SelectedItem as SysEnumMain;
            if ((m_SysEnumMain == null) || (string.IsNullOrEmpty(m_SysEnumMain.PKNO)))
            {
                return;
            }

            if (gridItem.SelectedItem == null)
            {
                return;
            }
            if (CBaseData.LoginNO != CBaseData.ADMINPKNO)
            {
                if ((m_SysEnumMain.ENUM_STATE == 2) || (m_SysEnumMain.ENUM_STATE == 3))
                {
                    System.Windows.Forms.MessageBox.Show("非管理员不能更改该基础信息明细信息", "维护明细信息", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }
            }
            //修改明细
            dictBasic.Header = "基础信息明细项  【修改】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
        }

        private void BtnItemDel_Click(object sender, RoutedEventArgs e)
        {
            //TreeListNode item = tvMain.SelectedItem as TreeListNode;
            //if ((item == null) || (item.Tag.ToString() == ""))
            //{
            //    gridItem.ItemsSource = null;
            //    return;
            //}
            //SysEnumMain m_SysEnumMain = item.Tag as SysEnumMain;

            SysEnumMain m_SysEnumMain = tvMain.SelectedItem as SysEnumMain;
            if ((m_SysEnumMain == null) || (string.IsNullOrEmpty(m_SysEnumMain.PKNO)))
            {
                return;
            }
            if (CBaseData.LoginNO != CBaseData.ADMINPKNO)
            {
                if ((m_SysEnumMain.ENUM_STATE == 2) || (m_SysEnumMain.ENUM_STATE == 3))
                {
                    System.Windows.Forms.MessageBox.Show("非管理员不能更改该基础信息明细信息", "维护明细信息", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }
            }
            //删除明细
            SysEnumItems m_SysEnumItems = gridItem.SelectedItem as SysEnumItems;
            if (m_SysEnumItems == null)
            {
                return;
            }

            if ( System.Windows.Forms.MessageBox.Show($"确定删除基础信息【{m_SysEnumItems.ITEM_NAME}】吗？", 
                    @"删除信息",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                ws.UseService(s => s.DelSysEnumItems(m_SysEnumItems.PKNO));

                //删除成功.
                List<SysEnumItems> mSysEnumItemses =
                    ws.UseService(s => s.GetSysEnumItemss($"ENUM_IDENTIFY = {m_SysEnumMain.ENUM_IDENTIFY} AND USE_FLAG >= 0"))
                    .OrderBy(c => c.ITEM_INDEX)
                    .ToList();
                gridItem.ItemsSource = mSysEnumItemses;

                BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
            }
        }

        private void BtnItemSave_Click(object sender, RoutedEventArgs e)
        {
            //TreeListNode item = tvMain.SelectedItem as TreeListNode;
            //if ((item == null) || (item.Tag.ToString() == ""))
            //{
            //    gridItem.ItemsSource = null;
            //    return;
            //}
            //SysEnumMain m_SysEnumMain = item.Tag as SysEnumMain;

            SysEnumMain m_SysEnumMain = tvMain.SelectedItem as SysEnumMain;
            if ((m_SysEnumMain == null) || (string.IsNullOrEmpty(m_SysEnumMain.PKNO)))
            {
                return;
            }

            SysEnumItems m_SysEnumItems = gbItem.DataContext as SysEnumItems;
            if (m_SysEnumItems == null)
            {
                return;
            }

            #region  校验

            if (string.IsNullOrEmpty(m_SysEnumItems.ITEM_NAME))
            {
                Brush oldBrush = this.tbItemName.BorderBrush;
                this.tbItemName.BorderBrush = Brushes.Red;
                System.Windows.Forms.MessageBox.Show("请输入明细名称。", "保存", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.tbItemName.BorderBrush = oldBrush;
                return;
            }

            #endregion

            if (string.IsNullOrEmpty(m_SysEnumItems.PKNO)) //新增
            {
                m_SysEnumItems.PKNO = Guid.NewGuid().ToString("N");

                ws.UseService(s => s.AddSysEnumItems(m_SysEnumItems));

                //重新刷新数据
                List<SysEnumItems> mSysEnumItemses =
                    ws.UseService(s => s.GetSysEnumItemss($"ENUM_IDENTIFY = {m_SysEnumMain.ENUM_IDENTIFY} AND USE_FLAG >= 0"))
                    .OrderBy(c => c.ITEM_INDEX)
                    .ToList();
                gridItem.ItemsSource = mSysEnumItemses;
            }
            else  //修改
            {
                ws.UseService(s => s.UpdateSysEnumItems(m_SysEnumItems));
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
        
        private void BtnRefresh_Click(object sender, ItemClickEventArgs e)
        {
            //重新刷新数据
            SysEnumMain m_SysEnumMain = tvMain.SelectedItem as SysEnumMain;
            if ((m_SysEnumMain == null) || (string.IsNullOrEmpty(m_SysEnumMain.PKNO)))
            {
                return;
            }
            List<SysEnumItems> mSysEnumItemses =
                ws.UseService(s => s.GetSysEnumItemss($"ENUM_IDENTIFY = {m_SysEnumMain.ENUM_IDENTIFY} AND USE_FLAG >= 0"))
                .OrderBy(c => c.ITEM_INDEX)
                .ToList();
            gridItem.ItemsSource = mSysEnumItemses;
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnMoreSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        //双击修改
        private void gridItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SysEnumMain m_SysEnumMain = tvMain.SelectedItem as SysEnumMain;
            if ((m_SysEnumMain == null) || (string.IsNullOrEmpty(m_SysEnumMain.PKNO)))
            {
                return;
            }

            if (gridItem.SelectedItem == null)
            {
                return;
            }
            //修改明细
            dictBasic.Header = "基础信息明细项  【修改】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
        }

        private void BtnOutport_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            try
            {
                string path = System.Environment.CurrentDirectory + "//Temp//";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string excelFile = path + "EnumItem.xls";

                gridItem.View.ExportToXls(excelFile);
                System.Diagnostics.Process.Start(excelFile);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
            Cursor = Cursors.Arrow;
        }
    }
}
