using OrderService.Core.Domain;
using OrderService.Core.DTOs;

namespace OrderService.Core.Mapping;

public static class OrderItemMapping
{
   public static OrderItemResponse ToResponse(this OrderItem orderItem) =>
       new OrderItemResponse(
           orderItem.ProductId, 
           orderItem.ProductName, 
           orderItem.Quantity, 
           orderItem.UnitPrice);
}
