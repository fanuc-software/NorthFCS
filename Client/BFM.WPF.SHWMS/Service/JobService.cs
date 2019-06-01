using BFM.Common.Base.PubData;
using BFM.Common.Base.Utils;
using BFM.ContractModel;
using BFM.Server.DataAsset.PLMService;
using BFM.WPF.FMS;
using BFM.WPF.SHWMS.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BFM.WPF.SHWMS.Service
{
    public class JobService
    {
        MainJobViewModel mainJobViewModel;
        bool isPreFinish = false;
        private const int MaxCount = 4;

        public bool IsCycleStop = false;
        private WcfClient<IPLMService> ws = new WcfClient<IPLMService>();

        public event Action<string> TaskJobFinishEvent;
        public JobService(MainJobViewModel mainJobViewModel)
        {
            this.mainJobViewModel = mainJobViewModel;
        }

        public void Start(CancellationTokenSource tokenSource)
        {
            IsCycleStop = false;
            while (!tokenSource.IsCancellationRequested)
            {
                var orders = mainJobViewModel.OrderNodes.Where(d => d.Sate == OrderStateEnum.Create).ToList();
                GenerateMachiningTask generateMachiningTask = new GenerateMachiningTask();
                Action[] actions = { () => generateMachiningTask.GenerateMachiningTask_Piece4(), () => generateMachiningTask.GenerateMachiningTask_Piece1()
                ,()=>generateMachiningTask.GenerateMachiningTask_Piece2(),()=>generateMachiningTask.GenerateMachiningTask_Piece3()};
                CncSafeAndCommunication cncSafe = new CncSafeAndCommunication();
                var isFinised = false;

                foreach (OrderViewModel item in orders)
                {
                    if (IsCycleStop)
                    {
                        TaskJobFinishEvent?.Invoke("循环停止成功结束!");
                        return;
                    }
                    var totalTask = item.Items.Sum(d => d.Count);
                    int count = totalTask / MaxCount;
                    int remainder = totalTask % MaxCount;
                    item.StartJob();
                    item.VMOne.StartMachiningCount();
                    item.LatheTwo.StartMachiningCount();
                    FinishJob(cncSafe, tokenSource, item, isFinised, count, actions[0]);
                    FinishJob(cncSafe, tokenSource, item, isFinised, 1, actions[remainder]);
                    item.FinishJob();
                    item.VMOne.StopMachiningCount();
                    item.LatheTwo.StopMachiningCount();
                }

                Thread.Sleep(2000);
            }
            TaskJobFinishEvent?.Invoke("任务强制取消成功!");



            //foreach (var item in orders)
            //{
            //    item.StartJob();
            //    item.Items.Sum(d => d.Count);
            //    foreach (var job in item.Items)
            //    {
            //        while (item.WorkItem(job))
            //        {
            //            Thread.Sleep(1000);
            //        }
            //    }
            //}
        }

        void FinishJob(CncSafeAndCommunication cncSafe, CancellationTokenSource tokenSource, OrderViewModel item, bool isFinised, int count,Action action)
        {

            for (int i = 0; i <= count;)
            {
                var state = false;
                if (cncSafe.GetJobTaskFinishStateFromSavePool(ref isFinised) == 0)
                {
                    if (cncSafe.GetDeviceProcessContolEmptyJobStateFromSavePool(ref state) == 0)
                    {
                        if (state && isFinised && !isPreFinish)
                        {
                            if (tokenSource.IsCancellationRequested)
                            {
                                item.Sate = OrderStateEnum.Cancel;
                                item.VMOne.StopMachiningCount();
                                item.LatheTwo.StopMachiningCount();
                                TaskJobFinishEvent?.Invoke("任务强制取消成功!");
                                return;
                            }
                            if (i < count)
                            {
                                action();
                            }
                            i++;
                        }
                    }
                    isPreFinish = isFinised;

                }
                Thread.Sleep(1000);
            }

        }
        public void CancelJobOrder()
        {
            List<MesJobOrder> mesJobOrders = ws.UseService(s => s.GetMesJobOrders(
                 $"USE_FLAG = 1 AND RUN_STATE < 100 AND LINE_PKNO = '{CBaseData.CurLinePKNO}'")).OrderBy(c => c.CREATION_DATE).ToList();
            if (mesJobOrders == null)
            {

                return;
            }

            new Thread(new ThreadStart(delegate ()
            {
                CncSafeAndCommunication cncSafe = new CncSafeAndCommunication();
                cncSafe.ClearAgvTask();
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

            })).Start();
        }

        public void TestStart(CancellationTokenSource tokenSource)
        {
            GenerateMachiningTask generateMachiningTask = new GenerateMachiningTask();
            CncSafeAndCommunication cncSafe = new CncSafeAndCommunication();

            bool empty_state = false;
            var ret = cncSafe.GetDeviceProcessContolEmptyJobStateFromSavePool(ref empty_state);
            if (ret == 0 && empty_state == true)
            {
                generateMachiningTask.GenerateMachiningTask_Test();
            }

        }
    }
}
