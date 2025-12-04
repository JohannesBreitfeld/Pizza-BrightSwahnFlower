using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Core.Features.CreateOrder;

namespace OrderService.Core;

public static class CoreExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(typeof(CreateOrderCommand).Assembly));

        services.AddValidatorsFromAssemblyContaining<CreateOrderValidator>();

        return services;
    }
}
