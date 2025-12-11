using PizzaInformationService.Application.Pizza.GetPizzaById;

namespace PizzaInformationService.Application.Mapping
{
    public static class IngredientMapping
    {
        public static IngredientDto ToIngredientDto(this Domain.Entities.Ingredient ingredient)
        {
            return new IngredientDto
            (
                ingredient.Id,
                ingredient.Name
            );
        }
    }
}
