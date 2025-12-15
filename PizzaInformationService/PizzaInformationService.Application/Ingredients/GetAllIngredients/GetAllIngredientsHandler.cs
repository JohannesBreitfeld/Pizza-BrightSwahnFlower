using MediatR;
using PizzaInformationService.Application.Abstractions;
using PizzaInformationService.Application.Mapping;
using PizzaInformationService.Application.Pizza.GetPizzaById;
using PizzaInformationService.Domain.Interfaces;

namespace PizzaInformationService.Application.Ingredients.GetAllIngredients
{
    public class GetAllIngredientsHandler : IRequestHandler<GetAllIngredientsQuery, List<IngredientDto>>
    {
        private readonly IIngredientsRepository _ingredientsRepository;
        private readonly ICacheService _cacheService;
        public GetAllIngredientsHandler(IIngredientsRepository ingredientsRepository, ICacheService cacheService)
        {
            _ingredientsRepository = ingredientsRepository;
            _cacheService = cacheService;
        }

        public async Task<List<IngredientDto>> Handle(GetAllIngredientsQuery request, CancellationToken cancellationToken)
        {
            const string key = "all_ingredients";

            var cached = await _cacheService.GetAsync<List<IngredientDto>>(key);
            if (cached != null) return cached;

            var ingredients = await _ingredientsRepository.GetAllIngredientsAsync();

            if(ingredients is null || !ingredients.Any())
            {
                return new List<IngredientDto>();
            }

            var returnedIngredients = ingredients.Select(p => p.ToIngredientDto()).ToList();
            await _cacheService.SetAsync(key, returnedIngredients, TimeSpan.FromHours(1));

            return returnedIngredients;
        }
    }
}
