using MediatR;
using PizzaInformationService.Application.Abstractions;
using PizzaInformationService.Application.Mapping;
using PizzaInformationService.Application.Pizza.GetPizzaById;
using PizzaInformationService.Domain.Interfaces;

namespace PizzaInformationService.Application.Pizza.GetAllPizzas
{
    public class GetAllPizzasHandler : IRequestHandler<GetAllPizzasQuery, List<PizzaResponse>>
    {
        private readonly IPizzaInformationRepository _pizzaInformationRepository;
        private readonly ICacheService _cacheService;
        public GetAllPizzasHandler(IPizzaInformationRepository pizzaInformationRepository, ICacheService cacheService)
        {
            _pizzaInformationRepository = pizzaInformationRepository;
            _cacheService = cacheService;
        }

        public async Task<List<PizzaResponse>> Handle(GetAllPizzasQuery request, CancellationToken cancellationToken)
        {
            const string key = "all_pizzas";

            var cached = await _cacheService.GetAsync<List<PizzaResponse>>(key);
            if (cached != null) return cached;

            var pizzas = await _pizzaInformationRepository.GetAllPizzaAsync();

            if(pizzas is null || !pizzas.Any())
            {
                return new List<PizzaResponse>();
            }

            var pizzasToReturn = pizzas.Select(p => p.ToPizzaResponse()).ToList();
            await _cacheService.SetAsync(key, pizzasToReturn, TimeSpan.FromHours(1));

            return pizzasToReturn;
        }
    }
}
