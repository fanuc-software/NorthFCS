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
        public JobService(MainJobViewModel mainJobViewModel)
        {
            this.mainJobViewModel = mainJobViewModel;
        }

        public void Start()
        {

            while (true)
            {

                Thread.Sleep(1000);
                var orders = mainJobViewModel.OrderNodes.Where(d => d.Sate == OrderStateEnum.Create).ToList();
                foreach (var item in orders)
                {
                    item.StartJob();
                    foreach (var job in item.Items)
                    {
                        while (item.WorkItem(job))
                        {
                            Thread.Sleep(1000);
                        }
                    }
                }
            }
        }
    }
}
