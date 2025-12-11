using MediatR;
using PizzaInformationService.Application.Pizza.GetAllPizzas;
using PizzaInformationService.Application.Pizza.GetPizzaById;

namespace PizzaInformationService.Api.Endpoints.Pizza
{
    public static class GetPizzaEndpoints
    {
        public static IEndpointRouteBuilder MapPizzaEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/pizzas")
                           .WithTags("Pizza");

            group.MapGet("/{id:int}", async (int id, IMediator mediator) =>
            {
                try
                {
                    var pizza = await mediator.Send(new GetPizzaByIdQuery(id));
                    return pizza is null ? Results.NotFound() : Results.Ok(pizza);
                }
                catch
                {
                    return Results.StatusCode(500);
                }
            })
            .WithName("GetPizzaById");

            group.MapGet("", async (IMediator mediator) =>
            {
                try
                {
                    var pizza = await mediator.Send(new GetAllPizzasQuery());
                    return pizza.Any() ? Results.Ok(pizza) : Results.NotFound();
                }
                catch
                {
                    return Results.StatusCode(500);
                }
            })
            .WithName("GetAllPizzas");

            return app;
        }
    }
}
