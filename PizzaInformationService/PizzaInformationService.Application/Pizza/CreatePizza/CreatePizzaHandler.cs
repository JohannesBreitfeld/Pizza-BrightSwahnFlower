using MediatR;
using PizzaInformationService.Application.Mapping;
using PizzaInformationService.Application.Pizza.GetPizzaById;
using PizzaInformationService.Domain.Interfaces;

namespace PizzaInformationService.Application.Pizza.CreatePizza
{
    public class CreatePizzaHandler : IRequestHandler<CreatePizzaCommand, PizzaResponse>
    {
        private readonly IPizzaInformationRepository _pizzaInformationRepository;
        private readonly IIngredientsRepository _ingredientsRepository;
        public CreatePizzaHandler(IPizzaInformationRepository pizzaInformationRepository, IIngredientsRepository ingredientsRepository)
        {
            _pizzaInformationRepository = pizzaInformationRepository;
            _ingredientsRepository = ingredientsRepository;
        }

        public async Task<PizzaResponse> Handle(CreatePizzaCommand createPizzaRequest, CancellationToken cancellationToken)
        {
            var ingredients = await _ingredientsRepository.GetIngredientsByIdsAsync(
                createPizzaRequest.request.IngredientIds!,
                cancellationToken);

            if (ingredients.Count == 0)
                throw new ArgumentException("No ingredients found with the provided IDs.");

            if (ingredients.Count != createPizzaRequest.request.IngredientIds!.Count())
                throw new ArgumentException("One or more ingredient IDs do not exist.");

            var pizzaToCreate = createPizzaRequest.request.ToPizzaEntity(ingredients);

            var createdPizza = await _pizzaInformationRepository.CreatePizzaAsync(pizzaToCreate);

            return createdPizza.ToPizzaResponse();
        }
    }
}
