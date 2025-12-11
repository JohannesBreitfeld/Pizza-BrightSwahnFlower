using MediatR;
using PizzaInformationService.Application.Ingredients.GetAllIngredients;

namespace PizzaInformationService.Api.Endpoints.Ingredients
{
    public static class GetIngredientsEndpoints
    {
        public static IEndpointRouteBuilder MapIngredientsEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/ingredients")
               .WithTags("Ingredients");

            group.MapGet("", async (IMediator mediator) =>
            {
                try
                {
                    var ingredients = await mediator.Send(new GetAllIngredientsQuery());
                    return ingredients.Any() ? Results.Ok(ingredients) : Results.Ok();
                }
                catch
                {
                    return Results.StatusCode(500);
                }
            })
            .WithName("GetAllIngredients");

            return app;
        }
    }
}
