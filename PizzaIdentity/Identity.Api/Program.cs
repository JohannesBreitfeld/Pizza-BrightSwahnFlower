using Identity.Api.Endpoints;
using Identity.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddIdentityServices();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

await app.InitializeDatabaseAsync();

app.UseGlobalExceptionHandler();

app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "OrderService API V1");
});

app.UseAuthentication();
app.UseAuthorization();

app.AddAuthEndpoints();

app.Run();

