using OrderService.Api.BackgroundServices;
using OrderService.Api.Endpoints;
using Serilog;

namespace OrderService.Api;

public static class ApiExtensions
{
    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .WriteTo.Console()
            .WriteTo.File(
                path: "logs/orderservice-.log",
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                fileSizeLimitBytes: 10_000_000, // 10MB
                retainedFileCountLimit: 30,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        builder.Host.UseSerilog();

        return builder;
    }

    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddOpenApi();

        services.AddHostedService<OutboxProcessorService>();

        return services;
    }

    public static WebApplication ConfigureApiPipeline(this WebApplication app)
    {
        app.MapOpenApi();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "OrderService API V1");
        });
        
        app.MapOrderEndpoints();

        return app;
    }

    public static async Task InitializeInfrastructureAsync(this WebApplication app)
    {
        try
        {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Starting Cosmos DB initialization...");

            var cosmosDbService = app.Services.GetRequiredService<Infrastructure.Persistence.CosmosDbService>();
            await cosmosDbService.InitializeDatabaseAsync();

            logger.LogInformation("Cosmos DB initialized successfully.");
        }
        catch (Exception ex)
        {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Failed to initialize Cosmos DB");
            throw;
        }
    }
}
