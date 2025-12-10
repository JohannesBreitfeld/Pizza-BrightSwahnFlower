using System.IdentityModel.Tokens.Jwt;
using PizzaFrontend.Interfaces;

namespace PizzaFrontend.Services;

public class TokenService : ITokenService
{
    private string? _accessToken;
    private string? _refreshToken;
    private DateTime? _expiresAt;
    private readonly JwtSecurityTokenHandler _jwtHandler = new();

    public Task SetTokensAsync(string accessToken, string refreshToken, int expiresIn)
    {
        _accessToken = accessToken;
        _refreshToken = refreshToken;
        _expiresAt = DateTime.UtcNow.AddSeconds(expiresIn);
        return Task.CompletedTask;
    }

    public Task<string?> GetAccessTokenAsync()
    {
        return Task.FromResult(_accessToken);
    }

    public Task<string?> GetRefreshTokenAsync()
    {
        return Task.FromResult(_refreshToken);
    }

    public Task<bool> IsAuthenticatedAsync()
    {
        if (string.IsNullOrEmpty(_accessToken) || _expiresAt == null)
        {
            return Task.FromResult(false);
        }

        return Task.FromResult(DateTime.UtcNow < _expiresAt);
    }

    public Task ClearTokensAsync()
    {
        _accessToken = null;
        _refreshToken = null;
        _expiresAt = null;
        return Task.CompletedTask;
    }

    public Task<bool> IsTokenExpiringSoonAsync()
    {
        if (_expiresAt == null)
        {
            return Task.FromResult(true);
        }

        var timeUntilExpiration = _expiresAt.Value - DateTime.UtcNow;
        return Task.FromResult(timeUntilExpiration.TotalMinutes < 5);
    }

    public Task<DateTime?> GetTokenExpirationAsync()
    {
        return Task.FromResult(_expiresAt);
    }
}
