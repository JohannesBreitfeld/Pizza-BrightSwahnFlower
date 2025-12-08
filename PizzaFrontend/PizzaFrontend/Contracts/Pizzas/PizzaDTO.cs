namespace PizzaFrontend.Contracts.Pizzas
{
    public sealed record PizzaDTO(int Id, string Name, List<IngredientDTO> Ingredients, string ImageUrl, decimal Price);

}
