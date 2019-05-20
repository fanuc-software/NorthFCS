using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Forms;
using BFM.Common.Base;
using BFM.Common.Base.Helper;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.Data.PubData;
using BFM.Server.DataAsset.EAMService;
using BFM.Server.DataAsset.PLMService;
using BFM.Server.DataAsset.RSMService;
using BFM.Server.DataAsset.TMSService;
using BFM.ContractModel;

namespace BFM.WPF.FMS.ProcessControl
{
    /// <summary>
    /// PrepareProcess.xaml 的交互逻辑
    /// </summary>
    public partial class ToolsPrepare : Page
    {
        private WcfClient<IPLMService> wsPLM = new WcfClient<IPLMService>(); //计划
        private WcfClient<IRSMService> wsRSM = new WcfClient<IRSMService>(); //工艺资源
        private WcfClient<IEAMService> wsEAM = new WcfClient<IEAMService>(); //设备
        private WcfClient<ITMSService> wsTMS = new WcfClient<ITMSService>(); //刀具

        public ToolsPrepare()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            cmbTask.ItemsSource =
                wsPLM.UseService(s => s.GetMesJobOrders($"USE_FLAG = 1 AND RUN_STATE = 1 AND LINE_PKNO = '{CBaseData.CurLinePKNO}'")) //生产准备完成 尚未执行的任务
                    .OrderBy(c => c.CREATION_DATE)
                    .ToList(); //工单

            GetPage();
        }

        private void GetPage()
        {
            gridToolsRequst.ItemsSource = null;
            gridToolsInv.ItemsSource = null;
            gridToolsFinal.ItemsSource = null;

            MesJobOrder jobOrder = cmbTask.SelectedItem as MesJobOrder;
            if (jobOrder == null) return;

            string itemPKNO = cmbProduct.SelectedValue.ToString();
            if (string.IsNullOrEmpty(itemPKNO)) return; //产品

            string processPKNO = cmbProcess.SelectedValue.ToString();
            RsRoutingDetail routingDetail = wsRSM.UseService(s => s.GetRsRoutingDetailById(processPKNO));
            if (routingDetail == null) return;

            List<RsRoutingTools> requsts = wsRSM.UseService(
                s => s.GetRsRoutingToolss($"USE_FLAG = 1 AND ROUTING_DETAIL_PKNO = '{routingDetail.PKNO}'"));

            gridToolsRequst.ItemsSource = requsts;

            AmAssetMasterN wcDevice = cmbDevice.SelectedItem as AmAssetMasterN;
            if (wcDevice == null) return;

            #region 机床刀具清单

            var devcieTools =
                wsTMS.UseService(s => s.GetTmsDeviceToolsPoss($"USE_FLAG = 1 AND DEVICE_PKNO = '{wcDevice.PKNO}'"))
                    .OrderBy(c => c.TOOLS_POS_NO)
                    .Select(c => new
                    {
                        c.PKNO,
                        c.DEVICE_PKNO,
                        c.TOOLS_POS_NO,
                        c.POS_INTROD,
                        c.TOOLS_STATE,
                        TOOLS_PKNO = (c.TOOLS_STATE == 10 || c.TOOLS_STATE == 2) ? c.NEW_TOOLS_PKNO : c.TOOLS_PKNO,
                        c.REMARK,
                    })
                    .ToList();
            gridToolsFinal.ItemsSource = devcieTools;

            #endregion

            #region 刀具需求

            List<TmsToolsMaster> toolsMasters = wsTMS.UseService(s => s.GetTmsToolsMasters("USE_FLAG = 1"));
            var allToolTypes = (from c in devcieTools
                join d in toolsMasters on c.TOOLS_PKNO equals d.PKNO
                select new {d.TOOLS_TYPE_PKNO, c.TOOLS_STATE})
                .GroupBy(c => c.TOOLS_TYPE_PKNO)
                .Select(a => new {TOOLS_TYPE_PKNO = a.Key, TOOLS_STATE = a.Min(c => c.TOOLS_STATE)}); //选择所有在库或已排入的刀

            var newRequest = from c in requsts
                join d in allToolTypes on c.TOOLS_TYPE_PKNO equals d.TOOLS_TYPE_PKNO into temp
                from tt in temp.DefaultIfEmpty()
                select new
                {
                    c.TOOLS_TYPE_PKNO,
                    c.TOOLS_NC_CODE,
                    REQUEST = (tt == null) ? "未排刀" : (tt.TOOLS_STATE == 1 ? "在刀位" : "已排入"),
                    c.INTROD,
                };

            gridToolsRequst.ItemsSource = newRequest;

            #region 按钮可用性

            

            #endregion

            #endregion
            
        }

