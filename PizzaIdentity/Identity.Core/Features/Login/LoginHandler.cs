using Identity.Core.Contracts;
using Identity.Core.Domain;
using Identity.Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.Core.Features.Login;

public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<LoginHandler> _logger;

    public LoginHandler(
        UserManager<IdentityUser> userManager,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtTokenService jwtTokenService,
        ILogger<LoginHandler> logger)
    {
        _userManager = userManager;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Username);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            _logger.LogWarning("Invalid login attempt for user: {Username}", request.Username);
            throw new InvalidCredentialsException();
        }

        var jwtToken = await _jwtTokenService.GenerateJwtTokenAsync(user, _userManager);
        var expiresAt = _jwtTokenService.GetTokenExpiration();

        var refreshTokenString = _jwtTokenService.GenerateRefreshToken();
        var refreshTokenExpirationDays = _jwtTokenService.GetRefreshTokenExpirationDays();

        var refreshToken = new RefreshToken
        {
            Token = refreshTokenString,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpirationDays),
            Revoked = false,
            CreatedAt = DateTime.UtcNow
        };

        await _refreshTokenRepository.AddAsync(refreshToken);

        _logger.LogInformation("User {Username} logged in successfully", request.Username);
        return new LoginResponse(jwtToken, refreshTokenString, expiresAt);
    }
}
