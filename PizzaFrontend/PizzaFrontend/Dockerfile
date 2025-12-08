# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy everything (single project)
COPY . ./

# Restore & publish
RUN dotnet restore PizzaFrontend.csproj
RUN dotnet publish PizzaFrontend.csproj -c Release -o /app --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app ./

EXPOSE 8080

ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_ENVIRONMENT=Production
ENV DOTNET_URLS=http://0.0.0.0:8080

ENTRYPOINT ["dotnet", "PizzaFrontend.dll"]