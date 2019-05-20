using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Forms;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.Data.PubData;
using BFM.Server.DataAsset.EAMService;
using BFM.Server.DataAsset.PLMService;
using BFM.Server.DataAsset.RSMService;
using BFM.Server.DataAsset.TMSService;
using BFM.ContractModel;
using BFM.Server.DataAsset.QMSService;
using ComboBox = System.Windows.Controls.ComboBox;

namespace BFM.WPF.FMS.ProcessControl
{
    /// <summary>
    /// PrepareProcess.xaml 的交互逻辑
    /// </summary>
    public partial class PrepareProcess : Page
    {
        private WcfClient<IPLMService> wsPLM = new WcfClient<IPLMService>(); //计划
        private WcfClient<IRSMService> wsRSM = new WcfClient<IRSMService>(); //工艺资源
        private WcfClient<IEAMService> wsEAM = new WcfClient<IEAMService>(); //设备
        private WcfClient<ITMSService> wsTMS = new WcfClient<ITMSService>(); //刀具
        private WcfClient<IQMSService> wsQMS = new WcfClient<IQMSService>(); //质量

        public PrepareProcess()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            GetPage();
        }

        private void GetPage()
        {
            cmbTask.ItemsSource =
                wsPLM.UseService(s => s.GetPmTaskLines("USE_FLAG = 1 AND RUN_STATE < 10 ")) //未完成的
                    .Where(c => c.TASK_QTY > c.PREPARED_QTY)
                    .OrderBy(c => c.CREATION_DATE)
                    .ToList(); //生产线任务
        }

        #region 获取查询条件

        private void cmbTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbProduct.SelectedIndex = -1;
            cmbRoutingMain.SelectedIndex = -1;

            cmbProduct.ItemsSource = null;
            cmbRoutingMain.ItemsSource = null;

            //选择生产线 => 获取产品
            PmTaskLine taskLine = cmbTask.SelectedItem as PmTaskLine;
            if (taskLine == null)
            {
                return;
            }

            tbLiftQty.Text = (taskLine.TASK_QTY - taskLine.PREPARED_QTY).ToString();  //任务剩余数量

            tbTaskQty.Text = tbLiftQty.Text;

            cmbProduct.ItemsSource =
                wsPLM.UseService(s => s.GetPmTaskLines($"USE_FLAG = 1 AND TASK_PKNO = '{taskLine.TASK_PKNO}'"))
                    .Join(wsRSM.UseService(s => s.GetRsItemMasters("USE_FLAG = 1")), c => c.ITEM_PKNO, d => d.PKNO,
                        (c, d) => new {d.PKNO, d.ITEM_NAME}).Distinct().ToList();

