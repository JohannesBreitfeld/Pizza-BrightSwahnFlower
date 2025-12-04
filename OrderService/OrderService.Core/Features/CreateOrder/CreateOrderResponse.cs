namespace OrderService.Core.Features.CreateOrder;

public record CreateOrderResponse
{
    public string OrderId { get; init; } = string.Empty;
    public string CustomerEmail { get; init; } = string.Empty;
    public decimal TotalAmount { get; init; }
    public decimal TaxAmount { get; init; }
    public DateTime CreatedAt { get; init; }
}
