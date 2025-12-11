using MediatR;
using PizzaInformationService.Application.Mapping;
using PizzaInformationService.Application.Pizza.GetPizzaById;
using PizzaInformationService.Domain.Interfaces;

namespace PizzaInformationService.Application.Ingredients.GetAllIngredients
{
    public class GetAllIngredientsHandler : IRequestHandler<GetAllIngredientsQuery, List<IngredientDto>>
    {
        private readonly IIngredientsRepository _ingredientsRepository;
        public GetAllIngredientsHandler(IIngredientsRepository ingredientsRepository)
        {
            _ingredientsRepository = ingredientsRepository;
        }

        public async Task<List<IngredientDto>> Handle(GetAllIngredientsQuery request, CancellationToken cancellationToken)
        {
            var ingredients = await _ingredientsRepository.GetAllIngredientsAsync();

            if(ingredients is null || !ingredients.Any())
            {
                return new List<IngredientDto>();
            }

            return ingredients.Select(p => p.ToIngredientDto()).ToList();
        }
    }
}
