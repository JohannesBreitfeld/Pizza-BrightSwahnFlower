using Microsoft.EntityFrameworkCore;
using PizzaInformationService.Api.Endpoints.Ingredients;
using PizzaInformationService.Api.Endpoints.Pizza;
using PizzaInformationService.Application;
using PizzaInformationService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddAuthorization();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Title = "Pizza Information API",
        Version = "v1",
        Description = "API to manage pizzas, ingredients, and orders"
    });
});

var app = builder.Build();

// Apply migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PizzaInformationService.Infrastructure.Persistance.PizzaInformationDbContext>();
    try
    {
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database");
        throw;
    }
}

app.MapPizzaEndpoints();
app.MapIngredientsEndpoints();
app.MapCreatePizzaEndpoints();
app.MapUpdatePizzaEndpoints();
app.MapDeletePizzaEndpoints();

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pizza Information API v1");
        c.RoutePrefix = string.Empty; // Swagger at root: http://localhost:8080/
    });
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseAuthorization();

app.Run();
