using System;
using System.IO;
using System.Collections.Generic;

namespace Sello.Model
{
    public class ShipmentRequest
    {
        public string CustomerName { get; set; }

        public string CustomerEmail { get; set; }
        public string OrderId { get; set; }
        public IList<ShipmentItem> ShipmentItems { get; set; }
    }

    public class ShipmentItem
    {
            public int ItemId { get; set; }

            public int Qty { get; set; }
    }
}