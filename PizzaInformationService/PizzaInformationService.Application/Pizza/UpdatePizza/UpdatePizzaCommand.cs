using MediatR;
using PizzaInformationService.Application.Pizza.GetPizzaById;

namespace PizzaInformationService.Application.Pizza.UpdatePizza
{
    public record UpdatePizzaCommand(int Id, UpdatePizzaRequest request) : IRequest<PizzaResponse?>;
}
