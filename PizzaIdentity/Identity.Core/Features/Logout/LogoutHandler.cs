using Identity.Core.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Identity.Core.Features.Logout;

public class LogoutHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly ILogger<LogoutHandler> _logger;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    public LogoutHandler(
        ILogger<LogoutHandler> logger,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _logger = logger;
        _refreshTokenRepository = refreshTokenRepository;
    }
    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
        if (refreshToken == null)
        {
            _logger.LogWarning("Refresh token not found.");
            return Unit.Value;
        }
        if (refreshToken.Revoked)
        {
            _logger.LogInformation("Refresh token already revoked.");
            return Unit.Value;
        }

        await _refreshTokenRepository.RevokeAsync(refreshToken.Token);
        _logger.LogInformation(
    "Revoked refresh token {Token} for user {UserId}.",
            refreshToken.Token,
            refreshToken.UserId);

        return Unit.Value;
    }
}
