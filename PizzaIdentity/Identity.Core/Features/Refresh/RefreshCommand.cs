using MediatR;

namespace Identity.Core.Features.Refresh;

public sealed record class RefreshCommand(string RefreshToken) : IRequest<RefreshResponse>;
