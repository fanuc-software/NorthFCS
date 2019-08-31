using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
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
using BFM.Common.Base.Log;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Server.DataAsset.FMSService;
using BFM.ContractModel;
using BFM.Server.DataAsset.RSMService;
using BFM.Server.DataAsset.WMSService;
using BFM.WPF.Base;
using BFM.WPF.Base.Controls;
using BFM.WPF.FlowDesign.Controls;

namespace BFM.WPF.FMS
{
    /// <summary>
    /// DeviceDataMoniter.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceDataMoniter : Page
    {
        #region 单例模式 - 完美方式

        private static DeviceDataMoniter instance = null;
        private static readonly object objLockInstance = new object();

        public static DeviceDataMoniter GetInstance()
        {
            if (instance == null)
            {
                lock (objLockInstance)
                {
                    if (instance == null)
                    {
                        instance = new DeviceDataMoniter();
                    }
                }
            }

            return instance;
        }

        #endregion 单例模式 - 完美方式

        WcfClient<IFMSService> ws = new WcfClient<IFMSService>();
        private WcfClient<IWMSService> wsWMS = new WcfClient<IWMSService>();
        private WcfClient<IRSMService> wsRsm = new WcfClient<IRSMService>();

        private bool bShowInfo = false;
        private bool bRefreshAllo = false;

        private int totalColumn = 6;
        private int totalLayer = 2;
        private string areaPKNO = "";

        public DeviceDataMoniter()
        {
            InitializeComponent();

            ThreadPool.QueueUserWorkItem(ThreadShowWorkState);

            new Thread(RefreshAllocation).Start();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (bShowInfo) return;

            Cursor = Cursors.Wait;
            cvMain.Visibility = Visibility;
            string fileName = System.Environment.CurrentDirectory + "\\Monitor\\default.lfd";
            if (File.Exists(fileName))
            {
                cvMain.LoadFile(fileName); //加载
            }

            bShowInfo = true;

            #region 显示库存信息

            WmsAreaInfo area = wsWMS.UseService(s => s.GetWmsAreaInfos($"USE_FLAG = 1 AND AREA_CODE = '轮毂立体料架'"))
                .FirstOrDefault();

            if (area != null)
            {
                //myShelf.TotalColumn = SafeConverter.SafeToInt(area.TOTAL_COL, 6);
                //totalColumn = SafeConverter.SafeToInt(area.TOTAL_COL, 6);
                //myShelf.TotalLayer = SafeConverter.SafeToInt(area.TOTAL_LAY, 2);
                //totalLayer = SafeConverter.SafeToInt(area.TOTAL_LAY, 2);
                //areaPKNO = area.PKNO;
            }


            List<RsItemMaster> items = wsRsm.UseService(s => s.GetRsItemMasters("USE_FLAG = 1"));  //物料信息

            #region 初始化物料显示图片

            SingleRowShelf.GoodsColors.Clear();
            SingleRowShelf.GoodsImages.Clear();

            string path = Environment.CurrentDirectory + "/images/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            foreach (RsItemMaster item in items)
            {
                string imgFile = path + item.ITEM_NAME + ".jpg";
                if (File.Exists(imgFile))
                {
                    SingleRowShelf.GoodsImages.Add(item.PKNO, new BitmapImage(new Uri(imgFile, UriKind.Absolute)));
                }
                else //不存在图片
                {
                    SingleRowShelf.GoodsColors.Add(item.PKNO, Brushes.Pink);
                }
            }

            #endregion

            #endregion

            bRefreshAllo = true;

            Cursor = Cursors.Arrow;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            bShowInfo = false;
            bRefreshAllo = false;
        }

        #region 后台线程 - 显示监控信息

        private Action<DragThumb, FmsAssetTagSetting> _showInfo; //显示界面信息

        private void ThreadShowWorkState(object obj)
        {
            //状态颜色 0：其他状态=脱机的颜色；1：工作状态颜色；2：故障状态颜色；3：待机状态颜色；4：急停状态颜色
            Brush[] StateColors = new Brush[5]
                {Brushes.Gray, Brushes.Green, Brushes.OrangeRed, Brushes.DarkGoldenrod, Brushes.DarkRed};

            #region 创建显示信息的函数，提高效率

            _showInfo = (thumb, tag) =>
            {
                if ((thumb == null) || (tag == null)) return;

                if (tag.STATE_MARK_TYPE == 0) //普通内容
                {
                    thumb.Text = tag.TAG_VALUE_NAME + " " + tag.CUR_VALUE + " " + tag.VALUE_UNIT;
                }
                else if (tag.STATE_MARK_TYPE == 1) //联机状态
                {
                    int state = SafeConverter.SafeToInt(tag.CUR_VALUE, 0);
                    if (state <= 4) //状态
                    {
                        thumb.Background = StateColors[state];
                        thumb.BorderBrush = StateColors[state];
                    }
                }
                else if (tag.STATE_MARK_TYPE == 2) //待机状态
                {
                    int state = SafeConverter.SafeToInt(tag.CUR_VALUE, 0);
                    if (state == 2)
                    {
                        state = 3;
                    }

                    if (state <= 4) //状态
                    {
                        thumb.Background = StateColors[state];
                        thumb.BorderBrush = StateColors[state];
                    }
                }
                else if (tag.STATE_MARK_TYPE == 4) //故障状态
                {
                    int state = SafeConverter.SafeToInt(tag.CUR_VALUE, -1);
                    if (state == 1) //故障，整个产线报警
                    {
                        thumb.Background = StateColors[2];
                        thumb.BorderBrush = StateColors[2];
                    }
                    else if (state == 0) //正常
                    {
                        thumb.Background = StateColors[1];
                        thumb.BorderBrush = StateColors[1];
                    }
                    else //离线状态
                    {
                        thumb.Background = StateColors[0];
                        thumb.BorderBrush = StateColors[0];
                    }
                }
                else if (tag.STATE_MARK_TYPE == 10) //状态
                {
                    int state = SafeConverter.SafeToInt(tag.CUR_VALUE, 0);
                    if ((state < 0) || (state >= StateColors.Length)) state = 0;

                    thumb.Background = StateColors[state];
                    thumb.BorderBrush = StateColors[state];
                }
            };

            #endregion

            #region 后台执行

            while (!CBaseData.AppClosing)
            {
                if (!bShowInfo)
                {
                    Thread.Sleep(500);
                    continue;
                }

                //显示状态
                try
                {
                    foreach (DragThumb thumb in cvMain.DragThumbs)
                    {
                        if (!bShowInfo) break;

                        FmsAssetTagSetting tag = DeviceMonitor.GetTagSettingById(thumb.CtrlName); //查找TagSetting
                        if (tag == null)
                        {
                            continue;
                        }

                        Dispatcher.BeginInvoke(_showInfo, thumb, tag);
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.Log($"!!!!!!实时监控错误，原因:{ex.Message}!!!!!!");
                }

                Thread.Sleep(500);
            }

            #endregion
        }

        #endregion

        #region 显示库存信息

        private Action<int, int, string, string, int, string> _showAlloc; //显示界面信息

        private void RefreshAllocation()
        {
            #region 创建显示信息的函数，提高效率

            _showAlloc = (col, lay, allInfo, goodsNO, allocProportion, palletInfo) =>
            {
                //myShelf.RefreshAlloInfo(col, lay, allInfo, goodsNO, allocProportion, palletInfo);
            };

            #endregion

            WcfClient<IWMSService> wsWMS = new WcfClient<IWMSService>();
            WcfClient<IRSMService> wsRsm1 = new WcfClient<IRSMService>();

            while (!CBaseData.AppClosing)
            {
                if (!bRefreshAllo)
                {
                    Thread.Sleep(500);
                    continue;
                }

                try
                {
                    //获取货位及库存状态
                    List<WmsAllocationInfo> allocations = wsWMS.UseService(s =>
                        s.GetWmsAllocationInfos($"USE_FLAG = 1 AND AREA_PKNO = '{areaPKNO}'"));
                    List<WmsInventory> inventories = wsWMS.UseService(s => s.GetWmsInventorys($"AREA_PKNO = '{areaPKNO}'"));  //库存信息

                    for (int col = 1; col <= totalColumn; col++)
                    {
                        for (int lay = 1; lay <= totalLayer; lay++)
                        {
                            WmsAllocationInfo allocation =
                                allocations.FirstOrDefault(c => c.ALLOCATION_COL == col && c.ALLOCATION_LAY == lay);

                            var col1 = col;
                            var lay1 = lay;
                            string allInfo = "";
                            string goodsNO = "";
                            int alloproportion = -1;  //禁用
                            string palletInfo = "";

                            if (allocation != null)
                            {
                                alloproportion = SafeConverter.SafeToInt(allocation.ALLOCATION_STATE);
                                palletInfo = allocation.ALLOCATION_NAME;

                                var inv = inventories.FirstOrDefault(c => c.ALLOCATION_PKNO == allocation.PKNO);
                                if (inv != null)  //有库存
                                {
                                    goodsNO = inv.MATERIAL_PKNO;
                                    RsItemMaster item =
                                        wsRsm1.UseService(s => s.GetRsItemMasterById(inv.MATERIAL_PKNO));
                                    if (item != null)
                                    {
                                        allInfo = item.ITEM_NAME + " (" + inv.INVENTORY_NUM?.ToString("f0") + ")";
                                    }
                                }
                            }

                            //显示信息
                            Dispatcher.BeginInvoke(_showAlloc, col1, lay1, allInfo, goodsNO, alloproportion, palletInfo);
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Thread.Sleep(1000);
            }
        }

        #endregion

        #region 手动打开文件

        private void bOpen_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "流程设计文件(*.lfd)|*.lfd|所有文件(*.*)|*.*";
            if (dialog.ShowDialog() == false)
            {
                return;
            }

            bool result = cvMain.LoadFile(dialog.FileName); //加载
            if (!result)
            {
                MessageBox.Show("加载失败，请核实.", "打开文件", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void bSave_Click(object sender, RoutedEventArgs e)
        {
            cvMain.Save("123.fld");
        }

        #endregion

        //机器人使能
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //机器人使能UI1=1 UI2=1 UI3=1 UI8=1 UI9=1
            FmsAssetTagSetting tag = null;
            int ret = 0;
            string error = "";
            int iWrite = 0;
            string tagCode = "UI1";



            tagCode = "UI1";
            #region 给出使能

            tag = DeviceMonitor.GetTagSettings($"TAG_CODE = '{tagCode}'").FirstOrDefault();

            if (tag == null)
            {
                return;
            }
            iWrite = 0;
            while (iWrite < ReWriteCount)
            {
                ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "1", out error);
                if (ret == 0)
                {
                    break;
                }
                iWrite++;
                Thread.Sleep(100);
            }

            #endregion

            tagCode = "UI2";
            #region 给出使能

            tag = DeviceMonitor.GetTagSettings($"TAG_CODE = '{tagCode}'").FirstOrDefault();

            if (tag == null)
            {
                return;
            }
            iWrite = 0;
            while (iWrite < ReWriteCount)
            {
                ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "1", out error);
                if (ret == 0)
                {
                    break;
                }
                iWrite++;
                Thread.Sleep(100);
            }

            #endregion

            tagCode = "UI3";
            #region 给出使能

            tag = DeviceMonitor.GetTagSettings($"TAG_CODE = '{tagCode}'").FirstOrDefault();

            if (tag == null)
            {
                return;
            }
            iWrite = 0;
            while (iWrite < ReWriteCount)
            {
                ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "1", out error);
                if (ret == 0)
                {
                    break;
                }
                iWrite++;
                Thread.Sleep(100);
            }

            #endregion

            tagCode = "UI8";
            #region 给出使能

            tag = DeviceMonitor.GetTagSettings($"TAG_CODE = '{tagCode}'").FirstOrDefault();

            if (tag == null)
            {
                return;
            }
            iWrite = 0;
            while (iWrite < ReWriteCount)
            {
                ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "1", out error);
                if (ret == 0)
                {
                    break;
                }
                iWrite++;
                Thread.Sleep(100);
            }

            #endregion

            tagCode = "UI9";
            #region 给出使能

            tag = DeviceMonitor.GetTagSettings($"TAG_CODE = '{tagCode}'").FirstOrDefault();

            if (tag == null)
            {
                return;
            }
            iWrite = 0;
            while (iWrite < ReWriteCount)
            {
                ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "1", out error);
                if (ret == 0)
                {
                    break;
                }
                iWrite++;
                Thread.Sleep(100);
            }

            #endregion

            if (ret == 0) WPFMessageBox.ShowInfo("机器人使能已经给出.", "启动机器人");
        }

        private void BtnRobotReboot_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;

            int ret = 0;
            string error = "";
            int iWrite = 0;
            string tagCode = "";
            FmsAssetTagSetting tag = null;

            #region 清空GI
            tagCode = "清空机器人GI";
            tag = DeviceMonitor.GetTagSettings($"TAG_CODE = '{tagCode}'").FirstOrDefault();

            if (tag == null)
            {
                Cursor = Cursors.Arrow;
                return;
            }
            iWrite = 0;
            while (iWrite < ReWriteCount)
            {
                ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "12", out error);
                if (ret == 0)
                {
                    break;
                }

                iWrite++;
                Thread.Sleep(100);
            }

