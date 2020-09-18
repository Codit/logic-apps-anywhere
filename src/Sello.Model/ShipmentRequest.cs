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
}