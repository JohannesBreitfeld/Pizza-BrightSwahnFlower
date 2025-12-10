using Identity.Core.Contracts;
using Identity.Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.Core.Features.Refresh;

public class RefreshTokenHandler : IRequestHandler<RefreshCommand, RefreshResponse>
{
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<RefreshTokenHandler> _logger;

    public RefreshTokenHandler(
        IJwtTokenService jwtTokenService,
        IRefreshTokenRepository refreshTokenRepository,
        UserManager<IdentityUser> userManager,
        ILogger<RefreshTokenHandler> logger)
    {
        _jwtTokenService = jwtTokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<RefreshResponse> Handle(RefreshCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
        if (refreshToken == null || refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            _logger.LogWarning("Invalid or expired refresh token attempted: {RefreshToken}", request.RefreshToken);
            throw new InvalidRefreshTokenException();
        }

        var user = await _userManager.FindByIdAsync(refreshToken.UserId);
        if (user == null)
        {
            _logger.LogWarning("Invalid refresh token attempted for non-existent user: {UserId}", refreshToken.UserId);
            throw new InvalidRefreshTokenException();
        }

        var jwtToken = await _jwtTokenService.GenerateJwtTokenAsync(user, _userManager);
        var expiresAt = _jwtTokenService.GetTokenExpiration();

        _logger.LogInformation("Refresh token successfully used for user: {UserId}", user.Id);
        return new RefreshResponse(jwtToken, request.RefreshToken, expiresAt);
    }
}
