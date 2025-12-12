using MediatR;
using PizzaInformationService.Domain.Interfaces;

namespace PizzaInformationService.Application.Pizza.DeletePizza
{
    public class DeletePizzaHandler : IRequestHandler<DeletePizzaCommand, bool>
    {
        private readonly IPizzaInformationRepository _pizzaInformationRepository;

        public DeletePizzaHandler(IPizzaInformationRepository pizzaInformationRepository)
        {
            _pizzaInformationRepository = pizzaInformationRepository;
        }

        public async Task<bool> Handle(DeletePizzaCommand request, CancellationToken cancellationToken)
        {
            return await _pizzaInformationRepository.DeletePizzaAsync(request.Id, cancellationToken);
        }
    }
}
