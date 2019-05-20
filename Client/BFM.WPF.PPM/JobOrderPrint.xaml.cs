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
using BFM.Server.DataAsset.SDMService;
using BFM.WPF.Report;
using BFM.WPF.Report.PPM;
using BFM.ContractModel;
using BFM.WPF.Base.Helper;

namespace BFM.WPF.PPM
{
    /// <summary>
    /// JobOrderPrint.xaml 的交互逻辑
    /// </summary>
    public partial class JobOrderPrint : Page
    {
        private WcfClient<IPLMService> ws = new WcfClient<IPLMService>();
        private WcfClient<IRSMService> wsRsm = new WcfClient<IRSMService>();
        private WcfClient<ISDMService> wsSdm = new WcfClient<ISDMService>();

        public JobOrderPrint()
        {
            InitializeComponent();

            GetPage();
        }

        private void GetPage()
        {
            List<MesJobOrder> mesJobOrders =
                ws.UseService(s => s.GetMesJobOrders($"USE_FLAG = 1 AND LINE_PKNO = '{CBaseData.CurLinePKNO}'"))
                    .OrderBy(c => c.CREATION_DATE).ToList();
            gridItem.ItemsSource = mesJobOrders;

            BindHelper.SetDictDataBindingGridItem(gbItem, gridItem);
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            //工单打印
            MesJobOrder mesJobOrder = gridItem.SelectedItem as MesJobOrder;
            if (mesJobOrder == null)
            {
                return;
            }
            List<MesJobOrder> mesJobOrders = new List<MesJobOrder>();
            mesJobOrders.Add(mesJobOrder);
            List<RsItemMaster> items = wsRsm.UseService(s => s.GetRsItemMasters("USE_FLAG = 1")); //ITem

            List< RsRoutingItem> routingItem = wsRsm.UseService(s => s.GetRsRoutingItems(""));

            List<RsRoutingHead> routings = wsRsm.UseService(s => s.GetRsRoutingHeads("USE_FLAG = 1"));

            List<SysAttachInfo> attachs = wsSdm.UseService(s => s.GetSysAttachInfos($"BELONGFUNCTION = '工艺管理' AND ISTATE = 1"));

            List< PmTaskLine> taskLines = ws.UseService(s => s.GetPmTaskLines("USE_FLAG = 1"));

            var OrderInfo = from c in mesJobOrders
                join d in items on c.ITEM_PKNO equals d.PKNO
                join j in taskLines on c.LINE_TASK_PKNO equals j.PKNO
                join f in routingItem on d.PKNO equals f.ITEM_PKNO
                join g in routings on f.ROUTING_PKNO equals g.PKNO
                join h in attachs on g.PKNO equals h.FUNCTIONPKNO into temp
                from attach in temp.DefaultIfEmpty()
                select new
                {
                    JobOrderNO = c.JOB_ORDER_NO,  //工单编号
                    RoutingPic = attach?.ATTACHINFO,
                    TaskNO = j.TASK_NO,  //任务号
                    MaterialModel = "", //材料牌号
                    MaterialSize = "", //下料尺寸
                    PartsName = d.ITEM_NAME, //
                    PartsDrawingNO = d.DRAWING_NO, //
                    //PartsImportant = d.KEY_PART_NORM, //
                    //PartsWeight = d.THEORETICAL_WEIGHT, //
                    Remark = c.REMARK, //

                };


            JobOrder report = new JobOrder() {DataSource = OrderInfo};

            DevReportHelper.BindLabel(report, OrderInfo, "cellOrderNO", "JobOrderNO");
            DevReportHelper.BindBarCode(report, OrderInfo, "barCodeInfo", "JobOrderNO");
            DevReportHelper.BindPicture(report, OrderInfo, "picRouting", "RoutingPic");
            DevReportHelper.BindLabel(report, OrderInfo, "cellTaskNO", "TaskNO");
            DevReportHelper.BindLabel(report, OrderInfo, "cellMaterialModel", "MaterialModel");
            DevReportHelper.BindLabel(report, OrderInfo, "cellMaterialSize", "MaterialSize");
            DevReportHelper.BindLabel(report, OrderInfo, "cellPartsName", "PartsName");
            DevReportHelper.BindLabel(report, OrderInfo, "cellPartsDrawingNO", "PartsDrawingNO");
            DevReportHelper.BindLabel(report, OrderInfo, "cellPartsImportant", "PartsImportant");
            DevReportHelper.BindLabel(report, OrderInfo, "cellPartsWeight", "PartsWeight");
            DevReportHelper.BindLabel(report, OrderInfo, "cellRemark", "Remark");

            DevReportHelper.PrintPreview(null, report);

        }
    }
}
