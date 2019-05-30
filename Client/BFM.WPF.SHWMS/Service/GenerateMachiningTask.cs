using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
using BFM.WPF.FMS;
using BFM.WPF.SDM.TableNO;
using BFM.WPF.SHWMS.ViewModel;

namespace BFM.WPF.SHWMS.Service
{
    public class GenerateMachiningTask
    {
        public string LaserPicName = "";

        private WcfClient<IWMSService> ws = new WcfClient<IWMSService>();
        private WcfClient<IRSMService> wsRsm = new WcfClient<IRSMService>();
        private WcfClient<IPLMService> wsPlm = new WcfClient<IPLMService>();
        private WcfClient<IFMSService> wsFms = new WcfClient<IFMSService>();

        public event Action<string, string> ShowTaskInfoEvent;


        public void GenerateMachiningTask_Piece4()
        {
            string sLathePieceNumOneTime = "4";

            WcfClient<IPLMService> ws2 = new WcfClient<IPLMService>();
            string LineCode = CBaseData.CurLinePKNO;//加工单元
            List<MesJobOrder> mesJobOrders =
                ws2.UseService(s =>
                        s.GetMesJobOrders(
                            $"USE_FLAG = 1 AND RUN_STATE < 100 AND LINE_PKNO = '{LineCode}'"))
                    .OrderBy(c => c.CREATION_DATE).ToList();
            if (mesJobOrders.Count > 20)
            {
                ShowTaskInfoEvent?.Invoke("当前订单过多，请等待加工完成", "加工工单");
                return;
            }


            //后台执行添加
            new Thread(delegate ()
            {
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
                List<RsItemMaster> items = wsRsm.UseService(s => s.GetRsItemMasters("USE_FLAG = 1"));

                RsItemMaster gyroscope = items.FirstOrDefault(c => c.ITEM_NAME == "指尖陀螺"); //产品信息


                #region 2.车床上料
                job = BuildNewJobOrder(gyroscope.PKNO, 2, "陀螺生产", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{图片名称}", LaserPicName); //定制图片
                ParamValues.Add("{车床上下料参数}", sLathePieceNumOneTime);
                //ParamValues.Add("{加工数量}", this.txt_Qty2.Text);

                #endregion

                sFormulaCode = "车床上料";

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

                #region 3.加工中心上料
                job = BuildNewJobOrder(gyroscope.PKNO, 2, "陀螺生产", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{图片名称}", LaserPicName); //定制图片
                ParamValues.Add("{车床上下料参数}", sLathePieceNumOneTime);
                //ParamValues.Add("{加工数量}", this.txt_Qty2.Text);

                #endregion

                sFormulaCode = "加工中心上料";

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

                #region 4.1.AGV充电 skip
                if (true)
                {
                    job = BuildNewJobOrder(gyroscope.PKNO, 2, "陀螺生产", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                    jobOrders.Add(job);

                    #region --设定参数--

                    ParamValues.Clear();
                    ParamValues.Add("{图片名称}", LaserPicName); //定制图片
                    ParamValues.Add("{车床上下料参数}", sLathePieceNumOneTime);
                    //ParamValues.Add("{加工数量}", this.txt_Qty2.Text);

                    #endregion

                    sFormulaCode = "AGV充电";

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
                }
                #endregion

                #region 4.2 加工中心先下料再上料
                job = BuildNewJobOrder(gyroscope.PKNO, 2, "陀螺生产", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{图片名称}", LaserPicName); //定制图片
                ParamValues.Add("{车床上下料参数}", sLathePieceNumOneTime);
                //ParamValues.Add("{加工数量}", this.txt_Qty2.Text); //生产设备

                #endregion

                sFormulaCode = "加工中心先下料再上料";

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

                #region 4.3.AGV充电 skip
                if (true)
                {
                    job = BuildNewJobOrder(gyroscope.PKNO, 2, "陀螺生产", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                    jobOrders.Add(job);

                    #region --设定参数--

                    ParamValues.Clear();
                    ParamValues.Add("{图片名称}", LaserPicName); //定制图片
                    ParamValues.Add("{车床上下料参数}", sLathePieceNumOneTime);
                    //ParamValues.Add("{加工数量}", this.txt_Qty2.Text); //生产设备

                    #endregion

                    sFormulaCode = "AGV充电";

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
                }
                #endregion

                #region 4.4 加工中心先下料再上料
                job = BuildNewJobOrder(gyroscope.PKNO, 2, "陀螺生产", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{图片名称}", LaserPicName); //定制图片
                ParamValues.Add("{车床上下料参数}", sLathePieceNumOneTime);
                //ParamValues.Add("{加工数量}", this.txt_Qty2.Text); //生产设备

                #endregion

                sFormulaCode = "加工中心先下料再上料";

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

                #region 4.3.AGV充电 skip
                if (true)
                {
                    job = BuildNewJobOrder(gyroscope.PKNO, 2, "陀螺生产", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                    jobOrders.Add(job);

                    #region --设定参数--

                    ParamValues.Clear();
                    ParamValues.Add("{图片名称}", LaserPicName); //定制图片
                    ParamValues.Add("{车床上下料参数}", sLathePieceNumOneTime);
                    //ParamValues.Add("{加工数量}", this.txt_Qty2.Text); //生产设备

                    #endregion

                    sFormulaCode = "AGV充电";

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

                }
                #endregion

                #region 4.4 加工中心先下料再上料
                job = BuildNewJobOrder(gyroscope.PKNO, 2, "陀螺生产", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{图片名称}", LaserPicName); //定制图片
                ParamValues.Add("{车床上下料参数}", sLathePieceNumOneTime);
                //ParamValues.Add("{加工数量}", this.txt_Qty2.Text); //生产设备

                #endregion

                sFormulaCode = "加工中心先下料再上料";

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

                #region 4.3.AGV充电 skip
                if (true)
                {
                    job = BuildNewJobOrder(gyroscope.PKNO, 2, "陀螺生产", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                    jobOrders.Add(job);

                    #region --设定参数--

                    ParamValues.Clear();
                    ParamValues.Add("{图片名称}", LaserPicName); //定制图片
                    ParamValues.Add("{车床上下料参数}", sLathePieceNumOneTime);
                    //ParamValues.Add("{加工数量}", this.txt_Qty2.Text); //生产设备

                    #endregion

                    sFormulaCode = "AGV充电";

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

                }
                #endregion

                #region 5.加工中心下料
                job = BuildNewJobOrder(gyroscope.PKNO, 2, "陀螺生产", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{图片名称}", LaserPicName); //定制图片
                ParamValues.Add("{车床上下料参数}", sLathePieceNumOneTime);
                //ParamValues.Add("{加工数量}", this.txt_Qty2.Text);

                #endregion

                sFormulaCode = "加工中心下料";

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

                #region 6.车床下料
                job = BuildNewJobOrder(gyroscope.PKNO, 2, "陀螺生产", jobOrderTime.AddSeconds(iJobOrderIndex++)); //--形成订单--
                jobOrders.Add(job);

                #region --设定参数--

                ParamValues.Clear();
                ParamValues.Add("{图片名称}", LaserPicName); //定制图片
                ParamValues.Add("{车床上下料参数}", sLathePieceNumOneTime);
                //ParamValues.Add("{加工数量}", this.txt_Qty2.Text);

                #endregion

                sFormulaCode = "车床下料";

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

                ShowTaskInfoEvent?.Invoke("FCS订单已下达", "指尖陀螺加工");
            }).Start();
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

    }
}
