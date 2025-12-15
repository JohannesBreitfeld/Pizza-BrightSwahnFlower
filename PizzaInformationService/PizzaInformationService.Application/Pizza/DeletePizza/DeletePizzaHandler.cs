using MediatR;
using PizzaInformationService.Domain.Interfaces;
using PizzaInformationService.Application.Abstractions;

namespace PizzaInformationService.Application.Pizza.DeletePizza
{
    public class DeletePizzaHandler : IRequestHandler<DeletePizzaCommand, bool>
    {
        private readonly IPizzaInformationRepository _pizzaInformationRepository;
        private readonly ICacheInvalidationService _cacheInvalidationService;

        public DeletePizzaHandler(IPizzaInformationRepository pizzaInformationRepository, ICacheInvalidationService cacheInvalidationService)
        {
            _pizzaInformationRepository = pizzaInformationRepository;
            _cacheInvalidationService = cacheInvalidationService;
        }

        public async Task<bool> Handle(DeletePizzaCommand request, CancellationToken cancellationToken)
        {
            var pizza =  await _pizzaInformationRepository.DeletePizzaAsync(request.Id, cancellationToken);

            if (pizza != null)
            {
                await _cacheInvalidationService.InvalidatePizzaCacheAsync();
                return true;
            }

            return false;
        }
    }
}
