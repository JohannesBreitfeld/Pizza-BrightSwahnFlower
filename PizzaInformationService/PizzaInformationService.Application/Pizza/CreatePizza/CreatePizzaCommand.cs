using MediatR;
using PizzaInformationService.Application.Pizza.GetPizzaById;

namespace PizzaInformationService.Application.Pizza.CreatePizza
{
    public record CreatePizzaCommand(CreatePizzaRequest request) : IRequest<PizzaResponse>;
}
