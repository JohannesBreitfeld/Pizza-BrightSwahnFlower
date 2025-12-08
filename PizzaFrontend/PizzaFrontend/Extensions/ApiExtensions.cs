using PizzaFrontend.Interfaces;
using PizzaFrontend.Services;

namespace PizzaFrontend.Extensions
{
    public static class ApiExtensions
    {
        public static IServiceCollection AddApiClients(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration.GetValue<string>("GatewayUrl");

            services.AddHttpClient<IInformationClient, InformationClient>(client =>
            {
                client.BaseAddress = new Uri($"{baseUrl}/api/information");
            });

            services.AddHttpClient<IOrderClient, OrderClient>(client =>
            {
                client.BaseAddress = new Uri($"{baseUrl}/api/orders");
            });

            return services;
        }
    }
}
