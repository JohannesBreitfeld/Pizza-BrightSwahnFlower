namespace OrderService.Core.DTOs
{
    public sealed record OrderResponse
    (
        string Id,
        string CustomerEmail,
        IEnumerable<OrderItemResponse> Items,
        decimal TotalAmount,
        decimal TaxAmount,
        DateTime CreatedAt
    );
}
