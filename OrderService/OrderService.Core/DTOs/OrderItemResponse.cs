namespace OrderService.Core.DTOs;

public sealed record OrderItemResponse
(
    string ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice
);

