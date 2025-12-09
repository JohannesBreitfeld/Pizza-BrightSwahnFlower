namespace PizzaMail.Contracts
{
    public class OrderItem
    {
        public required string ProductId { get; set; }
        public required string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
