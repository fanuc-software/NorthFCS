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
using BFM.Common.Data.PubData;
using BFM.Server.DataAsset.PLMService;
using BFM.Server.DataAsset.RSMService;
using BFM.ContractModel;
using BFM.Server.DataAsset;
using BFM.WPF.Base.Helper;

namespace BFM.WPF.PPM
{
    /// <summary>
    /// PlanMasterMang.xaml 的交互逻辑
    /// </summary>
    public partial class PlanMasterMang : Page
    {
        private const string HeaderName = "生产计划详细信息";
        private WcfClient<IPLMService> ws = new WcfClient<IPLMService>();
        private WcfClient<IRSMService> ws2 = new WcfClient<IRSMService>();

        public PlanMasterMang()
        {
            InitializeComponent();

            List<RsItemMaster> itemMasters = ws2.UseService(s => s.GetRsItemMasters("USE_FLAG = 1"));
            cmbMaterialInfo.ItemsSource = itemMasters;
        }

        private void PlanMasterMang_OnLoaded(object sender, RoutedEventArgs e)
        {
            GetPage();
        }

        private void GetPage()
        {
            List<PmPlanMaster> planMasters = ws.UseService(s => s.GetPmPlanMasters("RUN_STATE = 0 "));
            gridItem.ItemsSource = planMasters;

            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }

        #region 按钮动作

        #region 编辑

        //新增
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            #region 

            //TODO: 校验

            #endregion

            PmPlanMaster planMaster = new PmPlanMaster()
            {
                COMPANY_CODE = "",
                PLAN_DATE = DateTime.Now,
                PLAN_QTY = 1,
                RUN_STATE = 0,
                PLAN_START_TIME = DateTime.Now,
                PLAN_END_TIME = DateTime.Now,
                USE_FLAG = 1,  //启用
            };
            gbItem.DataContext = planMaster;

            dictBasic.Header = $"{HeaderName}  【新增】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
        }

        private void BtnMod_Click(object sender, RoutedEventArgs e)
        {
            ModItem();
        }

        /// <summary>
        /// 修改主体
        /// </summary>
        private void ModItem()
        {
            //修改
            #region 

            //TODO: 校验

            #endregion

            dictBasic.Header = $"{HeaderName} 【修改】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            //删除
            PmPlanMaster planMaster = gridItem.SelectedItem as PmPlanMaster;
            if (planMaster == null)
            {
                return;
            }

            if (System.Windows.Forms.MessageBox.Show($"确定删除计划【{planMaster.PLAN_NAME}】吗？",
                    @"删除信息",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                ws.UseService(s => s.DelPmPlanMaster(planMaster.PKNO));

                //删除成功.
                GetPage();
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //保存
            PmPlanMaster planMaster = gbItem.DataContext as PmPlanMaster;
            if (planMaster == null)
            {
                return;
            }

            #region  TODO: 校验

            //TODO: 校验

            #endregion

            if (string.IsNullOrEmpty(planMaster.PKNO)) //新增
            {
                planMaster.PKNO = Guid.NewGuid().ToString("N");
                planMaster.CREATED_BY = CBaseData.LoginName;
                planMaster.CREATION_DATE = DateTime.Now;
                planMaster.LAST_UPDATE_DATE = DateTime.Now;  //最后修改日期

                ws.UseService(s => s.AddPmPlanMaster(planMaster));
            }
            else  //修改
            {
                planMaster.UPDATED_BY = CBaseData.LoginName;
                planMaster.LAST_UPDATE_DATE = DateTime.Now;
                ws.UseService(s => s.UpdatePmPlanMaster(planMaster));
            }

            GetPage();  //重新刷新数据，根据需求是否进行刷新数据

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

        #endregion

        #region 查询

        private void BtnSearch_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //查询
        }

        private void BtnMoreSearch_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //高级查询
        }

        #endregion

        #region 导入导出
        private void BtnInPort_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //导入
        }

        private void BtnOutPort_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //导出
        }

        private void BtnReport_Click(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //报表
        }

        #endregion

        #endregion

        /// <summary>
        /// 双击表格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (gridItem.VisibleRowCount <= 0)
            {
                return;
            }
            //修改
            ModItem();
        }
    }
}
