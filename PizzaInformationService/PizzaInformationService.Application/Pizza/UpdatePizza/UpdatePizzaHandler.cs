using MediatR;
using PizzaInformationService.Application.Mapping;
using PizzaInformationService.Application.Pizza.GetPizzaById;
using PizzaInformationService.Domain.Interfaces;

namespace PizzaInformationService.Application.Pizza.UpdatePizza
{
    public class UpdatePizzaHandler : IRequestHandler<UpdatePizzaCommand, PizzaResponse?>
    {
        private readonly IPizzaInformationRepository _pizzaInformationRepository;
        private readonly IIngredientsRepository _ingredientsRepository;

        public UpdatePizzaHandler(IPizzaInformationRepository pizzaInformationRepository, IIngredientsRepository ingredientsRepository)
        {
            _pizzaInformationRepository = pizzaInformationRepository;
            _ingredientsRepository = ingredientsRepository;
        }

        public async Task<PizzaResponse?> Handle(UpdatePizzaCommand request, CancellationToken cancellationToken)
        {
            var ingredients = await _ingredientsRepository.GetIngredientsByIdsAsync(
                request.request.IngredientIds!,
                cancellationToken);

            if (ingredients.Count == 0)
                throw new ArgumentException("No ingredients found with the provided IDs.");

            if (ingredients.Count != request.request.IngredientIds!.Count())
                throw new ArgumentException("One or more ingredient IDs do not exist.");

            var pizzaToUpdate = new Domain.Entities.Pizza
            {
                Name = request.request.Name,
                ImageUrl = request.request.ImageUrl,
                Price = request.request.Price,
                Ingredients = ingredients
            };

            var updatedPizza = await _pizzaInformationRepository.UpdatePizzaAsync(request.Id, pizzaToUpdate, cancellationToken);

            return updatedPizza?.ToPizzaResponse();
        }
    }
}
