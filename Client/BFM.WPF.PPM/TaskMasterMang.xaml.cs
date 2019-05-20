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
using BFM.ContractModel;
using BFM.WPF.Base.Helper;

namespace BFM.WPF.PPM
{
    /// <summary>
    /// TaskMasterMang.xaml 的交互逻辑
    /// </summary>
    public partial class TaskMasterMang : Page
    {
        private const string HeaderName = "生产计划详细信息";
        private WcfClient<IPLMService> ws = new WcfClient<IPLMService>();

        public TaskMasterMang()
        {
            InitializeComponent();

            GetPage();
        }

        private void GetPage()
        {
            List<PmPlanMaster> planMasters = ws.UseService(s => s.GetPmPlanMasters("RUN_STATE = 0 AND DISPATCH_QTY = 0"));
            gridItem.ItemsSource = planMasters;

            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }

        #region 按钮动作

        private void BtnChange_Click(object sender, RoutedEventArgs e)
        {
            PmPlanMaster planMaster = gridItem.SelectedItem as PmPlanMaster;
            if (planMaster == null)
            {
                return;
            }

            if (
                System.Windows.Forms.MessageBox.Show("确定要进行计划转换吗？", "计划转换", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) != DialogResult.OK)
            {
                return;
            }

            planMaster.RUN_STATE = 1; //已转换
            planMaster.UPDATED_INTROD += " 计划转换成任务";
            planMaster.UPDATED_BY = CBaseData.LoginName;
            planMaster.LAST_UPDATE_DATE = DateTime.Now;

            PmTaskMaster taskMaster = new PmTaskMaster()
            {
                PKNO = Guid.NewGuid().ToString("N"),
                COMPANY_CODE = "",
                PLAN_PKNO = planMaster.PKNO,
                BATCH_NO = "",
                ITEM_PKNO = planMaster.ITEM_PKNO,
                TASK_START_TIME = planMaster.PLAN_START_TIME,
                TASK_FINISH_TIME = planMaster.PLAN_END_TIME,
                TASK_QTY = planMaster.PLAN_QTY,
                LINE_QTY = 0,
                COMPLETE_QTY = 0,
                ONLINE_QTY = 0,
                RUN_STATE = 0,
                CREATED_BY = CBaseData.LoginName,
                CREATION_DATE = DateTime.Now,
                LAST_UPDATE_DATE = DateTime.Now,  //最后修改日期
                USE_FLAG = 1,
                REMARK = "",
            };

            ws.UseService(s => s.UpdatePmPlanMaster(planMaster));
            ws.UseService(s => s.AddPmTaskMaster(taskMaster));

            //保存成功
            GetPage();
        }

        private void BtnCancle_Click(object sender, RoutedEventArgs e)
        {
            PmPlanMaster planMaster = gridItem.SelectedItem as PmPlanMaster;
            if (planMaster == null)
            {
                return;
            }

            if (
                System.Windows.Forms.MessageBox.Show("确定要进行撤销该计划吗？", "计划撤销", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) != DialogResult.OK)
            {
                return;
            }

            planMaster.RUN_STATE = 2; //已撤销
            planMaster.UPDATED_INTROD += " 计划撤销";
            planMaster.UPDATED_BY = CBaseData.LoginName;
            planMaster.LAST_UPDATE_DATE = DateTime.Now;
            ws.UseService(s => s.UpdatePmPlanMaster(planMaster));

            //保存成功
            GetPage();
        }

        #endregion
    }
}
