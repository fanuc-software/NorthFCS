using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BFM.Common.Base;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.EAMService;
using BFM.Server.DataAsset.FMSService;
using BFM.Server.DataAsset.PLMService;
using BFM.Server.DataAsset.RSMService;
using BFM.Server.DataAsset.SQLService;
using BFM.Server.DataAsset.TMSService;
using BFM.Server.DataAsset.WMSService;
using BFM.WPF.Base;
using BFM.WPF.Base.Controls;
using BFM.WPF.Base.ControlStyles;
using BFM.WPF.FMS;
using BFM.WPF.SDM.TableNO;
using BFM.WPF.WMS.ViewModel;

namespace BFM.WPF.WMS
{
    /// <summary>
    /// InventoryGraphic.xaml 的交互逻辑
    /// </summary>
    public partial class InventoryGraphic : Page
    {
        #region 单例模式 - 完美方式

        private static InventoryGraphic instance = null;
        private static readonly object objLockInstance = new object();

        public static InventoryGraphic GetInstance()
        {
            if (instance == null)
            {
                lock (objLockInstance)
                {
                    if (instance == null)
                    {
                        instance = new InventoryGraphic();
                    }
                }
            }

            return instance;
        }

        #endregion 单例模式 - 完美方式

        private InventoryGraphicViewModel viewModel = new InventoryGraphicViewModel();

        private WcfClient<IWMSService> ws = new WcfClient<IWMSService>();
        private WcfClient<IRSMService> wsRsm = new WcfClient<IRSMService>();
        private WcfClient<IPLMService> wsPlm = new WcfClient<IPLMService>();
        private WcfClient<IFMSService> wsFms = new WcfClient<IFMSService>();
        private WcfClient<IEAMService> wsEAM = new WcfClient<IEAMService>();
        private WcfClient<ITMSService> wsTMS = new WcfClient<ITMSService>();
        private WcfClient<ISQLService> wsSQL = new WcfClient<ISQLService>();

        private string AreaPKNO = "";  //当前库区

        private bool bRefreshAllo = false;
        private string curAlloPKNO = "";

        public InventoryGraphic()
        {
            InitializeComponent();

            DataContext = viewModel;

            new Thread(RefreshAllocation).Start();
        }

        private void InventoryGraphic_OnLoaded(object sender, RoutedEventArgs e)
        {
            GetPage();
            bRefreshAllo = true;
        }

        private void InventoryGraphic_OnUnloaded(object sender, RoutedEventArgs e)
        {
            bRefreshAllo = false;
            myShelf.SetAutoRefreshState(0);
        }

        private void GetPage()
        {
            WmsAreaInfo area = ws.UseService(s => s.GetWmsAreaInfos($"USE_FLAG = 1 AND AREA_CODE = '轮毂立体料架'"))
                .FirstOrDefault();

            if (area != null)
            {
                viewModel.TotalColumn = SafeConverter.SafeToInt(area.TOTAL_COL, 6);
                viewModel.TotalLayer = SafeConverter.SafeToInt(area.TOTAL_LAY, 2);
                AreaPKNO = area.PKNO;

                cmbProductAlloc.ItemsSource =
                    ws.UseService(s => s.GetWmsAllocationInfos($"USE_FLAG = 1 AND AREA_PKNO = '{AreaPKNO}'"))
                        .OrderBy(c => c.ALLOCATION_NAME).ToList();
            }

            List<RsItemMaster> items = wsRsm.UseService(s => s.GetRsItemMasters("USE_FLAG = 1 AND NORM_CLASS < 100 "));  //物料信息

            cmbDevice.ItemsSource = items.Where(c => c.NORM_CLASS == 10).ToList();  //成品
            cmbCheck.ItemsSource = items.Where(c => c.NORM_CLASS == 10).ToList();    //成品

            cmbInRaw.ItemsSource = items.Where(c => c.NORM_CLASS < 10).ToList();  //原料、半成品
            cmbAddItem.ItemsSource = items;

            cmbProcessDevice.ItemsSource = wsEAM.UseService(s => s.GetAmAssetMasterNs("USE_FLAG = 1 AND ASSET_TYPE = '机床'"))
                .OrderBy(c => c.ASSET_NAME).ToList();

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
        }

        private Action<int, int, string, string, int, string> _showInfo;  //显示界面信息，提高效率

        private void RefreshAllocation()
        {
            #region 创建显示信息的函数，提高效率

            _showInfo = (col, lay, allInfo, goodsNO, allocProportion, palletInfo) =>
            {
                myShelf.RefreshAlloInfo(col, lay, allInfo, goodsNO, allocProportion, palletInfo);
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
                        s.GetWmsAllocationInfos($"USE_FLAG = 1 AND AREA_PKNO = '{AreaPKNO}'"));
                    List<WmsInventory> inventories = wsWMS.UseService(s => s.GetWmsInventorys($"AREA_PKNO = '{AreaPKNO}'"));  //库存信息
                    for (int col = 1; col <= viewModel.TotalColumn; col++)
                    {
                        for (int lay = 1; lay <= viewModel.TotalLayer; lay++)
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
                            Dispatcher.BeginInvoke(_showInfo, col1, lay1, allInfo, goodsNO, alloproportion, palletInfo);
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

        //入库
        private void BtnIn_Click(object sender, RoutedEventArgs e)
        {
            cmbInRaw.SelectedIndex = -1;
            TbInRemark.Text = "";
            viewModel.ActionType = 1;
        }

        //添加作业任务
        private void BtnProcess_Click(object sender, RoutedEventArgs e)
        {
            cmbPrcessParam.SelectedIndex = -1;
            if (cmbChangeProduct.Items.Count > 0) cmbChangeProduct.SelectedIndex = 0;
            BoxItem1.Visibility = Visibility.Collapsed;
            BoxItem2.Visibility = Visibility.Collapsed;
            BoxItem3.Visibility = Visibility.Collapsed;
            BoxItem4.Visibility = Visibility.Collapsed;
            BoxItem5.Visibility = Visibility.Collapsed;
            BoxItem6.Visibility = Visibility.Collapsed;

            if (viewModel.ActionType == 12) //毛坯货位
            {
                BoxItem1.Visibility = Visibility.Visible;
                BoxItem2.Visibility = Visibility.Visible;
                BoxItem3.Visibility = Visibility.Visible;
                BoxItem4.Visibility = Visibility.Visible;
                viewModel.ActionType = 2;
            }
            else if (viewModel.ActionType == 11)  //空货位
            {
                BoxItem5.Visibility = Visibility.Visible;
                BoxItem6.Visibility = Visibility.Visible;
                BoxItem4.Visibility = Visibility.Visible;
                viewModel.ActionType = 7;
            }
            else  //其他情况，只显示 4（只下料，首件）
            {
                BoxItem4.Visibility = Visibility.Visible;
                viewModel.ActionType = 6;
            }
        }

        //出库
        private void BtnOut_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ActionType = 3;
        }

        //转换
        private void BtnChange_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ActionType = 4;
        }

        //添加库存
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ActionType = 5;
        }

        //清空库存
        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            WmsAllocationInfo allocation = ws.UseService(s => s.GetWmsAllocationInfoById(curAlloPKNO));
            if ((allocation == null) || (allocation.ALLOCATION_STATE < 0)) return;

            if (WPFMessageBox.ShowConfirm("确定要清空该货位的库存及还原状态吗？", "清空货位") != WPFMessageBoxResult.OK)
            {
                return;
            }

            allocation.ALLOCATION_STATE = 0;
            ws.UseService(s => s.UpdateWmsAllocationInfo(allocation));

            WmsInventory inv = ws.UseService(s => s.GetWmsInventorys($"ALLOCATION_PKNO = '{allocation.PKNO}'"))
                .FirstOrDefault();

            if (inv != null) ws.UseService(s => s.DelWmsInventory(inv.PKNO));

