# ============================
# 1. Build Stage
# ============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything and restore
COPY . .
RUN dotnet restore

# Publish the function app to /app
RUN dotnet publish -c Release -o /app

# ============================
# 2. Runtime Stage (Azure Functions Isolated)
# ============================
FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0

WORKDIR /home/site/wwwroot
COPY --from=build /app .

# Azure Functions custom handler entrypoint
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

# Optional: Better logging experience
ENV ASPNETCORE_URLS=http://+:80

EXPOSE 80