namespace PizzaFrontend.Contracts.Admin;

public sealed record UpdatePizzaRequest(
    string Name,
    decimal Price,
    string? ImageUrl,
    List<int> IngredientIds
);
