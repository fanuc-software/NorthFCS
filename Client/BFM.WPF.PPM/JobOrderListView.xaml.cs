using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using BFM.Common.Base;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.PLMService;
using BFM.Server.DataAsset.RSMService;
using BFM.Server.DataAsset.SDMService;
using BFM.Server.DataAsset.WMSService;
using BFM.WPF.Base;
using BFM.WPF.Base.ControlStyles;
using BFM.WPF.Base.Helper;
using BFM.WPF.FMS;

namespace BFM.WPF.PPM
{
    /// <summary>
    /// JobOrderListView.xaml 的交互逻辑
    /// </summary>
    public partial class JobOrderListView : Page
    {
        #region 单例模式 - 完美方式

        private static JobOrderListView instance = null;
        private static readonly object objLockInstance = new object();

        public static JobOrderListView GetInstance()
        {
            if (instance == null)
            {
                lock (objLockInstance)
                {
                    if (instance == null)
                    {
                        instance = new JobOrderListView();
                    }
                }
            }

            return instance;
        }

        #endregion 单例模式 - 完美方式

        private bool bRefreshDict = true;
        private WcfClient<IPLMService> ws = new WcfClient<IPLMService>();

        public JobOrderListView()
        {
            InitializeComponent();
            GetPage();
            new Thread(RefreshDict).Start();
        }

        //private Action<int, int, string, string, int, string> _showInfo;  //显示界面信息，提高效率

        private void RefreshDict()
        {
            while (!CBaseData.AppClosing)
            {
                if (!bRefreshDict)
                {
                    Thread.Sleep(200);
                    continue;
                }

                Dispatcher.BeginInvoke((Action) (delegate()
                {
                    MesJobOrder mesJobOrder = gridorder.SelectedItem as MesJobOrder;
                    if (mesJobOrder == null)
                    {
                        gridItem.ItemsSource = null;
                        return;
                    }

                    var process = ws.UseService(s =>
                            s.GetMesProcessCtrols($"JOB_ORDER_PKNO = '{mesJobOrder.PKNO}' "))
                        .OrderBy(c => c.PROCESS_INDEX).ThenBy(c => c.CREATION_DATE).ToList();

                    gridItem.ItemsSource = process;
                }));

                Thread.Sleep(2000);
            }
        }

        private void GetPage()
        {
            List<MesJobOrder> mesJobOrders =
                ws.UseService(s =>
                        s.GetMesJobOrders(
                            $"USE_FLAG = 1 AND RUN_STATE < 100 AND LINE_PKNO = '{CBaseData.CurLinePKNO}'"))
                    .OrderBy(c => c.CREATION_DATE).ToList();
            Dispatcher.BeginInvoke((Action)(delegate ()
            {
                gridorder.ItemsSource = mesJobOrders;
                gMain.Header = $"订单信息 共【{mesJobOrders.Count}】个";
            }));
        }

        private void btnCancelAll_Click(object sender, RoutedEventArgs e)
        {
            List<MesJobOrder> mesJobOrders = gridorder.ItemsSource as List<MesJobOrder>;
            if (mesJobOrders == null)
            {
                return;
            }

            if (WPFMessageBox.ShowConfirm("确定要取消所有订单吗？", "取消所有订单") != WPFMessageBoxResult.OK)
            {
                return;
            }

            new Thread(new ThreadStart(delegate()
            {
                WaitLoading.SetWait(this);

                DeviceProcessControl.PauseByLine(CBaseData.CurLinePKNO); //暂停，防止任务直接执行
                Thread.Sleep(1000);
                List<MesJobOrder> jobs =
                    ws.UseService(s =>
                            s.GetMesJobOrders(
                                $"USE_FLAG = 1 AND RUN_STATE < 100 AND LINE_PKNO = '{CBaseData.CurLinePKNO}'"))
                        .OrderBy(c => c.CREATION_DATE).ToList();
                foreach (MesJobOrder job in jobs)
                {

                    job.ACT_FINISH_TIME = DateTime.Now;
                    job.RUN_STATE = 102; //手动取消
                    ////mesJobOrder.ONCE_QTY = 1;//默认订单为1
                    ////mesJobOrder.COMPLETE_QTY = 1;
                    ws.UseService(s => s.UpdateMesJobOrder(job));

                    Thread.Sleep(100);

                    List<MesProcessCtrol> mesProcessCtrols =
                        ws.UseService(s => s.GetMesProcessCtrols("JOB_ORDER_PKNO = " + job.PKNO + ""));
                    if (mesProcessCtrols.Count < 0)
                    {
                        WaitLoading.SetDefault(this);
                        return;
                    }

                    foreach (var processCtrol in mesProcessCtrols)
                    {
                        processCtrol.PROCESS_STATE = 20; //不执行
                        ws.UseService(s => s.UpdateMesProcessCtrol(processCtrol));
                        Thread.Sleep(100);
                    }
                }

                DeviceProcessControl.RunByLine(CBaseData.CurLinePKNO); //启动动作流程

                GetPage();

                WaitLoading.SetDefault(this);

            })).Start();
        }

