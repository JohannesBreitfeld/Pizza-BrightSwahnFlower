using OrderService.Core.DTOs;

namespace OrderService.Core.Features.GetAllOrders;

public sealed record GetAllOrdersResponse(IEnumerable<OrderResponse> Orders);
