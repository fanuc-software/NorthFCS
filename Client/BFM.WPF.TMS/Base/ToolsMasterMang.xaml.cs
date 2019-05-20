using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using BFM.Common.Base;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.Data.PubData;
using BFM.Server.DataAsset.EAMService;
using BFM.Server.DataAsset.TMSService;
using BFM.Server.DataAsset.WMSService;
using BFM.ContractModel;
using BFM.WPF.Base.Helper;

namespace BFM.WPF.TMS
{
    /// <summary>
    /// TableNOSetting.xaml 的交互逻辑
    /// </summary>
    public partial class ToolsMasterMang : Page
    {
        private WcfClient<ITMSService> ws = new WcfClient<ITMSService>(); 
        private WcfClient<IEAMService> wsEAM = new WcfClient<IEAMService>(); 
        private WcfClient<IWMSService> wsWMS = new WcfClient<IWMSService>();
        public ToolsMasterMang()
        {
            InitializeComponent();

            GetPage();
        }

        private void GetPage()
        {
            List<TmsToolsType> toolsTypes = ws.UseService(s => s.GetTmsToolsTypes("USE_FLAG = 1"));
            cmbToolsType.ItemsSource = toolsTypes;

            List<TmsToolsMaster> tableNos = ws.UseService(s => s.GetTmsToolsMasters("USE_FLAG > 0"));
            gridItem.ItemsSource = tableNos;

            BFM.WPF.Base.Helper.BindHelper.SetDictDataBindingGridItem(dictInfo, gridItem);

            cmbInAreaInfo.ItemsSource = wsWMS.UseService(s => s.GetWmsAreaInfos("USE_FLAG = 1"));
        }

        #region 按钮动作

        #region 编辑

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            gbItem.IsCollapsed = false;
            //新增
            dictInfo.Header = "刀具台账详细信息  【新增】";
            dictInfo.Visibility = Visibility.Visible;

            TmsToolsMaster toolsMaster = new TmsToolsMaster()
            {
                COMPANY_CODE = "",
                USE_FLAG = 1,
                TOOLS_LIFE_METHOD = 1,
                TOOLS_LIFE_PLAN = 1000,
                TOOLS_LIFE_USED = 0,
                COMPENSATION_SHAPE_DIAMETER = 0,
                COMPENSATION_ABRASION_DIAMETER = 0,
                COMPENSATION_SHAPE_LENGTH = 0,
                COMPENSATION_ABRASION_LENGTH = 0,
            };
            dictInfo.DataContext = toolsMaster;
        }
        
        private void BtnAddIn_Click(object sender, RoutedEventArgs e)
        {
            gbItem.IsCollapsed = false;
            //新增&入库
            dictInfo.Header = "刀具台账详细信息  【新增】";
            dictInfo.Visibility = Visibility.Visible;

            TmsToolsMaster toolsMaster = new TmsToolsMaster()
            {
                COMPANY_CODE = "",
                USE_FLAG = 1,
                TOOLS_LIFE_METHOD = 1,
                TOOLS_LIFE_PLAN = 1000,
                TOOLS_LIFE_USED = 0,
                COMPENSATION_SHAPE_DIAMETER = 0,
                COMPENSATION_ABRASION_DIAMETER = 0,
                COMPENSATION_SHAPE_LENGTH = 0,
                COMPENSATION_ABRASION_LENGTH = 0,
            };
            dictInfo.DataContext = toolsMaster;
            
            //入库
            WmsInvOperate invOperate = new WmsInvOperate()
            {
                COMPANY_CODE = "",
                OPERATE_SOURCE = 1,
                OPERATE_NUM = 1,
                OPERATE_TYPE = 1,
            };
            inbound.DataContext = invOperate;
            inbound.Visibility = Visibility.Visible;
        }

