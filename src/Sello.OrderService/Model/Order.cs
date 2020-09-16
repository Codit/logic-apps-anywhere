using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sello.OrderService.Model
{
    public class Order
    {
        public string  OrderNr { get; set; }
        public DateTime OrderDate { get; set; }

        public Customer Customer { get; set; }

        public IList<OrderItem> OrderItems { get; set; }
    }

    public class OrderItem
    {
        public int OrderLineNr { get; set; }

        public int ItemId { get; set; }

        public int Qty { get; set; }
    }
}
