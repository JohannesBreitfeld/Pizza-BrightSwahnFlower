namespace PizzaFrontend.Contracts.Admin;

public sealed record OrderDTO(
    string OrderId,
    string CustomerEmail,
    List<OrderItemDTO> Items,
    decimal TotalAmount,
    decimal TaxAmount,
    DateTime CreatedAt,
    string Status
);

public sealed record OrderItemDTO(
    string ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice
);
