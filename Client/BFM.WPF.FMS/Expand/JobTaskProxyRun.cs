using BFM.Common.Base;
using BFM.Common.Base.PubData;
using BFM.ContractModel;
using BFM.WPF.SDM.TableNO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BFM.WPF.FMS.Expand
{
    public partial class JobTaskProxy
    {


        public bool StartCurrentProgerss()
        {
            if (curProcess.PROCESS_STATE == 1) //准备完成，未开始执行
            {
                if (!string.IsNullOrEmpty(startCondition) && LimitConditions.Contains(startCondition))  //如果前面的Job存在需要判断该状态，则不执行当前的
                {
                    string sErrorInfo = $"等待执行";
                    if (mesJobOrder.PROCESS_INFO != sErrorInfo)
                    {
                        mesJobOrder.PROCESS_INFO = sErrorInfo; //生产执行信息
                        PServiceEvent?.Invoke(d => d.UpdateMesJobOrder(mesJobOrder));
                    }

                    return false;
                }

                #region 开始执行工序 共 5 步

                string startCustomParam = ""; //开始执行的参数，增加到同步写入数据后面
                                              /************ ！！！工序开始的条件！！！  ************/

                #region 1. 检验当前执行的工序是否可以执行，启动条件

                if (startTag != null) //启动条件为空则直接执行
                {
                    string checkValue = startTag.CUR_VALUE.Split('|')[0]; //当前执行条件的结果
                    string startValue = curAction.START_CONDITION_VALUE;  //开始条件
                    string[] startValues = startValue.Split('|');  //多个开启条件

                    if ((string.IsNullOrEmpty(checkValue)) || (!startValues.Contains(checkValue))) //当前值不是工序开始检测值
                    {
                        #region 条件不符合的 流程分支情况

                        if (curProcess.PROCESS_ACTION_TYPE == 4) //流程分支，不启用，直接往下运行
                        {
                            #region 直接完成当前工序

                            #region 第一道工序处理

                            if (bFirstProcess) //第一道工序
                            {
                                mesJobOrder.RUN_STATE = 20; //正在执行
                                mesJobOrder.ACT_START_TIME = DateTime.Now; //修改任务开始时间
                                mesJobOrder.PROCESS_INFO = "正常"; //生产执行信息
                                PServiceEvent?.Invoke(d => d.UpdateMesJobOrder(mesJobOrder));
                            }

                            #endregion

                            curProcess.PROCESS_STATE = 10; //直接完成
                            curProcess.PROCESS_END_TYPE = 1;
                            curProcess.PROCESS_END_TIME = DateTime.Now;
                            curProcess.REMARK = "条件不满足，不启用流程分支，直接执行现有流程。";
                            PServiceEvent?.Invoke(d => d.UpdateMesJobOrder(mesJobOrder));

                            return false; //直接执行重新执行

                            #endregion
                        }

                        #endregion

                        #region 判断条件不满足

                        string sErrorInfo =
                            $"生产过程【{ctrolName}】开启条件【{startTag?.TAG_NAME}】不足，当前状态为【{checkValue}】,需要状态【{startValue}】";
                        if (mesJobOrder.PROCESS_INFO != sErrorInfo)
                        {
                            mesJobOrder.PROCESS_INFO = sErrorInfo; //生产执行信息
                            PServiceEvent?.Invoke(d => d.UpdateMesJobOrder(mesJobOrder));
                        }

                        if (!LimitConditions.Contains(startCondition))
                        {
                            LimitConditions.Add(startCondition); //起始条件不满足，则添加限制条件
                        }

                        return false;

                        #endregion
                    }
                }

                #endregion

                /************ ！！！工序开始的条件满足，直接执行！！！  ************/
                Console.WriteLine($"========生产过程【{ctrolName}】开始条件【{startTag?.TAG_NAME}】满足，开始执行=======");

                #region 2. 启动流程分支

                if (curProcess.PROCESS_ACTION_TYPE == 4) //流程分支
                {
                    string disableProcesses = curProcess.PROCESS_ACTION_PARAM1_VALUE; //禁用的流程
                    string enableProcesses = curProcess.PROCESS_ACTION_PARAM2_VALUE; //启用的流程

                    string stemp = $"触发流程分支.禁用流程[{disableProcesses}], 启用流程[{enableProcesses}]";
                    mesJobOrder.PROCESS_INFO = stemp; //生产执行信息
                    Console.WriteLine(stemp);

                    #region 启动流程分支

                    if (!string.IsNullOrEmpty(disableProcesses))
                    {
                        foreach (var disableP in disableProcesses.Split(';'))
                        {
                            MesProcessCtrol process = unFinishProcessCtrols.FirstOrDefault(c =>
                                c.PROCESS_INDEX == SafeConverter.SafeToInt(disableP, -1));
                            process.USE_FLAG = 0; //禁用
                            PServiceEvent?.Invoke(d => d.UpdateMesProcessCtrol(process));


                        }
                    }

                    if (!string.IsNullOrEmpty(enableProcesses))
                    {
                        foreach (var enableP in enableProcesses.Split(';'))
                        {
                            MesProcessCtrol process = unFinishProcessCtrols.FirstOrDefault(c =>
                                c.PROCESS_INDEX == SafeConverter.SafeToInt(enableP, -1));
                            process.USE_FLAG = 1; //启用
                            PServiceEvent?.Invoke(d => d.UpdateMesProcessCtrol(process));

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
                    PServiceEvent?.Invoke(d => d.UpdateMesProcessCtrol(curProcess));

                    #endregion

                    #region Job处理

                    if (bFirstProcess) //第一道工序
                    {
                        mesJobOrder.RUN_STATE = 20; //正在执行
                        mesJobOrder.ACT_START_TIME = DateTime.Now; //修改任务开始时间
                    }

                    PServiceEvent?.Invoke(d => d.UpdateMesJobOrder(mesJobOrder));
                    #endregion

                    DeviceMonitor.SetTagSettingValueById(curAction.START_CONDITION_TAG_PKNO, ""); //将检测结果置为空

                    //产品处理
                    if (productProcess != null)
                    {
                        productProcess.PRODUCT_STATE = curProcess.PROCESS_ACTION_TYPE; //当前状态
                        productProcess.LAST_UPDATE_DATE = DateTime.Now;
                        var process = productProcess;
                        PServiceEvent?.Invoke(d => d.UpdateMesProductProcess(process));

                    }

                    #endregion

                    return false; //直接开启下一次流程
                } //启动流程分支

                #endregion

                //增加限制条件
                if (!string.IsNullOrEmpty(startCondition) &&
                    !LimitConditions.Contains(startCondition)) //未完成的任务需要增加开始条件
                {
                    LimitConditions.Add(startCondition);
                }

                #region 3. 获取产品，第一道工序形成产品

                if (bFirstProcess || (productProcess == null)) //第一道控制，且没有形成
                {
                    string productPKNO = CBaseData.NewGuid();

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
                        LAST_UPDATE_DATE = DateTime.Now, //最后修改日期
                        USE_FLAG = 1,
                        REMARK = "", //备注
                    }; //生成新的产品

                    curProcess.CUR_PRODUCT_CODE_PKNO = productPKNO; //新产品编号
                }
                else
                {
                    curProcess.CUR_PRODUCT_CODE_PKNO = firstProcess.CUR_PRODUCT_CODE_PKNO; //将所有的产品
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
                sTagValue = GetDynamicValue(curProcess.PROCESS_ACTION_PARAM1_VALUE);

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
                        if (mesJobOrder.PROCESS_INFO != sErrorInfo)
                        {
                            mesJobOrder.PROCESS_INFO = sErrorInfo; //生产执行信息
                            PServiceEvent?.Invoke(d => d.UpdateMesJobOrder(mesJobOrder));
                        }

                        Console.WriteLine(sErrorInfo);
                        return false;
                    }

                    if (ret == 0)
                    {
                        bWriteSuccess = true; //写入成功
                    }
                }

                #endregion

                #region  4.2 写参数2

                tagPKNO = curAction.EXECUTE_PARAM2_TAG_PKNO;
                sTagValue = GetDynamicValue(curProcess.PROCESS_ACTION_PARAM2_VALUE);

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
                        if (mesJobOrder.PROCESS_INFO != sErrorInfo)
                        {
                            mesJobOrder.PROCESS_INFO = sErrorInfo; //生产执行信息
                            PServiceEvent?.Invoke(d => d.UpdateMesJobOrder(mesJobOrder));
                        }

                        Console.WriteLine(sErrorInfo);
                        return false;
                    }

                    if (ret == 0) bWriteSuccess = true; //写入成功
                }

                #endregion

                #region  4.3 写执行

                tagPKNO = curAction.EXECUTE_TAG_PKNO;
                sTagValue = GetDynamicValue(curAction.EXECUTE_WRITE_VALUE) +
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
                        if (mesJobOrder.PROCESS_INFO != sErrorInfo)
                        {
                            mesJobOrder.PROCESS_INFO = sErrorInfo; //生产执行信息
                            PServiceEvent?.Invoke(d => d.UpdateMesJobOrder(mesJobOrder));
                        }

                        Console.WriteLine(sErrorInfo);
                        return false;
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

                #region 5. 更新数据 共更新 4 个模块

                #region 5.1 加工控制

                curProcess.PROCESS_STATE = 2; //正在执行
                curProcess.PROCESS_START_TYPE = 1; //自动开始
                curProcess.PROCESS_START_TIME = DateTime.Now;

                PServiceEvent?.Invoke(d => d.UpdateMesProcessCtrol(curProcess));
                #endregion

                #region 5.2 业务相关处理

                if (curProcess.PROCESS_ACTION_TYPE == 11) //开始 出库
                {
                    mesJobOrder.ONLINE_QTY = curProcess.PROCESS_QTY; //在线数量
                }

                if (bFirstProcess)  //第一道工序，新增产品信息
                {
                    PServiceEvent?.Invoke(s => s.AddMesProductProcess(productProcess));
                }

                string sError = "";

                string result = DeviceProcessControl.BeginCurBusiness(curProcess, ref sError);  //开始当前控制的业务

                if (result != "OK")
                {
                    mesJobOrder.PROCESS_INFO = $"业务开始执行发生错误，写入数据库错误! 具体:{sError}";
                }

                #endregion

                #region 5.3 Job处理

                if (bFirstProcess) //第一道工序
                {
                    mesJobOrder.RUN_STATE = 20; //正在执行
                    mesJobOrder.ACT_START_TIME = DateTime.Now; //修改任务开始时间
                }

                PServiceEvent?.Invoke(d => d.UpdateMesJobOrder(mesJobOrder));
                #endregion

                #region 5.4 将开始和结束条件 置空

                DeviceMonitor.SetTagSettingValueById(curAction.START_CONDITION_TAG_PKNO, ""); //将检测结果置为空

                FmsAssetTagSetting finishTag = DeviceMonitor.GetTagSettingById(curAction.FINISH_CONDITION_TAG_PKNO);

                if (finishTag != null)
                {
                    if (finishTag.SAMPLING_MODE == 11) finishTag.SAMPLING_MODE = 10; //按照条件开启

                    DeviceMonitor.SetTagSettingValue(finishTag, "");
                }


                #endregion

                #endregion

                #endregion

                if (startTag != null)
                {
                    FMSControlService.iConditionStartAddPause = 500;  //按条件启动后增加的延时
                }

                FMSControlService.bStart = true;  //本次订单已经启动过了
            }

            return true;
        }

        public bool FinishProgress()
        {
            if (curProcess.PROCESS_STATE == 2)   //正在执行，完成该动作
            {
                if (!string.IsNullOrEmpty(startCondition) && !LimitConditions.Contains(startCondition))  //未完成的任务需要增加开始条件
                {
                    LimitConditions.Add(startCondition);
                }

                #region 完成执行工序 共 3 步

                /************ ！！！工序完成的条件！！！  ************/
                string sCurFinishValue = "";  //当前完成的结果

                #region 1. 检验当前工序是否完成，未完成，则下一个任务

                FmsAssetTagSetting finishTag = jobTaskMode.CopyTags.FirstOrDefault(c => c.PKNO == curAction.FINISH_CONDITION_TAG_PKNO);

                if (finishTag != null)
                {
                    string condition = finishTag.PKNO + ".VALUE = " + curAction.FINISH_CONDITION_VALUE;

                    //如果前面的Job存在需要判断该状态，则不执行完成当前
                    if ((startCondition != condition) && LimitConditions.Contains(condition))
                    {
                        string sErrorInfo =
                            $"正在执行[{ctrolName}],完成条件[{finishTag?.TAG_NAME}]不足.";
                        if (mesJobOrder.PROCESS_INFO != sErrorInfo)
                        {
                            mesJobOrder.PROCESS_INFO = sErrorInfo; //生产执行信息
                            PServiceEvent?.Invoke(s => s.UpdateMesJobOrder(mesJobOrder));
                        }

                        if (FMSControlService.bStart)
                        {
                            return true;
                        }

                    }

                    if (finishTag.SAMPLING_MODE == 11)  //动作开始后自动开启 => 尚未开启，需要开启
                    {
                        var tag0 = DeviceMonitor.GetTagSettingById(curAction.FINISH_CONDITION_TAG_PKNO);
                        tag0.SAMPLING_MODE = 10;  //尚未开启，需要开启
                        DeviceMonitor.SetTagSettingValue(tag0, "");  //清空完成条件

                        if (FMSControlService.bStart)
                        {
                            return true;
                        }

                    }

                    sCurFinishValue = finishTag.CUR_VALUE;  //当前设备的值

                    string checkValue = sCurFinishValue.Split('|')[0];  //多结果情况，适用于检测

                    string finishValue = curAction.FINISH_CONDITION_VALUE;
                    string[] finishValues = finishValue.Split('|');  //多个完成条件

                    if (!finishValues.Contains(checkValue)) //当前值不是工序结束值，不完成
                    {
                        string sErrorInfo =
                            $"正在执行[{ctrolName}],获取到完成结果为[{checkValue}]，判断完成条件为[{finishValue}]，生产过程[{ctrolName}]尚未完成.";
                        if (mesJobOrder.PROCESS_INFO != sErrorInfo)
                        {
                            mesJobOrder.PROCESS_INFO = sErrorInfo; //生产执行信息
                            PServiceEvent?.Invoke(s => s.UpdateMesJobOrder(mesJobOrder));
                        }

                        if (!LimitConditions.Contains(condition)) LimitConditions.Add(condition); //完成条件不满足


                        if (FMSControlService.bStart)
                        {
                            return true;
                        }
                    }

                    //移除完成限制条件
                    LimitConditions.Remove(condition);  //完成条件满足 则移除不满足的条件

                    if (string.IsNullOrEmpty(startCondition)) LimitConditions.Remove(startCondition);  //移除开始的限制条件

                }
                else  //没有动作完成的检测值，手动完成。
                {
                    string sErrorInfo = $"生产过程[{ctrolName}]没有设置过程完成的检测值，请手动完成该过程."; //生产执行信息
                    if (mesJobOrder.PROCESS_INFO != sErrorInfo)
                    {
                        mesJobOrder.PROCESS_INFO = sErrorInfo; //生产执行信息
                        PServiceEvent?.Invoke(s => s.UpdateMesJobOrder(mesJobOrder));
                    }

                    if (string.IsNullOrEmpty(startCondition)) LimitConditions.Remove(startCondition);  //移除开始的限制条件

                    if (FMSControlService.bStart)
                    {
                        return true;
                    }

                }

                #endregion

                /************ ！！！工序已经完成！！！  ************/

                Console.WriteLine($"收到设备完成指令，完成反馈结果为[{sCurFinishValue}]，完成生产过程[{ctrolName}]");

                #region 2. 根据工序类型处理 完成当前工序

                if (curProcess.PROCESS_ACTION_TYPE == 3) //检测完成
                {
                    //合格数量
                    string[] checkResult = sCurFinishValue.Split('|');
                    int qualifiedNumber = 1;  //合格品数量
                    if (checkResult.Count() >= 2)
                    {
                        qualifiedNumber = SafeConverter.SafeToInt(checkResult[1], 1);
                    }
                    curProcess.COMPLETE_QTY = qualifiedNumber;  //生产数量 - 含不合格的丢弃
                    curProcess.QUALIFIED_QTY = qualifiedNumber;  //合格品数量

                    mesJobOrder.ONLINE_QTY = mesJobOrder.ONLINE_QTY - firstProcess.PROCESS_QTY + curProcess.QUALIFIED_QTY; //在线数量，更新再制品数量
                    if (mesJobOrder.ONLINE_QTY < 0)
                    {
                        mesJobOrder.ONLINE_QTY = 0;
                    }
                }

                if (productProcess != null)
                {
                    if (bLastProcess) productProcess.PRODUCT_STATE = 100; //最后一道工序 正常完成

                    string result = DeviceProcessControl.FinishCurBusiness(curProcess);  //完成业务
                    if (result != "OK")
                    {
                        string sErrorInfo = $"业务完成失败，写入数据库错误！";
                        if (mesJobOrder.PROCESS_INFO != sErrorInfo)
                        {
                            mesJobOrder.PROCESS_INFO = sErrorInfo; //生产执行信息
                            PServiceEvent?.Invoke(s => s.UpdateMesJobOrder(mesJobOrder));
                        }
                    }
                }

                #region 已删除 - 之前的方式

                if (curProcess.PROCESS_ACTION_TYPE == 1) //加工完成
                {
                    //if (productProcess != null)
                    //{
                    //    productProcess.CUR_ITEM_PKNO = curProcess.FINISH_ITEM_PKNO; //生产完成
                    //}
                }
                else if (curProcess.PROCESS_ACTION_TYPE == 2) //搬运完成
                {
                    //if (productProcess != null)
                    //{
                    //    productProcess.PRODUCT_POSITION = curProcess.FINISH_POSITION; //目标位置
                    //}
                }
                else if (curProcess.PROCESS_ACTION_TYPE == 3) //检测完成
                {
                    //合格数量
                    string[] result = sCurFinishValue.Split('|');
                    int qualifiedNumber = 1;  //合格品数量
                    if (result.Count() >= 2)
                    {
                        qualifiedNumber = SafeConverter.SafeToInt(result[1], 1);
                    }
                    curProcess.COMPLETE_QTY = qualifiedNumber;  //生产数量 - 含不合格的丢弃
                    curProcess.QUALIFIED_QTY = qualifiedNumber;  //合格品数量

                    mesJobOrder.ONLINE_QTY = mesJobOrder.ONLINE_QTY - firstProcess.PROCESS_QTY + curProcess.QUALIFIED_QTY; //在线数量，更新再制品数量
                    if (mesJobOrder.ONLINE_QTY < 0) { mesJobOrder.ONLINE_QTY = 0; }
                }
                else if (curProcess.PROCESS_ACTION_TYPE == 11) //出库完成
                {
                    //if (productProcess != null) productProcess.PRODUCT_POSITION = curProcess.FINISH_POSITION; //目标位置

                    //#region 解锁货位地址 - 货位清空

                    //if (!string.IsNullOrEmpty(curProcess.BEGIN_POSITION))
                    //{
                    //    WmsAllocationInfo rawAllo = wsWMS.UseService(s =>
                    //        s.GetWmsAllocationInfoById(curProcess.BEGIN_POSITION));

                    //    if (rawAllo != null)
                    //    {
                    //        rawAllo.CUR_PALLET_NO = "";
                    //        rawAllo.ALLOCATION_STATE = 0;  //空

                    //        wsWMS.UseService(s => s.UpdateWmsAllocationInfo(rawAllo));

                    //        WmsInventory inv = wsWMS.UseService(s => s.GetWmsInventorys($"ALLOCATION_PKNO = '{rawAllo.PKNO}'"))
                    //            .FirstOrDefault();

                    //        if (inv != null) wsWMS.UseService(s => s.DelWmsInventory(inv.PKNO));
                    //    }
                    //}

                    //#endregion
                }
                else if (curProcess.PROCESS_ACTION_TYPE == 12) //入库完成
                {
                    //if (productProcess != null) productProcess.PRODUCT_POSITION = curProcess.FINISH_POSITION; //目标位置

                    //#region 产品入库处理 - 解锁货位地址、增加产品库存

                    //if (!string.IsNullOrEmpty(curProcess.FINISH_POSITION))
                    //{
                    //    WmsAllocationInfo prodAllo = wsWMS.UseService(s =>
                    //        s.GetWmsAllocationInfoById(curProcess.FINISH_POSITION));

                    //    if (prodAllo != null)  //入库货位
                    //    {
                    //        prodAllo.ALLOCATION_STATE = 100; //满货位

                    //        WmsInventory inv = new WmsInventory()
                    //        {
                    //            PKNO = CBaseData.NewGuid(),
                    //            COMPANY_CODE = "",
                    //            MATERIAL_PKNO = curProcess.FINISH_ITEM_PKNO,
                    //            ALLOCATION_PKNO = prodAllo.PKNO,
                    //            AREA_PKNO = prodAllo.AREA_PKNO,
                    //            BATCH_NO = curProcess.SUB_JOB_ORDER_NO,
                    //            INVENTORY_NUM = 1, //curProcess.QUALIFIED_QTY?? 1,
                    //            REMARK = "",
                    //        };  //库存

                    //        wsWMS.UseService(s => s.UpdateWmsAllocationInfo(prodAllo));  //修改货位

                    //        wsWMS.UseService(s => s.AddWmsInventory(inv));
                    //    }
                    //}

                    //#endregion
                }
                else if ((curProcess.PROCESS_ACTION_TYPE >= 40) &&
                         (curProcess.PROCESS_ACTION_TYPE < 50)) //换刀
                {
                    //40：换刀；41：取刀；42：卸刀；43：装刀；44：放刀
                    //if (curProcess.PROCESS_ACTION_TYPE == 41)  //取刀
                    //{
                    //    TmsToolsMaster mToolsMaster = wsTMS.UseService(s =>
                    //        s.GetTmsToolsMasterById(curProcess.BEGIN_ITEM_PKNO));   //装上刀具
                    //    if (mToolsMaster != null)
                    //    {
                    //        mToolsMaster.TOOLS_POSITION = 10;  //出库
                    //        mToolsMaster.TOOLS_POSITION_PKNO = "";
                    //        wsTMS.UseService(s => s.UpdateTmsToolsMaster(mToolsMaster));
                    //    }
                    //}
                    //else if (curProcess.PROCESS_ACTION_TYPE == 42)  //卸刀
                    //{
                    //    TmsDeviceToolsPos mTmsDeviceToolsPos = wsTMS.UseService(s => s.GetTmsDeviceToolsPosById(curProcess.BEGIN_ITEM_PKNO)); //卸下刀具
                    //    if (mTmsDeviceToolsPos != null)
                    //    {
                    //        mTmsDeviceToolsPos.TOOLS_PKNO = "";
                    //        wsTMS.UseService(s => s.UpdateTmsDeviceToolsPos(mTmsDeviceToolsPos));    //更新
                    //    }
                    //    TmsToolsMaster mToolsMaster = wsTMS.UseService(s =>
                    //        s.GetTmsToolsMasterById(curProcess.FINISH_ITEM_PKNO));  //卸下刀具
                    //    if (mToolsMaster != null)
                    //    {
                    //        mToolsMaster.TOOLS_POSITION = 10;  //已出库
                    //        mToolsMaster.TOOLS_POSITION_PKNO = "";
                    //        wsTMS.UseService(s => s.UpdateTmsToolsMaster(mToolsMaster));  //已出库
                    //    }
                    //}
                    //else if (curProcess.PROCESS_ACTION_TYPE == 43)  //装刀
                    //{
                    //    TmsDeviceToolsPos mTmsDeviceToolsPos = wsTMS.UseService(s => s.GetTmsDeviceToolsPosById(curProcess.BEGIN_ITEM_PKNO)); //装上刀具
                    //    if (mTmsDeviceToolsPos != null)
                    //    {
                    //        mTmsDeviceToolsPos.TOOLS_PKNO = curProcess.FINISH_ITEM_PKNO;  //装上刀具PKNO
                    //        wsTMS.UseService(s => s.UpdateTmsDeviceToolsPos(mTmsDeviceToolsPos));    //更新
                    //    }
                    //    TmsToolsMaster mToolsMaster = wsTMS.UseService(s =>
                    //        s.GetTmsToolsMasterById(curProcess.FINISH_ITEM_PKNO)); //装上刀具PKNO
                    //    if (mToolsMaster != null)
                    //    {
                    //        mToolsMaster.TOOLS_POSITION = 2;  //在设备
                    //        mToolsMaster.TOOLS_POSITION_PKNO = curProcess.BEGIN_POSITION;  //装刀机床PKNO
                    //        wsTMS.UseService(s => s.UpdateTmsToolsMaster(mToolsMaster));  //已出库
                    //    }
                    //}
                    //else if (curProcess.PROCESS_ACTION_TYPE == 44)  //放刀
                    //{
                    //    TmsToolsMaster mToolsMaster = wsTMS.UseService(s =>
                    //        s.GetTmsToolsMasterById(curProcess.BEGIN_ITEM_PKNO));   //卸下刀具
                    //    if (mToolsMaster != null)
                    //    {
                    //        mToolsMaster.TOOLS_POSITION = 1;
                    //        mToolsMaster.TOOLS_POSITION_PKNO = curProcess.FINISH_ITEM_PKNO;   //位置信息
                    //        wsTMS.UseService(s => s.UpdateTmsToolsMaster(mToolsMaster));  //已出库
                    //    }
                    //}
                }

                #endregion

                #endregion

                #region 3. 更新数据  共更新 4 个类

                #region 3.1 加工控制

                curProcess.PROCESS_END_TIME = DateTime.Now;
                curProcess.PROCESS_STATE = 10;
                curProcess.PROCESS_END_TYPE = 1;
                PServiceEvent?.Invoke(s => s.UpdateMesProcessCtrol(curProcess));
                #endregion

                #region 3.2 Job处理

                #region 最后一道工序的处理

                mesJobOrder.PROCESS_INFO = "正常";  //生产执行信息

                if (bLastProcess)  //最后一道工序
                {
                    mesJobOrder.COMPLETE_QTY = curProcess.COMPLETE_QTY;
                    mesJobOrder.ONLINE_QTY = 0;  //任务完成
                    mesJobOrder.ACT_FINISH_TIME = DateTime.Now;
                    mesJobOrder.RUN_STATE = 100; //正常完成
                    mesJobOrder.PROCESS_INFO = $"正常完成，计划数量[{mesJobOrder.TASK_QTY}]，完成数量[{mesJobOrder.COMPLETE_QTY}]";
                    Console.WriteLine($"订单[{mesJobOrder.JOB_ORDER_NO}]生产完成，计划数量[{mesJobOrder.TASK_QTY}]，完成数量[{mesJobOrder.COMPLETE_QTY}]");
                }

                #endregion
                PServiceEvent?.Invoke(s => s.UpdateMesJobOrder(mesJobOrder));


                #endregion

                #region 3.4 清空完成反馈状态，将完成条件置空

                var tag = DeviceMonitor.GetTagSettingById(curAction.FINISH_CONDITION_TAG_PKNO);
                if (tag != null) //将完成结果置为空
                {
                    if (tag.SAMPLING_MODE == 10) tag.SAMPLING_MODE = 11; //按照条件关闭
                    DeviceMonitor.SetTagSettingValue(tag, "");
                }

                #endregion

                #endregion

                #endregion

                Thread.Sleep(50);
                return true;
            }
            return true;
        }
    }
}
