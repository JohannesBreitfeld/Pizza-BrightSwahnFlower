using MediatR;
using PizzaInformationService.Application.Mapping;
using PizzaInformationService.Application.Pizza.GetPizzaById;
using PizzaInformationService.Domain.Interfaces;

namespace PizzaInformationService.Application.Pizza.GetAllPizzas
{
    public class GetAllPizzasHandler : IRequestHandler<GetAllPizzasQuery, List<PizzaResponse>>
    {
        private readonly IPizzaInformationRepository _pizzaInformationRepository;
        public GetAllPizzasHandler(IPizzaInformationRepository pizzaInformationRepository)
        {
            _pizzaInformationRepository = pizzaInformationRepository;
        }

        public async Task<List<PizzaResponse>> Handle(GetAllPizzasQuery request, CancellationToken cancellationToken)
        {
            var pizzas = await _pizzaInformationRepository.GetAllPizzaAsync();

            if(pizzas is null || !pizzas.Any())
            {
                return new List<PizzaResponse>();
            }
            
            return pizzas.Select(p => p.ToPizzaResponse()).ToList();
        }
    }
}
