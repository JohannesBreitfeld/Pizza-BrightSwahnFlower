using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PizzaInformationService.Infrastructure.Persistance;

namespace PizzaInformationService.Infrastructure
{
    public class PizzaDbContextFactory : IDesignTimeDbContextFactory<PizzaInformationDbContext>
    {
        public PizzaInformationDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=local.db";
            var optionsBuilder = new DbContextOptionsBuilder<PizzaInformationDbContext>();
            optionsBuilder.UseSqlServer(connectionString,
                b => b.MigrationsAssembly(typeof(PizzaInformationDbContext).Assembly.FullName));

            return new PizzaInformationDbContext(optionsBuilder.Options);
        }
    }
}
