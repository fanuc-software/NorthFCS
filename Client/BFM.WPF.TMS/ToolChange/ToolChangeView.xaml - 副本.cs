using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.EAMService;
using BFM.Server.DataAsset.FMSService;
using BFM.Server.DataAsset.PLMService;
using BFM.Server.DataAsset.TMSService;
using BFM.Server.DataAsset.WMSService;
using BFM.WPF.SDM.TableNO;

namespace BFM.WPF.TMS
{
    /// <summary>
    /// ToolChangeView.xaml 的交互逻辑
    /// </summary>
    public partial class ToolChangeView : Page
    {
        private WcfClient<IPLMService> wsPLM = new WcfClient<IPLMService>();
        private WcfClient<ITMSService> ws = new WcfClient<ITMSService>();
        private WcfClient<IEAMService> wsEAM = new WcfClient<IEAMService>();
        private WcfClient<IWMSService> wsWMS = new WcfClient<IWMSService>();
        private WcfClient<IFMSService> wsFms = new WcfClient<IFMSService>();
        public ToolChangeView()
        {
            InitializeComponent();
            ComAsset.ItemsSource = wsEAM.UseService(s => s.GetAmAssetMasterNs("USE_FLAG = 1 AND ASSET_TYPE = '机床'"));
            GridControlInvTool.ItemsSource = ws.UseService(s => s.GetTmsToolsMasters("USE_FLAG > 0 AND ")).OrderBy(n => n.CREATION_DATE).ToList(); ;
        }

        private void ComAsset_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            AmAssetMasterN asset = ComAsset.SelectedItem as AmAssetMasterN;
            if (asset == null)
            {
                GridControlDeviceTool.ItemsSource = null;
            }
            else
            {
                GridControlDeviceTool.ItemsSource =
                    ws.UseService(s => s.GetTmsDeviceToolsPoss($"USE_FLAG = 1 AND DEVICE_PKNO = '{asset.PKNO}'"))
                        .OrderBy(c => c.TOOLS_POS_NO)
                        .ToList();
            }
        }

        private void GridControl_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {

        }

        private void BtnChange_Click(object sender, RoutedEventArgs e)
        {
            TmsToolsMaster mToolsMaster = GridControlInvTool.SelectedItem as TmsToolsMaster;
            TmsDeviceToolsPos mTmsDeviceToolsPos = GridControlDeviceTool.SelectedItem as TmsDeviceToolsPos;

            if (mToolsMaster != null)
            {
                if (mTmsDeviceToolsPos != null)
                {
                    TmsToolsMaster mToolsMasterold = ws.UseService(s => s.GetTmsToolsMasterById(mTmsDeviceToolsPos.TOOLS_PKNO));
                    if (mToolsMasterold != null)
                    {
                        mToolsMasterold.TOOLS_POSITION = 0;
                        mToolsMasterold.TOOLS_POSITION_PKNO = "";
                        ws.UseService(s => s.UpdateTmsToolsMaster(mToolsMasterold));
                    }
                    mTmsDeviceToolsPos.TOOLS_PKNO = mToolsMaster.PKNO;
                    mTmsDeviceToolsPos.TOOLS_STATE = 1;
                    mTmsDeviceToolsPos.UPDATED_BY = CBaseData.LoginName;
                    ws.UseService(s => s.UpdateTmsDeviceToolsPos(mTmsDeviceToolsPos));
                }

                mToolsMaster.TOOLS_POSITION = 2;
                mToolsMaster.TOOLS_POSITION_PKNO = mTmsDeviceToolsPos.DEVICE_PKNO;
                ws.UseService(s => s.UpdateTmsToolsMaster(mToolsMaster));
                GridControlInvTool.ItemsSource = ws.UseService(s => s.GetTmsToolsMasters("USE_FLAG > 0"));
            }
        }

