using OrderService.Core.Domain;
using OrderService.Core.DTOs;

namespace OrderService.Core.Mapping;

public static class OrderMapping
{
    public static OrderResponse ToResponse(this Order order) =>
        new OrderResponse(
            order.Id, 
            order.CustomerEmail,
            order.Items.Select(OrderItemMapping.ToResponse),
            order.TotalAmount,
            order.TaxAmount,
            order.CreatedAt
        );

    public static IEnumerable<OrderResponse> ToResponse(this IEnumerable<Order> orders) =>
        orders.Select(ToResponse);
}