            #endregion

            Cursor = Cursors.Arrow;
        }

        //UI18下降沿
        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cursor = Cursors.Wait;

                int ret = 0;
                string error = "";
                int iWrite = 0;
                string tagCode = "";
                FmsAssetTagSetting tag = null;
                //#region 清空相关DI
                //#region 清空DI171
                //tagCode = "DI171";
                //tag = DeviceMonitor.GetTagSettings($"TAG_CODE = '{tagCode}'").FirstOrDefault();

                //if (tag == null)
                //{
                //    Cursor = Cursors.Arrow;
                //    return;
                //}
                //iWrite = 0;
                //while (iWrite < ReWriteCount)
                //{
                //    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                //    if (ret == 0)
                //    {
                //        break;
                //    }

                //    iWrite++;
                //    Thread.Sleep(100);
                //}

                //#endregion

                //#region 清空DI172
                //tagCode = "DI172";
                //tag = DeviceMonitor.GetTagSettings($"TAG_CODE = '{tagCode}'").FirstOrDefault();

                //if (tag == null)
                //{
                //    Cursor = Cursors.Arrow;
                //    return;
                //}
                //iWrite = 0;
                //while (iWrite < ReWriteCount)
                //{
                //    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                //    if (ret == 0)
                //    {
                //        break;
                //    }

                //    iWrite++;
                //    Thread.Sleep(100);
                //}

                //#endregion
                //#region 清空DI175
                //tagCode = "DI175";
                //tag = DeviceMonitor.GetTagSettings($"TAG_CODE = '{tagCode}'").FirstOrDefault();

                //if (tag == null)
                //{
                //    Cursor = Cursors.Arrow;
                //    return;
                //}
                //iWrite = 0;
                //while (iWrite < ReWriteCount)
                //{
                //    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                //    if (ret == 0)
                //    {
                //        break;
                //    }

                //    iWrite++;
                //    Thread.Sleep(100);
                //}

                //#endregion
                //#region 清空DI176
                //tagCode = "DI176";
                //tag = DeviceMonitor.GetTagSettings($"TAG_CODE = '{tagCode}'").FirstOrDefault();

                //if (tag == null)
                //{
                //    Cursor = Cursors.Arrow;
                //    return;
                //}
                //iWrite = 0;
                //while (iWrite < ReWriteCount)
                //{
                //    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                //    if (ret == 0)
                //    {
                //        break;
                //    }

                //    iWrite++;
                //    Thread.Sleep(100);
                //}

                //#endregion

                //#region 清空GI1
                //tagCode = "GI1";
                //tag = DeviceMonitor.GetTagSettings($"TAG_CODE = '{tagCode}'").FirstOrDefault();

                //if (tag == null)
                //{
                //    Cursor = Cursors.Arrow;
                //    return;
                //}
                //iWrite = 0;
                //while (iWrite < ReWriteCount)
                //{
                //    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                //    if (ret == 0)
                //    {
                //        break;
                //    }

                //    iWrite++;
                //    Thread.Sleep(100);
                //}

                //#endregion
                //#region 清空GI4
                //tagCode = "GI4";
                //tag = DeviceMonitor.GetTagSettings($"TAG_CODE = '{tagCode}'").FirstOrDefault();

                //if (tag == null)
                //{
                //    Cursor = Cursors.Arrow;
                //    return;
                //}
                //iWrite = 0;
                //while (iWrite < ReWriteCount)
                //{
                //    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                //    if (ret == 0)
                //    {
                //        break;
                //    }

                //    iWrite++;
                //    Thread.Sleep(100);
                //}

                //#endregion


                //#endregion
                //tagCode = "清空机器人GI";
                //#region 清空GI

                //tag = DeviceMonitor.GetTagSettings($"TAG_CODE = '{tagCode}'").FirstOrDefault();

                //if (tag == null)
                //{
                //    Cursor = Cursors.Arrow;
                //    return;
                //}
                //iWrite = 0;
                //while (iWrite < ReWriteCount)
                //{
                //    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                //    if (ret == 0)
                //    {
                //        break;
                //    }

                //    iWrite++;
                //    Thread.Sleep(100);
                //}

                //#endregion


                tagCode = "UI5";
                #region 给出UI5

                tag = DeviceMonitor.GetTagSettings($"TAG_CODE = '{tagCode}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "1", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                tagCode = "UI17";
                #region 给出UI17

                tag = DeviceMonitor.GetTagSettings($"TAG_CODE = '{tagCode}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "1", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                Thread.Sleep(300);

                tagCode = "UI18";
                #region UI18 下降沿

                tag = DeviceMonitor.GetTagSettings($"TAG_CODE = '{tagCode}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "1", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                if (ret != 0)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }

                Thread.Sleep(300);

                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                Thread.Sleep(1000);

                tagCode = "UI17";
                #region 清空 UI17

                tag = DeviceMonitor.GetTagSettings($"TAG_CODE = '{tagCode}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                tagCode = "UI5";
                #region 清空 UI5

                tag = DeviceMonitor.GetTagSettings($"TAG_CODE = '{tagCode}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                Cursor = Cursors.Arrow;

                if (ret == 0) WPFMessageBox.ShowInfo("机器人启动信号已经成功给出.", "启动机器人");
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Arrow;
            }
        }

        private const int ReWriteCount = 3;

        //UI6下降沿
        private void BtnPauseStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FmsAssetTagSetting tag = DeviceMonitor.GetTagSettings("TAG_CODE = 'UI6'").FirstOrDefault();

                if (tag == null)
                {
                    return;
                }

                Cursor = Cursors.Wait;
                int ret = 0;
                string error = "";
                int iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "1", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                if (ret != 0)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }

                Thread.Sleep(500);

                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                Cursor = Cursors.Arrow;
                if (ret == 0) WPFMessageBox.ShowInfo("机器人的暂停后启动信息已经成功给出！", "启动机器人");
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void btnRestDevice1_Click(object sender, RoutedEventArgs e)
        {
            //创胜特尔
            try
            {
                Cursor = Cursors.Wait;
                string assetCode = "SH00003";

                int ret = 0;
                string error = "";
                int iWrite = 0;
                string tagAddress = "";
                FmsAssetTagSetting tag = null;

                tagAddress = "#120";
                #region 清空换刀刀号

                tag = DeviceMonitor.GetTagSettings($"ASSET_CODE = '{assetCode}' AND TAG_ADDRESS = '{tagAddress}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                tagAddress = "#121";
                #region 清空下次换刀刀号

                tag = DeviceMonitor.GetTagSettings($"ASSET_CODE = '{assetCode}' AND TAG_ADDRESS = '{tagAddress}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                tagAddress = "E520.0";
                #region 清空启动程序

                tag = DeviceMonitor.GetTagSettings($"ASSET_CODE = '{assetCode}' AND TAG_ADDRESS = '{tagAddress}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                tagAddress = "E521.0";
                #region 清空 机床加工下发/完成

                tag = DeviceMonitor.GetTagSettings($"ASSET_CODE = '{assetCode}' AND TAG_ADDRESS = '{tagAddress}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                tagAddress = "E521.1";
                #region 清空 机床换刀中

                tag = DeviceMonitor.GetTagSettings($"ASSET_CODE = '{assetCode}' AND TAG_ADDRESS = '{tagAddress}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                tagAddress = "E530";
                #region 清空 机床加工程序号

                tag = DeviceMonitor.GetTagSettings($"ASSET_CODE = '{assetCode}' AND TAG_ADDRESS = '{tagAddress}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                tagAddress = "E531.0";
                #region 清空 机床松开刀

                tag = DeviceMonitor.GetTagSettings($"ASSET_CODE = '{assetCode}' AND TAG_ADDRESS = '{tagAddress}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                tagAddress = "E532.0";
                #region 清空 机床拉紧刀

                tag = DeviceMonitor.GetTagSettings($"ASSET_CODE = '{assetCode}' AND TAG_ADDRESS = '{tagAddress}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                tagAddress = "E533.0";
                #region 清空 机器人离开机床

                tag = DeviceMonitor.GetTagSettings($"ASSET_CODE = '{assetCode}' AND TAG_ADDRESS = '{tagAddress}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                Cursor = Cursors.Arrow;

                if (ret == 0) WPFMessageBox.ShowInfo("1#机床（创胜特尔）的FCS状态已复位.", "复位机床状态");
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void btnRestDevice2_Click(object sender, RoutedEventArgs e)
        {
            //英伟达
            try
            {
                Cursor = Cursors.Wait;
                string assetCode = "SH00002";

                int ret = 0;
                string error = "";
                int iWrite = 0;
                string tagAddress = "";
                FmsAssetTagSetting tag = null;
                tagAddress = "E530";
                #region 初始化101程序

                tag = DeviceMonitor.GetTagSettings($"ASSET_CODE = '{assetCode}' AND TAG_ADDRESS = '{tagAddress}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "101", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                tagAddress = "E540";
                #region 初始化504程序

                tag = DeviceMonitor.GetTagSettings($"ASSET_CODE = '{assetCode}' AND TAG_ADDRESS = '{tagAddress}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "101", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }
                #endregion

                tagAddress = "E520.0";
                #region 初始化开始状态

                tag = DeviceMonitor.GetTagSettings($"ASSET_CODE = '{assetCode}' AND TAG_ADDRESS = '{tagAddress}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "1", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                Cursor = Cursors.Arrow;

                if (ret == 0) WPFMessageBox.ShowInfo("2#机床（英伟达）的FCS状态已复位.", "复位机床状态");
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void btnRestDevice3_Click(object sender, RoutedEventArgs e)
        {
            //斌胜
            try
            {
                Cursor = Cursors.Wait;
                string assetCode = "SH00001";

                int ret = 0;
                string error = "";
                int iWrite = 0;
                string tagAddress = "";
                FmsAssetTagSetting tag = null;

                tagAddress = "#120";
                #region 清空换刀刀号

                tag = DeviceMonitor.GetTagSettings($"ASSET_CODE = '{assetCode}' AND TAG_ADDRESS = '{tagAddress}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                tagAddress = "#121";
                #region 清空下次换刀刀号

                tag = DeviceMonitor.GetTagSettings($"ASSET_CODE = '{assetCode}' AND TAG_ADDRESS = '{tagAddress}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                tagAddress = "E520.0";
                #region 清空启动程序

                tag = DeviceMonitor.GetTagSettings($"ASSET_CODE = '{assetCode}' AND TAG_ADDRESS = '{tagAddress}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                tagAddress = "E521.0";
                #region 清空 机床加工下发/完成

                tag = DeviceMonitor.GetTagSettings($"ASSET_CODE = '{assetCode}' AND TAG_ADDRESS = '{tagAddress}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                tagAddress = "E521.1";
                #region 清空 机床换刀中

                tag = DeviceMonitor.GetTagSettings($"ASSET_CODE = '{assetCode}' AND TAG_ADDRESS = '{tagAddress}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                tagAddress = "E530";
                #region 清空 机床加工程序号

                tag = DeviceMonitor.GetTagSettings($"ASSET_CODE = '{assetCode}' AND TAG_ADDRESS = '{tagAddress}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                tagAddress = "E531.0";
                #region 清空 机床松开刀

                tag = DeviceMonitor.GetTagSettings($"ASSET_CODE = '{assetCode}' AND TAG_ADDRESS = '{tagAddress}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                tagAddress = "E532.0";
                #region 清空 机床拉紧刀

                tag = DeviceMonitor.GetTagSettings($"ASSET_CODE = '{assetCode}' AND TAG_ADDRESS = '{tagAddress}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                tagAddress = "E533.0";
                #region 清空 机器人离开机床

                tag = DeviceMonitor.GetTagSettings($"ASSET_CODE = '{assetCode}' AND TAG_ADDRESS = '{tagAddress}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                iWrite = 0;
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }

                #endregion

                Cursor = Cursors.Arrow;

                if (ret == 0) WPFMessageBox.ShowInfo("3#机床（斌胜）的FCS状态已复位.", "复位机床状态");
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void Btn_reset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string tagCode = "DI118";
                var tag = DeviceMonitor.GetTagSettings($"TAG_CODE = '{tagCode}'").FirstOrDefault();

                if (tag == null)
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                var iWrite = 0;
                int ret = 0;
                string error = "";
                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "1", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }
                Thread.Sleep(3000);

                while (iWrite < ReWriteCount)
                {
                    ret = DeviceMonitor.WriteTagToDevice(tag.PKNO, "0", out error);
                    if (ret == 0)
                    {
                        break;
                    }

                    iWrite++;
                    Thread.Sleep(100);
                }
                if (ret == 0) WPFMessageBox.ShowInfo("复位FCS警告成功.", "复位FCS");
            }
            catch (Exception ex)
            {

                WPFMessageBox.ShowInfo(ex.Message, "复位FCS错误");
            }
          

        }
    }
}

