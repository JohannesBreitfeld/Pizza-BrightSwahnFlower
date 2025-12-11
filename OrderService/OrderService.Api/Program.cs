using OrderService.Api;
using OrderService.Core;
using OrderService.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging();

builder.Services.AddApiServices();
builder.Services.AddCoreServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

try
{
    await app.InitializeInfrastructureAsync();
    app.ConfigureApiPipeline();
    await app.SeedOrdersAsync();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
