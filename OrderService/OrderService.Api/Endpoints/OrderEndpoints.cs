using FluentValidation;
using MediatR;
using OrderService.Core.Features.CreateOrder;

namespace OrderService.Api.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/orders", HandleCreateOrderAsync)
            .WithName("CreateOrder")
            .WithTags("Orders")
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem();

        return app;
    }

    private static async Task<IResult> HandleCreateOrderAsync(
        CreateOrderCommand command,
        IMediator mediator,
        IValidator<CreateOrderCommand> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var response = await mediator.Send(command, cancellationToken);

        return Results.Created($"/api/orders/{response.OrderId}", response);
    }
}
