using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PizzaInformationService.Application.Abstractions;
using PizzaInformationService.Domain.Interfaces;
using PizzaInformationService.Infrastructure.Persistance;
using PizzaInformationService.Infrastructure.Repositories;

namespace PizzaInformationService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<PizzaInformationDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"))
                    .ConfigureWarnings(warnings =>
                        warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));

            services.AddScoped<IPizzaInformationDbContext>(provider =>
                provider.GetRequiredService<PizzaInformationDbContext>());

            services.AddScoped<IPizzaInformationRepository, PizzaInformationRepository>();
            services.AddScoped<IIngredientsRepository, IngredientsRepository>();

            return services;
        }
    }
}
