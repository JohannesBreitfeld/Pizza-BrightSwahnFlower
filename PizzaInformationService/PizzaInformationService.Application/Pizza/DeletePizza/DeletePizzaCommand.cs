using MediatR;

namespace PizzaInformationService.Application.Pizza.DeletePizza
{
    public record DeletePizzaCommand(int Id) : IRequest<bool>;
}
