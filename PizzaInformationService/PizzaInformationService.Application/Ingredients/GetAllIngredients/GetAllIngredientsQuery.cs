using MediatR;
using PizzaInformationService.Application.Pizza.GetPizzaById;

namespace PizzaInformationService.Application.Ingredients.GetAllIngredients
{
        public record GetAllIngredientsQuery : IRequest<List<IngredientDto>>;
}
