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
        private const int MaxCount = 4;
        public JobService(MainJobViewModel mainJobViewModel)
        {
            this.mainJobViewModel = mainJobViewModel;
        }

        public void Start()
        {

            var orders = mainJobViewModel.OrderNodes.Where(d => d.Sate == OrderStateEnum.Create).ToList();
         
            GenerateMachiningTask generateMachiningTask = new GenerateMachiningTask();
            CncSafeAndCommunication cncSafe = new CncSafeAndCommunication();

          

            foreach (OrderViewModel item in orders)
            {
                var totalTask = item.Items.Sum(d => d.Count);
                int count = totalTask / MaxCount;
                int remainder = totalTask % MaxCount;
                count = 1;
                item.StartJob();
                for (int i = 0; i < count;)
                {
                    var state = false;
                    if (cncSafe.GetDeviceProcessContolEmptyJobStateFromSavePool(ref state) == 0)
                    {
                        if (state)
                        {
                            generateMachiningTask.GenerateMachiningTask_Piece4();
                            i++;

                        }

                    }
                    Thread.Sleep(1000);
                }

            }




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
    }
}
