using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFM.WPF.SHWMS.ViewModel.PushOrder
{
    public enum OrderItemStateEnum
    {
        NEW,
        DOWORK,
        DONE
    }
    public class OrderRedisNode
    {
        public string Id { get; set; }

        public int Type { get; set; }

        public int Quantity { get; set; }
        public int ActualQuantity { get; set; }

        public OrderItemStateEnum State { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
