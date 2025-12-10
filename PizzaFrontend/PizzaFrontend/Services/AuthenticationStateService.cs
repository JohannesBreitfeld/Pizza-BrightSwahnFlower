using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using PizzaFrontend.Contracts.Identity;
using PizzaFrontend.Interfaces;

namespace PizzaFrontend.Services;

public class AuthenticationStateService
{
    private readonly ITokenService _tokenService;
    private readonly JwtSecurityTokenHandler _jwtHandler = new();

    public event EventHandler? AuthenticationStateChanged;

    public AuthenticationStateService(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        return await _tokenService.IsAuthenticatedAsync();
    }

    public async Task<bool> IsInRoleAsync(string role)
    {
        var userInfo = await GetCurrentUserAsync();
        return userInfo?.Roles.Contains(role, StringComparer.OrdinalIgnoreCase) ?? false;
    }

    public async Task<UserInfo?> GetCurrentUserAsync()
    {
        var token = await _tokenService.GetAccessTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        try
        {
            var jwtToken = _jwtHandler.ReadJwtToken(token);
            var claims = jwtToken.Claims.ToList();

            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "email")?.Value ?? "Unknown";
            var roles = claims.Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                              .Select(c => c.Value)
                              .ToArray();

            var claimsDictionary = claims
                .Where(c => c.Type != ClaimTypes.Email && c.Type != "email" && c.Type != ClaimTypes.Role && c.Type != "role")
                .GroupBy(c => c.Type)
                .ToDictionary(g => g.Key, g => g.First().Value);

            return new UserInfo(email, roles, claimsDictionary);
        }
        catch
        {
            return null;
        }
    }

    public void NotifyAuthenticationStateChanged()
    {
        AuthenticationStateChanged?.Invoke(this, EventArgs.Empty);
    }
}
