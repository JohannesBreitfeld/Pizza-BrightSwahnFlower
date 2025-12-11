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

app.MapPizzaEndpoints();
app.MapIngredientsEndpoints();
app.MapCreatePizzaEndpoints();

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
