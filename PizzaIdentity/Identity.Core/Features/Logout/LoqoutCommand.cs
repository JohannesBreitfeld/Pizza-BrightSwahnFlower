using MediatR;

namespace Identity.Core.Features.Logout;

public sealed record LogoutCommand : IRequest<Unit>
{
    public string RefreshToken { get; init; } = string.Empty;
}
