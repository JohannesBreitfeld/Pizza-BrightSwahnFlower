using FluentValidation;
using Identity.Core.Features.Login;
using Identity.Core.Features.Logout;
using Identity.Core.Features.Refresh;
using MediatR;

namespace Identity.Api.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder AddAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/auth/login", HandleLoginAsync)
            .WithName("Login")
            .WithTags("Auth")
            .Produces<LoginResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem();

        app.MapPost("api/auth/refresh", HandleRefreshAsync)
            .WithName("Refresh")
            .WithTags("Auth")
            .Produces<RefreshResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        app.MapPost("api/auth/logout", HandleLogoutAsync)
            .WithName("Logout")
            .WithTags("Auth")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem();


        return app;
    }

    private static async Task<IResult> HandleLogoutAsync(
        LogoutCommand command,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Results.Ok();
    }

    private static async Task<IResult> HandleRefreshAsync(
        RefreshCommand command,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(command, cancellationToken);

        return Results.Ok(response);
    }

    private static async Task<IResult> HandleLoginAsync(
        LoginCommand command,
        IMediator mediator,
        IValidator<LoginCommand> validator,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var response = await mediator.Send(command, cancellationToken);

        return Results.Ok(response);
    }
}
