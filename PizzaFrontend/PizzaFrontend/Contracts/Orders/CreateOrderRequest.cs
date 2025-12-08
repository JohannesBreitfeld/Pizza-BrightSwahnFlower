namespace PizzaFrontend.Contracts.Orders
{
    public sealed record CreateOrderRequest(string CustomerEmail, List<OrderItemDTO> Items);
}