        #region 获取查询条件

        private void cmbTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbProduct.SelectedIndex = -1;
            cmbProcess.SelectedIndex = -1;
            cmbDevice.SelectedIndex = -1;

            cmbProduct.ItemsSource = null;
            cmbProcess.ItemsSource = null;
            cmbDevice.ItemsSource = null;

            //选择生产线 => 获取产品
            MesJobOrder jobOrder = cmbTask.SelectedItem as MesJobOrder;
            if (jobOrder == null)
            {
                return;
            }

            cmbProduct.ItemsSource = wsRSM.UseService(s => s.GetRsItemMasters($"USE_FLAG = 1 AND PKNO = '{jobOrder.ITEM_PKNO}'"));

            if (cmbProduct.Items.Count > 0) cmbProduct.SelectedIndex = 0; //选择第一个
        }

        private void cmbProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //选择产品 => 获取工艺路线
            cmbProcess.SelectedIndex = -1;
            cmbDevice.SelectedIndex = -1;
            cmbProcess.ItemsSource = null;
            cmbDevice.ItemsSource = null;


            if (cmbProduct.SelectedValue == null) return;
            MesJobOrder jobOrder = cmbTask.SelectedItem as MesJobOrder;
            if (jobOrder == null)
            {
                return;
            }
            string itemPKNO = cmbProduct.SelectedValue.ToString();

            if (string.IsNullOrEmpty(itemPKNO))
            {
                return;
            }

            cmbProcess.ItemsSource =
                wsPLM.UseService(s => s.GetMesProcessCtrols($"USE_FLAG = 1 AND JOB_ORDER_PKNO = '{jobOrder.PKNO}' AND ITEM_PKNO = '{itemPKNO}' "))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .Join(wsRSM.UseService(s => s.GetRsRoutingDetails("USE_FLAG = 1")).OrderBy(c => c.OP_INDEX), c => c.ROUTING_DETAIL_PKNO,
                        d => d.PKNO,
                        (c, d) => new {d.PKNO, d.ROUTING_PKNO, OP_INFO = d.OP_NO + " " + d.OP_NAME});

            //需增加状态表，已经完成准备的工序不再显示
            if (cmbProcess.Items.Count > 0) cmbProcess.SelectedIndex = 0; //选择第一个
        }

        private void cmbProcess_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //选择工序 => 获取设备
            cmbDevice.SelectedIndex = -1;
            cmbDevice.ItemsSource = null;

            if (cmbProcess.SelectedValue == null) return;
            if (cmbProduct.SelectedValue == null) return;
            MesJobOrder jobOrder = cmbTask.SelectedItem as MesJobOrder;
            if (jobOrder == null)
            {
                return;
            }

            string processPKNO = cmbProcess.SelectedValue.ToString();
            MesProcessCtrol processCtrol = wsPLM
                .UseService(s =>
                    s.GetMesProcessCtrols(
                        $"USE_FLAG = 1 AND JOB_ORDER_PKNO = '{jobOrder.PKNO}' AND ROUTING_DETAIL_PKNO = '{processPKNO}'"))
                .OrderBy(c => c.PROCESS_INDEX).FirstOrDefault();

            if (processCtrol == null)
            {
                return;
            }

            cmbDevice.ItemsSource =
                wsEAM.UseService(s => s.GetAmAssetMasterNs($"USE_FLAG = 1 AND ASSET_CODE = '{processCtrol.PROCESS_DEVICE_PKNO}'"));

            if (cmbDevice.Items.Count > 0) cmbDevice.SelectedIndex = 0; //选择第一个

            GetPage();
        }

        #endregion
        
        #region 刀具准备

        //查询库存
        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //查询刀具库存
            gridToolsInv.ItemsSource = null;
            string toolsTypePKNO = tbToolsType.Text;
            if (string.IsNullOrEmpty(toolsTypePKNO)) return;

            gridToolsInv.ItemsSource =
                wsTMS.UseService(
                    s =>
                        s.GetTmsToolsMasters(
                            $"USE_FLAG = 1 AND TOOLS_POSITION = 1 AND TOOLS_TYPE_PKNO = '{toolsTypePKNO}'"));
        }

        //排刀
        private void bToolsBindDevice_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //排刀
            string detailPKNO = SafeConverter.SafeToStr(ClassHelper.GetPropertyValue(gridToolsFinal.SelectedItem, "PKNO"));

            TmsDeviceToolsPos deviceToolsPos = wsTMS.UseService(s => s.GetTmsDeviceToolsPosById(detailPKNO));
            if (deviceToolsPos == null)
            {
                return;
            }
            TmsToolsMaster toolsMaster = gridToolsInv.SelectedItem as TmsToolsMaster;
            if (toolsMaster == null)
            {
                MessageBox.Show($"请选择相应的库存刀具！", "排入刀具", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TmsDeviceToolsPos check =
                wsTMS
                    .UseService(s => s.GetTmsDeviceToolsPoss($"USE_FLAG = 1 AND DEVICE_PKNO = '{deviceToolsPos.DEVICE_PKNO}'"))
                    .FirstOrDefault(c => c.TOOLS_PKNO == toolsMaster.PKNO || c.NEW_TOOLS_PKNO == toolsMaster.PKNO);

            #region 排刀

            if (deviceToolsPos.TOOLS_STATE == 0) //空刀位
            {
                if (check != null)
                {
                    MessageBox.Show($"该刀具[{toolsMaster.TOOLS_CODE}]已经是{(check.TOOLS_STATE == 1 ? "在位" : "已排入")}状态，不能再排入！", "排入刀具",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                deviceToolsPos.TOOLS_STATE = 2; //已排入
                //deviceToolsPos.TOOLS_PKNO = toolsMaster.PKNO;
                deviceToolsPos.NEW_TOOLS_PKNO = toolsMaster.PKNO;
                deviceToolsPos.UPDATED_BY = CBaseData.LoginName;
                deviceToolsPos.LAST_UPDATE_DATE = DateTime.Now;
                deviceToolsPos.UPDATED_INTROD += "【" + DateTime.Now + "】排入刀具. ";
                wsTMS.UseService(s => s.UpdateTmsDeviceToolsPos(deviceToolsPos));
            }
            else if (deviceToolsPos.TOOLS_STATE == 1) //刀具在位
            {
                MessageBox.Show($"刀位[{deviceToolsPos.TOOLS_POS_NO}]不为空，不能再排入刀。", "排入刀具", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }
            else if (deviceToolsPos.TOOLS_STATE == 2) //已排入
            {
                if (MessageBox.Show($"刀位[{deviceToolsPos.TOOLS_POS_NO}]已经是排入状态，确定要！取消排入！吗？", "排入刀具",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) return;

                deviceToolsPos.TOOLS_STATE = 0; //=>空
                //deviceToolsPos.TOOLS_PKNO = "";
                deviceToolsPos.NEW_TOOLS_PKNO = "";
                deviceToolsPos.UPDATED_BY = CBaseData.LoginName;
                deviceToolsPos.LAST_UPDATE_DATE = DateTime.Now;
                deviceToolsPos.UPDATED_INTROD += "【" + DateTime.Now + "】取消排入. ";
                wsTMS.UseService(s => s.UpdateTmsDeviceToolsPos(deviceToolsPos));
            }
            else if (deviceToolsPos.TOOLS_STATE == 2) //已经移除 => 更换
            {
                if (check != null)
                {
                    MessageBox.Show($"该刀具[{toolsMaster.TOOLS_CODE}]已经是{(check.TOOLS_STATE == 1 ? "在位" : "已排入")}状态，不能再排入！", "排入刀具",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                deviceToolsPos.TOOLS_STATE = 10; //=>更换
                deviceToolsPos.NEW_TOOLS_PKNO = toolsMaster.PKNO; //刀具PKNO
                deviceToolsPos.UPDATED_BY = CBaseData.LoginName;
                deviceToolsPos.LAST_UPDATE_DATE = DateTime.Now;
                deviceToolsPos.UPDATED_INTROD += "【" + DateTime.Now + "】取消更换. ";
                wsTMS.UseService(s => s.UpdateTmsDeviceToolsPos(deviceToolsPos));
            }
            else if (deviceToolsPos.TOOLS_STATE == 10)
            {
                if (MessageBox.Show($"刀位[{deviceToolsPos.TOOLS_POS_NO}]已经是更换刀具，确定要！取消更换！吗？", "排入刀具",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) return;

                deviceToolsPos.TOOLS_STATE = 1; //刀具在位
                deviceToolsPos.NEW_TOOLS_PKNO = ""; //清空刀号
                deviceToolsPos.UPDATED_BY = CBaseData.LoginName;
                deviceToolsPos.LAST_UPDATE_DATE = DateTime.Now;
                deviceToolsPos.UPDATED_INTROD += "【" + DateTime.Now + "】取消更换刀具. ";
                wsTMS.UseService(s => s.UpdateTmsDeviceToolsPos(deviceToolsPos));
            }

            #endregion

            GetPage();
        }

        //移除
        private void btnToolsUnBindDevice_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //移除刀
            string detailPKNO = SafeConverter.SafeToStr(ClassHelper.GetPropertyValue(gridToolsFinal.SelectedItem, "PKNO"));

            TmsDeviceToolsPos deviceToolsPos = wsTMS.UseService(s => s.GetTmsDeviceToolsPosById(detailPKNO));
            if (deviceToolsPos == null)
            {
                return;
            }

            #region 移除刀

            if (deviceToolsPos.TOOLS_STATE == 0)
            {
                MessageBox.Show($"刀位[{deviceToolsPos.TOOLS_POS_NO}]为空，不能移除", "移除刀具", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }
            else if (deviceToolsPos.TOOLS_STATE == 1)
            {
                if (MessageBox.Show($"刀位[{deviceToolsPos.TOOLS_POS_NO}]已在刀位，确定要移除吗？", "移除刀具",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) return;

                deviceToolsPos.TOOLS_STATE = 3; //刀具一处
                deviceToolsPos.UPDATED_BY = CBaseData.LoginName;
                deviceToolsPos.LAST_UPDATE_DATE = DateTime.Now;
                deviceToolsPos.UPDATED_INTROD += "【" + DateTime.Now + "】刀具移除. ";
                wsTMS.UseService(s => s.UpdateTmsDeviceToolsPos(deviceToolsPos));

            }
            else if (deviceToolsPos.TOOLS_STATE == 3) //已经移除
            {
                if (MessageBox.Show($"刀位[{deviceToolsPos.TOOLS_POS_NO}]已经是移除状态，确定要！取消移除！吗？", "移除刀具",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) return;

                deviceToolsPos.TOOLS_STATE = 1; //刀具在位
                deviceToolsPos.UPDATED_BY = CBaseData.LoginName;
                deviceToolsPos.LAST_UPDATE_DATE = DateTime.Now;
                deviceToolsPos.UPDATED_INTROD += "【" + DateTime.Now + "】取消移除. ";
                wsTMS.UseService(s => s.UpdateTmsDeviceToolsPos(deviceToolsPos));
            }
            else if (deviceToolsPos.TOOLS_STATE == 2) //已排入
            {
                if (MessageBox.Show($"刀位[{deviceToolsPos.TOOLS_POS_NO}]已排入等待装刀，确定要！取消排入！吗？", "移除刀具",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) return;
                deviceToolsPos.TOOLS_STATE = 0; //刀具为空
                deviceToolsPos.NEW_TOOLS_PKNO = ""; //刀具PKNO

                deviceToolsPos.UPDATED_BY = CBaseData.LoginName;
                deviceToolsPos.LAST_UPDATE_DATE = DateTime.Now;
                deviceToolsPos.UPDATED_INTROD += "【" + DateTime.Now + "】取消排入. ";
                wsTMS.UseService(s => s.UpdateTmsDeviceToolsPos(deviceToolsPos));
            }
            else if (deviceToolsPos.TOOLS_STATE == 10)
            {
                if (MessageBox.Show($"刀位[{deviceToolsPos.TOOLS_POS_NO}]已经是更换刀具，确定要！取消更换！吗？", "移除刀具",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK) return;
                deviceToolsPos.TOOLS_STATE = 1; //刀具在位
                deviceToolsPos.NEW_TOOLS_PKNO = ""; //清空刀号
                deviceToolsPos.UPDATED_BY = CBaseData.LoginName;
                deviceToolsPos.LAST_UPDATE_DATE = DateTime.Now;
                deviceToolsPos.UPDATED_INTROD += "【" + DateTime.Now + "】取消更换刀具. ";
                wsTMS.UseService(s => s.UpdateTmsDeviceToolsPos(deviceToolsPos));
            }

            #endregion

            GetPage();
        }

        //完成
        private void bFinishPrepare_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MesJobOrder jobOrder = cmbTask.SelectedItem as MesJobOrder;
            if (jobOrder == null)
            {
                return;
            }
            string itemPKNO = cmbProduct.SelectedValue.ToString();

            if (string.IsNullOrEmpty(itemPKNO))
            {
                return;
            }

            #region 检验是否排刀

            List<MesProcessCtrol> processCtrols = wsPLM.UseService(s =>
                s.GetMesProcessCtrols(
                    $"USE_FLAG = 1 AND JOB_ORDER_PKNO = '{jobOrder.PKNO}' AND ITEM_PKNO = '{itemPKNO}' "));

            foreach (MesProcessCtrol processCtrol in processCtrols)
            {
                string detailPKNO = processCtrol.ROUTING_DETAIL_PKNO;

                RsRoutingDetail detail = wsRSM.UseService(s => s.GetRsRoutingDetailById(detailPKNO));

                List<RsRoutingTools> requestTools =
                    wsRSM.UseService(
                        s => s.GetRsRoutingToolss($"USE_FLAG = 1 AND ROUTING_DETAIL_PKNO = '{detail.PKNO}'"));

                AmAssetMasterN device = wsEAM.UseService(s =>
                        s.GetAmAssetMasterNs($"USE_FLAG = 1 AND ASSET_CODE = '{processCtrol.PROCESS_DEVICE_PKNO}'"))
                    .FirstOrDefault();
                if (device == null)
                {
                    continue;

                }
                List<TmsDeviceToolsPos> deviceTools =
                    wsTMS.UseService(
                        s =>
                            s.GetTmsDeviceToolsPoss(
                                $"USE_FLAG = 1 AND DEVICE_PKNO = '{device.PKNO}' AND TOOLS_STATE <> 0 AND TOOLS_STATE <> 3"));
                List<TmsToolsMaster> toolsMasters = wsTMS.UseService(s => s.GetTmsToolsMasters("USE_FLAG = 1"));

                var allToolsTypes = from c in deviceTools
                    join d in toolsMasters on ((c.TOOLS_STATE == 10 || c.TOOLS_STATE == 2)
                        ? c.NEW_TOOLS_PKNO
                        : c.TOOLS_PKNO) equals d.PKNO
                    select new {d.TOOLS_TYPE_PKNO}; //选择所有在库或已排入的刀

                var check = from c in requestTools
                    join d in allToolsTypes on c.TOOLS_TYPE_PKNO equals d.TOOLS_TYPE_PKNO into temp
                    from tt in temp.DefaultIfEmpty()
                    where (tt == null)
                    select c;

                if (check.Any())
                {
                    MessageBox.Show($"工序[{detail.OP_NAME}]还有没有排产的刀具，不能完成。", "完成生产准备", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
            }

            #endregion

            jobOrder.RUN_STATE = 2; //完成刀具准备

            jobOrder.UPDATED_BY = CBaseData.LoginName;
            jobOrder.UPDATED_INTROD += DateTime.Now + "完成排刀. ";
            jobOrder.LAST_UPDATE_DATE = DateTime.Now;

            wsPLM.UseService(s => s.UpdateMesJobOrder(jobOrder));

            cmbTask.SelectedIndex = -1;
            cmbTask.ItemsSource = null;
            cmbTask.ItemsSource =
                wsPLM.UseService(s => s.GetMesJobOrders($"USE_FLAG = 1 AND RUN_STATE = 1 AND LINE_PKNO = '{CBaseData.CurLinePKNO}'")) //生产准备完成 尚未执行的任务
                    .OrderBy(c => c.CREATION_DATE)
                    .ToList(); //工单

            GetPage();  //刷新数据
        }

        #endregion
        
    }
}
