namespace PizzaInformationService.Application.Pizza.GetPizzaById
{
    public record PizzaResponse
    (
        int Id,
        string Name,
        string ImageUrl,
        decimal Price,

        IEnumerable<IngredientDto>? Ingredients
    );
}
