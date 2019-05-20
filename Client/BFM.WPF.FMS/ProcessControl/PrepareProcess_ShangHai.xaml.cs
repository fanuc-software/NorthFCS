using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
using BFM.Server.DataAsset.FMSService;
using BFM.WPF.FMS.BasicData;
using ComboBox = System.Windows.Controls.ComboBox;
using MessageBox = System.Windows.Forms.MessageBox;

namespace BFM.WPF.FMS.ProcessControl
{
    /// <summary>
    /// PrepareProcess_FuZhou.xaml 的交互逻辑
    /// </summary>
    public partial class PrepareProcess_ShangHai : Page
    {
        private WcfClient<IPLMService> wsPLM = new WcfClient<IPLMService>(); //计划
        private WcfClient<IRSMService> wsRSM = new WcfClient<IRSMService>(); //工艺资源
        private WcfClient<IEAMService> wsEAM = new WcfClient<IEAMService>(); //设备
        private WcfClient<ITMSService> wsTMS = new WcfClient<ITMSService>(); //刀具
        private WcfClient<IFMSService> wsFMS = new WcfClient<IFMSService>();

        public PrepareProcess_ShangHai()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (cmbTask.SelectedItem == null)
            {
                cmbTask.ItemsSource =
                    wsPLM.UseService(s => s.GetPmTaskLines("USE_FLAG = 1 AND RUN_STATE < 10 ")) //未完成的
                        .Where(c => c.TASK_QTY > c.PREPARED_QTY)
                        .OrderBy(c => c.CREATION_DATE)
                        .ToList(); //生产线任务
            }

            List<AmAssetMasterN> assets = wsEAM.UseService(s => s.GetAmAssetMasterNs($"USE_FLAG = {(int)EmUseFlag.Useful}"));
            List<FmsAssetCommParam> assetComm =
                wsFMS.UseService(s => s.GetFmsAssetCommParams($"USE_FLAG != {(int)EmUseFlag.Deleted} AND INTERFACE_TYPE = 2"));  //OPC通讯

            var assets2 = from c in assets
                          join d in assetComm on c.ASSET_CODE equals d.ASSET_CODE
                          select c;
            cmbAssetInfo.ItemsSource = assets2;
            if (cmbAssetInfo.Items.Count == 1)
            {
                cmbAssetInfo.SelectedIndex = 0;
            }
        }

