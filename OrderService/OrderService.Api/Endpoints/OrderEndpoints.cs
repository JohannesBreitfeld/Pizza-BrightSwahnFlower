using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using OrderService.Core.Features.CreateOrder;
using OrderService.Core.Features.GetAllOrders;

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

        app.MapGet("/api/orders/", HandleGetAllAsync)
            .WithName("GetAllOrders")
            .WithTags("Orders")
            .Produces(StatusCodes.Status200OK);

        return app;
    }

    private static async Task<IResult> HandleGetAllAsync(
        HttpContext context, 
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        int? page = null;
        int? pageSize = null;

        if (int.TryParse(context.Request.Query["page"], out var parsedPage))
            page = parsedPage;

        if (int.TryParse(context.Request.Query["pageSize"], out var parsedPageSize))
            pageSize = parsedPageSize;

        var query = new GetAllOrdersQuery(page, pageSize);

        var result = await mediator.Send(query);

        return Results.Ok(result);
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
