using PizzaFrontend.Interfaces;
using PizzaFrontend.Services;

namespace PizzaFrontend.Extensions
{
    public static class ApiExtensions
    {
        public static IServiceCollection AddApiClients(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration.GetValue<string>("GatewayUrl");

            services.AddSingleton<ITokenService, TokenService>();
            services.AddScoped<AuthenticationStateService>();
            services.AddTransient<AuthenticationDelegatingHandler>();

            services.AddHttpClient<IAuthenticationClient, AuthenticationClient>(client =>
            {
                client.BaseAddress = new Uri($"{baseUrl}/api/identity/");
            });

            services.AddHttpClient<IInformationClient, InformationClient>(client =>
            {
                client.BaseAddress = new Uri($"{baseUrl}/api/information/");
            });

            services.AddHttpClient<IOrderClient, OrderClient>(client =>
            {
                client.BaseAddress = new Uri($"{baseUrl}/api/orders/");
            });

            services.AddHttpClient<IAdminPizzaClient, AdminPizzaClient>(client =>
            {
                client.BaseAddress = new Uri($"{baseUrl}/api/admin/information/");
            })
            .AddHttpMessageHandler<AuthenticationDelegatingHandler>();

            services.AddHttpClient<IAdminOrderClient, AdminOrderClient>(client =>
            {
                client.BaseAddress = new Uri($"{baseUrl}/api/admin/orders/");
            })
            .AddHttpMessageHandler<AuthenticationDelegatingHandler>();

            return services;
        }
    }
}
