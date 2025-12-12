using MediatR;
using PizzaInformationService.Application.Pizza.DeletePizza;

namespace PizzaInformationService.Api.Endpoints.Pizza
{
    public static class DeletePizzaEndpoints
    {
        public static IEndpointRouteBuilder MapDeletePizzaEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/pizzas")
               .WithTags("Pizzas");

            group.MapDelete("/{id:int}", async (int id, IMediator mediator) =>
            {
                try
                {
                    var result = await mediator.Send(new DeletePizzaCommand(id));

                    if (!result)
                        return Results.NotFound();

                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("DeletePizza");

            return app;
        }
    }
}
