namespace PizzaMail.Contracts
{
    public class OrderCreatedEventData
    {
        public required string OrderId { get; set; }
        public required string CustomerEmail { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public required List<OrderItem> OrderItems { get; set; }
    }
}
