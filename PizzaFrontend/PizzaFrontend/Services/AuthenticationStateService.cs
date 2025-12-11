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

            var username = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name || c.Type == "unique_name")?.Value ?? "Unknown";
            var roles = claims.Where(c => c.Type == ClaimTypes.Role || c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                              .Select(c => c.Value)
                              .ToArray();

            var claimsDictionary = claims
                .Where(c => c.Type != ClaimTypes.Name && c.Type != "unique_name" && c.Type != ClaimTypes.Role && c.Type != "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                .GroupBy(c => c.Type)
                .ToDictionary(g => g.Key, g => g.First().Value);

            return new UserInfo(username, roles, claimsDictionary);
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
