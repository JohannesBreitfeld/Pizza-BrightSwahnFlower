using MediatR;
using PizzaInformationService.Application.Pizza.UpdatePizza;

namespace PizzaInformationService.Api.Endpoints.Pizza
{
    public static class UpdatePizzaEndpoints
    {
        public static IEndpointRouteBuilder MapUpdatePizzaEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/pizzas")
               .WithTags("Pizzas");

            group.MapPut("/{id:int}", async (int id, UpdatePizzaRequest request, IMediator mediator) =>
            {
                try
                {
                    var pizzaResponse = await mediator.Send(new UpdatePizzaCommand(id, request));

                    if (pizzaResponse == null)
                        return Results.NotFound();

                    return Results.Ok(pizzaResponse);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("UpdatePizza");

            return app;
        }
    }
}
