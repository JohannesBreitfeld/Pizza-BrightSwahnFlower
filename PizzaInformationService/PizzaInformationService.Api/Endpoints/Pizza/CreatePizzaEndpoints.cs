using MediatR;
using PizzaInformationService.Application.Pizza.CreatePizza;

namespace PizzaInformationService.Api.Endpoints.Pizza
{
    public static class CreatePizzaEndpoints
    {
        public static IEndpointRouteBuilder MapCreatePizzaEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/pizzas/create")
               .WithTags("Pizzas");

            group.MapPost("", async (CreatePizzaRequest request, IMediator mediator) =>
            {
                try
                {
                    var pizzaResponse = await mediator.Send(new CreatePizzaCommand(request));
                    return Results.Created($"/api/pizzas/{pizzaResponse.Id}", pizzaResponse);
                }
                catch(Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("CreatePizza");

            return app;
        }
    }
}
