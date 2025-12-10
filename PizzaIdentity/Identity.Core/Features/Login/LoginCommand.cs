using MediatR;

namespace Identity.Core.Features.Login;

public sealed record LoginCommand : IRequest<LoginResponse>
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

