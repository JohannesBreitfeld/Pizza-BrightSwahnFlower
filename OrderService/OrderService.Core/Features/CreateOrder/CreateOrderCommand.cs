using MediatR;

namespace OrderService.Core.Features.CreateOrder;

public record CreateOrderCommand : IRequest<CreateOrderResponse>
{
    public string CustomerEmail { get; init; } = string.Empty;
    public List<OrderItemDto> Items { get; init; } = new();
}

public record OrderItemDto
{
    public string ProductId { get; init; } = string.Empty;
    public string ProductName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal UnitPrice { get; init; }
}
