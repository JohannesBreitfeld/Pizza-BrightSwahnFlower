using MediatR;
using PizzaInformationService.Application.Mapping;
using PizzaInformationService.Domain.Interfaces;

namespace PizzaInformationService.Application.Pizza.GetPizzaById
{
    public class GetPizzaByIdHandler : IRequestHandler<GetPizzaByIdQuery, PizzaResponse?>
    {
        private readonly IPizzaInformationRepository _pizzaInformationRepository;
        public GetPizzaByIdHandler(IPizzaInformationRepository pizzaInformationRepository)
        {
            _pizzaInformationRepository = pizzaInformationRepository;
        }
        public async Task<PizzaResponse?> Handle(GetPizzaByIdQuery request, CancellationToken cancellationToken)
        {
            var pizza = await _pizzaInformationRepository.GetPizzaByIdAsync(request.Id);

            if (pizza is null) return null;

            return pizza.ToPizzaResponse();
        }
    }
}
