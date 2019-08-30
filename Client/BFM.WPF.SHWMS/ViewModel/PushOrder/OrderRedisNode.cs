using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFM.WPF.SHWMS.ViewModel.PushOrder
{
    public enum OrderItemStateEnum
    {
        NEW = 1,
        DOWORK = 2,
        DONE = 3
    }

    public enum ProductTypeEnum
    {
        Teacaddy,
        Brakedisc,
        Phoneshell,
        Flange,
        Seal

    }
    public class OrderItem
    {
        public string Id { get; set; }

        public int Type { get; set; }
        public ProductTypeEnum ProductType { get; set; }

        public int Quantity { get; set; }
        public int ActualQuantity { get; set; }

        public OrderItemStateEnum State { get; set; }

        public string StartTime { get; set; }
        public string FinishTime { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