        //完成订单： 订单标记完成，数量改写，标记process结束。
        private void BtnComplete_click(object sender, RoutedEventArgs e)
        {
            MesJobOrder mesJobOrder = gridorder.GetFocusedRow() as MesJobOrder;
            if (mesJobOrder == null)
            {
                return;

            }

            if (WPFMessageBox.ShowConfirm("确定要完成该订单吗？", "完成订单") != WPFMessageBoxResult.OK)
            {
                return;
            }

            Cursor = Cursors.Wait;

            mesJobOrder.ACT_FINISH_TIME =DateTime.Now;
            mesJobOrder.RUN_STATE = 101;//手动完成
            mesJobOrder.ONCE_QTY = 1;//默认订单为1
            mesJobOrder.COMPLETE_QTY = 1;
            
            ws.UseService(s => s.UpdateMesJobOrder(mesJobOrder));

            Thread.Sleep(500);

            List<MesProcessCtrol> mesProcessCtrols =
                ws.UseService(s => s.GetMesProcessCtrols("JOB_ORDER_PKNO = " + mesJobOrder.PKNO + ""));
            if (mesProcessCtrols.Count < 0)
            {
                Cursor = Cursors.Arrow;
                return;
            }
            foreach (var processCtrol in mesProcessCtrols)
            {
                processCtrol.PROCESS_STATE = 10;//完成状态
                ws.UseService(s => s.UpdateMesProcessCtrol(processCtrol));
            }
            GetPage();

            Cursor = Cursors.Arrow;
        }

        //取消订单： 订单标记取消，标记process结束。
        private void BtnCancle_click(object sender, RoutedEventArgs e)
        {
            MesJobOrder mesJobOrder = gridorder.GetFocusedRow() as MesJobOrder;
            if (mesJobOrder == null)
            {
                return;

            }

            if (WPFMessageBox.ShowConfirm("确定要取消该订单吗？", "取消订单") != WPFMessageBoxResult.OK)
            {
                return;
            }

            Cursor = Cursors.Wait;

            mesJobOrder.ACT_FINISH_TIME = DateTime.Now;
            mesJobOrder.RUN_STATE = 102;  //手动取消
            ////mesJobOrder.ONCE_QTY = 1;//默认订单为1
            ////mesJobOrder.COMPLETE_QTY = 1;
            ws.UseService(s => s.UpdateMesJobOrder(mesJobOrder));

            Thread.Sleep(500);

            List<MesProcessCtrol> mesProcessCtrols =
                ws.UseService(s => s.GetMesProcessCtrols("JOB_ORDER_PKNO = " + mesJobOrder.PKNO + ""));
            if (mesProcessCtrols.Count < 0)
            {
                Cursor = Cursors.Arrow;
                return;
            }
            foreach (var processCtrol in mesProcessCtrols)
            {
                processCtrol.PROCESS_STATE = 20;//不执行
                ws.UseService(s => s.UpdateMesProcessCtrol(processCtrol));
            }

            GetPage();

            Cursor = Cursors.Arrow;
        }

        //刷新全部
        private void Btnrefresh_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            GetPage();
            Cursor = Cursors.Arrow;
        }

        //自动刷新
        private void Btnautorefresh_Click(object sender, RoutedEventArgs e)
        {
            if (bRefreshDict)
            {
                bRefreshDict = false;
                btnautorefresh.Content = "开启自动更新明细";
            }
            else
            {
                bRefreshDict = true;
                btnautorefresh.Content = "关闭自动更新明细";
            }

        }

