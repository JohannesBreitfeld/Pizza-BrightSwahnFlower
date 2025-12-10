using PizzaFrontend.Contracts.Identity;

namespace PizzaFrontend.Interfaces;

public interface IAuthenticationClient
{
    Task<HttpResponseMessage> LoginAsync(LoginRequest request);
    Task<HttpResponseMessage> LogoutAsync();
    Task<HttpResponseMessage> RefreshTokenAsync(RefreshTokenRequest request);
}
