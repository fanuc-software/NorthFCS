using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.PLMService;
using BFM.WPF.Base.Helper;

namespace BFM.WPF.FMS
{
    /// <summary>
    /// ConfirmProductionView.xaml 的交互逻辑
    /// </summary>
    public partial class ConfirmProductionView : Page
    {

        private WcfClient<IPLMService> ws = new WcfClient<IPLMService>();
        private String OrderNo;
        public ConfirmProductionView()
        {
            InitializeComponent();
            GetPage();
        }

        private void GetPage()
        {
            List<MesJobOrder> taskLines = ws.UseService(s =>
                s.GetMesJobOrders(
                    $"RUN_STATE >= 1 AND RUN_STATE < 10 AND USE_FLAG = 1 AND LINE_PKNO = '{CBaseData.CurLinePKNO}' "));
            gridItem.ItemsSource = taskLines;

            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }

        private void txt_orderNO_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OrderNo = txt_orderNO.Text.Trim();
                txt_orderNO.Text = "";
                OrderNoStateChange();
                //System.Windows.Forms.MessageBox.Show("任务"+ OrderNo + "已开工");
                GetPage();
            }

        }
        /// <summary>
        /// 订单状态变更
        /// </summary>
        public void OrderNoStateChange()
        {
            //检验刀具是否都准备完成

            List<MesJobOrder> taskLines = ws.UseService(s => s.GetMesJobOrders($"JOB_ORDER_NO = '{OrderNo}' AND LINE_PKNO = '{CBaseData.CurLinePKNO}' "));
            if (taskLines!=null&&taskLines.Count!=0)
            {
                taskLines[0].RUN_STATE = 10;  //开工确认
                ws.UseService(s => s.UpdateMesJobOrder(taskLines[0]));
            }

        }
    }
}