        //重做当前
        private void bReWork_click(object sender, RoutedEventArgs e)
        {
            MesJobOrder mesJobOrder = gridorder.GetFocusedRow() as MesJobOrder;
            if (mesJobOrder == null)
            {
                return;

            }

            if (WPFMessageBox.ShowConfirm("确定要重做当前工序吗？", "重做工序") != WPFMessageBoxResult.OK)
            {
                return;
            }

            MesProcessCtrol processCtrol =
                ws.UseService(s =>
                        s.GetMesProcessCtrols($"JOB_ORDER_PKNO = '{mesJobOrder.PKNO}' AND PROCESS_STATE < 10 "))
                    .OrderBy(c => c.PROCESS_INDEX).FirstOrDefault();  //正在执行的
            if (processCtrol == null) return;
            
            processCtrol.PROCESS_STATE = 1;    //重新执行
            ws.UseService(s => s.UpdateMesProcessCtrol(processCtrol));

            gridItem.ItemsSource = ws.UseService(s =>
                    s.GetMesProcessCtrols($"JOB_ORDER_PKNO = '{mesJobOrder.PKNO}' "))
                .OrderBy(c => c.PROCESS_INDEX).ThenBy(c => c.CREATION_DATE).ToList();
        }

        //完成当前
        private void bFinishThis_click(object sender, RoutedEventArgs e)
        {
            MesJobOrder mesJobOrder = gridorder.GetFocusedRow() as MesJobOrder;
            if (mesJobOrder == null)
            {
                return;

            }

            if (WPFMessageBox.ShowConfirm("确定要完成当前工序吗？", "完成当前工序") != WPFMessageBoxResult.OK)
            {
                return;
            }

            List<MesProcessCtrol> processCtrols = ws.UseService(s => s.GetMesProcessCtrols($"PROCESS_STATE < 10 "))
                .OrderBy(c => c.PROCESS_INDEX).ThenBy(c => c.CREATION_DATE).ToList();  //执行的动作

            MesProcessCtrol processCtrol = processCtrols.FirstOrDefault(c => c.JOB_ORDER_PKNO == mesJobOrder.PKNO);
            if (processCtrol == null) return;

            var result = DeviceProcessControl.FinishCurBusiness(processCtrol);

            if (result != "OK")
            {
                WPFMessageBox.ShowError("当前工序的业务层完成错误，请核实。\r\n错误为：" + result, "完成当前工序");
                return;
            }
            processCtrol.PROCESS_STATE = 10;    //执行完成
            ws.UseService(s => s.UpdateMesProcessCtrol(processCtrol));
            
            
            if (processCtrol == processCtrols.LastOrDefault())
            {
                mesJobOrder.PROCESS_INFO = "手动完成当前过程";  //生产执行信息
                mesJobOrder.ACT_FINISH_TIME = DateTime.Now;
                mesJobOrder.RUN_STATE = 101;//手动完成
                mesJobOrder.ONCE_QTY = 1;//默认订单为1
                mesJobOrder.COMPLETE_QTY = 1;
            }
            else
            {
                mesJobOrder.PROCESS_INFO = "手动完成当前订单";  //生产执行信息
            }

            ws.UseService(s => s.UpdateMesJobOrder(mesJobOrder));

            gridItem.ItemsSource = ws.UseService(s =>
                    s.GetMesProcessCtrols($"JOB_ORDER_PKNO = '{mesJobOrder.PKNO}' "))
                .OrderBy(c => c.PROCESS_INDEX).ThenBy(c => c.CREATION_DATE).ToList();
        }

        private void Gridorder_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            MesJobOrder mesJobOrder = gridorder.SelectedItem as MesJobOrder;
            if (mesJobOrder == null)
            {
                gridItem.ItemsSource = null;
                return;
            }

            var process = ws.UseService(s =>
                    s.GetMesProcessCtrols($"JOB_ORDER_PKNO = '{mesJobOrder.PKNO}' "))
                .OrderBy(c => c.PROCESS_INDEX).ThenBy(c => c.CREATION_DATE).ToList();

            gridItem.ItemsSource = process;

        }
    }
}
