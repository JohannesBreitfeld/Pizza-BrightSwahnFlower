namespace PizzaFrontend.Contracts.Admin;

public sealed record OrderDTO(
    string Id,
    string CustomerEmail,
    List<OrderItemDTO> Items,
    decimal TotalAmount,
    decimal TaxAmount,
    DateTime CreatedAt
);
