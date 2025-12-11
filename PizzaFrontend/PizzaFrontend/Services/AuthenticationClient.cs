using PizzaFrontend.Contracts.Identity;
using PizzaFrontend.Interfaces;

namespace PizzaFrontend.Services;

public class AuthenticationClient(HttpClient client) : IAuthenticationClient
{
    public async Task<HttpResponseMessage> LoginAsync(LoginRequest request)
    {
        var response = await client.PostAsJsonAsync("login", request);
        return response;
    }

    public async Task<HttpResponseMessage> LogoutAsync()
    {
        return await client.PostAsync("logout", null);
    }

    public async Task<HttpResponseMessage> RefreshTokenAsync(RefreshTokenRequest request)
    {
        return await client.PostAsJsonAsync("refresh", request);
    }
}
