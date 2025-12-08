namespace PizzaFrontend.Contracts.Orders
{
    public sealed record OrderItemDTO(string ProductId, string ProductName, int Quantity, decimal UnitPrice);
}