            ShowAllocationInfo(myShelf.CurSelectedAllo.Column, myShelf.CurSelectedAllo.Layer);
        }

        //保存
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            WmsAllocationInfo allocation = ws.UseService(s => s.GetWmsAllocationInfoById(curAlloPKNO));
            if ((allocation == null) || (allocation.ALLOCATION_STATE < 0)) return;  //没有选择货位

            MesJobOrder jobOrder = null;
            WmsInventory inv = null;
            List<MesProcessCtrol> processCtrols = new List<MesProcessCtrol>();

            if (viewModel.ActionType == 1) //入库
            {
                if (cmbInRaw.SelectedIndex < 0)
                {
                    WPFMessageBox.ShowWarring("请选择原料类型！", "原料入库");
                    return;
                }

                RsItemMaster curProduct = wsRsm.UseService(s => s.GetRsItemMasterById(cmbInRaw.SelectedValue.ToString()));  //当前产品信息

                allocation.ALLOCATION_STATE = (allocation.ALLOCATION_STATE % 1000) + 3000; //入库锁定

                jobOrder = BuildNewJobOrder(cmbInRaw.SelectedValue.ToString(), 1, "手动入库", DateTime.Now);  //形成工单

                #region 形成生产过程表  => 原料入库

                string sFormulaCode = "原料入库";

                List<FmsActionFormulaDetail> formulaDetails = wsFms.UseService(s =>
                    s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails)
                {
                    MesProcessCtrol process = new MesProcessCtrol()
                    {
                        #region 标准信息

                        PKNO = CBaseData.NewGuid(),
                        COMPANY_CODE = CBaseData.BelongCompPKNO,
                        ITEM_PKNO = cmbInRaw.SelectedValue.ToString(),
                        JOB_ORDER_PKNO = jobOrder.PKNO,
                        JOB_ORDER = jobOrder.JOB_ORDER_NO,
                        SUB_JOB_ORDER_NO = "",
                        ROUTING_DETAIL_PKNO = "",  //

                        #endregion

                        PROCESS_CTROL_NAME = detail.FORMULA_DETAIL_NAME,  //名称
                        PROCESS_DEVICE_PKNO = detail.PROCESS_DEVICE_PKNO,             //生产设备
                        PROCESS_PROGRAM_NO = detail.PROCESS_PROGRAM_NO,              //加工程序号
                        PROCESS_PROGRAM_CONTENT = detail.PROCESS_PROGRAM_CONTENT,         //加工程序内容
                        PROCESS_INDEX = detail.PROCESS_INDEX,                   //工序顺序
                        BEGIN_ITEM_PKNO = detail.BEGIN_ITEM_PKNO?.Replace("{原料PKNO}", cmbInRaw.SelectedValue.ToString()),                 //生产前项目PKNO
                        FINISH_ITEM_PKNO = detail.FINISH_ITEM_PKNO?.Replace("{成品PKNO}", cmbInRaw.SelectedValue.ToString()),                //生产后项目PKNO
                        BEGIN_POSITION = detail.BEGIN_POSITION,                  //生产前位置
                        FINISH_POSITION = detail.FINISH_POSITION?.Replace("{目标货位}", allocation.PKNO),                 //生产后位置
                        PALLET_NO = detail.PALLET_NO,                       //托盘号
                        PROCESS_ACTION_TYPE = detail.PROCESS_ACTION_TYPE,          //工序动作类型
                        PROCESS_ACTION_PKNO = detail.PROCESS_ACTION_PKNO,             //工序动作控制PKNO
                        PROCESS_ACTION_PARAM1_VALUE = detail.PROCESS_ACTION_PARAM1_VALUE?.Replace("{轮毂型号}", curProduct.ITEM_NORM),     //工序动作参数1
                        PROCESS_ACTION_PARAM2_VALUE = detail.PROCESS_ACTION_PARAM2_VALUE?.Replace("{目标货位}", allocation.INTERFACE_NAME),     //工序动作参数2

                        CUR_PRODUCT_CODE_PKNO = "",           //当前生产加工的产品编码PKNO
                        PROCESS_QTY = 1,                     //加工数量（上线数量）
                        COMPLETE_QTY = 0,   //完成数量
                        QUALIFIED_QTY = 0,  //合格数量
                        PROCESS_STATE = 1,  //准备完成

                        CREATION_DATE = DateTime.Now,                   //创建日期
                        CREATED_BY = CBaseData.LoginNO,                      //创建人
                        LAST_UPDATE_DATE = DateTime.Now,  //最后修改日期
                        USE_FLAG = detail.USE_FLAG,                        //启用标识
                        REMARK = TbInRemark.Text,                          //备注
                    };

                    processCtrols.Add(process);
                }

                #endregion

            }
            else if ((viewModel.ActionType == 2) || (viewModel.ActionType == 6) || (viewModel.ActionType == 7)) //手动添加任务
            {
                string prodPKNO = SafeConverter.SafeToStr(cmbChangeProduct.SelectedValue);
                if (string.IsNullOrEmpty(prodPKNO))
                {
                    prodPKNO = SafeConverter.SafeToStr(cmbCheck.SelectedValue);
                    if (string.IsNullOrEmpty(prodPKNO))
                    {
                        prodPKNO = SafeConverter.SafeToStr(cmbDevice.SelectedValue);
                    }
                }
                RsItemMaster curProduct = wsRsm.UseService(s => s.GetRsItemMasterById(prodPKNO));  //当前产品信息

                WmsAllocationInfo proAllocationInfo = cmbProductAlloc.SelectedItem as WmsAllocationInfo;

                if ((viewModel.ActionType == 6) || (viewModel.ActionType == 7))  //空货位
                {
                    if (proAllocationInfo == null)
                    {
                        WPFMessageBox.ShowError("成品货位位置不正确，请重新选择", "手动添加任务");
                        return;
                    }
                    proAllocationInfo.ALLOCATION_STATE = (proAllocationInfo.ALLOCATION_STATE % 1000) + 3000; //入库锁定
                }

                #region 检验 

                if (cmbPrcessParam.SelectedValue == null)
                {
                    WPFMessageBox.ShowError("请选择工艺参数", "手动添加任务");
                    return;
                }

                AmAssetMasterN deviceProcess = cmbProcessDevice.SelectedItem as AmAssetMasterN;

                if ((cmbProcessDevice.IsEnabled) && (cmbProcessDevice.Visibility == Visibility.Visible)) //带加工
                {
                    if (deviceProcess == null)
                    {
                        WPFMessageBox.ShowError("请选择加工设备.", "手动添加任务");
                        return;
                    }
                }

                if (TbProgramNO.IsEnabled && string.IsNullOrEmpty(TbProgramNO.Text))
                {
                    WPFMessageBox.ShowError("请输入加工程序号.", "手动添加任务");
                    return;
                }

                if (cmbDevice.IsEnabled && (cmbDevice.SelectedValue == null))
                {
                    WPFMessageBox.ShowError("请选择机床轮毂型号.", "手动添加任务");
                    return;
                }

                if (cmbCheck.IsEnabled && (cmbCheck.SelectedValue == null))
                {
                    WPFMessageBox.ShowError("请选择三坐标轮毂型号.", "手动添加任务");
                    return;
                }

                #endregion //成品货位信息

                if (viewModel.ActionType == 2)
                {
                    allocation.ALLOCATION_STATE = (allocation.ALLOCATION_STATE % 1000) + 4000;  //出库锁定
                }
                else if ((viewModel.ActionType == 6) || (viewModel.ActionType == 7)) //空货位
                {
                    allocation.ALLOCATION_STATE = (allocation.ALLOCATION_STATE % 1000) + 3000; //入库锁定
                }

                #region 形成工单

                jobOrder = new MesJobOrder()
                {
                    PKNO = CBaseData.NewGuid(),
                    COMPANY_CODE = CBaseData.BelongCompPKNO,
                    LINE_PKNO = CBaseData.CurLinePKNO,
                    LINE_TASK_PKNO = "", //产线任务PKNO
                    ITEM_PKNO = curProduct?.PKNO,
                    JOB_ORDER_NO = TableNOHelper.GetNewNO("MES_JOB_ORDER.JOB_ORDER_NO", "J"),
                    BATCH_NO = "手动任务",
                    ROUTING_DETAIL_PKNO = "",
                    JOB_ORDER_TYPE = 2, //工单类型 1：原料入库；2：加工；3：成品出库；4：转换
                    TASK_QTY = 1,
                    COMPLETE_QTY = 0,
                    ONLINE_QTY = 0,
                    ONCE_QTY = 0,
                    RUN_STATE = 10,  //直接生产
                    CREATION_DATE = DateTime.Now,
                    CREATED_BY = CBaseData.LoginNO,
                    LAST_UPDATE_DATE = DateTime.Now,  //最后修改日期
                    USE_FLAG = 1,
                    REMARK = "",
                };

                #endregion

                #region 增加参数

                Dictionary<string, string> paramValues = new Dictionary<string, string>();
                paramValues.Add("{生产设备}", deviceProcess?.ASSET_CODE);  //生产设备
                paramValues.Add("{加工机床}", deviceProcess?.ASSET_LABEL);  //加工机床
                paramValues.Add("{加工程序号}", TbProgramNO.Text);  //加工程序号
                paramValues.Add("{原料PKNO}", viewModel.CurAlloItemPKNO);  //原料PKNO
                paramValues.Add("{成品PKNO}", curProduct?.PKNO);  //成品PKNO
                paramValues.Add("{原始货位PKNO}", allocation.PKNO);  //原始货位PKNO
                paramValues.Add("{成品货位PKNO}", proAllocationInfo?.PKNO);  //成品货位PKNO
                paramValues.Add("{原始货位}", allocation.INTERFACE_NAME);  //原始货位
                paramValues.Add("{成品货位}", proAllocationInfo?.INTERFACE_NAME);  //成品货位
                paramValues.Add("{轮毂型号}", curProduct?.ITEM_NORM);  //轮毂型号
                paramValues.Add("{机床轮毂型号}", (cmbDevice.SelectedItem as RsItemMaster)?.ITEM_NORM);  //机床轮毂型号
                paramValues.Add("{机床轮毂PKNO}", (cmbDevice.SelectedItem as RsItemMaster)?.PKNO);  //机床轮毂PKNO
                paramValues.Add("{三坐标轮毂型号}", (cmbCheck.SelectedItem as RsItemMaster)?.ITEM_NORM);  //三坐标轮毂型号
                paramValues.Add("{三坐标轮毂PKNO}", (cmbCheck.SelectedItem as RsItemMaster)?.PKNO);  //三坐标轮毂PKNO
                paramValues.Add("{检测程序}",
                    "WHSIZE" + ((cmbDevice.SelectedItem as RsItemMaster)?.ITEM_NORM == "1"
                        ? "16"
                        : ((cmbDevice.SelectedItem as RsItemMaster)?.ITEM_NORM == "2" ? "17" : "18")));  //检测程序，按照机床里面的轮毂型号进行检测
                paramValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss"));  //检测时间

                #endregion

                string sFormulaCode = "轮毂生产-6";
                if (cmbPrcessParam.SelectedValue.ToString() != "6")
                {
                    sFormulaCode = "轮毂生产-" + cmbPrcessParam.SelectedValue.ToString() + "-" + cmbProcessDevice.SelectedValue.ToString();
                }

                #region 形成生产过程表  => 原料加工

                List<FmsActionFormulaDetail> formulaDetails = wsFms.UseService(s =>
                    s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails)  //配方
                {
                    MesProcessCtrol process = new MesProcessCtrol()
                    {
                        #region 标准信息

                        PKNO = CBaseData.NewGuid(),
                        COMPANY_CODE = CBaseData.BelongCompPKNO,
                        ITEM_PKNO = cmbChangeProduct.SelectedValue?.ToString(),   //成品PKNO
                        JOB_ORDER_PKNO = jobOrder.PKNO,
                        JOB_ORDER = jobOrder.JOB_ORDER_NO,
                        SUB_JOB_ORDER_NO = "",
                        ROUTING_DETAIL_PKNO = "",  //

                        #endregion

                        PROCESS_CTROL_NAME = detail.FORMULA_DETAIL_NAME,  //名称
                        PROCESS_DEVICE_PKNO = ProcessParamReplace.Replace(detail.PROCESS_DEVICE_PKNO, paramValues),             //生产设备
                        PROCESS_PROGRAM_NO = ProcessParamReplace.Replace(detail.PROCESS_PROGRAM_NO, paramValues),              //加工程序号
                        PROCESS_PROGRAM_CONTENT = detail.PROCESS_PROGRAM_CONTENT,         //加工程序内容
                        PROCESS_INDEX = detail.PROCESS_INDEX,                   //工序顺序
                        BEGIN_ITEM_PKNO = ProcessParamReplace.Replace(detail.BEGIN_ITEM_PKNO, paramValues),                 //生产前项目PKNO
                        FINISH_ITEM_PKNO = ProcessParamReplace.Replace(detail.FINISH_ITEM_PKNO, paramValues),                //生产后项目PKNO
                        BEGIN_POSITION = ProcessParamReplace.Replace(detail.BEGIN_POSITION, paramValues),                  //生产前位置
                        FINISH_POSITION = ProcessParamReplace.Replace(detail.FINISH_POSITION, paramValues),                 //生产后位置
                        PALLET_NO = detail.PALLET_NO,                       //托盘号
                        PROCESS_ACTION_TYPE = detail.PROCESS_ACTION_TYPE,          //工序动作类型
                        PROCESS_ACTION_PKNO = detail.PROCESS_ACTION_PKNO,             //工序动作控制PKNO

                        PROCESS_ACTION_PARAM1_VALUE = ProcessParamReplace.Replace(detail.PROCESS_ACTION_PARAM1_VALUE, paramValues),     //工序动作参数1
                        PROCESS_ACTION_PARAM2_VALUE = ProcessParamReplace.Replace(detail.PROCESS_ACTION_PARAM2_VALUE, paramValues),     //工序动作参数2

                        CUR_PRODUCT_CODE_PKNO = "",           //当前生产加工的产品编码PKNO
                        PROCESS_QTY = 1,                     //加工数量（上线数量）
                        COMPLETE_QTY = 0,   //完成数量
                        QUALIFIED_QTY = 0,  //合格数量
                        PROCESS_STATE = 1,  //准备完成

                        CREATION_DATE = DateTime.Now,                   //创建日期
                        CREATED_BY = CBaseData.LoginNO,                      //创建人
                        LAST_UPDATE_DATE = DateTime.Now,  //最后修改日期
                        USE_FLAG = detail.USE_FLAG,                        //启用标识
                        REMARK = TbProcessRemark.Text,                          //备注
                    };

                    processCtrols.Add(process);
                }

                #endregion
            }
            else if (viewModel.ActionType == 3) //出库
            {
                RsItemMaster curProduct = wsRsm.UseService(s => s.GetRsItemMasterById(viewModel.CurAlloItemPKNO));  //当前产品信息

                allocation.ALLOCATION_STATE = (allocation.ALLOCATION_STATE % 1000) + 4000;  //出库锁定

                #region 形成工单

                jobOrder = new MesJobOrder()
                {
                    PKNO = CBaseData.NewGuid(),
                    COMPANY_CODE = CBaseData.BelongCompPKNO,
                    LINE_PKNO = CBaseData.CurLinePKNO,
                    LINE_TASK_PKNO = "", //产线任务PKNO
                    ITEM_PKNO = viewModel.CurAlloItemPKNO,  //库存物料
                    JOB_ORDER_NO = TableNOHelper.GetNewNO("MES_JOB_ORDER.JOB_ORDER_NO", "J"),
                    BATCH_NO = "手动出库",
                    ROUTING_DETAIL_PKNO = "",
                    JOB_ORDER_TYPE = 3, //工单类型 1：原料入库；2：加工；3：成品出库；4：转换
                    TASK_QTY = 1,
                    COMPLETE_QTY = 0,
                    ONLINE_QTY = 0,
                    ONCE_QTY = 0,
                    RUN_STATE = 10,  //直接生产
                    CREATION_DATE = DateTime.Now,
                    CREATED_BY = CBaseData.LoginNO,
                    LAST_UPDATE_DATE = DateTime.Now,  //最后修改日期
                    USE_FLAG = 1,
                    REMARK = "",
                };

                #endregion

                #region 形成生产过程表  => 成品出库

                string sFormulaCode = "成品出库";

                List<FmsActionFormulaDetail> formulaDetails = wsFms.UseService(s =>
                    s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails)
                {
                    MesProcessCtrol process = new MesProcessCtrol()
                    {
                        #region 标准信息

                        PKNO = CBaseData.NewGuid(),
                        COMPANY_CODE = CBaseData.BelongCompPKNO,
                        ITEM_PKNO = viewModel.CurAlloItemPKNO,
                        JOB_ORDER_PKNO = jobOrder.PKNO,
                        JOB_ORDER = jobOrder.JOB_ORDER_NO,
                        SUB_JOB_ORDER_NO = "",
                        ROUTING_DETAIL_PKNO = "",  //

                        #endregion

                        PROCESS_CTROL_NAME = detail.FORMULA_DETAIL_NAME,  //名称
                        PROCESS_DEVICE_PKNO = detail.PROCESS_DEVICE_PKNO,             //生产设备
                        PROCESS_PROGRAM_NO = detail.PROCESS_PROGRAM_NO,              //加工程序号
                        PROCESS_PROGRAM_CONTENT = detail.PROCESS_PROGRAM_CONTENT,         //加工程序内容
                        PROCESS_INDEX = detail.PROCESS_INDEX,                   //工序顺序
                        BEGIN_ITEM_PKNO = detail.BEGIN_ITEM_PKNO?.Replace("{原料PKNO}", viewModel.CurAlloItemPKNO),                 //生产前项目PKNO
                        FINISH_ITEM_PKNO = detail.FINISH_ITEM_PKNO?.Replace("{成品PKNO}", viewModel.CurAlloItemPKNO),                //生产后项目PKNO
                        BEGIN_POSITION = detail.BEGIN_POSITION?.Replace("{原始货位}", allocation.PKNO),                  //生产前位置
                        FINISH_POSITION = detail.FINISH_POSITION,                 //生产后位置
                        PALLET_NO = detail.PALLET_NO,                       //托盘号
                        PROCESS_ACTION_TYPE = detail.PROCESS_ACTION_TYPE,          //工序动作类型
                        PROCESS_ACTION_PKNO = detail.PROCESS_ACTION_PKNO,             //工序动作控制PKNO
                        PROCESS_ACTION_PARAM1_VALUE = detail.PROCESS_ACTION_PARAM1_VALUE?.Replace("{原始货位}", allocation.INTERFACE_NAME),     //工序动作参数1
                        PROCESS_ACTION_PARAM2_VALUE = detail.PROCESS_ACTION_PARAM2_VALUE?.Replace("{轮毂型号}", curProduct?.ITEM_NORM),     //工序动作参数2

                        CUR_PRODUCT_CODE_PKNO = "",           //当前生产加工的产品编码PKNO
                        PROCESS_QTY = 1,                     //加工数量（上线数量）
                        COMPLETE_QTY = 0,   //完成数量
                        QUALIFIED_QTY = 0,  //合格数量
                        PROCESS_STATE = 1,  //准备完成

                        CREATION_DATE = DateTime.Now,                   //创建日期
                        CREATED_BY = CBaseData.LoginNO,                      //创建人
                        LAST_UPDATE_DATE = DateTime.Now,  //最后修改日期
                        USE_FLAG = detail.USE_FLAG,                        //启用标识
                        REMARK = TbInRemark.Text,                          //备注
                    };

                    processCtrols.Add(process);
                }

                #endregion
            }
            else if (viewModel.ActionType == 4) //转换
            {
                #region 形成工单

                jobOrder = new MesJobOrder()
                {
                    PKNO = CBaseData.NewGuid(),
                    COMPANY_CODE = CBaseData.BelongCompPKNO,
                    LINE_PKNO = CBaseData.CurLinePKNO,
                    LINE_TASK_PKNO = "", //产线任务PKNO
                    ITEM_PKNO = cmbChangeRaw.SelectedValue.ToString(),
                    JOB_ORDER_NO = TableNOHelper.GetNewNO("MES_JOB_ORDER.JOB_ORDER_NO", "J"),
                    BATCH_NO = "手动转换",
                    ROUTING_DETAIL_PKNO = "",
                    JOB_ORDER_TYPE = 4, //工单类型 1：原料入库；2：加工；3：成品出库；4：转换
                    TASK_QTY = 1,
                    ACT_START_TIME = DateTime.Now,
                    ACT_FINISH_TIME = DateTime.Now,
                    COMPLETE_QTY = 1,
                    ONLINE_QTY = 1,
                    ONCE_QTY = 1,
                    RUN_STATE = 100,  //执行完成
                    CREATION_DATE = DateTime.Now,
                    CREATED_BY = CBaseData.LoginNO,
                    LAST_UPDATE_DATE = DateTime.Now,  //最后修改日期
                    USE_FLAG = 1,
                    REMARK = "",
                };

                #endregion

                //不需要控制
                WmsInventory inventory = ws.UseService(s =>
                        s.GetWmsInventorys($"AREA_PKNO = '{AreaPKNO}' AND ALLOCATION_PKNO = '{allocation.PKNO}'"))
                    .FirstOrDefault(); //库存信息

                if (inventory != null)
                {
                    inventory.MATERIAL_PKNO = cmbChangeRaw.SelectedValue.ToString();
                    inventory.REMARK = tbChangeRemark.Text;

                    ws.UseService(s => s.UpdateWmsInventory(inventory)); //修改库存
                }
            }
            else if (viewModel.ActionType == 5)  //新增库存
            {
                if (cmbAddItem.SelectedIndex < 0)
                {
                    WPFMessageBox.ShowWarring("请选择需要增加库存的物料信息！", "新增库存");
                    return;
                }

                allocation.ALLOCATION_STATE = 100; //满货位

                inv = new WmsInventory()
                {
                    PKNO = CBaseData.NewGuid(),
                    COMPANY_CODE = "",
                    MATERIAL_PKNO = cmbAddItem.SelectedValue?.ToString(),
                    ALLOCATION_PKNO = allocation.PKNO,
                    AREA_PKNO = AreaPKNO,
                    BATCH_NO = "",
                    INVENTORY_NUM = 1,
                    REMARK = TbAddRemark.Text,
                };  //库存
            }

            Cursor = Cursors.Wait;

            ws.UseService(s => s.UpdateWmsAllocationInfo(allocation));  //修改

            if (jobOrder != null)
            {
                wsPlm.UseService(s => s.AddMesJobOrder(jobOrder));
            }

            if (inv != null)
            {
                ws.UseService(s => s.AddWmsInventory(inv));
            }

            foreach (MesProcessCtrol processCtrol in processCtrols)
            {
                wsPlm.UseService(s => s.AddMesProcessCtrol(processCtrol));
            }

            Cursor = Cursors.Arrow;

            ShowAllocationInfo(myShelf.CurSelectedAllo.Column, myShelf.CurSelectedAllo.Layer);
        }

        //测试换刀
        private void btnTestChange_Click(object sender, RoutedEventArgs e)
        {
            MesJobOrder jobOrder = null;
            WmsInventory inv = null;
            List<MesProcessCtrol> processCtrols = new List<MesProcessCtrol>();

            #region 形成工单

            jobOrder = new MesJobOrder()
            {
                PKNO = CBaseData.NewGuid(),
                COMPANY_CODE = CBaseData.BelongCompPKNO,
                LINE_PKNO = CBaseData.CurLinePKNO,
                LINE_TASK_PKNO = "", //产线任务PKNO
                ITEM_PKNO = viewModel.CurAlloItemPKNO,
                JOB_ORDER_NO = TableNOHelper.GetNewNO("MES_JOB_ORDER.JOB_ORDER_NO", "J"),
                BATCH_NO = "测试换刀",
                ROUTING_DETAIL_PKNO = "",
                JOB_ORDER_TYPE = 2, //工单类型 1：原料入库；2：加工；3：成品出库；4：转换
                TASK_QTY = 1,
                COMPLETE_QTY = 0,
                ONLINE_QTY = 0,
                ONCE_QTY = 0,
                RUN_STATE = 10,  //直接生产
                CREATION_DATE = DateTime.Now,
                CREATED_BY = CBaseData.LoginNO,
                LAST_UPDATE_DATE = DateTime.Now,  //最后修改日期
                USE_FLAG = 1,
                REMARK = "",
            };

            #endregion

            #region 增加参数

            Dictionary<string, string> ParamValues = new Dictionary<string, string>();

            #endregion

            string sFormulaCode = "换刀";

            List<FmsActionFormulaDetail> formulaDetails = wsFms.UseService(s =>
                s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                .OrderBy(c => c.PROCESS_INDEX)
                .ToList();

            foreach (var detail in formulaDetails)  //配方
            {
                MesProcessCtrol process = new MesProcessCtrol()
                {
                    #region 标准信息

                    PKNO = CBaseData.NewGuid(),
                    COMPANY_CODE = CBaseData.BelongCompPKNO,
                    ITEM_PKNO = cmbChangeProduct.SelectedValue.ToString(),   //成品PKNO
                    JOB_ORDER_PKNO = jobOrder.PKNO,
                    JOB_ORDER = jobOrder.JOB_ORDER_NO,
                    SUB_JOB_ORDER_NO = "",
                    ROUTING_DETAIL_PKNO = "",  //

                    #endregion

                    PROCESS_CTROL_NAME = detail.FORMULA_DETAIL_NAME,  //名称
                    PROCESS_DEVICE_PKNO = ProcessParamReplace.Replace(detail.PROCESS_DEVICE_PKNO, ParamValues),             //生产设备
                    PROCESS_PROGRAM_NO = ProcessParamReplace.Replace(detail.PROCESS_PROGRAM_NO, ParamValues),              //加工程序号
                    PROCESS_PROGRAM_CONTENT = detail.PROCESS_PROGRAM_CONTENT,         //加工程序内容
                    PROCESS_INDEX = detail.PROCESS_INDEX,                   //工序顺序
                    BEGIN_ITEM_PKNO = ProcessParamReplace.Replace(detail.BEGIN_ITEM_PKNO, ParamValues),                 //生产前项目PKNO
                    FINISH_ITEM_PKNO = ProcessParamReplace.Replace(detail.FINISH_ITEM_PKNO, ParamValues),                //生产后项目PKNO
                    BEGIN_POSITION = ProcessParamReplace.Replace(detail.BEGIN_POSITION, ParamValues),                  //生产前位置
                    FINISH_POSITION = ProcessParamReplace.Replace(detail.FINISH_POSITION, ParamValues),                 //生产后位置
                    PALLET_NO = detail.PALLET_NO,                       //托盘号
                    PROCESS_ACTION_TYPE = detail.PROCESS_ACTION_TYPE,          //工序动作类型
                    PROCESS_ACTION_PKNO = detail.PROCESS_ACTION_PKNO,             //工序动作控制PKNO

                    PROCESS_ACTION_PARAM1_VALUE = ProcessParamReplace.Replace(detail.PROCESS_ACTION_PARAM1_VALUE, ParamValues),     //工序动作参数1
                    PROCESS_ACTION_PARAM2_VALUE = ProcessParamReplace.Replace(detail.PROCESS_ACTION_PARAM2_VALUE, ParamValues),     //工序动作参数2

                    CUR_PRODUCT_CODE_PKNO = "",           //当前生产加工的产品编码PKNO
                    PROCESS_QTY = 1,                     //加工数量（上线数量）
                    COMPLETE_QTY = 0,   //完成数量
                    QUALIFIED_QTY = 0,  //合格数量
                    PROCESS_STATE = 1,  //准备完成

                    CREATION_DATE = DateTime.Now,                   //创建日期
                    CREATED_BY = CBaseData.LoginNO,                      //创建人
                    LAST_UPDATE_DATE = DateTime.Now,  //最后修改日期
                    USE_FLAG = detail.USE_FLAG,                        //启用标识
                    REMARK = TbProcessRemark.Text,                          //备注
                };

                processCtrols.Add(process);
            }

            Cursor = Cursors.Wait;

            wsPlm.UseService(s => s.AddMesJobOrder(jobOrder));

            foreach (MesProcessCtrol processCtrol in processCtrols)
            {
                wsPlm.UseService(s => s.AddMesProcessCtrol(processCtrol));
            }

            Cursor = Cursors.Arrow;

            MessageBox.Show("换刀下单成功.");
        }

        //取消
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            ShowAllocationInfo(myShelf.CurSelectedAllo.Column, myShelf.CurSelectedAllo.Layer);
        }

        //选择
        private void MyShelf_MyMouseClickEvent(object sender, ShelfClickMouseEventArg args)
        {
            ShowAllocationInfo(myShelf.CurSelectedAllo.Column, myShelf.CurSelectedAllo.Layer);
        }

        //显示
        private void ShowAllocationInfo(int col, int lay)
        {
            viewModel.ActionType = 0;  //类型
            curAlloPKNO = "";

            viewModel.CurAlloCol = col;
            viewModel.CurAlloLay = lay;
            viewModel.CurAlloName = "已禁用";
            viewModel.CurAlloItemPKNO = "";
            viewModel.CurAlloItemName = "空";
            viewModel.CurAlloItemType = "";
            viewModel.CurAlloItemNorm = "";
            viewModel.CurAlloNumber = "";
            viewModel.CurInvRemark = "";
            viewModel.ALLOCATION_CODE = "";

            cmbChangeProduct.SelectedIndex = -1;
            cmbChangeRaw.SelectedIndex = -1;

            WmsAllocationInfo allocation = ws.UseService(s =>
                    s.GetWmsAllocationInfos(
                        $"USE_FLAG = 1 AND AREA_PKNO = '{AreaPKNO}' AND ALLOCATION_COL = {col} AND ALLOCATION_LAY = {lay}"))
                .FirstOrDefault();
            if (allocation != null)
            {
                curAlloPKNO = allocation.PKNO;
                viewModel.ALLOCATION_CODE = curAlloPKNO;
                viewModel.CurAlloName = allocation.ALLOCATION_NAME;
                viewModel.CurInvRemark = allocation.REMARK;

                cmbProductAlloc.SelectedValue = allocation.PKNO;

                WmsInventory inventory = ws.UseService(s =>
                        s.GetWmsInventorys($"AREA_PKNO = '{AreaPKNO}' AND ALLOCATION_PKNO = '{allocation.PKNO}'"))
                    .FirstOrDefault(); //库存信息
                if (inventory != null)  //库存存在
                {
                    viewModel.CurAlloNumber = inventory.INVENTORY_NUM?.ToString("f0");
                    RsItemMaster item = wsRsm.UseService(s => s.GetRsItemMasterById(inventory.MATERIAL_PKNO));
                    if (item != null)
                    {
                        viewModel.CurAlloItemPKNO = item.PKNO;
                        viewModel.CurAlloItemName = item.ITEM_NAME;
                        viewModel.CurAlloItemNorm = item.ITEM_NORM;

                        if (item.NORM_CLASS == 1)
                        {
                            var child = wsRsm.UseService(s => s.GetRsBoms($"ITEM_PKNO = '{item.PKNO}'")).FirstOrDefault();
                            if (child != null)
                            {
                                viewModel.ActionType = 12;
                                var product = wsRsm.UseService(s => s.GetRsBoms($"PKNO = '{child.PARENT_PKNO}'"));
                                cmbChangeProduct.ItemsSource = product;
                                if (cmbChangeProduct.Items.Count > 0) cmbChangeProduct.SelectedIndex = 0;
                            }
                        }
                        else if (item.NORM_CLASS == 10)  //成品
                        {
                            var parent = wsRsm.UseService(s => s.GetRsBoms($"ITEM_PKNO = '{item.PKNO}'")).FirstOrDefault();
                            if (parent != null)
                            {
                                viewModel.ActionType = 13;
                                var raw = wsRsm.UseService(s => s.GetRsBoms($"PARENT_PKNO = '{parent.PKNO}'"));
                                cmbChangeRaw.ItemsSource = raw;
                                if (cmbChangeRaw.Items.Count > 0) cmbChangeRaw.SelectedIndex = 0;
                            }
                        }
                        else
                        {
                            viewModel.ActionType = 14;
                        }

                        viewModel.CurAlloItemType = (item.NORM_CLASS == 1) ? "原料" :
                            (item.NORM_CLASS == 2) ? "半成品" :
                            (item.NORM_CLASS == 10) ? "成品" :
                            (item.NORM_CLASS == 101) ? "刀具" : "";
                    }
                }
                else  //没有库存
                {
                    viewModel.ActionType = 11;  //类型
                }

                if (allocation.ALLOCATION_STATE < 0 || allocation.ALLOCATION_STATE >= 1000)  //锁定情况下不能用
                {
                    viewModel.ActionType = 0;
                }
            }
        }

        //演示流程，
        //需要条件：16寸（毛坯库存）2个、17寸（毛坯库存）1个、18寸（毛坯库存）2个
        //          换刀 1#机床 T10刀 更换成自动货柜中的 38号刀
        private void bBeginShow_Click(object sender, RoutedEventArgs e)
        {
            WcfClient<IPLMService> ws2 = new WcfClient<IPLMService>();
            List<MesJobOrder> mesJobOrders =
                ws2.UseService(s =>
                        s.GetMesJobOrders(
                            $"USE_FLAG = 1 AND RUN_STATE < 100 AND LINE_PKNO = '{CBaseData.CurLinePKNO}'"))
                    .OrderBy(c => c.CREATION_DATE).ToList();
            if (mesJobOrders.Count > 0)
            {
                WaitLoading.SetDefault(this);
                WPFMessageBox.ShowError("目前还有未完成的任务，请将任务完成后再进行演示流程！", "演示程序");
                return;
            }

            if (WPFMessageBox.ShowConfirm("确定要开始演示程序吗？\r\n" +
                                          "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\r\n" +
                                          "请确保：\r\n1.机床、三坐标上都没有轮毂!!!\r\n" +
                                          "2.实际料架上货位[14]、[15]为18寸轮毂，[13]为17寸轮毂，[24]、[25]为16寸轮毂!!!\r\n3.料架上的实物轮毂都是成品!!!\r\n" +
                                          "4.三坐标上的卡盘开口为18寸轮毂的尺寸!!!\r\n5.1#机床（创胜特尔）上的[T10]刀为替换刀!!!\r\n" +
                                          "6.刀具编号为[38]的刀具在自动货柜中!!!\r\n7.各设备处于正常且联机自动状态!!!\r\n" +
                                          "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\r\n",
                    "1#演示流程") != WPFMessageBoxResult.OK)
            {
                return;
            }

            //后台执行添加
            new Thread(delegate ()
            {
                WaitLoading.SetWait(this);
                Thread.Sleep(1000);

                DateTime jobOrderTime = DateTime.Now.AddSeconds(-10);
                int iJobOrderIndex = 0;

                List<MesJobOrder> jobOrders = new List<MesJobOrder>(); //所有订单
                List<MesProcessCtrol> processCtrols = new List<MesProcessCtrol>(); //控制流程
                List<WmsAllocationInfo> allocationInfos = new List<WmsAllocationInfo>(); //需要修改的货位

                Dictionary<string, string> ParamValues = new Dictionary<string, string>();
                MesJobOrder job = null;
                string sFormulaCode = "";
                List<FmsActionFormulaDetail> formulaDetails;

                AmAssetMasterN asset1 = wsEAM.UseService(s => s.GetAmAssetMasterNs("ASSET_CODE = 'SH00003'"))
                    .FirstOrDefault(); //1#机床 - 斌胜
                AmAssetMasterN asset2 = wsEAM.UseService(s => s.GetAmAssetMasterNs("ASSET_CODE = 'SH00002'"))
                    .FirstOrDefault(); //2#机床 - 京元登
                AmAssetMasterN asset3 = wsEAM.UseService(s => s.GetAmAssetMasterNs("ASSET_CODE = 'SH00001'"))
                    .FirstOrDefault(); //3#机床 - 创圣特尔
                if ((asset1 == null) || (asset2 == null) || (asset3 == null))
                {
                    WaitLoading.SetDefault(this);
                    WPFMessageBox.ShowError("机床信息没有正确初始化，请联系管理员进行配置.", "演示程序");
                    return;
                }

                List<RsItemMaster> items = wsRsm.UseService(s => s.GetRsItemMasters("USE_FLAG = 1"));

                RsItemMaster product16 = items.FirstOrDefault(c => c.ITEM_NORM == "1" && c.NORM_CLASS == 10); //产品信息
                RsItemMaster product17 = items.FirstOrDefault(c => c.ITEM_NORM == "2" && c.NORM_CLASS == 10); //产品信息
                RsItemMaster product18 = items.FirstOrDefault(c => c.ITEM_NORM == "3" && c.NORM_CLASS == 10); //产品信息

                RsItemMaster raw16 = items.FirstOrDefault(c => c.ITEM_NORM == "1" && c.NORM_CLASS == 1); //原料信息
                RsItemMaster raw17 = items.FirstOrDefault(c => c.ITEM_NORM == "2" && c.NORM_CLASS == 1); //原料信息
                RsItemMaster raw18 = items.FirstOrDefault(c => c.ITEM_NORM == "3" && c.NORM_CLASS == 1); //原料信息
                if ((raw16 == null) || (raw17 == null) || (raw18 == null) || (product16 == null) ||
                    (product17 == null) || (product18 == null))
                {
                    WaitLoading.SetDefault(this);
                    WPFMessageBox.ShowError("轮毂物料信息没有正确初始化，请联系管理员进行配置.", "演示程序");
                    return;
                }

                string program16 = "70"; //加工程序 16寸
                string program17 = "80"; //加工程序 17寸
                string program18 = "90"; //加工程序 18寸



                #region 1. 1#机床换刀  T10 => 38

                TmsToolsMaster mToolsMasterUp = wsTMS.UseService(s => s.GetTmsToolsMasters("TOOLS_CODE = 38")).FirstOrDefault();  //刀具库存
                if ((mToolsMasterUp == null) || (mToolsMasterUp.TOOLS_POSITION != 1))
                {
                    WaitLoading.SetDefault(this);
                    WPFMessageBox.ShowError("刀具【38】没有在库，请核实状态后再进行下达指令.", "演示程序");
                    return;
                }

                TmsDeviceToolsPos mTmsDeviceToolsPos = wsTMS.UseService(s =>
                        s.GetTmsDeviceToolsPoss($"DEVICE_PKNO = '{asset1.PKNO}' AND TOOLS_POS_NO = 10 AND USE_FLAG = 1"))
                    .FirstOrDefault();  //机床刀位信息
                if (mTmsDeviceToolsPos == null)
                {
                    WaitLoading.SetDefault(this);
                    WPFMessageBox.ShowError("系统没有正确初始化，请联系管理员进行配置.", "演示程序");
                    return;
                }

                TmsToolsMaster mToolsMasterDown =
                    wsTMS.UseService(s => s.GetTmsToolsMasterById(mTmsDeviceToolsPos.TOOLS_PKNO));
                if (mToolsMasterDown == null)
                {
                    WaitLoading.SetDefault(this);
                    WPFMessageBox.ShowError("选中的需要换下的机床刀具信息不存在，请核实.", "演示程序");
                    return;
                }

                #region 增加参数

                ParamValues.Clear();

                ParamValues.Add("{机床刀号}", mTmsDeviceToolsPos.TOOLS_POS_NO);  //机床刀号
                ParamValues.Add("{卸下刀具编号}", mToolsMasterDown.TOOLS_CODE.PadRight(25));  //卸下刀具编号
                ParamValues.Add("{装上刀具编号}", mToolsMasterUp.TOOLS_CODE.PadRight(25));  //装上刀具编号
                ParamValues.Add("{装上刀具PKNO}", mToolsMasterUp.PKNO);  //装上刀具PKNO
                ParamValues.Add("{卸下刀具PKNO}", mToolsMasterDown.PKNO);  //卸下刀具PKNO
                ParamValues.Add("{长度形状补偿}", SafeConverter.SafeToStr(mToolsMasterUp.COMPENSATION_SHAPE_LENGTH));  //长度形状补偿 - 装上
                ParamValues.Add("{半径形状补偿}", SafeConverter.SafeToStr(mToolsMasterUp.COMPENSATION_SHAPE_DIAMETER));  //半径形状补偿 - 装上

                ParamValues.Add("{卸下刀位PKNO}", mTmsDeviceToolsPos.PKNO);  //卸下刀位PKNO
                ParamValues.Add("{装上刀位PKNO}", mTmsDeviceToolsPos.PKNO);  //装上刀位PKNO

                ParamValues.Add("{装刀机床PKNO}", asset1.PKNO);  //装刀机床PKNO

                #endregion

                job = BuildNewJobOrder("", 5, "#1演示流程", jobOrderTime.AddSeconds(iJobOrderIndex++));  //形成订单
                jobOrders.Add(job);

                sFormulaCode = "换刀-" + asset1.ASSET_CODE;
                formulaDetails = wsFms.UseService(s =>
                    s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails)  //配方
                {
                    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                    processCtrols.Add(process);
                }

                #endregion


                #region 2. 2#机床 18寸轮毂只上料，出（14）

                WmsAllocationInfo allocation14 =
                    ws.UseService(s => s.GetWmsAllocationInfos($"ALLOCATION_CODE = 14 AND USE_FLAG = 1"))
                        .FirstOrDefault(); //--货位--
                if ((allocation14 == null) || (allocation14.ALLOCATION_STATE < 0))
                {
                    WaitLoading.SetDefault(this);
                    WPFMessageBox.ShowError("14货位不存在或已禁用，请联系管理员.", "演示程序");
                    return;
                }

                allocation14.ALLOCATION_STATE = (allocation14.ALLOCATION_STATE % 1000) + 4000; //出库锁定
                allocationInfos.Add(allocation14);

                job = BuildNewJobOrder(product18.PKNO, 2, "1#演示流程", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{生产设备}", asset2.ASSET_CODE); //生产设备
                ParamValues.Add("{加工机床}", asset2.ASSET_LABEL); //加工机床标签
                ParamValues.Add("{加工程序号}", program18); //加工程序号
                ParamValues.Add("{原料PKNO}", raw18.PKNO); //原料PKNO
                ParamValues.Add("{成品PKNO}", product18.PKNO); //成品PKNO
                ParamValues.Add("{原始货位PKNO}", allocation14.PKNO); //原始货位PKNO
                ParamValues.Add("{成品货位PKNO}", allocation14.PKNO); //成品货位PKNO
                ParamValues.Add("{原始货位}", allocation14.INTERFACE_NAME); //原始货位
                ParamValues.Add("{成品货位}", allocation14.INTERFACE_NAME); //成品货位
                ParamValues.Add("{轮毂型号}", product18.ITEM_NORM); //轮毂型号
                ParamValues.Add("{机床轮毂型号}", ""); //机床轮毂型号
                ParamValues.Add("{机床轮毂PKNO}", ""); //机床轮毂PKNO
                ParamValues.Add("{三坐标轮毂型号}", ""); //三坐标轮毂型号
                ParamValues.Add("{三坐标轮毂PKNO}", ""); //三坐标轮毂PKNO
                ParamValues.Add("{检测程序}", "WHSIZE18"); //检测程序，按照机床里面的轮毂型号进行检测
                ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                #endregion

                sFormulaCode = "轮毂生产-" + 1 + "-" + asset2.ASSET_CODE; //--1. 只上料--

                #region 形成过程控制

                formulaDetails = wsFms.UseService(s =>
                        s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails) //配方
                {
                    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                    processCtrols.Add(process);
                }

                #endregion

                #endregion

                #region 3. 1#机床 17寸轮毂只上料，出（13）

                WmsAllocationInfo allocation13 =
                    ws.UseService(s => s.GetWmsAllocationInfos($"ALLOCATION_CODE = 13 AND USE_FLAG = 1"))
                        .FirstOrDefault(); //--货位--
                if ((allocation13 == null) || (allocation13.ALLOCATION_STATE < 0))
                {
                    WaitLoading.SetDefault(this);
                    WPFMessageBox.ShowError("13货位不存在或已禁用，请联系管理员.", "演示程序");
                    return;
                }

                allocation13.ALLOCATION_STATE = (allocation13.ALLOCATION_STATE % 1000) + 4000; //出库锁定
                allocationInfos.Add(allocation13);

                job = BuildNewJobOrder(product17.PKNO, 2, "1#演示流程", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region 设定参数

                ParamValues.Clear();
                ParamValues.Add("{生产设备}", asset1.ASSET_CODE); //生产设备
                ParamValues.Add("{加工机床}", asset1.ASSET_LABEL); //加工机床标签
                ParamValues.Add("{加工程序号}", program17); //加工程序号
                ParamValues.Add("{原料PKNO}", raw17.PKNO); //原料PKNO
                ParamValues.Add("{成品PKNO}", product17.PKNO); //成品PKNO
                ParamValues.Add("{原始货位PKNO}", allocation13.PKNO); //原始货位PKNO
                ParamValues.Add("{成品货位PKNO}", allocation13.PKNO); //成品货位PKNO
                ParamValues.Add("{原始货位}", allocation13.INTERFACE_NAME); //原始货位
                ParamValues.Add("{成品货位}", allocation13.INTERFACE_NAME); //成品货位
                ParamValues.Add("{轮毂型号}", product17.ITEM_NORM); //轮毂型号
                ParamValues.Add("{机床轮毂型号}", ""); //机床轮毂型号
                ParamValues.Add("{机床轮毂PKNO}", ""); //机床轮毂PKNO
                ParamValues.Add("{三坐标轮毂型号}", ""); //三坐标轮毂型号
                ParamValues.Add("{三坐标轮毂PKNO}", ""); //三坐标轮毂PKNO
                ParamValues.Add("{检测程序}", "WHSIZE17"); //检测程序，按照机床里面的轮毂型号进行检测
                ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                #endregion

                sFormulaCode = "轮毂生产-" + 1 + "-" + asset1.ASSET_CODE; //--1. 只上料--

                #region 形成过程控制

                formulaDetails = wsFms.UseService(s =>
                        s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails) //配方
                {
                    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                    processCtrols.Add(process);
                }

                #endregion

                #endregion

                #region 4. 3#机床 16寸轮毂只上料，出（25）

                WmsAllocationInfo allocation25 =
                    ws.UseService(s => s.GetWmsAllocationInfos($"ALLOCATION_CODE = 25 AND USE_FLAG = 1"))
                        .FirstOrDefault(); //--货位--
                if ((allocation25 == null) || (allocation25.ALLOCATION_STATE < 0))
                {
                    WaitLoading.SetDefault(this);
                    WPFMessageBox.ShowError("15货位不存在或已禁用，请联系管理员.", "演示程序");
                    return;
                }

                allocation25.ALLOCATION_STATE = (allocation25.ALLOCATION_STATE % 1000) + 4000; //出库锁定
                allocationInfos.Add(allocation25);

                job = BuildNewJobOrder(product16.PKNO, 2, "1#演示流程", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region 设定参数

                ParamValues.Clear();
                ParamValues.Add("{生产设备}", asset3.ASSET_CODE); //生产设备
                ParamValues.Add("{加工机床}", asset3.ASSET_LABEL); //加工机床标签
                ParamValues.Add("{加工程序号}", program16); //加工程序号
                ParamValues.Add("{原料PKNO}", raw16.PKNO); //原料PKNO
                ParamValues.Add("{成品PKNO}", product16.PKNO); //成品PKNO
                ParamValues.Add("{原始货位PKNO}", allocation25.PKNO); //原始货位PKNO
                ParamValues.Add("{成品货位PKNO}", allocation25.PKNO); //成品货位PKNO
                ParamValues.Add("{原始货位}", allocation25.INTERFACE_NAME); //原始货位
                ParamValues.Add("{成品货位}", allocation25.INTERFACE_NAME); //成品货位
                ParamValues.Add("{轮毂型号}", product16.ITEM_NORM); //轮毂型号
                ParamValues.Add("{机床轮毂型号}", ""); //机床轮毂型号
                ParamValues.Add("{机床轮毂PKNO}", ""); //机床轮毂PKNO
                ParamValues.Add("{三坐标轮毂型号}", ""); //三坐标轮毂型号
                ParamValues.Add("{三坐标轮毂PKNO}", ""); //三坐标轮毂PKNO
                ParamValues.Add("{检测程序}", "WHSIZE16"); //检测程序，按照机床里面的轮毂型号进行检测
                ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                #endregion

                sFormulaCode = "轮毂生产-" + 1 + "-" + asset3.ASSET_CODE; //--1. 只上料--

                #region 形成过程控制

                formulaDetails = wsFms.UseService(s =>
                        s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails) //配方
                {
                    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                    processCtrols.Add(process);
                }

                #endregion

                #endregion

                #region 5. 2#机床 18寸轮毂 上下料、首件检测（18寸），出（15）

                WmsAllocationInfo allocation15 =
                    ws.UseService(s => s.GetWmsAllocationInfos($"ALLOCATION_CODE = 15 AND USE_FLAG = 1"))
                        .FirstOrDefault(); //--货位--
                if ((allocation15 == null) || (allocation15.ALLOCATION_STATE < 0))
                {
                    WaitLoading.SetDefault(this);
                    WPFMessageBox.ShowError("24货位不存在或已禁用，请联系管理员.", "演示程序");
                    return;
                }

                allocation25.ALLOCATION_STATE = (allocation15.ALLOCATION_STATE % 1000) + 4000; //出库锁定
                allocationInfos.Add(allocation15);

                job = BuildNewJobOrder(product18.PKNO, 2, "1#演示流程", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{生产设备}", asset2.ASSET_CODE); //生产设备
                ParamValues.Add("{加工机床}", asset2.ASSET_LABEL); //加工机床标签
                ParamValues.Add("{加工程序号}", program18); //加工程序号
                ParamValues.Add("{原料PKNO}", raw18.PKNO); //原料PKNO
                ParamValues.Add("{成品PKNO}", product18.PKNO); //成品PKNO
                ParamValues.Add("{原始货位PKNO}", allocation15.PKNO); //原始货位PKNO
                ParamValues.Add("{成品货位PKNO}", allocation15.PKNO); //成品货位PKNO
                ParamValues.Add("{原始货位}", allocation15.INTERFACE_NAME); //原始货位
                ParamValues.Add("{成品货位}", allocation15.INTERFACE_NAME); //成品货位
                ParamValues.Add("{轮毂型号}", product18.ITEM_NORM); //轮毂型号
                ParamValues.Add("{机床轮毂型号}", product18.ITEM_NORM); //机床轮毂型号
                ParamValues.Add("{机床轮毂PKNO}", product18.PKNO); //机床轮毂PKNO
                ParamValues.Add("{三坐标轮毂型号}", ""); //三坐标轮毂型号
                ParamValues.Add("{三坐标轮毂PKNO}", ""); //三坐标轮毂PKNO
                ParamValues.Add("{检测程序}", "WHSIZE18"); //检测程序，按照机床里面的轮毂型号进行检测
                ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                #endregion

                sFormulaCode = "轮毂生产-" + 2 + "-" + asset2.ASSET_CODE; //--2. 上下料、只检测--

                #region 形成过程控制

                formulaDetails = wsFms.UseService(s =>
                        s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails) //配方
                {
                    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                    processCtrols.Add(process);
                }

                #endregion

                #endregion

                #region 6. 1#机床 16寸轮毂 上下料、中间检测（17寸）、18寸上架，出（24）入（14）

                WmsAllocationInfo allocation24 =
                    ws.UseService(s => s.GetWmsAllocationInfos($"ALLOCATION_CODE = 24 AND USE_FLAG = 1"))
                        .FirstOrDefault(); //--货位--
                if ((allocation24 == null) || (allocation24.ALLOCATION_STATE < 0))
                {
                    WaitLoading.SetDefault(this);
                    WPFMessageBox.ShowError("24货位不存在或已禁用，请联系管理员.", "演示程序");
                    return;
                }

                allocation24.ALLOCATION_STATE = (allocation24.ALLOCATION_STATE % 1000) + 4000; //出库锁定
                allocationInfos.Add(allocation24);

                allocation14.ALLOCATION_STATE = (allocation14.ALLOCATION_STATE % 1000) + 3000; //入库锁定
                allocationInfos.Add(allocation14);

                job = BuildNewJobOrder(product16.PKNO, 2, "1#演示流程", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{生产设备}", asset1.ASSET_CODE); //生产设备
                ParamValues.Add("{加工机床}", asset1.ASSET_LABEL); //加工机床标签
                ParamValues.Add("{加工程序号}", program16); //加工程序号
                ParamValues.Add("{原料PKNO}", raw16.PKNO); //原料PKNO

                ParamValues.Add("{成品PKNO}", product18.PKNO); //成品PKNO
                ParamValues.Add("{原始货位PKNO}", allocation24.PKNO); //原始货位PKNO
                ParamValues.Add("{原始货位}", allocation24.INTERFACE_NAME); //原始货位

                ParamValues.Add("{成品货位PKNO}", allocation14.PKNO); //成品货位PKNO
                ParamValues.Add("{成品货位}", allocation14.INTERFACE_NAME); //成品货位

                ParamValues.Add("{轮毂型号}", product16.ITEM_NORM); //轮毂型号

                ParamValues.Add("{机床轮毂型号}", product17.ITEM_NORM); //机床轮毂型号
                ParamValues.Add("{机床轮毂PKNO}", product17.PKNO); //机床轮毂PKNO
                ParamValues.Add("{三坐标轮毂型号}", product18.ITEM_NORM); //三坐标轮毂型号
                ParamValues.Add("{三坐标轮毂PKNO}", product18.PKNO); //三坐标轮毂PKNO

                ParamValues.Add("{检测程序}", "WHSIZE17"); //检测程序，按照机床里面的轮毂型号进行检测
                ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                #endregion

                sFormulaCode = "轮毂生产-" + 3 + "-" + asset1.ASSET_CODE; //--3. 上下料、检测上架--

                #region 形成过程控制

                formulaDetails = wsFms.UseService(s =>
                        s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails) //配方
                {
                    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                    processCtrols.Add(process);
                }

                #endregion

                #endregion

                #region 7. 3#机床 只下料（16寸）、中间检测（16寸）、17寸上架，入（13）

                allocation13.ALLOCATION_STATE = (allocation13.ALLOCATION_STATE % 1000) + 3000; //入库锁定
                allocationInfos.Add(allocation13);

                job = BuildNewJobOrder(product16.PKNO, 2, "1#演示流程", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{生产设备}", asset3.ASSET_CODE); //生产设备
                ParamValues.Add("{加工机床}", asset3.ASSET_LABEL); //加工机床标签
                ParamValues.Add("{加工程序号}", program16); //加工程序号
                ParamValues.Add("{原料PKNO}", raw16.PKNO); //原料PKNO
                ParamValues.Add("{成品PKNO}", product17.PKNO); //成品PKNO

                ParamValues.Add("{原始货位PKNO}", allocation13.PKNO); //原始货位PKNO
                ParamValues.Add("{原始货位}", allocation13.INTERFACE_NAME); //原始货位

                ParamValues.Add("{成品货位PKNO}", allocation13.PKNO); //成品货位PKNO
                ParamValues.Add("{成品货位}", allocation13.INTERFACE_NAME); //成品货位

                ParamValues.Add("{轮毂型号}", product16.ITEM_NORM); //轮毂型号

                ParamValues.Add("{机床轮毂型号}", product16.ITEM_NORM); //机床轮毂型号
                ParamValues.Add("{机床轮毂PKNO}", product16.PKNO); //机床轮毂PKNO
                ParamValues.Add("{三坐标轮毂型号}", product17.ITEM_NORM); //三坐标轮毂型号
                ParamValues.Add("{三坐标轮毂PKNO}", product17.PKNO); //三坐标轮毂PKNO

                ParamValues.Add("{检测程序}", "WHSIZE16"); //检测程序，按照机床里面的轮毂型号进行检测
                ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                #endregion

                sFormulaCode = "轮毂生产-" + 5 + "-" + asset3.ASSET_CODE; //--5. 只下料、检测上架--

                #region 形成过程控制

                formulaDetails = wsFms.UseService(s =>
                        s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails) //配方
                {
                    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                    processCtrols.Add(process);
                }

                #endregion

                #endregion

                #region 8. 2#机床 只下料（18寸）、中间检测（18寸）、16寸上架，入（25）

                allocation25.ALLOCATION_STATE = (allocation25.ALLOCATION_STATE % 1000) + 3000; //入库锁定
                allocationInfos.Add(allocation25);

                job = BuildNewJobOrder(product18.PKNO, 2, "1#演示流程", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{生产设备}", asset2.ASSET_CODE); //生产设备
                ParamValues.Add("{加工机床}", asset2.ASSET_LABEL); //加工机床标签
                ParamValues.Add("{加工程序号}", program18); //加工程序号
                ParamValues.Add("{原料PKNO}", raw16.PKNO); //原料PKNO
                ParamValues.Add("{成品PKNO}", product16.PKNO); //成品PKNO

                ParamValues.Add("{原始货位PKNO}", allocation25.PKNO); //原始货位PKNO
                ParamValues.Add("{原始货位}", allocation25.INTERFACE_NAME); //原始货位
                ParamValues.Add("{成品货位PKNO}", allocation25.PKNO); //成品货位PKNO
                ParamValues.Add("{成品货位}", allocation25.INTERFACE_NAME); //成品货位

                ParamValues.Add("{轮毂型号}", product18.ITEM_NORM); //轮毂型号

                ParamValues.Add("{机床轮毂型号}", product18.ITEM_NORM); //机床轮毂型号
                ParamValues.Add("{机床轮毂PKNO}", product18.PKNO); //机床轮毂PKNO
                ParamValues.Add("{三坐标轮毂型号}", product16.ITEM_NORM); //三坐标轮毂型号
                ParamValues.Add("{三坐标轮毂PKNO}", product16.PKNO); //三坐标轮毂PKNO

                ParamValues.Add("{检测程序}", "WHSIZE18"); //检测程序，按照机床里面的轮毂型号进行检测
                ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                #endregion

                sFormulaCode = "轮毂生产-" + 5 + "-" + asset2.ASSET_CODE; //--5. 只下料、检测上架--

                #region 形成过程控制

                formulaDetails = wsFms.UseService(s =>
                        s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails) //配方
                {
                    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                    processCtrols.Add(process);
                }

                #endregion

                #endregion

                #region 9. 1#机床 只下料（16寸）、中间检测（16寸）、18寸上架，入（15）

                allocation15.ALLOCATION_STATE = (allocation15.ALLOCATION_STATE % 1000) + 3000; //入库锁定
                allocationInfos.Add(allocation15);

                job = BuildNewJobOrder(product16.PKNO, 2, "1#演示流程", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{生产设备}", asset1.ASSET_CODE); //生产设备
                ParamValues.Add("{加工机床}", asset1.ASSET_LABEL); //加工机床标签
                ParamValues.Add("{加工程序号}", program16); //加工程序号
                ParamValues.Add("{原料PKNO}", raw16.PKNO); //原料PKNO
                ParamValues.Add("{成品PKNO}", product16.PKNO); //成品PKNO

                ParamValues.Add("{原始货位PKNO}", allocation15.PKNO); //原始货位PKNO
                ParamValues.Add("{原始货位}", allocation15.INTERFACE_NAME); //原始货位
                ParamValues.Add("{成品货位PKNO}", allocation15.PKNO); //成品货位PKNO
                ParamValues.Add("{成品货位}", allocation15.INTERFACE_NAME); //成品货位

                ParamValues.Add("{轮毂型号}", product16.ITEM_NORM); //轮毂型号

                ParamValues.Add("{机床轮毂型号}", product16.ITEM_NORM); //机床轮毂型号
                ParamValues.Add("{机床轮毂PKNO}", product16.PKNO); //机床轮毂PKNO
                ParamValues.Add("{三坐标轮毂型号}", product18.ITEM_NORM); //三坐标轮毂型号
                ParamValues.Add("{三坐标轮毂PKNO}", product18.PKNO); //三坐标轮毂PKNO

                ParamValues.Add("{检测程序}", "WHSIZE16"); //检测程序，按照机床里面的轮毂型号进行检测
                ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                #endregion

                sFormulaCode = "轮毂生产-" + 5 + "-" + asset1.ASSET_CODE; //--5. 只下料、检测上架--

                #region 形成过程控制

                formulaDetails = wsFms.UseService(s =>
                        s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails) //配方
                {
                    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                    processCtrols.Add(process);
                }

                #endregion

                #endregion

                #region 10. 16寸只上料架，入（24）

                allocation24.ALLOCATION_STATE = (allocation24.ALLOCATION_STATE % 1000) + 3000; //入库锁定
                allocationInfos.Add(allocation24);

                job = BuildNewJobOrder(product16.PKNO, 2, "1#演示流程", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{生产设备}", asset1.ASSET_CODE); //生产设备
                ParamValues.Add("{加工机床}", asset1.ASSET_LABEL); //加工机床标签
                ParamValues.Add("{加工程序号}", program16); //加工程序号
                ParamValues.Add("{原料PKNO}", raw16.PKNO); //原料PKNO
                ParamValues.Add("{成品PKNO}", product16.PKNO); //成品PKNO

                ParamValues.Add("{原始货位PKNO}", allocation24.PKNO); //原始货位PKNO
                ParamValues.Add("{原始货位}", allocation24.INTERFACE_NAME); //原始货位
                ParamValues.Add("{成品货位PKNO}", allocation24.PKNO); //成品货位PKNO
                ParamValues.Add("{成品货位}", allocation24.INTERFACE_NAME); //成品货位

                ParamValues.Add("{轮毂型号}", product16.ITEM_NORM); //轮毂型号

                ParamValues.Add("{机床轮毂型号}", product16.ITEM_NORM); //机床轮毂型号
                ParamValues.Add("{机床轮毂PKNO}", product16.PKNO); //机床轮毂PKNO
                ParamValues.Add("{三坐标轮毂型号}", product16.ITEM_NORM); //三坐标轮毂型号
                ParamValues.Add("{三坐标轮毂PKNO}", product16.PKNO); //三坐标轮毂PKNO

                ParamValues.Add("{检测程序}", "WHSIZE16"); //检测程序，按照机床里面的轮毂型号进行检测
                ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                #endregion

                sFormulaCode = "轮毂生产-" + 6; //--6. 只上料架--

                #region 形成过程控制

                formulaDetails = wsFms.UseService(s =>
                        s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails) //配方
                {
                    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                    processCtrols.Add(process);
                }

                #endregion

                #endregion



                //#region  11. 1#机床换刀  T10 => 10

                //#region 增加参数

                //ParamValues.Clear();

                //ParamValues.Add("{机床刀号}", mTmsDeviceToolsPos.TOOLS_POS_NO);  //机床刀号
                //ParamValues.Add("{卸下刀具编号}", mToolsMasterUp.TOOLS_CODE.PadRight(25));  //卸下刀具编号 => 之前装上的
                //ParamValues.Add("{装上刀具编号}", mToolsMasterDown.TOOLS_CODE.PadRight(25));  //装上刀具编号  => 之前卸下的
                //ParamValues.Add("{装上刀具PKNO}", mToolsMasterDown.PKNO);  //装上刀具PKNO  => 之前卸下的
                //ParamValues.Add("{卸下刀具PKNO}", mToolsMasterUp.PKNO);  //卸下刀具PKNO    => 之前装上的
                //ParamValues.Add("{长度形状补偿}", SafeConverter.SafeToStr(mToolsMasterDown.COMPENSATION_SHAPE_LENGTH));  //长度形状补偿 - 装上  => 之前卸下的
                //ParamValues.Add("{半径形状补偿}", SafeConverter.SafeToStr(mToolsMasterDown.COMPENSATION_SHAPE_DIAMETER));  //半径形状补偿 - 装上  => 之前卸下的

                //ParamValues.Add("{卸下刀位PKNO}", mTmsDeviceToolsPos.PKNO);  //卸下刀位PKNO
                //ParamValues.Add("{装上刀位PKNO}", mTmsDeviceToolsPos.PKNO);  //装上刀位PKNO

                //ParamValues.Add("{装刀机床PKNO}", asset1?.PKNO);  //装刀机床PKNO

                //#endregion

                //job = BuildNewJobOrder("", 5, "#1演示流程", jobOrderTime.AddSeconds(iJobOrderIndex++));  //形成订单
                //jobOrders.Add(job);

                //sFormulaCode = "换刀-" + asset1.ASSET_CODE;

                //formulaDetails = wsFms.UseService(s =>
                //    s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                //    .OrderBy(c => c.PROCESS_INDEX)
                //    .ToList();

                //foreach (var detail in formulaDetails)  //配方
                //{
                //    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                //    processCtrols.Add(process);
                //}

                //#endregion

                #region 12. 结束机器人运行状态

                //allocation24.ALLOCATION_STATE = (allocation24.ALLOCATION_STATE % 1000) + 3000; //入库锁定
                //allocationInfos.Add(allocation24);

                job = BuildNewJobOrder("", 2, "#1演示流程", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);
                sFormulaCode = "结束机器人运行状态"; //--结束机器人运行状态--

                #region 形成过程控制

                formulaDetails = wsFms.UseService(s =>
                        s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails) //配方
                {
                    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                    processCtrols.Add(process);
                }

                #endregion

                #endregion

                DeviceProcessControl.PauseByLine(CBaseData.CurLinePKNO); //暂停，防止任务直接执行

                #region 保存数据

                foreach (var allocationInfo in allocationInfos)
                {
                    ws.UseService(s => s.UpdateWmsAllocationInfo(allocationInfo));
                    Thread.Sleep(100);
                }

                foreach (var ctrol in processCtrols)
                {
                    wsPlm.UseService(s => s.AddMesProcessCtrol(ctrol));
                    Thread.Sleep(100);
                }

                foreach (var jobOrder in jobOrders) //订单
                {
                    wsPlm.UseService(s => s.AddMesJobOrder(jobOrder));
                    Thread.Sleep(100);
                }

                #endregion

                DeviceProcessControl.RunByLine(CBaseData.CurLinePKNO); //启动动作流程

                WaitLoading.SetDefault(this);

                WPFMessageBox.ShowInfo("演示任务已下达，请启动机器人开始程序.", "演示程序");

            }).Start();
        }

        /// <summary>
        /// 获取新工单
        /// </summary>
        /// <param name="productPKNO">产品PKNO</param>
        /// <param name="orderType">工单类型 1：原料入库；2：加工；3：成品出库；4：转换；5：换刀</param>
        /// <returns></returns>
        private MesJobOrder BuildNewJobOrder(string productPKNO, int orderType, string batchNO, DateTime dtCreateTime)
        {
            return new MesJobOrder()
            {
                PKNO = CBaseData.NewGuid(),
                COMPANY_CODE = CBaseData.BelongCompPKNO,
                LINE_PKNO = CBaseData.CurLinePKNO,
                LINE_TASK_PKNO = "", //
                ITEM_PKNO = productPKNO, // TODO:暂无
                JOB_ORDER_NO = TableNOHelper.GetNewNO("MES_JOB_ORDER.JOB_ORDER_NO", "J"),
                BATCH_NO = batchNO,
                ROUTING_DETAIL_PKNO = "",
                JOB_ORDER_TYPE = orderType,
                TASK_QTY = 1,
                COMPLETE_QTY = 0,
                ONLINE_QTY = 0,
                ONCE_QTY = 0,
                RUN_STATE = 10,  //直接生产
                CREATION_DATE = dtCreateTime,
                CREATED_BY = CBaseData.LoginNO,
                LAST_UPDATE_DATE = DateTime.Now,  //最后修改日期
                USE_FLAG = 1,
                REMARK = "",
            };
        }

        /// <summary>
        /// 形成过程控制信息
        /// </summary>
        /// <param name="jobOrder">订单</param>
        /// <param name="formulaDetail">配方明细</param>
        /// <param name="paramValues">参数</param>
        /// <returns></returns>
        private MesProcessCtrol BuildNewProcess(MesJobOrder jobOrder,
            FmsActionFormulaDetail formulaDetail, Dictionary<string, string> paramValues)
        {
            return new MesProcessCtrol()
            {
                #region 标准信息

                PKNO = CBaseData.NewGuid(),
                COMPANY_CODE = CBaseData.BelongCompPKNO,
                ITEM_PKNO = jobOrder?.ITEM_PKNO,   //成品PKNO TODO:暂无
                JOB_ORDER_PKNO = jobOrder?.PKNO,
                JOB_ORDER = jobOrder?.JOB_ORDER_NO,
                SUB_JOB_ORDER_NO = "",
                ROUTING_DETAIL_PKNO = "",  //

                #endregion

                PROCESS_CTROL_NAME = formulaDetail.FORMULA_DETAIL_NAME,  //名称
                PROCESS_DEVICE_PKNO = ProcessParamReplace.Replace(formulaDetail.PROCESS_DEVICE_PKNO, paramValues),             //生产设备
                PROCESS_PROGRAM_NO = ProcessParamReplace.Replace(formulaDetail.PROCESS_PROGRAM_NO, paramValues),              //加工程序号
                PROCESS_PROGRAM_CONTENT = formulaDetail.PROCESS_PROGRAM_CONTENT,         //加工程序内容
                PROCESS_INDEX = formulaDetail.PROCESS_INDEX,                   //工序顺序

                BEGIN_ITEM_PKNO = ProcessParamReplace.Replace(formulaDetail.BEGIN_ITEM_PKNO, paramValues),                 //生产前项目PKNO
                FINISH_ITEM_PKNO = ProcessParamReplace.Replace(formulaDetail.FINISH_ITEM_PKNO, paramValues),                //生产后项目PKNO
                BEGIN_POSITION = ProcessParamReplace.Replace(formulaDetail.BEGIN_POSITION, paramValues),                  //生产前位置
                FINISH_POSITION = ProcessParamReplace.Replace(formulaDetail.FINISH_POSITION, paramValues),                 //生产后位置

                PALLET_NO = formulaDetail.PALLET_NO,                       //托盘号
                PROCESS_ACTION_TYPE = formulaDetail.PROCESS_ACTION_TYPE,          //工序动作类型
                PROCESS_ACTION_PKNO = formulaDetail.PROCESS_ACTION_PKNO,             //工序动作控制PKNO

                PROCESS_ACTION_PARAM1_VALUE = ProcessParamReplace.Replace(formulaDetail.PROCESS_ACTION_PARAM1_VALUE, paramValues),     //工序动作参数1
                PROCESS_ACTION_PARAM2_VALUE = ProcessParamReplace.Replace(formulaDetail.PROCESS_ACTION_PARAM2_VALUE, paramValues),     //工序动作参数2

                CUR_PRODUCT_CODE_PKNO = "",           //当前生产加工的产品编码PKNO
                PROCESS_QTY = 1,                     //加工数量（上线数量）
                COMPLETE_QTY = 0,   //完成数量
                QUALIFIED_QTY = 0,  //合格数量
                PROCESS_STATE = 1,  //准备完成

                CREATION_DATE = DateTime.Now,                   //创建日期
                CREATED_BY = CBaseData.LoginNO,                      //创建人
                LAST_UPDATE_DATE = DateTime.Now,  //最后修改日期
                USE_FLAG = formulaDetail.USE_FLAG,                        //启用标识
                REMARK = "",                          //备注
            };
        }

        //选择工艺参数
        private void CmbPrcessParam_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPrcessParam.SelectedIndex < 0) return;

            RsItemMaster curProduct = wsRsm.UseService(s => s.GetRsItemMasterById(SafeConverter.SafeToStr(cmbChangeProduct.SelectedValue)));  //当前产品信息

            if (curProduct != null)
            {
                TbProgramNO.Text = (curProduct.ITEM_NORM == "1")
                    ? "70"
                    : ((curProduct.ITEM_NORM == "2") ? "80" : ((curProduct.ITEM_NORM == "3") ? "90" : ""));
            }

            cmbProcessDevice.IsEnabled = true;
            ItemProcessDevice.Background = Brushes.White;
            TbProgramNO.IsEnabled = true;
            ItemProgramNO.Background = Brushes.White;
            cmbDevice.IsEnabled = true;
            ItemDevice.Background = Brushes.White;
            cmbCheck.IsEnabled = true;
            ItemCheck.Background = Brushes.White;

            if (cmbPrcessParam.SelectedValue.ToString() == "6") //只从三坐标下料
            {
                cmbProcessDevice.SelectedIndex = -1;
                cmbProcessDevice.IsEnabled = false;
                ItemProcessDevice.Background = Brushes.LightGray;
                TbProgramNO.Text = "";
                TbProgramNO.IsEnabled = false;
                ItemProgramNO.Background = Brushes.LightGray;
                cmbDevice.SelectedIndex = -1;
                cmbDevice.IsEnabled = false;
                ItemDevice.Background = Brushes.LightGray;
            }
            else if (cmbPrcessParam.SelectedValue.ToString() == "5") //只下料，中间件
            {
                TbProgramNO.Text = "";
                TbProgramNO.IsEnabled = false;
                ItemProgramNO.Background = Brushes.LightGray;
            }
            else if (cmbPrcessParam.SelectedValue.ToString() == "4")  //只下料
            {
                TbProgramNO.Text = "";
                TbProgramNO.IsEnabled = false;
                ItemProgramNO.Background = Brushes.LightGray;

                cmbCheck.SelectedIndex = -1;
                cmbCheck.IsEnabled = false;
                ItemCheck.Background = Brushes.LightGray;
            }
            else if (cmbPrcessParam.SelectedValue.ToString() == "1") //只上料
            {
                cmbDevice.SelectedIndex = -1;
                cmbDevice.IsEnabled = false;
                ItemDevice.Background = Brushes.LightGray;

                cmbCheck.SelectedIndex = -1;
                cmbCheck.IsEnabled = false;
                ItemCheck.Background = Brushes.LightGray;
            }
            else if (cmbPrcessParam.SelectedValue.ToString() == "2") //下料 上料 首件
            {
                cmbCheck.SelectedIndex = -1;
                cmbCheck.IsEnabled = false;
                ItemCheck.Background = Brushes.LightGray;
            }
            else if (cmbPrcessParam.SelectedValue.ToString() == "3") //下料 上料 中间件
            {
            }
        }

        //启动循环演示流程
        private void bAllowRun_Click(object sender, RoutedEventArgs e)
        {
            bool bAddHead = false;
            string batchno = "";
            WcfClient<IPLMService> ws2 = new WcfClient<IPLMService>();
            List<MesJobOrder> mesJobOrders =
                ws2.UseService(s =>
                        s.GetMesJobOrders(
                            $"USE_FLAG = 1 AND RUN_STATE < 100 AND LINE_PKNO = '{CBaseData.CurLinePKNO}'"))
                    .OrderBy(c => c.CREATION_DATE).ToList();  //正在执行的
            if (mesJobOrders.Count <= 0)
            {
                List<string> emptyList = new List<string>();
                List<string> value = wsSQL.UseService(s =>
                    s.GetFirstRow($"SELECT BATCH_NO, RUN_STATE FROM MES_JOB_ORDER " +
                                  $" WHERE LINE_PKNO = '{CBaseData.CurLinePKNO}' ORDER BY CREATION_DATE DESC",
                        emptyList, emptyList));
                string state = "";
                if (value.Count > 0)
                {
                    batchno = value[0];
                }
                if (value.Count > 1)
                {
                    state = value[1];
                }

                if (!string.IsNullOrEmpty(batchno) && (batchno.Contains("循环演示") && (state == "100")))  //有任务且循环演示且正常完成
                {
                    if (WPFMessageBox.ShowConfirm("上一次循环演示流程已经正常完成，确定要增加一次循环吗？\r\n" +
                                                  "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\r\n" +
                                                  "请确保：当前循环演示流程没有手动处理过！\r\n" +
                                                  "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\r\n", "循环演示") !=
                        WPFMessageBoxResult.OK) return;
                }
                else  //空的演示流程
                {
                    batchno = "";
                    bAddHead = true;

                    if (WPFMessageBox.ShowConfirm("确定要开始下达循环演示流程吗？\r\n" +
                                                  "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\r\n" +
                                                  "请确保：\r\n1.机床、三坐标上都没有轮毂!!!\r\n" +
                                                  "2.实际料架上货位[14]、[15]为18寸轮毂，[13]、[23]为17寸轮毂，[24]、[25]为16寸轮毂!!!\r\n" +
                                                  "3.料架上的实物轮毂都是成品!!!\r\n" +
                                                  "4.三坐标上的卡盘开口为18寸轮毂的尺寸!!!\r\n" +
                                                  "5.各设备处于正常且联机自动状态!!!\r\n" +
                                                  "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\r\n", "循环演示") !=
                        WPFMessageBoxResult.OK) return;
                }
            }
            else
            {
                MesJobOrder lastJob = mesJobOrders.LastOrDefault();
                batchno = lastJob?.BATCH_NO;
                if (string.IsNullOrEmpty(batchno) || !batchno.Contains("循环演示"))
                {
                    WaitLoading.SetDefault(this);
                    WPFMessageBox.ShowError("目前还有未完成的其他任务，请将任务完成后再进行演示流程！", "循环演示");
                    return;
                }

                if (mesJobOrders.Count >= 20)
                {
                    WPFMessageBox.ShowError("当前未执行的流程还有很多，请执行一段时间后再添加循环流程！", "循环演示");
                    return;
                }

                if (WPFMessageBox.ShowConfirm("确定要增加一次循环吗？\r\n" +
                                              "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\r\n" +
                                              "请确保：当前循环演示流程没有手动处理过！\r\n" +
                                              "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\r\n", "循环演示") !=
                    WPFMessageBoxResult.OK) return;
            }

            //后台执行添加
            new Thread(delegate ()
            {
                WaitLoading.SetWait(this);

                #region 初始化各种参数

                DateTime jobOrderTime = DateTime.Now.AddSeconds(-12);
                int iJobOrderIndex = 0;

                List<MesJobOrder> jobOrders = new List<MesJobOrder>(); //所有订单
                List<MesProcessCtrol> processCtrols = new List<MesProcessCtrol>(); //控制流程
                List<WmsAllocationInfo> allocationInfos = new List<WmsAllocationInfo>(); //需要修改的货位

                Dictionary<string, string> ParamValues = new Dictionary<string, string>();
                MesJobOrder job = null;
                string sFormulaCode = "";
                List<FmsActionFormulaDetail> formulaDetails;

                AmAssetMasterN asset1 = wsEAM.UseService(s => s.GetAmAssetMasterNs("ASSET_CODE = 'SH00003'"))
                    .FirstOrDefault(); //1#机床 - 斌胜
                AmAssetMasterN asset2 = wsEAM.UseService(s => s.GetAmAssetMasterNs("ASSET_CODE = 'SH00002'"))
                    .FirstOrDefault(); //2#机床 - 京元登
                AmAssetMasterN asset3 = wsEAM.UseService(s => s.GetAmAssetMasterNs("ASSET_CODE = 'SH00001'"))
                    .FirstOrDefault(); //3#机床 - 创圣特尔
                if ((asset1 == null) || (asset2 == null) || (asset3 == null))
                {
                    WaitLoading.SetDefault(this);
                    WPFMessageBox.ShowError("机床信息没有正确初始化，请联系管理员进行配置.", "循环演示");
                    return;
                }

                List<RsItemMaster> items = wsRsm.UseService(s => s.GetRsItemMasters("USE_FLAG = 1"));

                RsItemMaster product16 = items.FirstOrDefault(c => c.ITEM_NORM == "1" && c.NORM_CLASS == 10); //产品信息
                RsItemMaster product17 = items.FirstOrDefault(c => c.ITEM_NORM == "2" && c.NORM_CLASS == 10); //产品信息
                RsItemMaster product18 = items.FirstOrDefault(c => c.ITEM_NORM == "3" && c.NORM_CLASS == 10); //产品信息

                RsItemMaster raw16 = items.FirstOrDefault(c => c.ITEM_NORM == "1" && c.NORM_CLASS == 1); //原料信息
                RsItemMaster raw17 = items.FirstOrDefault(c => c.ITEM_NORM == "2" && c.NORM_CLASS == 1); //原料信息
                RsItemMaster raw18 = items.FirstOrDefault(c => c.ITEM_NORM == "3" && c.NORM_CLASS == 1); //原料信息
                if ((raw16 == null) || (raw17 == null) || (raw18 == null) || (product16 == null) ||
                    (product17 == null) || (product18 == null))
                {
                    WaitLoading.SetDefault(this);
                    WPFMessageBox.ShowError("轮毂物料信息没有正确初始化，请联系管理员进行配置.", "演示程序");
                    return;
                }

                List<WmsAllocationInfo> allcations = ws.UseService(s => s.GetWmsAllocationInfos($"USE_FLAG = 1"));

                WmsAllocationInfo allocation14 = allcations.FirstOrDefault(c => c.ALLOCATION_CODE == "14");
                WmsAllocationInfo allocation15 = allcations.FirstOrDefault(c => c.ALLOCATION_CODE == "15");
                WmsAllocationInfo allocation24 = allcations.FirstOrDefault(c => c.ALLOCATION_CODE == "24");
                WmsAllocationInfo allocation25 = allcations.FirstOrDefault(c => c.ALLOCATION_CODE == "25");
                WmsAllocationInfo allocation13 = allcations.FirstOrDefault(c => c.ALLOCATION_CODE == "13");
                WmsAllocationInfo allocation23 = allcations.FirstOrDefault(c => c.ALLOCATION_CODE == "23");

                if ((allocation14 == null) || (allocation14.ALLOCATION_STATE < 0) ||
                    (allocation15 == null) || (allocation15.ALLOCATION_STATE < 0) ||
                    (allocation24 == null) || (allocation24.ALLOCATION_STATE < 0) ||
                    (allocation25 == null) || (allocation25.ALLOCATION_STATE < 0) ||
                    (allocation13 == null) || (allocation13.ALLOCATION_STATE < 0) ||
                    (allocation23 == null) || (allocation23.ALLOCATION_STATE < 0))
                {
                    WaitLoading.SetDefault(this);
                    WPFMessageBox.ShowError("货位设置不正确，请联系管理员.", "循环演示");
                    return;
                }

                string program16 = "70"; //加工程序 16寸
                string program17 = "80"; //加工程序 17寸
                string program18 = "90"; //加工程序 18寸

                #endregion

                if (string.IsNullOrEmpty(batchno))
                {
                    batchno = TableNOHelper.GetNewNO("MesJobOrder.BATCH_NO", "循环演示,,,001,001");

                }

                if (bAddHead) //添加循环演示流程的头
                {
                    //#region 1. 1#机床换刀  T10 => 38

                    //TmsToolsMaster mToolsMasterUp = wsTMS.UseService(s => s.GetTmsToolsMasters("TOOLS_CODE = 38")).FirstOrDefault();  //刀具库存

                    //if ((mToolsMasterUp == null) || (mToolsMasterUp.TOOLS_POSITION != 1))
                    //{
                    //    WaitLoading.SetDefault(this);
                    //    WPFMessageBox.ShowError("刀具【38】没有在库，请核实状态后再进行下达指令.", "演示程序");
                    //    return;
                    //}

                    //TmsDeviceToolsPos mTmsDeviceToolsPos = wsTMS.UseService(s =>
                    //        s.GetTmsDeviceToolsPoss($"DEVICE_PKNO = '{asset1.PKNO}' AND TOOLS_POS_NO = 10 AND USE_FLAG = 1"))
                    //    .FirstOrDefault();  //机床刀位信息
                    //if (mTmsDeviceToolsPos == null)
                    //{
                    //    WaitLoading.SetDefault(this);
                    //    WPFMessageBox.ShowError("系统没有正确初始化，请联系管理员进行配置.", "演示程序");
                    //    return;
                    //}

                    //TmsToolsMaster mToolsMasterDown =
                    //    wsTMS.UseService(s => s.GetTmsToolsMasterById(mTmsDeviceToolsPos.TOOLS_PKNO));
                    //if (mToolsMasterDown == null)
                    //{
                    //    WaitLoading.SetDefault(this);
                    //    WPFMessageBox.ShowError("选中的需要换下的机床刀具信息不存在，请核实.", "演示程序");
                    //    return;
                    //}

                    //#region 增加参数

                    //ParamValues.Clear();

                    //ParamValues.Add("{机床刀号}", mTmsDeviceToolsPos.TOOLS_POS_NO);  //机床刀号
                    //ParamValues.Add("{卸下刀具编号}", mToolsMasterDown.TOOLS_CODE.PadRight(25));  //卸下刀具编号
                    //ParamValues.Add("{装上刀具编号}", mToolsMasterUp.TOOLS_CODE.PadRight(25));  //装上刀具编号
                    //ParamValues.Add("{装上刀具PKNO}", mToolsMasterUp.PKNO);  //装上刀具PKNO
                    //ParamValues.Add("{卸下刀具PKNO}", mToolsMasterDown.PKNO);  //卸下刀具PKNO
                    //ParamValues.Add("{长度形状补偿}", SafeConverter.SafeToStr(mToolsMasterUp.COMPENSATION_SHAPE_LENGTH));  //长度形状补偿 - 装上
                    //ParamValues.Add("{半径形状补偿}", SafeConverter.SafeToStr(mToolsMasterUp.COMPENSATION_SHAPE_DIAMETER));  //半径形状补偿 - 装上

                    //ParamValues.Add("{卸下刀位PKNO}", mTmsDeviceToolsPos.PKNO);  //卸下刀位PKNO
                    //ParamValues.Add("{装上刀位PKNO}", mTmsDeviceToolsPos.PKNO);  //装上刀位PKNO

                    //ParamValues.Add("{装刀机床PKNO}", asset1.PKNO);  //装刀机床PKNO

                    //#endregion

                    //job = BuildNewJobOrder("", 5, "1#演示流程", jobOrderTime.AddSeconds(iJobOrderIndex++));  //形成订单
                    //jobOrders.Add(job);

                    //sFormulaCode = "换刀-" + asset1.ASSET_CODE;
                    //formulaDetails = wsFms.UseService(s =>
                    //    s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    //    .OrderBy(c => c.PROCESS_INDEX)
                    //    .ToList();

                    //foreach (var detail in formulaDetails)  //配方
                    //{
                    //    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                    //    processCtrols.Add(process);
                    //}

                    //#endregion
                    #region 添加循环演示流程的头 2# 1# 3# 只上料 2# 上下料 首件 1# 3# 上下料中间件

                    #region 1. 2#机床 18寸轮毂只上料，出（14）

                    allocation14.ALLOCATION_STATE = (allocation14.ALLOCATION_STATE % 1000) + 4000; //出库锁定
                    allocationInfos.Add(allocation14);

                    job = BuildNewJobOrder(product18.PKNO, 2, batchno,
                        jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                    jobOrders.Add(job);

                    #region --设定参数--

                    ParamValues.Clear();
                    ParamValues.Add("{生产设备}", asset2.ASSET_CODE); //生产设备
                    ParamValues.Add("{加工机床}", asset2.ASSET_LABEL); //加工机床标签
                    ParamValues.Add("{加工程序号}", program18); //加工程序号
                    ParamValues.Add("{原料PKNO}", raw18.PKNO); //原料PKNO
                    ParamValues.Add("{成品PKNO}", product18.PKNO); //成品PKNO
                    ParamValues.Add("{原始货位PKNO}", allocation14.PKNO); //原始货位PKNO
                    ParamValues.Add("{成品货位PKNO}", allocation14.PKNO); //成品货位PKNO
                    ParamValues.Add("{原始货位}", allocation14.INTERFACE_NAME); //原始货位
                    ParamValues.Add("{成品货位}", allocation14.INTERFACE_NAME); //成品货位
                    ParamValues.Add("{轮毂型号}", product18.ITEM_NORM); //轮毂型号
                    ParamValues.Add("{机床轮毂型号}", ""); //机床轮毂型号
                    ParamValues.Add("{机床轮毂PKNO}", ""); //机床轮毂PKNO
                    ParamValues.Add("{三坐标轮毂型号}", ""); //三坐标轮毂型号
                    ParamValues.Add("{三坐标轮毂PKNO}", ""); //三坐标轮毂PKNO
                    ParamValues.Add("{检测程序}", "WHSIZE18"); //检测程序，按照机床里面的轮毂型号进行检测
                    ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                    #endregion

                    sFormulaCode = "轮毂生产-" + 1 + "-" + asset2.ASSET_CODE; //--1. 只上料--

                    #region 形成过程控制

                    formulaDetails = wsFms.UseService(s =>
                            s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                        .OrderBy(c => c.PROCESS_INDEX)
                        .ToList();

                    foreach (var detail in formulaDetails) //配方
                    {
                        MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                        processCtrols.Add(process);
                    }

                    #endregion

                    #endregion

                    #region 2. 1#机床 17寸轮毂只上料，出（13）

                    allocation13.ALLOCATION_STATE = (allocation13.ALLOCATION_STATE % 1000) + 4000; //出库锁定
                    allocationInfos.Add(allocation13);

                    job = BuildNewJobOrder(product17.PKNO, 2, batchno,
                        jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                    jobOrders.Add(job);

                    #region --设定参数--

                    ParamValues.Clear();
                    ParamValues.Add("{生产设备}", asset1.ASSET_CODE); //生产设备
                    ParamValues.Add("{加工机床}", asset1.ASSET_LABEL); //加工机床标签
                    ParamValues.Add("{加工程序号}", program17); //加工程序号
                    ParamValues.Add("{原料PKNO}", raw17.PKNO); //原料PKNO
                    ParamValues.Add("{成品PKNO}", product17.PKNO); //成品PKNO
                    ParamValues.Add("{原始货位PKNO}", allocation13.PKNO); //原始货位PKNO
                    ParamValues.Add("{成品货位PKNO}", allocation13.PKNO); //成品货位PKNO
                    ParamValues.Add("{原始货位}", allocation13.INTERFACE_NAME); //原始货位
                    ParamValues.Add("{成品货位}", allocation13.INTERFACE_NAME); //成品货位
                    ParamValues.Add("{轮毂型号}", product17.ITEM_NORM); //轮毂型号
                    ParamValues.Add("{机床轮毂型号}", ""); //机床轮毂型号
                    ParamValues.Add("{机床轮毂PKNO}", ""); //机床轮毂PKNO
                    ParamValues.Add("{三坐标轮毂型号}", ""); //三坐标轮毂型号
                    ParamValues.Add("{三坐标轮毂PKNO}", ""); //三坐标轮毂PKNO
                    ParamValues.Add("{检测程序}", "WHSIZE17"); //检测程序，按照机床里面的轮毂型号进行检测
                    ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                    #endregion

                    sFormulaCode = "轮毂生产-" + 1 + "-" + asset1.ASSET_CODE; //--1. 只上料--

                    #region 形成过程控制

                    formulaDetails = wsFms.UseService(s =>
                            s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                        .OrderBy(c => c.PROCESS_INDEX)
                        .ToList();

                    foreach (var detail in formulaDetails) //配方
                    {
                        MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                        processCtrols.Add(process);
                    }

                    #endregion

                    #endregion

                    #region 3. 3#机床 16寸轮毂只上料，出（24）

                    allocation24.ALLOCATION_STATE = (allocation24.ALLOCATION_STATE % 1000) + 4000; //出库锁定
                    allocationInfos.Add(allocation24);

                    job = BuildNewJobOrder(product16.PKNO, 2, batchno,
                        jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                    jobOrders.Add(job);

                    #region --设定参数--

                    ParamValues.Clear();
                    ParamValues.Add("{生产设备}", asset3.ASSET_CODE); //生产设备
                    ParamValues.Add("{加工机床}", asset3.ASSET_LABEL); //加工机床标签
                    ParamValues.Add("{加工程序号}", program16); //加工程序号
                    ParamValues.Add("{原料PKNO}", raw16.PKNO); //原料PKNO
                    ParamValues.Add("{成品PKNO}", product16.PKNO); //成品PKNO
                    ParamValues.Add("{原始货位PKNO}", allocation24.PKNO); //原始货位PKNO
                    ParamValues.Add("{成品货位PKNO}", allocation24.PKNO); //成品货位PKNO
                    ParamValues.Add("{原始货位}", allocation24.INTERFACE_NAME); //原始货位
                    ParamValues.Add("{成品货位}", allocation24.INTERFACE_NAME); //成品货位
                    ParamValues.Add("{轮毂型号}", product16.ITEM_NORM); //轮毂型号
                    ParamValues.Add("{机床轮毂型号}", ""); //机床轮毂型号
                    ParamValues.Add("{机床轮毂PKNO}", ""); //机床轮毂PKNO
                    ParamValues.Add("{三坐标轮毂型号}", ""); //三坐标轮毂型号
                    ParamValues.Add("{三坐标轮毂PKNO}", ""); //三坐标轮毂PKNO
                    ParamValues.Add("{检测程序}", "WHSIZE17"); //检测程序，按照机床里面的轮毂型号进行检测
                    ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                    #endregion

                    sFormulaCode = "轮毂生产-" + 1 + "-" + asset3.ASSET_CODE; //--1. 只上料--

                    #region 形成过程控制

                    formulaDetails = wsFms.UseService(s =>
                            s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                        .OrderBy(c => c.PROCESS_INDEX)
                        .ToList();

                    foreach (var detail in formulaDetails) //配方
                    {
                        MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                        processCtrols.Add(process);
                    }

                    #endregion

                    #endregion

                    #region 4. 2#机床 18寸轮毂 上下料、首件检测（18寸），出（15）

                    allocation25.ALLOCATION_STATE = (allocation15.ALLOCATION_STATE % 1000) + 4000; //出库锁定
                    allocationInfos.Add(allocation15);

                    job = BuildNewJobOrder(product18.PKNO, 2, batchno,
                        jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                    jobOrders.Add(job);

                    #region --设定参数--

                    ParamValues.Clear();
                    ParamValues.Add("{生产设备}", asset2.ASSET_CODE); //生产设备
                    ParamValues.Add("{加工机床}", asset2.ASSET_LABEL); //加工机床标签
                    ParamValues.Add("{加工程序号}", program18); //加工程序号
                    ParamValues.Add("{原料PKNO}", raw18.PKNO); //原料PKNO
                    ParamValues.Add("{成品PKNO}", product18.PKNO); //成品PKNO
                    ParamValues.Add("{原始货位PKNO}", allocation15.PKNO); //原始货位PKNO
                    ParamValues.Add("{成品货位PKNO}", allocation15.PKNO); //成品货位PKNO
                    ParamValues.Add("{原始货位}", allocation15.INTERFACE_NAME); //原始货位
                    ParamValues.Add("{成品货位}", allocation15.INTERFACE_NAME); //成品货位
                    ParamValues.Add("{轮毂型号}", product18.ITEM_NORM); //轮毂型号
                    ParamValues.Add("{机床轮毂型号}", product18.ITEM_NORM); //机床轮毂型号
                    ParamValues.Add("{机床轮毂PKNO}", product18.PKNO); //机床轮毂PKNO
                    ParamValues.Add("{三坐标轮毂型号}", ""); //三坐标轮毂型号
                    ParamValues.Add("{三坐标轮毂PKNO}", ""); //三坐标轮毂PKNO
                    ParamValues.Add("{检测程序}", "WHSIZE18"); //检测程序，按照机床里面的轮毂型号进行检测
                    ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                    #endregion

                    sFormulaCode = "轮毂生产-" + 2 + "-" + asset2.ASSET_CODE; //--2. 上下料、只检测--

                    #region 形成过程控制

                    formulaDetails = wsFms.UseService(s =>
                            s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                        .OrderBy(c => c.PROCESS_INDEX)
                        .ToList();

                    foreach (var detail in formulaDetails) //配方
                    {
                        MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                        processCtrols.Add(process);
                    }

                    #endregion

                    #endregion

                    #region 5. 1#机床 17寸轮毂 上下料、中间检测（17寸）、18寸上架，出（23）入（14）

                    allocation23.ALLOCATION_STATE = (allocation23.ALLOCATION_STATE % 1000) + 4000; //出库锁定
                    allocationInfos.Add(allocation23);

                    allocation14.ALLOCATION_STATE = (allocation14.ALLOCATION_STATE % 1000) + 3000; //入库锁定
                    allocationInfos.Add(allocation14);

                    job = BuildNewJobOrder(product17.PKNO, 2, batchno,
                        jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                    jobOrders.Add(job);

                    #region --设定参数--

                    ParamValues.Clear();
                    ParamValues.Add("{生产设备}", asset1.ASSET_CODE); //生产设备
                    ParamValues.Add("{加工机床}", asset1.ASSET_LABEL); //加工机床标签
                    ParamValues.Add("{加工程序号}", program17); //加工程序号
                    ParamValues.Add("{原料PKNO}", raw17.PKNO); //原料PKNO

                    ParamValues.Add("{成品PKNO}", product18.PKNO); //成品PKNO
                    ParamValues.Add("{原始货位PKNO}", allocation23.PKNO); //原始货位PKNO
                    ParamValues.Add("{原始货位}", allocation23.INTERFACE_NAME); //原始货位

                    ParamValues.Add("{成品货位PKNO}", allocation14.PKNO); //成品货位PKNO
                    ParamValues.Add("{成品货位}", allocation14.INTERFACE_NAME); //成品货位

                    ParamValues.Add("{轮毂型号}", product17.ITEM_NORM); //轮毂型号

                    ParamValues.Add("{机床轮毂型号}", product17.ITEM_NORM); //机床轮毂型号
                    ParamValues.Add("{机床轮毂PKNO}", product17.PKNO); //机床轮毂PKNO
                    ParamValues.Add("{三坐标轮毂型号}", product18.ITEM_NORM); //三坐标轮毂型号
                    ParamValues.Add("{三坐标轮毂PKNO}", product18.PKNO); //三坐标轮毂PKNO

                    ParamValues.Add("{检测程序}", "WHSIZE17"); //检测程序，按照机床里面的轮毂型号进行检测
                    ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                    #endregion

                    sFormulaCode = "轮毂生产-" + 3 + "-" + asset1.ASSET_CODE; //--3. 上下料、检测上架--

                    #region 形成过程控制

                    formulaDetails = wsFms.UseService(s =>
                            s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                        .OrderBy(c => c.PROCESS_INDEX)
                        .ToList();

                    foreach (var detail in formulaDetails) //配方
                    {
                        MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                        processCtrols.Add(process);
                    }

                    #endregion

                    #endregion

                    #region 6. 3#机床 16寸轮毂 上下料、中间检测（16寸）、17寸上架，出（25）入（13）

                    allocation25.ALLOCATION_STATE = (allocation25.ALLOCATION_STATE % 1000) + 4000; //出库锁定
                    allocationInfos.Add(allocation25);

                    allocation13.ALLOCATION_STATE = (allocation13.ALLOCATION_STATE % 1000) + 3000; //入库锁定
                    allocationInfos.Add(allocation13);

                    job = BuildNewJobOrder(product16.PKNO, 2, batchno,
                        jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                    jobOrders.Add(job);

                    #region --设定参数--

                    ParamValues.Clear();
                    ParamValues.Add("{生产设备}", asset3.ASSET_CODE); //生产设备
                    ParamValues.Add("{加工机床}", asset3.ASSET_LABEL); //加工机床标签
                    ParamValues.Add("{加工程序号}", program16); //加工程序号
                    ParamValues.Add("{原料PKNO}", raw16.PKNO); //原料PKNO

                    ParamValues.Add("{成品PKNO}", product18.PKNO); //成品PKNO
                    ParamValues.Add("{原始货位PKNO}", allocation25.PKNO); //原始货位PKNO
                    ParamValues.Add("{原始货位}", allocation25.INTERFACE_NAME); //原始货位

                    ParamValues.Add("{成品货位PKNO}", allocation13.PKNO); //成品货位PKNO
                    ParamValues.Add("{成品货位}", allocation13.INTERFACE_NAME); //成品货位

                    ParamValues.Add("{轮毂型号}", product16.ITEM_NORM); //轮毂型号

                    ParamValues.Add("{机床轮毂型号}", product16.ITEM_NORM); //机床轮毂型号
                    ParamValues.Add("{机床轮毂PKNO}", product16.PKNO); //机床轮毂PKNO
                    ParamValues.Add("{三坐标轮毂型号}", product17.ITEM_NORM); //三坐标轮毂型号
                    ParamValues.Add("{三坐标轮毂PKNO}", product17.PKNO); //三坐标轮毂PKNO

                    ParamValues.Add("{检测程序}", "WHSIZE16"); //检测程序，按照机床里面的轮毂型号进行检测
                    ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                    #endregion

                    sFormulaCode = "轮毂生产-" + 3 + "-" + asset3.ASSET_CODE; //--3. 上下料、检测上架--

                    #region 形成过程控制

                    formulaDetails = wsFms.UseService(s =>
                            s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                        .OrderBy(c => c.PROCESS_INDEX)
                        .ToList();

                    foreach (var detail in formulaDetails) //配方
                    {
                        MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                        processCtrols.Add(process);
                    }

                    #endregion

                    #endregion

                    #endregion
                }

                #region 添加一组中间循环演示流程 2# 1# 3# 2# 1# 3# 只上料上下料中间件

                #region 1. 2#机床 18寸轮毂 上下料、中间检测（18寸）、16寸上架，出（14）入（24）

                allocation14.ALLOCATION_STATE = (allocation14.ALLOCATION_STATE % 1000) + 4000; //出库锁定
                allocationInfos.Add(allocation14);

                allocation24.ALLOCATION_STATE = (allocation24.ALLOCATION_STATE % 1000) + 3000; //入库锁定
                allocationInfos.Add(allocation24);

                job = BuildNewJobOrder(product18.PKNO, 2, batchno,
                    jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{生产设备}", asset2.ASSET_CODE); //生产设备
                ParamValues.Add("{加工机床}", asset2.ASSET_LABEL); //加工机床标签
                ParamValues.Add("{加工程序号}", program18); //加工程序号
                ParamValues.Add("{原料PKNO}", raw18.PKNO); //原料PKNO

                ParamValues.Add("{成品PKNO}", product18.PKNO); //成品PKNO

                ParamValues.Add("{原始货位PKNO}", allocation14.PKNO); //原始货位PKNO
                ParamValues.Add("{原始货位}", allocation14.INTERFACE_NAME); //原始货位

                ParamValues.Add("{成品货位PKNO}", allocation24.PKNO); //成品货位PKNO
                ParamValues.Add("{成品货位}", allocation24.INTERFACE_NAME); //成品货位

                ParamValues.Add("{轮毂型号}", product18.ITEM_NORM); //轮毂型号

                ParamValues.Add("{机床轮毂型号}", product18.ITEM_NORM); //机床轮毂型号
                ParamValues.Add("{机床轮毂PKNO}", product18.PKNO); //机床轮毂PKNO
                ParamValues.Add("{三坐标轮毂型号}", product16.ITEM_NORM); //三坐标轮毂型号
                ParamValues.Add("{三坐标轮毂PKNO}", product16.PKNO); //三坐标轮毂PKNO

                ParamValues.Add("{检测程序}", "WHSIZE18"); //检测程序，按照机床里面的轮毂型号进行检测
                ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                #endregion

                sFormulaCode = "轮毂生产-" + 3 + "-" + asset2.ASSET_CODE; //--3. 上下料、检测上架--

                #region 形成过程控制

                formulaDetails = wsFms.UseService(s =>
                        s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails) //配方
                {
                    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                    processCtrols.Add(process);
                }

                #endregion

                #endregion

                #region 2. 1#机床 17寸轮毂 上下料、中间检测（17寸）、18寸上架，出（13）入（15）

                allocation13.ALLOCATION_STATE = (allocation13.ALLOCATION_STATE % 1000) + 4000; //出库锁定
                allocationInfos.Add(allocation13);

                allocation15.ALLOCATION_STATE = (allocation15.ALLOCATION_STATE % 1000) + 3000; //入库锁定
                allocationInfos.Add(allocation15);

                job = BuildNewJobOrder(product17.PKNO, 2, batchno,
                    jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{生产设备}", asset1.ASSET_CODE); //生产设备
                ParamValues.Add("{加工机床}", asset1.ASSET_LABEL); //加工机床标签
                ParamValues.Add("{加工程序号}", program17); //加工程序号
                ParamValues.Add("{原料PKNO}", raw17.PKNO); //原料PKNO

                ParamValues.Add("{成品PKNO}", product17.PKNO); //成品PKNO

                ParamValues.Add("{原始货位PKNO}", allocation13.PKNO); //原始货位PKNO
                ParamValues.Add("{原始货位}", allocation13.INTERFACE_NAME); //原始货位

                ParamValues.Add("{成品货位PKNO}", allocation15.PKNO); //成品货位PKNO
                ParamValues.Add("{成品货位}", allocation15.INTERFACE_NAME); //成品货位

                ParamValues.Add("{轮毂型号}", product17.ITEM_NORM); //轮毂型号

                ParamValues.Add("{机床轮毂型号}", product17.ITEM_NORM); //机床轮毂型号
                ParamValues.Add("{机床轮毂PKNO}", product17.PKNO); //机床轮毂PKNO
                ParamValues.Add("{三坐标轮毂型号}", product18.ITEM_NORM); //三坐标轮毂型号
                ParamValues.Add("{三坐标轮毂PKNO}", product18.PKNO); //三坐标轮毂PKNO

                ParamValues.Add("{检测程序}", "WHSIZE17"); //检测程序，按照机床里面的轮毂型号进行检测
                ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                #endregion

                sFormulaCode = "轮毂生产-" + 3 + "-" + asset1.ASSET_CODE; //--3. 上下料、检测上架--

                #region 形成过程控制

                formulaDetails = wsFms.UseService(s =>
                        s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails) //配方
                {
                    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                    processCtrols.Add(process);
                }

                #endregion

                #endregion

                #region 3. 3#机床 16寸轮毂 上下料、中间检测（16寸）、17寸上架，出（24）入（23）

                allocation24.ALLOCATION_STATE = (allocation24.ALLOCATION_STATE % 1000) + 4000; //出库锁定
                allocationInfos.Add(allocation24);

                allocation23.ALLOCATION_STATE = (allocation23.ALLOCATION_STATE % 1000) + 3000; //入库锁定
                allocationInfos.Add(allocation23);

                job = BuildNewJobOrder(product16.PKNO, 2, batchno,
                    jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{生产设备}", asset3.ASSET_CODE); //生产设备
                ParamValues.Add("{加工机床}", asset3.ASSET_LABEL); //加工机床标签
                ParamValues.Add("{加工程序号}", program16); //加工程序号
                ParamValues.Add("{原料PKNO}", raw16.PKNO); //原料PKNO

                ParamValues.Add("{成品PKNO}", product16.PKNO); //成品PKNO

                ParamValues.Add("{原始货位PKNO}", allocation24.PKNO); //原始货位PKNO
                ParamValues.Add("{原始货位}", allocation24.INTERFACE_NAME); //原始货位

                ParamValues.Add("{成品货位PKNO}", allocation23.PKNO); //成品货位PKNO
                ParamValues.Add("{成品货位}", allocation23.INTERFACE_NAME); //成品货位

                ParamValues.Add("{轮毂型号}", product16.ITEM_NORM); //轮毂型号

                ParamValues.Add("{机床轮毂型号}", product16.ITEM_NORM); //机床轮毂型号
                ParamValues.Add("{机床轮毂PKNO}", product16.PKNO); //机床轮毂PKNO
                ParamValues.Add("{三坐标轮毂型号}", product17.ITEM_NORM); //三坐标轮毂型号
                ParamValues.Add("{三坐标轮毂PKNO}", product17.PKNO); //三坐标轮毂PKNO

                ParamValues.Add("{检测程序}", "WHSIZE16"); //检测程序，按照机床里面的轮毂型号进行检测
                ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                #endregion

                sFormulaCode = "轮毂生产-" + 3 + "-" + asset3.ASSET_CODE; //--3. 上下料、检测上架--

                #region 形成过程控制

                formulaDetails = wsFms.UseService(s =>
                        s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails) //配方
                {
                    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                    processCtrols.Add(process);
                }

                #endregion

                #endregion

                #region 4. 2#机床 18寸轮毂 上下料、中间检测（18寸）、16寸上架，出（15）入（25）

                allocation15.ALLOCATION_STATE = (allocation15.ALLOCATION_STATE % 1000) + 4000; //出库锁定
                allocationInfos.Add(allocation15);

                allocation25.ALLOCATION_STATE = (allocation25.ALLOCATION_STATE % 1000) + 3000; //入库锁定
                allocationInfos.Add(allocation25);

                job = BuildNewJobOrder(product18.PKNO, 2, batchno,
                    jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{生产设备}", asset2.ASSET_CODE); //生产设备
                ParamValues.Add("{加工机床}", asset2.ASSET_LABEL); //加工机床标签
                ParamValues.Add("{加工程序号}", program18); //加工程序号
                ParamValues.Add("{原料PKNO}", raw18.PKNO); //原料PKNO

                ParamValues.Add("{成品PKNO}", product18.PKNO); //成品PKNO

                ParamValues.Add("{原始货位PKNO}", allocation15.PKNO); //原始货位PKNO
                ParamValues.Add("{原始货位}", allocation15.INTERFACE_NAME); //原始货位

                ParamValues.Add("{成品货位PKNO}", allocation25.PKNO); //成品货位PKNO
                ParamValues.Add("{成品货位}", allocation25.INTERFACE_NAME); //成品货位

                ParamValues.Add("{轮毂型号}", product18.ITEM_NORM); //轮毂型号

                ParamValues.Add("{机床轮毂型号}", product18.ITEM_NORM); //机床轮毂型号
                ParamValues.Add("{机床轮毂PKNO}", product18.PKNO); //机床轮毂PKNO
                ParamValues.Add("{三坐标轮毂型号}", product16.ITEM_NORM); //三坐标轮毂型号
                ParamValues.Add("{三坐标轮毂PKNO}", product16.PKNO); //三坐标轮毂PKNO

                ParamValues.Add("{检测程序}", "WHSIZE18"); //检测程序，按照机床里面的轮毂型号进行检测
                ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                #endregion

                sFormulaCode = "轮毂生产-" + 3 + "-" + asset2.ASSET_CODE; //--3. 上下料、检测上架--

                #region 形成过程控制

                formulaDetails = wsFms.UseService(s =>
                        s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails) //配方
                {
                    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                    processCtrols.Add(process);
                }

                #endregion

                #endregion

                #region 5. 1#机床 17寸轮毂 上下料、中间检测（17寸）、18寸上架，出（23）入（14）

                allocation23.ALLOCATION_STATE = (allocation23.ALLOCATION_STATE % 1000) + 4000; //出库锁定
                allocationInfos.Add(allocation23);

                allocation14.ALLOCATION_STATE = (allocation14.ALLOCATION_STATE % 1000) + 3000; //入库锁定
                allocationInfos.Add(allocation14);

                job = BuildNewJobOrder(product17.PKNO, 2, batchno,
                    jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{生产设备}", asset1.ASSET_CODE); //生产设备
                ParamValues.Add("{加工机床}", asset1.ASSET_LABEL); //加工机床标签
                ParamValues.Add("{加工程序号}", program17); //加工程序号
                ParamValues.Add("{原料PKNO}", raw17.PKNO); //原料PKNO

                ParamValues.Add("{成品PKNO}", product18.PKNO); //成品PKNO
                ParamValues.Add("{原始货位PKNO}", allocation23.PKNO); //原始货位PKNO
                ParamValues.Add("{原始货位}", allocation23.INTERFACE_NAME); //原始货位

                ParamValues.Add("{成品货位PKNO}", allocation14.PKNO); //成品货位PKNO
                ParamValues.Add("{成品货位}", allocation14.INTERFACE_NAME); //成品货位

                ParamValues.Add("{轮毂型号}", product17.ITEM_NORM); //轮毂型号

                ParamValues.Add("{机床轮毂型号}", product17.ITEM_NORM); //机床轮毂型号
                ParamValues.Add("{机床轮毂PKNO}", product17.PKNO); //机床轮毂PKNO
                ParamValues.Add("{三坐标轮毂型号}", product18.ITEM_NORM); //三坐标轮毂型号
                ParamValues.Add("{三坐标轮毂PKNO}", product18.PKNO); //三坐标轮毂PKNO

                ParamValues.Add("{检测程序}", "WHSIZE17"); //检测程序，按照机床里面的轮毂型号进行检测
                ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                #endregion

                sFormulaCode = "轮毂生产-" + 3 + "-" + asset1.ASSET_CODE; //--3. 上下料、检测上架--

                #region 形成过程控制

                formulaDetails = wsFms.UseService(s =>
                        s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails) //配方
                {
                    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                    processCtrols.Add(process);
                }

                #endregion

                #endregion

                #region 6. 3#机床 16寸轮毂 上下料、中间检测（16寸）、17寸上架，出（25）入（13）

                allocation25.ALLOCATION_STATE = (allocation25.ALLOCATION_STATE % 1000) + 4000; //出库锁定
                allocationInfos.Add(allocation25);

                allocation13.ALLOCATION_STATE = (allocation13.ALLOCATION_STATE % 1000) + 3000; //入库锁定
                allocationInfos.Add(allocation13);

                job = BuildNewJobOrder(product16.PKNO, 2, batchno,
                    jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{生产设备}", asset3.ASSET_CODE); //生产设备
                ParamValues.Add("{加工机床}", asset3.ASSET_LABEL); //加工机床标签
                ParamValues.Add("{加工程序号}", program16); //加工程序号
                ParamValues.Add("{原料PKNO}", raw16.PKNO); //原料PKNO

                ParamValues.Add("{成品PKNO}", product18.PKNO); //成品PKNO
                ParamValues.Add("{原始货位PKNO}", allocation25.PKNO); //原始货位PKNO
                ParamValues.Add("{原始货位}", allocation25.INTERFACE_NAME); //原始货位

                ParamValues.Add("{成品货位PKNO}", allocation13.PKNO); //成品货位PKNO
                ParamValues.Add("{成品货位}", allocation13.INTERFACE_NAME); //成品货位

                ParamValues.Add("{轮毂型号}", product16.ITEM_NORM); //轮毂型号

                ParamValues.Add("{机床轮毂型号}", product16.ITEM_NORM); //机床轮毂型号
                ParamValues.Add("{机床轮毂PKNO}", product16.PKNO); //机床轮毂PKNO
                ParamValues.Add("{三坐标轮毂型号}", product17.ITEM_NORM); //三坐标轮毂型号
                ParamValues.Add("{三坐标轮毂PKNO}", product17.PKNO); //三坐标轮毂PKNO

                ParamValues.Add("{检测程序}", "WHSIZE16"); //检测程序，按照机床里面的轮毂型号进行检测
                ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                #endregion

                sFormulaCode = "轮毂生产-" + 3 + "-" + asset3.ASSET_CODE; //--3. 上下料、检测上架--

                #region 形成过程控制

                formulaDetails = wsFms.UseService(s =>
                        s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails) //配方
                {
                    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                    processCtrols.Add(process);
                }

                #endregion

                #endregion

                #endregion

                DeviceProcessControl.PauseByLine(CBaseData.CurLinePKNO); //暂停，防止任务直接执行

                #region 保存数据

                foreach (var allocationInfo in allocationInfos)
                {
                    ws.UseService(s => s.UpdateWmsAllocationInfo(allocationInfo));
                    Thread.Sleep(100);
                }

                foreach (var ctrol in processCtrols)
                {
                    wsPlm.UseService(s => s.AddMesProcessCtrol(ctrol));
                    Thread.Sleep(100);
                }

                foreach (var jobOrder in jobOrders) //订单
                {
                    wsPlm.UseService(s => s.AddMesJobOrder(jobOrder));
                    Thread.Sleep(100);
                }

                #endregion

                DeviceProcessControl.RunByLine(CBaseData.CurLinePKNO); //启动动作流程

                WaitLoading.SetDefault(this);

                WPFMessageBox.ShowInfo("循环演示流程已下达，请启动机器人开始程序.\r\n如果需要添加循环，直接再次点击本按钮即可。", "循环演示");

            }).Start();
        }

        //停止循环演示流程
        private void bStopAllow_Click(object sender, RoutedEventArgs e)
        {
            string batchNo = "";
            WcfClient<IPLMService> ws2 = new WcfClient<IPLMService>();
            List<MesJobOrder> mesJobOrders =
                ws2.UseService(s =>
                        s.GetMesJobOrders(
                            $"USE_FLAG = 1 AND RUN_STATE < 100 AND LINE_PKNO = '{CBaseData.CurLinePKNO}'"))
                    .OrderBy(c => c.CREATION_DATE).ToList(); //未执行的订单
            if (mesJobOrders.Count <= 0)
            {

                if (WPFMessageBox.ShowConfirm("目前没有正在执行的任务，确定要下达循环演示的收料流程吗？\r\n" +
                                              "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\r\n" +
                                              "请确保：刚才执行的是循环演示流程，并且状态都正确！！！\r\n" +
                                              "任务下达后还将运行一段时间，请关注系统状态！\r\n" +
                                              "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\r\n", "停止循环演示") !=
                    WPFMessageBoxResult.OK) return;
            }
            else
            {
                MesJobOrder lastJob = mesJobOrders.LastOrDefault();
                batchNo = lastJob?.BATCH_NO;
                if (string.IsNullOrEmpty(batchNo) || !batchNo.Contains("循环演示"))
                {
                    WaitLoading.SetDefault(this);
                    WPFMessageBox.ShowError("目前还有未完成的其他任务，请将任务完成后再添加循环演示流程！", "停止循环演示");
                    return;
                }

                if (WPFMessageBox.ShowConfirm("确定要下达循环演示的收料流程吗？\r\n" +
                                              "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\r\n" +
                                              "请确保：所执行的任务没有手动修改过！！！\r\n" +
                                              "任务下达后还将运行一段时间，请关注系统状态！\r\n" +
                                              "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\r\n", "停止循环演示") !=
                    WPFMessageBoxResult.OK) return;
            }

            //后台执行添加
            new Thread(delegate ()
            {
                WaitLoading.SetWait(this);

                #region 初始化各种参数

                DateTime jobOrderTime = DateTime.Now.AddSeconds(-12);
                int iJobOrderIndex = 0;

                List<MesJobOrder> jobOrders = new List<MesJobOrder>(); //所有订单
                List<MesProcessCtrol> processCtrols = new List<MesProcessCtrol>(); //控制流程
                List<WmsAllocationInfo> allocationInfos = new List<WmsAllocationInfo>(); //需要修改的货位

                Dictionary<string, string> ParamValues = new Dictionary<string, string>();
                MesJobOrder job = null;
                string sFormulaCode = "";
                List<FmsActionFormulaDetail> formulaDetails;

                AmAssetMasterN asset1 = wsEAM.UseService(s => s.GetAmAssetMasterNs("ASSET_CODE = 'SH00003'"))
                    .FirstOrDefault(); //1#机床 - 创圣特尔
                AmAssetMasterN asset2 = wsEAM.UseService(s => s.GetAmAssetMasterNs("ASSET_CODE = 'SH00002'"))
                    .FirstOrDefault(); //2#机床 - 京元登
                AmAssetMasterN asset3 = wsEAM.UseService(s => s.GetAmAssetMasterNs("ASSET_CODE = 'SH00001'"))
                    .FirstOrDefault(); //3#机床 - 斌胜
                if ((asset1 == null) || (asset2 == null) || (asset3 == null))
                {
                    WaitLoading.SetDefault(this);
                    WPFMessageBox.ShowError("机床信息没有正确初始化，请联系管理员进行配置.", "停止循环演示");
                    return;
                }

                List<RsItemMaster> items = wsRsm.UseService(s => s.GetRsItemMasters("USE_FLAG = 1"));

                RsItemMaster product16 = items.FirstOrDefault(c => c.ITEM_NORM == "1" && c.NORM_CLASS == 10); //产品信息
                RsItemMaster product17 = items.FirstOrDefault(c => c.ITEM_NORM == "2" && c.NORM_CLASS == 10); //产品信息
                RsItemMaster product18 = items.FirstOrDefault(c => c.ITEM_NORM == "3" && c.NORM_CLASS == 10); //产品信息

                RsItemMaster raw16 = items.FirstOrDefault(c => c.ITEM_NORM == "1" && c.NORM_CLASS == 1); //原料信息
                RsItemMaster raw17 = items.FirstOrDefault(c => c.ITEM_NORM == "2" && c.NORM_CLASS == 1); //原料信息
                RsItemMaster raw18 = items.FirstOrDefault(c => c.ITEM_NORM == "3" && c.NORM_CLASS == 1); //原料信息
                if ((raw16 == null) || (raw17 == null) || (raw18 == null) || (product16 == null) ||
                    (product17 == null) || (product18 == null))
                {
                    WaitLoading.SetDefault(this);
                    WPFMessageBox.ShowError("轮毂物料信息没有正确初始化，请联系管理员进行配置.", "停止循环演示");
                    return;
                }

                List<WmsAllocationInfo> allcations = ws.UseService(s => s.GetWmsAllocationInfos($"USE_FLAG = 1"));

                WmsAllocationInfo allocation14 = allcations.FirstOrDefault(c => c.ALLOCATION_CODE == "14");
                WmsAllocationInfo allocation15 = allcations.FirstOrDefault(c => c.ALLOCATION_CODE == "15");
                WmsAllocationInfo allocation24 = allcations.FirstOrDefault(c => c.ALLOCATION_CODE == "24");
                WmsAllocationInfo allocation25 = allcations.FirstOrDefault(c => c.ALLOCATION_CODE == "25");
                WmsAllocationInfo allocation13 = allcations.FirstOrDefault(c => c.ALLOCATION_CODE == "13");
                WmsAllocationInfo allocation23 = allcations.FirstOrDefault(c => c.ALLOCATION_CODE == "23");

                if ((allocation14 == null) || (allocation14.ALLOCATION_STATE < 0) ||
                    (allocation15 == null) || (allocation15.ALLOCATION_STATE < 0) ||
                    (allocation24 == null) || (allocation24.ALLOCATION_STATE < 0) ||
                    (allocation25 == null) || (allocation25.ALLOCATION_STATE < 0) ||
                    (allocation13 == null) || (allocation13.ALLOCATION_STATE < 0) ||
                    (allocation23 == null) || (allocation23.ALLOCATION_STATE < 0))
                {
                    WaitLoading.SetDefault(this);
                    WPFMessageBox.ShowError("货位设置不正确，请联系管理员.", "停止循环演示");
                    return;
                }

                string program16 = "70"; //加工程序 16寸
                string program17 = "80"; //加工程序 17寸
                string program18 = "90"; //加工程序 18寸

                #endregion

                List<MesJobOrder> CancelJobs = new List<MesJobOrder>();

                DeviceProcessControl.PauseByLine(CBaseData.CurLinePKNO); //暂停，防止任务直接执行

                Thread.Sleep(1000);

                List<MesJobOrder> jobs =
                    ws2.UseService(s =>
                            s.GetMesJobOrders(
                                $"USE_FLAG = 1 AND BATCH_NO = {batchNo} AND LINE_PKNO = '{CBaseData.CurLinePKNO}' "))
                        .OrderBy(c => c.CREATION_DATE).ToList(); //未执行的订单

                #region 移除多余的循环

                WmsAllocationInfo allocation24Or25 = allocation24;
                WmsAllocationInfo allocation15Or14 = allocation15;
                WmsAllocationInfo allocation23Or13 = allocation23;
                WmsAllocationInfo allocation25Or24 = allocation25;

                int iDelCount = 0;
                List<MesJobOrder> canOrders = jobs.Where(c => c.RUN_STATE <= 10).OrderByDescending(c => c.CREATION_DATE)
                    .ToList();

                if ((jobs.Count > 6) && (canOrders.Count > 3))
                {
                    int delCount = canOrders.Count / 3;
                    int max = (jobs.Count - 6) / 3;
                    if (delCount > max)
                    {
                        delCount = max;
                    }

                    for (int i = 0; i < delCount; i++)
                    {
                        if (canOrders.Count <= i * 3 + 2)
                        {
                            break;
                        }

                        CancelJobs.Add(canOrders[i * 3]);
                        CancelJobs.Add(canOrders[i * 3 + 1]);
                        CancelJobs.Add(canOrders[i * 3 + 2]);
                        iDelCount++;
                    }
                }

                if (iDelCount % 2 == 1)
                {
                    allocation24Or25 = allocation25;
                    allocation15Or14 = allocation14;
                    allocation23Or13 = allocation13;
                    allocation25Or24 = allocation24;
                }

                #endregion

                #region 添加收料任务

                #region 1. 2#机床 只下料（18寸）、中间检测（18寸）、16寸上架，入（24/25）

                allocation24Or25.ALLOCATION_STATE = (allocation24Or25.ALLOCATION_STATE % 1000) + 3000; //入库锁定
                allocationInfos.Add(allocation24Or25);

                job = BuildNewJobOrder(product18.PKNO, 2, "结束循环", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{生产设备}", asset2.ASSET_CODE); //生产设备
                ParamValues.Add("{加工机床}", asset2.ASSET_LABEL); //加工机床标签
                ParamValues.Add("{加工程序号}", program18); //加工程序号
                ParamValues.Add("{原料PKNO}", raw18.PKNO); //原料PKNO
                ParamValues.Add("{成品PKNO}", product18.PKNO); //成品PKNO

                ParamValues.Add("{原始货位PKNO}", allocation24Or25.PKNO); //原始货位PKNO
                ParamValues.Add("{原始货位}", allocation24Or25.INTERFACE_NAME); //原始货位
                ParamValues.Add("{成品货位PKNO}", allocation24Or25.PKNO); //成品货位PKNO
                ParamValues.Add("{成品货位}", allocation24Or25.INTERFACE_NAME); //成品货位

                ParamValues.Add("{轮毂型号}", product18.ITEM_NORM); //轮毂型号

                ParamValues.Add("{机床轮毂型号}", product18.ITEM_NORM); //机床轮毂型号
                ParamValues.Add("{机床轮毂PKNO}", product18.PKNO); //机床轮毂PKNO
                ParamValues.Add("{三坐标轮毂型号}", product16.ITEM_NORM); //三坐标轮毂型号
                ParamValues.Add("{三坐标轮毂PKNO}", product16.PKNO); //三坐标轮毂PKNO

                ParamValues.Add("{检测程序}", "WHSIZE18"); //检测程序，按照机床里面的轮毂型号进行检测
                ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                #endregion

                sFormulaCode = "轮毂生产-" + 5 + "-" + asset2.ASSET_CODE; //--5. 只下料、检测上架--

                #region 形成过程控制

                formulaDetails = wsFms.UseService(s =>
                        s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails) //配方
                {
                    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                    processCtrols.Add(process);
                }

                #endregion

                #endregion

                #region 9. 1#机床 只下料（17寸）、中间检测（17寸）、18寸上架，入（15/14）

                allocation15Or14.ALLOCATION_STATE = (allocation15Or14.ALLOCATION_STATE % 1000) + 3000; //入库锁定
                allocationInfos.Add(allocation15Or14);

                job = BuildNewJobOrder(product17.PKNO, 2, "结束循环", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{生产设备}", asset1.ASSET_CODE); //生产设备
                ParamValues.Add("{加工机床}", asset1.ASSET_LABEL); //加工机床标签
                ParamValues.Add("{加工程序号}", program17); //加工程序号
                ParamValues.Add("{原料PKNO}", raw17.PKNO); //原料PKNO
                ParamValues.Add("{成品PKNO}", product17.PKNO); //成品PKNO

                ParamValues.Add("{原始货位PKNO}", allocation15Or14.PKNO); //原始货位PKNO
                ParamValues.Add("{原始货位}", allocation15Or14.INTERFACE_NAME); //原始货位
                ParamValues.Add("{成品货位PKNO}", allocation15Or14.PKNO); //成品货位PKNO
                ParamValues.Add("{成品货位}", allocation15Or14.INTERFACE_NAME); //成品货位

                ParamValues.Add("{轮毂型号}", product17.ITEM_NORM); //轮毂型号

                ParamValues.Add("{机床轮毂型号}", product17.ITEM_NORM); //机床轮毂型号
                ParamValues.Add("{机床轮毂PKNO}", product17.PKNO); //机床轮毂PKNO
                ParamValues.Add("{三坐标轮毂型号}", product18.ITEM_NORM); //三坐标轮毂型号
                ParamValues.Add("{三坐标轮毂PKNO}", product18.PKNO); //三坐标轮毂PKNO

                ParamValues.Add("{检测程序}", "WHSIZE17"); //检测程序，按照机床里面的轮毂型号进行检测
                ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                #endregion

                sFormulaCode = "轮毂生产-" + 5 + "-" + asset1.ASSET_CODE; //--5. 只下料、检测上架--

                #region 形成过程控制

                formulaDetails = wsFms.UseService(s =>
                        s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails) //配方
                {
                    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                    processCtrols.Add(process);
                }

                #endregion

                #endregion

                #region 7. 3#机床 只下料（16寸）、中间检测（16寸）、17寸上架，入（23/13）

                allocation23Or13.ALLOCATION_STATE = (allocation23Or13.ALLOCATION_STATE % 1000) + 3000; //入库锁定
                allocationInfos.Add(allocation23Or13);

                job = BuildNewJobOrder(product16.PKNO, 2, "结束循环", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{生产设备}", asset3.ASSET_CODE); //生产设备
                ParamValues.Add("{加工机床}", asset3.ASSET_LABEL); //加工机床标签
                ParamValues.Add("{加工程序号}", program16); //加工程序号
                ParamValues.Add("{原料PKNO}", raw16.PKNO); //原料PKNO
                ParamValues.Add("{成品PKNO}", product16.PKNO); //成品PKNO

                ParamValues.Add("{原始货位PKNO}", allocation23Or13.PKNO); //原始货位PKNO
                ParamValues.Add("{原始货位}", allocation23Or13.INTERFACE_NAME); //原始货位

                ParamValues.Add("{成品货位PKNO}", allocation23Or13.PKNO); //成品货位PKNO
                ParamValues.Add("{成品货位}", allocation23Or13.INTERFACE_NAME); //成品货位

                ParamValues.Add("{轮毂型号}", product16.ITEM_NORM); //轮毂型号

                ParamValues.Add("{机床轮毂型号}", product16.ITEM_NORM); //机床轮毂型号
                ParamValues.Add("{机床轮毂PKNO}", product16.PKNO); //机床轮毂PKNO
                ParamValues.Add("{三坐标轮毂型号}", product17.ITEM_NORM); //三坐标轮毂型号
                ParamValues.Add("{三坐标轮毂PKNO}", product17.PKNO); //三坐标轮毂PKNO

                ParamValues.Add("{检测程序}", "WHSIZE16"); //检测程序，按照机床里面的轮毂型号进行检测
                ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                #endregion

                sFormulaCode = "轮毂生产-" + 5 + "-" + asset3.ASSET_CODE; //--5. 只下料、检测上架--

                #region 形成过程控制

                formulaDetails = wsFms.UseService(s =>
                        s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails) //配方
                {
                    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                    processCtrols.Add(process);
                }

                #endregion

                #endregion

                #region 10. 16寸只上料架，入（25/24）

                allocation25Or24.ALLOCATION_STATE = (allocation25Or24.ALLOCATION_STATE % 1000) + 3000; //入库锁定
                allocationInfos.Add(allocation25Or24);

                job = BuildNewJobOrder(product16.PKNO, 2, "结束循环", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{生产设备}", asset3.ASSET_CODE); //生产设备
                ParamValues.Add("{加工机床}", asset3.ASSET_LABEL); //加工机床标签
                ParamValues.Add("{加工程序号}", program16); //加工程序号
                ParamValues.Add("{原料PKNO}", raw16.PKNO); //原料PKNO
                ParamValues.Add("{成品PKNO}", product16.PKNO); //成品PKNO

                ParamValues.Add("{原始货位PKNO}", allocation25Or24.PKNO); //原始货位PKNO
                ParamValues.Add("{原始货位}", allocation25Or24.INTERFACE_NAME); //原始货位
                ParamValues.Add("{成品货位PKNO}", allocation25Or24.PKNO); //成品货位PKNO
                ParamValues.Add("{成品货位}", allocation25Or24.INTERFACE_NAME); //成品货位

                ParamValues.Add("{轮毂型号}", product16.ITEM_NORM); //轮毂型号

                ParamValues.Add("{机床轮毂型号}", ""); //机床轮毂型号
                ParamValues.Add("{机床轮毂PKNO}", ""); //机床轮毂PKNO
                ParamValues.Add("{三坐标轮毂型号}", product16.ITEM_NORM); //三坐标轮毂型号
                ParamValues.Add("{三坐标轮毂PKNO}", product16.PKNO); //三坐标轮毂PKNO

                ParamValues.Add("{检测程序}", "WHSIZE16"); //检测程序，按照机床里面的轮毂型号进行检测
                ParamValues.Add("{检测时间}", DateTime.Now.ToString("ddMMyyyyHHss")); //检测时间

                #endregion

                sFormulaCode = "轮毂生产-" + 6; //--6. 只上料架--

                #region 形成过程控制

                formulaDetails = wsFms.UseService(s =>
                        s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                    .OrderBy(c => c.PROCESS_INDEX)
                    .ToList();

                foreach (var detail in formulaDetails) //配方
                {
                    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                    processCtrols.Add(process);
                }

                #endregion

                #endregion

                //#region 1. 1#机床换刀  T10 => 40

                //TmsToolsMaster mToolsMasterUp = wsTMS.UseService(s => s.GetTmsToolsMasters("TOOLS_CODE = 38")).FirstOrDefault();  //刀具库存
                //if ((mToolsMasterUp == null) || (mToolsMasterUp.TOOLS_POSITION != 1))
                //{
                //    WaitLoading.SetDefault(this);
                //    WPFMessageBox.ShowError("刀具【40】没有在库，请核实状态后再进行下达指令.", "演示程序");
                //    return;
                //}

                //TmsDeviceToolsPos mTmsDeviceToolsPos = wsTMS.UseService(s =>
                //        s.GetTmsDeviceToolsPoss($"DEVICE_PKNO = '{asset1.PKNO}' AND TOOLS_POS_NO = 10 AND USE_FLAG = 1"))
                //    .FirstOrDefault();  //机床刀位信息
                //if (mTmsDeviceToolsPos == null)
                //{
                //    WaitLoading.SetDefault(this);
                //    WPFMessageBox.ShowError("系统没有正确初始化，请联系管理员进行配置.", "演示程序");
                //    return;
                //}

                //TmsToolsMaster mToolsMasterDown =
                //    wsTMS.UseService(s => s.GetTmsToolsMasterById(mTmsDeviceToolsPos.TOOLS_PKNO));
                //if (mToolsMasterDown == null)
                //{
                //    WaitLoading.SetDefault(this);
                //    WPFMessageBox.ShowError("选中的需要换下的机床刀具信息不存在，请核实.", "演示程序");
                //    return;
                //}

                //#region  11. 1#机床换刀  T10 => 10

                //#region 增加参数

                //ParamValues.Clear();

                //ParamValues.Add("{机床刀号}", mTmsDeviceToolsPos.TOOLS_POS_NO);  //机床刀号
                //ParamValues.Add("{卸下刀具编号}", mToolsMasterUp.TOOLS_CODE.PadRight(25));  //卸下刀具编号 => 之前装上的
                //ParamValues.Add("{装上刀具编号}", mToolsMasterDown.TOOLS_CODE.PadRight(25));  //装上刀具编号  => 之前卸下的
                //ParamValues.Add("{装上刀具PKNO}", mToolsMasterDown.PKNO);  //装上刀具PKNO  => 之前卸下的
                //ParamValues.Add("{卸下刀具PKNO}", mToolsMasterUp.PKNO);  //卸下刀具PKNO    => 之前装上的
                //ParamValues.Add("{长度形状补偿}", SafeConverter.SafeToStr(mToolsMasterDown.COMPENSATION_SHAPE_LENGTH));  //长度形状补偿 - 装上  => 之前卸下的
                //ParamValues.Add("{半径形状补偿}", SafeConverter.SafeToStr(mToolsMasterDown.COMPENSATION_SHAPE_DIAMETER));  //半径形状补偿 - 装上  => 之前卸下的

                //ParamValues.Add("{卸下刀位PKNO}", mTmsDeviceToolsPos.PKNO);  //卸下刀位PKNO
                //ParamValues.Add("{装上刀位PKNO}", mTmsDeviceToolsPos.PKNO);  //装上刀位PKNO

                //ParamValues.Add("{装刀机床PKNO}", asset1?.PKNO);  //装刀机床PKNO

                //#endregion

                //job = BuildNewJobOrder("", 5, "#1演示流程", jobOrderTime.AddSeconds(iJobOrderIndex++));  //形成订单
                //jobOrders.Add(job);

                //sFormulaCode = "换刀-" + asset1.ASSET_CODE;

                //formulaDetails = wsFms.UseService(s =>
                //        s.GetFmsActionFormulaDetails($"FORMULA_CODE = '{sFormulaCode}' AND USE_FLAG= 1"))
                //    .OrderBy(c => c.PROCESS_INDEX)
                //    .ToList();

                //foreach (var detail in formulaDetails)  //配方
                //{
                //    MesProcessCtrol process = BuildNewProcess(job, detail, ParamValues);

                //    processCtrols.Add(process);
                //}

                //#endregion

                //#endregion

                #endregion

                #region 保存数据

                foreach (var jobOrder in CancelJobs)
                {
                    jobOrder.ACT_FINISH_TIME = DateTime.Now;
                    jobOrder.RUN_STATE = 102; //手动取消
                    wsPlm.UseService(s => s.UpdateMesJobOrder(jobOrder));
                    Thread.Sleep(100);
                }

                foreach (var allocationInfo in allocationInfos)
                {
                    ws.UseService(s => s.UpdateWmsAllocationInfo(allocationInfo));
                    Thread.Sleep(100);
                }

                foreach (var ctrol in processCtrols)
                {
                    wsPlm.UseService(s => s.AddMesProcessCtrol(ctrol));
                    Thread.Sleep(100);
                }

                foreach (var jobOrder in jobOrders) //订单
                {
                    wsPlm.UseService(s => s.AddMesJobOrder(jobOrder));
                    Thread.Sleep(100);
                }

                #endregion

                DeviceProcessControl.RunByLine(CBaseData.CurLinePKNO); //启动动作流程

                WaitLoading.SetDefault(this);

                WPFMessageBox.ShowInfo("停止循环演示流程已下达，系统还将运行一段时间进行收料作业。", "停止循环演示");

            }).Start();
        }
    }
}
