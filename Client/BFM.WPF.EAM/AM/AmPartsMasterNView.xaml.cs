using BFM.Common.Data.PubData;
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
using BFM.Common.Base;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Server.DataAsset.EAMService;
using BFM.Server.DataAsset.WMSService;
using BFM.ContractModel;
using BFM.WPF.Base;
using BFM.WPF.Base.Helper;

namespace BFM.WPF.EAM.AM
{
    /// <summary>
    /// AmPartsMasterNView.xaml 的交互逻辑
    /// </summary>
    public partial class AmPartsMasterNView : Page
    {
        private WcfClient<IEAMService> ws;
        private WcfClient<IWMSService> wsWMS = new WcfClient<IWMSService>();
        public AmPartsMasterNView()
        {
            InitializeComponent();
            ws = new WcfClient<IEAMService>();
            wsWMS = new WcfClient<IWMSService>();
            GetPage();
        }

        private void GetPage()
        {
            List<AmPartsMasterN> source = ws.UseService(s => s.GetAmPartsMasterNs(""));
            //GetImage(source);
            gridItem.ItemsSource = source.OrderBy(s => s.CREATION_DATE);

        }
        


        #region 按钮动作

        #region 编辑

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //新增
            #region 

            //TODO: 校验

            #endregion
            AmPartsMasterN PartsMasterN = new AmPartsMasterN()
            {
                COMPANY_CODE = "",
                CREATION_DATE = DateTime.Now,
                LAST_UPDATE_DATE = DateTime.Now,  //最后修改日期
                USE_FLAG = 1,  //启用
            };
            dictInfo.DataContext = PartsMasterN;

            //dictBasic.Header = $"{HeaderName}  【新增】";
        
            gbItem.IsCollapsed = false;
         
            dictInfo.Visibility = Visibility.Visible;
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

            dictInfo.DataContext = gridItem.SelectedItem;
        
            gbItem.IsCollapsed = false;
            dictInfo.Visibility = Visibility.Visible;
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            //删除
            AmPartsMasterN PartsMasterN = gridItem.SelectedItem as AmPartsMasterN;
            if (PartsMasterN == null)
            {
                return;
            }

            if (WPFMessageBox.ShowConfirm($"确定删除部件【{PartsMasterN.DEPARTMENT_NAME}】吗？", "删除信息") == WPFMessageBoxResult.OK)
            {
                ws.UseService(s => s.DelAmAssetMasterN(PartsMasterN.PKNO));

                //删除成功.
                GetPage();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //保存
            AmPartsMasterN PartsMasterN = dictInfo.DataContext as AmPartsMasterN;
            if (PartsMasterN == null)
            {
                return;
            }

            #region  TODO: 校验

            //TODO: 校验

            #endregion

            if (string.IsNullOrEmpty(PartsMasterN.PKNO)) //新增
            {
                PartsMasterN.PKNO = Guid.NewGuid().ToString("N");
                PartsMasterN.CREATED_BY = CBaseData.LoginName;
                PartsMasterN.CREATION_DATE = DateTime.Now;
                PartsMasterN.LAST_UPDATE_DATE = DateTime.Now;  //最后修改日期

                ws.UseService(s => s.AddAmPartsMasterN(PartsMasterN));
            }
            else  //修改
            {
                PartsMasterN.UPDATED_BY = CBaseData.LoginName;
                PartsMasterN.LAST_UPDATE_DATE = DateTime.Now;
                ws.UseService(s => s.UpdateAmPartsMasterN(PartsMasterN));
            }

            if (inbound.Visibility == Visibility.Visible)  //入库
            {
                WmsInvOperate invOperate = inbound.DataContext as WmsInvOperate;

                //PartsMasterN.TOOLS_POSITION = 1;  //在库
                PartsMasterN.LAY_LOCATION = invOperate.ALLOCATION_PKNO;  //货位
                ws.UseService(s => s.UpdateAmPartsMasterN(PartsMasterN));

                invOperate.PKNO = Guid.NewGuid().ToString("N");
                invOperate.MATERIAL_PKNO = PartsMasterN.PKNO;
                invOperate.CREATED_BY = CBaseData.LoginName;
                invOperate.CREATION_DATE = DateTime.Now;
                wsWMS.UseService(s => s.AddWmsInvOperate(invOperate));
            }

            if (outbound.Visibility == Visibility.Visible) //出库
            {
                WmsInvOperate invOperate = outbound.DataContext as WmsInvOperate;

                //PartsMasterN.TOOLS_POSITION = (invOperate.OUT_TARGET == 1) ? 2 : 10;  //机床/出库

                //PartsMasterN.TOOLS_POSITION_PKNO = invOperate.INSTALL_POS;  //机床或其他
                ws.UseService(s => s.UpdateAmPartsMasterN(PartsMasterN));

                invOperate.PKNO = Guid.NewGuid().ToString("N");
                invOperate.MATERIAL_PKNO = PartsMasterN.PKNO;
                invOperate.CREATED_BY = CBaseData.LoginName;
                invOperate.CREATION_DATE = DateTime.Now;
                wsWMS.UseService(s => s.AddWmsInvOperate(invOperate));
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

        private void BtnIn_Click(object sender, RoutedEventArgs e)
        {
            AmPartsMasterN m_AmPartsMasterN = gridItem.SelectedItem as AmPartsMasterN;
            if (m_AmPartsMasterN == null)
            {
                return;
            }
            //if (m_AmPartsMasterN.TOOLS_POSITION == 1)
            //{
            //    System.Windows.Forms.MessageBox.Show("该刀具已经在库，不能再进行入库，请核实！", "入库", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            dictInfo.DataContext = gridItem.SelectedItem;
            List<WmsAreaInfo> m_areaInfo = wsWMS.UseService(s => s.GetWmsAreaInfos(" USE_FLAG = 1"))
                 .OrderBy(c => c.CREATION_DATE)
                 .ToList();
            cmbInAreaInfo.ItemsSource = m_areaInfo;
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
            AmPartsMasterN m_AmPartsMasterN = gridItem.SelectedItem as AmPartsMasterN;
            if (m_AmPartsMasterN == null)
            {
                return;
            }
            //if (m_AmPartsMasterN.TOOLS_POSITION != 1)
            //{
            //    System.Windows.Forms.MessageBox.Show("该刀具不在库，不能进行出库，请核实！", "出库", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            gbItem.IsCollapsed = false;
            //出库
            WmsInvOperate invOperate = new WmsInvOperate()
            {
                COMPANY_CODE = "",
                MATERIAL_PKNO = m_AmPartsMasterN.PKNO,
                ALLOCATION_PKNO = m_AmPartsMasterN.LAY_LOCATION, //货位
                OPERATE_SOURCE = 1,
                OPERATE_NUM = 1,
                OPERATE_TYPE = -1,  //出库
                OUT_TARGET = 0, //出库
            };
            outbound.DataContext = invOperate;
            outbound.Visibility = Visibility.Visible;
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
            if (SafeConverter.SafeToStr(cmbOutBound.SelectedValue) == "1") //机床
            {
                cmbOutPos.ItemsSource = ws.UseService(s => s.GetAmAssetMasterNs("USE_FLAG = 1"));
            }
            else
            {
                cmbOutPos.ItemsSource = null;
            }
        }
        #endregion

        #endregion

    }
}
