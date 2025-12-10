namespace PizzaFrontend.Contracts.Admin;

public sealed record CreatePizzaRequest(
    string Name,
    decimal Price,
    string? ImageUrl,
    List<int> IngredientIds
);
