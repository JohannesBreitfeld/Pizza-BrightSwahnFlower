namespace PizzaFrontend.Contracts.Orders
{
    public sealed record CreateOrderResponse(string OrderId, string Email, decimal TotalAmount, decimal TaxAmount, DateTime CreatedAt);
}
