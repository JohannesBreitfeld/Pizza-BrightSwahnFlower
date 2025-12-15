using MediatR;
using PizzaInformationService.Application.Abstractions;
using PizzaInformationService.Application.Mapping;
using PizzaInformationService.Application.Pizza.GetPizzaById;
using PizzaInformationService.Domain.Interfaces;

namespace PizzaInformationService.Application.Pizza.CreatePizza
{
    public class CreatePizzaHandler : IRequestHandler<CreatePizzaCommand, PizzaResponse>
    {
        private readonly IPizzaInformationRepository _pizzaInformationRepository;
        private readonly IIngredientsRepository _ingredientsRepository;
        private readonly ICacheInvalidationService _cacheInvalidationService;
        public CreatePizzaHandler(
            IPizzaInformationRepository pizzaInformationRepository, 
            IIngredientsRepository ingredientsRepository,
            ICacheInvalidationService cacheInvalidationService)
        {
            _pizzaInformationRepository = pizzaInformationRepository;
            _ingredientsRepository = ingredientsRepository;
            _cacheInvalidationService = cacheInvalidationService;
        }

        public async Task<PizzaResponse> Handle(CreatePizzaCommand createPizzaRequest, CancellationToken cancellationToken)
        {
            var ingredients = await _ingredientsRepository.GetIngredientsByIdsAsync(
                createPizzaRequest.request.IngredientIds!,
                cancellationToken);

            if (ingredients.Count != createPizzaRequest.request.IngredientIds?.Count())
            {
                throw new ArgumentException("One or more ingredient IDs are invalid or missing.");
            }

            var pizzaToCreate = createPizzaRequest.request.ToPizzaEntity(ingredients);

            var createdPizza = await _pizzaInformationRepository.CreatePizzaAsync(pizzaToCreate);

            await _cacheInvalidationService.InvalidatePizzaCacheAsync();

            return createdPizza.ToPizzaResponse();
        }
    }
}
