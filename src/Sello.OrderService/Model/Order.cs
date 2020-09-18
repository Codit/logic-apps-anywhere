using System;
using System.Collections.Generic;

namespace Sello.OrderService.Model
{
    public class Order
    {
        public string  OrderNr { get; set; }
        public DateTime OrderDate { get; set; }

        public Customer Customer { get; set; }

        public IList<OrderItem> OrderItems { get; set; }
    }
}
