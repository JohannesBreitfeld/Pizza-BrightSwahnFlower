using PizzaInformationService.Application.Pizza.CreatePizza;
using PizzaInformationService.Application.Pizza.GetPizzaById;
using PizzaInformationService.Domain.Entities;

namespace PizzaInformationService.Application.Mapping
{
    public static class PizzaMapping
    {
        public static PizzaResponse ToPizzaResponse(this Domain.Entities.Pizza pizza)
        {
            return new PizzaResponse
            (
                pizza.Id,
                pizza.Name,
                pizza.ImageUrl,
                pizza.Price,
                pizza.Ingredients?.Select(i => i.ToIngredientDto())
            );
        }
        public static Domain.Entities.Pizza ToPizzaEntity(this CreatePizzaRequest createPizzaRequest, List<Ingredient> ingredients)
        {
            return new Domain.Entities.Pizza
            {
                Name = createPizzaRequest.Name,
                ImageUrl = createPizzaRequest.ImageUrl,
                Price = createPizzaRequest.Price,
                Ingredients = ingredients
            };
        }
    }
}
