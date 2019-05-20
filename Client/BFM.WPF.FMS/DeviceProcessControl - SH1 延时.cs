using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BFM.Common.Base;
using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.Common.Data.PubData;
using BFM.Common.DeviceAsset;
using BFM.Server.DataAsset.EAMService;
using BFM.Server.DataAsset.FMSService;
using BFM.Server.DataAsset.PLMService;
using BFM.Server.DataAsset.RSMService;
using BFM.Server.DataAsset.WMSService;
using BFM.ContractModel;
using BFM.Server.DataAsset.TMSService;
using BFM.WPF.SDM.TableNO;

namespace BFM.WPF.FMS
{
    public class DeviceProcessControl_SH1
    {
        private const int ReWriteCount = 3;  //向设备写数据 重写次数

        #region 静态变量

        private static List<DeviceProcessControl> ProcessControls = new List<DeviceProcessControl>();

        /// <summary>
        /// 运行生产线任务
        /// </summary>
        /// <param name="linepkno">生产线PKNO</param>
        /// <returns>0：正常开启；1：生产线不存在</returns>
        public static int RunByLine(string linepkno)
        {
            List<DeviceProcessControl> NewAddProcessControls = new List<DeviceProcessControl>();

            if (string.IsNullOrEmpty(linepkno))  //开启所有的
            {
                WcfClient<IRSMService> ws = new WcfClient<IRSMService>();
                List<RsLine> lines = ws.UseService(s => s.GetRsLines("USE_FLAG = 1"));

                if (!lines.Any())
                {
                    return 1;
                }

                foreach (var line in lines)
                {
                    DeviceProcessControl control = ProcessControls.FirstOrDefault(c => c.LinePKNO == line.PKNO);

                    if (control == null) //之前没有添加
                    {
                        control = new DeviceProcessControl(line.PKNO);
                        NewAddProcessControls.Add(control);
                    }

                    control.Run(); //开启
                }

            }
            else //开启具体的某一个
            {
                DeviceProcessControl control = ProcessControls.FirstOrDefault(c => c.LinePKNO == linepkno);

                if (control == null) //之前没有添加
                {
                    WcfClient<IRSMService> ws = new WcfClient<IRSMService>();
                    List<RsLine> lines = ws.UseService(s => s.GetRsLines($"PKNO = '{linepkno}' AND USE_FLAG = 1"));
                    if (!lines.Any())
                    {
                        return 1;
                    }

                    control = new DeviceProcessControl(linepkno);
                    control.Run();

                    NewAddProcessControls.Add(control);
                }

                control.Run(); //开启
            }

            if (NewAddProcessControls.Any())
            {
                ProcessControls.AddRange(NewAddProcessControls);
            }

            return 0;
        }

        /// <summary>
        /// 暂停生产线任务
        /// </summary>
        /// <param name="linepkno">生产线PKNO</param>
        /// <returns>0：正常暂停；1：生产线不存在</returns>
        public static int PauseByLine(string linepkno)
        {
            if (string.IsNullOrEmpty(linepkno)) //暂停所有的
            {
                if (!ProcessControls.Any())
                {
                    return 1;
                }

                foreach (var control in ProcessControls)
                {
                    control.Pause();
                }
            }
            else
            {
                DeviceProcessControl control = ProcessControls.FirstOrDefault(c => c.LinePKNO == linepkno);
                if (control == null) //之前没有添加
                {
                    return 1;
                }

                control.Pause(); //暂停
            }

            return 0;
        }

        /// <summary>
        /// 停止生产线任务
        /// </summary>
        /// <param name="linepkno">生产线PKNO</param>
        /// <returns>0：正常停止；1：生产线不存在</returns>
        public static int StopByLine(string linepkno)
        {
            List<string> DelControls = new List<string>();

            if (string.IsNullOrEmpty(linepkno)) //停止所有
            {
                if (!ProcessControls.Any())
                {
                    return 1;
                }

                foreach (var control in ProcessControls)
                {
                    control.Stop();  //停止
                    DelControls.Add(control.LinePKNO);
                }
            }
            else
            {
                DeviceProcessControl control = ProcessControls.FirstOrDefault(c => c.LinePKNO == linepkno);
                if (control == null)  //之前没有添加
                {
                    return 1;
                }
                else
                {
                    control.Stop();  //停止
                    DelControls.Add(control.LinePKNO);
                }
            }

            if (DelControls.Any())
            {
                ProcessControls.RemoveAll(c => DelControls.Contains(c.LinePKNO));
            }

            return 0;
        }

        #endregion

        /// <summary>
        /// 产线PKNO
        /// </summary>
        public string LinePKNO { get; set; }

        /// <summary>
        /// //运行状态 0：暂停；1：运行；2：Stop
        /// </summary>
        private int runState = 0;

        private DeviceProcessControl(string linepkno)
        {
            LinePKNO = linepkno;
            new Thread(() =>
            {
                TheadAutoProcessControl(linepkno);
            }).Start();
        }

