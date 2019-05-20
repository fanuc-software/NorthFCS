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
using BFM.WPF.Base.Helper;
using BFM.WPF.SDM.TableNO;

namespace BFM.WPF.PPM
{
    /// <summary>
    /// TaskLineMang.xaml 的交互逻辑
    /// </summary>
    public partial class TaskLineMang : Page
    {
        private const string HeaderName = "生产任务产线下达";
        private WcfClient<IPLMService> ws = new WcfClient<IPLMService>();
        private WcfClient<IRSMService> ws2 = new WcfClient<IRSMService>();

        public TaskLineMang()
        {
            InitializeComponent();

            List<RsLine> lines = ws2.UseService(s => s.GetRsLines("USE_FLAG = 1"));
            cmbLineInfo.ItemsSource = lines;

        }

        private void TaskLineMang_OnLoaded(object sender, RoutedEventArgs e)
        {
            GetPage();
        }
        private void GetPage()
        {
            List<PmTaskMaster> taskMasters = ws.UseService(s => s.GetPmTaskMasters("RUN_STATE < 2 "));
            gridItem.ItemsSource = taskMasters;

            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }

        #region 按钮动作

        #region 编辑

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            PmTaskMaster taskMaster = gridItem.SelectedItem as PmTaskMaster;
            if (taskMaster == null)
            {
                return;
            }

            #region 

            //TODO: 校验

            #endregion

            PmTaskLine planLine = new PmTaskLine()
            {
                COMPANY_CODE = "",
                TASK_PKNO = taskMaster.PKNO,
                ITEM_PKNO = taskMaster.ITEM_PKNO,
                TASK_NO = TableNOHelper.GetNewNO("PmTaskLine.TASK_NO", "N"),
                TASK_START_TIME = taskMaster.TASK_START_TIME,
                TASK_FINISH_TIME = taskMaster.TASK_FINISH_TIME,
                TASK_QTY = taskMaster.TASK_QTY - taskMaster.LINE_QTY,
                PREPARED_QTY = 0,  //准备完成数量
                COMPLETE_QTY = 0,
                ONLINE_QTY = 0,
                ONCE_QTY = 0,
                RUN_STATE = 0,
                USE_FLAG = 1, //启用
            };
            gbItem.DataContext = planLine;

            dictBasic.Header = $"{HeaderName}  【分配产线】";
            gbItem.IsCollapsed = false;
            gbItem.Visibility = Visibility.Visible;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //保存
            PmTaskLine taskLine = gbItem.DataContext as PmTaskLine;
            if (taskLine == null)
            {
                return;
            }

            PmTaskMaster taskMaster = gridItem.SelectedItem as PmTaskMaster;
            if (taskMaster == null)
            {
                return;
            }

            #region  TODO: 校验

            if (string.IsNullOrEmpty(taskLine.LINE_PKNO))
            {
                System.Windows.Forms.MessageBox.Show("请选择产线", "保存", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(taskLine.TASK_NO))
            {
                System.Windows.Forms.MessageBox.Show("请输入相应的任务编号", "保存", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (taskMaster.TASK_QTY - taskMaster.LINE_QTY < taskLine.TASK_QTY)
            {
                System.Windows.Forms.MessageBox.Show("分配的数量大于剩余数量，请重新选择", "保存", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            #endregion

            taskMaster.LINE_QTY += taskLine.TASK_QTY;
            if (taskMaster.TASK_QTY <= taskMaster.LINE_QTY)
            {
                taskMaster.RUN_STATE = 2;
            }
            else
            {
                taskMaster.RUN_STATE = 1;
            }

            ws.UseService(s => s.UpdatePmTaskMaster(taskMaster));

            if (string.IsNullOrEmpty(taskLine.PKNO)) //新增
            {
                taskLine.PKNO = Guid.NewGuid().ToString("N");
                taskLine.CREATED_BY = CBaseData.LoginName;
                taskLine.CREATION_DATE = DateTime.Now;
                taskLine.LAST_UPDATE_DATE = DateTime.Now; //最后修改日期

                ws.UseService(s => s.AddPmTaskLine(taskLine));
            }
            else  //修改
            {
                taskLine.UPDATED_BY = CBaseData.LoginName;
                taskLine.LAST_UPDATE_DATE = DateTime.Now;
                ws.UseService(s => s.UpdatePmTaskLine(taskLine));
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

        #endregion

    }
}
