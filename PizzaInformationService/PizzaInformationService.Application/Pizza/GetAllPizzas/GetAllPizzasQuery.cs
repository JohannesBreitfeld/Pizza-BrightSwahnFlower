using MediatR;
using PizzaInformationService.Application.Pizza.GetPizzaById;

namespace PizzaInformationService.Application.Pizza.GetAllPizzas
{
    public record GetAllPizzasQuery : IRequest<List<PizzaResponse>>;
}
