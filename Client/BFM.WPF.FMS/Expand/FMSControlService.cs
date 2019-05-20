using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.FMSService;
using BFM.Server.DataAsset.PLMService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BFM.WPF.FMS.Expand
{
    public class FMSControlService : IDisposable
    {
        public List<FmsActionControl> ActionControls { get; private set; }
        private string linePKNO;

        private JobTaskModel jobTaskModel = new JobTaskModel();
        WcfClient<IPLMService> ws = new WcfClient<IPLMService>();
        private List<MesJobOrder> allJobs;

        List<Func<bool>> jobFunc = new List<Func<bool>>();
        public static int iConditionStartAddPause;
        public static bool bStart;

        public FMSControlService(string _linePKNO)
        {
            linePKNO = _linePKNO;
            WcfClient<IFMSService> wsFMS2 = new WcfClient<IFMSService>();
            ActionControls = wsFMS2.UseService(s => s.GetFmsActionControls(""));  //提高效率，基础数据先提取

            jobFunc.Add(GetMesJobOrder);
            jobFunc.Add(GetProductProgress);
            jobFunc.Add(CopyTag);
            jobFunc.Add(RunJob);
        }


        public void Run()
        {
            foreach (var item in jobFunc)
            {
                if (!item())
                {
                    break;
                }
            }
        }
        /// <summary>
        /// 获取工单
        /// </summary>
        private bool GetMesJobOrder()
        {
            allJobs =
                 ws.UseService(s => s.GetMesJobOrders($"RUN_STATE >= 10 AND RUN_STATE < 100 AND LINE_PKNO = '{linePKNO}'"))  //开工确认完成的
                     .OrderBy(c => c.CREATION_DATE)
                     .ToList();
            jobTaskModel.TaskSum = Convert.ToInt32(allJobs.Sum(c => c.TASK_QTY)); //计划数量
            jobTaskModel.CompleteSum = Convert.ToInt32(allJobs.Sum(c => c.COMPLETE_QTY)); //完成数量
            jobTaskModel.OnlineSum = Convert.ToInt32(allJobs.Sum(c => c.ONLINE_QTY));  //在线数量
            return allJobs.Count > 0;
        }


        private bool GetProductProgress()
        {
            jobTaskModel.ProductProcesses = ws.UseService(s =>
                       s.GetMesProductProcesss("USE_FLAG = 1 AND PRODUCT_STATE >= 0 AND PRODUCT_STATE < 100"));  //正在执行的产品信息

            List<MesProcessCtrol> allProcessCtrols = new List<MesProcessCtrol>();
            Parallel.ForEach(allJobs, (d) =>
            {
                WcfClient<IPLMService> ws = new WcfClient<IPLMService>();

                var processes = ws.UseService(s => s.GetMesProcessCtrols($"JOB_ORDER_PKNO = '{d.PKNO}'"))
                  .OrderBy(c => c.PROCESS_INDEX)
                  .ThenBy(c => c.CREATION_DATE)
                  .ToList(); //获取当前所有工序
                allProcessCtrols.AddRange(processes);
            });
            jobTaskModel.AllProcessCtrols = allProcessCtrols;
            return true;
        }

        private bool CopyTag()
        {

            jobTaskModel.Tags = DeviceMonitor.GetTagSettings("");
            jobTaskModel.CopyTags = new List<FmsAssetTagSetting>();

            foreach (var tag in jobTaskModel.Tags)
            {
                FmsAssetTagSetting copyTag = new FmsAssetTagSetting();
                copyTag.CopyDataItem(tag);
                jobTaskModel.CopyTags.Add(copyTag);
            }
            return true;
        }


        private bool RunJob()
        {
            foreach (var item in allJobs)
            {
                var taskProxy = new JobTaskProxy(item, jobTaskModel, ActionControls);
                taskProxy.PServiceEvent += (d) => ws.UseService(d);
                if (!taskProxy.Start())
                {
                    continue;
                }
                Thread.Sleep(iConditionStartAddPause);  //条件启动 附加延时

                if (bStart)
                {
                    break;
                }//已经进入开始条件后重新开始任务
            }
            return true;

        }

        private void UpdateServiceInfo(Expression<Func<IPLMService, bool>> action)
        {

            ws.UseService(action); //更新生产线任务

        }
        public void Dispose()
        {
        }
    }

    public class JobTaskModel
    {
        /// <summary>
        /// 计划数量
        /// </summary>
        public int TaskSum { get; set; }

        /// <summary>
        /// 完成数量
        /// </summary>
        public int CompleteSum { get; set; }

        /// <summary>
        /// 在线数量
        /// </summary>

        public int OnlineSum { get; set; }


        public List<MesProductProcess> ProductProcesses { get; set; }

        public List<MesProcessCtrol> AllProcessCtrols { set; get; }
        public List<FmsAssetTagSetting> Tags { set; get; }

        public List<FmsAssetTagSetting> CopyTags { get; set; }
    }
}