        /// <summary>
        /// 运行
        /// </summary>
        private void Run()
        {
            runState = 1;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        private void Pause()
        {
            runState = 0;
        }

        /// <summary>
        /// 停止
        /// </summary>
        private void Stop()
        {
            runState = 2;

        }

        #region 加工流程控制

        private Int64 live = 0;

        private void TheadAutoProcessControl(string linePKNO)
        {
            WcfClient<IFMSService> wsFMS2 = new WcfClient<IFMSService>();
            List<FmsActionControl> ActionControls = wsFMS2.UseService(s => s.GetFmsActionControls(""));  //提高效率，基础数据先提取

            while (!CBaseData.AppClosing)
            {
                Thread.Sleep(50);  //暂停50ms

                #region 暂停、停止

                if ((CBaseData.AppClosing) || (runState == 10)) //停止
                {
                    return;
                }

                if (runState == 0)  //暂停
                {
                    continue;
                }

                #endregion

                //Console.WriteLine($"ProcessControl alived LinePKNO [{linePKNO}] [{live++}]  [{DateTime.Now.ToString()}]");

                try
                {
                    WcfClient<IPLMService> ws = new WcfClient<IPLMService>();
                    WcfClient<IWMSService> wsWMS = new WcfClient<IWMSService>();
                    WcfClient<IRSMService> wsRSM = new WcfClient<IRSMService>();
                    WcfClient<ITMSService> wsTMS = new WcfClient<ITMSService>();

                    List<string> ProcessStartOrEndUnConditions = new List<string>();  //过程开始/结束的不满足的条件，防止后面的Job先触发

                    #region 获取工单并执行

                    List<MesJobOrder> allJobs =
                        ws.UseService(s => s.GetMesJobOrders($"RUN_STATE >= 10 AND RUN_STATE < 100 AND LINE_PKNO = '{linePKNO}'"))  //开工确认完成的
                            .OrderBy(c => c.CREATION_DATE)
                            .ToList();

                    List<MesProductProcess> productProcesses = ws.UseService(s =>
                        s.GetMesProductProcesss("USE_FLAG = 1 AND PRODUCT_STATE >= 0 AND PRODUCT_STATE < 100"));  //正在执行的产品信息

                    //统计总的数量
                    int taskSum = Convert.ToInt32(allJobs.Sum(c => c.TASK_QTY)); //计划数量
                    int completeSum = Convert.ToInt32(allJobs.Sum(c => c.COMPLETE_QTY)); //完成数量
                    int onlineSum = Convert.ToInt32(allJobs.Sum(c => c.ONLINE_QTY));  //在线数量
                    
                    //===========每一次重新获取订单================//
                    int iConditionStartAddPause = 20;  //条件启动启动后附加的延时  正常20ms，按条件启动后500ms
                    bool bStart = false;  //已经启动了，只要启动了就重新获取订单信息

                    foreach (MesJobOrder job in allJobs)
                    {
                        if ((runState != 1) && (!CBaseData.AppClosing))  //正在运行才执行
                        {
                            break;
                        }

                        #region 整线报警

                        FmsAssetTagSetting alertTag = DeviceMonitor.GetTagSettings("TAG_CODE = '整线报警'").FirstOrDefault();

                        string alertValue = alertTag?.CUR_VALUE;

                        if (alertValue == "1")  //有整线报警，系统退出
                        {
                            break;
                        }

                        #endregion

                        job.PROCESS_INFO = "";

                        string itemPKNO = job.ITEM_PKNO; //产品PKNO

                        bool bFirstProcess = false;  //当前工序是否为第一道工序
                        bool bLastProcess = false;  //当前工序是否为最后一道工序，最后一道工序不能为不可用的工序

                        MesProcessCtrol curProcess = null;  //当前工序
                        MesProcessCtrol firstProcess = null;  //第一道工序
                        
                        #region 获取当前工序控制信息

                        List<MesProcessCtrol> allProcessCtrols =
                            ws.UseService(s => s.GetMesProcessCtrols($"JOB_ORDER_PKNO = '{job.PKNO}'"))
                                .OrderBy(c => c.PROCESS_INDEX)
                                .ThenBy(c => c.CREATION_DATE)
                                .ToList();    //获取当前所有工序
                        
                        List<MesProcessCtrol> unFinishProcessCtrols = allProcessCtrols.Where(c => c.PROCESS_STATE < 10)
                                    .OrderBy(c => c.PROCESS_INDEX)
                                    .ThenBy(c => c.CREATION_DATE)
                                    .ToList();    //获取当前未执行的可用工序控制

                        List<MesProcessCtrol> usefulProcessCtrols =
                            unFinishProcessCtrols.Where(c => c.USE_FLAG == 1).OrderBy(c => c.PROCESS_INDEX)
                                .ThenBy(c => c.CREATION_DATE).ToList();  //未完成的可用工序控制

                        if (!usefulProcessCtrols.Any())
                        {
                            string sErrorInfo = $"没有未执行的可用流程信息";
                            if (job.PROCESS_INFO != sErrorInfo)
                            {
                                job.PROCESS_INFO = sErrorInfo; //生产执行信息
                                ws.UseService(s => s.UpdateMesJobOrder(job)); //更新生产线任务
                            }
                            continue;
                        }
                        firstProcess = allProcessCtrols.OrderBy(c => c.PROCESS_INDEX).ThenBy(c => c.CREATION_DATE).FirstOrDefault(c => c.USE_FLAG == 1);  //第一道工序可用工序
                        curProcess = usefulProcessCtrols.OrderBy(c => c.PROCESS_INDEX).ThenBy(c => c.CREATION_DATE).FirstOrDefault(c => c.PROCESS_STATE >= 0);  //当前执行的工序

                        #region 检验当前工序状态

                        if (curProcess == null)
                        {
                            string sErrorInfo = $"没有可用的未执行工序控制信息";
                            if (job.PROCESS_INFO != sErrorInfo)
                            {
                                job.PROCESS_INFO = sErrorInfo; //生产执行信息
                                ws.UseService(s => s.UpdateMesJobOrder(job)); //更新生产线任务
                            }
                            continue;
                        }

                        if (curProcess.PROCESS_STATE == 0)
                        {
                            string sErrorInfo = $"当前工序尚未准备好，请准备好该工序.";
                            if (job.PROCESS_INFO != sErrorInfo)
                            {
                                job.PROCESS_INFO = sErrorInfo; //生产执行信息
                                ws.UseService(s => s.UpdateMesJobOrder(job)); //更新生产线任务
                            }
                            continue;
                        }

                        #endregion

                        bFirstProcess = (curProcess.PROCESS_INDEX == firstProcess.PROCESS_INDEX);  //是否为第一道工序
                        bLastProcess = (curProcess.PROCESS_INDEX == usefulProcessCtrols.LastOrDefault().PROCESS_INDEX); //是否为最后一个工序

                        #endregion

                        string ctrolName = curProcess.PROCESS_CTROL_NAME ?? curProcess.PROCESS_INDEX.ToString();  //过程控制名称

                        FmsActionControl curAction = ActionControls.FirstOrDefault(c => c.PKNO == curProcess.PROCESS_ACTION_PKNO);  //当前动作
                        if (curAction == null)  //空动作的工序，手动完成
                        {
                            string sErrorInfo = $"当前生产过程[{ctrolName}]没有动作控制指令，须手动完成.";
                            if (job.PROCESS_INFO != sErrorInfo)
                            {
                                job.PROCESS_INFO = sErrorInfo; //生产执行信息
                                ws.UseService(s => s.UpdateMesJobOrder(job)); //更新生产线任务
                            }

                            Console.WriteLine(sErrorInfo);
                            continue;
                        }

                        RsItemMaster curProduct = wsRSM.UseService(s => s.GetRsItemMasterById(itemPKNO));  //当前产品信息

                        MesProductProcess productProcess = productProcesses.FirstOrDefault(c => c.PKNO == curProcess.CUR_PRODUCT_CODE_PKNO);  //产品生产情况;

                        //开始
                        if (curProcess.PROCESS_STATE == 1) //准备完成，未开始执行
                        {
                            #region 开始执行工序 共 5 步

                            FmsAssetTagSetting startConditionTag = null;
                            string startCustomParam = ""; //开始执行的参数，增加到同步写入数据后面
                            /************ ！！！工序开始的条件！！！  ************/

                            #region 1. 检验当前执行的工序是否可以执行，启动条件

                            startConditionTag = DeviceMonitor.GetTagSettingById(curAction.START_CONDITION_TAG_PKNO); //获取Tag值

                            if (startConditionTag != null) //启动条件为空则直接执行
                            {
                                string condition = startConditionTag.PKNO + ".VALUE = " + curAction.START_CONDITION_VALUE;

                                if (ProcessStartOrEndUnConditions.Contains(condition))  //如果前面的Job存在需要判断该状态，则不执行当前的
                                {
                                    string sErrorInfo = $"等待执行";
                                    if (job.PROCESS_INFO != sErrorInfo)
                                    {
                                        job.PROCESS_INFO = sErrorInfo; //生产执行信息
                                        ws.UseService(s => s.UpdateMesJobOrder(job)); //更新生产线任务
                                    }

                                    continue;
                                }

                                string checkValue = startConditionTag.CUR_VALUE?.Split('|')[0]; //当前执行条件的结果
                                string startValue = curAction.START_CONDITION_VALUE;  //开始条件
                                string[] startValues = startValue.Split('|');  //多个开启条件

                                if ((string.IsNullOrEmpty(checkValue)) || (!startValues.Contains(checkValue))) //当前值不是工序开始检测值
                                {
                                    #region 流程分支情况

                                    if (curProcess.PROCESS_ACTION_TYPE == 4) //流程分支，不启用，直接往下运行
                                    {
                                        #region 直接完成当前工序

                                        #region 第一道工序处理

                                        if (bFirstProcess) //第一道工序
                                        {
                                            job.RUN_STATE = 20; //正在执行
                                            job.ACT_START_TIME = DateTime.Now; //修改任务开始时间
                                            job.PROCESS_INFO = "正常"; //生产执行信息
                                            ws.UseService(s => s.UpdateMesJobOrder(job)); //更新生产线任务
                                        }

                                        #endregion

                                        curProcess.PROCESS_STATE = 10; //直接完成
                                        curProcess.PROCESS_END_TYPE = 1;
                                        curProcess.PROCESS_END_TIME = DateTime.Now;
                                        curProcess.REMARK = "条件不满足，不启用流程分支，直接执行现有流程。";
                                        ws.UseService(s => s.UpdateMesProcessCtrol(curProcess)); //更新工序控制

                                        break; //直接执行重新执行

                                        #endregion
                                    }

                                    #endregion

                                    #region 判断条件不满足

                                    string sErrorInfo =
                                        $"生产过程【{ctrolName}】开启条件【{startConditionTag?.TAG_NAME}】不足，当前状态为【{checkValue}】,需要状态【{startValue}】";
                                    if (job.PROCESS_INFO != sErrorInfo)
                                    {
                                        job.PROCESS_INFO = sErrorInfo; //生产执行信息
                                        ws.UseService(s => s.UpdateMesJobOrder(job)); //更新生产线任务
                                    }

                                    //Console.WriteLine(sErrorInfo);

                                    ProcessStartOrEndUnConditions.Add(condition); //不能执行则

                                    continue;

                                    #endregion
                                }

                                //ProcessStartOrEndUnConditions.Remove(condition);  //可以执行则移除不满足的条件  //移动到完成中
                            }

                            #endregion

                            /************ ！！！工序开始的条件满足，直接执行！！！  ************/
                            Console.WriteLine($"========生产过程【{ctrolName}】开始条件【{startConditionTag?.TAG_NAME}】满足，开始执行=======");

                            #region 2. 获取产品，第一道工序形成产品

                            if (bFirstProcess) //bTaskBeginProcess
                            {
                                string productPKNO = CBaseData.NewGuid();

                                #region 形成产品

                                productProcess = new MesProductProcess() //生成新的产品
                                {
                                    PKNO = productPKNO,
                                    COMPANY_CODE = CBaseData.BelongCompPKNO,
                                    ITEM_PKNO = curProcess.ITEM_PKNO,
                                    JOB_ORDER_PKNO = curProcess.JOB_ORDER_PKNO,
                                    JOB_ORDER = curProcess.JOB_ORDER,
                                    SUB_JOB_ORDER_NO = curProcess.SUB_JOB_ORDER_NO,

                                    PRODUCT_CODE = TableNOHelper.GetNewNO("MesProductProcess.PRODUCT_CODE", "P"),
                                    PRODUCT_POSITION = "", //当前位置
                                    CUR_ROCESS_CTROL_PKNO = curProcess.PKNO, //当前过程
                                    RAW_NUMBER = curProcess.PROCESS_QTY, //原料数量
                                    PRODUCT_NUMBER = curProcess.COMPLETE_QTY, //完成数量
                                    QUALIFIED_NUMBER = curProcess.QUALIFIED_QTY, //合格品数量
                                    PALLET_NO = curProcess.PALLET_NO, //托盘号
                                    PRODUCT_STATE = -1, //尚未开始

                                    CREATION_DATE = DateTime.Now,
                                    CREATED_BY = CBaseData.LoginNO,
                                    LAST_UPDATE_DATE = DateTime.Now,  //最后修改日期
                                    USE_FLAG = 1,
                                    REMARK = "", //备注
                                };

                                ws.UseService(s => s.AddMesProductProcess(productProcess));

                                #endregion

                                curProcess.CUR_PRODUCT_CODE_PKNO = productPKNO; //新产品编号

                                productProcess = ws.UseService(s => s.GetMesProductProcessById(productPKNO)); //产品生产情况
                            }
                            else
                            {
                                curProcess.CUR_PRODUCT_CODE_PKNO = firstProcess.CUR_PRODUCT_CODE_PKNO; //
                            }

                            #endregion

                            #region 3. 根据工序类型处理 更新加工工件状态，位置等

                            if (curProcess.PROCESS_ACTION_TYPE == 1) //开始加工
                            {
                                #region 加工动作处理

                                #endregion
                            }
                            else if (curProcess.PROCESS_ACTION_TYPE == 2) //开始搬运
                            {
                                #region 搬运动作处理

                                //1.检验目标是否为空
                                var checkProcess1 = productProcesses.Where(c =>
                                    c.PRODUCT_POSITION == curProcess.FINISH_POSITION); //目的位置
                                if (checkProcess1.Any())
                                {
                                    //目标位置不为空
                                    string sErrorInfo = $"搬运时，目标位置不为空.";
                                    if (job.PROCESS_INFO != sErrorInfo)
                                    {
                                        job.PROCESS_INFO = sErrorInfo; //生产执行信息
                                        ws.UseService(s => s.UpdateMesJobOrder(job)); //更新生产线任务
                                    }
                                    Console.WriteLine(sErrorInfo);
                                    //continue;
                                }

                                if (productProcess != null)
                                {
                                    //2.检验当前搬运器是否为空
                                    var checkProcess2 = productProcesses.FirstOrDefault(c =>
                                        c.PRODUCT_POSITION == curProcess.PROCESS_DEVICE_PKNO); //搬运器

                                    if (checkProcess2 != null && checkProcess2.PKNO != productProcess.PKNO)
                                    {
                                        //搬运器不为空
                                        string sErrorInfo = $"搬运时，搬运器不为空.";
                                        if (job.PROCESS_INFO != sErrorInfo)
                                        {
                                            job.PROCESS_INFO = sErrorInfo; //生产执行信息
                                            ws.UseService(s => s.UpdateMesJobOrder(job)); //更新生产线任务
                                        }
                                        Console.WriteLine(sErrorInfo);
                                        //continue;
                                    }

                                    //3.更新产品的位置
                                    productProcess.PRODUCT_POSITION = curProcess.PROCESS_DEVICE_PKNO; //将产品放置在搬运器上
                                }

                                #endregion
                            }
                            else if (curProcess.PROCESS_ACTION_TYPE == 3) //开始检测
                            {
                                #region 检测动作处理

                                #endregion
                            }
                            else if (curProcess.PROCESS_ACTION_TYPE == 4) //流程分支
                            {
                                string disableProcesses = curProcess.PROCESS_ACTION_PARAM1_VALUE; //禁用的流程
                                string enableProcesses = curProcess.PROCESS_ACTION_PARAM2_VALUE; //启用的流程

                                string stemp = $"触发流程分支.禁用流程[{disableProcesses}], 启用流程[{enableProcesses}]";
                                job.PROCESS_INFO = stemp; //生产执行信息
                                Console.WriteLine(stemp);

                                #region 启动流程分支

                                if (!string.IsNullOrEmpty(disableProcesses))
                                {
                                    foreach (var disableP in disableProcesses.Split(';'))
                                    {
                                        MesProcessCtrol process = unFinishProcessCtrols.FirstOrDefault(c =>
                                            c.PROCESS_INDEX == SafeConverter.SafeToInt(disableP, -1));
                                        process.USE_FLAG = 0; //禁用
                                        ws.UseService(s => s.UpdateMesProcessCtrol(process)); //更新工序控制
                                    }
                                }

                                if (!string.IsNullOrEmpty(enableProcesses))
                                {
                                    foreach (var enableP in enableProcesses.Split(';'))
                                    {
                                        MesProcessCtrol process = unFinishProcessCtrols.FirstOrDefault(c =>
                                            c.PROCESS_INDEX == SafeConverter.SafeToInt(enableP, -1));
                                        process.USE_FLAG = 1; //启用
                                        ws.UseService(s => s.UpdateMesProcessCtrol(process)); //更新工序控制

                                    }
                                }

                                #endregion

                                #region 更新数据

                                #region 加工控制

                                curProcess.PROCESS_STATE = 10; //直接完成
                                curProcess.PROCESS_END_TYPE = 1;
                                curProcess.PROCESS_START_TIME = DateTime.Now.AddMilliseconds(-100);
                                curProcess.PROCESS_END_TIME = DateTime.Now;
                                curProcess.REMARK = $"流程分支条件满足，启用流程分支，禁用流程[{disableProcesses}], 启用流程[{enableProcesses}]";
                                ws.UseService(s => s.UpdateMesProcessCtrol(curProcess)); //更新工序控制

                                #endregion

                                #region Job处理

                                if (bFirstProcess) //第一道工序
                                {
                                    job.RUN_STATE = 20; //正在执行
                                    job.ACT_START_TIME = DateTime.Now; //修改任务开始时间
                                }

                                ws.UseService(s => s.UpdateMesJobOrder(job)); //更新生产线任务

                                #endregion

                                DeviceMonitor.SetTagSettingValue(startConditionTag, ""); //将检测结果置为空

                                //产品处理
                                if (productProcess != null)
                                {
                                    productProcess.PRODUCT_STATE = curProcess.PROCESS_ACTION_TYPE; //当前状态
                                    productProcess.LAST_UPDATE_DATE = DateTime.Now;
                                    var process = productProcess;
                                    ws.UseService(s => s.UpdateMesProductProcess(process));
                                }

                                #endregion

                                break;  //直接开启下一次流程
                            }
                            else if (curProcess.PROCESS_ACTION_TYPE == 11) //开始 原料出库
                            {
                                job.ONLINE_QTY = curProcess.PROCESS_QTY; //在线数量

                                #region 开始出库

                                if (!string.IsNullOrEmpty(curProcess.BEGIN_POSITION))
                                {
                                    WmsAllocationInfo rawAllo = wsWMS.UseService(s =>
                                        s.GetWmsAllocationInfoById(curProcess.BEGIN_POSITION));

                                    if (rawAllo != null)
                                    {
                                        if (productProcess != null)
                                            productProcess.PALLET_NO = rawAllo.CUR_PALLET_NO; //托盘号

                                        rawAllo.ALLOCATION_STATE = 2000 + (rawAllo.ALLOCATION_STATE % 1000); //正在出库

                                        wsWMS.UseService(s => s.UpdateWmsAllocationInfo(rawAllo));
                                    }
                                }

                                #endregion

                            }
                            else if (curProcess.PROCESS_ACTION_TYPE == 12) //开始 产品入库
                            {
                                #region 产品入库处理

                                if (!string.IsNullOrEmpty(curProcess.FINISH_POSITION))
                                {
                                    WmsAllocationInfo prodAllo = wsWMS.UseService(s =>
                                        s.GetWmsAllocationInfoById(curProcess.FINISH_POSITION));

                                    if (prodAllo != null)
                                    {
                                        if ((productProcess != null) &&
                                            (!string.IsNullOrEmpty(productProcess.PALLET_NO)))
                                            prodAllo.CUR_PALLET_NO = productProcess.PALLET_NO; //托盘号

                                        prodAllo.ALLOCATION_STATE = 1000 + +(prodAllo.ALLOCATION_STATE % 1000); //正在入库

                                        wsWMS.UseService(s => s.UpdateWmsAllocationInfo(prodAllo));
                                    }
                                }

                                #endregion

                                #region 获取执行的参数

                                #endregion
                            }
                            else if ((curProcess.PROCESS_ACTION_TYPE >= 40) &&
                                     (curProcess.PROCESS_ACTION_TYPE < 50)) //换刀
                            {
                                #region 换刀

                                #endregion
                            }
                            else //普通动作
                            {
                                //ignore
                            }

                            #endregion

                            #region 4. 执行当前工序 => 向设备写入数据 写 3 个值

                            int ret = 0;
                            string error = "";
                            string tagPKNO = "";
                            string sTagValue = "";

                            bool bWriteSuccess = true;  //写入设备成功标志

                            #region  4.1 写参数1

                            tagPKNO = curAction.EXECUTE_PARAM1_TAG_PKNO;
                            sTagValue = ConvertDynamicValue(curProcess.PROCESS_ACTION_PARAM1_VALUE);

                            if ((!string.IsNullOrEmpty(tagPKNO)) && (!string.IsNullOrEmpty(sTagValue)))
                            {
                                #region 多次重写

                                int iWrite = 0;
                                while (iWrite < ReWriteCount)
                                {
                                    ret = DeviceMonitor.WriteTagToDevice(tagPKNO, sTagValue, out error);
                                    if (ret == 0)
                                    {
                                        Thread.Sleep(100);  //写入成功后暂停
                                        break;
                                    }
                                    iWrite++;
                                    Thread.Sleep(100);
                                }

                                #endregion

                                if (ret == 10) //写入设备失败
                                {
                                    string sErrorInfo = $"向设备写入参数1失败。错误为：{error}"; //生产执行信息
                                    if (job.PROCESS_INFO != sErrorInfo)
                                    {
                                        job.PROCESS_INFO = sErrorInfo; //生产执行信息
                                        ws.UseService(s => s.UpdateMesJobOrder(job)); //更新生产线任务
                                    }

                                    Console.WriteLine(sErrorInfo);
                                    break;
                                }

                                if (ret == 0) bWriteSuccess = true; //写入成功
                            }

                            #endregion

                            #region  4.2 写参数2

                            tagPKNO = curAction.EXECUTE_PARAM2_TAG_PKNO;
                            sTagValue = ConvertDynamicValue(curProcess.PROCESS_ACTION_PARAM2_VALUE);

                            if ((!string.IsNullOrEmpty(tagPKNO)) && (!string.IsNullOrEmpty(sTagValue)))
                            {
                                #region 多次重写

                                int iWrite = 0;
                                while (iWrite < ReWriteCount)
                                {
                                    ret = DeviceMonitor.WriteTagToDevice(tagPKNO, sTagValue, out error);
                                    if (ret == 0)
                                    {
                                        Thread.Sleep(100);  //写入成功后暂停
                                        break;
                                    }
                                    iWrite++;
                                    Thread.Sleep(100);
                                }

                                #endregion

                                if (ret == 10) //写入设备失败
                                {
                                    string sErrorInfo = $"向设备写入参数2失败。错误为：{error}"; //生产执行信息
                                    if (job.PROCESS_INFO != sErrorInfo)
                                    {
                                        job.PROCESS_INFO = sErrorInfo; //生产执行信息
                                        ws.UseService(s => s.UpdateMesJobOrder(job)); //更新生产线任务
                                    }

                                    Console.WriteLine(sErrorInfo);
                                    break;
                                }

                                if (ret == 0) bWriteSuccess = true; //写入成功
                            }

                            #endregion

                            #region  4.3 写执行

                            tagPKNO = curAction.EXECUTE_TAG_PKNO;
                            sTagValue = ConvertDynamicValue(curAction.EXECUTE_WRITE_VALUE) + 
                                        "|" + (string.IsNullOrEmpty(curProcess.PROCESS_ACTION_PARAM1_VALUE) ? "0" : curProcess.PROCESS_ACTION_PARAM1_VALUE) + 
                                        "|" + (string.IsNullOrEmpty(curProcess.PROCESS_ACTION_PARAM2_VALUE) ? "0" : curProcess.PROCESS_ACTION_PARAM2_VALUE) +
                                        "|" + (string.IsNullOrEmpty(startCustomParam) ? "0" : startCustomParam);

                            if ((!string.IsNullOrEmpty(tagPKNO)) && (!string.IsNullOrEmpty(sTagValue.Replace("|", ""))))
                            {
                                #region 多次重写

                                int iWrite = 0;
                                while (iWrite < ReWriteCount)
                                {
                                    ret = DeviceMonitor.WriteTagToDevice(tagPKNO, sTagValue, out error);
                                    if (ret == 0)
                                    {
                                        //Thread.Sleep(100);  //写入成功后暂停，最后不需要
                                        break;
                                    }
                                    iWrite++;
                                    Thread.Sleep(100);
                                }

                                #endregion

                                if (ret == 10) //写入设备失败
                                {
                                    string sErrorInfo = $"向设备写入开始动作值失败。错误为：{error}"; //生产执行信息
                                    if (job.PROCESS_INFO != sErrorInfo)
                                    {
                                        job.PROCESS_INFO = sErrorInfo; //生产执行信息
                                        ws.UseService(s => s.UpdateMesJobOrder(job)); //更新生产线任务
                                    }

                                    Console.WriteLine(sErrorInfo);
                                    break;
                                }

                                if (ret == 0) bWriteSuccess = true; //写入成功
                            }

                            #endregion

                            if (bWriteSuccess)
                            {
                                Console.WriteLine("给设备发送指令成功.开始进行执行" +
                                                  (string.IsNullOrEmpty(curProcess.SUB_JOB_ORDER_NO)
                                                      ? curProcess.JOB_ORDER_PKNO
                                                      : curProcess.SUB_JOB_ORDER_NO)); //给设备发送动作指令成功
                            }

                            #endregion

                            #region 5. 更新数据 共更新 5 个类

                            #region 5.1 加工控制

                            curProcess.PROCESS_STATE = 2; //正在执行
                            curProcess.PROCESS_START_TYPE = 1; //自动开始
                            curProcess.PROCESS_START_TIME = DateTime.Now;
                            ws.UseService(s => s.UpdateMesProcessCtrol(curProcess)); //更新工序控制

                            #endregion
                            
                            #region 5.2 Job处理

                            if (bFirstProcess) //第一道工序
                            {
                                job.RUN_STATE = 20; //正在执行
                                job.ACT_START_TIME = DateTime.Now; //修改任务开始时间
                            }

                            if (string.IsNullOrEmpty(job.PROCESS_INFO)) job.PROCESS_INFO = "开始执行"; //生产执行信息
                            ws.UseService(s => s.UpdateMesJobOrder(job)); //更新生产线任务

                            #endregion

                            //5.3 产品处理
                            if (productProcess != null)
                            {
                                productProcess.PRODUCT_STATE = curProcess.PROCESS_ACTION_TYPE; //当前状态
                                productProcess.LAST_UPDATE_DATE = DateTime.Now;
                                var process = productProcess;
                                ws.UseService(s => s.UpdateMesProductProcess(process));
                            }

                            //5.4 开启条件结果置空
                            if (startConditionTag != null)
                            {
                                DeviceMonitor.SetTagSettingValue(startConditionTag, "");//将开启条件结果置空
                            } 

                            #region 5.5 完成反馈状态，将完成条件置空

                            FmsAssetTagSetting finishTag = DeviceMonitor.GetTagSettingById(curAction.FINISH_CONDITION_TAG_PKNO);

                            if (finishTag != null)
                            {
                                if (finishTag.SAMPLING_MODE == 11) finishTag.SAMPLING_MODE = 10; //按照条件开启

                                DeviceMonitor.SetTagSettingValue(finishTag, "");
                            }

                            #endregion

                            #endregion

                            #endregion

                            Thread.Sleep(50);

                            if (startConditionTag != null)
                            {
                                iConditionStartAddPause = 800;  //按条件启动后增加的延时
                            }

                            bStart = true;  //本次订单已经启动过了
                        }
                        
                        Thread.Sleep(iConditionStartAddPause);  //条件启动 附加延时

                        //完成
                        if (curProcess.PROCESS_STATE == 2)   //正在执行，完成该动作
                        {
                            #region 完成执行工序 共 3 步
                            
                            /************ ！！！工序完成的条件！！！  ************/
                            string sCurFinishValue = "";  //当前完成的结果

                            #region 1. 检验当前工序是否完成，未完成，则下一个任务

                            FmsAssetTagSetting finishTag = DeviceMonitor.GetTagSettingById(curAction.FINISH_CONDITION_TAG_PKNO);
                            if (finishTag != null)
                            {
                                string condition = finishTag.PKNO + ".VALUE = " + curAction.FINISH_CONDITION_VALUE;

                                if (ProcessStartOrEndUnConditions.Contains(condition)) //如果前面的Job存在需要判断该状态，则不执行当前的
                                {
                                    string sErrorInfo =
                                        $"正在执行[{ctrolName}],完成条件[{finishTag?.TAG_NAME}]不足.";
                                    if (job.PROCESS_INFO != sErrorInfo)
                                    {
                                        job.PROCESS_INFO = sErrorInfo; //生产执行信息
                                        ws.UseService(s => s.UpdateMesJobOrder(job)); //更新生产线任务
                                    }

                                    #region 本次订单已经则重新刷新订单
                                    
                                    Thread.Sleep(iConditionStartAddPause);  //条件启动 附加延时

                                    if (bStart)
                                    {
                                        break;  //重新开始任务
                                    }

                                    #endregion

                                    continue;
                                }
                                
                                if (finishTag.SAMPLING_MODE == 11)  //动作开始后自动开启 => 尚未开启，需要开启
                                {
                                    finishTag.SAMPLING_MODE = 10;  //尚未开启，需要开启
                                    DeviceMonitor.SetTagSettingValue(finishTag, "");

                                    #region 本次订单已经则重新刷新订单

                                    Thread.Sleep(iConditionStartAddPause);  //条件启动 附加延时

                                    if (bStart)
                                    {
                                        break;  //重新开始任务
                                    }

                                    #endregion

                                    continue;
                                }
                                sCurFinishValue = finishTag.CUR_VALUE;  //当前设备的值

                                if (string.IsNullOrEmpty(sCurFinishValue))
                                {
                                    string sErrorInfo = $"正在执行[{ctrolName}]";
                                    if (job.PROCESS_INFO != sErrorInfo)
                                    {
                                        job.PROCESS_INFO = sErrorInfo; //生产执行信息
                                        ws.UseService(s => s.UpdateMesJobOrder(job)); //更新生产线任务
                                    }

                                    #region 本次订单已经 则重新刷新订单

                                    Thread.Sleep(iConditionStartAddPause);  //条件启动 附加延时

                                    if (bStart)
                                    {
                                        break;  //重新开始任务
                                    }

                                    #endregion

                                    continue;
                                }

                                string checkValue = sCurFinishValue.Split('|')[0];  //多结果情况，适用于检测
                                string finishValue = curAction.FINISH_CONDITION_VALUE;

                                #region 替换变量值

                                if (finishValue == "{加工数量}")
                                {
                                    finishValue = job.TASK_QTY.ToString();
                                }
                                else if (finishValue == "{当前产品}")
                                {
                                    finishValue = ((curProduct == null) ? "1" : curProduct.ITEM_ABV.ToString());
                                }

                                #endregion

                                string[] finishValues = finishValue.Split('|');  //多个完成条件

                                if ((string.IsNullOrEmpty(checkValue)) || (!finishValues.Contains(checkValue))) //当前值不是工序结束值，不完成
                                {
                                    string sErrorInfo =
                                        $"正在执行[{ctrolName}],获取到完成结果为[{checkValue}]，判断完成条件为[{finishValue}]，生产过程[{ctrolName}]尚未完成.";
                                    if (job.PROCESS_INFO != sErrorInfo)
                                    {
                                        job.PROCESS_INFO = sErrorInfo; //生产执行信息
                                        ws.UseService(s => s.UpdateMesJobOrder(job)); //更新生产线任务
                                    }

                                    ProcessStartOrEndUnConditions.Add(condition); //完成条件不满足

                                    #region 本次订单已经则重新刷新订单

                                    Thread.Sleep(iConditionStartAddPause);  //条件启动 附加延时

                                    if (bStart)
                                    {
                                        break;  //重新开始任务
                                    }

                                    #endregion

                                    continue;
                                }

                                ProcessStartOrEndUnConditions.Remove(condition);  //完成条件满足 则移除不满足的条件

                                #region 移除开始条件

                                FmsAssetTagSetting startTag = DeviceMonitor.GetTagSettingById(curAction.START_CONDITION_TAG_PKNO); //获取Tag值
                                if (startTag != null)
                                {
                                    string strartCondition = startTag.PKNO + ".VALUE = " + curAction.START_CONDITION_VALUE;
                                    ProcessStartOrEndUnConditions.Remove(strartCondition);
                                }

                                #endregion 
                            }
                            else  //没有动作完成的检测值，手动完成。
                            {
                                string sErrorInfo = $"生产过程[{ctrolName}]没有设置过程完成的检测值，请手动完成该过程."; //生产执行信息
                                if (job.PROCESS_INFO != sErrorInfo)
                                {
                                    job.PROCESS_INFO = sErrorInfo; //生产执行信息
                                    ws.UseService(s => s.UpdateMesJobOrder(job)); //更新生产线任务
                                }
                                
                                #region 移除开始条件

                                FmsAssetTagSetting startTag = DeviceMonitor.GetTagSettingById(curAction.START_CONDITION_TAG_PKNO); //获取Tag值
                                if (startTag != null)
                                {
                                    string strartCondition = startTag.PKNO + ".VALUE = " + curAction.START_CONDITION_VALUE;
                                    ProcessStartOrEndUnConditions.Remove(strartCondition);
                                }

                                #endregion 

                                #region 本次订单已经则重新刷新订单

                                Thread.Sleep(iConditionStartAddPause);  //条件启动 附加延时

                                if (bStart)
                                {
                                    break;  //重新开始任务
                                }

                                #endregion

                                continue;
                            }

                            #endregion

                            /************ ！！！工序已经完成！！！  ************/

                            Console.WriteLine($"收到设备完成指令，完成反馈结果为[{sCurFinishValue}]，完成生产过程[{ctrolName}]");

                            #region 2. 根据工序类型处理 完成当前工序

                            if (curProcess.PROCESS_ACTION_TYPE == 1) //加工完成
                            {
                                if (productProcess != null)
                                {
                                    productProcess.CUR_ITEM_PKNO = curProcess.FINISH_ITEM_PKNO; //生产完成
                                }
                            }
                            else if (curProcess.PROCESS_ACTION_TYPE == 2) //搬运完成
                            {
                                if (productProcess != null)
                                {
                                    productProcess.PRODUCT_POSITION = curProcess.FINISH_POSITION; //目标位置
                                }
                            }
                            else if (curProcess.PROCESS_ACTION_TYPE == 3) //检测完成
                            {
                                //合格数量
                                string[] result = sCurFinishValue.Split('|');
                                int qualifiedNumber = 1;  //合格品数量
                                if (result.Count() >= 2)
                                {
                                    int.TryParse(result[1], out qualifiedNumber);
                                }
                                curProcess.COMPLETE_QTY = qualifiedNumber;  //生产数量 - 含不合格的丢弃
                                curProcess.QUALIFIED_QTY = qualifiedNumber;  //合格品数量

                                job.ONLINE_QTY = job.ONLINE_QTY - firstProcess.PROCESS_QTY + curProcess.QUALIFIED_QTY; //在线数量，更新再制品数量
                                if (job.ONLINE_QTY < 0) job.ONLINE_QTY = 0;
                            }
                            else if (curProcess.PROCESS_ACTION_TYPE == 11) //出库完成
                            {
                                if (productProcess != null) productProcess.PRODUCT_POSITION = curProcess.FINISH_POSITION; //目标位置

                                #region 解锁货位地址 - 货位清空

                                if (!string.IsNullOrEmpty(curProcess.BEGIN_POSITION))
                                {
                                    WmsAllocationInfo rawAllo = wsWMS.UseService(s =>
                                        s.GetWmsAllocationInfoById(curProcess.BEGIN_POSITION));

                                    if (rawAllo != null)
                                    {
                                        rawAllo.CUR_PALLET_NO = "";
                                        rawAllo.ALLOCATION_STATE = 0;  //空

                                        wsWMS.UseService(s => s.UpdateWmsAllocationInfo(rawAllo));

                                        WmsInventory inv = wsWMS.UseService(s => s.GetWmsInventorys($"ALLOCATION_PKNO = '{rawAllo.PKNO}'"))
                                            .FirstOrDefault();

                                        if (inv != null) wsWMS.UseService(s => s.DelWmsInventory(inv.PKNO));
                                    }
                                }

                                #endregion
                            }
                            else if (curProcess.PROCESS_ACTION_TYPE == 12) //入库完成
                            {
                                if (productProcess != null) productProcess.PRODUCT_POSITION = curProcess.FINISH_POSITION; //目标位置

                                #region 产品入库处理 - 解锁货位地址、增加产品库存

                                if (!string.IsNullOrEmpty(curProcess.FINISH_POSITION))
                                {
                                    WmsAllocationInfo prodAllo = wsWMS.UseService(s =>
                                        s.GetWmsAllocationInfoById(curProcess.FINISH_POSITION));

                                    if (prodAllo != null)  //入库货位
                                    {
                                        prodAllo.ALLOCATION_STATE = 100; //满货位

                                        WmsInventory inv = new WmsInventory()
                                        {
                                            PKNO = CBaseData.NewGuid(),
                                            COMPANY_CODE = "",
                                            MATERIAL_PKNO = curProcess.FINISH_ITEM_PKNO,
                                            ALLOCATION_PKNO = prodAllo.PKNO,
                                            AREA_PKNO = prodAllo.AREA_PKNO,
                                            BATCH_NO = curProcess.SUB_JOB_ORDER_NO,
                                            INVENTORY_NUM = 1, //curProcess.QUALIFIED_QTY?? 1,
                                            REMARK = "",
                                        };  //库存

                                        wsWMS.UseService(s => s.UpdateWmsAllocationInfo(prodAllo));  //修改货位

                                        wsWMS.UseService(s => s.AddWmsInventory(inv));
                                    }
                                }

                                #endregion
                            }
                            else if ((curProcess.PROCESS_ACTION_TYPE >= 40) &&
                                     (curProcess.PROCESS_ACTION_TYPE < 50)) //换刀
                            {
                                //40：换刀；41：取刀；42：卸刀；43：装刀；44：放刀
                                if (curProcess.PROCESS_ACTION_TYPE == 41)  //取刀
                                {
                                    TmsToolsMaster mToolsMaster = wsTMS.UseService(s =>
                                        s.GetTmsToolsMasterById(curProcess.BEGIN_ITEM_PKNO));   //装上刀具
                                    if (mToolsMaster != null)
                                    {
                                        mToolsMaster.TOOLS_POSITION = 10;  //出库
                                        mToolsMaster.TOOLS_POSITION_PKNO = "";
                                        wsTMS.UseService(s => s.UpdateTmsToolsMaster(mToolsMaster));  
                                    }
                                }
                                else if (curProcess.PROCESS_ACTION_TYPE == 42)  //卸刀
                                {
                                    TmsDeviceToolsPos mTmsDeviceToolsPos = wsTMS.UseService(s => s.GetTmsDeviceToolsPosById(curProcess.BEGIN_ITEM_PKNO)); //卸下刀具
                                    if (mTmsDeviceToolsPos != null)
                                    {
                                        mTmsDeviceToolsPos.TOOLS_PKNO = "";
                                        wsTMS.UseService(s => s.UpdateTmsDeviceToolsPos(mTmsDeviceToolsPos));    //更新
                                    }
                                    TmsToolsMaster mToolsMaster = wsTMS.UseService(s =>
                                        s.GetTmsToolsMasterById(curProcess.FINISH_ITEM_PKNO));  //卸下刀具
                                    if (mToolsMaster != null)
                                    {
                                        mToolsMaster.TOOLS_POSITION = 10;  //已出库
                                        mToolsMaster.TOOLS_POSITION_PKNO = "";
                                        wsTMS.UseService(s => s.UpdateTmsToolsMaster(mToolsMaster));  //已出库
                                    }
                                }
                                else if (curProcess.PROCESS_ACTION_TYPE == 43)  //装刀
                                {
                                    TmsDeviceToolsPos mTmsDeviceToolsPos = wsTMS.UseService(s => s.GetTmsDeviceToolsPosById(curProcess.BEGIN_ITEM_PKNO)); //装上刀具
                                    if (mTmsDeviceToolsPos != null)
                                    {
                                        mTmsDeviceToolsPos.TOOLS_PKNO = curProcess.FINISH_ITEM_PKNO;  //装上刀具PKNO
                                        wsTMS.UseService(s => s.UpdateTmsDeviceToolsPos(mTmsDeviceToolsPos));    //更新
                                    }
                                    TmsToolsMaster mToolsMaster = wsTMS.UseService(s =>
                                        s.GetTmsToolsMasterById(curProcess.FINISH_ITEM_PKNO)); //装上刀具PKNO
                                    if (mToolsMaster != null)
                                    {
                                        mToolsMaster.TOOLS_POSITION = 2;  //在设备
                                        mToolsMaster.TOOLS_POSITION_PKNO = curProcess.BEGIN_POSITION;  //装刀机床PKNO
                                        wsTMS.UseService(s => s.UpdateTmsToolsMaster(mToolsMaster));  //已出库
                                    }
                                }
                                else if (curProcess.PROCESS_ACTION_TYPE == 44)  //放刀
                                {
                                    TmsToolsMaster mToolsMaster = wsTMS.UseService(s =>
                                        s.GetTmsToolsMasterById(curProcess.BEGIN_ITEM_PKNO));   //卸下刀具
                                    if (mToolsMaster != null)
                                    {
                                        mToolsMaster.TOOLS_POSITION = 1;
                                        mToolsMaster.TOOLS_POSITION_PKNO = curProcess.FINISH_ITEM_PKNO;   //位置信息
                                        wsTMS.UseService(s => s.UpdateTmsToolsMaster(mToolsMaster));  //已出库
                                    }
                                }
                            }

                            #endregion

                            #region 3. 更新数据  共更新 4 个类

                            #region 3.1 加工控制

                            curProcess.PROCESS_END_TIME = DateTime.Now;
                            curProcess.PROCESS_STATE = 10;
                            curProcess.PROCESS_END_TYPE = 1;
                            ws.UseService(s => s.UpdateMesProcessCtrol(curProcess));

                            #endregion

                            #region 3.2 Job处理

                            #region 最后一道工序的处理

                            if (bLastProcess)  //最后一道工序
                            {

                                job.COMPLETE_QTY = curProcess.COMPLETE_QTY;
                                job.ONLINE_QTY = 0;  //任务完成
                                job.ACT_FINISH_TIME = DateTime.Now;
                                job.RUN_STATE = 100; //正常完成
                                job.PROCESS_INFO = $"正常完成，计划数量[{job.TASK_QTY}]，完成数量[{job.COMPLETE_QTY}]";
                                Console.WriteLine($"订单[{job.JOB_ORDER_NO}]生产完成，计划数量[{job.TASK_QTY}]，完成数量[{job.COMPLETE_QTY}]");
                            }

                            #endregion

                            if (string.IsNullOrEmpty(job.PROCESS_INFO)) job.PROCESS_INFO = "正常";  //生产执行信息
                            ws.UseService(s => s.UpdateMesJobOrder(job));  //更新生产线任务

                            #endregion
                            
                            // 3.3 产品处理
                            if (productProcess != null)
                            {
                                if (bLastProcess) productProcess.PRODUCT_STATE = 100; //最后一道工序 正常完成

                                productProcess.LAST_UPDATE_DATE = DateTime.Now;
                                var process = productProcess;
                                ws.UseService(s => s.UpdateMesProductProcess(process));
                            }

                            #region 3.4 清空完成反馈状态，将完成条件置空

                            if (finishTag != null) //将完成结果置为空
                            {
                                if (finishTag.SAMPLING_MODE == 10) finishTag.SAMPLING_MODE = 11; //按照条件关闭
                                DeviceMonitor.SetTagSettingValue(finishTag, "");
                            }

                            #endregion

                            #endregion

                            #endregion

                            Thread.Sleep(50);
                            break;
                        }

                        #region 本次订单已经则重新刷新订单

                        Thread.Sleep(iConditionStartAddPause);  //条件启动 附加延时

                        if (bStart)
                        {
                            break;  //重新开始任务
                        }

                        #endregion

                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    Console.WriteLine("error:TheadAutoProcessControl message:" + ex.Message);
                }
            }

            Console.WriteLine("!!!!!!!!!!!控制程序停止运行!!!!!!!!!!!!!!!");
        }

        /// <summary>
        /// 将写入到设备的动态值进行转化
        /// </summary>
        /// <param name="value">原值</param>
        /// <returns>转换后的值</returns>
        private string ConvertDynamicValue(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            string result = value;

            if (value.Length > 4)  //"{[TagCode]}形式"
            {
                if ((value.Substring(0, 2) == "{[") && (value.Substring(value.Length - 2) == "]}"))  //按照TagCode获取Tag点的当前
                {
                    string tagCode = value.Substring(2, value.Length - 4);   //

                    FmsAssetTagSetting tag = DeviceMonitor.GetTagSettings($"TAG_CODE = '{tagCode}'").FirstOrDefault();
                    if (tag != null) result = tag.CUR_VALUE;  //
                }
            }

            return result;
        }

        #endregion
    }
}