        private void BtnMod_Click(object sender, RoutedEventArgs e)
        {
            TmsToolsMaster m_TmsToolsMaster = gridItem.SelectedItem as TmsToolsMaster;
            if (m_TmsToolsMaster == null)
            {
                return;
            }
            //修改
            gbItem.IsCollapsed = false;
            dictInfo.Header = "刀具台账详细信息  【修改】";
            dictInfo.Visibility = Visibility.Visible;
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            //删除
            TmsToolsMaster m_TmsToolsMaster = gridItem.SelectedItem as TmsToolsMaster;
            if (m_TmsToolsMaster == null)
            {
                return;
            }
            if (System.Windows.Forms.MessageBox.Show($"确定删除刀具台账信息【{m_TmsToolsMaster.TOOLS_NAME}】吗？",
                    @"删除信息",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                m_TmsToolsMaster.USE_FLAG = -1;
                ws.UseService(s => s.UpdateTmsToolsMaster(m_TmsToolsMaster));

                //删除成功.
                GetPage();  //重新加载
            }
        }


        private void BtnIn_Click(object sender, RoutedEventArgs e)
        {
            TmsToolsMaster m_TmsToolsMaster = gridItem.SelectedItem as TmsToolsMaster;
            if (m_TmsToolsMaster == null)
            {
                return;
            }
            if (m_TmsToolsMaster.TOOLS_POSITION == 1)
            {
                System.Windows.Forms.MessageBox.Show("该刀具已经在库，不能再进行入库，请核实！", "入库", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            gbItem.IsCollapsed = false;
            //入库
            WmsInvOperate invOperate = new WmsInvOperate()
            {
                COMPANY_CODE = "",
                OPERATE_SOURCE = 1,
                OPERATE_NUM = 1,
                OPERATE_TYPE = 1,
            };
            inbound.DataContext = invOperate;
            inbound.Visibility = Visibility.Visible;
        }

        private void BtnOut_Click(object sender, RoutedEventArgs e)
        {
            TmsToolsMaster m_TmsToolsMaster = gridItem.SelectedItem as TmsToolsMaster;
            if (m_TmsToolsMaster == null)
            {
                return;
            }
            if (m_TmsToolsMaster.TOOLS_POSITION != 1)
            {
                System.Windows.Forms.MessageBox.Show("该刀具不在库，不能进行出库，请核实！", 
                    "出库", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            gbItem.IsCollapsed = false;
            //出库
            WmsInvOperate invOperate = new WmsInvOperate()
            {
                COMPANY_CODE = "",
                MATERIAL_PKNO = m_TmsToolsMaster.PKNO,
                ALLOCATION_PKNO = m_TmsToolsMaster.TOOLS_POSITION_PKNO, //货位
                OPERATE_SOURCE = 1,
                OPERATE_NUM = 1,
                OPERATE_TYPE = -1,  //出库
                OUT_TARGET = 0, //机床
            };
            outbound.DataContext = invOperate;
            outbound.Visibility = Visibility.Visible;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //保存dictInfo
            TmsToolsMaster m_TmsToolsMaster = dictInfo.DataContext as TmsToolsMaster;
            if (m_TmsToolsMaster == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(m_TmsToolsMaster.TOOLS_CODE))
            {
                System.Windows.Forms.MessageBox.Show("请输入刀具编码！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(m_TmsToolsMaster.PKNO)) //新增
            {
                TmsToolsMaster check =
                    ws.UseService(s => s.GetTmsToolsMasters($"TOOLS_CODE = '{m_TmsToolsMaster.TOOLS_CODE}'"))
                        .FirstOrDefault();
                if (check != null)
                {
                    System.Windows.Forms.MessageBox.Show("该刀具编码已经存在，不能新增相同的刀具编码！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                m_TmsToolsMaster.PKNO = Guid.NewGuid().ToString("N");
                m_TmsToolsMaster.CREATION_DATE = DateTime.Now;
                m_TmsToolsMaster.CREATED_BY = CBaseData.LoginName;
                m_TmsToolsMaster.LAST_UPDATE_DATE = DateTime.Now;

                ws.UseService(s => s.AddTmsToolsMaster(m_TmsToolsMaster));
            }
            else  //修改、入库、出库
            {
                TmsToolsMaster check =
                    ws.UseService(
                        s =>
                            s.GetTmsToolsMasters(
                                $"TOOLS_CODE = '{m_TmsToolsMaster.TOOLS_CODE}' AND PKNO <> '{m_TmsToolsMaster.PKNO}'"))
                        .FirstOrDefault();
                if (check != null)
                {
                    System.Windows.Forms.MessageBox.Show("该刀具编码已经存在，不能修改为该刀具编码！", "保存", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                m_TmsToolsMaster.LAST_UPDATE_DATE = DateTime.Now;
                m_TmsToolsMaster.UPDATED_BY = CBaseData.LoginName;

                if (inbound.Visibility == Visibility.Visible) //入库
                {
                    m_TmsToolsMaster.TOOLS_POSITION = 1;
                    WmsInvOperate invOperate = inbound.DataContext as WmsInvOperate;
                    if (invOperate != null)
                    {
                        m_TmsToolsMaster.TOOLS_POSITION_PKNO = invOperate.ALLOCATION_PKNO;
                    }
                }

                if (outbound.Visibility == Visibility.Visible) //出库
                {
                    WmsInvOperate invOperate = outbound.DataContext as WmsInvOperate;
                    if (invOperate != null)
                    {
                        m_TmsToolsMaster.TOOLS_POSITION = invOperate.OUT_TARGET;
                        m_TmsToolsMaster.TOOLS_POSITION_PKNO = invOperate.INSTALL_POS;
                    }
                }

                ws.UseService(s => s.UpdateTmsToolsMaster(m_TmsToolsMaster));
            }

            GetPage();  //重新加载

            gbItem.IsCollapsed = true;

            dictInfo.Visibility = Visibility.Collapsed;
            inbound.Visibility = Visibility.Collapsed;
            outbound.Visibility = Visibility.Collapsed;
            BindHelper.SetDictDataBindingGridItem(dictInfo, gridItem);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //取消
            gbItem.IsCollapsed = true;

            dictInfo.Visibility = Visibility.Collapsed;
            inbound.Visibility = Visibility.Collapsed;
            outbound.Visibility = Visibility.Collapsed;
            BindHelper.SetDictDataBindingGridItem(dictInfo, gridItem);
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
            TmsToolsMaster m_TmsToolsMaster = gridItem.SelectedItem as TmsToolsMaster;
            if (m_TmsToolsMaster == null)
            {
                return;
            }
            //修改
            gbItem.IsCollapsed = false;
            dictInfo.Header = "刀具台账详细信息  【修改】";
            dictInfo.Visibility = Visibility.Visible;
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WmsAreaInfo areaInfo = cmbInAreaInfo.SelectedItem as WmsAreaInfo;
            if (areaInfo == null)
            {
                cmbInAllocation.ItemsSource = null;
                return;
            }
            cmbInAllocation.ItemsSource =
                wsWMS.UseService(s => s.GetWmsAllocationInfos($"AREA_PKNO = '{areaInfo.PKNO}' AND USE_FLAG = 1"))
                    .OrderBy(c => c.CREATION_DATE)
                    .ToList();
        }

        private void OutBound_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SafeConverter.SafeToStr(cmbOutBound.SelectedValue) == "2") //机床
            {
                cmbOutPos.ItemsSource = wsEAM.UseService(s => s.GetAmAssetMasterNs("USE_FLAG = 1"));
            }
            else
            {
                cmbOutPos.ItemsSource = null;
            }
        }
    }
}
