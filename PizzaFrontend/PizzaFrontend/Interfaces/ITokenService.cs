namespace PizzaFrontend.Interfaces;

public interface ITokenService
{
    Task SetTokensAsync(string accessToken, string refreshToken, int expiresIn);
    Task<string?> GetAccessTokenAsync();
    Task<string?> GetRefreshTokenAsync();
    Task<bool> IsAuthenticatedAsync();
    Task ClearTokensAsync();
    Task<bool> IsTokenExpiringSoonAsync();
    Task<DateTime?> GetTokenExpirationAsync();
}
