using FluentValidation;
using Identity.Core.Features;
using MediatR;

namespace Identity.Api.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder AddAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/auth/login", HandleLoginAsync)
            .WithName("Login")
            .WithTags("Auth")
            .ProducesValidationProblem();

        return app;
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
