namespace PizzaFrontend.Contracts.Admin;

public sealed record OrderItemDTO(
    string ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice
);
