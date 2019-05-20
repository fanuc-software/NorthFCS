using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.Data.PubData;
using BFM.Server.DataAsset.TMSService;
using BFM.ContractModel;
using BFM.Server.DataAsset.SDMService;
using BFM.WPF.Base.Helper;

namespace BFM.WPF.TMS
{
    /// <summary>
    /// ToolsTypeMang.xaml 的交互逻辑
    /// </summary>
    public partial class ToolsTypeMang : Page
    {
        private WcfClient<ITMSService> ws = new WcfClient<ITMSService>(); 
        private WcfClient<ISDMService> wsSDM = new WcfClient<ISDMService>(); 
        public ToolsTypeMang()
        {
            InitializeComponent();

            GetPage();
        }

        private void GetPage()
        {
            List<TmsToolsType> toolsTypes = ws.UseService(s => s.GetTmsToolsTypes("USE_FLAG > 0"));
            gridItem.ItemsSource = toolsTypes;

            BFM.WPF.Base.Helper.BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }

        #region 按钮动作

        #region 编辑

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //新增
            dictInfo.Header = "刀具类型信息详细  【新增】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;

            TmsToolsType toolsType = new TmsToolsType()
            {
                COMPANY_CODE = "",
                TOOLS_TYPE_PARAM = "",
                TOOLS_TYPE_ASSIST = "",
                USE_FLAG = 1,
            };
            gbItem.DataContext = toolsType;
        }

        private void BtnMod_Click(object sender, RoutedEventArgs e)
        {
            TmsToolsType m_TmsToolsType = gridItem.SelectedItem as TmsToolsType;
            if (m_TmsToolsType == null)
            {
                return;
            }
            //修改
            dictInfo.Header = "刀具类型信息详细  【修改】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            //删除
            TmsToolsType m_TmsToolsType = gridItem.SelectedItem as TmsToolsType;
            if (m_TmsToolsType == null)
            {
                return;
            }
            if (System.Windows.Forms.MessageBox.Show($"确定删除刀具类型【{m_TmsToolsType.TOOLS_TYPE_NAME}】吗？",
                    @"删除信息",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                m_TmsToolsType.USE_FLAG = -1;
                ws.UseService(s => s.UpdateTmsToolsType(m_TmsToolsType));

                //删除成功.
                GetPage();  //重新加载
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //保存
            TmsToolsType m_TmsToolsType = gbItem.DataContext as TmsToolsType;
            if (m_TmsToolsType == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(m_TmsToolsType.TOOLS_TYPE_CODE))
            {
                System.Windows.Forms.MessageBox.Show("请输入刀具类型编号！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(m_TmsToolsType.PKNO)) //新增
            {
                TmsToolsType check =
                    ws.UseService(s => s.GetTmsToolsTypes($"TOOLS_TYPE_CODE = '{m_TmsToolsType.TOOLS_TYPE_CODE}'"))
                        .FirstOrDefault();
                if (check != null)
                {
                    System.Windows.Forms.MessageBox.Show("该刀具类型编号已经存在，不能新增相同的刀具类型编号！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                m_TmsToolsType.PKNO = Guid.NewGuid().ToString("N");
                m_TmsToolsType.CREATION_DATE = DateTime.Now;
                m_TmsToolsType.CREATED_BY = CBaseData.LoginName;
                m_TmsToolsType.LAST_UPDATE_DATE = DateTime.Now;  //最后修改日期

                ws.UseService(s => s.AddTmsToolsType(m_TmsToolsType));
            }
            else  //修改
            {
                TmsToolsType check =
                    ws.UseService(
                        s =>
                            s.GetTmsToolsTypes(
                                $"TOOLS_TYPE_CODE = '{m_TmsToolsType.TOOLS_TYPE_CODE}' AND PKNO <> '{m_TmsToolsType.PKNO}'"))
                        .FirstOrDefault();
                if (check != null)
                {
                    System.Windows.Forms.MessageBox.Show("该刀具类型编号已经存在，不能修改为该刀具类型编号！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                m_TmsToolsType.LAST_UPDATE_DATE = DateTime.Now;
                m_TmsToolsType.UPDATED_BY = CBaseData.LoginName;
                ws.UseService(s => s.UpdateTmsToolsType(m_TmsToolsType));
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
            TmsToolsType m_TmsToolsType = gridItem.SelectedItem as TmsToolsType;
            if (m_TmsToolsType == null)
            {
                return;
            }
            //修改
            dictInfo.Header = "刀具类型信息详细  【修改】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
        }

        private void cmbTypeBasic_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SysEnumItems item = cmbTypeBasic.SelectedItem as SysEnumItems;
            if (item == null)
            {
                return;
            }

            SysEnumItems item2 = cmbTypeSpecified.SelectedItem as SysEnumItems;
            if (item2 == null)
            {
                return;
            }

            TmsToolsType m_TmsToolsType = gbItem.DataContext as TmsToolsType;
            if (m_TmsToolsType == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(m_TmsToolsType.PKNO))
            {
                string typeCode = "T" + item.ITEM_CODE + item2.ITEM_CODE +
                                                 m_TmsToolsType.TOOLS_TYPE_PARAM + m_TmsToolsType.TOOLS_TYPE_ASSIST;

                //tbTypeCode
                m_TmsToolsType.TOOLS_TYPE_CODE = typeCode;

                gbItem.DataContext = m_TmsToolsType;
            }
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            SysEnumItems item = cmbTypeBasic.SelectedItem as SysEnumItems;
            if (item == null)
            {
                return;
            }

            SysEnumItems item2 = cmbTypeSpecified.SelectedItem as SysEnumItems;
            if (item2 == null)
            {
                return;
            }

            TmsToolsType m_TmsToolsType = gbItem.DataContext as TmsToolsType;
            if (m_TmsToolsType == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(m_TmsToolsType.PKNO))
            {
                string typeCode = "T" + item.ITEM_CODE + item2.ITEM_CODE +
                                                 m_TmsToolsType.TOOLS_TYPE_PARAM + m_TmsToolsType.TOOLS_TYPE_ASSIST;

                //tbTypeCode
                m_TmsToolsType.TOOLS_TYPE_CODE = typeCode;

                gbItem.DataContext = m_TmsToolsType;
            }
        }
    }
}
