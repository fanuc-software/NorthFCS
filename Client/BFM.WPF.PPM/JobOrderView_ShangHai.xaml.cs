using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.EAMService;
using BFM.Server.DataAsset.FMSService;
using BFM.Server.DataAsset.PLMService;
using BFM.Server.DataAsset.RSMService;
using BFM.Server.DataAsset.TMSService;
using DevExpress.Data.Helpers;
using DevExpress.Xpf.Editors;

namespace BFM.WPF.PPM
{
    /// <summary>
    /// JobOrderView_ShangHai.xaml 的交互逻辑
    /// </summary>
    public partial class JobOrderView_ShangHai : Page
    {
        private WcfClient<IPLMService> wsPLM = new WcfClient<IPLMService>(); //计划
        private WcfClient<IRSMService> wsRSM = new WcfClient<IRSMService>(); //工艺资源
        private WcfClient<IEAMService> wsEAM = new WcfClient<IEAMService>(); //设备
        private WcfClient<ITMSService> wsTMS = new WcfClient<ITMSService>(); //刀具
        private WcfClient<IFMSService> wsFMS = new WcfClient<IFMSService>();
        private RsRoutingItem mRoutingItem = new RsRoutingItem();
        public JobOrderView_ShangHai()
        {
            InitializeComponent();
        }



        private void ListBoxEdit_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            gridProcessInfo.ItemsSource = null;
            ListBoxEdit lstbox = sender as ListBoxEdit;
            if (lstbox.SelectedIndex == 0)
            {
                txt_name.Text = "16寸轮毂";
                RsRoutingHead mRsRoutingHead = wsRSM.UseService(s => s.GetRsRoutingHeads("ROUTING_ABV = 16LG")).FirstOrDefault();

                if (mRsRoutingHead != null)
                {
                    getRouting(mRsRoutingHead.PKNO);
                    RsRoutingItem mRsRoutingItem = wsRSM.UseService(s => s.GetRsRoutingItems("ROUTING_PKNO = " + mRsRoutingHead.PKNO + "")).FirstOrDefault();
                    if (mRsRoutingItem != null) mRoutingItem = mRsRoutingItem;

                }

            }
            if (lstbox.SelectedIndex == 1)
            {

                txt_name.Text = "17寸轮毂";
                RsRoutingHead mRsRoutingHead = wsRSM.UseService(s => s.GetRsRoutingHeads("ROUTING_ABV = 17LG")).FirstOrDefault();

                if (mRsRoutingHead != null)
                {
                    getRouting(mRsRoutingHead.PKNO);
                    RsRoutingItem mRsRoutingItem = wsRSM.UseService(s => s.GetRsRoutingItems("ROUTING_PKNO = " + mRsRoutingHead.PKNO + "")).FirstOrDefault();
                    if (mRsRoutingItem != null) mRoutingItem = mRsRoutingItem;

                }
            }
            if (lstbox.SelectedIndex == 2)
            {

                txt_name.Text = "18寸轮毂";
                RsRoutingHead mRsRoutingHead = wsRSM.UseService(s => s.GetRsRoutingHeads("ROUTING_ABV = 18LG")).FirstOrDefault();

                if (mRsRoutingHead != null)
                {
                    getRouting(mRsRoutingHead.PKNO);
                    RsRoutingItem mRsRoutingItem = wsRSM.UseService(s => s.GetRsRoutingItems("ROUTING_PKNO = " + mRsRoutingHead.PKNO + "")).FirstOrDefault();
                    if (mRsRoutingItem != null) mRoutingItem = mRsRoutingItem;

                }
            }

        }

        private void getRouting(string routingMainPKNO)
        {
            //选择工艺路线 => 获取工艺路线



            if (string.IsNullOrEmpty(routingMainPKNO))
            {
                return;
            }

            List<RsRoutingDetail> m_rsRoutingDetails =
                wsRSM.UseService(s => s.GetRsRoutingDetails($"ROUTING_PKNO = '{routingMainPKNO}' AND USE_FLAG = 1"))
                    .OrderBy(c => c.OP_INDEX)
                    .ToList();

            foreach (RsRoutingDetail detail in m_rsRoutingDetails)
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
            gridProcessInfo.ItemsSource = m_rsRoutingDetails;

        }

        private void bSave_Click(object sender, RoutedEventArgs e)
        {
            int m_qty = 0;
            int orderQty = wsPLM.UseService(s =>
                s.GetMesJobOrderCount(
                    $"CREATION_DATE > '{DateTime.Today.ToString("yyyy-MM-dd")}' AND LINE_PKNO = '{CBaseData.CurLinePKNO}'"));
            if (orderQty==0)orderQty = 1;
            if (qty.Text != null && int.TryParse(qty.Text, out m_qty) && m_qty != 0)
            {
                MesJobOrder jobOrder = new MesJobOrder()
                {
                    PKNO = Guid.NewGuid().ToString("N"),
                    COMPANY_CODE = "",
                    LINE_PKNO = "",  //产线信息
                    LINE_TASK_PKNO = "",
                    ITEM_PKNO = mRoutingItem.ITEM_PKNO, 
                    JOB_ORDER_NO = DateTime.Now.ToString("yyMMdd")+ orderQty.ToString("0000"),
                    BATCH_NO = "",
                    ROUTING_DETAIL_PKNO = mRoutingItem.ROUTING_PKNO,//该字段为routing_head_PKNO
                    TASK_QTY = m_qty,
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
            }
            else
            {
                MessageBox.Show("请核实订单数量和产品信息");
            }
        }
    }
}