        private void GetPage()
        {
            gridProcessInfo.ItemsSource = null;

            PmTaskLine taskLine = cmbTask.SelectedItem as PmTaskLine;
            if (taskLine == null) return;

            string itemPKNO = cmbProduct.SelectedValue.ToString();
            if (string.IsNullOrEmpty(itemPKNO)) return; //产品
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
            if (cmbAssetInfo.Items.Count == 1) cmbAssetInfo.SelectedIndex = 0;  //PLC
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
                wsPLM.UseService(s => s.GetMesJobOrders($"LINE_TASK_PKNO = '{taskLine.PKNO}' AND USE_FLAG = 1"))
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
                detail.OP_TYPE = ""; //临时存放 Asset Code 用
                detail.REMARK = ""; //临时存放 指令动作名称 用
                //提取设备，唯一的设备
                List<AmAssetMasterN> assets =
                    wsEAM.UseService(s => s.GetAmAssetMasterNs($"ASSET_GROUP = '{detail.WC_CODE}'"));
                AmAssetMasterN assetMaster = assets.FirstOrDefault();
                if ((assetMaster != null) && (assets.Count == 1))
                {
                    detail.WC_ABV = assetMaster.ASSET_CODE;
                    detail.OP_TYPE = assetMaster.ASSET_NAME;
                }
            }
            gridProcessInfo.ItemsSource = rsRoutingDetails;
           
        }

        #endregion

        private void bSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (cmbRoutingMain.SelectedValue == null) return;

            #region 创建生产过程

            //生产线数据
            PmTaskLine taskLine = cmbTask.SelectedItem as PmTaskLine;
            //产品数据
            string itemPKNO = cmbProduct.SelectedValue.ToString();
            RsItemMaster product = wsRSM.UseService(s => s.GetRsItemMasterById(itemPKNO));
            if (product == null)
            {
                MessageBox.Show("产品信息不存在，请核实.", "完成生产准备", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
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

            AmAssetMasterN mainDevice = cmbAssetInfo.SelectedItem as AmAssetMasterN;

            if (mainDevice == null)
            {
                MessageBox.Show("请选择主控PLC", "完成生产准备", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            #region 创建工单表

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
                USE_FLAG = 1,
                REMARK = "",
            };

            #endregion

            List<MesProcessCtrol> newMesProcessCtrols = new List<MesProcessCtrol>();

            int iProcessIndex = 0;

            string DeviceInfos = "";
            string Programs = "";

            #region 获取加工设备、程序号

            foreach (RsRoutingDetail item in rsRoutingDetails)
            {
                if (string.IsNullOrEmpty(item.WC_ABV))
                {
                    MessageBox.Show($"工序【{item.OP_NAME}】加工设备不能为空，请选择加工设备!", 
                        "完成生产准备", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                AmAssetMasterN device = wsEAM.UseService(s => s.GetAmAssetMasterNById(item.WC_ABV));
                if (device == null)
                {
                    MessageBox.Show($"工序【{item.OP_NAME}】加工设备，请选择加工设备!",
                        "完成生产准备", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                DeviceInfos += device.ASSET_LABEL;  //设备
                Programs += item.NC_PRO_NAME;       //程序号
            }

           #endregion

            wsPLM.UseService(s => s.AddMesJobOrder(jobOrder));  //添加工单

            #region 添加加工工序，共5步

            FmsActionControl ctrl1 =
                wsFMS.UseService(s => s.GetFmsActionControls($"ASSET_CODE = '{mainDevice.ASSET_CODE}' AND ACTION_NAME = '发送产品信息'"))
                    .FirstOrDefault();  //
            FmsActionControl ctrl2 =
                wsFMS.UseService(s => s.GetFmsActionControls($"ASSET_CODE = '{mainDevice.ASSET_CODE}' AND ACTION_NAME = '发送数量'"))
                    .FirstOrDefault();  //
            FmsActionControl ctrl3 =
                wsFMS.UseService(s => s.GetFmsActionControls($"ASSET_CODE = '{mainDevice.ASSET_CODE}' AND ACTION_NAME = '发送设备及程序号'"))
                    .FirstOrDefault();  //
            FmsActionControl ctrl4 =
                wsFMS.UseService(s => s.GetFmsActionControls($"ASSET_CODE = '{mainDevice.ASSET_CODE}' AND ACTION_NAME = '订单确认'"))
                    .FirstOrDefault();  //
            FmsActionControl ctrl5 =
                wsFMS.UseService(s => s.GetFmsActionControls($"ASSET_CODE = '{mainDevice.ASSET_CODE}' AND ACTION_NAME = '订单完成'"))
                    .FirstOrDefault();  //

            #region 1.第一步发送产品信息

            MesProcessCtrol step1 = new MesProcessCtrol()
            {
                PKNO = CBaseData.NewGuid(),
                COMPANY_CODE = "",
                JOB_ORDER_PKNO = jobOrder.PKNO,
                JOB_ORDER = cmbTask.Text + lbBatchIndex.Content, //工单编号
                ITEM_PKNO = jobOrder.ITEM_PKNO,  //产品PKNO
                SUB_JOB_ORDER_NO = "",
                ROUTING_DETAIL_PKNO = "", //工序编号（工艺路线明细）为空，不按照工艺进行控制
                PROCESS_DEVICE_PKNO = mainDevice.PKNO, //加工设备为 主控PLC
                PROCESS_INDEX = 0,
                PROCESS_ACTION_TYPE = 0,
                PROCESS_ACTION_PKNO = (ctrl1 == null) ? "" : ctrl1.PKNO,
                PROCESS_ACTION_PARAM1_VALUE = product.ITEM_ABV, //产品简称
                PROCESS_ACTION_PARAM2_VALUE = "",
                CUR_PRODUCT_CODE_PKNO = "",
                PROCESS_QTY = preparedQty,
                COMPLETE_QTY = 0,
                QUALIFIED_QTY = 0,
                PROCESS_STATE = 1,
                CREATION_DATE = DateTime.Now,
                CREATED_BY = CBaseData.LoginName,
                USE_FLAG = 1,
                REMARK = "系统自动形成",
            };

            #endregion end 1

            #region 2.第二步发送数量

            MesProcessCtrol step2 = new MesProcessCtrol()
            {
                PKNO = CBaseData.NewGuid(),
                COMPANY_CODE = "",
                JOB_ORDER_PKNO = jobOrder.PKNO,
                JOB_ORDER = cmbTask.Text + lbBatchIndex.Content, //工单编号
                ITEM_PKNO = jobOrder.ITEM_PKNO,  //产品PKNO
                SUB_JOB_ORDER_NO = "",
                ROUTING_DETAIL_PKNO = "", //工序编号（工艺路线明细）为空，不按照工艺进行控制
                PROCESS_DEVICE_PKNO = mainDevice.PKNO, //加工设备为 主控PLC
                PROCESS_INDEX = 1,
                PROCESS_ACTION_TYPE = 0,
                PROCESS_ACTION_PKNO = (ctrl2 == null) ? "" : ctrl2.PKNO,
                PROCESS_ACTION_PARAM1_VALUE = preparedQty.ToString(), //数量
                PROCESS_ACTION_PARAM2_VALUE = "",
                CUR_PRODUCT_CODE_PKNO = "",
                PROCESS_QTY = preparedQty,
                COMPLETE_QTY = 0,
                QUALIFIED_QTY = 0,
                PROCESS_STATE = 1,
                CREATION_DATE = DateTime.Now,
                CREATED_BY = CBaseData.LoginName,
                USE_FLAG = 1,
                REMARK = "系统自动形成",
            };

            #endregion end 2

            #region 3.第三步发送设备及程序号

            MesProcessCtrol step3 = new MesProcessCtrol()
            {
                PKNO = CBaseData.NewGuid(),
                COMPANY_CODE = "",
                JOB_ORDER_PKNO = jobOrder.PKNO,
                JOB_ORDER = cmbTask.Text + lbBatchIndex.Content, //工单编号
                ITEM_PKNO = jobOrder.ITEM_PKNO,  //产品PKNO
                SUB_JOB_ORDER_NO = "",
                ROUTING_DETAIL_PKNO = "", //工序编号（工艺路线明细）为空，不按照工艺进行控制
                PROCESS_DEVICE_PKNO = mainDevice.PKNO, //加工设备为 主控PLC
                PROCESS_INDEX = 2,
                PROCESS_ACTION_TYPE = 0,
                PROCESS_ACTION_PKNO = (ctrl3 == null) ? "" : ctrl3.PKNO,
                PROCESS_ACTION_PARAM1_VALUE = DeviceInfos, //设备ID
                PROCESS_ACTION_PARAM2_VALUE = Programs,    //程序号
                CUR_PRODUCT_CODE_PKNO = "",
                PROCESS_QTY = preparedQty,
                COMPLETE_QTY = 0,
                QUALIFIED_QTY = 0,
                PROCESS_STATE = 1,
                CREATION_DATE = DateTime.Now,
                CREATED_BY = CBaseData.LoginName,
                USE_FLAG = 1,
                REMARK = "系统自动形成",
            };

            #endregion end 3

            #region 4.第四步发送订单确认

            MesProcessCtrol step4 = new MesProcessCtrol()
            {
                PKNO = CBaseData.NewGuid(),
                COMPANY_CODE = "",
                JOB_ORDER_PKNO = jobOrder.PKNO,
                JOB_ORDER = cmbTask.Text + lbBatchIndex.Content, //工单编号
                ITEM_PKNO = jobOrder.ITEM_PKNO,  //产品PKNO
                SUB_JOB_ORDER_NO = "",
                ROUTING_DETAIL_PKNO = "", //工序编号（工艺路线明细）为空，不按照工艺进行控制
                PROCESS_DEVICE_PKNO = mainDevice.PKNO, //加工设备为 主控PLC
                PROCESS_INDEX = 3,
                PROCESS_ACTION_TYPE = 0,
                PROCESS_ACTION_PKNO = (ctrl4 == null) ? "" : ctrl4.PKNO,
                PROCESS_ACTION_PARAM1_VALUE = cmbTask.Text + lbBatchIndex.Content, //订单ID
                PROCESS_ACTION_PARAM2_VALUE = "",    //
                CUR_PRODUCT_CODE_PKNO = "",
                PROCESS_QTY = preparedQty,
                COMPLETE_QTY = 0,
                QUALIFIED_QTY = 0,
                PROCESS_STATE = 1,
                CREATION_DATE = DateTime.Now,
                CREATED_BY = CBaseData.LoginName,
                USE_FLAG = 1,
                REMARK = "系统自动形成",
            };

            #endregion end 4

            #region 5.第五步订单完成

            MesProcessCtrol step5 = new MesProcessCtrol()
            {
                PKNO = CBaseData.NewGuid(),
                COMPANY_CODE = "",
                JOB_ORDER_PKNO = jobOrder.PKNO,
                JOB_ORDER = cmbTask.Text + lbBatchIndex.Content, //工单编号
                ITEM_PKNO = jobOrder.ITEM_PKNO,  //产品PKNO
                SUB_JOB_ORDER_NO = "",
                ROUTING_DETAIL_PKNO = "", //工序编号（工艺路线明细）为空，不按照工艺进行控制
                PROCESS_DEVICE_PKNO = mainDevice.PKNO, //加工设备为 主控PLC
                PROCESS_INDEX = 4,
                PROCESS_ACTION_TYPE = 0,
                PROCESS_ACTION_PKNO = (ctrl5 == null) ? "" : ctrl5.PKNO,
                PROCESS_ACTION_PARAM1_VALUE = "", //
                PROCESS_ACTION_PARAM2_VALUE = "",    //
                CUR_PRODUCT_CODE_PKNO = "",
                PROCESS_QTY = preparedQty,
                COMPLETE_QTY = 0,
                QUALIFIED_QTY = 0,
                PROCESS_STATE = 1,
                CREATION_DATE = DateTime.Now,
                CREATED_BY = CBaseData.LoginName,
                USE_FLAG = 1,
                REMARK = "系统自动形成",
            };

            #endregion end 5

            wsPLM.UseService(s => s.AddMesProcessCtrol(step1));
            wsPLM.UseService(s => s.AddMesProcessCtrol(step2));
            wsPLM.UseService(s => s.AddMesProcessCtrol(step3));
            wsPLM.UseService(s => s.AddMesProcessCtrol(step4));
            wsPLM.UseService(s => s.AddMesProcessCtrol(step5));

            #endregion

            #endregion

            //修改产线任务的完成数量
            taskLine.PREPARED_QTY += preparedQty;
            if (taskLine.RUN_STATE == 0) taskLine.RUN_STATE = 1;
            wsPLM.UseService(s => s.UpdatePmTaskLine(taskLine));

            cmbTask.ItemsSource =
                wsPLM.UseService(s => s.GetPmTaskLines("USE_FLAG = 1 AND RUN_STATE < 10 ")) //未完成的
                    .Where(c => c.TASK_QTY > c.PREPARED_QTY)
                    .OrderBy(c => c.CREATION_DATE)
                    .ToList(); //生产线任务

            cmbTask.SelectedIndex = -1;
            gridProcessInfo.ItemsSource = null;
        }
        /// <summary>
        /// 判断加工数量
        /// </summary>
        /// <returns></returns>
        public int CheckPlanQTY(string LineTaskNO)
        {
            List<MesJobOrder> processCtrols = wsPLM.UseService(s => s.GetMesJobOrders($"LINE_TASK_PKNO = '{LineTaskNO}' AND USE_FLAG = 1")).ToList();
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