        private void BtnAutoChange_Click(object sender, RoutedEventArgs e)
        {
            TmsToolsMaster mToolsMaster = GridControlInvTool.SelectedItem as TmsToolsMaster;
            TmsDeviceToolsPos mTmsDeviceToolsPos = GridControlDeviceTool.SelectedItem as TmsDeviceToolsPos;
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
                ITEM_PKNO = "", // TODO:暂无
                JOB_ORDER_NO = TableNOHelper.GetNewNO("MES_JOB_ORDER.JOB_ORDER_NO", "J"),
                BATCH_NO = "",
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
                    ITEM_PKNO ="",   //成品PKNO TODO:暂无
                    JOB_ORDER_PKNO = jobOrder.PKNO,
                    JOB_ORDER = jobOrder.JOB_ORDER_NO,
                    SUB_JOB_ORDER_NO = "",
                    ROUTING_DETAIL_PKNO = "",  //

                    #endregion

                    PROCESS_CTROL_NAME = detail.FORMULA_DETAIL_NAME,  //名称
                    PROCESS_DEVICE_PKNO = Replace(detail.PROCESS_DEVICE_PKNO, ParamValues),             //生产设备
                    PROCESS_PROGRAM_NO = Replace(detail.PROCESS_PROGRAM_NO, ParamValues),              //加工程序号
                    PROCESS_PROGRAM_CONTENT = detail.PROCESS_PROGRAM_CONTENT,         //加工程序内容
                    PROCESS_INDEX = detail.PROCESS_INDEX,                   //工序顺序
                    BEGIN_ITEM_PKNO = Replace(detail.BEGIN_ITEM_PKNO, ParamValues),                 //生产前项目PKNO
                    FINISH_ITEM_PKNO = Replace(detail.FINISH_ITEM_PKNO, ParamValues),                //生产后项目PKNO
                    BEGIN_POSITION = Replace(detail.BEGIN_POSITION, ParamValues),                  //生产前位置
                    FINISH_POSITION = Replace(detail.FINISH_POSITION, ParamValues),                 //生产后位置
                    PALLET_NO = detail.PALLET_NO,                       //托盘号
                    PROCESS_ACTION_TYPE = detail.PROCESS_ACTION_TYPE,          //工序动作类型
                    PROCESS_ACTION_PKNO = detail.PROCESS_ACTION_PKNO,             //工序动作控制PKNO

                    PROCESS_ACTION_PARAM1_VALUE = Replace(detail.PROCESS_ACTION_PARAM1_VALUE, ParamValues),     //工序动作参数1
                    PROCESS_ACTION_PARAM2_VALUE = Replace(detail.PROCESS_ACTION_PARAM2_VALUE, ParamValues),     //工序动作参数2

                    CUR_PRODUCT_CODE_PKNO = "",           //当前生产加工的产品编码PKNO
                    PROCESS_QTY = 1,                     //加工数量（上线数量）
                    COMPLETE_QTY = 0,   //完成数量
                    QUALIFIED_QTY = 0,  //合格数量
                    PROCESS_STATE = 1,  //准备完成

                    CREATION_DATE = DateTime.Now,                   //创建日期
                    CREATED_BY = CBaseData.LoginNO,                      //创建人
                    LAST_UPDATE_DATE = DateTime.Now,  //最后修改日期
                    USE_FLAG = detail.USE_FLAG,                        //启用标识
                    REMARK = "",                          //备注
                };

                processCtrols.Add(process);
            }

            Cursor = Cursors.Wait;

            wsPLM.UseService(s => s.AddMesJobOrder(jobOrder));

            foreach (MesProcessCtrol processCtrol in processCtrols)
            {
                wsPLM.UseService(s => s.AddMesProcessCtrol(processCtrol));
            }

            Cursor = Cursors.Arrow;

            MessageBox.Show("换刀下单成功.");
            GridControlInvTool.ItemsSource = ws.UseService(s => s.GetTmsToolsMasters("USE_FLAG > 0")).OrderBy(n => n.CREATION_DATE).ToList(); ;
        }
        public string Replace(string oldValue, Dictionary<string, string> paramValues)
        {
            if (string.IsNullOrEmpty(oldValue))
            {
                return "";
            }

            if (paramValues.Count <= 0)
            {
                return oldValue;
            }

            string sResult = oldValue;

            foreach (var paramValue in paramValues)
            {
                sResult = sResult.Replace(paramValue.Key, paramValue.Value);
            }

            if (sResult.Contains("{") && sResult.Contains("}"))
            {
                Console.WriteLine("参数没有完全替换成功.");
            }

            return sResult;
        }
    }
}

