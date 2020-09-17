namespace Sello.OrderService.Model
{
    public class OrderItem
    {
        public int OrderLineNr { get; set; }

        public int ItemId { get; set; }

        public int Qty { get; set; }
    }
}