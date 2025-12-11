using MediatR;

namespace PizzaInformationService.Application.Pizza.GetPizzaById
{
    public record GetPizzaByIdQuery(int Id) : IRequest<PizzaResponse>;
}
