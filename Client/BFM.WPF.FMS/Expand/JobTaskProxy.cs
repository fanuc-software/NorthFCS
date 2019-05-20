using BFM.Common.Base.PubData;
using BFM.ContractModel;
using BFM.Server.DataAsset.PLMService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BFM.WPF.FMS.Expand
{
    public partial class JobTaskProxy
    {
        private const int ReWriteCount = 3;  //向设备写数据 重写次数

        private MesJobOrder mesJobOrder;
        private JobTaskModel jobTaskMode;

        public event Action<Expression<Func<IPLMService, bool>>> PServiceEvent;
        private List<MesProcessCtrol> theJobProcessCtrols;
        private List<MesProcessCtrol> unFinishProcessCtrols;
        private List<MesProcessCtrol> usefulProcessCtrols;
        private List<FmsActionControl> actionControls;
        private bool bFirstProcess;
        private bool bLastProcess;
        private string ctrolName;
        private MesProcessCtrol firstProcess;
        private MesProcessCtrol curProcess;
        private FmsActionControl curAction;
        private MesProductProcess productProcess;
        private FmsAssetTagSetting startTag;
        private string startCondition;
        List<string> LimitConditions = new List<string>();  //过程开始/结束的不满足的条件，防止后面的Job先触发
        public int RunState { set; get; }

        List<Func<bool>> jobFunc = new List<Func<bool>>();

        public JobTaskProxy(MesJobOrder _mesJobOrder, JobTaskModel _jobTaskModel, List<FmsActionControl> _actionControls)
        {
            mesJobOrder = _mesJobOrder;
            jobTaskMode = _jobTaskModel;
            actionControls = _actionControls;
            jobFunc.Add(Init);
            jobFunc.Add(InitData);
            jobFunc.Add(CheckState);
            jobFunc.Add(ProductProgress);
            jobFunc.Add(StartCurrentProgerss);
            jobFunc.Add(FinishProgress);

        }

        public bool Start()
        {
            foreach (var item in jobFunc)
            {
                if (item())
                {
                    return false;
                }
            }
            return true;

        }

        public bool Init()
        {
            if ((RunState != 1) && (!CBaseData.AppClosing))  //正在运行才执行
            {
                return false;
            }

            #region 整线报警

            FmsAssetTagSetting alertTag = DeviceMonitor.GetTagSettings("TAG_CODE = '整线报警'").FirstOrDefault();

            string alertValue = alertTag?.CUR_VALUE;

            if (alertValue == "1")  //有整线报警，系统退出
            {
                //break;
            }

            #endregion
            FMSControlService.iConditionStartAddPause = 10;  //条件启动启动后附加的延时  正常10ms，按条件启动后500ms
            FMSControlService.bStart = false;  //已经启动了，只要启动了就重新获取订单信息
            return true;

        }

        public bool InitData()
        {
            theJobProcessCtrols = jobTaskMode.AllProcessCtrols.Where(c => c.JOB_ORDER_PKNO == mesJobOrder.PKNO)
                       .OrderBy(c => c.PROCESS_INDEX)
                       .ThenBy(c => c.CREATION_DATE)
                       .ToList();

            unFinishProcessCtrols = theJobProcessCtrols.Where(c => c.PROCESS_STATE < 10)
                          .OrderBy(c => c.PROCESS_INDEX)
                          .ThenBy(c => c.CREATION_DATE)
                          .ToList();
            usefulProcessCtrols =
                         unFinishProcessCtrols.Where(c => c.USE_FLAG == 1).OrderBy(c => c.PROCESS_INDEX)
                             .ThenBy(c => c.CREATION_DATE).ToList();  //未完成的可用工序控制

            if (!usefulProcessCtrols.Any())
            {
                string sErrorInfo = $"没有未执行的可用流程信息";
                if (mesJobOrder.PROCESS_INFO != sErrorInfo)
                {
                    mesJobOrder.PROCESS_INFO = sErrorInfo; //生产执行信息
                    PServiceEvent?.Invoke(d => d.UpdateMesJobOrder(mesJobOrder));
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// 检验当前工序状态
        /// </summary>
        /// <returns></returns>
        public bool CheckState()
        {
            firstProcess = theJobProcessCtrols.OrderBy(c => c.PROCESS_INDEX).ThenBy(c => c.CREATION_DATE).FirstOrDefault(c => c.USE_FLAG == 1);  //第一道工序可用工序
            curProcess = usefulProcessCtrols.OrderBy(c => c.PROCESS_INDEX).ThenBy(c => c.CREATION_DATE).FirstOrDefault(c => c.PROCESS_STATE >= 0);  //当前执行的工序

            if (curProcess == null)
            {
                string sErrorInfo = $"没有可用的未执行工序控制信息";
                if (mesJobOrder.PROCESS_INFO != sErrorInfo)
                {
                    mesJobOrder.PROCESS_INFO = sErrorInfo; //生产执行信息
                    PServiceEvent?.Invoke(d => d.UpdateMesJobOrder(mesJobOrder));

                }
                return false;
            }

            if (curProcess.PROCESS_STATE == 0)
            {
                string sErrorInfo = $"当前工序尚未准备好，请准备好该工序.";
                if (mesJobOrder.PROCESS_INFO != sErrorInfo)
                {
                    mesJobOrder.PROCESS_INFO = sErrorInfo; //生产执行信息
                    PServiceEvent?.Invoke(d => d.UpdateMesJobOrder(mesJobOrder));

                }
                return false;
            }

            bFirstProcess = (curProcess.PROCESS_INDEX == firstProcess.PROCESS_INDEX);  //是否为第一道工序
            bLastProcess = (curProcess.PROCESS_INDEX == usefulProcessCtrols.LastOrDefault().PROCESS_INDEX); //是否为最后一个工序


            ctrolName = curProcess.PROCESS_CTROL_NAME ?? curProcess.PROCESS_INDEX.ToString();  //过程控制名称

            curAction = actionControls.FirstOrDefault(c => c.PKNO == curProcess.PROCESS_ACTION_PKNO);  //当前动作
            if (curAction == null)  //空动作的工序，手动完成
            {
                string sErrorInfo = $"当前生产过程[{ctrolName}]没有动作控制指令，须手动完成.";
                if (mesJobOrder.PROCESS_INFO != sErrorInfo)
                {
                    mesJobOrder.PROCESS_INFO = sErrorInfo; //生产执行信息
                    PServiceEvent?.Invoke(d => d.UpdateMesJobOrder(mesJobOrder));
                }

                Console.WriteLine(sErrorInfo);
                return false;
            }
            return true;
        }


        public bool ProductProgress()
        {
            productProcess = jobTaskMode.ProductProcesses.FirstOrDefault(c => c.PKNO == curProcess.CUR_PRODUCT_CODE_PKNO);  //产品生产情况;

            #region 当前工序的开始条件

            startTag = jobTaskMode.CopyTags.FirstOrDefault(c => c.PKNO == curAction.START_CONDITION_TAG_PKNO);

            startCondition = "";
            if (startTag != null)
            {
                startCondition = startTag.PKNO + ".VALUE = " + curAction.START_CONDITION_VALUE;
            }
            #endregion

            return true;
        }


        /// <summary>
        /// 获取动态文本的值
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns>转换后的值</returns>
        private string GetDynamicValue(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            string result = text;

            if (text.Length > 4)  //"{[TagCode]}形式"
            {
                if ((text.Substring(0, 2) == "{[") && (text.Substring(text.Length - 2) == "]}"))  //按照TagCode获取Tag点的当前
                {
                    string tagCode = text.Substring(2, text.Length - 4);   //

                    FmsAssetTagSetting tag = DeviceMonitor.GetTagSettings($"TAG_CODE = '{tagCode}'").FirstOrDefault();
                    if (tag != null) result = tag.CUR_VALUE;  //
                }
            }

            return result;
        }

    }
}