            if (cmbProduct.Items.Count > 0) cmbProduct.SelectedIndex = 0; //选择第一个
        }

        private void cmbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //选择产品 => 获取工艺路线

            if (cmbProduct.SelectedValue == null) return;

            string itemPKNO = cmbProduct.SelectedValue.ToString(); //产品

            if (string.IsNullOrEmpty(itemPKNO))
            {
                return;
            }

            PmTaskLine taskLine = cmbTask.SelectedItem as PmTaskLine;
            if (taskLine == null)
            {
                return;
            }

            cmbRoutingMain.ItemsSource =
                wsRSM.UseService(s => s.GetRsRoutingItems($"USE_FLAG = 1 AND ITEM_PKNO = '{itemPKNO}'"))
                    .Join(wsRSM.UseService(s => s.GetRsRoutingHeads("USE_FLAG = 1")), c => c.ROUTING_PKNO,
                        d => d.PKNO, (c, d) => new {d.PKNO, d.ROUTING_NAME});

            //需增加状态表，已经完成准备的工序不再显示
            if (cmbRoutingMain.Items.Count > 0) cmbRoutingMain.SelectedIndex = 0; //选择第一个

            MesJobOrder jobOrder =
                wsPLM.UseService(s => s.GetMesJobOrders($"LINE_TASK_PKNO = '{taskLine.PKNO}' AND USE_FLAG = 1 AND LINE_PKNO = '{CBaseData.CurLinePKNO}'"))
                    .OrderByDescending(c => c.BATCH_NO)   //按照批次排序
                    .FirstOrDefault();

            int batchIndex = 1;
            if (jobOrder != null)
            {
                int.TryParse(jobOrder.BATCH_NO, out batchIndex);
                batchIndex++;
            }

            lbBatchIndex.Content = batchIndex.ToString(); //批次
        }

        private void cmbRoutingMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //选择工艺路线 => 获取工艺路线

            if (cmbRoutingMain.SelectedValue == null) return;

            string routingMainPKNO = cmbRoutingMain.SelectedValue.ToString(); //工艺路线

            if (string.IsNullOrEmpty(routingMainPKNO))
            {
                return;
            }
            List<RsRoutingDetail> rsRoutingDetails = 
                wsRSM.UseService(s => s.GetRsRoutingDetails($"ROUTING_PKNO = '{routingMainPKNO}' AND USE_FLAG = 1"))
                    .OrderBy(c => c.OP_INDEX)
                    .ToList();
            foreach (RsRoutingDetail detail in rsRoutingDetails)
            {
                detail.OP_TYPE = "";  //临时存放 Asset Code 用
                detail.REMARK = "";   //临时存放 指令动作名称 用
            }
            gridProcessInfo.ItemsSource = rsRoutingDetails;
           
        }

        #endregion

        private void bPrepare_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void bSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (cmbRoutingMain.SelectedValue == null) return;

            #region 创建生产过程

            //生产线数据
            PmTaskLine taskLine = cmbTask.SelectedItem as PmTaskLine;
            //产品数据
            string itemPKNO = cmbProduct.SelectedValue.ToString();
            //准备完成
            List<MesProcessCtrol> processCtrols = new List<MesProcessCtrol>();
            List<RsRoutingDetail> rsRoutingDetails = gridProcessInfo.ItemsSource as List<RsRoutingDetail>;
            
            //已创建加工数量
            int qty = CheckPlanQTY(taskLine.TASK_NO);
            decimal preparedQty = 0;
            decimal.TryParse(tbTaskQty.Text.ToString(), out preparedQty);
            if (preparedQty <= 0)
            {
                MessageBox.Show("请输入正确的任务数量.", "完成生产准备", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            #region 判断加工数量

            if (tbTaskQty.Text.ToString() == "")
            {
                MessageBox.Show($"请输入数量", "未输入数量", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (int.Parse(tbTaskQty.Text.ToString()) + qty > taskLine.TASK_QTY)
            {
                MessageBox.Show($"输入数量超出订单加工数量", "数量超限", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            #endregion

            MesJobOrder jobOrder = new MesJobOrder()
            {
                PKNO = Guid.NewGuid().ToString("N"),
                COMPANY_CODE = "",
                LINE_PKNO = taskLine.LINE_PKNO,  //产线信息
                LINE_TASK_PKNO = taskLine.PKNO,
                ITEM_PKNO = itemPKNO,
                JOB_ORDER_NO = taskLine.TASK_NO + lbBatchIndex.Content.ToString(),
                BATCH_NO = lbBatchIndex.Content.ToString(),
                ROUTING_DETAIL_PKNO = cmbRoutingMain.SelectedValue.ToString(),
                TASK_QTY = preparedQty,
                COMPLETE_QTY = 0,
                ONLINE_QTY = 0,
                ONCE_QTY = 0,
                RUN_STATE = 1,  //准备完成  
                CREATION_DATE = DateTime.Now,
                CREATED_BY = CBaseData.LoginName,
                LAST_UPDATE_DATE = DateTime.Now,  //最后修改日期
                USE_FLAG = 1,
                REMARK = "",
            };

            List<MesProcessCtrol> newMesProcessCtrols = new List<MesProcessCtrol>();

            int iProcessIndex = 0;

            foreach (RsRoutingDetail item in rsRoutingDetails)
            {
                if (string.IsNullOrEmpty(item.WC_ABV))
                {
                    MessageBox.Show($"工序【{item.OP_NAME}】加工设备不能为空，请选择加工设备!", 
                        "完成生产准备", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                //if (string.IsNullOrEmpty(item.PROCESS_ACTION_PKNO))
                //{
                //    MessageBox.Show($"工序【{item.OP_NAME}】指令动作不能为空，请选择指令动作!",
                //        "完成生产准备", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    return;
                //}


                MesProcessCtrol mesProcess = new MesProcessCtrol();
                mesProcess.PKNO = Guid.NewGuid().ToString("N");
                mesProcess.COMPANY_CODE = "";
                mesProcess.ITEM_PKNO = jobOrder.ITEM_PKNO;
                mesProcess.JOB_ORDER_PKNO = jobOrder.PKNO;
                mesProcess.SUB_JOB_ORDER_NO = jobOrder.JOB_ORDER_NO + "-" + iProcessIndex;

                mesProcess.ROUTING_DETAIL_PKNO = item.PKNO;  //工序PKNO
                mesProcess.PROCESS_DEVICE_PKNO = item.WC_ABV;  //设备信息
                mesProcess.PROCESS_INDEX = iProcessIndex;

                mesProcess.PROCESS_ACTION_TYPE = item.PROCESS_ACTION_TYPE;
                mesProcess.PROCESS_ACTION_PKNO = item.PROCESS_ACTION_PKNO;
                mesProcess.PROCESS_ACTION_PARAM1_VALUE = item.PROCESS_ACTION_PARAM1_VALUE;   //控制参数1
                mesProcess.PROCESS_ACTION_PARAM2_VALUE = item.PROCESS_ACTION_PARAM2_VALUE;   //控制参数2

                mesProcess.PROCESS_QTY = 0;
                mesProcess.COMPLETE_QTY = 0;
                mesProcess.QUALIFIED_QTY = 0;
                mesProcess.PROCESS_STATE = 1;  //准备完成

                mesProcess.CREATED_BY = CBaseData.LoginName;
                mesProcess.CREATION_DATE = DateTime.Now;
                mesProcess.LAST_UPDATE_DATE = DateTime.Now;  //最后修改日期
                mesProcess.USE_FLAG = 1;

                iProcessIndex++;
                newMesProcessCtrols.Add(mesProcess);
            }

            wsPLM.UseService(s => s.AddMesJobOrder(jobOrder));  //添加工单
            foreach (MesProcessCtrol mesProcessCtrol in newMesProcessCtrols)
            {
                wsPLM.UseService(s => s.AddMesProcessCtrol(mesProcessCtrol));  //添加具体工序
            }

            #endregion

            //修改产线任务的完成数量
            taskLine.PREPARED_QTY += preparedQty;
            if (taskLine.RUN_STATE == 0) taskLine.RUN_STATE = 1;
            wsPLM.UseService(s => s.UpdatePmTaskLine(taskLine));

            tbLiftQty.Text = "";
            cmbTask.SelectedIndex = -1;
            gridProcessInfo.ItemsSource = null;
            tbTaskQty.Text = "";
            lbBatchIndex.Content = "";
            if (System.Windows.Forms.MessageBox.Show($"确定生成质检计划吗？",
                    @"生成质检信息",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                QmsCheckParam qmsCheckParam = wsQMS.UseService(s =>
                    s.GetQmsCheckParams(" USE_FLAG = 1 AND ITEM_PKNO = " + taskLine.ITEM_PKNO + "")).FirstOrDefault();
                if (qmsCheckParam==null)
                {
                    return;
                }

                List<QmsRoutingCheck> qmsRoutingChecks = wsQMS.UseService(s =>
                    s.GetQmsRoutingChecks(" USE_FLAG = 1 AND CHECK_PARAM_PKNO = " + qmsCheckParam.PKNO + ""));
                if (qmsRoutingChecks.Count<=0)
                {
                    return;
                }
                foreach (var itemRoutingCheck in qmsRoutingChecks)
                {
                    //创建质检主表内容
                    if (itemRoutingCheck.CHK_MODE=="首检"|| itemRoutingCheck.CHK_MODE == "尾检")//首尾检
                    {
                        QmsCheckMaster qmsCheckMaster = new QmsCheckMaster();
                        qmsCheckMaster.PKNO = Guid.NewGuid().ToString("N");
                        if (itemRoutingCheck.CHK_MODE == "首检")
                        {
                            qmsCheckMaster.CHECK_NO = "ZJ" + jobOrder.JOB_ORDER_NO + (1 * 1000).ToString("0000");
                        }
                        else
                        {
                            qmsCheckMaster.CHECK_NO = "ZJ" + jobOrder.JOB_ORDER_NO + (2 * 1000).ToString("0000");
                        }

                        qmsCheckMaster.CHECK_PARAM_PKNO = itemRoutingCheck.CHECK_PARAM_PKNO;
                        qmsCheckMaster.ROUTING_CHECK_PKNO = itemRoutingCheck.PKNO;
                        qmsCheckMaster.CREATION_DATE = DateTime.Now;
                        qmsCheckMaster.LAST_UPDATE_DATE = DateTime.Now;  //最后修改日期
                        qmsCheckMaster.USE_FLAG = 1;
                        qmsCheckMaster.CHECK_STATUS = "1";
                        qmsCheckMaster.TASKLINE_PKNO = jobOrder.JOB_ORDER_NO;//以工单编号应用为关联
                        qmsCheckMaster.PROCESS_PKNO = itemRoutingCheck.PROCESS_PKNO;
                        qmsCheckMaster.CHK_MODE = itemRoutingCheck.CHK_MODE;
                        wsQMS.UseService(s => s.AddQmsCheckMaster(qmsCheckMaster));
                    }
                    else
                    {
                        //todo:未判断首尾检对批次检测影响
                        if (itemRoutingCheck.CHK_FREQ_VALUE==String.Empty)
                        {
                            return;
                            
                        }
                        int count = int.Parse(jobOrder.TASK_QTY.ToString()) / int.Parse(itemRoutingCheck.CHK_FREQ_VALUE);
                        for (int i = 1; i <= count; i++)
                        {
                            QmsCheckMaster qmsCheckMaster = new QmsCheckMaster();
                            qmsCheckMaster.PKNO = Guid.NewGuid().ToString("N");
                            qmsCheckMaster.CHECK_PARAM_PKNO = itemRoutingCheck.CHECK_PARAM_PKNO;
                            qmsCheckMaster.ROUTING_CHECK_PKNO = itemRoutingCheck.PKNO;
                            qmsCheckMaster.CHECK_NO = "ZJ" + jobOrder.JOB_ORDER_NO + ((3 * 1000)+i).ToString("0000");//3为批次检测，抽检需要另外规则定制生成
                            qmsCheckMaster.CREATION_DATE = DateTime.Now;
                            qmsCheckMaster.LAST_UPDATE_DATE = DateTime.Now;  //最后修改日期
                            qmsCheckMaster.USE_FLAG = 1;
                            qmsCheckMaster.CHECK_STATUS = "1";
                            qmsCheckMaster.TASKLINE_PKNO = jobOrder.JOB_ORDER_NO;//以工单编号应用为关联
                            qmsCheckMaster.PROCESS_PKNO = itemRoutingCheck.PROCESS_PKNO;
                            qmsCheckMaster.CHK_MODE = itemRoutingCheck.CHK_MODE;
                            wsQMS.UseService(s => s.AddQmsCheckMaster(qmsCheckMaster));
                        }
                    }
                   
                }
            }
            GetPage();
        }
        /// <summary>
        /// 判断加工数量
        /// </summary>
        /// <returns></returns>
        public int CheckPlanQTY(string LineTaskNO)
        {
            List<MesJobOrder> processCtrols = wsPLM.UseService(s =>
                    s.GetMesJobOrders(
                        $"LINE_TASK_PKNO = '{LineTaskNO}' AND USE_FLAG = 1  AND LINE_PKNO = '{CBaseData.CurLinePKNO}'"))
                .ToList();
            int qty = 0;
            int count = 0;
            foreach (MesJobOrder item in processCtrols)
            {
                qty +=int.Parse(item.TASK_QTY.ToString());
                count++;
            }
            if (count==0)
            {
                count = 1;
            }
            qty = qty / count;
            return qty;
        }

        public static void FinishProcessPrepare(PmTaskLine taskLine, List<MesProcessCtrol> processCtrols)
        {
            WcfClient<IPLMService> wsPLM = new WcfClient<IPLMService>(); //计划

            #region 更新任务状态

            if (taskLine.RUN_STATE == 0)
            {
                taskLine.RUN_STATE = 1;
                taskLine.UPDATED_BY = CBaseData.LoginName;
                taskLine.LAST_UPDATE_DATE = DateTime.Now;
                wsPLM.UseService(s => s.UpdatePmTaskLine(taskLine));
            }

            #endregion

            foreach (MesProcessCtrol process in processCtrols)
            {
                process.PKNO = CBaseData.NewGuid();
                process.COMPANY_CODE = "";
                process.CREATED_BY = CBaseData.LoginName;
                process.CREATION_DATE = DateTime.Now;
                process.LAST_UPDATE_DATE = DateTime.Now;  //最后修改日期
                process.USE_FLAG = 1;
                wsPLM.UseService(s => s.AddMesProcessCtrol(process));
            }
        }

        public static bool AutoFinishProcessPrepare(PmTaskLine taskLine, out string error)
        {
            error = "";
            return true;
        }

        private void gridProcessInfo_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
        }

        private void ButtonEditSettings_OnDefaultButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            RsRoutingDetail routingDetail = gridProcessInfo.SelectedItem as RsRoutingDetail;
            if (routingDetail != null)
            {
                DeviceSelect deviceSelect = new DeviceSelect(routingDetail);
                deviceSelect.Closed += DeviceSelect_Closed;
                deviceSelect.Show();
            }
        }

        private void DeviceSelect_Closed(object sender, EventArgs e)
        {
            DeviceSelect deviceSelect = sender as DeviceSelect;
            AmAssetMasterN amAssetMaster = deviceSelect.Tag as AmAssetMasterN;
            RsRoutingDetail routingDetail = gridProcessInfo.SelectedItem as RsRoutingDetail;
            if (routingDetail != null && amAssetMaster != null)
            {
                if (routingDetail.WC_ABV != amAssetMaster.ASSET_CODE)
                {
                    routingDetail.WC_ABV = amAssetMaster.ASSET_CODE;
                    routingDetail.OP_TYPE = amAssetMaster.ASSET_NAME;
                }
            }
        }

        private void ProcessAction_OnDefaultButtonClick(object sender, System.Windows.RoutedEventArgs e)
        { //选择指令动作
            RsRoutingDetail routingDetail = gridProcessInfo.SelectedItem as RsRoutingDetail;
            if (routingDetail != null)
            {
                if (string.IsNullOrEmpty(routingDetail.WC_ABV))
                {
                    MessageBox.Show("请先确定加工设备!", "选择指令动作", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                ProcessActionSetting actionSelect = new ProcessActionSetting(routingDetail, routingDetail.OP_TYPE);
                actionSelect.Closed += ActionSelectOnClosed;
                actionSelect.Show();
            }
        }

        private void ActionSelectOnClosed(object sender, EventArgs eventArgs)
        {
            ProcessActionSetting actionSelect = sender as ProcessActionSetting;
            RsRoutingDetail routingDetail = gridProcessInfo.SelectedItem as RsRoutingDetail;
            if ((actionSelect != null) && (routingDetail != null))
            {
                routingDetail.PROCESS_ACTION_TYPE = actionSelect.RoutingDetail.PROCESS_ACTION_TYPE;
                routingDetail.PROCESS_ACTION_PKNO = actionSelect.RoutingDetail.PROCESS_ACTION_PKNO;
                routingDetail.PROCESS_ACTION_PARAM1_VALUE = actionSelect.RoutingDetail.PROCESS_ACTION_PARAM1_VALUE;
                routingDetail.PROCESS_ACTION_PARAM2_VALUE = actionSelect.RoutingDetail.PROCESS_ACTION_PARAM2_VALUE;
                routingDetail.REMARK = actionSelect.RoutingDetail.REMARK;
            }
        }
    }
}
